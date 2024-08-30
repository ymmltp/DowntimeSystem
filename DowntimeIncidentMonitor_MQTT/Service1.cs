using System;
using System.ServiceProcess;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using MQTTnet;
using MQTTnet.Client;
using MQTTnet.Protocol;
using Npgsql;
using System.Text.RegularExpressions;
using System.Diagnostics;

namespace DowntimeIncidentMonitor_MQTT
{
    public partial class Service1 : ServiceBase
    {
        private string connectionString = "Host=cnwuxm1medb01;Database=EC;Username=ECUser;Password=Jabil123";
        private string mqttServerUri = "cnwuxg0te01";
        private NpgsqlConnection connection;
        private Task listeningTask;
        private CancellationTokenSource cancellationTokenSource;
        // 创建一个新的 MQTT 客户端
        private static MqttFactory factory;
        private static IMqttClient mqttClient;
        private bool isStopping;

        public Service1()
        {
            InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {
            isStopping = false;
            //创建一个MQTT连接
            factory = new MqttFactory();
            mqttClient = factory.CreateMqttClient();
            var options = new MqttClientOptionsBuilder()
                .WithClientId(Guid.NewGuid().ToString())
                .WithTcpServer(mqttServerUri, 1883) // MQTT Broker 地址和端口
                .WithCleanSession()
                .Build();
            // 注册连接成功事件
            mqttClient.ConnectedAsync += async e =>
            {
                EventLog.WriteEntry("Connected successfully with MQTT Broker.");
                //// 订阅主题
                await mqttClient.SubscribeAsync(new MqttTopicFilterBuilder().WithTopic("test/topic").Build());
                //Console.WriteLine("Subscribed to topic: test/topic");
            };

            // 注册连接断开事件
            mqttClient.DisconnectedAsync += async e =>
            {
                EventLog.WriteEntry("Disconnected from MQTT Broker.");
                if (!isStopping)
                {
                    EventLog.WriteEntry("Disconnected from MQTT Broker. Reconnecting...");
                    // 断开连接后重新连接
                    await Task.Delay(TimeSpan.FromSeconds(5)); // 等待5秒后重连
                    try
                    {
                        await mqttClient.ConnectAsync(options); // 重新连接
                    }
                    catch (Exception err)
                    {
                        EventLog.WriteEntry($"Reconnect failed. Will retry. ErrorMsg:{err.Message}");
                    }
                }
            };

            // 连接到 MQTT Broker
            mqttClient.ConnectAsync(options);


            //创建npgsql监听事件
            connection = new NpgsqlConnection(connectionString);
            connection.Open();

            cancellationTokenSource = new CancellationTokenSource();
            listeningTask = ListenToPostgreSqlAsync(connection, cancellationTokenSource.Token);
        }

        protected override void OnStop()
        {
            isStopping = true;
            cancellationTokenSource.Cancel();
            listeningTask.Wait();
            // Ensure the connection is closed when the task is canceled
            if (connection != null && connection.State == System.Data.ConnectionState.Open)
            {
                connection.Close();
            }
            mqttClient.DisconnectAsync();
        }


        private static async Task ListenToPostgreSqlAsync(NpgsqlConnection connection, CancellationToken cancellationToken)
        {
            using (var command = new NpgsqlCommand("LISTEN table_insert;", connection))
            {
                await command.ExecuteNonQueryAsync();
            }

            using (var command = new NpgsqlCommand("LISTEN table_update;", connection))
            {
                await command.ExecuteNonQueryAsync();
            }

            connection.Notification += async (o, e) =>
            {
                if (e.Condition == "table_insert")
                {
                    await SendMessageToMQTTServer(e.AdditionalInformation);
                }
                else if (e.Condition == "table_update")
                {
                    await SendMessageToMQTTServer(e.AdditionalInformation);
                }
            };
            try
            {
                while (!cancellationToken.IsCancellationRequested)
                {
                    await Task.Delay(1000, cancellationToken);
                    connection.Wait();
                }
            }
            catch (OperationCanceledException)
            {
                if (connection.State == System.Data.ConnectionState.Open)
                {
                    connection.Close();
                }
            }
            finally
            {
                // Ensure the connection is closed when the task is canceled
                if (connection.State == System.Data.ConnectionState.Open)
                {
                    connection.Close();
                }
            }
        }

        private static async Task SendMessageToMQTTServer( string messageContent)
        {
            Regex regex = new Regex("\"machine\":\"([^\"]+)\"");
            Match match = regex.Match(messageContent);
            string machineName = match.Groups[1].Value;
            var message = new MqttApplicationMessageBuilder()
                .WithTopic($"Downtime/{machineName}")
                .WithPayload(messageContent)
                .WithQualityOfServiceLevel(MqttQualityOfServiceLevel.ExactlyOnce)
                .WithRetainFlag(false)
                .Build();
            await mqttClient.PublishAsync(message);
        }

    }
}

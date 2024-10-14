using System;
using System.Threading;
using System.Threading.Tasks;
using MQTTnet;
using MQTTnet.Client;
using MQTTnet.Protocol;
using Npgsql;
using System.Text.RegularExpressions;
using System.Diagnostics;

namespace MqttMonitor
{
    class Program
    {
        private static string connectionString = "Host=cnwuxm1medb01;Database=EC;Username=ECUser;Password=Jabil123";
        private static string mqttServerUri = "cnwuxg0te01";
        private static NpgsqlConnection connection;
        private static CancellationTokenSource cancellationTokenSource;
        private static Task listeningTask;
        // 创建一个新的 MQTT 客户端
        private static MqttFactory factory;
        private static IMqttClient mqttClient;
        private static bool isStopping;

        static void Main(string[] args)
        {
            AppDomain.CurrentDomain.UnhandledException += (sender, e) =>
            {
                Exception ex = (Exception)e.ExceptionObject;
                Console.WriteLine($"Unhandled exception: {ex.Message}");
            };
            isStopping = false;

            //创建npgsql监听事件
            connection = new NpgsqlConnection(connectionString);
            connection.Open();

            cancellationTokenSource = new CancellationTokenSource();
            listeningTask = ListenToPostgreSqlAsync(connection, cancellationTokenSource.Token);

            Console.ReadKey();
        }

        private static async Task ListenToPostgreSqlAsync(NpgsqlConnection connection, CancellationToken cancellationToken)
        {
            try
            {
                using (var command = new NpgsqlCommand("LISTEN table_insert;", connection))
                {
                    await command.ExecuteNonQueryAsync();
                }


                using (var command = new NpgsqlCommand("LISTEN table_update;", connection))
                {
                    await command.ExecuteNonQueryAsync();
                }
            }          
            catch (NpgsqlException npgsqlEx)
            {
                Console.WriteLine($"NpgsqlException: {npgsqlEx.Message}");
                // Optionally log additional details, such as:
                Console.WriteLine($"Stack Trace: {npgsqlEx.StackTrace}");
                if (npgsqlEx.InnerException != null)
                {
                    Console.WriteLine($"Inner Exception: {npgsqlEx.InnerException.Message}");
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }

            connection.Notification += async (o, e) =>
            {
                if (e.Condition == "table_insert")
                {
                   Console.WriteLine( e.AdditionalInformation);
                }
                else if (e.Condition == "table_update")
                {
                    Console.WriteLine( e.AdditionalInformation);
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
    }


}



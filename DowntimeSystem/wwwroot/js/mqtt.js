/*<script src="~/lib/paho-mqtt.min.js"></script>*/


class MQTTWebSocketHandler {
    constructor() {
        this.host = "cnwuxg0te01";
        this.port = 8084;
        this.path = "";
        this.userName = null;
        this.topic = null;
        this.onmessageEvent = null;
        this.client = null;
        this.showStatus = null;
        this.SubTitle = "Downtime/";
        this._onConnect = this._onConnect.bind(this);
        this._onFailure = this._onFailure.bind(this);
        this.isConnecting = false;
    }

    initializeSocket(userName, showStatus, onmessageEvent) {
        if (userName == "" || userName == null) {
            showWarning("请先输入UserName");
            return;
        }
        if (this.topic == userName) {
            console.log("已经订阅过该topic了");
            return;
        }
        this.userName = `${userName} -${new Date().getTime()}`;
        this.topic = userName;
        this.showStatus = showStatus;
        this.onmessageEvent = onmessageEvent;
        this.client = new Paho.MQTT.Client(this.host, Number(this.port), this.path, this.userName); //用户名相同时，后者会取代前者
        let options = {
            useSSL: true,  // 使用 WebSocket Secure
            userName: "",  // 如果有用户名，请填写
            password: "",  // 如果有密码，请填写
            onSuccess: this._onConnect,
            onFailure: this._onFailure
        };
        this.isConnecting = true;
        this.client.connect(options);

        // 收到消息时的回调函数
        this.client.onMessageArrived =  (message)=> {
            this.onmessageEvent(JSON.parse(message.payloadString));
        };

        // 处理连接断开事件
        this.client.onConnectionLost = (responseObject)=> {
            if (responseObject.errorCode !== 0) {
                console.log("Connection lost:", responseObject.errorMessage);
                this._checkStatus();
            }
            // 仅在当前未连接时自动重新连接
            if (!this.client.isConnected()) {
                console.log("尝试重新连接...");
                this.reconnect(); // 尝试重新连接的方法，需自行定义
            }
        };
    }

    reconnect() {
        if (!this.isConnecting) {
            this.isConnecting = true; // 设置为正在连接
            setTimeout(() => {
                console.log("重连中...");
                const options = {
                    useSSL: true,
                    userName: "",
                    password: "",
                    onSuccess: this._onConnect,
                    onFailure: this._onFailure
                };
                this.client.connect(options);
            }, 5000); // 5秒后重连
        }
    }

    publish(topic, message) {
        if (this.client.isConnected()) {
            // 创建消息对象
            const messageObj = new Paho.MQTT.Message(message);
            messageObj.destinationName = topic;
            // 发布消息
            this.client.send(messageObj);
            console.log(`Message published to topic: ${topic}`);
        } else {
            console.log("请先连接再发送消息");
        }
    }

    disconnect() {
        if (this.client) {
            this.client.disconnect();
            this._checkStatus();
            console.log("MQTT断连成功");
        }
    }

    _checkStatus() {
        if (this.client.isConnected()) this.showStatus.val(true).change();
        else this.showStatus.val(false).change();
        return this.client.isConnected();
    }

    // 连接成功时的回调函数
    _onConnect() {
        this.isConnecting = false;
        this._checkStatus();
        console.log("Connected to MQTT broker");
        let topic = this.SubTitle + this.topic
        this.client.subscribe(topic);
        console.log(`Subscribed to topic: ${topic}`);
    }

    // 连接失败时的回调函数
    _onFailure(error) {
        this.isConnecting = false;
        showError("请进行WebSocket安全认证");
        console.log("Connection failed:", error);
        this._checkStatus();
    }
}

class WebSocketHandler {
    constructor() {
        this.userName = null;
        this.onmessageEvent = null;
        this.socket = null;
    }

    initializeSocket(userName, onmessageEvent) {
        if (this.socket!=null && this.socket.readyState == 1) {
            this._closeSocket();
        }
        if (userName == "" || userName == null) {
            showWarning("请先输入UserName");
            return;
        }
        this.socket = new WebSocket("ws://10.136.16.135:1234");
        this.userName = userName;
        this.onmessageEvent = onmessageEvent;
        this.socket.onopen = (event) => this._handleOpen(event);
        this.socket.onmessage = (event) => this._handleMessage(event);
        this.socket.onerror = (event) => this._handleError(event);
        this.socket.onclose = (event) => this._handleClose(event);
    }

    _closeSocket() {
        this.socket.close();
        this.socket = null;
        this.userName = null;
    }
    _checkStatus() {
        //switch (this.socket.readyState) {
        //    case 0:
        //        showInfo("连接尚未建立");
        //        break;
        //    case 1:
        //        showInfo("连接已经建立");
        //        break;
        //    case 2:
        //        showInfo("连接正在进行关闭");
        //        break;
        //    case 3:
        //        showInfo("连接已经关闭");
        //        break;
        //    default:
        //        break;
        //}
        return this.socket.readyState;
    }

    _handleOpen(event) {
        if (this.userName !== "") {
            const message = "username:" + this.userName;
            this.socket.send(message);
            $("#socketstatus").val(true).change();
        }
    }

    _handleMessage(event) {
        const message = event.data;
        if (message.indexOf("error") !== -1) {
            console.log(message.substring(message.lastIndexOf(":") + 1));
        } else if (message.toUpperCase() !== "SUCCESS") {
            try {
                const obj = JSON.parse(message);
                if (obj && typeof obj === 'object') {
                    this.onmessageEvent(obj);
                }
            } catch (e) {
                console.log(message);
            }
        }
    }

    _handleError(event) {
        console.log("WebSocket错误: " + event);
    }

    _handleClose(event) {
        console.log("用户已断连");
        $("#socketstatus").val(false).change();
    }
}

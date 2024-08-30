class WebSocketHandler {
    constructor() {
        this.userName = null;
        this.onmessageEvent = null;
        this.socket = null;
        this.ShowStatus = $("#socketstatus") ;
    }

    //用户名，状态显示按钮，收到消息时触发的动作
    initializeSocket(userName,showStat, onmessageEvent) {
        if (this.socket!=null && this.socket.readyState == 1) {
            this._closeSocket();
        }
        if (userName == "" || userName == null) {
            showWarning("请先输入UserName");
            return;
        }
        this.userName = userName;
        this.ShowStatus = showStat;
        this.onmessageEvent = onmessageEvent;
        this.socket = new WebSocket("ws://10.136.16.135:1234");
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
        if (this.socket.readyState == 1) this.ShowStatus.val(true).change();
        else this.ShowStatus.val(false).change();
        return this.socket.readyState;
    }

    _handleOpen(event) {
        if (this.userName !== "") {
            const message = "username:" + this.userName;
            this.socket.send(message);
        }
    }

    _handleMessage(event) {
        const message = event.data;
        if (message.indexOf("error") !== -1) {
            if (message.indexOf("抱歉该用户已存在") > 0) {
                showWarning( "机台 "+this.userName+" 已经在线，无法重复注册相同的机台。")
            }
            console.log(message.substring(message.lastIndexOf(":") + 1));
            this._closeSocket();
        } else if (message.toUpperCase() !== "SUCCESS") {
            try {
                const obj = JSON.parse(message);
                if (obj && typeof obj === 'object') {
                    this.onmessageEvent(obj);
                }
            } catch (e) {
                console.log(message);
            } finally {
                this._checkStatus();
            }
        }
    }

    _handleError(event) {
        console.log("WebSocket错误: " + event);
        this._checkStatus();
    }

    _handleClose(event) {
        console.log("用户已断连");
        this._checkStatus();
    }
}

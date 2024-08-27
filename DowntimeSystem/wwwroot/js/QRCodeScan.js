////依赖的库
////       <script src="~/Scripts/Exteral/reqrcode.js"></script>
////       <script src="~/Scripts/Exteral/jsQR.js"></script>

////界面元素
////        < i class="fa fa-camera blue-font" id = "ScanQRCode" ></i >  
////        <input type="file" name="file" id="file" multiple="multiple" style="display:none;">
////        <video id="video"></video>
////        <canvas id="canvas" hidden></canvas>

////初始化
////        var video = document.querySelector('video');
////        var startbtn = $("#ScanQRCode");
////        var fileInput = $("#file");
////        var canvas = document.getElementById('canvas');
////        scan = new QRCodeScan(video, startbtn, fileInput, canvas, GenerateSNList);
   

class QRCodeScan {
    /**
     * 创建一个 MyClass 实例。
     * @constructor
     * @param {object} video - video元素。
     * @param {object} startbtn - 控制video开关的按钮。
     * @param {object} fileInput - 无法使用video时支持图片输入。
     * @param {object} canvasElement - 显示当前画面的Canvas。
     * @param {object} onScanEvent - 解析二维码之后执行的动作。
     *
     */
    constructor(video,startbtn,fileInput,canvasElement,onScanEvent) {
        this._video = video;
        this._fileInput = fileInput;
        this._btn = startbtn;
        this._isCameraStart = false;
        this._canvasElement = canvasElement;
        this._canvas = this._canvasElement.getContext('2d');
        this._t1 = null;
        this._onScanEvent = onScanEvent;
        this._media = null;
        this.vars = {
            result: null,
        };
        this._btn.click( ()=> {
            this._Scan()
        });
        this._fileInput.change( () =>{
            this._ScanCode(this._fileInput[0]);
        })
    }
    //点击按钮开始或停止扫码
    _Scan() {
        if (!navigator.mediaDevices || !navigator.mediaDevices.enumerateDevices) {
            console.log("不支持摄像模式，请选择图片");
            this._fileInput.trigger("click");
        }
        else {
            navigator.mediaDevices.getUserMedia({ video: true })
                .then(res => {
                    if (this._isCameraStart) {
                        this._qstop();
                    } else {
                        this._qstart();
                    }
                })
                .catch(err => {
                    console.log(err);
                    this._fileInput.trigger("click");
                });
        }
    }
    //关闭摄像头
    _qstop() {
        try {
            this._isCameraStart = false;
            window.clearInterval(this._t1);
            if (this._video) {
                this._video.pause();
                this._media.getTracks()[0].stop()
            }
            this._video.style.visibility = "hidden";
        } catch (err) {
            showError("stop:" + err);
        }
    }
    //打开摄像头
    _qstart() {
        this._isCameraStart = true;
        $(this._btn).children('.description').html("Stop");
        navigator.mediaDevices.getUserMedia({ audio: false, video: { facingMode: { ideal: 'environment' }} })
            .then(stream => {
                //// 旧的浏览器可能没有srcObject
                //video = document.createElement('video');
                //// 旧的浏览器可能没有srcObject
                if ("srcObject" in this._video) {
                    this._video.srcObject = stream;
                } else {
                    // 防止在新的浏览器里使用它，应为它已经不再支持了
                    this._video.src = window.URL.createObjectURL(stream);
                }
                this._media = stream;
                this._video.setAttribute('playsinline', true); // required to tell iOS safari we don't want fullscreen
                this._video.style.visibility = "visible";
                this._video.play();
                window.requestAnimationFrame(this._tick.bind(this));
            })
            .catch(err =>  {
                showError("start :" + err.name + ": " + err.message);
                this._qstop();
            });
}
    //获取摄像头内容，动态识别
    _tick() {
        try {
            let that = this;
            that._t1 = window.setInterval( function(e) {
                if (that._video.readyState === that._video.HAVE_ENOUGH_DATA) {
                    that._canvasElement.height = that._video.videoHeight;
                    that._canvasElement.width = that._video.videoWidth;
                    that._canvas.drawImage(that._video, 0, 0, that._canvasElement.width, that._canvasElement.height);
                    var imageData = that._canvas.getImageData(0, 0, that._canvasElement.width, that._canvasElement.height);
                    var code = jsQR(imageData.data, imageData.width, imageData.height, {
                        inversionAttempts: 'dontInvert',
                    });
                    if (code) {
                        that.vars.result = code.data;
                        that._onScanEvent(code.data);
                        that._qstop();
                    }
                }
            }, 100);
        } catch (err) {
            console.log("tick:" + err);
            this._qstop();
        }
    }
    //直接解图片
    _ScanCode(file) {
        console.log(getObjectURL(file.files[0]));           // newfile[0]是通过input file上传的二维码图片文件
        qrcode.decode(getObjectURL(file.files[0]));
        qrcode.callback =  (imgMsg)=> {
            this._onScanEvent(imgMsg);
        }
    }
}
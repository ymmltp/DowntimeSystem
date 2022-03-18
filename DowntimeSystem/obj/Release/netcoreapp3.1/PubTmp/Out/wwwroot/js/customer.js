function iniDatetimepicker() {
    $(".form_datetime").datetimepicker({
        fontAwesome: 'font-awesome',
        format: 'yyyy-mm-dd',//hh:00:00', //时间显示的格式
        todayBtn: true, //一键选中今天的日期
        minDate: '2022/01/01',
        maxDate: 0,//今天
        pickerPosition: "bottom-left", //打开选择卡的位置
        weekStart: 1, //周开始的星期：0-6 星期日-星期六
        autoclose: true,//选好时间后自动关闭
        startView: 2,
        maxView: 4,
        minView: 2,//显示的最小选项卡：0-4 hour,day,month,year,decade
        //minuteStep: 5,
        language: 'zh-CN',
        startDate: new Date("2022-01-01"),
        endDate: new Date()
    });
}

//正数为后一天，负数为前一天，str为日期间隔符，例如 -,/ 
function getDay(num, str) {
    var today = new Date();
    var nowTime = today.getTime();
    var ms = 24 * 3600 * 1000 * num;
    today.setTime(parseInt(nowTime + ms));
    var oYear = today.getFullYear();
    var oMoth = (today.getMonth() + 1).toString();
    if (oMoth.length <= 1)
        oMoth = '0' + oMoth;
    var oDay = today.getDate().toString();
    if (oDay.length <= 1)
        oDay = '0' + oDay;
    return oYear + str + oMoth + str + oDay;
}


//Date->string
function changeDateFormat(cellval) {
    if (cellval != null) {
        var d = new Date(parseInt(cellval.replace("/Date(", "").replace(")/", ""), 10));
        return formatDate(d);
    } else {
        return cellval;
    }
}
function formatDateTime(date) {
    var y = date.getFullYear();
    var m = date.getMonth() + 1;
    m = m < 10 ? ('0' + m) : m;
    var d = date.getDate();
    d = d < 10 ? ('0' + d) : d;
    var h = date.getHours();
    var minute = date.getMinutes();
    minute = minute < 10 ? ('0' + minute) : minute;
    var second = date.getSeconds();
    second = second < 10 ? ('0' + second) : second;
    return y + '-' + m + '-' + d + ' ' + h + ':' + minute + ':' + second;
}
function formatDate(date) {
    var y = date.getFullYear();
    var m = date.getMonth() + 1;
    m = m < 10 ? ('0' + m) : m;
    var d = date.getDate();
    d = d < 10 ? ('0' + d) : d;
    return y + '-' + m + '-' + d;
}


function getData(url, para = null) {
    return new Promise(function (resolve, reject) {
        $.ajax({
            url: url,
            data: para,
            type: "GET",
            dataType: "json",
            success: function (data, status, xhr) {
                if (xhr.status === 200) {
                    resolve(data);
                }
                else {
                    reject(xhr.statusText);
                }
            },
            fail: function (err, status, xhr) {
                reject(err.responseTextr);
            },
            error: function (err, status, xhr) {
                reject(err.responseText);
            }
        });
    });
}
function getDataWithArray(url, para = null) {
    return new Promise(function (resolve, reject) {
        $.ajax({
            url: url,
            data: para,
            type: "GET",
            traditional: true,
            dataType: "json",
            success: function (data, status, xhr) {
                resolve(data);
            },
            fail: function (err, status, xhr) {
                reject({ err: err, status: status, xhr: xhr });
            },
            error: function (e) {
                reject(e.responseText);
            }
        });
    });
}

//查询及一些基础ajax方法
function getLine(obj) {
    $.ajax({
        url: '/EC/GetLine',
        method: 'GET',
        dataType: 'json',
        success: function (data) {
            var option = "";
            for (var i = 0; i < data.length; i++) {
                option += '<option value="' + data[i] + '">' + data[i] + '</option>';
            }
            obj.html(option);
            obj.selectpicker('refresh');
        },
        fail: function (err) {
            console.log(err);
            alert(err.statusText);
        },
        error: function (err) {
            console.log(err);
            alert(err.statusText);
        }
    })
}
function getProject(obj,line) {
    $.ajax({
        url: '/EC/GetProject',
        method: 'GET',
        data: {
            Line: line?line[0]:null,
        },
        dataType: 'json',
        success: function (data) {
            var option = "";
            for (var i = 0; i < data.length; i++) {
                option += '<option value="' + data[i] + '">' + data[i] + '</option>';
            }
            obj.html(option);
            obj.selectpicker('refresh');
        },
        fail: function (err) {
            console.log(err);
            alert(err.statusText);
        },
        error: function (err) {
            console.log(err);
            alert(err.statusText);
        }
    })
}
function getStation(obj,line,project) {
    $.ajax({
        url: '/EC/GetStation',
        method: 'GET',
        data: {
            Line: line?line[0]:null,
            Project: project?project[0]:null,
        },
        dataType: 'json',
        success: function (data) {
            var option = "";
            for (var i = 0; i < data.length; i++) {
                option += '<option value="' + data[i] + '">' + data[i] + '</option>';
            }
            obj.html(option);
            obj.selectpicker('refresh');
        },
        fail: function (err) {
            console.log(err);
            alert(err.statusText);
        },
        error: function (err) {
            console.log(err);
            alert(err.statusText);
        }
    })
}


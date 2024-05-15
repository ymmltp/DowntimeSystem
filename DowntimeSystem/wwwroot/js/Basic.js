const BasicURL = "http://cnwuxg0te01:9000";


//#region datetimepicker 初始化设定
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
function iniDatetimepicker_withTime() {
    $(".form_datetime").datetimepicker({
        fontAwesome: 'font-awesome',
        format: 'yyyy-mm-dd hh:ii:00',//hh:00:00', //时间显示的格式
        todayBtn: true, //一键选中今天的日期
        minDate: '2022/01/01',
        maxDate: 0,//今天
        pickerPosition: "bottom-left", //打开选择卡的位置
        weekStart: 1, //周开始的星期：0-6 星期日-星期六
        autoclose: true,//选好时间后自动关闭
        startView: 0,
        maxView: 4,
        minView: 0,//显示的最小选项卡：0-4 hour,day,month,year,decade
        minuteStep: 2,
        language: 'zh-CN',
        startDate: new Date("2022-01-01"),
        endDate: new Date()
    });
}
//#endregion

//#region 可隐藏的搜索栏
$("#searchBox").on('click', function () {
    if ($("#searchBoxMenu").hasClass("elehide")) {
        $("#searchBoxMenu").removeClass("elehide");
        $("#searchBox").removeClass("fa-cog");
        $("#searchBox").addClass("fa-minus-square");
    }
    else {
        $("#searchBoxMenu").addClass("elehide");
        $("#searchBox").removeClass("fa-minus-square");
        $("#searchBox").addClass("fa-cog");
    }
})
//#endregion

//#region 格式化日期2024-02-01/ 
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
//#endregion

//#region 时间格式化 Date->string
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
//#endregion

//#region Promise封装
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
function postData(url, para) {
    return new Promise(function (resolve, reject) {
        $.ajax({
            url: url,
            type: "POST",
            data: para,
            success(data, status, xhr) {
                resolve({ data: data, status: status, xhr: xhr });
            },
            fail(err, status, xhr) {
                reject(err.responseTextr);
            },
            error: function (err) {
                reject(err.responseText);
            }
        });
    });
}
function postDataWithArray(url, para) {
    return new Promise(function (resolve, reject) {
        $.ajax({
            url: url,
            type: "POST",
            contentType: "application/json;charset=utf-8",
            data: JSON.stringify(para),
            success(data, status, xhr) {
                resolve({ data: data, status: status, xhr: xhr });
            },
            fail(err, status, xhr) {
                reject(err.responseTextr);
            },
            error: function (err) {
                reject(err.responseText);
            }
        });
    });
}
function deleteData(url, para) {
    return new Promise(function (resolve, reject) {
        $.ajax({
            url: url,
            type: "Delete",
            data: para,
            success(data, status, xhr) {
                resolve({ data: data, status: status, xhr: xhr });
            },
            fail(err, status, xhr) {
                reject(err.responseTextr);
            },
            error: function (err) {
                reject(err.responseText);
            }
        });
    });
}
function postData_CORS(url, para, contentype = "application/json") {
    return new Promise(function (resolve, reject) {
        $.ajax({
            url: url,
            method: "POST",          
            contentType: contentype,
            beforeSend: function (xhr) {
                xhr.setRequestHeader('Access-Control-Allow-Origin', '*');
            },
            data: JSON.stringify(para),
            success(data, status, xhr) {
                resolve({ data: data, status: status, xhr: xhr });
            },
            fail(err, status, xhr) {
                reject(err);
            },
            error: function (err) {
                reject(err);
            }
        });
    });
}
//#endregion

//#region ajax跨域查询Api接口
function getDataFromAPI(url, para = null) {
    return new Promise(function (resolve, reject) {
        $.ajax({
            url: url,
            data: para,
            beforeSend: function (xhr) {
                xhr.setRequestHeader('Access-Control-Allow-Origin', '*');
            },
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
function postDatatoAPI(url, para) {
    return new Promise(function (resolve, reject) {
        $.ajax({
            url: url,
            type: "POST",
            contentType: "application/json",
            beforeSend: function (xhr) {
                xhr.setRequestHeader('Access-Control-Allow-Origin', '*');
            },
            data: JSON.stringify(para),
            success(data, status, xhr) {
                resolve({ data: data, status: status, xhr: xhr });
            },
            fail(err, status, xhr) {
                reject(err.responseText);
            },
            error: function (xhr, status, error) {
                reject(xhr.responseText);
            }
        });
    });
}
function deleteDatatoAPI(url, para = null) {
    return new Promise(function (resolve, reject) {
        $.ajax({
            url: url,
            type: "delete",
            beforeSend: function (xhr) {
                xhr.setRequestHeader('Access-Control-Allow-Origin', '*');
            },
            data: para,
            success(data, status, xhr) {
                resolve({ data: data, status: status, xhr: xhr });
            },
            fail(err, status, xhr) {
                reject({ err: err, status: status, xhr: xhr });
            },
            error: function (e) {
                reject(e.responseText);
            }
        });
    });
}
//put 类型不被认可
function putDatatoAPI(url, para = null) {
    return new Promise(function (resolve, reject) {
        $.ajax({
            url: url,
            type: "put",
            beforeSend: function (xhr) {
                xhr.setRequestHeader('Access-Control-Allow-Origin', '*');
            },
            data: para,
            success(data, status, xhr) {
                resolve({ data: data, status: status, xhr: xhr });
            },
            fail(err, status, xhr) {
                reject({ err: err, status: status, xhr: xhr });
            },
            error: function (e) {
                reject(e.responseText);
            }
        });
    });
}
//#endregion

//#region 获取URL中的参数内容
function GetParms(name) {
    var url = decodeURI(window.location.href);
    var index = url.indexOf('?');
    var str = url.substring(index + 1);
    var arr = str.split('&');
    var result = {};
    arr.forEach((item) => {
        var a = item.split('=');
        result[a[0]] = a[1];
    });
    return result[name];
}
//#endregion

//#region 判断界面元素不为空
function checkFormNoNull(parentid) {
    var flag = true;
    $(parentid + " .noNull").not('.elehide').each(function () {
        var name = $(this).attr("name");
        if ($(this).attr("type") == "radio") {
            if ($('input[name="' + name + '"]:checked').length < 1) {
                showWarning($(this).attr('noNull') + "不能为空!");
                flag = false;
                return false;
            }
        }
        else if ($(this).attr("type") == "checkbox") {
            if ($('input[name="' + name + '"]:checked').length < 1) {
                showWarning($(this).attr('noNull') + "不能为空!");
                flag = false;
                return false;
            }
        }
        else if ($(this).val().length == 0) {
            if ($(this)[0].attributes.length > 1) {
                showWarning($(this).attr('noNull') + "不能为空!");
                flag = false;
                return false;
            }
        }
    });
    return flag;
}
//#endregion

//#region 配合alert.css,显示弹出信息
function showWarning(text) {
    $('.alert').attr('class', 'alert');
    $('.alert').html(text).addClass('alert-danger').show().delay(1500).fadeOut();
}
function showInfo(text) {
    $('.alert').attr('class', 'alert');
    $('.alert').html(text).addClass('alert-info').show().delay(1500).fadeOut();
}
function showInfo_long(text) {
    $('.alert').attr('class', 'alert');
    $('.alert').html(text).addClass('alert-info').show().delay(8000).fadeOut();
}
function showSuccess(text) {
    $('.alert').attr('class', 'alert');
    $('.alert').html(text).addClass('alert-success').show().delay(1500).fadeOut();
}
function showError(text) {
    $('.alert').attr('class', 'alert');
    $('.alert').html(text).addClass('alert-danger').show().delay(1500).fadeOut();
}
//#endregion

//#region 获取对应的select的option

    //针对返回格式为array的类型 []
function GetSelectOptions(url, paras, obj, value = null) {
    $.ajax({
        url: url,
        method: 'GET',
        data: paras,
        dataType: 'json',
        traditional: true,
        success: function (data) {
            let option = '';
            for (var i = 0; i < data.length; i++) {
                option += '<option value="' + data[i] + '">' + data[i] + '</option>';
            }
            obj.html(option);
            if (value != null && value.length >= 0) {
                obj.val(value);
            }
            obj.selectpicker("refresh");
        },
        fail: function (err) {
            showWarning(err.responseText);
        },
        error: function (err) {
            showWarning(err.responseText);
        }
    })
}

    //针对返回格式为list{paras：}  的类型 
function GetSelectOptions_paras(url, paras, obj, value = null) {
    $.ajax({
        url: url,
        method: 'GET',
        data: paras,
        dataType: 'json',
        traditional: true,
        success: function (data) {
            let option = '';
            for (var i = 0; i < data.length; i++) {
                option += '<option value="' + data[i].paras + '">' + data[i].paras + '</option>';
            }
            obj.html(option);
            if (value != null && value.length >= 0) {
                obj.val(value);
            }
            obj.selectpicker("refresh");
        },
        fail: function (err) {
            showWarning(err.responseText);
        },
        error: function (err) {
            showWarning(err.responseText);
        }
    })
}

    //针对返回格式为list{key： ,value：}  的类型 
function GetSelectOptions_KeyVal(url, paras, obj, value = null) {
    $.ajax({
        url: url,
        method: 'GET',
        data: paras,
        dataType: 'json',
        traditional: true,
        success: function (data) {
            let option = '';
            for (var i = 0; i < data.length; i++) {
                option += '<option value="' + data[i].key + '">' + data[i].value + '</option>';
            }
            obj.html(option);
            if (value != null && value.length >= 0) {
                obj.val(value);
            }
            obj.selectpicker("refresh");
        },
        fail: function (err) {
            showWarning(err.responseText);
        },
        error: function (err) {
            showWarning(err.responseText);
        }
    })
}

//#endregion

//#region 自动清空模态框内的input内容  不执行
document.addEventListener('hidden.bs.modal', function (event) {
    // 获取触发事件的模态框
    var modal = event.target;
    if (modal.classList.contains('modal')) {
        // 清空模态框内部所有输入框的值
        var inputs = modal.querySelectorAll('input');
        inputs.forEach(function (input) {
            input.value = '';
        });
        var textarea = modal.querySelectorAll('textarea');
        textarea.forEach(function (i) {
            i.value = '';
        });
    }
});
//#endregion


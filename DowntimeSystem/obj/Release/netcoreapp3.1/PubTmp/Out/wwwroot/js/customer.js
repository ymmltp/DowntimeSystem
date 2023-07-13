//#region datetimepicker 设定
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


//获取周
function getWeekList(obj) {
    var currentYear = new Date().getFullYear();
    $.ajax({
        url: '/Basic/GetCurrentWeek',
        method: 'GET',
        dataType: 'json',
        data: { date: new Date(currentYear + "-12-31").format('yyyy-MM-dd') },
        success: function (data) {
            var option = "";
            for (var i = 1; i <= data; i++) {
                option += '<option value="' + currentYear.toString() + (Array(2).join(0) + i).slice(-2) + '">' + i + '</option>';
            }
            obj.html(option);
            obj.selectpicker('refresh');
        },
        fail: function (err) {
            showWarning(err.statusText);
        },
        error: function (err) {
            showWarning(err.statusText);
        }
    })


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
function postData(url, para) {
    return new Promise(function (resolve, reject) {
        $.ajax({
            url: url,
            type: "POST",
            data:para,
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


//查询及一些基础ajax方法
function getDepartment(obj) {
    $.ajax({
        url: '/EC/GetDepartment',
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
            showWarning(err.statusText);
        },
        error: function (err) {
            console.log(err);
            showWarning(err.statusText);
        }
    })
}
function getLine(obj,project) {
    $.ajax({
        url: '/EC/GetLine',
        method: 'GET',
        data: {
            Project: project ? project[0] : null,
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
            showWarning(err.statusText);
        },
        error: function (err) {
            console.log(err);
            showWarning(err.statusText);
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
            showWarning(err.statusText);
        },
        error: function (err) {
            console.log(err);
            showWarning(err.statusText);
        }
    })
}
function getStation(obj, department, project,line) {
    getDataWithArray("/EC/GetStation",
        {
            departmentList: department ,
            projectList: project,
            lineList: line,
        })
        .then(data => {
            var option = "";
            for (var i = 0; i < data.length; i++) {
                option += '<option value="' + data[i] + '">' + data[i] + '</option>';
            }
            obj.html(option);
            obj.selectpicker('refresh');
        })
}
function getDashboardSystem(obj) {
    $.ajax({
        url: '/Dashboard/GetSystem',
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
            showWarning(err.statusText);
        },
        error: function (err) {
            console.log(err);
            showWarning(err.statusText);
        }
    })
}

//获取URL中的参数内容
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

function checkFormNoNull(parentid) {
    var flag = true;

    $( parentid+" .noNull").each(function () {
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


/**
 * 合并单元格
 * @param data 原始数据（在服务端完成排序）
 * @param fieldName 合并属性名称
 * @param colspan  合并列
 * @param target  目标表格对象
 */
function mergeCells(data, fieldName, colspan, target) {
    //声明一个map计算相同属性值在data对象出现的次数和
    var sortMap = {};
    var totalqtyMap = {};
    var startindexMap = {};
    var sumQTY = 0;
    for (var i = 0; i < data.length; i++) {
        sumQTY += data[i]['qty'];
        for (var prop in data[i]) {
            if (prop == fieldName) {
                var key = data[i][prop]
                if (sortMap.hasOwnProperty(key)) {
                    sortMap[key] = sortMap[key] * 1 + 1;
                    totalqtyMap[key] = totalqtyMap[key] + data[i]['qty'];
                } else {
                    sortMap[key] = 1;
                    totalqtyMap[key] = data[i]['qty'];
                    startindexMap[key] = i;
                }
                break;
            } 
        }
    }

    //更新Analysis Table的数据
    for (var prop in startindexMap) {
        $(target).bootstrapTable('updateCell', {
            index: startindexMap[prop],
            field: 'totalqty',
            value: totalqtyMap[prop]
        });
        $(target).bootstrapTable('updateCell', {
            index: startindexMap[prop],
            field: 'totalPercent',
            value: parseInt(totalqtyMap[prop] / sumQTY * 100) + "%"
        });
    }
    for (var i = 0; i < data.length; i++) {
        $(target).bootstrapTable('updateCell', {
            index: i,
            field: 'subPercent',
            value: parseInt(data[i]['qty'] / sumQTY * 100) + "%"
        });
    }


    //合并单元格
    var index = 0;
    for (var prop in sortMap) {
        var count = sortMap[prop] * 1;
        $(target).bootstrapTable('mergeCells', { index: index, field: fieldName, colspan: colspan, rowspan: count });
        $(target).bootstrapTable('mergeCells', { index: index, field: 'totalqty', colspan: colspan, rowspan: count });
        $(target).bootstrapTable('mergeCells', { index: index, field: 'totalPercent', colspan: colspan, rowspan: count });
        index += count;
    }
}


//获取对应的select的option
function GetSelectOptions(url,paras,obj,value=null) {
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


const _FAC= "WUX-FATP"
//#region  获取iFactory中的信息
function GetIFRoute(obj, fac = _FAC) {
    var paras = {
        url: "/api/Route/GetRoutes?Factory=" + fac,
    };
    $.ajax({
        url: 'http://cnwuxg0te01:9000/api/iFactory/iFactoryGetMethod',
        method: 'GET',
        data: paras,
        dataType: 'json',
        traditional: true,
        success: function (res) {
            var data = res["routes"];
            let option = '';
            for (var i = 0; i < data.length; i++) {
                option += '<option value="' + data[i]["name"] + '">' + data[i]["name"] + '</option>';
            }
            obj.html(option);
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
function GetIFRouteStep(obj, route, fac = _FAC) {
    var paras = {
        url: "/api/Route/GetRouteSteps?Factory=" + fac + "&RouteName=" + route.val()[0],
    };
    $.ajax({
        url: 'http://cnwuxg0te01:9000/api/iFactory/iFactoryGetMethod',
        method: 'GET',
        data: paras,
        dataType: 'json',
        traditional: true,
        success: function (res) {
            var data = res["routeSteps"];
            let option = '';
            for (var i = 0; i < data.length; i++) {
                option += '<option value="' + data[i]["resourceName"] + '">' + data[i]["resourceName"]  + '</option>';
            }
            obj.html(option);
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

//#region  获取PMMS中的信息
function GetPMMSDepartment(obj) {
    //GetSelectOptions('/PMMS/GetDepartment', null, obj);
    GetSelectOptions('http://cnwuxg0te01:9000/api/PMMS/Get_Department', null, obj);

}
function GetPMMSProject(obj) {
    //GetSelectOptions('/PMMS/GetProject', null, obj);
    GetSelectOptions('http://cnwuxg0te01:9000/api/PMMS/Get_Project', null, obj);

}
function GetPMMSLine(obj, department, project) {
    var paras = {
        department: department ? department.val() : null,
        project: project ? project.val() : null,
    };
    GetSelectOptions('http://cnwuxg0te01:9000/api/PMMS/Get_Line', paras, obj);
    //GetSelectOptions('/PMMS/GetLine', paras, obj);
}
function GetPMMSEQType(obj, department, project, line) {
    var paras = {
        department: department ? department.val() : null,
        project: project ? project.val() : null,
        line: line ? line.val() : null,
    };
    //GetSelectOptions('/PMMS/GetEQType', paras, obj);
    GetSelectOptions('http://cnwuxg0te01:9000/api/PMMS/Get_Category', paras, obj);
}

function GetEQID(obj, department, project, line, type, eqid) {
    var paras = {
        department: department ? department.val() : null,
        project: project ? project.val() : null,
        line: line ? line.val() : null,
        type: type ? type.val() : null,
    };
    //GetSelectOptions('/PMMS/getEQID', paras, obj, eqid);
    GetSelectOptions('http://cnwuxg0te01:9000/api/PMMS/Get_EQID', paras, obj);
}
//#endregion

//#region  获取Sparepart中的信息
function GetSPDepartment(obj) {
    GetSelectOptions('http://cnwuxg0te01:9000/api/Basic/GetDepartment', null, obj);
}
function GetSPProject(obj) {
    GetSelectOptions('http://cnwuxg0te01:9000/api/Basic/GetProject', null, obj);
}
function GetSPCategory(obj) {
    GetSelectOptions('http://cnwuxg0te01:9000/api/SparepartBasic/GetCategory', null, obj);
}
function GetSPSubCategory(obj, category) {
    var paras = {
        category: category ? category : null,
    }
    if (category) {
        GetSelectOptions('http://cnwuxg0te01:9000/api/SparepartBasic/GetSubCategory_ByCategory', paras, obj);
    }
    else {
        GetSelectOptions('http://cnwuxg0te01:9000/api/SparepartBasic/GetSubCategory', null, obj);
    }


}
function GetPN(obj, category, subcategory) {
    var paras = {
        category: category ? category.val() : null,
        subcategory: subcategory ? subcategory.val() : null,
    };
    GetSelectOptions('http://cnwuxg0te01:9000/api/SparepartDescription/GetPN_byCategory', paras, obj);
}
//#endregion


//#region 获取PN AlarmType
function GetPNAlarmType(obj) {
    $.ajax({
        url: 'http://cnwuxg0te01:9000/api/EqSparepartChange/Query_AlarmType',
        method: 'GET',
        dataType: 'json',
        traditional: true,
        success: function (res) {
            let option = '';
            for (var i = 0; i < data.length; i++) {
                option += '<option value="' + data[i]["value"] + '">' + data[i]["key"] + '</option>';
            }
            obj.html(option);
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
function GetSparepart_HistoryCfrom(obj) {
    $.ajax({
        url: 'http://cnwuxg0te01:9000/api/EqSparepartChange/Query_History_Cfrom',
        method: 'GET',
        dataType: 'json',
        traditional: true,
        success: function (data) {
            let option = '';
            for (var i = 0; i < data.length; i++) {
                option += '<option value="' + data[i] + '">' + data[i] + '</option>';
            }
            obj.html(option);
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
function GetEQIDLink_PN(obj,eqid) {
    $.ajax({
        url: 'http://cnwuxg0te01:9000/api/SparepartDescription/GetSparepartDescription_ByEQ_PN_LinkInfo',
        method: 'GET',
        dataType: 'json',
        data: {
            eqid: eqid ? eqid : null,
        },
        success: function (data) {
                let option = '';
                for (var i = 0; i < data.length; i++) {
                    option += '<option value="' + data[i]["pn"] + '">' + data[i]["pn"] + "/" + data[i]["brand"] + "/" + data[i]["modelType"] + "/" + data[i]["name"] + '</option>';
                }
                if (obj.find('optgroup').filter('[label="PN Linked with EQID"]').length > 0) {
                    $(obj.find('optgroup').filter('[label="PN Linked with EQID"]')[0]).html(option);
                }
                else {
                    obj.html(option);
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
function GetAllPN(obj, category, subcategory) {
    $.ajax({
        url: 'http://cnwuxg0te01:9000/api/SparepartDescription/GetSparepartDescription_byCategory',
        method: 'GET',
        dataType: 'json',
        traditional: true,
        data: {
            Category: category ? category : null,
            subCategory: subcategory ? subcategory : null,
        },
        success: function (data) {
                let option = '';
                for (var i = 0; i < data.length; i++) {
                    option += '<option value="' + data[i]["pn"] + '">' + data[i]["pn"] + "/" + data[i]["brand"] + "/" + data[i]["modelType"] + "/" + data[i]["name"] + '</option>';
                }
                if (obj.find('optgroup').filter('[label="Other PN"]').length > 0) {
                    $(obj.find('optgroup').filter('[label="Other PN"]')[0]).html(option);
                }
                else {
                    obj.html(option);
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
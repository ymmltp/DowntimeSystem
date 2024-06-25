
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

//查询及一些基础ajax方法
function getDepartment(obj) {
    
    $.ajax({       
        url: '/EC/GetDepartment',
        method: 'GET',
        dataType: 'json',
        contentType:'json',
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
function getDepartment2(obj) {

    $.ajax({
        url: '/EC/GetDepartment',
        method: 'GET',
        dataType: 'json',
        contentType: 'json',
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
function getDepartment1(obj) {

    $.ajax({
        url: 'http://cnwuxg0te01:9000/api/eCallingBasic/GetDepartment',
        method: 'GET',
        dataType: 'json',
        contentType: 'json',
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
function getLine(obj, project) {
    getDataWithArray('/EC/GetLine', { projectList: project })
        .then(data => {
            var option = "";
            for (var i = 0; i < data.length; i++) {
                option += '<option value="' + data[i] + '">' + data[i] + '</option>';
            }
            obj.html(option);
            obj.selectpicker('refresh');
        })
}
function getLine1(obj, project) {
    getDataWithArray('http://cnwuxg0te01:9000/api/eCallingBasic/GetLine', { project: project })
        .then(data => {
            var option = "";
            for (var i = 0; i < data.length; i++) {
                option += '<option value="' + data[i].paras + '">' + data[i].paras + '</option>';
            }
            obj.html(option);
            obj.selectpicker('refresh');
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
function getProject1(obj) {
    $.ajax({
        url: 'http://cnwuxg0te01:9000/api/eCallingBasic/GetProject',
        method: 'GET',
       
        dataType: 'json',
        success: function (data) {
            var option = "";
            for (var i = 0; i < data.length; i++) {
                option += '<option value="' + data[i].paras + '">' + data[i].paras + '</option>';
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
function getStation1(obj, project, line) {
    getDataWithArray('http://cnwuxg0te01:9000/api/eCallingBasic/GetStation',
        {
           
            Line: line,
            Project: project,
        })
        .then(data => {
            var option = "";
            for (var i = 0; i < data.length; i++) {
                option += '<option value="' + data[i].paras + '">' + data[i].paras + '</option>';
            }
            obj.html(option);
            obj.selectpicker('refresh');
        })
}

function getErrorCode(obj, Department, vbnumber) {
    var paras = {
        department: Department,
        vbnumber: vbnumber,
    };
    $.ajax({
        url: 'http://cnwuxg0te01:9000/api/eCallingBasic/GetErrorCode',
        method: 'GET',
        data: paras,
        dataType: 'json',
        success: function (data) {
            let option = '';
            for (var i = 0; i < data.length; i++) {
                option += '<option value="' + data[i].issue + '" data-issue="' + data[i].callDepartment + ',' + data[i].callIssue + '" data-isdowntime="' + data[i].isdowntime + '">' + data[i].issue + '</option>';
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
    });
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
function getMachine(machine, Line, Project, obj) {
    $.ajax({
        url: 'http://cnwuxg0te01:9000/api/eCallingBasic/GetMachine',
        data: {
            Station: machine,
            Line: Line,
            Project:Project,
        },
        method: 'GET',
        dataType: 'json',
        success: function (data) {
            var option = "";
            for (var i = 0; i < data.length; i++) {
                option += '<option value="' + data[i].resource + '" data-server="' + data[i].server + '" data-vbnumber="' + data[i].vbnumber + '">' + data[i].resource + '</option>';
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
function getLastIncdent(Machine, obj) {
    return getData("http://cnwuxg0te01:9000/api/Downtime/Query_LastIncdentByFilter", { Machine: Machine })
        .then(data => {
            const startDateValue = $("#startdate").val(); // 获取 #startdate 元素的值

            const startDate = new Date(startDateValue); // 创建 Date 对象
            const isoString = startDate.toISOString(); // 转换为 ISO 8601 格式的字符串

            data.occurtime = isoString; // 赋值给 data.occurtime
           /* data.occurtime = $("#startdate").val();*/
            data.creator = $("#creator").val();
            data.frequency = Number($("#howoften").val());
            data.pieces = Number( $("#howmuch").val()); // 可能会引发异常的代码  //
            console.log(data);
            return postDataWithArray("http://cnwuxg0te01:9000/api/Downtime/Update", data);
           
        })
    
        .catch(err => {
            showWarning(err);
        })
}


//function getDepartment1(obj) {

//    $.ajax({
//        url: 'http://cnwuxg0te01:9000/api/DowntimeBasic/GetDepartment_FromMatrix',
//        method: 'GET',
//        dataType: 'json',
//        contentType: 'json',
//        success: function (data) {
//            var option = "";
//            for (var i = 0; i < data.length; i++) {
//                option += '<option value="' + data[i].paras + '">' + data[i].paras + '</option>';
//            }
//            obj.html(option);
//            obj.selectpicker('refresh');
//        },
//        fail: function (err) {
//            console.log(err);
//            showWarning(err.statusText);
//        },
//        error: function (err) {
//            console.log(err);
//            showWarning(err.statusText);
//        }
//    })
//}



/**
 *合并单元格
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
    GetSelectOptions('http://cnwuxg0te01:9000/api/PMMS/Get_Department', null, obj);

}
function GetPMMSProject(obj) {
    GetSelectOptions('http://cnwuxg0te01:9000/api/PMMS/Get_Project', null, obj);

}
function GetPMMSLine(obj, department, project) {
    var paras = {
        department: department ? department.val() : null,
        project: project ? project.val() : null,
    };
    GetSelectOptions('http://cnwuxg0te01:9000/api/PMMS/Get_Line', paras, obj);
}
function GetPMMSEQType(obj, department, project, line) {
    var paras = {
        department: department ? department.val() : null,
        project: project ? project.val() : null,
        line: line ? line.val() : null,
    };
    GetSelectOptions('http://cnwuxg0te01:9000/api/PMMS/Get_Category', paras, obj);
}
function GetEQID(obj, department, project, line, type, eqid) {
    var paras = {
        department: department ? department.val() : null,
        project: project ? project.val() : null,
        line: line ? line.val() : null,
        type: type ? type.val() : null,
    };
    GetSelectOptions('http://cnwuxg0te01:9000/api/PMMS/Get_EQID', paras, obj);
}
//#endregion

//#region  获取Sparepart中的信息
function GetSPDepartment(obj) {
    GetSelectOptions('http://cnwuxg0te01:9000/api/Basic/GetDepartmentWithOutAll', null, obj);
}
function GetSPProject(obj) {
    GetSelectOptions('http://cnwuxg0te01:9000/api/Basic/GetProjectWithOutAll', null, obj);
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

//#region 获取eCalling中的信息
function GeteCallingDepartment(obj) {
    GetSelectOptions_paras(BasicURL +'/api/eCallingBasic/GetDepartment', null, obj);
}
function GeteCallingProject(obj) {
    GetSelectOptions_paras(BasicURL + '/api/eCallingBasic/GetProject', null, obj);
}
function GeteCallingLine(obj, project) {
    let paras = { project: project };
    GetSelectOptions_paras(BasicURL + '/api/eCallingBasic/GetLine', paras, obj);
}
function GeteCallingStation(obj, project, line) {
    let paras = {
        project: project,
        line:line,
    };
    GetSelectOptions_paras(BasicURL + '/api/eCallingBasic/GetStation', paras, obj);
}
function GeteCallingMachine(obj,  project, line, station, eqid) {
    let paras = {
        project: project,
        line: line,
        station:station,
    };
    $.ajax({
        url: BasicURL + '/api/eCallingBasic/GetMachine',
        method: 'GET',
        data: paras,
        dataType: 'json',
        traditional: true,
        success: function (data) {
            let option = '';
            for (var i = 0; i < data.length; i++) {
                option += '<option value="' + data[i].resource + '" data-server="' + data[i].server + '" data-vbnumber="' + data[i].vbnumber + '">' + data[i].resource + '</option>';
            }
            obj.html(option);
            if (eqid != null && eqid.length >= 0) {
                obj.val(eqid);
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
function GeteCallingErrocCode(obj, department, vbnumber) {
    var paras = {
        department: department,
        vbnumber: vbnumber
    };
    $.ajax({
        url: BasicURL + '/api/eCallingBasic/GetErrorCode',
        method: 'GET',
        data: paras,
        dataType: 'json',
        traditional: true,
        success: function (data) {
            let option = '';
            for (var i = 0; i < data.length; i++) {
                option += '<option value="' + data[i].issue + '" data-callissue="' + data[i].callDepartment + ',' + data[i].callIssue + '" data-isdowntime="' + data[i].isdowntime+'">' + data[i].issue + '</option>';
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




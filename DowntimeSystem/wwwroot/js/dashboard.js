﻿var myChart1;
var myChart2;
var myChart3;
var myChart4;

// #region Chart1 Top Five Error Code with station,Line,rootcause information
function gettopErrorcode_bycount(system, project, department,lastday, currentDay) {
    getDataWithArray("/Dashboard/GetTopErrorCode_ByCount", { comefrom: system, departmentlist: department, projectlist: project, currentDay: currentDay, lastday: lastday })
        .then(res => {
            var dataArray = {
                axis: [],
                data: []
            };
            for (var i = 0; i < res.length; i++) {
                dataArray.axis.push(res[i].item);
                var item = {
                    value: res[i].value,
                    groupId: res[i].item,
                };
                dataArray.data.push(item);
            }
            return dataArray;
        })
        .then(res => {
            var errorcode;
            var station;
            var line;
            var dataFloor = 0;
            var chartDom = document.getElementById('chart1');
            if (myChart1 != null && myChart1 != "" && myChart1 != undefined) {
                myChart1.dispose();//解决echarts dom已经加载的报错
            }
            myChart1 = echarts.init(chartDom);
            var optionArray = [];
            var option = {
                title: {
                    text: 'Top 5 Defect Code',
                    subtext: 'With Line/Station/Root Cause information',
                    left: 'center',
                    textStyle: {
                        fontSize:15,
                    }
                },
                color: ['#5470c6'],
                grid: {
                    left: '3%',
                    right: '4%',
                    bottom: '3%',
                    containLabel: true,
                },
                graphic: [
                    {
                        type:'image',
                        invisible: true,
                    }
                ],
                dataGroupId: '',
                animationDurationUpdate: 400,
                xAxis: [
                    {
                        //name: 'ErrorCode',
                        type: 'category',
                        data: res.axis,
                        axisLabel: {
                            interval: 0,//如果设置为 1，表示『隔一个标签显示一个标签』，如果值为 2，表示隔两个标签显示一个标签，以此类推。
                            overflow: 'break',
                            width: 110,
                        },
                        axisTick: {
                            alignWithLabel: true
                        },
                    }
                ],
                yAxis: [
                    {
                        type: 'value',
                        name:'Frequency',
                    }
                ],
                series: [
                    {
                        id: 'errorcode',
                        data: res.data,
                        type: 'bar',
                        barWidth: '30%',
                        label: {
                            show: true
                        },
                        showBackground: true,
                        backgroundStyle: {
                            color: 'rgba(180, 180, 180, 0.2)'
                        }
                    }
                ],
                universalTransition: {
                    enabled: true,
                    divideShape: 'clone'
                }
            };
            optionArray[0] = option;
            myChart1.on('click', function (event) {
                if (event.data) {
                    var url;
                    let titlename;
                     if (dataFloor == 0) {
                        url = "/Dashboard/GetLine_ByErrorCode";
                        errorcode = event.name
                         dataFloor = 1;
                         titlename = 'Line';
                    }
                    else if (dataFloor == 1) {
                         url = "/Dashboard/GetStation_ByLine"
                        line = event.name;
                         dataFloor = 2;
                         titlename = 'Station';
                     }
                     else if (dataFloor == 2) {
                         url = "/Dashboard/GetRootCause_ByStation"
                         station = event.name;
                         dataFloor = 3;
                         titlename = 'Root Cause';
                     }
                    else {
                        return;
                    }
                    getDataWithArray(url, { errorcode: errorcode, comefrom: system, departmentlist:department,projectlist: project, currentDay: currentDay, lastday: lastday,line: line, station: station })
                        .then(res => {
                            var dataArray = {
                                dataGroupId: event.data.groupId,
                                data: [],
                            };
                            for (var i = 0; i < res.length; i++) {
                                var item = {
                                    value: res[i].value,
                                    groupId: res[i].item,
                                };
                                dataArray.data.push(item);
                            }
                            return dataArray;
                        })
                        .then(res => {
                            var option1 = {
                                title: {
                                    text: 'Top 5 ' + titlename+' Information',
                                    left: 'center',
                                    textStyle: {
                                        fontSize: 15,
                                    }
                                },
                                xAxis: {
                                    data: res.data.map(function (item) {
                                        return item.groupId;
                                    }),
                                },
                                series: {
                                    type: 'bar',
                                    id: 'errorcode',
                                    dataGroupId: res.dataGroupId,
                                    data: res.data.map(function (item) {
                                        return item.value;
                                    }),
                                    universalTransition: {
                                        enabled: true,
                                        divideShape: 'clone'
                                    }
                                },
                                graphic: [
                                    {
                                        type: 'image',
                                        right: 40,
                                        top: 20,
                                        style: {
                                            image: '/img/arrowleft.png',
                                        }, 
                                        invisible: false,                                       
                                        onclick: function () {
                                            if (dataFloor) {
                                                dataFloor -= 1;
                                            }
                                            myChart1.setOption(optionArray[dataFloor]);
                                        }
                                    }
                                ]
                            };
                            optionArray[dataFloor] = option1;
                            myChart1.setOption(optionArray[dataFloor]);
                        })
                }
            });
            myChart1.setOption(optionArray[dataFloor]);
        })
}
// #endregion

// #region Chart2 open/close/ongoing Downtime 时间统计
function getOpenCloseCount(system, project, department, lastday, currentDay) {
    getDataWithArray("/Dashboard/OpenClose_ByCount", { comefrom: system, departmentlist: department, projectlist: project, currentDay: currentDay, lastday: lastday })
        .then(res => {
            var dataArray = {
                axis: [],
                data: []
            };
            for (var i = 0; i < res.length; i++) {
                dataArray.axis.push(res[i].item);
                var item = {
                    value: res[i].value,
                    groupId: res[i].item,
                    itemStyle: {
                        color: res[i].item.toLowerCase() == 'open' ? "#a90000" : res[i].item.toLowerCase() == 'closed' ? "#91cc75" :"#fd7e14",// "#DC582A",
                    },
                };
                dataArray.data.push(item);
            }
            return dataArray;
        })
        .then(res => {
            var chartDom = document.getElementById('chart2');
            if (myChart2 != null && myChart2 != "" && myChart2 != undefined) {
                myChart2.dispose();//解决echarts dom已经加载的报错
            }
            myChart2 = echarts.init(chartDom);
            var option = {
                title: {
                    text: 'Downtime Status',
                    left: 'center',
                    textStyle: {
                        fontSize: 15,
                    }
                },
                color: ['#5470c6'],
                grid: {
                    left: '3%',
                    right: '4%',
                    bottom: '3%',
                    containLabel: true,
                },
                dataGroupId: '',
                animationDurationUpdate: 400,
                xAxis: [
                    {
                        //name: 'ErrorCode',
                        type: 'category',
                        data: res.axis,
                        axisLabel: {
                            interval: 0,//如果设置为 1，表示『隔一个标签显示一个标签』，如果值为 2，表示隔两个标签显示一个标签，以此类推。
                            overflow: 'break',
                            width: 110,
                        },
                        axisTick: {
                            alignWithLabel: true
                        },
                    }
                ],
                yAxis: [
                    {
                        type: 'value',
                        name: 'Downtime(H)',
                    }
                ],
                series: [
                    {
                        id: 'errorcode',
                        data: res.data,
                        type: 'bar',
                        barWidth: '30%',
                        label: {
                            show: true
                        },
                        showBackground: true,
                        backgroundStyle: {
                            color: 'rgba(180, 180, 180, 0.2)'
                        }
                    }
                ]
            };
            myChart2.on('click', function (event) {
                if (event.data) {
                    //$("#detaillist").bootstrapTable('destroy').bootstrapTable({
                    //    cache: false,
                    //    type: 'GET',
                    //    url: '/Dashboard/OpenClose_Items',
                    //    queryParams: {
                    //        status: event.name,
                    //        projectlist: project,
                    //        departmentlist: department,
                    //        comefrom: system,
                    //        currentDay: currentDay,
                    //        lastday: lastday
                    //    },
                    //    ajaxOptions: {                      //传参ajax设置
                    //        traditional: true,              //允许传递数组类型的参数
                    //    },
                    //    dataType: 'json',
                    //    columns: [{
                    //        field: 'id',
                    //        title: 'Ticket No.',
                    //        align: 'center',
                    //        valign: 'middle',
                    //    }, {
                    //        field: 'department',
                    //        title: 'Department',
                    //        align: 'center',
                    //        valign: 'middle',
                    //    }, {
                    //        field: 'project',
                    //        title: 'Project',
                    //        align: 'center',
                    //        valign: 'middle',
                    //    }, {
                    //        field: 'line',
                    //        title: 'Line',
                    //        align: 'center',
                    //        valign: 'middle',
                    //    }, {
                    //        field: 'station',
                    //        title: 'Station Name',
                    //        align: 'center',
                    //        valign: 'middle',
                    //    }, {
                    //        field: 'machine',
                    //        title: 'Machine Name',
                    //        align: 'center',
                    //        valign: 'middle',
                    //    }, {
                    //        field: 'occurtime',
                    //        title: 'Occurt Time',
                    //        align: 'center',
                    //        valign: 'middle',
                    //        formatter: function (value, row, index) {
                    //            return new Date(value).format('yyyy-MM-dd hh:mm:ss');
                    //        },
                    //    }, {
                    //        field: 'issue',
                    //        title: 'Defect Code',
                    //        align: 'center',
                    //        valign: 'middle',
                    //    }, {
                    //        field: 'issueremark',
                    //        title: 'Issue Description',
                    //        align: 'center',
                    //        valign: 'middle',
                    //    }, {
                    //        field: 'finishtime',
                    //        title: 'Finish Time',
                    //        align: 'center',
                    //        valign: 'middle',
                    //        visible: function (value, row, index) {
                    //            if (row['incidentstatus'] == 2) return true;
                    //            else return false;
                    //        },
                    //        formatter: function (value, row, index) {
                    //            if (value)
                    //                return new Date(value).format('yyyy-MM-dd hh:mm:ss');
                    //            else
                    //                return value;
                    //        },
                    //    }, {
                    //        field: 'downday',
                    //        title: 'Downtime(H)',
                    //        align: 'center',
                    //        valign: 'middle',
                    //        visible: function (value, row, index) {
                    //            if (row['incidentstatus'] == 2) return true;
                    //            else return false;
                    //        },
                    //        formatter: function (value, row, index) {
                    //            if (row['incidentstatus'] == 2) {
                    //                return parseInt(Math.abs(new Date(row['finishtime']) - new Date(row['occurtime'])) / 3600000);
                    //                //return parseFloat(Math.abs(new Date(row['finishtime']) - new Date(row['occurtime'])) / 3600000).toFixed(2);
                    //            }
                    //            return "--";
                    //        },
                    //    }, {
                    //        field: 'openday',
                    //        title: 'Open Hours(H)',
                    //        align: 'center',
                    //        valign: 'middle',
                    //        visible: function (value, row, index) {
                    //            if (row['incidentstatus'] == 2) return false;
                    //            else return true;
                    //        },
                    //        formatter: function (value, row, index) {
                    //            if (row['incidentstatus'] == 2) {
                    //                return 0;
                    //            } else {
                    //                return parseInt(Math.abs(Date.now() - new Date(row['occurtime'])) / 3600000);
                    //                //return parseFloat(Math.abs(Date.now() - new Date(row['occurtime'])) / 3600000).toFixed(2);
                    //            }
                    //        },
                    //    }
                    //    ]
                    //})
                    openCloseDowntime_StationPieChart(system, project, department, lastday, currentDay, event.name);
                    //if (event.name == "Closed") {
                    //    $("#modalName").html("Closed Downtime incident Detail Information")
                    //    $('#detaillist').bootstrapTable('hideColumn', 'openday');
                    //    $('#detaillist').bootstrapTable('showColumn', 'downday');
                    //    $('#detaillist').bootstrapTable('showColumn', 'finishtime');
                    //} else {
                    //    $("#modalName").html("Open Downtime incident Detail Information")
                    //    $('#detaillist').bootstrapTable('showColumn', 'openday');
                    //    $('#detaillist').bootstrapTable('hideColumn', 'downday');
                    //    $('#detaillist').bootstrapTable('hideColumn', 'finishtime');
                    //}
                    $("#Chart2Modal").modal('show');
                }
            });
            myChart2.setOption(option);
        })
}
function openCloseDowntime_StationPieChart(system, project, department, lastday, currentDay, status) {
    getDataWithArray("/Dashboard/OpenCloseDowntime_StationPieChart", { comefrom: system, departmentlist: department, projectlist: project, currentDay: currentDay, lastday: lastday, status: status})
        .then(res => {
            var dataArray = []
            for (var i = 0; i < res.length; i++) {
                var item = {
                    value: res[i].value,
                    name:res[i].key
                };
                dataArray.push(item);
            }
            return dataArray;
        })
        .then(res => {
            var chartDom = document.getElementById('chart21');
            var myChart21 = echarts.init(chartDom);
            var option = {
                title: {
                    text: 'Referer of a Website',
                    subtext: 'Fake Data',
                    left: 'center'
                },
                tooltip: {
                    trigger: 'item'
                },
                legend: {
                    orient: 'vertical',
                    left: 'left'
                },
                series: [
                    {
                        name: 'Access From',
                        type: 'pie',
                        radius: '50%',
                        data: [
                            { value: 1048, name: 'Search Engine' },
                            { value: 735, name: 'Direct' },
                            { value: 580, name: 'Email' },
                            { value: 484, name: 'Union Ads' },
                            { value: 300, name: 'Video Ads' }
                        ],
                        emphasis: {
                            itemStyle: {
                                shadowBlur: 10,
                                shadowOffsetX: 0,
                                shadowColor: 'rgba(0, 0, 0, 0.5)'
                            }
                        }
                    }
                ]
            };
            myChart21.setOption(option);
        })
}
// #endregion

// #region Chart2 open/close/ongoing Downtime 时间统计 _backup 层叠图
function getOpenCloseCount_back(system, project, department, lastday, currentDay) {
    getDataWithArray("/Dashboard/OpenClose_ByCount", { comefrom: system, departmentlist: department, projectlist: project, currentDay: currentDay, lastday: lastday })
        .then(res => {
            var department = [];
            var axis = [];
            var dataArray = [];
            for (var i = 0; i < res.length; i++) {
                if (axis.length <= 0 || axis.indexOf(res[i].item) < 0)
                    axis.push(res[i].item);
                if (department.indexOf(res[i].department) < 0) {
                    var item = {
                        name: res[i].department,
                        stack: 'total',
                        type: 'bar',
                        emphasis: {
                            focus: 'series'
                        },
                        data: [],
                        barWidth: '30%',
                        label: {
                            show: true
                        },
                    };
                    dataArray.push(item);
                    department.push(res[i].department)
                }
                dataArray.map(function (o) {
                    if (o.name == res[i].department) {
                        var index = axis.indexOf(res[i].item);
                        o.data[index] = res[i].value;
                    }
                })
            }
            return { axis: axis, data: dataArray };
        })
        .then(res => {
            var chartDom = document.getElementById('chart2');
            if (myChart2 != null && myChart2 != "" && myChart2 != undefined) {
                myChart2.dispose();//解决echarts dom已经加载的报错
            }
            myChart2 = echarts.init(chartDom);
            var option = {
                title: {
                    text: 'Downtime Status',
                    left: 'center',
                    textStyle: {
                        fontSize: 15,
                    }
                },
                legend: { top: 20 },
                color: ['#01A1FF', '#60D937', '#8E8E8E', '#F8BA00', '#FF2500'],//['#5470c6',],
                grid: {
                    left: '3%',
                    right: '4%',
                    bottom: '3%',
                    containLabel: true,
                },
                dataGroupId: '',
                animationDurationUpdate: 400,
                xAxis: [
                    {
                        //name: 'ErrorCode',
                        type: 'category',
                        data: res.axis,
                        axisLabel: {
                            interval: 0,//如果设置为 1，表示『隔一个标签显示一个标签』，如果值为 2，表示隔两个标签显示一个标签，以此类推。
                            overflow: 'break',
                            width: 110,
                        },
                        axisTick: {
                            alignWithLabel: true
                        },
                    }
                ],
                yAxis: [
                    {
                        type: 'value',
                        name: 'Downtime(H)',
                    }
                ],
                series: res.data,
            };
            myChart2.on('click', function (event) {
                if (event.data) {
                    $("#detaillist").bootstrapTable('destroy').bootstrapTable({
                        cache: false,
                        type: 'GET',
                        url: '/Dashboard/OpenClose_Items',
                        queryParams: {
                            status: event.name,
                            projectlist: project,
                            departmentlist: department,
                            comefrom: system,
                            currentDay: currentDay,
                            lastday: lastday
                        },
                        ajaxOptions: {                      //传参ajax设置
                            traditional: true,              //允许传递数组类型的参数
                        },
                        dataType: 'json',
                        columns: [{
                            field: 'id',
                            title: 'Ticket No.',
                            align: 'center',
                            valign: 'middle',
                        }, {
                            field: 'department',
                            title: 'Department',
                            align: 'center',
                            valign: 'middle',
                        }, {
                            field: 'project',
                            title: 'Project',
                            align: 'center',
                            valign: 'middle',
                        }, {
                            field: 'line',
                            title: 'Line',
                            align: 'center',
                            valign: 'middle',
                        }, {
                            field: 'station',
                            title: 'Station',
                            align: 'center',
                            valign: 'middle',
                        }, {
                            field: 'machine',
                            title: 'Machine',
                            align: 'center',
                            valign: 'middle',
                        }, {
                            field: 'occurtime',
                            title: 'Occurt Time',
                            align: 'center',
                            valign: 'middle',
                            formatter: function (value, row, index) {
                                return new Date(value).format('yyyy-MM-dd hh:mm:ss');
                            },
                        }, {
                            field: 'issue',
                            title: 'Issue',
                            align: 'center',
                            valign: 'middle',
                        }, {
                            field: 'issueremark',
                            title: 'Issue Description',
                            align: 'center',
                            valign: 'middle',
                        }, {
                            field: 'finishtime',
                            title: 'Finish Time',
                            align: 'center',
                            valign: 'middle',
                            visible: function (value, row, index) {
                                if (row['incidentstatus'] == 2) return true;
                                else return false;
                            },
                            formatter: function (value, row, index) {
                                if (value)
                                    return new Date(value).format('yyyy-MM-dd hh:mm:ss');
                                else
                                    return value;
                            },
                        }, {
                            field: 'downday',
                            title: 'Downtime(H)',
                            align: 'center',
                            valign: 'middle',
                            visible: function (value, row, index) {
                                if (row['incidentstatus'] == 2) return true;
                                else return false;
                            },
                            formatter: function (value, row, index) {
                                if (row['incidentstatus'] == 2) {
                                    return parseInt(Math.abs(new Date(row['finishtime']) - new Date(row['occurtime'])) / 3600000);
                                    //return parseFloat(Math.abs(new Date(row['finishtime']) - new Date(row['occurtime'])) / 3600000).toFixed(2);
                                }
                                return "--";
                            },
                        }, {
                            field: 'openday',
                            title: 'Open Hours(H)',
                            align: 'center',
                            valign: 'middle',
                            visible: function (value, row, index) {
                                if (row['incidentstatus'] == 2) return false;
                                else return true;
                            },
                            formatter: function (value, row, index) {
                                if (row['incidentstatus'] == 2) {
                                    return 0;
                                } else {
                                    return parseInt(Math.abs(Date.now() - new Date(row['occurtime'])) / 3600000);
                                    //return parseFloat(Math.abs(Date.now() - new Date(row['occurtime'])) / 3600000).toFixed(2);
                                }
                            },
                        }
                        ]
                    })
                    if (event.name == "Close") {
                        $("#modalName").html("Closed Downtime incident Detail Information")
                        $('#detaillist').bootstrapTable('hideColumn', 'openday');
                        $('#detaillist').bootstrapTable('showColumn', 'downday');
                        $('#detaillist').bootstrapTable('showColumn', 'finishtime');
                    } else {
                        $("#modalName").html("Open Downtime incident Detail Information")
                        $('#detaillist').bootstrapTable('showColumn', 'openday');
                        $('#detaillist').bootstrapTable('hideColumn', 'downday');
                        $('#detaillist').bootstrapTable('hideColumn', 'finishtime');
                    }
                    $("#Chart2Modal").modal('show');
                }
            });
            myChart2.setOption(option);
        })
}
// #endregion

// #region Chart3 Downtime时间最多的前五个站
function gettopErrorCode_byDowntime(system, project, department, lastday, currentDay) {
    getDataWithArray("/Dashboard/GetTopDowntime_ByStation", { comefrom: system, departmentlist:department,projectlist: project,currentDay: currentDay, lastday: lastday })
        .then(res => {
            var dataArray = {
                axis: [],
                data: []
            };
            for (var i = 0; i < res.length; i++) {
                dataArray.axis.push(res[i].item);
                dataArray.data.push(res[i].value);
            }
            return dataArray;
        })
        .then(res => {
            if (res) {
                var dataFloor = 0;
                var chartDom = document.getElementById('chart3');
                if (myChart3 != null && myChart3 != "" && myChart3 != undefined) {
                    myChart3.dispose();//解决echarts dom已经加载的报错
                }
                myChart3 = echarts.init(chartDom);
                var optionArray = [];
                var option = {
                    title: {
                        text: 'Top 5 Stations Downtime Distribution',
                        left: 'center',
                        textStyle: {
                            fontSize: 15,
                        }
                    },
                    color: ['#5470c6'],
                    grid: {
                        left: '3%',
                        right: '4%',
                        bottom: '3%',
                        containLabel: true,
                    },
                    dataGroupId: '',
                    animationDurationUpdate: 400,
                    xAxis: [
                        {
                            //name: 'RootCause',
                            type: 'category',
                            data: res.axis,
                            axisLabel: {
                                interval: 0,//如果设置为 1，表示『隔一个标签显示一个标签』，如果值为 2，表示隔两个标签显示一个标签，以此类推。
                                width: 110,
                                overflow: 'break',
                            },
                            axisTick: {
                                alignWithLabel: true
                            },
                        }
                    ],
                    yAxis: [
                        {
                            type: 'value',
                            name: 'Downtime(H)',
                        }
                    ],
                    series: [
                        {
                            id: 'department',
                            data: res.data,
                            type: 'bar',
                            barWidth: '30%',
                            label: {
                                show: true
                            },
                            showBackground: true,
                            backgroundStyle: {
                                color: 'rgba(180, 180, 180, 0.2)'
                            }
                        }
                    ],
                };
                optionArray[0] = option;
                myChart3.setOption(optionArray[dataFloor]);
            }
            else {
                console.log("无数据");
            }
        })
}
// #endregion

// #region Chart4 各部门的downtime时间 层叠图
function getDowntime_byDepartment(system, project, department, lastday, currentDay) {
    getDataWithArray("/Dashboard/GetTopDowntime_ByDepartment", { comefrom: system, departmentlist: department, projectlist: project, currentDay: currentDay, lastday: lastday })
        .then(res => {
            var status = [];
            var color = [];
            var axis = [];
            var dataArray = [];
            for (var i = 0; i < res.length; i++) {
                var statusCode = res[i].status == 2 ? "Closed" : res[i].status == 1 ? "On-going" : "Open";
                if (axis.length <= 0 || axis.indexOf(res[i].item) < 0) 
                    axis.push(res[i].item);
                if (status.indexOf(statusCode) < 0) {
                    if (statusCode === "Closed") color.push('#91cc75');
                    else if (statusCode === "On-going") color.push('#fd7e14');//('#DC582A');
                    else if (statusCode === "Open") color.push('#a90000');
                    var item = {
                        name: statusCode,
                        stack: 'total',
                        type: 'bar',
                        emphasis: {
                            focus: 'series'
                        },
                        data: [],
                        barWidth: '30%',
                        label: {
                            show: true
                        },
                    };
                    dataArray.push(item);
                    status.push(statusCode)
                }
                dataArray.map(function (o) {
                    if (o.name == statusCode) {
                        var index = axis.indexOf(res[i].item);
                        o.data[index] = res[i].value;
                    }
                })
            }
            return { axis: axis, data: dataArray,color:color };
        })
        .then(res => {
            if (res) {
                var dataFloor = 0;
                var chartDom = document.getElementById('chart4');
                if (myChart4 != null && myChart4 != "" && myChart4 != undefined) {
                    myChart4.dispose();//解决echarts dom已经加载的报错
                }
                myChart4 = echarts.init(chartDom);
                var optionArray = [];
                var option = {
                    title: {
                        text: 'Downtime By Function Team Contribution',
                        left: 'center',
                        textStyle: {
                            fontSize: 15,
                        }
                    },
                    color: ['#5470c6'],
                    grid: {
                        left: '3%',
                        right: '4%',
                        bottom: '3%',
                        containLabel: true,
                    },
                    dataGroupId: '',
                    animationDurationUpdate: 400,
                    legend: { top: 20 },
                    color: res.color, 
                    xAxis: [
                        {
                            //name: 'RootCause',
                            type: 'category',
                            data: res.axis,
                            axisLabel: {
                                interval: 0,//如果设置为 1，表示『隔一个标签显示一个标签』，如果值为 2，表示隔两个标签显示一个标签，以此类推。
                                width: 60,
                                overflow: 'break',
                            },
                            axisTick: {
                                alignWithLabel: true
                            },
                        }
                    ],
                    yAxis: [
                        {
                            type: 'value',
                            name: 'Downtime(H)',
                        }
                    ],
                    series:  res.data,
                };
                optionArray[0] = option;
                myChart4.setOption(optionArray[dataFloor]);
            }
            else {
                console.log("无数据");
            }
        })
}
// #endregion

// #region Chart4 各部门的downtime时间 _backup
function getDowntime_byDepartment_backup(system, project, department, lastday, currentDay) {
    getDataWithArray("/Dashboard/GetTopDowntime_ByDepartment", { comefrom: system, departmentlist:department,projectlist: project, currentDay: currentDay, lastday: lastday })
        .then(res => {
            var dataArray = {
                axis: [],
                data: []
            };
            for (var i = 0; i < res.length; i++) {
                dataArray.axis.push(res[i].item);
                dataArray.data.push(res[i].value);
            }
            return dataArray;
        })
        .then(res => {
            if (res) {
                var dataFloor = 0;
                var chartDom = document.getElementById('chart4');
                if (myChart4 != null && myChart4 != "" && myChart4 != undefined) {
                    myChart4.dispose();//解决echarts dom已经加载的报错
                }
                myChart4 = echarts.init(chartDom);
                var optionArray = [];
                var option = {
                    title: {
                        text: 'Downtime By Function Team Contribution',
                        left: 'center',
                        textStyle: {
                            fontSize: 15,
                        }
                    },
                    color: ['#5470c6'],
                    grid: {
                        left: '3%',
                        right: '4%',
                        bottom: '3%',
                        containLabel: true,
                    },
                    dataGroupId: '',
                    animationDurationUpdate: 400,
                    xAxis: [
                        {
                            //name: 'RootCause',
                            type: 'category',
                            data: res.axis,
                            axisLabel: {
                                interval: 0,//如果设置为 1，表示『隔一个标签显示一个标签』，如果值为 2，表示隔两个标签显示一个标签，以此类推。
                                width: 60,
                                overflow: 'break',
                            },
                            axisTick: {
                                alignWithLabel: true
                            },
                        }
                    ],
                    yAxis: [
                        {
                            type: 'value',
                            name: 'Downtime(H)',
                        }
                    ],
                    series: [
                        {
                            id: 'department',
                            data: res.data,
                            type: 'bar',
                            barWidth: '30%',
                            label: {
                                show: true
                            },
                            showBackground: true,
                            backgroundStyle: {
                                color: 'rgba(180, 180, 180, 0.2)'
                            }
                        }
                    ],
                };
                optionArray[0] = option;
                myChart4.setOption(optionArray[dataFloor]);
            }
            else {
                console.log("无数据");
            }
        })
}
// #endregion

//#region 窗口大小调整，echart图的大小随之改变
window.onresize = function () {
    if (myChart1)  myChart1.resize();
    if (myChart2)  myChart2.resize();
    if (myChart3)  myChart3.resize();
    if (myChart4)  myChart4.resize();
}
$("#sideToggle").on('change', function () {
    setTimeout(function () {
        myChart1.resize();
        myChart2.resize();
        myChart3.resize();
        myChart4.resize();
    },350);
})
//#endregion



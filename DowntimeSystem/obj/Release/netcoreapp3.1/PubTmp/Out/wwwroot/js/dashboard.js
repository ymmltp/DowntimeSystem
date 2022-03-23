var myChart1;
var myChart2;
var myChart3;
var myChart4;

//Chart1
function gettopErrorcode_bycount(project, lastday, currentDay) {
    getDataWithArray("/Dashboard/GetTopErrorCode_ByCount", { project: project, currentDay: currentDay, lastday: lastday })
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
                    text: 'Top 5 Error Code with Line/Station/Root Cause information',
                    //subtext: '',
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
                     if (dataFloor == 0) {
                        url = "/Dashboard/GetLine_ByErrorCode";
                        errorcode = event.name
                        dataFloor = 1;
                    }
                    else if (dataFloor == 1) {
                         url = "/Dashboard/GetStation_ByLine"
                        line = event.name;
                        dataFloor = 2;
                     }
                     else if (dataFloor == 2) {
                         url = "/Dashboard/GetRootCause_ByStation"
                         station = event.name;
                         dataFloor = 3;
                     }
                    else {
                        return;
                    }
                    getDataWithArray(url, { errorcode: errorcode, project: project, currentDay: currentDay, lastday: lastday,line: line, station: station })
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

//Chart2
function gettopRootCause_byDowntime() {
    getData("/Dashboard/GetTopRootCause_ByDowntime")
        .then(res => {
            var dataArray = {
                axis: [],
                data: []
            };
            for (var i = 0; i < res.length; i++) {
                if (res[i].value > 0) {
                    dataArray.axis.push(res[i].item);
                    var item = {
                        value: res[i].value,
                        groupId: res[i].item,
                    };
                    dataArray.data.push(item);
                }
            }
            return dataArray;
        })
        .then(res => {
            if (res) {
                var rootcause;
                var station;
                var dataFloor = 0;
                var chartDom = document.getElementById('chart2');
                var myChart = echarts.init(chartDom);
                var optionArray = [];
                var option = {
                    title: {
                        text: 'Top Five Downtime with RootCause',
                        subtext: 'With Error Code',
                        left: 'center',
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
                            type: 'image',
                            invisible: true,
                        }
                    ],
                    dataGroupId: '',
                    animationDurationUpdate: 400,
                    xAxis: [
                        {
                            //name: 'RootCause',
                            type: 'category',
                            data: res.axis,
                            axisLabel: {
                                interval: 0,//如果设置为 1，表示『隔一个标签显示一个标签』，如果值为 2，表示隔两个标签显示一个标签，以此类推。
                                width:120,
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
                            name: 'Downtime(s)',
                        }
                    ],
                    series: [
                        {
                            id: 'rootcause',
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
                myChart.on('click', function (event) {
                    if (event.data) {
                        var url;
                        if (dataFloor == 0) {
                            url = "/Dashboard/GetTopRootCause_ByDowntime_ErrorCode";
                            rootcause = event.name
                            dataFloor = 1;
                        }
                        else {
                            return;
                        }
                        getData(url, { rootcause: rootcause, station: station })
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
                                    xAxis: {
                                        data: res.data.map(function (item) {
                                            return item.groupId;
                                        }),
                                    },
                                    series: {
                                        type: 'bar',
                                        id: 'rootcause',
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
                                                image: '/img/arrowleft1.png',
                                            },
                                            invisible: false,
                                            onclick: function () {
                                                if (dataFloor) {
                                                    dataFloor -= 1;
                                                }
                                                myChart.setOption(optionArray[dataFloor]);
                                            }
                                        }
                                    ]
                                };
                                if (dataFloor == 1) {
                                    optionArray[1] = option1;
                                }
                                myChart.setOption(optionArray[dataFloor]);
                            })
                    }
                });
                myChart.setOption(optionArray[dataFloor]);
            }
            else {
                console.log("无数据");
            }
        })
}
function getAllErrorCode_TopFiveRootCause() {
    getData("/Dashboard/GetAllErrorCode_ByCount")
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
            var dataFloor = 0;
            var chartDom = document.getElementById('chart2');
            var myChart = echarts.init(chartDom);
            var optionArray = [];
            var option = {
                title: {
                    text: 'Top Five Error Code',
                    subtext: 'With RootCause',
                    left: 'center',
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
                        type: 'image',
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
                        name: 'Frequency',
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
            myChart.on('click', function (event) {
                if (event.data) {
                    var url;
                    if (dataFloor == 0) {
                        url = "/Dashboard/GetTopRootCause_ByErrorCode";
                        errorcode = event.name
                        dataFloor = 1;
                    }
                    else {
                        return;
                    }
                    getData(url, { errorcode: errorcode })
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
                                            image: '/img/arrowleft1.png',
                                        },
                                        invisible: false,
                                        onclick: function () {
                                            if (dataFloor) {
                                                dataFloor -= 1;
                                            }
                                            myChart.setOption(optionArray[dataFloor]);
                                        }
                                    }
                                ]
                            };
                            if (dataFloor == 1) {
                                optionArray[1] = option1;
                            }
                            else if (dataFloor == 2) {
                                optionArray[2] = option1;
                            }
                            myChart.setOption(optionArray[dataFloor]);
                        })
                }
            });
            myChart.setOption(optionArray[dataFloor]);
        })
}
function getOpenCloseCount(project, lastday, currentDay) {
    getDataWithArray("/Dashboard/OpenClose_ByCount", { project: project,currentDay: currentDay, lastday: lastday})
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
                        color: res[i].item.toLowerCase() == 'open' ? "#a90000" : "#91cc75",
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
                    text: 'Downtime Open & Close QTY and Detail information',
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
                        name: 'Downtime(h)',
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
                    $("#detaillist").bootstrapTable('destroy').bootstrapTable({
                        cache: false,
                        type: 'GET',
                        url: '/Dashboard/OpenClose_Items',
                        queryParams: {
                            status: event.name,
                            project: project,
                            currentDay: currentDay,
                            lastday: lastday
                        },
                        ajaxOptions: {                      //传参ajax设置
                            traditional: true,              //允许传递数组类型的参数
                        },
                        dataType: 'json',
                        columns: [ {
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
                        },{
                            field: 'occurtime',
                            title: 'Occurt Time',
                            align: 'center',
                            valign: 'middle',
                            formatter: function (value, row, index) {
                                return value.replace('T', ' ');
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
                        },{
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
                                    return value.replace('T', ' ');
                                else
                                    return value;
                            },
                        }, {
                            field: 'downday',
                            title: 'Downtime(Days)',
                            align: 'center',
                            valign: 'middle',
                            visible: function (value, row, index) {
                                if (row['incidentstatus'] == 2) return true;
                                else return false;
                            },
                            formatter: function (value, row, index) {
                                if (row['incidentstatus'] == 2) {
                                    return parseInt(Math.abs(new Date(row['finishtime'].replace('T', ' ')) - new Date(row['occurtime'].replace('T', ' '))) / 1000 / 60 / 60 / 24);
                                }
                                return "--";
                            },
                        }, {
                            field: 'openday',
                            title: 'Open Days(Days)',
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
                                    return parseInt(Math.abs(Date.now() - new Date(row['occurtime'].replace('T', ' '))) / 1000 / 60 / 60 / 24);
                                }
                            },
                        }
                        ]
                    })
                    if (event.name == "Open") {
                        $("#modalName").html("Open Downtime incident Detail Information")
                        $('#detaillist').bootstrapTable('showColumn', 'openday');
                        $('#detaillist').bootstrapTable('hideColumn', 'downday');
                        $('#detaillist').bootstrapTable('hideColumn', 'finishtime');
                    } else {
                        $("#modalName").html("Closed Downtime incident Detail Information")
                        $('#detaillist').bootstrapTable('hideColumn', 'openday');
                        $('#detaillist').bootstrapTable('showColumn', 'downday');
                        $('#detaillist').bootstrapTable('showColumn', 'finishtime');
                    }
                    $("#ListModal").modal('show');
                }
            });
            myChart2.setOption(option);
        })
}

//Chart3
function gettopErrorCode_byDowntime(project, lastday, currentDay) {
    getDataWithArray("/Dashboard/GetTopDowntime_ByStation", { project: project,currentDay: currentDay, lastday: lastday })
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
                        text: 'Top Five Station with Downtime',
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
                            name: 'Downtime(h)',
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

//Chart4
function getDowntime_byDepartment(project, lastday, currentDay) {
    getDataWithArray("/Dashboard/GetTopDowntime_ByDepartment", { project: project, currentDay: currentDay, lastday: lastday })
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
                        text: 'Total Downtime per Functional Team',
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
                            name: 'Downtime(h)',
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


//监听Dom元素的大小变化
//let mainchart = document.querySelector(".silder-right-body");
//let observer = new MutationObserver(function (mutations, observer) {
//    mutations.forEach(function (mutation) {
//        console.log(mutation);
//    });

//    //console.log("发生了");
//    //myChart1.resize();
//    //myChart2.resize();
//    //myChart3.resize();
//    //myChart4.resize();
//});
//observer.observe(mainchart, { attributes: true, attributeFilter: ['style'], attributeOldValue: true });

//监听窗口的大小变化
window.onresize = function () {
    myChart1.resize();
    myChart2.resize();
    myChart3.resize();
    myChart4.resize();
}

$("#sideToggle").on('change', function () {
    setTimeout(function () {
        myChart1.resize();
        myChart2.resize();
        myChart3.resize();
        myChart4.resize();
    },350);
})



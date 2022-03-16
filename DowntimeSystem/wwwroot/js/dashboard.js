function gettopErrorcode_bycount() {
    getData("/Dashboard/GetTopErrorCode_ByCount")
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
            var dataFloor = 0;
            var chartDom = document.getElementById('CountTopCode');
            var myChart = echarts.init(chartDom);
            var optionArray = [];
            var option = {
                title: {
                    text: 'Top Five Error Code',
                    subtext: 'With Station/Line information',
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
            myChart.on('click', function (event) {
                if (event.data) {
                    var url;
                    if (dataFloor == 1) {
                        url = "/Dashboard/GetTopErrorCode_ByCount_line"
                        station = event.name;
                        dataFloor = 2;
                    }
                    else if (dataFloor == 0) {
                        url = "/Dashboard/GetTopErrorCode_ByCount_station";
                        errorcode = event.name
                        dataFloor = 1;
                    }
                    else {
                        return;
                    }
                    getData(url, { errorcode: errorcode, station: station })
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
                var chartDom = document.getElementById('CountTopRootCause');
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
            var chartDom = document.getElementById('CountTopRootCause');
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

function gettopErrorCode_byDowntime() {
    getData("/Dashboard/GetTopDowntime_ByStation")
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
                var chartDom = document.getElementById('DowntimeOfStation');
                var myChart = echarts.init(chartDom);
                var optionArray = [];
                var option = {
                    title: {
                        text: 'Top Five Downtime with Station',
                        left: 'center',
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
                            name: 'Downtime(s)',
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
                myChart.setOption(optionArray[dataFloor]);
            }
            else {
                console.log("无数据");
            }
        })
}

function getDowntime_byDepartment() {
    getData("/Dashboard/GetTopDowntime_ByDepartment")
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
                var chartDom = document.getElementById('DowntimePerDepartment');
                var myChart = echarts.init(chartDom);
                var optionArray = [];
                var option = {
                    title: {
                        text: 'Total Downtime per Functional Team',
                        left: 'center',
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
                            name: 'Downtime(s)',
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
                myChart.setOption(optionArray[dataFloor]);
            }
            else {
                console.log("无数据");
            }
        })
}
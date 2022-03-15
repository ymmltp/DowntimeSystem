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
                    value: res[i].count,
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
                    text: 'Top Five Error Code ',
                    subtext: 'Order By Number of Occurrences',
                    left: 'center',
                },
                color: ['#5470c6'],
                grid: {
                    left: '3%',
                    right: '4%',
                    bottom: '3%',
                    containLabel: true
                },
                dataGroupId: '',
                animationDurationUpdate: 400,
                xAxis: [
                    {
                        type: 'category',
                        data: res.axis,
                        axisLabel: {
                            interval: 0,//如果设置为 1，表示『隔一个标签显示一个标签』，如果值为 2，表示隔两个标签显示一个标签，以此类推。
                            overflow: 'break',
                        },
                        axisTick: {
                            alignWithLabel: true
                        },
                    }
                ],
                yAxis: [
                    {
                        type: 'value'
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
                                    value: res[i].count,
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
                                    })
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
                                        type: 'text',
                                        left: 40,
                                        top: 20,
                                        style: {
                                            text: 'Back',
                                            fontSize: 18
                                        },
                                        //type: 'image',
                                        //left: 40,
                                        //top: 20,
                                        //style: {
                                        //    image: '/img/arrowleft.png',
                                        //},
                                        onclick: function () {
                                            console.log("dianjil");
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
}

function gettopErrorCode_byDowntime() {
}
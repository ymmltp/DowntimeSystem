var myChart1;
var myChart2;
var myChart3;
var myChart4;
var myChart5;
var myChart6;

function getTotalMTTR(system, project, department, lastday, currentDay) {
    getDataWithArray("/Dashboard/TotalMTTR", { comefrom: system, departmentlist: department, projectlist: project, currentDay: currentDay, lastday: lastday })
        .then(res => {
            var dataFloor = 0;
            var chartDom = document.getElementById('chart1');
            if (myChart1 != null && myChart1 != "" && myChart1 != undefined) {
                myChart1.dispose();
            }
            myChart1 = echarts.init(chartDom);
            var optionArray = [];
            var option = {
                title: {
                    text: 'MTTR-Mean Time To Repair(mins)',
                    left: 'center',
                    textStyle: {
                        fontSize: 15,
                    }
                },
                series: [
                    {
                        min: 0,
                        max: 200,
                        splitNumber: 4,
                        type: 'gauge',
                        axisLine: {
                            lineStyle: {
                                width: 20,
                                color: [
                                    [0.8, '#91cc75'],
                                    [1, '#fd666d']
                                ]
                            }
                        },
                        pointer: {
                            itemStyle: {
                                color: 'auto'
                            }
                        },
                        axisTick: {
                            distance: -30,
                            length: 20,
                            lineStyle: {
                                color: '#fff',
                                width: 2
                            }
                        },
                        splitLine: {
                            distance: -30,
                            length: 30,
                            lineStyle: {
                                color: '#fff',
                                width: 3
                            }
                        },
                        axisLabel: {
                            color: 'auto',
                            distance: 30,
                            fontSize: 15
                        },
                        detail: {
                            valueAnimation: true,
                            formatter: '{value}',
                            color: 'auto',
                            fontSize: 15
                        },
                        data: [
                            {
                                value: res[0].totalMTTR
                            }
                        ]
                    }
                ]
            };
            optionArray[0] = option;
            myChart1.setOption(optionArray[dataFloor]);
        })
}
function getMTTRByDepartment(system, project, department, lastday, currentDay) {
    getDataWithArray("/Dashboard/MTTRByDepartment", { comefrom: system, departmentlist: department, projectlist: project, currentDay: currentDay, lastday: lastday })
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
            var dataFloor = 0;
            var chartDom = document.getElementById('chart2');
            if (myChart2 != null && myChart2 != "" && myChart2 != undefined) {
                myChart2.dispose();
            }
            myChart2 = echarts.init(chartDom);
            var optionArray = [];
            var option = {
                title: {
                    text: 'MTTR By Department(mins)',
                    left: 'center',
                    textStyle: {
                        fontSize: 15,
                    }
                },
                tooltip: {
                    trigger: 'axis',
                    axisPointer: {
                        type: 'shadow'
                    }
                },
                grid: {
                    left: '3%',
                    right: '4%',
                    bottom: '3%',
                    containLabel: true
                },
                xAxis: {
                    type: 'value',
                    name: 'mins',
                },
                yAxis: {
                    type: 'category',
                    data: res.axis,
                    name: 'Department',
                },
                series: [
                    {
                        name: 'MTTR',
                        type: 'bar',
                        label: {
                            show: true,
                            position: 'right'
                        },
                        data: res.data
                    },
                ],
            };
            optionArray[0] = option;
            myChart2.setOption(optionArray[dataFloor]);
        })
}
function getMTTRByProject(system, project, department, lastday, currentDay) {
    getDataWithArray("/Dashboard/MTTRByWorkcell", { comefrom: system, departmentlist: department, projectlist: project, currentDay: currentDay, lastday: lastday })
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
            var dataFloor = 0;
            var chartDom = document.getElementById('chart3');
            if (myChart3 != null && myChart3 != "" && myChart3 != undefined) {
                myChart3.dispose();
            }
            myChart3 = echarts.init(chartDom);
            var optionArray = [];
            var option = {
                title: {
                    text: 'MTTR By Project(mins)',
                    left: 'center',
                    textStyle: {
                        fontSize: 15,
                    }
                },
                tooltip: {
                    trigger: 'axis',
                    axisPointer: {
                        type: 'shadow'
                    }
                },
                grid: {
                    left: '3%',
                    right: '4%',
                    bottom: '3%',
                    containLabel: true
                },
                xAxis: {
                    type: 'value',
                    boundaryGap: [0, 0.01]
                },
                yAxis: {
                    type: 'category',
                    data: res.axis,
                    name: 'Project',
                },
                series: [
                    {
                        name: 'MTTR',
                        type: 'bar',
                        label: {
                            show: true,
                            position: 'right'
                        },
                        data: res.data
                    },
                ],
            };
            optionArray[0] = option;
            myChart3.setOption(optionArray[dataFloor]);
        })
}


function getTotalMTBF(system, project, department, lastday, currentDay) {
    getDataWithArray("/Dashboard/TotalMTBF", { comefrom: system, departmentlist: department, projectlist: project, currentDay: currentDay, lastday: lastday })
        .then(res => {
            var dataFloor = 0;
            var chartDom = document.getElementById('chart4');
            if (myChart4 != null && myChart4 != "" && myChart4 != undefined) {
                myChart4.dispose();
            }
            myChart4 = echarts.init(chartDom);
            var optionArray = [];
            var option = {
                title: {
                    text: 'MTBF-Mean Time Between Failure(hours)',
                    left: 'center',
                    textStyle: {
                        fontSize: 15,
                    }
                },
                series: [
                    {
                        min: 0,
                        max: 10000,
                        splitNumber: 4,
                        type: 'gauge',
                        axisLine: {
                            lineStyle: {
                                width: 20,
                                color: [
                                    [0.2, '#fd666d'],
                                    [1, '#91cc75'],
                                ]
                            }
                        },
                        pointer: {
                            itemStyle: {
                                color: 'auto'
                            }
                        },
                        axisTick: {
                            distance: -30,
                            length: 20,
                            lineStyle: {
                                color: '#fff',
                                width: 2
                            }
                        },
                        splitLine: {
                            distance: -30,
                            length: 30,
                            lineStyle: {
                                color: '#fff',
                                width: 3
                            }
                        },
                        axisLabel: {
                            color: 'auto',
                            distance: 30,
                            fontSize: 15
                        },
                        detail: {
                            valueAnimation: true,
                            formatter: '{value}',
                            color: 'auto',
                            fontSize: 15
                        },
                        data: [
                            {
                                value: res[0].totalMTBF
                            }
                        ]
                    }
                ]
            };
            optionArray[0] = option;
            myChart4.setOption(optionArray[dataFloor]);
        })
}
function getMTBFByDepartment(system, project, department, lastday, currentDay) {
    getDataWithArray("/Dashboard/MTBFByDepartment", { comefrom: system, departmentlist: department, projectlist: project, currentDay: currentDay, lastday: lastday })
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
            var dataFloor = 0;
            var chartDom = document.getElementById('chart5');
            if (myChart5 != null && myChart5 != "" && myChart5 != undefined) {
                myChart5.dispose();
            }
            myChart5 = echarts.init(chartDom);
            var optionArray = [];
            var option = {
                title: {
                    text: 'MTBF By Department(hours)',
                    left: 'center',
                    textStyle: {
                        fontSize: 15,
                    }
                },
                tooltip: {
                    trigger: 'axis',
                    axisPointer: {
                        type: 'shadow'
                    }
                },
                grid: {
                    left: '3%',
                    right: '4%',
                    bottom: '3%',
                    containLabel: true
                },
                xAxis: {
                    type: 'value',
                    name: 'hours',
                    nameLocation : 'end'
                },
                yAxis: {
                    type: 'category',
                    data: res.axis,
                    name: 'Department',
                },
                series: [
                    {
                        name: 'MTBF',
                        type: 'bar',
                        label: {
                            show: true,
                            position: 'right'
                        },
                        data: res.data
                    },
                ],
            };
            optionArray[0] = option;
            myChart5.setOption(optionArray[dataFloor]);
        })
}
function getMTBFByProject(system, project, department, lastday, currentDay) {
    getDataWithArray("/Dashboard/MTBFByWorkcell", { comefrom: system, departmentlist: department, projectlist: project, currentDay: currentDay, lastday: lastday })
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
            var dataFloor = 0;
            var chartDom = document.getElementById('chart6');
            if (myChart6 != null && myChart6 != "" && myChart6 != undefined) {
                myChart6.dispose();
            }
            myChart6 = echarts.init(chartDom);
            var optionArray = [];
            var option = {
                title: {
                    text: 'MTBF By Project(hours)',
                    left: 'center',
                    textStyle: {
                        fontSize: 15,
                    }
                },
                tooltip: {
                    trigger: 'axis',
                    axisPointer: {
                        type: 'shadow'
                    }
                },
                grid: {
                    left: '3%',
                    right: '4%',
                    bottom: '3%',
                    containLabel: true
                },
                xAxis: {
                    type: 'value',
                    boundaryGap: [0, 0.01]
                },
                yAxis: {
                    type: 'category',
                    data: res.axis,
                    name: 'Project',
                },
                series: [
                    {
                        name: 'MTBF',
                        type: 'bar',
                        label: {
                            show: true,
                            position: 'right'
                        },
                        data: res.data
                    },
                ],
            };
            optionArray[0] = option;
            myChart6.setOption(optionArray[dataFloor]);
        })
}
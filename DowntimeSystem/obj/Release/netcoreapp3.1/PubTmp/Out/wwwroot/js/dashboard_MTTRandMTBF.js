var myChart1;
var myChart2;
var myChart3;

var myChart4;
var myChart5;
var myChart6;

var myChart7;
var myChart8;
var myChart9;

function getTotalMTTR(system, project, department,station, lastday, currentDay) {
    getDataWithArray("/Dashboard/TotalMTTR", { comefrom: system, departmentlist: department, projectlist: project, stationlist: station, currentDay: currentDay, lastday: lastday })
        .then(res => {
            if (res == []) {
                return new Promise.reject("本时间段没有MTTR数据")
            }
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
function getMTTRByDepartment(system, project, department, station, lastday, currentDay) {
    getDataWithArray("/Dashboard/MTTRByDepartment", { comefrom: system, departmentlist: department, projectlist: project, stationlist: station, currentDay: currentDay, lastday: lastday })
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
                    left: '5%',
                    right: '7%',
                    bottom: '3%',
                    containLabel: true
                },
                xAxis: {
                    type: 'value',
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
                            position: 'right',
                        },
                        tooltip: {
                            valueFormatter: function (value) {
                                return value + ' mins';
                            }
                        },
                        data: res.data
                    },
                ],
            };
            optionArray[0] = option;
            myChart2.setOption(optionArray[dataFloor]);
        })
}
function getMTTRByProject(system, project, department, station, lastday, currentDay) {
    getDataWithArray("/Dashboard/MTTRByWorkcell", { comefrom: system, departmentlist: department, projectlist: project, stationlist: station, currentDay: currentDay, lastday: lastday })
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
                    left: '5%',
                    right: '5%',
                    bottom: '3%',
                    containLabel: true
                },
                xAxis: {
                    type: 'category',
                    data: res.axis,
                    //name: 'Project',
                },
                yAxis: {
                    type: 'value',
                    boundaryGap: [0, 0.01]
                },
                series: [
                    {
                        name: 'MTTR',
                        type: 'bar',
                        label: {
                            show: true,
                            position: 'top'
                        },
                        tooltip: {
                            valueFormatter: function (value) {
                                return value + ' mins';
                            }
                        },
                        data: res.data
                    },
                ],
            };
            optionArray[0] = option;
            myChart3.setOption(optionArray[dataFloor]);
        })
}

function getTotalMTTA(system, project, department, station, lastday, currentDay) {
    getDataWithArray("/Dashboard/TotalMTTA", { comefrom: system, departmentlist: department, projectlist: project, stationlist: station, currentDay: currentDay, lastday: lastday })
        .then(res => {
            var dataFloor = 0;
            var chartDom = document.getElementById('chart7');
            if (myChart7 != null && myChart7 != "" && myChart7 != undefined) {
                myChart7.dispose();
            }
            myChart7 = echarts.init(chartDom);
            var optionArray = [];
            var option = {
                title: {
                    text: 'MTTA-Mean Time To Attendance(mins)',
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
            myChart7.setOption(optionArray[dataFloor]);
        })
}
function getMTTAByDepartment(system, project, department, station, lastday, currentDay) {
    getDataWithArray("/Dashboard/MTTAByDepartment", { comefrom: system, departmentlist: department, projectlist: project, stationlist: station, currentDay: currentDay, lastday: lastday })
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
            var chartDom = document.getElementById('chart8');
            if (myChart8 != null && myChart8 != "" && myChart8 != undefined) {
                myChart8.dispose();
            }
            myChart8 = echarts.init(chartDom);
            var optionArray = [];
            var option = {
                title: {
                    text: 'MTTA By Department(mins)',
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
                    left: '5%',
                    right: '7%',
                    bottom: '3%',
                    containLabel: true
                },
                xAxis: {
                    type: 'value',
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
                        tooltip: {
                            valueFormatter: function (value) {
                                return value + ' mins';
                            }
                        },
                        data: res.data
                    },
                ],
            };
            optionArray[0] = option;
            myChart8.setOption(optionArray[dataFloor]);
        })
}
function getMTTAByProject(system, project, department, station, lastday, currentDay) {
    getDataWithArray("/Dashboard/MTTAByWorkcell", { comefrom: system, departmentlist: department, projectlist: project, stationlist: station, currentDay: currentDay, lastday: lastday })
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
            var chartDom = document.getElementById('chart9');
            if (myChart9 != null && myChart9 != "" && myChart9 != undefined) {
                myChart9.dispose();
            }
            myChart9 = echarts.init(chartDom);
            var optionArray = [];
            var option = {
                title: {
                    text: 'MTTA By Project(mins)',
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
                    left: '5%',
                    right: '5%',
                    bottom: '3%',
                    containLabel: true
                },
                xAxis: {
                    type: 'category',
                    data: res.axis,
                    //name: 'Project',
                },
                yAxis: {
                    type: 'value',
                    boundaryGap: [0, 0.01]
                },
                series: [
                    {
                        name: 'MTTR',
                        type: 'bar',
                        label: {
                            show: true,
                            position: 'top'
                        },
                        tooltip: {
                            valueFormatter: function (value) {
                                return value + ' mins';
                            }
                        },
                        data: res.data
                    },
                ],
            };
            optionArray[0] = option;
            myChart9.setOption(optionArray[dataFloor]);
        })
}

function getTotalMTBF(system, project, department, station,lastday, currentDay) {
    getDataWithArray("/Dashboard/TotalMTBF", { comefrom: system, departmentlist: department, projectlist: project, stationlist: station, currentDay: currentDay, lastday: lastday })
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
                        max: 500,
                        splitNumber: 4,
                        type: 'gauge',
                        axisLine: {
                            lineStyle: {
                                width: 20,
                                color: [
                                    [0.1, '#fd666d'],
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
        .catch(err => {
            showWarning(err);
        })
}
function getMTBFByDepartment(system, project, department, station,lastday, currentDay) {
    getDataWithArray("/Dashboard/MTBFByDepartment", { comefrom: system, departmentlist: department, projectlist: project, stationlist: station, currentDay: currentDay, lastday: lastday })
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
                    left: '5%',
                    right: '7%',
                    bottom: '3%',
                    containLabel: true
                },
                xAxis: {
                    type: 'value',
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
                        tooltip: {
                            valueFormatter: function (value) {
                                return value + ' hours';
                            }
                        },
                        data: res.data
                    },
                ],
            };
            optionArray[0] = option;
            myChart5.setOption(optionArray[dataFloor]);
        })
        .catch(err => {
            showWarning(err);
        })
}
function getMTBFByProject(system, project, department, station,lastday, currentDay) {
    getDataWithArray("/Dashboard/MTBFByWorkcell", { comefrom: system, departmentlist: department, projectlist: project, stationlist: station, currentDay: currentDay, lastday: lastday })
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
                    left: '5%',
                    right: '5%',
                    bottom: '3%',
                    containLabel: true
                },
                xAxis: {
                    type: 'category',
                    data: res.axis,
                },
                yAxis: {
                    type: 'value',
                    boundaryGap: [0, 0.01]
                },
                series: [
                    {
                        name: 'MTBF',
                        type: 'bar',
                        label: {
                            show: true,
                            position: 'top'
                        },
                        tooltip: {
                            valueFormatter: function (value) {
                                return value + ' hours';
                            }
                        },
                        data: res.data
                    },
                ],
            };
            optionArray[0] = option;
            myChart6.setOption(optionArray[dataFloor]);
        })
        .catch(err => {
            showWarning(err);
        })
}

//#region 窗口大小调整，echart图的大小随之改变
window.onresize = function () {
    if (myChart1) myChart1.resize();
    if (myChart2) myChart2.resize();
    if (myChart3) myChart3.resize();
    if (myChart4) myChart4.resize();
    if (myChart5) myChart5.resize();
    if (myChart6) myChart6.resize();
    if (myChart7) myChart7.resize();
    if (myChart8) myChart8.resize();
    if (myChart9) myChart9.resize();
}
$("#sideToggle").on('change', function () {
    setTimeout(function () {
        myChart1.resize();
        myChart2.resize();
        myChart3.resize();
        myChart4.resize();
        myChart5.resize();
        myChart6.resize();
        myChart7.resize();
        myChart8.resize();
        myChart9.resize();
    }, 350);
})
//#endregion
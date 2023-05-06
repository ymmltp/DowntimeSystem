var myChart1;
var myChart2;
var myChart3;
var myChart4;
var myChart5;
var myChart6;
var myChart7;
var myChart8;


// #region Chart1 Downtime Distribution by Project,department
function getDTDistribution_GroupByProandDec(system, project, department ,station,lastday, currentDay) {
    getDataWithArray("/Dashboard/GetDTDistribution_ByDepartment_Project", { comefrom: system, departmentlist: department, projectlist: project, stationlist: station, currentDay: currentDay, lastday: lastday })
        .then(res => {
            if (res.length == 0) {
                return new Promise.reject("无数据")
            }
            var dataArray = {
                axis: [],
                data: []
            };
            for (var i = 0; i < res.length; i++) {
                if (dataArray.axis.length == 0 || dataArray.axis.find(e=>e.name==res[i].project) ==undefined) {
                    dataArray.axis.push({ name: res[i].project });
                }
            }
            for (var i = 0; i < res.length; i++) {
                if (dataArray.data.length==0 || dataArray.data.find(e=>e.name==res[i].department) == undefined) {
                    var item = {
                        value: new Array(dataArray.axis.length),
                        name: res[i].department,
                    };
                    dataArray.data.push(item);
                }
                dataArray.data.find(e => e.name == res[i].department).value[dataArray.axis.findIndex(e => e.name == res[i].project)] = res[i].dt;
            }
            return dataArray;
        })
        .then(res => {
            var dataFloor = 0;
            var chartDom = document.getElementById('chart1');
            if (myChart1 != null && myChart1 != "" && myChart1 != undefined) {
                myChart1.dispose();//解决echarts dom已经加载的报错
            }
            myChart1 = echarts.init(chartDom);
            var optionArray = [];
            var option = {
                title: {
                    text: 'DT Project Distribution',
                    textStyle: {
                        fontSize: 15,
                    },
                    left: 'center'
                },
                radar: {
                    center: ['50%', '60%'],
                    radius: '60%',
                    indicator: res.axis,
                },
                tooltip: {
                    trigger: 'item',
                    confine: true,
                    valueFormatter: function (value) {
                        return value + ' hours';
                    }
                },
                legend: {
                    top: '10%',
                    itemGap: 3,
                    textStyle: {
                        fontSize: 10,
                    }
                },
                series: [
                    {
                        name: 'DT Distribution',
                        type: 'radar',
                        data: res.data,
                    }
                ]
            };
            optionArray[0] = option;
            myChart1.setOption(optionArray[dataFloor]);
        })
        .catch(err => {
            showWarning(err);
        })
}
// #endregion

// #region Chart2 DT Status Trick
function getDTcount_GroupByStatus(system, project, department, station,lastday, currentDay) {
    getDataWithArray("/Dashboard/GetCountGroupByDowntimeStatus", { comefrom: system, departmentlist: department, projectlist: project, stationlist:station, currentDay: currentDay, lastday: lastday })
        .then(res => {
            if (res.length == 0) {
                return new Promise.reject("无数据")
            }
            var chartDom = document.getElementById('chart2');
            if (myChart2 != null && myChart2 != "" && myChart2 != undefined) {
                myChart2.dispose();//解决echarts dom已经加载的报错
            }
            myChart2 = echarts.init(chartDom);
            var option = {
                color: ['#5470c6', '#e66', '#fac858', '#91cc75'],
                title: {
                    text: 'DT Ticket Status',
                    left: 'center',
                    textStyle: {
                        fontSize: 15,
                    }
                },
                tooltip: {
                    trigger: 'item',
                    formatter: '{b}: <b>{c} piece</b> ({d}%)',
                    confine: true,
                },
                legend: {
                    orient: 'horizontal',
                    top: '10%',
                    itemGap: 3,
                    textStyle: {
                        fontSize: 10,
                    }
                },
                series: [
                    {
                        name: 'DT',
                        type: 'pie',
                        radius: '70%',
                        center: ['50%', '65%'],
                        data: res,
                        itemStyle: {
                            //borderRadius: 6,
                            borderColor: '#fff',
                            borderWidth: 2
                        },
                        label: {
                            show: true,
                            position: 'inner',
                            formatter: '{d}%',
                        },
                        emphasis: {
                            itemStyle: {
                                //shadowBlur: 10,
                                shadowOffsetX: 0,
                                shadowColor: 'rgba(0, 0, 0, 0.5)'
                            }
                        }
                    }
                ]
            };
            myChart2.setOption(option);
        })
        .catch(err => {
            console.log(err);
        })
}
// #endregion

// #region Chart3 个项目MTTR实际和目标对比
function getMTTR_ActualvsTarget_GroupByProject(system, project, department, station, lastday, currentDay) {
    getDataWithArray("/Dashboard/MTTRByWorkcell", { comefrom: system, departmentlist: department, projectlist: project, stationlist: station, currentDay: currentDay, lastday: lastday })
        .then(res => {
            if (res.length == 0) {
                return new Promise.reject("无数据")
            }
            var dataArray = {
                axis: [],
                data: [],
                target:[],
            };
            for (var i = 0; i < res.length; i++) {
                dataArray.axis.push(res[i].item);
                dataArray.data.push(res[i].value);
                dataArray.target.push(res[i].target);
            }
            return dataArray;
        })
        .then(res => {       
                var dataFloor = 0;
                var chartDom = document.getElementById('chart3');
                if (myChart3 != null && myChart3 != "" && myChart3 != undefined) {
                    myChart3.dispose();//解决echarts dom已经加载的报错
                }
                myChart3 = echarts.init(chartDom);
                var optionArray = [];
                var option = {
                    title: {
                        text: 'MTTR Tracking (by project)',
                        textStyle: {
                            fontSize: 15,
                        }
                    },
                    tooltip: {
                        trigger: 'axis',
                        axisPointer: {
                            type: 'cross',
                            crossStyle: {
                                color: '#999'
                            }
                        }
                    },
                    legend: {
                        right: 'right',
                        textStyle: {
                            fontSize: 10,
                        }
                    },
                    grid: {
                        left: '4%',
                        right: '10%',
                        bottom: '0%',
                        containLabel: true
                    },
                    xAxis: {
                        type: 'category',
                        data: res.axis,
                    },
                    yAxis: {
                        type: 'value'
                    },
                    series: [
                        {
                            name: 'MTTR',
                            data: res.data,
                            type: 'bar',
                            showBackground: true,
                            backgroundStyle: {
                                color: 'rgba(180, 180, 180, 0.2)'
                            },
                            label:
                            {
                                show: true,
                                position: 'top',
                            },
                            tooltip: {
                                valueFormatter: function (value) {
                                    return value + ' mins';
                                }
                            },
                            itemStyle: {
                                color: new echarts.graphic.LinearGradient(0, 0, 0, 1, [
                                    { offset: 0, color: '#83bff6' },
                                    { offset: 0.5, color: '#188df0' },
                                    { offset: 1, color: '#188df0' }
                                ])
                            },
                        },
                        {
                            name: 'Target',
                            data:res.target,
                            type: 'line',
                            symbol: 'none',
                            lineStyle: {
                                color: '#188df0',
                                width: 3
                            },
                            tooltip: {
                                valueFormatter: function (value) {
                                    return value + ' mins';
                                }
                            },
                        }
                    ]
                };
                optionArray[0] = option;
            myChart3.setOption(optionArray[dataFloor]);
        })
        .catch(err => {
            console.log(err);
        })
}
// #endregion

// #region Chart4 个项目MTBF实际和目标对比
function getMTBF_ActualvsTarget_GroupByProject(system, project, department, station,lastday, currentDay) {
    getDataWithArray("/Dashboard/MTBFByWorkcell", { comefrom: system, departmentlist: department, projectlist: project, stationlist: station, currentDay: currentDay, lastday: lastday })
        .then(res => {
            if (res.length == 0) {
                return new Promise.reject("无数据")
            }
            var dataArray = {
                axis: [],
                data: [],
                target:[],
            };
            for (var i = 0; i < res.length; i++) {
                dataArray.axis.push(res[i].item);
                dataArray.data.push(res[i].value);
                dataArray.target.push(res[i].target);
            }
            return dataArray;
        })
        .then(res => {       
            var dataFloor = 0;
            var chartDom = document.getElementById('chart4');
            if (myChart4 != null && myChart4 != "" && myChart4 != undefined) {
                myChart4.dispose();//解决echarts dom已经加载的报错
            }
            myChart4 = echarts.init(chartDom);
            var optionArray = [];
            var option = {
                title: {
                    text: 'MTBF Tracking (by project)',
                    textStyle: {
                        fontSize: 15,
                    }
                },
                tooltip: {
                    trigger: 'axis',
                    axisPointer: {
                        type: 'cross',
                        crossStyle: {
                            color: '#999'
                        }
                    }
                },
                legend: {
                    right: 'right',
                    textStyle: {
                        fontSize: 10,
                    }
                },
                grid: {
                    left: '4%',
                    right: '10%',
                    bottom: '0%',
                    containLabel: true
                },
                xAxis: {
                    type: 'category',
                    data: res.axis,
                },
                yAxis: {
                    type: 'value'
                },
                series: [
                    {
                        name: 'MTBF',
                        data: res.data,
                        type: 'bar',
                        showBackground: true,
                        backgroundStyle: {
                            color: 'rgba(180, 180, 180, 0.2)'
                        },
                        label:
                        {
                            show: true,
                            position: 'top',
                        },
                        tooltip: {
                            valueFormatter: function (value) {
                                return value + ' mins';
                            }
                        },
                        itemStyle: {
                            color: new echarts.graphic.LinearGradient(0, 0, 0, 1, [
                                { offset: 0, color: '#83bff6' },
                                { offset: 0.5, color: '#188df0' },
                                { offset: 1, color: '#188df0' }
                            ])
                        },
                    },
                    {
                        name: 'Target',
                        data:res.target,
                        type: 'line',
                        symbol: 'none',
                        lineStyle: {
                            color: '#188df0',
                            width: 3
                        },
                        tooltip: {
                            valueFormatter: function (value) {
                                return value + ' mins';
                            }
                        },
                    }
                ]
            };
            optionArray[0] = option;
            myChart4.setOption(optionArray[dataFloor]);
        })
        .catch(err => {
            console.log(err);
        })
}
// #endregion

// #region Chart5 个项目MTBF,MTBF,MTTA总览
function getMTBF_MTTR_MTTA_Overview(system, project, department, station,lastday, currentDay) {
    var item;
    getDataWithArray("/Dashboard/MTTR_MTTA_Overview", { comefrom: system, departmentlist: department, projectlist: project, stationlist: station,currentDay: currentDay, lastday: lastday })
        .then(res => {
            if (res.length == 0) {
                return new Promise.reject("无数据")
            }
            item = res;
            return getDataWithArray("/Dashboard/TotalMTBF", { comefrom: system, departmentlist: department, projectlist: project, currentDay: currentDay, lastday: lastday })
        })
        .then(res => {
            return { totalMTTA: item[0].totalMTTA, totalMTTR: item[0].totalMTTR, totalMTBF: res[0].totalMTBF}
        })
        .then(res => {
            var dataFloor = 0;
            var chartDom = document.getElementById('chart5');
            if (myChart5 != null && myChart5 != "" && myChart5 != undefined) {
                myChart5.dispose();//解决echarts dom已经加载的报错
            }
            myChart5 = echarts.init(chartDom);
            var optionArray = [];
            var option = {
                tooltip: {
                    formatter: '{b} : {c}'
                },
                series: [
                    {
                        title: {
                            text:'MTTA/MTTR/MTBF Indicator',
                            fontSize: 12,
                            fontWeight: 600
                        },
                        center: ['17%', '55%'],
                        radius: '90%',
                        type: 'gauge',
                        min: 0,
                        max: 200,
                        axisLine: {
                            lineStyle: {
                                width: 15,
                                color: [
                                    [0.8, '#91cc75'],
                                    [1, '#fd666d']
                                ]
                            }
                        },
                        pointer: {
                            width: 4,
                            length: "60%",
                            showAbove: false,
                            itemStyle: {
                                color: 'auto'
                            }
                        },
                        axisTick: {
                            distance: -15,
                            length: 8,
                            lineStyle: {
                                color: '#fff',
                                width: 1
                            }
                        },
                        splitLine: {
                            distance: -15,
                            length: 15,
                            lineStyle: {
                                color: '#fff',
                                width: 2
                            }
                        },
                        axisLabel: {
                            color: 'auto',
                            distance: 20,
                            fontSize: 12
                        },
                        detail: {
                            valueAnimation: true,
                            offsetCenter: [0, '80%'],
                            formatter: '{value} mins',
                            color: 'auto',
                            fontSize: 12,
                        },
                        tooltip: {
                            formatter: '{b} : {c} mins'
                        },
                        data: [
                            {
                                value: res.totalMTTA,
                                name: 'MTTA'
                            }
                        ]
                    },
                    {
                        title: {
                            fontSize: 12,
                            fontWeight: 600
                        },
                        tooltip: {
                            formatter: '{b} : {c} mins'
                        },
                        center: ['50%', '55%'],
                        radius: '90%',
                        type: 'gauge',
                        min: 0,
                        max: 200,
                        axisLine: {
                            lineStyle: {
                                width: 15,
                                color: [
                                    [0.8, '#91cc75'],
                                    [1, '#fd666d']
                                ]
                            }
                        },
                        pointer: {
                            width: 4,
                            length: "60%",
                            showAbove: false,
                            itemStyle: {
                                color: 'auto'
                            }
                        },
                        axisTick: {
                            distance: -15,
                            length: 8,
                            lineStyle: {
                                color: '#fff',
                                width: 1
                            }
                        },
                        splitLine: {
                            distance: -15,
                            length: 15,
                            lineStyle: {
                                color: '#fff',
                                width: 2
                            }
                        },
                        axisLabel: {
                            color: 'auto',
                            distance: 20,
                            fontSize: 12
                        },
                        detail: {
                            valueAnimation: true,
                            offsetCenter: [0, '80%'],
                            formatter: '{value} mins',
                            color: 'auto',
                            fontSize: 12,
                        },
                        data: [
                            {
                                value: res.totalMTTR,
                                name: 'MTTR'
                            }
                        ]
                    },
                    {
                        title: {
                            fontSize: 12,
                            fontWeight: 600
                        },
                        tooltip: {
                            formatter: '{b} : {c} hours'
                        },
                        center: ['83%', '55%'],
                        radius: '90%',
                        type: 'gauge',
                        min: 0,
                        max: 200,
                        axisLine: {
                            lineStyle: {
                                width: 15,
                                color: [
                                    [0.2, '#fd666d'],
                                    [1, '#91cc75']
                                ]
                            }
                        },
                        pointer: {
                            width: 4,
                            length: "60%",
                            showAbove: false,
                            itemStyle: {
                                color: 'auto'
                            }
                        },
                        axisTick: {
                            distance: -15,
                            length: 8,
                            lineStyle: {
                                color: '#fff',
                                width: 1
                            }
                        },
                        splitLine: {
                            distance: -15,
                            length: 15,
                            lineStyle: {
                                color: '#fff',
                                width: 2
                            }
                        },
                        axisLabel: {
                            color: 'auto',
                            distance: 20,
                            fontSize: 12
                        },
                        detail: {
                            valueAnimation: true,
                            offsetCenter: [0, '80%'],
                            formatter: '{value} hours',
                            color: 'auto',
                            fontSize: 12,
                        },
                        data: [
                            {
                                value: res.totalMTBF,
                                name: 'MTBF'
                            }
                        ]
                    }
                ]
            };
            optionArray[0] = option;
            myChart5.setOption(optionArray[dataFloor]);
        })
}
// #endregion

// #region Chart6 个项目MTTR实际和目标对比
function getMTTR_ActualvsTarget_EachMonth(system, project, department, station,lastday, currentDay) {
    getDataWithArray("/Dashboard/MTTRByMonth", { comefrom: system, departmentlist: department, projectlist: project, stationlist: station, currentDay: currentDay, lastday: lastday })
        .then(res => {
            if (res.length == 0) {
                return new Promise.reject("无数据")
            }
            var dataArray = {
                axis: [],
                data: [],
                target: [],
            };
            for (var i = 0; i < res.length; i++) {
                dataArray.axis.push(res[i].item);
                dataArray.data.push(res[i].value);
                dataArray.target.push(res[i].target);
            }
            return dataArray;
        })
        .then(res => {
                var dataFloor = 0;
                var chartDom = document.getElementById('chart6');
                if (myChart6 != null && myChart6 != "" && myChart6 != undefined) {
                    myChart6.dispose();//解决echarts dom已经加载的报错
                }
                myChart6 = echarts.init(chartDom);
                var optionArray = [];
                var option = {
                    title: {
                        text: 'Monthly MTTR Trend',
                        textStyle: {
                            fontSize: 15,
                        }
                    },
                    tooltip: {
                        trigger: 'axis',
                        axisPointer: {
                            type: 'cross',
                            crossStyle: {
                                color: '#999'
                            }
                        }
                    },
                    legend: {
                        right: 'right',
                        textStyle: {
                            fontSize: 10,
                        }
                    },
                    grid: {
                        left: '4%',
                        right: '10%',
                        bottom: '0%',
                        containLabel: true
                    },
                    xAxis: {
                        type: 'category',
                        data: res.axis,
                    },
                    yAxis: {
                        type: 'value'
                    },
                    series: [
                        {
                            name: 'MTTR',
                            data: res.data,
                            type: 'bar',
                            showBackground: true,
                            backgroundStyle: {
                                color: 'rgba(180, 180, 180, 0.2)'
                            },
                            label:
                            {
                                show: true,
                                position: 'top',
                            },
                            tooltip: {
                                valueFormatter: function (value) {
                                    return value + ' mins';
                                }
                            },
                            itemStyle: {
                                color: new echarts.graphic.LinearGradient(0, 0, 0, 1, [
                                    { offset: 0, color: '#28a745' },
                                    { offset: 0.5, color: '#20c997' },
                                    { offset: 1, color: '#17a2b8' }
                                ])
                            },
                        },
                        {
                            name: 'Target',
                            data: res.target,
                            type: 'line',
                            symbol: 'none',
                            lineStyle: {
                                color: '#188df0',
                                width: 3
                            },
                            tooltip: {
                                valueFormatter: function (value) {
                                    return value + ' mins';
                                }
                            },
                        }
                    ]
                };
                optionArray[0] = option;
                myChart6.setOption(optionArray[dataFloor]);
        })
        .catch(err => {
            showWarning(err);
        })
}
// #endregion

// #region Chart7 个项目MTBF实际和目标对比
function getMTBF_ActualvsTarget_EachMonth(system, project, department, station, lastday, currentDay) {
    getDataWithArray("/Dashboard/MTBFByMonth", { comefrom: system, departmentlist: department, projectlist: project, stationlist: station, currentDay: currentDay, lastday: lastday })
        .then(res => {
            if (res.length == 0) {
                return new Promise.reject("无数据")
            }
            var dataArray = {
                axis: [],
                data: [],
                target: [],
            };
            for (var i = 0; i < res.length; i++) {
                dataArray.axis.push(res[i].item);
                dataArray.data.push(res[i].value);
                dataArray.target.push(res[i].target);
            }
            return dataArray;
        })
        .then(res => {          
                var dataFloor = 0;
                var chartDom = document.getElementById('chart7');
                if (myChart7 != null && myChart7 != "" && myChart7 != undefined) {
                    myChart7.dispose();//解决echarts dom已经加载的报错
                }
                myChart7 = echarts.init(chartDom);
                var optionArray = [];
                var option = {
                    title: {
                        text: 'Monthly MTBF Trend',
                        textStyle: {
                            fontSize: 15,
                        }
                    },
                    tooltip: {
                        trigger: 'axis',
                        axisPointer: {
                            type: 'cross',
                            crossStyle: {
                                color: '#999'
                            }
                        }
                    },
                    legend: {
                        right: 'right',
                        textStyle: {
                            fontSize: 10,
                        }
                    },
                    grid: {
                        left: '4%',
                        right: '10%',
                        bottom: '0%',
                        containLabel: true
                    },
                    xAxis: {
                        type: 'category',
                        data: res.axis,
                    },
                    yAxis: {
                        type: 'value'
                    },
                    series: [
                        {
                            name: 'MTBF',
                            data: res.data,
                            type: 'bar',
                            showBackground: true,
                            backgroundStyle: {
                                color: 'rgba(180, 180, 180, 0.2)'
                            },
                            label:
                            {
                                show: true,
                                position: 'top',
                            },
                            tooltip: {
                                valueFormatter: function (value) {
                                    return value + ' mins';
                                }
                            },
                            itemStyle: {
                                color: new echarts.graphic.LinearGradient(0, 0, 0, 1, [
                                    { offset: 0, color: '#28a745' },
                                    { offset: 0.5, color: '#20c997' },
                                    { offset: 1, color: '#17a2b8' }
                                ])
                            },
                        },
                        {
                            name: 'Target',
                            data: res.target,
                            type: 'line',
                            symbol: 'none',
                            lineStyle: {
                                color: '#188df0',
                                width: 3
                            },
                            tooltip: {
                                valueFormatter: function (value) {
                                    return value + ' mins';
                                }
                            },
                        }
                    ]
                };
                optionArray[0] = option;
                myChart7.setOption(optionArray[dataFloor]);
        })
}
// #endregion

// #region Chart8 个项目MTTR BY Area总览
function getMTTR_ByArea(system, project, department, station, lastday, currentDay) {
    getDataWithArray("/Dashboard/MTTRByArea", { comefrom: system, departmentlist: department, projectlist: project, stationlist: station, currentDay: currentDay, lastday: lastday })
        .then(res => {
            if (res.length == 0) {
                return new Promise.reject("无数据")
            }
            var dataArray = {
                axis: [],
                data: [],
                target: [],
            };
            for (var i = 0; i < res.length; i++) {
                dataArray.axis.push(res[i].item);
                dataArray.data.push(res[i].value);
                dataArray.target.push(res[i].target);
            }
            return dataArray;
        })
        .then(res => {
                var dataFloor = 0;
                var chartDom = document.getElementById('chart8');
                if (myChart8 != null && myChart8 != "" && myChart8 != undefined) {
                    myChart8.dispose();//解决echarts dom已经加载的报错
                            }
                myChart8 = echarts.init(chartDom);
                var optionArray = [];
                var option = option = {
                    title: {
                        text: 'Area MTTR Actual vs Target',
                        textStyle: {
                            fontSize: 15,
                        }
                    },
                    tooltip: {
                        trigger: 'axis',
                        axisPointer: {
                            label: {
                                backgroundColor: '#6a7985'
                            }
                        }
                    },
                    grid: {
                        left: '4%',
                        right: '10%',
                        bottom: '0%',
                        containLabel: true
                    },
                    legend: {
                        right: 'right',
                        textStyle: {
                            fontSize: 10,
                        }
                    },
                    xAxis: {
                        type: 'category',
                        boundaryGap: false,
                        data: res.axis,
                    },
                    yAxis: {
                        type: 'value'
                    },
                    series: [
                        {
                            name: 'MTTR',
                            data: res.data,
                            type: 'line',
                            label:
                            {
                                show: true,
                                position: 'top',
                            },
                            areaStyle: {}
                        },
                        {
                            name: 'Target',
                            color: 'red',
                            data: res.target,
                            type: 'line',
                            symbol: 'none',
                            lineStyle: {
                                type: 'dashed',
                                width: 3,
                            }
                        }
                    ]
                };
                optionArray[0] = option;
                myChart8.setOption(optionArray[dataFloor]);
        })
        .catch(err => {
            showWarning(err);
        })
}
// #endregion

//#region 窗口大小调整，echart图的大小随之改变
window.onresize = function () {
    if (myChart1)  myChart1.resize();
    if (myChart2)  myChart2.resize();
    if (myChart3)  myChart3.resize();
    if (myChart4)  myChart4.resize();
    if (myChart5)  myChart5.resize();
    if (myChart6)  myChart6.resize();
    if (myChart7)  myChart7.resize();
    if (myChart8)  myChart8.resize();
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
    },350);
})
//#endregion



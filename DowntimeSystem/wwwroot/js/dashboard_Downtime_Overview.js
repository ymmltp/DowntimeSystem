var myChart1;
var myChart2;
var myChart3;
var myChart4;
var myChart5;
var myChart6;


// #region Chart1 Downtime Distribution by Project,department
function getTotalDT(system, project, department ,station,lastday, currentDay) {
    getDataWithArray("/Dashboard/GetTotalDT", { comefrom: system, departmentlist: department, projectlist: project, stationlist: station, currentDay: currentDay, lastday: lastday })
        .then(res => {
            var dataFloor = 0;
            var chartDom = document.getElementById('chart1');
            if (myChart1 != null && myChart1 != "" && myChart1 != undefined) {
                myChart1.dispose();//解决echarts dom已经加载的报错
            }
            myChart1 = echarts.init(chartDom);
            var optionArray = [];
            var option = {
                tooltip: {
                    confine: true,
                },
                series: [
                    {
                        title: {
                            fontSize: 15,
                            fontWeight: 600,
                            offsetCenter: [0, '-130%'],
                        },
                        center: ['50%', '55%'],
                        radius: '95%',
                        type: 'gauge',
                        min: 0,
                        max: 150000,
                        itemStyle: {
                            color: '#1aafad',
                            shadowColor: 'rgba(0,138,255,0.45)',
                            shadowBlur: 13,
                            shadowOffsetX: 2,
                            shadowOffsetY: 2
                        },
                        progress: {
                            show: true,
                            roundCap: true,
                            width: 12
                        },
                        pointer: {
                            icon: 'path://M2090.36389,615.30999 L2090.36389,615.30999 C2091.48372,615.30999 2092.40383,616.194028 2092.44859,617.312956 L2096.90698,728.755929 C2097.05155,732.369577 2094.2393,735.416212 2090.62566,735.56078 C2090.53845,735.564269 2090.45117,735.566014 2090.36389,735.566014 L2090.36389,735.566014 C2086.74736,735.566014 2083.81557,732.63423 2083.81557,729.017692 C2083.81557,728.930412 2083.81732,728.84314 2083.82081,728.755929 L2088.2792,617.312956 C2088.32396,616.194028 2089.24407,615.30999 2090.36389,615.30999 Z',
                            length: "65%",
                            showAbove: false,
                            width: 10,
                        },
                        tooltip: {
                            formatter: '{b}:<b>{c}</b>hours'
                        },
                        axisTick: {
                            distance: 6,
                            splitNumber: 4,
                            lineStyle: {
                                color: '#999',
                                width: 1
                            }
                        },
                        splitLine: {
                            distance: 6,
                            length: 10,
                            lineStyle: {
                                color: '#999',
                                width: 2
                            }
                        },
                        axisLine: {
                            roundCap: true,
                        },
                        axisLabel: {
                            distance: 12,
                            fontSize: 8
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
                                value: res[0].value,
                                name: 'Total Downtime',
                            }
                        ]
                    },
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
function getDTDistribution_ByDepartment_Status(system, project, department, station,lastday, currentDay) {
    getDataWithArray("/Dashboard/GetDTDistribution_ByDepartment_Status", { comefrom: system, departmentlist: department, projectlist: project, stationlist:station, currentDay: currentDay, lastday: lastday })
        .then(res => {
            if (res.length == 0) {
                return new Promise.reject("无数据")
            }
            var dataArray = {
                axis: [],
                data: []
            };
            for (var i = 0; i < res.length; i++) {
                if (dataArray.axis.length == 0 || dataArray.axis.indexOf(res[i].department) == -1) {
                    dataArray.axis.push(res[i].department);
                }
            }
            for (var i = 0; i < res.length; i++) {
                if (dataArray.data.length == 0 || dataArray.data.find(e => e.name == res[i].status) == undefined) {
                    var item = {
                        name: res[i].status,
                        type: 'bar',
                        stack: 'total',
                        label: {
                            show: true
                        },
                        emphasis: {
                            focus: 'series'
                        },
                        data: new Array(dataArray.axis.length)
                    };
                    dataArray.data.push(item);
                }
                dataArray.data.find(e => e.name == res[i].status).data[dataArray.axis.indexOf(res[i].department)] = res[i].dt;
            }
            return dataArray;
        })
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
                title: {
                    text: 'Function Team DT Status',
                    textStyle: {
                        fontSize: 15,
                    }
                },
                color: ['#ee6666', '#fac858', '#91cc75', '#5470c6'],
                tooltip: {
                    trigger: 'axis',
                    axisPointer: {
                        type: 'shadow' 
                    },
                    valueFormatter: function (value) {
                        return value + ' hours';
                    }
                },
                legend: {
                    right: 'left',
                    textStyle: {
                        fontSize: 10,
                    },
                },
                grid: {
                    left: '3%',
                    right: '4%',
                    bottom: '3%',
                    containLabel: true
                },
                xAxis: {
                    type: 'category',
                    data: res.axis

                },
                yAxis: {
                    type: 'value',
               
                },
                series: res.data,
            };
            myChart2.setOption(option);
        })
        .catch(err => {
            console.log(err);
        })
}
// #endregion

// #region Chart3 个项目MTTR实际和目标对比
function getDT_ByMonth(system, project, department, station, lastday, currentDay) {
    getDataWithArray("/Dashboard/GetDTDistribution_ByMonth", { comefrom: system, departmentlist: department, projectlist: project, stationlist: station, currentDay: currentDay, lastday: lastday })
        .then(res => {
            if (res.length == 0) {
                return new Promise.reject("无数据")
            }
            var dataArray = {
                axis: [],
                data: [],
            };
            for (var i = 0; i < res.length; i++) {
                dataArray.axis.push(res[i].month);
                dataArray.data.push(res[i].dt);
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
                    text: 'Plant Downtime Track',
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
                    },
                    valueFormatter: function (value) {
                        return value + ' hours';
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
                    data: res.axis
                },
                yAxis: {
                    type: 'value'
                },
                series: [
                    {
                        name: 'DT 时间',
                        data: res.data,
                        type: 'line',
                        label:
                        {
                            show: true,
                            position: 'top',
                        },
                        areaStyle: {}
                    },
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
function getDT_ByArea(system, project, department, station,lastday, currentDay) {
    getDataWithArray("/Dashboard/GetDTDistribution_ByArea", { comefrom: system, departmentlist: department, projectlist: project, stationlist: station, currentDay: currentDay, lastday: lastday })
        .then(res => {
            if (res.length == 0) {
                return new Promise.reject("无数据")
            }
            var dataArray = {
                axis: [],
                data: [],
            };
            for (var i = 0; i < res.length; i++) {
                dataArray.axis.push(res[i].item);
                var item = {
                    value: res[i].dt,
                    name: res[i].item
                }
                dataArray.data.push(item);
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
                    text: "Downtime Distribution",
                    left: 'center',
                    textStyle: {
                        fontSize: 15,
                    }
                },
                tooltip: {
                    trigger: 'item',
                    formatter: '{b}: <b>{c} hours</b> ({d}%)',
                    confine: true,
                },
                legend: {
                    top: '15%',
                    itemGap: 3,
                },
                series: [
                    {
                        name: 'DT',
                        type: 'pie',
                        radius: ['45%', '80%'],
                        center: ['50%', '65%'],
                        avoidLabelOverlap: false,
                        itemStyle: {
                            borderRadius: 6,
                            borderColor: '#fff',
                            borderWidth: 2
                        },
                        label: {
                            show: true,
                            position: 'inner',
                            formatter: '{b} \n{d}%',
                        },
                        data: res.data,
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
function getDT_Frquency_ByMonth(system, project, department, station,lastday, currentDay) {
    getDataWithArray("/Dashboard/GetDTDistribution_ByMonth", { comefrom: system, departmentlist: department, projectlist: project, stationlist: station,currentDay: currentDay, lastday: lastday })
        .then(res => {
            if (res.length==0) {
                return new Promise.reject("无数据")
            }
            var dataArray = {
                axis: [],
                data: [],
                count:[],
            };
            for (var i = 0; i < res.length; i++) {
                dataArray.axis.push(res[i].month);
                dataArray.data.push(res[i].dt);
                dataArray.count.push(res[i].count);
            }
            return dataArray;
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
                title: {
                    text: 'Downtime & Frequency Trade',
                    textStyle: {
                        fontSize: 15,
                    }
                },
                tooltip: {
                    trigger: 'axis',
                    axisPointer: {                       
                        crossStyle: {
                            color: '#999'
                        }
                    }
                },
                legend: {
                    right: 'left',
                    textStyle: {
                        fontSize: 10,
                    }
                },
                grid: {
                    left: '4%',
                    right: '5%',
                    bottom: '0%',
                    containLabel: true
                },
                xAxis: {
                    type: 'category',
                    data: res.axis,
                },
                yAxis: [
                    {
                        type: 'value',
                        name: 'DT 时间',
                        axisLabel: {
                            formatter: '{value} hours'
                        }
                    },
                    {
                        type: 'value',
                        name: 'DT 频次',
                        axisLabel: {
                            formatter: '{value} '
                        }
                    }
                ],
                series: [
                    {
                        name: 'DT 时间',
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
                                return value + ' hours';
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
                        name: 'DT 频次',
                        yAxisIndex: 1,
                        data: res.count,
                        type: 'line',
                        symbol: 'none',
                        lineStyle: {
                            color: '#188df0',
                            width: 3
                        },
                        tooltip: {
                            valueFormatter: function (value) {
                                return value ;
                            }
                        },
                    }
                ]
            };
            optionArray[0] = option;
            myChart5.setOption(optionArray[dataFloor]);
        })
}
// #endregion

// #region Chart6 个项目MTTR实际和目标对比
function getDT_CompareByMonth(system, project, department, station,lastday, currentDay) {
    getDataWithArray("/Dashboard/GetDTDistribution_ByMonth", { comefrom: system, departmentlist: department, projectlist: project, stationlist: station, currentDay: currentDay, lastday: lastday })
        .then(res => {
            if (res.length == 0) {
                return new Promise.reject("无数据")
            }
            if (res.length >= 2) {
                var dataArray = {
                    current: res[res.length - 1].dt,
                    last: 0
                };
                var sum = 0;
                for (var i = res.length - 2; i >= 0; i--) {
                    sum += res[i].dt;
                }
                dataArray.last = sum / (res.length-1)
            } else {
                var dataArray = {
                    current: res[res.length - 1].dt,
                    last: res[res.length - 1].dt
                };
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
                series: [
                    {
                        type: 'gauge',
                        startAngle: 90,
                        endAngle: -270,
                        pointer: {
                            show: false
                        },
                        radius: '95%',
                        max: 1500000,
                        progress: {
                            show: true,
                            overlap: false,
                            roundCap: true,
                            clip: false,
                            itemStyle: {
                                borderWidth: 1,
                                borderColor: '#464646'
                            }
                        },
                        axisLine: {
                            lineStyle: {
                                width: 40
                            }
                        },
                        splitLine: {
                            show: false,
                            distance: 0,
                            length: 10
                        },
                        axisTick: {
                            show: false
                        },
                        axisLabel: {
                            show: false,
                            distance: 50
                        },
                        data: [
                            {
                                value: res.current,
                                name: '当月',
                                title: {
                                    offsetCenter: ['0%', '-35%']
                                },
                                detail: {
                                    valueAnimation: true,
                                    offsetCenter: ['0%', '-18%']
                                }
                            },
                            {
                                value: res.last,
                                name: '同期',
                                title: {
                                    offsetCenter: ['0%', '10%']
                                },
                                detail: {
                                    valueAnimation: true,
                                    offsetCenter: ['0%', '27%']
                                }
                            },
                        ],
                        title: {
                            fontSize: 12
                        },
                        detail: {
                            width: 50,
                            height: 8,
                            fontSize: 10,
                            color: 'auto',
                            borderColor: 'auto',
                            borderRadius: 20,
                            borderWidth: 1,
                            formatter: '{value} hours'
                        }
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

//#region 窗口大小调整，echart图的大小随之改变
window.onresize = function () {
    if (myChart1)  myChart1.resize();
    if (myChart2)  myChart2.resize();
    if (myChart3)  myChart3.resize();
    if (myChart4)  myChart4.resize();
    if (myChart5)  myChart5.resize();
    if (myChart6)  myChart6.resize();
}
$("#sideToggle").on('change', function () {
    setTimeout(function () {
        myChart1.resize();
        myChart2.resize();
        myChart3.resize();
        myChart4.resize();
        myChart5.resize();
        myChart6.resize();
    },350);
})
//#endregion



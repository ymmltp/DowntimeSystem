var myChart1;
var myChart2;
var myChart3;
var myChart4;
var myChart5;


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
                                name: 'Plant Total DT',
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
function getDTcount_GroupByStatus(system, project, department, station, lastday, currentDay) {
    getDataWithArray("/Dashboard/GetCountGroupByDowntimeStatus", { comefrom: system, departmentlist: department, projectlist: project, stationlist: station, currentDay: currentDay, lastday: lastday })
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
                    text: 'DT Status',
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
                    //orient: 'vertical',
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

// #region Chart3 TOP5 Defect Code
function getDT_ByDefectCode_Time(system, project, department, station, lastday, currentDay, filterType) {
    getDataWithArray("/Dashboard/GetDTDistribution_byDefectCode_Time", { comefrom: system, departmentlist: department, projectlist: project, stationlist: station, currentDay: currentDay, lastday: lastday, filterType: filterType })
        .then(res => {
            if (res.length == 0) {
                return new Promise.reject("无数据")
            }
            var dataArray = {
                axis: [],
                data: []
            };
            for (var i = 0; i < res.length; i++) {
                if (dataArray.axis.length == 0 || dataArray.axis.indexOf(res[i].filterType) == -1) {
                    dataArray.axis.push(res[i].filterType);
                }
            }
            for (var i = 0; i < res.length; i++) {
                if (dataArray.data.length == 0 || (dataArray.data.find(e => e.name == res[i].issue) == undefined && dataArray.data.length<5)) {
                    var item = {
                        name: res[i].issue,
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
                dataArray.data.find(e => e.name == res[i].issue) != undefined ? dataArray.data.find(e => e.name == res[i].issue).data[dataArray.axis.indexOf(res[i].filterType)] = res[i].dt : null;
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
                        text: 'Top5 DT (by Defect Code)',
                        extStyle: {
                            fontSize: 15,
                        }
                },
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
                    top: '10%',
                    type: 'scroll',
                    textStyle: {
                        fontSize: 10,
                    }
                },
                grid: {
                    left: '3%',
                    right: '4%',
                    bottom: '3%',
                    containLabel: true
                },
                xAxis: {
                    type: 'category',
                    data: res.axis,
                },
                yAxis: {
                    type: 'value'
                },
                series: res.data
            };
                optionArray[0] = option;
            myChart3.setOption(optionArray[dataFloor]);
        })
        .catch(err => {
            console.log(err);
        })
}
// #endregion

// #region Chart4 TOP5 Station
function getDT_ByStation_Time(system, project, department, station, lastday, currentDay, filterType) {
    getDataWithArray("/Dashboard/GetDTDistribution_byStation_Time", { comefrom: system, departmentlist: department, projectlist: project, stationlist: station, currentDay: currentDay, lastday: lastday, filterType: filterType })
        .then(res => {
            if (res.length == 0) {
                return new Promise.reject("无数据")
            }
            var dataArray = {
                axis: [],
                data: []
            };
            for (var i = 0; i < res.length; i++) {
                if (dataArray.axis.length == 0 || dataArray.axis.indexOf(res[i].filterType) == -1) {
                    dataArray.axis.push(res[i].filterType);
                }
            }
            for (var i = 0; i < res.length; i++) {
                if (dataArray.data.length == 0 || (dataArray.data.find(e => e.name == res[i].station) == undefined && dataArray.data.length < 5)) {
                    var item = {
                        name: res[i].station,
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
                dataArray.data.find(e => e.name == res[i].station) != undefined ? dataArray.data.find(e => e.name == res[i].station).data[dataArray.axis.indexOf(res[i].filterType)] = res[i].dt : null;
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
                    text: 'Top5 DT (by Station)',
                    textStyle: {
                        fontSize: 15,
                    }
                },
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
                    top: '10%',
                    type: 'scroll',
                    textStyle: {
                        fontSize: 10,
                    }
                },
                grid: {
                    left: '3%',
                    right: '4%',
                    bottom: '3%',
                    containLabel: true
                },
                xAxis: {
                    type: 'category',
                    data: res.axis,
                },
                yAxis: {
                    type: 'value'
                },
                series: res.data
            };
            optionArray[0] = option;
            myChart4.setOption(optionArray[dataFloor]);
        })
        .catch(err => {
            console.log(err);
        })
}
// #endregion

// #region Chart5 TOP5 Line
function getDT_ByLine_Time(system, project, department, station, lastday, currentDay, filterType) {
    getDataWithArray("/Dashboard/GetDTDistribution_byLine_Time", { comefrom: system, departmentlist: department, projectlist: project, stationlist: station, currentDay: currentDay, lastday: lastday, filterType: filterType })
        .then(res => {
            if (res.length == 0) {
                return new Promise.reject("无数据")
            }
            var dataArray = {
                axis: [],
                data: []
            };
            for (var i = 0; i < res.length; i++) {
                if (dataArray.axis.length == 0 || dataArray.axis.indexOf(res[i].filterType) == -1) {
                    dataArray.axis.push(res[i].filterType);
                }
            }
            for (var i = 0; i < res.length; i++) {
                if (dataArray.data.length == 0 || (dataArray.data.find(e => e.name == res[i].line) == undefined && dataArray.data.length < 5)) {
                    var item = {
                        name: res[i].line,
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
                dataArray.data.find(e => e.name == res[i].line) != undefined ? dataArray.data.find(e => e.name == res[i].line).data[dataArray.axis.indexOf(res[i].filterType)] = res[i].dt : null;
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
                    text: 'Top5 DT (by Line)',
                    textStyle: {
                        fontSize: 15,
                    }
                },
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
                    top: '10%',
                    type: 'scroll',
                    textStyle: {
                        fontSize: 10,
                    }
                },
                grid: {
                    left: '3%',
                    right: '4%',
                    bottom: '3%',
                    containLabel: true
                },
                xAxis: {
                    type: 'category',
                    data: res.axis,
                },
                yAxis: {
                    type: 'value'
                },
                series: res.data
            };
            optionArray[0] = option;
            myChart5.setOption(optionArray[dataFloor]);
        })
        .catch(err => {
            console.log(err);
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
}
$("#sideToggle").on('change', function () {
    setTimeout(function () {
        myChart1.resize();
        myChart2.resize();
        myChart3.resize();
        myChart4.resize();
        myChart5.resize();
    },350);
})
//#endregion



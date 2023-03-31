var myChart1;
var myChart2;
var myChart3;
var myChart4;
var myChart5;

//var lastday = "";

// #region Chart1 Downtime Distribution by Project,department
function getMTTR_GroupByDepartment_Time(system, project, department,station, lastday, currentDay, filterType) {
    getDataWithArray("/Dashboard/MTTR_MTTA_Detail_ByDepartment_Time", { comefrom: system, departmentlist: department, projectlist: project, currentDay: currentDay, lastday: lastday, filterType: filterType })
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
                if (dataArray.data.length==0 || dataArray.data.find(e=>e.name==res[i].department) == undefined) {
                    var item = {
                        name: res[i].department,
                        type: 'line',
                        stack: 'Total',
                        data: new Array(dataArray.axis.length),
                    };
                    dataArray.data.push(item);
                }
                dataArray.data.find(e => e.name == res[i].department).data[dataArray.axis.indexOf(res[i].filterType)] = res[i].mttr;
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
                    text: 'MTTR ' + filterType+' Tracking (by Functional Team) ',
                    textStyle: {
                        fontSize: 15,
                    }
                },
                tooltip: {
                    trigger: 'axis',
                    valueFormatter: function (value) {
                        return value + ' mins';
                    }
                },
                legend: {
                    data: ['TE', 'ME', 'IE'],
                    top: '10%',
                    right: 'left',
                    itemGap: 3,
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
                    boundaryGap: false,
                    data: res.axis,
                },
                yAxis: {
                    type: 'value'
                },
                series: res.data,
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
function getMTBF_GroupByDepartment_Time(system, project, department, station, lastday, currentDay, filterType) {
    getDataWithArray("/Dashboard/MTBF_Detail_ByDepartment_Time", { comefrom: system, departmentlist: department, projectlist: project, currentDay: currentDay, lastday: lastday, filterType: filterType })
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
                if (dataArray.data.length == 0 || dataArray.data.find(e => e.name == res[i].department) == undefined) {
                    var item = {
                        name: res[i].department,
                        type: 'line',
                        stack: 'Total',
                        data: new Array(dataArray.axis.length),
                    };
                    dataArray.data.push(item);
                }
                dataArray.data.find(e => e.name == res[i].department).data[dataArray.axis.indexOf(res[i].filterType)] = res[i].value;
            }

            return dataArray;
        })
        .then(res => {
            var dataFloor = 0;
            var chartDom = document.getElementById('chart2');
            if (myChart2 != null && myChart2 != "" && myChart2 != undefined) {
                myChart2.dispose();//解决echarts dom已经加载的报错
            }
            myChart2 = echarts.init(chartDom);
            var optionArray = [];
            var option = {
                title: {
                    text: 'MTBF ' + filterType +' Tracking (by Functional Team) ',
                    textStyle: {
                        fontSize: 15,
                    }
                },
                tooltip: {
                    trigger: 'axis',
                    valueFormatter: function (value) {
                        return value + ' hours';
                    }
                },
                legend: {
                    data: ['TE', 'ME', 'IE'],
                    top: '10%',
                    right: 'left',
                    itemGap: 3,
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
                    boundaryGap: false,
                    data: res.axis,
                },
                yAxis: {
                    type: 'value'
                },
                series: res.data,
            };
            optionArray[0] = option;
            myChart2.setOption(optionArray[dataFloor]);
        })
        .catch(err => {
            showWarning(err);
        })
}
// #endregion

// #region Chart3 个项目MTTR实际和目标对比
function getMTTR_MTTA_ByTime(system, project, department, station, lastday, currentDay, filterType) {
    getDataWithArray("/Dashboard/MTTR_MTTA_Detail_byTime", { comefrom: system, departmentlist: department, projectlist: project, currentDay: currentDay, lastday: lastday, filterType: filterType })
        .then(res => {
            if (res.length == 0) {
                return new Promise.reject("无数据")
            }
            var dataArray = {
                axis: [],
                mttr: [],
                mtta: [],
            };
            for (var i = 0; i < res.length; i++) {
                dataArray.axis.push(res[i].filterType);
                dataArray.mttr.push(res[i].mttr);
                dataArray.mtta.push(res[i].mtta);
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
                color: ["#fac858", "#5470c6"],
                title: {
                    text: 'MTTA/MTTR' + filterType+' Tracking',
                    textStyle: {
                        fontSize: 15,
                    }
                },
                tooltip: {
                    trigger: 'axis',
                    axisPointer: {
                        // Use axis to trigger tooltip
                        type: 'shadow' // 'shadow' as default; can also be 'line' or 'shadow'
                    },
                    valueFormatter: function (value) {
                        return value + ' mins';
                    }
                },
                legend: {
                    data: ['MTTA', 'MTTR'],
                    top: '5%',
                    right: 'left',
                    itemGap: 3,
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
                    type: 'value'
                },
                yAxis: {
                    type: 'category',
                    data: res.axis
                },
                    series: [{
                        name: 'MTTA',
                        type: 'bar',
                        stack: 'total',
                        label: {
                            show: true
                        },
                        emphasis: {
                            focus: 'series'
                        },
                        data:res.mtta
                    },
                    {
                        name: 'MTTR',
                        type: 'bar',
                        stack: 'total',
                        label: {
                            show: true
                        },
                        emphasis: {
                            focus: 'series'
                        },
                        data: res.mttr
                    },],
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
function getMTTR_withTarget_ByTime(system, project, department, station, lastday, currentDay, filterType) {
    getDataWithArray("/Dashboard/MTTR_MTTA_Detail_byTime", { comefrom: system, departmentlist: department, projectlist: project, currentDay: currentDay, lastday: lastday, filterType: filterType })
        .then(res => {
            if (res.length == 0) {
                return new Promise.reject("无数据")
            }
            var dataArray = {
                axis: [],
                mttr: [],
                target:[],
            };
            for (var i = 0; i < res.length; i++) {
                dataArray.axis.push(res[i].filterType);
                dataArray.mttr.push(res[i].mttr);
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
                    text: 'MTTR ' + filterType+' Tracking(Avg.)',
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
                    data: res.axis
                },
                yAxis: {
                    type: 'value'
                },
                series: [
                    {
                        name: 'MTTR',
                        data: res.mttr,
                        type: 'bar',
                        showBackground: true,
                        backgroundStyle: {
                            color: 'rgba(180, 180, 180, 0.2)'
                        },
                        tooltip: {
                            valueFormatter: function (value) {
                                return value + ' mins';
                            }
                        },
                        label: {
                            show: true,
                            position: 'top',
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
            myChart4.setOption(optionArray[dataFloor]);
        })
        .catch(err => {
            console.log(err);
        })
}
// #endregion

// #region Chart5 个项目MTBF,MTBF,MTTA总览
function getMTBF_withTarget_ByTime(system, project, department, station, lastday, currentDay, filterType) {
    getDataWithArray("/Dashboard/MTBF_Detail_ByTime", { comefrom: system, departmentlist: department, projectlist: project, currentDay: currentDay, lastday: lastday, filterType: filterType })
        .then(res => {
            if (res.length == 0) {
                return new Promise.reject("无数据")
            }
            var dataArray = {
                axis: [],
                value: [],
                target: [],
            };
            for (var i = 0; i < res.length; i++) {
                dataArray.axis.push(res[i].filterType);
                dataArray.value.push(res[i].value);
                dataArray.target.push(res[i].target);
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
                    text: 'MTBF ' + filterType + ' Tracking(Avg.)',
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
                    data: res.axis
                },
                yAxis: {
                    type: 'value'
                },
                series: [
                    {
                        name: 'MTBF',
                        data: res.value,
                        type: 'bar',
                        showBackground: true,
                        backgroundStyle: {
                            color: 'rgba(180, 180, 180, 0.2)'
                        },
                        label: {
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
                                return value + ' hours';
                            }
                        },
                    }
                ]
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



var myChart1;
const colors = ['#5470C6', '#91CC75', '#EE6666'];

function EmployeeWorkEffiency(system, project, department, lastday, currentDay) {
    getDataWithArray("/Dashboard/EmployeeWorkEffiency", { comefrom: system, departmentlist: department, projectlist: project, currentDay: currentDay, lastday: lastday })
        .then(res => {
            var dataArray = {
                axis: [],
                data: []
            };
            var barchart = {
                name: 'Repair Count',
                type: 'bar',
                label: {
                    show: true
                },
                tooltip: {
                    valueFormatter: function (value) {
                        return value + ' piece';
                    }
                },
                data: [ ]
            };
            var linechart = {  
                name: 'Repair mean time',
                type: 'line',
                label: {
                    show: true
                },
                yAxisIndex: 1,
                tooltip: {
                    valueFormatter: function (value) {
                        return value + ' mins';
                    }
                },
                data: [] 
            }
            var totallinechart = {
                name: 'Total Repair time',
                type: 'line',
                label: {
                    show: true
                },
                yAxisIndex: 2,
                tooltip: {
                    valueFormatter: function (value) {
                        return value + ' mins';
                    }
                },
                data: []
            }
            for (var i = 0; i < res.length; i++) {
                dataArray.axis.push(res[i].item);
                barchart.data.push(res[i].count);
                linechart.data.push(res[i].value);
                totallinechart.data.push(res[i].totalValue);
            }
            dataArray.data.push(barchart);
            dataArray.data.push(linechart);
            dataArray.data.push(totallinechart);
            return dataArray;
        })
        .then(res => {
            var dataFloor = 0;
            var chartDom = document.getElementById('chart1');
            if (myChart1 != null && myChart1 != "" && myChart1 != undefined) {
                myChart1.dispose();
            }
            myChart1 = echarts.init(chartDom);
            var optionArray = [];
            var option = {
                color: colors,
                title: {
                    text: 'Employee Work',
                    left: 'center',
                    textStyle: {
                        fontSize: 15,
                    }
                },
                tooltip: {
                    trigger: 'axis',
                    axisPointer: {
                        type: 'shadow',
                        label: {
                            show: true
                        }
                    }
                },
                legend: {
                    data: ['Repair Count', 'Repair mean time','Total Repair time'],
                    top:25,
                },
                dataZoom: [
                    {
                        show: true,
                        start: 0,
                        end: 100,
                        maxValueSpan: 20
                    },
                ],
                grid: {
                    left: '10%',
                    right: '10%',
                    containLabel: true
                },
                xAxis: [
                    {
                        type: 'category',
                        data: res.axis,
                        axisPointer: {
                            type: 'shadow'
                        }
                    }
                ],
                yAxis: [
                    {
                        type: 'value',
                        name: 'Repair Count',
                        axisLabel: {
                            formatter: '{value} piece'
                        },
                        min: 0,
                        axisLine: {
                            show: true,
                            lineStyle: {
                                color: colors[0]
                            }
                        },
                    },
                    {
                        type: 'value',
                        name: 'Mean time',
                        axisLabel: {
                            formatter: '{value} mins'
                        },
                        min: 0,
                        axisLine: {
                            show: true,
                            lineStyle: {
                                color: colors[1]
                            }
                        },
                    },
                    {
                        type: 'value',
                        name: 'Total time',
                        axisLabel: {
                            formatter: '{value} mins'
                        },
                        min: 0,
                        position: 'right',
                        alignTicks: true,
                        offset: 80,
                        axisLine: {
                            show: true,
                            lineStyle: {
                                color: colors[2]
                            }
                        },
                    }
                ],
                series: res.data,
            };
            optionArray[0] = option;
            myChart1.setOption(optionArray[dataFloor]);
        })
}
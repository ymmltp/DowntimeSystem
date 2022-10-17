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
                name: 'Repaire Count',
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
                name: 'Repaire mean time',
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
            for (var i = 0; i < res.length; i++) {
                dataArray.axis.push(res[i].item);
                barchart.data.push(res[i].count);
                linechart.data.push(res[i].value);
            }
            dataArray.data.push(barchart);
            dataArray.data.push(linechart);
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
                    data: ['Repaire Count', 'Repaire mean time'],
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
                        name: 'Repaire Count (piece)',
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
                        name: 'Repaire mean time (mins)',
                        min: 0,
                        axisLine: {
                            show: true,
                            lineStyle: {
                                color: colors[1]
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
var myChart1;

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
                        type: 'cross',
                        crossStyle: {
                            color: '#999'
                        }
                    }
                },
                legend: {
                    data: ['Repaire Count', 'Repaire mean time']
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
                        name: 'Repaire Count',
                        min: 0,
                        max: 250,
                        interval: 50,
                        axisLabel: {
                            formatter: '{value} piece'
                        }
                    },
                    {
                        type: 'value',
                        name: 'Repaire mean time',
                        min: 0,
                        max: 25,
                        interval: 5,
                        axisLabel: {
                            formatter: '{value} mins'
                        }
                    }
                ],
                series: res.data,
            };
            optionArray[0] = option;
            myChart1.setOption(optionArray[dataFloor]);
        })
}
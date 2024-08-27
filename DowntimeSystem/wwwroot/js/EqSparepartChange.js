//刷新 sparepart 表格
function refreshSparepart_Table(sparepartList) {
    $("#sparepart_table").bootstrapTable('destroy').bootstrapTable({
        data: sparepartList,
        dataType: 'json',
        width: '100%',
        detailView: true,
        columns: [{
            field: 'pn',
            title: 'PN',
        }, {
            field: 'desc',
            title: 'Description',
        }, {
            field: 'qty',
            title: 'QTY',
        }, {
            field: 'operation',
            title: '操作',
            formatter: function (value, row, index) {
                return ['<button type="button" class="btn bad text-light" id="delete"><i class="fa fa-trash"></i></button>']
            },
            events: {
                "click #delete": function (e, value, row, index) {
                    if (confirm("确定删除这个 Sparepart 更换记录")) {
                        sparepartList.splice(index, 1);
                        $("#sparepart_table").bootstrapTable("refreshOptions", { data: sparepartList });
                    }
                }
            }
        }],
        //注册加载子表的事件。注意下这里的三个参数！
        onExpandRow: function (index, row, $detail) {
            InitSubTable(index, row, $detail);
        },
    });
}
//初始化子表格(无线循环)
function InitSubTable(index, row, $detail) {
    let cur_table = $detail.html('<table class="tablewhite"></table>').find('table');
    let snlist = sparepartList.find(i => i.pn == row.pn);
    const tableData = snlist.list;
    $(cur_table).bootstrapTable({
        data: tableData,
        rowStyle: "rowStyleChild",
        columns: [{
            field: 'sn',
            title: 'SN',
        }],
    });
};


//SN信息存到一个list里面
function GenerateSparepartList(sn) {
    var pn = sn.split('.')[0]
    var item = sparepartList.find(i => i.pn == pn);
    if (item == undefined) {
        getData(BasicURL + "/api/SparepartDescription/QueryPN?pn=" + pn).then(res => {
            sparepartList.push(
                {
                    "pn": pn,
                    "desc": res.name,
                    "qty": 1,
                    "list": [{ "sn": sn }]
                });
            showSuccess("添加成功");
            refreshSparepart_Table(sparepartList);
        }).catch(err => {
            showError(err);
        })
    }
    else {
        if (item.list.some(j => j.sn == sn)) {
            showWarning("该备件已添加，请勿重复添加");
            return;
        }
        let i = { "sn": sn }
        item.list.push(i);
        item.qty += 1;
        showSuccess("添加成功");
        refreshSparepart_Table(sparepartList);
    }
}
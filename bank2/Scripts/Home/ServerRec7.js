var datatable;
var DataList;
$(document).ready(function () {

    datatable = $("#tab_dataInfo").DataTable({
        oLanguage: LanguagePage,
        height: 300,
        sScrollY: 420, //DataTables的高 
        "bPaginate": false, //是否显示（应用）分页器
        ajax: {
            url: hosturl + '/Home/Search_SercerRec2_1',
            type: 'POST',
            dataSrc: function (json) {
                DataList = isJsonString(json.data2);
                DataList = DataList.filter(function (item) {
                    if (item.LM_Balance == 0 && item.Balance == 0) {
                        return false;
                    } else {
                        return true;
                    }
                })
                return DataList;
            }
        },
        columns: [
            {//序号 0
                "data": null,
                "defaultContent": "",
                "orderable": false,
                "searchable": false,
                "visible": true,
                "width": "30px"
            },
            {// id 隐藏 1
                "data": "id",
                "defaultContent": '',
                "orderable": false,
                "searchable": false,
                "visible": false
            },
            {// 板块 2
                "data": "cmp.SType.TypeName",
                "defaultContent": '',
                "orderable": false,
            },
            {// 企业名称 3
                "data": "cmp.Company",
                "defaultContent": '',
                "orderable": false,
                "searchable": true,
            },
            {// 下月到期 4
                "data": "LM_Balance",
                "defaultContent": '',
                "orderable": false,
                "searchable": true,
                render: function (data, style, rows) {
                    return "<a href='/Home/ServerRec3?Company=" + rows.cmp.Company + "&Methods=1'>" + data.toFixed(2) + "</a>"
                }
            },
            {// 总余额 5
                "data": "Balance",
                "defaultContent": '',
                "orderable": false,
                "searchable": true,
                render: function (data, style, rows) {
                    return "<a href='/Home/ServerRec3?Company=" + rows.cmp.Company + "&Methods=0'>" + data.toFixed(2) + "</a>"
                }
            }
        ],
        drawCallback: function (settings) {
            var api = this.api();
            var rows = api.rows({ page: 'current' }).nodes();
            var Datalength = api.column(4, { page: 'current' }).data().length;
            var remain = [0, 0];

            api.column(3, { page: 'current' }).data().each(function (group, i) {
                var Money1 = api.column(4, { page: 'current' }).data()[i];
                var Money2 = api.column(5, { page: 'current' }).data()[i];
                remain = CalcRemain(remain, Money1, Money2);
                if (i == Datalength - 1) {
                    $(rows).eq(i).after(
                        "<tr style='font-size:110%;font-weight:600;'>" +
                        "<td colspan='3'>合计</td>" +
                        "<td><a href='/Home/ServerRec3?Methods=1'>" + remain[0].toFixed(2) + "</a></td>" +
                        "<td><a href='/Home/ServerRec3?Methods=2'>" + remain[1].toFixed(2) + "</a></td>"
                        + "</tr>"
                    )
                }
            })
        }
    });
    datatable.on('order.dt search.dt', function () {
        datatable.column(0, { search: 'applied', order: 'applied' }).nodes().each(function (cell, i) {
            cell.innerHTML = i + 1;
        });
    }).draw();
})
function CalcRemain(remain, Money1, Money2) {
    remain[0] += Money1;
    remain[1] += Money2;
    return remain;
}
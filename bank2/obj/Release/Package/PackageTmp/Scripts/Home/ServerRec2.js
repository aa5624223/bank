var datatable1;
var datatable2;
var DataList1;
var DataList2;
var dialog_print;
var DiaLog;
var UserAuth;
$(document).ready(function () {
    dialog_print = $("#dialog_print");
    //获得切换方式
    if (method == "1") {
        UserAuth = getUserFunction("我的桌面");
    } else {
        UserAuth = getUserFunction("还款提醒");
    }
    //var Tabbuttons = [];
    
    //Search_SercerRec2
    datatable1 = $("#tab_dataInfo1").DataTable({
        oLanguage: LanguagePage,
        //"bStateSave": true,
        height: 300,
        "bSort": false,
        sScrollY: 420, //DataTables的高 
        "bPaginate": false, //是否显示（应用）分页器
        ajax: {
            url: hosturl + '/Home/Search_SercerRec2_1',
            type: 'POST',
            dataSrc: function (json) {
                DataList1 = isJsonString(json.data2);
                DataList1 = DataList1.filter(function (item) {
                    if (item.LM_Balance == 0 && item.Balance == 0) {
                        return false;
                    } else {
                        return true;
                    }
                })
                return DataList1;
                //DataList2 = isJsonString();
            }
        },
        columns: [
            {//序号 0
                "data": null,
                "defaultContent": "",
                "orderable": false,
                "searchable": false,
                "visible": true,
                "width": "25px"
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
            {// 公司名称 3
                "data": "cmp.Company",
                "defaultContent": '',
                "orderable": false,
                "searchable": true,
            },
            {// 下个月到期 4
                "data": "LM_Balance",
                "defaultContent": '',
                "orderable": false,
                "searchable": true,
                render: function (data, style, rows) {
                    return "<a href='/Home/ServerRec3?Company=" + rows.cmp.Company + "&Methods=1'>" + data.toFixed(2) + "</a>"
                }
            },
            {//总余额 5
                "data": "Balance",
                "defaultContent": '',
                "orderable": false,
                "searchable": true,
                render: function (data,style,rows) {
                    return "<a href='/Home/ServerRec3?Company=" + rows.cmp.Company + "&Methods=0'>" + data.toFixed(2)+"</a>"
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
                if (i == Datalength-1) {
                    $(rows).eq(i).after(
                        "<tr style='font-size:110%;font-weight:600;'>" +
                            "<td colspan='3'>合计</td>" +
                        "<td><a href='/Home/ServerRec3?Methods=1'>" + remain[0].toFixed(2) + "</a></td>" +
                        "<td><a href='/Home/ServerRec3?Methods=2'>" + remain[1].toFixed(2) + "</a></td>"
                        +"</tr>"
                    )
                }
            })
        }
    });
    datatable2 = $("#tab_dataInfo2").DataTable({
        oLanguage: LanguagePage,
        //"bStateSave": true,
        height: 300,
        "bSort": false,
        sScrollY: 420, //DataTables的高 
        "bPaginate": false, //是否显示（应用）分页器
        ajax: {
            url: hosturl + '/Home/Search_SercerRec2_2',
            type: 'POST',
            dataSrc: function (json) {
                DataList2 = isJsonString(json.data1);
                DataList2 = DataList2.filter(function (item) {
                    if (item.LM_Balance == 0 && item.Balance == 0) {
                        return false;
                    } else {
                        return true;
                    }
                })
                return DataList2;
                //DataList2 = isJsonString();
            }
        },
        columns: [
            {//序号 0
                "data": null,
                "defaultContent": "",
                "orderable": false, 
                "searchable": false,
                "visible": true,
                "width": "25px"
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
            {// 公司名称 3
                "data": "cmp.Company",
                "defaultContent": '',
                "orderable": false,
                "searchable": true,
            },
            {// 下个月到期 4
                "data": "LM_Balance",
                "defaultContent": '',
                "orderable": false,
                "searchable": true,
                render: function (data, style, rows) {
                    return "<a href='/Home/ServerRec4?Company=" + rows.cmp.Company + "&Methods=1'>" + data.toFixed(2) + "</a>"
                }
            },
            {//总余额 5
                "data": "Balance",
                "defaultContent": '',
                "orderable": false,
                "searchable": true,
                render: function (data, style, rows) {
                    return "<a href='/Home/ServerRec4?Company=" + rows.cmp.Company + "&Methods=0'>" + data.toFixed(2) + "</a>"
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
                    //<a href='/Home/ServerRec4?Company=""&Methods=1'>" + remain[0] + "</a>
                    //<a href='/Home/ServerRec4?Company=""&Methods=0'>" + remain[1] + "</a>
                    $(rows).eq(i).after(
                        "<tr style='font-size:110%;font-weight:600;'>" +
                        "<td colspan='3'>合计</td>" +
                        "<td><a href='/Home/ServerRec4?Methods=1'>" + remain[0].toFixed(2) + "</a></td>" +
                        "<td><a href='/Home/ServerRec4?Methods=2'>" + remain[1].toFixed(2) + "</a></td>"
                        + "</tr>"
                    )
                }
            })
        }
    });
    datatable1.on('order.dt search.dt', function () {
        datatable1.column(0, { search: 'applied', order: 'applied' }).nodes().each(function (cell, i) {
            cell.innerHTML = i + 1;
        });
    }).draw();
    datatable2.on('order.dt search.dt', function () {
        datatable2.column(0, { search: 'applied', order: 'applied' }).nodes().each(function (cell, i) {
            cell.innerHTML = i + 1;
        });
    }).draw();
})
function CalcRemain(remain, Money1, Money2) {
    remain[0] += Money1;
    remain[1] += Money2;
    return remain;
}
function Dialog_Print() {
    if (UserAuth.indexOf("打印") < 0) {
        Alert1("提示", "没有打印的权限！", "btn-danger", "red");
        return;
    }
    dialog_print.css("display", "block");
    vue_print = new Vue({
        el:'#printArea',
        data: {
            PrintList1: datatable1.rows({ page: 'current' }).data().toArray(),
            PrintList2: datatable2.rows({ page: 'current' }).data().toArray(),
            Credit_Total_1: 0,//当月
            Credit_Total_2: 0,//剩余
            Acceptance_Total_1: 0,//当月
            Acceptance_Total_2: 0,//剩余
            Month_Now:0,
        },
        methods: {

        },
        mounted: function () {
            var _this = this;
            this.PrintList1.forEach(function (item) {
                _this.Credit_Total_1 += item.LM_Balance;
                _this.Credit_Total_2 += item.Balance;
            })
            this.PrintList2.forEach(function (item) {
                _this.Acceptance_Total_1 += item.LM_Balance;
                _this.Acceptance_Total_2 += item.Balance;
            })
            this.Month_Now = new Date().getMonth() + 1;
        }
    });
    DiaLog = $.dialog({
        title: '当月到期提醒-打印预览',
        closeIcon: true,
        columnClass: '',
        content: dialog_print,
    })
}
function print() {
    setTimeout(function () {
        var printHtml = document.getElementById('printArea').innerHTML;
        //var oldstr = document.body.innerHTML;
        var oPop = window.open('', 'oPop');
        oPop.document.write(printHtml);
        oPop.print();
        oPop.close();
    }, 1000);
}


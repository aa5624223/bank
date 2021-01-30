var datatable;
var DataList;
var dialog_print;
var DiaLog;
var CompanyList = [];
var vue_Search;
var vue_print;
$(document).ready(function () {
    dialog_print = $("#dialog_print");
    var UserAuth = getUserFunction("授信记录"); 
    var Tabbuttons = [];
    if (UserAuth.indexOf("打印") >= 0) {
        Tabbuttons.push({
            //extend:'print',
            text: '打印',
            action: function () {
                Dialog_Print();
            }
        });
    }
    //初始化搜索框
    init_Search();
    datatable = $("#tab_dataInfo").DataTable({
        oLanguage: LanguagePage,
        //"bStateSave": true,
        height: 300,
        sScrollY: 520, //DataTables的高 
        "bPaginate": false, //是否显示（应用）分页器
        ajax: {
            url: hosturl + '/Home/Search_CreditInfo',
            type: 'POST',
            dataSrc: function (json) {
                DataList = isJsonString(json.data);
                if (DataList == false) {
                    return [];
                }
                for (var j = 0; j < DataList.length;j++) {
                    var flg = true;
                    for (var i = 0; i < CompanyList.length; i++) {
                        if (CompanyList[i].Company == DataList[j].Company) {
                            flg = false;
                            var flg2 = true;

                            if (flg2) {
                                CompanyList[i].BankList.push(DataList[j].Bank);
                            }
                        }

                    }
                    if (flg) {
                        CompanyList.push({ Company: DataList[j].Company, BankList: [DataList[j].Bank] })
                    }
                }
                CompanyList.forEach(function (item) {
                    item.BankList.unshift("全部");
                })
                CompanyList.unshift({ Company: "全部", BankList: ["全部"] });
                vue_Search._data.CompanyList = CompanyList;
                vue_Search._data.CmpIdx = 0;
                vue_Search._data.BankList = CompanyList[0].BankList;
                vue_Search._data.BankId = 0;
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
            {// 公司名称 3
                "data": "Company",
                "defaultContent": '',
                "orderable": false,
                "searchable": true,
            },
            {// 银行名称 4
                "data": "Bank",
                "defaultContent": '',
                "orderable": false,
                "searchable": true,
            },
            {// 信用额度 5
                "data": "Credit",
                "defaultContent": '',
                "orderable": false,
            },
            {//贷款额度 6 
                "data": "Loans",
                "defaultContent": '',
                "orderable": false,
            },
            {//承兑额度 7
                "data": "Acceptance",
                "defaultContent": '',
                "orderable": false,
            },
            {//利率 8
                "data": "Rates",
                "defaultContent": '',
                "orderable": false,
            },
            {//未还总额 9
                "data": "Arrears",
                "defaultContent": '',
                "orderable": false,
            }
        ],
        dom: '<"bottom"Bfrtip>',
        buttons: Tabbuttons,
        "drawCallback": function (settings) {
            var api = this.api();
            var rows = api.rows({ page: 'current' }).nodes();
            //[信用额度,贷款额度,承兑额度,未还总额]
            var remain = [0, 0, 0, 0];
            var Count = [0, 0, 0, 0];
            var Datalength = api.column(3, { page: 'current' }).data().length;
            var last = api.column(3, { page: 'current' }).data()[0];
            api.column(3, { page: 'current' }).data().each(function (group, i) {
                var Money1 = api.column(5, { page: 'current' }).data()[i];//信用额度
                var Money2 = api.column(6, { page: 'current' }).data()[i];//贷款额度
                var Money3 = api.column(7, { page: 'current' }).data()[i];//承兑额度
                var Money4 = api.column(9, { page: 'current' }).data()[i];//未还总额
                if (last != group) {
                    //$(rows).eq(i).before(
                    //    '<tr  style="font-size:140%;font-weight:600;text-align:center">' +
                    //    '<td colspan="4">企业:'+last+' 合计</td>' +
                    //    '<td colspan="1">' + remain[0] + '</td>' +
                    //    '<td colspan="1">' + remain[1] + '</td>' +
                    //    '<td colspan="1">' + remain[2] + '</td>' +
                    //    '<td colspan="1"></td>' +
                    //    '<td colspan="1">' + remain[3] + '</td>' +
                    //    '</tr>'
                    //);
                    //last = group;
                    //remain = [0, 0, 0, 0];
                }
                if (i == Datalength - 1) {
                    remain = CalcRemain(remain, Money1, Money2, Money3, Money4);
                    Count = CalcRemain(Count, Money1, Money2, Money3, Money4);
                    $(rows).eq(i).after(
                        '<tr  style="font-size:140%;font-weight:600;text-align:center">' +
                        '<td colspan="4">合计</td>' +
                        '<td colspan="1">' + Count[0] + '</td>' +
                        '<td colspan="1">' + Count[1] + '</td>' +
                        '<td colspan="1">' + Count[2] + '</td>' +
                        '<td colspan="1"></td>' +
                        '<td colspan="1">' + Count[3] + '</td>' +
                        '</tr>'
                    );
                    //$(rows).eq(i).after(
                    //    '<tr style="font-size:140%;font-weight:600;text-align:center">' +
                    //    '<td colspan="4">企业:' + last + ' 合计</td>' +
                    //    '<td colspan="1">' + remain[0] + '</td>' +
                    //    '<td colspan="1">' + remain[1] + '</td>' +
                    //    '<td colspan="1">' + remain[2] + '</td>' +
                    //    '<td colspan="1"></td>' +
                    //    '<td colspan="1">' + remain[3] + '</td>' +
                    //    '</tr>'
                    //);
                }
                remain = CalcRemain(remain, Money1, Money2, Money3, Money4);
                Count = CalcRemain(Count, Money1, Money2, Money3, Money4);
            });
        }
    });
    datatable.on('order.dt search.dt', function () {
        datatable.column(0, { search: 'applied', order: 'applied' }).nodes().each(function (cell, i) {
            cell.innerHTML = i + 1;
        });
    }).draw();
    $('#tab_dataInfo tbody').on('mouseenter', 'td', function () {
        if (datatable.cell(this).index() != undefined) {
            var colIdx = datatable.cell(this).index().column;
            $(datatable.cells().nodes()).removeClass('highlight');
            $(datatable.column(colIdx).nodes()).addClass('highlight');
        }
    });
});
//计算总计
function CalcRemain(remain,Money1,Money2,Money3,Money4) {
    remain[0] += Money1;
    remain[1] += Money2;
    remain[2] += Money3;
    remain[3] += Money4;
    return remain;
}
/* 表格搜索功能 */
$.fn.dataTable.ext.search.push(
    function (settings, data, dataIndex) {
        var Company = datatable.data()[dataIndex].Company;
        var Bank = datatable.data()[dataIndex].Bank;
        //公司 
        var Search_Company = $("#Search_Company").find("option:selected").text();
        //银行
        var Search_Bank = $("#Search_Bank").find("option:selected").text();
        var flg = true;
        if (Company != Search_Company && Search_Company != "" && Search_Company!="全部") {
            flg = false;
        }
        if (Bank != Search_Bank && Search_Bank != "" && Search_Bank!="全部") {
            flg = false;
        }
        return flg;
    }
)

/* 初始化搜索框 */
function init_Search() {
    vue_Search = new Vue({
        el: '#table_Search',
        data: {
            CompanyList: [],
            CmpIdx: 1,
            BankList: [],
            BankIdx: 1,
        },
        watch: {
            CmpIdx: function () {
                this.BankList = this.CompanyList[this.CmpIdx].BankList;
                this.BankIdx = 0;
                $("#Search_Bank").val(0);
                //切换表
                setTimeout(function () {
                    datatable.draw();
                }, 200)
            },
            BankIdx: function () {
                //切换表
                setTimeout(function () {
                    datatable.draw();
                }, 200)
            }
        }
    })
}

function Dialog_Print() {
    dialog_print.css("display", "block");
    //datatable.rows({page:'current'}).data().toArray()//当前打印的数据
    DiaLog = $.dialog({
        title: '授信记录-打印预览',
        closeIcon: true,
        columnClass: 'col-lg-11',
        content: dialog_print,
        onOpen: function () {
            var vue_print = new Vue({
                el: '#dialog_print',
                data: {
                    PrintList: datatable.rows({ page: 'current' }).data().toArray(),
                    Credit_Total: 0,
                    Acceptance_Total: 0,
                    Loans_Total: 0,
                    Arrears_Total: 0
                },
                methods: {

                },
                mounted: function () {
                    var _this = this;
                    this.PrintList.forEach(function (item) {
                        _this.Credit_Total += item.Credit
                        _this.Acceptance_Total += item.Acceptance;
                        _this.Loans_Total += item.Loans;
                        _this.Arrears_Total += item.Arrears;
                    })
                    _this.Credit_Total = _this.Credit_Total.toFixed(2);
                    _this.Acceptance_Total = _this.Acceptance_Total.toFixed(2);
                    _this.Loans_Total = _this.Loans_Total.toFixed(2);
                    _this.Arrears_Total = _this.Arrears_Total.toFixed(2);
                }
            })
        }
    });
}
//打印
function print() {
    setTimeout(function () {
        var printHtml = document.getElementById('printArea').innerHTML;
        //var oldstr = document.body.innerHTML;
        var oPop = window.open('', 'oPop');
        oPop.document.write(printHtml);
        oPop.print();
        oPop.close();
    }, 800)
}
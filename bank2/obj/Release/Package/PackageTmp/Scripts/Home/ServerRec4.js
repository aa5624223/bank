var Vue_Search;
var datatable;
var datatable2;
var dialog_print;
var datatableMode = "datatable";//记录当前在使用哪个表格的数据
var SearchFlg = false;
var UserAuth;
var firstFlg = true;
$(document).ready(function () {
    dialog_print = $("#dialog_print")
    UserAuth = getUserFunction("承兑记录");
    //汇总表格
    datatable = $("#tab_dataInfo").DataTable({
        oLanguage: LanguagePage,
        height: 300,
        sScrollY: 520, //DataTables的高 
        "bPaginate": false, //是否显示（应用）分页器
        columns: [
            {// 序号 0
                "data": null,
                "defaultContent": "",
                "orderable": false,
                "searchable": false,
                "visible": true,
                "width": "30px"
            },
            {// 板块/公司名 1
                "data": "cmp.SType.TypeName",
                "defaultContent": '',
                "orderable": false,
                "searchable": true,
                render: function (data, style, rows) {
                    return data;
                }
            },
            {// 借款总金额 2
                "data": "Sum_LoanAmount",
                "defaultContent": '',
                "orderable": false,
                "searchable": true,
                render: function (data) {
                    return data.toFixed(2)
                }
            },
            {//保证金总额
                "data": "Sum_Margin",
                "defaultContent": '',
                "orderable": false,
                "searchable": true,
                render: function (data) {
                    return data.toFixed(2)
                }
            },
            {// 已还总金额 3
                "data": "Sum_Repayed",
                "defaultContent": '',
                "orderable": false,
                "searchable": true,
                render: function (data) {
                    return data.toFixed(2)
                }
            },
            {// 余额 4 
                "data": "Sum_Balance",
                "defaultContent": '',
                "orderable": false,
                "searchable": true,
                render: function (data) {
                    return data.toFixed(2)
                }
            },
        ],
        drawCallback: function (settings) {
            var api = this.api();
            var rows = api.rows({ page: 'current' }).nodes();
            var Datalength = api.column(2, { page: 'current' }).data().length;
            var remain = [0, 0, 0, 0];
            api.column(2, { page: 'current' }).data().each(function (group, i) {
                var Money1 = group;
                var Money2 = api.column(3, { page: 'current' }).data()[i];
                var Money3 = api.column(4, { page: 'current' }).data()[i];
                var Money4 = api.column(5, { page: 'current' }).data()[i];

                CalcRemain(remain, Money1, Money2, Money3, Money4);
                if (i == Datalength - 1) {
                    $(rows).eq(i).after(
                        "<tr>" +
                        "<td colspan='2' style='font-weight:600'>合计</td>" +
                        "<td>" + remain[0].toFixed(2) + "</td>" +
                        "<td>" + remain[1].toFixed(2) + "</td>" +
                        "<td>" + remain[2].toFixed(2) + "</td>" +
                        "<td>" + remain[3].toFixed(2) + "</td>" +
                        +"</tr>"
                    );
                }
            })
        }
    })
    datatable.on('order.dt search.dt', function () {
        datatable.column(0, { search: 'applied', order: 'applied' }).nodes().each(function (cell, i) {
            cell.innerHTML = i + 1;
        });
    }).draw();
    //明细表格
    datatable2 = $("#tab_dataInfo2").DataTable({
        oLanguage: LanguagePage,
        height: 300,
        bAutoWidth: true,
        sScrollY: 520, //DataTables的高 
        "bPaginate": false, //是否显示（应用）分页器
        columns: [
            {// 序号 0
                "data": null,
                "defaultContent": "",
                "orderable": false,
                "searchable": false,
                "visible": true,
                "width": "30px"
            },
            {// 类型 1
                "data": "Type",
                "defaultContent": '',
                "orderable": false,
                "searchable": true,
                render: function (data) {
                    if (data == "贷") {
                        return "<font style='color:red'>" + data + "</font>"
                    } else {
                        return data;
                    }
                }
            },
            {// 摘要 2
                "data": "Abstract",
                "defaultContent": '',
                "orderable": false,
                "searchable": true,
            },
            {// 业务日期 3
                "data": "OccDate",
                "defaultContent": '',
                "orderable": false,
                "searchable": true,
                render: function (data) {
                    var date = new Date(data);
                    return date.getFullYear() + "-" + (date.getMonth() + 1) + "-" + date.getDate();
                }
            },
            {// 到期日期 4
                "data": "EndDate",
                "defaultContent": '',
                "orderable": false,
                "searchable": true,
                render: function (data) {
                    var date = new Date(data);
                    return date.getFullYear() + "-" + (date.getMonth() + 1) + "-" + date.getDate();
                }
            },
            {// 承兑金额 5
                "data": "LoanAmount",
                "defaultContent": '',
                "orderable": false,
                "searchable": true,
                render: function (data) {
                    return data.toFixed(2)
                }
            },
            {// 还款金额 6
                "data": "Repayed",
                "defaultContent": '',
                "orderable": false,
                "searchable": false,
                render: function (data) {
                    return data.toFixed(2)
                }
            },
            {//保证金 7
                "data": "Margin",
                "defaultContent": '',
                "orderable": false,
                "searchable": false,
                render: function (data) {
                    return data.toFixed(2)
                }
            },
            {// 余额 合并计算 8
                "data": "Balance",
                "defaultContent": '',
                "orderable": false,
                "searchable": false,
                render: function (data) {
                    return data.toFixed(2)
                }
            },
            
            {//状态 9
                "data": "Flag",
                "defaultContent": '',
                "orderable": false,
                "searchable": false,
                render: function (data) {
                    if (data == "未清") {
                        return "<font style='color:red'>" + data + "</font>"
                    } else {
                        return data;
                    }
                }
            },
            {// 利率 10
                "data": "Rates",
                "defaultContent": '',
                "orderable": false,
                "searchable": false,
            },
            {// 企业名称 11
                "data": "Company",
                "defaultContent": '',
                "orderable": false,
                "searchable": true,
                "visible": true,
            },
            {// 银行 12
                "data": "Bank",
                "defaultContent": '',
                "orderable": false,
                "searchable": true,
                "visible": true
            },
            {// 创建日期 13
                "data": "BuildDate",
                "defaultContent": '',
                "orderable": false,
                "searchable": false,
                render: function (data) {
                    var date = new Date(data);
                    return date.getFullYear() + "-" + (date.getMonth() + 1) + "-" + date.getDate();
                }
            },
            {// 备注信息 14
                "data": "Remarks",
                "defaultContent": '',
                "orderable": false,
                "searchable": false,
            },
        ],
        drawCallback: function (settings) {
            var api = this.api();
            var rows = api.rows({ page: 'current' }).nodes();
            var Datalength = api.column(11, { page: 'current' }).data().length;
            var last = "企业:"+api.column(11, { page: 'current' }).data()[0] + ',银行:' + api.column(12, { page: 'current' }).data()[0];
            //[贷款金额 5,已还金额 6，保证金7,余额8,]
            var remain = [0, 0, 0, 0];
            //11 企业 12 银行
            api.column(11, { page: 'current' }).data().each(function (group, i) {
                var Money1 = api.column(5, { page: 'current' }).data()[i];
                var Money2 = api.column(6, { page: 'current' }).data()[i];
                var Money3 = api.column(7, { page: 'current' }).data()[i];
                var Money4 = api.column(8, { page: 'current' }).data()[i];
                var Bank = api.column(12, { page: 'current' }).data()[i];
                var newStr = "企业:" + group + ",银行:" + Bank
                group = newStr
                if (last != group) {
                    $(rows).eq(i).before(
                        '<tr  style="font-size:140%;font-weight:600;text-align:left">' +
                        '<td colspan="1"></td>' +
                        '<td colspan="5">' + last + '</td>' +
                        '<td style="text-align:center">合计</td>' +
                        '<td colspan="8">承兑金额:' + remain[0].toFixed(2) + '，已还：' + remain[1].toFixed(2) + '，保证金:' + remain[2].toFixed(2) + '，待还：' + remain[3].toFixed(2) + '</td>' +
                        '</tr>'
                    );
                    last = group;
                    remain = [0, 0, 0, 0];
                }
                if (i == Datalength - 1) {
                    if (Datalength != 1) {
                        remain = CalcRemain(remain, Money1, Money2, Money3, Money4);
                    }
                    $(rows).eq(i).after(
                        '<tr  style="font-size:140%;font-weight:600;text-align:left">' +
                        '<td colspan="1"></td>' +
                        '<td colspan="5">' + last + '</td>' +
                        '<td style="text-align:center">合计</td>' +
                        '<td colspan="8">承兑金额:' + remain[0].toFixed(2) + '，已还：' + remain[1].toFixed(2) + '，保证金:' + remain[2].toFixed(2) + '，待还：' + remain[3].toFixed(2) + '</td>' +
                        '</tr>'
                    );
                }
                remain = CalcRemain(remain, Money1, Money2, Money3, Money4);
            });
            

        }
    })
    datatable2.on('order.dt search.dt', function () {
        datatable2.column(0, { search: 'applied', order: 'applied' }).nodes().each(function (cell, i) {
            cell.innerHTML = i + 1;
        });
    }).draw();
    $('#tab_dataInfo2 tbody').on('mouseenter', 'td', function () {
        if (datatable2.cell(this).index() != undefined) {
            var colIdx = datatable2.cell(this).index().column;
            $(datatable2.cells().nodes()).removeClass('highlight');
            $(datatable2.column(colIdx).nodes()).addClass('highlight');
        }
    });
    Search_init();
})

function Dialog_Print() {
    if (UserAuth.indexOf("打印") < 0) {
        Alert1("提示", "没有打印的权限！", "btn-danger", "red");
        return;
    }

    dialog_print.css("display", "block");

    //
    DiaLog = $.dialog({
        title: '承兑记录-打印预览',
        closeIcon: true,
        columnClass: 'col-lg-11',
        content: dialog_print,
        onClose: function () {
            // before the modal is hidden.
            //dialog_print = $("#dialog_print");
            //alert('onClose');
        },
        onOpen: function () {
            if (datatableMode == "datatable") {//"datatable2";
                $("#Print_datatable1").css('display', 'block');
                $("#Print_datatable1").css('display', '');
                $("#Print_datatable2").css('display', 'none');
            } else {
                $("#Print_datatable1").css('display', 'none');
                $("#Print_datatable2").css('display', 'block');
                $("#Print_datatable2").css('display', '');
            }
            var vue_print = new Vue({
                el: "#dialog_print",
                data: {
                    PrintList: [],
                    PrintList2: [],
                    Btn_warm: '打印布局请选择纵向',
                    Total1: 0,//借款总额 总计
                    Total2: 0,//已还总额 总计
                    Total3: 0,//余额总计
                    Total4: 0,//保证金总金额
                },
                mounted: function () {
                    var _this = this;
                    if (datatableMode == "datatable") {
                        this.PrintList = datatable.rows({ page: 'current' }).data().toArray();
                    } else {
                        this.PrintList2 = datatable2.rows({ page: 'current' }).data().toArray();
                    }
                    if (datatableMode == "datatable") {
                        $("#Btn_warm").text("打印布局请选择纵向");
                        _this.PrintList.forEach(function (item) {
                            _this.Total1 += item.Sum_LoanAmount
                            _this.Total2 += item.Sum_Repayed;
                            _this.Total3 += item.Sum_Balance;
                            _this.Total4 += item.Sum_Margin;
                        })
                    } else {
                        $("#Btn_warm").text("打印布局请选择横向");
                        _this.PrintList2.forEach(function (item) {
                            item.OccDate = item.OccDate.substring(0, 10);
                            item.EndDate = item.EndDate.substring(0, 10);

                            item.LoanAmount = item.LoanAmount.toFixed(2);
                            item.RepayAmount = item.RepayAmount.toFixed(2);
                            item.Balance = item.Balance.toFixed(2);
                            item.Sum_Margin = item.Sum_Margin.toFixed(2);

                            _this.Total1 += item.LoanAmount;
                            _this.Total2 += item.RepayAmount;
                            _this.Total3 += item.Balance;
                            _this.Total4 += item.Sum_Margin;
                        })
                    }
                    _this.Total1 = _this.Total1.toFixed(2);
                    _this.Total2 = _this.Total2.toFixed(2);
                    _this.Total3 = _this.Total3.toFixed(2);
                    _this.Total4 = _this.Total4.toFixed(2);
                }
            })

        },
    });
}


function CalcRemain(remain, Money1, Money2, Money3,Money4) {
    remain[0] += Money1;
    remain[1] += Money2;
    remain[2] += Money3;
    remain[3] += Money4;
    return remain;
}
function Search_init() {
    Vue_Search = new Vue({
        el: '#table_Search',
        data: {
            TypeList: [],
            CompanyList: [],
            BankList: [],
            TypeName: '',
            Company: '',
            Bank: '',
            DateModel: 0,
            Model: 0,
            Flag: '未清',
            Ser_Begin: '',
            Ser_End: '',
            End_Begin: '',
            End_End: ''
        },
        methods: {
            btn_Search: function () {
                if (SearchFlg) {
                    return;
                }
                var _this = this;
                var formData = new FormData();
                //板块id
                formData.append("TypeName", this.TypeName);
                //公司名
                formData.append("Company", this.Company);
                //银行名
                formData.append("Bank", this.Bank);
                //凭证状态
                formData.append("Flag", this.Flag);
                //业务日期 开始
                formData.append("Ser_Begin", this.Ser_Begin);
                formData.append("Ser_End", this.Ser_End);
                //业务日期模式
                //formData.append("DateModel", this.DateModel);//0之前 1当月
                //查询模式
                formData.append("Model", this.Model);//0 板块汇总 1 公司汇总 2 公司明细
                //业务日期 结束
                //formData.append("Ser_End", this.Ser_End);
                //到期日期 开始
                formData.append("End_Begin", this.End_Begin);
                //到期日期 结束
                formData.append("End_End", this.End_End);
                $.ajax({
                    type: 'POST',
                    url: hosturl + '/Home/Search_ServerRec4',
                    data: formData,
                    dataType: 'JSON',
                    cache: false,
                    processData: false,
                    contentType: false,
                    beforeSend: function () {
                        SearchFlg = true;
                    },
                    complete: function () {
                        SearchFlg = false;
                    },
                    success: function (res) {
                        if (res.msg == "OK") {
                            //添加数据到表格上
                            var data = isJsonString(res.data);
                            if (_this.Model == "2") {
                                datatableMode = "datatable2";
                                $("#tab_dataInfo_wrapper").css("display", "none");
                                $("#tab_dataInfo2_wrapper").css("display", "block");
                                datatable2.clear();
                                datatable2.rows.add(data).draw();
                            } else {
                                datatableMode = "datatable";
                                $("#tab_dataInfo_wrapper").css("display", "block");
                                $("#tab_dataInfo2_wrapper").css("display", "none");
                                datatable.clear();
                                datatable.rows.add(data).draw();
                            }

                        } else if (res.msg == "refuse") {
                            Alert1("提示", "登录超时", "btn-danger", "red");
                        } else if (res.msg == "error") {
                            Alert1("提示", "登录超时", "btn-danger", "red");
                        }
                    },
                    error: function () {
                        Alert1("提示", "服务器无响应", "btn-danger", "red");
                    }
                });
            },
            Search_DrawList: function () {
                //Search_Type_Company_Bank 查询出联动下拉框
                var _this = this;
                var formData = new FormData();
                formData.append("Type", "AcceptancesInfo");
                $.ajax({
                    type: 'POST',
                    url: hosturl + '/Home/Search_Type_Company_Bank',
                    data: formData,
                    dataType: 'JSON',
                    cache: false,
                    processData: false,
                    contentType: false,
                    beforeSend: function () {
                    },
                    complete: function () {
                    },
                    success: function (res) {
                        if (res.msg == "OK") {
                            _this.TypeList = isJsonString(res.data);
                            var TypeIdx = 0;
                            var ComIdx = 0;
                            var FindFlg = false;
                            if (Company != '') {
                                _this.TypeList.forEach(function (Typeitem) {
                                    Typeitem.CompanyList.forEach(function (ComItem) {
                                        if (ComItem.Company == Company && !FindFlg) {
                                            FindFlg = true;//找到了
                                        }
                                        if (!FindFlg) {//没找到
                                            ComIdx++;
                                        }
                                    })
                                    if (!FindFlg) {
                                        ComIdx = 0;
                                        TypeIdx++;
                                    }
                                })
                                _this.TypeName = _this.TypeList[TypeIdx].TypeName;
                                //_this.TypeList = _this.TypeList[TypeIdx];
                                _this.Company = Company
                                _this.CompanyList = _this.TypeList[TypeIdx].CompanyList;
                                _this.BankList = _this.CompanyList[ComIdx].BankList;
                            } else {
                                _this.CompanyList = _this.TypeList[0].CompanyList;
                                _this.BankList = _this.TypeList[0].CompanyList[0].BankList;
                            }
                        }
                    },
                    error: function () {
                        Alert1("提示", "服务器无响应", "btn-danger", "red");
                    }
                })
            }
        },
        mounted: function () {
            this.Company = Company;
            if (Company != '') {
                this.Model = 2;
            }

            var now = new Date();
            //格式化日，如果小于9，前面补0
            var day = ("0" + now.getDate()).slice(-2);
            //格式化月，如果小于9，前面补0
            var month = ("0" + (now.getMonth() + 1)).slice(-2);
            var lastday = new Date(now.getFullYear(), now.getMonth() + 1, 0);
            var today;
            //拼装完整日期格式
            if (Company != '' && Methods == "0") {
                //today = "2000-" + (month) + "-" + ("01");
                //lastday = lastday.getFullYear() + "-" + (month) + "-" + (fix(lastday.getDate(), 2));
                today = "";
                lastday = "";
            } else if (Methods == "1") {
                today = (now.getFullYear()-1) + "-" + (month) + "-" + (fix(now.getDate(), 2));
                var year = lastday.getFullYear();
                if (month == 12) {
                    year += 1;
                }
                lastday = year + "-" + fix((month + 1) % 12, 2) + "-" + (fix(lastday.getDate(), 2));
            } else if (Methods == 2) {
                today = "";
                lastday = "";
            }else {
                //today = now.getFullYear() + "-" + fix(month,2) + "-" + ("01");
                //lastday = lastday.getFullYear() + "-" + fix(month,2) + "-" + (fix(lastday.getDate(), 2));
                today = "";
                lastday = "";
            }

            
            //this.Ser_Begin = today;
            //this.Ser_End = lastday;

            this.End_Begin = today;
            this.End_End = lastday;

            this.btn_Search();
            this.Search_DrawList();
        },
        watch: {
            TypeName: function () {
                var _this = this;
                this.TypeList.forEach(function (item) {
                    if (item.TypeName == _this.TypeName) {
                        _this.CompanyList = item.CompanyList;
                        
                    }
                });
                if (firstFlg) {
                    firstFlg = false;
                    return;
                }
                this.Company = "";
                this.Bank = "";
            },
            Company: function () {
                var _this = this;
                this.CompanyList.forEach(function (item) {
                    if (item.Company == _this.Company) {
                        _this.BankList = item.BankList;
                        _this.Bank = "";
                    }
                });
            }
        }
    })

}
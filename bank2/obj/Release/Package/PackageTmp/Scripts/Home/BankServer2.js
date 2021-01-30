var datatable;
var DataList;
var EditFlg = false;
var dialog_add;
var dialog_edit;
var dialog_addType;
var subTable;
var vue__add_repay;
var vue_Search;
//[[公司名,银行名],[]]
var CompanyList = [];
var firstFlg = true;//控制删除时不刷新下拉列表
$(document).ready(function () {
    dialog_add_loan = $("#dialog_add_loan");
    dialog_add_repay = $("#dialog_add_repay");
    dialog_edit = $("#dialog_edit");
    var UserAuth = getUserFunction("承兑业务"); 
    var Tabbuttons = [{
        extend: 'excelHtml5',
        autoFilter: true,
        messageTop: '承兑业务',
        title: '承兑业务',
        text: '导出本页',// 显示文字
        exportOptions: {
            columns: ':not(.td-oprate)'
        }
    },
        {
            extend: 'copy',
            text: '复制表格',// 显示文字
            exportOptions: {
                columns: ':not(.td-oprate)'
            }
        }];
    if (UserAuth.indexOf("修改") >= 0) {
        Tabbuttons.push({
            text: '新增承兑',
            action: function () {
                Dialog_Add_Loan();
            }
        });
        Tabbuttons.push({
            text: '承兑还款',
            action: function () {
                Dialog_Add_Repay();
            }
        });
        
    }
    //初始化搜索框
    init_Search();
    datatable = $("#tab_dataInfo").DataTable({
        oLanguage: LanguagePage,
        //"bStateSave": true,
        height: 290,
        sScrollY: 510, //DataTables的高 
        "bPaginate": false, //是否显示（应用）分页器
        ajax: {
            url: hosturl + '/Home/Search_BankServer2',
            type: 'POST',
            'dataSrc': function (json) {
                DataList = isJsonString(json.data);
                DataList.forEach(item => {
                    var flg = true;
                    for (var i = 0; i < CompanyList.length; i++) {
                        if (CompanyList[i].Company == item.Company) {
                            flg = false;
                            var flg2 = true;
                            for (var j = 0; j < CompanyList[i].BankList.length; j++) {
                                if (CompanyList[i].BankList[j] == item.Bank) {
                                    flg2 = false
                                }
                            }
                            if (flg2) {
                                CompanyList[i].BankList.push(item.Bank);
                            }
                        }
                    }

                    if (flg) {
                        CompanyList.push({ Company: item.Company, TypeName: item.cmp.SType.TypeName, TypeId: item.cmp.SType.id, BankList: [item.Bank] })
                    }
                })
                //CompanyList.forEach(function (item) {
                //    item.BankList.unshift("全部");
                //})
                //CompanyList.unshift({ Company: "全部", BankList: ["全部"] });

                if (firstFlg) {
                    firstFlg = false;
                    vue_Search._data.CompanyList = CompanyList;
                    vue_Search._data.CompanyList2 = CompanyList.filter(function (item) {
                        if (item.TypeId == vue_Search._data.TypeIdx) {
                            return true;
                        } else {
                            return false;
                        }
                    })
                    vue_Search._data.CmpIdx = 0;
                    vue_Search._data.BankList = CompanyList[0].BankList;
                }
               
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
                sClass: "td-oprate",
                "width": "30px"
            },
            {// id 隐藏 1
                "data": "id",
                "defaultContent": '',
                "orderable": false,
                "searchable": false,
                sClass: "td-oprate",
                "visible": false
            },
            {// 类型 2
                "data": "Type",
                "defaultContent": '',
                "orderable": false,
                "searchable": true,
            },
            //{// 还款 3
            //    "data": "Type",
            //    "defaultContent": '',
            //    "orderable": true,
            //    "searchable": false,

            //},
            {// 摘要 3
                "data": "Abstract",
                "defaultContent": '',
                "orderable": false,
                "searchable": true,
            },
            {// 业务日期 4
                "data": "OccDate",
                "defaultContent": '',
                "orderable": false,
                "searchable": false,
            },
            {// 到期日期 5
                "data": "EndDate",
                "defaultContent": '',
                "orderable": false,
                "searchable": false,
            },
            {// 承兑金额 6
                "data": "LoanAmount",
                "defaultContent": '',
                "orderable": false,
                "searchable": false,
                render: function (data) {
                    return parseFloat(data).toFixed(2);
                }
            },
            {// 对应承兑 7 已还金额    *****
                "data": "Repayed",
                "defaultContent": '',
                "orderable": false,
                "searchable": false,
                render: function (data) {
                    return parseFloat(data).toFixed(2);
                }
            },
            {// 本次 还款金额 8
                "data": "RepayAmount",
                "defaultContent": '',
                "orderable": false,
                "searchable": false,
                render: function (data) {   
                    return parseFloat(data).toFixed(2);
                }
            },
            {// 余额 合并计算 9
                "data": "Balance",
                "defaultContent": '',
                "orderable": false,
                "searchable": false,
                render: function (data) {
                    return parseFloat(data).toFixed(2);
                }
            },
            {// 业务状态 10
                "data": "Flag",
                "defaultContent": '',
                "orderable": false,
                "searchable": true,
            },
            {// 保证金 11
                "data": "Margin",
                "defaultContent": '',
                "orderable": false,
                "searchable": false,
                render: function (data) {
                    return parseFloat(data).toFixed(2);
                }
            },
            {// 企业名称 12 CompanyAndBnak
                "data": "CompanyAndBnak",
                "defaultContent": '',
                "orderable": false,
                "searchable": true,
                "visible": false,
                sClass: "td-oprate",
            },
            {// 企业名称 13 
                "data": "Company",
                "defaultContent": '',
                "orderable": false,
                "searchable": true,
                "visible": false,
            },
            {// 银行 14
                "data": "Bank",
                "defaultContent": '',
                "orderable": false,
                "searchable": true,
                "visible": false
            },
            {// 创建日期 15
                "data": "BuildDate",
                "defaultContent": '',
                "orderable": false,
                "searchable": false,
            },
            {// 备注信息 16
                "data": "Remarks",
                sWidth: "11%",
                "defaultContent": '',
                "orderable": false,
                "searchable": false,
            },
            {// 操作员 17
                "data": "User.NickName",
                "defaultContent": '',
                "orderable": false,
                "searchable": true,
            },
            {//操作 18
                "data": null,
                "defaultContent": '',
                "orderable": false,
                sClass: "td-oprate",
                "searchable": false,
            }
        ],
        columnDefs: [
            {
                "targets": 2,
                "render": function (data, type, row) {
                    if (data == '贷') {
                        return '<b style="color:red">' + data + '</b>'
                    } else {
                        return data;
                    }

                }
            },
            {
                "targets": 4,
                "render": function (data, type, row) {
                    var date = new Date(data);
                    return date.getFullYear() + "-" + (date.getMonth() + 1) + "-" + date.getDate();
                }
            },
            {
                "targets": 5,
                "render": function (data, type, row) {
                    var date = new Date(data);
                    var str = date.getFullYear() + "-" + (date.getMonth() + 1) + "-" + date.getDate();
                    if (str == '1-1-1') {
                        return '';
                    } else {
                        return str;
                    }
                }
            },
            {
                "targets": 10,
                "render": function (data, type, row) {
                    if (data == '未清') {
                        return '<b style="color:red">' + data + '</b>'
                    } else {
                        return data;
                    }
                }
            },
            {
                "targets": 15,
                "render": function (data, type, row) {
                    var date = new Date(data);
                    return date.getFullYear() + "-" + (date.getMonth() + 1) + "-" + date.getDate();
                }
            },
            {
                "targets": 18,
                "render": function (data, type, row) {
                    var btnStr = "";
                    if (UserAuth.indexOf("删除") >= 0) {
                        btnStr += "<button type='button' onclick='Delete_data(" + row.id + "," + row.Status + ",\"" + row.Type + "\")' style='font-size:80%;' class='btn btn-danger btn-sm'>删除</button>";
                    }
                    if (UserAuth.indexOf("修改") >= 0) {
                        btnStr += "<button type='button' onclick='Dialog_Edit(" + JSON.stringify(row) + ")' style='margin-left:10px;font-size:80%' class='btn btn-success btn-sm'>编辑</button>"
                    }
                    return btnStr;
                }
            }
        ],
        "drawCallback": function (settings) {
            var api = this.api();
            var rows = api.rows({ page: 'current' }).nodes();
            //[承兑金额,已还金额，待还金额, 保证金]
            var remain = [0, 0, 0, 0, 0];
            var Datalength = api.column(12, { page: 'current' }).data().length;
            var last = api.column(12, { page: 'current' }).data()[0];
            api.column(12, { page: 'current' }).data().each(function (group, i) {
                var Type = api.column(2, { page: 'current' }).data()[i];
                var Money1 = api.column(6, { page: 'current' }).data()[i];
                var Money2 = api.column(7, { page: 'current' }).data()[i];
                var Money3 = api.column(9, { page: 'current' }).data()[i];
                var Money5 = api.column(11, { page: 'current' }).data()[i];
                if (last != group) {
                    var Money4 = 0;
                    if (remain[1] == 0) {
                        Money4 = remain[3];
                    } else {
                        Money4 = remain[1];
                    }
                    $(rows).eq(i).before(
                        '<tr style="font-size:140%;font-weight:600;text-align:left">' +
                        '<td colspan="8">' + last + '</td>' +
                        '<td style="text-align:center">合计</td>' +
                        '<td colspan="6">保证金:' + remain[4].toFixed(2) + '，承兑金额:' + remain[0].toFixed(2) + '，已还：' + Money4.toFixed(2) + '，待还：' + remain[2].toFixed(2) + '</td>' +
                        '</tr>'
                    );
                    last = group;
                    remain = [0, 0, 0, 0 , 0];
                }
                if (i == Datalength - 1) {
                    
                    remain = CalcRemain(remain, Type, Money1, Money2, Money3, Money5);
                    var Money4 = 0;
                    if (remain[1] == 0) {
                        Money4 = remain[3];
                    } else {
                        Money4 = remain[1];
                    }

                    $(rows).eq(i).after(
                        '<tr  style="font-size:140%;font-weight:600;text-align:left">' +
                        '<td colspan="7">' + last + '</td>' +
                        '<td style="text-align:center">合计</td>' +
                        '<td colspan="7">保证金:' + remain[4].toFixed(2) + '，承兑金额:' + remain[0].toFixed(2) + '，已还：' + Money4.toFixed(2) + '，待还：' + remain[2].toFixed(2) + '</td>' +
                        '</tr>'
                    );
                }
                remain = CalcRemain(remain, Type, Money1, Money2, Money3,Money5);
            });
        },
        //dom: 'Bfrtip',
        dom:'<"bottom"Bfrtip>',
        buttons: Tabbuttons
    })
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
})

//计算承兑金额
function CalcRemain(remain, Type, Money1, Money2, Money3, Money4) {
    if (Type == '贷') {
        remain[0] += Money1;
        remain[2] += Money3;
        remain[3] += Money2;
        if (Money4 != "" && Money4!=null) {
            remain[4] += Money4;
        }
        
    } else if (Type == '还') {
        remain[1] += Money2;
    }
    return remain;
}
/* 对话框 */

//新增承兑
function Dialog_Add_Loan() {
    dialog_add_loan.css("display", "block");
    $.confirm({
        title: '银行承兑业务-承兑',
        closeIcon: true,
        columnClass: '',
        //type:'green',
        content: dialog_add_loan,
        buttons: {
            add_loan: {
                text: '添加',
                btnClass: 'btn btn-success',
                action: function () {
                    add_Loan();
                }
            },
            close: {
                text: '关闭'
            }
        }
    }); 
}

//承兑还款
function Dialog_Add_Repay() {
    dialog_add_repay.css("display", "block");
    //dataSet 过滤出数据
    
    $.confirm({
        title: '银行承兑业务-还款',
        closeIcon: true,
        columnClass: '',
        //type:'green',
        content: dialog_add_repay,
        onOpen: function () {
            vue__add_repay = new Vue({
                el: '#dialog_add_repay',
                data: {
                    test: 'test',
                    CompanyList: CompanyList,
                    CmpIdx: 0,
                    BankList: CompanyList[0].BankList,
                    BankIdx: 0,
                    TableList: [],
                    Now_Repayed: 0,//本次还款
                    Now_Total_Balance: 0,//还款后余额
                    Total_Margin:0,//保证金
                    Total_Loan: 0,//贷款金额
                    Total_Repayed: 0,//已还金额
                    Total_Balance: 0,//余额
                    Now_RepayedFlg: true,//控制Now_Repayed的watch是否触发
                    //提交内容
                    OccDate: null,
                    Abstract: '还款',
                    Remarks:'',
                },
                methods: {
                    SubTabSwitch: function (DataList) {
                        var _this = this;
                        this.Total_Loan = 0;
                        this.Total_Margin = 0;
                        this.Total_Repayed = 0;
                        this.Total_Balance = 0;
                        this.Now_Total_Balance = 0;
                        this.Now_Repayed = 0;
                        var FilterData = DataList.filter(function (item) {
                            if (item.Company == _this.CompanyList[_this.CmpIdx].Company) {
                                if (item.Bank == _this.BankList[_this.BankIdx] &&  item.Flag=='未清') {
                                    _this.Total_Loan += parseFloat(item.LoanAmount);
                                    _this.Total_Margin += parseFloat(item.Margin);
                                    _this.Total_Repayed += parseFloat(item.Repayed);
                                    _this.Total_Balance += parseFloat(item.Balance);
                                    return true;
                                }
                            }
                            return false;
                        })
                        this.Now_Total_Balance = this.Total_Balance;
                        FilterData.forEach(function (item) {
                            item.Now_Repayed = 0;
                        })
                        return FilterData;
                    },
                    ToDate: function (d) {
                        var date = new Date(d);
                        return date.getFullYear() + "-" + (date.getMonth() + 1) + "-" + date.getDate();
                    },
                    ttttt: function () {
                        var tt = 0;
                    }
                },
                watch: {
                    CmpIdx: function () {
                        this.BankList = this.CompanyList[this.CmpIdx].BankList;
                        this.BankIdx = 0;
                        this.TableList = this.SubTabSwitch(DataList);
                    },
                    BankIdx: function () {
                        //过滤数据 更新 subTable
                        this.TableList = this.SubTabSwitch(DataList);
                        //Create_subTab(FilterData);
                    },
                    Now_Repayed: function () {
                        this.Now_Repayed = parseFloat(this.Now_Repayed);
                        if (this.Now_Repayed == 0 && !this.Now_RepayedFlg) {
                            this.Now_RepayedFlg = true;
                            return;
                        }
                        if (this.Now_Repayed < 0) {
                            Alert1("提示", "请核对数据的正确性!", "btn-danger", "red");
                            return;
                        }
                        var remain = this.Now_Repayed;
                        if (this.TableList.length <= 0) {
                            Alert1("提示", "本银行不需要还款", "btn-danger", "red");
                            this.Now_RepayedFlg = false;
                            this.Now_Repayed = 0;
                            return;
                        }
                        this.TableList.forEach(function (item) {
                            item.Now_Repayed = 0;
                        })
                        this.TableList.forEach(function (item) {
                            if (remain == 0) {
                                return;
                            }
                            item.Now_Repayed = 0;
                            //Balance 余额
                            //Now_Repayed 页面显示
                            //当前需要扣减余额
                            //item.Now_Repayed = 0;
                            item.Now_Repayed = 0;
                            var ItemRemain = item.Balance;

                            if (ItemRemain > 0) {//从总余额里面扣除
                                if (ItemRemain > remain) {
                                    item.Now_Repayed += remain;
                                    remain = 0;
                                } else {
                                    item.Now_Repayed = item.Balance;
                                    remain -= item.Balance;
                                }
                            }
                        });
                        if (remain > 0) {
                            Alert1("提示", "本次还款大于余额,请核实余额数！", "btn-danger", "red");
                            this.TableList.forEach(function (item) {
                                item.Now_Repayed = 0;
                            })
                            this.Now_Repayed = 0;
                            return;
                        }
                    }
                },
                computed: {
                    Now_Repayed2: function () {
                        var result = 0.0;
                        this.TableList.forEach(function (item) {
                            if (item.Now_Repayed != "") {
                                result += parseFloat(item.Now_Repayed);
                            }
                        })
                        return result;
                    },
                    Now_Total_Balance2: function () {
                        var result = this.Total_Balance;
                        this.TableList.forEach(function (item) {
                            if (item.Now_Repayed != "") {
                                result -= parseFloat(item.Now_Repayed);
                            }
                        })
                        return result.toFixed(4);
                    },
                    Now_Repayed3: function () {
                        var total = 0;
                        this.TableList.forEach(function (item) {
                            if (item.Now_Repayed != "") {
                                total += parseFloat(item.Now_Repayed);
                            }
                        });
                        this.Now_Repayed = total;
                        return total;
                    }
                },
                mounted: function () {
                    var now = new Date();
                    //格式化日，如果小于9，前面补0
                    var day = ("0" + now.getDate()).slice(-2);
                    //格式化月，如果小于9，前面补0
                    var month = ("0" + (now.getMonth() + 1)).slice(-2);
                    //拼装完整日期格式
                    var today = now.getFullYear() + "-" + (month) + "-" + (day);
                    this.OccDate = today;
                }
            });
            Create_subTab();
        },
        buttons: {
            add_repay: {
                text: '保存',
                btnClass: 'btn btn-success',
                action: function () {
                    add_repay();
                }
            },
            close: {
                text: '关闭'
            }
        }
    });
    
}

//修改承兑
var Edit_id;
function Dialog_Edit(obj) {
    if (obj.Type == '还') {
        Alert1("提示", "还款记录不可修改！", "btn-danger", "red");
        return;
    }
    if (obj.Status == 1 || obj.Status == "1") {
        Alert1("提示", "本记录已有还款，无法修改", "btn-danger", "red");
        return;
    }
    dialog_edit.css("display", "block");
    Edit_id = obj.id;
    
    dialog_edit.find("#edit_Company").val(obj.Company);
    dialog_edit.find("#edit_Bank").val(obj.Bank);
    dialog_edit.find("#edit_Abstract").val(obj.Abstract);
    dialog_edit.find("#edit_LoanAmount").val(obj.LoanAmount); 
    dialog_edit.find("#edit_Margin").val(obj.Margin);
    dialog_edit.find("#edit_OccDate").val(SteDateToinputDate(obj.OccDate));
    dialog_edit.find("#edit_EndDate").val(SteDateToinputDate(obj.EndDate));
    dialog_edit.find("#edit_Remarks").val(obj.Remarks);
    $.confirm({
        title: '修改贷款信息',
        closeIcon: true,
        columnClass: 'col-lg-5',
        //type:'green',
        content: dialog_edit,
        buttons: {
            Edit_Company: {
                text: '修改',
                btnClass: 'btn btn-success',
                action: function () {
                    //编辑贷款
                    Edit_BankService2();
                }
            },
            close: {
                text: '关闭'
            }
        }
    });
    
}

/* 添加信息 */

//新增承兑
function add_Loan() {
    var formData = new FormData();

    var Company = $("#add_Company1").val();
    var Bank = $("#add_Bank1").val();
    var Abstract = $("#add_Abstract").val();
    var LoanAmount = $("#add_LoanAmount").val();
    var Margin = $("#add_Margin").val();
    var OccDate = $("#add_OccDate").val();
    var EndDate = $("#add_EndDate").val();
    if (OccDate == "") {
        Alert1("提示", "请填写承兑日期", "btn-danger", "red");
        return;
    }
    if (EndDate == "") {
        Alert1("提示", "请填写到期日期", "btn-danger", "red");
        return;
    }

    var Remarks = $("#add_Remarks").val();

    formData.append("Company", Company);
    formData.append("Bank", Bank);
    formData.append("Abstract", Abstract);
    formData.append("LoanAmount", LoanAmount);
    formData.append("Margin", Margin);
    formData.append("OccDate", OccDate);
    formData.append("EndDate", EndDate);
    formData.append("Remarks", Remarks);

    $.ajax({
        type: 'POST',
        url: hosturl + '/Home/Add_BankServer2_1',
        data: formData,
        cache: false,
        dataType: 'JSON',
        processData: false,
        contentType: false,
        beforeSend: function () {
        },
        complete: function () {
        },
        success: function (res) {
            var json = isJsonString(res);
            if (res.msg == "OK") {
                //关闭此对话框
                Alert1("提示", "添加成功", "btn-success", "green");
                datatable.ajax.reload();
            }
            else if (res.msg == "refuse") {
                //关闭此对话框
                Alert1("提示", "登录超时", "btn-danger", "red");
            }
            else if (res.msg == "error") {
                //关闭此对话框
                Alert1("提示", "填写错误。", "btn-danger", "red");
            }
        },
        error: function () {
            Alert1("提示", "服务器无响应", "btn-danger", "red");
        }
    });

}

//承兑还款
function add_repay() {
    var formData = new FormData();
    var Data = vue__add_repay._data;
    //企业名称
    var Company = Data.CompanyList[Data.CmpIdx].Company;
    //银行名称
    var Bank = Data.BankList[Data.BankIdx];
    //摘要
    var Abstract = Data.Abstract;
    //承兑日期
    var OccDate = Data.OccDate;
    //备注
    var Remarks = Data.Remarks;
    //还款金额对照id,xx钱/id,xx钱 
    var Repayrecord = '';
    var RepayAmount = 0.0;
    var flg = false;
    Data.TableList.forEach(function (item) {
        if (item.Now_Repayed != "" && item.Now_Repayed != 0) {
            //判断一下 输入的Now_Repayed（还款金额） 是否 大于Balance（未清余额）
            if (flg) {
                return;
            }
            if (parseFloat(item.Now_Repayed) > parseFloat(item.Balance)) {
                Alert1("提示", "还款金额大于余额,请检查数据正确性。", "btn-danger", "red");
                flg = true;
                return;
            }
            RepayAmount += parseFloat(item.Now_Repayed);
            Repayrecord += item.id + ',' + item.Now_Repayed + '/'
        }
    })
    if (flg) {
        return;
    }

    if (RepayAmount == 0 || RepayAmount == '' || RepayAmount == undefined) {
        //请填写还款金额
        Alert1("提示", "请填写还款金额", "btn-danger", "red");
        return;
    }
    formData.append("Company", Company);
    formData.append("Bank", Bank);
    formData.append("Abstract", Abstract);
    formData.append("OccDate", OccDate);
    formData.append("Remarks", Remarks);
    formData.append("Repayrecord", Repayrecord);
    formData.append("RepayAmount", RepayAmount);
    $.ajax({
        type: 'POST',
        url: hosturl + '/Home/Add_BankServer2_2',
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
                //Alert1("提示", "还款成功", "btn-primary", "green");
                $.alert({
                    backgroundDismiss: true,
                    type: "green",
                    title: "提示",
                    content: "还款成功",
                    buttons: {
                        OK: {
                            text: '确定',
                            btnClass: "btn-primary",
                            action: function () {
                                location.reload();
                            }
                        }
                    }
                })
                //datatable.ajax.reload();
            } else if (res.msg == "refuse") {
                Alert1("提示", "登录超时", "btn-danger", "red");
            } else if (res.msg == "error") {
                Alert1("提示", "登录超时", "btn-danger", "red");
            }
        },
        error: function () {
            Alert1("提示", "服务器无响应", "btn-danger", "red");
        }
    })
}

/* 修改信息 */
function Edit_BankService2() {
    if (EditFlg) {
        return;
    }
    var formData = new FormData();
    var id = Edit_id;
    if (id == 0 || id == undefined) {
        Alert1("提示", "没有选择对应记录", "btn-danger", "red");
        return;
    }
    var Company = $("#edit_Company").val();
    var Bank = $("#edit_Bank").val();
    var Abstract = $("#edit_Abstract").val();
    var Margin = $("#edit_Margin").val();
    var LoanAmount = $("#edit_LoanAmount").val();
    var OccDate = $("#edit_OccDate").val();
    var EndDate = $("#edit_EndDate").val();
    var Remarks = $("#edit_Remarks").val();

    formData.append("id", id);
    formData.append("Company", Company);
    formData.append("Bank", Bank);
    formData.append("Abstract", Abstract);
    formData.append("Margin", Margin);
    formData.append("LoanAmount", LoanAmount);
    formData.append("OccDate", OccDate);
    formData.append("EndDate", EndDate);
    formData.append("Remarks", Remarks);

    $.ajax({
        type: 'POST',
        url: hosturl + '/Home/Edit_BankServer2',
        data: formData,
        dataType: 'JSON',
        cache: false,
        processData: false,
        contentType: false,
        beforeSend: function () {
            EditFlg = true;
        },
        complete: function () {
            EditFlg = false;
        },
        success: function (res) {
            if (res.msg == "OK") {
                Alert1("提示", "修改成功", "btn-success", "green");
                //关闭对话框
                datatable.ajax.reload();
            } else if (res.msg == "refuse") {
                Alert1("提示", "登录超时", "btn-danger", "red");
            } else if (res.msg == "error") {
                Alert1("提示", "修改失败", "btn-danger", "red");
            }
        },
        error: function () {
            Alert1("提示", "服务器无响应", "btn-danger", "red");
        }
    });

}

/* 删除信息 */
function Delete_data(id, Status, Type) {
    if (Type == '还') {
        $.confirm({
            title: '提示',
            content: '确定删除信息吗？',
            buttons: {
                confirm: {
                    text: "确定",
                    btnClass: 'btn-success',
                    action: function () {
                        var formData = new FormData();
                        formData.append("id", id);
                        $.ajax({
                            type: 'POST',
                            url: hosturl + '/Home/Del_BankServer2_1',
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
                                    Alert1("提示", "删除成功", "btn-success", "green");
                                    datatable.ajax.reload();
                                } else if (res.msg == "refuse") {
                                    Alert1("提示", "登录超时", "btn-danger", "red");
                                } else {
                                    Alert1("提示", "删除失败", "btn-danger", "red");
                                }
                            },
                            error: function () {
                                Alert1("提示。", "服务器无响应", "btn-danger", "red");
                            }
                        });

                    }
                },
                cancel: {
                    text: '取消',
                    btnClass: 'btn',
                    action: function () {
                        return;
                    }
                }
            }
        });
    } else if (Type == '贷') {

        if (Status == 1 || Status == "1") {
            Alert1("提示", "本记录已有还款，无法删除", "btn-danger", "red");
            return;
        }
        $.confirm({
            title: '提示',
            content: '确定删除信息吗？',
            buttons: {
                confirm: {
                    text: "确定",
                    btnClass: 'btn-success',
                    action: function () {
                        var formData = new FormData();
                        formData.append("id", id);
                        $.ajax({
                            type: 'POST',
                            url: hosturl + '/Home/Del_BankServer2_2',
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
                                    Alert1("提示", "删除成功", "btn-success", "green");
                                    datatable.ajax.reload();
                                } else if (res.msg == "refuse") {
                                    Alert1("提示", "登录超时", "btn-danger", "red");
                                } else if (res.msg == "Fail") {
                                    Alert1("提示", "本记录已有还款，无法删除", "btn-danger", "red");
                                } else {
                                    Alert1("提示", "删除失败", "btn-danger", "red");
                                }
                            },
                            error: function () {
                                Alert1("提示。", "服务器无响应", "btn-danger", "red");
                            }
                        });

                    }
                },
                cancel: {
                    text: '取消',
                    btnClass: 'btn',
                    action: function () {
                        return;
                    }
                }
            }
        });

    }
}

/* 创建表格 */
function Create_subTab() {
    var subTable = $("#tab_repay").DataTable({
        oLanguage: LanguagePage,
        searching: false,
        retrieve: true,
        bLengthChange: false,//去掉每页dao多少条框bai体
        height: 80,
        "bSort": false,
        sScrollY: 260, //DataTables的高 
        //data: Source,
        "bPaginate": false, //是否显示（应用）分页器
        columns: [
            {
                "data": null,
                "defaultContent": '',
                "orderable": false,
                "searchable": false,
            },
            {//id 1
                "data": "id",
                "defaultContent": '',
                "orderable": false,
                "searchable": false,
                "visible": false
            },
            {// 状态 2
                "data": "Flag",
                "defaultContent": '',
                "orderable": false,
            },
            {// 摘要 3
                "data": "Abstract",
                "defaultContent": '',
                "orderable": false,
            },
            {// 承兑日期 4
                "data": "OccDate",
                "defaultContent": '',
                "orderable": false,
            },
            {// 到期日期 5
                "data": "EndDate",
                "defaultContent": '',
                "orderable": false,
            },
            {// 承兑金额 6
                "data": "LoanAmount",
                "defaultContent": '',
                "orderable": false,
                "searchable": false,
            },
            {// 保证金 7
                "data": "LoanAmount",
                "defaultContent": '',
                "orderable": false,
                "searchable": false,
            },
            {// 已还金额 8
                "data": "Repayed",
                "defaultContent": '',
                "orderable": false,
                "searchable": false,
            },
            {// 余额 9
                "data": "Balance",
                "defaultContent": '',
                "orderable": false,
                "searchable": false,
            },
            {// 本次还款 10
                "data": null,
                "defaultContent": '',
                "orderable": false,
            },
            {// 备注 11
                "data": "Remarks",
                "defaultContent": '',
                "orderable": false,
            },
        ],
        "columnDefs": [
            {
                "targets": 3,
                "render": function (data, type, row) {
                    var date = new Date(data);
                    return date.getFullYear() + "-" + (date.getMonth() + 1) + "-" + date.getDate();
                }
            },
            {
                "targets": 4,
                "render": function (data, type, row) {
                    var date = new Date(data);
                    return date.getFullYear() + "-" + (date.getMonth() + 1) + "-" + date.getDate();
                }
            },
            {
                "targets": 9,
                "render": function (data, type, row) {
                    return '<input type="number" value="0"/> '
                }
            }
        ]
    })
    //处理table
    //subTable.on('order.dt search.dt', function () {
    //    subTable.column(0, { search: 'applied', order: 'applied' }).nodes().each(function (cell, i) {
    //        cell.innerHTML = i + 1;
    //    });
    //}).draw();
}

//
function SteDateToinputDate(d) {
    var now = new Date(d);
    //格式化日，如果小于9，前面补0
    var day = ("0" + now.getDate()).slice(-2);
    //格式化月，如果小于9，前面补0
    var month = ("0" + (now.getMonth() + 1)).slice(-2);
    //拼装完整日期格式
    var today = now.getFullYear() + "-" + (month) + "-" + (day);

    return today;
}

/* 表格搜索功能 */
$.fn.dataTable.ext.search.push(
    function (settings, data, dataIndex) {
        var OccDate = new Date(datatable.data()[dataIndex].OccDate);
        var EndDate = new Date(datatable.data()[dataIndex].EndDate);
        var Company = datatable.data()[dataIndex].Company;
        var Bank = datatable.data()[dataIndex].Bank;
        var Type = data[2];
        var TableType = datatable.data()[dataIndex].cmp.SType.id;
        //业务日期
        //var Search_Begin = new Date($("#Ser_Begin").val());
        //var Search_SerEnd = new Date($("#Ser_End").val());
        //到期日期
        var Search_EndBegin = new Date($("#End_Begin").val());
        var Search_EndEnd = new Date($("#End_End").val());
        //板块
        var Search_Type = $("#Search_Type").find("option:selected").val();
        //公司 
        var Search_Company = $("#Search_Company").find("option:selected").text();
        //银行
        var Search_Bank = $("#Search_Bank").find("option:selected").text();

        var flg = true;
        //if (!(OccDate.getTime() >= Search_Begin.getTime() && OccDate.getTime() <= Search_SerEnd.getTime())) {
        //    flg = false;
        //}
        //if (Type == "还") {

        //} else if (!(EndDate.getTime() >= Search_EndBegin.getTime() && EndDate.getTime() <= Search_EndEnd.getTime())){
        //    flg = false;
        //}
        if (Company != Search_Company && Search_Company!="全部") {
            flg = false;
        }
        if (Bank != Search_Bank && Search_Bank!="全部") {
            flg = false;
        }

        if (TableType != Search_Type && Search_Type != "全部") {
            flg = false;
        }

        return flg;
    }
);

/* 初始化搜索 */
function init_Search() {
    
    vue_Search = new Vue({
        el: '#table_Search',
        data: {
            CompanyList: [],
            CompanyList2: [],
            CmpIdx: 1,
            BankList: [],
            BankIdx: 1,
            TypeIdx: 40,
        },
        watch: {
            TypeIdx: function (val) {
                var _this = this;
                //vue_Search._data
                this.CompanyList2 = this.CompanyList.filter(function (item) {
                    if (_this.TypeIdx == item.TypeId) {
                        return true;
                    } else {
                        return false;
                    }
                })
                this.CmpIdx = 0;

                if (this.CompanyList2.length > 0) {
                    this.BankList = this.CompanyList2[this.CmpIdx].BankList;
                } else {
                    this.BankList = [];
                }

                if (this.CmpIdx == 0) {
                    this.BankIdx = 0;
                } else {
                    this.BankIdx = 0;
                }

                $("#Search_Bank").val(0);
                //切换表
                setTimeout(function () {
                    datatable.draw();
                }, 200)

            },
            CmpIdx: function () {
                if (this.CompanyList2.length > 0) {
                    this.BankList = this.CompanyList2[this.CmpIdx].BankList;
                } else {
                    this.BankList = [];
                }

                if (this.CmpIdx == 0) {
                    this.BankIdx = 0;
                } else {
                    this.BankIdx = 0;
                }
                
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
    var d1 = CurrentMonthFirst();
    var d2 = CurrentMonthLast();
    var Year = d1.getFullYear();
    var Month1 = fix(1, 2);
    var Month2 = fix(12, 2);
    var d1_d = fix(d1.getDate(), 2);
    var d2_d = fix(d2.getDate(), 2);
    //$("#Ser_Begin").val(Year + "-" + Month1 + "-" + d1_d);
    //$("#Ser_End").val(Year + "-" + Month2 + "-" + d2_d);
    //$("#End_Begin").val(Year + "-" + Month1 + "-" + d1_d);
    //$("#End_End").val(Year + "-" + Month2 + "-" + d2_d);
    //$("#Ser_Begin,#Ser_End,#End_Begin,#End_End,#Search_Company,#Search_Bank").change(function () {
    //    datatable.draw();
    //})
}

function CurrentMonthFirst() {
    var date = new Date();
    date.setDate(1);
    return date;
}

function CurrentMonthLast(){
    var date = new Date();
    var currentMonth = date.getMonth();
    var nextMonth = ++currentMonth;
    var nextMonthFirstDay = new Date(date.getFullYear(), nextMonth, 1);
    var oneDay = 1000 * 60 * 60 * 24;
    return new Date(nextMonthFirstDay - oneDay);
}

/* 控件事件 */
function Search_Type_Change() {
    setTimeout(function () { datatable.draw(); }, 200)
}
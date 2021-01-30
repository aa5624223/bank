var datatable;
var CreditList;
var EditFlg = false;
var dialog_add;
var dialog_edit;
var dialog_addType;
/* 页面初始化 */
$(document).ready(function () {
    dialog_add = $("#dialog_add");
    dialog_edit = $("#dialog_edit");
    var UserAuth = getUserFunction("银行授信");
    var Tabbuttons = [
        {
            extend: 'excelHtml5',
            autoFilter: true,
            messageTop: '授信信息',
            title: '授信信息',
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
        }
    ];
    if (UserAuth.indexOf("修改") >= 0) {
        Tabbuttons.push({
            text: '添加记录',
            action: function (e, dt, node, config) {
                Dialog_Add();
            }
        });
    }
    datatable = $("#tab_BankInfo").DataTable({
        oLanguage: LanguagePage,
        //"bStateSave": true,
        height: 300,
        sScrollY: 520, //DataTables的高 
        //"bPaginate": false, //是否显示（应用）分页器
        ajax: {
            url: hosturl + '/Home/Search_CreditInfo',
            type: 'POST',
            'dataSrc': function (json) {
                BankList = isJsonString(json.data);
                return BankList;
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
            {//id 1
                "data": "id",
                "defaultContent": '',
                "orderable": false,
                "searchable": false,
                sClass: "td-oprate",
                "visible": false
            },
            {//企业名称 2
                "data": "Company",
                "defaultContent": '',
                "width": "380px",
                "orderable": false,
                "searchable": true,
            },
            {//银行名称 3
                "data": "Bank",
                "defaultContent": '',
                "width": "300px",
                "orderable": false,
                "searchable": true,
            },
            {//信用额度 4
                "data": "Credit",
                "defaultContent": '',
                "width": "170px",
                "orderable": false,
                "searchable": false,
            },
            {//承兑额度 5
                "data": "Acceptance",
                "defaultContent": '',
                "width": "170px",
                "orderable": false,
                "searchable": false,
            },
            {//贷款额度 6
                "data": "Arrears",
                "defaultContent": '',
                "width": "170px",
                "orderable": false,
                "searchable": false,
            },
            {//贷款利率 7
                "data": "Rates",
                "defaultContent": '',
                "width": "140px",
                "orderable": false,
                "searchable": false,
            },
            {//欠款总额 8
                "data": "Arrears",
                "defaultContent": '',
                "width": "170px",
                "orderable": false,
                "searchable": false,
            },
            {//备注信息 9
                "data": "Remarks",
                "defaultContent": '',
                "width": "300px",
                "orderable": false,
                "searchable": false,
            },
            {//创建日期 10
                "data": "Builddate",
                "defaultContent": '',
                "width": "200px",
                "orderable": false,
                "searchable": false,
            },
            {//操作 11
                "data": null,
                "defaultContent": '',
                "width": "150px",
                "orderable": false,
                sClass: "td-oprate",
                "searchable": false,
            },
        ],
        columnDefs: [
            {
                "targets": 11,
                "render": function (data, type, row) {
                    var btnStr = "";
                    if (UserAuth.indexOf("删除") >= 0) {
                        btnStr += "<button type='button' onclick='Delete_data(" + row.id + ")' style='font-size:80%;' class='btn btn-danger btn-sm'>删除</button>";
                    }
                    if (UserAuth.indexOf("修改") >= 0) {
                        btnStr += "<button type='button' onclick='Dialog_Edit(" + JSON.stringify(row) + ")' style='margin-left:10px;font-size:80%' class='btn btn-success btn-sm'>编辑</button>";
                    }
                    return btnStr;
                }
            },
            {
                "targets": 10,
                "render": function (data, type, row) {
                    var date = new Date(data);
                    return date.getFullYear() + "-" + (date.getMonth() + 1) + "-" + date.getDate();
                },
                
            }
        ],
        dom: 'Bfrtip',
        buttons: Tabbuttons
    });
    datatable.on('order.dt search.dt', function () {
        datatable.column(0, { search: 'applied', order: 'applied' }).nodes().each(function (cell, i) {
            cell.innerHTML = i + 1;
        });
    }).draw();
})
/* 对话框 */
//添加授信 对话框
function Dialog_Add() {
    dialog_add.css("display", "block");
    $.confirm({
        title: '添加银行授信',
        closeIcon: true,
        columnClass: '',
        content: dialog_add,
        buttons: {
            add_Credit: {
                text: '添加',
                btnClass: 'btn btn-success',
                action: function () {
                    add_Credit();
                }
            },
            close: {
                text:'关闭'
            }
        }
    })
}
//编辑授信 对话框
function Dialog_Edit(obj) {
    dialog_edit.css("display", "block");
    dialog_edit.find("#Edit_Company").val(obj.Company);
    dialog_edit.find("#Edit_Bank").val(obj.Bank);
    
    dialog_edit.find("#Edit_Credit").val(obj.Credit);

    dialog_edit.find("#Edit_Acceptance").val(obj.Acceptance);
    dialog_edit.find("#Edit_Loans").val(obj.Loans);
    dialog_edit.find("#Edit_Rates").val(obj.Rates);
    dialog_edit.find("#Edit_Arrears").val(obj.Arrears);
    dialog_edit.find("#Edit_Remarks").val(obj.Remarks);
    Edit_id = obj.id;

    $.confirm({
        title: '修改银行授信信息',
        closeIcon: true,
        columnClass: '',
        //type:'green',
        content: dialog_edit,
        buttons: {
            Edit_Credit: {
                text: '修改',
                btnClass: 'btn btn-success',
                action: function () {
                    Edit_Credit();
                }
            },
            close: {
                text: '关闭'
            }
        }
    });

}
/* 添加信息 */

//添加授信
function add_Credit() {
    var formData = new FormData();
    var Company = $("#add_Company").val();
    var Bank = $("#add_Bank").val();
    var Credit = $("#add_Credit").val();
    if (Credit=="") {
        Alert1("提示。", "请输入信用额度", "btn-danger", "red");
        return;
    }
    var Acceptance = $("#add_Acceptance").val();
    if (Acceptance == "") {
        Alert1("提示。", "请输入承兑额度", "btn-danger", "red");
        return;
    }
    var Loans = $("#add_Loans").val();
    if (Loans == "") {
        Alert1("提示。", "清输入贷款额度", "btn-danger", "red");
        return;
    }
    var Rates = $("#add_Rates").val();
    var Arrears = $("#add_Arrears").val();
    var Remarks = $("#add_Remarks").val();

    if (parseFloat(Credit) != parseFloat(Acceptance) + parseFloat(Loans)) {
        Alert1("提示。", "承兑额度+贷款额度 不等于信用额度", "btn-danger", "red");
        return;
    }
    /* 清空数据 */

    formData.append("Company", Company);
    formData.append("Bank", Bank);
    formData.append("Credit", Credit);
    formData.append("Acceptance", Acceptance);
    formData.append("Loans", Loans);
    formData.append("Rates", Rates);
    formData.append("Arrears", Arrears);
    formData.append("Remarks", Remarks);

    $.ajax({
        type: 'POST',
        url: hosturl + '/Home/Add_CreditInfo',
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
                //$("#add_Company").val();
                //$("#add_Bank").val();
                $("#add_Credit").val("");
                $("#add_Acceptance").val("");
                $("#add_Loans").val("");
                $("#add_Rates").val("");
                $("#add_Arrears").val("");
                $("#add_Remarks").val("");
                datatable.ajax.reload();
            }
            else if (res.msg == "refuse") {
                //关闭此对话框
                Alert1("提示", "登录超时", "btn-danger", "red");
            }
            else if (res.msg == "error") {
                //关闭此对话框
                Alert1("提示。", "填写错误,请检查银行名称、银行名称是否重复", "btn-danger", "red");
            }
        },
        error: function () {
            Alert1("提示。", "服务器无响应", "btn-danger", "red");
        }
    })
}

/* 删除信息 */

//删除授信
function Delete_data(id) {
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
                        url: hosturl + '/Home/Del_CreditInfo',
                        data: formData,
                        dataType: 'JSON',
                        cache: false,
                        processData: false,
                        contentType: false,
                        beforeSend: function () {
                            LoginFlg = true;
                        },
                        complete: function () {
                            LoginFlg = false;
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
            }
        }
    });
}

/* 修改信息 */

//修改授信
var Edit_id = 0;
function Edit_Credit() {
    if (EditFlg) {
        return;
    }
    var formData = new FormData();
    var Company = $("#Edit_Company").val();
    var Bank = $("#Edit_Bank").val();
    var Credit = $("#Edit_Credit").val();
    var Acceptance = $("#Edit_Acceptance").val();
    var Loans = $("#Edit_Loans").val();
    var Rates = $("#Edit_Rates").val();
    var Arrears = $("#Edit_Arrears").val();
    var Remarks = $("#Edit_Remarks").val();

    formData.append("id", Edit_id);
    formData.append("Company", Company);
    formData.append("Bank", Bank);
    formData.append("Credit", Credit);
    formData.append("Acceptance", Acceptance);
    formData.append("Loans", Loans);
    formData.append("Rates", Rates);
    formData.append("Arrears", Arrears);
    formData.append("Remarks", Remarks);

    $.ajax({
        type: 'POST',
        url: hosturl + '/Home/Edit_CreditInfo',
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
                //之后更改
                //Alert1("提示", "修改失败,请检查名或编号、否重复", "btn-danger", "red");
            }
        },
        error: function () {
            Alert1("提示", "服务器无响应", "btn-danger", "red");
        }
    });

}
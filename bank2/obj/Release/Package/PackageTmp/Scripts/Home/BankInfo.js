var datatable;
var BankList;
var EditFlg = false;
var dialog_add;
var dialog_edit;
var dialog_addType;
$(document).ready(function () {
    dialog_add = $("#dialog_add");
    dialog_edit = $("#dialog_edit");
    dialog_addType = $("#dialog_addType");
    var UserAuth = getUserFunction("银行信息");
    var Tabbuttons = [{
            extend: 'excelHtml5',
            autoFilter: true,
            messageTop: '银行信息',
            title: '银行信息',
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
            text: '添加银行',
            action: function (e, dt, node, config) {
                Dialog_Add();
            }
        });
        Tabbuttons.push(
            {
                text: '添加银行类别',
                action: function (e, dt, node, config) {
                    Dialog_AddType();
                }
            }
        )
    }

    datatable = $("#tab_BankInfo").DataTable({
        oLanguage: LanguagePage,
        //"bStateSave": true,
        height: 300,
        sScrollY: 520, //DataTables的高 
        //"bPaginate": false, //是否显示（应用）分页器
        ajax: {
            url: hosturl + '/Home/Search_Bank',
            type: 'POST',
            'dataSrc': function (json) {
                BankList = isJsonString(json.data);
                return BankList;
            }
        },
        "aaSorting": [[2, 'asc']],
        "order": [[2, "asc"]],
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
                "visible": false,
                sClass: "td-oprate"
            },
            {//银行编号 2
                "data": "SortID",
                "defaultContent": "",
                "width": "80px",
            },
            {//银行类型 3
                "data": "SType.TypeName",
                "defaultContent": '',
            },
            {//银行名称 4
                "data": "Bank",
                "defaultContent": '',
                "width": "300px"
            },
            {//备注 5
                "data": "Remarks",
                "defaultContent": '',
                "orderable": false,
                "searchable": false
            },
            {//操作 6
                "orderable": false,
                "searchable": false,
                "width": "120px",
                sClass: "td-oprate"
            }
        ],
        "columnDefs": [
            {
                "render": function (data, type, row) {
                    var btnStr = "";
                    if (UserAuth.indexOf("删除") >= 0) {
                        btnStr += "<button type='button' onclick='Delete_data(" + row.id + ")' style='font-size:80%;' class='btn btn-danger btn-sm'>删除</button>";
                    }
                    if (UserAuth.indexOf("修改") >= 0) {
                        btnStr +="<button type='button' onclick='Dialog_Edit(" + JSON.stringify(row) + ")' style='margin-left:10px;font-size:80%' class='btn btn-success btn-sm'>编辑</button>"
                    }
                    return btnStr;
                },
                "targets": 6
            },
            { "visible": false, "targets": 3 }
        ],
        "order": [[3, 'asc']],
        "drawCallback": function (settings) {
            var api = this.api();
            var rows = api.rows({ page: 'current' }).nodes();
            var last = null;
            api.column(3, { page: 'current' }).data().each(function (group, i) {
                if (last !== group) {
                    $(rows).eq(i).before(
                        '<tr class="group" style="font-size:140%;font-weight:600"><td colspan="5">' + group + '</td></tr>'
                    );
                    last = group;
                }
            });
        },
        dom: 'Bfrtip',
        buttons: Tabbuttons
    });
    datatable.on('order.dt search.dt', function () {
        datatable.column(0, { search: 'applied', order: 'applied' }).nodes().each(function (cell, i) {
            cell.innerHTML = i + 1;
        });
    }).draw();
})
/* 1.对话框 */
//添加银行信息对话框
function Dialog_Add() {
    dialog_add.css("display", "block");
    $.confirm({
        title: '添加银行',
        closeIcon: true,
        columnClass: '',
        //type:'green',
        content: dialog_add,
        buttons: {
            add_Bank: {
                text: '添加',
                btnClass: 'btn btn-success',
                action: function () {
                    add_Bank();
                }
            },
            close: {
                text: '关闭'
            }
        }
    }); 

}
//添加银行类别对话框
function Dialog_AddType() {
    dialog_addType.css("display", "block");
    $.confirm({
        title: '添加银行类别',
        closeIcon: true,
        columnClass: '',
        //type:'green',
        content: dialog_addType,
        buttons: {
            add_Company: {
                text: '添加',
                btnClass: 'btn btn-success',
                action: function () {
                    Add_BankType();
                }
            },
            close: {
                text: '关闭'
            }
        }
    });

}

//编辑银行信息对话框
function Dialog_Edit(obj) {
    dialog_edit.css("display", "block");
    dialog_edit.find("#Edit_Bank").val(obj.Bank);
    dialog_edit.find("#Edit_SortID").val(obj.SortID);
    dialog_edit.find("#Edit_TypeID").val(obj.TypeID);
    dialog_edit.find("#Edit_Remarks").val(obj.Remarks);
    Edit_id = obj.id;
    $.confirm({
        title: '修改银行信息',
        closeIcon: true,
        columnClass: '',
        //type:'green',
        content: dialog_edit,
        buttons: {
            Edit_Company: {
                text: '修改',
                btnClass: 'btn btn-success',
                action: function () {
                    Edit_Bank();
                }
            },
            close: {
                text: '关闭'
            }
        }
    });

}

/* 2.添加请求 */

//添加银行
function add_Bank() {
    var formData = new FormData();
    var Bank = $("#add_Bank").val();
    var TypeID = $("#add_TypeID").val();
    var SortID = $("#add_SortID").val();
    var Remarks = $("#add_Remarks").val();
    formData.append("Bank", Bank);
    formData.append("TypeID", TypeID);
    formData.append("SortID", SortID);
    formData.append("Remarks", Remarks);
    $.ajax({
        type: 'POST',
        url: hosturl + '/Home/Add_Bank',
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
                Alert1("提示。", "填写错误,请检查银行名称、编号是否重复", "btn-danger", "red");
            }
        },
        error: function () {
            Alert1("提示。", "服务器无响应", "btn-danger", "red");
        }
    });

}

//添加银行信息请求
function Add_BankType() {
    var formData = new FormData();
    var SName = $("#addType_Name").val();
    formData.append("SName", SName);
    formData.append("TableName", "BankInfo");
    $.ajax({
        type: 'POST',
        url: hosturl + '/Home/Add_CompanyType',
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
                //Alert1("提示", "添加成功", "btn-success", "green");
                $.alert({
                    backgroundDismiss: true,
                    title: "提示",
                    type: "green",
                    content: "添加成功",
                    buttons: {
                        confirm: {
                            text: '确定',
                            btnClass: "btn-success",
                            action: function () {
                                location.reload();
                            }
                        }
                    }
                })
                datatable.ajax.reload();
            }
            else if (res.msg == "refuse") {
                //关闭此对话框
                Alert1("提示", "登录超时", "btn-danger", "red");
            }
            else if (res.msg == "error") {
                //关闭此对话框
                Alert1("提示。", "填写错误,请检查银行名称、编号是否重复", "btn-danger", "red");
            } else if (res.msg == "EXIST") {
                Alert1("提示", "已存在相同名字的类别", "btn-danger", "red");
            }
        },
        error: function () {
            Alert1("提示。", "服务器无响应", "btn-danger", "red");
        }
    });
}

/* 3.删除请求 */
//删除银行信息
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
                        url: hosturl + '/Home/Del_Bank',
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

}
/* 4.修改请求 */
//编辑银行信息
var Edit_id = 0;
function Edit_Bank() {
    if (EditFlg) {
        return;
    }
    var formData = new FormData();
    var Bank = $("#Edit_Bank").val();
    var SortID = $("#Edit_SortID").val();
    var TypeID = $("#Edit_TypeID").val();
    var Remarks = $("#Edit_Remarks").val();
    var id = Edit_id;

    formData.append("Bank", Bank);
    formData.append("SortID", SortID);
    formData.append("TypeID", TypeID);
    formData.append("Remarks", Remarks);
    formData.append("id", id);
    $.ajax({
        type: 'POST',
        url: hosturl + '/Home/Edit_Bank',
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
                Alert1("提示", "修改失败,请检查银行名或编号、否重复", "btn-danger", "red");
            }
        },
        error: function () {
            Alert1("提示", "服务器无响应", "btn-danger", "red");
        }
    });

}

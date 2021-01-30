var datatable;
var CompanyList;
var EditFlg = false;
var dialog_add;
var dialog_edit;
var dialog_addType;
$(document).ready(function () {
    //SideAuth
    //获得用户对当前页面的权限 的字符串 ,间隔
    var UserAuth = getUserFunction("企业信息");
    //if (UserAuth.indexOf("打印")>=0) {

    //}
    var Tabbuttons = [{
        extend: 'excelHtml5',
        autoFilter: true,
        messageTop: '企业信息',
        title: '企业信息',
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
            text: '添加企业',
            action: function (e, dt, node, config) {
                Dialog_Add();
            }
        });
        Tabbuttons.push({
            text: '添加企业类别',
            action: function (e, dt, node, config) {
                Dialog_AddType();
            }
        });
    }
    dialog_add = $("#dialog_add");
    dialog_edit = $("#dialog_edit");
    dialog_addType = $("#dialog_addType");
    datatable = $("#tab_CompanyInfo").DataTable({
        oLanguage: LanguagePage,
        //"bStateSave": true,
        height: 300,
        bAutoWidth: true,
        sScrollY: 520, //DataTables的高 
        //"bPaginate": false, //是否显示（应用）分页器
        ajax: {
            url: hosturl + '/Home/Search_Company',
            type: 'POST',
            'dataSrc': function (json) {
                CompanyList = isJsonString(json.data);
                return CompanyList;
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
            {//企业编号 2
                "data": "SortID",
                "defaultContent": "",
                "width": "80px",
                render: function (data) {
                    return data ;
                }
            },
            {//企业类型 3
                "data": "SType.TypeName",
                "defaultContent": '',
            },
            {//企业名称 4
                "data": "Company",
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
                        btnStr += "<button type='button' onclick='Dialog_Edit(" + JSON.stringify(row) + ")' style='margin-left:10px;font-size:80%' class='btn btn-success btn-sm'>编辑</button>";
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
                        '<tr class="group" style="font-size:140%;font-weight:600"><td colspan="5">企业类型:' + group + '</td></tr>'
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
});
/* 1. 对话框 */

//添加企业信息对话框
function Dialog_Add() {
    //显示对话框
    dialog_add.css("display","block");
    $.confirm({
        title: '添加企业',
        closeIcon: true,
        columnClass: '',
        //type:'green',
        content: dialog_add,
        buttons: {
            add_Company: {
                text: '添加',
                btnClass: 'btn btn-success',
                action: function () {
                    add_Company();
                }
            },
            close: {
                text:'关闭'
            }
        }
    }); 
}

//添加企业类别对话框
function Dialog_AddType() {
    dialog_addType.css("display", "block");
    $.confirm({
        title: '添加企业类别',
        closeIcon: true,
        columnClass: '',
        //type:'green',
        content: dialog_addType,
        buttons: {
            add_Company: {
                text: '添加',
                btnClass: 'btn btn-success',
                action: function () {
                    Add_CompanyType();
                }
            },
            close: {
                text: '关闭'
            }
        }
    });

}

//编辑企业信息对话框
function Dialog_Edit(obj) {
    dialog_edit.css("display", "block");
    dialog_edit.find("#Edit_Company").val(obj.Company);
    dialog_edit.find("#Edit_SortID").val(obj.SortID);
    dialog_edit.find("#Edit_TypeID").val(obj.TypeID);
    dialog_edit.find("#Edit_Remarks").val(obj.Remarks);
    $.confirm({
        title: '修改企业信息',
        closeIcon: true,
        columnClass: '',
        //type:'green',
        content: dialog_edit,
        buttons: {
            Edit_Company: {
                text: '修改',
                btnClass: 'btn btn-success',
                action: function () {
                    Edit_Company();
                }
            },
            close: {
                text: '关闭'
            }
        }
    });


    Edit_id = obj.id;

    //显示对话框
}

/* 2. 添加请求 */

//添加企业
function add_Company() {
    var formData = new FormData();
    var Company = $("#add_Company").val();
    var TypeID = $("#add_TypeID").val();
    var SortID = $("#add_SortID").val();
    var Remarks = $("#add_Remarks").val();
    formData.append("Company", Company);
    formData.append("TypeID", TypeID);
    formData.append("SortID", SortID);
    formData.append("Remarks", Remarks);
    $.ajax({
        type: 'POST',
        url: hosturl + '/Home/Add_Company',
        data: formData,
        cache: false,
        dataType:'JSON',
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
                Alert1("提示", "添加成功", "btn-success","green");
                datatable.ajax.reload();
            }
            else if (res.msg == "refuse") {
                //关闭此对话框
                Alert1("提示", "登录超时", "btn-danger","red");
            }
            else if (res.msg== "error") {
                //关闭此对话框
                Alert1("提示。", "填写错误,请检查企业名称、编号是否重复", "btn-danger", "red");
            }
        },
        error: function () {
            Alert1("提示。", "服务器无响应", "btn-danger", "red");
        }
    });

}

//添加企业信息请求
function Add_CompanyType() {
    var formData = new FormData();
    var SName = $("#addType_Name").val();
    formData.append("SName", SName);
    formData.append("TableName", "CompanyInfo");
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
                Alert1("提示。", "填写错误,请检查企业名称、编号是否重复", "btn-danger", "red");
            } else if (res.msg =="EXIST") {
                Alert1("提示", "已存在相同名字的类别", "btn-danger", "red");
            }
        },
        error: function () {
            Alert1("提示。", "服务器无响应", "btn-danger", "red");
        }
    });
}

/* 3. 删除请求 */

//删除企业信息
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
                        url: hosturl + '/Home/Del_Company',
                        data: formData,
                        dataType:'JSON',
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

/* 4. 编辑企业信息 */

//修改公司信息
var Edit_id = 0;
function Edit_Company() {
    if (EditFlg) {
        return;
    }
    var formData = new FormData();
    var Company = $("#Edit_Company").val();
    var SortID = $("#Edit_SortID").val();
    var TypeID = $("#Edit_TypeID").val();
    var Remarks = $("#Edit_Remarks").val();
    var id = Edit_id;

    formData.append("Company", Company);
    formData.append("SortID", SortID);
    formData.append("TypeID", TypeID);
    formData.append("Remarks", Remarks);
    formData.append("id", id);
    $.ajax({
        type: 'POST',
        url: hosturl + '/Home/Edit_Company',
        data: formData,
        dataType:'JSON',
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
                Alert1("提示", "修改失败,请检查企业名称、编号是否重复", "btn-danger", "red");
            }
        },
        error: function () {
            Alert1("提示", "服务器无响应", "btn-danger", "red");
        }
    });

}


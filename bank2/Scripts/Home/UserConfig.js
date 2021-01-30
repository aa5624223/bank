var PeopleTree;
var Vue_Area;
var SubmitEditFlg = false;
var dialog_add;
var dialog_add_dep;
var dialog_add_per;
var Vue_add_per;
var CheckCmpId = 0;
var CheckDptId = 0;
var CheckPerId = 0;
$(document).ready(function () {
    //对话框
    dialog_add = $("#dialog_add");
    dialog_add_dep = $("#dialog_add_dep");
    dialog_add_per = $("#dialog_add_per");
    //侧边选择树
    $('#CompanyTree').treeview({ //
        levels:1,
        expandIcon: "icon-folder1", 
        collapseIcon: "icon-folder icon-folder_open",
        selectable: true,
        //showTags:true,
        onNodeSelected: function (event, node) {
            if (node.parentId == undefined) {//选择的是部门
                PeopleTreeInit(getPeopleTree(node.Id, 0));
                CheckCmpId = node.Id;
                CheckDptId = 0;
            } else {//选择的是企业
                PeopleTreeInit(getPeopleTree(0, node.Id));
                CheckCmpId = 0;
                CheckDptId = node.Id;
            }
        },
        onNodeUnselected: function (event, node) {
            if (node.parentId == undefined) {//选择的是部门
                CheckDptId = 0;
                CheckCmpId = 0;
            } else {//选择的是企业
                CheckDptId = 0;
                CheckCmpId = 0;
            }
        },
        data: getComPanyTree()
    });
    PeopleTreeInit(getPeopleTree(0, 0));
    //Vue
    Vue_Area = new Vue({
        el: "#Vue_Area",
        data: {
            User_Id:0,//当前选择的用户id
            SName: "",//用户名称
            User_Name:"",//对应用户名
            DepSel: 0,//对应的部门
            CmpAll:[],
            Cmp1: [],//企业权限列表
            SiteAll:[],
            Site1:[]//功能权限列表
        },
        methods: {
            //选择所有企业权限
            SelectAllCmp: function () {
                this._data.Cmp1 = this._data.CmpAll;
            },
            //撤销所有企业权限
            CancelAllCmp: function () {
                this._data.Cmp1 = [];
            },
            //选择所有功能权限
            SelectAllRole: function () {
                this._data.Site1 = this._data.SiteAll;
            },
            //取消所有功能权限
            CancelAllRole: function () {
                this._data.Site1 = [];
            },
            //提交修改
            SubmitEdit: function () {
                var _this = this;
                if (this._data.User_Id == 0 || this._data.User_Id == undefined || this._data.User_Id == null) {
                    $.alert({
                        backgroundDismiss: true,
                        title: '提示',
                        content: '请选择用户',
                        buttons: {
                            cancel: {
                                text: '确定',
                                btnClass: 'btn-primary',
                            }
                        }
                    })
                    return;
                }
                $.confirm({
                    title: '提示',
                    buttons: {
                        confirm: {
                            text: "确定",
                            btnClass: 'btn-primary',
                            action: function () {
                                //do something
                                if (SubmitEditFlg) {
                                    window.location.reload(); 
                                }
                            }
                        }
                    },
                    content: function () {
                        var self = this;
                        var formData = new FormData();
                        formData.append("User_Id", _this._data.User_Id);
                        formData.append("DepSel", _this._data.DepSel);
                        formData.append("Cmp1", _this._data.Cmp1.join(","));
                        formData.append("Site1", _this._data.Site1.join(","));
                        return $.ajax({
                            type: 'POST',
                            url: hosturl+'/Home/Edit_AdminInfo_Role',
                            data: formData,
                            processData: false,
                            contentType: false,
                        }).done(function (res) {
                            var json = isJsonString(res);
                            if (json.msg == "OK") {
                                self.setContentAppend('<div>成功更改</div>');
                                SubmitEditFlg = true;
                            } else if (json.msg == "refuse") {
                                self.setContentAppend('<div>登录超时，请重新登录</div>');
                                SubmitEditFlg = false;
                            } else if (json.msg == "error") {
                                self.setContentAppend('<div>更新失败</div>');
                                SubmitEditFlg = false;
                            }
                        }).fail(function () {
                            self.setContentAppend('<div>网络错误或服务器无响应</div>');
                            SubmitEditFlg = false;
                        })
                    }
                })
            }
        },
        mounted: function () {
            //
            var _this = this;
            this._data.CmpAll = Companys2.map(item => {
                return item.id
            });
            this._data.SiteAll = Roles.map(item => {
                return item.id
            });
            Roles.forEach(item => {
                _this._data.SiteAll.push(item.id);
                if (item.Child.length>0) {
                    item.Child.forEach(item2 => {
                        _this._data.SiteAll.push(item2.id);
                    })
                }
                
            })
        }
    });
});
/* 1.对话框  */
function Dialog_Add() {
    dialog_add.css("display", "block");
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
                text: '关闭'
            }
        }
    }); 
}

function Dialog_add_dep() {
    dialog_add_dep.css("display", "block");
    $.confirm({
        title: '添加部门',
        closeIcon: true,
        columnClass: '',
        //type:'green',
        content: dialog_add_dep,
        buttons: {
            add_Company: {
                text: '添加',
                btnClass: 'btn btn-success',
                action: function () {
                    add_Dep();
                }
            },
            close: {
                text: '关闭'
            }
        }
    }); 
}

function Dialog_add_per() {
    dialog_add_per.css("display", "block");
    $.confirm({
        title: '添加人员',
        closeIcon: true,
        columnClass: '',
        //type:'green',
        content: dialog_add_per,
        onOpen: function () {
            Vue_add_per = new Vue({
                el: '#dialog_add_per',
                data: {
                    CompanyList: [],
                    DepList: [],
                    ComID: 0,
                    DepID: 0,
                    NickName: '',
                    WorkID: '',
                    Username: '',
                    Password: '',

                },
                methods: {

                },
                mounted: function () {
                    this.CompanyList = Companys;
                    this.DepList = Companys[0].deps;
                },
                watch: {
                    ComID: function () {
                        var _this = this;
                        this.CompanyList.forEach(function (item) {
                            if (item.Id == _this.ComID) {
                                _this.DepList = item.deps;
                            }
                        })
                    },
                    DepID: function () {

                    }
                }
            })
        },
        buttons: {
            add_per: {
                text: '提交',
                btnClass: 'btn btn-success',
                action: function () {
                    add_per();
                }
            },
            close: {
                text:'取消'
            }
        }
    })
}
//删除企业或者部门
function Dialog_del_cmpAndDpt() {
    $.confirm({
        title: '提醒',
        closeIcon: true,
        columnClass: '',
        type:'red',
        content: "是否确认删除",
        buttons: {
            OK: {
                text: '确定',
                btnClass: 'btn btn-danger',
                action: function () {
                    Del_CmpAndDpt();
                }
            },
            close: {
                text:'取消'
            }
        }
    })
}
//删除员工
function Dialog_del_per() {
    $.confirm({
        title: '提醒',
        closeIcon: true,
        columnClass: '',
        type: 'red',
        content: "是否确认删除",
        buttons: {
            OK: {
                text: '确定',
                btnClass: 'btn btn-danger',
                action: function () {
                    Del_Per();
                }
            },
            close: {
                text: '取消'
            }
        }
    })
}
/* 2.添加请求  */
//添加企业
function add_Company() {
    var formData = new FormData();
    var Company = $("#add_Company").val();
    var SortID = $("#add_SortID").val();
    if (SortID.length > 5) {
        Alert1("提示", "企业只能五位", "btn-danger", "red");
        return;
    }
    formData.append("Company", Company);
    formData.append("SortID", SortID);
    
    $.ajax({
        type: 'POST',
        url: hosturl + '/Home/Add_Company2',
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
            }
            else if (res.msg == "refuse") {
                //关闭此对话框
                Alert1("提示", "登录超时", "btn-danger", "red");
            }
            else if (res.msg == "exist") {
                //关闭此对话框
                Alert1("提示。", "填写错误,请检查企业名称、编号是否重复", "btn-danger", "red");
            }
        },
        error: function () {
            Alert1("提示。", "服务器无响应", "btn-danger", "red");
        }
    });

}
//添加部门编号
function add_Dep() {
    var formData = new FormData();
    var CmpID = $("#addDpt_CmpID").val();
    var DptName = $("#addDpt_Name").val();
    var SortID = $("#addDpt_SortID").val();
    if (SortID.length > 2) {
        Alert1("提示", "部门只能两位", "btn-danger", "red");
        return;
    }
    formData.append("CmpID", CmpID);
    formData.append("DptName", DptName);
    formData.append("SortID", SortID);
    $.ajax({
        type: 'POST',
        url: hosturl + '/Home/Add_DepartmentInfo',
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
            }
            else if (res.msg == "refuse") {
                //关闭此对话框
                Alert1("提示", "登录超时", "btn-danger", "red");
            }
            else if (res.msg == "exist") {
                //关闭此对话框
                Alert1("提示。", "填写错误,请检查部门编号是否重复", "btn-danger", "red");
            }
        },
        error: function () {
            Alert1("提示。", "服务器无响应", "btn-danger", "red");
        }
    });
}
//添加人员
function add_per() {
    var NickName = Vue_add_per._data.NickName;
    var WorkID = Vue_add_per._data.WorkID;
    var Username = Vue_add_per._data.Username;
    var Password = Vue_add_per._data.Password;
    var ComID = Vue_add_per._data.ComID;
    var DepID = Vue_add_per._data.DepID;
    var formData = new FormData();
    formData.append("NickName", NickName);
    formData.append("WorkID", WorkID);
    formData.append("Username", Username);
    formData.append("Password", Password);
    formData.append("CmpID", ComID);
    formData.append("DptID", DepID);
    $.ajax({
        type: 'POST',
        url: hosturl + '/Home/Add_Person',
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
            }
            else if (res.msg == "refuse") {
                //关闭此对话框
                Alert1("提示", "登录超时", "btn-danger", "red");
            }
            else if (res.msg == "exist") {
                //关闭此对话框
                Alert1("提示。", "账号重复，请重新填写", "btn-danger", "red");
            }
        },
        error: function () {
            Alert1("提示。", "服务器无响应", "btn-danger", "red");
        }
    });
}
/* 删除请求  */
//删除部门或者企业
function Del_CmpAndDpt() {
    //CheckCmpId
    //CheckDptId
    var formData = new FormData();
    var DelTab = "";
    var Id = 0;
    if (CheckCmpId != 0) {
        DelTab = "Cmp";
        Id = CheckCmpId;
    } else if (CheckDptId != 0) {
        DelTab = "Dpt";
        Id = CheckDptId;
    } else {
        Alert1("提示", "请先选择要删除的项", "btn-danger", "red");
        return;
    }
    formData.append("DelTab", DelTab);
    formData.append("Id", Id);
    $.ajax({
        type: 'POST',
        url: hosturl + '/Home/Del_ComAndDpt',
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
                $.alert({
                    backgroundDismiss: true,
                    title: "提示",
                    type: "green",
                    content: "删除成功",
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
            }
            else if (res.msg == "refuse") {
                //关闭此对话框
                Alert1("提示", "登录超时", "btn-danger", "red");
            } else if (res.msg == "exist1") {
                Alert1("提示", "请先删除该企业下的部门", "btn-danger", "red");
            } else if (res.msg == "exist2") {
                Alert1("提示", "请先删除该部门下的员工", "btn-danger", "red");
            }
        },
        error: function () {
            Alert1("提示。", "服务器无响应", "btn-danger", "red");
        }
    });
}

function Del_Per() {
    //Del_Per
    var formData = new FormData();
    var DelTab = "Per";
    var Id = CheckPerId;
    if (Id == 0) {
        Alert1("提示", "请先选择要删除的项", "btn-danger", "red");
        return;
    }
    formData.append("DelTab", DelTab);
    formData.append("Id", Id);
    $.ajax({
        type: 'POST',
        url: hosturl + '/Home/Del_ComAndDpt',
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
                $.alert({
                    backgroundDismiss: true,
                    title: "提示",
                    type: "green",
                    content: "删除成功",
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
            }
            else if (res.msg == "refuse") {
                //关闭此对话框
                Alert1("提示", "登录超时", "btn-danger", "red");
            } else if (res.msg == "exist1") {
                Alert1("提示", "请先删除该企业下的部门", "btn-danger", "red");
            } else if (res.msg == "exist2") {
                Alert1("提示", "请先删除该部门下的员工", "btn-danger", "red");
            } else if (res.msg == "exist3") {
                Alert1("提示", "该员工已有操作记录无法删除", "btn-danger", "red");
            }
        },
        error: function () {
            Alert1("提示。", "服务器无响应", "btn-danger", "red");
        }
    });
}

/* 方法 */
//获取企业树
function getComPanyTree() {

    var tree = [];
    for (var i = 0; i < Companys.length; i++) {
        single = {};
        single.Id = Companys[i].Id;
        single.text = Companys[i].SName;
        single.tags = ['' + Companys[i].deps.length];
        //single.icon =  
        //single. = ;
        //single. = true;
        var childs = [];
        for (var j = 0; j < Companys[i].deps.length; j++) {
            var child = {};
            child.Id = Companys[i].deps[j].Id;
            child.text = Companys[i].deps[j].SName;
            childs.push(child);
        }
        single.nodes = childs;
        tree.push(single);
    }
    return tree;
}
/*
 * 0,0返回所有的信息
 * x,0 返回x企业的
 * x(0),x 返回x部门的
 * 获取人员树
 */
function getPeopleTree(ComId, DepId) {
    var tree = [];
    for (var i = 0; i < People.length; i++) {
        var flg = false;
        if (ComId == 0 && DepId == 0) {
            flg = true;
        }
        if (ComId == People[i].CmpID) {
            flg = true;
        }
        if (DepId == People[i].DptID) {
            flg = true;
        }
        if (flg) {
            single = {};
            single.Id = People[i].id;
            single.Username = People[i].Username;
            single.NickName = People[i].NickName;
            single.text = People[i].NickName;// + "(" + People[i].Username + ")";
            single.DptID = People[i].DptID;
            single.RoleCmp = People[i].RoleCmp;
            //single.RoleConfig = People[i].RoleConfig;
            single.SiteConfig = People[i].SiteConfig;
            tree.push(single);
        }

    }
    return tree;
}

function PeopleTreeInit(data) {
    $('#PeopleTree').treeview({ //
        levels: 1,
        expandIcon: "icon-folder1",
        collapseIcon: "icon-folder icon-folder_open",
        selectable: true,
        onNodeSelected: function (event, node) {
            var i = 0;
            CheckPerId = node.Id;
            Vue_Area._data.User_Id = node.Id;

            Vue_Area._data.SName = node.NickName;
            Vue_Area._data.User_Name = node.Username;
            Vue_Area._data.DepSel = node.DptID;
            if (node.RoleCmp == null) {
                Vue_Area._data.Cmp1 = [];
            } else {
                Vue_Area._data.Cmp1 = node.RoleCmp.split(",");
            }
            if (node.SiteConfig == null) {
                Vue_Area._data.Site1 = [];
            } else {
                Vue_Area._data.Site1 = node.SiteConfig.split(",");
            }

        },
        onNodeUnselected: function () {
            CheckPerId = 0;
        },
        data: data
    });
}
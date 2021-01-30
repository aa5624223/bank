var LoginFlg = false;
var hosturl = window.location.protocol + "//" + window.location.host;
$(document).ready(function () {
    var check1 = localStorage.getItem("check1");
    var check2 = localStorage.getItem("check2");
    var oldName = localStorage.getItem("userName");
    var oldPass = localStorage.getItem("passWord");
    //记住密码
    if (check1 == "true") {
        //login-username
        //login-password
        $("#login-username").val(oldName);
        $("#login-password").val(oldPass);
        $("#check1").prop('checked', true);
    } else {
        $("#login-username").val('');
        $("#login-password").val('');
        $("#check1").prop('checked', false);
    }
    //自动登录
    if (check1 == "true") {
        //发送登录请求
        $("#login-username").val(oldName);
        $("#login-password").val(oldPass);
        //Login();
        $("#check2").prop('checked', true);
    } else {
        $("#check2").prop('checked', false);
    }
    $("#login-password").bind("keydown", function (e) {
        // 兼容FF和IE和Opera
        var theEvent = e || window.event;
        var code = theEvent.keyCode || theEvent.which || theEvent.charCode;
        if (code == 13) {
            //回车执行查询
            if ($("#login-password").val()!="" && $("#login-username").val()!="") {
                Login();
            }
        }
    });
})
function Login(e) {
    if (LoginFlg) {
        return;
    }
    var formData = new FormData();
    var userName = $("#login-username").val();
    var Pass = $("#login-password").val();
    formData.append("username", userName);
    formData.append("password", Pass);

    $.ajax({
        type: 'POST',
        url: hosturl+"/Login/LoginEvn",
        data: formData,
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
            var json = isJsonString(res);
            if (json.msg == "OK") {
                //是否记住密码
                if ($("#check1").prop("checked") == true) {
                    localStorage.setItem("userName", userName);
                    localStorage.setItem("passWord", Pass);
                    localStorage.setItem("check1","true");
                } else {
                    localStorage.setItem("userName", "");
                    localStorage.setItem("passWord", "");
                    localStorage.setItem("check1","")
                }
                //是否自动登录
                if ($("#check1").prop("checked") == true) {
                    localStorage.setItem("userName", userName);
                    localStorage.setItem("passWord", Pass);
                    localStorage.setItem("check2", "true");
                } else {
                    localStorage.setItem("userName","");
                    localStorage.setItem("passWord","");
                    localStorage.setItem("check2","");
                }
                window.location.href = hosturl +'/Home/ServerRec2?method=1'
            } else {
                //账号密码错误
                $.alert({
                    backgroundDismiss: true,
                    title: '提示',
                    content: '账号或密码错误',
                    buttons: {
                        cancel: {
                            text: '确定',
                            btnClass: 'btn-primary',
                        }
                    }
                })
            }
        },
        error: function (error) {
            $.alert({
                backgroundDismiss: true,
                title: '提示',
                content: '网络错误或服务器无响应',
                buttons: {
                    cancel: {
                        text: '确定',
                        btnClass: 'btn-primary',
                    }
                }
            })
        }
    });
}
function isJsonString(str) {
    try {
        return JSON.parse(str);
    } catch (e) {
        return false;
    }
    return false;
}
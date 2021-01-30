//侧边栏的配置
//type=0可折叠 type=1不可折叠
var hosturl = window.location.protocol + "//" + window.location.host;
var LanguagePage = {
	"sProcessing": "数据获取中",
	"sLengthMenu": "显示 _MENU_ 条",
	"sZeroRecords": "没有您要搜索的内容",
	"sInfo": "从 _START_ 到  _END_ 条记录 总记录 _TOTAL_ 条",
	"sInfoEmpty": "记录数为0",
	"sInfoFiltered": "(全部记录 _MAX_ 条)",
	"sInfoPostFix": "",
	"sSearch": "搜索：",
	"sUrl": "",
	"oPaginate": {
		"sFirst": "第一页",
		"sPrevious": "上一页",
		"sNext": "下一页",
		"sLast": "最后一页"
	},
	buttons: {
		copyTitle: '表格已复制',
		copySuccess: {
			_: '%d 行数据已复制',
			1: '1 行数据已复制'
		}
	}
};
// Todays Date
var app;
var DiaLog;
// Loading
//个人信息对话框
var dialog_personInfo;
//修改密码对话框
var dialog_pasEdit;
$(function () {
	dialog_personInfo = $("#dialog_personInfo");
	dialog_pasEdit = $("#dialog_pasEdit");
	$("#loading-wrapper").fadeOut(3000);
});

$(function () {
	$(".app-actions .btn").click(function () {
		$(".app-actions .btn").removeClass("active");
		$(this).addClass("active");
	});
});

// Textarea characters left
$(function () {
	$('#characterLeft').text('140 characters left');
	$('#message').keydown(function () {
		var max = 140;
		var len = $(this).val().length;
		if (len >= max) {
			$('#characterLeft').text('You have reached the limit');
			$('#characterLeft').addClass('red');
			$('#btnSubmit').addClass('disabled');
		}
		else {
			var ch = max - len;
			$('#characterLeft').text(ch + ' characters left');
			$('#btnSubmit').removeClass('disabled');
			$('#characterLeft').removeClass('red');
		}
	});
});
// Todo list
$('.todo-body').on('click', 'li.todo-list', function () {
	$(this).toggleClass('done');
});
// Tasks
(function ($) {
	var checkList = $('.task-checkbox'),
		toDoCheck = checkList.children('input[type="checkbox"]');
	toDoCheck.each(function (index, element) {
		var $this = $(element),
			taskItem = $this.closest('.task-block');
		$this.on('click', function (e) {
			taskItem.toggleClass('task-checked');
		});
	});
})(jQuery);

// Tasks Important Active
$('.task-actions').on('click', '.important', function () {
	$(this).toggleClass('active');
});

// Tasks Important Active
$('.task-actions').on('click', '.star', function () {
	$(this).toggleClass('active');
});
$(document).ready(function () {
	app = new Vue({
		el: '#sidebar',
		data: {
			hosturl: hosturl,
			sideBar: SideAuth
		},
		beforeMount: function () {
			//获得需要激活的侧边栏
			this.sideBar.forEach(function (item) {
				var flg = false;
				item.Child.forEach(function (item2) {
					if (item2.SName == ViewTitle && item.SName !="我的桌面") {
						item2.active = true;
						flg = true;
					}
				})
				if (item.Child.length == 0) {
					if (item.SName == ViewTitle) {
						item.active = true;
						flg = true;
					}
				}
				if (flg) {
					item.active = true;
				}
			})
		}
	});
});


// Bootstrap JS ***********

// Tooltip
$(function () {
	$('[data-toggle="tooltip"]').tooltip()
})

$(function () {
	$('[data-toggle="popover"]').popover()
})

// Custom Sidebar JS
jQuery(function ($) {

	// Dropdown menu
	$(".sidebar-dropdown > a").click(function () {
		$(".sidebar-submenu").slideUp(200);
		if ($(this).parent().hasClass("active")) {
			$(".sidebar-dropdown").removeClass("active");
			$(this).parent().removeClass("active");
		} else {
			$(".sidebar-dropdown").removeClass("active");
			$(this).next(".sidebar-submenu").slideDown(200);
			$(this).parent().addClass("active");
		}
	});



	//toggle sidebar
	$("#toggle-sidebar").click(function () {
		$(".page-wrapper").toggleClass("toggled");
	});



	// Pin sidebar on click
	$("#pin-sidebar").click(function () {
		if ($(".page-wrapper").hasClass("pinned")) {
			// unpin sidebar when hovered
			$(".page-wrapper").removeClass("pinned");
			$("#sidebar").unbind("hover");
		} else {
			$(".page-wrapper").addClass("pinned");
			$("#sidebar").hover(
				function () {
					$(".page-wrapper").addClass("sidebar-hovered");
				},
				function () {
					$(".page-wrapper").removeClass("sidebar-hovered");
				}
			)
		}
	});



	// Pinned sidebar
	$(function () {
		$(".page-wrapper").hasClass("pinned");
		$("#sidebar").hover(
			function () {
				$(".page-wrapper").addClass("sidebar-hovered");
			},
			function () {
				$(".page-wrapper").removeClass("sidebar-hovered");
			}
		)
	});




	// Toggle sidebar overlay
	$("#overlay").click(function () {
		$(".page-wrapper").toggleClass("toggled");
	});



	// Added by Srinu 
	$(function () {
		// When the window is resized, 
		$(window).resize(function () {
			// When the width and height meet your specific requirements or lower
			if ($(window).width() <= 768) {
				$(".page-wrapper").removeClass("pinned");
			}
		});
		// When the window is resized, 
		$(window).resize(function () {
			// When the width and height meet your specific requirements or lower
			if ($(window).width() >= 768) {
				$(".page-wrapper").removeClass("toggled");
			}
		});
	});


});


// Chat JS
$(function () {
	$("#chat-circle").click(function () {
		$("#chat-circle").toggle('scale');
		$(".chat-box").toggle('scale');
	})

	$(".chat-box-toggle").click(function () {
		$("#chat-circle").toggle('scale');
		$(".chat-box").toggle('scale');
	})
})
//public methods
function isJsonString(str) {
	try {
		return JSON.parse(str);
	} catch (e) {
		return false;
	}
	return false;
}
function Alert1(title, content,btnClass,type) {
	$.alert({
		backgroundDismiss: true,
		title: title,
		type: type,
		content: content,
		buttons: {
			cancel: {
				text: '确定',
				btnClass: btnClass
			}
		}
	})
}
function AlertAndReload(title, content, btnClass) {
	$.alert({
		backgroundDismiss: true,
		title: title,
		content: content,
		buttons: {
			cancel: {
				text: '确定',
				btnClass: btnClass,
				actions: function () {
					window.location.reload();//刷新页面
				}
			}
		}
	})
}

function AlertAndReload2(title, content, btnClass, type) {
	$.confirm({
		backgroundDismiss: true,
		title: title,
		content: content,
		buttons: {
			cancel: {
				text: '确定',
				btnClass: btnClass,
				actions: function () {
					window.location.reload();//刷新页面
				}
			}
		}
	})
}


function fix(num, length) {
	return ('' + num).length < length ? ((new Array(length + 1)).join('0') + num).slice(-length) : '' + num;
}
var LoginOutFlg = false;
//退出登录
function LoginOut() {
	$.ajax({
		type: 'POST',
		url: hosturl + "/Login/LoginOut",
		data: '',
		dataType: 'JSON',
		cache: false,
		processData: false,
		contentType: false,
		beforeSend: function () {
			LoginOutFlg = true;
		},
		complete: function () {
			LoginOutFlg = false;
		},
		success: function (res) {
			if (res.msg == "OK") {
				//成功
				window.location.href = hosturl + '/Login/Login'
			} else {
				Alert1("提示", "服务器无响应", "btn-danger", "red");
			}
		}, error: function () {
			Alert1("提示", "服务器无响应", "btn-danger", "red");
		}
	})
}
//个人信息
function Dialog_PersonInfo() {
	dialog_personInfo.css("display", "block");
	$.ajax({
		type: 'POST',
		url: hosturl + '/Login/getUserInfo',
		data: "",
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
				$("#User_Username").val(res.Username);
				$("#User_WorkID").val(res.WorkID);
				$("#User_NickName").val(res.NickName);
			} else if (res.msg == "refuse") {
				Alert1("提示", "登录超时,无法获取个人信息", "btn-danger", "red");
			}
		},
		error: function () {
			Alert1("提示", "服务器无响应", "btn-danger", "red");
		}
	});
	DiaLog = $.confirm({
		title: '个人信息',
		closeIcon: true,
		columnClass: 'col-lg-4',
		content: dialog_personInfo,
		buttons: {
			cancel: {
				text:'关闭'
			}
		}
	})
}
//密码修改
function Dialog_PasEdit() {
	dialog_pasEdit.css("display", "block");
	
	DiaLog = $.confirm({
		title: '密码修改',
		closeIcon: true,
		columnClass: '',
		content: dialog_pasEdit,
		onOpen: function () {
			$("#Pas_Password").val("");
			$("#Pas_Password2").val("");
			$("#Pas_Password3").val("");
		},
		buttons: {
			cancel: {
				text: '取消'
			},
			OK: {
				text: '确定',
				btnClass: 'btn btn-primary',
				action: function () {
					var formData = new FormData();
					var Password = $("#Pas_Password").val();
					var Password2 = $("#Pas_Password2").val();
					var Password3 = $("#Pas_Password3").val();
					if (Password2 != Password3) {
						Alert1("提示", "两次密码输入不一致,请重新输入", "btn-danger", "red");
						return;
					}
					formData.append("Pas_Password", Password);
					formData.append("Pas_Password2", Password2);
					formData.append("Pas_Password3", Password3);

					$.ajax({
						type: 'POST',
						url: hosturl + '/Login/PasEdit',
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
								Alert1("提示", "密码修改成功", "btn-danger", "green");
							} else if (res.msg == "refuse") {
								Alert1("提示", "登录超时", "btn-danger", "red");
							} else if (res.msg == "NOTFOUNT") {
								Alert1("提示", "原始密码输入错误", "btn-danger", "red");
							}
						},
						error: function () {
							Alert1("提示", "服务器无响应", "btn-danger", "red");
						}
					});

				}
            }
		}
	})
}
function getUserFunction(PageName) {
	//SideAuth
	var UserAuth = "";
	for (var i = 0; i < SideAuth.length; i++) {
		for (var j = 0; j < SideAuth[i].Child.length;j++) {
			var item = SideAuth[i].Child[j];
			if (item.Child != undefined && item.SName == PageName) {
				for (var k = 0; k < item.Child.length; k++) {
					var item2 = item.Child[k];
					UserAuth += item2.SName+",";
				}
			}
			
		}
		if (SideAuth[i].SName == PageName) {
			for (var j = 0; j < SideAuth[i].Child.length; j++) {
				UserAuth += SideAuth[i].Child[j].SName;
			}
		}
	}

	return UserAuth;
}
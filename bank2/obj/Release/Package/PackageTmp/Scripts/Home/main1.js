//侧边栏的配置
//type=0可折叠 type=1不可折叠
var sideBar = [
	{
		title: "can die able",
		type: 0,
		icon: "icon-home2",
		submenu: [
			{
				href: "baidu.com",
				title: "菜单1"
			},
			{
				href: "17173.com",
				title: "菜单2"
			},
			{
				title: "菜单3"
			}
		]
	},
	{
		title: "不可折叠的",
		href: "baidu.com",
		type: 1,
		icon: "icon-circular-graph",
	}
];

// Todays Date
var app;
// Loading
$(function () {
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
		el: '#app',
		data: {
			sideBar: sideBar
		},
		mounted: function () {
			//获得需要激活的侧边栏
			this.sideBar.forEach(function (item) {
				var flg = false;
				item.Child.forEach(function (item2) {
					if (item2.SName == ViewTitle) {
						item2.active = true;
						flg = true;
					}
				})
				if (item.Child.length == 0) {
					if (item.SName == ViewTitle || (ViewTitle == "还款提醒" && item.SName == "我的桌面")) {
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
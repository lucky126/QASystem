// JavaScript Document

//左侧面板导航

$(document).ready(function () {
    var Menu_Out = $('.OutMenu li a').filter(".noclass");
    var Menu_In = $('.OutMenu li .InMenu');

    Menu_Out.first().addClass('active').next().slideDown('normal');

    Menu_Out.bind('click', function (event) {

        if ($(this).attr('class') != 'active') {
            Menu_In.slideUp('normal');
            $(this).next().stop(true, true).slideToggle('normal');
            Menu_Out.removeClass('active');
            $(this).addClass('active');
        }
    });
});


/*
排序的自增自减
*/
function sub_val() {
	if (document.getElementById('column_px').value > 0) {
		document.getElementById('column_px').value--;
	} else {
		return false;
	}
}
function add_val() {
	document.getElementById('column_px').value++;
}	
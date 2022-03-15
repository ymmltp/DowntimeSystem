$("#leftside-navigation .sub-menu > a").click(function (e) {
    var isClose = $(this).find('.fa-angle-down').length == 0 ? true : false;  // 判断当前的控件是否处于打开状态
    $("#leftside-navigation").find('.fa-angle-down').removeClass('fa-angle-down').addClass('fa-angle-right');
    $("#leftside-navigation ul ul").slideUp(), $(this).next().is(":visible") || $(this).next().slideDown(),
        e.stopPropagation();
    if (isClose) {
        $(this).find('.fa-angle-right').removeClass('fa-angle-right').addClass('fa-angle-down');
    }
})
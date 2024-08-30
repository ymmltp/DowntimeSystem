//#region 配合alert.css,显示弹出信息
function showWarning(text) {
    $('.alert').attr('class', 'alert');
    $('.alert').html(text).addClass('alert-warning').show().delay(3000).fadeOut();
}
function showInfo(text) {
    $('.alert').attr('class', 'alert');
    $('.alert').html(text).addClass('alert-info').show().delay(1500).fadeOut();
}
function showInfo_long(text) {
    $('.alert').attr('class', 'alert');
    $('.alert').html(text).addClass('alert-info').show().delay(8000).fadeOut();
}
function showSuccess(text) {
    $('.alert').attr('class', 'alert');
    $('.alert').html(text).addClass('alert-success').show().delay(1500).fadeOut();
}
function showError(text) {
    $('.alert').attr('class', 'alert');
    $('.alert').html(text).addClass('alert-danger').show().delay(3000).fadeOut();
}
//#endregion
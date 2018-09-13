//打开弹框,传入参数对象
function dialogOpen(options) {
    var _options = {
        id: 'dialogDefault',
        title: '弹框',
        width: 400,
        height: 200,
        content: null,
        url:'',
        closed: false,
        cache: false,
        modal: true,
    };
    $.extend(_options, options);

    var id = _options.id;
    var src = _options.url;
    var $html = $('#{0}'.format(id));
    if ($html.length == 0)
        $html = $('<div id="{0}" style="padding:5px;display:none;"></div>'.format(id, src)).appendTo("body");

    var content = '<iframe frameborder="0" style="height:99%;width:100%" src="{0}"></iframe>'.format(src);
    $html.dialog({
        title: _options.title,
        width: _options.width,
        height: _options.height,
        content: content,
        closed: false,
        cache: false,
        modal: true
    });
}

//关闭弹框,dialogId为弹框参数id
function dialogClose(dialogId) {
    $('#' + dialogId).dialog('close');
}

//右下角显示消息
function dialogMsg(msg) {
    $.messager.show({
        title: "操作提示",
        msg: msg,
        showType: 'slide',
        timeout: 3000
    });
}

//弹出警告消息框
function dialogError(msg) {
    $.messager.alert("操作提示", msg, "error");
}

//弹出确认框
function dialogComfirm(msg, succcess, cancel) {
    var _succcess = succcess || function () { };
    var _cancel = cancel || function () { };
    $.messager.confirm("操作提示", msg, function (data) {
        if (data) {
            _succcess();
        }
        else {
            _cancel();
        }
    });
}
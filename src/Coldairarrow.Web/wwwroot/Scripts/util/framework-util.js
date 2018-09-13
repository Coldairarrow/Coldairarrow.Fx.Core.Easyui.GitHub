//搜索表格
function searchGrid(searchBtnObj, gridSelector) {
    var $wrapper = $(searchBtnObj).closest("div.search_wrapper");
    if (!$wrapper || !$wrapper.length) {
        return;
    }

    var params = $wrapper.getValues();
    $(gridSelector).datagrid("load", params);
}

//加载动画
function loading(isLoading) {
    var loading = true;
    if (typeof (isLoading) != 'undefined')
        loading = isLoading;
    if (loading) {
        $('<div id="loadingMask" class="datagrid-mask"></div>')
            .css({
                display: "block",
                'z-index': 998,
                width: "100%",
                height: '100%'
            })
            .appendTo("body");
        $("<div id=\"loadingMaskMsg\" class=\"datagrid-mask-msg\"></div>")
            .html("加载中，请稍候。。。")
            .css({
                display: "block",
                'z-index': 999,
                left: ($(document.body).outerWidth(true) - 190) / 2,
                top: ($(window).height() - 45) / 2
            })
            .appendTo("body");
    } else {
        $("#loadingMask").remove();
        $("#loadingMaskMsg").remove();
    }
}

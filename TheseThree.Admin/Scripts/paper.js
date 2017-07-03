function onDel(i) {
    Ewin.confirm({ message: "确认要删除选择的数据吗？" }).on(function (e) {
        if (!e) {
            return;
        }
        var postdata = {};
        postdata.id = i;
        $.ajax({
            type: "post",
            url: "/Teaching/DelPaper",
            data: postdata,
            success: function (data) {
                var d = eval(data);
                if (d.status == "success") {
                    $.toast("删除成功", null);
                    $("#tikuTable").bootstrapTable('refresh');
                } else {
                    $.toast(d.status, null);
                }
            },
            error: function () {
                $.toast('Error');
            }
        });
    });
}

$(function () {
    //1.初始化Table
    var oTable = new TableInit();
    oTable.Init();

    //2.初始化Button的点击事件
    var oButtonInit = new ButtonInit();
    oButtonInit.Init();
});

var TableInit = function () {
    var oTableInit = new Object();
    oTableInit.Init = function () {
        $('#tikuTable').bootstrapTable({
            url: '/Teaching/GetPaper',
            method: 'get',
            toolbar: '#toolbar',
            // toolbarAlign: 'right',
            striped: true,
            cache: false,
            pagination: true,
            sortable: true,
            sortOrder: "asc",
            queryParams: oTableInit.queryParams,
            sidePagination: "server",
            pageNumber: 1,
            pageSize: 10,
            pageList: [10, 25, 50, 100],
            search: false,
            strictSearch: true,
            showColumns: false,
            showRefresh: false,
            minimumCountColumns: 1,
            clickToSelect: true,
            uniqueId: "ID",
            showToggle: false,
            cardView: false,
            detailView: false,
            columns: [
                {
                    field: 'Name',
                    title: '名称',
                    formatter: function (value, row, index) {
                        var e = '<a data-ajax="true" data-ajax-mode="replace" data-ajax-update="#main" href="/Teaching/PaperEdit/' + row.Id + '">' + row.Name + '</a> ';
                        return e;
                    }
                }, {
                    field: 'Count',
                    title: '题数'
                }, {
                    field: 'TotalCent',
                    title: '总分'
                }, {
                    field: 'Remark',
                    title: '科室'
                }, {
                    title: '操作',
                    formatter: function (value, row, index) {
                        var e = '<a data-ajax="true" data-ajax-mode="replace" data-ajax-update="#main" href="/Teaching/PaperEdit/' + row.Id + '">编辑</a> ';;
                        var d = '&nbsp;&nbsp;&nbsp;&nbsp;<button  type="button" class="btn" onclick="onDel(' + row.Id + ')">删除</button >';
                        return  e + d;
                    }
                }
            ]
        });
    };

    oTableInit.queryParams = function (params) {
        var temp = {
            limit: params.limit,
            offset: params.offset,
            name: $("#txt_name").val()
        };
        return temp;
    };
    return oTableInit;
};

var ButtonInit = function () {
    var oInit = new Object();
    var postdata = {};
    oInit.Init = function () {
        $("#btn_query").click(function () {
            $("#tikuTable").bootstrapTable('refresh');
        });
    };
    return oInit;
};
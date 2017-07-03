function onDel(i) {
    Ewin.confirm({ message: "确认要删除选择的数据吗？" }).on(function (e) {
        if (!e) {
            return;
        }
        var postdata = {};
        postdata.id = i;
        alert(1);
        $.ajax({
            type: "post",
            url: "/Teaching/DelTrain",
            data: postdata,
            success: function (data) {
                var d = eval(data);
                if (d.status == "success") {
                    $.toast("删除成功", null);
                    $("#trainTable").bootstrapTable('refresh');
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
        $('#trainTable').bootstrapTable({
            url: '/Teaching/GetTrain',
            method: 'get',
            toolbar: '#toolbar',
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
                    field: 'Zhuti',
                    title: '主题',
                    formatter: function (value, row, index) {
                        var e = '<a data-ajax="true" data-ajax-mode="replace" data-ajax-update="#main" href="/Teaching/EditTrain/' + row.Id + '">' + row.Zhuti + '</a> ';
                        return e;
                    }
                }, {
                    field: 'Org',
                    title: '主办方'
                }, {
                    field: 'Time',
                    title: '时间'
                }, {
                    field: 'Adress',
                    title: '地点'
                }, {
                    field: 'Score',
                    title: '学分'
                }, {
                    field: 'Type',
                    title: '类型',
                    formatter: function (value, row, index) {
                        if (row.Type == "0") {
                            return "院内培训";
                        }
                        if (row.Type == "1"){
                            return "院外培训";
                        }
                    }
                }, {
                    title: '操作',
                    formatter: function (value, row, index) {
                        var e = '&nbsp;&nbsp;<a data-ajax="true" data-ajax-mode="replace" data-ajax-update="#main" href="/Teaching/EditTrain/' + row.Id + '">编辑</a>';
                        var d = '&nbsp;&nbsp;<button  type="button" class="btn" onclick="onDel(' + row.Id + ')">删除</button >';
                        return e + d;
                    }
                }
            ]
        });
    };

    oTableInit.queryParams = function (params) {
        var temp = {
            limit: params.limit,
            offset: params.offset,
            name: $("#txt_name").val(),
            orgname: $("#txt_org_name").val(),
            orgtype: $("#sel_train_type option:selected").val()
        };
        return temp;
    };
    return oTableInit;
};

var ButtonInit = function () {
    var oInit = new Object();
    oInit.Init = function () {
        $("#btn_query").click(function () {
            $("#trainTable").bootstrapTable('refresh');
        });
    };
    return oInit;
};
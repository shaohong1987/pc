function onDel(i) {
    Ewin.confirm({ message: "确认要删除选择的数据吗？" }).on(function (e) {
        if (!e) {
            return;
        }
        var postdata = {};
        postdata.id = i;
        $.ajax({
            type: "post",
            url: "/Notice/DelNotice",
            data: postdata,
            success: function (data) {
                var d = eval(data);
                if (d.status == "success") {
                    $.toast("删除成功", null);
                    $("#noticeTable").bootstrapTable('refresh');
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
        $('#noticeTable').bootstrapTable({
            url: '/Notice/GetNotice',
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
                    field: 'Content',
                    title: '内容'
                }, {
                    field: 'GroupName',
                    title: '范围'
                },  {
                    field: 'Type',
                    title: '类型',
                    formatter: function (value, row, index) {
                        if (row.Type == 2) {
                            return "培训通知";
                        } else if(row.Type==1)
                        { return "考试通知" }
                    }
                }, {
                    field: 'Sendtime',
                    title: '时间'
                }, {
                    title: '操作',
                    formatter: function (value, row, index) {
                        var i = '<button  type="button" class="btn" onclick="onUpload(' + row.Id + ')">查看</button >';
                        var d = '&nbsp;&nbsp;<button  type="button" class="btn" onclick="onDel(' + row.Id + ')">删除</button >';
                        return i + d;
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
        $("#btn_add").click(function () {
            $("#mymodal").find(".form-control").val("");
            $('#mymodal').modal({
                show: true,
                backdrop: 'static'
            });
        });

        $("#btn_save").click(function () {
            postdata.name = $("#txt_new_name").val();
            if (postdata.name == null || postdata.name.length <= 0) {
                $.toast("请输入名称", null);
                return;
            }
            var sectionId = $("#hid_section_id").val();
            postdata.id = sectionId;
            $.ajax({
                type: "post",
                url: "/Notice/AddOrUpdateSubject",
                data: postdata,
                success: function (data) {
                    var d = eval(data);
                    if (d.status == "success") {
                        $('#mymodal').modal("hide");
                        $.toast("提交数据成功", null);
                        $("#noticeTable").bootstrapTable('refresh');
                    } else {
                        $.toast(d.status, null);
                    }
                },
                error: function () {
                    $.toast("Error", null);
                }
            });
        });

        $("#btn_query").click(function () {
            $("#noticeTable").bootstrapTable('refresh');
        });
    };
    return oInit;
};
function delAdv(i) {
    Ewin.confirm({ message: "确认要删除选择的数据吗？" }).on(function (e) {
        if (!e) {
            return;
        }
        var postdata = {};
        postdata.id = i;
        $.ajax({
            type: "post",
            url: "/Home/DelAdv",
            data: postdata,
            success: function (data) {
                var d = eval(data);
                if (d.status == "success") {
                    $.toast("删除成功", null);
                    $("#picTable").bootstrapTable('refresh');
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
    var oTable = new ExamStaticInit();
    oTable.Init();

    var oButtonInit = new ButtonInit();
    oButtonInit.Init();
});

var ExamStaticInit = function () {
    var oRableInit = new Object();
    oRableInit.Init = function () {
        $('#picTable').bootstrapTable({
            url: '/Home/GetPic',
            method: 'get',
            striped: true,
            cache: false,
            pagination: true,
            sortable: true,
            toolbar: '#toolbar',
            sortOrder: "asc",
            sidePagination: "server",
            pageNumber: 1,
            pageSize: 10,
            pageList: [10, 25, 50, 100],
            queryParams: oRableInit.queryParams,
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
                    title: '操作',
                    formatter: function (value, row) {
                        var d = '&nbsp;&nbsp;&nbsp;&nbsp;<button  type="button" class="btn" onclick="delAdv(' + row.Id + ');">删除</button >';
                        return d;
                    }
                },
                {
                    field: 'Title',
                    title: '标题'
                },
                {
                    field: 'Url',
                    title: '文件名'
                }
            ]
        });
    };

    oRableInit.queryParams = function (params) {
        var temp = {
            limit: params.limit,
            offset: params.offset
        };
        return temp;
    };
    return oRableInit;
};

var ButtonInit = function () {
    var oInit = new Object();
    var postdata = {};
    oInit.Init = function () {
        $("#btn_upload_pic").click(function() {
            $('#picmodal').modal({
                show: true,
                backdrop: 'static'
            });
        });

        $("#btn_save_pic").click(function() {
            var formData = new FormData();
            formData.append('files', $('#file_upload')[0].files[0]);
            formData.append('title', $("#txt_pic_name").val());
            $.ajax({
                url: '/Home/UploadPic',
                type: 'POST',
                cache: false,
                data: formData,
                processData: false,
                contentType: false,
                success: function () {
                    $('#picmodal').modal("hide");
                    $("#picTable").bootstrapTable('refresh');
                }
            });
        });
    };
    return oInit;
};
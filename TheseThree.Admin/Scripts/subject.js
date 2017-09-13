
function onUpload(i) {
    $("#hid_section_id_upload").val(i);
    $('#mymodal_more').modal({
        show: true,
        backdrop: 'static'
    });
}

function onEdit(i, name) {
    $("#myModalLabel").text("编辑");
    $("#txt_new_name").val(name);
    $("#hid_section_id").val(i);
    
}

function onShare(i, name) {
    $("#hid_share_tkid").val(i);
    $("#txt_tk_name").val(name);
    $('#mymodal_share').modal({
        show: true,
        backdrop: 'static'
    });
}

function onDel(i) {
    Ewin.confirm({ message: "确认要删除选择的数据吗？" }).on(function (e) {
        if (!e) {
            return;
        }
        var postdata = {};
        postdata.id = i;
        $.ajax({
            type: "post",
            url: "/Teaching/DelTiKu",
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
    $('#sel_team').multipleSelect();
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
            url: '/Teaching/GetTiKu',
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
                       var e = '<a data-ajax="true" data-ajax-mode="replace" data-ajax-update="#main" href="/Teaching/EditSection/' + row.Id + '">' + row.Name + '</a> ';
                       return e;
                   }
               }, {
                   field: 'Count',
                   title: '题数'
               }, {
                   field: 'Remark',
                   title: '创建者'
               }, {
                   title: '操作',
                   formatter: function (value, row, index) {
                       var ut = $("#hid_ut").val();
                       var x = "";
                       var i = '<button  type="button" class="btn" onclick="onUpload(' + row.Id + ')">导入题目</button >';
                       var e = '&nbsp;&nbsp;<button  type="button" class="btn" onclick="onEdit(' + row.Id + ',\'' + row.Name + '\')">修改</button >';
                       if (ut == 1) {
                          x= '&nbsp;&nbsp;<button  type="button" class="btn" onclick="onShare(' + row.Id + ',\'' + row.Name + '\')">分享题库</button >';
                       }
                       var d = '&nbsp;&nbsp;<button  type="button" class="btn" onclick="onDel(' + row.Id + ')">删除</button >';
                       return i + e +x+ d;
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
            $("#hid_section_id").val(-1);
            $('#mymodal').modal({
                show: true,
                backdrop: 'static'
            });
        });

        $("#btn_add_more").click(function () {
            $('#mymodal_more').modal({
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
                url: "/Teaching/AddOrUpdateSubject",
                data: postdata,
                success: function (data) {
                    var d = eval(data);
                    if (d.status == "success") {
                        $('#mymodal').modal("hide");
                        $.toast("提交数据成功", null);
                        $("#tikuTable").bootstrapTable('refresh');
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
            $("#tikuTable").bootstrapTable('refresh');
        });

        $("#btn_share").click(function () {
            var tkid=$("#hid_share_tkid").val();
            var sectionids = $("#sel_team").multipleSelect("getSelects");
            alert(sectionids);
            alert($("#sel_team").multipleSelect("getSelects", "text"));
            postdata.tkid = tkid;
            postdata.sectionids = sectionids;
            $.ajax({
                type: "post",
                url: "/Teaching/ShareSubject",
                data: postdata,
                success: function (data) {
                    var d = eval(data);
                    if (d.status == "success") {
                        $('#mymodal_share').modal("hide");
                        $.toast("保存成功", null);
                    } else {
                        $.toast(d.status, null);
                    }
                },
                error: function () {
                    $.toast("Error", null);
                }
            });
        });
      
        $("#btn_upload").click(function () {
            var formData = new FormData();
            formData.append('files', $('#file_upload')[0].files[0]);
            formData.append('sectionId', $("#hid_section_id_upload").val());
            $("#tip>ul").append("<li>如果文件太大可能需要花费几分钟上传，请耐心等待！</li>");
            $.ajax({
                url: '/Teaching/UploadSubject',
                type: 'POST',
                cache: false,
                data: formData,
                processData: false,
                contentType: false,
                success: function(d) {
                    $("#tip>ul").empty();
                    var msg = JSON.stringify(d.Message);
                    if (msg.length > 2) {
                        if (msg.indexOf(",") >= 0) {
                            var arr = msg.split(",");
                            for (var i = 0; i < arr.length; i++) {
                                $("#tip>ul").append("<li>上传失败！</li>");
                                $("#tip>ul").append("<li>" + arr[i] + "</li>");
                            }
                        } else {
                            $("#tip>ul").append("<li>上传失败！</li>");
                            $("#tip>ul").append("<li>" + msg + "</li>");
                        }
                    } else {
                        $("#tip>ul").append("<li>上传成功！</li>");
                        //$('#mymodal_more').modal("hide");
                        //$.toast('文件 ' + file.name + ' 已经上传成功');
                        $("#tikuTable").bootstrapTable('refresh');
                    }
                }
            });
        });
    };
    return oInit;
};
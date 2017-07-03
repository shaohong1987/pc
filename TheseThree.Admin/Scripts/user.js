$(function () {

    $('#sel_new_team').multipleSelect();

$("#file_upload").uploadify({
        'auto': false,
        'swf': "/Scripts/uploadify/uploadify.swf",
        'uploader': "/User/Upload",
        'buttonText': '请选择上传文件',
        'onUploadSuccess': function (file, data, response) {
            $("#tip>ul").empty();
            var d = eval('(' + data + ')');
            var msg = JSON.stringify(d.Message);
            if (msg.length > 2) {
                if (msg.indexOf(",") >= 0) {
                    alert(3);
                    var arr = msg.split(",");
                    for (var i = 0; i < arr.length; i++) {
                        $("#tip>ul").append("<li>" + arr[i] + "</li>");
                    }
                } else {
                    $("#tip>ul").append("<li>" + msg + "</li>");
                }    
            } else {
                $('#mymodal_more').modal("hide");
                $.toast('文件 ' + file.name + ' 已经上传成功');
                $("#endUserTable").bootstrapTable('refresh');
            }
        }
    });

    $('#mymodal_more').on('hide.bs.modal',
        function() {
            $("#tip>ul").empty();
        });

    //1.初始化Table
    var oTable = new TableInit();
    oTable.Init();

    //2.初始化Button的点击事件
    var oButtonInit = new ButtonInit();
    oButtonInit.Init();
});


var TableInit = function () {
    var oTableInit = new Object();
    //初始化Table
    oTableInit.Init = function() {
        $('#endUserTable').bootstrapTable({
            url: '/User/GetEndUSER', //请求后台的URL（*）
            method: 'get', //请求方式（*）
            toolbar: '#toolbar', //工具按钮用哪个容器
            // toolbarAlign: 'right',
            striped: true, //是否显示行间隔色
            cache: false, //是否使用缓存，默认为true，所以一般情况下需要设置一下这个属性（*）
            pagination: true, //是否显示分页（*）
            sortable: true, //是否启用排序
            sortOrder: "asc", //排序方式
            queryParams: oTableInit.queryParams, //传递参数（*）
            sidePagination: "server", //分页方式：client客户端分页，server服务端分页（*）
            pageNumber: 1, //初始化加载第一页，默认第一页
            pageSize: 10, //每页的记录行数（*）
            pageList: [10, 25, 50, 100], //可供选择的每页的行数（*）
            search: false, //是否显示表格搜索，此搜索是客户端搜索，不会进服务端，所以，个人感觉意义不大
            strictSearch: true,
            showColumns: false, //是否显示所有的列
            showRefresh: false, //是否显示刷新按钮
            minimumCountColumns: 1, //最少允许的列数
            clickToSelect: true, //是否启用点击选中行
            uniqueId: "ID", //每一行的唯一标识，一般为主键列
            showToggle: false, //是否显示详细视图和列表视图的切换按钮
            cardView: false, //是否显示详细视图
            detailView: false, //是否显示父子表
            columns: [
                {
                    checkbox: true
                }, {
                    field: 'Name',
                    title: '姓名'
                }, {
                    field: 'LoginId',
                    title: '工号'
                }, {
                    field: 'Deptname',
                    title: '科室'
                }, {
                    field: 'Phone',
                    title: '手机号'
                }, {
                    field: 'Zcname',
                    title: '职称'
                }, {
                    field: 'Gwname',
                    title: '职务'
                }, {
                    field: 'Lvname',
                    title: '层级'
                }, {
                    field: 'Dename',
                    title: '学历'
                }
                , {
                    field: 'Xzname',
                    title: '小组'
                }
            ]
        });
    };

    oTableInit.queryParams = function(params) {
        var temp = { 
            limit: params.limit, 
            offset: params.offset, 
            name: $("#txt_name").val(),
            phone: $("#txt_phone").val(),
            loginId: $("#txt_loginid").val(),
            deptname: $("#sel_deptname option:selected").text(),
            deptcode: $("#sel_deptname option:selected").val(),
            gwcode: $("#sel_gangwei option:selected").val(),
            gwname: $("#sel_gangwei option:selected").text(),
            zccode: $("#sel_zhichen option:selected").val(),
            zcname: $("#sel_zhichen option:selected").text(),
            lvcode: $("#sel_level option:selected").val(),
            lvname: $("#sel_level option:selected").text(),
            decode: $("#sel_degree option:selected").val(),
            dename: $("#sel_degree option:selected").text(),
            xzcode: $("#sel_team option:selected").val(),
            xzname: $("#sel_team option:selected").text()
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
            postdata.id = -1;
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

        $("#btn_edit").click(function () {
            var arrselections = $("#endUserTable").bootstrapTable('getSelections');
            if (arrselections.length > 1) {
                $.toast('只能选择一行进行编辑');
                return;
            }
            if (arrselections.length <= 0) {
                $.toast('请选择有效数据');
                return;
            }
            $("#myModalLabel").text("编辑");
            $("#txt_new_name").val(arrselections[0].Name);
            $("#txt_new_loginId").val(arrselections[0].LoginId);
            $("#txt_new_phone").val(arrselections[0].Phone);
            $("#sel_new_deptname").val(arrselections[0].Deptcode);
            $("#sel_new_gangwei").val(arrselections[0].Gwcode);
            $("#sel_new_zhichen").val(arrselections[0].Zccode);
            $("#sel_new_level").val(arrselections[0].Lvcode);
            $("#sel_new_degree").val(arrselections[0].Decode);
            $("#sel_new_team").multipleSelect("setSelects", [arrselections[0].Xzcode]);
            postdata.id = arrselections[0].Id;
            $('#mymodal').modal({
                show: true,
                backdrop: 'static'
            });
        });

        $("#btn_delete").click(function () {
            var arrselections = $("#endUserTable").bootstrapTable('getSelections');
            if (arrselections.length <= 0) {
                $.toast('请选择有效数据');
                return;
            }
            var ids = "";
            for (var i = 0; i < arrselections.length; i++) {
                ids += "," + arrselections[i].Id;
            }
            postdata.id = ids;
            Ewin.confirm({ message: "确认要删除选择的数据吗？" }).on(function (e) {
                if (!e) {
                    return;
                }
                $.ajax({
                    type: "post",
                    url: "/User/DelUser",
                    data: postdata,
                    success: function (data) {
                        var d = eval(data);
                        if (d.status == "success") {
                            $.toast("删除成功", null);
                            $("#endUserTable").bootstrapTable('refresh');
                        } else {
                            $.toast(d.status, null);
                        }
                    },
                    error: function () {
                        $.toast('Error');
                    },
                    complete: function () {

                    }

                });
            });
        });

        $("#btn_resetpassword").click(function () {
            var arrselections = $("#endUserTable").bootstrapTable('getSelections');
            if (arrselections.length <= 0) {
                $.toast('请选择有效数据');
                return;
            }
            var ids = "";
            for (var i = 0; i < arrselections.length; i++) {
                ids += "," + arrselections[i].Id;
            }
            postdata.id = ids;
            Ewin.confirm({ message: "确认要重置这些用户的密码？" }).on(function (e) {
                if (!e) {
                    return;
                }
                $.ajax({
                    type: "post",
                    url: "/User/ResetPwd",
                    data: postdata,
                    success: function (data) {
                        var d = eval(data);
                        if (d.status == "success") {
                            $.toast("重置密码成功", null);
                        } else {
                            $.toast(d.status, null);
                        }
                    },
                    error: function () {
                        $.toast('Error');
                    },
                    complete: function () {

                    }

                });
            });
        });

        $("#btn_save").click(function () {
            postdata.name = $("#txt_new_name").val();
            postdata.phone = $("#txt_new_phone").val();
            postdata.loginId = $("#txt_new_loginId").val();
            postdata.deptname = $('#sel_new_deptname option:selected').text();
            postdata.deptcode = $('#sel_new_deptname option:selected').val();
            postdata.gwcode = $('#sel_new_gangwei option:selected').val();
            postdata.gwname = $('#sel_new_gangwei option:selected').text();
            postdata.zccode = $('#sel_new_zhichen option:selected').val();
            postdata.zcname = $('#sel_new_zhichen option:selected').text();
            postdata.lvcode = $('#sel_new_level option:selected').val();
            postdata.lvname = $('#sel_new_level option:selected').text();
            postdata.decode = $('#sel_new_degree option:selected').val();
            postdata.dename = $('#sel_new_degree option:selected').text();
            postdata.xzcode = $("#sel_new_team").multipleSelect("getSelects");
            postdata.xzname = $("#sel_new_team").multipleSelect("getSelects", "text");
            if (postdata.name == null || postdata.name.length <= 0) {
                $.toast("请输入姓名", null);
                return;
            }
            if (postdata.phone == null || postdata.phone.length <= 0) {
                $.toast("请输入手机号", null);
                return;
            }
            if (postdata.loginId == null || postdata.loginId.length <= 0) {
                $.toast("请输入工号", null);
                return;
            }
            if (postdata.deptcode==undefined||postdata.deptcode <= 0) {
                $.toast("请选择科室", null);
                return;
            }
            if (postdata.gwcode == undefined ||postdata.gwcode <= 0) {
                $.toast("请选择职务", null);
                return;
            }
            if (postdata.zccode == undefined ||postdata.zccode <= 0) {
                $.toast("请选择职称", null);
                return;
            }
            if (postdata.lvcode == undefined || postdata.lvcode <= 0) {
                $.toast("请选择层级", null);
                return;
            }
            if (postdata.decode == undefined || postdata.decode <= 0) {
                $.toast("请选择学历", null);
                return;
            }
            $.ajax({
                type: "post",
                url: "/User/AddOrUpdateEndUser",
                data: postdata,
                success: function(data) {
                    var d = eval(data);
                    if (d.status == "success") {
                        $('#mymodal').modal("hide");
                        $.toast("提交数据成功", null);
                        $("#endUserTable").bootstrapTable('refresh');
                    } else {
                        $.toast(d.status, null);
                    }
                },
                error: function() {
                    $.toast("Error", null);
                },
                complete: function() {

                }
            });
        });

        $("#btn_query").click(function () {
            $("#endUserTable").bootstrapTable('refresh');
        });

        $("#btn_upload").click(function() {
            $('#file_upload').uploadify('upload', '*');
        });
    };
    return oInit;
};
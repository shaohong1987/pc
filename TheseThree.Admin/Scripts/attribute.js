$(function () {

    //1.初始化Table
    var oTable = new TableInit();
    oTable.Init();

    var gwTable = new GwTableInit();
    gwTable.Init();

    var levelTable = new LevelTableInit();
    levelTable.Init();

    var degreeTable = new DegreeTableInit();
    degreeTable.Init();

    //2.初始化Button的点击事件
    var oButtonInit = new ButtonInit();
    oButtonInit.Init();

});


var TableInit = function () {
    var oTableInit = new Object();
    //初始化Table
    oTableInit.Init = function () {
        $('#zcTable').bootstrapTable({
            url: '/Home/GetZC',         //请求后台的URL（*）
            method: 'get',                      //请求方式（*）
            toolbar: '#toolbar',                //工具按钮用哪个容器
            striped: true,                      //是否显示行间隔色
            cache: false,                       //是否使用缓存，默认为true，所以一般情况下需要设置一下这个属性（*）
            pagination: true,                   //是否显示分页（*）
            sortable: false,                     //是否启用排序
            sortOrder: "asc",                   //排序方式
            queryParams: oTableInit.queryParams,//传递参数（*）
            sidePagination: "server",           //分页方式：client客户端分页，server服务端分页（*）
            pageNumber: 1,                       //初始化加载第一页，默认第一页
            pageSize: 10,                       //每页的记录行数（*）
            pageList: [10, 25, 50, 100],        //可供选择的每页的行数（*）
            search: false,                       //是否显示表格搜索，此搜索是客户端搜索，不会进服务端，所以，个人感觉意义不大
            strictSearch: true,
            showColumns: true,                  //是否显示所有的列
            showRefresh: false,                  //是否显示刷新按钮
            minimumCountColumns: 2,             //最少允许的列数
            clickToSelect: true,                //是否启用点击选中行
            height: 500,                        //行高，如果没有设置height属性，表格自动根据记录条数觉得表格高度
            uniqueId: "ID",                     //每一行的唯一标识，一般为主键列
            showToggle: false,                    //是否显示详细视图和列表视图的切换按钮
            cardView: false,                    //是否显示详细视图
            detailView: false,                   //是否显示父子表
            columns: [{
                checkbox: true
            }, {
                field: 'Name',
                title: '名称'
            }]
        });
    };
    //得到查询的参数
    oTableInit.queryParams = function (params) {
        var temp = {   //这里的键的名字和控制器的变量名必须一直，这边改动，控制器也需要改成一样的
            limit: params.limit,   //页面大小
            offset: params.offset,  //页码
            wardname: $("#txt_search_name").val()
        };
        return temp;
    };
    return oTableInit;
};

var GwTableInit = function () {
    var oTeamTableInit = new Object();
    //初始化Table
    oTeamTableInit.Init = function () {
        $('#gwTable').bootstrapTable({
            url: '/Home/GetGw',         //请求后台的URL（*）
            method: 'get',                      //请求方式（*）
            toolbar: '#toolbar_gw',                //工具按钮用哪个容器
            striped: true,                      //是否显示行间隔色
            cache: false,                       //是否使用缓存，默认为true，所以一般情况下需要设置一下这个属性（*）
            pagination: true,                   //是否显示分页（*）
            sortable: false,                     //是否启用排序
            sortOrder: "asc",                   //排序方式
            queryParams: oTeamTableInit.queryParams,//传递参数（*）
            sidePagination: "server",           //分页方式：client客户端分页，server服务端分页（*）
            pageNumber: 1,                       //初始化加载第一页，默认第一页
            pageSize: 10,                       //每页的记录行数（*）
            pageList: [10, 25, 50, 100],        //可供选择的每页的行数（*）
            search: false,                       //是否显示表格搜索，此搜索是客户端搜索，不会进服务端，所以，个人感觉意义不大
            strictSearch: true,
            showColumns: true,                  //是否显示所有的列
            showRefresh: false,                  //是否显示刷新按钮
            minimumCountColumns: 2,             //最少允许的列数
            clickToSelect: true,                //是否启用点击选中行
            height: 500,                        //行高，如果没有设置height属性，表格自动根据记录条数觉得表格高度
            uniqueId: "ID",                     //每一行的唯一标识，一般为主键列
            showToggle: false,                    //是否显示详细视图和列表视图的切换按钮
            cardView: false,                    //是否显示详细视图
            detailView: false,                   //是否显示父子表
            columns: [{
                checkbox: true
            }, {
                field: 'Name',
                title: '职务'
            }]
        });
    };
    //得到查询的参数
    oTeamTableInit.queryParams = function (params) {
        var temp = {   //这里的键的名字和控制器的变量名必须一直，这边改动，控制器也需要改成一样的
            limit: params.limit,   //页面大小
            offset: params.offset,  //页码
            teamname: $("#txt_search_name_gw").val()
        };
        return temp;
    };
    return oTeamTableInit;
};

var LevelTableInit = function () {
    var oTeamTableInit = new Object();
    //初始化Table
    oTeamTableInit.Init = function () {
        $('#levelTable').bootstrapTable({
            url: '/Home/GetLevel',         //请求后台的URL（*）
            method: 'get',                      //请求方式（*）
            toolbar: '#toolbar_level',                //工具按钮用哪个容器
            striped: true,                      //是否显示行间隔色
            cache: false,                       //是否使用缓存，默认为true，所以一般情况下需要设置一下这个属性（*）
            pagination: true,                   //是否显示分页（*）
            sortable: false,                     //是否启用排序
            sortOrder: "asc",                   //排序方式
            queryParams: oTeamTableInit.queryParams,//传递参数（*）
            sidePagination: "server",           //分页方式：client客户端分页，server服务端分页（*）
            pageNumber: 1,                       //初始化加载第一页，默认第一页
            pageSize: 10,                       //每页的记录行数（*）
            pageList: [10, 25, 50, 100],        //可供选择的每页的行数（*）
            search: false,                       //是否显示表格搜索，此搜索是客户端搜索，不会进服务端，所以，个人感觉意义不大
            strictSearch: true,
            showColumns: true,                  //是否显示所有的列
            showRefresh: false,                  //是否显示刷新按钮
            minimumCountColumns: 2,             //最少允许的列数
            clickToSelect: true,                //是否启用点击选中行
            height: 500,                        //行高，如果没有设置height属性，表格自动根据记录条数觉得表格高度
            uniqueId: "ID",                     //每一行的唯一标识，一般为主键列
            showToggle: false,                    //是否显示详细视图和列表视图的切换按钮
            cardView: false,                    //是否显示详细视图
            detailView: false,                   //是否显示父子表
            columns: [{
                checkbox: true
            }, {
                field: 'Name',
                title: '层级'
            }]
        });
    };
    //得到查询的参数
    oTeamTableInit.queryParams = function (params) {
        var temp = {   //这里的键的名字和控制器的变量名必须一直，这边改动，控制器也需要改成一样的
            limit: params.limit,   //页面大小
            offset: params.offset,  //页码
            teamname: $("#txt_search_name_level").val()
        };
        return temp;
    };
    return oTeamTableInit;
};

var DegreeTableInit = function () {
    var oTeamTableInit = new Object();
    //初始化Table
    oTeamTableInit.Init = function () {
        $('#degreeTable').bootstrapTable({
            url: '/Home/GetDegree',         //请求后台的URL（*）
            method: 'get',                      //请求方式（*）
            toolbar: '#toolbar_degree',                //工具按钮用哪个容器
            striped: true,                      //是否显示行间隔色
            cache: false,                       //是否使用缓存，默认为true，所以一般情况下需要设置一下这个属性（*）
            pagination: true,                   //是否显示分页（*）
            sortable: false,                     //是否启用排序
            sortOrder: "asc",                   //排序方式
            queryParams: oTeamTableInit.queryParams,//传递参数（*）
            sidePagination: "server",           //分页方式：client客户端分页，server服务端分页（*）
            pageNumber: 1,                       //初始化加载第一页，默认第一页
            pageSize: 10,                       //每页的记录行数（*）
            pageList: [10, 25, 50, 100],        //可供选择的每页的行数（*）
            search: false,                       //是否显示表格搜索，此搜索是客户端搜索，不会进服务端，所以，个人感觉意义不大
            strictSearch: true,
            showColumns: true,                  //是否显示所有的列
            showRefresh: false,                  //是否显示刷新按钮
            minimumCountColumns: 2,             //最少允许的列数
            clickToSelect: true,                //是否启用点击选中行
            height: 500,                        //行高，如果没有设置height属性，表格自动根据记录条数觉得表格高度
            uniqueId: "ID",                     //每一行的唯一标识，一般为主键列
            showToggle: false,                    //是否显示详细视图和列表视图的切换按钮
            cardView: false,                    //是否显示详细视图
            detailView: false,                   //是否显示父子表
            columns: [{
                checkbox: true
            }, {
                field: 'Name',
                title: '学历'
            }]
        });
    };
    //得到查询的参数
    oTeamTableInit.queryParams = function (params) {
        var temp = {   //这里的键的名字和控制器的变量名必须一直，这边改动，控制器也需要改成一样的
            limit: params.limit,   //页面大小
            offset: params.offset,  //页码
            teamname: $("#txt_search_name_degree").val()
        };
        return temp;
    };
    return oTeamTableInit;
};


var ButtonInit = function () {
    var oInit = new Object();
    var postdata = {};

    oInit.Init = function () {
        $("#btn_add").click(function () {
            $("#myModalLabel").text("新增");
            $("#mymodal").find(".form-control").val("");
            postdata.id = -1;
            $('#mymodal').modal({
                show: true,
                backdrop: 'static'
            });
        });

        $("#btn_save").click(function () {
            var wardName = $("#txt_name").val();
            if (wardName != null && wardName.length > 0) {
                postdata.wardname = wardName;
                $.ajax({
                    type: "post",
                    url: "/Home/AddOrUpdateZC",
                    data: postdata,
                    success: function (data) {
                        var d = eval(data);
                        if (d.status == "success") {
                            $('#mymodal').modal("hide");
                            $.toast("提交数据成功", null);
                            $("#zcTable").bootstrapTable('refresh');
                        } else {
                            $.toast(d.status, null);
                        }
                    },
                    error: function () {
                        $.toast("Error", null);
                    },
                    complete: function () {

                    }
                });
            }
        });

        $("#btn_edit").click(function () {
            var arrselections = $("#zcTable").bootstrapTable('getSelections');
            if (arrselections.length > 1) {
                $.toast('只能选择一行进行编辑');
                return;
            }
            if (arrselections.length <= 0) {
                $.toast('请选择有效数据');

                return;
            }
            $("#myModalLabel").text("编辑");
            $("#mymodal").find(".form-control").val(arrselections[0].Name);
            postdata.id = arrselections[0].Id;
            postdata.type = 0;
            $('#mymodal').modal({
                show: true,
                backdrop: 'static'
            });
        });

        $("#btn_delete").click(function () {
            var arrselections = $("#zcTable").bootstrapTable('getSelections');
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
                    url: "/Home/DeleteZC",
                    data: postdata,
                    success: function (data) {
                        var d = eval(data);
                        if (d.status == "success") {
                            $.toast("提交数据成功", null);
                            $("#zcTable").bootstrapTable('refresh');
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

        $("#btn_query").click(function () {
            $("#zcTable").bootstrapTable('refresh');
        });

        $("#btn_add_gw").click(function () {
            $("#myModalLabel").text("新增");
            $("#mymodal").find(".form-control").val("");
            postdata.id = -1;
            postdata.type = 1;
            $('#mymodal_gw').modal({
                show: true,
                backdrop: 'static'
            });
        });

        $("#btn_edit_gw").click(function () {
            var arrselections = $("#gwTable").bootstrapTable('getSelections');
            if (arrselections.length > 1) {
                $.toast('只能选择一行进行编辑');
                return;
            }
            if (arrselections.length <= 0) {
                $.toast('请选择有效数据');

                return;
            }
            $("#myModalLabel_gw").text("编辑");
            $("#mymodal_gw").find(".form-control").val(arrselections[0].Name);
            postdata.id = arrselections[0].Id;
            postdata.type = 1;
            $('#mymodal_gw').modal({
                show: true,
                backdrop: 'static'
            });
        });

        $("#btn_delete_gw").click(function () {
            var arrselections = $("#gwTable").bootstrapTable('getSelections');
            if (arrselections.length <= 0) {
                $.toast('请选择有效数据');
                return;
            }
            var ids = "";
            for (var i = 0; i < arrselections.length; i++) {
                ids += "," + arrselections[i].Id;
            }
            postdata.id = ids;
            postdata.type = 1;
            Ewin.confirm({ message: "确认要删除选择的数据吗？" }).on(function (e) {
                if (!e) {
                    return;
                }
                $.ajax({
                    type: "post",
                    url: "/Home/DeleteGW",
                    data: postdata,
                    success: function (data) {
                        var d = eval(data);
                        if (d.status == "success") {
                            $.toast("提交数据成功", null);
                            $("#gwTable").bootstrapTable('refresh');
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

        $("#btn_save_gw").click(function () {
            var wardName = $("#txt_name_gw").val();
            if (wardName != null && wardName.length > 0) {
                postdata.wardname = wardName;
                $.ajax({
                    type: "post",
                    url: "/Home/AddOrUpdateGW",
                    data: postdata,
                    success: function (data) {
                        var d = eval(data);
                        if (d.status == "success") {
                            $('#mymodal_gw').modal("hide");
                            $.toast("提交数据成功", null);
                            $("#gwTable").bootstrapTable('refresh');
                        } else {
                            $.toast(d.status, null);
                        }
                    },
                    error: function () {
                        $.toast("Error", null);
                    },
                    complete: function () {

                    }
                });
            }
        });

        $("#btn_query_gw").click(function () {
            $("#gwTable").bootstrapTable('refresh');
        });


        $("#btn_add_level").click(function () {
            $("#myModalLabel_level").text("新增");
            $("#mymodal_level").find(".form-control").val("");
            postdata.id = -1;
            postdata.type = 1;
            $('#mymodal_level').modal({
                show: true,
                backdrop: 'static'
            });
        });

        $("#btn_edit_level").click(function () {
            var arrselections = $("#levelTable").bootstrapTable('getSelections');
            if (arrselections.length > 1) {
                $.toast('只能选择一行进行编辑');
                return;
            }
            if (arrselections.length <= 0) {
                $.toast('请选择有效数据');

                return;
            }
            $("#myModalLabel_level").text("编辑");
            $("#mymodal_level").find(".form-control").val(arrselections[0].Name);
            postdata.id = arrselections[0].Id;
            postdata.type = 1;
            $('#mymodal_level').modal({
                show: true,
                backdrop: 'static'
            });
        });

        $("#btn_delete_level").click(function () {
            var arrselections = $("#levelTable").bootstrapTable('getSelections');
            if (arrselections.length <= 0) {
                $.toast('请选择有效数据');
                return;
            }
            var ids = "";
            for (var i = 0; i < arrselections.length; i++) {
                ids += "," + arrselections[i].Id;
            }
            postdata.id = ids;
            postdata.type = 1;
            Ewin.confirm({ message: "确认要删除选择的数据吗？" }).on(function (e) {
                if (!e) {
                    return;
                }
                $.ajax({
                    type: "post",
                    url: "/Home/DeleteLevel",
                    data: postdata,
                    success: function (data) {
                        var d = eval(data);
                        if (d.status == "success") {
                            $.toast("提交数据成功", null);
                            $("#levelTable").bootstrapTable('refresh');
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

        $("#btn_save_level").click(function () {
            var wardName = $("#txt_name_level").val();
            if (wardName != null && wardName.length > 0) {
                postdata.wardname = wardName;
                $.ajax({
                    type: "post",
                    url: "/Home/AddOrUpdateLevel",
                    data: postdata,
                    success: function (data) {
                        var d = eval(data);
                        if (d.status == "success") {
                            $('#mymodal_level').modal("hide");
                            $.toast("提交数据成功", null);
                            $("#levelTable").bootstrapTable('refresh');
                        } else {
                            $.toast(d.status, null);
                        }
                    },
                    error: function () {
                        $.toast("Error", null);
                    },
                    complete: function () {

                    }
                });
            }
        });

        $("#btn_query_level").click(function () {
            $("#levelTable").bootstrapTable('refresh');
        });


        $("#btn_add_degree").click(function () {
            $("#myModalLabel_degree").text("新增");
            $("#mymodal_degree").find(".form-control").val("");
            postdata.id = -1;
            postdata.type = 1;
            $('#mymodal_degree').modal({
                show: true,
                backdrop: 'static'
            });
        });

        $("#btn_edit_degree").click(function () {
            var arrselections = $("#degreeTable").bootstrapTable('getSelections');
            if (arrselections.length > 1) {
                $.toast('只能选择一行进行编辑');
                return;
            }
            if (arrselections.length <= 0) {
                $.toast('请选择有效数据');

                return;
            }
            $("#myModalLabel_degree").text("编辑");
            $("#mymodal_degree").find(".form-control").val(arrselections[0].Name);
            postdata.id = arrselections[0].Id;
            postdata.type = 1;
            $('#mymodal_degree').modal({
                show: true,
                backdrop: 'static'
            });
        });

        $("#btn_delete_degree").click(function () {
            var arrselections = $("#degreeTable").bootstrapTable('getSelections');
            if (arrselections.length <= 0) {
                $.toast('请选择有效数据');
                return;
            }
            var ids = "";
            for (var i = 0; i < arrselections.length; i++) {
                ids += "," + arrselections[i].Id;
            }
            postdata.id = ids;
            postdata.type = 1;
            Ewin.confirm({ message: "确认要删除选择的数据吗？" }).on(function (e) {
                if (!e) {
                    return;
                }
                $.ajax({
                    type: "post",
                    url: "/Home/DeleteDegree",
                    data: postdata,
                    success: function (data) {
                        var d = eval(data);
                        if (d.status == "success") {
                            $.toast("提交数据成功", null);
                            $("#degreeTable").bootstrapTable('refresh');
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

        $("#btn_save_degree").click(function () {
            var wardName = $("#txt_name_degree").val();
            if (wardName != null && wardName.length > 0) {
                postdata.wardname = wardName;
                $.ajax({
                    type: "post",
                    url: "/Home/AddOrUpdateDegree",
                    data: postdata,
                    success: function (data) {
                        var d = eval(data);
                        if (d.status == "success") {
                            $('#mymodal_degree').modal("hide");
                            $.toast("提交数据成功", null);
                            $("#degreeTable").bootstrapTable('refresh');
                        } else {
                            $.toast(d.status, null);
                        }
                    },
                    error: function () {
                        $.toast("Error", null);
                    },
                    complete: function () {

                    }
                });
            }
        });

        $("#btn_query_degree").click(function () {
            $("#degreeTable").bootstrapTable('refresh');
        });
    };

    return oInit;
};
function changeTest() {
    var paper = $("#sel_paper option:selected").val();
    var arr = paper.split('-');
    $("#txt_total_score").val(arr[1]);
}

function onDel(i, j) {
    Ewin.confirm({ message: "确认要删除选择的数据吗？" }).on(function (e) {
        if (!e) {
            return;
        }
        var postdata = {};
        postdata.id = i;
        postdata.userType = j;
        postdata.examid = $("#examid").val();
        $.ajax({
            type: "post",
            url: "/Teaching/DelExamUser",
            data: postdata,
            success: function (data) {
                var d = eval(data);
                if (d.status == "success") {
                    $.toast("删除成功", null);
                    refreshTable(j);
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

function refreshTable(i) {
    if (i == 0) {
        $("#ksTable").bootstrapTable('refresh');
    } else {
        $("#qdTable").bootstrapTable('refresh');
    }
}

$(function () {

    $('#txt_end_time').datetimepicker({
        language:  'zh-CN',
        weekStart: 1,
        todayBtn: 1,
        autoclose: 1,
        todayHighlight: 1,
        startView: 2,
        forceParse: 0,
        showMeridian: 1
    });

    $('#txt_start_time').datetimepicker({
        language: 'zh-CN',
        weekStart: 1,
        todayBtn: 1,
        autoclose: 1,
        todayHighlight: 1,
        startView: 2,
        forceParse: 0,
        showMeridian: 1
    });
    //1.初始化Table
    var oTable = new KsInit();
    oTable.Init();

    var oRecognized = new JkInit();
    oRecognized.Init();

    var oTiMu = new UserInit();
    oTiMu.Init();

    //2.初始化Button的点击事件
    var oButtonInit = new ButtonInit();
    oButtonInit.Init();
});

var KsInit = function () {
    var oRableInit = new Object();
    oRableInit.Init = function () {
        $('#ksTable').bootstrapTable({
            url: '/Teaching/GetKsUserForExam',
            method: 'get',
            striped: true,
            cache: false,
            pagination: true,
            sortable: true,
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
                    field: 'Name',
                    title: '姓名'
                }, {
                    field: 'LoginId',
                    title: '工号'
                }, {
                    field: 'Deptname',
                    title: '科室'
                }, {
                    title: '操作',
                    formatter: function (value, row) {
                        var d = '&nbsp;&nbsp;&nbsp;&nbsp;<button  type="button" class="btn" onclick="onDel(' + row.Id + ',0)">删除</button >';
                        return d;
                    }
                }
            ]
        });
    };

    oRableInit.queryParams = function (params) {
        var temp = {
            limit: params.limit,
            offset: params.offset,
            examid: $("#examid").val(),
            name: $("#txt_ks_name").val(),
            loginId: $("#txt_ks_loginId").val(),
            deptname: $("#sel_dept option:selected").text(),
            deptcode: $("#sel_dept option:selected").val(),
            gwcode: $("#sel_gangwei option:selected").val(),
            gwname: $("#sel_gangwei option:selected").text(),
            zccode: $("#sel_zhichen option:selected").val(),
            zcname: $("#sel_zhichen option:selected").text(),
            lvcode: $("#sel_level option:selected").val(),
            lvname: $("#sel_level option:selected").text(),
            xzcode: $("#sel_team option:selected").val(),
            xzname: $("#sel_team option:selected").text()
        };
        return temp;
    };
    return oRableInit;
};

var JkInit = function () {
    var oRableInit = new Object();
    oRableInit.Init = function () {
        $('#qdTable').bootstrapTable({
            url: '/Teaching/GetUserForExam',
            method: 'get',
            striped: true,
            cache: false,
            pagination: true,
            sortable: true,
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
                    field: 'Name',
                    title: '姓名'
                }, {
                    field: 'LoginId',
                    title: '工号'
                }, {
                    field: 'Deptname',
                    title: '科室'
                }, {
                    title: '操作',
                    formatter: function (value, row) {
                        var d = '&nbsp;&nbsp;&nbsp;&nbsp;<button  type="button" class="btn" onclick="onDel(' + row.Id + ',1)">删除</button >';
                        return d;
                    }
                }
            ]
        });
    };

    oRableInit.queryParams = function (params) {
        var temp = {
            limit: params.limit,
            offset: params.offset,
            examId: $("#examid").val()
        };
        return temp;
    };

    return oRableInit;
};

var UserInit = function () {
    var oRableInit = new Object();
    oRableInit.Init = function () {
        $('#userTable').bootstrapTable({
            url: '/User/GetUser',
            method: 'get',
            striped: true,
            cache: false,
            pagination: true,
            sortable: true,
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
                {checkbox:true},
                {
                    field: 'Name',
                    title: '姓名'
                }, {
                    field: 'LoginId',
                    title: '工号'
                }, {
                    field: 'Deptname',
                    title: '科室'
                }
            ]
        });
    };

    oRableInit.queryParams = function (params) {
        var temp = {
            limit: params.limit,
            offset: params.offset,
            examid: $("#examid").val(),
            name: $("#txt_user_name").val(),
            loginId: $("#txt_user_loginId").val(),
            deptname: $("#sel_user_ks option:selected").text(),
            deptcode: $("#sel_user_ks option:selected").val(),
            gwcode: $("#sel_user_gw option:selected").val(),
            gwname: $("#sel_user_gw option:selected").text(),
            zccode: $("#sel_user_zc option:selected").val(),
            zcname: $("#sel_user_zc option:selected").text(),
            lvcode: $("#sel_user_level option:selected").val(),
            lvname: $("#sel_user_level option:selected").text(),
            xzcode: $("#sel_user_xz option:selected").val(),
            xzname: $("#sel_user_xz option:selected").text()
        };
        return temp;
    };

    return oRableInit;
};

var ButtonInit = function () {
    var oInit = new Object();
    var postdata = {};
    oInit.Init = function () {
        $("#btn_query").click(function () {

            $("#userTable").bootstrapTable('refresh');
        });
        
        $("#btn_kaos_query").click(function () {
            $("#ksTable").bootstrapTable('refresh');
        });

        $("#btn_save_exam").click(function () {
            var postdata = {};
            postdata.examname = $("#txt_name").val();
            postdata.examid = $("#examid").val();
            postdata.paper = $("#sel_paper option:selected").val();
            postdata.startTime = $("#txt_start_time").val();
            postdata.endTime = $("#txt_end_time").val();
            postdata.address = $("#txt_address").val();
            postdata.jigescroe = $("#txt_jige_score").val();
            $.ajax({
                type: "post",
                url: "/Teaching/SaveExam",
                data: postdata,
                success: function (data) {
                    var d = eval(data);
                    if (d.status == "success") {
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

        //保存考生或签到人
        $("#btn_save").click(function () {
            var arrselections = $("#userTable").bootstrapTable('getSelections');
            if (arrselections.length <= 0) {
                $.toast('请选择有效数据');
                return;
            }
            var ids = "";
            for (var i = 0; i < arrselections.length; i++) {
                ids += "," + arrselections[i].Id;
            }
            postdata.id = ids;
            postdata.userType = $("#hid_user_type").val();
            postdata.examid = $("#examid").val();
            $.ajax({
                type: "post",
                url: "/Teaching/AddExamUser",
                data: postdata,
                success: function (data) {
                    var d = eval(data);
                    if (d.status == "success") {
                        $('#mymodal').modal("hide");
                        $.toast("添加成功", null);
                        refreshTable(postdata.userType);
                    } else {
                        $.toast(d.status, null);
                    }
                },
                error: function () {
                    $.toast("Error", null);
                }
            });
        });

        $("#btn_kaos_add").click(function () {
            $("#hid_user_type").val(0);
            $("#userTable").bootstrapTable('refresh');
            $('#mymodal').modal({
                show: true,
                backdrop: 'static'
            });
        });

        $("#btn_qiandao_add").click(function () {
            $("#hid_user_type").val(1);
            $("#userTable").bootstrapTable('refresh');
            $('#mymodal').modal({
                show: true,
                backdrop: 'static'
            });
        });
    };
    return oInit;
};
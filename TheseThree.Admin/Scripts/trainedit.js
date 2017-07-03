function onDel(i) {
    Ewin.confirm({ message: "确认要删除选择的数据吗？" }).on(function (e) {
        if (!e) {
            return;
        }
        var postdata = {};
        postdata.id = i;
        postdata.examid = $("#trainid").val();
        $.ajax({
            type: "post",
            url: "/Teaching/DelTrainUser",
            data: postdata,
            success: function (data) {
                var d = eval(data);
                if (d.status == "success") {
                    $.toast("删除成功", null);
                    $("#ksTable").bootstrapTable('refresh');
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

function onDelTd(i) {
    Ewin.confirm({ message: "确认要删除选择的数据吗？" }).on(function (e) {
        if (!e) {
            return;
        }
        var postdata = {};
        postdata.id = i;
        postdata.examid = $("#trainid").val();
        $.ajax({
            type: "post",
            url: "/Teaching/DelTrainDetail",
            data: postdata,
            success: function (data) {
                var d = eval(data);
                if (d.status == "success") {
                    $.toast("删除成功", null);
                    $("#trainDetailTable").bootstrapTable('refresh');
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

function onDelFj(i,j) {
    Ewin.confirm({ message: "确认要删除选择的数据吗？" }).on(function (e) {
        if (!e) {
            return;
        }
        var postdata = {};
        postdata.id = i;
        postdata.type = j;
        postdata.examid = $("#trainid").val();
        $.ajax({
            type: "post",
            url: "/Teaching/DelTrainFj",
            data: postdata,
            success: function (data) {
                var d = eval(data);
                if (d.status == "success") {
                    $.toast("删除成功", null);
                    $("#kjmodal").bootstrapTable('refresh');
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

    $("#file_upload").uploadify({
        'auto': false,
        'swf': "/Scripts/uploadify/uploadify.swf",
        'uploader': "/Teaching/UploadFuJian",
        'buttonText': '请选择上传文件',
        'fileSizeLimit': '10MB',
        'multi': false,
        'onUploadSuccess': function (file, data, response) {
            $('#kjmodal').modal("hide");
            $.toast('文件 ' + file.name + ' 已经上传成功');
            $("#fjTable").bootstrapTable('refresh');
            return false;
        },
        'onUploadStart': function (file) {
            var trainid = $("#trainid").val();
            var fjname = $("#txt_kj_name").val();
            var type = $("#sel_kj_type option:selected").val();
            $("#file_upload").uploadify("settings", "formData", { 'trainid': trainid, 'kjname': fjname,'type':type });
        }
    });

    $('#txt_time').datetimepicker({
        language: 'zh-CN',
        weekStart: 1,
        todayBtn: 1,
        autoclose: 1,
        todayHighlight: 1,
        startView: 2,
        forceParse: 0,
        showMeridian: 1
    });

    var trainTable = new TrainDetailInit();
    trainTable.Init();

    var fjTable = new TrainFjInit();
    fjTable.Init();

    //1.初始化Table
    var oTable = new KsInit();
    oTable.Init();

    var oTiMu = new UserInit();
    oTiMu.Init();

    //2.初始化Button的点击事件
    var oButtonInit = new ButtonInit();
    oButtonInit.Init();
});

var TrainDetailInit = function () {
    var oRableInit = new Object();
    oRableInit.Init = function () {
        $('#trainDetailTable').bootstrapTable({
            url: '/Teaching/GetTrainDetail',
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
                    field: 'Title',
                    title: '课题'
                }, {
                    field: 'Tec',
                    title: '主讲人'
                }, {
                    field: '',
                    title: '操作',
                    formatter: function (value, row) {
                        var d = '&nbsp;&nbsp;&nbsp;&nbsp;<button  type="button" class="btn" onclick="onDelTd(' + row.Id + ')">删除</button >';
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
            examid: $("#trainid").val()
        };
        return temp;
    };

    return oRableInit;
};

var TrainFjInit = function () {
    var oRableInit = new Object();
    oRableInit.Init = function () {
        $('#fjTable').bootstrapTable({
            url: '/Teaching/GetTrainFj',
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
                    field: 'Type',
                    title: '类型',
                    formatter: function (value, row) {
                       if (row.Type == 0) {
                           return "课件";
                       }
                        return "视频";
                    }
                }, {
                    field: 'Name',
                    title: '名称'
                }, {
                    field: '',
                    title: '操作',
                    formatter: function (value, row) {
                        var d = '&nbsp;&nbsp;&nbsp;&nbsp;<button  type="button" class="btn" onclick="onDelFj(' + row.Id + ','+row.Type+')">删除</button >';
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
            examid: $("#trainid").val()
        };
        return temp;
    };

    return oRableInit;
};

var KsInit = function () {
    var oRableInit = new Object();
    oRableInit.Init = function () {
        $('#ksTable').bootstrapTable({
            url: '/Teaching/GetKsUser',
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
                        var d = '&nbsp;&nbsp;&nbsp;&nbsp;<button  type="button" class="btn" onclick="onDel(' + row.Id + ')">删除</button >';
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
            examid: $("#trainid").val(),
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

var UserInit = function () {
    var oRableInit = new Object();
    oRableInit.Init = function () {
        $('#userTable').bootstrapTable({
            url: '/User/GetEndUsersForTrain',
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
                { checkbox: true },
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
            examid: $("#trainid").val(),
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

        $("#btn_save_train").click(function () {
            var postdata = {};
            postdata.zhuti = $("#txt_name").val();
            postdata.trainid = $("#trainid").val();
            postdata.org = $("#txt_org").val();
            postdata.time = $("#txt_time").val();
            postdata.teacher = $("#txt_teacher").val();
            postdata.address = $("#txt_address").val();
            postdata.score = $("#txt_score").val();
            postdata.type = $("#txt_traintype option:selected").val();
            $.ajax({
                type: "post",
                url: "/Teaching/SaveTrain",
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

        $("#btn_save_train_detail").click(function() {
            var postdata = {};
            postdata.title = $("#txt_kc_title").val();
            postdata.tec = $("#txt_kc_tec").val();
            postdata.trainid = $("#trainid").val();
            $.ajax({
                type: "post",
                url: "/Teaching/SaveTrainDetail",
                data: postdata,
                success: function (data) {
                    var d = eval(data);
                    if (d.status == "success") {
                        $('#modal').modal("hide");
                        $("#trainDetailTable").bootstrapTable('refresh');
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
            postdata.trainid = $("#trainid").val();
            $.ajax({
                type: "post",
                url: "/Teaching/AddTrainUser",
                data: postdata,
                success: function (data) {
                    var d = eval(data);
                    if (d.status == "success") {
                        $('#mymodal').modal("hide");
                        $.toast("添加成功", null);
                        $("#ksTable").bootstrapTable('refresh');
                    } else {
                        $.toast(d.status, null);
                    }
                },
                error: function () {
                    $.toast("Error", null);
                }
            });
        });

        $("#btn_save_train_kj").click(function() {
            $('#file_upload').uploadify('upload', '*');
        });

        $("#btn_kaos_add").click(function () {
            $("#userTable").bootstrapTable('refresh');
            $('#mymodal').modal({
                show: true,
                backdrop: 'static'
            });
        });

        $("#btn_add").click(function () {
            $('#modal').modal({
                show: true,
                backdrop: 'static'
            });
        });
        $("#btn_add_fj").click(function () {
            $('#kjmodal').modal({
                show: true,
                backdrop: 'static'
            });
        });
    };
    return oInit;
};
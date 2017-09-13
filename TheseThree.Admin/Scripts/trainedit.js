function onDel(i) {
    Ewin.confirm({ message: "确认要删除选择的数据吗？" }).on(function (e) {
        if (!e) {
            return;
        }
        var postdata = {};
        postdata.id = i;
        postdata.trainid = $("#trainid").val();
        $.ajax({
            type: "post",
            url: "/Teaching/DelTrainQdUser",
            data: postdata,
            success: function (data) {
                var d = eval(data);
                if (d.status == "success") {
                    $.toast("删除成功", null);
                    $("#qdTable").bootstrapTable('refresh');
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

function onDelTiMu(i) {
    Ewin.confirm({ message: "确认要删除选择的数据吗？" }).on(function (e) {
        if (!e) {
            return;
        }
        var postdata = {};
        postdata.id = i;
        postdata.trainid = $("#trainid").val();
        $.ajax({
            type: "post",
            url: "/Teaching/DelTrainTiMu",
            data: postdata,
            success: function (data) {
                var d = eval(data);
                if (d.status == "success") {
                    $.toast("删除成功", null);
                    $("#khTable").bootstrapTable('refresh');
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

function onDelFj(i) {
    Ewin.confirm({ message: "确认要删除选择的数据吗？" }).on(function (e) {
        if (!e) {
            return;
        }
        var postdata = {};
        postdata.id = i;
        postdata.examid = $("#trainid").val();
        $.ajax({
            type: "post",
            url: "/Teaching/DelTrainFj",
            data: postdata,
            success: function (data) {
                var d = eval(data);
                if (d.status == "success") {
                    $.toast("删除成功", null);
                    $("#fjTable").bootstrapTable('refresh');
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

function progressHandlingFunction(e) {
    if (e.lengthComputable) {
        $('progress').attr({ value: e.loaded, max: e.total }); 
        var percent = e.loaded / e.total * 100;
        $('#progress').html(e.loaded + "/" + e.total + " bytes. " + percent.toFixed(2) + "%");
    }
    return false;
}

$(function () {
    $('#txt_org').editableSelect({
        effects: 'slide',
        onSelect: function (element) {
            $('#txt_org').attr('data-val', element.val());
        }
    }).prop('placeholder', '请输入或选择主办单位.');

    $('#txt_level').editableSelect({
        effects: 'slide',
        onSelect: function (element) {
            $('#txt_txt_levelorg').attr('data-val', element.val());
        }
    }).prop('placeholder', '请输入或选择培训级别.');

    $('#txt_style').editableSelect({
        effects: 'slide',
        onSelect: function (element) {
            $('#txt_style').attr('data-val', element.val());
        }
    }).prop('placeholder', '请输入或选择培训形式.');

    $('#summernote').summernote({
        lang: 'zh-CN' 
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

    var fjTable = new TrainFjInit();
    fjTable.Init();

    var oTiMu = new UserInit();
    oTiMu.Init();

    var oKh = new KhInit();
    oKh.Init();

    var oUser = new AUserInit();
    oUser.Init();

    var osTiMu = new TimuInit();
    osTiMu.Init();

    var qd = new JkInit();
    qd.Init();

    var oButtonInit = new ButtonInit();
    oButtonInit.Init();
});

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
            columns: [ {
                    field: 'Name',
                    title: '名称',
                    formatter: function (value, row) {
                        var d = '&nbsp;&nbsp;&nbsp;&nbsp;<a class="btn" href="Uploads/' + row.Url+'" target="_blank">'+row.Name+'</button >';
                        return d;
                    }
                }, {
                    field: '',
                    title: '操作',
                    formatter: function (value, row) {
                        var d = '&nbsp;&nbsp;&nbsp;&nbsp;<button  type="button" class="btn" onclick="onDelFj(' + row.Id + ')">删除</button >';
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
var KhInit = function () {
    var oRableInit = new Object();
    oRableInit.Init = function () {
        $('#khTable').bootstrapTable({
            url: '/Teaching/GetKhTiMu',
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
                    field: 'Question',
                    title: '题干'
                }, {
                    field: '',
                    title: '选项',
                    formatter: function (value, row, index) {
                        var a = "";
                        if (row.ItemA.length > 0) {
                            a += row.ItemA + "<br />";
                        }
                        if (row.ItemB.length > 0) {
                            a += row.ItemB + "<br />";
                        }
                        if (row.ItemC.length > 0) {
                            a += row.ItemC + "<br />";
                        }
                        if (row.ItemD.length > 0) {
                            a += row.ItemD + "<br />";
                        }
                        if (row.ItemE.length > 0) {
                            a += row.ItemE + "<br />";
                        }
                        if (row.ItemF.length > 0) {
                            a += row.ItemF + "<br />";
                        }
                        if (row.ItemG.length > 0) {
                            a += row.ItemG + "<br />";
                        }
                        if (row.ItemH.length > 0) {
                            a += row.ItemH + "<br />";
                        }
                        if (row.ItemI.length > 0) {
                            a += row.ItemI + "<br />";
                        }
                        if (row.ItemJ.length > 0) {
                            a += row.ItemJ + "<br />";
                        }
                        return a;
                    }
                }, {
                    field: 'Answer',
                    title: '答案'
                }, {
                    field: 'Cent',
                    title: '分值'
                }, {
                    title: '操作',
                    formatter: function (value, row) {
                        var d = '&nbsp;&nbsp;&nbsp;&nbsp;<button  type="button" class="btn" onclick="onDelTiMu(' + row.Id+')">删除</button >';
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
            trainid: $("#trainid").val()
        };
        return temp;
    };
    return oRableInit;
};
var AUserInit = function () {
    var oRableInit = new Object();
    oRableInit.Init = function () {
        $('#auserTable').bootstrapTable({
            url: '/Teaching/GetEndUsersForTrain',
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
var TimuInit = function () {
    var oRableInit = new Object();
    oRableInit.Init = function () {
        $('#timuTable').bootstrapTable({
            url: '/Teaching/GetTiMuForTrain',
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
            showFooter: true,
            columns: [{
                    checkbox: true
                },
                {
                    field: 'Question',
                    title: '题干'
                }, {
                    field: '',
                    title: '选项',
                    formatter: function (value, row, index) {
                        var a = "";
                        if (row.ItemA.length > 0) {
                            a += row.ItemA + "<br />";
                        }
                        if (row.ItemB.length > 0) {
                            a += row.ItemB + "<br />";
                        }
                        if (row.ItemC.length > 0) {
                            a += row.ItemC + "<br />";
                        }
                        if (row.ItemD.length > 0) {
                            a += row.ItemD + "<br />";
                        }
                        if (row.ItemE.length > 0) {
                            a += row.ItemE + "<br />";
                        }
                        if (row.ItemF.length > 0) {
                            a += row.ItemF + "<br />";
                        }
                        if (row.ItemG.length > 0) {
                            a += row.ItemG + "<br />";
                        }
                        if (row.ItemH.length > 0) {
                            a += row.ItemH + "<br />";
                        }
                        if (row.ItemI.length > 0) {
                            a += row.ItemI + "<br />";
                        }
                        if (row.ItemJ.length > 0) {
                            a += row.ItemJ + "<br />";
                        }
                        return a;
                    }
                }, {
                    field: 'Answer',
                    title: '答案'
                }, {
                    field: 'cent',
                    title: '出题次数'
                }
                , {
                    field: 'cent',
                    title: '错误率'
                }
            ]
        });
    };

    oRableInit.queryParams = function (params) {
        var temp = {
            limit: params.limit,
            offset: params.offset,
            section: $("#sel_tiku option:selected").val(),
            name: $("#txt_timu_name").val(),
            labelname: $("#sel_label option:selected").text(),
            labelcode: $("#sel_label option:selected").val(),
            exerciseType: $("#sel_exercisetype option:selected").val()
        };
        return temp;
    };

    return oRableInit;
}
var JkInit = function () {
    var oRableInit = new Object();
    oRableInit.Init = function () {
        $('#qdTable').bootstrapTable({
            url: '/Teaching/GetUserForTrain',
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
            trainid: $("#trainid").val()
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
            $("#auserTable").bootstrapTable('refresh');
        });

        $("#btn_save_train").click(function () {
            var postdata = {};
            postdata.zhuti = $("#txt_name").val();
            postdata.trainid = $("#trainid").val();
            postdata.org = $("#txt_org option:selected").val();
            postdata.time = $("#txt_time").val();
            postdata.teacher = $("#txt_teacher").val();
            postdata.address = $("#txt_address").val();
            postdata.score = 0;
            postdata.style = $("#txt_style option:selected").val();
            postdata.level = $("#txt_level option:selected").val();
            postdata.apppush = $("#cb_app_push").is(':checked') ? 1 : 0;
            postdata.smspush = $("#cb_sms").is(':checked') ? 1 : 0;
            $.ajax({
                type: "post",
                url: "/Teaching/SaveTrain",
                data: postdata,
                success: function (data) {
                    var d = eval(data);
                    if (d.status == "success") {
                        $.toast("保存成功", null);
                        $('#tipModel').modal("hide");
                    } else {
                        $.toast(d.status, null);
                    }
                },
                error: function () {
                    $.toast("Error", null);
                }
            });
        });

        $("#btn_timu_save").click(function() {
            var arrselections = $("#timuTable").bootstrapTable('getSelections');
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
            postdata.cent = $("#txt_timu_cent").val();
            $.ajax({
                type: "post",
                url: "/Teaching/AddTrainKhTiMu",
                data: postdata,
                success: function (data) {
                    var d = eval(data);
                    if (d.status == "success") {
                        $('#mymodal2').modal("hide");
                        $.toast("添加成功", null);
                        $("#khTable").bootstrapTable('refresh');
                    } else {
                        $.toast(d.status, null);
                    }
                },
                error: function () {
                    $.toast("Error", null);
                }
            });
        });

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
            postdata.usertype = $("#hid_user_type").val();
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
                        if (postdata.usertype == 0)
                            $("#auserTable").bootstrapTable('refresh');
                        else {
                            $("#qdTable").bootstrapTable('refresh');
                        }
                    } else {
                        $.toast(d.status, null);
                    }
                },
                error: function () {
                    $.toast("Error", null);
                }
            });
        });

        $("#btn_kaos_del").click(function () {
            Ewin.confirm({ message: "确认要删除选择的数据吗？" }).on(function(e) {
                if (!e) {
                    return;
                }
                var arrselections = $("#auserTable").bootstrapTable('getSelections');
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
                    url: "/Teaching/DelTrainUser",
                    data: postdata,
                    success: function(data) {
                        var d = eval(data);
                        if (d.status == "success") {
                            $('#mymodal').modal("hide");
                            $.toast("添加成功", null);
                            $("#auserTable").bootstrapTable('refresh');
                        } else {
                            $.toast(d.status, null);
                        }
                    },
                    error: function() {
                        $.toast("Error", null);
                    }
                });
            });
        });

        $("#btn_save_train_kj").click(function () {
            var formData = new FormData();
            formData.append('files', $('#file_upload')[0].files[0]);
            formData.append('trainid', $("#trainid").val());
            formData.append('kjname', $("#txt_kj_name").val());
            $.ajax({
                url: '/Teaching/UploadFuJian',
                type: 'POST',
                cache: false,
                data: formData,
                processData: false,
                contentType: false,
                success: function (data) {
                    $('#kjmodal').modal("hide");
                    $("#fjTable").bootstrapTable('refresh');
                }
            });
        });

        $("#btn_save_train_nr").click(function () {
            postdata.trainid = $("#trainid").val();
            postdata.nrname = $("#txt_nr_name").val();
            postdata.content = $('#summernote').summernote('code');
            $.ajax({
                type: "post",
                url: "/Teaching/UploadNr",
                data: postdata,
                success: function (data) {
                    var d = eval(data);
                    if (d.status == "success") {
                        $('#kjmodal').modal("hide");
                        $("#fjTable").bootstrapTable('refresh');
                        return false;
                    } else {
                        $.toast(d.status, null);
                        return false;
                    }
                },
                error: function () {
                    $.toast("Error", null);
                    return false;
                }
            });
            return false;
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

        $("#btn_add_fj").click(function () {
            $("#txt_kj_name").val("");
            $("#txt_nr_name").val("");
            $("#file_upload").val("");
            $('#summernote').summernote('code','');
            $('#kjmodal').modal({
                show: true,
                backdrop: 'static'
            });
        });

        $("#btn_add_zsd").click(function () {
            $('#zsdmodal').modal({
                show: true,
                backdrop: 'static'
            });
        });

        $("#btn_add_kh_tiku").click(function () {
            $('#mymodal2').modal({
                show: true,
                backdrop: 'static'
            });
        });

        $("#btn_timu_query").click(function () {
            $("#timuTable").bootstrapTable('refresh');
        });

        $("#btn_add_kh").click(function () {
            $('#mymodal3').modal({
                show: true,
                backdrop: 'static'
            });
        });

        $("#btn_stimu_save").click(function () {
            postdata.anli = $("#txt_new_anli").val();
            postdata.remark = $("#txt_new_desc").val();
            postdata.question = $("#txt_new_question").val();
            if (postdata.question.length <= 0) {
                $.toast("请输入题干", null);
                return;
            }
            postdata.trainid = $("#trainid").val();
            postdata.label = $("#txt_new_label").val();
            postdata.difficulty = $("#txt_new_difficulty").val();
            postdata.type = $("#txt_new_type option:selected").val();
            postdata.itema = $('#txt_new_A').val();
            if (postdata.itema.length > 0 && $('#cbA').is(":checked")) {
                postdata.itema_c = 1;
            } else {
                postdata.itema_c = 0;
            }

            postdata.itemb = $('#txt_new_B').val();
            if (postdata.itemb.length > 0 && $('#cbB').is(":checked")) {
                postdata.itemb_c = 1;
            } else {
                postdata.itemb_c = 0;
            }
            postdata.itemc = $('#txt_new_C').val();
            if (postdata.itemc.length > 0 && $('#cbC').is(":checked")) {
                postdata.itemc_c = 1;
            } else {
                postdata.itemc_c = 0;
            }
            postdata.itemd = $('#txt_new_D').val();
            if (postdata.itemd.length > 0 && $('#cbD').is(":checked")) {
                postdata.itemd_c = 1;
            } else {
                postdata.itemd_c = 0;
            }
            postdata.iteme = $('#txt_new_E').val();
            if (postdata.iteme.length > 0 && $('#cbE').is(":checked")) {
                postdata.iteme_c = 1;
            } else {
                postdata.iteme_c = 0;
            }
            postdata.itemf = $('#txt_new_F').val();
            if (postdata.itemf.length > 0 && $('#cbF').is(":checked")) {
                postdata.itemf_c = 1;
            } else {
                postdata.itemf_c = 0;
            }
            postdata.itemg = $('#txt_new_G').val();
            if (postdata.itemg.length > 0 && $('#cbG').is(":checked")) {
                postdata.itemg_c = 1;
            } else {
                postdata.itemg_c = 0;
            }
            postdata.itemh = $('#txt_new_H').val();
            if (postdata.itemh.length > 0 && $('#cbH').is(":checked")) {
                postdata.itemh_c = 1;
            } else {
                postdata.itemh_c = 0;
            }
            postdata.itemi = $('#txt_new_I').val();
            if (postdata.itemi.length > 0 && $('#cbI').is(":checked")) {
                postdata.itemi_c = 1;
            } else {
                postdata.itemi_c = 0;
            }
            postdata.itemj = $('#txt_new_J').val();
            if (postdata.itemj.length > 0 && $('#cbJ').is(":checked")) {
                postdata.itemj_c = 1;
            } else {
                postdata.itemj_c = 0;
            }
            if (postdata.itema.length <= 0 &&
                postdata.itemb.length <= 0 &&
                postdata.itemc.length <= 0 &&
                postdata.itemd.length <= 0 &&
                postdata.iteme.length <= 0 &&
                postdata.itemf.length <= 0 &&
                postdata.itemg.length <= 0 &&
                postdata.itemh.length <= 0 &&
                postdata.itemi.length <= 0 &&
                postdata.itemj.length <= 0
            ) {
                $.toast("请至少输入一个选项内容", null);
                return;
            }
            if (postdata.itema_c <= 0 &&
                postdata.itemb_c <= 0 &&
                postdata.itemc_c <= 0 &&
                postdata.itemd_c <= 0 &&
                postdata.iteme_c <= 0 &&
                postdata.itemf_c <= 0 &&
                postdata.itemg_c <= 0 &&
                postdata.itemh_c <= 0 &&
                postdata.itemi_c <= 0 &&
                postdata.itemj_c <= 0
            ) {
                $.toast("请至少选中一个正确答案", null);
            }
            $.ajax({
                type: "post",
                url: "/Teaching/AddOrUpdateTrainTiMu",
                data: postdata,
                success: function (data) {
                    var d = eval(data);
                    if (d.status == "success") {
                        $('#mymodal3').modal("hide");
                        $.toast("提交数据成功", null);
                        $("#khTable").bootstrapTable('refresh');
                    } else {
                        $.toast(d.status, null);
                    }
                },
                error: function () {
                    $.toast("Error", null);
                }
            });
        });

        $("#btn_tip").click(function () {
            if ($("#txt_name").val().length <= 0) {
                $.toast("请填写培训名称", null);
                return;
            }
            if ($("#txt_address").val().length <= 0) {
                $.toast("请填写培训地点", null);
                return;
            }
            $('#tipModel').modal({
                show: true,
                backdrop: 'static'
            });
        });
    };
    return oInit;
};
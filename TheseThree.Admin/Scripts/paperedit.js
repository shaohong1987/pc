function onDel(i,j) {
    Ewin.confirm({ message: "确认要删除选择的数据吗？" }).on(function (e) {
        if (!e) {
            return;
        }
        var postdata = {};
        postdata.id = i;
        postdata.exerciseType = j;
        postdata.paperid = $("#paperid").val();
        $.ajax({
            type: "post",
            url: "/Teaching/DelPaperQuestion",
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
    getSta();
    if (i == 3) {
        $("#recognizedTable").bootstrapTable('refresh');
    } else if (i == 1) {
        $("#radioTable").bootstrapTable('refresh');
    } else if (i == 2) {
        $("#multipleChoiceTable").bootstrapTable('refresh');
    }else if (i == 0) {
        $("#anliChoiceTable").bootstrapTable('refresh');
    }
}

function getSta() {
    var postdata = {};
    postdata.paperId = $("#paperid").val();
    $.ajax({
        type: "post",
        url: "/Teaching/GetPQuestionStas",
        data: postdata,
        success: function (data) {
            var d = eval(data);
            if (d.status == "success") {
                for (var i = 0; i < d.data.length; i++) {
                    $("#" + d.data[i].Name).html(d.data[i].Value);
                }
            } else {
                $.toast(d.status, null);
            }
        },
        error: function () {
            $.toast('Error');
        }
    });
}

$(function () {
    //1.初始化Table
    var oTable = new RadioInit();
    oTable.Init();

    var oRecognized = new RecognizedInit();
    oRecognized.Init(); 

    var oMultipleChoice = new MultipleChoiceInit();
    oMultipleChoice.Init();

    var oAnliChoice = new AnliChoiceInit();
    oAnliChoice.Init();

    var oTiMu = new TimuInit();
    oTiMu.Init();

    //2.初始化Button的点击事件
    var oButtonInit = new ButtonInit();
    oButtonInit.Init();

    getSta();
});

var RecognizedInit = function () {
    var oRableInit = new Object();
    oRableInit.Init = function () {
        $('#recognizedTable').bootstrapTable({
            url: '/Teaching/GetPaperTimu',
            method: 'get',
            striped: true,
            cache: false,
            pagination: false,
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
                            a += "A."+row.ItemA + "<br />";
                        }
                        if (row.ItemB.length > 0) {
                            a += "B." +row.ItemB + "<br />";
                        }
                        if (row.ItemC.length > 0) {
                            a += "C." +row.ItemC + "<br />";
                        }
                        if (row.ItemD.length > 0) {
                            a += "D." +row.ItemD + "<br />";
                        }
                        if (row.ItemE.length > 0) {
                            a += "E." +row.ItemE + "<br />";
                        }
                        if (row.ItemF.length > 0) {
                            a += "F." +row.ItemF + "<br />";
                        }
                        if (row.ItemG.length > 0) {
                            a += "G." +row.ItemG + "<br />";
                        }
                        if (row.ItemH.length > 0) {
                            a += "H." +row.ItemH + "<br />";
                        }
                        if (row.ItemI.length > 0) {
                            a += "I." +row.ItemI + "<br />";
                        }
                        if (row.ItemJ.length > 0) {
                            a += "J." +row.ItemJ + "<br />";
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
                        var d = '&nbsp;&nbsp;&nbsp;&nbsp;<button  type="button" class="btn" onclick="onDel(' + row.Id + ',' + row.ExerciseType+')">删除</button >';
                        return  d;
                    }
                }
            ]
        });
    };

    oRableInit.queryParams = function (params) {
        var temp = {
            exerciseType: 3,
            paperid: $("#paperid").val()
        };
        return temp;
    };
    return oRableInit;
};

var RadioInit = function () {
    var oRableInit = new Object();
    oRableInit.Init = function () {
        $('#radioTable').bootstrapTable({
            url: '/Teaching/GetPaperTimu',
            method: 'get',
            striped: true,
            cache: false,
            pagination: false,
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
                    field: 'Question',
                    title: '题干'
                }, {
                    field: '',
                    title: '选项',
                    formatter: function (value, row, index) {
                        var a = "";
                        if (row.ItemA.length > 0) {
                            a += "A." + row.ItemA + "<br />";
                        }
                        if (row.ItemB.length > 0) {
                            a += "B." + row.ItemB + "<br />";
                        }
                        if (row.ItemC.length > 0) {
                            a += "C." + row.ItemC + "<br />";
                        }
                        if (row.ItemD.length > 0) {
                            a += "D." + row.ItemD + "<br />";
                        }
                        if (row.ItemE.length > 0) {
                            a += "E." + row.ItemE + "<br />";
                        }
                        if (row.ItemF.length > 0) {
                            a += "F." + row.ItemF + "<br />";
                        }
                        if (row.ItemG.length > 0) {
                            a += "G." + row.ItemG + "<br />";
                        }
                        if (row.ItemH.length > 0) {
                            a += "H." + row.ItemH + "<br />";
                        }
                        if (row.ItemI.length > 0) {
                            a += "I." + row.ItemI + "<br />";
                        }
                        if (row.ItemJ.length > 0) {
                            a += "J." + row.ItemJ + "<br />";
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
                        var d = '&nbsp;&nbsp;&nbsp;&nbsp;<button  type="button" class="btn" onclick="onDel(' + row.Id + ',' + row.ExerciseType+')">删除</button >';
                        return d;
                    }
                }
            ]
        });
    };

    oRableInit.queryParams = function (params) {
        var temp = {
            exerciseType: 1,
            paperid: $("#paperid").val()
        };
        return temp;
    };

    return oRableInit;
};

var MultipleChoiceInit = function () {
    var oRableInit = new Object();
    oRableInit.Init = function () {
        $('#multipleChoiceTable').bootstrapTable({
            url: '/Teaching/GetPaperTimu',
            method: 'get',
            striped: true,
            cache: false,
            pagination: false,
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
                    field: 'Question',
                    title: '题干'
                }, {
                    field: '',
                    title: '选项',
                    formatter: function (value, row, index) {
                        var a = "";
                        if (row.ItemA.length > 0) {
                            a += "A." + row.ItemA + "<br />";
                        }
                        if (row.ItemB.length > 0) {
                            a += "B." + row.ItemB + "<br />";
                        }
                        if (row.ItemC.length > 0) {
                            a += "C." + row.ItemC + "<br />";
                        }
                        if (row.ItemD.length > 0) {
                            a += "D." + row.ItemD + "<br />";
                        }
                        if (row.ItemE.length > 0) {
                            a += "E." + row.ItemE + "<br />";
                        }
                        if (row.ItemF.length > 0) {
                            a += "F." + row.ItemF + "<br />";
                        }
                        if (row.ItemG.length > 0) {
                            a += "G." + row.ItemG + "<br />";
                        }
                        if (row.ItemH.length > 0) {
                            a += "H." + row.ItemH + "<br />";
                        }
                        if (row.ItemI.length > 0) {
                            a += "I." + row.ItemI + "<br />";
                        }
                        if (row.ItemJ.length > 0) {
                            a += "J." + row.ItemJ + "<br />";
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
                        var d = '&nbsp;&nbsp;&nbsp;&nbsp;<button  type="button" class="btn" onclick="onDel(' + row.Id + ',' + row.ExerciseType+')">删除</button >';
                        return d;
                    }
                }
            ]
        });
    };

    oRableInit.queryParams = function (params) {
        var temp = {
            exerciseType: 2,
            paperid: $("#paperid").val()
    };
        return temp;
    };
    return oRableInit;
};

var AnliChoiceInit = function() {
    var oRableInit = new Object();
    oRableInit.Init = function() {
        $('#anliChoiceTable').bootstrapTable({
            url: '/Teaching/GetPaperTimu',
            method: 'get',
            striped: true,
            cache: false,
            pagination: false,
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
                    field: 'Anli',
                    title: '案例'
                }, 
                {
                    field: 'Question',
                    title: '题干'
                }, {
                    field: '',
                    title: '选项',
                    formatter: function(value, row, index) {
                        var a = "";
                        if (row.ItemA.length > 0) {
                            a += "A." + row.ItemA + "<br />";
                        }
                        if (row.ItemB.length > 0) {
                            a += "B." + row.ItemB + "<br />";
                        }
                        if (row.ItemC.length > 0) {
                            a += "C." + row.ItemC + "<br />";
                        }
                        if (row.ItemD.length > 0) {
                            a += "D." + row.ItemD + "<br />";
                        }
                        if (row.ItemE.length > 0) {
                            a += "E." + row.ItemE + "<br />";
                        }
                        if (row.ItemF.length > 0) {
                            a += "F." + row.ItemF + "<br />";
                        }
                        if (row.ItemG.length > 0) {
                            a += "G." + row.ItemG + "<br />";
                        }
                        if (row.ItemH.length > 0) {
                            a += "H." + row.ItemH + "<br />";
                        }
                        if (row.ItemI.length > 0) {
                            a += "I." + row.ItemI + "<br />";
                        }
                        if (row.ItemJ.length > 0) {
                            a += "J." + row.ItemJ + "<br />";
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
                    formatter: function(value, row) {
                        var d = '&nbsp;&nbsp;&nbsp;&nbsp;<button  type="button" class="btn" onclick="onDel(' +
                            row.Id +
                            ',' +
                            row.ExerciseType +
                            ')">删除</button >';
                        return d;
                    }
                }
            ]
        });
    };

    oRableInit.queryParams = function(params) {
        var temp = {
            exerciseType: 0,
            paperid: $("#paperid").val()
        };
        return temp;
    };
    return oRableInit;
};

var TimuInit=function() { 
    var oRableInit = new Object();
    oRableInit.Init = function () {
        $('#timuTable').bootstrapTable({
            url: '/Teaching/GetTimus',
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
            }, {
                    field: 'Anli',
                    title: '案例',
                    width: '200px'
                }, 
                {
                    field: 'Question',
                    title: '题干',
                    width: '200px'
                }, {
                    field: '',
                    title: '选项',
                    width: '300px',
                    formatter: function (value, row, index) {
                        var a = "";
                        if (row.ItemA.length > 0) {
                            a += "A." + row.ItemA + "<br />";
                        }
                        if (row.ItemB.length > 0) {
                            a += "B." + row.ItemB + "<br />";
                        }
                        if (row.ItemC.length > 0) {
                            a += "C." + row.ItemC + "<br />";
                        }
                        if (row.ItemD.length > 0) {
                            a += "D." + row.ItemD + "<br />";
                        }
                        if (row.ItemE.length > 0) {
                            a += "E." + row.ItemE + "<br />";
                        }
                        if (row.ItemF.length > 0) {
                            a += "F." + row.ItemF + "<br />";
                        }
                        if (row.ItemG.length > 0) {
                            a += "G." + row.ItemG + "<br />";
                        }
                        if (row.ItemH.length > 0) {
                            a += "H." + row.ItemH + "<br />";
                        }
                        if (row.ItemI.length > 0) {
                            a += "I." + row.ItemI + "<br />";
                        }
                        if (row.ItemJ.length > 0) {
                            a += "J." + row.ItemJ + "<br />";
                        }
                        return a;
                    }
                }, {
                    field: 'Answer',
                    title: '答案',
                    width: '80px'
                }, {
                    field: 'cent',
                    title: '出题次数',
                    width: '80px'
                }
                , {
                    field: 'cent',
                    title: '错误率',
                    width: '80px'
                }
            ]
        });
    };

    oRableInit.queryParams = function (params) {
        var temp = {
            limit: params.limit,
            offset: params.offset,
            paperid:$("#paperid").val(),
            section: $("#sel_tiku option:selected").val(),
            name: $("#txt_timu_name").val(),
            labelname: $("#sel_label option:selected").text(),
            labelcode: $("#sel_label option:selected").val(),
            exerciseType: $("#hid_exercise_type").val()
        };
        return temp;
    };

    return oRableInit;
}

var ButtonInit = function () {
    var oInit = new Object();
    var postdata = {};
    oInit.Init = function() {
        $("#btn_recognized_add").click(function() {
            $("#hid_exercise_type").val(3);
            $("#myModalLabel").text("请选择题目（判断题）");
            $("#timuTable").bootstrapTable('refresh');
            $('#mymodal').modal({
                show: true,
                backdrop: 'static'
            });
        });

        $("#btn_radio_add").click(function() {
            $("#hid_exercise_type").val(1);
            $("#myModalLabel").text("请选择题目（单选题）");
            $("#timuTable").bootstrapTable('refresh');
            $('#mymodal').modal({
                show: true,
                backdrop: 'static'
            });
        });

        $("#btn_multipleChoice_add").click(function() {
            $("#hid_exercise_type").val(2);
            $("#myModalLabel").text("请选择题目（多选题）");
            $("#timuTable").bootstrapTable('refresh');
            $('#mymodal').modal({
                show: true,
                backdrop: 'static'
            });
        });

        $("#btn_anliChoice_add").click(function () {
            $("#hid_exercise_type").val(0);
            $("#myModalLabel").text("请选择题目（案例题）");
            $("#timuTable").bootstrapTable('refresh');
            $('#mymodal').modal({
                show: true,
                backdrop: 'static'
            });
        });

        function initControl() {
            $("#txt_new_anli").val("");
            $("#txt_new_question").val("");
            $("#txt_new_label").val("");
            $("#txt_new_difficulty").val("");
            $("#txt_new_A").val("");
            $("#txt_new_B").val("");
            $("#txt_new_C").val("");
            $("#txt_new_D").val("");
            $("#txt_new_E").val("");
            $("#txt_new_F").val("");
            $("#txt_new_G").val("");
            $("#txt_new_H").val("");
            $("#txt_new_I").val("");
            $("#txt_new_J").val(""); 
            $("#txt_new_desc").val("");
            $("input[name='checkbox']").removeAttr("checked"); 
        }

        $("#btn_recognized_add_manual").click(function () {
            $("#hid_table_id").val("recognizedTable");
            $("#h4_title").text("新增判断题");
            $("#txt_new_type").val(3);
            initControl();
            $('#mymodal3').modal({
                show: true,
                backdrop: 'static'
            });
        });

        $("#btn_radio_add_manual").click(function () {
            $("#hid_table_id").val("radioTable");
            $("#h4_title").text("新增单选题");
            $("#txt_new_type").val(1);
            initControl();
            $('#mymodal3').modal({
                show: true,
                backdrop: 'static'
            });
        });

        $("#btn_multipleChoice_add_manual").click(function () {
            $("#hid_table_id").val("multipleChoiceTable");
            $("#h4_title").text("新增多选题");
            $("#txt_new_type").val(2);
            initControl();
            $('#mymodal3').modal({
                show: true,
                backdrop: 'static'
            });
        });

        $("#btn_anliChoice_add_manual").click(function () {
            $("#hid_table_id").val("anliChoiceTable");
            $("#h4_title").text("新增案例题");
            initControl();
            $('#mymodal3').modal({
                show: true,
                backdrop: 'static'
            });
        });

        $("#btn_query").click(function() {
            $("#timuTable").bootstrapTable('refresh');
        });

        $("#btn_save_paper").click(function () {
            var postdata = {};
            postdata.papername = $("#txt_name").val();
            postdata.paperid = $("#paperid").val();
            postdata.totalMini = $("#sel_total_min option:selected").val();
            $.ajax({
                type: "post",
                url: "/Teaching/SavePaper",
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

        $("#btn_save").click(function() {
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
            postdata.exerciseType = $("#hid_exercise_type").val();
            postdata.paperid = $("#paperid").val();
            var c = $("#txt_timu_cent").val();
            if (c.length > 0)
                postdata.cent = c;
            else
                postdata.cent = 0;
            $.ajax({
                type: "post",
                url: "/Teaching/AddPaperQuestion",
                data: postdata,
                success: function(data) {
                    var d = eval(data);
                    if (d.status == "success") {
                        $('#mymodal').modal("hide");
                        $.toast("添加题目成功", null);
                        refreshTable(postdata.exerciseType);
                    } else {
                        $.toast(d.status, null);
                    }
                },
                error: function() {
                    $.toast("Error", null);
                }
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
            postdata.paperid = $("#paperid").val();
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
                url: "/Teaching/AddOrUpdateExamTiMu",
                data: postdata,
                success: function (data) {
                    var d = eval(data);
                    if (d.status == "success") {
                        $('#mymodal3').modal("hide");
                        $.toast("提交数据成功", null);
                        var table = $("#hid_table_id").val();
                        $("#" + table).bootstrapTable('refresh');
                    } else {
                        $.toast(d.status, null);
                    }
                },
                error: function () {
                    $.toast("Error", null);
                }
            });
        }); 
    };
    return oInit;
};
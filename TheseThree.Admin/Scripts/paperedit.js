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
                        var d = '&nbsp;&nbsp;&nbsp;&nbsp;<button  type="button" class="btn" onclick="onDel(' + row.Id + ',' + row.ExerciseType+')">删除</button >';
                        return e + d;
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
            postdata.cent = $("#txt_timu_cent").val();
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
    };
    return oInit;
};
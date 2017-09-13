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
    //初始化Table
    oTableInit.Init = function () {
        $('#tikuTable').bootstrapTable({
            url: '/Teaching/GetTiMu', //请求后台的URL（*）
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
                    field: 'exerciseType',
                    title: '题型',
                    formatter: function (value, row, index) {
                        if (row.ExerciseType == 1) {
                            return "单选题";
                        }
                        if (row.ExerciseType == 2) {
                            return "多选题";
                        }
                        if (row.ExerciseType == 3) {
                            return "判断题";
                        }
                        return "";
                    }
                }, {
                    field: 'Anli',
                    title: '案例',
                    width: '400px'
                }, {
                    field: 'Question',
                    title: '题干',
                    width: '300px'
                }, {
                    field: '',
                    title: '选项',
                    formatter: function (value, row, index) {
                        var a="";
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
                    width: '100px'
                }, {
                    field: 'Label',
                    title: '标签',
                    width: '150px'
                }, {
                    field: 'Remark',
                    title: '描述',
                    width: '250px'
                }
            ]
        });
    };

    oTableInit.queryParams = function (params) {
        var temp = {
            limit: params.limit,
            offset: params.offset,
            section: $("#hid_section_id").val(),
            name: $("#txt_name").val(),
            type: $("#sel_type option:selected").val(),
            labelname: $("#sel_label option:selected").text(),
            labelcode: $("#sel_label option:selected").val()
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

        $("#btn_edit").click(function () {
            var arrselections = $("#tikuTable").bootstrapTable('getSelections');
            if (arrselections.length > 1) {
                $.toast('只能选择一行进行编辑');
                return;
            }
            if (arrselections.length <= 0) {
                $.toast('请选择有效数据');
                return;
            }
            $("#myModalLabel").text("编辑");
            $("#txt_new_anli").val(arrselections[0].Anli);
            $("#txt_new_desc").val(arrselections[0].Remark);
            $("#txt_new_question").val(arrselections[0].Question);
            $("#txt_new_label").val(arrselections[0].Label);
            $("#txt_new_type").val(arrselections[0].ExerciseType);
            $("#txt_new_A").val(arrselections[0].ItemA);
            $("#txt_new_B").val(arrselections[0].ItemB);
            $("#txt_new_C").val(arrselections[0].ItemC);
            $("#txt_new_D").val(arrselections[0].ItemD);
            $("#txt_new_E").val(arrselections[0].ItemE);
            $("#txt_new_F").val(arrselections[0].ItemF);
            $("#txt_new_G").val(arrselections[0].ItemG);
            $("#txt_new_H").val(arrselections[0].ItemH);
            $("#txt_new_I").val(arrselections[0].ItemI);
            $("#txt_new_J").val(arrselections[0].ItemJ);
            $("#txt_new_difficulty").val(arrselections[0].Difficulty);
            var answer = arrselections[0].Answer;
            for (var i = 0; i < answer.length; i++) {
                var a = answer.substr(i, i + 1).toUpperCase();
                switch (a) {
                case 'A':
                    $("#cbA").attr("checked", "true");
                    break;
                case 'B':
                    $("#cbB").attr("checked", "true");
                    break;
                case 'C':
                    $("#cbC").attr("checked", "true");
                    break;
                case 'D':
                    $("#cbD").attr("checked", "true");
                    break;
                case 'E':
                    $("#cbE").attr("checked", "true");
                    break;
                case 'F':
                    $("#cbF").attr("checked", "true");
                    break;
                case 'G':
                    $("#cbG").attr("checked", "true");
                    break;
                case 'H':
                    $("#cbH").attr("checked", "true");
                    break;
                case 'I':
                    $("#cbI").attr("checked", "true");
                    break;
                case 'J':
                    $("#cbJ").attr("checked", "true");
                    break;
                }
            }
            postdata.id = arrselections[0].Id;
            $('#mymodal').modal({
                show: true,
                backdrop: 'static'
            });
        });

        $("#btn_delete").click(function () {
            var arrselections = $("#tikuTable").bootstrapTable('getSelections');
            if (arrselections.length <= 0) {
                $.toast('请选择有效数据');
                return;
            }
            var ids = "";
            for (var i = 0; i < arrselections.length; i++) {
                ids += "," + arrselections[i].Id;
            }
            postdata.id = ids;
            postdata.sectionId = $("#hid_section_id").val();
            Ewin.confirm({ message: "确认要删除选择的数据吗？" }).on(function (e) {
                if (!e) {
                    return;
                }
                $.ajax({
                    type: "post",
                    url: "/Teaching/DelTiMu",
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
        });

        $("#btn_save").click(function () {
            postdata.anli = $("#txt_new_anli").val();
            postdata.remark = $("#txt_new_desc").val();
            postdata.question = $("#txt_new_question").val();
            if (postdata.question.length <= 0) {
                $.toast("请输入题干", null);
                return;
            }
            postdata.sectionId = $("#hid_section_id").val();
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
                url: "/Teaching/AddOrUpdateTiMu",
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
    };
    return oInit;
};
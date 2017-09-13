function exportExamForPerson(i, j,k,l,m) {
    var postdata = {};
    postdata.uid =j;
    postdata.tid = i;
    postdata.ks = k;
    postdata.name = l;
    postdata.score = m;
     $.ajax({
         type: "post",
         url: "/Teaching/ExportExamForPerson",
         data: postdata,
         success: function (data) {
             var d = eval(data);
             if (d.status == "success") {
                 var url = "Uploads/" + d.title;
                 window.open(url);
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
    var oTable = new ExamStaticInit();
    oTable.Init();

    var oButtonInit = new ButtonInit();
    oButtonInit.Init();
});

var ExamStaticInit = function () {
    var oRableInit = new Object();
    oRableInit.Init = function () {
        $('#staticExamTable').bootstrapTable({
            url: '/Statistic/GetStatisticExam',
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
                        var d = '&nbsp;&nbsp;&nbsp;&nbsp;<button  type="button" class="btn" onclick="exportExamForPerson(' + row.TestId + ',' + row.UserId + ',\'' + row.Dept + '\',\'' + row.Name + '\',' + row.Score +');">导出试卷</button >';
                        return d;
                    }
                },
                {
                    field: 'Level',
                    title: '类型'
                },
                {
                    field: 'Month',
                    title: '月份'
                },
                {
                    field: 'Content',
                    title: '内容'
                },
                {
                    field: 'Dept',
                    title: '科室'
                }, {
                    field: 'LoginId',
                    title: '工号'
                }, {
                    field: 'Name',
                    title: '姓名'
                },
                {
                    field: 'Score',
                    title: '成绩'
                },
                {
                    field: 'Remark',
                    title: '备注'
                },
                {
                    field: 'Attend',
                    title: '出勤'
                },
                {
                    field: 'AttendRemark',
                    title: '缺席原因'
                }
            ]
        });
    };

    oRableInit.queryParams = function (params) {
        var temp = {
            limit: params.limit,
            offset: params.offset,
            grade: $("#sel_level option:selected").val(),
            month: $("#sel_month option:selected").val(),
            id: $("#hid_exam_id").val(),
            deptcode: $("#sel_dept option:selected").val(),
            loginId: $("#txt_loginid").val(),
            name: $("#txt_name").val()
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
            $("#staticExamTable").bootstrapTable('refresh');
        });

        $("#btn_export_xls").click(function () {
            postdata.grade = $("#sel_level option:selected").val();
            postdata.month = $("#sel_month option:selected").val();
            postdata.id = $("#hid_exam_id").val();
            postdata.deptcode = $("#sel_dept option:selected").val();
            postdata.loginId = $("#txt_loginid").val();
            postdata.name = $("#txt_name").val();
            $.ajax({
                type: "post",
                url: "/Teaching/exportExamXls",
                data: postdata,
                success: function (data) {
                    var d = eval(data);
                    if (d.status == "success") {
                        var url = "Uploads/" + d.title;
                        window.open(url);
                    } else {
                        $.toast(d.status, null);
                    }
                },
                error: function () {
                    $.toast('Error');
                }
            });
        });

        $("#btn_export_word").click(function () {
            postdata.grade = $("#sel_level option:selected").val();
            postdata.month = $("#sel_month option:selected").val();
            postdata.id = $("#hid_exam_id").val();
            postdata.deptcode = $("#sel_dept option:selected").val();
            postdata.loginId = $("#txt_loginid").val();
            postdata.name = $("#txt_name").val();
            $.ajax({
                type: "post",
                url: "/Teaching/exportExamWord",
                data: postdata,
                success: function (data) {
                    var d = eval(data);
                    if (d.status == "success") {
                        var url = "Uploads/" + d.title;
                        window.open(url);
                    } else {
                        $.toast(d.status, null);
                    }
                },
                error: function () {
                    $.toast('Error');
                }
            });
        });
    };
    return oInit;
};
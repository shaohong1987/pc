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
    oTableInit.Init = function () {
        $('#examTable').bootstrapTable({
            url: '/Teaching/GetExam',
            method: 'get',
            toolbar: '#toolbar',
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
                    field: 'Title',
                    title: '考试名称',
                    formatter: function (value, row, index) {
                        var e = '<a data-ajax="true" data-ajax-mode="replace" data-ajax-update="#main" href="/Statistic/StatisticExam/' + row.Id + '">' + row.Title + '</a> ';
                        return e;
                    }
                }, {
                    field: 'Begintime',
                    title: '开始时间'
                }, {
                    field: 'Endtime',
                    title: '结束时间'
                }, {
                    title: '操作',
                    formatter: function (value, row, index) {
                        var e = '<a data-ajax="true" data-ajax-mode="replace" data-ajax-update="#main" href="/Statistic/StatisticExam/' + row.Id + '">查看</a> ';;
                        return e;
                    }
                }
            ]
        });
    };

    oTableInit.queryParams = function (params) {
        var temp = {
            limit: params.limit,
            offset: params.offset,
            name: $("#txt_name").val(),
            startTime: $("#txt_strat_time").val(),
            endTime: $("#txt_end_time").val()
        };
        return temp;
    };
    return oTableInit;
};

var ButtonInit = function () {
    var oInit = new Object();
    var postdata = {};
    oInit.Init = function () {
        $("#btn_query").click(function () {
            $("#examTable").bootstrapTable('refresh');
        });
    };
    return oInit;
};
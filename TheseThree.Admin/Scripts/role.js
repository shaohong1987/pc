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
        $('#jueseTable').bootstrapTable({
            url: '/Home/GetRoles',
            method: 'get',
            toolbar: '#toolbar',
            // toolbarAlign: 'right',
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
                    field: 'RoleName',
                    title: '角色名称',
                }, {
                    field: 'RoleDesc',
                    title: '描述'
                },{
                    title: '操作',
                    formatter: function (value, row, index) {
                        var e = '<a data-ajax="true" data-ajax-mode="replace" data-ajax-update="#main" href="/Home/RoleEdit/' + row.Id + '">配置人员</a> ';;
                        return e;
                    }
                }
            ]
        });
    };
    return oTableInit;
};
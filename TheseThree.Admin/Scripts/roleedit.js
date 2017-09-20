function onDel(i) {
    Ewin.confirm({ message: "确认要删除选择的数据吗？" }).on(function (e) {
        if (!e) {
            return;
        }
        var postdata = {};
        postdata.id = i;
        $.ajax({
            type: "post",
            url: "/Home/DelRole",
            data: postdata,
            success: function (data) {
                var d = eval(data);
                if (d.status == "success") {
                    $.toast("删除成功", null);
                    refreshTable();
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
    $("#jsryTable").bootstrapTable('refresh');
}
$(function () {
    //1.初始化Table
    var oTable = new TableInit();
    oTable.Init();

    var oTiMu = new UserInit();
    oTiMu.Init();

    //2.初始化Button的点击事件
    var oButtonInit = new ButtonInit();
    oButtonInit.Init();
});
var TableInit = function () {
    var oTableInit = new Object();
    oTableInit.Init = function () {
        $('#jsryTable').bootstrapTable({
            url: '/Home/GetUserForRole',
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
                    field: 'LoginId',
                    title: '手机号'
                }, {
                    field: 'UserName',
                    title: '姓名'
                }, {
                    field: 'Depart',
                    title: '科室'
                }, {
                    field: 'Gw',
                    title: '职务'
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

    oTableInit.queryParams = function (params) {
        var temp = {
            limit: params.limit,
            offset: params.offset,
            roleid: $("#roleid").val(),
            loginId: $("#txt_ks_loginId").val(),
            dept: $("#sel_ks_ks option:selected").val()
        };
        return temp;
    };
    return oTableInit;
};
var UserInit = function () {
    var oRableInit = new Object();
    oRableInit.Init = function () {
        $('#userTable').bootstrapTable({
            url: '/User/GetUserForRole',
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
            roleid: $("#roleid").val(),
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

        $("#btn_jsry_query").click(function () {
            $("#jsryTable").bootstrapTable('refresh');
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
            postdata.roleid = $("#roleid").val();
            $.ajax({
                type: "post",
                url: "/Home/AddRoleUser",
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

        $("#btn_jsry_add").click(function () {
            $("#hid_user_type").val(0);
            $("#userTable").bootstrapTable('refresh');
            $('#mymodal').modal({
                show: true,
                backdrop: 'static'
            });
        });
    };
    return oInit;
};
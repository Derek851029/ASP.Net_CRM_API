<%@ Page Language="C#" EnableEventValidation="true" AutoEventWireup="true" MasterPageFile="~/MasterPage.master" CodeFile="0060010001.aspx.cs" Inherits="_0060010001" %>

<asp:Content ID="Content" ContentPlaceHolderID="head2" runat="Server">
    <script src="../DataTables/jquery.dataTables.min.js"></script>
    <script type="text/javascript">
        $(function () {
            $('#System').css({ "background-color": "#1a9bfc", "color": "white" })
            $('[data-toggle="tooltip"]').tooltip();
            bindTable();
        });

        function bindTable() {
            $.ajax({
                url: '0060010001.aspx/GetPartnerList',
                type: 'POST',
                contentType: 'application/json; charset=UTF-8',
                dataType: "json",       //如果要回傳值，請設成 json
                success: function (doc) {
                    var table = $('#data').DataTable({
                        destroy: true,
                        data: eval(doc.d),
                        "oLanguage": {
                            "sLengthMenu": "顯示 _MENU_ 筆記錄",
                            "sZeroRecords": "無符合資料",
                            "sInfo": "顯示第 _START_ 至 _END_ 項結果，共 _TOTAL_ 項",
                            "sInfoFiltered": "(從 _MAX_ 項結果過濾)",
                            "sInfoPostFix": "",
                            "sSearch": "搜索:",
                            "sUrl": "",
                            "oPaginate": {
                                "sFirst": "首頁",
                                "sPrevious": "上頁",
                                "sNext": "下頁",
                                "sLast": "尾頁"
                            }
                        },
                        "columnDefs": [{
                            "targets": -1,
                            "data": null,
                            "searchable": false,
                            "paging": false,
                            "ordering": false,
                            "info": false
                        }],
                        columns: [
                                { data: "ROLE_ID" },
                                { data: "ROLE_NAME" },
                                { data: "UpDateUser" },
                                { data: "UpDateDate" },
                                {
                                    data: "ROLE_ID", render: function (data, type, row, meta) {
                                        var bt = "<button type='button' id='button' class='btn btn-outline-success btn-lg' " +
                                            "data-bs-toggle='modal' data-bs-target='#myModal' >" +
                                            "<span class='glyphicon glyphicon-plus'>" +
                                            "</span>&nbsp;開放功能</button>" +

                                            " <button type='button' id='edit' class='btn btn-outline-primary btn-lg' " +
                                            "data-bs-toggle='modal' data-bs-target='#newModal' >" +
                                            "<span class='glyphicon glyphicon-pencil'>" +
                                            "</span>&nbsp;修改</button>";
                                        return bt
                                    }
                                },
                        ]
                    });

                    $('#data tbody').unbind('click').
                        on('click', '#button', function () {
                            var table = $('#data').DataTable();
                            var ROLE_ID = table.row($(this).parents('tr')).data().ROLE_ID;
                            var ROLE_NAME = table.row($(this).parents('tr')).data().ROLE_NAME;
                            document.getElementById("txt_title").innerHTML = '【' + ROLE_NAME + '】選單設定';
                            List_PROGLIST(ROLE_ID);
                        }).
                        on('click', '#edit', function () {
                            var table = $('#data').DataTable();
                            var ROLE_ID = table.row($(this).parents('tr')).data().ROLE_ID;
                            Load_Modal(ROLE_ID);
                        });
                }
            });
        }

        function List_PROGLIST(ROLE_ID) {
            $.ajax({
                url: '0060010001.aspx/List_PROGLIST',
                type: 'POST',
                data: JSON.stringify({ ROLE_ID: ROLE_ID }),
                contentType: 'application/json; charset=UTF-8',
                dataType: "json",
                success: function (doc) {
                    var obj = JSON.parse('{"data":' + doc.d + '}');
                    if (obj.data[0].TREE_ID == "NULL") {
                        alert("查無此權限代碼，，請詢問管理人員，謝謝。");
                        return;
                    }
                    var table = $('#data2').DataTable({
                        destroy: true,
                        data: eval(doc.d),
                        "oLanguage": {
                            "sLengthMenu": "顯示 _MENU_ 筆記錄",
                            "sZeroRecords": "無符合資料",
                            "sInfo": "顯示第 _START_ 至 _END_ 項結果，共 _TOTAL_ 項",
                            "sInfoFiltered": "(從 _MAX_ 項結果過濾)",
                            "sInfoPostFix": "",
                            "sSearch": "搜索:",
                            "sUrl": "",
                            "oPaginate": {
                                "sFirst": "首頁",
                                "sPrevious": "上頁",
                                "sNext": "下頁",
                                "sLast": "尾頁"
                            }
                        },
                        "aLengthMenu": [[25, 50, 100], [25, 50, 100]],
                        "iDisplayLength": 25,
                        "columnDefs": [{
                            "targets": -1,
                            "data": null,
                            "searchable": false,
                            "paging": false,
                            "ordering": false,
                            "info": false
                        }],
                        columns: [
                                { data: "TREE_ID" },
                                { data: "M_TREE_NAME" },
                                { data: "TREE_NAME" },
                                { data: "Agent_Name" },
                                { data: "UpDateDate" },
                                {
                                    data: "NowStatus", render: function (data, type, row, meta) {
                                        var checked = 'checked/>'
                                        if (data == '0') {
                                            checked = '/>'
                                        }
                                        return "<div class='checkbox'><label>" +
                                            "<input type='checkbox' style='width: 30px; height: 30px;' id=check " +
                                            checked + "</label></div>";
                                    }
                                }]
                    });
                    //================================

                    $('#data2 tbody').unbind('click').
                        on('click', '#check', function () {
                            var table = $('#data2').DataTable();
                            var Flag = table.row($(this).parents('tr')).data().NowStatus;
                            var TREE_ID = table.row($(this).parents('tr')).data().TREE_ID;
                            Check(Flag, TREE_ID, ROLE_ID.toString());
                        });
                }
            });
        }

        function Check(Flag, TREE_ID, ROLE_ID) {
            $.ajax({
                url: '0060010001.aspx/Check_Menu',
                type: 'POST',
                data: JSON.stringify({
                    Flag: Flag,
                    TREE_ID: TREE_ID,
                    ROLE_ID: ROLE_ID,
                }),
                contentType: 'application/json; charset=UTF-8',
                dataType: "json",
                success: function (doc) {
                    var json = JSON.parse(doc.d.toString());
                    alert(json.status);
                }
            });
            List_PROGLIST(ROLE_ID);
        }

        //================ 新增【使用者權限】===============
        function New(Flag) {
            document.getElementById("btn_update").disabled = true;
            document.getElementById("btn_new").disabled = true;
            var Flag = Flag;
            var txt_ROLE_ID = document.getElementById("txt_ROLE_ID").value;
            var txt_ROLE_NAME = document.getElementById("txt_ROLE_NAME").value;
            $.ajax({
                url: '0060010001.aspx/New',
                type: 'POST',
                data: JSON.stringify({
                    Flag: Flag,
                    ROLE_ID: txt_ROLE_ID,
                    ROLE_NAME: txt_ROLE_NAME,
                }),
                contentType: 'application/json; charset=UTF-8',
                dataType: "json",
                success: function (doc) {
                    var json = JSON.parse(doc.d.toString());
                    if (json.status == "new") {
                        alert("新增完成！")
                        bindTable();
                    }
                    else if (json.status == "update") {
                        alert("修改完成！");
                        bindTable();
                    }
                    else {
                        alert(json.status);
                    }
                    document.getElementById("btn_update").disabled = false;
                    document.getElementById("btn_new").disabled = false;
                },
                error: function () {
                    document.getElementById("btn_update").disabled = false;
                    document.getElementById("btn_new").disabled = false;
                }
            });
        }

        function Xin_De() {
            Load_Modal(0);
        }

        function Load_Modal(Flag) {
            document.getElementById("txt_ROLE_ID").disabled = false;
            document.getElementById("title_modal").innerHTML = '';
            document.getElementById("txt_ROLE_ID").value = '';
            document.getElementById("txt_ROLE_NAME").value = '';
            document.getElementById("btn_new").style.display = "none";
            document.getElementById("btn_update").style.display = "none";
            if (Flag == 0) {
                document.getElementById("btn_new").style.display = "";
                document.getElementById("title_modal").innerHTML = '使用者權限（新增）';
            } else {
                document.getElementById("txt_ROLE_ID").disabled = true;
                document.getElementById("btn_update").style.display = "";
                document.getElementById("title_modal").innerHTML = '使用者權限（修改）';
                $.ajax({
                    url: '0060010001.aspx/Load_ROLELIST',
                    type: 'POST',
                    data: JSON.stringify({ ROLE_ID: Flag, }),
                    contentType: 'application/json; charset=UTF-8',
                    dataType: "json",       //如果要回傳值，請設成 json
                    success: function (doc) {
                        var obj = JSON.parse('{"data":' + doc.d + '}');
                        document.getElementById("txt_ROLE_ID").value = obj.data[0].ROLE_ID;
                        document.getElementById("txt_ROLE_NAME").value = obj.data[0].ROLE_NAME;
                    },
                    error: function () {

                    }
                });
            }
        }
    </script>
    <style>
        body {
            font-family: "Microsoft JhengHei",Helvetica,Arial,Verdana,sans-serif;
            font-size: 18px;
        }
    </style>
    <!-- ====== Modal ====== -->
    <div class="modal fade" id="myModal" data-bs-keyboard="false" data-bs-backdrop="static" style="overflow: auto">
        <div class="modal-dialog modal-lg">

            <!-- Modal content-->

            <div class="modal-content">
                <div class="modal-header">
                    <h2 class="modal-title" id="txt_title">新增會員資料</h2>
                    <button type="button" class="btn-close" data-bs-dismiss="modal" aria-label="Close"></button>
                </div>
                <div class="modal-body">
                    <!-- ========================================== -->
                    <table id="data2" class="display table table-striped" style="width: 99%">
                        <thead>
                            <tr>
                                <th style="text-align: center;">選單編號</th>
                                <th style="text-align: center;">選單分類</th>
                                <th style="text-align: center;">選單名稱</th>
                                <th style="text-align: center;">異動者</th>
                                <th style="text-align: center;">異動日期</th>
                                <th style="text-align: center;">維護</th>
                            </tr>
                        </thead>
                    </table>
                    <!-- ========================================== -->
                </div>
                <div class="modal-footer">
                    <button type="button" class="btn btn-secondary" data-bs-dismiss="modal">關閉</button>
                </div>
            </div>

            <!-- =========== Modal content =========== -->

        </div>
    </div>

    <!-- ====== Modal ====== -->
    <div class="modal fade" id="newModal" data-bs-keyboard="false" data-bs-backdrop="static" style="overflow: auto">
        <div class="modal-dialog modal-lg">

            <!-- Modal content-->

            <div class="modal-content">
                <div class="modal-header">
                    <h2 class="modal-title" id="title_modal"></h2>
                    <button type="button" class="btn-close" data-bs-dismiss="modal" aria-label="Close"></button>
                </div>
                <div class="modal-body">
                    <!-- ========================================== -->
                    <table class="display table table-striped" style="width: 99%">
                        <thead>
                            <tr>
                                <th style="text-align: center" colspan="4">
                                    <span style="font-size: 20px"><strong>使用者權限</strong></span>
                                </th>
                            </tr>
                        </thead>
                        <tbody>
                            <tr>
                                <th style="text-align: center; width: 50%; height: 55px;">
                                    <strong>權限代碼</strong>
                                </th>
                                <th style="text-align: center; width: 50%">
                                    <div data-toggle="tooltip" title="必填，只能填大於１００小於９９９的數字">
                                        <input type="text" id="txt_ROLE_ID" name="txt_Agent_Team" class="form-control" placeholder="權限代碼"
                                            onkeyup="value=value.replace(/[^\d]/g,'') " maxlength="3" style="Font-Size: 18px; background-color: #ffffbb" />
                                    </div>
                                </th>
                            </tr>
                            <!-- ========================================== -->
                            <tr>
                                <th style="text-align: center; width: 50%; height: 55px;">
                                    <strong>權限名稱</strong>
                                </th>
                                <th style="text-align: center; width: 50%">
                                    <div data-toggle="tooltip" title="必填，不能超過１０個字元">
                                        <input type="text" id="txt_ROLE_NAME" name="txt_Agent_Name" class="form-control" placeholder="權限名稱"
                                            maxlength="10" style="Font-Size: 18px; background-color: #ffffbb" />
                                    </div>
                                </th>
                            </tr>
                        </tbody>
                    </table>
                    <!-- ========================================== -->
                </div>
                <div class="modal-footer">
                    <button type="button" class="btn btn-secondary" data-bs-dismiss="modal">關閉</button>
                    <button id="btn_new" type="button" class="btn btn-success" onclick="New(0)">新增</button>
                    <button id="btn_update" type="button" class="btn btn-primary" onclick="New(1)">修改</button>
                </div>
            </div>

            <!-- =========== Modal content =========== -->

        </div>
    </div>
    <!--===================================================-->
    <div class="col-6">
                <nav aria-label="breadcrumb">
                    <ol class="breadcrumb mb-0 d-flex align-items-center">
                        <li class="breadcrumb-item">
                            <a href="Member.aspx" class="link">
                                <i class="bi-house"></i>
                            </a>
                        </li>
                        <li class="breadcrumb-item active" aria-current="page">系統參數維護</li>
                    </ol>
                    </nav>
                <h1 class="mb-0 fw-bold">使用者權限維護
                    <button type="button" class="btn btn-success btn-lg" data-bs-toggle="modal" data-bs-target="#newModal" onclick="Xin_De()"><span class="glyphicon glyphicon-plus">新增權限</span></button>
                </h1> 
        </div>
        <table id="data" class="table table-striped" style="text-align:center;width:100%">
            <thead>
                <tr>
                    <th style="text-align: center;">權限代碼</th>
                    <th style="text-align: center;">權限名稱</th>
                    <th style="text-align: center;">異動人員</th>
                    <th style="text-align: center;">異動時間</th>
                    <th style="text-align: center;">功能選單</th>
                </tr>
            </thead>
        </table>
</asp:Content>

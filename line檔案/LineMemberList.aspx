<%@ Page Title="LINE會員資料" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="LineMemberList.aspx.cs" Inherits="LineMemberList" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="head2" Runat="Server">
    
    <script src="../DataTables/jquery.dataTables.min.js"></script>
    <script src="js/moment.min.js"></script>

    
    <style>
        body {
            font-family: "Microsoft JhengHei",Helvetica,Arial,Verdana,sans-serif;
            font-size: 18px;
        }

        thead th {
            background-color: #666666;
            color: white;
        }

        tr td:first-child,
        tr th:first-child {
            border-top-left-radius: 8px;
            border-bottom-left-radius: 8px;
        }

        tr td:last-child,
        tr th:last-child {
            border-top-right-radius: 8px;
            border-bottom-right-radius: 8px;
        }

        #data_company td:nth-child(2), #data_company td:nth-child(1),
        #data2 td:nth-child(3), #data2 td:nth-child(1),
        #data td:nth-child(6), #data td:nth-child(5), #data td:nth-child(4),
        #data td:nth-child(3), #data td:nth-child(2), #data td:nth-child(1) {
            text-align: center;
        }
        .auto-style1 {
            width: 182px;
        }

        #data th, #data td{
            text-align:center;
        }
        #agent-data th, #agent-data td{
            text-align:center;
        }
    </style>

    

    <div class="container">
        <div class="row">
            <h2>
                <strong>
                    LINE會員資料
                </strong>
            </h2>
        </div>
        <div class="row">
            <table id="data" class="display table table-striped table-sm" style="width:100%;text-align:center;">
                <thead>
                    <tr>
                        <th>編號</th>
                        <th>身分</th>
                        <th>姓名</th>
                        <th>LINE名稱</th>
                        <th>狀態</th>
                        <th>編輯</th>
                        <th>聊天紀錄</th>
                    </tr>
                </thead>
                <tbody></tbody>
            </table>
        </div>

    </div>

    



    <script type="text/javascript">

        $(function () {
            bindMember();

        });


        function bindMember() {
            $.ajax({
                type: 'post',
                url: 'LineMemberList.aspx/bindMember',
                contentType: 'application/json;utf-8',
                data: JSON.stringify({}),
                dataType: 'json',
                success: function (doc) {
                    $('#data').dataTable({
                        searching: true,
                        destroy: true,
                        data: eval(doc.d),
                        deferRender: true,
                        "oLanguage": {
                            "sLengthMenu": "顯示 _MENU_ 筆",
                            "sZeroRecords": "無資料",
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
                        "aaSorting": [[0, 'asc']],
                        "aLengthMenu": [[10,50], [10,50]],
                        "iDisplayLength": 10,
                        "columnDefs": [{
                            "targets": -1,
                            "data": null,
                            "searchable": false,
                            "paging": false,
                            "ordering": false,
                            "info": false,
                        }],
                        columns: [
                            { "data": "LMBID" },
                            { "data": "MIdentity" },
                            { "data": "MName" },
                            { "data": "UserName" },
                            {
                                "data": "flag",
                                "render": function (data, type, full, meta) {
                                    if (data == '1') {
                                        return '封鎖';
                                    }
                                    return '好友';
                                }
                            },
                            {
                                "data": "LMBID",
                                "render": function (data, type, full, meta) {
                                    let bt = '<button id="" type="button" class="btn btn-primary btn-lg" onclick="openedit(' + data + ')">編輯</button>';
                                    return bt;
                                }
                            },
                            {
                                "data": "LMBID",
                                "render": function (data, type, full, meta) {
                                    var bt = '<button id="view" type="button" class="btn btn-info btn-lg" onclick="openpage(' + data + ')">訊息</button>';
                                    return bt;
                                }
                            }
                        ]
                    });


                }
            });
        }

        function openpage(id) {
            $.ajax({
                type: 'post',
                url: 'LineMemberList.aspx/openpage',
                contentType: 'application/json;utf-8',
                data: JSON.stringify({ID:id}),
                dataType: 'json',
                success: function (doc) {
                    location.href = 'LineMsgList.aspx';
                }
            })
        }

        function openedit(id) {
            $.ajax({
                type: 'post',
                url: 'LineMemberList.aspx/openedit',
                contentType: 'application/json;utf-8',
                data: JSON.stringify({ ID: id }),
                dataType: 'json',
                success: function (doc) {
                    location.href = 'MemberEdit.aspx';
                }
            })
        }


    </script>


</asp:Content>


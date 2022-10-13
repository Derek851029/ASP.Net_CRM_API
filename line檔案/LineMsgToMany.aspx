<%@ Page Title="LINE群發訊息" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="LineMsgToMany.aspx.cs" Inherits="LineMsgToMany" %>

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
        
        #familydata table , #familydata th, #familydata td{
            font-size:25px;
            width:20%;
            padding:5px;
            margin:auto;
            border:3px double #aaa;
        }
    </style>

    

    <div class="container">
        <div>
            <h2><strong>LINE群發訊息</strong></h2>
        </div>
        <div id="msgbar" style="margin-top:20px;">
            <input type="text" id="msg" style="width:90%;" />
            <button type="button" id="send" class="btn btn-info" onclick="getSelect();">發送</button>
        </div>
        <div>
            <table style="width:100%;margin-top:50px;">
                <tr>
                    <th style="text-align:right;">
                        <button type="button" id="all" class="btn btn-success" onclick="SelectAll()">全選</button>
                        <button type="button" id="clear" class="btn btn-warning" onclick="ClearSelect()">清除</button>
                    </th>
                </tr>
            </table>
        </div>
        <div id="memberdiv" style="margin-top:20px;margin-bottom:100px;">
            <table id="data" class="display table table-striped table-sm" style="width:100%;text-align:center;">
                <thead>
                    <tr>
                        <th>編號</th>
                        <th>家長姓名</th>
                        <th>LINE名稱</th>
                        <th>班級</th>
                        <th>學生姓名</th>
                        <th>選擇</th>
                    </tr>
                </thead>
                <tbody></tbody>
            </table>

        </div>
    </div>




    <script type="text/javascript">

        $(function () {
            bindata();
        });

        function bindata() {
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
                        "aLengthMenu": [[10, 50], [10, 50]],
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
                            { "data": "Name" },
                            { "data": "UserName" },
                            { "data": "Class" },
                            { "data": "StudentName" },
                            {
                                "data": "LMBID",
                                "render": function (data, type, full, meta) {
                                    return '<input type="checkbox" value="' + data + '" style="zoom:200%;" />';
                                }
                            }
                        ]
                    });


                }
            });

        }


        function SelectAll() {
            var table = $('#data').DataTable();
            var choosecollection = table.$('input[type="checkbox"]', { "page": "all" });
            choosecollection.each(function () {
                //console.log($(this).val());
                $(this).prop('checked', true);
            });
        }

        function ClearSelect() {
            var table = $('#data').DataTable();
            var choosecollection = table.$('input[type="checkbox"]', { "page": "all" });
            choosecollection.each(function () {
                //console.log($(this).val());
                $(this).prop('checked', false);
            });
        }


        function getSelect() {
            var table = $('#data').DataTable();
            var choosecollection = table.$('input[type="checkbox"]:checked', { "page": "all" });
            let msg = $('#msg').val();
            choosecollection.each(function () {
                //console.log($(this).val());
                SendMsg($(this).val(), msg);
            });
        }


        function SendMsg(id,txt) {
            $.ajax({
                type: 'post',
                url: 'LineMsgList.aspx/SendMsg',
                contentType: 'application/json;utf-8',
                data: JSON.stringify({
                    ID: id,
                    msg: txt
                }),
                dataType: 'json',
                success: function (doc) {
                    
                }
            })
        }


    </script>

</asp:Content>


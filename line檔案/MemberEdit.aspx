<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="MemberEdit.aspx.cs" Inherits="MemberEdit" %>

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
        <div style="margin-top:20px;">
            <button type="button" class="btn btn-warning btn-lg" onclick="location.href='LineMemberList.aspx';">返回</button>

        </div>
        <div>
            <table id="familydata" style="margin-top:5px;width:100%;">
                <tr>
                    <th colspan="3">
                        人員資料
                        <input type="hidden" id="MID" data-addtype="f" />
                    </th>
                </tr>
                <tr>
                    <th style="width:33%;">姓名</th>
                    <th style="width:33%;">性別</th>
                    <th style="width:33%;">手機</th>
                </tr>
                <tr>
                    <th>
                        <input type="text" id="MName" name="MName" data-addtype="f" />
                    </th>
                    <th>
                        <select id="Sex" name="Sex" data-addtype="f">
                            <option>男</option>
                            <option>女</option>
                        </select>
                    </th>
                    <th>
                        <input type="text" id="Phone" name="Phone" data-addtype="f" />
                    </th>
                </tr>
                <tr>
                    <th>身分</th>
                    <th colspan="2">
                        <select id="MIdentity" data-addtype="f" >
                            <option>志工</option>
                            <option>會員</option>
                            <option>親友</option>
                            <option>非會員</option>
                        </select>
                    </th>
                </tr>
                <tr>
                    <th>信箱</th>
                    <th colspan="2">
                        <input type="text" style="width:100%;" id="Email" data-addtype="f" />
                    </th>
                </tr>
                <tr>
                    <th>地址</th>
                    <th colspan="3">
                        <input type="text" style="width:100%;" id="MAddress" data-addtype="f" />
                    </th>
                </tr>
                <tr>
                    <th colspan="3" style="text-align:center;">
                        <button type="button" class="btn btn-info btn-lg" onclick="EditUser();">儲存修改</button>
                    </th>
                </tr>
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
                url: 'MemberEdit.aspx/bindata',
                contentType: 'application/json;utf-8',
                data: JSON.stringify({}),
                dataType: 'json',
                success: function (doc) {
                    if (doc.d == '') {
                        alert('找不到此人');
                        location.href = 'LineMemberList.aspx';
                    }
                    let datas = JSON.parse(doc.d);
                    let data = datas.user[0];
                    $('[data-addtype="f"]').each(function () {
                        $(this).val(eval('data.' + $(this).attr('id')));
                    });
                }
            })
        }


        function EditUser() {
            let data = {};
            $('[data-addtype="f"]').each(function () {
                data[$(this).attr('id')] = $(this).val();
            });
            $.ajax({
                type: 'post',
                url: 'MemberEdit.aspx/EditUser',
                contentType: 'application/json;utf-8',
                data: JSON.stringify({ data : data }),
                dataType: 'json',
                success: function (doc) {
                    alert(doc.d);
                }
            });
        }


    </script>

</asp:Content>


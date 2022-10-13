<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="0060010005.aspx.cs" Inherits="_2021_case_0060010005" %>

<asp:Content ID="Content2" ContentPlaceHolderID="head2" Runat="Server">
    <style>
        body
        {
            font-family: "Microsoft JhengHei",Helvetica,Arial,Verdana,sans-serif;
            font-size: 18px;
        }

        thead th
        {
            background-color: #666666;
            color: white;
        }

        tr td:first-child,
        tr th:first-child
        {
            border-top-left-radius: 8px;
            border-bottom-left-radius: 8px;
        }

        tr td:last-child,
        tr th:last-child
        {
            border-top-right-radius: 8px;
            border-bottom-right-radius: 8px;
        }

        #data2 td:nth-child(6), #data2 td:nth-child(5), #data2 td:nth-child(4),
        #data2 td:nth-child(3), #data2 td:nth-child(2), #data2 td:nth-child(1),
        #data td:nth-child(6), #data td:nth-child(5), #data td:nth-child(4),
        #data td:nth-child(3), #data td:nth-child(2), #data td:nth-child(1), #data th:nth-child(5)
        {
            text-align: center;
        }
    </style>
     <div class="table-responsive" style="width: 95%; margin: 10px 20px">
        <h2><strong>案件資料清單&nbsp; &nbsp;
<%--                <button type="button" class="btn btn-success btn-lg" data-toggle="modal" data-target="#newModal" style="Font-Size: 20px;" onclick="New()">
                    <span class='glyphicon glyphicon-plus'></span>&nbsp;&nbsp;指定查詢</button>--%>
        </strong></h2>
        <table id="data" class="display table table-striped" style="width: 99%">
            <thead>
                <tr>                    
                    <th style="text-align: center; width: 10%;">登錄日期</th>
                    <th style="text-align: center; width: 15%;">案件編號</th>
                    <th style="text-align: center; width: 17.5%">案件名稱</th>
                    <th style="text-align: center; width: 10%">意見類別</th>
                    <th style="text-align: center; width: 10%">派工人員</th>
                    <th style="text-align: center; width: 10%">案件狀態</th>
                    <th style="text-align: center; width: 10%">詳情</th>
                    <th style="text-align: center; width: 10%">修改</th>
                </tr>
            </thead>
        </table>
    </div>

    <script src="../DataTables/jquery.dataTables.min.js"></script>
    <script src="0060010005.js"></script>
</asp:Content>


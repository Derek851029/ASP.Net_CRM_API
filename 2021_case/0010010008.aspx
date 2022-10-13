<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="0010010008.aspx.cs" Inherits="_2021_case_0010010008" %>

<asp:Content ID="Content2" ContentPlaceHolderID="head2" Runat="Server">
    <style>
        body
        {
            font-family: "Microsoft JhengHei",Helvetica,Arial,Verdana,sans-serif;
            font-size: 18px;
        }
        input, textarea {
           font-size: initial;
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

    <div id="main" style="text-align:center">
        <table class="display table table-striped" style="width: 99%">
            <thead>
                <tr>
                    <th style="text-align: center" colspan="4">
                        <span style="font-size: 20px"><strong><label id="">搜尋條件</label></strong></span>
                    </th>
                </tr>
            </thead>
            <tbody>
                <tr>
                    <th>
                        <strong>選擇部門/人員</strong>
                    </th>
                    <th>
                        <select id="Depart" class="form-control" style="background-color:#ffffbb;">
                            <option value="-1">請選擇部門…</option>
                        </select>
                        <select id="People" class="form-control" style="background-color: #ffffbb">
                            <option value="-1">請選擇人員…</option>
                        </select>
                    </th>
                </tr>
                <tr>
                    <th>
                        <strong>案件狀態</strong>
                    </th>
                    <th>
                        <select id="Case" class="form-control" style="background-color:#ffffbb;">
                            <option value="-1">請選擇案件狀態…</option>
                            <option value="0">開發中</option>
                            <option value="1">裝機中</option>
                            <option value="2">維護中</option>
                            <option value="3">結案</option>
                        </select>
                    </th>
                </tr>
            </tbody>
        </table>
        <button id="Search" type="button" class="btn btn-success btn-lg"><span class="glyphicon glyphicon-search"></span>&nbsp;&nbsp;搜尋</button>
    </div>

    <div id="main2">
        <div class="table-responsive" style="width: 95%; margin: 10px 20px">
            <h2><strong>&nbsp; &nbsp; 案件列表&nbsp; &nbsp;</strong></h2>
            
           <table id="data" class="display table table-striped" style="width: 99%">
                <thead>
                    <tr>
                        <th style="text-align: center;" width: 20%;>案件名稱</th>
                        <th style="text-align: center;" width: 20%;>客戶名稱</th>
                        <th style="text-align: center;" width: 20%;>客戶窗口</th>
                        <th style="text-align: center;" width: 10%;>窗口電話</th>
                        <th style="text-align: center;" width: 10%;>負責人</th>
                        <th style="text-align: center;" width: 10%;>狀態</th>
                        <th style="text-align: center;" width: 20%;>查詢</th>
                    </tr>
                    <%--  =========== 勞工資料 ===========--%>
                </thead>
            </table>
        </div>
        <div class="table-responsive" style="width: 95%; margin: 10px 20px">
            <h2><strong>&nbsp; &nbsp; 交辦列表&nbsp; &nbsp;</strong>
            </h2>
            
           <table id="data2" class="display table table-striped" style="width: 99%">
                <thead>
                    <tr>
                        <th style="text-align: center;" width: 20%;>案件名稱</th>
                        <th style="text-align: center;" width: 20%;>緊急程度</th>
                        <th style="text-align: center;" width: 20%;>交辦事項</th>
                        <th style="text-align: center;" width: 20%;>負責人</th>
                        <th style="text-align: center;" width: 10%;>狀態</th>
                    </tr>
                    <%--  =========== 勞工資料 ===========--%>
                </thead>
            </table>
        </div>
    </div>

    <script src="../js/notiflix.js"></script>
    <link href="../js/notiflix.css" rel="stylesheet" />
    <script src="../js/jquery.min.js"></script>
    <link href="../DataTables/jquery.dataTables.min.css" rel="stylesheet" />
    <script src="../DataTables/jquery.dataTables.min.js"></script>
    <script src="0010010008.js"></script>
</asp:Content>


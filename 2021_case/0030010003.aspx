<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="0030010003.aspx.cs" Inherits="_2021_case_0030010003" %>

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
    <div id="main">
        <div class="table-responsive" style="width: 95%; margin: 10px 20px">
            <h2><strong>&nbsp; &nbsp; 接單處理&nbsp; &nbsp;</strong></h2>

           <table id="data" class="display table table-striped" style="width: 99%">
            
                <thead>
                    <tr>
                        <th style="text-align: center;" width: 10%;>登錄日期</th>
                        <th style="text-align: center;" width: 10%;>預計時程</th>
                        <th style="text-align: center;" width: 10%;>案件編號</th>
                        <th style="text-align: center;" width: 20%;>案件名稱</th>
                        <th style="text-align: center;" width: 10%;>意見類別</th>
                        <th style="text-align: center;" width: 10%;>派工人員</th>
                        <th style="text-align: center;" width: 10%;>案件狀態</th>
                        <th style="text-align: center;" width: 10%;>功能</th>
                    </tr>
                    <%--  =========== 勞工資料 ===========--%>
           
                </thead>
            </table>


        
            <div id="calendar"></div>
        </div>
    </div>

    <link href="../fullcalendar-2.8.0/fullcalendar.css" rel="stylesheet" />
    <link href="../fullcalendar-2.8.0/fullcalendar.print.css" rel="stylesheet" media="print" />
    <link href="../fullcalendar-2.8.0/fullcalendar.min.css" rel="stylesheet" />
    <script src="../fullcalendar-2.8.0/lib/moment.min.js"></script>
    <script src="../fullcalendar-2.8.0/fullcalendar.min.js"></script>
    <script src="../fullcalendar-2.8.0/lang-all.js"></script>
    <script src="../DataTables/jquery.dataTables.min.js"></script>
    <script src="0030010003.js"></script>
</asp:Content>


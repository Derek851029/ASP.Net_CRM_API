<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="0010010006.aspx.cs" Inherits="_2021_case_0010010006" %>

<asp:Content ID="Content2" ContentPlaceHolderID="head2" Runat="Server">
    <style>
        body
        {
            font-family: "Microsoft JhengHei",Helvetica,Arial,Verdana,sans-serif;
            font-size: 22px;
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
        .circle_box{
            width: 1.3em;
            height: 1.3em;
            background-color: white;
            border-radius: 50%;
            vertical-align: middle;
            border: 1px solid #ddd;
            -webkit-appearance: none;
            outline: none;
            cursor: pointer;
        }
        .circle_box:checked {
            background-color: greenyellow;
        }
        ol li{
            float:left;
            margin:2px 20px;
            white-space: nowrap;
        }

        @media screen and (max-width: 600px){ /*如果畫面小於600*/
            .form-control-case{
                width:50%
            }
            .modal-dialog{
                width:600px
            }
            #bar{
                display:none
            }
            #navbar-example{
                display:none
            }
            .modal.modal-side {max-width:100%;}
        }
        @media screen and (min-width: 600px){ /*如果畫面大於600*/
            .modal-dialog{
                width:1600px
            }
        }
    </style>
    <div id="main">
        <div class="table-responsive" style="width: 95%; margin: 10px 20px">
            <h2><strong>&nbsp; &nbsp; 個人行程管理&nbsp; &nbsp;</strong>
                <button type="button" id="btn_Add_Schdule" class="btn btn-success btn-lg" data-toggle="modal" data-target="#newModal">
                    <span class='glyphicon glyphicon-plus'></span>&nbsp;&nbsp;新增行程
                </button>
                <button type="button" id="btn_Change_Calendar" class="btn btn-warning btn-lg">
                    <span class="glyphicon glyphicon-list"></span>&nbsp;&nbsp;切換行事曆
                </button>
            </h2>
            
           <table id="data" class="display table table-striped" style="width: 99%">
                <thead>
                    <tr>
                        <th style="text-align: center;" width: 20%;>客戶姓名</th>
                        <th style="text-align: center;" width: 20%;>客戶連絡電話</th>
                        <th style="text-align: center;" width: 20%;>拜訪事項</th>
                        <th style="text-align: center;" width: 10%;>建立日期</th>
                        <th style="text-align: center;" width: 10%;>預計拜訪時程</th>
                        <th style="text-align: center;" width: 10%;>建立人員</th>
                        <th style="text-align: center;" width: 5%;>功能</th>
                    </tr>
                    <%--  =========== 勞工資料 ===========--%>
                </thead>
            </table>
            <div id="calendar" style="padding-top:30px"></div>
        </div>
    </div>

    <%--==========================================新增行程dialog=====================================--%>
    <div class="modal fade" id="newModal" role="dialog" data-backdrop="static" data-keyboard="false" style="overflow: auto">
        <div class="modal-dialog">
            <!-- Modal content-->
            <div class="modal-content">
                <div class="modal-header">
                    <button type="button" class="close" data-dismiss="modal">&times;</button>
                    <h2 class="modal-title"><strong>
                        <label id="title_modal"></label>
                    </strong>
                    </h2>
                </div>
                <div class="modal-body">
                    <table class="display table table-bordered table-striped" style="width: 99%">
                        <thead>
                            <tr>
                                <th style="text-align: center" colspan="4">
                                    <span style="font-size: 20px"><strong>拜訪行程<label id="PID"></label></strong></span>
                                </th>
                            </tr>
                        </thead>
                        <tbody>
                            <tr>
                                <th>
                                    <button id="More_Visit" type="button" class="btn btn-info btn-lg"><span class="glyphicon glyphicon-plus"></span>新增多人行程</button>
                                </th>
                            </tr>
                            <tr id="Select_Peolple_tr">
                                <th>
                                    <strong>選擇人員</strong>
                                </th>
                                <th>
                                    <button type="button" id="Choose_People" class="btn btn-primary btn-lg" data-toggle="modal" data-target="#newModal2">
                                        <span class='glyphicon glyphicon-plus'></span>&nbsp;&nbsp;選擇
                                    </button>
                                    <div><ol id="ol_People"></ol></div>
                                </th>
                            </tr>
                            <tr id="Notification_tr">
                                <th>
                                    <strong>通知方式</strong>
                                </th>
                                <th>
                                    <input type="checkbox" id="Line" style="width:20px;height:20px"/>&nbsp;<strong>Line</strong>
                                    <input type="checkbox" id="App" style="width:20px;height:20px"/>&nbsp;<strong>App</strong>
                                </th>
                            </tr>


                            <tr>
                                <th>
                                    <strong>選擇案件</strong>
                                </th>
                                <th colspan="3">
                                    <input list="Case" id="Case_Name" class="form-control-case" style="background-color: #ffffbb" placeholder="請選擇案件…"/>
                                    <datalist id="Case" class="form-control-case" style="background-color:#ffffbb"></datalist>
                                </th>
                            </tr>
                            <tr>
                                <th>
                                    <strong>客戶名稱/統一編號</strong>
                                </th>
                                <th colspan="3">
                                    <input type="text" id="Bussiness_Name" class="form-control-case" placeholder="客戶名稱" maxlength="100" style="background-color: #ffffbb"/>
                                    <input type="text" id="Bussiness_ID" class="form-control-case" placeholder="統一編號" maxlength="100" style="background-color: #ffffbb"/>
                                </th>
                            </tr> 
                            <tr>
                                <th>
                                    <strong>聯絡窗口／電話</strong>
                                </th>
                                <th colspan="3">
                                    <input type="text" id="txt_contactPerson" class="form-control-case" placeholder="聯絡人" maxlength="50" style="background-color: #ffffbb"/>
                                    <input type="text" id="txt_contactPhoneNumber" class="form-control-case" placeholder="連絡電話" maxlength="50" style="background-color: #ffffbb"/>
                                </th>     
                            </tr>   
                            <tr>
                                <th>
                                    <strong>預計拜訪時程</strong>
                                    <!--預定處理時間 -->
                                </th>
                                <th colspan="3">
                                    <div style="float: left" data-toggle="tooltip" title="">
                                        <input type="datetime-local" class="form-control-case" id="datetimepicker01" name="datetimepicker01"/>
                                    </div>
                                </th>
                            </tr>
                            <tr>
                                <th>
                                    <strong>預計結束拜訪時程</strong>
                                    <!--預定處理時間 -->
                                </th>
                                <th colspan="3">
                                    <div style="float: left" data-toggle="tooltip" title="">
                                        <input type="datetime-local" class="form-control-case" id="datetimepicker02" name="datetimepicker02"/>
                                    </div>
                                </th>
                            </tr>
                            <tr>
                                <th>
                                    <strong>拜訪事項</strong>
                                </th>
                                <th colspan="3">
                                    <textarea id="txt_Vistit_Content" class="form-control" cols="80" rows="8" placeholder="拜訪事項" style="resize: none;background-color: #ffffbb"></textarea>
                                </th>
                            </tr>
                            <tr>
                                <th>
                                    <strong>建立人員</strong>
                                </th>
                                <th colspan="3">
                                    <label id="Create_Agent"></label>
                                </th>
                            </tr>
                            <tr>
                                <th></th>
                            </tr>
                            <tr id="Service_tr">
                                <th style="text-align: center;background-color:#666666" colspan="4">
                                    <span style="font-size: 20px;color:white"><strong>服務紀錄</strong></span>
                                </th>
                            </tr>
                            <tr id="Service_tr2" style="display:none">
                                <th style="text-align: center; background-color: #B2E1FF;">
                                    <strong>服務紀錄</strong>
                                </th>
                                <th colspan="3">
                                    <textarea id="Service_Context" class="form-control" cols="90" rows="9" placeholder="填寫服務紀錄" style="resize: none; background-color: #ffffbb"></textarea>
                                </th>
                            </tr>
                            <tr id="Add_Log_tr">
                                <th></th>
                                <th style="text-align: center;">
                                    <button type="button" class="btn btn-success btn-lg" onclick="Add_Log()"><span class="glyphicon glyphicon-ok"></span>&nbsp;&nbsp;新增服務紀錄</button>
                                </th>
                            </tr>
                        </tbody>
                    </table>
                </div>
                <div class="modal-footer">
                    <button id="btn_new" type="button" class="btn btn-success btn-lg" onclick="Safe_Visit()"><span class="glyphicon glyphicon-ok"></span>&nbsp;&nbsp;新增</button>
                    <button type="button" class="btn btn-danger btn-lg" data-dismiss="modal">
                    <span class="glyphicon glyphicon-remove"></span> 
                    &nbsp;取消</button>
            </div>
            </div>
        </div>
    </div>
    
    <%--==========================================新增行程dialog=====================================--%>
    <%--===========================拜訪人員選擇dialog=======================================--%>
<div class="modal fade" id="newModal2" role="dialog" data-backdrop="static" data-keyboard="false">
    <div class="modal-dialog" style="width: 800px;">
        <div class="modal-content">
            <div class="modal-header">
                <button type="button" class="close" data-dismiss="modal">&times;</button>
                <h2 class="modal-title"><strong>人員選取</strong></h2>
            </div>
            <div class="modal-body">
                <select id="Assign_Depart" class="form-control" style="background-color:#ffffbb;margin-bottom:10px">
                    <option value="-1">請選擇部門…</option>
                </select>
                <div id="People_List_check">
                </div>
            </div>

            <div class="modal-footer">
                <button id="Push_People_List" type="button" class="btn btn-success btn-lg" data-dismiss="modal"><span class="glyphicon glyphicon-ok">新增</span></button>
                <button type="button" class="btn btn-danger btn-lg" data-dismiss="modal"><span class="glyphicon glyphicon-remove" >關閉</span></button>
            </div>
        </div>
    </div>
</div>
    <%--===========================拜訪人員選擇dialog=======================================--%>
<%--    <script src="../bootstrap-5.0.2-dist/js/bootstrap.min.js"></script>
    <link href="../bootstrap-5.0.2-dist/css/bootstrap.min.css" rel="stylesheet" />--%>
    <script src="../js/notiflix.js"></script>
    <link href="../js/notiflix.css" rel="stylesheet" />
    <link href="../fullcalendar-2.8.0/fullcalendar.css" rel="stylesheet" />
    <link href="../fullcalendar-2.8.0/fullcalendar.print.css" rel="stylesheet" media="print" />
    <link href="../fullcalendar-2.8.0/fullcalendar.min.css" rel="stylesheet" />
    <script src="../fullcalendar-2.8.0/lib/moment.min.js"></script>
    <script src="../fullcalendar-2.8.0/fullcalendar.min.js"></script>
    <script src="../fullcalendar-2.8.0/lang-all.js"></script>
    <script src="../DataTables/jquery.dataTables.min.js"></script>
    <script src="0010010006.js"></script>
</asp:Content>


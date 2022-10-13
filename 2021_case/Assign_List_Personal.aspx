<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="Assign_List_Personal.aspx.cs" Inherits="_2021_case_Assign_List_Personal" %>

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
                width:auto
            }
            .modal-dialog{
                width:auto
            }
            body{
                font-size:18px
            }
        }
        @media screen and (min-width: 600px){ /*如果畫面大於600*/
            .modal-dialog{
                width:600px
            }
        }
    </style>
    <div id="main">
        <div class="table-responsive" style="width: 95%; margin: 10px 20px">
            <h2><strong>&nbsp; &nbsp; 個人事項管理&nbsp; &nbsp;</strong>
                <%--<button type="button" id="btn_Add_Schdule" class="btn btn-success btn-lg" data-toggle="modal" data-target="#newModal">
                    <span class='glyphicon glyphicon-plus'></span>&nbsp;&nbsp;新增行程
                </button>--%>
            </h2>
            
           <table id="data" class="display table table-striped" style="width: 99%">
                <thead>
                    <tr>
                        <th style="text-align: center;" width: 20%;>案件編號</th>
                        <th style="text-align: center;" width: 20%;>案件名稱</th>
                        <th style="text-align: center;" width: 20%;>緊急程度</th>
                        <th style="text-align: center;" width: 10%;>事項標題</th>
                        <th style="text-align: center;" width: 10%;>預計時程</th>
                        <th style="text-align: center;" width: 5%;>功能</th>
                        <th style="text-align: center;" width: 5%;>詳情</th>
                    </tr>
                </thead>
            </table>
            <div id="calendar" style="padding-top:30px"></div>
        </div>
    </div>

    <%--===========================退單dialog=======================================--%>
    <div class="modal fade" id="dialog5" role="dialog" data-backdrop="static" data-keyboard="false">
        <div class="modal-dialog" style="width: 600px;">
            <!-- Modal content-->
            <div class="modal-content">
                <div class="modal-header">
                    <button type="button" class="close" data-dismiss="modal">&times;</button>
                    <h2 class="modal-title"><strong>
                        <%--<label id="title_modal"></label>--%>
                    </strong></h2>
                </div>
                <div class="modal-body">
                    <table class="display table table-striped" style="width: 99%">
                        <thead>
                            <tr>
                                <th style="text-align: center" colspan="4">
                                    <span style="font-size: 20px"><strong><label id="">退單原因</label></strong></span>
                                </th>
                            </tr>
                        </thead>
                        <tbody>
                            <tr>
                                <th>
                                    <strong>退單原因</strong>
                                </th>
                                <th>
                                    <textarea id="Chargeback_text" class="form-control" cols="40" rows="4" placeholder="退單內容" style="resize: none; background-color: #ffffbb"></textarea> <%--cols = width row = heigth--%>
                                </th>
                            </tr>
                        </tbody>
                    </table>
            </div>
            <div class="modal-footer">
                <button id="Assign_Chargeback_btn" type="button" class="btn btn-danger btn-lg" data-dismiss="modal"><span class="glyphicon glyphicon-pencil"></span>&nbsp;&nbsp;送出</button>
                <button type="button" class="btn btn-danger btn-lg" data-dismiss="modal"><span class="glyphicon glyphicon-remove"></span> &nbsp;關閉</button>
            </div>
            </div>
        </div>
    </div>
</div>
    <%--===========================退單dialog=======================================--%>
    <script src="../DataTables/jquery.dataTables.min.js"></script>
    <script src="Assign_List_Personal.js"></script>
</asp:Content>


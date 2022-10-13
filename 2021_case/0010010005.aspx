<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="0010010005.aspx.cs" Inherits="_2021_case_0010010005" %>

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
        @media screen and (max-width: 600px) {
            input{
                width:auto
            }
        }
        @media screen and (min-width: 600px) {
            .modal-dialog{
                width: 1600px
            }
        }

    </style>
<div style="font-size:24px">
        <div class="table-responsive" style="width:95%" >
            <h2>案件清單&nbsp; &nbsp;
                <%--<button id="btn_Add_Case" type="button" class="btn btn-success btn-lg">
                    <span class='glyphicon glyphicon-plus' ></span>&nbsp;&nbsp;新增案件
                </button>--%>
                <button type="button" id="btn_add_NewCase" class="btn btn-success btn-lg" data-toggle="modal" data-target="#newModal">
                    <span class='glyphicon glyphicon-plus'></span>&nbsp;&nbsp;新增案件
                </button>
                <button type="button" id="Date_Search" class="btn btn-info btn-lg" data-toggle="modal" data-target="#newModal2">
                    <span class='glyphicon glyphicon-search'></span>&nbsp;&nbsp;日期區間搜尋
                </button>
            </h2>
            <table id="dataTable_Case" class="display table table-striped" style="text-align:center;width:100%">
                <thead>
                    <tr>
                        <th style="text-align:center;">案件編號</th>
                        <th style="text-align:center;">案件名稱</th>
                        <th style="text-align:center;">客戶名稱</th>
                        <th style="text-align:center;">客戶窗口</th>
                        <th style="text-align:center;">窗口電話</th>
                        <th style="text-align:center;">負責人</th>
                        <th style="text-align:center;">創建日期</th>
                        <th style="text-align:center;">案件狀態</th>
                        <th style="text-align:center;">查詢</th>
                    </tr>
                </thead>
            </table>
        </div>
        
    <%--===========================新增案件表格=======================================--%>
    <div class="modal fade" id="newModal" role="dialog" data-backdrop="static" data-keyboard="false" style="overflow: auto;">
        <div class="modal-dialog modal-lg">
            <!-- Modal content-->
            <div class="modal-content">
                <div class="modal-header">
                    <button type="button" class="close" data-dismiss="modal">&times;</button>
                    <h2 class="modal-title"><strong>
                        <label id="title_modal"></label>
                    </strong>
                        <button type="button" id="btn_Change_Status" class="btn btn-warning btn-lg" data-toggle="modal" data-target="#dialog8" style="display:none"><span class="glyphicon glyphicon-list"></span>&nbsp;&nbsp;案件狀態更改</button>
                    </h2>
                </div>
                <div class="modal-body">
                    <table class="display table table-bordered table-striped" style="width: 99%">
                        <thead>
                                <tr id="Case_End_Reason_tr">
                                    <th style="text-align: center" colspan="4">
                                        <span style="font-size: 20px"><strong>案件<label>結案</label></strong></span>
                                    </th>
                                </tr>
                                <tr id="Case_End_Reason_tr2">
                                    <th colspan="3" style="background-color:white">
                                        <textarea id="Case_End_Reason" class="form-control" cols="90" rows="9" placeholder="結案原因" style="resize: none; background-color: #ffffbb"></textarea>
                                    </th>
                                </tr>

                            <tr>
                                <th style="text-align: center" colspan="4">
                                    <span style="font-size: 20px"><strong>案件<label id="PID">(新增)</label></strong></span>
                                </th>
                            </tr>
                        </thead>
                        <tbody>
                            <tr>
                                <th>
                                    <strong>專案名稱</strong>
                                </th>
                                <th colspan="3">
                                    <input type="text" id="txt_caseName" class="form-control" placeholder="案件名稱" maxlength="100" style="background-color: #ffffbb"/>
                                </th>
                            </tr>
                            <tr>
                                <th>
                                    <strong>客戶名稱/統一編號</strong>
                                </th>
                                <th colspan="3">
                                    <input list="select_caseList" id="BUSINESSNAME" class="form-control" style="background-color: #ffffbb" placeholder="請選擇客戶…"/>
                                    <datalist id="select_caseList"></datalist>
                                    <input type="text" id="Bussiness_ID" class="form-control" placeholder="統一編號" maxlength="100" style="background-color: #ffffbb"/>
                                </th>
                            </tr>
                            <tr>
                                <th>
                                    <strong>聯絡窗口／電話</strong>
                                </th>
                                <th colspan="3">
                                    <input type="text" id="txt_contactPerson" class="form-control" placeholder="聯絡人" maxlength="50" style="background-color: #ffffbb"/>
                                    <input type="text" id="txt_contactPhoneNumber" class="form-control" placeholder="連絡電話" maxlength="50" style="background-color: #ffffbb"/>
                                </th>     
                            </tr>                           
                            <%--<tr id="Work_List_tr">--%>
                            <tr>
                                <th>
                                    <strong>系統資料<br />(非必填)
                                        <button type="button" id="btn_SystemService" class="btn btn-primary btn-lg" data-toggle="modal" data-target="#Modal2">
                                            <span class='glyphicon glyphicon-plus'></span>選擇
                                        </button>
                                    </strong>
                                </th>
                                <th>
                                    <div id="List_system">
                                        <input type="number" id="Count" value="0" style="display:none"/>
                                    </div>
                                </th>
                                <td colspan="2">
                                    <strong>系統資料(工作事項):</strong>
                                    <ol id="ol_Work"></ol>
                                </td>
                            </tr>
                            <tr>
                                <th>
                                    <strong>配合廠商<br />(非必填)</strong>
                                </th>
                                <th colspan="3">
                                    <select id="select_Assist_Company" class="form-control">
                                        <option value="0">請選擇配合廠商…</option>
                                    </select>
                                </th>
                            </tr>
                            <tr>
                                <th>
                                    <strong>備註<br />(非必填)</strong>
                                </th>
                                <th colspan="3">
                                    <textarea id="txt_projectRemark" class="form-control" cols="80" rows="8" placeholder="備註" style="resize: none;"></textarea>
                                </th>
                            </tr>
                            <tr>
                                <th>
                                    <strong>案件說明</strong>
                                </th>
                                <th colspan="3">
                                    <textarea id="txt_projectContext" class="form-control" cols="90" rows="9" placeholder="案件描述" style="resize: none; background-color: #ffffbb"></textarea>
                                </th>        
                            </tr>
                            <tr>
                                <th>
                                    <strong>處理人員</strong>
                                </th>
                                <th colspan="3">
                                    <select id="txt_Personnel" class="form-control" style="background-color: #ffffbb">
                                    </select>
                                </th>
                            </tr>  
                            <tr>
                                <th>
                                </th>
                                <th style="text-align:right">
                                    <button id="btn_update" type="button" class="btn btn-primary btn-lg" onclick="Safe(1)"><span class="glyphicon glyphicon-pencil"></span>&nbsp;&nbsp;修改</button>
                                </th>
                            </tr>
                        </tbody>
                    </table>
                    <%--===========================服務紀錄=======================================--%>
                    <div id="Service" style="display:none">
                        <span style="font-size: 36px"><strong>服務紀錄</strong></span>
                        <button type="button" id="Service_btn" class="btn btn-success btn-lg" ><span class="glyphicon glyphicon-plus"></span>&nbsp;&nbsp;填寫服務紀錄</button>
                    
                        <table class="table table-striped" id="Service_table" style="width: 99%;display:none;margin-top:20px">
                            <thead>
                                <tr>
                                    <th style="text-align: center" colspan="8">
                                        <span style="font-size: 20px;"><strong>新增服務紀錄</strong></span><label id="Work_Log_SYSID" style="display:none"></label>
                                    </th>
                                </tr>
                            </thead>
                            <tbody>
                                <tr>
                                    <th style="text-align: center; background-color: #B2E1FF;">
                                        <strong>選擇工作事項</strong>
                                    </th>
                                    <th style="text-align: center; width:35%" >
                                            <select id="select_ListCase"  onchange="" style="background-color:#ffffbb;width:100%">
                                            </select>
                                    </th>
                                </tr>
                                <tr>
                                    <th style="text-align: center; background-color: #B2E1FF;">
                                        <strong>服務紀錄</strong>
                                    </th>
                                    <td style="text-align: left; " colspan="7">
                                        <div align="left" data-toggle="tooltip" title="必填，不能超過１０００個字元">
                                            <textarea id="txt_Work_Log" class="form-control" cols="90" rows="5" placeholder="服務紀錄"
                                                style="width: 95%; resize: none; background-color: #ffffbb; Font-Size: 18px" maxlength="1000" onkeyup='' ></textarea>
                                        </div>
                                    </td>
                                </tr>
                                <tr>
                                    <th style="text-align: center; height: 55px; width: 20%;" ></th>
                                    <th style="text-align: center; height: 55px;" colspan="6">
                                        <button id="btn_AddNewWorkLog" type="button" class="btn btn-success btn-lg">
                                            <span class="glyphicon glyphicon-ok">新增</span>
                                        </button>
                                    </th>
                                    <th style="text-align: right; height: 55px;" >
                                        <button type="button" class="btn btn-danger btn-lg" id="Service_remove_btn">
                                            <span class="glyphicon glyphicon-remove">關閉</span></button>
                                    </th>
                                </tr>
                            </tbody>
                        </table>
                    </div>
                    <div id="Service_html" style="display:none">
                        <table id="data3" class="display table table-striped" style="width: 99%">
                                <thead>
                                    <tr>
                                        <th style="text-align: center; width: 10%;">建立者</th>
                                        <th style="text-align: center; width: 10%;">工作事項</th>
                                        <th style="text-align: center; width: 10%;">日誌內容</th>
                                        <th style="text-align: center; width: 10%">建立時間</th>
                                        <th style="text-align: center; width: 10%">修改</th>
                                    </tr>
                                </thead>
                        </table>
                        
                    </div>
                    <%--===========================服務紀錄=======================================--%>
                    <%--===========================交辦紀錄=======================================--%>
                    <div id="Assign_html" style="display:none">
                        <span style="font-size: 36px"><strong>待辦事項</strong></span>
                        <button type="button" id="Assign_btn" class="btn btn-success btn-lg" data-toggle='modal' data-target='#dialog4'><span class="glyphicon glyphicon-plus"></span>&nbsp;&nbsp;交辦</button>
                        <table id="data4" class="display table table-striped" style="width: 99%">
                                <thead>
                                    <tr>
                                        <%--<th style="text-align: center; width: 10%;">交辦編號</th>--%>
                                        <th style="text-align: center; width: 10%;">建立時間</th>
                                        <th style="text-align: center; width: 10%;">建立人</th>
                                        <th style="text-align: center; width: 10%">緊急程度</th>
                                        <th style="text-align: center; width: 10%">交辦對象</th>
                                        <th style="text-align: center; width: 10%">交辦標題</th>
                                        <th style="text-align: center; width: 10%">預計時程</th>
                                        <th style="text-align: center; width: 10%">功能</th>
                                        <th style="text-align: center; width: 10%">狀態</th>
                                        <th style="text-align: center; width: 10%">查詢</th>
                                    </tr>
                                </thead>
                        </table>
                    </div>
                    <%--===========================交辦紀錄=======================================--%>
            </div>
            <div class="modal-footer">
                <button id="btn_new" type="button" class="btn btn-success btn-lg" onclick="Safe(0)"><span class="glyphicon glyphicon-ok"></span>&nbsp;&nbsp;新增</button>
                <button type="button" class="btn btn-danger btn-lg" data-dismiss="modal">
                    <span class="glyphicon glyphicon-remove"></span> 
                    &nbsp;取消</button>
            </div>
            </div>
        </div>
    </div>
    <%--===========================新增案件表格=======================================--%>

    <%--===========================選擇系統資料=======================================--%>
    <div class="modal fade" id="Modal2" role="dialog" data-backdrop="static" data-keyboard="false">
        <div class="modal-dialog" style="width: 1000px;">
            <div class="modal-content">
                <div class="modal-header">
                    <button type="button" class="close" data-dismiss="modal">&times;</button>
                    <h2 class="modal-title"><strong>
                        <label >系統資料</label>
                    </strong></h2>
                </div>
                <div class="modal-body">
                    <table id="data2" class="display table table-striped" style="width: 99%">
                        <thead>
                            <tr>
                                <th style="text-align: center;">商品名稱</th>
                                <th style="text-align: center;">參考價格</th>
                                <th style="text-align: center;">實際價格</th>
                                <th style="text-align: center;">選擇</th>
                                <th style="text-align: center;">數量</th>
                            </tr>
                        </thead>
                    </table>
                    <!-- ========================================== -->
                </div>
                <div class="modal-footer">
                    <button type="button" id="button3" class="btn btn-success btn-lg" data-dismiss="modal">
                        <span class="glyphicon glyphicon-ok"></span> &nbsp;確定</button>
                    <button type="button" class="btn btn-danger btn-lg" data-dismiss="modal">
                        <span class="glyphicon glyphicon-remove"></span> &nbsp;關閉</button>
                </div>
            </div>
        </div>
    </div>
    <%--===========================選擇系統資料=======================================--%>

    <%--===========================交辦dialog=======================================--%>

    <div class="modal fade" id="dialog4" role="dialog" data-backdrop="static" data-keyboard="false">
        <div class="modal-dialog" style="width: 1200px;">
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
                                    <span style="font-size: 20px"><strong><label id="">交辦事項</label></strong></span>
                                </th>
                            </tr>
                        </thead>
                        <tbody>
                            <tr>
                                <th>
                                    <strong>交辦部門/人員</strong>
                                </th>
                                <th>
                                    <select id="Assign_Depart" class="form-control" style="background-color:#ffffbb;">
                                        <option value="-1">請選擇部門…</option>
                                    </select>
                                    <select id="Select_Assign_People" class="form-control" style="background-color: #ffffbb">
                                        <option value="-1">請選擇交辦人員…</option>
                                    </select>
                                </th>
                            </tr>
                            <tr>
                                <th>
                                    <strong>緊急程度</strong>
                                </th>
                                <th>
                                    <select id="Urgent" style="background-color: #ffffbb">
                                        <option value="-1">請選擇緊急程度…</option>
                                        <option value="軟體維護">軟體維護</option>
                                        <option value="軟體修改">軟體修改</option>
                                        <option value="硬體維護">硬體維護</option>
                                        <option value="硬體設定">硬體設定</option>
                                        <option value="緊急故障">緊急故障</option>
                                        <option value="系統建置">系統建置</option>
                                        <option value="軟體介接">軟體介接</option>
                                    </select>
                                </th>
                            </tr>
                            <tr>
                                <th>
                                    <strong>客戶聯絡人/電話</strong>
                                </th>
                                <th>
                                    <input type="text" id="Assign_Company_Connection" class="form-control-case" placeholder="聯絡人" maxlength="50"/>
                                    <input type="text" id="Assign_Company_Phone" class="form-control-case" placeholder="連絡電話" maxlength="50"/>
                                </th>
                            </tr>
                            <tr>
                                <th>
                                    <strong>交辦事項標題</strong>
                                </th>
                                <th>
                                    <input type="text" id="Assign_title" class="form-control" placeholder="交辦事項標題" style="resize: none; background-color: #ffffbb;width:100%"/> <%--cols = width row = heigth--%>
                                </th> 
                            </tr>
                            <tr>
                                <th>
                                    <strong>交辦事項</strong>
                                </th>
                                <th>
                                    <textarea id="Assign_text" class="form-control" cols="90" rows="9" placeholder="交辦事項" style="resize: none; background-color: #ffffbb;width:100%"></textarea> <%--cols = width row = heigth--%>
                                </th> 
                            </tr>
                            <tr>
                                <th>
                                    <strong>建立人員</strong>
                                </th>
                                <th>
                                    <label id="Assign_Create_Agent"></label>
                                </th>
                            </tr>
                            <tr>
                                <td style="text-align: left; width: 15%">
                                    <strong>預計完成時間</strong>
                                    <!--預定處理時間 -->
                                </td>
                                <td style="width: 10%">
                                    <div style="float: left" data-toggle="tooltip" title="">
                                        <input type="date" class="form-control" id="datetimepicker01" name="datetimepicker01" style="width:100%"/>
                                    </div>
                                </td>
                            </tr>
                        </tbody>
                    </table>

                    <table class="display table table-striped" id="Chargeback_table" style="width: 99%;display:none">
                        <thead>
                            <tr>
                                <th style="text-align: center" colspan="4">
                                    <span style="font-size: 20px"><strong><label id="">退單</label></strong></span>
                                </th>
                            </tr>
                        </thead>
                        <tbody>
                            <tr>
                                <th>
                                    <strong>退單原因</strong>
                                </th>
                                <th>
                                    <textarea id="List_Chargeback_text" class="form-control" cols="80" rows="8" placeholder="退單內容" style="resize: none; background-color: #ffffbb"></textarea> <%--cols = width row = heigth--%>
                                </th>
                            </tr>
                        </tbody>
                    </table>
            </div>
            <div class="modal-footer">
                <button id="Assign_Add_btn" type="button" class="btn btn-danger btn-lg" data-dismiss="modal"><span class="glyphicon glyphicon-pencil"></span>&nbsp;&nbsp;交辦</button>
            </div>
            </div>
        </div>
    </div>
    <%--===========================交辦dialog=======================================--%>

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
    <%--===========================區間搜尋dialog=======================================--%>
<div class="modal fade" id="newModal2" role="dialog" data-backdrop="static" data-keyboard="false">
    <div class="modal-dialog" style="width: 1100px;">
        <div class="modal-content">
            <div class="modal-header">
                <button type="button" class="close" data-dismiss="modal">&times;</button>
                <h2 class="modal-title"><strong>
                    <label></label>區間搜尋</strong></h2>
            </div>
            <div class="modal-body">
                <table class="display table table-striped" style="width: 99%">
                    <thead>
                        <tr>
                            <th style="text-align: center" colspan="4">
                                <span style="font-size: 20px"><strong><label>查詢條件</label></strong></span>
                            </th>
                        </tr>
                    </thead>
                    <tbody>
                        <tr style="height: 55px;">
                            <th style="text-align: center; width: 35%" >
                                <strong>搜尋建單日期(起)</strong>
                            </th>
                            <td style="text-align: center; width: 35%">
                                <div style="float: left" data-toggle="tooltip" title="">
                                    <!--<input type="text" class="form-control" id="datetimepicker01" name="datetimepicker01"  placeholder=""  />-->
                                    <input type="date" class="form-control" id="datepicker01" name="datepicker01"/>
                                </div>
                            </td> 
                            <td style="text-align: center; width: 35%">
                                <strong>搜尋建單日期(末)</strong>
                            </td>
                            <td style="width: 35%">
                                <div style="float: left" data-toggle="tooltip" title="">
                                    <!--<input type="text" class="form-control" id="datetimepicker02" name="datetimepicker02" />-->
                                    <input type="date" class="form-control" id="datepicker02" name="datepicker02"/>
                                </div>
                            </td>
                        </tr>
                        <tr>
                            <th style="text-align: center; width:15%">
                                <strong>選擇負責人</strong>
                            </th>
                            <th style="text-align: center; width: 35%">
                                <div style="float: left" data-toggle="tooltip" title="">
                                    <%--<input type="text" name="" onkeyup="value=value.replace(/[^\d]/g,'')"/>--%>
                                    <select id="Dispatch_Name" name="Dispatch_Name" class="chosen-select" onchange="">
                                    </select>
                                </div>
                            </th>
<%--                            <th style="text-align: center; width: 15%">
                                <strong>選擇客戶</strong>
                            </th>
                            <th style="width: 35%">
                                <div data-toggle="tooltip" title="" style="width: 100%">
                                    <select id="DropClientCode" name="DropClientCode" class="chosen-select" onchange="">
                                        <option value="">請選擇客戶…</option>
                                    </select>
                                </div>
                            </th>--%>
                            
                        </tr>
                        <tr>
                            <th style="text-align: center; height: 55px; width: 20%;" ></th>
                            <th style="text-align: center; height: 55px;" colspan="6">
                                <button id="Search_Date" type="button" class="btn btn-success btn-lg" data-dismiss="modal">
                                    <span class="glyphicon glyphicon-ok">搜尋</span>
                                </button>
                            </th>
                            <th style="text-align: right; height: 55px;" >
                                <button type="button" class="btn btn-danger btn-lg" data-dismiss="modal">
                                    <span class="glyphicon glyphicon-remove" >關閉</span></button>
                            </th>
                        </tr>
                    </tbody>
                </table>
            </div>
        </div>
    </div>
</div>
    <%--===========================區間搜尋dialog=======================================--%>
    <%--===========================工作事項選擇dialog=======================================--%>
<div class="modal fade" id="dialog6" role="dialog" data-backdrop="static" data-keyboard="false">
    <div class="modal-dialog" style="width: 800px;">
        <div class="modal-content">
            <div class="modal-header">
                <button type="button" class="close" data-dismiss="modal">&times;</button>
                <h2 class="modal-title"><strong>工作事項選取</strong>
                    <button type="button" id="Add_Work" class="btn btn-success btn-lg" data-toggle="modal" data-target="#dialog7">
                        <span class='glyphicon glyphicon-plus'></span>&nbsp;&nbsp;新增工作事項
                    </button>
                </h2>
            </div>
            <div class="modal-body">
                <label id="Work_List_title" style="text-align:center;background-color:#666666;color:white;width:800px;height:30px;font-size:20px"></label>
                <div id="Work_List_check">
                </div>
            </div>

            <div class="modal-footer">
                <button id="Push_Work_List" type="button" class="btn btn-success btn-lg" data-dismiss="modal"><span class="glyphicon glyphicon-ok">新增</span></button>
                <button type="button" class="btn btn-danger btn-lg" data-dismiss="modal"><span class="glyphicon glyphicon-remove" >關閉</span></button>
            </div>
        </div>
    </div>
</div>
    <%--===========================工作事項選擇dialog=======================================--%>
        <%--===========================工作事項新增dialog=======================================--%>
<div class="modal fade" id="dialog7" role="dialog" data-backdrop="static" data-keyboard="false">
    <div class="modal-dialog" style="width: 800px;">
        <div class="modal-content">
            <div class="modal-header">
                <button type="button" class="close" data-dismiss="modal">&times;</button>
                <h2 class="modal-title"><strong>
                    <label></label>工作事項新增</strong>
                </h2>
            </div>
            <div class="modal-body">
                <table class="display table table-striped" style="width: 99%">
                    <thead>
                        <tr>
                            <th style="text-align: center" colspan="4">
                                <span style="font-size: 20px"><strong><label>新增工作事項</label></strong></span>
                            </th>
                        </tr>
                    </thead>
                    <tbody>
                        <tr>
                            <th style="text-align: center; width: 15%; height: 55px;">
                                <strong>工作事項</strong>
                            </th>
                            <th style="text-align: left; width: 35%">
                                <div data-toggle="tooltip" title="必填，不能超過１００個字元">
                                    <input type="text" id="Text5" name="Text5" class="form-control" placeholder="工作事項" style="background-color: #ffffbb;width:70%" />
                                </div>
                            </th>
                        </tr>
                    </tbody>
                </table>
            </div>

            <div class="modal-footer">
                <button id="Add_Work_List" type="button" class="btn btn-success btn-lg" data-dismiss="modal"><span class="glyphicon glyphicon-ok">新增</span></button>
                <button type="button" class="btn btn-danger btn-lg" data-dismiss="modal"><span class="glyphicon glyphicon-remove" >關閉</span></button>
            </div>
        </div>
    </div>
</div>
    <%--===========================工作事項新增dialog=======================================--%>
    <%--===========================案件狀態更改dialog=======================================--%>
<div class="modal fade" id="dialog8" role="dialog" data-backdrop="static" data-keyboard="false">
    <div class="modal-dialog" style="width: 800px;">
        <div class="modal-content">
            <div class="modal-header">
                <button type="button" class="close" data-dismiss="modal">&times;</button>
            </div>
            <div class="modal-body">
                <table class="display table table-striped" style="width: 99%">
                    <thead>
                        <tr>
                            <th style="text-align: center" colspan="4">
                                <span style="font-size: 20px"><strong><label>案件狀態變更</label></strong></span>
                            </th>
                        </tr>
                    </thead>
                    <tbody>
                        <tr>
                            <th style="text-align: center; width: 15%; height: 55px;">
                                <strong>選擇狀態</strong>
                            </th>
                            <th style="text-align: left; width: 35%">
                                <select id="Status" class="form-control" style="background-color:#ffffbb;">
                                    <option value="-1">請選擇案件狀態…</option>
                                    <option value="0">開發中</option>
                                    <option value="1">裝機中</option>
                                    <option value="2">維護中</option>
                                    <option value="3">結案</option>
                                </select>
                            </th>
                        </tr>
                        <tr id="End_Reason_tr" style="display:none">
                            <th style="text-align: center; width: 15%; height: 55px;">
                                <strong>結案原因</strong>
                            </th>
                            <th colspan="3">
                                <textarea id="End_Reason" class="form-control" cols="90" rows="9" placeholder="結案原因" style="resize: none; background-color: #ffffbb"></textarea>
                            </th>        
                        </tr>
                    </tbody>
                </table>
            </div>

            <div class="modal-footer">
                <button id="Change_Status" type="button" class="btn btn-success btn-lg"><span class="glyphicon glyphicon-ok">確定</span></button>
                <button type="button" class="btn btn-danger btn-lg" data-dismiss="modal"><span class="glyphicon glyphicon-remove" >取消</span></button>
            </div>
        </div>
    </div>
</div>
    <%--===========================案件狀態更改dialog=======================================--%>
    
    
    <script src="../js/notiflix.js"></script>
    <link href="../js/notiflix.css" rel="stylesheet" />
    <script src="../fullcalendar-2.8.0/lib/moment.min.js"></script>
    <script src="https://code.jquery.com/jquery-3.5.1.js"></script>
    <script src="https://cdn.datatables.net/1.10.25/js/jquery.dataTables.min.js"></script>
    <script src="https://cdn.datatables.net/responsive/2.2.9/js/dataTables.responsive.min.js"></script>
    <link href="https://cdn.datatables.net/1.10.25/css/jquery.dataTables.min.css" rel="stylesheet" />
    <link href="https://cdn.datatables.net/rowreorder/1.2.8/css/rowReorder.dataTables.min.css" rel="stylesheet" />
    <link href="https://cdn.datatables.net/responsive/2.2.9/css/responsive.dataTables.min.css" rel="stylesheet" />
    <script src="0010010005.js"></script>
    
    <script type="text/javascript">
        var seqno = '<%= seqno %>';
    </script>
</asp:Content>


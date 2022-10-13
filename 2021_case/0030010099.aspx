<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="0030010099.aspx.cs" Inherits="_2021_case_0030010099" %>

<asp:Content ID="Content2" ContentPlaceHolderID="head2" Runat="Server">
    <div id="main">
        <h2>
            <label id="str_title"></label>
        </h2>
         <%--==============案件資料紀錄===================--%>                    
        <table class="table table-bordered table-striped">
            <thead>
                <tr>
                    <th style="text-align: center" colspan="4">
                        <span style="font-size: 20px"><strong>案件資料紀錄</strong></span>
                    </th>
                </tr>
            </thead>
            <tbody>
                <tr>
                    <th style="text-align: center; width: 15%;">
                        <strong>案件編號</strong><br />   
                    </th>
                    <th style="width: 35%">
                        <label id="str_sysid"></label>
                    </th>
                    <td style="text-align: center; width: 15%;">
                        <strong>來電號碼</strong>
                    </td>
                    <td>
                        <div style="float: left" data-toggle="tooltip" title="">
                            <input type="text" class="form-control" id="Text1" name="Text1" maxlength="20" value="" />
                        </div>
                    </td>
                </tr>
                <tr id="tr_sysid" runat="server">
                    <td style="text-align: center">
                        <strong>最後修改日期</strong>
                    </td>
                    <td>
                        <div style="float: left" data-toggle="tooltip" title="更改日期時間請用上方補單鈕">
                            <label id="LoginTime" name="LoginTime" ></label>
                            <!--<input type="text" class="form-control" id="LoginTime" name="LoginTime" style="background-color: #ffffbb" value="" />-->
                        </div>
                    </td>
                    <%--<td style="text-align: center">
                        <strong>登錄人員</strong>
                    </td>
                    <td>
                        <div style="float: left" data-toggle="tooltip" title="必填">
                            <input type="text" class="form-control" id="Text2" name="Text2" maxlength="20" style="background-color: #ffffbb" value="" />
                        </div>
                    </td>--%>
                </tr>
            </tbody>
        </table>
        <%--==============案件資料紀錄===================--%>   

        <%--===================反應內容===================--%>
        <table class="table table-bordered table-striped">
            <thead>
                <tr>
                    <th style="text-align: center" colspan="4">
                        <span style="font-size: 20px"><strong>反應內容</strong></span>
                    </th>
                </tr>
            </thead>
            <tbody>
                <tr style="height: 55px;">
                    <td style="text-align: center; width: 15%">
                        <strong>選擇案件</strong>
                    </td>
                    <td style="text-align: center; width: 35%">
                        <select id="select_ListCase"  onchange="" style="background-color:#ffffbb;width:100%"></select>                       
                    </td>
                    <th style="text-align: center; width: 15%">
                        <strong>緊急程度</strong>
                    </th>
                    <th style="width: 35%">
                        <div data-toggle="tooltip" title="必選" style="width: 100%">
                            <select id="SelectUrgency" name="SelectUrgency" class="chosen-select" onchange="">
                                <option value="">請選擇緊急程度…</option>
                                <option value="維護">維護</option>
                                <option value="緊急故障">緊急故障</option>
                                <option value="重要故障">重要故障</option>
                                <option value="一般故障">一般故障</option>
                                <option value="軟體設定">軟體設定</option>
                                <option value="其他">其他</option>
                            </select>
                        </div>
                    </th>
                    
                </tr>
                <tr>
                    <td style="text-align: center; width: 15%">
                        <strong>預計時程</strong>
                        <!--預定處理時間 -->
                    </td>
                    <td style="width: 35%">
                        <div style="float: left" data-toggle="tooltip" title="">
                            <input type="date" class="form-control" id="datetimepicker01" name="datetimepicker01" />
                        </div>
                    </td>
                    <td style="text-align: center">
                        <strong>處理狀態</strong>
                    </td>
                    <th>
                        <label id="DealingProcess"></label>
                    </th>
                </tr>
                <tr style="height: 55px;">
                    <th style="text-align: center; width: 15%">
                        <strong>意見類型</strong>
                    </th>
                    <th style="width: 35%">
                        <div data-toggle="tooltip" title="必選" style="width: 100%">
                            <select id="SelectOpinionType" name="SelectOpinionType" class="chosen-select">
                                <option value="">請選擇意見類型…</option>
                                <option value="故障報修(到場)">故障報修(到場)</option>
                                <option value="故障報修(遠端)">故障報修(遠端)</option>
                                <option value="軟體修改(到場)">軟體修改(到場)</option>
                                <option value="軟體修改(遠端)">軟體修改(遠端)</option>
                                <option value="技術諮詢">技術諮詢</option>
                                <option value="其他服務">其他服務</option>
                                <option value="保固">保固</option>
                                <option value="租約">租約</option>
                                <option value="定期維護">定期維護</option>
                                <option value="測試">測試</option>
                                <option value="專案">專案</option>
                                <option value="設備擴充">設備擴充</option>
                                <option value="駐點服務">駐點服務</option>
                            </select>
                        </div>
                    </th>
                    <td style="text-align: center">
                        <strong>回覆方式</strong>
                    </td>
                    <td>
                        <div data-toggle="tooltip" title="必選" style="width: 100%">
                            <select id="ReplyType" name="ReplyType" class="chosen-select" style="background-color:#ffffff">
                                <option value="-1">請選擇回覆方式…</option>
                                <option>行動電話</option>
                                <option>市內電話</option>
                                <option>e-mail回覆</option>
                                <option>傳真回覆</option>
                            </select>
                        </div>
                    </td>
                </tr>
                <tr style="height: 55px;">
                    <td style="text-align: center; width: 15%">
                        <strong>意見內容</strong>
                    </td>
                    <td style="text-align: center; width: 85%" colspan="3">
                        <div style="float: left" data-toggle="tooltip" title="必填，不能超過1000 個字">
                            <textarea id="Opinion" name="Opinion" class="form-control" cols="120" rows="5" placeholder="意見內容" maxlength="1000" onkeyup=""
                                style="resize: none; background-color: #ffffbb"></textarea>
                        </div>
                    </td>
                </tr>
                <tr>
                    <td style="text-align: center">
                        <strong>處理人員</strong>
                    </td>
                    <td>
                         <%-- <strong>員工編號: </strong>--%>
                        <label id="A_ID"></label>
                    </td>
                    <td style="text-align: center;">
                        <strong>是否派工</strong>
                    </td>
                    <td style="zoom:120%;">
                            <input type="checkbox" id="checkbox1"/><label id="yes">是</label>
                            <input type="checkbox" id="checkbox2"/><label id="no">否</label>
                    </td>
                </tr>
            </tbody>
        </table>
        <%--===================反應內容===================--%>

        <%--===================派工===================--%>
        <table class="table table-bordered table-striped" id="Ins_people" style="display:none">
            <thead>
                <tr>
                    <th style="text-align: center" colspan="4">
                        <span style="font-size: 20px"><strong>派工人員</strong></span>
                    </th>
                </tr>
            </thead>
            <tbody>
                <tr style="height: 55px;">
                    <th style="text-align: center; width: 15%">
                        <strong>派工部門</strong>
                    </th>
                    <th style="width: 35%">
                        <select id="Chose_depart" style="background-color:#ffffbb;">
                            <option value="">請選擇派工部門…</option>
                            <option value="工程師">工程師</option>
                            <option value="業務">業務</option>
                            <option value="助理工程師">助理工程師</option>
                        </select>
                    </th>
                    <th style="text-align: center; width: 15%">
                        <strong>派工人員</strong>
                    </th>
                    <th style="width: 35%">
                        <select id="Dispatch_Name" style="background-color:#ffffbb;">
                        </select>
                    </th>
                </tr>
  <!--              <tr>
                    <td style="text-align: center">
                        <strong>派工備註</strong>
                    </td>
                    <td>
                        <div style="float: left" data-toggle="tooltip" title="最多50字">
                            <textarea id="Agent_PS" name="Agent_PS" class="form-control" cols="45" rows="3" placeholder="派工備註" maxlength="50" onkeyup="cs(this);"
                                style="resize: none;"></textarea>
                        </div>
                    </td>
                </tr>   -->
            </tbody>
        </table>
         <%--===================派工===================--%>
        <div id="down_button" style="text-align:center">
            <button type="button" class="btn btn-success btn-lg"  id="add" data-dismiss="modal" onclick="">新增</button>
            <button type="button" class="btn btn-default btn-lg" data-dismiss="modal">取消</button>
        </div>
    </div>
    <script type="text/javascript">
        var seqno = '<%= seqno %>';
        var new_mno = '<%= new_mno %>';
    </script>
    <style type="text/css">
        body {
            font-family: "Microsoft JhengHei",Helvetica,Arial,Verdana,sans-serif;
            font-size: 16px;
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

        #Location_Table td:nth-child(6), #Location_Table td:nth-child(5), #Location_Table td:nth-child(4),
        #Location_Table td:nth-child(3), #Location_Table td:nth-child(2), #Location_Table td:nth-child(1),
        #data td:nth-child(10), #data td:nth-child(9), #data td:nth-child(8), #data td:nth-child(7), #data td:nth-child(6), #data td:nth-child(5),
        #data td:nth-child(4), #data td:nth-child(3), #data td:nth-child(2), #data td:nth-child(1), #data th:nth-child(5) {
            text-align: center;
        }

        .auto-style1 {
            height: 47px;
        }
    </style>
    <script src="../js/jquery.js"></script>
    <script src="../js/jquery.min.js"></script>
    <script src="0030010099.js"></script>
</asp:Content>


<%@ Page Language="C#" EnableEventValidation="true" AutoEventWireup="true" MasterPageFile="MasterPage.master" CodeFile="Report_013.aspx.cs" Inherits="Report_013" %>

<asp:Content ID="Content" ContentPlaceHolderID="head2" runat="Server">
    <link href="../js/jquery-ui.min.css" rel="stylesheet" />
    <link href="../DataTables/jquery.dataTables.min.css" rel="stylesheet" />
    <script src="../js/jquery-ui.min.js"></script>
    <script src="../DataTables/jquery.dataTables.min.js"></script>
    <script src="../DataTables/jquery.datepicker-zh-TW.js"></script>
    <script type="text/javascript">
        $(function () {
            $("[id$='txt_S_DATETime'],[id$='txt_E_DATETime']").datepicker({ dateFormat: "yy-mm-dd" });
            $("[id$='Search_Name']");
            $("[id$='data']").on('load', function () {
                debugger;
                $("[id$='div_report']").append($(this).contents().find("style"))
                                       .append($(this).contents().find("table"));
                $("[id$='div_report'] table").addClass("table table-bordered table-striped");
            });
        });
        //var Search_Flag="1";
        var ddlText, ddlValue, ddl, lblMesg;   //options, 
        var ddlText02, ddlValue02, ddl02;
        function CacheItems() {
            //alert("CacheItems");
            ddlText = new Array();
            ddlValue = new Array();
            ddl = document.getElementById("<%=drop_businessname.ClientID %>");
            //lblMesg = document.getElementById("< %=lblMessage.ClientID%>");
            for (var i = 0; i < ddl.options.length; i++) {
                ddlText[ddlText.length] = ddl.options[i].text;
                ddlValue[ddlValue.length] = ddl.options[i].value;
            }
            //if (Search_Flag == "2") {
                /*alert("CacheItems02");
                ddlText02 = new Array();
                ddlValue02 = new Array();
                ddl02 = document.getElementById("< %=drop_enginner.ClientID %>");
                for (var i = 0; i < ddl02.options.length; i++) {
                    ddlText02[ddlText02.length] = ddl02.options[i].text;
                    ddlValue02[ddlValue02.length] = ddl02.options[i].value;
                }//*/
            //}
        }
        window.onload = CacheItems; //???*/

        function List_Search(value, flag) {
            //alert("List_Search V=" + value + "  F=" + flag);
            //Search_Flag = flag;
            if (flag == "1") {
                ddl.options.length = 0;
                for (var i = 0; i < ddlText.length; i++) {
                    if (ddlText[i].toLowerCase().indexOf(value) != -1) {
                        AddItem(ddlText[i], ddlValue[i]);
                    }
                }
                /*for (var j = 0; j < ddlText02.length; j++) {
                    if (ddlText02[i].toLowerCase().indexOf("") != -1) {
                        AddItem(ddlText02[i], ddlValue02[i]);
                    }
                }//*/
                if (ddl.options.length == 0) {
                    AddItem("查無資料", "查無資料");
                }
            }
            else {
                //alert();
                //ddl02.options.length = 0;
                alert("List_Search V=" + value + "  F=" + flag);
            }
        }
        function AddItem(text, value) {
            //alert("AddItem T=" + text + "  V=" + value);
            var opt = document.createElement("option");
            opt.text = text;
            opt.value = value;
            ddl.options.add(opt);
            /*var opt02 = document.createElement("option");
            opt02.text = text;
            opt02.value = value;
            ddl02.options.add(opt02);//*/
        }


    </script>
    <style type="text/css">
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
    </style>
    <div style="width: 1280px; margin: 10px 20px">
        <!--===================================================-->
        <h2><strong>【新服務單報表】</strong></h2>
        <table class="table table-bordered table-striped" style="width: 600px;">
            <thead>
                <tr>
                    <th style="text-align: center" colspan="4">
                        <span style="font-size: 20px"><strong>查詢條件設定</strong></span>
                    </th>
                </tr>
            </thead>
            <tbody>
                <!-------------第一行------------->
                <tr>
                    <td style="text-align: center; width: 25%">
                        <strong>開始時間</strong>
                    </td>
                    <td style="text-align: center; width: 75%">
                        <asp:TextBox ID="txt_S_DATETime" class="form-control" placeholder="開始時間"
                            runat="server" Text=""></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td style="text-align: center; width: auto">
                        <strong>結束時間</strong>
                    </td>
                    <td style="text-align: center; width: auto">
                        <asp:TextBox ID="txt_E_DATETime" class="form-control" placeholder="結束時間"
                            runat="server" Text=""></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td style="text-align: center; width: auto">
                        <strong>查詢客戶</strong>
                    </td>
                    <td style="text-align: center; width: auto">                        
                        <asp:TextBox ID="txt_businessname" class="form-control" placeholder="搜客戶名稱"
                            runat="server" Text=""></asp:TextBox>
                        <asp:DropDownList ID="drop_businessname" runat="server" class="form-control" DataTextField="Agent_Company" DataValueField="Agent_Company">
                        </asp:DropDownList>
                        <div data-toggle="tooltip" title="搜英文時 請輸入小寫">
                            <asp:TextBox ID="txtSearchName" class="form-control" placeholder="搜客戶選項"
                                runat="server" Text="" onkeyup="List_Search(this.value,'1')"></asp:TextBox>
                            <!--<asp:Button ID="Button1" runat="server" Text="查詢" Width="100px" TabIndex="-1" Enabled="true" 
                            type="button" class="btn btn-success" OnClick="Btn_Query_Click" />-->
                        </div>
                    </td>
                </tr>
                <tr>
                    <td style="text-align: center; width: auto">
                        <strong>處理狀態</strong>
                    </td>
                    <td style="text-align: center; width: auto">
                        <asp:DropDownList ID="drop_type" runat="server" class="form-control">
                            <asp:ListItem Value="">全部狀態…</asp:ListItem>
                            <asp:ListItem Value="未到點">未到點</asp:ListItem>
                            <asp:ListItem Value="處理中">處理中</asp:ListItem>
                            <asp:ListItem Value="已結案">已結案</asp:ListItem>
                            <asp:ListItem Value="已結案已簽核">已結案已簽核</asp:ListItem>
                        </asp:DropDownList>
                    </td>
                </tr>
                <tr>
                    <td style="text-align: center; width: auto">
                        <strong>查詢工程師</strong>
                    </td>
                    <td style="text-align: center; width: auto">
                        <asp:DropDownList ID="drop_enginner" runat="server" class="form-control" AutoPostBack="true">
                        </asp:DropDownList><!--  OnSelectedIndexChanged="drop_service_change"    
                        <asp:TextBox ID="txtSearchEng" class="form-control" placeholder="搜工程師" 
                            runat="server" Text="" onkeyup = "List_Search(this.value,'2')"></asp:TextBox>-->
                    </td>
                </tr>
                <tr>
                    <td style="text-align: center; width: auto">
                        <strong>意見類型</strong>
                    </td>
                    <td style="text-align: center; width: auto">
                        <asp:DropDownList ID="drop_opinion_type" runat="server" class="form-control" AutoPostBack="true">
                        </asp:DropDownList><!--  OnSelectedIndexChanged="drop_service_change"    
                        <asp:TextBox ID="txtSearchOType" class="form-control" placeholder="搜意見類型" 
                            runat="server" Text="" onkeyup = "ShowOType(this.value)"></asp:TextBox>-->
                    </td>
                </tr>
                <tr>
                    <td style="text-align: center; width: auto">
                        <strong>查詢意見內容</strong>
                    </td>
                    <td style="text-align: center; width: auto">
                        <!--<asp:DropDownList ID="drop_servicename" runat="server" class="form-control">
                            <asp:ListItem Value="">全部服務內容</asp:ListItem>
                        </asp:DropDownList>-->
                        <asp:TextBox ID="txt_opinioncontent" class="form-control" placeholder="意見內容"
                            runat="server" Text=""></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td colspan="4" style="text-align: center">
                        <asp:Button ID="Btn_Query" runat="server" Text="查詢" Width="100px" TabIndex="-1" Enabled="true" type="button" class="btn btn-success" OnClick="Btn_Query_Click" />
                        <asp:Button ID="Btn_exportExcel" runat="server" Text="匯出" Width="100px" TabIndex="-1" Enabled="true" type="button" class="btn btn-success" OnClick="Btn_exportExcel_Click" />
                        <iframe id="data" runat="server" style="display: none;"></iframe>
                    </td>
                </tr>
            </tbody>
        </table>
        <!--===================================================-->
    </div>
    <div style="width: 2400px; margin: 10px 20px">
        <h2 id="report_title" runat="server"><strong></strong></h2>
        <div id="div_report"></div>
    </div>
</asp:Content>

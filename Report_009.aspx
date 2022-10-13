<%@ Page Language="C#" EnableEventValidation="true" AutoEventWireup="true" MasterPageFile="MasterPage.master" CodeFile="Report_009.aspx.cs" Inherits="Report_009" %>

<asp:Content ID="Content" ContentPlaceHolderID="head2" runat="Server">
    <link href="../js/jquery-ui.min.css" rel="stylesheet" />
    <link href="../DataTables/jquery.dataTables.min.css" rel="stylesheet" />
    <script src="../js/jquery-ui.min.js"></script>
    <script src="../DataTables/jquery.dataTables.min.js"></script>
    <script src="../DataTables/jquery.datepicker-zh-TW.js"></script>
    <script type="text/javascript">
        $(function () {
            $("[id$='txt_S_DATETime'],[id$='txt_E_DATETime']").datepicker({ dateFormat: "yy-mm-dd" });
            $("[id$='data']").on('load', function () {
                debugger;
                $("[id$='div_report']").append($(this).contents().find("style"))
                                       .append($(this).contents().find("table"));
                $("[id$='div_report'] table").addClass("table table-bordered table-striped");
            });
        });
        var ddlText, ddlValue, ddl;
        function CacheItems() {
            ddlText = new Array();
            ddlValue = new Array();
            ddl = document.getElementById("<%=drop_service.ClientID %>");
            for (var i = 0; i < ddl.options.length; i++) {
                ddlText[ddlText.length] = ddl.options[i].text;
                ddlValue[ddlValue.length] = ddl.options[i].value;
            }
        }
        window.onload = CacheItems;
        function List_Search(value) {
            ddl.options.length = 0;
            for (var i = 0; i < ddlText.length; i++) {
                if (ddlText[i].toLowerCase().indexOf(value) != -1) {
                    AddItem(ddlText[i], ddlValue[i]);
                }
            }
            if (ddl.options.length == 0) {
                AddItem("查無資料", "查無資料");
            }
        }
        function AddItem(text, value) {
            var opt = document.createElement("option");
            opt.text = text;
            opt.value = value;
            ddl.options.add(opt);
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
        <h2><strong>【員工打卡明細表】</strong></h2>
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
                    <td style="text-align: center; width: 50%">
                        <strong>開始時間</strong>
                    </td>
                    <td style="text-align: center; width: 50%">
                        <asp:TextBox ID="txt_S_DATETime" class="form-control" placeholder="開始時間"
                            runat="server" Text=""></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td style="text-align: center; width: 50%">
                        <strong>結束時間</strong>
                    </td>
                    <td style="text-align: center; width: 50%">
                        <asp:TextBox ID="txt_E_DATETime" class="form-control" placeholder="結束時間"
                            runat="server" Text=""></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td style="text-align: center; width: 50%">
                        <strong>查詢部門</strong>
                    </td>
                    <td style="text-align: center; width: 50%">
                        <asp:DropDownList ID="drop_team" runat="server" class="form-control" DataTextField="Agent_Company" DataValueField="Agent_Company">
                        </asp:DropDownList>
                    </td>
                </tr>
                <tr>
                    <td style="text-align: center; width: 50%">
                        <strong>查詢狀態</strong>
                    </td>
                    <td style="text-align: center; width: 50%">
                        <asp:DropDownList ID="drop_type" runat="server" class="form-control">
                            <asp:ListItem Value="">全部狀態</asp:ListItem>
                            <asp:ListItem Value="未打卡">未打卡</asp:ListItem>
                            <asp:ListItem Value="已上班">已上班</asp:ListItem>
                            <asp:ListItem Value="已下班">已下班</asp:ListItem>
                        </asp:DropDownList>
                    </td>
                </tr>
                <tr>
                    <td style="text-align: center; width: 50%">
                        <strong>查詢員工</strong>
                    </td>
                    <td style="text-align: center; width: 50%">
                        <asp:DropDownList ID="drop_service" runat="server" class="form-control" AutoPostBack="true">
                        </asp:DropDownList><!--  OnSelectedIndexChanged="drop_service_change"    -->
                        <asp:TextBox ID="txtSearchName" class="form-control" placeholder="搜員工" 
                            runat="server" Text="" onkeyup = "List_Search(this.value)"></asp:TextBox>
                    </td>
                </tr>
                     <tr>
                    <td style="text-align: center; width: 50%">
                        <strong>搜尋行程內容</strong>
                    </td>
                    <td style="text-align: center; width: 50%">
                        <!--<asp:DropDownList ID="drop_servicename" runat="server" class="form-control">
                            <asp:ListItem Value="">全部服務內容</asp:ListItem>
                        </asp:DropDownList>-->
                        <asp:TextBox ID="txt_servicename" class="form-control" placeholder="行程內容"
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
    <div style="width: 1280px; margin: 10px 20px">
        <h2 id="report_title" runat="server"><strong></strong></h2>
        <div id="div_report"></div>
    </div>
</asp:Content>

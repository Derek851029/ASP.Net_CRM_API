<%@ Page Language="C#" EnableEventValidation="true" AutoEventWireup="true" MasterPageFile="MasterPage.master" CodeFile="Report_2.aspx.cs" Inherits="_Default" %>

<asp:Content ID="Content" ContentPlaceHolderID="head" runat="Server">
    <link href="js/jquery-ui.min.css" rel="stylesheet" />
    <script src="DataTables/jquery-1.12.3.js"></script>
    <script src="js/jquery-ui.min.js"></script>
    <script type="text/javascript">
        $(function () {
            $("[id$='txt_S_DATETime'],[id$='txt_E_DATETime']").datepicker({ dateFormat: "yy-mm-dd" });
        });
    </script>
    <tr>
        <td>
            <!--===================================================-->
            <h2><strong>&nbsp; &nbsp;查詢</strong></h2>
            <table class="table table-bordered table-striped" style="width: 1024px">
                <%--<thead>
                    <tr>
                        <th style="text-align: center; width: 50px;"></th>
                        <th style="width: 150px"></th>
                    </tr>
                </thead>--%>
                <tbody>
                    <!-------------第一行------------->
                    <tr>
                        <td style="text-align: right">
                            <h5><strong>預定起始時間起訖</strong></h5>
                        </td>
                        <td>
                            <asp:TextBox ID="txt_S_DATETime" class="form-control" placeholder="開始時間"
                                runat="server" Text="2017-01-01"></asp:TextBox>
                            <asp:TextBox ID="txt_E_DATETime" class="form-control" placeholder="結束時間"
                                runat="server" Text="2017-01-31"></asp:TextBox>
                            <asp:TextBox ID="txt_Team" class="form-control" placeholder="部門"
                                runat="server" Text=""></asp:TextBox>
                        </td>

                    </tr>
                    <!-------------第一行------------->
                    <tr>
                        <td colspan="4" style="text-align: center">
                            <asp:Button ID="Btn_Query" runat="server" Text="查詢" Width="100px" TabIndex="-1" Enabled="true" type="button" class="btn btn-success" OnClick="Btn_Query_Click" />
                        </td>
                    </tr>
                </tbody>
            </table>
            <!--===================================================-->
        </td>

    </tr>
</asp:Content>

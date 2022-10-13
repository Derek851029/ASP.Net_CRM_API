<%@ Control Language="VB" AutoEventWireup="false" CodeFile="Menu.ascx.vb" Inherits="UserControl_Menu" %>
<div>
    <table id="bar" style="width: 100%;">
        <tr>
            <td style="height: 70px; width: 1024px;">
                <a href="../Default.aspx">
                    <img align="left" src="../images/LOGO.png" style="height: 90px; width: 315px;" /></a>
            </td>
        </tr>
    </table>
    <nav id="navbar-example" class="navbar navbar-expand-lg navbar-light" style="background-color: #e3f2fd;" role="navigation">
        <div class="container-fluid">
            <div class="collapse navbar-collapse">
                <div id="the_Menu" runat="server"></div>
                <ul class="navbar-nav" style="padding-left:40%">
                    <li id="fat-menu" class="nav-item dropdown">
                        <a id="agent_info" class="nav-link dropdown-toggle" data-toggle="dropdown" aria-haspopup="true" role="button" aria-expanded="false" runat="server"></a>
                        <ul class="dropdown-menu" role="menu" aria-labelledby="agent_info">
                            <li id="user_li" runat="server"></li>
                            <li role="presentation" class="divider"></li>
                            <li>&nbsp;&nbsp;
                                <button id="Btn_Default" type="button" class="btn btn-success" onserverclick="Logout" runat="server">
                                    <span class="glyphicon glyphicon-home"></span>&nbsp;登出</button>
                            </li>
                        </ul>
                    </li>
                </ul>
            </div>
            <!-- /.nav-collapse -->
        </div>
        <!-- /.container-fluid -->
    </nav>
    <style type="text/css">
        body {
            font-family: "Microsoft JhengHei",Helvetica,Arial,Verdana,sans-serif;
            font-size: 16px;
        }

        .dropdown:hover .dropdown-menu {
            display: block;
        }
    </style>
</div>

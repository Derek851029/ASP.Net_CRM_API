<%@ Page Language="VB" AutoEventWireup="false" CodeFile="Login.aspx.vb" Inherits="Login" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title>Login</title>
    <script src="bootstrap-5.0.2-dist/js/bootstrap.min.js"></script>
    <link href="bootstrap-5.0.2-dist/css/bootstrap.min.css" rel="stylesheet" />
    <style>
        .gradient-custom-2 {
          /* fallback for old browsers */
          background: #fccb90;

          /* Chrome 10-25, Safari 5.1-6 */
          background: -webkit-linear-gradient(to right, #ee7724, #d8363a, #dd3675, #b44593);

          /* W3C, IE 10+/ Edge, Firefox 16+, Chrome 26+, Opera 12+, Safari 7+ */
          background: linear-gradient(to right, #ee7724, #d8363a, #dd3675, #b44593);
        }

        @media (min-width: 768px) {
              .gradient-form {
                height: 100vh !important;
              }
        }
        @media (min-width: 769px) {
            .gradient-custom-2 {
                border-top-right-radius: .3rem;
                border-bottom-right-radius: .3rem;
            }
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
     <section class="h-100 gradient-form" style="background-color: #eee;">
      <div class="container py-5 h-100">
        <div class="row d-flex justify-content-center align-items-center h-100">
          <div class="col-xl-10">
            <div class="card rounded-3 text-black">
                  <div class="card-body p-md-5 mx-md-4">
                    <div class="text-center">
                      <img src="../images/APEX_Health.png" class="auto-style1" style="height: 90px; width: 315px;">
                    </div>

                    <div>
                      <p>請輸入您的帳號</p>

                      <div class="form-outline mb-4">
                        <asp:TextBox ID="txt_id" runat="server" MaxLength="50" class="form-control" placeholder="帳號" type="text" autocomplete="off" ></asp:TextBox>
                      </div>

                      <div class="form-outline mb-4">
                        <asp:TextBox ID="txt_pwd" runat="server" MaxLength="50" class="form-control" TextMode="Password" placeholder="密碼" Style="font-size: 18px;"></asp:TextBox>
                      </div>

                      <div class="text-center pt-1 mb-5 pb-1">

                        <asp:Button ID="UserLogin" runat="server" Text="登入" class="btn btn-primary btn-block fa-lg gradient-custom-2 mb-3" />
                        <asp:Button ID="Cancel" runat="server" Text="取消" class="btn btn-primary btn-block fa-lg gradient-custom-2 mb-3" OnClientClick="window.close();" />
                      </div>
                    </div>
                  </div>
            </div>
          </div>
        </div>
      </div>
    </section>
        <%--<table style="width: 100%;">
            <tr style="width: auto; height: 80px;">
                <td style="height: 70px; width: 1024px;">
                    <!--<a href="../Default.aspx">
                        <img align="left" src="/images/LOGO.png" style="height: 90px; width: 315px;"/>
                    </a>-->
                    <img align="left" src="../images/APEX_Health.png" class="auto-style1" style="height: 90px; width: 315px;" ;/>
                </td>
                <td style="width: auto; height: 80px;"></td>
            </tr>
        </table>
        <br />
        <br />
        <br />
        <br />
        <table class="TableStyle" cellspacing="1" align="center" style="width: 30%">
            <tr style="height: 40px">
                <td style="width: 30%" align="right">
                    <h3><strong>帳號：</strong></h3>
                </td>
                <td style="width: 200px" align="center">
                    <asp:TextBox ID="txt_id" runat="server" MaxLength="50" class="form-control" placeholder="帳號" type="text" autocomplete="off" Style="font-size: 18px;"></asp:TextBox>
                </td>
                <td style="width: 30%"></td>
            </tr>
            <tr style="height: 40px">
                <td style="width: 30%" align="right">
                    <h3><strong>密碼：</strong></h3>
                </td>
                <td style="width: 200px" align="center">
                    <asp:TextBox ID="txt_pwd" runat="server" MaxLength="50" class="form-control" TextMode="Password" placeholder="密碼" Style="font-size: 18px;"></asp:TextBox>
                </td>
                <td style="width: 30%"></td>
            </tr>
            <tr style="height: 40px">
                <td style="width: 30%"></td>
                <td style="width: 250px" align="center">
                    <asp:Button ID="UserLogin" runat="server" Text="登入" Style="font-size: 18px; font-family: Microsoft JhengHei;" class="btn btn-primary" />&nbsp;&nbsp;&nbsp;
                    <asp:Button ID="Cancel" runat="server" Text="取消" Style="font-size: 18px; font-family: Microsoft JhengHei;" class="btn btn-default" OnClientClick="window.close();" />
                </td>
                <td style="width: 30%"></td>
            </tr>
        </table>--%>
    </form>
</body>
</html>

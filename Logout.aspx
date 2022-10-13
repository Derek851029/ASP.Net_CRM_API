<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Logout.aspx.cs" Inherits="Logout" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title></title>
    <link href="css/bootstrap.min.css" rel="stylesheet" />
    <script src="js/jquery.min.js"></script>
    <script src="js/bootstrap.min.js"></script>
    <script type="text/javascript">
        $(document).ready(function () {
            setTimeout(Login, 2500);
            function Login() {
                window.location.href = "/Login.aspx";
            }
        });
    </script>
</head>
<body>
    <style>
        body {
            font-family: "Microsoft JhengHei",Helvetica,Arial,Verdana,sans-serif;
            font-size: 18px;
        }
    </style>
    <form id="form1" runat="server">
        <div class="container" style="width: 100%">
            <br />
            <br />
            <br />
            <div class="alert alert-info" style="width: 100%">
                <div style="width: 100%; height: 500px;">
                    <h1 class="alert-heading" style="float: left;"><strong>提醒：</strong>帳號已登出，等待三秒回到登入頁面。</h1>
                    <img style="width: 20%; height: auto; float: right;" src="images/Logo_CC.jpg" />
                </div>
            </div>
        </div>
    </form>
</body>
</html>

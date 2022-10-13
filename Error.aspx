<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Error.aspx.cs" Inherits="Error" %>

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
            <div class="alert alert-danger" style="width: 100%">
                <div><label style="font-size:72px;">ERROR 404</label> </div>
                <div style="width: 100%; height: 500px;">
                    <h1 class="alert-heading" style="float: left;"><strong>提醒：</strong>網頁發生錯誤，請詢問管理人員<br/>　　　３秒後將回到登入頁面<br/>　　　造成您的不便請見諒，謝謝。</h1>
                    <img style="width: 20%; height: auto; float: right;" src="images/LOGO_001.png" />
                </div>
            </div>
        </div>
    </form>
</body>
</html>

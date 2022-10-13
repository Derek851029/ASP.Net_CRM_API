<%@ Page Language="VB" AutoEventWireup="false" CodeFile="ExHandle.aspx.vb" Inherits="ExHandle" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title>系統訊息</title>
    <link href="css/bootstrap.min.css" rel="stylesheet" />
    <script src="js/jquery.min.js"></script>
    <script src="js/bootstrap.min.js"></script>
</head>
<body>
    <form id="form1" runat="server">

        <div class="container" style="width: 1280px;">
            <h2></h2>
            <div class="alert alert-success">
                <strong>Success!</strong> This alert box could indicate a successful or positive action.
            </div>
            <div class="alert alert-info">
                <strong>Info!</strong> This alert box could indicate a neutral informative change or action.
            </div>
            <div class="alert alert-warning">
                <strong>Warning!</strong> This alert box could indicate a warning that might need attention.
            </div>
            <div class="alert alert-danger">
                <strong>Danger!</strong> This alert box could indicate a dangerous or potentially negative action.
            </div>
            <div class="alert alert-danger">
                <h1 class="alert-heading">抱歉！無法連結到您要求的網頁。</h1>
                <h4>可能是：
					<ul>
                        <li>網頁不存在。</li>
                        <li>系統發生異常，請稍後再試。</li>
                        <li>系統忙碌中，請稍後再試。</li>
                    </ul>
                </h4>
                <p class="Right">
                    <a class="btn btn-danger" href="../Login.aspx" style="font-size: 16px">確定並回到首頁</a>
                </p>
            </div>
        </div>
    </form>
</body>
</html>

<%@ Page Language="C#" AutoEventWireup="true" CodeFile="2016111603.aspx.cs" Inherits="Update" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title></title>
    <script type="text/javascript">
        var min = "2";
        var max = "5";
        function A() {
            var image = document.getElementById("update_image");
            document.getElementById('txt_image').innerHTML = min + " / " + max;
            image.src = "22" + min + ".png";
            if (min == '5') {
                min = '1';
            } else {
                min++;
            }
        }
    </script>
    <style type="text/css">
        body {
            font-family: "Microsoft JhengHei",Helvetica,Arial,Verdana,sans-serif;
            font-size: 16px;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <div style="height: 55px; width: 1000px;">
            <a href="../Update.aspx">
                <img style="float: left;" src="/images/LOGO_001.png" />
            </a>
        </div>
        <table class="" style="width: 1024px; margin: 10px 20px">
            <thead>
                <tr>
                    <th style="text-align: center" colspan="1">
                        <span style="font-size: 24px"><strong>2016/11/16 更新訊息</strong></span>
                    </th>
                </tr>
            </thead>
            <tbody>
                <tr>
                    <th>
                        <div style="float: left; font-size: 20px"> 3. 【系統參數維護】新增【服務審核管理】</div>
                    </th>
                </tr>
                <tr>
                    <th>
                        <label id="txt_image" style="float: right; font-size: 20px">1 / 5</label>
                    </th>
                </tr>
                <tr>
                    <th>
                        <img src="221.png" id="update_image" onclick="A()" style="float: left; width: 1024px;" />
                    </th>
                </tr>
            </tbody>
        </table>
    </form>
</body>
</html>

<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Report_001.aspx.cs" Inherits="Report_Report_001" %>

<%@ Register Assembly="Microsoft.ReportViewer.WebForms, Version=10.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a"
    Namespace="Microsoft.Reporting.WebForms" TagPrefix="rsweb" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <link href="../css/bootstrap.min.css" rel="stylesheet" />
    <link href="../css/bootstrap-theme.min.css" rel="stylesheet" />
    <link href="../css/bootstrap-responsive.min.css" rel="stylesheet" />
    <link href="../bootstrap-chosen-master/bootstrap-chosen.css" rel="stylesheet" />
    <script src="../js/jquery.min.js"></script>
    <script src="../js/bootstrap.min.js"></script>
    <script src="../chosen/chosen.jquery.js"></script>
    <script type="text/javascript">
        var str_CNo = '<%= seqno %>';
        $(function () {
            //Company_List();
        });

        function Company_List() {
            $.ajax({
                url: 'Report_001.aspx/Company_List',
                type: 'POST',
                contentType: 'application/json; charset=UTF-8',
                dataType: "json",
                success: function (doc) {
                    var json = JSON.parse(doc.d);
                    var $select_elem = $("#select_Company");
                    $select_elem.chosen("destroy")
                    $.each(json, function (idx, obj) {
                        $select_elem.append("<option value='" + obj.SYSID + "'>" + obj.Company_CName + "</option>");
                    });
                    $select_elem.chosen({
                        width: "25%",
                        search_contains: true
                    });
                }
            });
        }

        function RE() {
            var s = document.getElementById("select_Company");
            var str_value = s.options[s.selectedIndex].value;
            window.location.href = '/Report/Report_001.aspx?seqno=' + str_CNo + '&company=' + str_value;
        }

        function Service_Type() {
            window.location.href = '/Report/Report_001_Picture.aspx?seqno=' + str_CNo;
        }

        function SignResult() {
            window.location.reload();
        }

        function Eng() {
            document.getElementById('sig-submitBtn').style.display = "";
            document.getElementById('Button4').style.display = "none";
        }
        function Cus() {
            document.getElementById('sig-submitBtn').style.display = "none";
            document.getElementById('Button4').style.display = "";
        }

        function A_Sign() {
            $.ajax({
                url: 'Report_001.aspx/UploadPic0',
                ache: false,
                type: 'POST',
                data: JSON.stringify({}),
                contentType: 'application/json; charset=UTF-8',
                dataType: "json",       //如果要回傳值，請設成 json
                success: function (doc) {
                    var json = JSON.parse(doc.d.toString());
                    if (json.flag == "0") {
                        //alert("簽名完成。");
                        window.location.href = json.txt;
                    } else { alert(json.txt); }
                }
            });
        };
    </script>
    <style type="text/css">
        body {
            font-family: "Microsoft JhengHei",Helvetica,Arial,Verdana,sans-serif;
            font-size: 22px;
            padding-top: 20px;
            padding-bottom: 20px;
        }

        #sig-canvas {
            border: 2px dotted #CCCCCC;
            border-radius: 5px;
            cursor: crosshair;
        }
    </style>
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
        <!-- ====== Modal ====== -->
        <div class="modal fade" id="Div_Signature" role="dialog" data-backdrop="static" data-keyboard="false">
            <div class="modal-dialog" style="width: 970px; height: 650px">
                <!-- Modal content-->
                <div class="modal-content">
                    <div class="modal-header">
                        <button type="button" class="close" data-dismiss="modal">&times; 關閉</button>
                        <h2 class="modal-title"><strong>簽名板</strong></h2>
                    </div>
                    <div class="modal-body">
                        <!-- ========================================== -->
                        <div class="row" style="margin: 5px 5px">
                            <div class="col-md-12">
                                <canvas id="sig-canvas" width="900" height="240">Get a better browser, bro.
                                </canvas>
                            </div>
                        </div>
                        <div class="row" style="text-align: center">
                            <div class="col-md-12">
                                <button type="button" class="btn btn-primary btn-lg" id="sig-submitBtn">簽名完成</button>&nbsp;&nbsp;
                                <button type="button" class="btn btn-primary btn-lg" id="Button4">客戶簽名完成</button>&nbsp;&nbsp;
                                <button type="button" class="btn btn-default btn-lg" id="sig-clearBtn">重新簽名</button>&nbsp;&nbsp;
                                <button type="button" class="btn btn-default btn-lg" id="Button2" data-dismiss="modal">關閉簽名板</button>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>

        <div class="modal fade" id="Div1" role="dialog" data-backdrop="static" data-keyboard="false">
            <div class="modal-dialog" style="width: 400px; height: 300px">
                <div class="modal-content">
                    <div class="modal-header" style="text-align: center">
                        <th style="text-align: center; width: 20%; height: 55px;">
                            <strong>確認要簽名嗎</strong>
                        </th>
                    </div>
                    <div class="modal-body" style="text-align: center">
                        <th style="text-align: center; width: 20%; height: 55px;">
                            <button type="button" class="btn btn-primary btn-lg" data-dismiss="modal" onclick="A_Sign();"><span class="glyphicon glyphicon-pencil"></span>&nbsp;&nbsp;確認</button>
                            <button type="button" class="btn btn-danger btn-lg" data-dismiss="modal"><span class="glyphicon glyphicon-remove"></span>&nbsp;取消</button>
                        </th>
                    </div>
                </div>
            </div>
        </div>
        <!-- ====== Modal ====== -->
        <br />
        <div>
            &nbsp;&nbsp;&nbsp;
            電信商：                       
            <select id="select_Company" name="select_Company" class="chosen-select" style="width: 25%">
                <option value="">請選擇電信廠商…</option>
                <option value="1">中華電信</option>
                <option value="2">遠傳電信</option>
            </select>
            &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<!---->
              <button id="Btn_Refresh" type="button" onclick="RE();" class="btn btn-success btn-lg ">
                  <span class="glyphicon glyphicon-refresh"></span>
                  &nbsp;&nbsp;重新產生表單
              </button>
            &nbsp;&nbsp;&nbsp;
            <!--<button id="Button1" type="button" class="btn btn-primary btn-lg " data-toggle="modal" data-target="#Div_Signature" onclick="Eng();">-->
            <button id="Button1" type="button" class="btn btn-primary btn-lg " data-toggle="modal" data-target="#Div1" onclick="">
                <span class="glyphicon glyphicon-pencil"></span>&nbsp;&nbsp;工程師簽名</button>
            
            <button id="Button3" type="button" class="btn btn-primary btn-lg " data-toggle="modal" data-target="#Div_Signature" onclick="Cus();"><span class="glyphicon glyphicon-pencil"></span>&nbsp;&nbsp;客戶簽名</button>
            <%--<button id="Button1" type="button" onclick="Service_Type();" class="btn btn-primary btn-lg "><span class="glyphicon glyphicon-search"></span>&nbsp;&nbsp;電子簽核表單</button>--%>
        </div>
        <br />
        <div>
            <asp:ScriptManager ID="ScriptManager1" runat="server" />
            <rsweb:ReportViewer ID="rptviewer" runat="server" Width="100%" Height="2000px" />
        </div>
        <script>
            $(function () {

                // Get a regular interval for drawing to the screen
                window.requestAnimFrame = (function (callback) {
                    return window.requestAnimationFrame ||
                                window.webkitRequestAnimationFrame ||
                                window.mozRequestAnimationFrame ||
                                window.oRequestAnimationFrame ||
                                window.msRequestAnimaitonFrame ||
                                function (callback) {
                                    window.setTimeout(callback, 1000 / 60);
                                };
                })();

                // Set up the canvas
                var canvas = document.getElementById("sig-canvas");
                var ctx = canvas.getContext("2d");
                ctx.fillStyle = "white";
                ctx.fillRect(0, 0, canvas.width, canvas.height);
                ctx.strokeStyle = "#222222";
                ctx.lineWidth = 2;

                // Set up the UI
                var clearBtn = document.getElementById("sig-clearBtn");
                var submitBtn = document.getElementById("sig-submitBtn");
                clearBtn.addEventListener("click", function (e) {
                    clearCanvas();
                }, false);

                //========================================

                submitBtn.addEventListener("click", function (e) {
                    var Pic = canvas.toDataURL("image/png");
                    Pic = Pic.replace(/^data:image\/(png|jpg);base64,/, "")
                    $.ajax({
                        type: 'POST',
                        url: 'Report_001.aspx/UploadPic',
                        data: '{ "imageData" : "' + Pic + '" }',
                        contentType: 'application/json; charset=utf-8',
                        dataType: 'json',
                        success: function (doc) {
                            var json = JSON.parse(doc.d.toString());
                            if (json.flag == "0") {
                                alert("簽名完成。");
                                window.location.href = json.txt;
                            } else { alert(json.txt); }
                        }
                    });
                }, false
                );//========================================
                var submitBtn_2 = document.getElementById("Button4");
                submitBtn_2.addEventListener("click", function (e) {
                    var Pic = canvas.toDataURL("image/png");
                    Pic = Pic.replace(/^data:image\/(png|jpg);base64,/, "")
                    $.ajax({
                        type: 'POST',
                        url: 'Report_001.aspx/UploadPic_C',
                        data: '{ "imageData" : "' + Pic + '" }',
                        contentType: 'application/json; charset=utf-8',
                        dataType: 'json',
                        success: function (doc) {
                            var json = JSON.parse(doc.d.toString());
                            if (json.flag == "0") {
                                alert("簽名完成。");
                                window.location.href = json.txt;
                            } else { alert(json.txt); }
                        }
                    });
                }, false
                );

                //====================================================================

                var drawing = false;
                var mousePos = { x: 0, y: 0 };
                var lastPos = mousePos;
                canvas.addEventListener("mousedown", function (e) { drawing = true; lastPos = getMousePos(canvas, e); }, false);
                canvas.addEventListener("mouseup", function (e) { drawing = false; }, false); canvas.addEventListener("mousemove", function (e) { mousePos = getMousePos(canvas, e); }, false);
                canvas.addEventListener("touchstart", function (e) {
                    mousePos = getTouchPos(canvas, e); var touch = e.touches[0]; var mouseEvent = new MouseEvent("mousedown", { clientX: touch.clientX, clientY: touch.clientY });
                    canvas.dispatchEvent(mouseEvent);
                }, false);
                canvas.addEventListener("touchend", function (e) { var mouseEvent = new MouseEvent("mouseup", {}); canvas.dispatchEvent(mouseEvent); }, false);
                canvas.addEventListener("touchmove", function (e) {
                    var touch = e.touches[0]; var mouseEvent = new MouseEvent("mousemove", { clientX: touch.clientX, clientY: touch.clientY });
                    canvas.dispatchEvent(mouseEvent);
                }, false);

                // Prevent scrolling when touching the canvas
                document.body.addEventListener("touchstart", function (e) {
                    if (e.target == canvas) {
                        e.preventDefault();
                    }
                }, false);
                document.body.addEventListener("touchend", function (e) {
                    if (e.target == canvas) {
                        e.preventDefault();
                    }
                }, false);
                document.body.addEventListener("touchmove", function (e) {
                    if (e.target == canvas) {
                        e.preventDefault();
                    }
                }, false);

                // Get the position of the mouse relative to the canvas
                function getMousePos(canvasDom, mouseEvent) {
                    var rect = canvasDom.getBoundingClientRect();
                    return {
                        x: mouseEvent.clientX - rect.left,
                        y: mouseEvent.clientY - rect.top
                    };
                }

                // Get the position of a touch relative to the canvas
                function getTouchPos(canvasDom, touchEvent) {
                    var rect = canvasDom.getBoundingClientRect();
                    return {
                        x: touchEvent.touches[0].clientX - rect.left,
                        y: touchEvent.touches[0].clientY - rect.top
                    };
                }

                // Draw to the canvas
                function renderCanvas() {
                    if (drawing) {
                        ctx.moveTo(lastPos.x, lastPos.y);
                        ctx.lineTo(mousePos.x, mousePos.y);
                        ctx.stroke();
                        lastPos = mousePos;
                    }
                }

                function clearCanvas() {
                    canvas.width = canvas.width;
                    ctx.fillStyle = "white";
                    ctx.fillRect(0, 0, canvas.width, canvas.height);
                    ctx.strokeStyle = "#222222";
                    ctx.lineWidth = 2;
                }

                // Allow for animation
                (function drawLoop() {
                    requestAnimFrame(drawLoop);
                    renderCanvas();
                })();

            });

        </script>
    </form>
</body>
</html>

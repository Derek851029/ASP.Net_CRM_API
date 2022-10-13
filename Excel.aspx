<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Excel.aspx.cs" Inherits="Excel" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
        <div style="margin: 10px 20px; width: 1280px">
            <div>
                <input type="file" name="xlfile" id="xlf" />
            </div>
            <br />
            <div>
                <table id="data" class="display table table-striped">
                    <thead>
                        <tr>
                            <th style="text-align: center;">編號</th>
                            <th style="text-align: center;">姓名</th>
                            <th style="text-align: center;">電話</th>
                            <th style="text-align: center;">手機</th>
                            <th style="text-align: center;">公司</th>
                        </tr>
                    </thead>
                </table>
            </div>
        </div>
    </form>
    <script src="js/jquery.min.js"></script>
    <script src="DataTables/jquery.dataTables.min.js"></script>
    <link href="css/bootstrap.min.css" rel="stylesheet" />
    <link href="css/bootstrap-theme.min.css" rel="stylesheet" />
    <link href="css/bootstrap-responsive.min.css" rel="stylesheet" />
    <link href="DataTables/jquery.dataTables.min.css" rel="stylesheet" />
    <link href="css/iosOverlay.css" rel="stylesheet" />
    <link href="css/prettify.css" rel="stylesheet" />
    <link href="css/custom.css" rel="stylesheet" />
    <script src="js/modernizr-2.0.6.min.js"></script>
    <script src="Excel/shim.js"></script>
    <script src="Excel/jszip.js"></script>
    <script src="Excel/xlsx.js"></script>
    <script src="Excel/ods.js"></script>
    <script src="js/iosOverlay.js"></script>
    <script src="js/prettify.js"></script>
    <script src="js/spin.min.js"></script>
    <script>
        var X = XLSX;
        var xlf = document.getElementById('xlf');

        if (xlf.addEventListener) {
            xlf.addEventListener('change', handleFile, false)
        };

        function handleFile(e) {
            var files = e.target.files;
            var f = files[0];
            {
                var reader = new FileReader();
                var name = f.name;
                reader.onload = function (e) {
                    if (typeof console !== 'undefined') {
                        console.log("onload", new Date())
                    };
                    var data = e.target.result;
                    var wb;
                    var arr = fixdata(data);
                    wb = X.read(btoa(arr), { type: 'base64' });
                    var val = to_json(wb);
                    console.log("val", val);
                    SQL(val);
                    bindTable(val);
                };
                reader.readAsArrayBuffer(f);
            }
        }

        function fixdata(data) {
            var o = "", l = 0, w = 10240;
            for (; l < data.byteLength / w; ++l) o += String.fromCharCode.apply(null, new Uint8Array(data.slice(l * w, l * w + w)));
            o += String.fromCharCode.apply(null, new Uint8Array(data.slice(l * w)));
            return o;
        }

        function icon() {
            var opts = {
                lines: 13, // The number of lines to draw
                length: 11, // The length of each line
                width: 5, // The line thickness
                radius: 17, // The radius of the inner circle
                corners: 1, // Corner roundness (0..1)
                rotate: 0, // The rotation offset
                color: '#FFF', // #rgb or #rrggbb
                speed: 1, // Rounds per second
                trail: 60, // Afterglow percentage
                shadow: false, // Whether to render a shadow
                hwaccel: false, // Whether to use hardware acceleration
                className: 'spinner', // The CSS class to assign to the spinner
                zIndex: 2e9, // The z-index (defaults to 2000000000)
                top: 'auto', // Top position relative to parent in px
                left: 'auto' // Left position relative to parent in px
            };
            var target = document.createElement("div");
            document.body.appendChild(target);
            var spinner = new Spinner(opts).spin(target);

            var overlay = iosOverlay({
                text: "Loading",
                spinner: spinner
            });
            //====================
            console.log("Loading");
            overlay.update({
                icon: "img/check.png",
                text: "Success"
            });
            //====================

            overlay.hide();
        }

        function to_json(workbook) {
            var result = {};
            workbook.SheetNames.forEach(function (sheetName) {
                result = X.utils.sheet_to_row_object_array(workbook.Sheets[sheetName]);
                console.log("sheetName", sheetName);
                console.log("result", result);
            });
            return result;
        }

        function SQL(val) {
            var opts = {
                lines: 13, // The number of lines to draw
                length: 11, // The length of each line
                width: 5, // The line thickness
                radius: 17, // The radius of the inner circle
                corners: 1, // Corner roundness (0..1)
                rotate: 0, // The rotation offset
                color: '#FFF', // #rgb or #rrggbb
                speed: 1, // Rounds per second
                trail: 60, // Afterglow percentage
                shadow: false, // Whether to render a shadow
                hwaccel: false, // Whether to use hardware acceleration
                className: 'spinner', // The CSS class to assign to the spinner
                zIndex: 2e9, // The z-index (defaults to 2000000000)
                top: 'auto', // Top position relative to parent in px
                left: 'auto' // Left position relative to parent in px
            };
            var target = document.createElement("div");
            document.body.appendChild(target);
            var spinner = new Spinner(opts).spin(target);

            var overlay = iosOverlay({
                text: "Loading",
                spinner: spinner
            });

            window.setTimeout(function () {
                overlay.update({
                    icon: "img/check.png",
                    text: "Success"
                });

                window.setTimeout(function () {
                    overlay.hide();
                }, 500);

            }, val.length * 100);
            //====================
            
            for (var i = 0; i < val.length; i++) {
                (function (i) {
                    window.setTimeout(function () {
                        //===========================
                        console.log("第" + i + "次");
                        $.ajax({
                            url: 'Excel.aspx/SQL',
                            type: 'POST',
                            data: JSON.stringify({ No: val[i].編號, Name: val[i].姓名, TEL: val[i].電話, Phone: val[i].手機, Company: val[i].公司 }),
                            contentType: 'application/json; charset=UTF-8',
                            dataType: "json",       //如果要回傳值，請設成 json
                            success: function () {
                            }
                        });
                        //===========================
                    }, i * 100);
                }(i));
            };
        }

        function bindTable(val) {
            $("#data").dataTable({
                "oLanguage": {
                    "sLengthMenu": "顯示 _MENU_ 筆記錄",
                    "sZeroRecords": "無符合資料",
                    "sInfo": "顯示第 _START_ 至 _END_ 項結果，共 _TOTAL_ 項",
                    "sInfoFiltered": "(從 _MAX_ 項結果過濾)",
                    "sInfoPostFix": "",
                    "sSearch": "搜索:",
                    "sUrl": "",
                    "oPaginate": {
                        "sFirst": "首頁",
                        "sPrevious": "上頁",
                        "sNext": "下頁",
                        "sLast": "尾頁"
                    }
                },
                "aLengthMenu": [[10, 25, 50, 100], [10, 25, 50, 100]],
                "iDisplayLength": 100,
                "aaData": val,
                "aoColumns": [
                    { "mData": "編號" },
                    { "mData": "姓名" },
                    { "mData": "電話" },
                    { "mData": "手機" },
                    { "mData": "公司" }
                ]
            })
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

        #data td:nth-child(5), #data td:nth-child(4), #data td:nth-child(3),
        #data td:nth-child(2), #data td:nth-child(1), #data th:nth-child(5) {
            text-align: center;
        }
    </style>
</body>
</html>

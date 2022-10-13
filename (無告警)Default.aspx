<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="複製 - Default.aspx.cs" Inherits="_Default" %>

<asp:Content ID="Content" ContentPlaceHolderID="head2" runat="Server">
    <link href="../DataTables/jquery.dataTables.min.css" rel="stylesheet" />
    <script src="../js/jquery.validate.min.js"></script>
    <script src="../DataTables/jquery.dataTables.min.js"></script>
    <script type="text/javascript">
        $(function () {
            List_Record();
            //alert("Agent_LV = " + "<%= Session["Agent_LV"]%>");
            //alert("UserIDNAME = " + "<%= Session["UserIDNAME"]%>");
            //alert("UserID = " + "<%= Session["UserID"]%>");
            Check_Prot_Time();

            if ("<%= Session["UserID"]%>" == '70472615')
            {
                document.getElementById('Button2').style.display = "";
                document.getElementById('Button1').style.display = "none";
            }
            else document.getElementById('Button2').style.display = "none";


            if ("<%= Session["Agent_LV"]%>" != '10') {
                //bindTable();
            } else {
                bindTable3();
            };
            switch ('<%= str_time %>') {
                case "2":
                    $("#btn_a").attr('class', 'btn btn-warning btn-lg');
                    $("#btn_b").attr('class', 'btn btn-warning btn-lg');
                    $("#btn_c").attr('class', 'btn btn-default btn-lg disabled');
                    break;

                case "1":
                    $("#btn_a").attr('class', 'btn btn-warning btn-lg');
                    $("#btn_b").attr('class', 'btn btn-default btn-lg disabled ');
                    $("#btn_c").attr('class', 'btn btn-warning btn-lg');
                    break;

                default:
                    $("#btn_a").attr('class', 'btn btn-default btn-lg disabled ');
                    $("#btn_b").attr('class', 'btn btn-warning btn-lg');
                    $("#btn_c").attr('class', 'btn btn-warning btn-lg');
                    break;
            }

            if ("<%= type %>" != '') {
                bindTable2("<%= type %>");
            };
        });

        function Check_Prot_Time() {    //檢查維護終止日並寄信
            $.ajax({
                url: 'Default.aspx/Check_Prot_Time',
                type: 'POST',
                //data: JSON.stringify({ mno: OE_Case_ID }),
                contentType: 'application/json; charset=UTF-8',
                dataType: "json",
                success: function (doc) {
                    var json = JSON.parse(doc.d.toString());
                    //alert(json.status);
                }
            });
        }

        function moreFields(str_mno_1, str_mno_2, Flag) {
            var LV = "<%= Session["Agent_LV"]%>";
            var str_mno_1 = str_mno_1;
            var str_mno_2 = str_mno_2;
            var MNo = "";
            str_mno_1 = String(str_mno_1);
            str_mno_2 = String(str_mno_2);
            MNo = str_mno_1 + str_mno_2.substr(1, str_mno_2.length);

            //============================
            var cno;
            if (Flag == '1') {
                cno = "0020010000/0020010007.aspx?seqno=" + MNo;
                window.location.href = cno;
            }
            else if (Flag == '2' || Flag == '3' || Flag == '4' || Flag == '7') {
                if (LV == '10') {
                    cno = "0030010099.aspx?seqno=" + MNo;
                }
                else {
                    cno = "0040010002.aspx?seqno=" + MNo;
                };
                window.location.href = cno;
            }
            else if (Flag == '5') {
                cno = "0030010000/0030010003.aspx?seqno=" + MNo + "&type=5";
                window.location.href = cno;
            }
            else if (Flag == '6' || Flag == '8' || Flag == '9') {
                cno = "0020010000/0020010008.aspx?seqno=" + MNo;
                window.location.href = cno;
            }
            else if (Flag == '0') {
                cno = "0040010002.aspx?seqno=" + MNo;
                window.location.href = cno;
            }
        };

        

            function bindTable() {
                var str_url;
                $.ajax({
                    url: 'Default.aspx/GetClassGroup',
                    type: 'POST',
                    contentType: 'application/json; charset=UTF-8',
                    dataType: "json",       //如果要回傳值，請設成 json
                    success: function (doc) {
                        var table = $('#data').DataTable({
                            destroy: true,
                            searching: false,
                            bLengthChange: false,
                            bPaginate: false,
                            data: eval(doc.d),
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
                            "columnDefs": [{
                                "targets": -1,
                                "data": null,
                                "searchable": false,
                                "paging": false,
                                "ordering": false,
                                "info": false
                            }],
                            // Type_0 : 取消
                            // Type_1 : 尚未審核
                            // Type_2 : 尚未派工
                            // Type_3 : 尚未結案
                            // Type_4 : 已經結案
                            // Type_5 : 退單
                            // Type_6 : 處理中
                            // Type_7 : 母單
                            // Type_8 : 子單
                            // Type_9 : 暫結案
                            columns: [
                                    {
                                        data: "Type_1", render: function (data, type, row, meta) {
                                            if ("<%= type %>" == '1') {
                                                return "<a class='btn btn-default btn-lg disabled'><span class='glyphicon glyphicon-hourglass'></span><br/>（需求單）<br/>尚未審核<br/>" + data + " 件</a>";
                                            } else {
                                                return "<button id='DT1' type='button' class='btn btn-info btn-lg '><span class='glyphicon glyphicon-hourglass'></span><br/>（需求單）<br/>尚未審核<br/>" + data + " 件</button>";
                                            };
                                        }
                                    },
                                        {
                                            data: "Type_2", render: function (data, type, row, meta) {
                                                if ("<%= type %>" == '2') {
                                                    return "<a class='btn btn-default btn-lg disabled'><span class='glyphicon glyphicon-hourglass'></span><br/>（需求單）<br/>尚未派工<br/>" + data + " 件</a>";
                                                } else {
                                                    return "<button id='DT2' type='button' class='btn btn-info btn-lg'><span class='glyphicon glyphicon-hourglass'></span><br/>（需求單）<br/>尚未派工<br/>" + data + " 件</button>";
                                                };
                                            }
                                        },
                                         {
                                             data: "Type_6", render: function (data, type, row, meta) {
                                                 if ("<%= type %>" == '6') {
                                                     return "<a class='btn btn-default btn-lg disabled'><span class='glyphicon glyphicon-alert'></span><br/>（派工單）<br/>處理中<br/>" + data + " 件</a>";
                                                 } else {
                                                     return "<button id='DT6' type='button' class='btn btn-warning btn-lg'><span class='glyphicon glyphicon-alert'></span><br/>（派工單）<br/>處理中<br/>" + data + " 件</button>";
                                                 };
                                             }
                                         },
                                        {
                                            data: "Type_9", render: function (data, type, row, meta) {
                                                if ("<%= type %>" == '9') {
                                                    return "<a class='btn btn-default btn-lg disabled'><span class='glyphicon glyphicon-hourglass'></span><br/>（派工單）<br/>暫結案<br/>" + data + " 件</a>";
                                                } else {
                                                    return "<button id='DT9' type='button' class='btn btn-info btn-lg'><span class='glyphicon glyphicon-hourglass'></span><br/>（派工單）<br/>暫結案<br/>" + data + " 件</button>";
                                                };
                                            }
                                        },
                                        {
                                            data: "Type_3", render: function (data, type, row, meta) {
                                                if ("<%= type %>" == '3') {
                                                    return "<a class='btn btn-default btn-lg disabled'><span class='glyphicon glyphicon-hourglass'></span><br/>（需求單）<br/>尚未結案<br/>" + data + " 件</a>";
                                                } else {
                                                    return "<button id='DT3' type='button' class='btn btn-info btn-lg'><span class='glyphicon glyphicon-hourglass'></span><br/>（需求單）<br/>尚未結案<br/>" + data + " 件</button>";
                                                };
                                            }
                                        },
                                        {
                                            data: "Type_4", render: function (data, type, row, meta) {
                                                if ("<%= type %>" == '4') {
                                                    return "<a class='btn btn-default btn-lg disabled'><span class='glyphicon glyphicon-hourglass'></span><br/>（需求單）<br/>已經結案<br/>" + data + " 件</a>";
                                                } else {
                                                    return "<button id='DT4' type='button' class='btn btn-info btn-lg'><span class='glyphicon glyphicon-hourglass'></span><br/>（需求單）<br/>已經結案<br/>" + data + " 件</button>";
                                                };
                                            }
                                        },
                                        {
                                            data: "Type_0", render: function (data, type, row, meta) {
                                                if ("<%= type %>" == '0') {
                                                    return "<a class='btn btn-default btn-lg disabled'><span class='glyphicon glyphicon-remove'></span><br/>（需求單）<br/>取消<br/>" + data + " 件</a>";
                                                } else {
                                                    return "<button id='DT0' type='button' class='btn btn-danger btn-lg'><span class='glyphicon glyphicon-remove'></span><br/>（需求單）<br/>取消<br/>" + data + " 件</button>";
                                                };
                                            }
                                        },
                                        {
                                            data: "Type_5", render: function (data, type, row, meta) {
                                                if ("<%= type %>" == '5') {
                                                    return "<a class='btn btn-default btn-lg disabled'><span class='glyphicon glyphicon-remove'></span><br/>（需求單）<br/>退單<br/>" + data + " 件</a>";
                                                } else {
                                                    return "<button id='DT5' type='button' class='btn btn-danger btn-lg'><span class='glyphicon glyphicon-remove'></span><br/>（需求單）<br/>退單<br/>" + data + " 件</button>";
                                                };
                                            }
                                        },
                                        {
                                            data: "Type_7", render: function (data, type, row, meta) {
                                                if ("<%= type %>" == '7') {
                                                    return "<a class='btn btn-default btn-lg disabled'><span class='glyphicon glyphicon-search'></span><br/>需求單<br/>" + data + " 件</a>";
                                                } else {
                                                    return "<button id='DT7' type='button' class='btn btn-success btn-lg'><span class='glyphicon glyphicon-search'></span><br/>需求單<br/>" + data + " 件</button>";
                                                };
                                            }
                                        },
                                        {
                                            data: "Type_8", render: function (data, type, row, meta) {
                                                if ("<%= type %>" == '8') {
                                                    return "<a class='btn btn-default btn-lg disabled'><span class='glyphicon glyphicon-search'></span><br/>派工單<br/>" + data + " 件</a>";
                                                } else {
                                                    return "<button id='DT8' type='button' class='btn btn-success btn-lg'><span class='glyphicon glyphicon-search'></span><br/>派工單<br/>" + data + " 件</button>";
                                                };
                                            }
                                        }
                            ]
                        });

                        $('#data tbody').on('click', '#DT1', function () {
                            str_url = "../Default.aspx?type=1&str_time=<%= str_time %>";
                            window.location.href = str_url;
                        });

                        $('#data tbody').on('click', '#DT2', function () {
                            str_url = "../Default.aspx?type=2&str_time=<%= str_time %>";
                            window.location.href = str_url;
                        });

                        $('#data tbody').on('click', '#DT3', function () {
                            str_url = "../Default.aspx?type=3&str_time=<%= str_time %>";
                            window.location.href = str_url;
                        });

                        $('#data tbody').on('click', '#DT4', function () {
                            str_url = "../Default.aspx?type=4&str_time=<%= str_time %>";
                            window.location.href = str_url;
                        });

                        $('#data tbody').on('click', '#DT5', function () {
                            str_url = "../Default.aspx?type=5&str_time=<%= str_time %>";
                            window.location.href = str_url;
                        });

                        $('#data tbody').on('click', '#DT6', function () {
                            str_url = "../Default.aspx?type=6&str_time=<%= str_time %>";
                            window.location.href = str_url;
                        });

                        $('#data tbody').on('click', '#DT7', function () {
                            str_url = "../Default.aspx?type=7&str_time=<%= str_time %>";
                            window.location.href = str_url;
                        });

                        $('#data tbody').on('click', '#DT8', function () {
                            str_url = "../Default.aspx?type=8&str_time=<%= str_time %>";
                            window.location.href = str_url;
                        });

                        $('#data tbody').on('click', '#DT9', function () {
                            str_url = "../Default.aspx?type=9&str_time=<%= str_time %>";
                            window.location.href = str_url;
                        });

                        $('#data tbody').on('click', '#DT0', function () {
                            str_url = "../Default.aspx?type=0&str_time=<%= str_time %>";
                            window.location.href = str_url;
                        });
                    }
                });
            }

            function List_Record() {  //列出職務代理
                $.ajax({
                    url: 'Default.aspx/List_Record',
                    type: 'POST',
                    data: JSON.stringify({ }),
                    contentType: 'application/json; charset=UTF-8',
                    dataType: "json",       //如果要回傳值，請設成 json
                    success: function (doc) {
                        var table = $('#tab_Record').DataTable({
                            destroy: true,
                            data: eval(doc.d), "oLanguage": {
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
                            "aLengthMenu": [[10, 25], [10, 25]],
                            "iDisplayLength": 10,
                            "columnDefs": [{
                                "targets": -1,
                                "data": null,
                                "searchable": false,
                                "paging": false,
                                "ordering": false,
                                "info": false
                            }],
                            columns: [
                                    { data: "C" },
                                    //{ data: "A" },
                                    { data: "B" },
                                    { data: "D" },
                                    { data: "E" },
                                    {
                                        data: "D", render: function (data, type, row, meta) {
                                            if (row.D != "休假申請") {
                                                return "";
                                            } else {
                                                return "<button type='button' class='btn btn-primary btn-lg' id='button' " +
                                                    "data-toggle='modal' data-target='#myModal' >" +
                                                    "<span class='glyphicon glyphicon-pencil'>" +
                                                    "</span>&nbsp;接受</button>";
                                            }
                                        }
                                    },
                                    {
                                        data: "D", render: function (data, type, row, meta) {
                                            if (row.D != "休假申請") {
                                                return "";
                                            } else {
                                                return "<button type='button' class='btn btn-danger btn-lg' id='delete'>" +
                                                    "<span class='glyphicon glyphicon-remove'>" +
                                                    "</span>&nbsp;拒絕</button>";
                                            }
                                        }
                                    },
                            ]
                        });
                        $('#tab_Record tbody').unbind('click').
                            on('click', '#button', function () {
                                var table = $('#tab_Record').DataTable();
                                var cno = table.row($(this).parents('tr')).data().A;
                                Accept(cno);
                            })
                            .on('click', '#delete', function () {
                                var table = $('#tab_Record').DataTable();
                                var cno = table.row($(this).parents('tr')).data().A;
                                Reject(cno);
                            });
                    }
                });
            }

            function Accept(DI_No) {
                //alert(Agent_ID);
                if (confirm("確定要接受嗎？")) {
                    $.ajax({
                        url: 'Default.aspx/Accept',
                        ache: false,
                        type: 'POST',
                        //async: false,
                        data: JSON.stringify({ DI_No: DI_No }),
                        contentType: 'application/json; charset=UTF-8',
                        dataType: "json",       //如果要回傳值，請設成 json
                        success: function (data) {
                            var json = JSON.parse(data.d.toString());
                            if (json.status == "success") {
                                List_Record();
                            }
                        }
                    });
                }   //*/
            };
            function Reject(DI_No) {
                //alert(Agent_ID);
                if (confirm("確定要拒絕嗎？")) {
                    $.ajax({
                        url: 'Default.aspx/Reject',
                        ache: false,
                        type: 'POST',
                        //async: false,
                        data: JSON.stringify({ DI_No: DI_No }),
                        contentType: 'application/json; charset=UTF-8',
                        dataType: "json",       //如果要回傳值，請設成 json
                        success: function (data) {
                            var json = JSON.parse(data.d.toString());
                            if (json.status == "success") {
                                List_Record();
                            }
                        }
                    });
                }
            };

            function bindTable3() {
                var str_type = "<%= type %>";
                var str_url;
                $.ajax({
                    url: 'Default.aspx/GetClassGroup',
                    type: 'POST',
                    contentType: 'application/json; charset=UTF-8',
                    dataType: "json",       //如果要回傳值，請設成 json
                    success: function (doc) {
                        var table = $('#data3').DataTable({
                            destroy: true,
                            searching: false,
                            bLengthChange: false,
                            bPaginate: false,
                            data: eval(doc.d), "oLanguage": {
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
                            "columnDefs": [{
                                "targets": -1,
                                "data": null,
                                "searchable": false,
                                "paging": false,
                                "ordering": false,
                                "info": false
                            }],
                            columns: [
                                    {
                                        data: "Type_6", render: function (data, type, row, meta) {
                                            if (str_type == '6') {
                                                return "<a class='btn btn-default btn-lg disabled'><span class='glyphicon glyphicon-alert'></span><br/>（派工單）<br/>尚未處理 " + data + " 件</a>";
                                            } else {
                                                return "<button id='DT6' type='button' class='btn btn-warning btn-lg'><span class='glyphicon glyphicon-alert'></span><br/>（派工單）<br/>尚未處理 " + data + " 件</button>";
                                            };
                                        }
                                    },
                                    {
                                        data: "Type_3", render: function (data, type, row, meta) {
                                            if (str_type == '3') {
                                                return "<a class='btn btn-default btn-lg disabled'><span class='glyphicon glyphicon-hourglass'></span><br/>（需求單）<br/>尚未結案 " + data + " 件</a>";
                                            } else {
                                                return "<button id='DT3' type='button' class='btn btn-info btn-lg'><span class='glyphicon glyphicon-hourglass'></span><br/>（需求單）<br/>尚未結案 " + data + " 件</button>";
                                            };
                                        }
                                    },
                                    {
                                        data: "Type_5", render: function (data, type, row, meta) {
                                            if (str_type == '5') {
                                                return "<a class='btn btn-default btn-lg disabled'><span class='glyphicon glyphicon-remove'></span><br/>（需求單）<br/>退單 " + data + " 件</a>";
                                            } else {
                                                return "<button id='DT5' type='button' class='btn btn-danger btn-lg'><span class='glyphicon glyphicon-remove'></span><br/>（需求單）<br/>退單 " + data + " 件</button>";
                                            };
                                        }
                                    },
                                    {
                                        data: "Type_7", render: function (data, type, row, meta) {
                                            if (str_type == '7') {
                                                return "<a class='btn btn-default btn-lg disabled'><span class='glyphicon glyphicon-search'></span><br/>需求單<br/>" + data + " 件</a>";
                                            } else {
                                                return "<button id='DT7' type='button' class='btn btn-success btn-lg'><span class='glyphicon glyphicon-search'></span><br/>需求單<br/>" + data + " 件</button>";
                                            };
                                        }
                                    },
                                    {
                                        data: "Type_8", render: function (data, type, row, meta) {
                                            if (str_type == '8') {
                                                return "<a class='btn btn-default btn-lg disabled'><span class='glyphicon glyphicon-search'></span><br/>派工單<br/>" + data + " 件</a>";
                                            } else {
                                                return "<button id='DT8' type='button' class='btn btn-success btn-lg'><span class='glyphicon glyphicon-search'></span><br/>派工單<br/>" + data + " 件</button>";
                                            };
                                        }
                                    }]
                        });

                        $('#data3 tbody').on('click', '#DT3', function () {
                            str_url = "../Default.aspx?type=3&str_time=<%= str_time %>";
                            window.location.href = str_url;
                        });

                        $('#data3 tbody').on('click', '#DT5', function () {
                            str_url = "../Default.aspx?type=5&str_time=<%= str_time %>";
                            window.location.href = str_url;
                        });

                        $('#data3 tbody').on('click', '#DT6', function () {
                            str_url = "../Default.aspx?type=6&str_time=<%= str_time %>";
                            window.location.href = str_url;
                        });

                        $('#data3 tbody').on('click', '#DT7', function () {
                            str_url = "../Default.aspx?type=7&str_time=<%= str_time %>";
                            window.location.href = str_url;
                        });

                        $('#data3 tbody').on('click', '#DT8', function () {
                            str_url = "../Default.aspx?type=8&str_time=<%= str_time %>";
                            window.location.href = str_url;
                        });
                    }
                });
            }

            function bindTable2(Flag) {
                var a = "需求單編號";
                var b = "填單人員";
                var c = "勞工姓名";
                var service_url = "Default_MNo";
                if (Flag == "6" || Flag == "8" || Flag == "9") {
                    a = "派工單編號";
                    b = "處理人員";
                    c = "緊急程度";
                    service_url = "Default_CNo";
                }
                $("#data2").dataTable({
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
                    "columns": [
                        null,
                        { "title": a },
                        null,
                        { "title": b },
                        null,
                        null,
                        { "title": c },
                        null
                    ],
                    //"aLengthMenu": [[25, 50, 100, 150, 250, 500, -1], [25, 50, 100, 150, 250, 500, "All"]],
                    "aLengthMenu": [[10, 25, 50, 100], [10, 25, 50, 100]],
                    "iDisplayLength": 10,
                    "searching": false,
                    "bSortClasses": false,
                    "bStateSave": false,
                    "bPaginate": true,
                    "bAutoWidth": false,
                    "bProcessing": true,
                    "bServerSide": true,
                    "bDestroy": true,
                    "sAjaxSource": "../WebService.asmx/" + service_url,
                    "sServerMethod": "POST",
                    "contentType": "application/json; charset=UTF-8",
                    "sPaginationType": "full_numbers",
                    "bDeferRender": true,
                    "fnServerParams": function (aoData) {
                        aoData.push(
                            { "name": "iParticipant", "value": $("#participant").val() },
                            { "name": "Flag", "value": Flag },
                            { "name": "Time_Flag", "value": "<%= str_time %>" },
                            { "name": "sSearch", "value": $("#txt_Search").val() }
                            );
                    },
                    "fnServerData": function (sSource, aoData, fnCallback) {
                        $("#btn_search").attr('class', 'btn btn-info btn-lg disabled');
                        $("#Div_Loading").modal();
                        $.ajax({
                            "dataType": 'json',
                            "contentType": 'application/json; charset=UTF-8',
                            "type": "GET",
                            "url": sSource,
                            "data": aoData,
                            "success":
                                function (msg) {
                                    var json = jQuery.parseJSON(msg.d);
                                    fnCallback(json);
                                    $("#data2").show();
                                    $("#Div_Loading").modal('hide');
                                    $("#btn_search").attr('class', 'btn btn-info btn-lg');
                                },
                            "error": function (msg) {
                                alert("資料明細讀取發生錯誤，請重新嘗試或詢問管理人員謝謝。");
                                $("#Div_Loading").modal('hide');
                                $("#btn_search").attr('class', 'btn btn-info btn-lg');
                            }
                        });
                    }
                });
                //============================================================
            }        
            function Btn_Search() {
                alert();
                if ("<%= type %>" != '') { bindTable2("<%= type %>"); };
            }        
            function Link() {
                //alert('456');
                window.location.href = "/0030010000/0030010000.aspx";
            }

            function URL2() {
                $.ajax({
                    url: 'Default.aspx/URL',
                    type: 'POST',
                    data: JSON.stringify({  }),
                    contentType: 'application/json; charset=UTF-8',
                    dataType: "json",
                    success: function (doc) {
                        var json = JSON.parse(doc.d.toString());
                        if (json.type == "ok") {
                            window.location = json.status;
                        } else {
                            alert(json.status);
                        }
                    }
                });
            }
        
    </script>
    <style>
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

        #data2 td:nth-child(8), #data2 td:nth-child(7), #data2 td:nth-child(6), #data2 td:nth-child(5),
        #data2 td:nth-child(4), #data2 td:nth-child(3), #data2 td:nth-child(2), #data2 td:nth-child(1),
        #data3 td:nth-child(5), #data3 td:nth-child(4), #data3 td:nth-child(3), #data3 td:nth-child(2),
        #data3 td:nth-child(1), #data td:nth-child(10), #data td:nth-child(9),
        #data td:nth-child(8), #data td:nth-child(7), #data td:nth-child(6), #data td:nth-child(5),
        #data td:nth-child(4), #data td:nth-child(3), #data td:nth-child(2), #data td:nth-child(1) {
            text-align: center;
        }
    </style>

    <!-- ====== Loading ====== -->
    <div class="modal fade" id="Div_Loading" role="dialog" data-backdrop="static" data-keyboard="false">
        <div class="modal-dialog" style="width: 350px;">
            <div class="modal-content" style="margin-top: 100%; margin-left: 25%;">
                <div style="text-align: center">
                    <br />
                    <img src="img/ajax-loader.gif" />
                    <h3>Loading...</h3>
                    <br />
                </div>
            </div>
        </div>
    </div>
    <!-- ====== Loading ====== -->
    <!----------------------------------------------->
    <div class="table-responsive" style="width: 95%; margin: 10px 20px" runat="server">
        <h1><strong>個人事項導覽</strong></h1>
            <div style="float: right;">
                <button id="Button2" type="button" onclick="URL2();" class="btn btn-success btn-lg ">&nbsp;補打卡功能&nbsp;</button> 
                <button id="Button1" type="button" onclick="Link();" class="btn btn-success btn-lg ">&nbsp;每日打卡&nbsp;</button>   
                <a id="btn_a" href="Default.aspx?type=<%= type %>&str_time=0" class="btn btn-warning btn-lg"><span class='glyphicon glyphicon-search'></span>&nbsp;今日</a>&nbsp;
                <a id="btn_b" href="Default.aspx?type=<%= type %>&str_time=1" class="btn btn-warning btn-lg"><span class='glyphicon glyphicon-search'></span>&nbsp;本月</a>&nbsp;
                <a id="btn_c" href="Default.aspx?type=<%= type %>&str_time=2" class="btn btn-warning btn-lg"><span class='glyphicon glyphicon-search'></span>&nbsp;全部</a>
            </div>
        
    </div>
    <div id="table_data" class="table-responsive" style="width: 95%; margin: 10px 20px" runat="server">
        <h2><strong>職務代理(超過一週不顯示)</strong></h2>
        <table id="tab_Record" class="display table table-striped" style="width: 99%">
            <thead>
                <tr>
                    <th style="text-align: center;">休假日期</th>
                    <!--<th style="text-align: center;">資料編號</th>-->
                    <th style="text-align: center;">休假人員</th>
                    <th style="text-align: center;">狀態</th>
                    <th style="text-align: center;">修改時間</th>
                    <th style="text-align: center;">接受</th>
                    <th style="text-align: center;">拒絕</th>
                </tr>
            </thead>
        </table>
        <!--
        <table id="data" class="display" style="width: 99%">
            <thead>
                <tr>
                    <th style="text-align: center; width: 10%;">尚未審核</th>
                    <th style="text-align: center; width: 10%;">尚未派工</th>
                    <th style="text-align: center; width: 10%;">處理中</th>
                    <th style="text-align: center; width: 10%;">暫結案</th>
                    <th style="text-align: center; width: 10%;">尚未結案</th>
                    <th style="text-align: center; width: 10%;">已經結案</th>
                    <th style="text-align: center; width: 10%;">取消</th>
                    <th style="text-align: center; width: 10%;">退單</th>
                    <th style="text-align: center; width: 10%;">需求單</th>
                    <th style="text-align: center; width: 10%;">派工單</th>        
                </tr>
            </thead>
        </table>-->
    </div>

    <div id="table_data3" class="table-responsive" style="width: 95%; margin: 10px 20px" runat="server">
        <table id="data3" class="display" style="width: 99%">
            <thead>
                <tr>
                    <th style="text-align: center; width: 20%;">尚未處理</th>
                    <th style="text-align: center; width: 20%;">尚未結案</th>
                    <th style="text-align: center; width: 20%;">退單</th>
                    <th style="text-align: center; width: 20%;">需求單</th>
                    <th style="text-align: center; width: 20%;">派工單</th>
                </tr>
            </thead>
        </table>
    </div>

    <div class="table-responsive" style="width: 95%; margin: 10px 20px">
        <div style="border: 1px solid #EAEAEA; border-radius: 8px; padding: 0px 20px 20px; margin-top: 10px;">
            <div class="form-inline">
                <div style="text-align: right">
                    <br />
                    <label>搜索：</label>
                    <input type="text" class="form-control" id="txt_Search" />
                    <button type="button" class="btn btn-info btn-lg" id="btn_search" onclick="Btn_Search();"><span class='glyphicon glyphicon-search'></span>&nbsp;開始搜索</button>
                </div>
            </div>
        </div>
        <br />
        <table id="data2" class="display table table-striped" style="width: 99%">
            <thead>
                <tr>
                    <th style="text-align: center;">預定時間</th>
                    <th style="text-align: center;">需求單編號</th>
                    <th style="text-align: center;">狀態</th>
                    <th style="text-align: center;">填單人員</th>
                    <th style="text-align: center;">廠商名稱</th>
                    <th style="text-align: center;">服務內容</th>
                    <th style="text-align: center;">勞工姓名</th>
                    <th style="text-align: center;">明細</th>
                </tr>
            </thead>
        </table>
    </div>
</asp:Content>


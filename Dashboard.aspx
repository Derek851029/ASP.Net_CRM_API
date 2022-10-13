<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="Dashboard.aspx.cs" Inherits="Dashboard" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">

<div class="content">
    <div class="page-breadcrumb">
        <div class="row align-items-center">
            <div class="col-6">
                <nav aria-label="breadcrumb">
                    <ol class="breadcrumb mb-0 d-flex align-items-center">
                        <li class="breadcrumb-item">
                            <a href="Dashboard.aspx" class="link">
                                <i class="bi-house"></i>
                            </a>
                        </li>
                        <li class="breadcrumb-item active" aria-current="page">儀錶板</li>
                    </ol>
                    </nav>
                <h1 class="mb-0 fw-bold">儀錶板</h1> 
            </div>
        </div>

        <div class="container-fluid">
            <div class="row">
                <div class="col-lg-8" style="height:500px">
                    <div class="card">
                        <div class="card-body">
                            <div id="chartContainer"></div>
                        </div>
                    </div>
                </div>
                <div class="col-lg-4" style="height:600px">
                    <div class="card">
                        <div class="card-body">
                            <h4 class="card-title">每周統計</h4>
                                <h6 class="card-subtitle">會員平均消費</h6>
                                <div class="mt-5 pb-3 d-flex align-items-center">
                                    <i class="bi-award" style="font-size: 2rem; color: #FFD700;"></i>
                                    <div class="ms-3">
                                        <h5 class="mb-0 fw-bold" id="sales1">方XX</h5>
                                    </div>
                                    <div class="ms-auto">
                                        <span class="badge bg-light text-muted" id="price1">$100000</span>
                                    </div>
                                </div>
                                <div class="py-3 d-flex align-items-center">
                                        <i class="bi-award" style="font-size: 2rem; color: #778899;"></i>
                                    <div class="ms-3">
                                        <h5 class="mb-0 fw-bold" id="sales2">何XX</h5>
                                    </div>
                                    <div class="ms-auto">
                                        <span class="badge bg-light text-muted" id="price2">$50000</span>
                                    </div>
                                </div>
                                <div class="py-3 d-flex align-items-center">
                                    <i class="bi-award" style="font-size: 2rem; color: #DAA520;"></i>
                                    <div class="ms-3">
                                        <h5 class="mb-0 fw-bold" id="sales3">蘇XX</h5>
                                    </div>
                                    <div class="ms-auto">
                                        <span class="badge bg-light text-muted" id="price3">$30000</span>
                                    </div>
                                </div>
                                <div class="py-3 d-flex align-items-center">
                                    <i class="bi-award" style="font-size: 2rem; color: #6495ED;"></i>
                                    <div class="ms-3">
                                        <h5 class="mb-0 fw-bold" id="sales4">許XX</h5>
                                    </div>
                                    <div class="ms-auto">
                                        <span class="badge bg-light text-muted" id="price4">$10000</span>
                                    </div>
                                </div>
                                <div class="pt-3 d-flex align-items-center">
                                    <i class="bi-award" style="font-size: 2rem; color: #7CFC00;"></i>
                                    <div class="ms-3">
                                        <h5 class="mb-0 fw-bold" id="sales5">張XX</h5>
                                    </div>
                                    <div class="ms-auto">
                                        <span class="badge bg-light text-muted" id="price5">$5000</span>
                                    </div>
                                </div>
                        </div>
                    </div>
                </div>
            </div>
            <div class="row" style="height:450px">
                <div class="col-lg-8">
                    <div class="card">
                        <div class="card-body">
                            <div id="chartContainer2"></div>
                        </div>
                    </div>
                </div>
            </div>
            <div class="row">
                <div class="col-12">
                    <div class="card">
                        <div class="card-body">
                            <div class="d-md-flex">
                                <div>
                                    <h4 class="card-title">每月暢銷商品</h4>
                                    <h5 class="card-subtitle" style="color:#777e89">暢銷商品概覽</h5>
                                </div>
                            </div>
                            <table id="table1" class="display table table-striped" style="text-align:center;width:100%">
                                <thead>
                                    <tr>
                                        <th style="text-align:center;">商品條碼</th>
                                        <th style="text-align:center;">商品名稱</th>
                                        <th style="text-align:center;">銷售量</th>
                                        <th style="text-align:center;">收益</th>
                                        <th style="text-align:center;">更新時間</th>
                                    </tr>
                                </thead>
                            </table>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
</div>
    <script src="js/canvasjs.min.js"></script>
    <script type="text/javascript">
        var date = new Date();
        var year = date.getFullYear()
        var month = date.getMonth()
        var firstDay = new Date(year, month - 1, 1);
        var lastDay = new Date(year, month, 0);
        $(function () {
            listener_event()
            chartContainer()
            DailyTurnover()
            CommodityPayList()
        })

        function listener_event() {
            $('#dashboard').css({ "background-color": "#1a9bfc", "color": "white" })
        }

        function chartContainer() {
            var chart = new CanvasJS.Chart("chartContainer", {
                exportEnabled: true,
                animationEnabled: true,
                title: {
                    text: "消費年齡層分佈"
                },
                legend: {
                    cursor: "pointer",
                    itemclick: explodePie
                },
                data: [{
                    type: "pie",
                    showInLegend: true,
                    toolTipContent: "{name}: <strong>{y}人</strong>",
                    indexLabel: "{name} - {y}人",
                    dataPoints: [
                        { y: 17, name: "年齡18~29", exploded: true },
                        { y: 58, name: "年齡30~39" },
                        { y: 32, name: "年齡40~49" },
                        { y: 29, name: "年齡50~59" },
                        { y: 12, name: "年齡60~69" },
                        { y: 5, name: "年齡70~79" },
                    ]
                }]
            });
            chart.render();
            function toggleDataSeries(e) {
                if (typeof (e.dataSeries.visible) === "undefined" || e.dataSeries.visible) {
                    e.dataSeries.visible = false;
                }
                else {
                    e.dataSeries.visible = true;
                }
                chart.render();
            }
        }

        function DailyTurnover() {
            var data;
            $.ajax({
                url: 'Dashboard.aspx/DailyTurnover',
                type: 'POST',
                async: false,
                data: JSON.stringify({}),
                contentType: 'application/json; charset=UTF-8',
                dataType: "json",       //如果要回傳值，請設成 json
                success: function (doc) {
                    data = JSON.parse(doc.d)
                }
            });

            var newData = []
            for (let i = 0; i < data.length; i++) {
                var sqldata = data[i]

                var sqldate = sqldata.T_DAY
                var sqlprice = sqldata.PRICE
                var test = new Date(sqldata.T_DAY)

                var year = sqldate.substring(0, 4)
                var month = sqldate.substring(4, 6) -1
                if (month[0] == 0) {
                    month = sqldate.substring(5, 6) -1
                }
                var day = sqldate.substring(6, 8)
                if (day[0] == 0) {
                    day = sqldate.substring(7, 8)
                }

                newData.push({ x: new Date(year, month, day), y: sqlprice })
            }

            var chart = new CanvasJS.Chart("chartContainer2", {
                animationEnabled: true,
                title: {
                    text: "每日收入統計"
                },
                axisY: {
                    title: "收入(TWD)",
                },
                data: [{
                    type: "column",
                    showInLegend: true,
                    legendMarkerColor: "grey",
                    legendText: "營業額",
                    yValueFormatString: "NT$##0.00",
                    dataPoints: newData
                }]
            });
            chart.render();
        }

        function explodePie(e) {
            if (typeof (e.dataSeries.dataPoints[e.dataPointIndex].exploded) === "undefined" || !e.dataSeries.dataPoints[e.dataPointIndex].exploded) {
                e.dataSeries.dataPoints[e.dataPointIndex].exploded = true;
            } else {
                e.dataSeries.dataPoints[e.dataPointIndex].exploded = false;
            }
            e.chart.render();
        }

        function CommodityPayList() {
            $.ajax({
                type: 'post',
                url: 'Dashboard.aspx/CommodityPayList',
                contentType: 'application/json;utf-8',
                data: JSON.stringify({}),
                dataType: 'json',
                success: function (doc) {
                    var full_data = eval(doc.d)
                    console.log(full_data)
                    $('#table1').dataTable({
                        //"searching": false,
                        //"bLengthChange": false,
                        //"bInfo": false,
                        responsive: true,
                        destroy: true,
                        data: full_data,
                        "oLanguage": {
                            "sUrl": "",
                            "oPaginate": {
                                "sFirst": "首頁",
                                "sPrevious": "上頁",
                                "sNext": "下頁",
                                "sLast": "尾頁"
                            }
                        },
                        //"aaSorting": [[0, 'asc']],
                        //"aLengthMenu": [[10,50], [10,50]],
                        //"iDisplayLength": 10,
                        "columnDefs": [{
                            "targets": -1,
                            "data": null,
                            "searchable": false,
                            "paging": false,
                            "ordering": false,
                            "info": false,
                        }],
                        columns: [
                            { "data": "PLU_CODE" },
                            { "data": "PC_NAME" },
                            { "data": "CNT" },
                            { "data": "PRICE" },
                            {
                                "data": "PRICE", "render": function (data, type, full, meta) {
                                    let date = new Date();
/*                                    console.log(date.toISOString().split('T')[0]);*/
                                    return date.toISOString().split('T')[0]
                                }
                            },
                        ]
                    });
                }
            });
        }
    </script>
</asp:Content>

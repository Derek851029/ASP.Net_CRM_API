<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="Default.aspx.cs" Inherits="_Default" %>

<asp:Content ID="Content" ContentPlaceHolderID="head2" runat="Server">
    <script src="js/canvasjs.min.js"></script>
    <script type="text/javascript">
        $(function () {
            chartContainer()
            chartContainer2()
            //chartContainer3()
            //chartContainer4()
        })

        function chartContainer() {
            var chart = new CanvasJS.Chart("chartContainer", {
                animationEnabled: true,
                title: {
                    text: "年銷售總結"
                },
                toolTip: {
                    shared: true
                },
                legend: {
                    cursor: "pointer",
                    itemclick: toggleDataSeries
                },
                dataPointWidth: 20,
                data: [{
                    type: "column",
                    name: "2021",
                    legendText: "2021",
                    showInLegend: true,
                    dataPoints: [
                        { label: "Mon", y: 266.21 },
                        { label: "Tue", y: 302.25 },
                        { label: "Wed", y: 157.20 },
                        { label: "Thu", y: 148.77 },
                        { label: "Fri", y: 101.50 },
                        { label: "Sat", y: 97.8 },
                        { label: "Sun", y: 97.8 }
                    ]
                },
                {
                    type: "column",
                    name: "2022",
                    legendText: "2022",
                    axisYType: "secondary",
                    showInLegend: true,
                    dataPoints: [
                        { label: "Mon", y: 266.21 },
                        { label: "Tue", y: 302.25 },
                        { label: "Wed", y: 157.20 },
                        { label: "Thu", y: 148.77 },
                        { label: "Fri", y: 101.50 },
                        { label: "Sat", y: 97.8 },
                        { label: "Sun", y: 97.8 }
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

        function chartContainer2() {
            var chart = new CanvasJS.Chart("chartContainer2", {
                animationEnabled: true,
                title: {
                    text: "每日收入統計"
                },
                axisX: {
                    valueFormatString: "DDD",
                    minimum: new Date(2022, 4, 22, 23),
                    maximum: new Date(2022, 4, 29, 1)
                },
                legend: {
                    verticalAlign: "top",
                    horizontalAlign: "right",
                    dockInsidePlotArea: true
                },
                toolTip: {
                    shared: true
                },
                data: [{
                    name: "營業額",
                    showInLegend: true,
                    legendMarkerType: "square",
                    type: "area",
                    color: "rgba(40,175,101,0.6)",
                    markerSize: 0,
                    yValueFormatString: "$##0.00",
                    dataPoints: [
                        { x: new Date(2022, 4, 23), y: 220 },
                        { x: new Date(2022, 4, 24), y: 120 },
                        { x: new Date(2022, 4, 25), y: 144 },
                        { x: new Date(2022, 4, 26), y: 162 },
                        { x: new Date(2022, 4, 27), y: 129 },
                        { x: new Date(2022, 4, 28), y: 109 },
                        { x: new Date(2022, 4, 29), y: 129 }
                    ]
                }]
            });
            chart.render();
        }

        function chartContainer3() {
            var chart = new CanvasJS.Chart("chartContainer3", {
                theme: "light2", // "light1", "light2", "dark1", "dark2"
                animationEnabled: true,
                title: {
                    text: "每月申辦會員數"
                },
                axisX: {
                    interval: 1,
                    intervalType: "month",
                    valueFormatString: "MMM"
                },
                axisY: {
                    title: "人數",
                    includeZero: true,
                },
                data: [{
                    type: "line",
                    markerSize: 12,
                    xValueFormatString: "MMM, YYYY",
                    dataPoints: [
                        { x: new Date(2021, 00, 1), y: 61, indexLabel: "gain", markerType: "triangle", markerColor: "#6B8E23" },
                        { x: new Date(2021, 01, 1), y: 71, indexLabel: "gain", markerType: "triangle", markerColor: "#6B8E23" },
                        { x: new Date(2021, 02, 1), y: 55, indexLabel: "loss", markerType: "cross", markerColor: "tomato" },
                        { x: new Date(2021, 03, 1), y: 50, indexLabel: "loss", markerType: "cross", markerColor: "tomato" },
                        { x: new Date(2021, 04, 1), y: 65, indexLabel: "gain", markerType: "triangle", markerColor: "#6B8E23" },
                        { x: new Date(2021, 05, 1), y: 85, indexLabel: "gain", markerType: "triangle", markerColor: "#6B8E23" },
                        { x: new Date(2021, 06, 1), y: 68, indexLabel: "loss", markerType: "cross", markerColor: "tomato" },
                        { x: new Date(2021, 07, 1), y: 28, indexLabel: "loss", markerType: "cross", markerColor: "tomato" },
                        { x: new Date(2021, 08, 1), y: 34, indexLabel: "gain", markerType: "triangle", markerColor: "#6B8E23" },
                        { x: new Date(2021, 09, 1), y: 24, indexLabel: "loss", markerType: "cross", markerColor: "tomato" },
                        { x: new Date(2021, 10, 1), y: 44, indexLabel: "gain", markerType: "triangle", markerColor: "#6B8E23" },
                        { x: new Date(2021, 11, 1), y: 34, indexLabel: "loss", markerType: "cross", markerColor: "tomato" }
                    ]
                }]
            });
            chart.render();
        }

        function chartContainer4() {
            var chart = new CanvasJS.Chart("chartContainer4", {
                animationEnabled: true,
                title: {
                    text: "每日來客數"
                },
                axisX: {
                    valueFormatString: "DD MMM",
                    crosshair: {
                        enabled: true,
                        snapToDataPoint: true
                    }
                },
                axisY: {
                    title: "人數",
                    crosshair: {
                        enabled: true,
                        snapToDataPoint: true,
                    }
                },
                data: [{
                    type: "area",
                    xValueFormatString: "DD MMM",
                    dataPoints: [
                        { x: new Date(2021, 07, 01), y: 76 }, //如果要出現8月 data內就要填7月
                        { x: new Date(2021, 07, 02), y: 75 },
                        { x: new Date(2021, 07, 03), y: 76 },
                        { x: new Date(2021, 07, 04), y: 75 },
                        { x: new Date(2021, 07, 05), y: 77 },
                        { x: new Date(2021, 07, 08), y: 77 },
                        { x: new Date(2021, 07, 09), y: 79 },
                        { x: new Date(2021, 07, 10), y: 79 },
                        { x: new Date(2021, 07, 11), y: 80 },
                        { x: new Date(2021, 07, 12), y: 79 },
                        { x: new Date(2021, 07, 15), y: 80 },
                        { x: new Date(2021, 07, 16), y: 79 },
                        { x: new Date(2021, 07, 17), y: 78 },
                        { x: new Date(2021, 07, 18), y: 78 },
                        { x: new Date(2021, 07, 19), y: 77 },
                        { x: new Date(2021, 07, 22), y: 76 },
                        { x: new Date(2021, 07, 23), y: 77 },
                        { x: new Date(2021, 07, 24), y: 77 },
                        { x: new Date(2021, 07, 25), y: 76 },
                        { x: new Date(2021, 07, 26), y: 76 },
                        { x: new Date(2021, 07, 29), y: 76 },
                        { x: new Date(2021, 07, 30), y: 78 },
                        { x: new Date(2021, 07, 31), y: 77 }
                    ]
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
    </script>
<div class="content">
    <div class="page-breadcrumb">
        <div class="row align-items-center">
            <div class="col-6">
                <nav aria-label="breadcrumb">
                    <ol class="breadcrumb mb-0 d-flex align-items-center">
                        <li class="breadcrumb-item">
                            <a href="Default.aspx" class="link">
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
                <div class="col-lg-8">
                    <div class="card">
                        <div class="card-body">
                            <div id="chartContainer"></div>
                        </div>
                    </div>
                </div>
                <div class="col-lg-4">
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
                                        <th style="text-align:center;">商品名稱</th>
                                        <th style="text-align:center;">進貨商</th>
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
    
    
</asp:Content>

<%--<div style="text-align:center;height:100px"><label style="font-size:36px">儀錶板</label></div>
    <div>
        <div id="chartContainer2" style="height: 370px; width: 49%; display:inline-block"></div>
        <div id="chartContainer3" style="height: 370px; width: 49%; display:inline-block"></div>
    </div>
    <div style="height:100px"></div>
    <div>
        <div id="chartContainer" style="height: 370px; width: 49%; display:inline-block"></div>
        <div id="chartContainer4" style="height: 370px; width: 49%; display:inline-block"></div>
    </div>--%>


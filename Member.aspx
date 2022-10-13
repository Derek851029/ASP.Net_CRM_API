<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="Member.aspx.cs" Inherits="Users_Users" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="head2" Runat="Server">
    <h2 id="h2_back" style="display:none">
        <%--<button type="button" class="btn btn-primary"><span class="glyphicon glyphicon-pencil">修改</span></button>--%>
        <button type="button" id="back" class="btn btn-success btn-lg">返回</button>
    </h2>
     <div id="table_view" style="width: 95%; margin: 10px 20px;display:inline-block">
         <div class="col-6">
            <nav aria-label="breadcrumb">
                <ol class="breadcrumb mb-0 d-flex align-items-center">
                    <li class="breadcrumb-item">
                        <a href="Member.aspx" class="link">
                            <i class="bi-house"></i>
                        </a>
                    </li>
                    <li class="breadcrumb-item active" aria-current="page">會員管理</li>
                </ol>
                </nav>
            <h1 class="mb-0 fw-bold">會員申請
                <button type="button" class="btn btn-success btn-lg" data-bs-toggle="modal" data-bs-target="#dialog1" onclick="Clear()"><span class="glyphicon glyphicon-plus">新增會員</span></button>
            </h1> 
        </div>
        <table id="table1" class="table table-striped" style="text-align:center;width:100%">
            <thead>
                <tr>
                    <th style="text-align:center;">會員編號</th>
                    <th style="text-align:center;">姓名</th>
                    <th style="text-align:center;">身分證字號</th>
                    <th style="text-align:center;">出生年月日</th>
                    <th style="text-align:center;">性別</th>
                    <th style="text-align:center;">連絡電話</th>
                    <th style="text-align:center;">Email</th>
                    <th style="text-align:center;">功能選單</th>
                </tr>
            </thead>
        </table>
    </div>
    <%--------------------------------會員資料--------------------------------%>
    <div class="row" id="view1" style="display:none">
        <div class="col-lg-4">
            <div class="card">
                <div class="card-header" style="text-align:center">
                    <img id="image" src="https://bootdey.com/img/Content/avatar/avatar7.png" alt="Admin" class="rounded-circle" width="150">
                </div>
                <ul class="list-group list-group-flush">
                    <li class="list-group-item d-flex justify-content-between align-items-center flex-wrap">
                        <h6  class="mb-0">會員編號:</h6>
                        <span id="CardNum" class="text-secondary"></span>
                    </li>
                    <li class="list-group-item d-flex justify-content-between align-items-center flex-wrap">
                        <h6  class="mb-0">會員姓名:</h6>
                        <span id="CardName" class="text-secondary"></span>
                    </li>
                    <li class="list-group-item d-flex justify-content-between align-items-center flex-wrap">
                        <h6 class="mb-0">身分證字號:</h6>
                        <span id="CardID" class="text-secondary"></span>
                    </li>
                    <li class="list-group-item d-flex justify-content-between align-items-center flex-wrap">
                        <h6 class="mb-0">出生年月日:</h6>
                        <span id="CardBirthday" class="text-secondary"></span>
                    </li>
                    <li class="list-group-item d-flex justify-content-between align-items-center flex-wrap">
                        <h6 class="mb-0">性別:</h6>
                        <span id="CardSex" class="text-secondary"></span>
                    </li>
                    <li class="list-group-item d-flex justify-content-between align-items-center flex-wrap">
                        <h6 class="mb-0">Email:</h6>
                        <span id="CardEmail" class="text-secondary"></span>
                    </li>
                </ul>
            </div>
        </div>
        <div class="col-lg-8">
            <div class="card-header">
                <h1>歷史紀錄</h1>
                <div class="row justify-content-around">
                    起始日期:<input id="startDate"  class="form-control" style="width:30%; background-color:white"/>
                    結束日期:<input id="endDate"  class="form-control" style="width:30%; background-color:white"/>
                    <button type="button" id="search" onclick="" class="btn btn-info" style="width:10%"><span class="glyphicon glyphicon-search">查詢</span></button>
                </div>
                
            </div>
            <div class="card">
            <div class="card-body">
                <table id="table2" class="table table-striped" style="text-align:center;width:100%">
                    <thead>
                        <tr>
                            <th style="text-align:center;">到店時間</th>
                            <th style="text-align:center;">消費金額</th>
                            <th style="text-align:center;">消費明細</th>
                        </tr>
                    </thead>
                </table>
            </div>
            </div>
        </div>
        <%--<div class="col-md-8" id="view2" style="display:none">
            <ul class="nav nav-tabs">
              <li class="nav-item">
                <button type="button" id="li_bt1" class="nav-link active">來店紀錄</button>
                  <ul>
                      <li>2021/10/10 08:00 in class</li>
                      <li>2021/10/10 09:00 in class</li>
                  </ul>
              </li>
              <li class="nav-item">
                <button type="button" id="li_bt2" class="nav-link">消費紀錄</button>
              </li>
              <li class="nav-item">
                <button type="button" id="li_bt3" class="nav-link">點數</button>
              </li>
              <li class="nav-item">
                <button type="button" id="li_bt4" class="nav-link">購買紀錄</button>
              </li>
            </ul>
        </div>--%>
    </div>
    <%--------------------------------會員資料--------------------------------%>

    <!-- ====== 新增會員dialog ====== -->
    <div class="modal fade" id="dialog1" data-bs-keyboard="false" data-bs-backdrop="static" style="overflow: auto">
        <div class="modal-dialog modal-lg">
            <div class="modal-content">
                <div class="modal-header">
                    <h2 class="modal-title" id="dialogtitle">新增會員資料</h2>
                    <button type="button" class="btn-close" data-bs-dismiss="modal" aria-label="Close"></button>
                </div>
                <div class="modal-body">
                    <table class="table table-bordered">
                        <thead class="table-dark">
                            <tr>
                                <th style="text-align:center;" colspan="4">
                                    <span><strong>會員資料</strong></span>
                                </th>
                            </tr>
                        </thead>
                        <tbody>
                            <tr>
                                <th><strong>會員編號</strong></th>
                                <th>
                                    <input type="text" id="MemberNum" class="form-control" disabled="disabled" />
                                </th>
                            </tr>
                            <tr>
                                <th><strong>會員姓名</strong></th>
                                <th>
                                    <input type="text" id="MemberName" class="form-control" placeholder="請輸入會員姓名" required="required"/>
                                </th>
                            </tr>
                            <tr>
                                <th><strong>身分證字號</strong></th>
                                <th>
                                    <input type="text" id="MemberID" class="form-control" placeholder="請輸入身分證字號" required="required"/>
                                </th>
                            </tr>
                            <tr>
                                <th><strong>出生年月日</strong></th>
                                <th>
                                    <input class="form-control" id="MemberDate" placeholder="請選擇出生年月日" required="required"/>
                                </th>
                            </tr>
                            <tr>
                                <th><strong>性別</strong></th>
                                <th>
                                    <input type="checkbox" id="Sex_Man" />先生
                                    <input type="checkbox" id="Sex_Lady"/>小姐
                                </th>
                            </tr>
                            <tr>
                                <th><strong>行動電話</strong></th>
                                <th>
                                    <input type="text" id="MemberPhone" class="form-control" placeholder="請輸入行動電話" required="required"/>
                                </th>
                            </tr>
                            <tr>
                                <th><strong>Email</strong></th>
                                <th>
                                    <input type="text" id="MemberEmail" class="form-control" placeholder="請輸入信箱" required="required"/>
                                </th>
                            </tr>
                            <tr>
                                <th><strong>上傳照片</strong></th>
                                <th>
                                    <div>
                                        <input type="file" id="file" class="form-control" required="required"/>
                                        <button type="button" id="showImage" data-bs-toggle="modal" data-bs-target="#dialog2" onclick="" class="btn btn-outline-primary" style="display:none"><span class="glyphicon glyphicon-search">檢視照片</span></button>
                                        <button type="button" id="Camera" onclick="Start_Camera()" class="btn btn-info"><span class="glyphicon glyphicon-camera"></span>拍照</button>
                                    </div>
                                    
                                </th>
                            </tr>
                        </tbody>
                    </table>
                </div>
                <div class="modal-footer">
                    <button type="button" class="btn btn-secondary" data-bs-dismiss="modal">關閉</button>
                    <button type="button" id="btnAdd" onclick="LoadingControl(0)" class="btn btn-success">新增</button>
                    <button type="button" id="btnEdit" onclick="LoadingControl(1)" class="btn btn-primary">修改</button>
                </div>
            </div>
        </div>
    </div>
    <!-- ====== 新增會員dialog ====== -->
    <!-- ====== 看照片dialog ====== -->
    <div class="modal fade" id="dialog2" data-bs-keyboard="false" data-bs-backdrop="static" style="overflow: auto">
        <div class="modal-dialog modal-lg">
            <div class="modal-content">
                <div class="modal-header">
                    <h2 class="modal-title">檢視照片</h2>
                    <button type="button" class="btn-close" data-bs-dismiss="modal" aria-label="Close"></button>
                </div>
                <div class="modal-body">
                    <div id="View_image" style="text-align:center"></div>
                </div>
                <div class="modal-footer">
                    <button type="button" class="btn btn-secondary" data-bs-dismiss="modal">關閉</button>
                </div>
            </div>
        </div>
    </div>
    <!-- ====== 看照片dialog ====== -->
    <!-- ====== 開啟相機dialog ====== -->
    <div class="modal fade" id="dialog3" data-bs-keyboard="false" data-bs-backdrop="static" style="overflow: auto">
        <div class="modal-dialog modal-lg">
            <div class="modal-content">
                <div class="modal-header">
                    <h2 class="modal-title">拍攝照片</h2>
                    <button type="button" class="btn-close" data-bs-dismiss="modal" aria-label="Close"></button>
                </div>
                <div class="modal-body">
                    <div style="text-align:center">
                        <video id="webcam" autoplay="autoplay" playsinline="" width="580" height="440"></video>     
                        <canvas id="canvas" style="display:none"></canvas>
                        <%--<canvas id="canvas" class="d-none"></canvas>
                        <audio id="snapSound" src="audio/snap.wav" preload = "auto"></audio>--%>
                    </div>
                    <div style="text-align:center">
                        <button type="button" style="width: 70px;
                                                                          height: 70px;
                                                                          padding: 10px 16px;
                                                                          font-size: 24px;
                                                                          line-height: 1.33;
                                                                          border-radius: 35px;"
                            id="photo" class="btn btn-info btn-xl"><i class="glyphicon glyphicon-camera"></i></button>
                    </div>
                </div>
                <div class="modal-footer">
                    <button type="button" class="btn btn-secondary" id="dialog3Close"  data-bs-dismiss="modal" onclick="webcam.stop()"><span class="glyphicon glyphicon-remove"></span>&nbsp;關閉</button>
                    <button type="button" id="restart_photo" class="btn btn-info" style="display:none"><span class="glyphicon glyphicon-refresh"></span>重新拍攝</button>
                    <button type="button" id="init_photo" class="btn btn-success" style="display:none"><span class="glyphicon glyphicon-ok"></span>使用照片</button>
                </div>
            </div>
        </div>
    </div>
    <!-- ====== 開啟相機dialog ====== -->
    <!-- ====== 看消費紀錄dialog ====== -->
    <div class="modal fade" id="myModal" data-bs-keyboard="false" data-bs-backdrop="static" style="overflow: auto">
        <div class="modal-dialog modal-lg">
            <div class="modal-content">
                <div class="modal-header">
                    <h2 class="modal-title">檢視明細</h2>
                    <button type="button" class="btn-close" data-bs-dismiss="modal" aria-label="Close"></button>
                </div>
                <div class="modal-body">
                    <div class="container-fluid">
                        <div class="row">
                            <div class="col-md-4">
                                <div class="card">
                                    <div class="card-header">會員姓名:</div>
                                    <div class="card-body">
                                        <h5 class="card-title">Special title treatment</h5>
                                    </div>
                                </div>
                            </div>
                            <div class="col-md-4">
                                <div class="card">
                                    <div class="card-header">收銀員代號:</div>
                                    <div class="card-body">
                                        <h5 class="card-title">Special title treatment</h5>
                                    </div>
                                </div>
                            </div>
                            <div class="col-md-4">
                                <div class="card">
                                    <div class="card-header">總金額:</div>
                                    <div class="card-body">
                                        <h5 class="card-title">Special title treatment</h5>
                                    </div>
                                </div>
                            </div>
                        </div>
                        <table id="table3" class="table table-striped" style="text-align:center;width:100%">
                            <thead>
                                <tr>
                                    <th style="text-align:center;">會員編號</th>
                                    <th style="text-align:center;">姓名</th>
                                    <th style="text-align:center;">身分證字號</th>
                                    <th style="text-align:center;">出生年月日</th>
                                    <th style="text-align:center;">性別</th>
                                    <th style="text-align:center;">連絡電話</th>
                                    <th style="text-align:center;">Email</th>
                                    <th style="text-align:center;">功能選單</th>
                                </tr>
                            </thead>
                        </table>
                    </div>
                </div>
            </div>
        </div>
    </div>
    <!-- ====== 看消費紀錄dialog ====== -->
    <script src="js/webcam.js"></script>
    <script src="js/bootstrap-datepicker.min.js"></script>
    <link href="css/bootstrap-datepicker3.min.css" rel="stylesheet" />
    <script src="js/notiflix.js"></script>
    <link href="js/notiflix.css" rel="stylesheet" />
    <style>
        body{
            font-size: 18px
        }
    </style>
    <script>
        var d = new Date()

        var b;
        var webcamElement = document.getElementById('webcam');
        var canvasElement = document.getElementById('canvas');
/*        canvasElement.toBlob(function (blob) { console.log(blob) }, "image/jpeg", 0.8)*/
        var webcam = new Webcam(webcamElement, 'user', canvasElement) 
        var picture;
        var photo_base64 = "";

        $(function () {
            Listener_event()
            bindtable()
            initHtml()
/*            test123()*/
        })

        function test123(){
            $.ajax({
                url: 'Member.aspx/test123',
                type: 'POST',
                data: JSON.stringify({
                }),
                contentType: 'application/json; charset=UTF-8',
                dataType: "json",       //如果要回傳值，請設成 json
                success: function (doc) {
                    status = doc.d
                    console.log(status)
                }
            });
        }

        function Listener_event() {
            $('#file').change(function () {
                $.map($(this).get(0).files, function (file, index) {
                    var img_size = Math.floor(file.size / 1024)
                    var type = file.type.split('/')
                    type = type[0]
                    if (type != 'image') {
                        Notiflix.Report.Warning('請上傳圖片類型', '', '確定');
                        $('#file').val('')
                        $('#View_image').html('')
                        return false;
                    }
                    if (file) {
                        $('#View_image').html('')
                        var reader = new FileReader();
                        reader.readAsDataURL(file);
                        reader.onload = function (e) {
                            var urlData = reader.result;
                            var base64 = urlData.split(',')[1]
                            $('#showImage').show()
                            $('#View_image').append('<img src="' + urlData + '" width=300 heigth:200></img>')
                        };
                    }
                });
            })
            $('#Sex_Man').click(function () {
                if ($('#Sex_Lady').is(':checked')) {
                    $('#Sex_Lady').removeAttr("checked")
                }
            })
            $('#Sex_Lady').click(function () {
                if ($('#Sex_Man').is(':checked')) {
                    $('#Sex_Man').removeAttr("checked")
                }
            })

            //按下拍照
            $('#photo').click(function () {
                picture = webcam.snap()

                photo_base64 = picture.split(",")[1]; //取的base64字串
                webcam.stop()
                $('#restart_photo').show()
                $('#init_photo').show()
                $('#webcam').hide()
                $('#canvas').show()
            })

            //按下重新拍攝
            $('#restart_photo').click(function () {
                webcam.start()
                $('#webcam').show()
                $('#canvas').hide()

                $('#restart_photo').hide()
                $('#init_photo').hide()
            })

            $('#init_photo').click(function () {
                if (photo_base64 == "") {
                    Notiflix.Report.Warning('未偵測到照片', '', '確定');
                    return false
                } else {
                    $('#dialog3Close').click()
                    $('#showImage').show()
                    $('#View_image').html('')
                    $('#View_image').append('<img src="' + picture + '" id="SnapImage" width=300 heigth:200></img>')

                    //let file = null;
                    //let img = $('#SnapImage').val()
                    //for (let i = 0; i < img.length; i++) {
                    //    if (img[i].type.match(/^image\//)) {
                    //        file = picture[i];
                    //        break;
                    //    }
                    //}
                    canvasElement.toBlob(function (blob) {
                        let file = new File([blob], "CameraImage.jpeg", { type: "image/jpeg", lastModified: new Date().getTime() }, 'utf-8');
                        let container = new DataTransfer();
                        container.items.add(file);
                        document.querySelector('#file').files = container.files
                        console.log(blob);
                    }, "image/jpeg", 0.8)
                }

            })
        }

        function doSomethingWithFiles(fileList) {
            let file = null;

            for (let i = 0; i < fileList.length; i++) {
                if (fileList[i].type.match(/^image\//)) {
                    file = fileList[i];
                    break;
                }
            }

            if (file !== null) {
                output.src = URL.createObjectURL(file);
            }
        }

        function bindtable() {
            $.ajax({
                type: 'post',
                url: 'Member.aspx/bindtable',
                contentType: 'application/json;utf-8',
                data: JSON.stringify({}),
                dataType: 'json',
                success: function (doc) {
                    var full_data = eval(doc.d)
                    console.log(full_data)
                    $('#table1').DataTable({
                        responsive: true,
                        destroy: true,
                        data: full_data,
                        "oLanguage": {
                            "sLengthMenu": "顯示 _MENU_ 筆",
                            "sZeroRecords": "無資料",
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
                            { "data": "MemberNum" },
                            { "data": "MemberName" },
                            { "data": "MemberID" },
                            { "data": "MemberDate" },
                            { "data": "Sex" },
                            { "data": "MemberPhone" },
                            { "data": "MemberEmail" },
                            {
                                "data": "SYSID",
                                "render": function (data, type, full, meta) {
                                    var bt = '<button id="view_' + data + '" type="button" class="btn btn-outline-info btn-lg" ><span class="glyphicon glyphicon-search">查詢</span></button>';
                                    bt += ' <button id="edit_' + data + '" type="button" class="btn btn-outline-primary btn-lg" data-bs-toggle="modal" data-bs-target="#dialog1"><span class="glyphicon glyphicon-pencil">編輯</span></button>';
                                    bt += ' <button id="delete_' + data + '" type="button" class="btn btn-outline-danger btn-lg" ><span class="glyphicon glyphicon-remove">刪除</span></button>';
                                    return bt;
                                }
                            },
                        ]
                    });
                    $('#table1 tbody').unbind('click').
                        on('click', 'button[id^=view_]', function () {
                            var SYSID = $(this).attr('id').split('_')[1]
                            $.each(full_data, function (index, obj) {
                                var ID = obj.SYSID
                                if (SYSID == ID) {
                                    initCard(full_data[index])
                                }
                            })
                        }).on('click', 'button[id^=edit_]', function () {
                            var SYSID = $(this).attr('id').split('_')[1]
                            $.each(full_data, function (index, obj) {
                                var ID = obj.SYSID
                                if (SYSID == ID) {
                                    initdialog(full_data[index])
                                }
                            })
                        }).on('click', 'button[id^=delete_]', function () {
                            var SYSID = $(this).attr('id').split('_')[1]
                            var MemberNum;
                            $.each(full_data, function (index, obj) {
                                var ID = obj.SYSID
                                if (SYSID == ID) {
                                    MemberNum = full_data[index].MemberNum
                                }
                            })
                            if (confirm('確定要刪除此會員嗎?')) {
                                DeleteStatus(SYSID, MemberNum)
                            }
                        })

                }
            });
        }

        function MemberHistory(MemberNum) {
            $.ajax({
                type: 'post',
                url: 'Member.aspx/MemberHistory',
                contentType: 'application/json;utf-8',
                data: JSON.stringify({ MemberNum: MemberNum}),
                dataType: 'json',
                success: function (doc) {
                    var full_data = eval(doc.d)
                    console.log(full_data)
                    $('#table2').dataTable({
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
                            {
                                "data": "T_DAY",
                                "render": function (data, type, full, meta) {
                                    if (full.Date == null) {
                                        return data
                                    }
                                    else {
                                        return full.Date
                                    }
                                }
                            },
                            {
                                "data": "Total",
                                "render": function (data, type, full, meta) {
                                    if (data == null) {
                                        return "未消費"
                                    }
                                    else {
                                        return data;
                                    }
                                }
                            },
                            {
                                "data": "T_DAY",
                                "render": function (data, type, full, meta) {
                                    if (data == null) {
                                        return ""
                                    }
                                    else {
                                        var bt = '<button id="list_' + data+'" type="button" class="btn btn-info btn-lg" data-bs-toggle="modal" data-bs-target="#myModal"><span class="glyphicon glyphicon-search">檢視明細</span></button>';
                                    } 
                                    return bt;
                                }
                            }
                        ]
                    });
                    $('#table2 tbody').unbind('click').
                        on('click', 'button[id^=list]', function () {
                            var MemberNum = $(this).attr('id').split('_')[1]
                            /*BuyList(full_data)*/
                            //$.each(full_data, function (index, obj) {
                            //    var ID = obj.SYSID
                            //    if (SYSID == ID) {
                            //        initCard(full_data[index])
                            //    }
                            //})
                        })
                }
            });
        }

        function BuyList(data) {

        }

        function initHtml() {
            var year = d.getFullYear()
            var month = d.getMonth()
            var day = d.getDate()
            //var hours = d.getHours().toString()
            //var min = d.getMinutes().toString()
            //var sec = d.getSeconds().toString()
            /*var num = year + month + day + hours + min + sec*/
            var monthAgo = new Date(year, (month - 1), day)

            randomWord()

            $('#member').css({ "background-color": "#1a9bfc", "color": "white" })

            $('#MemberDate').datepicker({
                autoclose: true,
                format: 'yyyy/mm/dd',
            })

            $('#startDate').datepicker({
                autoclose: true,
                format: 'yyyy/mm/dd',
            }).datepicker("setDate", monthAgo )

            $('#endDate').datepicker({
                autoclose: true,
                format: 'yyyy/mm/dd',
            }).datepicker("setDate", 'now')

            $('#search').click(function () {
                let startDate = $('#startDate').val()
                let endDate = $('#endDate').val()
                if (Date.parse(startDate).valueOf() > Date.parse(endDate).valueOf()) {
                    Notiflix.Report.Warning('起始時間大於結束時間，請重新選擇', '', '確定');
                }
            })

            $('#btnAdd').show()
            $('#btnEdit').hide()
        }

        function Clear() {
            randomWord();
            $('#btnAdd').show()
            $('#btnEdit').hide()
            $('#showImage').hide()
            $('#MemberNum').val('')
            $('#MemberName').val('')
            $('#MemberID').val('')
            $('#MemberDate').val('')
            $('#Sex_Man').removeAttr("checked")
            $('#Sex_Lady').removeAttr("checked")

            $('#MemberPhone').val('')
            $('#MemberEmail').val('')
            $('#file').val('')

            $('#MemberID').attr('disabled', false)
            $('#View_image').html('')
        }

        function initdialog(data) {
            $('#btnAdd').show()
            $('#btnEdit').hide()
            $('#MemberNum').val('')
            $('#MemberName').val('')
            $('#MemberID').val('')
            $('#MemberDate').val('')
            $('#Sex_Man').removeAttr("checked")
            $('#Sex_Lady').removeAttr("checked")

            $('#MemberPhone').val('')
            $('#MemberEmail').val('')
            $('#file').attr('disabled', false)
            $('#View_image').html('')

            $('#dialogtitle').text('編輯會員資料')
            $('#btnAdd').hide()
            $('#btnEdit').show()
            $('#MemberNum').val(data.MemberNum)
            $('#MemberName').val(data.MemberName)
            $('#MemberID').val(data.MemberID)
            $('#MemberDate').val(data.MemberDate)
            var sex = data.Sex
            if (sex == "Male") {
                $('#Sex_Man').prop("checked", true)
            } else {
                $('#Sex_Lady').prop("checked", true)
            }
            $('#MemberPhone').val(data.MemberPhone)
            $('#MemberEmail').val(data.MemberEmail)

            $('#MemberID').attr('disabled',true)
            /*$('#file').attr('disabled', true)*/

            $('#showImage').show()
            $('#View_image').append('<img src="' + data.ImageURL + '" width=300 heigth:200></img>')
        }

        function initCard(data) {
            MemberHistory(data.MemberNum)

            $('#view1').show()
            /*$('#view2').show()*/
            $('#h2_back').show()
            $('#table_view').hide()

            $('#back').click(function () {
                $('#view1').hide()
                /*$('#view2').hide()*/
                $('#h2_back').hide()
                $('#table_view').show()
            })
            $('#image').attr('src', data.ImageURL)
            $('#CardNum').text(data.MemberNum)
            $('#CardName').text(data.MemberName)
            $('#CardID').text(data.MemberID)
            $('#CardBirthday').text(data.MemberDate)
            $('#CardSex').text(data.Sex)
            $('#CardEmail').text(data.MemberEmail)
        }

        function randomWord() {
            let characters = 'ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789'
            let result1 = ' '
            for (let i = 0; i < 10; i++) {
                result1 += characters.charAt(Math.floor(Math.random() * characters.length));
            }

            $.ajax({
                url: 'Member.aspx/randomWord',
                type: 'POST',
                data: JSON.stringify({
                    result1: result1
                }),
                contentType: 'application/json; charset=UTF-8',
                dataType: "json",       //如果要回傳值，請設成 json
                success: function (doc) {
                    var status = doc.d
                    if (status == "ok") {
                        $('#MemberNum').val(result1)
                    }
                    else {
                        randomWord()
                    }
                }
            });
        }

        function LoadingControl(index) {
            Notiflix.Loading.Dots('loading...');
            if (index == 0) {
                var status = AddMember(0)
                if (status == false) {
                    Notiflix.Loading.Remove(100);
                }
            } else {
                var status = AddMember(1)
                if (status == false) {
                    Notiflix.Loading.Remove(100);
                }
            }
            
        }

        function AddMember(type) {
            var MemberNum = $('#MemberNum').val().replace(/\s+/g, '')
            var MemberName = $('#MemberName').val().replace(/\s+/g, '')
            var MemberID = $('#MemberID').val().replace(/\s+/g, '')
            var MemberDate = $('#MemberDate').val().replace(/\s+/g, '')
            var Sex
            
            
            var MemberPhone = $('#MemberPhone').val().replace(/\s+/g, '')
            var MemberEmail = $('#MemberEmail').val().replace(/\s+/g, '')
            var filelist = $('#file').get(0).files[0]

            if (MemberName == '') {
                Notiflix.Report.Warning('請輸入會員姓名', '', '確定');
                return false
            }

            if (MemberID.length != 10 || /[a-z]/i.test(MemberID) == false) {
                Notiflix.Report.Warning('身分證錯誤，請輸入正確身分證字號', '', '確定');
                return false;
            }

            if (MemberDate == '') {
                Notiflix.Report.Warning('請選擇出生年月日', '', '確定');
                return false
            }

            if ($('#Sex_Man').is(':checked')) {
                Sex = 'Male'
            }
            else {
                if ($('#Sex_Lady').is(':checked')) {
                    Sex = 'Female'
                }
                else {
                    Notiflix.Report.Warning('請選擇性別', '', '確定');
                    return false
                }
            }

            if (MemberPhone.length != 10) {
                Notiflix.Report.Warning('行動電話錯誤，請輸入正確行動電話', '', '確定');
                return false
            }

            if (MemberEmail.indexOf('@') == -1) {
                Notiflix.Report.Warning('信箱錯誤，請輸入正確信箱', '', '確定');
                return false
            }

            if (filelist) {
                var SizeKB = filelist.size / 1024
                var reader = new FileReader();
                reader.readAsDataURL(filelist);
                reader.onload = function (e) {
                    var urlData = reader.result;
                    var file_base64 = urlData.split(',')[1]

                    $.ajax({
                        url: 'Member.aspx/Add_Member',
                        type: 'POST',
                        async: false,
                        data: JSON.stringify({
                            type: type,
                            MemberNum: MemberNum,
                            MemberName: MemberName,
                            MemberID: MemberID,
                            MemberDate: MemberDate,
                            Sex: Sex,
                            MemberPhone: MemberPhone,
                            MemberEmail: MemberEmail,
                            SizeKB: SizeKB,
                            file_base64: file_base64,
                        }),
                        contentType: 'application/json; charset=UTF-8',
                        dataType: "json",       //如果要回傳值，請設成 json
                        success: function (doc) {
                            var data = doc.d
                            if (data == "fail") {
                                Notiflix.Report.Warning('此身分證已經是會員', '', '確定');
                            }
                            else {
                                if (type == 0) {
                                    var img_url = doc.d
                                    var CreateMember = FaceControl(0, MemberName, img_url, MemberNum)
                                    if (CreateMember == 'ok') {
                                        var CreateFace = FaceControl(1, MemberName, img_url, MemberNum)
                                        if (CreateFace == 'ok') {
                                            Notiflix.Loading.Remove(1923);
                                            bindtable()
                                            Notiflix.Report.Success('會員新增成功', '', '完成');
                                        } else {
                                            Notiflix.Loading.Remove(1923);
                                            Notiflix.Report.Warning('人臉辨識建立失敗，請聯絡管理員', '', '確定');
                                        }
                                    } else {
                                        Notiflix.Loading.Remove(1923);
                                        Notiflix.Report.Warning('會員建立失敗，請聯絡管理員', '', '確定');
                                    }
                                }
                                else {
                                    var img_url = doc.d
                                    var CreateFace = FaceControl(1, MemberName, img_url, MemberNum)
                                    if (CreateFace == 'ok') {
                                        Notiflix.Loading.Remove(1923);
                                        bindtable()
                                        Notiflix.Report.Success('會員修改成功', '', '完成');
                                    }
                                }
                            }
                        }
                    });
                };
            }
            else {
                Notiflix.Report.Warning('請拍攝照片或選擇照片', '', '確定');
                return false
            }
        }

        function EditMember() {
            var MemberNum = $('#MemberNum').val().replace(/\s+/g, '')
            var MemberName = $('#MemberName').val().replace(/\s+/g, '')
            var MemberID = $('#MemberID').val().replace(/\s+/g, '')
            var MemberDate = $('#MemberDate').val().replace(/\s+/g, '')
            var Sex

            var MemberPhone = $('#MemberPhone').val().replace(/\s+/g, '')
            var MemberEmail = $('#MemberEmail').val().replace(/\s+/g, '')

            if (MemberName == '') {
                return false
            }

            if (MemberID.length != 10 || /[a-z]/i.test(MemberID) == false) {
                alert('身分證錯誤，請輸入正確身分證字號。')
                return false;
            }

            if (MemberDate == '') {
                alert('請選擇出生年月日。')
                return false
            }

            if ($('#Sex_Man').is(':checked')) {
                Sex = 'Male'
            }
            else {
                if ($('#Sex_Lady').is(':checked')) {
                    Sex = 'Female'
                }
                else {
                    alert('請選擇性別。')
                    return false
                }
            }

            if (MemberPhone.length != 10) {
                alert('行動電話錯誤，請輸入正確行動電話。')
                return false
            }

            if (MemberEmail.indexOf('@') == -1) {
                alert('信箱錯誤，請輸入正確信箱。')
                return false
            }

            $('#View_image').html('')
            $.ajax({
                url: 'Member.aspx/EditMember',
                type: 'POST',
                async: false,
                data: JSON.stringify({
                    MemberNum: MemberNum,
                    MemberName: MemberName,
                    MemberID: MemberID,
                    MemberDate: MemberDate,
                    Sex: Sex,
                    MemberPhone: MemberPhone,
                    MemberEmail: MemberEmail,
                }),
                contentType: 'application/json; charset=UTF-8',
                dataType: "json",       //如果要回傳值，請設成 json
                success: function (doc) {
                    var status = doc.d
                    if (status == 'ok') {
                        var EditFace = FaceControl(2, MemberName, '', MemberNum)
                        if (EditFace == 'ok') {
                            bindtable()
                            alert('會員修改成功。')
                            Notiflix.Loading.Remove(100);
                        } else {
                            Notiflix.Loading.Remove(100);
                            alert('人臉辨識修改失敗，請聯絡管理員。')
                        }
                    } else {
                        alert('伺服器發生錯誤，請重新嘗試或聯絡管理員')
                        Notiflix.Loading.Remove(100);
                    }
                }
            });
        }

        function DeleteStatus(SYSID, MemberNum) {
            Notiflix.Loading.Dots('loading...');
            $.ajax({
                url: 'Member.aspx/DeleteStatus',
                type: 'POST',
                async: false,
                data: JSON.stringify({
                    SYSID: SYSID,
                }),
                contentType: 'application/json; charset=UTF-8',
                dataType: "json",       //如果要回傳值，請設成 json
                success: function (doc) {
                    var status = doc.d
                    if (status == 'ok') {
                        var EditFace = FaceControl(3, '', '', MemberNum)
                        if (EditFace == 'ok') {
                            bindtable()
                            Notiflix.Report.Success('會員刪除成功', '', '完成');
                            Notiflix.Loading.Remove(100);
                        } else {
                            Notiflix.Loading.Remove(100);
                            alert('人臉辨識刪除失敗，請聯絡管理員。')
                        }
                    } else {
                        alert('伺服器發生錯誤，請重新嘗試或聯絡管理員')
                        Notiflix.Loading.Remove(100);
                    }
                }
            });
        }

        function FaceControl(type, MemberName, img_url, MemberNum) {
            console.log(MemberNum, MemberName)
            var data;
            var status;
            switch (type) {
                case 0:
                    data = {
                        'UserInfo': {
                            'employeeNo': MemberNum,
                            'name': MemberName,
                            'userType': 'normal',
                            'Valid': {
                                'enable': true,
                                'beginTime': '2021-10-08T00:00:00',
                                'endTime': '2037-12-31T23:59:59',
                                'timeType': 'local',
                            },
                            'doorRight': '1',
                            'RightPlan': [{
                                'doorNo': 1,
                                'planTemplateNo': '1'
                            }],
                        }
                    }
                    break
                case 1:
                    data = {
                        'faceURL': img_url,
                        'faceLibType': 'blackFD',
                        'FDID': '1',
                        'FPID': MemberNum
                    }
                    break
                case 2:
                    data = {
                        'UserInfo': {
                            'employeeNo': MemberNum,
                            'name': MemberName,
                            'userType': 'normal',
                            'Valid': {
                                'enable': true,
                                'beginTime': '2021-10-08T00:00:00',
                                'endTime': '2037-12-31T23:59:59',
                                'timeType': 'local',
                            },
                            'doorRight': '1',
                            'RightPlan': [{
                                'doorNo': 1,
                                'planTemplateNo': '1'
                            }],
                        }
                    }
                    break
                case 3:
                    data = {
                        'UserInfoDelCond': {
                            'EmployeeNoList': [{ 'employeeNo': MemberNum }],
                            'operateType': 'byTerminal',
                            'terminalNoList': [1]
                        }
                    }
                    break
            }
            

            $.ajax({
                url: 'Member.aspx/FaceControl',
                type: 'POST',
                async: false,
                data: JSON.stringify({
                    data: JSON.stringify(data),
                    type: type
                }),
                contentType: 'application/json; charset=UTF-8',
                dataType: "json",       //如果要回傳值，請設成 json
                success: function (doc) {
                    status = doc.d
                    
                }
            });
            return status
        }

        function Start_Camera() {
            photo_base64 = ""
            var myModal = new bootstrap.Modal(document.getElementById('dialog3'))
            myModal.show()
            $('#webcam').show()
            $('#canvas').hide()

            $('#restart_photo').hide()
            $('#init_photo').hide()

            $('#View_image').html('')
            
            var context = canvas.getContext('2d');
            context.clearRect(0, 0, canvasElement.width, canvasElement.height);


            webcam.start()
                .then(result => {
                    console.log("webcam started");
                })
                .catch(err => {
                    Notiflix.Report.Success('相機開啟失敗，請確認是否連接USB相機', '', '確定');
                    console.log(err);
                });
        }
    </script>
</asp:Content>


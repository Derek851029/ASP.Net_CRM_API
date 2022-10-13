<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="CommodityList.aspx.cs" Inherits="CommodityList" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="head2" Runat="Server">
    <link href="css/bootstrap-datepicker3.min.css" rel="stylesheet" />
    <style>
        body{
            font-size:18px
        }
        .content-list{
            margin-top:20px;
            margin-bottom: 100px;
            padding: 10px;
        }
    </style>

    <div>
        <div class="title">
            <nav aria-label="breadcrumb">
                <ol class="breadcrumb mb-0 d-flex align-items-center">
                    <li class="breadcrumb-item">
                        <a href="CommodityList.aspx" class="link">
                            <i class="bi-house"></i>
                        </a>
                    </li>
                    <li class="breadcrumb-item active" aria-current="page">商品管理</li>
                </ol>
                </nav>
            <h1 class="mb-0 fw-bold">商品資訊
                <button type="button" id="addbt"  class="btn btn-success btn-lg" data-bs-toggle="modal" data-bs-target="#dialog1"><span class="glyphicon glyphicon-plus">新增商品</span></button>
            </h1>
        </div>
        <div class="content-list">
            <table id="datalist" class="table table-striped" style="text-align:center;width:100%">
                <thead>
                    <tr>
                        <th style="text-align:center;">產品名</th>
                        <th style="text-align:center;">原產地</th>
                        <th style="text-align:center;">數量</th>
                        <th style="text-align:center;">有效日期</th>
                        <th style="text-align:center;">進貨日期</th>
                        <th style="text-align:center;">條碼</th>
                        <th style="text-align:center;">功能選單</th>
                    </tr>
                </thead>
            </table>
        </div>
    </div>
    
    <!-- ====== 新增會員dialog ====== -->
    <div class="modal fade" id="dialog1" data-bs-keyboard="false" data-bs-backdrop="static" style="overflow: auto">
        <div class="modal-dialog modal-lg">
            <div class="modal-content">
                <div class="modal-header">
                    <h2 class="modal-title">新增商品資料</h2>
                    <button type="button" class="btn-close" data-bs-dismiss="modal" aria-label="Close"></button>
                </div>
                <div class="modal-body">
                    <input type="hidden" id="CDID" data-add="c"/>
                    <table class="table table-bordered">
                        <thead class="table-dark">
                            <tr>
                                <th style="text-align:center;" colspan="4">
                                    <span><strong>商品資料</strong></span>
                                </th>
                            </tr>
                        </thead>
                        <tbody>
                            <tr>
                                <th><strong>產品名</strong></th>
                                <th>
                                    <input type="text" id="ProductName" data-add="c" class="form-control" placeholder="請輸入產品名" required="required"/>
                                </th>
                            </tr>
                            <tr>
                                <th><strong>成分</strong></th>
                                <th>
                                    <textarea id="Ingredient" data-add="c" class="form-control" style="width:100%;height:200px;resize:none;" placeholder="請輸入成分" required="required"></textarea>
                                </th>
                            </tr>
                            <tr>
                                <th><strong>原產地</strong></th>
                                <th>
                                    <input type="text" id="PlaceOfOrigin" data-add="c" class="form-control" placeholder="請輸入原產地" required="required" />
                                </th>
                            </tr>
                            <tr>
                                <th><strong>數量</strong></th>
                                <th>
                                    <input type="number" id="Number" data-add="c" class="form-control" placeholder="請輸入/選擇數量" required="required"/>
                                </th>
                            </tr>
                            <tr>
                                <th><strong>位置</strong></th>
                                <th>
                                    
                                </th>
                            </tr>
                            <tr>
                                <th><strong>有效日期</strong></th>
                                <th>
                                    <input type="text" id="EffectDate" data-add="c" class="form-control"  placeholder="請選擇有效日期" required="required"/>
                                </th>
                            </tr>
                            <tr>
                                <th><strong>進貨日期</strong></th>
                                <th>
                                    <input type="text" id="PurchaseDate" data-add="c" class="form-control" placeholder="請選擇進貨日期" required="required"/>
                                </th>
                            </tr>
                            <tr>
                                <th><strong>條碼</strong></th>
                                <th>
                                    <input type="text" id="Barcode" data-add="c" class="form-control" placeholder="請輸入條碼" required="required"/>
                                </th>
                            </tr>
                            <tr>
                                <th><strong>產品圖</strong></th>
                                <th>
                                    <input type="file" id="ImageFile" class="form-control"/>
                                    <button type="button" id="showImage" data-bs-toggle="modal" data-bs-target="#dialog2" onclick="" class="btn btn-outline-primary" style="display:none"><span class="glyphicon glyphicon-search">檢視照片</span></button>
                                </th>
                            </tr>
                            <tr>
                                <th><strong>產品影片</strong></th>
                                <th>
                                    <input type="file" id="VideoFile" class="form-control"/>
                                    <button type="button" id="showVideo" data-bs-toggle="modal" data-bs-target="#dialog2" onclick="" class="btn btn-outline-primary" style="display:none"><span class="glyphicon glyphicon-search">檢視影片</span></button>
                                </th>
                            </tr>
                        </tbody>
                    </table>
                </div>
                <div class="modal-footer">
                    <button type="button" id="cancelbt" class="btn btn-secondary" data-bs-dismiss="modal">關閉</button>
                    <button type="button" id="editbt" onclick="" class="btn btn-success">新增</button>
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
                    <h2 class="modal-title" id="Viewtitle">檢視照片</h2>
                    <button type="button" class="btn-close" data-bs-dismiss="modal" aria-label="Close"></button>
                </div>
                <div class="modal-body">
                    <div id="View_image" style="text-align:center;display:none">
                    </div>
                    <div id="View_video" style="text-align:center;display:none"></div>
                </div>
                <div class="modal-footer">
                    <button type="button" class="btn btn-secondary" data-bs-dismiss="modal">關閉</button>
                </div>
            </div>
        </div>
    </div>
    <!-- ====== 看照片dialog ====== -->

    <script src="js/moment.min.js"></script>
    <script src="js/bootstrap-datepicker.min.js"></script>
    <script>

        var ImageFile = "";

        $(function () {
            $('#Commodity').css({ "background-color": "#1a9bfc", "color": "white" })

            $('#EffectDate,#PurchaseDate').datepicker({
                format: 'yyyy/mm/dd',
            });
            $('#addbt').click(function () {
                $('[data-add="c"]').each(function () { $(this).val(''); });
                $('#editbt').attr('onclick', "EditList('add');");
                $('#editbt').attr('class', "btn btn-success");
                $('#editbt').text('新增');
            });

            $('#ImageFile').change(function () {
                $.map($(this).get(0).files, function (file, index) {
                    var img_size = Math.floor(file.size / 1024)
                    var type = file.type.split('/')
                    type = type[0]
                    if (type != 'image') {
                        $('#ImageFile').val('')
                        alert("請上傳圖片類型。");
                        return false;
                    }
                    else {
                        var reader = new FileReader();
                        reader.readAsDataURL(file);
                        reader.onload = function (e) {
                            var urlData = reader.result;
                            ImageFile = urlData.split(',')[1]
                            $('#showImage').show()
                            $('#View_image').html('')
                            $('#View_image').append("<img src='" + urlData + "' id='Image' width='300' heigth='200'></img>")
                        }
                    }
                });
            })

            $('#VideoFile').change(function () {
                $.map($(this).get(0).files, function (file, index) {
                    var img_size = Math.floor(file.size / 1024)
                    var type = file.type.split('/')
                    type = type[0]
                    console.log(type)
                    if (type != 'video') {
                        $('#VideoFile').val('')
                        alert("請上傳影片類型。");
                        return
                    } else {
                        var reader = new FileReader();
                        reader.readAsDataURL(file);
                        reader.onload = function (e) {
                            var urlData = reader.result;
                            $('#showVideo').show()
                            $('#View_video').html('')
                            $('#View_video').append("<video width='640' heigth='480' src='" + urlData + "' controls></video>")
                        }
                    }
                });
            })

            $('#showImage').click(function () {
                $('#View_video').hide()
                $('#View_image').show()
                $('#Viewtitle').text('檢視照片')
            })

            $('#showVideo').click(function () {
                $('#View_video').show()
                $('#View_image').hide()
                $('#Viewtitle').text('檢視影片')
            })
            bindlist();
        });

        function bindlist() {
            $.ajax({
                type: 'post',
                url: 'CommodityList.aspx/bindlist',
                contentType: 'application/json;utf-8',
                data: JSON.stringify({}),
                dataType: 'json',
                success: function (doc) {

                    $('#datalist').dataTable({
                        responsive: true,
                        destroy: true,
                        data: eval(doc.d),
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
                        "aaSorting": [[4, 'asc']],
                        "aLengthMenu": [[10,50], [10,50]],
                        "iDisplayLength": 10,
                        "bAutoWidth": false,
                        "columnDefs": [{
                            "targets": -1,
                            "data": null,
                            "searchable": false,
                            "paging": false,
                            "ordering": false,
                            "info": false,
                        }],
                        columns: [
                            { "data": "ProductName" },
                            { "data": "PlaceOfOrigin" },
                            { "data": "Number" },
                            {
                                "data": "EffectDate",
                                "render": function (data, type, full, meta) {
                                    return moment(data).format('YYYY-MM-DD');
                                }
                            },
                            {
                                "data": "PurchaseDate",
                                "render": function (data, type, full, meta) {
                                    return moment(data).format('YYYY-MM-DD');
                                }
                             },
                            { "data": "Barcode" },
                            {
                                "data": "CDID",
                                "render": function (data, type, full, meta) {
                                    let bt = '<button id="update" type="button" class="btn btn-outline-primary btn-lg" data-bs-toggle="modal" data-bs-target="#dialog1" ><span class="glyphicon glyphicon-pencil">修改</span></button>';
                                    bt += '  <button id="delete" type="button" class="btn btn-outline-danger btn-lg" ><span class="glyphicon glyphicon-remove">刪除</span></button>';
                                    return bt;
                                }
                            }
                        ]
                    });

                    $('#datalist tbody').unbind('click')
                        .on('click', '#update', function () {
                            let table = $('#datalist').DataTable();
                            let data = table.row($(this).parents('tr')).data();
                            $('[data-add="c"]').each(function () {
                                let $thisID = $(this).attr('id');
                                let v = eval('data.' + $thisID);
                                if ($thisID == 'EffectDate' || $thisID == 'PurchaseDate') {
                                    if (!(v == null || '')) {
                                        v = moment(v).format('YYYY-MM-DD');
                                    }
                                }
                                $(this).val(v);
                            });
                            DataTableEdit(data)
                            console.log(data)
                        })
                        .on('click', '#delete', function () {
                            if (confirm('確定要刪除這個商品嗎?')) {
                                let table = $('#datalist').DataTable();
                                let data = table.row($(this).parents('tr')).data();
                                $('[data-add="c"]').each(function () {
                                    let v = eval('data.' + $(this).attr('id'));
                                    $(this).val(v);
                                });
                                EditList('delete');
                            }
                        });
                },
                error: function () {
                    console.log('bind list error');
                }
            });
        }

        function DataTableEdit(data) {
            let date = new Date();
            let stringDate = date.toISOString().split('T')[0]

            let imgURL = data.ImageURL + "?v=" + stringDate
            let vidURL = data.VideoURL + "?v=" + stringDate
            $('#View_video').html('');
            $('#View_image').html('');
            if (data.ImageURL != null) {
                $('#showImage').show()
                $('#Image').attr('scr', data.ImageURL)
                $('#View_image').append("<img src='" + imURL + "' id='Image' width='300' heigth='200'></img>")
            }
            else {
                $('#showImage').hide()
            }
            if (data.VideoURL != null) {
                $('#showVideo').show()
                $('#View_video').append("<video width='640' heigth='480' src='" + vidURL + "' controls></video>")
                /*$('#Video').attr('scr', data.VideoURL)*/
            }
            else {
                $('#showImage').hide()
            }
            $('#editbt').attr('onclick', "EditList('update');");
            $('#editbt').attr('class', 'btn btn-primary')
            $('#editbt').text('修改')
        }

        function EditList(type) {
            let data = {};
            $('[data-add="c"]').each(function () {
                data[$(this).attr('id')] = $(this).val();
            });
            if (data.ProductName == '') {
                alert('請輸入產品名。')
                return
            }if (data.Ingredient == '') {
                alert('請輸入成分。')
                return
            } if (data.PlaceOfOrigin == '') {
                alert('請輸入原產地。')
                return
            } if (data.Number == '') {
                alert('請輸入數量。')
                return
            } if (data.EffectDate == '') {
                alert('請選擇有效日期。')
                return
            } if (data.PurchaseDate == '') {
                alert('請選擇進貨日期。')
                return
            } if (data.Barcode == '') {
                alert('請輸入條碼。')
                return
            }

            Notiflix.Loading.Dots('loading...');

            $.ajax({
                type: 'post',
                url: 'CommodityList.aspx/EditList',
                contentType: 'application/json;utf-8',
                async:false,
                data: JSON.stringify({
                    type: type,
                    data: data
                }),
                dataType: 'json',
                success: function (doc) {
                    var status = doc.d
                    if (status == 'Success') {
                        bindlist();
                        var save = SaveFile(data.Barcode)
                        console.log(save)
                        if (save == 'Success') {
                            var upload = UploadVideo(data.Barcode)
                            if (upload == 'Success') {
                                if (type == 'add') {
                                    alert("產品新增成功。");
                                }
                                else {
                                    alert("產品修改成功。");
                                }
                                
                                $('#cancelbt').click();
                                Notiflix.Loading.Remove(300);
                            }
                            else {
                                Notiflix.Loading.Remove(100);
                                alert('上傳影片發生錯誤，請聯絡管理員。');
                            }
                        }
                        else {
                            Notiflix.Loading.Remove(100);
                            alert('上傳照片發生錯誤，請聯絡管理員。');
                            /*Notiflix.Loading.Remove(300);*/
                        }
                    }
                    else {
                        alert('產品新增失敗，請重新嘗試或聯絡管理員。');
                        Notiflix.Loading.Remove(100);
                        /*Notiflix.Loading.Remove(300);*/
                    }
                },
                error: function () {
                    alert('伺服器發生錯誤，請重新嘗試或聯絡管理員');
                    /*Notiflix.Loading.Remove(300);*/
                }
            })
        }

        function SaveFile(Barcode) {
            if (ImageFile == "") {
                return 'Success';
            }
            var status;
            $.ajax({
                type: 'post',
                url: 'CommodityList.aspx/SaveFile',
                contentType: 'application/json;utf-8',
                async:false,
                data: JSON.stringify({
                    ImageFile: ImageFile,
                    Barcode: Barcode,
                }),
                dataType: 'json',
                success: function (doc) {
                    status = doc.d
                    
                },
                error: function (e) {
                    console.log(e)
                }
            })

            if (status == 'Success') {
                return 'Success'
            } else {
                return 'fail'
            }
        }

        function UploadVideo(Barcode) {
            let status;

            if ($('#VideoFile').val() == '') {
                Notiflix.Loading.Remove(300);
                return "Success";
            }
            let file = $('#VideoFile')[0].files[0];
            let size = file.size;
            let name = file.name;
            name = Barcode + ".mp4"
            let extindex = name.lastIndexOf('.');
            let ext = name.substr(extindex + 1, name.length);

            let cutsize = 2 * 1024 * 1024;
            let cutcount = Math.ceil(size / cutsize);
            console.log('start upload');
            let data = { Status: '', FilePath: '' };
            for (let i = 0; i < cutcount; i++) {
                let start = i * cutsize;
                let end = Math.min(size, start + cutsize);

                let form = new FormData();
                form.append('type', 'video');
                form.append('Barcode', Barcode)
                form.append('name', name);
                form.append('ext', ext);
                form.append('index', i + 1);
                form.append('total', cutcount);
                form.append('file', file.slice(start, end));

/*                Notiflix.Loading.Standard('UpLoading...' + Math.ceil((i / cutcount) * 100) + '%');*/

                $.ajax({
                    type: 'post',
                    url: 'UploadFile.ashx',
                    data: form,
                    async: false,
                    contentType: false,
                    processData: false,
                    dataType: 'json',
                    success: function (doc) {
                        //alert(doc.Status);
                        status = doc.Status;
                    },
                    error: function () {
                        data.Status = '上傳失敗';
                    }
                });

                if (status != '上傳成功') {
                    break;
                }
            }
            if (status == 'Success') {
                return "Success"
            }
            else {
                return
            }
        }

    </script>
</asp:Content>


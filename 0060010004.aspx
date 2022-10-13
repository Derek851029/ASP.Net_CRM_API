<%@ Page Language="C#" EnableEventValidation="true" AutoEventWireup="true" MasterPageFile="~/MasterPage.master" CodeFile="0060010004.aspx.cs" Inherits="_0060010004" %>

<asp:Content ID="Content" ContentPlaceHolderID="head2" runat="Server">
    <link href="../bootstrap-chosen-master/bootstrap-chosen.css" rel="stylesheet" />
    <link href="css/0060010004.css" rel="stylesheet" />
     <style>
        #txt_Agent_ID, 
        #txt_Agent_Name,
        #txt_Agent_Company,
        #txt_Agent_Team,
        #txt_UserID,
        #txt_Password{
            width: 100%; 
            Font-Size: 18px; 
            background-color: #ffffbb;
        }
        body{
            font-size:18px
        }
    </style>
    <!-- ====== Modal ====== -->
    <div class="modal fade" id="myModal"  data-bs-keyboard="false" data-bs-backdrop="static" style="overflow: auto">
        <div class="modal-dialog modal-lg">
            <!-- Modal content-->
            <div class="modal-content">
                <div class="modal-header">
                    <h2 class="modal-title" id="txt_title">人員資料（新增）</h2>
                    <button type="button" class="btn-close" data-bs-dismiss="modal" aria-label="Close"></button>
                    <input id="txt_hid_id" name="txt_hid_id" type="hidden" />
                </div>
                <div class="modal-body">
                    <!-- ========================================== -->
                    <table id="data2" class="table table-bordered" style="width: 99%">
                        <thead>
                            <tr>
                                <th style="text-align: center" colspan="4">
                                    <span style="font-size: 20px"><strong>人員資料</strong></span>
                                </th>
                            </tr>
                        </thead>
                        <tbody>
                            <tr>
                                <td style="width: 20%; color: #D50000;">人員編號</td>
                                <td style="width: 30%">
                                    <div>
                                        <input id="txt_Agent_ID" class="form-control" placeholder="人員編號" onkeyup="value=value.replace(/[\W]/g,'')"
                                            data-toggle="tooltip" title="必填，只能填英文或數字"/>
                                        <label id="lab_Agent_ID"></label>
                                    </div>
                                </td>
                                <td style="width: 20%; color: #D50000;">人員姓名</td>
                                <td style="width: 30%">
                                    <div>
                                        <input id="txt_Agent_Name" class="form-control" placeholder="人員姓名"/>
                                    </div>
                                </td>
                            </tr>
                            <!-- ========================================== -->
                            <tr>
                                <td style="width: 20%">選擇所屬單位</td>
                                <td style="width: 30%">
                                    <select id="select_Agent_Company" name="select_Agent_Company" class="chosen-select" style="width: 100%" onchange="Agent_Company()">
                                        <option value="">選擇所屬單位…</option>
                                    </select>
                                </td>
                                <td style="width: 20%">選擇所屬部門</td>
                                <td style="width: 30%">
                                    <select id="select_Agent_Team" name="select_Agent_Team" class="chosen-select" style="width: 100%" onchange="Agent_Team()">
                                        <option value="">選擇所屬部門…</option>
                                    </select>
                                </td>
                            </tr>
                            <tr>
                                <td style="width: 20%; color: #D50000;">所屬單位</td>
                                <td style="width: 30%">
                                    <input id="txt_Agent_Company" class="form-control" placeholder="可在此新增所屬單位"/>
                                </td>
                                <td style="width: 20%; color: #D50000;">所屬部門</td>
                                <td style="width: 30%">
                                    <input id="txt_Agent_Team" class="form-control" placeholder="可在此新增所屬部門"/>
                                </td>
                            </tr>
                            <!-- ========================================== -->
                            <tr>
                                <td style="width: 20%; color: #D50000;">登入帳號</td>
                                <td style="width: 30%">
                                    <div data-toggle="tooltip" title="必填，並且只能填英文或數字">
                                        <input id="txt_UserID" class="form-control" placeholder="登入帳號"/>
                                    </div>
                                </td>
                                 <td style="width: 20%; color: #D50000;">登入密碼</td>
                                <td style="width: 30%">
                                    <div>
                                        <input id="txt_Password" class="form-control" placeholder="登入密碼" type="password"
                                            onkeyup="value=value.replace(/[\W]/g,'')"
                                            data-toggle="tooltip" title="必填，只能填英文或數字" />
                                    </div>
                                </td>
                            </tr>
                            <tr>
                                <td style="width: 20%">人員權限</td>
                                <td style="width: 30%">
                                    <select id="Agent_LV" style="width: 100%; Font-Size: 18px">
                                        <option value="10">系統管理員</option>
                                        <option value="20">員工</option>
                                    </select>
                                </td>
                                <td style="width: 20%">系統選單權限</td>
                                <td style="width: 30%">
                                    <select id="Role_ID" style="width: 100%; Font-Size: 18px"></select>
                                </td>
                            </tr>
                            <tr>
                                <td style="width: 20%">上傳照片</td>
                                <td style="width: 30%">
                                    <input type="file" id="file" class="form-control" required="required"/>              
                                </td>
                                <td style="width: 20%">
                                    <button type="button" id="showImage" data-bs-toggle="modal" data-bs-target="#dialog2" onclick="" class="btn btn-outline-primary" style="display:none"><span class="glyphicon glyphicon-search">檢視照片</span></button>
                                </td>
                                <td style="width: 40%"></td>
                            </tr>
                        </tbody>
                    </table>
                    <!-- ========================================== -->

                </div>
                <div class="modal-footer">
                    <button type="button" id="btn_close" class="btn btn-secondary" data-bs-dismiss="modal">關閉</button>
                    <button id="btn_new" type="button" class="btn btn-success" onclick="New_Agent()">新增</button>
                    <button id="btn_update" type="button" class="btn btn-primary" onclick="New_Agent()">修改</button>
                </div>
            </div>
            <!-- =========== Modal content =========== -->
        </div>
    </div>
    <!--===================================================-->

    <!-- ====== Modal 搜索條件設定 搜索條件設定 ====== -->
    <div class="modal fade" id="Div1" data-bs-keyboard="false" data-bs-backdrop="static" style="overflow: auto">
        <div class="modal-dialog modal-lg">
            <!-- Modal content-->
            <div class="modal-content">
                <div class="modal-header">
                    <h2 class="modal-title">搜索條件設定（最多只能勾選五筆）</h2>
                    <button type="button" class="btn btn-info btn-lg" style="float: right" onclick="bindTable()">
                    <span class='glyphicon glyphicon-search'></span>&nbsp;&nbsp;開始搜索</button>
                    <button type="button" class="btn-close" data-bs-dismiss="modal" aria-label="Close"></button>
                </div>
                <div class="modal-body">
                    <table class="table table-bordered table-striped" style="width: 99%">
                        <tbody>
                            <tr>
                                <td>
                                    <table id="data_company" class="display table table-striped" style="width: 99%">
                                        <thead>
                                            <tr>
                                                <th>所屬部門</th>
                                                <th>選擇</th>
                                            </tr>
                                        </thead>
                                    </table>
                                </td>
                            </tr>
                        </tbody>
                    </table>
                    <!-- ========================================== -->
                </div>
                <div class="modal-footer">
                    <button type="button" class="btn btn-secondary" data-bs-dismiss="modal">關閉</button>
                </div>
            </div>
            <!-- =========== Modal content =========== -->
        </div>
    </div>
    <!--===================================================-->

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

    <!--===================================================-->
    <div class="col-6">
            <nav aria-label="breadcrumb">
                <ol class="breadcrumb mb-0 d-flex align-items-center">
                    <li class="breadcrumb-item">
                        <a href="0060010004.aspx" class="link">
                            <i class="bi-house"></i>
                        </a>
                    </li>
                    <li class="breadcrumb-item active" aria-current="page">員工管理</li>
                </ol>
                </nav>
            <h1 class="mb-0 fw-bold">人員資料管理（瀏覽）
                <button type="button" class="btn btn-success btn-lg" data-bs-toggle="modal" data-bs-target="#myModal" onclick="Xin_De()"><span class="glyphicon glyphicon-plus">新增人員</span></button>
            </h1> 
        </div>
<%--    <button type="button" class="btn btn-warning btn-lg" data-bs-toggle="modal" data-bs-target="#Div1" style="Font-Size: 20px; float: right;">
    <span class='glyphicon glyphicon-cog'></span>&nbsp;&nbsp;搜索條件設定</button>--%>

        <table id="data" class="table table-striped" style="text-align:center;width:100%">
            <thead>
                <tr>
                    <th style="text-align:center;">員工編號</th>
                    <th style="text-align:center;">員工姓名</th>
                    <th style="text-align:center;">所屬部門</th>
                    <th style="text-align:center;">所屬單位</th>
                    <th style="text-align:center;">系統選單權限</th>
                    <<%--th style="text-align:center;">人員權限</th>--%>
                    <th style="text-align:center;">功能選單</th>
                </tr>
            </thead>
        </table>

    <script src="../chosen/chosen.jquery.js"></script>
    <script src="../js/jquery.validate.min.js"></script>
    <script src="js/notiflix.js"></script>
    <link href="js/notiflix.css" rel="stylesheet" />
    <script type="text/javascript">
        var Role_ID = '<%= Session["RoleID"] %>',Agent_ID = '<%= Session["UserID"] %>';
        var array_mno = [];
        $(function () {
            Listener()
            $('[data-toggle="tooltip"]').tooltip();
            Agent_Company_List();
            Agent_Team_List();
            ROLELIST_List();
            bindTable();
            style("Agent_LV", "10");
            style("Role_ID", "100");
        });

        function Listener() {
            $('#member').css({ "background-color": "#1a9bfc", "color": "white" })

            $('#file').change(function () {
                $.map($(this).get(0).files, function (file, index) {
                    var img_size = Math.floor(file.size / 1024)
                    var type = file.type.split('/')
                    type = type[0]
                    if (type != 'image') {
                        alert("請上傳圖片類型。");
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
                            $('#showImage').show()
                            $('#View_image').append('<img src="' + urlData + '" width=300 heigth:200></img>')
                        };
                    }
                });
            })
        }

        //=============================================
        function bindTable() {
            $.ajax({
                url: '0060010004.aspx/GetPartnerList',
                type: 'POST',
                data: JSON.stringify({ Array: array_mno }),
                contentType: 'application/json; charset=UTF-8',
                dataType: "json",
                success: function (doc) {
                    var table = $('#data').DataTable({
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
                        "aLengthMenu": [[25, 50, 100], [25, 50, 100]],
                        "iDisplayLength": 100,
                        "columnDefs": [{
                            "targets": -1,
                            "data": null,
                            "searchable": false,
                            "paging": false,
                            "ordering": false,
                            "info": false
                        }],
                        columns: [
                            { data: "Agent_ID" },
                            { data: "Agent_Name" },
                            { data: "Agent_Company" },
                            { data: "Agent_Team" },
                            { data: "ROLE_NAME" },
/*                            { data: "Agent_LV" },*/
                            {
                                data: "Agent_ID", render: function (data, type, row, meta) {
                                    var bt = "<button type='button' class='btn btn-outline-primary btn-lg' id='button' " +
                                        "data-bs-toggle='modal' data-bs-target='#myModal' >" +
                                        "<span class='glyphicon glyphicon-pencil'>" +
                                        "</span>&nbsp;修改</button>" +
                                        " <button type='button' class='btn btn-outline-danger btn-lg' id='delete'>" +
                                        "<span class='glyphicon glyphicon-remove'>" +
                                        "</span>&nbsp;刪除</button>";
                                    return bt
                                }
                            },
                        ]
                    });
                    //=========================================================
                    $('#data tbody').unbind('click').
                        on('click', '#button', function () {
                            var table = $('#data').DataTable();
                            var cno = table.row($(this).parents('tr')).data().Agent_ID;
                            Button(cno);
                        })
                        .on('click', '#delete', function () {
                            var table = $('#data').DataTable();
                            var cno = table.row($(this).parents('tr')).data().Agent_ID;
                            Delete(cno);
                        });
                    //=========================================================
                }
            });
        }

        function Agent_Company_List() {
            $.ajax({
                url: '0060010004.aspx/Agent_Company_List',
                type: 'POST',
                contentType: 'application/json; charset=UTF-8',
                dataType: "json",       //如果要回傳值，請設成 json
                success: function (doc) {
                    AgentsCompany(doc);
                    AgentsCompanyTable(doc);
                }
            });
        }

        //================ 帶入【所屬公司】資訊 ===============
        function AgentsCompany(datas) {
            var json = JSON.parse(datas.d);
            var $select_elem = $("#select_Agent_Company");
            $select_elem.html('');
            $select_elem.chosen("destroy")
            $.each(json, function (idx, obj) {
                $select_elem.append("<option value='" + obj.Agent_Company + "'>" + obj.Agent_Company + "</option>");
            });
            $select_elem.chosen({
                width: "100%",
                search_contains: true
            });
        }

        function AgentsCompanyTable(datas) {
            $('#data_company').DataTable({
                destroy: true,
                data: eval(datas.d), "oLanguage": {
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
                "aLengthMenu": [[25, 50, 100], [25, 50, 100]],
                "iDisplayLength": 25,
                "columnDefs": [{
                    "targets": -1,
                    "data": null,
                    "searchable": false,
                    "paging": false,
                    "ordering": false,
                    "info": false
                }],
                columns: [
                    { data: "Agent_Company" },
                    {
                        data: "Agent_Company", render: function (data, type, row, meta) {
                            return "<div class='checkbox'><label>" +
                                "<input type='checkbox' style='width: 30px; height: 30px;' id='chack' />" +
                                "</label></div>";
                        }
                    }]
            });
            $('#data_company tbody').on('click', '#chack', function () {
                var table = $('#data_company').DataTable();
                var cno = table.row($(this).parents('tr')).data().Agent_Company;
                var a = this.checked;
                if (a == true) {
                    if (array_mno.length > 4) {
                        table.row($(this).prop('checked', false));
                        alert('最多只能勾選五筆');
                    } else {
                        array_mno.push(cno);
                    }
                }
                else {
                    array_mno.splice($.inArray(cno, array_mno), 1);
                }
            });
        }

        //================ 帶入【所屬部門】資訊 ===============
        function Agent_Team_List() {
            $.ajax({
                url: '0060010004.aspx/Agent_Team_List',
                type: 'POST',
                contentType: 'application/json; charset=UTF-8',
                dataType: "json",
                success: function (doc) {
                    var json = JSON.parse(doc.d);
                    var $select_elem = $("#select_Agent_Team");
                    $select_elem.html('');
                    $select_elem.chosen("destroy")
                    $.each(json, function (idx, obj) {
                        $select_elem.append("<option value='" + obj.Agent_Team + "'>" + obj.Agent_Team + "</option>");
                    });
                    $select_elem.chosen({
                        width: "100%",
                        search_contains: true
                    });
                }
            });
        };

        function Agent_Company() {
            var company = document.getElementById("select_Agent_Company").value;
            document.getElementById("txt_Agent_Company").value = company.trim();
        };

        function Agent_Team() {
            var team = document.getElementById("select_Agent_Team").value;
            document.getElementById("txt_Agent_Team").value = team.trim();
        };

        //================ 帶入【系統選單權限】資訊 ===============
        function ROLELIST_List() {
            $.ajax({
                url: '0060010004.aspx/ROLELIST_List',
                type: 'POST',
                contentType: 'application/json; charset=UTF-8',
                dataType: "json",
                success: function (doc) {
                    var json = JSON.parse(doc.d);
                    var $select_elem = $("#Role_ID");
                    $select_elem.html('');
                    $select_elem.chosen("destroy")
                    $.each(json, function (idx, obj) {
                        $select_elem.append("<option value='" + obj.ROLE_ID + "'>" + obj.ROLE_NAME + "</option>");
                    });
                    $select_elem.chosen({
                        width: "100%",
                        search_contains: true
                    });
                }
            });
        };

        //================ 帶入【人員】資訊 ===============
        function Xin_De() { Button("0"); }

        function Button(Agent_ID) {
            style("select_Agent_Company", "");
            style("select_Agent_Team", "");
            style("Agent_LV", "10");
            style("Role_ID", "100");
            document.getElementById("txt_Agent_ID").value = "";
            document.getElementById("txt_Agent_Name").value = "";
            document.getElementById("txt_Agent_Company").value = "";
            document.getElementById("txt_Agent_Team").value = "";
            document.getElementById("txt_UserID").value = "";
            document.getElementById("txt_Password").value = "";
            document.getElementById("txt_Agent_ID").disabled = false;
            document.getElementById("txt_Agent_ID").style.display = "none";
            document.getElementById("btn_new").style.display = "none";
            document.getElementById("btn_update").style.display = "none";
            document.getElementById("file").value = "";
            $("#txt_hid_id").val(Agent_ID);
            $('#View_image').html('')
            if (Agent_ID == '0') {
                document.getElementById("btn_new").style.display = "";
                document.getElementById("txt_Agent_ID").style.display = "";
                document.getElementById("lab_Agent_ID").innerHTML = "";
                /*document.getElementById('file').disabled = false;*/
                document.getElementById("showImage").style.display = "none";
                document.getElementById("View_image").innerHTML = "";
                document.getElementById("txt_title").innerHTML = "人員資料（新增）";
            } else {
                document.getElementById("txt_Agent_ID").disabled = true;
                document.getElementById("btn_update").style.display = "";
                document.getElementById("showImage").style.display = "";
                /*document.getElementById('file').disabled = true;*/
                document.getElementById("txt_title").innerHTML = "人員資料（修改）";
                $.ajax({
                    url: '0060010004.aspx/DispatchSystem',
                    type: 'POST',
                    data: JSON.stringify({
                        Agent_ID: Agent_ID,
                    }),
                    contentType: 'application/json; charset=UTF-8',
                    dataType: "json",
                    success: function (doc) {
                        var obj = JSON.parse('{"data":' + doc.d + '}');
                        document.getElementById("txt_Agent_ID").value = obj.data[0].Agent_ID;
                        document.getElementById("lab_Agent_ID").innerHTML = obj.data[0].Agent_ID;
                        document.getElementById("txt_Agent_Name").value = obj.data[0].Agent_Name;
                        document.getElementById("txt_Agent_Company").value = obj.data[0].Agent_Company;
                        style("select_Agent_Company", obj.data[0].Agent_Company);
                        document.getElementById("txt_Agent_Team").value = obj.data[0].Agent_Team;
                        style("select_Agent_Team", obj.data[0].Agent_Team);
                        document.getElementById("txt_UserID").value = obj.data[0].UserID;
                        style("Agent_LV", obj.data[0].Agent_LV);
                        style("Role_ID", obj.data[0].Role_ID);
                        console.log(obj.data[0].ImageURL)
                        $('#View_image').append('<img src="' + obj.data[0].ImageURL + '" width=300 heigth:200></img>')
                    }
                });
            }
        };

        function Delete(Agent_ID) {
            if (confirm("確定要刪除嗎？")) {
                $.ajax({
                    url: '0060010004.aspx/Delete',
                    ache: false,
                    type: 'POST',
                    data: JSON.stringify({ Agent_ID: Agent_ID }),
                    contentType: 'application/json; charset=UTF-8',
                    dataType: "json",
                    success: function (data) {
                        var json = JSON.parse(data.d.toString());
                        if (json.status == "success") {
                            var DeleteFace = FaceControl(3, '', '', Agent_ID)
                            if (DeleteFace == 'ok') {
                                bindTable();
                            }
                            else {
                                alert('人臉辨識刪除失敗，請聯絡管理員。')
                            }
                        }
                    }
                });
            }
        };

        function style(Name, value) {
            var $select_elem = $("#" + Name);
            $select_elem.chosen("destroy")
            document.getElementById(Name).value = value;
            $select_elem.chosen({
                width: "100%",
                search_contains: true
            });
        }

        function New_Agent() {
            document.getElementById("btn_update").disabled = true;
            document.getElementById("btn_new").disabled = true;
            var Agent_ID = document.getElementById("txt_Agent_ID").value;
            var Agent_Name = document.getElementById("txt_Agent_Name").value;
            var Agent_Company = document.getElementById("txt_Agent_Company").value;
            var Agent_Team = document.getElementById("txt_Agent_Team").value;
            var UserID = document.getElementById("txt_UserID").value;
            var Password = document.getElementById("txt_Password").value;
            var Agent_LV = document.getElementById("Agent_LV").value;
            var Role_ID = document.getElementById("Role_ID").value;
            var Flag = ($("#txt_hid_id").val() == '0') ? '0' : '1';

            var filelist = $('#file').get(0).files[0]
            if (filelist) {
                Notiflix.Loading.Dots('loading...');
                var SizeKB = filelist.size / 1024
                var reader = new FileReader();
                reader.readAsDataURL(filelist);
                reader.onload = function (e) {
                    var urlData = reader.result;
                    var Base64File = urlData.split(',')[1]
                    $.ajax({
                        url: '0060010004.aspx/New_Agent',
                        type: 'POST',
                        async: false,
                        data: JSON.stringify({
                            Agent_ID: Agent_ID,
                            Agent_Name: Agent_Name,
                            Agent_Company: Agent_Company,
                            Agent_Team: Agent_Team,
                            UserID: UserID,
                            Password: Password,
                            Role_ID: Role_ID,
                            Agent_LV: Agent_LV,
                            Flag: Flag,
                            SizeKB: SizeKB,
                            Base64File: Base64File,
                        }),
                        contentType: 'application/json; charset=UTF-8',
                        dataType: "json",
                        success: function (doc) {
                            var json = JSON.parse(doc.d.toString());
                            if (json.status == "new") {
                                var CreateStaff = FaceControl(0, Agent_Name, '', Agent_ID)
                                if (CreateStaff == 'ok') {
                                    var CreateFace = FaceControl(1, '', json.ImageURL, Agent_ID)
                                    if (CreateFace === 'ok') {
                                        Notiflix.Loading.Remove(100);
                                        document.getElementById("btn_update").disabled = false;
                                        document.getElementById("btn_new").disabled = false;
                                        alert("新增完成！")
                                        $('#btn_close').click()
                                        bindTable();
                                        Agent_Company_List();
                                        Agent_Team_List();
                                    }
                                    else {
                                        Notiflix.Loading.Remove(100);
                                        alert('人臉辨識建立失敗，請聯絡管理員。')
                                    }
                                }
                                else {
                                    alert('人員建立失敗，請聯絡管理員。')
                                }
                            }
                            else if (json.status == "update") {
                                var EditRole = FaceControl(2, Agent_Name, '', Agent_ID)
                                if (EditRole == 'ok') {
                                    var EditFace = FaceControl(1, Agent_Name, json.ImageURL, Agent_ID)
                                    if (EditFace == 'ok') {
                                        Notiflix.Loading.Remove(100);
                                        alert("修改完成！");
                                        $('#btn_close').click()
                                        document.getElementById("btn_update").disabled = false;
                                        document.getElementById("btn_new").disabled = false;
                                        bindTable();
                                        Agent_Company_List();
                                        Agent_Team_List();
                                    }
                                    else {
                                        Notiflix.Loading.Remove(100);
                                        alert('人臉辨識修改失敗，請聯絡管理員。')
                                    }
                                }
                                else {
                                    alert('人員修改失敗，請聯絡管理員。')
                                }
                            }
                            else {
                                alert(json.status);
                            }
                        },
                        error: function () {
                            document.getElementById("btn_update").disabled = false;
                            document.getElementById("btn_new").disabled = false;
                        }
                    });
                }
            }
            else {
                document.getElementById("btn_update").disabled = false;
                document.getElementById("btn_new").disabled = false;
                alert('請上傳圖片。')
                return
            }
            //else {
            //    if (Flag == '1') {
            //        $.ajax({
            //            url: '0060010004.aspx/New_Agent',
            //            type: 'POST',
            //            async:false,
            //            data: JSON.stringify({
            //                Agent_ID: Agent_ID,
            //                Agent_Name: Agent_Name,
            //                Agent_Company: Agent_Company,
            //                Agent_Team: Agent_Team,
            //                UserID: UserID,
            //                Password: Password,
            //                Role_ID: Role_ID,
            //                Agent_LV: Agent_LV,
            //                Flag: Flag,
            //                SizeKB: 1.1,
            //                Base64File: ''
            //            }),
            //            contentType: 'application/json; charset=UTF-8',
            //            dataType: "json",
            //            success: function (doc) {
            //                var json = JSON.parse(doc.d.toString());
            //                if (json.status == "new") {
            //                    alert("新增完成！")
            //                    bindTable();
            //                    Agent_Company_List();
            //                    Agent_Team_List();
            //                }
            //                else if (json.status == "update") {
                                //var EditRole = FaceControl(2, Agent_Name, '', Agent_ID)
                                //if (EditRole == 'ok') {
                                //    var EditFace = aceControl(1, Agent_Name, json.ImageURL, Agent_ID)
                                //    if (EditFace == 'ok') {
                                //        Notiflix.Loading.Remove(100);
                                //        alert("修改完成！");
                                //        $('#btn_close').click()
                                //        document.getElementById("btn_update").disabled = false;
                                //        document.getElementById("btn_new").disabled = false;
                                //        bindTable();
                                //        Agent_Company_List();
                                //        Agent_Team_List();
                                //    }
                                //    else {
                                //        Notiflix.Loading.Remove(100);
                                //        alert('人臉辨識修改失敗，請聯絡管理員。')
                                //    }
            //                    }
            //                }
            //                else {
            //                    alert(json.status);
            //                }
            //            },
            //            error: function () {
            //                document.getElementById("btn_update").disabled = false;
            //                document.getElementById("btn_new").disabled = false;
            //            }
            //        });
            //    }
            //    else {
            //        alert('請上傳圖片。')
            //        return
            //    }
         /*   }*/
            
        }

        function FaceControl(type, Agent_Name, img_url, Agent_ID) {
            console.log(Agent_ID, Agent_Name)
            var data;
            var status;
            switch (type) {
                case 0:
                    data = {
                        'UserInfo': {
                            'employeeNo': Agent_ID,
                            'name': Agent_Name,
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
                        'FPID': Agent_ID
                    }
                    break
                case 2:
                    data = {
                        'UserInfo': {
                            'employeeNo': Agent_ID,
                            'name': Agent_Name,
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
                            'EmployeeNoList': [{ 'employeeNo': Agent_ID }],
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
    </script>
</asp:Content>

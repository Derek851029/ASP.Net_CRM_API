<%@ Page Language="C#" EnableEventValidation="false" AutoEventWireup="true" MasterPageFile="~/MasterPage.master" CodeFile="~/0030010099.aspx.cs" Inherits="_0030010099" %>

<asp:Content ID="Content" ContentPlaceHolderID="head2" runat="Server">
    <link href="../css/jquery.datetimepicker.min.css" rel="stylesheet" />
    <link href="../DataTables/jquery.dataTables.min.css" rel="stylesheet" />
    <link href="../bootstrap-chosen-master/bootstrap-chosen.css" rel="stylesheet" />
    <script src="../js/jquery.datetimepicker.full.min.js"></script>
    <script src="../chosen/chosen.jquery.js"></script>
    <script src="../js/jquery.validate.min.js"></script>
    <script src="../DataTables/jquery.dataTables.min.js"></script>
    <script type="text/javascript">
        var seqno = '<%= seqno %>';
        var new_mno = '<%= new_mno %>';
        var Agent_Mail = '<%= Session["Agent_Mail"] %>';
        //====================================================
        $(function () {
            $('[data-toggle="tooltip"]').tooltip();
            Load();
            Service_List();  //帶入【服務分類 】資訊
            Select_Telecomm();
            Client_Code_Search();
            //Owner_List();   //帶入【廠商 】資訊
            //Hospital_List();  //帶入【醫療院所 】資訊
            //Cust_Table();  //顯示【客戶地址】名單
            ShowTime();  //抓現在時間
            if (seqno != '0') {
                Load_CaseData();
            } else {
                bindTable();  //顯示【新增多筆資訊（瀏覽）】名單
                Labor_Table();  //顯示【新增雇主及外勞】名單
            }
        });

        //================ 讀取資訊 ===============
        function Load() {
            document.getElementById('tab_Location').style.display = "none";  //隱藏地址
            document.getElementById('Hospital_Table_1').style.display = "none";  //隱藏醫療院所、就醫類型
            if (seqno == '0') {
                document.getElementById("str_sysid").innerHTML = new_mno; //【創造母單編號】
                document.getElementById('Btn_Update').style.display = "none";  //隱藏修改按鈕
                //document.getElementById('table_cancel').style.display = 'none';  //隱藏【取消需求單】
                $.ajax({
                    url: '0030010099.aspx/Load',
                    type: 'POST',
                    contentType: 'application/json; charset=UTF-8',
                    dataType: "json",
                    success: function (doc) {
                        var obj = JSON.parse('{"data":' + doc.d + '}');
                        document.getElementById("str_name").innerHTML = obj.data[0].Agent_Name.trim();
                        //document.getElementById("str_team").innerHTML = obj.data[0].Agent_Team.trim();
                        //document.getElementById("ContactName").value = obj.data[0].Agent_Name.trim();
                        //document.getElementById("Agent_Phone_2").value = obj.data[0].Agent_Phone_2;
                        //document.getElementById("Agent_Phone_3").value = obj.data[0].Agent_Phone_3;
                        //document.getElementById("Agent_Co_TEL").value = obj.data[0].Agent_Co_TEL;
                    }
                });
            } else {
                new_mno = seqno;
                document.getElementById("str_sysid").innerHTML = new_mno;
                //document.getElementById('table_modal').style.display = "none";  //隱藏【選擇雇主或外勞】
                //document.getElementById('Btn_New').style.display = "none";  //隱藏送出按鈕
                //document.getElementById('Add_Table_2').style.display = 'none';  //隱藏【新增多筆資訊】
            }
        }

        //=============

        function ShowTime() {
            var NowDate = new Date();
            var h = NowDate.getHours();
            var m = NowDate.getMinutes();
            var s = NowDate.getSeconds();
            var y = NowDate.getFullYear();
            var mon = NowDate.getMonth()+1;
            var d = NowDate.getDate();
            
            <%--if (mon < 10) {
                if (d < 10) {
                    if (h < 10) {
                        document.getElementById('LoginTime').value = y + "/0" + mon + "/0" + d + " " + h + ":" + m;
                    }
                } else { document.getElementById('LoginTime').value = y + "/0" + mon + "/" + d + " " + h + ":" + m; }
            } else {
                if (d < 10) {
                    document.getElementById('LoginTime').value = y + "/" + mon + "/0" + d + " " + h + ":" + m;
                } else { document.getElementById('LoginTime').value = y + "/" + mon + "/" + d + " " + h + ":" + m; }
                   }--%>
            document.getElementById('LoginTime').value = y + "/" + mon + "/" + d + " " + h + ":" + m;
            //document.getElementById('EstimatedFinishTime').innerHTML = y + "/" + mon + "/" + d + " " + h + ":" + m;             // 估計完成時間
            
        }

        //================ 帶入【服務分類 】資訊 ===============
        function Service_List() {
            $.ajax({
                url: '0030010099.aspx/Service_List',
                type: 'POST',
                contentType: 'application/json; charset=UTF-8',
                dataType: "json",
                success: function (doc) {
                    var json = JSON.parse(doc.d);
                    var $select_elem = $("#DropService");
                    $select_elem.chosen("destroy")
                    $.each(json, function (idx, obj) {
                        $select_elem.append("<option value='" + obj.Service + "'>" + obj.Service + "</option>");
                    });
                    $select_elem.chosen(
                    {
                        width: "100%",
                        search_contains: true
                    });
                    $('.chosen-single').css({ 'background-color': '#ffffbb' });
                }
            });
        }

        //function Esti_Fin_Time() { }

        function ServiceChanged() {
            document.getElementById('tab_Location').style.display = "none";  //隱藏地址

            var s = document.getElementById("DropService");
            var str_value = s.options[s.selectedIndex].value;
            $.ajax({
                url: '0030010099.aspx/ServiceName_List',            // ???
                type: 'POST',
                data: JSON.stringify({ service: str_value }),
                contentType: 'application/json; charset=UTF-8',
                dataType: "json",
                success: function (doc) {
                    var json = JSON.parse(doc.d);
                    var $select_elem = $("#DropServiceName");
                    $select_elem.chosen("destroy");
                    $select_elem.empty();
                    $select_elem.append("<option value=''>" + "請選擇服務內容…" + "</option>");
                    $.each(json, function (idx, obj) {
                        $select_elem.append("<option value='" + obj.Service_ID + "'>" + obj.ServiceName + "</option>");
                    });
                    $select_elem.chosen({
                        width: "100%",
                        search_contains: true
                    });
                    $('.chosen-single').css({ 'background-color': '#ffffbb' });
                }
            });

            if (str_value == "醫療") {
                document.getElementById('Hospital_Table_1').style.display = "";  //顯示醫療院所、就醫類型
                style("HospitalName", "");
                style("HospitalClass", "");
            }
            else {
                document.getElementById('Hospital_Table_1').style.display = "none";  //隱藏醫療院所、就醫類型
            }
        }


        function ServiceNameChanged() {
            document.getElementById('button_01').style.display = "none";  //客戶地址
            document.getElementById('button_02').style.display = "none";  //單位地址
            document.getElementById("txt_Location").innerHTML = "";
            document.getElementById("hid_PostCode").value = "";
            var str_index = document.getElementById("DropServiceName").selectedIndex;
            var v = document.getElementById("DropServiceName").options[str_index].value;
            $.ajax({
                url: '0030010099.aspx/ServiceName_Flag',
                type: 'POST',
                data: JSON.stringify({ Service_ID: v }),
                contentType: 'application/json; charset=UTF-8',
                dataType: "json",
                success: function (doc) {
                    var json = JSON.parse(doc.d.toString());
                    if (json.status == "A") {
                        document.getElementById('button_02').style.display = "";  //單位地址
                    } else if (json.status == "B") {
                        v = "39";
                        document.getElementById('button_02').style.display = "";  //單位地址
                    } else if (json.status == "C") {
                        document.getElementById('button_01').style.display = "";  //客戶地址
                    }
                    Location(v);
                    document.getElementById('tab_Location').style.display = "";  //顯示地址
                }
            });
        }

        function Select_Telecomm() {
            $.ajax({
                url: '0030010099.aspx/Select_Telecomm',
                type: 'POST',
                contentType: 'application/json; charset=UTF-8',
                dataType: "json",
                success: function (doc) {
                    var json = JSON.parse(doc.d);
                    var $select_elem = $("#SelectTelecomm");
                    $select_elem.chosen("destroy")
                    $.each(json, function (idx, obj) {
                        $select_elem.append("<option value='" + obj.B + "'>" + obj.A + "</option>");    // 選擇電信商,  obj.A 來自 Telecomm 資料
                    });
                    $select_elem.chosen(
                    {
                        width: "100%",
                        search_contains: true
                    });
                    $('.chosen-single').css({ 'background-color': '#ffffbb' });
                }
            });
        }

        function Client_Code_Search() {
            $.ajax({
                url: '0030010099.aspx/Client_Code_Search',
                type: 'POST',
                contentType: 'application/json; charset=UTF-8',
                dataType: "json",
                success: function (doc) {
                    var json = JSON.parse(doc.d);
                    var $select_elem = $("#DropClientCode");
                    $select_elem.chosen("destroy")
                    $.each(json, function (idx, obj) {
                        $select_elem.append("<option value='" + obj.C + "'>[" + obj.B + "]" + obj.A + "</option>");     // 顯示客戶代碼選單
                    });
                    $select_elem.chosen(
                    {
                        width: "100%",
                        search_contains: true
                    });
                    $('.chosen-single').css({ 'background-color': '#ffffbb' });
                }
            });
        }

        function ShowClientData() {
            var str_index = document.getElementById("DropClientCode").value;
            $.ajax({
                url: '0030010099.aspx/Show_Client_Data',
                type: 'POST',
                data: JSON.stringify({ value: str_index }),
                contentType: 'application/json; charset=UTF-8',
                dataType: "json",
                success: function (doc) {
                    var obj = JSON.parse('{"data":' + doc.d + '}'); // 組合JSON 格式
                    document.getElementById("str_Client_Name").innerHTML = obj.data[0].C;
                    document.getElementById("str_contact").innerHTML = obj.data[0].D;
                    document.getElementById("str_com_tel").innerHTML = "("+ obj.data[0].E + ")" + obj.data[0].F;
                    document.getElementById("str_mob_phone").innerHTML = obj.data[0].G;
                    document.getElementById("str_hardware").innerHTML = obj.data[0].H;
                    document.getElementById("str_software").innerHTML = obj.data[0].I;
                    document.getElementById("str_serv_typ").innerHTML = obj.data[0].J;
                }
            });
        }

 /*       function ShowCaseData() {
            var str_index = document.getElementById("DropClientCode").value;
            $.ajax({
                url: '0030010099.aspx/Show_Client_Data',
                type: 'POST',
                data: JSON.stringify({ value: str_index }),
                contentType: 'application/json; charset=UTF-8',
                dataType: "json",
                success: function (doc) {
                    var obj = JSON.parse('{"data":' + doc.d + '}'); // 組合JSON 格式
                    document.getElementById("str_Client_Name").innerHTML = obj.data[0].C;
                    document.getElementById("str_contact").innerHTML = obj.data[0].D;
                    document.getElementById("str_com_tel").innerHTML = "(" + obj.data[0].E + ")" + obj.data[0].F;
                    document.getElementById("str_mob_phone").innerHTML = obj.data[0].G;
                    document.getElementById("str_hardware").innerHTML = obj.data[0].H;
                    document.getElementById("str_software").innerHTML = obj.data[0].I;
                    document.getElementById("str_serv_typ").innerHTML = obj.data[0].J;
                }
            });
        }           */

        function Data_Save() {                  //右方 存入 左方
            
            var str_sysid = document.getElementById("str_sysid").innerHTML;
            var str_telecomm_id = document.getElementById("SelectTelecomm").value;
            var str_login_time = document.getElementById("LoginTime").value;
            var str_client_id = document.getElementById("DropClientCode").value;
            var str_urgency = document.getElementById("SelectUrgency").value;
            var str_op_source = document.getElementById("SelectOpinionSource").value;
            var str_op_type = document.getElementById("SelectOpinionType").value;
            var str_esti_fin_time = document.getElementById("datetimepicker02").value;
            var str_op_sub = document.getElementById("SelectOpinionSubject").value;
            var str_onspot_time = document.getElementById("datetimepicker01").value;
            var str_opinion = document.getElementById("Opinion").value;
            var str_rep_type = document.getElementById("ReplyType").value;
            var str_ps = document.getElementById("PS").value;
            var str_deal_process = document.getElementById("DealingProcess").value;
            var str_rep = document.getElementById("Reply").value;
            $.ajax({
                url: '0030010099.aspx/Case_Save',
                type: 'POST',
                data: JSON.stringify({
                    sysid: str_sysid,
                    telecomm_id:str_telecomm_id,
                    login_time: str_login_time,
                    client_id: str_client_id,
                    urgency: str_urgency,
                    op_source: str_op_source,
                    op_type: str_op_type,
                    esti_fin_time: str_esti_fin_time,
                    op_sub: str_op_sub,
                    onspot_time: str_onspot_time,
                    opinion: str_opinion,
                    rep_type: str_rep_type,
                    ps: str_ps,
                    deal_process: str_deal_process,
                    rep: str_rep,
                }),
                contentType: 'application/json; charset=UTF-8',
                dataType: "json",
                success: function (doc) {
                    var json = JSON.parse(doc.d.toString());
                    if (json.status == "new") {
                        alert('新增完成');
                        window.location.href = "/0030010000/0030010099.aspx";
                    }
                    else if (json.status == "update") {
                        alert('修改完成');
                        window.location.href = "/0030010000/0030010099.aspx";
                    }
                    else {
                        alert(json.status);
                    }
                    $("#Div_Loading").modal('hide');
                    document.getElementById("Btn_New").disabled = false;
                }, error: function () { alert("請填完所有必填欄位"); }
            });
        }
        
        //================ 帶入【廠商】資訊 ===============
        function Owner_List() {
            $.ajax({
                url: '0030010099.aspx/Owner_List',
                type: 'POST',
                contentType: 'application/json; charset=UTF-8',
                dataType: "json",
                success: function (doc) {
                    var json = JSON.parse(doc.d);
                    var $select_elem = $("#DropOwner");
                    $select_elem.empty();
                    $select_elem.append("<option value=''>" + "顯示全部廠商…" + "</option>");
                    $.each(json, function (idx, obj) {
                        $select_elem.append("<option value='" + obj.Cust_ID + "'>" + "（" + obj.Cust_ID + "）" + obj.Cust_FullName + "</option>");
                    });
                    $select_elem.chosen({
                        width: "50%",
                        search_contains: true
                    });
                }
            });
        }

        //================ 帶入【客戶地址】資訊 ===============
        function Cust_Table() {
            $("#Cust_Table").dataTable({
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
                "bSortClasses": false,
                "bStateSave": false,
                "bPaginate": true,
                "bAutoWidth": false,
                "bProcessing": true,
                "bServerSide": true,
                "bDestroy": true,
                "sAjaxSource": "../WebService.asmx/Cust_System",
                "sServerMethod": "POST",
                "contentType": "application/json; charset=UTF-8",
                "sPaginationType": "full_numbers",
                "bDeferRender": true,
                "fnServerParams": function (aoData) {
                    aoData.push(
                        { "name": "iParticipant", "value": $("#participant").val() },
                        { "name": "Cust_ID", "value": "" }
                        );
                },
                "fnServerData": function (sSource, aoData, fnCallback) {
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
                                $("#Cust_System").show();
                            }
                    });
                }
            });
        }

        function DropOwnerChanged() {
            Labor_Table();
        }
        //================ 帶入【醫療院所 】資訊 ===============
        function Hospital_List() {
            $.ajax({
                url: '0030010099.aspx/Hospital_List',
                type: 'POST',
                contentType: 'application/json; charset=UTF-8',
                dataType: "json",
                success: function (doc) {
                    var listItems = "";
                    var obj = JSON.parse('{"data":' + doc.d + '}');
                    for (var i = 0; i < obj.data.length; i++) {
                        listItems += "<option value='" + obj.data[i].HospitalName + "'>" + obj.data[i].HospitalName + "</option>";
                    }
                    $('#HospitalName').append(listItems);
                }
            });
        }

        //============= 顯示【新增多筆資訊（瀏覽）】名單 =============
        function bindTable() {
            $("#Div_Loading").modal();
            $.ajax({
                url: '0030010099.aspx/GetCNoList',
                type: 'POST',
                contentType: 'application/json; charset=UTF-8',
                dataType: "json",       //如果要回傳值，請設成 json
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
                        "columnDefs": [{
                            "targets": -1,
                            "data": null,
                            "searchable": false,
                            "paging": false,
                            "ordering": false,
                            "info": false
                        }],
                        columns: [
                            { data: "Cust_FullName" },
                            { data: "Labor_CName" },
                            { data: "Labor_ID" },
                            { data: "Labor_PID" },
                            { data: "Labor_RID" },
                            { data: "Labor_EID" },
                            { data: "Labor_Phone" },
                            { data: "Labor_Country" },
                            { data: "Labor_Valid" },
                            {
                                data: "SYS_ID", render: function (data, type, row, meta) {
                                    return "<button id='delete' type='button' class='btn btn-danger btn-lg' ><span class='glyphicon glyphicon-remove'></span>&nbsp;取消</button>"
                                }
                            }
                        ]
                    });
                    //=========================================
                    $('#data tbody').unbind('click').
                    on('click', '#delete', function () {
                        var table = $('#data').DataTable();
                        var SYS_ID = table.row($(this).parents('tr')).data().SYS_ID;
                        Labor_Delete(SYS_ID);
                    });

                    $("#Div_Loading").modal('hide');
                }, error: function () {
                    $("#Div_Loading").modal('hide');
                }
            });
        }

        //=============== 顯示【新增雇主及外勞】名單 ===============
        function Labor_Table() {
            $("#Labor_Table").dataTable({
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
                "bSortClasses": false,
                "bStateSave": false,
                "bPaginate": true,
                "bAutoWidth": false,
                "bProcessing": true,
                "bServerSide": true,
                "bDestroy": true,
                "sAjaxSource": "../WebService.asmx/Labor_System",
                "sServerMethod": "POST",
                "contentType": "application/json; charset=UTF-8",
                "sPaginationType": "full_numbers",
                "bDeferRender": true,
                "fnServerParams": function (aoData) {
                    aoData.push(
                        { "name": "iParticipant", "value": $("#participant").val() },
                        { "name": "Cust_ID", "value": document.getElementById("DropOwner").value },
                        { "name": "Flag", "value": "A" }
                        );
                },
                "fnServerData": function (sSource, aoData, fnCallback) {
                    $.ajax({
                        "dataType": 'json',
                        "contentType": 'application/json; charset=UTF-8',
                        "type": "GET",
                        "url": sSource,
                        "data": aoData,
                        "success":
                            function (msg) {
                                //var json = JSON.parse(msg.d);
                                var json = jQuery.parseJSON(msg.d);
                                fnCallback(json);
                                $("#Labor_Table").show();
                            }
                    });
                }
            });
        }

        //=============== 帶入【新增雇主及外勞】資訊 ===============
        function Labor_add(SYSID, Flag) {
            var MNo = new_mno;
            $.ajax({
                url: '0030010099.aspx/Labor_Value',
                type: 'POST',
                data: JSON.stringify({ SYSID: SYSID, Flag: Flag }),
                contentType: 'application/json; charset=UTF-8',
                dataType: "json",
                success: function (doc) {
                    var json = JSON.parse(doc.d.toString());
                    if (json.status != "null") {
                        var text = '{"data":' + doc.d + '}';
                        var obj = JSON.parse(text);
                        document.getElementById("txt_Cust_FullName").innerHTML = obj.data[0].Cust_FullName;
                        document.getElementById("txt_Labor_Team").innerHTML = obj.data[0].Labor_Team;
                        document.getElementById("txt_Labor_Name").innerHTML = obj.data[0].Labor_CName;
                        document.getElementById("txt_Labor_Country").innerHTML = obj.data[0].Labor_Country;
                        document.getElementById("txt_Labor_ID").innerHTML = obj.data[0].Labor_ID;
                        document.getElementById("txt_Labor_PID").innerHTML = obj.data[0].Labor_PID;
                        document.getElementById("txt_Labor_RID").innerHTML = obj.data[0].Labor_RID;
                        document.getElementById("txt_Labor_EID").innerHTML = obj.data[0].Labor_EID;
                        document.getElementById("txt_Labor_Phone").innerHTML = obj.data[0].Labor_Phone;
                        document.getElementById("txt_Labor_Address").innerHTML = obj.data[0].Labor_Address;
                        document.getElementById("Labor_Address2").innerHTML = obj.data[0].Labor_Address2;
                        document.getElementById("txt_Labor_Valid").innerHTML = obj.data[0].Labor_Valid;
                        document.getElementById("txt_Labor_Language").innerHTML = obj.data[0].Language;

                        //=============== 帶入【已選擇的雇主或外勞（瀏覽）】資訊 ===============
                        $.ajax({
                            url: '0030010099.aspx/Btn_Add_Click',
                            type: 'POST',
                            data: JSON.stringify({ Cust_ID: obj.data[0].Cust_ID, Labor_ID: obj.data[0].Labor_ID }),
                            contentType: 'application/json; charset=UTF-8',
                            dataType: "json",
                            success: function (doc) {
                                var json = JSON.parse(doc.d.toString());
                                alert(json.status);
                            }
                        });
                        bindTable();
                        //=============== 帶入【已選擇的雇主或外勞（瀏覽）】資訊 ===============
                    }
                }
            });
        }

        //================ 刪除【新增多筆資訊 】資訊 ===============
        function Labor_Delete(SYS_ID) {
            $.ajax({
                url: '0030010099.aspx/Labor_Delete',
                type: 'POST',
                data: JSON.stringify({ SYS_ID: SYS_ID }),
                contentType: 'application/json; charset=UTF-8',
                dataType: "json",       //如果要回傳值，請設成 json
                success: function () {
                }
            });
            bindTable();
        }

        //================ 【Flag = 0 新增需求單】 ===============
        //================ 【Flag = 1 修改需求單】 ===============
 /*       function Btn_New_Click(Flag) {
            document.getElementById("Btn_New").disabled = true;
            $("#Div_Loading").modal();
            if (Agent_Mail == '0') {
                alert('【您為新進人員或是沒有E-MAIL資料所以無法新增需求單，請尋求管理人員協助新增，謝謝】');
                document.getElementById("Btn_New").disabled = false;
                $("#Div_Loading").modal('hide');
                return;
            }
            var PostCode = $("#hid_PostCode").val();
            var Location = document.getElementById("txt_Location").innerHTML;  //地址
            var Service_ID = document.getElementById("DropServiceName").value;  //服務內容
            var time_01 = document.getElementById("datetimepicker01").value;    //預定起始時間
            var time_02 = document.getElementById("datetimepicker02").value;    //預定終止時間
            var time_03 = document.getElementById("datetimepicker03").value;    //登錄日期
            var LocationStart = document.getElementById("LocationStart").value;  //行程起點 
            var LocationEnd = document.getElementById("LocationEnd").value;     //行程終點
            var CarSeat = document.getElementById("txt_CarSeat").value;                //搭車人數
            var ContactName = document.getElementById("ContactName").value;    //聯絡人
            var ContactPhone2 = document.getElementById("Agent_Phone_2").value;  //手機簡碼
            var ContactPhone3 = document.getElementById("Agent_Phone_3").value;  //手機號碼
            var Contact_Co_TEL = document.getElementById("Agent_Co_TEL").value; //公司電話
            var HospitalName = document.getElementById("HospitalName").value;   //醫療院所
            var HospitalClass = document.getElementById("HospitalClass").value;       //就醫類型
            var Question = document.getElementById("Question").value;                       //狀況說明

            $.ajax({
                url: '0030010099.aspx/Check_Form',
                type: 'POST',
                data: JSON.stringify({
                    Flag: Flag,
                    Service_ID: Service_ID,
                    Time_01: time_01,
                    Time_02: time_02,
                    PostCode: PostCode,
                    Location: Location,
                    LocationStart: LocationStart,
                    LocationEnd: LocationEnd,
                    CarSeat: CarSeat,
                    ContactName: ContactName,
                    ContactPhone2: ContactPhone2,
                    ContactPhone3: ContactPhone3,
                    Contact_Co_TEL: Contact_Co_TEL,
                    Hospital: HospitalName,
                    HospitalClass: HospitalClass,
                    Question: Question
                }),
                contentType: 'application/json; charset=UTF-8',
                dataType: "json",
                success: function (doc) {
                    var json = JSON.parse(doc.d.toString());
                    if (json.status == "new") {
                        alert('新增完成');
                        window.location.href = "/0030010000/0030010002.aspx";
                    }
                    else if (json.status == "update") {
                        alert('修改完成');
                        window.location.href = "/0030010000/0030010002.aspx";
                    }
                    else {
                        alert(json.status);
                    }
                    $("#Div_Loading").modal('hide');
                    document.getElementById("Btn_New").disabled = false;
                }
            });
        };*/

        //=========================================
/*        function Load_MNo() {
            $("#Div_Loading").modal();
            $.ajax({
                url: '0030010099.aspx/Load_MNo',
                type: 'POST',
                data: JSON.stringify({ MNo: new_mno }),
                contentType: 'application/json; charset=UTF-8',
                dataType: "json",       //如果要回傳值，請設成 json
                success: function (doc) {
                    var text = '{"data":' + doc.d + '}';
                    var obj = JSON.parse(text);
                    if (obj.data[0].MNo == "NULL") {
                        alert("查無【" + new_mno + "】母單編號");
                        window.location.href = "/0030010000/0030010002.aspx";
                        return;
                    } else if (obj.data[0].MNo == "NO") {
                        //================ 判斷是否為該部門的單 =======================  
                        alert('您沒有訪問此頁面的權限，此需求單非您部門的需求單。');
                        window.location.href = "/0030010000/0030010002.aspx";
                        return;
                        //================ 判斷是否為該部門的單 =======================
                    }
                    document.getElementById("str_name").innerHTML = obj.data[0].Create_Name;
                    document.getElementById("str_team").innerHTML = obj.data[0].Create_Team;
                    style('DropService', obj.data[0].Service);
                    ServiceChanged();
                    document.getElementById("txt_Cust_FullName").innerHTML = obj.data[0].Cust_FullName;
                    document.getElementById("txt_Labor_Team").innerHTML = obj.data[0].Labor_Team;
                    document.getElementById("txt_Labor_Name").innerHTML = obj.data[0].Labor_CName;
                    document.getElementById("txt_Labor_Country").innerHTML = obj.data[0].Labor_Country;
                    document.getElementById("txt_Labor_Language").innerHTML = obj.data[0].Labor_Language;
                    document.getElementById("txt_Labor_ID").innerHTML = obj.data[0].Labor_ID;
                    document.getElementById("txt_Labor_PID").innerHTML = obj.data[0].Labor_PID;
                    document.getElementById("txt_Labor_RID").innerHTML = obj.data[0].Labor_RID;
                    document.getElementById("txt_Labor_EID").innerHTML = obj.data[0].Labor_EID;
                    document.getElementById("txt_Labor_Phone").innerHTML = obj.data[0].Labor_Phone;
                    document.getElementById("txt_Labor_Address").innerHTML = obj.data[0].Labor_Address;
                    document.getElementById("txt_Labor_Valid").innerHTML = obj.data[0].Labor_Valid;
                    document.getElementById("Labor_Address2").innerHTML = obj.data[0].Labor_Address2;
                    //=====================================================

                    document.getElementById("txt_Location").innerHTML = obj.data[0].Location;
                    document.getElementById("LocationStart").value = obj.data[0].LocationStart;
                    document.getElementById("LocationEnd").value = obj.data[0].LocationEnd;
                    document.getElementById("ContactName").value = obj.data[0].ContactName;
                    document.getElementById("Agent_Phone_2").value = obj.data[0].ContactPhone2;
                    document.getElementById("Agent_Phone_3").value = obj.data[0].ContactPhone3;
                    document.getElementById("Agent_Co_TEL").value = obj.data[0].Contact_Co_TEL;
                    document.getElementById("HospitalClass").value = obj.data[0].HospitalClass;
                    document.getElementById("txt_CarSeat").value = obj.data[0].CarSeat;
                    document.getElementById("Question").value = obj.data[0].Question;
                    document.getElementById("datetimepicker01").value = obj.data[0].Time_01.replace(/-/g, '/').substr(0, 10) + ' ' + obj.data[0].Time_01.substr(11, 5);
                    document.getElementById("datetimepicker02").value = obj.data[0].Time_02.replace(/-/g, '/').substr(0, 10) + ' ' + obj.data[0].Time_02.substr(11, 5);

                }
            });
            document.getElementById('button_01').style.display = "none";  //客戶地址
            document.getElementById('button_02').style.display = "none";  //單位地址
            document.getElementById('tab_Location').style.display = "";  //顯示地址
            $("#Div_Loading").modal('hide');
        }
        */
       //================   Load_CaseData()       
        function Load_CaseData() {            
            //var str_index = document.getElementById("DropClientCode").value;
            $.ajax({
                url: '0030010099.aspx/Load_CaseData',
                type: 'POST',
                data: JSON.stringify({ CaseData: new_mno }),
                contentType: 'application/json; charset=UTF-8',
                dataType: "json",       //如果要回傳值，請設成 json
                success: function (doc) {
                    var text = '{"data":' + doc.d + '}';
                    var obj = JSON.parse(text);
                    //style('DropService', obj.data[0].Service);
                    //ServiceChanged();
                    alert(obj.data[0].OpinionSubject);
                    document.getElementById("txt_SYS_ID").value = obj.data[0].SYS_ID;
                    document.getElementById("txt_Case_ID").innerHTML = obj.data[0].Case_ID;
                    document.getElementById("txt_ReplyType").innerHTML = obj.data[0].ReplyType;
                    document.getElementById("txt_PS").innerHTML = obj.data[0].PS;
                    document.getElementById("txt_DealingProcess").innerHTML = obj.data[0].PS;
                    document.getElementById("txt_Reply").innerHTML = obj.data[0].Reply;
                    document.getElementById("txt_Telecomm_ID").innerHTML = obj.data[0].Telecomm_ID;
                    document.getElementById("txt_Type_Value").innerHTML = obj.data[0].Type_Value;
                    document.getElementById("txt_Type").innerHTML = obj.data[0].Type;
                    document.getElementById("txt_Cust_Name").innerHTML = obj.data[0].Cust_Name;
                    document.getElementById("txt_Upload_Time").innerHTML = obj.data[0].Upload_Time;
                    document.getElementById("txt_ID").innerHTML = obj.data[0].ID    ;
                    document.getElementById("txt_Urgency").innerHTML = obj.data[0].Urgency;
                    document.getElementById("txt_OpinionSource").innerHTML = obj.data[0].OpinionSource;
                    document.getElementById("txt_OpinionType").innerHTML = obj.data[0].OpinionType;
                  
                    document.getElementById("SelectOpinionSubject").value = "123";//obj.data[0].OpinionSubject;

                    document.getElementById("datetimepicker02").innerHTML = obj.data[0].EstimatedFinishTime;
                    document.getElementById("datetimepicker01").innerHTML = obj.data[0].OnSpotTime;
                    
                    //document.getElementById("datetimepicker01").value = obj.data[0].OnSpotTime.replace(/-/g, '/').substr(0, 10) + ' ' + obj.data[0].Time_01.substr(11, 5);
                    //document.getElementById("datetimepicker02").value = obj.data[0].EstimatedFinishTime.replace(/-/g, '/').substr(0, 10) + ' ' + obj.data[0].Time_02.substr(11, 5);

                }
            });
            //document.getElementById('button_01').style.display = "none";  //客戶地址    額外的按鈕          
            //document.getElementById('button_02').style.display = "none";  //單位地址
            //document.getElementById('tab_Location').style.display = "";  //顯示地址
            $("#Div_Loading").modal('hide');        // 功能??
        }

        //================ 【取消需求單】 ===============
        function Btn_Cancel_Click() {
            $("#Div_Loading").modal();
            var txt_cancel = document.getElementById("txt_cancel").value;
            $.ajax({
                url: '0030010099.aspx/Btn_Cancel_Click',
                type: 'POST',
                data: JSON.stringify({ txt_cancel: txt_cancel }),
                contentType: 'application/json; charset=UTF-8',
                dataType: "json",       //如果要回傳值，請設成 json
                success: function (doc) {
                    var json = JSON.parse(doc.d.toString());
                    if (json.status == "success") {
                        alert('需求單【' + json.mno + '】已取消');
                        window.location.href = "/0030010000/0030010002.aspx";
                    }
                    else {
                        alert(json.status);
                    }
                }
            });
            $("#Div_Loading").modal('hide');
        };

        //================== 【返回】 =================
        function Btn_Back_Click() {
            window.location.href = "/0030010000/0030010002.aspx";
        };

        //============= 顯示【地址】名單 =============
        function Location(Type) {
            $.ajax({
                url: '0030010099.aspx/Location_List',
                type: 'POST',
                data: JSON.stringify({ Type: Type }),
                contentType: 'application/json; charset=UTF-8',
                dataType: "json",       //如果要回傳值，請設成 json
                success: function (doc) {
                    var table = $('#Location_Table').DataTable({
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
                        "aLengthMenu": [[10, 25, 50, 100], [10, 25, 50, 100]],
                        "iDisplayLength": 50,
                        "columnDefs": [{
                            "targets": -1,
                            "data": null,
                            "searchable": false,
                            "paging": false,
                            "ordering": false,
                            "info": false
                        }],
                        columns: [
                            { data: "Postcode" },
                            { data: "Location" },
                            { data: "Name" },
                            { data: "Address" },
                            { data: "TEL" },
                            {
                                data: "SYS_ID", render: function (data, type, row, meta) {
                                    return "<button id='edit' type='button' class='btn btn-success btn-lg' data-dismiss='modal'>" +
                                        "<span class='glyphicon glyphicon-ok'></span>&nbsp;&nbsp;選擇</button>"
                                }
                            }
                        ]
                    });
                    //================================================
                    $('#Location_Table tbody').unbind('click').
                    on('click', '#edit', function () {
                        var table = $('#Location_Table').DataTable();
                        var SYS_ID = table.row($(this).parents('tr')).data().SYS_ID;
                        Add_Location(SYS_ID, "0");
                    });
                }
            });
        }

        function Add_Location(ID, Flag) {
            $.ajax({
                url: '0030010099.aspx/Add_Location',
                type: 'POST',
                data: JSON.stringify({ ID: ID, Flag: Flag }),
                contentType: 'application/json; charset=UTF-8',
                dataType: "json",       //如果要回傳值，請設成 json
                success: function (doc) {
                    var text = '{"data":' + doc.d + '}';
                    var obj = JSON.parse(text);
                    document.getElementById("txt_Location").innerHTML = "名稱：【" + obj.data[0].Name + " - " + obj.data[0].Location +
                        "】地址：【" + obj.data[0].Postcode + "】【" + obj.data[0].Address + "】";
                    document.getElementById("hid_PostCode").value = obj.data[0].Postcode;
                }
            });
        }
        //style("SelectOpinionType_chosen", "8");
        //================【下拉選單】 CSS 修改 ================
        function style(Name, value) {
            var $select_elem = $("#" + Name);
            $select_elem.chosen("destroy")
            document.getElementById(Name).value = value;
            $select_elem.chosen({
                width: "100%",
                search_contains: true
            });
            $('.chosen-single').css({ 'background-color': '#ffffbb' });
        }

        //================================================
    </script>
    <style type="text/css">
        body {
            font-family: "Microsoft JhengHei",Helvetica,Arial,Verdana,sans-serif;
            font-size: 16px;
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

        #Location_Table td:nth-child(6), #Location_Table td:nth-child(5), #Location_Table td:nth-child(4),
        #Location_Table td:nth-child(3), #Location_Table td:nth-child(2), #Location_Table td:nth-child(1),
        #Cust_Table td:nth-child(7), #Cust_Table td:nth-child(6), #Cust_Table td:nth-child(5),
        #Cust_Table td:nth-child(4), #Cust_Table td:nth-child(3), #Cust_Table td:nth-child(2), #Cust_Table td:nth-child(1),
        #Labor_Table td:nth-child(12), #Labor_Table td:nth-child(11), #Labor_Table td:nth-child(10), #Labor_Table td:nth-child(9),
        #Labor_Table td:nth-child(8), #Labor_Table td:nth-child(7), #Labor_Table td:nth-child(6), #Labor_Table td:nth-child(5),
        #Labor_Table td:nth-child(4), #Labor_Table td:nth-child(3), #Labor_Table td:nth-child(2), #Labor_Table td:nth-child(1),
        #data td:nth-child(10), #data td:nth-child(9), #data td:nth-child(8), #data td:nth-child(7), #data td:nth-child(6), #data td:nth-child(5),
        #data td:nth-child(4), #data td:nth-child(3), #data td:nth-child(2), #data td:nth-child(1), #data th:nth-child(5) {
            text-align: center;
        }

        .auto-style1 {
            height: 47px;
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
    <!-- ====== Modal ====== -->

    <div class="modal fade" id="myModal" role="dialog" data-backdrop="static" data-keyboard="false">
        <div class="modal-dialog" style="width: 95%;">

            <!-- Modal content-->
            <div class="modal-content">
                <div class="modal-header">
                    <button type="button" class="close" data-dismiss="modal">&times;</button>
                    <h2 class="modal-title"><strong>雇主及外勞資料</strong></h2>
                    <select id="DropOwner" name="DropOwner" onchange="DropOwnerChanged()" style="Font-Size: 16px">
                        <option value="">顯示全部廠商</option>
                    </select>
                </div>
                <div class="modal-body">
                    <%-- ========================================== --%>
                    <table id="Labor_Table" class="display table table-striped" style="width: 99%">
                        <thead>
                            <tr>
                                <th style="text-align: center; width: 8%">雇主編號</th>
                                <th style="text-align: center; width: 8%">雇主名稱</th>
                                <th style="text-align: center; width: 8%">勞工姓名</th>
                                <th style="text-align: center; width: 9%">勞工編號</th>
                                <th style="text-align: center; width: 9%">護照號碼</th>
                                <th style="text-align: center; width: 9%">居留證號</th>
                                <th style="text-align: center; width: 9%">職工編號</th>
                                <th style="text-align: center; width: 8%">連絡電話 </th>
                                <th style="text-align: center; width: 8%">國籍</th>
                                <th style="text-align: center; width: 8%">狀態 </th>
                                <th style="text-align: center; width: 8%">選擇</th>
                                <th style="text-align: center; width: 8%">選擇</th>
                            </tr>
                        </thead>
                    </table>
                    <%-- ========================================== --%>
                </div>
                <div class="modal-footer">
                    <button type="button" class="btn btn-default" data-dismiss="modal">關閉</button>
                </div>
            </div>
        </div>
    </div>
    <!-- ====== Modal ====== -->
    <div class="modal fade" id="MyCust" role="dialog" data-backdrop="static" data-keyboard="false">
        <div class="modal-dialog" style="width: 95%;">
            <!-- Modal content-->
            <div class="modal-content">
                <div class="modal-header">
                    <button type="button" class="close" data-dismiss="modal">&times;</button>
                    <h2 class="modal-title"><strong>選擇客戶地址</strong></h2>

                </div>
                <div class="modal-body">
                    <%-- ========================================== --%>
                    <table id="Cust_Table" class="display table table-striped" style="width: 99%">
                        <thead>
                            <tr>
                                <th style="text-align: center; width: 15%">客戶編號</th>
                                <th style="text-align: center; width: 15%">客戶名稱</th>
                                <th style="text-align: center; width: 15%">郵遞區號</th>
                                <th style="text-align: center; width: 15%">客戶地址</th>
                                <th style="text-align: center; width: 15%">客戶電話</th>
                                <th style="text-align: center; width: 15%">統一編號</th>
                                <th style="text-align: center; width: 10%">選擇</th>
                            </tr>
                        </thead>
                    </table>
                    <%-- ========================================== --%>
                </div>
                <div class="modal-footer">
                    <button type="button" class="btn btn-default" data-dismiss="modal">關閉</button>
                </div>
            </div>
        </div>
    </div>
    <!--===================================================-->
    <!-- ====== Modal ====== -->
    <div class="modal fade" id="Modal_1" role="dialog" data-backdrop="static" data-keyboard="false">
        <div class="modal-dialog" style="width: 95%;">
            <!-- Modal content-->
            <div class="modal-content">
                <div class="modal-header">
                    <button type="button" class="close" data-dismiss="modal">&times;</button>
                    <h2 class="modal-title"><strong>選擇單位地址</strong></h2>
                </div>
                <div class="modal-body">
                    <%-- ========================================== --%>
                    <table id="Location_Table" class="display table table-striped" style="width: 99%">
                        <thead>
                            <tr>
                                <th style="text-align: center; width: 17%">郵遞區號</th>
                                <th style="text-align: center; width: 17%">區域名稱</th>
                                <th style="text-align: center; width: 17%">單位名稱</th>
                                <th style="text-align: center; width: 17%">地址</th>
                                <th style="text-align: center; width: 17%">電話</th>
                                <th style="text-align: center; width: 15%">選擇</th>
                            </tr>
                        </thead>
                    </table>
                    <%-- ========================================== --%>
                </div>
                <div class="modal-footer">
                    <button type="button" class="btn btn-default" data-dismiss="modal">關閉</button>
                </div>
            </div>
        </div>
    </div>
    <!--===================================================-->
    <div style="width: 1280px; margin: 10px 20px">
        <h2><strong>(<%= str_title %>)服務作業單</strong></h2>
        <table class="table table-bordered table-striped">
            <thead>
                <tr>
                    <th style="text-align: center" colspan="4">
                        <span style="font-size: 20px"><strong>案件資料紀錄</strong></span>
                    </th>
                </tr>
            </thead>
            <tbody>
                <tr>
                    <th style="text-align: center; width: 15%;">
                        <strong>案件編號</strong>
                    </th>
                    <th style="width: 35%">
                        <label id="str_sysid"></label>
                        <strong><%= str_type %></strong>
                    </th>
                    <th style="text-align: center; width: 15%">
                        <strong>負責電信商</strong>
                    </th>
                    <th style="width: 35%">
                        <div data-toggle="tooltip" title="必選" style="width: 100%">
                            <select id="SelectTelecomm" name="SelectTelecomm" class="chosen-select">
                                <option value="">請選擇</option>
                                <option value="0">德瑪科技</option>
                                <option value="1">遠傳電信</option>
                                <option value="2">中華電信</option>
                                
                            </select>
                        </div>
                    </th>
                </tr>
                <%--  =========== 勞工資料 ===========--%>

                <%--  =========== 勞工資料 ===========--%>
                <tr id="tr_sysid" runat="server">                                                       <%--  Table 001--%>
                    <td style="text-align: center">
                        <strong>登錄日期</strong>
                    </td>
                    <td>
                        <div style="float: left" data-toggle="tooltip" title="必填">
                            <input type="text" class="form-control" id="LoginTime" name="LoginTime" style="background-color: #ffffbb" value=""/>
                        </div>
                    </td>
                    <td style="text-align: center">
                        <strong>登錄人員</strong>
                    </td>
                    <td>
                        <strong>系統管理者</strong>
                        <!-- id待建立 -->
                    </td>
                </tr>
                <tr>
                    <td style="text-align: center">
                        <strong>來電號碼</strong>
                    </td>
                    <td>
                        <label id="str_name"></label>
                    </td>
                    <td style="text-align: center">
                        <strong></strong>
                    </td>
                    <td>
                        <%-- <label id="str_team"></label>--%>
                    </td>
                </tr>
                <%--  ========== 服務人員資料 ===========--%>

                <%--  ========== 服務項目 ===========--%>

                <%--  ========== 服務項目 ===========--%>




                <tr id="tab_Location">
                    <td style="text-align: center">
                        <strong>地址與郵遞區號</strong>
                    </td>
                    <td>
                        <label id="txt_Location"></label>
                    </td>
                    <td style="text-align: center; color: #D50000;">
                        <strong>選擇地址</strong>
                    </td>
                    <td>
                        <div style="float: left" data-toggle="tooltip" title="建議選取，以方便人員快速派單，如無選項則不用選取">
                            <div>
                                <button id="button_01" type="button" class="btn btn-primary btn-lg" data-toggle="modal" data-target="#MyCust">
                                    <span class='glyphicon glyphicon-plus'></span>&nbsp;&nbsp;選擇客戶地址</button>
                            </div>
                            <div>
                                <button id="button_02" type="button" class="btn btn-primary btn-lg" data-toggle="modal" data-target="#Modal_1">
                                    <span class='glyphicon glyphicon-plus'></span>&nbsp;&nbsp;選擇單位地址</button>
                            </div>
                        </div>
                    </td>
                </tr>
                <%--  =========== 行程 ============--%>



                <%--  ========== 醫療院所 =========== --%>

                <tr id="Hospital_Table_1">
                    <td style="text-align: center; color: #D50000;">
                        <strong>醫療院所</strong>
                    </td>
                    <td>
                        <div data-toggle="tooltip" title="必選" style="width: 100%">
                            <select id="HospitalName" name="HospitalName" class="chosen-select">
                                <option value="">請選擇醫療院所…</option>
                            </select>
                        </div>
                    </td>
                    <td style="text-align: center; color: #D50000;">
                        <strong>就醫類型</strong>
                    </td>
                    <td>
                        <div data-toggle="tooltip" title="必選" style="width: 100%">
                            <select id="HospitalClass" name="HospitalClass" class="chosen-select">
                                <option value="">請選擇就醫類型…</option>
                                <option value="上班就醫">上班就醫</option>
                                <option value="上班就醫 ( 需診斷書 )">上班就醫 ( 需診斷書 )</option>
                                <option value="下班就醫">下班就醫</option>
                                <option value="下班就醫 ( 需診斷書 )">下班就醫 ( 需診斷書 ) </option>
                            </select>
                        </div>
                    </td>
                </tr>      

                <%--  ========== 醫療院所 ===========--%>
            </tbody>
        </table>
        <%--  =========== 勞工資料 ===========--%>

        <%--  =========== 勞工資料 ===========--%>
        <table class="table table-bordered table-striped">                      <%--  Table 002-1  --%>
            <thead>
                <tr>
                    <th style="text-align: center" colspan="4">
                        <span style="font-size: 20px"><strong>客戶資料</strong></span>
                    </th>
                </tr>
            </thead>
            <tbody>
                <tr style="height: 55px;">
                    <th style="text-align: center; width: 15%">
                        <strong>客戶代碼(*)
                            <br>
                            <input type="checkbox" name="e-mail" value="1">發送email
                        </strong>
                    </th>
                    <th style="width: 35%">
                        
                        <div data-toggle="tooltip" title="必選" style="width: 100%">
                            <select id="DropClientCode" name="DropClientCode" class="chosen-select" onchange="ShowClientData()">
                                <option value="">請選擇</option>
                            </select>
                        </div>
                    </th>
                </tr>
                <%--  =========== 客戶資料 ===========--%>
                <tr style="height: 55px;">                                                          <%--  Table 002-2  --%>
                    <td style="text-align: center; width: 15%">
                        <strong>客戶名稱</strong>
                    </td>
                    <td style="width: 35%">
                        <label id="str_Client_Name"></label>

                    </td>
                    <td style="text-align: center; width: 15%">
                        <strong>聯絡人</strong>
                    </td>
                    <td style="width: 35%">
                        <label id="str_contact"></label>
                    </td>
                </tr>

                <%--  =========== 勞工資料 ===========--%>

                <tr style="height: 55px;">
                    <td style="text-align: center">
                        <strong>公司電話</strong>
                    </td>
                    <td>
                        <label id="str_com_tel"></label>
                    </td>
                    <td style="text-align: center">
                        <strong>行動電話</strong>
                    </td>
                    <td>
                        <label id="str_mob_phone"></label>
                    </td>
                </tr>

                <%--  =========== 勞工資料 ===========--%>
                <tr style="height: 55px;">
                    <td style="text-align: center">
                        <strong>硬體</strong>
                    </td>
                    <td>
                        <label id="str_hardware"></label>
                    </td>
                    <td style="text-align: center">
                        <strong>軟體</strong>
                    </td>
                    <td>
                        <label id="str_software"></label>
                    </td>
                </tr>
                <%--  =========== 勞工資料 ===========--%>

                <%--  =========== 勞工資料 ===========--%>
                <tr style="height: 55px;">
                    <td style="text-align: center">
                        <strong>服務類型</strong>
                    </td>
                    <td>
                        <label id="str_serv_typ"></label>
                    </td>
                    <td style="text-align: center">
                        <strong></strong>
                        <br />
                        <strong></strong>
                    </td>
                    <td>
                        <label id="txt_Labor_EID"></label>
                    </td>
                </tr>

                <%--  =========== 勞工資料 ===========--%>

               
                <%--  =========== 勞工資料 ===========--%>
                <tr>
                    <td colspan="4" style="text-align: center">
                        <input id="hid_PostCode" name="hid_PostCode" type="hidden" />
                        
                        <button id="Btn_Update" type="button" onclick="Btn_New_Click('1');" class="btn btn-primary btn-lg "><span class="glyphicon glyphicon-pencil"></span>&nbsp;&nbsp;修改</button>&nbsp; &nbsp;
                        
                    </td>
                </tr>
            </tbody>
        </table>

        <table class="table table-bordered table-striped">
            <thead>
                <tr>
                    <th style="text-align: center" colspan="4">                                                             <%--  Table 003  --%>
                        <span style="font-size: 20px"><strong>反應內容</strong></span>
                    </th>
                </tr>
            </thead>
            <tbody>
                <tr style="height: 55px;">
                    <th style="text-align: center; width: 15%">
                        <strong>緊急程度(*)</strong>
                    </th>
                    <th style="width: 35%">
                        <div data-toggle="tooltip" title="必選" style="width: 100%">
                            <select id="SelectUrgency" name="SelectUrgency" class="chosen-select" style="background-color: #ffffbb">
                                <option value="">請選擇緊急程度...</option>
                                <option value="0">維護</option>
                                <option value="1">緊急故障</option>
                                <option value="2">重要故障</option>
                                <option value="3">一般故障</option>
                            </select>
                        </div>
                    </th>
                    <td style="text-align: center; width: 15%">
                        <strong>預定完成日(*)</strong>
                    </td>
                    <td style="width: 35%">
                        <div style="float: left" data-toggle="tooltip" title="必填">
                            <input type="text" class="form-control" id="datetimepicker02" name="datetimepicker02" style="background-color: #ffffbb" />
                        </div>
                    </td>
                    
                </tr>
                <%--  =========== 勞工資料 ===========--%>

                <%--  =========== 勞工資料 ===========--%>
                <tr style="height: 55px;">
                    <th style="text-align: center; width: 15%">
                        <strong>意見來源(*)</strong>
                    </th>
                    <th style="width: 35%">
                        <div data-toggle="tooltip" title="必選" style="width: 100%">
                            <select id="SelectOpinionSource" name="SelectOpinionSource" class="chosen-select" style="background-color: #ffffbb">
                                <option value="">請選擇意見來源...</option>
                                <option value="0">電話</option>
                                <option value="1">e-mail</option>
                                <option value="2">傳真</option>
                                <option value="3">合約</option>
                                <option value="4">保固</option>
                            </select>
                        </div>
                    </th>
                    <th style="text-align: center; width: 15%">
                        <strong>意見類型(*)</strong>
                    </th>
                    <th style="width: 35%">
                        <div data-toggle="tooltip" title="必選" style="width: 100%">
                            <select id="SelectOpinionType" name="SelectOpinionType" class="chosen-select" style="background-color: #ffffbb">
                                <option value="">請選擇意見類型...</option>
                                <option value="0">故障報修</option>
                                <option value="1">軟體修改</option>
                                <option value="2">技術諮詢</option>
                                <option value="3">其他服務</option>
                                <option value="4">定期維護</option>
                                <option value="5">測試</option>
                                <option value="6">專案</option>
                                <option value="7">設備擴充</option>
                                <option value="8">駐點服務</option>
                            </select>
                        </div>
                    </th>
                    
                </tr>

                <%--  =========== 勞工資料 ===========--%>
                
                <tr style="height: 55px;">
                    <td style="text-align: center">
                        <strong>意見主旨</strong>
                    </td>
                    <td>
                        <div style="float: left" data-toggle="tooltip" title="必填，不能超過150 個字元，並且含有不正確的符號">      
                                <textarea id="SelectOpinionSubject" name="OpinionSubject" class="form-control" cols="45" rows="1" placeholder="意見主旨" maxlength="150" onkeyup="cs(this);"
                                style="resize: none; background-color: #ffffbb"  ></textarea>
                        </div>
                    </td>
                    <td style="text-align: center">
                        <strong>到點時間</strong>
                    </td>
                    <td>
                        <div style="float: left" data-toggle="tooltip" title="必填">
                            <input type="text" class="form-control" id="datetimepicker01" name="datetimepicker01" style="background-color: #ffffbb" />
                        </div>
                    </td>
                </tr>

                <%--  =========== 勞工資料 ===========--%>
                <tr style="height: 55px;">
                    <td style="text-align: center">
                        <strong>意見內容</strong>
                    </td>
                    <td>
                        <div style="float: left" data-toggle="tooltip" title="必填，不能超過400 個字元，並且含有不正確的符號">
                            <textarea id="Opinion" name="Opinion" class="form-control" cols="45" rows="3" placeholder="意見內容" maxlength="400" onkeyup="cs(this);"
                                style="resize: none; background-color: #ffffbb"></textarea>
                        </div>
                    </td>
                    <td style="text-align: center">
                        <strong>回覆方式(*)</strong>
                    </td>
                    <td>
                        <div data-toggle="tooltip" title="必選" style="width: 100%">
                            <select id="ReplyType" name="ReplyType" class="chosen-select" style="background-color: #ffffbb">
                                <option value="">-請選擇</option>
                                <option>行動電話</option>
                                <option>市內電話</option>
                                <option>e-mail回覆</option>
                                <option>傳真回覆</option>

                            </select>
                        </div>
                    </td>
                </tr>
                <%--  =========== 勞工資料 ===========--%>

                <%--  =========== 勞工資料 ===========--%>
                <tr style="height: 55px;">
                    <td style="text-align: center">
                        <strong>處理日期</strong>
                    </td>
                    <td>
                        <label id="Label7"></label>
                    </td>
                    <td style="text-align: center">
                        <strong>處理人員</strong>
                    </td>
                    <td>
                        <strong>系統管理員</strong>
                    </td>
                </tr>

                <tr style="height: 55px;">
                    <td style="text-align: center">
                        <strong>備註</strong>
                    </td>
                    <td>
                        <div style="float: left" data-toggle="tooltip" title="">
                            <textarea id="PS" name="PS" class="form-control" cols="45" rows="3" placeholder="備註" maxlength="400" onkeyup="cs(this);"
                                style="resize: none;"></textarea>
                        </div>
                    </td>
                    <td style="text-align: center">
                        <strong>處理狀態(*)</strong>
                    </td>
                    <td>
                        <div data-toggle="tooltip" title="必選" style="width: 100%">
                            <select id="DealingProcess" name="DealingProcess" class="chosen-select" style="background-color: #ffffbb">
                                <option value="">-請選擇</option>
                                <option value="0">處理中</option>
                                <option value="1">結案</option>

                            </select>
                        </div>
                    </td>
                </tr>
            </tbody>
        </table>
        <table class="table table-bordered table-striped">
            <thead>
                <tr>
                    <th style="text-align: center" colspan="4">
                        <span style="font-size: 20px"><strong>客服處理</strong></span>
                    </th>
                </tr>
            </thead>
            <tbody>
                <tr style="height: 55px;">
                    <th style="text-align: center; width: 15%">
                        <strong>客服處理<br />
                            回覆內容</strong>
                    </th>
                    <th style="width: 85%">
                        <div style="float: left" data-toggle="tooltip" title="">
                            <textarea id="Reply" name="Reply" class="form-control" cols="45" rows="3" placeholder="客服處理 回覆內容" maxlength="400" onkeyup="cs(this);"
                                style="resize: none;"></textarea>
                        </div>
                    </th>

                </tr>

            </tbody>
        </table>
        <table class="table table-bordered table-striped">
            <thead>
                <tr>
                    <th style="text-align: center" colspan="4">
                        <span style="font-size: 20px"><strong>結案資料</strong></span>
                    </th>
                </tr>
            </thead>
            <tbody>
                <tr style="height: 55px;">
                    <th style="text-align: center; width: 15%">
                        <strong>滿意度(*)</strong>
                    </th>
                    <th style="width: 35%"></th>
                    <th style="text-align: center; width: 15%">
                        <strong>結案日期</strong>
                    </th>
                    <th style="width: 35%"></th>

                </tr>

            </tbody>
        </table>
        <table class="table table-bordered table-striped">
            <thead>
                <tr>
                    <th style="text-align: center" colspan="4">
                        <span style="font-size: 20px"><strong>案件歷史紀錄</strong></span>
                    </th>
                </tr>
            </thead>
            <tbody>
                <tr style="height: 55px;">
                    <th style="text-align: center; width: 15%">
                        <strong>客戶名稱</strong>
                    </th>
                    <th style="width: 85%">
                        <input name="client_search" type="text" maxlength="20" id="client_search" title="客戶名稱" class="form-control" style="float: left; margin-right: 10px;" />&nbsp;
                        <button id="Button2" type="button" class="btn btn-success btn-lg " style="float: left">搜尋</button>
                    </th>

                </tr>

            </tbody>
        </table>


        <%--=========== 子單 ===========--%>

        <div style="text-align: center">

            <button id="Button1" type="button" onclick="Data_Save();" class="btn btn-success btn-lg "><span class="glyphicon glyphicon-ok"></span>確定&nbsp;&nbsp;</button>

            <button id="Button3" type="button" onclick="Btn_Back_Click();" class="btn btn-default btn-lg ">&nbsp;&nbsp;取消<span class="glyphicon glyphicon-share-alt"></span></button>

        </div>
        <!--===================================================-->
        <script>
            $.datetimepicker.setLocale('ch');
            $('#datetimepicker01').datetimepicker({
                allowTimes: [
                    '00:00', '00:30', '01:00', '01:30', '02:00', '02:30', '03:00', '03:30', '04:00', '04:30', '05:00', '05:30',
                    '06:00', '06:30', '07:00', '07:30', '08:00', '08:30', '09:00', '09:30', '10:00', '10:30', '11:00', '11:30',
                    '12:00', '12:30', '13:00', '13:30', '14:00', '14:30', '15:00', '15:30', '16:00', '16:30', '17:00', '17:30',
                    '18:00', '18:30', '19:00', '19:30', '20:00', '20:30', '21:00', '21:30', '22:00', '22:30', '23:00', '23:30'
                ]
            });

            $('#datetimepicker02').datetimepicker({
                allowTimes: [
                    '00:00', '00:30', '01:00', '01:30', '02:00', '02:30', '03:00', '03:30', '04:00', '04:30', '05:00', '05:30',
                    '06:00', '06:30', '07:00', '07:30', '08:00', '08:30', '09:00', '09:30', '10:00', '10:30', '11:00', '11:30',
                    '12:00', '12:30', '13:00', '13:30', '14:00', '14:30', '15:00', '15:30', '16:00', '16:30', '17:00', '17:30',
                    '18:00', '18:30', '19:00', '19:30', '20:00', '20:30', '21:00', '21:30', '22:00', '22:30', '23:00', '23:30'
                ]
            });
            $('#datetimepicker03').datetimepicker({
                allowTimes: [
                    '00:00', '00:30', '01:00', '01:30', '02:00', '02:30', '03:00', '03:30', '04:00', '04:30', '05:00', '05:30',
                    '06:00', '06:30', '07:00', '07:30', '08:00', '08:30', '09:00', '09:30', '10:00', '10:30', '11:00', '11:30',
                    '12:00', '12:30', '13:00', '13:30', '14:00', '14:30', '15:00', '15:30', '16:00', '16:30', '17:00', '17:30',
                    '18:00', '18:30', '19:00', '19:30', '20:00', '20:30', '21:00', '21:30', '22:00', '22:30', '23:00', '23:30'
                ]
            });

            $(function () {
                $('.chosen-select').chosen();
                $('.chosen-select-deselect').chosen({ allow_single_deselect: true });
                $('.chosen-single').css({ 'background-color': '#ffffbb' });
            });
        </script>
    </div>
</asp:Content>

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
            //Select_Telecomm();
            //List_Dispatch_Team();   //選部門
            Change_Dispatch();
            Client_Code_Search();       //選客戶
            ShowTime();             //抓時間         
            document.getElementById("Chose_Company").innerHTML = "Engineer";
            if (seqno != '0') {
                document.getElementById('Button1').style.display = "none";  //隱藏確定鈕
                document.getElementById('Button6').style.display = "none";  //隱藏派工並存檔
                document.getElementById("str_title").innerHTML = "(修改)服務單作業";
                Load_Data();
            }
            else {
                document.getElementById("str_title").innerHTML = "(新增)服務單作業";
                document.getElementById('Button2').style.display = "none";  //隱藏修改鈕
                document.getElementById('Button4').style.display = "none";  //隱藏派工
                document.getElementById('Button5').style.display = "none";  //隱藏轉派工
                //document.getElementById("DealingProcess").innerHTML = "未到點";
                
            }
        });

        //================ 讀取資訊 ===============
        function Load() {
            if (seqno == '0') {
                document.getElementById("str_sysid").innerHTML = new_mno; //【創造母單編號】
                $.ajax({
                    url: '0030010099.aspx/Load',
                    type: 'POST',
                    contentType: 'application/json; charset=UTF-8',
                    dataType: "json",
                    success: function (doc) {
                        var obj = JSON.parse('{"data":' + doc.d + '}');
                        document.getElementById('Text2').value = obj.data[0].Agent_Name;
                        var Agent_ID = obj.data[0].Agent_ID;
                        //alert(Agent_ID);
                        //document.getElementById("str_name").innerHTML = obj.data[0].Agent_Name.trim();
                    }
                });
            } else {
                //new_mno = seqno;
                document.getElementById("str_sysid").innerHTML = seqno;
                //alert(new_mno);
                //document.getElementById('table_modal').style.display = "none";  //隱藏【選擇雇主或外勞】
                //document.getElementById('Btn_New').style.display = "none";  //隱藏送出按鈕
                //document.getElementById('Add_Table_2').style.display = 'none';  //隱藏【新增多筆資訊】
            }
        }

        function ShowTime() {
            $.ajax({
                url: '0030010099.aspx/ShowTime',
                type: 'POST',
                contentType: 'application/json; charset=UTF-8',
                dataType: "json",
                success: function (doc) {
                    document.getElementById('LoginTime').value = doc.d;
                    //document.getElementById('EstimatedFinishTime').innerHTML = doc.d;
                }
            });
        }

        //================   Load_CaseData()       
        function Load_Data() {
            var Case_ID = document.getElementById("str_sysid").innerHTML;            
            //str_title = "修改";
            $.ajax({
                url: '0030010099.aspx/Load_Data',
                type: 'POST',
                data: JSON.stringify({ Case_ID: Case_ID }),
                contentType: 'application/json; charset=UTF-8',
                dataType: "json",       //如果要回傳值，請設成 json
                success: function (doc) {
                    var text = '{"data":' + doc.d + '}';
                    var obj = JSON.parse(text);
                    
                    //document.getElementById("Textarea2").value = obj.data[0].Close_Out_PS;    //範例

                    //document.getElementById("str_sysid").innerHTML = obj.data[0].str_sysid ;      //已經有了
                    document.getElementById("Text1").value = obj.data[0].Text1;
                    document.getElementById("Text2").value = obj.data[0].Text2;     // 來電號碼+建單人            
                    //document.getElementById("LoginTime").value = obj.data[0].str_login_time ; //時間會自動抓

                    //document.getElementById("DropClientCode").value = obj.data[0].str_client_id;
                    style('DropClientCode', obj.data[0].str_client_id);     //選項用?
                    Client_Code_Search2();  //提取子公司選項
                    document.getElementById("str_id").innerHTML = obj.data[0].str_id;
                    document.getElementById("str_Client_Name").innerHTML = obj.data[0].str_B_Name;
                    document.getElementById("Service_Location").value = obj.data[0].str_addr;
                    document.getElementById("str_contact").value = obj.data[0].str_appname;
                    document.getElementById("str_mob_phone").value = obj.data[0].str_appmtel;
                    document.getElementById("str_telecomm_id").innerHTML = obj.data[0].str_T_ID;
                    document.getElementById("txt_Email").value = obj.data[0].str_appemail;
                    document.getElementById("str_hardware").innerHTML = obj.data[0].str_hardware;
                    document.getElementById("str_software").innerHTML = obj.data[0].str_software;
                    document.getElementById("str_serv_typ").innerHTML = obj.data[0].str_serviceitem;

                    //document.getElementById("DropSubsidiaryCode").value = obj.data[0].str_pid_2;
                    style('DropSubsidiaryCode', obj.data[0].str_pid_2);
                    document.getElementById("str_Client_Name_2").innerHTML = obj.data[0].str_Name_2;
                    document.getElementById("str_contact_2").value = obj.data[0].str_appname_2;
                    document.getElementById("str_ID_2").innerHTML = obj.data[0].str_id_2;
                    document.getElementById("str_mob_phone_2").value = obj.data[0].str_appmtel_2;
                    document.getElementById("str_C_addr_2").value = obj.data[0].str_addr_2;
                    document.getElementById("str_hardware_2").innerHTML = obj.data[0].str_hardware_2;
                    document.getElementById("str_software_2").innerHTML = obj.data[0].str_software_2;

                    //document.getElementById("SelectUrgency").value = obj.data[0].str_urgency;
                    //alert(obj.data[0].str_op_type)
                    style('SelectUrgency', obj.data[0].str_urgency);
                    document.getElementById("datetimepicker01").value = obj.data[0].str_onspot_time;
                    //document.getElementById("SelectOpinionType").value = obj.data[0].str_op_type;
                    style('SelectOpinionType', obj.data[0].str_op_type);
                    //document.getElementById("ReplyType").value = obj.data[0].str_rep_type;
                    style('ReplyType', obj.data[0].str_rep_type);
                    document.getElementById("Opinion").value = obj.data[0].str_opinion;
                    //document.getElementById("DealingProcess").innerHTML = obj.data[0].str_type;
                    document.getElementById("A_ID").innerHTML = obj.data[0].str_A_ID;
                    document.getElementById("A_N").innerHTML = obj.data[0].str_Handle_Agent;
                    //document.getElementById("PS").value = obj.data[0].str_ps;
                    document.getElementById("Reply").value = obj.data[0].str_rep;
                    //document.getElementById("Label1").innerHTML = obj.data[0].str_R_Time;
                    //document.getElementById("Label2").innerHTML = obj.data[0].str_F_Time;
                    document.getElementById("SetupTime").innerHTML = obj.data[0].str_S_Time;
                    if (obj.data[0].str_A_ID != "") {
                        document.getElementById('Button4').style.display = "none";  //隱藏
                    }
                    else document.getElementById('Button5').style.display = "none";  //隱藏

                }
            });
            //document.getElementById('button_01').style.display = "none";  //客戶地址    額外的按鈕          
            
            $("#Div_Loading").modal('hide');        // 功能??
        }

        function Esti_Fin_Time() { }

        function ServiceChanged() {
            var s = document.getElementById("DropService");
            var str_value = s.options[s.selectedIndex].value;
            $.ajax({
                url: '0030010099.aspx/ServiceName_List',
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
                        $select_elem.append("<option value='" + obj.B + "'>" + obj.A + "</option>");
                    });
                    $select_elem.chosen(
                    {
                        width: "100%",
                        search_contains: true
                    });
                    $('.chosen-single').css({ 'background-color': '#ffffbb' });
                }
            });
        }///未使用

        function Client_Code_Search() {     //選客戶
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
                        $select_elem.append("<option value='" + obj.C + "'>【" + obj.B + "】" + obj.A + "</option>");
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

        function Client_Code_Search2() {    //選子公司
            var str_index2 = document.getElementById("DropClientCode").value;
            //alert(str_index2);
            $.ajax({
                url: '0030010099.aspx/Client_Code_Search2',
                type: 'POST',
                data: JSON.stringify({ PID: str_index2 }),
                contentType: 'application/json; charset=UTF-8',
                dataType: "json",
                success: function (doc) {
                    var json = JSON.parse(doc.d);
                    var $select_elem = $("#DropSubsidiaryCode");
                    $select_elem.chosen("destroy")
                    $select_elem.empty();
                    $select_elem.append("<option value=''>" + "請選擇" + "</option>");
                    $.each(json, function (idx, obj) {
                        $select_elem.append("<option value='" + obj.B + "'>" + obj.A + "</option>");     // 顯示客戶代碼選單
                    });
                    $select_elem.chosen(
                    {
                        width: "100%",
                        search_contains: true
                    });
                    //$('.chosen-single').css({ 'background-color': '#ffffbb' });
                }
            });
        }

        function ShowClientData() {     //選顧客後抓資料
            var str_index = document.getElementById("DropClientCode").value;
            Client_Code_Search2();     //子公司選單
            List_Record(str_index);
            $.ajax({
                url: '0030010099.aspx/Show_Client_Data',
                type: 'POST',
                data: JSON.stringify({ value: str_index }),
                contentType: 'application/json; charset=UTF-8',
                dataType: "json",
                success: function (doc) {
                    var obj = JSON.parse('{"data":' + doc.d + '}'); // 組合JSON 格式
                    document.getElementById("str_Client_Name").innerHTML = obj.data[0].C;
                    document.getElementById("str_contact").value = obj.data[0].D;//顯示成可更改的欄位
                    document.getElementById("str_mob_phone").value = obj.data[0].G;//顯示成可更改的欄位
                    document.getElementById("str_hardware").innerHTML = obj.data[0].H;
                    document.getElementById("str_software").innerHTML = obj.data[0].I;
                    document.getElementById("str_serv_typ").innerHTML = obj.data[0].J;
                    document.getElementById("str_telecomm_id").innerHTML = obj.data[0].K;
                    document.getElementById("Service_Location").value = obj.data[0].L;
                    //document.getElementById("txt_memo").innerHTML = obj.data[0].M;
                    document.getElementById("txt_Email").value = obj.data[0].N;
                    document.getElementById("str_id").innerHTML = obj.data[0].O;
                }
            });
        }

        function ShowSubsidiaryData() {
            var str_index_2 = document.getElementById("DropSubsidiaryCode").value;
            $.ajax({
                url: '0030010099.aspx/Show_Client_Data2',
                type: 'POST',
                data: JSON.stringify({ value_2: str_index_2 }),
                contentType: 'application/json; charset=UTF-8',
                dataType: "json",
                success: function (doc) {
                    var obj = JSON.parse('{"data":' + doc.d + '}'); // 組合JSON 格式
                    document.getElementById("str_ID_2").innerHTML = obj.data[0].B;
                    document.getElementById("str_Client_Name_2").innerHTML =  obj.data[0].C;
                    document.getElementById("str_contact_2").value = obj.data[0].D;
                    document.getElementById("str_C_addr_2").value = obj.data[0].E;
                    //document.getElementById("str_fax_phone_2").value = obj.data[0].F;
                    document.getElementById("str_mob_phone_2").value = obj.data[0].G;
                    document.getElementById("str_hardware_2").innerHTML = obj.data[0].H;
                    document.getElementById("str_software_2").innerHTML = obj.data[0].I;
                }
            });
        }

        function List_Record(id) {  //列出顧客案件
            $.ajax({
                url: '0030010099.aspx/List_Record',
                type: 'POST',
                data: JSON.stringify({ id: id }),
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
                                { data: "A" },
                                { data: "B" },
                                { data: "C" },
                                { data: "D" },
                                { data: "E" },
                                { data: "F" },
                                { data: "G" }
                        ]
                    });
                }
            });
        }

        function Data_Save() {

            var str_sysid = document.getElementById("str_sysid").innerHTML;
            var Text1 = document.getElementById("Text1").value;
            var Text2 = document.getElementById("Text2").value;     // 來電號碼+建單人            
            var str_login_time = document.getElementById("LoginTime").value;

            var str_client_id = document.getElementById("DropClientCode").value;
            var str_id = document.getElementById("str_id").innerHTML;
            var str_B_Name = document.getElementById("str_Client_Name").innerHTML;
            var str_addr = document.getElementById("Service_Location").value;
            var str_appname = document.getElementById("str_contact").value;
            var str_appmtel = document.getElementById("str_mob_phone").value;
            var str_T_ID = document.getElementById("str_telecomm_id").innerHTML;
            var str_appemail = document.getElementById("txt_Email").value;
            var str_hardware = document.getElementById("str_hardware").innerHTML;
            var str_software = document.getElementById("str_software").innerHTML;
            var str_serviceitem = document.getElementById("str_serv_typ").innerHTML;

            var str_pid_2 = document.getElementById("DropSubsidiaryCode").value;
            var str_Name_2 = document.getElementById("str_Client_Name_2").innerHTML;
            var str_appname_2 = document.getElementById("str_contact_2").value;
            var str_id_2 = document.getElementById("str_ID_2").innerHTML;
             var str_appmtel_2 = document.getElementById("str_mob_phone_2").value;
            var str_addr_2 = document.getElementById("str_C_addr_2").value;           
            var str_hardware_2 = document.getElementById("str_hardware_2").innerHTML;
            var str_software_2 = document.getElementById("str_software_2").innerHTML;

            var str_urgency = document.getElementById("SelectUrgency").value;
            var str_onspot_time = document.getElementById("datetimepicker01").value;
            var str_op_type = document.getElementById("SelectOpinionType").value;
            var str_rep_type = document.getElementById("ReplyType").value;
            var str_opinion = document.getElementById("Opinion").value;
            //var str_type = document.getElementById("DealingProcess").innerHTML;
            var str_A_ID = document.getElementById("A_ID").innerHTML;
            var str_Handle_Agent = document.getElementById("A_N").innerHTML;
            //var str_ps = document.getElementById("PS").value;
            
            var str_rep = document.getElementById("Reply").value;
            //alert(str_agent_id);
            //var e = document.getElementById("Dispatch_Name");
            //var str_agent_name = e.options[e.selectedIndex].text;
            //$("#Div_Loading").modal();
            $.ajax({
                url: '0030010099.aspx/Case_Save',
                type: 'POST',
                data: JSON.stringify({
                    sysid: str_sysid,
                    Caller_ID: Text1,
                    Creat_Agent: Text2,
                    login_time: str_login_time,

                    PID: str_client_id,
                    ID: str_id,
                    BUSINESSNAME: str_B_Name,
                    ADDR: str_addr,
                    APPNAME: str_appname,
                    APP_MTEL: str_appmtel,
                    Telecomm_ID: str_T_ID,
                    APP_EMAIL: str_appemail,
                    HardWare: str_hardware,
                    SoftwareLoad: str_software,
                    SERVICEITEM: str_serviceitem,

                    PNumber: str_pid_2,
                    Name: str_Name_2,
                    APPNAME_2: str_appname_2,
                    ID_2: str_id_2,
                    APP_MTEL_2: str_appmtel_2,
                    ADDR_2: str_addr_2,
                    HardWare_2: str_hardware_2,
                    SoftwareLoad_2: str_software_2,
                    
                    urgency: str_urgency,
                    onspot_time: str_onspot_time,
                    op_type: str_op_type,
                    rep_type: str_rep_type,
                    opinion: str_opinion,
                    //type: str_type,
                    A_ID: str_A_ID,
                    Handle_Agent: str_Handle_Agent,
                    //ps: str_ps,

                    rep: str_rep,
                    //agent_name: str_agent_name
                }),
                contentType: 'application/json; charset=UTF-8',
                dataType: "json",
                success: function (doc) {
                    var json = JSON.parse(doc.d.toString());
                    if (json.status == "new") {
                        //alert('新增完成');
                        URL();
                        //window.location.href = "/0030010000/0030010099.aspx";     //網址錯了 卻會轉跳?
                    }
                    else if (json.status == "update") {
                        //alert('修改完成');
                        URL();
                        //window.location.href = "/0030010000/0030010099.aspx";   //網址錯了 卻會轉跳?
                    }
                    else {
                        alert(json.status);
                    }
                    $("#Div_Loading").modal('hide');
                },
                error: function () { alert("新增服務單發生錯誤"); }
                //error: function () { alert("請填完所有必填欄位"); }
            });
        }

        function Data_Update() {

            var str_sysid = document.getElementById("str_sysid").innerHTML;
            var Text1 = document.getElementById("Text1").value;
            var Text2 = document.getElementById("Text2").value;     // 來電號碼+建單人            
            var str_login_time = document.getElementById("LoginTime").value;

            var str_client_id = document.getElementById("DropClientCode").value;
            var str_id = document.getElementById("str_id").innerHTML;
            var str_B_Name = document.getElementById("str_Client_Name").innerHTML;
            var str_addr = document.getElementById("Service_Location").value;
            var str_appname = document.getElementById("str_contact").value;
            var str_appmtel = document.getElementById("str_mob_phone").value;
            var str_T_ID = document.getElementById("str_telecomm_id").innerHTML;
            var str_appemail = document.getElementById("txt_Email").value;
            var str_hardware = document.getElementById("str_hardware").innerHTML;
            var str_software = document.getElementById("str_software").innerHTML;
            var str_serviceitem = document.getElementById("str_serv_typ").innerHTML;

            var str_pid_2 = document.getElementById("DropSubsidiaryCode").value;
            var str_Name_2 = document.getElementById("str_Client_Name_2").innerHTML;
            var str_appname_2 = document.getElementById("str_contact_2").value;
            var str_id_2 = document.getElementById("str_ID_2").innerHTML;
            var str_appmtel_2 = document.getElementById("str_mob_phone_2").value;
            var str_addr_2 = document.getElementById("str_C_addr_2").value;
            var str_hardware_2 = document.getElementById("str_hardware_2").innerHTML;
            var str_software_2 = document.getElementById("str_software_2").innerHTML;

            var str_urgency = document.getElementById("SelectUrgency").value;
            var str_onspot_time = document.getElementById("datetimepicker01").value;
            var str_op_type = document.getElementById("SelectOpinionType").value;
            var str_rep_type = document.getElementById("ReplyType").value;
            var str_opinion = document.getElementById("Opinion").value;
            //var str_type = document.getElementById("DealingProcess").innerHTML;
            var str_A_ID = document.getElementById("A_ID").innerHTML;
            var str_Handle_Agent = document.getElementById("A_N").innerHTML;
            //var str_ps = document.getElementById("PS").value;

            var str_rep = document.getElementById("Reply").value;
            //alert(str_agent_id);
            //var e = document.getElementById("Dispatch_Name");
            //var str_agent_name = e.options[e.selectedIndex].text;
            //$("#Div_Loading").modal();
            $.ajax({
                url: '0030010099.aspx/Case_Update',
                type: 'POST',
                data: JSON.stringify({
                    sysid: str_sysid,
                    Caller_ID: Text1,
                    Creat_Agent: Text2,
                    login_time: str_login_time,

                    PID: str_client_id,
                    ID: str_id,
                    BUSINESSNAME: str_B_Name,
                    ADDR: str_addr,
                    APPNAME: str_appname,
                    APP_MTEL: str_appmtel,
                    Telecomm_ID: str_T_ID,
                    APP_EMAIL: str_appemail,
                    HardWare: str_hardware,
                    SoftwareLoad: str_software,
                    SERVICEITEM: str_serviceitem,

                    PNumber: str_pid_2,
                    Name: str_Name_2,
                    APPNAME_2: str_appname_2,
                    ID_2: str_id_2,
                    APP_MTEL_2: str_appmtel_2,
                    ADDR_2: str_addr_2,
                    HardWare_2: str_hardware_2,
                    SoftwareLoad_2: str_software_2,

                    urgency: str_urgency,
                    onspot_time: str_onspot_time,
                    op_type: str_op_type,
                    rep_type: str_rep_type,
                    opinion: str_opinion,
                    //type: str_type,
                    A_ID: str_A_ID,
                    Handle_Agent: str_Handle_Agent,
                    //ps: str_ps,

                    rep: str_rep,
                    //agent_name: str_agent_name
                }),
                contentType: 'application/json; charset=UTF-8',
                dataType: "json",
                success: function (doc) {
                    var json = JSON.parse(doc.d.toString());
                    if (json.status == "new") {
                        alert('新增完成');
                        window.location.href = "/0030010000/0030010003.aspx";
                    }
                    else if (json.status == "update") {
                        alert('修改完成');
                        //URL();
                        //window.location.href = "/0030010000/0030010003.aspx";
                    }
                    else {
                        alert(json.status);
                    }
                    $("#Div_Loading").modal('hide');
                },
                error: function () { alert("修改服務單發生錯誤"); }
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
        //=========================================
        function Load_MNo() {
            $("#Div_Loading").modal();
            $.ajax({
                url: '0030010099.aspx/Load_MNo',
                type: 'POST',
                data: JSON.stringify({ MNo: new_mno }),
                contentType: 'application/json; charset=UTF-8',
                dataType: "json",       //如果要回傳值，請設成 json
                success: function (doc) {
                    var text = '{"data":' + doc.d + '}';
                    var obj = JSON.parse(text); str_pid
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
                    //document.getElementById("str_name").innerHTML = obj.data[0].Create_Name;
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
                    document.getElementById("txt_CarSeat").value = obj.data[0].CarSeat;
                    document.getElementById("Question").value = obj.data[0].Question;
                    document.getElementById("datetimepicker01").value = obj.data[0].Time_01.replace(/-/g, '/').substr(0, 10) + ' ' + obj.data[0].Time_01.substr(11, 5);
                    document.getElementById("datetimepicker02").value = obj.data[0].Time_02.replace(/-/g, '/').substr(0, 10) + ' ' + obj.data[0].Time_02.substr(11, 5);

                }
            });
            $("#Div_Loading").modal('hide');
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

        function List_Dispatch_Team() {     //選派工部門選項(停用)
            $.ajax({
                url: '0030010099.aspx/List_Dispatch_Team',
                type: 'POST',
                contentType: 'application/json; charset=UTF-8',
                dataType: "json",
                success: function (doc) {
                    var json = JSON.parse(doc.d);
                    var $select_elem = $("#Chose_Company");
                    $select_elem.chosen("destroy")
                    $.each(json, function (idx, obj) {
                        $select_elem.append("<option value='" + obj.A + "'>" + obj.A + "</option>");
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

        function Change_Dispatch() {
            //var value = document.getElementById("Chose_Company").value;
            var value = "Engineer";
            $.ajax({
                url: '0030010099.aspx/List_Dispatch_Name',
                type: 'POST',
                data: JSON.stringify({ value: value }),
                contentType: 'application/json; charset=UTF-8',
                dataType: "json",
                success: function (doc) {
                    var json = JSON.parse(doc.d);
                    var $select_elem = $("#Dispatch_Name");     //派公人員選項
                    $select_elem.chosen("destroy")
                    $select_elem.empty();
                    $select_elem.append("<option value=''>" + "請選擇派工人員…" + "</option>");
                    $.each(json, function (idx, obj) {
                        $select_elem.append("<option value='" + obj.B + "'>" + obj.A + "</option>");
                        //document.getElementById("A_ID").value = obj.B;      //data[0].
                        //document.getElementById("Handle_Agent").value = obj.A;      //data[0].
                    });
                    $select_elem.chosen(
                    {
                        width: "100%",
                        search_contains: true
                    });                                        
                    //$('.chosen-single').css({ 'background-color': '#ffffbb' });
                }
            });
        }

        // ===================確認新增頁面派工
        function Send_Save() {
            var str_sysid = document.getElementById("str_sysid").innerHTML;
            var value = document.getElementById("Dispatch_Name").value;
            //var PS = document.getElementById("Agent_PS").value;
            var C_Name = document.getElementById("str_Client_Name").innerHTML;
            var S_O_Type = document.getElementById("SelectOpinionType").value;
            //document.getElementById('Button5').style.display = "none";

            if (confirm("確認要派工並通知工程師嗎？"+"\n"+"(即使取消新增也會發送mail)")) {
                $.ajax({
                    url: '0030010099.aspx/Send_Save',
                    ache: false,
                    type: 'POST',
                    //async: false,
                    data: JSON.stringify({ sysid: str_sysid, value: value, C_Name: C_Name, S_O_Type: S_O_Type }),
                    contentType: 'application/json; charset=UTF-8',
                    dataType: "json",       //如果要回傳值，請設成 json
                    success: function (data) {
                        var json = JSON.parse(data.d.toString());
                        if (json.status == "success") {
                            Show(value);          //顯示 處理人員                             
                            alert("派工成功");
                            document.getElementById('Button6').style.display = "none";      //隱藏派工紐
                        }
                        else alert(status);
                    }
                });
            }   //*/            
        };

        // ===================確認派工
        function Send() {
            var str_sysid = document.getElementById("str_sysid").innerHTML;
            var value = document.getElementById("Dispatch_Name").value;
            //var PS = document.getElementById("Agent_PS").value;
            var C_Name = document.getElementById("str_Client_Name").innerHTML;
            var S_O_Type = document.getElementById("SelectOpinionType").value;
            //document.getElementById('Button5').style.display = "none";
            
            if (confirm("確認要派工並通知工程師嗎？")) {
                $.ajax({
                    url: '0030010099.aspx/Send',
                    ache: false,
                    type: 'POST',
                    //async: false,
                    data: JSON.stringify({ sysid: str_sysid, value: value, C_Name: C_Name, S_O_Type: S_O_Type }),
                    contentType: 'application/json; charset=UTF-8',
                    dataType: "json",       //如果要回傳值，請設成 json
                    success: function (data) {
                        var json = JSON.parse(data.d.toString());
                        if (json.status == "success") {   
                            Show(value);          //顯示 處理人員                             
                            alert("派工成功");
                            document.getElementById('Button4').style.display = "none";      //隱藏派工紐
                            document.getElementById('Button5').style.display = "";
                        }
                        else alert(status);
                    }
                });
            }   //*/            
        };

        // ===================確認 轉派
        function Change() {
            var str_sysid = document.getElementById("str_sysid").innerHTML;
            var value = document.getElementById("Dispatch_Name").value;
            //var PS = document.getElementById("Agent_PS").value;
            var C_Name = document.getElementById("str_Client_Name").innerHTML;
            var S_O_Type = document.getElementById("SelectOpinionType").value;
            var value2 = document.getElementById("A_ID").innerHTML;
            //alert(value2);
            if (confirm("確認要轉派工並發送通知嗎？")) {
                $.ajax({
                    url: '0030010099.aspx/Change',
                    ache: false,
                    type: 'POST',
                    //async: false,
                    data: JSON.stringify({
                        sysid: str_sysid,
                        value: value,
                        //PS: PS,
                        C_Name: C_Name,
                        S_O_Type: S_O_Type,
                        value2: value2
                    }),
                    contentType: 'application/json; charset=UTF-8',
                    dataType: "json",       //如果要回傳值，請設成 json
                    success: function (data) {
                        var json = JSON.parse(data.d.toString());
                        if (json.status == "success") {
                            //bindTable();
                            Show(value);    //顯示 處理人員                            
                            alert("轉派工完畢");
                        }
                        else alert(status);
                    }
                });
            }   //*/            
        };

        //============ 確認轉派公
        /*       function Change() {
                   if (confirm("確認轉派工並通知嗎？")) {
                       $.ajax({
                           url: '0060010004.aspx/Delete',
                           ache: false,
                           type: 'POST',
                           //async: false,
                           data: JSON.stringify({ Agent_ID: Agent_ID }),
                           contentType: 'application/json; charset=UTF-8',
                           dataType: "json",       //如果要回傳值，請設成 json
                           success: function (data) {
                               var json = JSON.parse(data.d.toString());
                               if (json.status == "success") {
                                   bindTable();
                               }
                           }
                       });
                   }
               };      //*/

        function URL() {
            var Case_ID = document.getElementById("str_sysid").innerHTML;
            $.ajax({
                url: '0030010099.aspx/URL',
                type: 'POST',
                data: JSON.stringify({ Case_ID: Case_ID }),
                contentType: 'application/json; charset=UTF-8',
                dataType: "json",
                success: function (doc) {
                    var json = JSON.parse(doc.d.toString());
                    if (json.type == "ok") {
                        window.location = json.status;      //跳到status 紀錄的網址
                    } else {
                        alert(json.status);     //顯示status 紀錄的訊息
                    }
                }
            });
        }

        function Show(value) {
            $.ajax({
                url: '0030010099.aspx/Show',
                type: 'POST',
                data: JSON.stringify({ value: value }),
                contentType: 'application/json; charset=UTF-8',
                dataType: "json",
                success: function (doc) {                    
                        var text = '{"data":' + doc.d + '}';
                        var obj = JSON.parse(text);                        
                        document.getElementById("A_ID").innerHTML = obj.data[0].B;      //data[0].
                        document.getElementById("A_N").innerHTML = "  " + obj.data[0].A;
                        //alert("A=" + A_ID);
                        //if (!A_ID.IsNullOrEmpty) document.getElementById('Button5').style.display = "";  //顯示轉派鍵                           
                }
            });
        }       
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
    <!-- ====== Loading ====== -->
    <div style="width: 1280px; margin: 10px 20px">
        <h2><label id="str_title"></label></h2>
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
                    <td style="text-align: center; width: 15%;">
                        <strong>來電號碼</strong>
                    </td>
                    <td>
                        <div style="float: left" data-toggle="tooltip" title="">
                            <input type="text" class="form-control" id="Text1" name="Text1" maxlength="20" value="" />
                        </div>
                    </td>
                </tr>
                <tr id="tr_sysid" runat="server">
                    <td style="text-align: center">
                        <strong>最後修改日期</strong>
                    </td>
                    <td>
                        <div style="float: left" data-toggle="tooltip" title="必填">
                            <input type="text" class="form-control" id="LoginTime" name="LoginTime" style="background-color: #ffffbb" value="" />
                        </div>
                    </td>
                    <td style="text-align: center">
                        <strong>登錄人員</strong>
                    </td>
                    <td>
                        <div style="float: left" data-toggle="tooltip" title="必填">
                            <input type="text" class="form-control" id="Text2" name="Text2" maxlength="20" style="background-color: #ffffbb" value="" />
                        </div>
                    </td>
                </tr>
            </tbody>
        </table>
        <table class="table table-bordered table-striped">
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
                        <strong>客戶代碼
                        </strong>
                    </th>
                    <th style="width: 35%">

                        <div data-toggle="tooltip" title="必選" style="width: 100%">
                            <select id="DropClientCode" name="DropClientCode" class="chosen-select" onchange="ShowClientData()">
                                <option value="">請選擇客戶…</option>
                            </select>
                        </div>
                    </th>
                    <th style="text-align: center; width: 15%">統一編號</th>   <!-- 發送 E-Mail-->
                    <th style="width: 35%">
                        <label id="str_id"></label>
                        <!-- <input type="checkbox" name="e-mail" style="width: 25px; height: 25px;" value="1" />-->
                    </th>
                </tr>
                <%--  =========== 勞工資料 ===========--%>
                <tr style="height: 55px;">
                    <td style="text-align: center; width: 15%">
                        <strong>客戶名稱</strong>
                    </td>
                    <td style="width: 35%">
                        <label id="str_Client_Name"></label>

                    </td>

                    <th style="text-align: center; width: 15%">
                        <strong>叫修地址</strong>
                    </th>
                    <th style="width: 35%">
                        <div style="float: left" data-toggle="tooltip" title="必填，不能超過100 個字，且不能含有不正確的符號">
                            <textarea id="Service_Location" name="Service_Location" class="form-control" cols="45" rows="1" placeholder="叫修地址" maxlength="100" onkeyup="cs(this);"
                                style="resize: none; background-color: #ffffbb"></textarea>
                        </div>
                    </th>
                </tr>
                <%--  =========== 勞工資料 ===========--%>
                <tr style="height: 55px;">
                    <td style="text-align: center">
                        <strong>聯絡人</strong>
                    </td>
                    <td>
                        <!--<label id="str_contact"></label>-->
                        <div style="float: left" data-toggle="tooltip" title="必填">
                            <input type="text" class="form-control" id="str_contact" name="Contact" style="background-color: #ffffbb" maxlength="25" value="" />
                        </div>
                    </td>
                    <td style="text-align: center">
                        <strong>聯絡人電話</strong>
                    </td>
                    <td>
                        <!--<label id="str_mob_phone"></label>-->
                        <div style="float: left" data-toggle="tooltip" title="必填">
                            <input type="text" class="form-control" id="str_mob_phone" name="Contact_mob_phone" style="background-color: #ffffbb" maxlength="25" value="" />
                        </div>
                    </td>
                </tr>
                <%--  =========== 勞工資料 ===========--%>
                <tr style="height: 55px;">
                    <td style="text-align: center">
                        <strong>負責電信商</strong>
                    </td>
                    <td>
                        <label id="str_telecomm_id"></label>
                    </td>
                    <td style="text-align: center">
                        <strong>客戶 E-mail</strong>
                    </td>
                    <td>
                        <input type="text" class="form-control" id="txt_Email" name="txt_Email" maxlength="100" value="" />
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
                        <strong>合約狀態</strong>
                    </td>
                    <td>
                        <label id="str_serv_typ"></label>
                    </td>
                    <td style="text-align: center">
                        <strong><br />
                            </strong>    <!--（搭配經銷商） -->
                    </td>
                    <td>
                        <label id="Test"></label>
                    </td>
                </tr>
                <%--  =========== 勞工資料 ===========--%>
            </tbody>
        </table>

        <%--  =========== 子公司資料 表單三===========--%>
    <div style="text-align: center; width: 1280px; margin: 10px 20px">
        <table class="table table-bordered table-striped">                      
            <thead>
                <tr>
                    <th style="text-align: center" colspan="4">
                        <span style="font-size: 20px"><strong>子公司資料</strong></span>
                    </th>
                </tr>
            </thead>
            <tbody>
                <tr style="height: 55px;">
                    <th style="text-align: center; width: 15%">
                        <strong>子公司選擇
                            <br />(以名稱搜尋)                            
                        </strong>
                    </th>
                    <th style="width: 35%">                        
                        <div data-toggle="tooltip" title="非必選" style="width: 100%">
                            <select id="DropSubsidiaryCode" name="DropSubsidiaryCode" class="chosen-select" onchange="ShowSubsidiaryData()">
                                <option value="">請選擇</option>
                            </select>
                        </div>
                    </th>
                    <th style="text-align: center; width: 15%">
                        <strong>子公司名稱</strong>
                    </th>
                    <th style="text-align: center; width: 15%">                        
                        <label id="str_Client_Name_2"></label>
                    </th>
                </tr>
                <tr style="height: 55px;">                                                          <%--  Table 002-2  --%>
                    <td style="text-align: center">
                        <strong>(子)聯絡人</strong>
                    </td>
                    <td>
                        <input type="text" class="form-control" id="str_contact_2" name="str_contact" maxlength="25"  placeholder="(子)聯絡人"/>  
                    </td>
                    <td style="text-align: center">
                        <strong>(子)統一編號</strong>
                    </td>
                    <td>
                        <label id="str_ID_2"></label><br />
                    </td>

                    
                </tr>
                <tr style="height: 55px;">
                    <td style="text-align: center">
                        <strong>(子)聯絡電話</strong>
                    </td>
                    <td>
                        <input type="text" class="form-control" id="str_mob_phone_2" name="str_contact" maxlength="25"  placeholder="(子)聯絡電話"/>  
                    </td>
                    <td style="text-align: center; width: 15%">
                        <strong>(子)地址</strong>
                    </td>
                    <td style="width: 35%">                                         
                        <div style="float: left" data-toggle="tooltip" title="不能超過100 個字，且不能含有不正確的符號">      
                                <textarea id="str_C_addr_2" name="str_C_addr_2" class="form-control" cols="45" rows="1" placeholder="地址" maxlength="100" onkeyup="cs(this);"
                                style="resize: none; "  ></textarea>
                        </div>
                    </td>
                </tr>
                <tr style="height: 55px;">
                    <td style="text-align: center">
                        <strong>(子)硬體</strong>
                    </td>
                    <td>
                        <label id="str_hardware_2"></label>
                    </td>
                    <td style="text-align: center">
                        <strong>(子)軟體</strong>
                    </td>
                    <td>
                        <label id="str_software_2"></label>
                    </td>
                </tr>
            </tbody>
        </table>
    </div>


        <h2><strong>客戶歷史紀錄</strong></h2>
        <table id="tab_Record" class="display table table-striped" style="width: 99%">
            <thead>
                <tr>
                    <th style="text-align: center;">案件編號</th>
                    <th style="text-align: center;">登單時間</th>
                    <th style="text-align: center;">客戶名稱</th>
                    <th style="text-align: center;">意見內容</th>
                    <th style="text-align: center;">備註</th>
                    <th style="text-align: center;">處理回覆</th>
                    <th style="text-align: center;">案件狀態</th>
                </tr>
            </thead>
        </table>
        <table class="table table-bordered table-striped">
            <thead>
                <tr>
                    <th style="text-align: center" colspan="4">
                        <span style="font-size: 20px"><strong>反應內容</strong></span>
                    </th>
                </tr>
            </thead>
            <tbody>
                <tr style="height: 55px;">
                    <th style="text-align: center; width: 15%">
                        <strong>緊急程度</strong>
                    </th>
                    <th style="width: 35%">
                        <div data-toggle="tooltip" title="必選" style="width: 100%">
                            <select id="SelectUrgency" name="SelectUrgency" class="chosen-select" onchange="Esti_Fin_Time()">
                                <option value="">請選擇緊急程度…</option>
                                <option value="維護">維護</option>
                                <option value="緊急故障">緊急故障</option>
                                <option value="重要故障">重要故障</option>
                                <option value="一般故障">一般故障</option>
                            </select>
                        </div>
                    </th>
                    <td style="text-align: center; width: 15%">
                        <strong>預計到點</strong>     <!--預定完成日期 -->
                    </td>
                    <td style="width: 35%">
                        <div style="float: left" data-toggle="tooltip" title="">
                            <input type="text" class="form-control" id="datetimepicker01" name="datetimepicker01"  />
                        </div>
                        <!--<label id=""></label>    EstimatedFinishTime -->
                    </td>

                </tr>
                <tr style="height: 55px;">
                    <th style="text-align: center; width: 15%">
                        <strong>意見類型</strong>
                    </th>
                    <th style="width: 35%">
                        <div data-toggle="tooltip" title="必選" style="width: 100%">
                            <select id="SelectOpinionType" name="SelectOpinionType" class="chosen-select">
                                <option value="">請選擇意見類型…</option>
                                <option value="故障報修">故障報修</option>
                                <option value="軟體修改">軟體修改</option>
                                <option value="技術諮詢">技術諮詢</option>
                                <option value="其他服務">其他服務</option>
                                <option value="定期維護">定期維護</option>
                                <option value="測試">測試</option>
                                <option value="專案">專案</option>
                                <option value="設備擴充">設備擴充</option>
                                <option value="駐點服務">駐點服務</option>
                            </select>
                        </div>
                    </th>
                    <td style="text-align: center">
                        <strong>回覆方式</strong>
                    </td>
                    <td>
                        <div data-toggle="tooltip" title="必選" style="width: 100%">
                            <select id="ReplyType" name="ReplyType" class="chosen-select">
                                <option value="">請選擇回覆方式…</option>
                                <option>行動電話</option>
                                <option>市內電話</option>
                                <option>e-mail回覆</option>
                                <option>傳真回覆</option>
                            </select>
                        </div>
                    </td>
                </tr>
                <tr style="height: 55px;">
                    <td style="text-align: center">
                        <strong>意見內容</strong>
                    </td>
                    <td>
                        <div style="float: left" data-toggle="tooltip" title="必填，不能超過250 個字元，並且含有不正確的符號">
                            <textarea id="Opinion" name="Opinion" class="form-control" cols="45" rows="3" placeholder="意見內容" maxlength="250" onkeyup="cs(this);"
                                style="resize: none; background-color: #ffffbb"></textarea>
                        </div>
                    </td>
                    <td style="text-align: center">
                        <strong>處理人員</strong>
                    </td>
                    <td>
                         <%-- <strong>員工編號: </strong>--%>
                        <label id="A_ID"></label><label id="A_N"></label><br />
                        <%-- <label id="Handle_Agent"></label>             --%>           
                    </td>                    
                </tr>
                <!--
                <tr style="height: 55px;">
                    <td style="text-align: center">
                        <strong>到點時間</strong>
                    </td>
                    <td>
                        <label id="Label1"></label>
                    </td>
                    <td style="text-align: center">
                        <strong>處理狀態</strong>
                    </td>
                    <th>
                        <label id="DealingProcess"></label>
                    </th>
                </tr>

                <tr style="height: 55px;">
                    <td style="text-align: center">
                        <strong>完成時間</strong>
                    </td>
                    <td>
                        <label id="Label2"></label>
                    </td>
                    <td style="text-align: center">
                        <strong>完成備註</strong>
                    </td>
                    <td>
                        <div style="float: left" data-toggle="tooltip" title="">
                            <textarea id="PS" name="PS" class="form-control" cols="45" rows="3" placeholder="結案時工程師填" maxlength="400" onkeyup="cs(this);"
                                style="resize: none;"></textarea>
                        </div>
                    </td>
                </tr>       -->
            </tbody>
        </table>
        
        <!-- 派工人員選擇-->
         <table class="table table-bordered table-striped">
            <thead>
                <tr>
                    <th style="text-align: center" colspan="4">
                        <span style="font-size: 20px"><strong>派工人員</strong></span>
                    </th>
                </tr>
            </thead>
            <tbody>
                <tr style="height: 55px;">
                    <th style="text-align: center; width: 15%">
                        <strong>派工部門</strong>
                    </th>
                    <th style="width: 35%">
                        <label id="Chose_Company"></label>
                        <!--
                        <select id="Chose_Company" name="Chose_Company" class="chosen-select" onchange="Change_Dispatch()">
                            <option value="">請選擇派工部門…</option>
                        </select> -->
                    </th>
                    <th style="text-align: center; width: 15%">
                        <strong>派工人員</strong>
                    </th>
                    <th style="width: 35%">
                        <select id="Dispatch_Name" name="Dispatch_Name" class="chosen-select" onchange="">
                            <option value="">請選擇派工人員…</option>
                        </select>
                    </th>
                </tr>
  <!--              <tr>
                    <td style="text-align: center">
                        <strong>派工備註</strong>
                    </td>
                    <td>
                        <div style="float: left" data-toggle="tooltip" title="最多50字">
                            <textarea id="Agent_PS" name="Agent_PS" class="form-control" cols="45" rows="3" placeholder="派工備註" maxlength="50" onkeyup="cs(this);"
                                style="resize: none;"></textarea>
                        </div>
                    </td>
                </tr>   -->
            </tbody>
        </table>
        <!-- -->
        <div style="text-align: center">
            <button id="Button6" type="button" onclick="Send_Save();" class="btn btn-default btn-lg ">&nbsp;派工&nbsp;</button>   
            <button id="Button4" type="button" onclick="Send();" class="btn btn-success btn-lg ">&nbsp;派工&nbsp;</button>&nbsp;&nbsp
            <button id="Button5" type="button" onclick="Change();" class="btn btn-default btn-lg ">&nbsp;轉派工&nbsp;</button>
        </div>
        <!-- --> 
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
                    <th style="width: 35%">
                        <div style="float: left" data-toggle="tooltip" title="不能超過250 個字元，並且含有不正確的符號">
                            <textarea id="Reply" name="Reply" class="form-control" cols="45" rows="3" placeholder="客服處理 回覆內容" maxlength="250" onkeyup="cs(this);"
                                style="resize: none;"></textarea>
                        </div>
                    </th>
                    <th style="text-align: center; width: 15%">
                        <strong>建單時間</strong>
                    </th>
                    <th style="text-align: center; width: 35%">
                        <label id="SetupTime"></label>
                    </th>
                </tr>

            </tbody>
        </table>

       
 <%--        <table class="table table-bordered table-striped">
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
                        <strong>滿意度</strong>
                    </th>
                    <th style="width: 35%"></th>
                    <th style="text-align: center; width: 15%">
                        <strong>結案日期</strong>
                    </th>
                    <th style="width: 35%"></th>
                </tr>
            </tbody>
        </table>    --%>
        <%--=========== 子單 ===========--%>

        <div style="text-align: center">
            <button id="Button1" type="button" onclick="Data_Save();" class="btn btn-success btn-lg "><span class="glyphicon glyphicon-ok"></span>確定&nbsp;&nbsp;</button>&nbsp;&nbsp
            <button id="Button2" type="button" onclick="Data_Update();" class="btn btn-success btn-lg "><span class="glyphicon glyphicon-pencil"></span>修改&nbsp;&nbsp;</button>&nbsp;&nbsp
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

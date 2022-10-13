<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="LineMsgList.aspx.cs" Inherits="LineMsgList" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="head2" Runat="Server">
    
    <script src="../DataTables/jquery.dataTables.min.js"></script>
    <script src="js/moment.min.js"></script>

    
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

        #data_company td:nth-child(2), #data_company td:nth-child(1),
        #data2 td:nth-child(3), #data2 td:nth-child(1),
        #data td:nth-child(6), #data td:nth-child(5), #data td:nth-child(4),
        #data td:nth-child(3), #data td:nth-child(2), #data td:nth-child(1) {
            text-align: center;
        }
        .auto-style1 {
            width: 182px;
        }

        #data th, #data td{
            text-align:center;
        }
        #agent-data th, #agent-data td{
            text-align:center;
        }

        .hotkey{
            position:fixed;
            /*right:0;*/
            top:60px;
            display:none;
        }
        .msgbar{
            position:fixed;
            bottom:40px;
            /*left:100px;*/
        }
        #navpill li a{
          color:black;
          font-size:25px;
          border:1px #aaa solid;
        }
        .NoReadCount{
            margin-left:10px;
            background-color:gold;
            border-radius: 50%;
        }
    </style>

    
    <div style="margin-left:100px;margin-right:100px;">
        <div class="">
            <h2 id="title">
                <strong>
                    LINE訊息紀錄
                </strong>
            </h2>
            <button type="button" class="btn btn-success btn-lg" onclick="location.href='LineMemberList.aspx'">返回</button>
        </div>

    </div>

    <div class="col-md-12" style="margin-top:10px;margin-bottom:30px;">
        <div id="userlist" class="col-md-3">
            <ul id="navpill" class="nav nav-pills nav-stacked">
                <li class="active"><a data-toggle="pill" href="#table_Level" style="display:none;">測試1</a></li>
                <li style="display:none;"><a data-toggle="pill" href="#table_CaseSource" onclick="">測試2<button disabled="disabled" class="NoReadCount">3</button></a></li>
            </ul>
        </div>
        <div class="col-md-9" <%--style="margin-left:100px;margin-right:100px;"--%>>
            <div class="" align="center">
                <input type="text" id="LMBID" style="                        display: none;
                "/>
                <input type="date" id="sdate" />
                ~
                <input type="date" id="edate" />
            </div>
            <div class="" style="margin-top:10px;margin-bottom:100px;" align="center">
                <table id="data" style="border:solid black 1px;width:100%;">
                
                </table>
            </div>


            <div id="msgbar" class="msgbar" style="width:100%;display:none;"> 
                <input id="msgtxt" type="text" style="width:68%;" />
                <button type="button" class="btn btn-warning" title="發送" style="padding-left:10px;" onclick="SendMsg()"><span class="glyphicon glyphicon-send"></span></button>
                <table style="width:100%;">
                    <tr>
                        <th style="width:80%;">
                            
                        </th>
                        <th style="padding-right:100px;">
                        </th>
                    </tr>
                </table>
            </div>
        </div>
    </div>

    <div class="hotkey">
        <button type="button" class="btn btn-info" onclick="location.href='#title';"><span class="glyphicon glyphicon-arrow-up"></span></button>
        <br />
        <button type="button" class="btn btn-info" onclick="location.href='#newest';"><span class="glyphicon glyphicon-arrow-down"></span></button>
    </div>



    <script type="text/javascript">
        var name = '';
        var nid = '0';
        var active = '';
        var useractive = '';
        var today = '';
        var checkUserListTime = '';

        $(function () {
            bindata();

            $('#sdate').change(function () {
                clearInterval(active);
                active = setInterval(function () { bindNewestMsg() }, 500);
                bindMsg();
            });
            $('#edate').change(function () {
                clearInterval(active);
                active = setInterval(function () { bindNewestMsg() }, 500);
                bindMsg();
            });
        });

        function bindUserList() {
            $.ajax({
                type: 'post',
                url: 'LineMsgList.aspx/bindUserList',
                contentType: 'application/json;utf-8',
                data: JSON.stringify({
                    time: checkUserListTime
                }),
                dataType: 'json',
                success: function (doc) {
                    let data = JSON.parse(doc.d);
                    //console.log(data);
                    let txt = '';
                    for (let i = 0; i < data.length; i++) {
                        txt += '<li';
                        if (data[i].LMBID == $('#LMBID').val()) {
                            txt += ' class="active" ';
                        }
                        txt += '><a data-toggle="pill" href="#" onclick="ClickUser(' + data[i].LMBID + ');" >';
                        txt += data[i].UserName;
                        if (data[i].SumNoRead != '0' && data[i].LMBID != $('#LMBID').val()) {
                            txt += '<button disabled="disabled" class="NoReadCount">' + data[i].SumNoRead + '</button>';
                        }
                        txt += '</a></li>';
                    }
                    $('#navpill').html(txt);
                }
            })
        }

        function ClickUser(id) {
            $.ajax({
                type: 'post',
                url: 'LineMsgList.aspx/ClickUser',
                contentType: 'application/json;utf-8',
                data: JSON.stringify({
                    ID: id
                }),
                dataType: 'json',
                success: function (doc) {
                    location.href = 'LineMsgList.aspx';
                }
            })
        }

        function bindata() {
            $.ajax({
                type: 'post',
                url: 'LineMsgList.aspx/bindata',
                contentType: 'application/json;utf-8',
                data: JSON.stringify({}),
                dataType: 'json',
                success: function (doc) {
                    let data = JSON.parse(doc.d);
                    $('#LMBID').val(data.LMBID);
                    name = data.Name;
                    getdate();
                    //active = setInterval(function () { bindMsg() }, 1000);
                    bindMsg();
                    console.log(data);
                    if (data.status != 1) {
                        $('#msgbar').show();
                    }

                    bindUserList();
                    useractive = setInterval(function () { bindUserList() }, 1000);
                }
            })
        }

        function getdate() {
            var now = Date.now();
            var sdate = new Date(now);
            sdate.setDate(sdate.getDate() - 7);
            $('#sdate').val(moment(sdate).format('YYYY-MM-DD'));
            today = moment(now).format('YYYY-MM-DD');
            $('#edate').val(today);
            checkUserListTime = moment(now).format('YYYY-MM-DD HH:mm:ss');
        }


        function bindMsg() {
            $.ajax({
                type: 'post',
                url: 'LineMsgList.aspx/bindMsg',
                contentType: 'application/json;utf-8',
                data: JSON.stringify({
                    ID: $('#LMBID').val(),
                    sdate: $('#sdate').val(),
                    edate: $('#edate').val()
                }),
                dataType: 'json',
                success: function (doc) {
                    if (doc.d == '') {
                        return;
                    }
                    let data = JSON.parse(doc.d);
                    $('#data').html('');
                    let txt = '';
                    let final = data.length - 1;
                    let tobottom = false;
                    for (let i = 0; i < data.length; i++) {
                        txt = '';
                        txt += '<tr><th';
                        if (i == final) {
                            txt += ' id="newest"';
                            if (nid != data[i].LMID && nid < data[i].LMID) {
                                nid = data[i].LMID;
                                tobottom = true;
                            }
                        }
                        if (data[i].FromUserID != null && data[i].FromUserID != '') {
                            txt += ' style="text-align:left;">';
                            txt += name + ':';
                            switch (data[i].MsgType) {
                                case 'text':
                                    txt += '<label style="max-width:50%;font-size:30px;margin-left:10px;word-break:break-all;">'
                                    txt += data[i].Msg;
                                    txt += '</label>';
                                    txt += '<sub style="margin-left:5px;">';
                                    txt += moment(data[i].CreateDate).format('YYYY-MM-DD HH:mm:ss');
                                    txt += '</sub>';
                                    break;
                                case 'location':
                                    let LocationData = JSON.parse(data[i].Msg);
                                    txt += '<sub style="margin-left:5px;">';
                                    txt += moment(data[i].CreateDate).format('YYYY-MM-DD HH:mm:ss');
                                    txt += '</sub>';
                                    txt += '<iframe width="50%" height="300" frameborder="0" ';
                                    txt += 'src="https://www.google.com/maps?';
                                    txt += 'q=' + LocationData.latitude + ',' + LocationData.longitude;
                                    txt += '&hl=zh-TW&output=embed" '
                                    txt += '</iframe>';
                                    break;
                            }
                            txt += '</th></tr>';
                        }
                        else {
                            txt += ' style="text-align:right;">';
                            txt += '<sub style="margin-right:5px;">';
                            txt += moment(data[i].CreateDate).format('YYYY-MM-DD HH:mm:ss');
                            txt += '</sub>';
                            txt += '<label style="max-width:50%;font-size:30px;margin-right:5px;word-break:break-all;">'
                            txt += data[i].Msg;
                            txt += '</label>';
                            txt += '</th></tr>';
                        }
                        $('#data').append(txt);
                    }


                    if ($('#sdate').val() == today || $('#edate').val() == today) {
                        location.href = '#newest';
                    }
                    active = setInterval(function () { bindNewestMsg() }, 500);
                }
            });
        }


        function bindNewestMsg() {
            if ($('#sdate').val() != today && $('#edate').val() != today) {
                return;
            }
            $.ajax({
                type: 'post',
                url: 'LineMsgList.aspx/bindNewestMsg',
                contentType: 'application/json;utf-8',
                data: JSON.stringify({
                    ID: $('#LMBID').val()
                }),
                dataType: 'json',
                success: function (doc) {
                    let data = JSON.parse(doc.d);
                    if (data.length > 0 && data[0].LMID != nid) {
                        $('#newest').attr('id', '');
                        let txt = '';
                        txt += '<tr><th';
                        nid = data[0].LMID;
                        txt += ' id="newest"';
                        if (data[0].FromUserID != null && data[0].FromUserID != '') {
                            txt += ' style="text-align:left;">';
                            txt += name + ':';
                            txt += '<label style="max-width:50%;font-size:30px;margin-left:10px;word-break:break-all;">'
                            txt += data[0].Msg;
                            txt += '</label>';
                            txt += '<sub style="margin-left:5px;">';
                            txt += moment(data[0].CreateDate).format('YYYY-MM-DD HH:mm:ss');
                            txt += '</sub>';
                            txt += '</th></tr>';
                        }
                        else {
                            txt += ' style="text-align:right;">';
                            txt += '<sub style="margin-right:5px;">';
                            txt += moment(data[0].CreateDate).format('YYYY-MM-DD HH:mm:ss');
                            txt += '</sub>';
                            txt += '<label style="max-width:50%;font-size:30px;margin-right:5px;word-break:break-all;">'
                            txt += data[0].Msg;
                            txt += '</label>';
                            txt += '</th></tr>';
                        }
                        $('#data').append(txt);
                        location.href = '#newest';
                    }
                }
            })
        }

        function SendMsg() {
            $.ajax({
                type: 'post',
                url: 'LineMsgList.aspx/SendMsg',
                contentType: 'application/json;utf-8',
                data: JSON.stringify({
                    ID: $('#LMBID').val(),
                    msg: $('#msgtxt').val()
                }),
                dataType: 'json',
                success: function (doc) {
                    $('#msgtxt').val('');
                    $('#edate').val(today);
                    clearInterval(active);
                    bindMsg();
                }
            })
        }

    </script>


</asp:Content>


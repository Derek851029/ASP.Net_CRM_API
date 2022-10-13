var Agent_SYSID;
var Agent_Name;
var Case_SYSID;
var People_List_array = []
var People_Name_array = []
var View_Calendar = 'month'

var screen_width = window.screen.width;
var screen_height = window.screen.height; 

$(function () {
    $('#Select_Peolple_tr').hide()
    $('#Notification_tr').hide()
    Page_Load()
    
    renderCalendar()
    Listener_event()
    isMobile()

    Get_Case_Data("")

    List_Depart()
})

function isMobile() {
    var userAgentInfo = navigator.userAgent;

    var mobileAgents = ["Android", "iPhone", "SymbianOS", "Windows Phone", "iPad", "iPod"];

    

    //根據userAgent判斷是否是手機
    //for (var v = 0; v < mobileAgents.length; v++) {
    //    if (userAgentInfo.indexOf(mobileAgents[v]) > 0) { //yes to phone
    //        $('#bar').hide();
    //        $('#navbar-example').hide()
    //        $('.fc-scroller fc-time-grid-container').css('height', 'auto')
    //    }
    //}
    if (screen_width < 500 && screen_height < 800) {
        $('#bar').hide();
        $('#navbar-example').hide()
        //$('.fc-scroller.fc-time-grid-container').css('height', 'auto')
    }
}

function Listener_event() {
    $('#btn_Add_Schdule').click(function () {
        $('#PID').html('(新增)')
        $('#Create_Agent').html(Agent_Name)
        $('#Case').val(-1)
        $('#Bussiness_Name').val('')
        $('#Bussiness_ID').val('')
        $('#txt_contactPerson').val('')
        $('#txt_contactPhoneNumber').val('')
        $('#datetimepicker01').val('')
        $('#txt_Vistit_Content').val('')

        $('#Service_tr').hide()
        $('#Service_tr2').hide()
        $('#Add_Log_tr').hide()
        $('#btn_new').show()
    })

    $('#Case_Name').bind('input', function () {
        let showVal = $('#Case_Name').val() //chinese value
        let Case_SYSID = $("#Case option[value='" + showVal + "']").attr('data-value'); //get data-value
        Get_Case_Data(Case_SYSID)
    })

    //$('#Case').change(function () {
    //    let Case_SYSID = $('#Case').val()
    //    if (Case_SYSID != '-1') {
    //        Get_Case_Data(Case_SYSID)
    //    }
    //})

    $('#btn_Change_Calendar').click(function () {
        $('#calendar').fullCalendar('destroy');
        renderCalendar()
    })

    $('#Assign_Depart').change(function () {
        $("#Select_Assign_People").html('')
        let choose = $('#Assign_Depart').val()
        List_people(choose);
    })

    
    $('#More_Visit').click(function () {
        if ($('#Select_Peolple_tr').is(':hidden')) {
            $('#Select_Peolple_tr').show()
            $('#Notification_tr').show()
        }
        else {
            $('#Select_Peolple_tr').hide()
            $('#Notification_tr').hide()
        }
        
    })

    $('#Push_People_List').click(function () {
        $('#ol_People').html('')
        $.each(People_List_array, function (index, value) {
            $('#ol_People').append('<li id="' + value + '">' + People_Name_array[index] + '</li>')
        })
    })

    $('#test').click(function () {
        Send_Notification_App('客戶名稱','2021-08-16 16:55')
    })
}

function Page_Load() {
    $.ajax({
        url: '0010010006.aspx/Load',
        type: 'POST',
        contentType: 'application/json; charset=UTF-8',
        dataType: "json",
        success: function (doc) {
            var obj = JSON.parse('{"data":' + doc.d + '}');
            $('#Assign_Create_Agent').html(obj.data[0].Agent_Name)
            Agent_SYSID = obj.data[0].SYSID
            Agent_Name = obj.data[0].Agent_Name
            var Agent_ID = obj.data[0].Agent_ID;
        }
    });
}

function renderCalendar() {
    switch (View_Calendar) {
        case 'month':
            View_Calendar = 'agendaWeek'
            break
        case 'agendaWeek':
            View_Calendar = 'month'
    }
    console.log(View_Calendar)
    $('#calendar').fullCalendar({
        header: {
            center: 'title',
            left: '',
        },
        editable: false,
        defaultView: View_Calendar,
        //defaultView: 'agendaWeek',
        defaultDate: new Date(),
        lang: 'zh-tw',
        eventClick: function (calEvent, jsEvent, view) {
            bindtalbe(calEvent.id)
        },
        eventAfterRender: function (calEvent, jsEvent, view) {
            //  類型  -1：未派案  0：派案但未接案  1：已接案(處理中)    2:已結案 3: 退單
            if (calEvent.type == '-1') {
                jsEvent.css('background-color', '#9E9E9E');
            } else if (calEvent.type == '0') {
                jsEvent.css('background-color', '#0000CD');
            } else if (calEvent.type == '1') {
                jsEvent.css('background-color', '#1231e0');
            } else if (calEvent.type == '2') {
                jsEvent.css('background-color', '#72797d');
            } else if (calEvent.type == '3') {
                jsEvent.css('background-color', '#d10419');
            }
        },
        events: function (start, end, timezone, callback) {
            $.ajax({
                url: '0010010006.aspx/renderCalendar',
                type: 'POST',
                //async: false,
                data: JSON.stringify({
                    start: start.format("YYYY/MM/DD"),
                    end: end.format("YYYY/MM/DD"),
                }),
                contentType: 'application/json; charset=UTF-8',
                dataType: "json",       //如果要回傳值，請設成 json
                success: function (doc) {
                    var events = [];
                    //console.log(eval(doc.d))
                    $(eval(doc.d)).each(function () {
                        let start = new Date(this.start).format("yyyy-MM-dd HH:mm:ss")
                        let end = new Date(this.end_visit).format("yyyy-MM-dd HH:mm:ss")
                        events.push({
                            name: 'agendaDay',
                            title: this.Agent_Name + '- ' + this.title,
                            start: start,
                            end: end,
                            type: this.type,
                            id: this.id,
                        });
                    });
                    callback(events);
                    $('#calendar').fullCalendar('render');
                    $('.fc-scroller.fc-time-grid-container').css('height', '1600px')
                }
            });
        }
    });
    $('.fc-scroller.fc-time-grid-container').css('height', '1600px')
}

function bindtalbe(SYSID) {
    $.ajax({
        url: '0010010006.aspx/bindtalbe',
        type: 'POST',
        data: JSON.stringify({ SYSID: SYSID }),
        contentType: 'application/json; charset=UTF-8',
        dataType: "json",       //如果要回傳值，請設成 json
        success: function (doc) {
            window.scroll(0,0) //go to top
            var table = $('#data').DataTable({
                destroy: true,
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
                "aaSorting": [[0, 'desc']],
                "aLengthMenu": [[10, 25, 50, 100], [10, 25, 50, 100]],
                "iDisplayLength": 10,
                "columnDefs": [{
                    "targets": -1,
                    "data": null,
                    "searchable": false,
                    "paging": false,
                    "ordering": false,
                    "info": false
                }],
                columns: [                                      // 顯示資料列
                    { data: "Visit_Person" },
                    { data: "Visit_Phone" },
                    { data: "Visit_Content" },
                    { data: "Create_Date" },
                    {
                        data: "Visit_Date", render: function (data, type, row, meta) {
                            return data.replace('T',' 下午 ')
                        }
                    },
                    { data: "Agent_Name" },
                    {
                        data: "SYSID", render: function (data, type, row, meta) {
                            return "<button type='button' id='btn1' class='btn btn-info btn-lg' data-toggle='modal' data-target='#newModal'>" +
                                    "<span class='glyphicon glyphicon-pencil'></span>&nbsp;填寫服務紀錄</button>";
                        }
                    },
                ]
            });
            $('#data tbody').unbind('click').
                on('click', '#btn1', function () {
                    var table = $('#data').DataTable();
                    var SYSID = table.row($(this).parents('tr')).data().SYSID;
                    $('#Service_tr').show()
                    $('#Service_tr2').show()
                    $('#Add_Log_tr').show()
                    $('#btn_new').hide()
                    Load_Data(SYSID);
                }).on('click', '#btn2', function () {
                    var table = $('#data').DataTable();
                    window.open('../0050010000/0050010001.aspx')
                });
        }
    });
}

function Load_Data(Visit_SYSID) {
    $.ajax({
        url: '0010010006.aspx/Load_Data',
        type: 'POST',
        data: JSON.stringify({ Visit_SYSID: Visit_SYSID }),
        contentType: 'application/json; charset=UTF-8',
        dataType: "json",       //如果要回傳值，請設成 json
        success: function (doc) {
            var obj = JSON.parse(doc.d);
            $('#PID').html('(查詢)')
            let Case_Name = $("#Case option[data-value='" + obj[0].Case_SYSID + "']").val();
            $('#Case_Name').val(Case_Name)
            //$('#Case').val(obj[0].Case_SYSID)
            $('#Bussiness_Name').val(obj[0].Visit_Customer)
            $('#Bussiness_ID').val(obj[0].Visit_Customer_ID)
            $('#txt_contactPerson').val(obj[0].Visit_Person)
            $('#txt_contactPhoneNumber').val(obj[0].Visit_Phone)
            $('#datetimepicker01').val(obj[0].Visit_Date)
            $('#datetimepicker02').val(obj[0].Visit_Leave_Date)
            $('#txt_Vistit_Content').val(obj[0].Visit_Content)
            $('#Create_Agent').html(obj[0].Agent_Name)
        }
    });
}

function Get_Case_Data(Case_SYSID) {
    $.ajax({
        url: '0010010006.aspx/Get_Case_Data',
        type: 'POST',
        /*data: JSON.stringify({ value: value }),*/
        contentType: 'application/json; charset=UTF-8',
        dataType: "json",       //如果要回傳值，請設成 json
        success: function (doc) {
            var obj = JSON.parse(doc.d);
            if (Case_SYSID == '') {
                $.each(obj, function (idx, obj) {
                    $("#Case").append("<option data-value='" + obj.SYSID + "' value='" + obj.Case_Name + "'></option>");
                });
            }
            else {
                $.each(obj, function (idx, obj) {
                    if (obj.SYSID == Case_SYSID) {
                        $('#Bussiness_Name').val(obj.BUSINESSNAME)
                        $('#Bussiness_ID').val(obj.Clinet_Name)
                        $('#txt_contactPerson').val(obj.Contact)
                        $('#txt_contactPhoneNumber').val(obj.Phone)
                    }
                });
            }
        }
    });
}

function Safe_Visit() {
    Notiflix.Loading.Standard('Loading...');
    let showVal = $('#Case_Name').val() //chinese value
    let Case_SYSID = $("#Case option[value='" + showVal + "']").attr('data-value'); //get data-value
    let Case = Case_SYSID

    let Bussiness_Name = $('#Bussiness_Name').val()
    let Bussiness_ID = $('#Bussiness_ID').val()
    let Visit_Person = $('#txt_contactPerson').val()
    let Visit_Phone = $('#txt_contactPhoneNumber').val()
    let Visit_Date = $('#datetimepicker01').val().replace('T', ' ')
    let Visit_Leave_Date = $('#datetimepicker02').val().replace('T', ' ')
    let txt_Vistit_Content = $('#txt_Vistit_Content').val()
    let Create_Agent = Agent_SYSID

    let Peoloe_list = ''
    let Notification_Line = false
    let Notification_App = false
    if ($('#Select_Peolple_tr').is(':hidden') == false) { //if tr not display none
        if (People_List_array.length == 0) {
            Notiflix.Loading.Remove(800);
            alert('請選擇人員。')
            return
        }
        else {
            Peoloe_list = People_List_array.toString()
        }

        if ($('#Line').prop('checked') == false && $('#App').prop('checked') == false) {
            Notiflix.Loading.Remove(800);
            alert('請選擇通知方式。')
            return
        }
        else {
            if ($('#Line').prop('checked')) {
                Notification_Line = true
            } else {
                Notification_Line = false
            }
            if ($('#App').prop('checked')) {
                Notification_App = true
            } else {
                Notification_App = false
            }
        }
    }
    console.log(Visit_Date)
    if (showVal == '') {
        Notiflix.Loading.Remove(800);
        alert('請選擇案件。')
        return
    }
    if (Bussiness_Name == '') {
        Notiflix.Loading.Remove(800);
        alert('請填選客戶名稱')
        return
    }
    if (Bussiness_ID == '') {
        Notiflix.Loading.Remove(800);
        alert('請填選客戶統一編號')
        return
    }
    if (Visit_Person == '') {
        Notiflix.Loading.Remove(800);
        alert('請填選聯絡人')
        return
    }
    if (Visit_Phone == '') {
        Notiflix.Loading.Remove(800);
        alert('請填選聯絡人電話')
        return
    }
    if (Visit_Date == '') {
        alert('請選擇拜訪日期')
        Notiflix.Loading.Remove(800);
        return
    }
    if (Visit_Leave_Date == '') {
        Notiflix.Loading.Remove(800);
        alert('請選擇結束拜訪日期')
        return
    }
    if (txt_Vistit_Content == '') {
        Notiflix.Loading.Remove(800);
        alert('請填選拜訪事項')
        return
    }

    $.ajax({
        url: '0010010006.aspx/Safe_Visit',
        type: 'POST',
        data: JSON.stringify({
            Case: Case,
            Bussiness_Name: Bussiness_Name,
            Bussiness_ID: Bussiness_ID,
            Visit_Person: Visit_Person,
            Visit_Phone: Visit_Phone,
            Visit_Leave_Date: Visit_Leave_Date,
            Visit_Date: Visit_Date,
            txt_Vistit_Content: txt_Vistit_Content,
            Create_Agent: Create_Agent,
            Peoloe_list: Peoloe_list,
            Notification_Line: Notification_Line,
            Notification_App: Notification_App
        }),
        contentType: 'application/json; charset=UTF-8',
        dataType: "json",       //如果要回傳值，請設成 json
        success: function (doc) {
            if (Notification_App == true) {
                Send_Notification_App(Bussiness_Name,Visit_Date)
            }
            var json = JSON.parse(doc.d);
            Notiflix.Loading.Remove(800);
            alert(json.status)
            window.location.reload();
        }
    });
}

function Add_Log() {
    let Case_SYSID = $('#Case').val()
    let Service_Context = $('#Service_Context').val()
    if (Service_Context == '') {
        alert('請填選服務紀錄。')
        return
    }
    $.ajax({
        url: '0010010006.aspx/Add_Log',
        type: 'POST',
        data: JSON.stringify({
            Case_SYSID: Case_SYSID,
            Agent_Name: Agent_Name,
            Service_Context: Service_Context,
        }),
        contentType: 'application/json; charset=UTF-8',
        dataType: "json",       //如果要回傳值，請設成 json
        success: function (doc) {
            var json = JSON.parse(doc.d);
            alert(json.status)
            window.location.reload();
        }
    });
}

function List_Depart() {
    $.ajax({
        url: "0010010005.aspx/List_Depart",
        type: 'POST',
        contentType: 'application/json; charset=UTF-8',
        dataType: "json",
        success: function (data) {
            var jsonParse = JSON.parse(data.d);
            let Assign_Depart = $("#Assign_Depart");
            let Check_same_Depart = []

            $.each(jsonParse, function (index, value) {
                if (Check_same_Depart.indexOf(value.Agent_Team) != -1) {
                    return
                }
                else {
                    Assign_Depart.append(`<option value='${value.Agent_Team}'>${value.Agent_Team}</option>`);
                }
                Check_same_Depart.push(value.Agent_Team)
            });
        }
    });
}

function List_people(Depart) {
    $.ajax({
        url: "0010010005.aspx/List_people",
        type: 'POST',
        data: JSON.stringify({
            Depart: Depart
        }),
        contentType: 'application/json; charset=UTF-8',
        dataType: "json",
        success: function (data) {
            var jsonParse = JSON.parse(data.d);
            
            $('#People_List_check').html('')
            if (jsonParse.length != 0) {
                $('#People_List_check').append('<input type="checkbox" id="Select_all" class="circle_box">全選</input> <br />')
                $('#People_List_check').append('<label style="margin-bottom:20px">-----------</label>')
                $.each(jsonParse, function (index, value) {
                    let location = People_List_array.indexOf(value.SYSID)
                    let checked = ''
                    if (location == -1) {
                        checked = ''
                    }
                    else {
                        checked = 'checked'
                    }
                    $('#People_List_check').append('<p><input type="checkbox" id="Peoplecheck_' + value.SYSID + '" class="circle_box" ' + checked + ' /><label id="Peopleval_' + value.SYSID + '">' + value.Agent_Name + '</label></p>')
                    if (index + 1 == $("input[type='checkbox']:checked").length) {
                        $('#Select_all').prop("checked", true);
                    } else {
                        $('#Select_all').prop("checked", false);
                    }
                })
            }

            $('input[id^="Peoplecheck_"]').click(function () {
                let checkbox_ID = $(this).attr('id').split('_')
                let Agent_SYSID = checkbox_ID[1]
                let checked_value = $('#Peopleval_' + Agent_SYSID + '').html()
                let location = People_List_array.indexOf(Agent_SYSID)

                if (location == -1) {
                    People_List_array.push(Agent_SYSID)
                    People_Name_array.push(checked_value)
                }
                else {
                    People_List_array.splice(location, 1)
                    People_Name_array.splice(location, 1)
                    $('#Select_all').prop("checked", false);
                }
            })

            $('#Select_all').click(function () {
                if ($("#Select_all").prop("checked")) { //如果點亮checbox
                    $("input[id^='Peoplecheck_']").each(function () {
                        let checkbox_ID = $(this).attr('id').split('_')
                        let Agent_SYSID = checkbox_ID[1]
                        let checked_value = $('#Peopleval_' + Agent_SYSID + '').html()
                        let location = People_List_array.indexOf(Agent_SYSID)

                        if (location == -1) {
                            People_List_array.push(Agent_SYSID)
                            People_Name_array.push(checked_value)
                        }
                        $(this).prop("checked", true);//把所有的方框都變成勾選
                    })
                }
                else {
                    $("input[id^='Peoplecheck_']").each(function () {
                        let checkbox_ID = $(this).attr('id').split('_')
                        let Agent_SYSID = checkbox_ID[1]
                        let checked_value = $('#Peopleval_' + Agent_SYSID + '').html()
                        let location = People_List_array.indexOf(Agent_SYSID)

                        if (location == -1) {
                            People_List_array.push(Agent_SYSID)
                            People_Name_array.push(checked_value)
                        }
                        else {
                            People_List_array.splice(location, 1)
                            People_Name_array.splice(location, 1)
                        }
                        $(this).prop("checked", false);//把所有的核方框的property都取消勾選
                    })
                }
            })
        }
    });
}

function Send_Notification_App(Bussiness_Name, Visit_Date) {
    let People_SYSID = People_List_array.toString()
    $.ajax({
        url: 'http://192.168.2.227:5008/Send_Notification',
        type: 'POST',
        data: JSON.stringify({
            type: 'Schedule',
            Visit_Customer: Bussiness_Name,
            Visit_Date: Visit_Date,
            SYSID: People_SYSID
        }),
        contentType: 'application/json; charset=UTF-8',
        dataType: "json",       //如果要回傳值，請設成 json
        success: function (doc) {
            console.log(doc)
        }
    });
}
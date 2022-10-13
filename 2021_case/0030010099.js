var d = new Date(), str = '';
str += d.getFullYear() + '/';
str += d.getMonth() + 1 + '/';
str += d.getDate() + ' ';
str += d.getHours() + ':';
str += d.getMinutes() + '';
$(function () {
    bindSelect_ListCase()
    Page_Load()
    

    //監聽下拉案件
    $('#Chose_depart').change(function () {
        $('#Dispatch_Name').html('')
        let choose = $('#Chose_depart').val()
        List_people(choose,'0');
    })

    //新增
    $('#add').click(function () {
        Add()
    })

    //監聽派工
    $('#checkbox1').click(function () {
        if (($(this).prop('checked'))) {
            $('#Ins_people').show();
            $("#checkbox2").prop("checked", '');
        }
        else {
            $('#Ins_people').hide();
        }
    })
    $('#checkbox2').click(function () {
        if (($(this).prop('checked'))) {
            $("#checkbox1").prop("checked", '');
            $('#Ins_people').hide();
        }
        else {
            $('#Ins_people').hide();
        }
    })
})

function Page_Load() {
    if (seqno == '0') { //seqno宣告在aspx
        $('#str_title').html('新增服務單') //改標題
        $('#str_sysid').html(new_mno) //【創造母單編號】
        $('#LoginTime').html(str) //最後修改日期

        $('#DealingProcess').html('新建服務單')
        $.ajax({
            url: '0030010099.aspx/Load',
            type: 'POST',
            contentType: 'application/json; charset=UTF-8',
            dataType: "json",
            success: function (doc) {
                var obj = JSON.parse('{"data":' + doc.d + '}');
                $('#A_ID').html(obj.data[0].Agent_Name)
                var Agent_ID = obj.data[0].Agent_ID;
            }
        });
    } else {
        $('#str_title').html('查詢服務單')
        $('#add').hide()
        Load_Data()
    }
}

function bindSelect_ListCase() {
    $.ajax({
        url: "0030010099.aspx/bindSelect_ListCase",
        type: 'POST',
        contentType: 'application/json; charset=UTF-8',
        dataType: "json",
        success: function (data) {
            var jsonParse = JSON.parse(data.d);
            let select_ListCase = $("#select_ListCase");
            select_ListCase.append(`<option value='-1'>請選擇案件</option>`);
            $.each(jsonParse, function (index, value) {
                select_ListCase.append(`<option value='${value.SYSID}'>${value.Case_Name}</option>`);
            });
        }
    });
}

function List_people(depart,type) {
    console.log(depart)
    $.ajax({
        url: "0030010099.aspx/List_people",
        type: 'POST',
        data: JSON.stringify({ depart: depart }),
        contentType: 'application/json; charset=UTF-8',
        dataType: "json",
        success: function (data) {
            var jsonParse = JSON.parse(data.d);
            let select_ListCase = $("#Dispatch_Name");
            select_ListCase.append(`<option value='-1'>請選擇派工</option>`);
            $.each(jsonParse, function (index, value) {
                select_ListCase.append(`<option value='${value.SYSID}'>${value.Agent_Name}</option>`);
            });

            if (type != '0') {
                $('#Dispatch_Name').val(type)
            }
        }
    });
}

function Add() {
    let str_sysid = $('#str_sysid').html()
    let Text1 = $('#Text1').val()
    let Create_date = $('#LoginTime').html()
    let select_ListCase = $('#select_ListCase').val()
    let SelectUrgency = $('#SelectUrgency').val()
    let End_date = $('#datetimepicker01').val()
    let SelectOpinionType = $('#SelectOpinionType').val()
    let ReplyType = $('#ReplyType').val()
    let Opinion_content = $('#Opinion').val()
    let A_ID = $('#A_ID').html()
    let yes_or_no;
    let checkbox = $('#checkbox1').prop('checked')
    let checkbox2 = $('#checkbox2').prop('checked')
    let Chose_depart = $('#Chose_depart').val()
    let Dispatch_Name = $('#Dispatch_Name').val()
    let type = '0'

    if (select_ListCase == '-1') {
        alert('請選擇案件')
        return
    }
    if (SelectUrgency == '') {
        alert('請選擇緊急程度')
        return
    }
    if (End_date == '') {
        alert('請選擇預計時程')
        return
    }
    if (SelectOpinionType == '') {
        alert('請選擇意見類型')
        return
    }
    if (Opinion_content == '') {
        alert('請填寫意見內容')
        return
    }
    if (checkbox == false && checkbox2 == false) {
        alert('請選擇是否派工')
        return
    }
    else {
        if (checkbox) { //if yes checkbox checked
            if (Chose_depart == '') {
                alert('請選擇派工部門')
                return
            }
            if (Dispatch_Name == '-1') {
                alert('請選擇派工人員')
                return
            }
            yes_or_no = $('#yes').html()
        }
        else {
            yes_or_no = $('#no').html()
            type = '-1'
            Chose_depart = ''
            Dispatch_Name = ''
        }
    }

    $.ajax({
        url: "0030010099.aspx/Add",
        type: 'POST',
        data: JSON.stringify({
            str_sysid: str_sysid,
            Text1: Text1,
            Create_date: Create_date,
            select_ListCase: select_ListCase,
            SelectUrgency: SelectUrgency,
            End_date: End_date,
            SelectOpinionType: SelectOpinionType,
            ReplyType: ReplyType,
            Opinion_content: Opinion_content,
            A_ID: A_ID,
            yes_or_no: yes_or_no,
            Chose_depart: Chose_depart,
            Dispatch_Name: Dispatch_Name,
            type: type
        }),
        contentType: 'application/json; charset=UTF-8',
        dataType: "json",
        success: function (data) {
            var jsonParse = JSON.parse(data.d)
            console.log(jsonParse)
            alert(jsonParse.status)
            window.location.reload()
        }
    });
}

function Load_Data() {
    var SYSID = seqno

    $.ajax({
        url: '0030010099.aspx/Load_Data',
        type: 'POST',
        data: JSON.stringify({ SYSID: SYSID }),
        contentType: 'application/json; charset=UTF-8',
        dataType: "json",       //如果要回傳值，請設成 json
        success: function (doc) {
            var text = '{"data":' + doc.d + '}';
            var obj = JSON.parse(doc.d);
            let Process_Status_str;
            $('#str_sysid').html(obj[0].Case_Num)
            $('#Text1').val(obj[0].Caller_ID)
            $('#LoginTime').html(obj[0].Create_Date)
            $('#select_ListCase').val(obj[0].Case_List_type)
            console.log(obj[0].Case_List_type)
            $('#SelectUrgency').val(obj[0].Urgency)
            $('#datetimepicker01').val(obj[0].End_Date)
            switch (obj[0].Process_Status) {
                case '-1':
                    Process_Status_str =  "未派案"
                    break
                case '0':
                    Process_Status_str =  "已派案"
                    break
                case '1':
                    Process_Status_str =  "處理中"
                    break
                case '2':
                    Process_Status_str =  "已完成"
                    break
            }
            $('#DealingProcess').html(Process_Status_str)

            $('#SelectOpinionType').val(obj[0].OpinionType)
            $('#ReplyType').val('-1')
            $('#Opinion').val(obj[0].Opinion_Content)
            $('#A_ID').html(obj[0].Create_Agent)

            if (obj[0].Create_Agent == '否') {
                $("#checkbox1").prop("checked", '');
                $("#checkbox2").prop("checked", true);
            }
            else {
                $("#checkbox2").prop("checked", '');
                $("#checkbox1").prop("checked", true);

                List_people(obj[0].Dispatch_Depart, obj[0].Dispatch_Name)
                $('#Chose_depart').val(obj[0].Dispatch_Depart)
                console.log(obj[0].Dispatch_Name)
                
                $('#Ins_people').show();
            }
        }
    });
}
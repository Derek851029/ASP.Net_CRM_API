$(function () {
    renderCalendar()
})

function renderCalendar() {
    $('#calendar').fullCalendar({
        header: {
            center: 'title',
        },
        editable: false,
        defaultDate: new Date(),
        lang: 'zh-tw',
        eventClick: function (calEvent, jsEvent, view) {
            bindtalbe_Case(calEvent.id)
        },
        eventAfterRender: function (calEvent, jsEvent, view) {
            //  類型  -1：未派案  0：派案但未接案  1：已接案(處理中)    2:已結案 3: 退單
            if (calEvent.type == '-1') {
                jsEvent.css('background-color', '#9E9E9E');
            } else if (calEvent.type == '0') {
                jsEvent.css('background-color', '#00ed3f');
            } else if (calEvent.type == '1') {
                jsEvent.css('background-color', '#1231e0');//424242
            } else if (calEvent.type == '2') {
                jsEvent.css('background-color', '#72797d');
            } else if (calEvent.type == '3') {
                jsEvent.css('background-color', '#d10419');
            }
        },
        events: function (start, end, timezone, callback) {
            $.ajax({
                url: '0030010003.aspx/renderCalendar',
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
                        events.push({
                            title: this.title,
                            start: this.start,
                            type: this.type,
                            id: this.id
                        });
                    });
                    callback(events);
                }
            });
        }
    });
}

function bindtalbe_Case(SYSID) {
    $.ajax({
        url: '0030010003.aspx/bindtalbe_Case',
        type: 'POST',
        data: JSON.stringify({SYSID: SYSID}),
        contentType: 'application/json; charset=UTF-8',
        dataType: "json",       //如果要回傳值，請設成 json
        success: function (doc) {
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
                    { data: "Create_Date" },
                    { data: "End_Date" },
                    { data: "Case_Num" },
                    { data: "Case_Name" },
                    { data: "OpinionType" },
                    { data: "Create_Agent" },
                    {
                        data: "Process_Status", render: function (data, type, row, meta) {
                            switch (data) {
                                case '0':
                                    return "未接案"
                                    break
                                case '1':
                                    return "已接案"
                                    break
                                case '2':
                                    return "已完成"
                                    break
                            }
                        }
                    },
                    {
                        data: "SYSID", render: function (data, type, row, meta) {
                            switch (row.Process_Status) {
                                case '0':
                                    return "<button type='button' id='btn1' class='btn btn-info btn-lg'>" +
                                        "<span class='glyphicon glyphicon-pencil'></span>&nbsp;接案</button>";
                                    break
                                case '1':
                                    return "<button type='button' id='btn2' class='btn btn-info btn-lg'>" +
                                        "<span class='glyphicon glyphicon-pencil'></span>&nbsp;服務紀錄</button>";
                                    break
                                case '2':
                                    return "已完成"
                                    break
                            }
                        }
                    },
                ]
            });
            $('#data tbody').unbind('click').
                on('click', '#btn1', function () {
                    var table = $('#data').DataTable();
                    var SYSID = table.row($(this).parents('tr')).data().SYSID;
                    SetCase_Type(SYSID);
                }).on('click', '#btn2', function () {
                    var table = $('#data').DataTable();
                    window.open('../0050010000/0050010001.aspx')
                });
        }
    });
}

function SetCase_Type(SYSID) {
    $.ajax({
        url: '0030010003.aspx/SetCase_Type',
        type: 'POST',
        //async: false,
        data: JSON.stringify({ SYSID: SYSID}),
        contentType: 'application/json; charset=UTF-8',
        dataType: "json",       //如果要回傳值，請設成 json
        success: function (doc) {
            let jsonParse = JSON.parse(doc.d)
            alert(jsonParse.status)
        }
    });
}
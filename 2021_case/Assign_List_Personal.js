var Agent_SYSID;
var Assign_SYSID;

$(function () {
    Page_Load()
    bindtable()
})

function Page_Load() {
    $.ajax({
        url: '0010010005.aspx/Load',
        type: 'POST',
        contentType: 'application/json; charset=UTF-8',
        dataType: "json",
        success: function (doc) {
            var obj = JSON.parse('{"data":' + doc.d + '}');
            $('#Assign_Create_Agent').html(obj.data[0].Agent_Name)
            Agent_SYSID = obj.data[0].SYSID
        }
    });
}

function bindtable() {
    $.ajax({
        url: 'Assign_List_Personal.aspx/bindtalbe',
        type: 'POST',
        //data: JSON.stringify({ SYSID: SYSID }),
        contentType: 'application/json; charset=UTF-8',
        dataType: "json",       //如果要回傳值，請設成 json
        success: function (doc) {
            window.scroll(0, 0) //go to top
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
                    { data: "Case_SYSID" },
                    { data: "Case_Name" },
                    { data: "Urgent" },
                    { data: "Assign_title" },
                    { data: "End_date" },
                    {
                        data: "SYSID", render: function (data, type, row, meta) {
                            switch (row.Status) {
                                case '0':
                                    if (row.Assign_People == Agent_SYSID) {
                                        return "<button type='button' id='Agree_Assign' class='btn btn-info'>" +
                                            "<span class='glyphicon glyphicon-plus'>接單</span>" +
                                            "</button>&nbsp&nbsp" +

                                            "<button type='button' id='Chargeback_Assign' class='btn btn-danger' data-toggle='modal' data-target='#dialog5'>" +
                                            "<span class='glyphicon glyphicon-pencil'>退單</span></button>";
                                        break
                                    }
                                case '1':
                                    return "已接單"
                                    break
                                case '-1':
                                    return "已退單"
                                    break
                            }
                        }
                    },
                    {
                        data: "Case_SYSID", render: function (data, type, row, meta) {
                            return `<button type='button' id='info' class='btn btn-info btn-lg' data-toggle='modal' data-target='#newModal' >
                                        <span class='glyphicon glyphicon-search'></span>
                                    </button>`
                                + "&nbsp&nbsp";
                        }
                    },
                ]
            });
            $('#data tbody').unbind('click').
                on('click', '#info', function () {
                    var table = $('#data').DataTable();
                    var Case_SYSID = table.row($(this).parents('tr')).data().Case_SYSID;
                    window.location.href = './0010010005.aspx?seqno=' + Case_SYSID + ''
                })
                .on('click', '#Agree_Assign', function () {
                    var table = $('#data').DataTable();
                    var SYSID = table.row($(this).parents('tr')).data().SYSID;
                    Assign_SYSID = SYSID //push Assign case to var
                    Change_Assign_Status('1')
                })
                .on('click', '#Chargeback_Assign', function () {
                    var table = $('#data').DataTable();
                    var SYSID = table.row($(this).parents('tr')).data().SYSID;
                    Assign_SYSID = SYSID
                    $('#Assign_Chargeback_btn').click(function () {
                        Change_Assign_Status('-1')
                    })
                })
        }
    });
}

function Change_Assign_Status(Set) {
    let Chargeback_text = ''
    if (Set == '-1') {
        Chargeback_text = $('#Chargeback_text').val()
    }
    $.ajax({
        url: "0010010005.aspx/Change_Assign_Status",
        type: 'POST',
        data: JSON.stringify({
            Set: Set,
            Assign_SYSID: Assign_SYSID,
            Chargeback_text: Chargeback_text,
        }),
        contentType: 'application/json; charset=UTF-8',
        dataType: "json",
        success: function (data) {
            var jsonParse = JSON.parse(data.d)
            alert(jsonParse.status)
            window.location.reload()
        }
    });
}
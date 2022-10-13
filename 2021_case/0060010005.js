$(function () {
    bindTable()
})
function bindTable() {
    $.ajax({
        url: '0060010005.aspx/GetCaseList',
        type: 'POST',
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
                    { data: "Case_Num" },
                    { data: "Case_Name" },
                    { data: "OpinionType" },
                    { data: "Create_Agent" },
                    {
                        data: "Process_Status", render: function (data, type, row, meta) {
                            switch (data) {
                                case '-1':
                                    return "未派案"
                                    break
                                case '0':
                                    return "已派案"
                                    break
                                case '1':
                                    return "處理中"
                                    break
                                case '2':
                                    return "已完成"
                                    break
                            }
                        }
                    },
                    {
                        data: "SYSID", render: function (data, type, row, meta) {
                            return "<button type='button' id='search' class='btn btn-info btn-lg'>" +
                                "<span class='glyphicon glyphicon-search'></span>&nbsp;查看</button>";
                        }
                    },
                    {
                        data: "Process_Status", render: function (data, type, row, meta) {
                            if (data == '-1') {
                                return "<button type='button' id='edit02' class='btn btn-danger btn-lg'>" +
                                    "<span class='glyphicon glyphicon-pencil'></span>&nbsp;前往派案</button>";
                            }
                            else {
                                return ''
                            }
                        }
                    },
                ]
            });
            $('#data tbody').unbind('click').
                on('click', '#search', function () {
                    var table = $('#data').DataTable();
                    var SYSID = table.row($(this).parents('tr')).data().SYSID;
                    window.open('../2021_case/0030010099.aspx?seqno=' + SYSID +''); //另開視窗用
                }).on('click', '#edit', function () {
                    var table = $('#data').DataTable();
                    var Case_ID = table.row($(this).parents('tr')).data().Case_ID;
                    //alert(PID);
                    URL2(Case_ID);
                });
        }
    });
}

function URL() {
    var newwin = window.open(); //另開視窗用 要放 $.ajax({ 前
    newwin.location = json.status;  //另開視窗指令
}
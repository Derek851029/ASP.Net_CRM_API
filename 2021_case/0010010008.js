var People_var;
$(function () {
    $('#main2').hide()
    Listener_event()

    List_Depart()
})

function Listener_event() {
    $('#Depart').change(function () {
        $("#People").html('')
        let choose = $('#Depart').val()
        List_people(choose);
    })

    $('#Search').click(function () {
        Search()
    })
}

function List_Depart() {
    $.ajax({
        url: "0010010005.aspx/List_Depart",
        type: 'POST',
        contentType: 'application/json; charset=UTF-8',
        dataType: "json",
        success: function (data) {
            var jsonParse = JSON.parse(data.d);
            let Depart = $("#Depart");
            let Check_same_Depart = []

            $.each(jsonParse, function (index, value) {
                if (Check_same_Depart.indexOf(value.Agent_Team) != -1) {
                    return
                }
                else {
                    Depart.append(`<option value='${value.Agent_Team}'>${value.Agent_Team}</option>`);
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
            let People = $("#People");
            People.append(`<option value='-1'>請選擇人員…</option>`);
            $.each(jsonParse, function (index, value) {
                People.append(`<option value='${value.SYSID}'>${value.Agent_Name}</option>`);
            });
        }
    });
}

function Search() {
    
    let Depart = $('#Depart').val()
    let People_val = $("#People").val()
    let People = $("#People option:checked").text() //獲取以選擇白色值
    let Case = $('#Case').val()

    if (Depart == '-1' || People_val == '-1') {
        alert('請選擇部門/人員。')
        return
    }
    Notiflix.Loading.Standard('Loading...');
    $.ajax({
        url: '0010010008.aspx/Search',
        type: 'POST',
        data: JSON.stringify({
            People: People,
            Case: Case,
        }),
        contentType: 'application/json; charset=UTF-8',
        dataType: "json",       //如果要回傳值，請設成 json
        success: function (doc) {
            let table = $('#data').DataTable({
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
                "columnDefs": [{
                    "targets": -1,
                    "data": null,
                    "searchable": false,
                    "paging": false,
                    "ordering": false,
                    "info": false
                }],
                columns: [
                    { data: "Case_Name" },
                    { data: "BUSINESSNAME" },
                    { data: "Contact" },
                    { data: "Phone" },
                    { data: "Personnel" },
                    {
                        data: "Status",
                        render: function (data, type, row, meta) {
                            switch (data) {
                                case '0':
                                    return '開發中'
                                case '1':
                                    return '裝機中'
                                case '2':
                                    return '維護中'
                                case '3':
                                    return '結案'
                            }
                        }
                    },
                    {
                        data: "SYSID",
                        render: function (data, type, row, meta) {
                            return "<button type='button' id='info' onclick='Search_Assign("+data+")' class='btn btn-info btn-lg' data-toggle='modal' data-target='#newModal' >"+
                                        "<span class='glyphicon glyphicon-search'>交辦事項查詢</span></button>"
                                    + "&nbsp&nbsp";
                        }
                    },
                ]
            });
            Search_Assign("0")
            //$('#data tbody')
            //    .unbind('click')
            //    .on('click', '#info', function () {
            //        let dataTable_rowData = table.row($(this).parents('tr')).data();
            //        let SYSID = dataTable_rowData.SYSID

            //        $('#Service').show()
            //        $('#btn_Change_Status').show()

            //        Load_Case(dataTable_rowData);

            //        GetWorkLogs(SYSID)
            //        Assign_Case_List(SYSID)


            //        //Get_Work_List(Product_Name_array)

            //        Case_SYSID = SYSID //push case SYSID to var
            //    })
        }
    });
}

function Search_Assign(Case_SYSID) {
    let People = $("#People").val()
    Case_SYSID = Case_SYSID.toString()
    $.ajax({
        url: '0010010008.aspx/Search_Assign',
        type: 'POST',
        data: JSON.stringify({
            Case_SYSID: Case_SYSID,
            People: People,
        }),
        contentType: 'application/json; charset=UTF-8',
        dataType: "json",       //如果要回傳值，請設成 json
        success: function (doc) {
            let table = $('#data2').DataTable({
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
                "columnDefs": [{
                    "targets": -1,
                    "data": null,
                    "searchable": false,
                    "paging": false,
                    "ordering": false,
                    "info": false
                }],
                columns: [
                    { data: "Case_Name" },
                    { data: "Urgent" },
                    { data: "Assign_text" },
                    { data: "Agent_Name" },
                    {
                        data: "Status", render: function (data, type, row, meta) {
                            switch (data) {
                                case '-1':
                                    return "退單"
                                    break
                                case '0':
                                    return "等待 接/退單"
                                    break
                                case '1':
                                    return "已接單"
                                    break
                            }
                        }
                    },
                ]
            });
        }
    });
    $('#main2').show()
    Notiflix.Loading.Remove(800);
}
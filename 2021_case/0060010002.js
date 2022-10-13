var Main_list_array = []

$(function () {
    Notiflix.Loading.Standard('Loading...');
    Page_Load()
    bind_table();

    Listener_event()
})

function Listener_event() {
    $('.close').click(function () { //if dialog 'x' click
        window.location.assign('../2021_case/0060010002.aspx?seqno=0')
    })

    $('#Add_Vendor').click(function () {//listen dialog add vendor button
        Add_Vendor()
    })

    $('#Add_Main').click(function () {
        Get_Main(seqno)
    })

    $('#Edit_Vendor').click(function () {
        Add_Vendor()
        List_Owner_Main()
        //window.location.assign('../2021_case/0060010002.aspx?seqno=0')
    })
}

function Page_Load() {
    $.ajax({
        url: '0060010002.aspx/Load',
        type: 'POST',
        contentType: 'application/json; charset=UTF-8',
        dataType: "json",
        success: function (doc) {
            var obj = JSON.parse('{"data":' + doc.d + '}');
            $('#Create_Agent').html(obj.data[0].Agent_Name)
        }
    });

    if (seqno == '0') { //Get create agent
        $('#title_modal').show()
        $('#title_modal').html('廠商資料(新增)')
        $('#Edit_Vendor').hide()
        $('#Add_Vendor').show()
        $('#Vendor_Owner_Main').hide()
    } else { // if seqno == Vendor SYSID
        List_Owner_Main()
        $('#Vendor_Owner_Main').show()
        $('#title_modal').hide()
        $('#Edit_Vendor').show()
        $('#Add_Vendor').hide()
        $('#btn_Add').click()
        $.ajax({
            url: '0060010002.aspx/Load_Vendor_Data',
            type: 'POST',
            data: JSON.stringify({
                seqno: seqno
            }),
            contentType: 'application/json; charset=UTF-8',
            dataType: "json",
            success: function (doc) {
                var obj = JSON.parse('{"data":' + doc.d + '}');

                $('#Vendor_Name').val(obj.data[0].Vendor_Name)
                $('#Vendor_ID').val(obj.data[0].Vendor_ID)
                $('#Vendor_Connection').val(obj.data[0].Vendor_Connection)
                $('#Vendor_phone').val(obj.data[0].Vendor_phone)
                $('#Create_Agent').html(obj.data[0].Create_Agent)
                if (obj.data[0].Select_Main != '') {
                    Main_list_array = obj.data[0].Select_Main.split(',') //when search buuton click will reload and var will clean, so can use it
                }
                console.log(Main_list_array)
                Get_Main()
            }
        });
    }
    Notiflix.Loading.Remove(600);
}

function bind_table() {
    $.ajax({
        url: '0060010002.aspx/Get_VendorList',
        type: 'POST',
        contentType: 'application/json; charset=UTF-8',
        dataType: "json",       //如果要回傳值，請設成 json
        success: function (doc) {
            var table = $('#Vendor_List').DataTable({
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
                    { data: "SYSID" },
                    { data: "Vendor_Name" },
                    { data: "Vendor_Connection" },
                    { data: "Vendor_phone" },
                    {
                        data: "SYSID", render: function (data, type, row, meta) {
                            return "<button type='button' id='search' data-target='#dialog1' class='btn btn-info btn-lg'>" +
                                "<span class='glyphicon glyphicon-search'></span>&nbsp;查詢</button>";
                        }
                    },
                ]
            });
            $('#Vendor_List tbody').unbind('click').
                on('click', '#search', function () {
                    var table = $('#Vendor_List').DataTable();
                    var SYSID = table.row($(this).parents('tr')).data().SYSID;
                    window.location.assign('../2021_case/0060010002.aspx?seqno='+SYSID+'')
                    //Update_Vendor(SYSID)
                }).on('click', '#edit', function () {
                    var table = $('#data').DataTable();
                    var Case_ID = table.row($(this).parents('tr')).data().Case_ID;
                    //alert(PID);
                    URL2(Case_ID);
                });
        }
    });
}

function Get_Main() {
    $.ajax({
        url: '0060010002.aspx/Get_Main',
        type: 'POST',
        contentType: 'application/json; charset=UTF-8',
        dataType: "json",       //如果要回傳值，請設成 json
        success: function (doc) {
            var table = $('#Main_list').DataTable({
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
                columns: [ // 顯示資料列
                    { data: "Main" },
                    { data: "Detail" },
                    {
                        data: "OE_T_ID",
                        render: function (data, type, row, meta) {
                            let checked = ''
                            if (Main_list_array.indexOf(data) == -1) {
                                checked = ''
                            } else {
                                checked = 'checked'
                            }
                            return "<div style=' text-align:center;'><label>" +
                                "<input type='checkbox' style='width: 30px; height: 30px;' id='checkbox' name='checkbox' value='" + data + "' "+checked+" />" +
                                "</label></div>";
                        }
                    }
                ]
            });
            var key = [];

            $('#Main_list tbody').unbind('click').
                on('click', '#checkbox', function () {
                    var table = $('#Main_list').DataTable();
                    let Main = table.row($(this).parents('tr')).data().Main;
                    let OE_T_ID = table.row($(this).parents('tr')).data().OE_T_ID;
                    let location = Main_list_array.indexOf(OE_T_ID)

                    if (location == -1) {
                        Main_list_array.push(OE_T_ID)
                    }
                    else {
                        Main_list_array.splice(location,1)
                    }

                    console.log(Main_list_array)
                }).on('click', '#btn2', function () {
                    var table = $('#data').DataTable();
                    window.open('../0050010000/0050010001.aspx')
                });
        }
    });
}

function Add_Vendor() {
    let Vendor_Name = $('#Vendor_Name').val()
    let Vendor_ID = $('#Vendor_ID').val()
    let Vendor_Connection = $('#Vendor_Connection').val()
    let Vendor_phone = $('#Vendor_phone').val()
    let Select_Main = Main_list_array.toString().replace(/\s*/g, "");
    let Create_Agent = $('#Create_Agent').html()

    if (Vendor_Name == '') {
        alert('請填寫廠商名稱。')
        return
    }
    if (Vendor_ID == '') {
        alert('請填寫統一編號。')
        return
    }
    if (Vendor_Connection == '') {
        alert('請填寫聯絡人。')
        return
    }
    if (Vendor_phone == '') {
        alert('請填寫連絡電話。')
        return
    }
    if (Select_Main == '-1') {
        alert('請選擇主分類。')
        return
    }

    $.ajax({
        url: "0060010002.aspx/Add_Vendor",
        type: 'POST',
        data: JSON.stringify({
            Vendor_Name: Vendor_Name,
            Vendor_ID: Vendor_ID,
            Vendor_Connection: Vendor_Connection,
            Vendor_phone: Vendor_phone,
            Select_Main: Select_Main,
            Create_Agent: Create_Agent,
            seqno: seqno,
        }),
        contentType: 'application/json; charset=UTF-8',
        dataType: "json",
        success: function (data) {
            var jsonParse = JSON.parse(data.d)
            console.log(jsonParse)
            List_Owner_Main()
            alert(jsonParse.status)
        }
    });
}

function List_Owner_Main() {            //案件列表程式
    let Vendor_SYSID = seqno
    $.ajax({
        url: '0060010002.aspx/List_Owner_Main',
        type: 'POST',
        data: JSON.stringify({ Vendor_SYSID: Vendor_SYSID}),
        contentType: 'application/json; charset=UTF-8',
        dataType: "json",       //如果要回傳值，請設成 json
        success: function (doc) {
            var table = $('#data4').DataTable({
                destroy: true,
                data: eval(doc.d), "oLanguage": {
                    "sLengthMenu": "顯示 _MENU_ 項商品",
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
                    "info": false,
                    "defaultContent": "<button type='button' id='edit' class='btn btn-info btn-lg'>" +
                        "<span class='glyphicon glyphicon-search'></span>&nbsp;明細</button>"
                }],
                columns: [
                    { data: "OE_T_ID" },
                    { data: "Main" },
                    { data: "Detail" },
                ]
            });
            $('#data tbody').unbind('click').
                on('click', '#edit', function () {
                });
        }
    });
}
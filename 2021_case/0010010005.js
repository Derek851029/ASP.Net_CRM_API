var Agent_SYSID;
var Agent_Name;
var Case_Data;

var Product_Name_array = [];
var Unit_Price_array = [];
var OE_ID_array = [];
var Quantity_array = [];
var total_price = 0

var Give_List_Work = []
var Work_List_array = []

var Case_SYSID = '';
var Assign_SYSID;

//$('#test123').click(function () {
            //    var shownVal = document.getElementById("BUSINESSNAME").value;
            //    var value2send = document.querySelector("#select_caseList option[value='" + shownVal + "']").dataset.value;
            //})

$('#btn_Add_Case').click(function () {
    //window.open().location = `${document.location.origin}/2021_case/0010010004.aspx`;
})

$(function () {
    if (seqno != '') {
        Case_SYSID = seqno
    }
    Notiflix.Loading.Standard('Loading...');
    Page_Load()
    
    Listener_event()

    List_Customer(); /*dataTable_Case*/
    List_Personel()
    List_people("") //for add new case and info to push value
    List_Assist_Company();
    //Assign list department
    List_Depart()

    bindtable("", "", "")
    Notiflix.Loading.Remove(800);
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
            Agent_Name = obj.data[0].Agent_Name
            var Agent_ID = obj.data[0].Agent_ID;
        }
    });
}

function Listener_event() {
    $('#Case_End_Reason_tr').hide()
    $('#Case_End_Reason_tr2').hide()
    // add new Case
    $('#btn_add_NewCase').click(function () {
        initial_AddCaseModel()
        
        Product_Name_array = []
        Unit_Price_array = []
        OE_ID_array = []
        Quantity_array = []
        Work_List_array = []
        $('#ol_Work').html('')
        $('#Service').hide()
        $('#Change_Status').hide()
    });

    $('#btn_SystemService').click(function () {
        List_Commodity()
    });

    $('#button3').click(function () {
        //Get th Quantity
        //var Quantity_var;
        //$.each(OE_ID_array, function (index, value) {
        //    let Quantity = $('#Quantity' + value + '').val();
        //    if (Quantity == '') {
        //        Quantity_var = Quantity

        //        Product_Name_array.splice(1, index)
        //        Unit_Price_array.splice(1,index)
        //        alert('選取商品但未填數量。')
                
        //        return false
        //    }
        //})
        //if (Quantity_var == '') {
        //    return
        //}

        $('#List_system').html('');
        $('#List_system').append('<ol id="ol"></ol>');
        if (Product_Name_array.length != 0) {

            // Two foreach check Quantity and 將Quantity標記在html中 by if else
            $.each(Product_Name_array, function (index, value) {
                let new_str;
                //let Quantity = $('#Quantity' + OE_ID_array[index] + '').val();
                let Quantity = Quantity_array[index]
                if (Quantity == '1') {
                    $('#ol').append('<li>' + value + '<button type="button" id="libtn_' + OE_ID_array[index] +'" style="margin-left:10px" class="btn btn-primary" data-toggle="modal" data-target="#dialog6">選擇工作事項</button></li>');
                }
                else {
                    let location = Product_Name_array.indexOf(value)
                    let location_x = value.indexOf('x')
                    if (location_x == -1) {
                        Product_Name_array.splice(location, 1, '' + value + 'x' + Quantity + '')
                    }
                    else {
                        //如果遇到已經含有x的, 將整個字串取代成新的(解決如果更改數量後)
                        let location = value.indexOf('/')
                        let Single_Product_Name = value.substring(0, location)
                        let price = Unit_Price_array[index]
                        new_str = '' + Single_Product_Name + '/' + price + 'x' + Quantity + ''

                        Product_Name_array.splice(index, 1, new_str)
                    }
                    if (value == '' + value + 'x' + Quantity + '' || Product_Name_array[index] == new_str) {
                        $('#ol').append(
                            '<li>' + new_str + '<button type="button" id="libtn_' + OE_ID_array[index] + '" style="margin-left:10px" class="btn btn-primary" data-toggle="modal" data-target="#dialog6">選擇工作事項</button></li>'
                        );
                    }
                    else {
                        $('#ol').append('<li>' + value + 'x' + Quantity + '<button type="button" id="libtn_' + OE_ID_array[index] +'" style="margin-left:10px" class="btn btn-primary" data-toggle="modal" data-target="#dialog6">選擇工作事項</button></li>');
                    }
                    
                }
            })
            //Listen all first string is libtn_'s button
            $('button[id^="libtn_"]').click(function () { 
                console.log('123')
                let btn_ID = $(this).attr('id').split('_')
                let OE_ID = btn_ID[1]
                let location = OE_ID_array.indexOf(OE_ID)
                let Name = (Product_Name_array[location].split('/'))[0] //because Product_Name_array = Ex: IVR服務/1800
                $('#Work_List_title').html(Name + '_' + OE_ID)
                Give_List_Work = []
                List_Work(OE_ID)
            })

            //calculate total price
            let total = 0
            $.each(Unit_Price_array, function (index, value) {
                let Quantity = Quantity_array[index]
                if (Quantity == '1') {
                    total = total + parseInt(value)
                }
                else {
                    total = total + (parseInt(value) * parseInt(Quantity))
                }
                
            })
            total_price = total
            $('#List_system').append('<p style="color:red">總價(未稅):NT$ =' + total + '</p>');

        }

        $('#Work_List').html('')
        Work_List_array = []
        //Get_Work_List(Product_Name_array);
    })

    $('#Service_btn').click(function () {
        $('#Service_table').show()
        $('#btn_AddNewWorkLog').html('新增')
    })

    $('#btn_AddNewWorkLog').click(function () {
        let return_text = Add_Work_Log()
        if (return_text != false) {
            $('#Service_remove_btn').click()
            $('#txt_Work_Log').val('')
        }
        GetWorkLogs(Case_SYSID)
    })

    $('#Service_remove_btn').click(function () {
        $('#Service_table').hide()
        $('#txt_Work_Log').val('')
    })

    $('#Assign_btn').click(function () {
        $('#Chargeback_table').hide()
        $('#Assign_Add_btn').show()

        $('#Assign_Depart').val(-1)
        $('#Select_Assign_People').html('')
        $('#Select_Assign_People').append('<option value="-1">請選擇交辦人員…</option>')
        $('#Select_Assign_People').append(-1)
        $('#Urgent').val(-1)
        $('#Assign_Company_Connection').val('')
        $('#Assign_Company_Phone').val('')
        $('#Assign_title').val('')
        $('#Assign_text').val('')
        $('#Assign_Create_Agent').html(Agent_Name)
        $('#datetimepicker01').val('')
    })

    $('#Assign_Depart').change(function () {
        $("#Select_Assign_People").html('')
        let choose = $('#Assign_Depart').val()
        List_people(choose);
    })

    $('#Urgent').change(function () {
        var currDate = new Date()

        let year = currDate.getFullYear()
        let month = currDate.getMonth() + 1
        let day = currDate.getDate()
        if (parseInt(month) < 10) {
            month = '0' + month
        }
        if (parseInt(day) < 10) {
            day = '0' + day
        }

        var today = year + '-' + month + '-' + day

        var beforeOneWeekDate = new Date(currDate)
        beforeOneWeekDate.setDate(currDate.getDate() + 7)
        let year2 = beforeOneWeekDate.getFullYear()
        let month2 = beforeOneWeekDate.getMonth() + 1
        let day2 = beforeOneWeekDate.getDate()
        if (parseInt(month2) < 10) {
            month2 = '0' + month2
        }
        if (parseInt(day2) < 10) {
            day2 = '0' + day2
        }
        var formatBeforeOneWeekDate = year2 + '-' + month2 + '-' + day2
        let Urgent = $('#Urgent').val()
        switch (Urgent){
            case '軟體維護':
                $('#datetimepicker01').val(formatBeforeOneWeekDate)
                break
            case '硬體維護':
                $('#datetimepicker01').val(formatBeforeOneWeekDate)
                break
            case '硬體設定':
                $('#datetimepicker01').val(formatBeforeOneWeekDate)
                break
            case '軟體修改':
                $('#datetimepicker01').val(formatBeforeOneWeekDate)
                break
            case '系統建置':
                $('#datetimepicker01').val(formatBeforeOneWeekDate)
                break
            case '軟體介接':
                $('#datetimepicker01').val(formatBeforeOneWeekDate)
                break
            case '緊急故障':
                $('#datetimepicker01').val(today)
                break
        }
    })

    $('#Assign_Add_btn').click(function () {
        Assign_To_People();
    })

    $('#Assign_Chargeback_btn').click(function () {
        Change_Assign_Status('-1')
    })

    $('#Search_Date').click(function () {
        let Start_Date = $('#datepicker01').val()
        let End_Date = $('#datepicker02').val()
        let Personel = $('#Dispatch_Name').val()
        if (Start_Date=='' || End_Date == '') {
            alert('起始/結束時間未填。')
            return
        }
        if ((Date.parse(Start_Date)).valueOf() > (Date.parse(End_Date)).valueOf()) {
            alert('起始時間大於結束時間，請重新選擇。')
            return
        }
        if (Personel == '-1') {
            alert('請選擇負責人。')
            return
        }
        bindtable(Start_Date, End_Date, Personel)
    })

    $('#Push_Work_List').click(function () {
        $('#ol_Work').html('')
        $.each(Work_List_array, function (index, value) {
            $('#ol_Work').append('<li id="' + value + '">' + value + '</li>')
        })
    })

    $('#Add_Work_List').click(function () {
        Add_Work_List()
    })

    $('#btn_Change_Status').click(function () {
        $('#End_Reason_tr').hide()
    })

    $('#Change_Status').click(function () {
        Change_Status()
    })

    $('#Status').change(function () {
        let Status = $('#Status').val()
        if (Status == '3') {
            $('#End_Reason_tr').show()
        } else {
            $('#End_Reason_tr').hide()
        }
    })

    //listen to datatable change page
    //$('#data2').on('page.dt', function () {
        
    //    let Check_ID = $('input[type="checkbox"]:checked').map(function () { return this.id }).get()
    //    $.each(Check_ID, function (index, value) {
    //        console.log(value)
    //    })
    //})
}

// 新增案件表格
function initial_AddCaseModel() {
    $('#PID').text('(新增)');
    $('#btn_update').hide();
    $('#btn_new').show();
    $('#List_system').html("");
    $('#txt_caseName').val("");
    $('#BUSINESSNAME').val("");
    $('#Bussiness_ID').val("");
    $('#txt_contactPerson').val("");
    $('#txt_contactPhoneNumber').val("");
    $('#txt_Personnel').val(-1);
    $('#select_Assist_Company').val(0);
    $('#txt_projectContext').val("");
    $('#txt_projectRemark').val("");

    $('#Work_List_tr').hide()
    $('#Work_List').html('')

    $('#Service_html').hide()
    $('#Assign_html').hide()
    Product_Name_array = []
    Unit_Price_array = []
    OE_ID_array = []
    Quantity_array = []
    Work_List_array = []
}

function bindtable(Start_Date, End_Date, Personel) {
    $.ajax({
        url: '0010010005.aspx/bindtable',
        type: 'POST',
        data: JSON.stringify({
            Start_Date: Start_Date,
            End_Date: End_Date,
            Personel: Personel,
        }),
        contentType: 'application/json; charset=UTF-8',
        dataType: "json",       //如果要回傳值，請設成 json
        success: function (doc) {
            Case_Data = eval(doc.d)
            let table = $('#dataTable_Case').DataTable({
                rowReorder: {
                    selector: 'td:nth-child(2)'
                },
                responsive: true,
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
                    //"data": null,
                    "searchable": false,
                    "paging": false,
                    "ordering": false,
                    "info": false
                }],
                columns: [
                    { data: "SYSID" },
                    { data: "Case_Name" },
                    { data: "BUSINESSNAME" },
                    { data: "Contact" },
                    { data: "Phone" },
                    { data: "Personnel" },
                    {
                        data: "updateDate",
                        render: function (data, type, row, meta) {
                            return moment(data).format('YYYY-MM-DD HH:mm:ss');
                        }
                    },
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
                        data: null,
                        render: function (data, type, row, meta) {
                            return `<button type='button' id='info_${row.SYSID}' class='btn btn-info btn-lg' data-toggle='modal' data-target='#newModal' >
                                        <span class='glyphicon glyphicon-search'></span>
                                    </button>`
                                + "&nbsp&nbsp";
                        }
                    },
                    //{
                    //    data: null,
                    //    render: function (data, type, row, meta) {
                    //        return `<button type='button' id='assign' class='btn btn-danger btn-lg' data-toggle='modal' data-target='#dialog4' >
                    //                    <span class='glyphicon glyphicon-pencil'></span>
                    //                </button>`;
                    //    }
                    //}
                ]
            });
            if (seqno != '') {
                $.each(Case_Data, function (index, obj) {
                    let SYSID = obj.SYSID
                    if (SYSID == Case_SYSID) {
                        num = index
                        return false
                    }
                })
                $('#Service').show()
                $('#btn_Change_Status').show()

                Load_Case(Case_Data[num]);
                GetWorkLogs(Case_SYSID)
                Assign_Case_List(Case_SYSID)
                List_Assign_title(Case_SYSID)
                $('#info').click()
            }

            $('#dataTable_Case tbody').on('click', 'button[id^=info_]', function () {
                let data_num = 0;
                let SYSID = $(this).attr('id').split('_')[1]
                $.each(Case_Data, function (index, obj) {
                    let obj_SYSID = obj.SYSID
                    if (obj_SYSID == SYSID) {
                        data_num = index
                        return false
                    }
                })

                $('#Service').show()
                $('#btn_Change_Status').show()

                Load_Case(Case_Data[data_num]);

                GetWorkLogs(SYSID)
                Assign_Case_List(SYSID)
                List_Assign_title(SYSID)

                    
                //Get_Work_List(Product_Name_array)

                Case_SYSID = SYSID //push case SYSID to var
            })
                //.on('click', '#assign', function () {
                //    var SYSID = table.row($(this).parents('tr')).data().SYSID;
                //    Case_SYSID = SYSID //push case SYSID to var
                //});
        }
    });
}

//按下查詢
function Load_Case(dataTable_rowData) {
    initial_AddCaseModel();

    $('#Service_html').show()
    $('#btn_new').hide();
    $('#btn_update').show();
    $('#PID').text('(基本資料)');
    $('#Assign_html').show()

    $('#txt_caseName').val(dataTable_rowData.Case_Name);
    let BUSINESSNAME = $("#select_caseList option[data-value='" + dataTable_rowData.Clinet_Name + "']").val();
    $('#BUSINESSNAME').val(BUSINESSNAME)
    //$("#select_caseList").val(dataTable_rowData.Clinet_Name)

    $("#Bussiness_ID").val(dataTable_rowData.Clinet_Name)
    
    if (dataTable_rowData.System_Data != null && dataTable_rowData.System_Data != '') {
        Product_Name_array = dataTable_rowData.System_Data.split(',');
    }
    if (dataTable_rowData.System_Data_ID != null && dataTable_rowData.System_Data_ID != '') {
        OE_ID_array = dataTable_rowData.System_Data_ID.split(',');
    }

    $('#ol_Work').html('')
    $('#select_ListCase').html('')
    $('#select_ListCase').append('<option value="-1">請選擇工作事項…</option>')
    if (dataTable_rowData.Work_List != null && dataTable_rowData.Work_List != '') {
        Work_List_array = dataTable_rowData.Work_List.split(',');

        $.each(Work_List_array, function (index, value) {
            $('#ol_Work').append('<li id="' + value + '">' + value + '</li>')
            $('#select_ListCase').append('<option value="'+value+'">'+value+'</option>')
        })
    }

    if (Product_Name_array != '' && Product_Name_array != null)
    {
        $('#List_system').append('<ol id="ol"></ol>')
        $.each(Product_Name_array, function (index, value) {
            $('#ol').append(
                '<li>' + Product_Name_array[index] + '<button type="button" id="libtn_' + OE_ID_array[index] + '" style="margin-left:10px" class="btn btn-primary" data-toggle="modal" data-target="#dialog6">選擇工作事項</button></li>'
            );
            // put Quantity to var  Quantity_array
            let location = value.indexOf('x')
            if (location == -1) {
                Quantity_array.push('1')
            }
            else {
                let Quantity = value.slice(location + 1)
                Quantity_array.push(Quantity)
            }
        })

        $('button[id^="libtn_"]').click(function () {
            let btn_ID = $(this).attr('id').split('_')
            let OE_ID = btn_ID[1]
            let location = OE_ID_array.indexOf(OE_ID)
            let Name = (Product_Name_array[location].split('/'))[0] //because Product_Name_array = Ex: IVR服務/1800
            $('#Work_List_title').html(Name + '_' + OE_ID)
            Give_List_Work = []
            List_Work(OE_ID)
        })


    }
    
        
    total_price = dataTable_rowData.Total_price
    //Get price from "ACD/199999"
    $.each(Product_Name_array, function (index, value) {
        let location = value.indexOf('/')
        let price = value.substring(location + 1, 20)

        let location2 = price.indexOf('x')
        if (location2 == -1) {
            Unit_Price_array.push(price)
        } else {
            price = price.substring(0, location - 1)
            Unit_Price_array.push(price)
        }
    })
    console.log()
    if (dataTable_rowData.Status == '3') {
        $('#Case_End_Reason_tr').show()
        $('#Case_End_Reason_tr2').show()
        $('#Case_End_Reason').val(dataTable_rowData.End_Reason)
    }
    else {
        $('#Case_End_Reason_tr').hide()
        $('#Case_End_Reason_tr2').hide()
    }

    $('#List_system').append('<p style="color:red">總價(未稅):NT$ = ' + dataTable_rowData.Total_price + '</p>');
    $('#txt_contactPerson').val(dataTable_rowData.Contact);
    $('#txt_contactPhoneNumber').val(dataTable_rowData.Phone);
    $('#txt_Personnel').val(dataTable_rowData.Personnel);
    $("#select_Assist_Company").val(dataTable_rowData.Vendor_SYSID)
    $('#txt_projectContext').val(dataTable_rowData.Project_Content);
    $('#txt_projectRemark').val(dataTable_rowData.Remark);
    $('#Status').val(dataTable_rowData.Status);
}

function List_Assign_title(SYSID) {
    console.log(SYSID)
    $.ajax({
        url: '0010010005.aspx/List_Assign_title',
        type: 'POST',
        data: JSON.stringify({ SYSID: SYSID }),
        contentType: 'application/json; charset=UTF-8',
        dataType: "json",       //如果要回傳值，請設成 json
        success: function (doc) {
            var obj = JSON.parse(doc.d);
            //$("#select_caseList").append("<option value='0'''>" + "請選擇客戶" + "</option>");
            let check_same = []
            $.each(obj, function (idx, obj) {
                let location = check_same.indexOf(obj.Assign_title)
                if (location == -1) {
                    $("#select_ListCase").append("<option value='" + obj.Assign_title + "'>" + obj.Assign_title + "</option>");
                }
                check_same.push(obj.Assign_title)
            });
        }
    });
}

function List_Customer() {
    $.ajax({
        url: '0010010005.aspx/List_Customer',
        type: 'POST',
        /*data: JSON.stringify({ value: value }),*/
        contentType: 'application/json; charset=UTF-8',
        dataType: "json",       //如果要回傳值，請設成 json
        success: function (doc) {
            var obj = JSON.parse(doc.d);
            //$("#select_caseList").append("<option value='0'''>" + "請選擇客戶" + "</option>");
            $.each(obj, function (idx, obj) {
                $("#select_caseList").append("<option data-value='" + obj.ID + "' value='" + obj.BUSINESSNAME + "'></option>");
                $('#DropClientCode').append("<option data-value='" + obj.ID + "' value='" + obj.BUSINESSNAME + "'></option>")
            });

            $('#BUSINESSNAME').bind('input', function () {
                let showVal = $('#BUSINESSNAME').val() //chinese value
                let ID = $("#select_caseList option[value='" + showVal + "']").attr('data-value'); //get data-value
                $.each(obj, function (idx, obj) {
                    if (obj.ID == ID) {
                        $('#Bussiness_ID').val(obj.ID)
                        $('#txt_contactPerson').val(obj.APPNAME)
                        $('#txt_contactPhoneNumber').val(obj.APP_MTEL)
                    }
                });
            })
        }
    });
}

function List_Personel() { //for search
    $.ajax({
        url: '0010010005.aspx/List_Personel',
        type: 'POST',
        /*data: JSON.stringify({ value: value }),*/
        contentType: 'application/json; charset=UTF-8',
        dataType: "json",       //如果要回傳值，請設成 json
        success: function (doc) {
            var obj = JSON.parse(doc.d);
            $("#Dispatch_Name").append("<option value='-1'>請選擇負責人…</option>");
            $.each(obj, function (idx, obj) {
                $("#Dispatch_Name").append("<option value='" + obj.Personnel + "'>" + obj.Personnel + "</option>");
            });
        }
    });
}

// 查詢：系統資料
function List_Commodity() {
    $.ajax({
        url: '0010010005.aspx/List_Commodity',
        type: 'POST',
        contentType: 'application/json; charset=UTF-8',
        dataType: "json",       //如果要回傳值，請設成 json
        success: function (doc) {
            var table = $('#data2').DataTable({
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
                    { data: "Product_Name" },
                    { data: "Unit_Price" },
                    {
                        data: "OE_ID", render: function (data, type, row, meta) {
                            let location = OE_ID_array.indexOf(data)
                            if (location == -1) {
                                return "<input type='intput' id='Price_" + data + "' class='form-control' style=' width: 150px;' " +
                                    " maxlength='10'  onchange=''/>";
                            }
                            else {
                                return "<input type='intput' id='Price_" + data + "' value='"+Unit_Price_array[location]+"' class='form-control' style=' width: 150px;' " +
                                    " maxlength='10'  onchange=''/>";
                            }
                        }
                    },
                    {
                        data: "OE_ID", render: function (data, type, row, meta) {
                            let check_ornot = " ";
                            if (OE_ID_array.indexOf(data) != -1) {
                                check_ornot = "checked"
                            }
                            return "<div class='checkbox'><label>" +
                                "<input type='checkbox' style='width: 30px; height: 30px;' id='check_"+data+"'  " + check_ornot + "/></label></div>";
                        }
                    },
                    {
                        data: "OE_ID", render: function (data, type, row, meta) {
                            let location = OE_ID_array.indexOf(data)
                            if (location == -1) {
                                return '<input type="number" id="Quantity_' + data + '" class="form-control" min="1" style="width: 70px;" ' +
                                    ' maxlength="5" />';
                            }
                            else {
                                return '<input type="number" id="Quantity_' + data + '" value="'+Quantity_array[location]+'" class="form-control" min="1" style="width: 70px;" ' +
                                    ' maxlength="5" />';
                            }
                            
                        }
                    },
                ]
            });
            //function end to run this put the input Quantity to default 1
            $.each(OE_ID_array, function (index, value) {
                let Single_str = Product_Name_array[index]
                let location = Single_str.indexOf('x')
                let price;
                if (location == -1) {
                    price = Single_str.replace(/[^0-9]/ig, ""); //get number of string
                    $('#Price_' + value + '').val(price);
                    $('#Quantity_' + value + '').val('1');
                }
                else {
                    let remove_x = Single_str.replace('x' + Single_str[location + 1], ' ') //if 2 or more Quantity remove "x2" and get price
                    price = remove_x.replace(/[^0-9]/ig, "");
                    $('#Price_' + value + '').val(price);
                    $('#Quantity_' + value + '').val(Single_str[location + 1]); //(Ex: x2, x3) "x"'s location +1 = Quantity
                }
                
            })

            $('#data2 tbody').on('change', 'input[id^=Price_]', function () {
                let OE_ID = $(this).attr('id').split('_')[1]
                let Change_price = $('#Price_' + OE_ID + '').val()
                let location = OE_ID_array.indexOf(OE_ID)
                if (location != -1) {
                    let Product_Name = Product_Name_array[location].split('/')[0]
                    let Price = Product_Name_array[location].split('/')[1]

                    Product_Name_array.splice(location, 1, '' + Product_Name + '/' + Change_price + '')
                    Unit_Price_array.splice(location, 1, Change_price)
                }
            })

            $('#data2 tbody').on('change', 'input[id^=Quantity_]', function () {
                let OE_ID = $(this).attr('id').split('_')[1]
                let Change_Quantity = $('#Quantity_' + OE_ID + '').val()
                console.log(Change_Quantity)
                let location = OE_ID_array.indexOf(OE_ID)
                if (location != -1) {
                    Quantity_array.splice(location, 1, Change_Quantity)
                }
            })

            $('#data2 tbody').unbind('click').
                on('click', 'input[id^=check_]', function () {
                    var table = $('#data2').DataTable();
                    var Product_Name = table.row($(this).parents('tr')).data().Product_Name.replace(/^\s*|\s*$/g, "");
                    var Unit_Price = table.row($(this).parents('tr')).data().Unit_Price.replace(/^\s*|\s*$/g, "");
                    var OE_ID = table.row($(this).parents('tr')).data().OE_ID;

                    //put default value in input
                    let Default_input = $('#Price_' + OE_ID + '').val()
                    if (Default_input == '') {
                        $('#Price_' + OE_ID + '').val(Unit_Price)
                        $('#Quantity_' + OE_ID + '').val('1')
                    }
                    else {
                        $('#Price_' + OE_ID + '').val('')
                        $('#Quantity_' + OE_ID + '').val('')
                    }
                    let Real_Price = $('#Price_' + OE_ID + '').val()
                    var location = OE_ID_array.indexOf(OE_ID)

                    if (location == -1) {
                        Product_Name_array.push(Product_Name + '/' + Real_Price)
                        Unit_Price_array.push(Real_Price)
                        OE_ID_array.push(OE_ID)
                        Quantity_array.push('1')
                    }
                    else {
                        Product_Name_array.splice(location, 1)
                        Unit_Price_array.splice(location, 1)
                        OE_ID_array.splice(location, 1)
                        Quantity_array.splice(location,1)
                        $('#ol_Work').html('')
                    }
                    //console.log(Product_Name_array)
                    //console.log(Unit_Price_array)
                    //console.log(OE_ID_array)  
                })
        }
    });
    
}

function Safe(type) {
    let Case_Name = $("#txt_caseName").val();

    let showVal = $('#BUSINESSNAME').val() //chinese value
    let ID = $("#select_caseList option[value='" + showVal + "']").attr('data-value'); //get data-value
    let Clinet_Name = ID;

    let Contact = $("#txt_contactPerson").val();
    let Phone = $("#txt_contactPhoneNumber").val();
    let System_Data = Product_Name_array.toString().replace(/\s*/g, ""); // let array to string and clean blank
    let System_Data_ID = OE_ID_array.toString().replace(/\s*/g, "");
    let Work_List = Work_List_array.toString().replace(/\s*/g, "");
    let Total_Price = total_price.toString();
    let Personnel = $("#txt_Personnel").val();
    let Assit_Company = $("#select_Assist_Company").val();
    let Project_Content = $("#txt_projectContext").val();
    let Remark = $("#txt_projectRemark").val();

    if (Case_Name == "") {
        alert('請輸入案件名稱。')
        return
    }
    if (showVal == "") {
        alert('請選擇客戶。')
        return
    }
    if (Contact == "") {
        alert('請輸入聯絡人。')
        return
    }
    if (Phone == '') {
        alert('請輸入連絡電話。')
        return
    }
    if (Personnel == "-1") {
        alert('請選擇處理人員。')
        return
    }
    if (Project_Content == "") {
        alert('請輸入專案內容。')
        return
    }

    $.ajax({
        url: '0010010005.aspx/Safe',
        type: 'POST',
        data: JSON.stringify({
            Case_SYSID: Case_SYSID,
            Type: type.toString(),
            Case_Name: Case_Name,
            Clinet_Name: Clinet_Name,
            Contact: Contact,
            Phone: Phone,
            System_Data: System_Data,
            System_Data_ID: System_Data_ID,
            Work_List: Work_List,
            Total_Price: Total_Price,
            Personnel: Personnel,
            Assit_Company: Assit_Company,
            Project_Content: Project_Content,
            Remark: Remark
        }),
        contentType: 'application/json; charset=UTF-8',
        dataType: "json",       //如果要回傳值，請設成 json
        success: function (doc) {
            var json = JSON.parse(doc.d);
            console.log(json)
            var alert_text = json.status
            alert(alert_text)
            if (alert_text != '已有相同的案件名稱。') {
                window.location.reload();
            }
        }
    });
}

function List_Work(OE_ID) {
    $.ajax({
        url: '0010010005.aspx/List_Work',
        type: 'POST',
        data: JSON.stringify({
            OE_ID: OE_ID
        }),
        contentType: 'application/json; charset=UTF-8',
        dataType: "json",       //如果要回傳值，請設成 json
        success: function (doc) {
            var obj = JSON.parse(doc.d);
            let Work_List_item = obj[0].Work_List.split(',')
            $('#Work_List_check').html('')
            if (obj[0].Work_List != '') {
                $.each(Work_List_item, function (index, value) {
                    let location = Work_List_array.indexOf(value)
                    let checked = ''
                    if (location == -1) {
                        checked = ''
                    }
                    else {
                        Give_List_Work.push(value)
                        checked = 'checked'
                    }
                    $('#Work_List_check').append('<p><input type="checkbox" id="Workcheck_' + index + '" class="circle_box" ' + checked + ' /><label id="Workval_' + index + '">' + value + '</label></p>')
                })
            }

            $('input[id^="Workcheck_"]').click(function () {
                let checkbox_ID = $(this).attr('id').split('_')
                let index = checkbox_ID[1]
                let checked_value = $('#Workval_' + index + '').html()

                let location = Work_List_array.indexOf(checked_value)
                let location2 = Give_List_Work.indexOf(checked_value)
                if (location == -1) {
                    Work_List_array.push(checked_value)
                    Give_List_Work.push(checked_value)
                }
                else {
                    Work_List_array.splice(location, 1)
                    Give_List_Work.splice(location2, 1)
                }
            })
        }
    });
}

//function Get_Work_List(Product_Name_arr) {
//    $.each(Product_Name_arr, function (index, value) {
//        let location = value.indexOf('/')
//        let Single_Product_Name = value.substring(0, location)
//        $.ajax({
//            url: '0010010005.aspx/Get_Work_List',
//            type: 'POST',
//            data: JSON.stringify({
//                Single_Product_Name: Single_Product_Name
//            }),
//            contentType: 'application/json; charset=UTF-8',
//            dataType: "json",       //如果要回傳值，請設成 json
//            success: function (doc) {
//                var obj = JSON.parse(doc.d);
//                let Work_List = obj[0].Work_List
//                //check list open or not
//                let location = Work_List_array.indexOf(Work_List)
//                let location2 = Work_List_array.indexOf('"無"工作事項' + index + '')
//                if (location != -1 || location2 != -1) {
//                    return
//                }

//                if (Work_List == null || Work_List == '') {
//                    $('#Work_List').append('<p>' + Single_Product_Name + ':"無"工作事項</p>') //push html to worklist
//                    Work_List_array.push('"無"工作事項' + index + '') //this index is easy to check same list
//                }
//                else{
//                    $('#Work_List').append('<p>' + Single_Product_Name + ':' + Work_List + '</p>') //push html to worklist
//                    Work_List_array.push(Work_List)
//                }
//            }
//        });
//    })
//}

function GetWorkLogs(SYSID) {
    $.ajax({
        url: '0010010005.aspx/GetWorkLogs',
        type: 'POST',
        data: JSON.stringify({ SYSID: SYSID }),
        contentType: 'application/json; charset=UTF-8',
        dataType: "json",
        success: function (doc) {
            var table = $('#data3').DataTable({
                destroy: true,
                data: eval(doc.d),
                //bAutoWidth: false, //not auto set width
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
                    {
                        data: "Create_Agent",
                    },
                    {
                        data: "Work_Name",
                    },
                    {
                        data: "Work_Log"
                    },
                    {
                        data: "Create_Date",
                    },
                    {
                        data: "SYSID",
                        render: function (data, type, row, meta) {
                            return `<button type='button' id='Edit_Work_Log' class='btn btn-primary btn-lg'>
                                        <span class='glyphicon glyphicon-pencil'></span></button>`;
                        }
                    },
                ],
            });

            $('#data3 tbody').unbind('click')
                .on('click', '#Edit_Work_Log', function () {
                    var table = $('#data3').DataTable();
                    var Work_Name = table.row($(this).parents('tr')).data().Work_Name;
                    var Work_Log = table.row($(this).parents('tr')).data().Work_Log;
                    var SYSID = table.row($(this).parents('tr')).data().SYSID;

                    $('#Work_Log_SYSID').html(SYSID)
                    $('#Service_table').show()
                    $('#select_ListCase').val(Work_Name)
                    $('#txt_Work_Log').val(Work_Log)
                    $('#btn_AddNewWorkLog').html('修改')
                });
        }
    });
}

function Add_Work_Log() {
    //for update Log
    let btn_html = $('#btn_AddNewWorkLog').html() //string edit
    let Work_Log_SYSID = $('#Work_Log_SYSID').html()

    let select_ListCase = $('#select_ListCase').val()
    let txt_Work_Log = $('#txt_Work_Log').val()
    let SYSID = Case_SYSID;

    if (select_ListCase == '-1') {
        alert('請選擇工作事項。')
        return false
    }
    if (txt_Work_Log == '') {
        alert('請填寫服務紀錄。')
        return false
    }

    $.ajax({
        url: '0010010005.aspx/Add_Work_Log',
        type: 'POST',
        data: JSON.stringify({
            btn_html: btn_html,
            Work_Log_SYSID: Work_Log_SYSID,
            SYSID: SYSID,
            txt_Work_Log: txt_Work_Log,
            select_ListCase: select_ListCase
        }),
        contentType: 'application/json; charset=UTF-8',
        dataType: "json",       //如果要回傳值，請設成 json
        success: function (doc) {
            var jsonparse = JSON.parse(doc.d);

            alert(jsonparse.status)
            $('#Work_Log_SYSID').html('')
            GetWorkLogs(SYSID)
            //window.location.reload()
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
            if (Depart == "") {
                let txt_Personnel = $('#txt_Personnel')
                txt_Personnel.html('')
                txt_Personnel.append('<option value="-1">請選擇處理人員…</option>')
                $.each(jsonParse, function (index, value) {
                    txt_Personnel.append(`<option value='${value.Agent_Name}'>${value.Agent_Name}</option>`);
                });
            }
            else {
                let Select_Assign_People = $("#Select_Assign_People");
                Select_Assign_People.append(`<option value='-1'>請選擇交辦人員</option>`);
                $.each(jsonParse, function (index, value) {
                    Select_Assign_People.append(`<option value='${value.SYSID}'>${value.Agent_Name}</option>`);
                });
            }
        }
    });
}

function List_Assist_Company() {
    $.ajax({
        url: "0010010005.aspx/List_Assist_Company",
        type: 'POST',
        contentType: 'application/json; charset=UTF-8',
        dataType: "json",
        success: function (data) {
            var jsonParse = JSON.parse(data.d);
            let select_Assist_Company = $("#select_Assist_Company");

            $.each(jsonParse, function (index, value) {
                select_Assist_Company.append(`<option value='${value.SYSID}'>${value.Vendor_Name}</option>`);
            });
        }
    });
}

function Assign_To_People() {
    let Assign_Depart = $('#Assign_Depart').val()
    let Select_Assign_People = $('#Select_Assign_People').val()
    let Urgent = $('#Urgent').val()
    let Assign_Company_Connection = $('#Assign_Company_Connection').val()
    let Assign_Company_Phone = $('#Assign_Company_Phone').val()
    let Assign_title = $('#Assign_title').val()
    let Assign_text = $('#Assign_text').val()
    let End_date = $('#datetimepicker01').val()
    let Assign_Create_Agent = $('#Assign_Create_Agent').html()

    if (Assign_Depart == "-1") {
        alert('請選擇交辦部門。')
        return
    }
    if (Select_Assign_People == "-1") {
        alert('請選擇交辦人員。')
        return
    }
    if (Urgent == "-1") {
        alert('請選擇緊急程度。')
        return
    }
    if (Assign_title == '') {
        alert('請填入交辦事項標題。')
        return
    }
    if (Assign_text == '') {
        alert('請填入交辦事項。')
        return
    }
    if (End_date == "") {
        alert('請選擇預計時程。')
        return
    }

    $.ajax({
        url: "0010010005.aspx/Assign_To_People",
        type: 'POST',
        data: JSON.stringify({
            Assign_Depart: Assign_Depart,
            Select_Assign_People: Select_Assign_People,
            Urgent: Urgent,
            Assign_Company_Connection: Assign_Company_Connection,
            Assign_Company_Phone: Assign_Company_Phone,
            Assign_title: Assign_title,
            Assign_text: Assign_text,
            End_date: End_date,
            Assign_Create_Agent: Assign_Create_Agent,
            Case_SYSID: Case_SYSID,
        }),
        contentType: 'application/json; charset=UTF-8',
        dataType: "json",
        success: function (data) {
            List_Assign_title(Case_SYSID)
            Assign_Case_List(Case_SYSID)
            var jsonParse = JSON.parse(data.d)
            alert(jsonParse.status)
        }
    });
}

function Assign_Case_List(SYSID) {
    $.ajax({
        url: '0010010005.aspx/Assign_Case_List',
        type: 'POST',
        data: JSON.stringify({
            SYSID: SYSID,
        }),
        contentType: 'application/json; charset=UTF-8',
        dataType: "json",       //如果要回傳值，請設成 json
        success: function (doc) {
            let table = $('#data4').DataTable({
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
                    //{ data: "SYSID" },
                    { data: "Create_Date" },
                    { data: "Assign_Create_Agent" },
                    { data: "Urgent" },
                    { data: "Agent_Name" },
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
                                    else {
                                        return ""
                                        break
                                    }
                                case '1':
                                    return ""
                                    break
                                case '-1':
                                    return ""
                                    break
                            }
                        }
                    },
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
                    {
                        data: "SYSID", render: function (data, type, row, meta) {
                            return "<button type='button' id='Assign_Search' class='btn btn-info btn-lg' data-toggle='modal' data-target='#dialog4'>" +
                                    "<span class='glyphicon glyphicon-search'></span>" +
                                    "</button>&nbsp&nbsp"
                        }
                    },
                ]
            });
            $('#data4 tbody').unbind('click').
                on('click', '#Agree_Assign', function () {
                    var table = $('#data4').DataTable();
                    var SYSID = table.row($(this).parents('tr')).data().SYSID;
                    Assign_SYSID = SYSID //push Assign case to var
                    Change_Assign_Status('1')
                })
                .on('click', '#Chargeback_Assign', function () {
                    var table = $('#data4').DataTable();
                    var SYSID = table.row($(this).parents('tr')).data().SYSID;
                    Assign_SYSID = SYSID
                })
                .on('click', '#Assign_Search', function () {
                    var table = $('#data4').DataTable();
                    var data = table.row($(this).parents('tr')).data()
                    Push_Assign_Case_html(data)
                });
        }
    });
}

function Push_Assign_Case_html(data) {
    $('#Assign_Depart').val(data.Assign_Depart)
    $('#Select_Assign_People').html('')
    $('#Select_Assign_People').append('<option value="' + data.Agent_Name + '">' + data.Agent_Name + '</option>')
    $('#Select_Assign_People').val(data.Agent_Name)
    $('#Urgent').val(data.Urgent)
    $('#Assign_Company_Connection').val(data.Assign_Company_Connection)
    $('#Assign_Company_Phone').val(data.Assign_Company_Phone)
    $('#Assign_title').val(data.Assign_title)
    $('#Assign_text').val(data.Assign_text)
    $('#datetimepicker01').val(data.End_date)
    $('#Assign_Create_Agent').html(data.Assign_Create_Agent)

    $('#Assign_Add_btn').hide()

    if (data.Status == '-1') {
        $('#Chargeback_table').show()
        $('#List_Chargeback_text').val(data.Chargeback_Content)
    }
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
            Assign_Case_List(Case_SYSID)
        }
    });
}

function Add_Work_List() {
    let Work_Name = $('#Text5').val()
    let Work_List_title = $('#Work_List_title').html()
    let OE_ID = Work_List_title.split('_')[1]
    if (Work_Name == '') {
        alert('請輸入工作事項')
        return
    }
    console.log(Work_List_title)
    console.log(OE_ID)

    $.ajax({
        url: "0010010005.aspx/Add_Work_List",
        type: 'POST',
        data: JSON.stringify({
            OE_ID: OE_ID,
            Work_Name: Work_Name
        }),
        contentType: 'application/json; charset=UTF-8',
        dataType: "json",
        success: function (data) {
            var jsonParse = JSON.parse(data.d)
            alert(jsonParse.status)
            List_Work(OE_ID)
        }
    });
}

function Change_Status() {
    let confirm_str = ''
    let End_Reason = $('#End_Reason').val();
    let Status = $('#Status').val()

    if (Status == -1) {
        alert('請選擇案件狀態。')
        return
    }
    switch (Status) {
        case "0":
            confirm_str = '開發中'
            break
        case "1":
            confirm_str = '裝機中'
            break
        case "2":
            confirm_str = '維護中'
            break
        case "3":
            confirm_str = '結案'
            break
    }
    if (Status == '3') {
        if (End_Reason == '') {
            alert('請輸入結案原因。')
            return
        }
    }

    if (confirm('案件狀態即將轉成'+confirm_str+'，按下確定以完成更改，若想離開請點擊取消。')) {
        $.ajax({
            url: "0010010005.aspx/Change_Status",
            type: 'POST',
            data: JSON.stringify({
                Status: Status,
                Case_SYSID: Case_SYSID,
                End_Reason: End_Reason,
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
}
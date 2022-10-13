<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="0060010002.aspx.cs" Inherits="_0060010002" %>

<asp:Content ID="Content2" ContentPlaceHolderID="head2" Runat="Server">
    <style>
        body
        {
            font-family: "Microsoft JhengHei",Helvetica,Arial,Verdana,sans-serif;
            font-size: 18px;
        }

        thead th
        {
            background-color: #666666;
            color: white;
        }

        tr td:first-child,
        tr th:first-child
        {
            border-top-left-radius: 8px;
            border-bottom-left-radius: 8px;
        }

        tr td:last-child,
        tr th:last-child
        {
            border-top-right-radius: 8px;
            border-bottom-right-radius: 8px;
        }

        #data2 td:nth-child(6), #data2 td:nth-child(5), #data2 td:nth-child(4),
        #data2 td:nth-child(3), #data2 td:nth-child(2), #data2 td:nth-child(1),
        #data td:nth-child(6), #data td:nth-child(5), #data td:nth-child(4),
        #data td:nth-child(3), #data td:nth-child(2), #data td:nth-child(1), #data th:nth-child(5)
        {
            text-align: center;
        }
    </style>
    <div id="main" class="container" style="width:95%">
        <div class="table-responsive" >
            <h2>廠商資料&nbsp; &nbsp;
                <button id="btn_Add" type="button" data-toggle="modal" data-target="#dialog1" class="btn btn-success btn-lg">
                    <span class='glyphicon glyphicon-plus' ></span>&nbsp;&nbsp;新增廠商
                </button>
                <%--<button type="button" id="btn_add_NewCase" class="btn btn-success btn-lg" data-toggle="modal" data-target="#newModal">
                    <span class='glyphicon glyphicon-plus'></span>&nbsp;&nbsp;新增案件
                </button>--%>
            </h2>
            <table id="Vendor_List" class="display table table-striped" style="text-align:center;">
                <thead>
                    <tr>
                        <th style="text-align:center;">編號</th>
                        <th style="text-align:center;">廠商名稱</th>
                        <th style="text-align:center;">聯絡人</th>
                        <th style="text-align:center;">連絡電話</th>
                        <th style="text-align:center;">查詢</th>
                    </tr>
                </thead>
            </table>
        </div>
    </div>
        <%--===================廠商內容===================--%>
    <div class="modal fade" id="dialog1" role="dialog" data-backdrop="static" data-keyboard="false">
        <div class="modal-dialog" style="width: 1200px;"
            >
        <!-- Modal content-->
            <div class="modal-content">

                <div class="modal-header">
                    <button type="button" class="close" data-dismiss="modal">&times;</button>
                    <h2 class="modal-title"><strong>
                        <label id="title_modal"></label>
                    </strong></h2>
                </div>

                <div class="modal-body">
                    <table class="table table-bordered table-striped">
                        <thead>
                            <tr>
                                <th style="text-align: center" colspan="4">
                                    <span style="font-size: 20px"><strong>廠商資料</strong></span>
                                </th>
                            </tr>
                        </thead>
                        <tbody>
                            <tr style="height: 55px;">
                                <td style="text-align: center; width: 15%">
                                    <strong>廠商名稱</strong>
                                </td>
                                <td style="text-align: center; width: 35%">
                                   <input type="text" class="form-control" id="Vendor_Name" name="Vendor_Name" maxlength="30"  style="background-color: #ffffbb" value="" />                       
                                </td>
                                <th style="text-align: center; width: 15%">
                                    <strong>統一編號</strong>
                                </th>
                                <th style="width: 35%">
                                    <input type="text" class="form-control" id="Vendor_ID" name="Vendor_ID" maxlength="30"  style="background-color: #ffffbb" value="" />
                                </th>
                    
                            </tr>
                            <tr>
                                <td style="text-align: center; width: 15%">
                                    <strong>聯絡人</strong>
                                    <!--預定處理時間 -->
                                </td>
                                <td style="width: 35%">
                                    <input type="text" class="form-control" id="Vendor_Connection" name="txt_Type_Value" maxlength="30"  style="background-color: #ffffbb" value="" />
                                </td>
                                <td style="text-align: center">
                                    <strong>連絡電話</strong>
                                </td>
                                <th>
                                    <input type="text" class="form-control" id="Vendor_phone" name="txt_Type_Value" maxlength="30"  style="background-color: #ffffbb" value="" />
                                </th>
                            </tr>
                            <tr style="height: 55px;">
                                <th style="text-align: center; width: 15%">
                                    <strong>選擇主分類</strong>
                                </th>
                                <th style="width: 35%">
                                    <button type="button" class="btn btn-primary" data-toggle="modal" data-target="#dialog2" id="Add_Main">
                                    <span class="glyphicon glyphicon-ok"></span>&nbsp;&nbsp;添加</button>
                                </th>
                                <td style="text-align: center">
                                    <strong>建立人員</strong>
                                </td>
                                <td>
                                    <label id="Create_Agent"></label>
                                </td>
                            </tr>
                            <tr>
                                <th style="text-align: center;" colspan="4">
                                    <button type="button" class="btn btn-success btn-lg" id="Add_Vendor">
                                        <span class="glyphicon glyphicon-ok"></span>&nbsp;&nbsp;新增</button>
                                    <button type="button" class="btn btn-success btn-lg" id="Edit_Vendor">
                                        <span class="glyphicon glyphicon-pencil"></span>&nbsp;&nbsp;修改</button>
                                </th>
                            </tr>
                        </tbody>
                    </table>
                    <div id="Vendor_Owner_Main" style="display:block">
                        <span style="font-size: 36px"><strong>廠商系統清單</strong></span>
                        <table id="data4" class="display table table-striped" style="width: 99%">
                                <thead>
                                    <tr>
                                        <th style="text-align: center; width: 10%;">編號</th>
                                        <th style="text-align: center; width: 10%;">主分類</th>
                                        <th style="text-align: center; width: 10%;">子分類</th>
                                    </tr>
                                </thead>
                        </table>
                    </div>
                </div>
            </div>
        </div>
    </div>
     <!-- =========== 選擇主分類 =========== -->
    <div class="modal fade" style="width:100%" id="dialog2" role="dialog" data-backdrop="static" data-keyboard="false">
        <div class="modal-dialog" style="width: 1200px;">
            <!-- Modal content-->
            <div class="modal-content">
                <div class="modal-header">
                    <button type="button" class="close" data-dismiss="modal">&times;</button>
                    <h2 class="modal-title"><strong>
                        <label>選擇主分類</label>
                    </strong></h2>
                </div>
                <div class="modal-body">
                    <table id="Main_list" class="table table-bordered table-striped" style="width: 99%">
                        <tbody>
                            <tr>
                                <td>
                                    <table class="display table table-striped" style="width: 99%">
                                        <thead>
                                            <tr>
                                                <th style="text-align: center;">主分類</th>
                                                <th style="text-align: center;">子分類</th>
                                                <th style="text-align: center;">選擇</th>
                                            </tr>
                                        </thead>
                                    </table>
                                </td>
                            </tr>
                        </tbody>
                    </table>
                </div>
                <div class="modal-footer">
                    <button type="button" id="Add_Main_List" data-dismiss="modal" class="btn btn-success btn-lg"style="Font-Size: 20px; " >
                    <span class='glyphicon glyphicon-ok'></span>&nbsp;&nbsp;確定</button>
                    <button type="button" class="btn btn-danger btn-lg" data-dismiss="modal"><span class='glyphicon glyphicon-remove'></span>關閉</button>
                </div>
            </div>
        </div>
    </div>
    <!-- =========== 選擇主分類 =========== -->
     
    <script type="text/javascript">
        var seqno = '<%= seqno %>';
    </script>
    <script src="../js/jquery.js"></script>
    <script src="../js/notiflix.js"></script>
    <link href="../js/notiflix.css" rel="stylesheet" />
    <script src="../DataTables/jquery.dataTables.min.js"></script>
    <script src="../fullcalendar-2.8.0/lib/moment.min.js"></script>
    <script src="0060010002.js"></script>
</asp:Content>


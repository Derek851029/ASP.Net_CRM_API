<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="0010010004.aspx.cs" Inherits="_2021_case_0010010004" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="head2" Runat="Server">
    <div class="container">
        <table class="table table-bordered">
            <tr>
                <td class="titleclass" colspan="4">
                    <h3>新增案件</h3>
                </td>
            </tr>
            <tr>
                <td class="thclass">案件名稱／客戶名稱</td>
                <td>
                    <input id="txt_Case_Name" name="caseData" placeholder="案件名稱..."/>
                    <hr />
                    <input id="txt_Clinet_Name" list="select_Clinet_Name" placeholder="客戶名稱..." name="caseData"/>
                    <datalist id="select_Clinet_Name"></datalist> 
                    <button id="btn_Clean_Clinet_Name" type="button" class="btn btn-danger">重選</button>
                    <br />
                    (若沒出現選項可直接填寫)
                </td>
                <td class="thclass">負責人</td>
                <td>
                    <input id="txt_Personnel" list="select_Personnel" placeholder="負責人..." name="caseData"/>
                    <datalist id="select_Personnel"></datalist> 
                    <button id="btn_Clean_Personnel" type="button" class="btn btn-danger">重選</button>
                    <br />
                    <hr />
                    <select id="Chose_depart">
                            <option value="-1">請選擇處理狀態…</option>
                            <option value="工程師">規劃中</option>
                            <option value="業務">進行中</option>
                            <option value="助理工程師">維護中</option>
                            <option value="助理工程師">已結案</option>
                    </select>
                    
                </td>
            </tr>
            <tr>
                <td class="thclass">聯繫方式</td>
                <td colspan="3">
                    <table class="table table-bordered">
                        <tr>
                            <td class="tthclass">聯絡窗口</td>
                            <td class="tthclass">聯絡電話</td>
                            <td class="tthclass">聯絡e-mail</td>
                            <td class="tthclass"></td>
                        </tr>
                        <tr>
                            <td>
                                <input id="txt_ContactPerson" placeholder="窗口..."/>
                            </td>
                            <td>
                                <input id="txt_ContactPhoneNumber" placeholder="電話..."/>
                            </td>
                            <td>
                                <input id="txt_ContactEmail" placeholder="email..."/>
                            </td>
                            <td>
                                <button id="btn_Add_Contact" type="button" class="btn btn-primary">
                                    <span>+ 新增</span>
                                </button>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="4">
                                <input type="number" id="txt_count_ContactItem" value="0"/>
                                <ol id="ul_ContactList" name="contactItem"></ol>
                            </td>
                        </tr>
                        <tr>
                            <td class="tthclass" colspan="4"></td>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr>
                <td class="thclass">OE資料</td>
                <td colspan="3">
                    <table class="table table-bordered">
                        <tr>
                            <td class="tthclass">OE項目</td>
                            <td class="tthclass">價位</td>
                            <td class="tthclass"></td>
                        </tr>
                        <tr>
                            <td>
                                <input id="txt_OEItem" list="select_OEList" placeholder="OE項目..."/>
                                <datalist id="select_OEList"></datalist>
                                <button id="btn_Clean_OE" type="button" class="btn btn-danger">重選</button>
                            </td>
                            <td>
                                <input id="txt_OEPrice" type="number" placeholder="$..."/>
                            </td>
                            <td>
                                <button id="btn_Add_OEItem" type="button" class="btn btn-primary"><span>+ 新增</span></button>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="3">
                                <input id="txt_count_OEItem" type="number" value="0"/>
                                <ol id="ol_OEList" name="OEItem"></ol>
                                <hr />
                                總計：<input id="txt_Total_OEItem" type="number" value="0" name="caseData"/>元
                            </td>
                        </tr>
                        <tr>
                            <td class="tthclass" colspan="3"></td>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr>
                <td class="thclass">備註</td>
                <td colspan="3">
                    <textarea id="txt_projectRemark" name="caseDatatxtArea" placeholder="備註..."></textarea>
                </td>
            </tr>
            <tr>
                <td class="thclass">專案內容</td>
                <td colspan="3">
                    <textarea id="txt_projectContext" name="caseDatatxtArea" placeholder="專案內容..."></textarea>
                </td>
            </tr>
            <tr>
                <td colspan="4" style="text-align:center">
                    <button id="btn_save" type="button" class="btn btn-success">儲存</button>
                    <button id="btn_back" type="button" class="btn btn-warning">返回</button>
                </td>
            </tr>
        </table>
    </div>
    <style>
        .titleclass{
            background-color:coral;
        }
        .thclass {
            background-color:antiquewhite;
        }
        .tthclass{
            background-color:lavender;
        }
        #txt_projectRemark,#txt_projectContext{
            width:100%;
        }
        #txt_projectContext{
            height: 500px;
        }
    </style>
    <script>
        $('#txt_count_ContactItem').hide()
        $('#txt_count_OEItem').hide()
        getParterList();
        getPersonnelList();
        getOEList();

        $('#btn_Add_Contact').click(addContact);
        $('#btn_Add_OEItem').click(addOE);
        $('#btn_save').click(saveCaseData);
        $('#btn_Clean_OE').click(Clean_OE);
        $('#btn_Clean_Clinet_Name').click(Clean_Clinet_Name);
        $('#btn_Clean_Personnel').click(Clean_Personnel);
        
        function getParterList() {
            let select_Clinet_Name = $('#select_Clinet_Name');
            $.ajax({
                url: "0010010004.aspx/getParterList",
                type: 'POST',
                contentType: 'application/json; charset=UTF-8',
                dataType: "json",
                success: function (data) {
                    let jsonParse = JSON.parse(data.d);
                    select_Clinet_Name.html();
                    $.each(jsonParse, function (index,value) {
                        select_Clinet_Name.append(`<option value="${value.BUSINESSNAME}"></option>`);
                    })
                }
            })
        }

        function getPersonnelList() {
            let select_Personnel = $('#select_Personnel')
            $.ajax({
                url: "0010010004.aspx/getAgentNameList",
                type: 'POST',
                contentType: 'application/json; charset=UTF-8',
                dataType: "json",
                success: function (data) {
                    let jsonParse = JSON.parse(data.d);
                    select_Personnel.html();
                    $.each(jsonParse, function (index, value) {
                        select_Personnel.append(`<option value="${value.Agent_Name}"></option>`);
                    })
                }
            })
        }

        function getOEList() {
            let txt_OEItem = $('#txt_OEItem')
            let select_OEList = $('#select_OEList')
            $.ajax({
                url: "0010010004.aspx/getOEList",
                type: 'POST',
                contentType: 'application/json; charset=UTF-8',
                dataType: "json",
                success: function (data) {
                    let jsonParse = JSON.parse(data.d);
                    select_OEList.html();
                    $.each(jsonParse, function (index, value) {
                        select_OEList.append(`<option value="${value.Product_Name}"></option>`);
                    })
                    txt_OEItem.on('change', function () {
                        var OEItem = $(this).val();
                        getOEItemPrice(jsonParse,OEItem);
                    })
                }
            })
        }

        function getOEItemPrice(jsonParse,OEItem) {
            console.log(jsonParse, OEItem)
            $.each(jsonParse, function (index, value) {
                if (jsonParse[index].Product_Name == OEItem) {
                    $('#txt_OEPrice').val(jsonParse[index].Unit_Price.trim());
                }
            })
        }

        function addContact() {
            let ul_ContactList = $('#ul_ContactList');
            let txt_ContactPerson = $('#txt_ContactPerson').val();
            let txt_ContactPhoneNumber = $('#txt_ContactPhoneNumber').val();
            let txt_ContactEmail = $('#txt_ContactEmail').val();
            let txt_count_ContactItem = $('#txt_count_ContactItem').val();
            ul_ContactList.append(`<li id="liContactItem${txt_count_ContactItem}">
                <a>${txt_ContactPerson}／${txt_ContactPhoneNumber}／${txt_ContactEmail}</a>
            </li>`);
            $("#liContactItem" + txt_count_ContactItem)
                .click({ liID: `#liContactItem${txt_count_ContactItem}` }, removeContact)
            $('#txt_count_ContactItem').val(parseInt(txt_count_ContactItem) + 1)
            $('#txt_ContactPerson').val('')
            $('#txt_ContactPhoneNumber').val('')
            $('#txt_ContactEmail').val('')
        }

        function addOE() {
            let ol_OEList = $('#ol_OEList');
            let txt_OEItem = $('#txt_OEItem').val();
            let txt_OEPrice = $('#txt_OEPrice').val();
            let txt_Total_OEItem = $('#txt_Total_OEItem').val();
            let txt_count_OEItem = $('#txt_count_OEItem').val();
            ol_OEList.append(
                `<li id="liOE${txt_count_OEItem}">
                    <a>${txt_OEItem}／${txt_OEPrice}</a>
                </li>`);
            $("#liOE" + txt_count_OEItem).click({ liID: `#liOE${txt_count_OEItem}`, price: txt_OEPrice }, removeOEItem);
            $('#txt_Total_OEItem').val(parseInt(txt_Total_OEItem) + parseInt(txt_OEPrice));
            $('#txt_count_OEItem').val(parseInt(txt_count_OEItem) + 1);
            $('#txt_OEItem').val('');
            $('#txt_OEPrice').val('');
        }

        function removeContact(event) {
            let liID = event.data.liID;
            $(liID).remove();
        }

        function removeOEItem(event) {
            let liID = event.data.liID;
            let price = event.data.price;
            let txt_Total_OEItem = $('#txt_Total_OEItem').val();
            $('#txt_Total_OEItem').val(parseInt(txt_Total_OEItem) - parseInt(price));
            $(liID).remove();
        }

        function saveCaseData() {
            let caseData = $("input[name*='caseData']");
            let ul_ContactList_options = $("#ul_ContactList li a");
            let ul_ContactList_saveString = "";
            let ol_OEList_options = $("#ol_OEList li a");
            let ol_OEList_saveString = "";
            let caseDatatxtArea = $("[name*='caseDatatxtArea']");
            let saveCaseDataList = {};

            $.each(caseData, function (index, value) {
                saveCaseDataList[value.id] = value.value
            })

            $.each(ul_ContactList_options, function (index, value) {
                ul_ContactList_saveString += value.innerText+';'
            })
            saveCaseDataList["ul_ContactList"] = ul_ContactList_saveString

            $.each(ol_OEList_options, function (index, value) {
                ol_OEList_saveString += value.innerText + ';'
            })
            saveCaseDataList["ol_OEList"] = ol_OEList_saveString

            $.each(caseDatatxtArea, function (index, value) {
                saveCaseDataList[value.id] = value.value
            })

            console.log(saveCaseDataList);

            $.ajax({
                url: "0010010004.aspx/saveCaseData",
                type: 'POST',
                data: JSON.stringify({
                    saveCaseDataList: saveCaseDataList
                }),
                contentType: 'application/json; charset=UTF-8',
                dataType: "json",
                success: function (data) {
                    
                }
            })
        }

        function Clean_OE() {
            $('#txt_OEItem').val('')
            $('#txt_OEPrice').val('')
        }

        function Clean_Clinet_Name() {
            $('#txt_Clinet_Name').val('')
        }

        function Clean_Personnel() {
            $('#txt_Personnel').val('')
        }
    </script>
</asp:Content>


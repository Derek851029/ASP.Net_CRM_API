<%--<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="0010010006.aspx.cs" Inherits="_0010010006" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="head2" Runat="Server">
	<link href="js/Notiflix-2.1.4/src/notiflix.css" rel="stylesheet" />

	<script src="js/Notiflix-2.1.4/src/notiflix.js"></script>

    

    <div class="container" style="margin-bottom:100px;">
        <input type="hidden" id="SCID" data-addtype="s" />
        <table style="width:100%;">
            <tr>
                <th><h3>夏令營名稱:</h3></th>
            </tr>
            <tr>
                <th>
                    <input type="text" id="CampName" style="width:100%;" data-addtype="s" />
                </th>
            </tr>
            <tr>
                <th><h3>夏令營封面</h3></th>
            </tr>
            <tr>
                <th>
                    <input type="file" id="upCampCoverImg" onchange="UploadFile('CampCoverImg');" />
                    <input type="hidden" id="CampCoverImg" data-addtype="s"/>
                    <div>
                        <img id="vCampCoverImg" src="#" style="width:100%;"/>
                    </div>
                </th>
            </tr>
            <tr>
                <th><h3>夏令營大綱:</h3></th>
            </tr>
            <tr>
                <th>
                    <textarea id="CampOutline"></textarea>
                </th>
            </tr>
            <tr>
                <th><h3>夏令營內容:</h3></th>
            </tr>
            <tr>
                <th>
                    <table style="margin:0px;width:100%;">
                        <tr>
                            <th style="width:50%;">
                                <input type="file" id="upCampContentImg" onchange="UploadFile('CampContentImg');" />
                                <input type="hidden" id="CampContentImg" data-addtype="s"/>
                                <div>
                                    <img id="vCampContentImg" src="#" style="width:100%;"/>
                                </div>
                            </th>
                            <th>
                                <textarea id="CampContent"></textarea>
                            </th>
                        </tr>
                    </table>
                </th>
            </tr>
            <tr>
                <th><h3>老師介紹:</h3></th>
            </tr>
            <tr>
                <th>
                    <table style="margin:0px;width:100%;">
                        <tr>
                            <th style="width:50%;">
                                <h4>試教影片:</h4>
                                <input type="file" id="upTeachVideo" <%--onchange="UploadVideo('TeachVideo');"--%> />
                                <input type="hidden" id="TeachVideo" data-addtype="s"/>
                                <div id="vTeachVideo">
                                    <video style="width:100%;">
                                        <source src="#" />
                                    </video>
                                </div>
                            </th>
                            <th>
                                <h4>老師照片:</h4>
                                <input type="file" id="upTeacherImg" onchange="UploadFile('TeacherImg');" />
                                <input type="hidden" id="TeacherImg" data-addtype="s"/>
                                <div>
                                    <img id="vTeacherImg" src="#" style="width:100%;"/>
                                </div>
                                <br />
                                <h4>老師姓名:</h4>
                                <input type="text" id="TeacherName" style="width:100%;" data-addtype="s" />
                                <br />
                                <br />
                                <textarea id="TeacherIntro"></textarea>
                            </th>
                        </tr>
                    </table>
                </th>
            </tr>
            <tr>
                <th style="text-align:center;">
                    <a href="0010010005.aspx" class="btn btn-info btn-lg" style="margin-top:10px;">完成</a>
                </th>
            </tr>
        </table>

    </div>

    
        
    <div class="modal fade" id="LoadModal" role="dialog" data-backdrop="static" data-keyboard="false">
        <div class="modal-dialog" style="width: 600px;">
            <div class="modal-content">
                <div class="modal-header">
                    <button id="closemodal" type="button" class="close" data-dismiss="modal">&times;</button>
                    <h2 class="modal-title"><strong>
                        <label id="">編輯章節</label>
                    </strong></h2>
                </div>
                <div class="modal-body">
                    <div class="spinner-border" role="status">
                      <span class="sr-only">Loading...</span>
                    </div>
                </div>
            </div>
        </div>
    </div>

    <button id="showmodal" type="button" data-toggle="modal" data-target="#LoadModal" style="display:none;"></button>
    
    <script src="ckeditor/ckeditor.js"></script>
    <script>
        $(function () {
            initCKEdit();
            bindata();

            $('[data-addtype="s"]').change(function () { Edit(); });
            CKEDITOR.instances.CampOutline.on('change', function () { Edit(); });
            CKEDITOR.instances.CampContent.on('change', function () { Edit(); });
            CKEDITOR.instances.TeacherIntro.on('change', function () { Edit(); });

            //$('#showmodal').click();
            
            $('#upTeachVideo').change(function () {
                Notiflix.Loading.Standard('UpLoading...');
                setTimeout(function () { UploadVideo('TeachVideo'); }, 500);
                
            });
        });


        function initCKEdit() {
            CKEDITOR.config.height = 200;
            CKEDITOR.config.image_previewText = ' ';
            CKEDITOR.config.removeDialogTabs = 'image:advanced;image:Link';
            CKEDITOR.config.filebrowserImageUploadUrl = 'UploadCKEditor.ashx';
            CKEDITOR.config.filebrowserUploadMethod = 'form';
            CKEDITOR.config.toolbarGroups = [
                { name: 'document', groups: ['mode', 'document', 'doctools'] },
                { name: 'clipboard', groups: ['clipboard', 'undo'] },
                { name: 'editing', groups: ['find', 'selection', 'spellchecker', 'editing'] },
                { name: 'forms', groups: ['forms'] },
                { name: 'basicstyles', groups: ['basicstyles', 'cleanup'] },
                { name: 'paragraph', groups: ['list', 'indent', 'blocks', 'align', 'bidi', 'paragraph'] },
                { name: 'links', groups: ['links'] },
                '/',
                { name: 'styles', groups: ['styles'] },
                { name: 'colors', groups: ['colors'] },
                { name: 'tools', groups: ['tools'] },
                { name: 'others', groups: ['others'] },
                { name: 'about', groups: ['about'] }
            ];
            CKEDITOR.config.removeButtons = 'Form,Checkbox,Radio,TextField,Textarea,Select,Button,ImageButton,HiddenField,Source,Templates,Replace,Scayt,CopyFormatting,CreateDiv,Language,Anchor,Flash,Table,HorizontalRule,PageBreak,Iframe,Maximize,ShowBlocks,About,Save,NewPage,ExportPdf,Preview,Print,Paste,PasteText,PasteFromWord';

            CKEDITOR.replace('CampOutline');
            CKEDITOR.replace('CampContent', { height:'800px' });
            CKEDITOR.replace('TeacherIntro');
        }


        function UploadFile(id) {
            if ($('#up' + id).val() == '') {
                return;
            }
            var form = new FormData();
            form.append('type', 'summercamp');
            form.append('file', $('#up' + id)[0].files[0]);

            console.log('start upload');
            $.ajax({
                type: 'post',
                url: 'UploadFile.ashx',
                data: form,
                contentType: false,
                processData: false,
                dataType: 'json',
                success: function (doc) {
                    alert(doc.Status);
                    if (doc.Status != '上傳成功') {
                        return;
                    }
                    $('#' + id).val(doc.FilePath);
                    console.log(doc.FilePath);
                    $('#v' + id).attr('src', doc.FilePath);
                    $('#v' + id).attr('data-path', doc.FilePath);
                    Edit();
                }

            });

        }

        function UploadVideo(id) {
            if ($('#up' + id).val() == '') {
                Notiflix.Loading.Remove(300);
                return;
            }
            let file = $('#up' + id)[0].files[0];
            let size = file.size;
            let name = file.name;
            let extindex = name.lastIndexOf('.');
            let ext = name.substr(extindex + 1, name.length);

            let cutsize = 2 * 1024 * 1024;
            let cutcount = Math.ceil(size / cutsize);

            console.log('start upload');
            let data = { Status : '', FilePath: ''};
            for (let i = 0; i < cutcount; i++) {
                let start = i * cutsize;
                let end = Math.min(size, start + cutsize);

                let form = new FormData();
                form.append('type', 'video');
                form.append('name', name);
                form.append('ext', ext);
                form.append('index', i + 1);
                form.append('total', cutcount);
                form.append('file', file.slice(start, end));

                Notiflix.Loading.Standard('UpLoading...' + Math.ceil((i / cutcount) * 100) + '%' );

                $.ajax({
                    type: 'post',
                    url: 'UploadFile.ashx',
                    data: form,
                    async: false,
                    contentType: false,
                    processData: false,
                    dataType: 'json',
                    success: function (doc) {
                        //alert(doc.Status);
                        data = doc;
                    },
                    error: function () {
                        data.Status ='上傳失敗';
                    }
                });

                if (data.Status != '上傳成功') {
                    break;
                }
            }

            alert(data.Status);
            if (data.Status == '上傳成功') {
                $('#' + id).val(data.FilePath);
                console.log(data.FilePath);
                $('#v' + id).html('<video style="width:100%;" controls ><source src="' + data.FilePath + '" /></video>');
                Edit();
            }

            Notiflix.Loading.Remove(300);
        }

        function bindata() {
            let UrlStr = location.href;
            let url = new URL(UrlStr);
            let id = url.searchParams.get('id');
            if (id == null) {
                return;
            }
            $.ajax({
                type: 'post',
                url: '0010010006.aspx/bindata',
                contentType: 'application/json;utf-8',
                data: JSON.stringify({ ID: id }),
                dataType: 'json',
                success: function (doc) {
                    let data = JSON.parse(doc.d);
                    $('[data-addtype="s"]').each(function () {
                        $(this).val(eval('data.' + $(this).attr('id')));
                    });
                    CKEDITOR.instances.CampOutline.setData(data.CampOutline);
                    CKEDITOR.instances.CampContent.setData(data.CampContent);
                    CKEDITOR.instances.TeacherIntro.setData(data.TeacherIntro);
                    $('#vCampCoverImg').attr('src', data.CampCoverImg);
                    $('#vCampContentImg').attr('src', data.CampContentImg);
                    $('#vTeacherImg').attr('src', data.TeacherImg);
                    $('#vTeachVideo').html('<video style="width:100%;" controls ><source src="' + data.TeachVideo + '" /></video>');
                }
            })
        }

        function Edit() {
            let data = {};
            $('[data-addtype="s"]').each(function () {
                data[$(this).attr('id')] = $(this).val();
            });
            data['CampOutline'] = CKEDITOR.instances.CampOutline.getData();
            data['CampContent'] = CKEDITOR.instances.CampContent.getData();
            data['TeacherIntro'] = CKEDITOR.instances.TeacherIntro.getData();
            console.log(data);
            $.ajax({
                type: 'post',
                url: '0010010006.aspx/Edit',
                contentType: 'application/json;utf-8',
                data: JSON.stringify({ data: data }),
                dataType: 'json',
                success: function (doc) {
                    console.log(doc.d);
                }
            })
        }

    </script>
</asp:Content>
--%>

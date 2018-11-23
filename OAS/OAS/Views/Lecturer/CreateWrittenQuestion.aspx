<%@ Page Title="OAS | Create Question" Language="C#" MasterPageFile="~/MasterPage/MainSite.Master" ValidateRequest="false" AutoEventWireup="true" CodeBehind="CreateWrittenQuestion.aspx.cs" Inherits="OAS.Views.Lecturer.CreateWrittenQuestion" %>

<asp:Content ID="head" ContentPlaceHolderID="head" runat="server">

    <!-- Overide the #contentBody height value default is 1.8 -->
    <script>
        $(document).ready(function () {
            dynamicSetHeight(0.75);
            setLeftTriangle();
        });

        $(window).on('resize', function () {
            var instance = $.fn.deviceDetector;
            if (instance.isDesktop()) {
                dynamicSetHeight(0.75);
            }
            if ($(window).outerWidth() > 958) {
                setLeftTriangle();
            } else {
                document.getElementById("triangle-div").style.left = "";
            }
        });
    </script>

    <style>
        video {
            opacity: 0.6;
        }

        .background-image {
            filter: brightnes(250%);
            background-size: cover;
            background-position: center;
        }

        .box-wrapper {
            background-color: #fff;
            position: absolute;
            top: 50%;
            right: 0;
            bottom: 0;
            left: 50%;
            width: 85%;
            height: 80%;
            transform: translate(-50%, -50%);
            user-select: none;
        }

        .box {
            background-color: #fff;
            width: 70%;
            height: 100%;
            position: absolute;
            top: 50%;
            right: 0;
            bottom: 0;
            left: 50%;
            transform: translate(-50%, -50%);
            text-align: left;
        }

        input[type=submit], button {
            border-color: #0067b8;
            background-color: #0067b8;
            color: #fff;
            padding: 4px 12px 4px 12px;
            height: 32px;
            border-width: 1px;
            border-style: solid;
            cursor: pointer;
            text-overflow: ellipsis;
            touch-action: manipulation;
            -webkit-appearance: button;
            float: right;
        }

        .ck-link_selected {
            all: unset !important;
            color: blue !important;
            text-decoration: underline !important;
        }

            .ck-link_selected:hover {
                all: unset !important;
            }
    </style>
</asp:Content>

<asp:Content ID="videoSource" ContentPlaceHolderID="videoSource" runat="server">
    <source runat="server" type="video/mp4" src="~/Content/videos/bg_video2.mp4">
    <script>
        var path = window.location.protocol + "//" + window.location.host;
        document.getElementsByClassName("background-image")[0].style.backgroundImage = "url('" + path + "/Content/images/background_images/login_signup_bg.jpg')";
    </script>
</asp:Content>

<asp:Content ID="backgoundText" ContentPlaceHolderID="backgoundText" runat="server">
    <!-- background text -->
    <div style="position: fixed; top: 14%; width: 100%; color: #fff">
        <div id="sloganDiv" style="text-align: center;">
            <div style="margin: 20px;">
                <span id="slogan">Create Written Question</span>
            </div>
        </div>
    </div>
</asp:Content>

<asp:Content ID="contentBody" ContentPlaceHolderID="contentBody" runat="server">
    <!-- content start -->
    <div id="bodyContent" style="background-color: #fff; border-bottom: 1px solid rgb(205,205,205); z-index: 1">
        <div class="content">


            <div class="box-wrapper">
                <div class="box" style="width: 100%; top: 40%; height: 85%">


                    <div style="float: right">
                        <label>
                            Question Level : 
                        <asp:DropDownList ID="LevelDropDownList" runat="server">
                            <asp:ListItem>Easy</asp:ListItem>
                            <asp:ListItem>Medium</asp:ListItem>
                            <asp:ListItem>Hard</asp:ListItem>
                            <asp:ListItem>Difficult</asp:ListItem>
                            <asp:ListItem>Insane</asp:ListItem>
                        </asp:DropDownList>
                        </label>
                    </div>

                    <table style="width: 100%; height: 100%; table-layout: fixed">
                        <tr style="height: 100%">
                            <td style="width: 20%;">
                                <h4 style="font-family: 'Segoe UI Emoji';">Question Image</h4>
                                <div style="border: 1px solid rgba(0,0,0,0.2); text-align: center; width: 100%; height: 100%; margin: auto; background-color: ">
                                    <%--img id="userAvartar" runat="server" style="width: 100%; height: 100%; background-color: #d7d7d7; background-size: cover; background-position: center;"/--%>
                                    <div id="imageUploadId" runat="server" style="width: 100%; height: 100%; background-color: #d7d7d7; background-size: cover; background-position: center; background-image: url('../../Content/images/gif/gif1.gif')"></div>
                                </div>
                            </td>
                            <td>
                                <h4 style="font-family: 'Segoe UI Emoji'">Insert your Question</h4>

                                <div id="scrollEditor" style="border: 1px solid rgba(0,0,0,0.2); width: 100%; height: 100%; overflow: scroll" onscroll="scrollEditorFunc()">
                                    <asp:FileUpload ID="ImageUpload" runat="server" Style="display: none;" />
                                    <label id="changePhoto" title="Insert Image" runat="server" for="contentBody_ImageUpload" style="background-color: rgba(0,0,0,0.2); cursor: pointer; position: absolute; transform: translate(965%,10%); z-index: 1; color: #fff; width: 33px; height: 33px;">
                                    </label>

                                    <asp:TextBox ID="editor" runat="server" TextMode="MultiLine">
                                        <%--figure class="image image-style-side">
                                            <img src="" alt="" >
                                        </figure>
                                        <figcaption>Enter Your Question Here...</figcaption>
                                         --%>
                                    </asp:TextBox>

                                </div>

                            </td>
                        </tr>
                        <tr>
                            <td></td>
                            <td>
                                <br /><br /><br /><br />
                                <asp:Label Style="float: left;" ID="MessageLabel" runat="server" Text=""></asp:Label>
                                <asp:Button ID="SubmitButton" runat="server" OnClick="SubmitButton_OnClick" Text="Create Question" />
                            </td>
                        </tr>
                    </table>

                </div>
            </div>

        </div>
    </div>
    <!-- content end -->
    <script>
        ClassicEditor
            .create(document.querySelector('#contentBody_editor'), {
                image: {
                    toolbar: ['imageStyle:alignRight'], styles: [

                        // This represents an image aligned to the right.
                        'alignRight'
                    ]
                }, ckfinder: {
                    uploadUrl: '/'
                }
            },

            )
            .then(editor => {
                console.log(editor);
            })
            .catch(error => {
                console.error(error);
            });

        function scrollEditorFunc() {
            if (document.getElementById('scrollEditor').scrollTop > 15) {
                document.getElementById("contentBody_changePhoto").style.display = "none";
            } else {
                document.getElementById("contentBody_changePhoto").style.display = "block";
            }
        }

        $(function () {
            $("#contentBody_ImageUpload").on("change", function () {
                var files = !!this.files ? this.files : [];
                if (!files.length || !window.FileReader) return; // no file selected, or no FileReader support

                if (/^image/.test(files[0].type)) { // only image file
                    var reader = new FileReader(); // instance of the FileReader
                    reader.readAsDataURL(files[0]); // read the local file

                    reader.onloadend = function () {
                        $("#contentBody_imageUploadId").css("background-image", "url(" + this.result + ")");
                    }
                }
            });
        });
    </script>
</asp:Content>

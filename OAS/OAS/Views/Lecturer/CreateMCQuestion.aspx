<%@ Page Title="OAS | Create Question" Language="C#" MasterPageFile="~/MasterPage/MainSite.Master" ValidateRequest="false" AutoEventWireup="true" CodeBehind="CreateMCQuestion.aspx.cs" Inherits="OAS.Views.Lecturer.CreateMCQuestion" %>

<asp:Content ID="head" ContentPlaceHolderID="head" runat="server">

    <!-- Overide the #contentBody height value default is 1.8 -->
    <script>
        $(document).ready(function () {
            dynamicSetHeight(3);
            setLeftTriangle();
        });

        $(window).on('resize', function () {
            var instance = $.fn.deviceDetector;
            if (instance.isDesktop()) {
                dynamicSetHeight(3);
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
            height: 90%;
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

        .box table td {
            padding-bottom: 80px !important;
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
                <span id="slogan">Create MCQ Question</span>
            </div>
        </div>
    </div>
</asp:Content>

<asp:Content ID="contentBody" ContentPlaceHolderID="contentBody" runat="server">
    <!-- content start -->
    <div id="bodyContent" style="background-color: #fff; border-bottom: 1px solid rgb(205,205,205); z-index: 1">
        <div class="content">


            <div class="box-wrapper">
                <div class="box" style="width: 100%; top: 50%; height: 100%">

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
                    <div>
                        Status : <asp:Label ID="MessageLabel" ForeColor="LightGreen" runat="server" Text=""></asp:Label>
                    </div>

                    <asp:PlaceHolder ID="MCQTablePlaceholder" runat="server"></asp:PlaceHolder>
                    <asp:Button ID="SubmitButton" runat="server" OnClick="SubmitButton_OnClick" Text="Create Question" />

                </div>
            </div>

        </div>
    </div>
    <!-- content end -->
    <script>
        <% 
        for (int i = 0; i < 5; i++)
        {
        %>

        ClassicEditor
            .create(document.querySelector('#contentBody_editor<%= i %>'), {
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


        $(function () {
            $("#contentBody_ImageFileUpload<%= i %>").on("change", function () {
                var files = !!this.files ? this.files : [];
                if (!files.length || !window.FileReader) return; // no file selected, or no FileReader support

                if (/^image/.test(files[0].type)) { // only image file
                    var reader = new FileReader(); // instance of the FileReader
                    reader.readAsDataURL(files[0]); // read the local file

                    reader.onloadend = function () {
                        $("#contentBody_imageUploadId<%= i %>").css("background-image", "url(" + this.result + ")");
                    }
                }
            });
        });
        <%
        }
        %>

        function scrollEditorFunc() {
            <% for (int i = 0; i < 5; i++)
        {%>
            if (document.getElementById('contentBody_scrollEditor<%= i %>').scrollTop > 15) {
                document.getElementById("contentBody_showUpload<%= i %>").style.display = "none";
            }
            else {
                document.getElementById("contentBody_showUpload<%= i %>").style.display = "block";
            }
            <%}%>
        }
    </script>
</asp:Content>

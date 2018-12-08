<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage/MainSite.Master" AutoEventWireup="true" CodeBehind="ReviewMCQAnswer.aspx.cs" Inherits="OAS.Views.Student.ReviewMCQAnswer" %>
<asp:Content ID="head" ContentPlaceHolderID="head" runat="server">

    <!-- Overide the #contentBody height value default is 1.8 -->
    <script>
        $(document).ready(function () {
            dynamicSetHeight(1);
            setLeftTriangle();
        });

        $(window).on('resize', function () {
            var instance = $.fn.deviceDetector;
            if (instance.isDesktop()) {
                dynamicSetHeight(1);
            }
            if ($(window).outerWidth() > 958) {
                setLeftTriangle();
            } else {
                document.getElementById("triangle-div").style.left = "";
            }
        });

        $(document).ready(function () {
            $('#contentBody_datatables').DataTable({
                pageLength: 1,
                paging: false,
                //stateSave: true,
                "scrollY": "100%",
                "scrollCollapse": true,
                "searching": false,
                "ordering": false,
                "lengthChange": false,
                "columnDefs": [{ "width": "10%", "targets": 0 }],
                "dom": '<"top"i>rt<"bottom"flp><"clear">',
                "oLanguage": {
                    "sInfo": "Showing Question _START_ of total _TOTAL_ Question."
                },
                fixedColumns: { heightMatch: 'none' }

            });
        });

        var table = $('#contentBody_datatables').DataTable();

        // Sort by column 1 and then re-draw
        table
            .draw(false);

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

        #contentBody_datatables_next {
            all: initial;
        }

        .dataTables_info {
            text-align: left;
        }

        .dt-body-center {
            text-align: center !important;
        }

        .box a:hover {
            background-color: rgba(0,0,0,0.1) !important;
        }

        .box td {
            padding: 0 !important;
        }

        p {
            word-break: break-all;
            text-align: justify;
        }

        .optionDiv {
            width: 100%;
            display: inline-block;
        }

            .optionDiv p {
                display: inline-block !important;
                text-align: justify;
            }

        #contentBody_datatables a: {
            background-color: aliceblue !important;
        }

        .optionDiv:visited {
            background-color: aliceblue !important;
        }

        .RadioButtonClass {
            width: 100%;
        }

            .RadioButtonClass td {
                padding: 0 !important;
                background-color: #fff;
                border: none !important;
            }

            .RadioButtonClass input {
                display: none;
            }

            .RadioButtonClass label {
                width: 100%;
                line-height: 2.5;
                margin: 0;
            }

            .RadioButtonClass p {
                margin: 0;
                word-break: break-word;
            }

        input[type=radio]:checked + label {
            background-color: rgba(185,246,202,0.35);
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
            display: inline-block;
            -webkit-appearance: button;
            float: right;
        }

        #contentBody_tableDiv {
            border: 1px solid rgba(0,0,0,0.2);
        }

        .dataTables_info {
            padding: 10px;
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
                <span id="slogan">Review MCQ Question & Answer</span>
            </div>
        </div>
    </div>
</asp:Content>

<asp:Content ID="contentBody" ContentPlaceHolderID="contentBody" runat="server">
    <!-- content start -->
    <div id="bodyContent" style="background-color: #fff; border-bottom: 1px solid rgb(205,205,205); z-index: 1">
        <div class="content">

            <div class="box-wrapper">
                <div class="box" style="width: 100%; top: 50%; height: 80%; background-color: ;">
                    <div style="width: 100%">
                        <asp:Label ID="Message" runat="server" Style="text-align: center; display: block; font-weight: 500; font-size: 14px; letter-spacing: 0.5px;"></asp:Label>
                    </div>
                    <div id="tableDiv" runat="server" style="height: 100%; overflow: scroll;" class="reset-parent bootstrap-iso">
                        <!-- Any HTML here will be styled with Bootstrap CSS -->
                        <asp:PlaceHolder ID="AnswerTablePlaceHolder" runat="server"></asp:PlaceHolder>
                    </div>
                </div>
            </div>

        </div>
    </div>
    <!-- content end -->
</asp:Content>

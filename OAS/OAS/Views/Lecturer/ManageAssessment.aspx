<%@ Page Title="OAS | Manage Assessment" Language="C#" MasterPageFile="~/MasterPage/MainSite.Master" AutoEventWireup="true" CodeBehind="ManageAssessment.aspx.cs" Inherits="OAS.Views.Lecturer.ManageAssessment" %>

<asp:Content ID="head" ContentPlaceHolderID="head" runat="server">

    <!-- Overide the #contentBody height value default is 1.8 -->
    <script>
        $(document).ready(function () {
            dynamicSetHeight(1.3);
            setLeftTriangle();
        });

        $(window).on('resize', function () {
            var instance = $.fn.deviceDetector;
            if (instance.isDesktop()) {
                dynamicSetHeight(1.3);
            }
            if ($(window).outerWidth() > 958) {
                setLeftTriangle();
            } else {
                document.getElementById("triangle-div").style.left = "";
            }
        });

        $(document).ready(function () {
            $('#contentBody_datatables').DataTable({
                pageLength: 5,
                paging: true,
                "lengthChange": false,
                "columnDefs": [{ "width": "10%", "targets": 0 }],
                "columnDefs": [{ "orderable": false, className: "dt-body-center", "targets": 7 }],
                fixedColumns: { heightMatch: 'none' }

            });
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

        #contentBody_datatables_next {
            all: initial;
        }

        .dataTables_info {
            text-align: left;
        }

        .dt-body-center {
            text-align: center !important;
        }

        .material-icons {
            font-size: 24px;
        }

        .actionButton {
            all: initial !important;
            cursor: pointer !important;
            color: red !important;
        }

            .actionButton + a {
                color: #007bff !important;
            }

                .actionButton + a + a {
                    color: #5d4037 !important;
                }

        @keyframes hvr-icon-pulse {
            25% {
                -webkit-transform: scale(1.3);
                transform: scale(1.3);
            }

            75% {
                -webkit-transform: scale(0.8);
                transform: scale(0.8);
            }
        }

        .hvr-icon-pulse {
            display: inline-block;
            vertical-align: middle;
            -webkit-transform: perspective(1px) translateZ(0);
            transform: perspective(1px) translateZ(0);
            box-shadow: 0 0 1px rgba(0, 0, 0, 0);
        }

            .hvr-icon-pulse .hvr-icon {
                -webkit-transform: translateZ(0);
                transform: translateZ(0);
                -webkit-transition-timing-function: ease-out;
                transition-timing-function: ease-out;
            }

            .hvr-icon-pulse:hover .hvr-icon, .hvr-icon-pulse:focus .hvr-icon, .hvr-icon-pulse:active .hvr-icon {
                -webkit-animation-name: hvr-icon-pulse;
                animation-name: hvr-icon-pulse;
                -webkit-animation-duration: 1s;
                animation-duration: 1s;
                -webkit-animation-timing-function: linear;
                animation-timing-function: linear;
                -webkit-animation-iteration-count: infinite;
                animation-iteration-count: infinite;
            }

        #spin {
            font-size: 20px;
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
                <span id="slogan">Manage Assessment</span>
            </div>
        </div>
    </div>
</asp:Content>

<asp:Content ID="contentBody" ContentPlaceHolderID="contentBody" runat="server">
    <!-- content start -->
    <div id="bodyContent" style="background-color: #fff; border-bottom: 1px solid rgb(205,205,205); z-index: 1">
        <div class="content">

            <div class="box-wrapper">
                <div class="box" style="width: 100%; top: 50%; height: 90%; background-color: ;">
                    <div style="width: 100%;">
                        Status : <asp:Label ID="Message" runat="server" Style="text-align: left; color: green; font-weight: 500; font-size: 14px; letter-spacing: 0.5px;"></asp:Label>
                    </div>
                    <div id="tableDiv" runat="server" style="height: 100%; overflow: auto" class="reset-parent bootstrap-iso">
                        <!-- Any HTML here will be styled with Bootstrap CSS -->
                        <asp:PlaceHolder ID="AssessmentTablePlaceHolder" runat="server"></asp:PlaceHolder>
                    </div>
                </div>
            </div>

        </div>
    </div>
    <!-- content end -->

</asp:Content>

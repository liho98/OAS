<%@ Page Title="OAS | View Student's Assessment" Language="C#" MasterPageFile="~/MasterPage/MainSite.Master" AutoEventWireup="true" CodeBehind="MarkingOrViewScore.aspx.cs" Inherits="OAS.Views.Lecturer.MarkingOrViewScore" %>

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

        $(document).ready(function () {
            $('#contentBody_datatables').DataTable({
                pageLength: 5,
                paging: true,
                "lengthChange": false,
                "columnDefs": [{ className: "dt-body-center", "targets": [3, 4] }, { "orderable": false, className: "dt-body-center", "targets": [5] }],
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
            padding-bottom: 0 !important;
        }
        .material-icons{
            font-size: 24px;
        }
        #spin{
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
                <span id="slogan">View Student's Assessment</span>
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
                    <div id="tableDiv" runat="server" style="height: 100%" class="reset-parent bootstrap-iso">
                        <!-- Any HTML here will be styled with Bootstrap CSS -->
                        <asp:PlaceHolder ID="StudentAssessmentTablePlaceHolder" runat="server"></asp:PlaceHolder>
                        <div style="width: 100%">
                            <asp:Label ID="Message" runat="server" Style="text-align: center; display: block; color: green; font-weight: 500; font-size: 14px; letter-spacing: 0.5px;"></asp:Label>
                        </div>
                    </div>
                </div>
            </div>

        </div>
    </div>
    <!-- content end -->

</asp:Content>

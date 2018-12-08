<%@ Page Title="OAS | Create Assessment" Language="C#" EnableViewState="true" ViewStateEncryptionMode="Always" MasterPageFile="~/MasterPage/MainSite.Master" AutoEventWireup="true" CodeBehind="CreateAssessment.aspx.cs" Inherits="OAS.Views.Lecturers.CreateAssessment" %>

<asp:Content ID="head" ContentPlaceHolderID="head" runat="server">

    <!-- Overide the #contentBody height value default is 1.8 -->
    <script>
        $(document).ready(function () {
            dynamicSetHeight(1.4);
            setLeftTriangle();
        });

        $(window).on('resize', function () {
            var instance = $.fn.deviceDetector;
            if (instance.isDesktop()) {
                dynamicSetHeight(1.4);
            }
            if ($(window).outerWidth() > 958) {
                setLeftTriangle();
            } else {
                document.getElementById("triangle-div").style.left = "";
            }
        });

        $(document).ready(function () {
            $('#contentBody_datatables').DataTable({
                "scrollY": "130px",
                pageLength: 3,
                paging: false,
                "lengthChange": false,
                "columnDefs": [{ "width": "10%", "targets": 0 }],
                "columnDefs": [{ "orderable": false, className: "dt-body-center", "targets": 2 }],
                fixedColumns: { heightMatch: 'none' }

            });
        });
        $(document).ready(function () {
            $('#contentBody_datatables2').DataTable({
                "scrollY": "300px",
                pageLength: 5,
                paging: false,
                "lengthChange": false,
                "columnDefs": [{ "width": "10%", "targets": 0 }],
                "columnDefs": [{ "orderable": false, className: "dt-body-center", "targets": 2 }],
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

        .dataTables_scrollBody::-webkit-scrollbar {
            width: 2px;
        }

        .dataTables_scrollBody::-webkit-scrollbar-track {
            -webkit-box-shadow: inset 0 0 5px rgba(0,0,0,0);
            background-color: white;
        }

        .dataTables_scrollBody::-webkit-scrollbar-thumb {
            background-color: rgba(100,100,100,1);
            outline: 1px solid slategrey;
        }

        .dataTables_scrollHeadInner, .dataTable {
            width: 100% !important;
        }

        .dt-body-center {
            text-align: center !important;
        }

        #contentBody_datatables_next, #contentBody_datatables2_next {
            all: initial;
        }

        #contentBody_datatables tr {
            height: 0px;
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
        }

        input, select, textarea {
            border-width: 1px;
            border-color: #666;
            border-color: rgba(0,0,0,.3);
            outline: none;
            border-radius: 0;
            -webkit-border-radius: 0;
            width: 100%;
            border-top-width: 0;
            border-left-width: 0;
            border-right-width: 0;
            font-size: 13px;
            font-weight: 100;
            color: black !important;
            -webkit-transition: all 1s linear;
            transition: all 1s linear;
            font-family: sans-serif;
        }

        textarea {
            border: 1px solid rgba(0,0,0,.3);
        }

            input:focus, textarea:focus {
                border-color: rgb(102,102,255);
            }

        .box tr {
            height: 60px;
            vertical-align: inherit;
            text-align: left;
        }

        input[type=submit], button, input[type=button] {
            width: auto;
            border-color: #0067b8;
            background-color: #0067b8;
            color: #fff !important;
            padding: 4px 12px 4px 12px;
            height: 32px;
            border-width: 1px;
            border-style: solid;
            cursor: pointer;
            text-overflow: ellipsis;
            touch-action: manipulation;
            -webkit-appearance: button;
            float: right;
            transition: none;
        }

        input[type=checkbox] {
            transition: none;
        }

        .col-md-6 {
            transform: translateX(-100%);
        }

        @media screen and (max-width: 767px) {
            .col-md-6 {
                transform: translateX(0%);
            }
        }

        #contentBody_datatables2_wrapper .col-md-6 {
            transform: translateX(0) !important;
        }

        #contentBody_datatables2_info {
            text-align: left;
        }

        #contentBody_datatables2_wrapper {
            width: 80%;
            margin: auto;
        }

        #contentBody_AssessmentType input {
            width: auto;
        }

        #contentBody_AssessmentType {
            clear: both;
        }

            #contentBody_AssessmentType label {
                width: 200px;
                border-radius: 25px;
                border: 1px solid #D1D3D4
            }
        /* hide input */
        input[type=radio]:empty {
            display: none;
        }

            /* style label */
            input[type=radio]:empty ~ label {
                position: relative;
                float: left;
                line-height: 1.5em;
                text-indent: 3.25em;
                cursor: pointer;
                -webkit-user-select: none;
                -moz-user-select: none;
                -ms-user-select: none;
                user-select: none;
            }

                input[type=radio]:empty ~ label:before {
                    position: absolute;
                    display: block;
                    top: -1px;
                    bottom: 0;
                    left: 0;
                    content: '';
                    width: 2.5em;
                    background: #D1D3D4;
                    border-radius: 25px 0 0 25px;
                }

        /* toggle hover */
        input[type=radio]:hover:not(:checked) ~ label:before {
            content: '\2714';
            text-indent: .9em;
            color: #C2C2C2;
        }

        input[type=radio]:hover:not(:checked) ~ label {
            color: #888;
        }

        /* toggle on */
        input[type=radio]:checked ~ label:before {
            content: '\2714';
            text-indent: .9em;
            color: #9CE2AE;
            background-color: #4DCB6D;
        }

        input[type=radio]:checked ~ label {
            color: #777;
        }

        /* radio focus */
        input[type=radio]:focus ~ label:before {
            box-shadow: 0 0 0 1px rgb(102,102,255);
        }

        .popupBox {
            background-color: transparent;
            z-index: 1;
            height: 100%;
            top: 0%;
            left: 0;
            width: 100%;
            transform: translate(0, 0);
            position: fixed;
            background-color: rgba(0,0,0,0.6);
            display: none;
        }

        .popupButton {
            margin-left: 10px !important;
        }
        ul{
            padding: 0;
        }
        li{
            display:none;
        }
        ul li:first-child{
            list-style-type: none;
            display:block;
        }
        #contentBody_ValidationSummary{
            display:inline-block;
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
                <span id="slogan">Create Assessment</span>
            </div>
        </div>
    </div>
</asp:Content>

<asp:Content ID="contentBody" ContentPlaceHolderID="contentBody" runat="server">
    <!-- content start -->
    <div id="bodyContent" style="background-color: #fff; border-bottom: 1px solid rgb(205,205,205); z-index: 1">
        <div class="content">

            <div id="popupBoxId" class="box-wrapper popupBox">
                <div class="box" style="width: 60%; top: 52%; height: 86%; background-color: #fff;">
                    <div id="tableDiv2" runat="server" class="reset-parent bootstrap-iso">
                        <!-- Any HTML here will be styled with Bootstrap CSS -->
                        <div style="width: 80%; margin: auto">
                            <h5 style="margin-bottom: 30px; text-align: left; font-family: cursive; font-size: unset; margin-top: 40px;">Select student(s) who are allowed to answer this assessment.</h5>
                        </div>
                        <asp:PlaceHolder ID="StudentTablePlaceHolder" runat="server"></asp:PlaceHolder>

                        <br />
                        <br />
                        <div style="width: 80%; margin: auto">
                            <asp:Button ID="SelectButton" UseSubmitBehavior="false" class="popupButton" runat="server" OnClick="Select_OnClick" Text="Select" />
                        </div>

                    </div>
                </div>
            </div>


            <div id="assessmentId" class="box-wrapper">
                <div class="box" style="background-color: ">
                    <div style="width:100%;text-align: left;">

                        <asp:RequiredFieldValidator ID="TitleTextBoxRequiredFieldValidator"
                            runat="server"
                            ControlToValidate="TitleTextBox"
                            ErrorMessage="Please fill the Assessment Title field." style="display:none"
                            ValidationGroup="ValidateGroup"
                            Display="Static" ForeColor="Red" CssClass="errorMsg" />
                        <asp:RequiredFieldValidator ID="DescriptionTextAreaRequiredFieldValidator"
                            runat="server"
                            ControlToValidate="DescriptionTextArea"
                            ErrorMessage="Please fill the Description field." style="display:none"
                            ValidationGroup="ValidateGroup"
                            Display="Static" ForeColor="Red" CssClass="errorMsg" />

                        <span>
                            Status : 
                        </span>
                        <asp:Label ID="MessageLabel" runat="server" ForeColor="Green" Text="" Style="font-family: monospace"></asp:Label>
                        <asp:ValidationSummary ID="ValidationSummary" runat="server" Font-Size="Small" ForeColor="Red" ValidationGroup="ValidateGroup" 
                            DisplayMode="BulletList" ShowSummary="true" />                    
                    </div>
                    <hr />
                    <br />
                    <table style="width: 100%; table-layout: fixed;">
                        <tr>
                            <td style="width: 30%">
                                <label>Assessment Title</label>
                            </td>
                            <td style="width: 10%">:
                            </td>
                            <td>
                                <asp:TextBox ID="TitleTextBox" autofocus="autofocus" placeholder="Assessment title" runat="server"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td style="width: 30%;">
                                <label>Assessment Type</label>
                            </td>
                            <td style="width: 10%">:
                            </td>
                            <td>
                                <div class="inputGroup">
                                    <asp:RadioButtonList ID="AccessmentTypeRadioList" AutoPostBack="true" Width="100%" RepeatDirection="Horizontal" runat="server">
                                        <asp:ListItem Selected="True">Written</asp:ListItem>
                                        <asp:ListItem>MCQ&nbsp;&nbsp;&nbsp;&nbsp;</asp:ListItem>
                                    </asp:RadioButtonList>
                                </div>
                            </td>
                        </tr>
                        <tr>
                            <td style="width: 30%;">
                                <label>Assessment Access</label>
                            </td>
                            <td style="width: 10%">:
                            </td>
                            <td>
                                <div class="inputGroup">
                                    <asp:RadioButtonList ID="AccessmentAccessRadioList" AutoPostBack="true" Width="100%" OnSelectedIndexChanged="AssessmentAccess_OnChanged" RepeatDirection="Horizontal" runat="server">
                                        <asp:ListItem Selected="True">Public</asp:ListItem>
                                        <asp:ListItem>Private</asp:ListItem>
                                    </asp:RadioButtonList>
                                </div>
                            </td>
                        </tr>
                        <tr>
                            <td style="width: 30%;">
                                <label>Assessment Duration</label>
                            </td>
                            <td style="width: 10%">:
                            </td>
                            <td>
                                <asp:DropDownList ID="DurationDropDownList" runat="server">
                                    <asp:ListItem Selected="True">30 (mins)</asp:ListItem>
                                    <asp:ListItem>60 (mins)</asp:ListItem>
                                    <asp:ListItem>90 (mins)</asp:ListItem>
                                </asp:DropDownList>
                            </td>
                        </tr>
                        <tr>
                            <td style="width: 30%;">
                                <label>Assessment Descriptiton</label>
                            </td>
                            <td style="width: 10%">:
                            </td>
                            <td>
                                <asp:TextBox ID="DescriptionTextArea" TextMode="MultiLine" Style="height: 100px; text-align: justify;" placeholder="Description of Assessment to be created" runat="server"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td style="width: 30%;">
                                <br />
                                <br />
                                <label>Contributor</label>
                            </td>
                            <td style="width: 10%">
                                <br />
                                <br />
                                :
                            </td>
                            <td>
                                <br />
                                <br />
                                <div id="tableDiv" runat="server" class="reset-parent bootstrap-iso">
                                    <!-- Any HTML here will be styled with Bootstrap CSS -->
                                    <asp:PlaceHolder ID="ContributorTablePlaceHolder" runat="server"></asp:PlaceHolder>

                                </div>

                            </td>
                        </tr>
                        <tr>
                            <td colspan="3">
                                <br />
                                <hr />
                                <br />
                                <asp:Button ID="CreateButton" runat="server" Text="Create" OnClick="CreateButton_OnClick" ValidationGroup="ValidateGroup" />
                            </td>
                        </tr>


                    </table>

                </div>
            </div>

        </div>
    </div>
    <!-- content end -->
    <script type="text/javascript">
        //On document ready
        $(function () {
            $("#contentBody_AccessmentAccessRadioList_1").on('click', function (e) {
                swapDiv("popupBox");
            });
        });
        function swapDiv(div) {
            if (div == "popupBox") {
                document.getElementById("popupBoxId").style.cssText = "display:block";
                document.getElementById("assessmentId").style.cssText = "display:none";
                document.getElementsByTagName("body")[0].style.cssText = "overflow:hidden";
            } else {
                document.getElementById("popupBoxId").style.cssText = "";
                document.getElementById("assessmentId").style.cssText = "";
                document.getElementsByTagName("body")[0].style.cssText = "";
            }
        }
    </script>

</asp:Content>

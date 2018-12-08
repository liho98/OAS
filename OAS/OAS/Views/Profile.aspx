<%@ Page Title="OAS | Profile" Language="C#" MasterPageFile="~/MasterPage/MainSite.Master" AutoEventWireup="true" CodeBehind="Profile.aspx.cs" Inherits="OAS.Views.Profile" %>

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
                "scrollY": "285px",
                "scrollCollapse": true,
                paging: false,
                "searching": false,
                "info": false,
                "ordering": false,
                "lengthChange": false,
                "columnDefs": [{ "width": "10%", "targets": 0 }],
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
            width: 3px;
        }

        .dataTables_scrollBody::-webkit-scrollbar-track {
            -webkit-box-shadow: inset 0 0 5px rgba(0,0,0,0);
            background-color: white;
        }

        .dataTables_scrollBody::-webkit-scrollbar-thumb {
            background-color: rgba(100,100,100,1);
            outline: 1px solid slategrey;
        }

        .box-wrapper {
            background-color: #fff;
            position: absolute;
            top: 43%;
            right: 0;
            bottom: 0;
            left: 50%;
            width: 70%;
            height: 90%;
            transform: translate(-50%, -50%);
            user-select: none;
            font-family: Segoe UI;
        }

        .box {
            background-color: #fff;
            width: 75%;
            height: 75%;
            position: absolute;
            top: 50%;
            right: 0;
            bottom: 0;
            left: 50%;
            transform: translate(-50%, -50%);
        }

        #footerDiv {
            margin-top: -5px;
        }

        .linkOption a, .linkOption a:hover {
            all: unset;
            cursor: pointer;
            color: #495057;
            font-weight: 600;
            font-size: 14px;
            margin-bottom: 8px;
            display: block;
        }

        .editBtn {
            border: none;
            border-radius: 1.5rem;
            padding: 2%;
            font-weight: 600;
            color: #495057;
            cursor: pointer;
        }

        .rightPanel a, .rightPanel a:hover {
        }

        .swapButton {
            -webkit-appearance: none;
            display: inline-block;
            border: none;
            border-bottom: 2px solid #0062cc !important;
            font-weight: 600;
            color: #495057 !important;
            background-color: #fff !important;
            padding: .5rem 1rem !important;
            font-size: 1rem;
            outline: none;
            cursor: pointer;
            transition: none !important;
        }
        /* Fading animation */
        .fade {
            -webkit-animation-name: fade;
            -webkit-animation-duration: 1s;
            animation-name: fade;
            animation-duration: 1s;
        }

        @-webkit-keyframes fade {
            from {
                opacity: .4
            }

            to {
                opacity: 1
            }
        }

        @keyframes fade {
            from {
                opacity: .4
            }

            to {
                opacity: 1
            }
        }

        #timelineDiv, #aboutDiv, #passwordDiv {
            width: 80%;
            height: 100%;
            margin: auto;
        }

        input[type=text], input[type=email], input[type=date], input[type=password], select {
            all: unset;
        }

        tr:nth-child(2) input, select {
            font-size: 15px;
            font-weight: 500;
            color: #0062cc;
        }

        .inputBox {
            border-bottom: 1px solid rgb(200,200,200) !important;
            width: 100% !important;
        }

        #contentBody_datatables {
            text-align: left;
            font-weight: 400;
            font-size: 13px;
        }

            #contentBody_datatables tr {
                padding-bottom: 50px;
            }

            #contentBody_datatables td {
                border: 1px solid rgba(0,0,0,0.1);
            }
            #contentBody_CalendarUserControl_Literal{
                all:initial!important;
                cursor:pointer!important;
                transform: translateY(-120%)!important;
                float: right!important;
            }
        #contentBody_CalendarUserControl_Calendar {
            color: #fff;
            padding: 60px 40px 80px 40px;
            width: 40%;
            box-shadow: 0px 2px 6px rgba(2,2,2,0.2);
            position: relative;
            text-decoration: none !important;
            font-family: 'Avenir', sans-serif;
            background: linear-gradient(-45deg, #EE7752, #E73C7E, #23A6D5, #23D5AB);
            background-size: 400% 400%;
            animation: Gradient 15s ease infinite;
            border: none;
            font-size: 13px;
            position: absolute;
            z-index: 1;
            transform: translateY(-100%);
        }

            #contentBody_CalendarUserControl_Calendar tr td:first-child {
                background-color: transparent !important;
                font-size: 15px;
                font-weight: 700;
            }

            #contentBody_CalendarUserControl_Calendar tbody tr:first-child td {
                padding: 10px;
            }

            #contentBody_CalendarUserControl_Calendar a {
                all:initial;
                cursor:pointer;
                color: #fff !important;
                text-decoration: none;
            }

        @keyframes Gradient {
            0% {
                background-position: 0% 50%
            }

            50% {
                background-position: 100% 50%
            }

            100% {
                background-position: 0% 50%
            }
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
                <span id="slogan">Profile</span>
            </div>
        </div>
    </div>
</asp:Content>

<asp:Content ID="contentBody" ContentPlaceHolderID="contentBody" runat="server">
    <!-- content start -->
    <div id="bodyContent" style="background: linear-gradient(to bottom, rgba(0,0,0,0.1) 0%, #ffffff 100%);">
        <div class="content">

            <div class="box-wrapper" style="border-radius: 5px;">
                <div class="box" style="border-radius: 5px; width: 100%; height: 100%">

                    <table style="border-radius: 5px; width: 100%; height: 100%; table-layout: fixed;">
                        <tr style="height: 30%">

                            <td class="leftPanel" style="width: 30%; height: 45%;">

                                <div class="profile-img" style="transform: translate(0, 14%); text-align: center; width: 70%; height: 80%; margin: auto; background-color: blueviolet">
                                    <%--img id="userAvartar" runat="server" style="width: 100%; height: 100%; background-color: #d7d7d7; background-size: cover; background-position: center;"/--%>
                                    <div id="userAvartar" runat="server" style="width: 100%; height: 100%; background-color: #d7d7d7; background-size: cover; background-position: center;"></div>
                                    <asp:FileUpload ID="userAvatarUpload" runat="server" Style="display: none;" />
                                    <label id="changePhoto" visible="false" runat="server" for="contentBody_userAvatarUpload" style="cursor: pointer; background-color: rgba(33, 37, 41, 0.72); color: #fff; word-break: keep-all; width: 100%; line-height: 35px; display: inline-block; margin-top: -60px; vertical-align: middle;">
                                        Change Photo
                                    </label>
                                </div>
                            </td>

                            <td class="rightPanel" style="width: 70%; height: 45%; text-align: left">

                                <div style="transform: translate(0, 20%); width: 80%; height: 72%; margin: auto; background-color: white; border-bottom: 1px solid rgb(220,220,220)">

                                    <asp:Button ID="editProfileButton" autopostback="false" UseSubmitBehavior="false" OnClick="editProfileButton_OnClick" CssClass="editBtn" Style="float: right" runat="server" Text="Edit Profile" />
                                    <asp:Button ID="updateButton" OnClick="updateProfileButton_OnClick" CssClass="editBtn" Visible="false" Style="float: right" runat="server" Text="Save Changes" />


                                    <h3 id="Name" runat="server" style="font-family: Segoe UI Semibold; margin: 0; color: #333; margin-bottom: 1rem;"></h3>
                                    <h3 id="Role" runat="server" style="font-family: Segoe UI Semibold; font-size: 1rem; font-weight: 500; margin: 0; color: #333; color: #0062cc; margin-bottom: 1rem;"></h3>

                                    <p style="font-size: 12px; color: #818182; margin-top: 5%;">
                                        STATUS : 
                                        <span id="StatusText" runat="server" style="color: #495057; font-size: 15px; font-weight: 600;"></span>
                                    </p>

                                    <p style="margin-top: 20px; text-align: center; font-size: 14px;">
                                        <asp:Label ID="message" runat="server" Text=""></asp:Label>
                                    </p>

                                    <div style="position: absolute; bottom: 0">
                                        <a id="timelineButton" class="swapButton" onclick="swapDiv('Timeline');">Timeline
                                        </a>
                                        <a id="aboutButton" style="border-bottom: none!important; color: #007bff!important" class="swapButton" onclick="swapDiv('About');">About
                                        </a>
                                        <a id="passwordButton" style="border-bottom: none!important; color: #007bff!important" class="swapButton" onclick="swapDiv('Password');">Change password
                                        </a>
                                    </div>
                                </div>

                            </td>

                        </tr>
                        <tr style="height: 70%">
                            <td class="leftPanel" style="width: 30%;">

                                <div class="linkOption" style="transform: translate(0, 14%); width: 65%; height: 80%; text-align: left; margin: auto;">

                                    <% if (User.IsInRole("Administrators")){ %>

                                    <h4 style="font-size: 12px; color: #818182; font-weight: 600; margin-top: 0px; margin-bottom: 20px;">Manage User
                                    </h4>
                                    <a runat="server" href="~/Views/Administrator/ManageUserAccount.aspx">Manage User Account</a>
                                    <a runat="server" href="~/Views/Administrator/CreateUserAccount.aspx">Create User Account</a>
                                    <a runat="server" href="~/Views/Administrator/ManageRoles.aspx">Manage Roles</a>
                                    <a runat="server" href="~/Views/Administrator/UsersAndRoles.aspx">Provide Roles</a>
                                    <br />
                                    <h4 style="font-size: 12px; color: #818182; font-weight: 600; margin-top: 0px; margin-bottom: 20px;">Manage Assessment
                                    </h4>
                                    <a runat="server" href="~/Views/Lecturer/ManageAssessment.aspx">Assessment</a>
                                    <a runat="server" href="~/Views/Lecturer/CreateAssessment.aspx">Create</a>

                                    <%}else if (User.IsInRole("Lecturers")){ %>

                                    <h4 style="font-size: 12px; color: #818182; font-weight: 600; margin-top: 0px; margin-bottom: 20px;">Manage Assessment
                                    </h4>
                                    <a runat="server" href="~/Views/Lecturer/ManageAssessment.aspx">Assessment</a>
                                    <a runat="server" href="~/Views/Lecturer/CreateAssessment.aspx">Create</a>

                                    <%}else{ %>

                                    <h4 style="font-size: 12px; color: #818182; font-weight: 600; margin-top: 0px; margin-bottom: 20px;">Written
                                    </h4>
                                    <a runat="server" href="#">Written</a>

                                    <br />
                                    <h4 style="font-size: 12px; color: #818182; font-weight: 600; margin-top: 0px; margin-bottom: 20px;">MCQuestion
                                    </h4>
                                    <a runat="server" href="#">MCQ</a>

                                    <%} %>
                                </div>

                            </td>
                            <td id="container" class="rightPanel" style="transform: translate(0, 0%); font-weight: 600; width: 70%; height: 55%;">

                                <div id="timelineDiv" class="fade">

                                    <asp:PlaceHolder ID="TimelinePlaceHolder" runat="server"></asp:PlaceHolder>

                                </div>
                                <div id="aboutDiv" class="fade" style="display: none;">
                                    <table style="width: 100%; height: 90%; table-layout: fixed; text-align: left;">
                                        <tr>
                                            <td>
                                                <label>User ID</label></td>
                                            <td>
                                                <asp:TextBox ID="UserIdText" runat="server" Enabled="false"></asp:TextBox>
                                            </td>
                                        </tr>
                                        <%
                                            if (Roles.IsUserInRole(HttpContext.Current.User.Identity.Name.ToUpper(), "Students"))
                                            {
                                        %>
                                        <tr>
                                            <td>
                                                <label>Programme</label></td>
                                            <td>
                                                <asp:DropDownList ID="ProgrammeDropDownList" Enabled="false" runat="server">
                                                    <asp:ListItem>RSF</asp:ListItem>
                                                    <asp:ListItem>RSD</asp:ListItem>
                                                    <asp:ListItem>REI</asp:ListItem>
                                                    <asp:ListItem>RIP</asp:ListItem>
                                                </asp:DropDownList>
                                            </td>
                                        </tr>
                                        <% 
                                            }
                                            else
                                            {
                                        %>
                                        <tr>
                                            <td>
                                                <label>Position</label></td>
                                            <td>
                                                <asp:DropDownList ID="PositionDropDownList" Enabled="false" runat="server">
                                                    <asp:ListItem>Lecturer</asp:ListItem>
                                                    <asp:ListItem>Senior Lecturer</asp:ListItem>
                                                    <asp:ListItem>Principal Lecturer</asp:ListItem>
                                                </asp:DropDownList>
                                            </td>
                                        </tr>
                                        <%
                                            }
                                        %>


                                        <tr>
                                            <td>
                                                <label>First Name</label></td>
                                            <td>
                                                <asp:TextBox ID="FirstNametext" required="required" runat="server" Enabled="false"></asp:TextBox>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <label>Last Name</label></td>
                                            <td>
                                                <asp:TextBox ID="LastNameText" required="required" runat="server" Enabled="false"></asp:TextBox>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <label>Email</label></td>
                                            <td>
                                                <asp:TextBox ID="EmailText" required="required" TextMode="Email" runat="server" Enabled="false"></asp:TextBox>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <label>Phone</label></td>
                                            <td>
                                                <asp:TextBox ID="PhoneText" required="required" runat="server" Enabled="false"></asp:TextBox>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <label>Date of Birth</label></td>
                                            <td>
                                                <%-- 
                                                <asp:TextBox ID="DateOfBirthText" required="required" TextMode="Date" runat="server" Enabled="false"></asp:TextBox>
                                                --%>
                                                <uc:Calendar runat="server" ID="CalendarUserControl" OnCalendarVisibilityChanged="CalendarUserControl_OnCalendarVisibilityChanged" />
                                            </td>
                                        </tr>
                                    </table>
                                </div>
                                <div id="passwordDiv" class="fade" style="display: none;">
                                    <table style="width: 100%; height: 70%; table-layout: fixed; text-align: left">
                                        <tr>
                                            <td>
                                                <label>Current password*</label></td>
                                            <td>
                                                <asp:TextBox ID="currentPasswordText" TextMode="Password" class="inputBox" Style="color: black" runat="server" ValidationGroup="pass"></asp:TextBox>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <label>New password*</label></td>
                                            <td>
                                                <asp:TextBox ID="newPasswordText" TextMode="Password" class="inputBox" Style="color: black" runat="server" ValidationGroup="pass"></asp:TextBox>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <label>Confrim new password*</label></td>
                                            <td>
                                                <asp:TextBox ID="confirmPasswordText" TextMode="Password" class="inputBox" Style="color: black" runat="server" ValidationGroup="pass"></asp:TextBox>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td colspan="2">
                                                <asp:Button ID="changePass" ValidationGroup="pass" UseSubmitBehavior="false" OnClick="changePassButton_OnClick" CssClass="editBtn" Style="font-weight: 500; color: #495057; float: right" runat="server" Text="Change Password" />
                                            </td>

                                        </tr>
                                    </table>
                                    <asp:RequiredFieldValidator ID="OldPasswordRequiredFieldValidator"
                                        runat="server" SetFocusOnError="true" ErrorMessage="" ForeColor="red"
                                        ControlToValidate="currentPasswordText" Style="display: none"
                                        ValidationGroup="pass"
                                        Display="Dynamic" />

                                    <asp:RequiredFieldValidator ID="NewPasswordRequiredFieldValidator"
                                        runat="server" SetFocusOnError="true" ErrorMessage="" ForeColor="red"
                                        ControlToValidate="newPasswordText" Style="display: none"
                                        ValidationGroup="pass"
                                        Display="Dynamic" />

                                    <asp:RequiredFieldValidator ID="PasswordConfirmRequiredValidator"
                                        runat="server" SetFocusOnError="true" ErrorMessage="" ForeColor="red"
                                        ControlToValidate="confirmPasswordText" Style="display: none"
                                        ValidationGroup="pass"
                                        Display="Dynamic" />

                                    <asp:CompareValidator ID="PasswordConfirmCompareValidator" runat="server" ControlToValidate="confirmPasswordText" ControlToCompare="newPasswordText"
                                        Display="Dynamic" ForeColor="red" ErrorMessage="Confirm password must match password." SetFocusOnError="true"
                                        ValidationGroup="pass"></asp:CompareValidator>
                                </div>
                            </td>
                        </tr>
                    </table>

                </div>
            </div>
        </div>
    </div>
    <!-- content end -->

    <script type="text/javascript">
        function swapDiv(div) {
            if (div == "Timeline") {
                document.getElementById("timelineButton").style.cssText = "border-bottom: 2px solid #0062cc!important;";
                document.getElementById("aboutButton").style.cssText = "border-bottom:none!important;color:#007bff!important";
                document.getElementById("passwordButton").style.cssText = "border-bottom:none!important;color:#007bff!important";

                document.getElementById("timelineDiv").style.cssText = "display:block;";
                document.getElementById("aboutDiv").style.cssText = "display:none;";
                document.getElementById("passwordDiv").style.cssText = "display:none;";
            } else if (div == "About") {
                document.getElementById("timelineButton").style.cssText = "border-bottom: none!important;color:#007bff!important";
                document.getElementById("aboutButton").style.cssText = "border-bottom:2px solid #0062cc!important;";
                document.getElementById("passwordButton").style.cssText = "border-bottom:none!important;color:#007bff!important";

                document.getElementById("timelineDiv").style.cssText = "display:none";
                document.getElementById("aboutDiv").style.cssText = "display:block;";
                document.getElementById("passwordDiv").style.cssText = "display:none;";
            } else {
                document.getElementById("timelineButton").style.cssText = "border-bottom: none!important;color:#007bff!important";
                document.getElementById("aboutButton").style.cssText = "border-bottom: none!important;color:#007bff!important";
                document.getElementById("passwordButton").style.cssText = "border-bottom:2px solid #0062cc!important;";

                document.getElementById("timelineDiv").style.cssText = "display:none";
                document.getElementById("aboutDiv").style.cssText = "display:none;";
                document.getElementById("passwordDiv").style.cssText = "display:block;";
            }
            return false;
        }//contentBody_changePhotocontentBody_userAvatarUpload

        $(function () {
            $("#contentBody_userAvatarUpload").on("change", function () {
                var files = !!this.files ? this.files : [];
                if (!files.length || !window.FileReader) return; // no file selected, or no FileReader support

                if (/^image/.test(files[0].type)) { // only image file
                    var reader = new FileReader(); // instance of the FileReader
                    reader.readAsDataURL(files[0]); // read the local file

                    reader.onloadend = function () { // set image data as background of div
                        $("#contentBody_userAvartar").css("background-image", "url(" + this.result + ")");
                    }
                }
            });
        });
    </script>
</asp:Content>

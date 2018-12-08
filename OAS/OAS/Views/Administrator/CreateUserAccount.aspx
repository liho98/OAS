<%@ Page Title="OAS | Create User Account" Language="C#" MasterPageFile="~/MasterPage/AdministratorSite.Master" AutoEventWireup="true" CodeBehind="CreateUserAccount.aspx.cs" Inherits="OAS.Views.Administrator.CreateUserAccount" %>

<asp:Content ID="AdminHead" ContentPlaceHolderID="AdminHead" runat="server">
    <!-- Overide the #contentBody height value default is 1.8 -->
    <script>
        $(document).ready(function () {
            dynamicSetHeight(1.1);
            setLeftTriangle();
        });

        $(window).on('resize', function () {
            var instance = $.fn.deviceDetector;
            if (instance.isDesktop()) {
                dynamicSetHeight(1.1);
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

        #bodyContent {
            background-color: #fff;
        }

        h2 {
            margin: 0;
            font-size: 20px;
            color: #5f5d5d;
            font-weight: 500;
            font-family: "Segoe UI","Helvetica Neue","Lucida Grande","Roboto","Ebrima","Nirmala UI","Gadugi","Segoe Xbox Symbol","Segoe UI Symbol","Meiryo UI","Khmer UI","Tunga","Lao UI","Raavi","Iskoola Pota","Latha","Leelawadee","Microsoft YaHei UI","Microsoft JhengHei UI","Malgun Gothic","Estrangelo Edessa","Microsoft Himalaya","Microsoft New Tai Lue","Microsoft PhagsPa","Microsoft Tai Le","Microsoft Yi Baiti","Mongolian Baiti","MV Boli","Myanmar Text","Cambria Math";
        }

        select, input, #contentBody_RolesList {
            border-width: 1px;
            border-color: #666;
            border-color: rgba(0,0,0,.3);
            height: 35px;
            outline: none;
            border-radius: 0;
            -webkit-border-radius: 0;
            width: 100%;
            border-top-width: 0;
            border-left-width: 0;
            border-right-width: 0;
            font-size: 13px;
            font-weight: 100;
            margin-top: 10px;
            -webkit-transition: all 1s linear;
            transition: all 1s linear;
        }

            select, input[type=date] {
                color: black !important;
            }

        #contentBody_firstName {
            float: left
        }

        input[type=text]:focus, input[type=password]:focus {
            border-color: rgb(102,102,255);
        }

        #contentBody_Navigation_statusMessage {
            text-align: left;
            font-weight: 500;
            font-size: 11px;
            letter-spacing: 0.5px;
            display: inline-block;
            float: left;
            margin-top: 43px;
            max-width: 235px;
        }

        input[type=submit], button {
            border-color: #0067b8;
            background-color: #0067b8;
            color: #fff;
            padding: 4px 12px 4px 12px;
            height: 32px;
            width: auto;
            border-width: 1px;
            border-style: solid;
            cursor: pointer;
            text-overflow: ellipsis;
            touch-action: manipulation;
            position: absolute;
            display: inline-block;
            -webkit-appearance: button;
            bottom: 0;
            right: 0;
        }

        .box-wrapper {
            background-color: transparent;
            position: absolute;
            top: 50%;
            right: 0;
            bottom: 0;
            left: 50%;
            width: 440px;
            height: 360px;
            transform: translate(-50%, -50%);
            user-select: none;
            -webkit-box-shadow: 0px 0px 70px -10px rgba(138,242,226,1);
            -moz-box-shadow: 0px 0px 70px -10px rgba(138,242,226,1);
            box-shadow: 0px 0px 70px -10px rgba(138,242,226,1);
        }

        .box {
            background-color: transparent;
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

        .errorMsg {
            font-weight: 500;
            font-size: 11px;
            letter-spacing: 0.5px;
        }

        #contentBody_SiteMapPath a {
            padding: 0;
            background-color: #fff !important;
            color: rgba(125,155,125,1);
            text-decoration: underline;
        }

        #contentBody_Menu_SkipLink {
            display: none;
        }

        #contentBody_Navigation_CalendarUserControl_Literal {
            all: initial !important;
            cursor: pointer !important;
            transform: translateY(-140%) !important;
            float: right !important;
        }

        #contentBody_Navigation_CalendarUserControl_Calendar {
            color: #fff;
            padding: 60px 40px 80px 40px;
            width: 100%;
            height:;
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

            #contentBody_Navigation_CalendarUserControl_Calendar tr td:first-child {
                background-color: transparent !important;
                font-size: 15px;
                font-weight: 700;
            }

            #contentBody_Navigation_CalendarUserControl_Calendar tbody tr:first-child td {
                padding: 10px;
            }

            #contentBody_Navigation_CalendarUserControl_Calendar a {
                all: initial;
                cursor: pointer;
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
        #contentBody_Navigation_ValidationSummary ul{
            padding: 0;text-align:left;max-width: 235px;
        }
        #contentBody_Navigation_ValidationSummary li{
            display:none;
        }
        #contentBody_Navigation_ValidationSummary ul li:first-child{
            list-style-type: none;
            display:block;
        }
    </style>
</asp:Content>

<asp:Content ID="AdminVideoSource" ContentPlaceHolderID="AdminVideoSource" runat="server">
    <source runat="server" type="video/mp4" src="~/Content/videos/bg_video2.mp4">
    <script>
        var path = window.location.protocol + "//" + window.location.host;
        document.getElementsByClassName("background-image")[0].style.backgroundImage = "url('" + path + "/Content/images/background_images/login_signup_bg.jpg')";
    </script>
</asp:Content>

<asp:Content ID="AdminBackgroundText" ContentPlaceHolderID="AdminBackgroundText" runat="server">
    <!-- background text -->
    <div style="position: fixed; top: 14%; width: 100%; color: #fff">
        <div id="sloganDiv" style="text-align: center;">
            <div style="margin: 20px;">
                <span id="slogan">Manage User</span>
            </div>
        </div>
    </div>
</asp:Content>

<asp:Content ID="Navigation" ContentPlaceHolderID="Navigation" runat="server">

    <div class="box-wrapper" style="height: 685px; background-color: #fff">
        <div class="box" style="height: 83%">

            <asp:RequiredFieldValidator ID="userIDRequiredFieldValidator"
                runat="server"
                ControlToValidate="userID"
                ErrorMessage="Please fill the User ID field." style="display:none"
                ValidationGroup="ValidateGroup"
                Display="Static" ForeColor="Red" CssClass="errorMsg" />

            <asp:RequiredFieldValidator ID="firstNameRequiredFieldValidator"
                runat="server"
                ControlToValidate="firstName"
                ErrorMessage="Please fill the First Name field." style="display:none"
                ValidationGroup="ValidateGroup"
                Display="Static" ForeColor="Red" CssClass="errorMsg" />

            <asp:RequiredFieldValidator ID="lastNameRequiredFieldValidator"
                runat="server"
                ControlToValidate="lastName"
                ErrorMessage="Please fill the Last Name field." style="display:none"
                ValidationGroup="ValidateGroup"
                Display="Static" ForeColor="Red" CssClass="errorMsg" />

            <asp:RequiredFieldValidator ID="emailRequiredFieldValidator"
                runat="server"
                ControlToValidate="email"
                ErrorMessage="Please fill the Email field." style="display:none"
                ValidationGroup="ValidateGroup"
                Display="Static" ForeColor="Red" CssClass="errorMsg" />

            <asp:RequiredFieldValidator ID="passwordRequiredFieldValidator"
                runat="server"
                ControlToValidate="password"
                ErrorMessage="Please fill the Password field." style="display:none"
                ValidationGroup="ValidateGroup"
                Display="Static" ForeColor="Red" CssClass="errorMsg" />

            <asp:RequiredFieldValidator ID="confirmPasswordRequiredValidator"
                runat="server"
                ControlToValidate="confirmPassword"
                ErrorMessage="Please fill the confirm password field." style="display:none"
                ValidationGroup="ValidateGroup"
                Display="Static" ForeColor="Red" CssClass="errorMsg" />

            <asp:CompareValidator ID="confirmPasswordCompareValidator" runat="server" ControlToValidate="confirmPassword" ControlToCompare="password"
                Display="Static" ForeColor="red" ErrorMessage="Confirm password must match password." style="display:none"
                ValidationGroup="ValidateGroup" CssClass="errorMsg"></asp:CompareValidator>

            <asp:RequiredFieldValidator ID="RolesListRequiredFieldValidator"
                runat="server"
                ControlToValidate="RolesList"
                ErrorMessage="Please select the Roles List field." style="display:none"
                ValidationGroup="ValidateGroup"
                Display="Static" ForeColor="Red" CssClass="errorMsg" />

            <asp:RequiredFieldValidator ID="contactNoRequiredFieldValidator"
                runat="server"
                ControlToValidate="contactNo"
                ErrorMessage="Please enter the Contact No. field." style="display:none"
                ValidationGroup="ValidateGroup"
                Display="Static" ForeColor="Red" CssClass="errorMsg" />

            <h2>Create User account</h2>
            <asp:TextBox ID="userID" runat="server" placeholder="User ID" autofocus="autofocus"></asp:TextBox>
            <asp:TextBox ID="firstName" runat="server" placeholder="First name" Style="width: 49%;float: left;"></asp:TextBox>
            <asp:TextBox ID="lastName" runat="server" placeholder="Last name" Style="width: 49%; float: right"></asp:TextBox>
            <asp:TextBox ID="email" runat="server" TextMode="Email" placeholder="Email address"></asp:TextBox>
            <asp:TextBox ID="password" runat="server" TextMode="Password" placeholder="Password"></asp:TextBox>
            <asp:TextBox ID="confirmPassword" runat="server" TextMode="Password" placeholder="Confirm password"></asp:TextBox>
            <asp:DropDownList ID="RolesList" AutoPostBack="true" runat="server" OnSelectedIndexChanged="RolesList_OnSelectedChange" AppendDataBoundItems="true" Style="color: rgba(0,0,0,0.6); font-size: 13px;">
                <asp:ListItem Text="Select a Role" Selected="True" Value=""></asp:ListItem>
            </asp:DropDownList>

            <asp:DropDownList ID="gender" CssClass="input" Style="color: rgba(0,0,0,0.6); font-size: 13px;" runat="server">
                <asp:ListItem Selected="True">Male</asp:ListItem>
                <asp:ListItem>Female</asp:ListItem>
            </asp:DropDownList>
            <asp:TextBox ID="contactNo" runat="server" placeholder="Contact No."></asp:TextBox>
            <%-- 
            <asp:TextBox ID="dateOfBirth" runat="server" required="required" CssClass="input" Style="color: rgba(0,0,0,0.6); font-size: 13px;" placeholder="Date of Birth" onfocus="(this.type='date')"></asp:TextBox>
            --%>
            <uc:Calendar runat="server" ID="CalendarUserControl" />

            <asp:DropDownList ID="ProgramCode" Visible="false" CssClass="input" Style="color: rgba(0,0,0,0.6); font-size: 13px; margin-top: -5px;" runat="server">
                <asp:ListItem Selected="True">RSF</asp:ListItem>
                <asp:ListItem>RSD</asp:ListItem>
                <asp:ListItem>REI</asp:ListItem>
                <asp:ListItem>RIP</asp:ListItem>
            </asp:DropDownList>

            <asp:DropDownList ID="position" Visible="false" CssClass="input" Style="color: rgba(0,0,0,0.6); font-size: 13px; margin-top: -5px;" runat="server">
                <asp:ListItem Selected="True">Lecturer</asp:ListItem>
                <asp:ListItem>Senior Lecturer</asp:ListItem>
                <asp:ListItem>Principal Lecturer</asp:ListItem>
            </asp:DropDownList>

            <asp:Label ID="statusMessage" runat="server" ForeColor="Red"></asp:Label>
            <asp:ValidationSummary ID="ValidationSummary" runat="server" Font-Size="Small" ForeColor="Red" ValidationGroup="ValidateGroup" 
                DisplayMode="BulletList" ShowSummary="true"  />

            <asp:Button ID="CreateButton" runat="server" Text="Create" ValidationGroup="ValidateGroup" OnClick="CreateAccountButton_Click" />

        </div>
    </div>
</asp:Content>

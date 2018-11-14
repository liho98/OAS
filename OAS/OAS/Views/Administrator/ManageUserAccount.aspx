<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage/AdministratorSite.Master" AutoEventWireup="true" CodeBehind="ManageUserAccount.aspx.cs" Inherits="OAS.Views.Administrator.ManageUserAccount" %>

<asp:Content ID="AdminHead" ContentPlaceHolderID="AdminHead" runat="server">

    <link rel="stylesheet" id="bootstrapCss">
    <link rel="stylesheet" href="https://cdn.datatables.net/1.10.19/css/dataTables.bootstrap4.min.css">

    <script src="https://code.jquery.com/jquery-3.3.1.js"></script>
    <script src="https://cdn.datatables.net/1.10.19/js/jquery.dataTables.min.js"></script>
    <script src="https://cdn.datatables.net/1.10.19/js/dataTables.bootstrap4.min.js"></script>

    <style>
        video {
            opacity: 0.6;
        }

        .background-image {
            filter: brightnes(250%);
            background-size: cover;
            background-position: center;
        }

        .dataTables_wrapper {
            width: 80%;
            margin: auto;
            margin-top: 80px;
        }

        .reset-parent, #contentBody_Navigation_datatables_next {
            all: initial;
        }

        .form-control {
            height: 30px !important;
        }

        .dt-body-center {
            text-align: center !important;
        }

        .actionButton {
            all: initial !important;
            cursor: pointer !important;
            color: red !important;
        }

            .actionButton + button {
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

        h2 {
            display: inline-block;
            margin-top: 0;
            font-size: 20px;
            color: #5f5d5d;
            font-weight: 500;
            font-family: "Segoe UI","Helvetica Neue","Lucida Grande","Roboto","Ebrima","Nirmala UI","Gadugi","Segoe Xbox Symbol","Segoe UI Symbol","Meiryo UI","Khmer UI","Tunga","Lao UI","Raavi","Iskoola Pota","Latha","Leelawadee","Microsoft YaHei UI","Microsoft JhengHei UI","Malgun Gothic","Estrangelo Edessa","Microsoft Himalaya","Microsoft New Tai Lue","Microsoft PhagsPa","Microsoft Tai Le","Microsoft Yi Baiti","Mongolian Baiti","MV Boli","Myanmar Text","Cambria Math";
        }

        input, select {
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
            color: black !important;
            -webkit-transition: all 1s linear;
            transition: all 1s linear;
        }

        #contentBody_Navigation_firstName {
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
            margin-top: 25px;
            max-width: 235px;
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

        #contentBody_Navigation_updateDiv {
            display: none !important;
        }

        .box {
            background-color: transparent;
            width: 75%;
            height: 70%;
            position: absolute;
            top: 50%;
            right: 0;
            bottom: 0;
            left: 50%;
            transform: translate(-50%, -50%);
        }
        /* Fading animation */
        .fade {
            display: block !important;
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
        #bodyContent{
            background-color:#fff;
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

    <div id="tableDiv" runat="server" class="reset-parent bootstrap-iso fade">
        <!-- Any HTML here will be styled with Bootstrap CSS -->
        <asp:PlaceHolder ID="TablePlaceHolder" runat="server"></asp:PlaceHolder>
        <div style="width: 100%">
            <asp:Label ID="Message" runat="server" Style="text-align: center; display: block; color: green;font-weight: 500;font-size: 14px;letter-spacing: 0.5px;"></asp:Label>
        </div>
    </div>

    <div id="updateDiv" runat="server" class="box-wrapper fade">
        <div class="box" style="height: 85%">

            <script runat="server">

            </script>

            <h2>Edit User account</h2>
            <asp:LinkButton ID="returnLink" runat="server" OnClick="returnLink_OnClick" title="Back" Style="all: unset; right: 0; position: absolute; cursor: pointer">
                            <i class="material-icons" style="float:right;margin-top:5px;font-weight:500;font-size:25px;">arrow_back</i>
            </asp:LinkButton>

            <asp:TextBox ID="userID" runat="server" Enabled="false" Style="background-color: transparent; cursor: not-allowed"></asp:TextBox>
            <asp:TextBox ID="firstName" runat="server" required="required" autofocus="autofocus" placeholder="First name" Style="width: 49%"></asp:TextBox>
            <asp:TextBox ID="lastName" runat="server" required="required" placeholder="Last name" Style="width: 49%; float: right"></asp:TextBox>
            <asp:TextBox ID="email" TextMode="Email" required="required" runat="server" placeholder="Email address"></asp:TextBox>
            <asp:TextBox ID="password" runat="server" required="required" TextMode="Password" placeholder="New password"></asp:TextBox>
            <asp:TextBox ID="confirmPassword" runat="server" required="required" TextMode="Password" placeholder="Confirm new password"></asp:TextBox>

            <asp:DropDownList ID="RolesList" runat="server" AutoPostBack="true" OnSelectedIndexChanged="RolesList_OnSelectedChange" AppendDataBoundItems="true" required="required">
            </asp:DropDownList>

            <asp:DropDownList ID="gender" CssClass="input" Style="color: rgba(0,0,0,0.6); font-size: 13px;" runat="server">
                <asp:ListItem Selected="True">Male</asp:ListItem>
                <asp:ListItem>Female</asp:ListItem>
            </asp:DropDownList>
            <asp:TextBox ID="contactNo" runat="server" required="required" placeholder="Contact No."></asp:TextBox>
            <asp:TextBox ID="dateOfBirth" runat="server" TextMode="Date" required="required" CssClass="input" Style="color: rgba(0,0,0,0.6); font-size: 13px;"></asp:TextBox>

            <asp:DropDownList ID="ProgramCode" Visible="false" CssClass="input" Style="color: rgba(0,0,0,0.6); font-size: 13px;" runat="server">
                <asp:ListItem Selected="True">RSF</asp:ListItem>
                <asp:ListItem>RSD</asp:ListItem>
                <asp:ListItem>REI</asp:ListItem>
                <asp:ListItem>RIP</asp:ListItem>
            </asp:DropDownList>

            <asp:DropDownList ID="position" Visible="false" CssClass="input" Style="color: rgba(0,0,0,0.6); font-size: 13px;" runat="server">
                <asp:ListItem Selected="True">Lecturer</asp:ListItem>
                <asp:ListItem>Senior Lecturer</asp:ListItem>
                <asp:ListItem>Principal Lecturer</asp:ListItem>
            </asp:DropDownList>

            <asp:CompareValidator ID="PasswordConfirmCompareValidator" runat="server" ControlToValidate="confirmPassword" ControlToCompare="password"
                Display="Static" ForeColor="red" ErrorMessage="Confirm password must match password."
                ValidationGroup="pass" CssClass="errorMsg"></asp:CompareValidator>

            <asp:Label ID="statusMessage" runat="server" ForeColor="Red"></asp:Label>
            <asp:RequiredFieldValidator ID="PasswordConfirmRequiredValidator"
                runat="server"
                ControlToValidate="confirmPassword"
                ErrorMessage=""
                ValidationGroup="pass"
                Display="Dynamic" ForeColor="Red" CssClass="errorMsg" />

            <asp:Button ID="UpdateButton" runat="server" ValidationGroup="pass" Text="Save Changes" Style="color: #fff!important; width: auto!important;" OnClick="UpdateAccountButton_Click" />

        </div>
    </div>

</asp:Content>

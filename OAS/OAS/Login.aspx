<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage/LoginSite.Master" AutoEventWireup="true" CodeBehind="Login.aspx.cs" Inherits="OAS.Login" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="loginBox" ContentPlaceHolderID="loginBox" runat="server">

    <div class="box-wrapper">
        <div class="box">
            <a runat="server" href="~/Default.aspx" title="OAS">
                <span>
                    <img runat="server" id="icon" style="width: 35px;" src="~/Content/images/icons_logos/oas_blue.png" alt="Logo"></span>
                <span id="logo-name">&nbsp;O&nbsp;A&nbsp;S<span style="font-size: 8px"> &copy;</span></span>
            </a>

            <h2>Sign in</h2>

            <asp:TextBox ID="emailID" runat="server" placeholder="Email or User ID" autofocus="autofocus"></asp:TextBox>
            <asp:TextBox ID="password" runat="server" TextMode="Password" placeholder="Password"></asp:TextBox>

            <div style="font-weight: 100; font-size: 13px;">
                <br />
                <!--span>No account?&nbsp;</!--span-->
                <a runat="server" href="#" style="color: #0067b8;">&nbsp;Forgot password?</a>
                <label style="float: right">
                    <asp:CheckBox ID="rememberMe" runat="server" />
                    <span style="margin-top: -7px; display: inline-block; vertical-align: middle;">Remember me</span>
                </label>
            </div>

            <asp:Label ID="invalidCredentialsMessage" runat="server" Visible="false" ForeColor="Red" Text="Your username or password is invalid. Please try again."></asp:Label>

            <asp:Button ID="loginButton" runat="server" Text="Sign in" OnClick="loginButton_Click" />

        </div>
    </div>
</asp:Content>

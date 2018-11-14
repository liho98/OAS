<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage/LoginSite.Master" AutoEventWireup="true" CodeBehind="Login.aspx.cs" Inherits="OAS.Login" %>

<asp:Content ID="head" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="loginBox" ContentPlaceHolderID="loginBox" runat="server">

    <div class="box-wrapper">
        <div class="box">
            <a runat="server" href="~/Default.aspx" title="OAS">
                <span>
                    <img runat="server" id="icon" style="width: 35px;" src="~/Content/images/icons_logos/oas_blue.png" alt="Logo"></span>
                <span class="logo-name">&nbsp;O&nbsp;A&nbsp;S<span style="font-size: 8px"> &copy;</span></span>
            </a>

            <h2>Sign in</h2>

            <asp:TextBox ID="emailID" runat="server" placeholder="Email or User ID" autofocus="autofocus"></asp:TextBox>
            <asp:TextBox ID="password" runat="server" TextMode="Password" placeholder="Password"></asp:TextBox>

            <div style="font-weight: 100; font-size: 13px;">
                <br />
                <!--span>No account?&nbsp;</!--span-->
                <label style="float: right">
                    <asp:CheckBox ID="rememberMe" runat="server" />
                    <span style="margin-top: -7px; display: inline-block; vertical-align: middle;">Remember me</span>
                </label>
                <a runat="server" href="#" style="color: #0067b8;margin-left:-3px;">&nbsp;Forgot password?</a>
                <div style="margin-top:3px;">
                    <span>No account? </span>
                    <a runat="server" href="~/SignUp.aspx" style="color: #0067b8;">&nbsp;&nbsp;Create one!</a>
                </div>
            </div>

            <asp:Label ID="invalidCredentialsMessage" style="margin-top:16px;" runat="server" Visible="false" ForeColor="Red" Text="Your username or password is invalid. Please try again."></asp:Label>

            <asp:Button ID="loginButton" runat="server" Text="Sign in" OnClick="loginButton_Click" />

        </div>
    </div>
</asp:Content>

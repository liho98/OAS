<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage/LoginSite.Master" AutoEventWireup="true" CodeBehind="SignUp.aspx.cs" Inherits="OAS.Others.SignUp" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="loginBox" ContentPlaceHolderID="loginBox" runat="server">
    <div class="box-wrapper" style="height: 480px">
        <div class="box" style="height: 80%">
            <a runat="server" href="~/Default.aspx" title="OAS">
                <span>
                    <img runat="server" id="icon" style="width: 35px;" src="~/Content/images/icons_logos/oas_blue.png" alt="Logo"></span>
                <span id="logo-name">&nbsp;O&nbsp;A&nbsp;S<span style="font-size: 8px"> &copy;</span></span>
            </a>

            <h2>Create account</h2>
            <asp:TextBox ID="userID" runat="server" placeholder="User ID" autofocus="autofocus"></asp:TextBox>
            <asp:TextBox ID="firstName" runat="server" placeholder="First name" Style="width: 49%"></asp:TextBox>
            <asp:TextBox ID="lastName" runat="server" placeholder="Last name" Style="width: 49%; float: right"></asp:TextBox>
            <asp:TextBox ID="email" runat="server" placeholder="Email address"></asp:TextBox>
            <asp:TextBox ID="password" runat="server" TextMode="Password" placeholder="Password"></asp:TextBox>
            <asp:TextBox ID="confirmPassword" runat="server" TextMode="Password" placeholder="Confirm password"></asp:TextBox>

            <asp:Label ID="invalidDetailsMessage" runat="server" ForeColor="Red"></asp:Label>

            <asp:Button ID="signUpButton" runat="server" Text="Sign up" OnClick="SignUpButton_Click" />
        </div>
    </div>
</asp:Content>

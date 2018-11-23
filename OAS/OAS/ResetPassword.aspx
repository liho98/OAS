<%@ Page Title="OAS | Reset Password" Language="C#" MasterPageFile="~/MasterPage/LoginSite.Master" AutoEventWireup="true" CodeBehind="ResetPassword.aspx.cs" Inherits="OAS.ResetPassword" %>

<asp:Content ID="head" ContentPlaceHolderID="head" runat="server">
    <style>
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

        #forgotPassDiv {
            display: none !important;
        }

        .msg {
            font-weight: 500;
            font-size: 11px;
            letter-spacing: 0.5px;
            display: inline-block;
            margin-top: 55px;
            max-width: 235px;
        }
    </style>
</asp:Content>
<asp:Content ID="loginBox" ContentPlaceHolderID="loginBox" runat="server">

    <div class="box-wrapper">
        <div class="box">

            <a runat="server" href="~/Default.aspx" title="OAS">
                <span>
                    <img runat="server" id="icon" style="width: 35px;" src="~/Content/images/icons_logos/oas_blue.png" alt="Logo"></span>
                <span class="logo-name">&nbsp;O&nbsp;A&nbsp;S<span style="font-size: 8px"> &copy;</span></span>
            </a>

            <h2 style="margin-bottom: 10px;">Reset Your Pasword</h2>

            <asp:TextBox ID="newPassword" runat="server" TextMode="Password" required="required" placeholder="New password" autofocus="autofocus"></asp:TextBox>
            <asp:TextBox ID="confirmpassword" runat="server" TextMode="Password" required="required" placeholder="Confirm new password"></asp:TextBox>

            <asp:Label ID="Message" CssClass="msg" Style="margin-top: 55px;" runat="server"></asp:Label>

            <asp:Button ID="submitButton" runat="server" Text="Submit" OnClick="submitButton_Click" ValidationGroup="pass" />

            <asp:RequiredFieldValidator ID="PasswordConfirmRequiredValidator"
                runat="server"
                ControlToValidate="confirmPassword"
                ErrorMessage=""
                ValidationGroup="pass"
                Display="Dynamic" ForeColor="Red" CssClass="msg" />

            <asp:CompareValidator ID="PasswordConfirmCompareValidator" runat="server" ControlToValidate="confirmPassword" ControlToCompare="newPassword"
                Display="Static" ForeColor="red" ErrorMessage="Confirm password must match new password."
                ValidationGroup="pass" CssClass="msg"></asp:CompareValidator>


        </div>
    </div>

</asp:Content>

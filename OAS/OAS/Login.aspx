<%@ Page Title="OAS | Login" Language="C#" MasterPageFile="~/MasterPage/LoginSite.Master" Async="true" AutoEventWireup="true" CodeBehind="Login.aspx.cs" Inherits="OAS.Login" %>

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
            margin-top: 38px;
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
            <a id="returnLink" runat="server" title="Back" onclick="swapDiv('return');" style="display: none;">
                <i class="material-icons" style="cursor: pointer; float: right!important; margin-top: 5px; font-weight: 500; font-size: 25px;">arrow_back</i>
            </a>


            <div id="loginDiv" class="fade">

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
                    <a runat="server" style="color: #0067b8; margin-left: -3px; cursor: pointer" onclick="swapDiv('forgotPassword');">&nbsp;Forgot password?</a>
                    <div style="margin-top: 3px;">
                        <span>No account? </span>
                        <a runat="server" href="~/SignUp.aspx" style="color: #0067b8;">&nbsp;&nbsp;Create one!</a>
                    </div>
                </div>

                <asp:Label ID="invalidCredentialsMessage" Style="margin-top: 16px;" runat="server" Visible="false" ForeColor="Red" Text="Your username or password is invalid. Please try again."></asp:Label>

                <asp:Button ID="loginButton" runat="server" Text="Sign in" OnClick="loginButton_Click" />

            </div>



            <div id="forgotPassDiv" class="fade">

                <h2>Recover your account</h2>

                <p style="font-family: Segoe UI; font-size: 14px; margin-bottom: 5px; text-align: justify">
                    We can help you reset your password and security info. First, enter your OAS account email and follow the instructions below.
                </p>

                <asp:TextBox ID="EmailTextBox" TextMode="Email" runat="server" placeholder="Email Address" autofocus="autofocus"></asp:TextBox>

                <asp:Label ID="StatusLabel" CssClass="msg" runat="server" Style="text-align: justify;"></asp:Label>

                <asp:Button ID="NextButton" runat="server" Text="Next" OnClick="NextButton_OnClick" />

            </div>

        </div>
    </div>


    <script type="text/javascript">
        function swapDiv(div) {
            if (div == "forgotPassword") {
                document.getElementById("loginDiv").style.cssText = "display:none!important;";
                document.getElementById("forgotPassDiv").style.cssText = "display:block!important;";
                document.getElementById("loginBox_returnLink").style.cssText = "";
                document.getElementById("loginBox_EmailTextBox").focus();

            } else {
                document.getElementById("loginDiv").style.cssText = "display:block!important;";
                document.getElementById("forgotPassDiv").style.cssText = "display:none!important;";
                document.getElementById("loginBox_returnLink").style.cssText = "display:none!important;";
            }
            return false;
        }
    </script>


</asp:Content>

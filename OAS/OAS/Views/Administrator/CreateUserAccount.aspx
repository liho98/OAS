<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage/MainSite.Master" AutoEventWireup="true" CodeBehind="CreateUserAccount.aspx.cs" Inherits="OAS.Views.Administrator.CreateUserAccount" %>

<asp:Content ID="head" ContentPlaceHolderID="head" runat="server">
    <!-- Overide the #contentBody height value default is 1.8 -->
    <script>
        var path = window.location.protocol + "//" + window.location.host;

        $.when(
            $.getScript(path + "/Scripts/js/dynamicSetHeight.js"),
            $.Deferred(function (deferred) {
                $(deferred.resolve);
            })
        ).done(function () {
            //place your code here, the scripts are all loaded
            dynamicSetHeight(0.85);
            setLeftTriangle();
        });

        $.when(
            $.getScript(path + "/Scripts/js/jquery.device.detector.js"),
            $.Deferred(function (deferred) {
                $(deferred.resolve);
            })
        ).done(function () {
            $(window).on('resize', function () {
                var instance = $.fn.deviceDetector;
                if (instance.isDesktop()) {
                    dynamicSetHeight(0.85);
                }
                if ($(window).outerWidth() > 958) {
                    setLeftTriangle();
                } else {
                    document.getElementById("triangle-div").style.left = "";
                }
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

        h2 {
            margin-bottom: 0;
            font-size: 20px;
            color: #5f5d5d;
            font-weight: 500;
            font-family: "Segoe UI","Helvetica Neue","Lucida Grande","Roboto","Ebrima","Nirmala UI","Gadugi","Segoe Xbox Symbol","Segoe UI Symbol","Meiryo UI","Khmer UI","Tunga","Lao UI","Raavi","Iskoola Pota","Latha","Leelawadee","Microsoft YaHei UI","Microsoft JhengHei UI","Malgun Gothic","Estrangelo Edessa","Microsoft Himalaya","Microsoft New Tai Lue","Microsoft PhagsPa","Microsoft Tai Le","Microsoft Yi Baiti","Mongolian Baiti","MV Boli","Myanmar Text","Cambria Math";
        }

        input[type=text], input[type=password], #contentBody_RolesList {
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

        #contentBody_firstName {
            float: left
        }

        input[type=text]:focus, input[type=password]:focus {
            border-color: rgb(102,102,255);
        }

        #contentBody_statusMessage {
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
                <span id="slogan">Manage User</span>
            </div>
        </div>
    </div>
</asp:Content>

<asp:Content ID="contentBody" ContentPlaceHolderID="contentBody" runat="server">
    <!-- content start -->
    <div id="bodyContent" style="background-color: #fff; border-bottom: 1px solid rgb(205,205,205); z-index: 1">
        <div class="content">

            <div class="box-wrapper" style="height: 480px">
                <div class="box" style="height: 83%">

                    <script runat="server">
                        protected void Page_Load(object sender, EventArgs e)
                        {
                            if (!Page.IsPostBack)
                            {
                                BindRolesToRolesList();

                            }
                        }

                        private void BindRolesToRolesList()
                        {
                            // Get all of the roles 
                            RolesList.DataSource = Roles.GetAllRoles();
                            RolesList.DataBind();

                            RolesList.Attributes.CssStyle.Add("color", "rgba(0,0,0,0.6)");
                            RolesList.Items[0].Attributes.Add("disabled", "disabled");
                            RolesList.Items[0].Attributes.CssStyle.Add("background-color", "rgba(200,200,200,0.6)");
                        }
                        protected void CreateAccountButton_Click(object sender, EventArgs e)
                        {
                            MembershipCreateStatus createStatus;
                            MembershipUser newUser;
                            try
                            {
                                // Create new user.
                                if (Membership.RequiresQuestionAndAnswer)
                                {
                                    newUser = Membership.CreateUser(
                                      userID.Text,
                                      password.Text,
                                      email.Text,
                                      "",
                                      "",
                                      false,
                                      out createStatus);
                                }
                                else
                                {
                                    newUser = Membership.CreateUser(
                                      userID.Text,
                                      password.Text,
                                      email.Text);
                                }
                                Roles.AddUserToRole(userID.Text, RolesList.SelectedValue);
                                statusMessage.ForeColor = System.Drawing.Color.Green;
                                statusMessage.Text = "Successfully created user " + userID.Text + ".";
                            }
                            catch (MembershipCreateUserException ex)
                            {
                                statusMessage.ForeColor = System.Drawing.Color.Red;
                                statusMessage.Text = GetErrorMessage(ex.StatusCode);
                            }
                            catch (HttpException ex)
                            {
                                statusMessage.ForeColor = System.Drawing.Color.Red;
                                statusMessage.Text = ex.Message;
                            }
                        }
                    </script>

                    <h2>Create User account</h2>
                    <asp:TextBox ID="userID" runat="server" placeholder="User ID" autofocus="autofocus"></asp:TextBox>
                    <asp:TextBox ID="firstName" runat="server" placeholder="First name" Style="width: 49%"></asp:TextBox>
                    <asp:TextBox ID="lastName" runat="server" placeholder="Last name" Style="width: 49%; float: right"></asp:TextBox>
                    <asp:TextBox ID="email" runat="server" placeholder="Email address"></asp:TextBox>
                    <asp:TextBox ID="password" runat="server" TextMode="Password" placeholder="Password"></asp:TextBox>
                    <asp:TextBox ID="confirmPassword" runat="server" TextMode="Password" placeholder="Confirm password"></asp:TextBox>
                    <asp:DropDownList ID="RolesList" runat="server" AppendDataBoundItems="true" required="required" onchange="changeColor()">
                        <asp:ListItem Text="Select a Role" Selected="True" Value=""></asp:ListItem>
                    </asp:DropDownList>

                    <asp:Label ID="statusMessage" runat="server" ForeColor="Red"></asp:Label>

                    <asp:Button ID="CreateButton" runat="server" Text="Create" OnClick="CreateAccountButton_Click" />
                </div>
            </div>

        </div>
    </div>
    <!-- content end -->
    <script>
        function changeColor() {
            document.getElementById("contentBody_RolesList").style.color = "black";
        }
    </script>
</asp:Content>

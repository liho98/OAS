<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage/MainSite.Master" AutoEventWireup="true" CodeBehind="ManageRoles.aspx.cs" Inherits="OAS.Views.Administrator.ManageRoles" %>

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
            dynamicSetHeight(0.65);
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
                    dynamicSetHeight(0.65);
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

        .roleTable::-webkit-scrollbar {
            width: 5px;
            height: 5px;
        }

        .roleTable::-webkit-scrollbar-track {
            -webkit-box-shadow: inset 0 0 5px rgba(0,0,0,0);
            background-color: white;
        }

        .roleTable::-webkit-scrollbar-thumb {
            background-color: rgba(100,100,100,1);
            outline: 1px solid slategrey;
        }

        input[type=text] {
            border-width: 1px;
            border-color: #666;
            border-color: rgba(0,0,0,.3);
            outline: none;
            border-radius: 0;
            -webkit-border-radius: 0;
            /*width:96%;*/
            width: 100%;
            border-top-width: 0;
            border-left-width: 0;
            border-right-width: 0;
            font-size: 13px;
            font-weight: 100;
            -webkit-transition: all 1s linear;
            transition: all 1s linear;
        }

            input[type=text]:focus, input[type=password]:focus {
                width: 100%;
                border-color: rgb(102,102,255);
            }

        input[type=submit], button {
            border-color: #0067b8;
            background-color: #0067b8;
            color: #fff;
            padding: 4px 12px 4px 12px;
            border-width: 1px;
            border-style: solid;
            cursor: pointer;
            text-overflow: ellipsis;
            touch-action: manipulation;
            display: inline-block;
            -webkit-appearance: button;
            bottom: 0;
            right: 0;
        }

        #contentBody_createMsg, #contentBody_deleteMsg {
            font-weight: 500;
            font-size: 11px;
            letter-spacing: 0.5px;
        }

        #bodyContent {
            background-color: #fff;
            z-index: 1;
        }

        .tableContent {
            table-layout: fixed;
            width: 80%;
            height: 290px;
            margin: auto;
            margin-top: 100px;
        }

            .tableContent td {
                background-color: rgba(240,240,240,1);
                padding-top: 5px;
                padding-right: 20px;
            }

        .roleDiv {
            height: 150px;
            width: 60%;
            margin: auto;
        }

        .roleTable {
            display: block;
            height: 90%;
            width: 100%;
            margin: auto;
            text-align: left;
            overflow-y: scroll;
        }

        /* (when the browser window is 959px wide or less) */
        @media only screen and (max-width:959px) {
            .tableContent {
            }

            td {
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
                <span id="slogan">Manage Roles</span>
            </div>
        </div>
    </div>
</asp:Content>

<script runat="server">

    public void Page_Load(object sender, EventArgs args)
    {
        if (!IsPostBack)
        {
            // Bind roles to RepeaterView.
            RolesRepeater.DataSource = Roles.GetAllRoles();
            RolesRepeater.DataBind();

        }
    }

    public void CreateRole_OnClick(object sender, EventArgs args)
    {
        string roleName = RoleTextBox.Text.Trim();
        try
        {
            if (Roles.RoleExists(roleName))
            {
                createMsg.ForeColor = System.Drawing.Color.Red;
                createMsg.Text = "Role \"" + Server.HtmlEncode(roleName) + "\" already exists. Please specify a different role name.";
                RoleTextBox.Text = "";
                return;
            }

            Roles.CreateRole(roleName);
            createMsg.ForeColor = System.Drawing.Color.Green;
            createMsg.Text = "Role \"" + Server.HtmlEncode(roleName) + "\" created.";
            RoleTextBox.Text = "";

            // Re-bind roles to RepeaterView
            RolesRepeater.DataSource = Roles.GetAllRoles(); ;
            RolesRepeater.DataBind();
        }
        catch (Exception ex)
        {
            createMsg.ForeColor = System.Drawing.Color.Red;
            createMsg.Text = "Please enter a valid Role name.";
            RoleTextBox.Text = "";
        }
    }
    public void DeleteRole_OnCommand(object sender, CommandEventArgs e)
    {
        try
        {
            Roles.DeleteRole(e.CommandArgument.ToString());
            // Re-bind roles to RepeaterView
            RolesRepeater.DataSource = Roles.GetAllRoles(); ;
            RolesRepeater.DataBind();
            deleteMsg.ForeColor = System.Drawing.Color.Green;
            deleteMsg.Text = "Role \"" + e.CommandArgument.ToString() + "\" deleted.";
        }
        catch
        {
            deleteMsg.ForeColor = System.Drawing.Color.Red;
            deleteMsg.Text = "Role \"" + e.CommandArgument.ToString() + "\" not deleted. Because there are user bind with this role. Kindly remove all the users from this role and proceed this operation again.";
        }

    }
</script>

<asp:Content ID="contentBody" ContentPlaceHolderID="contentBody" runat="server">
    <!-- content start -->
    <div id="bodyContent">
        <div class="content">
            <table class="tableContent">
                <tr>
                    <td>
                        <h4>Existing Roles</h4>

                        <div class="roleDiv">
                            <asp:Repeater ID="RolesRepeater" runat="server">
                                <HeaderTemplate>
                                    <table class="roleTable">
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <tr>
                                        <td style="width: 30%;"><%# Container.ItemIndex + 1 %>.</td>
                                        <td style="width: 50%;">
                                            <%# Container.DataItem %></td>
                                        <td style="width: 20%;">
                                            <asp:Button ID="DeleteRole" runat="server" Text="Remove" CommandName="Delete" CommandArgument="<%# Container.DataItem %>" OnCommand="DeleteRole_OnCommand" Style="float: right" />
                                        </td>
                                    </tr>
                                </ItemTemplate>
                                <FooterTemplate>
                                    </table>
                                </FooterTemplate>
                            </asp:Repeater>
                            <div style="margin-top: 5%">
                                <asp:Label ID="deleteMsg" runat="server"></asp:Label>
                            </div>
                        </div>
                    </td>
                    <td>
                        <h4>Create a Role</h4>
                        <div class="roleDiv">
                            <table class="roleTable" style="overflow: hidden">
                                <tr>
                                    <td style="width: 40%; text-align: left">
                                        <asp:Label ID="Label1" runat="server" Text="Label">Role name</asp:Label>
                                    </td>
                                    <td style="width: 10%;">:</td>
                                    <td style="width: 45%;">
                                        <asp:TextBox ID="RoleTextBox" runat="server" placeholder="eg. Admin" MaxLength="30" autofocus="autofocus"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="2" style="text-align: left">
                                        <br />
                                    </td>
                                    <td>
                                        <br />
                                        <asp:Button ID="CreateRoleButton" runat="server" Text="Create Role" OnClick="CreateRole_OnClick" Style="float: right" />
                                    </td>
                                </tr>
                            </table>
                            <div style="margin-top: 5%">
                                <asp:Label ID="createMsg" runat="server"></asp:Label>
                            </div>
                        </div>
                    </td>
                </tr>
            </table>
        </div>
    </div>
    <!-- content end -->

</asp:Content>

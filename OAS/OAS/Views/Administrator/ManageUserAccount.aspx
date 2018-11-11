<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage/MainSite.Master" AutoEventWireup="true" CodeBehind="ManageUserAccount.aspx.cs" Inherits="OAS.Views.Administrator.ManageUserAccount" %>

<asp:Content ID="head" ContentPlaceHolderID="head" runat="server">

    <link rel="stylesheet" id="bootstrapCss">
    <link rel="stylesheet" href="https://cdn.datatables.net/1.10.19/css/dataTables.bootstrap4.min.css">

    <script src="https://code.jquery.com/jquery-3.3.1.js"></script>
    <script src="https://cdn.datatables.net/1.10.19/js/jquery.dataTables.min.js"></script>
    <script src="https://cdn.datatables.net/1.10.19/js/dataTables.bootstrap4.min.js"></script>

    <script>
        document.getElementById("bootstrapCss").href = path + "/Content/css/bootstrap.css";

        $(document).ready(function () {
            $('#contentBody_datatables').DataTable({
                "pageLength": 5,
                "lengthChange": false,
                "columnDefs": [
                    { "width": "10%", "targets": 3 }
                ],
                "columnDefs": [
                    { "orderable": false, className: "dt-body-center", "targets": 3 }
                ]

            });
        });
    </script>

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
            dynamicSetHeight(0.77);
            setLeftTriangle();
        });

        $.when(
            $.getScript(path + "/Scripts/js/jquery.device.detector.js"),
            $.Deferred(function (deferred) {
                $(deferred.resolve);
            })
        ).done(function () {
            //place your code here, the scripts are all loaded
            $(window).on('resize', function () {
                var instance = $.fn.deviceDetector;
                if (instance.isDesktop()) {
                    dynamicSetHeight(0.77);
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

        .dataTables_wrapper {
            width: 80%;
            margin: auto;
            margin-top: 80px;
        }

        .reset-parent, #contentBody_datatables_next {
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

            .actionButton + a {
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

        #contentBody_updateDiv {
            display: none !important;
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

<script runat="server">
    List<MembershipUser> AllUsersList = new List<MembershipUser>();
    protected void Page_Load(object sender, EventArgs e)
    {
        foreach (MembershipUser user in Membership.GetAllUsers())
        {
            AllUsersList.Add(user);
        }

        createTable();

        if (!Page.IsPostBack)
        {
        }
    }
    protected void removeUser_OnClick(object sender, EventArgs e)
    {
        LinkButton linkButton = sender as LinkButton;

        Membership.DeleteUser(linkButton.Text);

        Message.Text = linkButton.Text;
        Response.Redirect(Request.RawUrl);
    }
    protected void editUser_OnClick(object sender, EventArgs e)
    {
        HtmlButton button = sender as HtmlButton;
        //Table table;
        //table = (Table)TablePlaceHolder.FindControl("datatables");
        //table.Attributes.CssStyle.Add("display", "none");
        tableDiv.Attributes["style"] = "display:none!important;";
        updateDiv.Attributes["style"] = "display:block!important;height:480px;";

        Message.ForeColor = System.Drawing.Color.Green;
        Message.Text = button.Attributes["value"];

    }
    protected void createTable()
    {
        String[] userRoles;
        String showRoles;
        Table table = new System.Web.UI.WebControls.Table();
        TableRow tableRow;
        TableCell tableCell;
        HtmlGenericControl span; HtmlButton button;
        LinkButton linkButton;

        table.ID = "datatables";
        table.CssClass = "table table-striped table-bordered";
        table.Attributes.CssStyle.Add("width", "100%");

        tableRow = new TableRow();
        tableRow.TableSection = TableRowSection.TableHeader;

        tableCell = new TableCell();
        tableCell.Text = "User ID";
        tableRow.Cells.Add(tableCell);
        tableCell = new TableCell();
        tableCell.Text = "Email";
        tableRow.Cells.Add(tableCell);
        tableCell = new TableCell();
        tableCell.Text = "Role";
        tableRow.Cells.Add(tableCell);
        tableCell = new TableCell();
        tableCell.Text = "Action";
        tableRow.Cells.Add(tableCell);
        table.Rows.Add(tableRow);

        for (int i = 0; i < AllUsersList.Count; i++)
        {
            tableRow = new TableRow();
            tableCell = new TableCell();
            tableCell.Text = AllUsersList[i].UserName;
            tableRow.Cells.Add(tableCell);
            tableCell = new TableCell();
            tableCell.Text = AllUsersList[i].Email;
            tableRow.Cells.Add(tableCell);

            showRoles = "";
            userRoles = Roles.GetRolesForUser(AllUsersList[i].UserName);
            for (int j = 0; j < userRoles.Length; j++)
            {
                showRoles = showRoles + userRoles[j] + ", ";
            }
            if (showRoles.Length != 0)
            {
                showRoles = showRoles.Remove(showRoles.Length - 2, 2);
            }
            tableCell = new TableCell();
            tableCell.Text = showRoles;
            tableRow.Cells.Add(tableCell);

            tableCell = new TableCell();
            span = new HtmlGenericControl("span");
            span.InnerHtml = "clear";
            span.Attributes["class"] = "material-icons hvr-icon";

            linkButton = new LinkButton();
            linkButton.ID = "removeUser" + i;
            linkButton.Text = AllUsersList[i].UserName;
            linkButton.CssClass = "actionButton hvr-icon-pulse";
            linkButton.Controls.Add(span);
            // Register the event-handling method for the OnClientClick event. 
            linkButton.Click += new EventHandler(this.removeUser_OnClick);
            linkButton.OnClientClick = "return confirm('Are you sure to delete this user " + AllUsersList[i].UserName + "?');";
            tableCell.Controls.Add(linkButton);

            span = new HtmlGenericControl("span");
            span.InnerHtml = "edit";
            span.Attributes["class"] = "material-icons hvr-icon";

            button = new HtmlButton();

            button.ID = "editUser" + i;
            button.Attributes["value"] = AllUsersList[i].UserName;
            button.Attributes["class"] = "actionButton hvr-icon-pulse";
            button.Controls.Add(span);
            // Register the event-handling method for the OnClientClick event. 
            button.ServerClick += new EventHandler(editUser_OnClick);
            tableCell.Controls.Add(button);

            tableRow.Cells.Add(tableCell);

            table.Rows.Add(tableRow);

        }
        TablePlaceHolder.Controls.Add(table);
    }
</script>

<asp:Content ID="contentBody" ContentPlaceHolderID="contentBody" runat="server">
    <!-- content start -->
    <div id="bodyContent" style="background-color: #fff; border-bottom: 1px solid rgb(205,205,205); z-index: 1">
        <div class="content">
            <div id="tableDiv" runat="server" class="reset-parent bootstrap-iso fade">
                <!-- Any HTML here will be styled with Bootstrap CSS -->
                <asp:PlaceHolder ID="TablePlaceHolder" runat="server"></asp:PlaceHolder>
                <asp:Label ID="Message" runat="server"></asp:Label>
            </div>

            <div id="updateDiv" runat="server" class="box-wrapper fade">
                <div class="box" style="height: 83%">

                    <script runat="server">


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
                                //statusMessage.Text = GetErrorMessage(ex.StatusCode);
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

</asp:Content>

<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage/MainSite.Master" AutoEventWireup="true" CodeBehind="UsersAndRoles.aspx.cs" Inherits="OAS.Views.Administrator.UsersAndRoles" %>

<%@ Import Namespace="System.Data" %>
<%@ Import Namespace="System.Data.SqlClient" %>

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
            dynamicSetHeight(0.7);
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
                    dynamicSetHeight(0.7);
                }
                if ($(window).outerWidth() > 958) {
                    setLeftTriangle();
                } else {
                    document.getElementById("triangle-div").style.left = "";
                }
            });
        });

        function searchUserFunction() {
            var input, filter, table, tr, td, i;
            input = document.getElementById("searchUserIDInput");
            filter = input.value.toUpperCase();
            table = document.getElementsByClassName("roleTable");
            tr = table[0].getElementsByTagName("tr");
            for (i = 1; i < tr.length; i++) {
                td = tr[i].getElementsByTagName("td")[1];
                if (td) {
                    if (td.innerHTML.toUpperCase().indexOf(filter) > -1) {
                        tr[i].style.display = "";
                    } else {
                        tr[i].style.display = "none";
                    }
                }
            }
        }
        function searchRoleFunction() {
            var input, filter, table, tr, td, innerTD, i, j;
            input = document.getElementById("searchRoleInput");
            filter = input.value.toUpperCase();
            table = document.getElementsByClassName("roleTable");
            tr = table[0].getElementsByTagName("tr");

            td = tr[0].getElementsByTagName("td");

            for (i = 2; i < td.length; i++) {
                if (td[i]) {
                    if (td[i].innerHTML.toUpperCase().indexOf(filter) > -1) {
                        for (j = 0; j < tr.length; j++) {
                            innerTD = tr[j].getElementsByTagName("td")[i];
                            innerTD.style.display = "";
                        }
                    } else {

                        for (j = 0; j < tr.length; j++) {
                            innerTD = tr[j].getElementsByTagName("td")[i];
                            innerTD.style.display = "none";
                        }
                    }
                }

            }
        }

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

        .roleDiv::-webkit-scrollbar {
            width: 5px;
            height: 5px;
        }

        .roleDiv::-webkit-scrollbar-track {
            -webkit-box-shadow: inset 0 0 5px rgba(0,0,0,0);
            background-color: white;
        }

        .roleDiv::-webkit-scrollbar-thumb {
            background-color: rgba(100,100,100,1);
            outline: 1px solid slategrey;
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

        #contentBody_ActionStatusByUser, #contentBody_ActionStatusByRole {
            font-weight: 500;
            font-size: 11px;
            letter-spacing: 0.5px;
            display: inline-block;
            margin-top: 15px;
        }

        #bodyContent {
            background-color: #fff;
            z-index: 1;
        }

        #searchUserIDInput, #searchRoleInput {
            background-image: url('../../../Content/images/icons_logos/searchicon.png');
            background-size: 12px;
            background-position: 10px 6px;
            background-repeat: no-repeat;
            width: 65%;
            font-size: 12px;
            padding: 5px 10px 5px 40px;
            display: block;
            border: none;
            border-bottom: 1px solid #ddd;
            margin: 0;
            outline: none;
            -webkit-transition: all 1s linear;
            transition: all 1s linear;
        }

        #searchRoleInput {
            float: right;
        }

            #searchUserIDInput:focus, #searchRoleInput:focus {
                border-color: rgb(102,102,255);
            }

        .tableContent {
            table-layout: fixed;
            width: 80%;
            height: 280px;
            margin: auto;
            margin-top: 100px;
        }

            .tableContent td {
                background-color: rgba(240,240,240,1);
            }

        .roleDiv {
            height: 200px;
            display: block;
            width: 100%;
            overflow: scroll;
            white-space: nowrap;
            border: 1px solid #ddd;
        }

        .roleTable {
            border-width: 0;
            border-collapse: collapse;
            text-align: left;
            width: 100%;
        }

            .roleTable tr {
                border-bottom: 1px solid #ddd;
            }

            .roleTable td, .roleTable thead td {
                padding: 10px;
            }

                .roleTable thead td, .roleTable thead td:hover {
                    background-color: #fff;
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

    List<MembershipUser> AllUsersList = new List<MembershipUser>();
    String[] AllRolesList;
    protected void Page_Load(object sender, EventArgs e)
    {
        foreach (MembershipUser user in Membership.GetAllUsers())
        {
            AllUsersList.Add(user);
        }

        AllRolesList = Roles.GetAllRoles();

        createTable();

        if (!Page.IsPostBack)
        {
            // Check the selected user's roles 
            CheckRolesForUser(AllUsersList, AllRolesList);
        }
    }

    protected void RoleUserCheckBox_CheckChanged(object sender, EventArgs e)
    {
        // Reference the CheckBox that raised this event 
        CheckBox CheckBox = sender as CheckBox;
        String[] userRole = CheckBox.Text.Split(',');
        string selectedUserName = "";
        string selectedroleName = "";
        if (CheckBox.ID.Contains("RoleUser"))
        {
            selectedUserName = userRole[0];
            selectedroleName = userRole[1];
        }
        else if (CheckBox.ID.Contains("Role"))
        {
            selectedroleName = userRole[0];
        }
        else
        {
            selectedUserName = userRole[0];
        }
        // Determine if we need to add or remove the user from this role 
        try
        {
            if (CheckBox.Checked)
            {
                ActionStatusByUser.ForeColor = System.Drawing.Color.Green;
                if (CheckBox.ID.Contains("RoleUser"))
                {
                    // Add the user to the role 
                    Roles.AddUserToRole(selectedUserName, selectedroleName);
                    // Display a status message 
                    ActionStatusByUser.Text = string.Format("User {0} was added to role {1}.", selectedUserName, selectedroleName);
                }
                else if (CheckBox.ID.Contains("Role"))
                {
                    for (int i = 0; i < AllUsersList.Count; i++)
                    {
                        if (!Roles.IsUserInRole(AllUsersList[i].UserName, selectedroleName))
                        {
                            Roles.AddUserToRole(AllUsersList[i].UserName, selectedroleName);
                            ActionStatusByUser.Text = string.Format("All users were added to role {0}.", selectedroleName);
                        }
                    }
                }
                else
                {
                    for (int i = 0; i < AllRolesList.Length; i++)
                    {
                        if (!Roles.IsUserInRole(selectedUserName, AllRolesList[i]))
                        {
                            Roles.AddUserToRole(selectedUserName, AllRolesList[i]);
                            ActionStatusByUser.Text = string.Format("User {0} was added to all roles.", selectedUserName);
                        }
                    }
                }
            }
            else
            {
                ActionStatusByUser.ForeColor = System.Drawing.Color.Red;
                if (CheckBox.ID.Contains("RoleUser"))
                {
                    // Remove the user from the role 
                    Roles.RemoveUserFromRole(selectedUserName, selectedroleName);
                    // Display a status message 
                    ActionStatusByUser.Text = string.Format("User {0} was removed from role {1}.", selectedUserName, selectedroleName);
                }
                else if (CheckBox.ID.Contains("Role"))
                {
                    for (int i = 0; i < AllUsersList.Count; i++)
                    {
                        if (Roles.IsUserInRole(AllUsersList[i].UserName, selectedroleName))
                        {
                            Roles.RemoveUserFromRole(AllUsersList[i].UserName, selectedroleName);
                            ActionStatusByUser.Text = string.Format("All users were removed from role {0}.", selectedroleName);
                        }
                    }
                }
                else
                {
                    for (int i = 0; i < AllRolesList.Length; i++)
                    {
                        if (Roles.IsUserInRole(selectedUserName, AllRolesList[i]))
                        {
                            Roles.RemoveUserFromRole(selectedUserName, AllRolesList[i]);
                            ActionStatusByUser.Text = string.Format("User {0} was removed from all roles.", selectedUserName);
                        }
                    }
                }
            }
            //reset the checkbox
            CheckRolesForUser(AllUsersList, AllRolesList);
        }
        catch (Exception ex)
        {
            CheckRolesForUser(AllUsersList, AllRolesList);
        }
    }

    private void CheckRolesForUser(List<MembershipUser> UserList, String[] RoleList)
    {
        // Determine what roles the selected user belongs to 
        string[] UsersRoles; CheckBox RoleUserCheckBox, RoleCheckBox, UserCheckBox;

        for (int i = 0; i < UserList.Count; i++)
        {
            UsersRoles = Roles.GetRolesForUser(UserList[i].UserName);
            for (int j = 0; j < RoleList.Length; j++)
            {
                RoleUserCheckBox = (CheckBox)RoleTable.FindControl("RoleUserCheckBox" + i + j);
                if (UsersRoles.Contains<string>(RoleList[j]))
                {
                    RoleUserCheckBox.Checked = true;
                    RoleUserCheckBox.ToolTip = "Remove user " + UserList[i].ToString() + " from " + RoleList[j].ToString();
                }
                else
                {
                    RoleUserCheckBox.Checked = false;
                    RoleUserCheckBox.ToolTip = "Add user " + UserList[i].ToString() + " to " + RoleList[j].ToString();
                }
            }
        }
        for (int i = 0; i < RoleList.Length; i++)
        {
            RoleCheckBox = (CheckBox)RoleTable.FindControl("RoleCheckBox" + i);
            if (UserList.Count == Roles.GetUsersInRole(RoleList[i]).Length && Roles.GetUsersInRole(RoleList[i]).Length != 0)
            {
                RoleCheckBox.Checked = true;
                RoleCheckBox.ToolTip = "Remove all users from " + RoleList[i].ToString();
            }
            else
            {
                RoleCheckBox.Checked = false;
                RoleCheckBox.ToolTip = "Add all users to " + RoleList[i].ToString();
            }
        }
        for (int i = 0; i < UserList.Count; i++)
        {
            UserCheckBox = (CheckBox)RoleTable.FindControl("UserCheckBox" + i);
            if (RoleList.Length == Roles.GetRolesForUser(UserList[i].UserName).Length)
            {
                UserCheckBox.Checked = true;
                UserCheckBox.ToolTip = "Remove user " + UserList[i].ToString() + " from all roles";
            }
            else
            {
                UserCheckBox.Checked = false;
                UserCheckBox.ToolTip = "Add user " + UserList[i].ToString() + " to all roles";
            }
        }
    }

    private void createTable()
    {
        Table table = new System.Web.UI.WebControls.Table();
        TableRow tableRow;
        TableCell tableCell;
        CheckBox checkBox;

        tableRow = new TableRow();
        tableRow.TableSection = TableRowSection.TableHeader;

        tableCell = new TableCell();
        tableCell.Text = "No.";
        tableRow.Cells.Add(tableCell);

        tableCell = new TableCell();
        tableCell.Text = "User ID";
        tableRow.Cells.Add(tableCell);

        for (int i = 0; i < AllRolesList.Length; i++)
        {
            tableCell = new TableCell();
            checkBox = new CheckBox();
            checkBox.ID = "RoleCheckBox" + i;
            checkBox.Text = AllRolesList[i];
            checkBox.AutoPostBack = true;
            // Register the event-handling method for the CheckedChanged event. 
            checkBox.CheckedChanged += new EventHandler(this.RoleUserCheckBox_CheckChanged);
            tableCell.Controls.Add(checkBox);
            tableRow.Cells.Add(tableCell);
        }
        table.Rows.Add(tableRow);

        for (int i = 0; i < AllUsersList.Count; i++)
        {
            tableRow = new TableRow();

            tableCell = new TableCell();
            tableCell.Text = (i + 1).ToString() + ".";
            tableRow.Cells.Add(tableCell);

            tableCell = new TableCell();
            checkBox = new CheckBox();
            checkBox.ID = "UserCheckBox" + i;
            checkBox.Text = AllUsersList[i].UserName;
            checkBox.AutoPostBack = true;
            // Register the event-handling method for the CheckedChanged event. 
            checkBox.CheckedChanged += new EventHandler(this.RoleUserCheckBox_CheckChanged);
            tableCell.Controls.Add(checkBox);
            tableRow.Cells.Add(tableCell);

            for (int j = 0; j < AllRolesList.Length; j++)
            {
                tableCell = new TableCell();
                checkBox = new CheckBox();
                checkBox.ID = "RoleUserCheckBox" + i + j;
                checkBox.Text = AllUsersList[i].UserName + "," + AllRolesList[j]; checkBox.LabelAttributes.CssStyle.Add("display", "none");
                checkBox.AutoPostBack = true;
                // Register the event-handling method for the CheckedChanged event. 
                checkBox.CheckedChanged += new EventHandler(this.RoleUserCheckBox_CheckChanged);
                tableCell.Controls.Add(checkBox);
                tableRow.Cells.Add(tableCell);
            }
            table.Rows.Add(tableRow);
        }
        table.CssClass = "roleTable";
        RoleTable.Controls.Add(table);
    }
</script>

<asp:Content ID="contentBody" ContentPlaceHolderID="contentBody" runat="server">
    <!-- content start -->
    <div id="bodyContent">
        <div class="content">
            <table class="tableContent">
                <tr>
                    <td>
                        <h4>Assign Roles to Users</h4>
                        <table style="width: 100%; table-layout: fixed">
                            <tr>
                                <td>
                                    <input type="text" id="searchUserIDInput" onkeyup="searchUserFunction()" placeholder="Search for User..." title="Type in an ID" onkeydown="if (event.keyCode == 13) return false /">
                                </td>
                                <td>
                                    <input type="text" id="searchRoleInput" onkeyup="searchRoleFunction()" placeholder="Search for Role..." title="Type in a role" onkeydown="if (event.keyCode == 13) return false" />
                                </td>
                            </tr>
                        </table>
                        <div id="RoleTable" runat="server" class="roleDiv">
                        </div>
                        <div style="height: 40px; width: 100%;">
                            <asp:Label ID="ActionStatusByUser" runat="server"></asp:Label>
                        </div>
                    </td>
                </tr>
            </table>
        </div>
    </div>
    <!-- content end -->
</asp:Content>

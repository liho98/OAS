using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace OAS.Views.Administrator
{
    public partial class ManageUserAccount : System.Web.UI.Page
    {
        string connectionString = ConfigurationManager.ConnectionStrings["oasDB"].ConnectionString;

        List<MembershipUser> AllUsersList = new List<MembershipUser>();
        protected void Page_Load(object sender, EventArgs e)
        {
            password.Attributes.Add("value", password.Text);
            confirmPassword.Attributes.Add("value", confirmPassword.Text);

            foreach (MembershipUser user in Membership.GetAllUsers())
            {
                AllUsersList.Add(user);
            }
            Message.Text = (String)Request.QueryString["Message"];
            createTable();

            if (!Page.IsPostBack)
            {
                Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "RegisteredSetHeightScript", setHeight("0.77"), true);
                BindRolesToRolesList();
            }
            else
            {
                if (CalendarUserControl.IsVisible)
                {
                    Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "RegisteredSetHeightScript", setHeight("1.1"), true);
                }
            }
        }
        protected void removeUser_OnClick(object sender, EventArgs e)
        {
            LinkButton linkButton = sender as LinkButton;

            Membership.DeleteUser(linkButton.Text);

            Message.Text = "Successfully deleted user " + linkButton.Text + ".";

            Response.Redirect(Request.Url.GetLeftPart(UriPartial.Path) + "?Message=" + Message.Text);
        }
        protected void editUser_OnClick(object sender, EventArgs e)
        {
            Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "RegisteredSetHeightScript", setHeight("1.1"), true);
            HtmlButton button = sender as HtmlButton;
            getProfile(button.Attributes["value"]);

            tableDiv.Attributes["style"] = "display:none!important;";
            updateDiv.Attributes["style"] = "display:block!important;height:685px;";

            Message.ForeColor = System.Drawing.Color.Green;
        }
        private String setHeight(String height)
        {
            return "var path = window.location.protocol + \"//\" + window.location.host;" +
            "$.when(" +
                "$.getScript(path + \"/Scripts/js/dynamicSetHeight.js\")," +
                "$.Deferred(function (deferred) {" +
                    "$(deferred.resolve);" +
                "})" +
            ").done(function () {" +

                "dynamicSetHeight(" + height + ");" +
                "setLeftTriangle();" +
            "});" +

            "$.when(" +
                "$.getScript(path + \"/Scripts/js/jquery.device.detector.js\")," +
                "$.Deferred(function (deferred) {" +
                    "$(deferred.resolve);" +
                "})" +
            ").done(function () {" +

                "$(window).on('resize', function () {" +
                    "var instance = $.fn.deviceDetector;" +
                    "if (instance.isDesktop()) {" +
                        "dynamicSetHeight(" + height + ");" +
                    "}" +
                    "if ($(window).outerWidth() > 958) {" +
                        "setLeftTriangle();" +
                    "} else {" +
                        "document.getElementById(\"triangle-div\").style.left = \"\";" +
                    "}" +
                "});" +
            "});";
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
        private void BindRolesToRolesList()
        {
            // Get all of the roles 
            RolesList.DataSource = Roles.GetAllRoles();
            RolesList.DataBind();

            RolesList.Attributes.CssStyle.Add("color", "rgba(0,0,0,0.6)");
            RolesList.Items[0].Attributes.Add("disabled", "disabled");
            RolesList.Items[0].Attributes.CssStyle.Add("background-color", "rgba(200,200,200,0.6)");
        }
        protected void RolesList_OnSelectedChange(object sender, EventArgs e)
        {
            if (RolesList.SelectedValue == "Students")
            {
                ProgramCode.Visible = true;
                position.Visible = false;
            }
            else
            {
                ProgramCode.Visible = false;
                position.Visible = true;
            }
            Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "RegisteredSetHeightScript", setHeight("1.1"), true);
        }
        protected void CalendarUserControl_OnCalendarVisibilityChanged(object sender, EventArgs e)
        {
            Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "RegisteredSetHeightScript", setHeight("1.1"), true);
        }
        protected void UpdateAccountButton_Click(object sender, EventArgs e)
        {
            Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "RegisteredSetHeightScript", setHeight("1.1"), true);

            try
            {
                MembershipUser user = Membership.GetUser(userID.Text.Substring(9));
                Guid UserId = (Guid)user.ProviderUserKey;
                String username = userID.Text.Substring(9);

                String updateSql = "";
                SqlConnection con = new SqlConnection(connectionString);

                if (RolesList.SelectedValue == "Students")
                {
                    updateSql = "UPDATE [dbo].[UserProfiles] SET FirstName = @FirstName, LastName = @LastName, " +
                        "ContactNo = @ContactNo, Gender = @Gender," +
                        "ProgCode = @ProgCode, " + "Position = NULL, " +
                        "DateOfBirth = @DateOfBirth WHERE UserId = @UserId;";
                }
                else
                {
                    updateSql = "UPDATE [dbo].[UserProfiles] SET FirstName = @FirstName, LastName = @LastName, " +
                        "ContactNo = @ContactNo, Gender = @Gender," +
                        "ProgCode = NULL, " + "Position = @Position, " +
                        "DateOfBirth = @DateOfBirth WHERE UserId = @UserId;";
                }

                user.Email = email.Text;
                Membership.Provider.UpdateUser(user);
                user.ChangePassword(user.ResetPassword(), password.Text);

                Roles.RemoveUserFromRole(username, Roles.GetRolesForUser(username)[0]);
                Roles.AddUserToRole(username, RolesList.SelectedValue);

                con.Open();
                SqlCommand updateCommand = new SqlCommand(updateSql, con);
                updateCommand.Parameters.AddWithValue("@FirstName", firstName.Text);
                updateCommand.Parameters.AddWithValue("@LastName", lastName.Text);
                updateCommand.Parameters.AddWithValue("@ContactNo", contactNo.Text);
                updateCommand.Parameters.AddWithValue("@Gender", gender.SelectedValue.Substring(0, 1));
                updateCommand.Parameters.AddWithValue("@DateOfBirth", CalendarUserControl.SelectedDate);
                updateCommand.Parameters.AddWithValue("@UserId", UserId);
                if (RolesList.SelectedValue == "Students")
                {
                    updateCommand.Parameters.AddWithValue("@ProgCode", ProgramCode.SelectedValue);
                }
                else
                {
                    updateCommand.Parameters.AddWithValue("@Position", position.SelectedValue);
                }
                updateCommand.ExecuteNonQuery();
                con.Close();

                statusMessage.ForeColor = System.Drawing.Color.Green;
                statusMessage.Text = "Successfully updated user " + username + ".";
            }
            catch (Exception ex)
            {
                statusMessage.ForeColor = System.Drawing.Color.Red;
                statusMessage.Text = "Update failed, it may caused by email address already exists or use 8 characters or more for your password, and must contain at least 1 non alphanumeric characters.";
            }
        }
        protected void returnLink_OnClick(object sender, EventArgs e)
        {
            Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "RegisteredSetHeightScript", setHeight("0.77"), true);
            tableDiv.Attributes["style"] = "display:block!important;";
            updateDiv.Attributes["style"] = "display:none!important";
            Response.Redirect(Request.Url.GetLeftPart(UriPartial.Path));
        }
        private void displayDetailList()
        {
            if (RolesList.SelectedValue == "Students")
            {
                ProgramCode.Visible = true;
                position.Visible = false;
            }
            else
            {
                ProgramCode.Visible = false;
                position.Visible = true;
            }
        }
        private void getProfile(String userName)
        {
            //Get the UserId of the just-added user
            Guid UserId = (Guid)(Membership.GetUser(userName)).ProviderUserKey;

            SqlConnection con = new SqlConnection(connectionString);

            string selectSql = "Select FirstName,LastName,Gender,ContactNo,DateOfBirth,Status,Position,ProgCode, Email " +
                    "from [dbo].[UserProfiles] u, [dbo].[aspnet_Membership] m where u.UserId = @UserId and m.UserId = @UserId";

            con.Open();
            SqlCommand sqlCommand = new SqlCommand(selectSql, con);
            sqlCommand.Parameters.AddWithValue("@UserId", UserId);
            SqlDataReader userRecords = sqlCommand.ExecuteReader();

            String[] userRoles;
            userRoles = Roles.GetRolesForUser(userName);

            for (int j = 0; j < RolesList.Items.Count; j++)
            {
                if (RolesList.Items[j].Text == userRoles[0])
                {
                    RolesList.ClearSelection();
                    RolesList.Items[j].Selected = true;
                }
            }
            userRecords.Read();
            userID.Text = "User ID : " + userName;
            firstName.Text = userRecords["FirstName"].ToString();
            lastName.Text = userRecords["LastName"].ToString();
            email.Text = userRecords["Email"].ToString();
            contactNo.Text = userRecords["ContactNo"].ToString();

            for (int i = 0; i < gender.Items.Count; i++)
            {
                if (gender.Items[i].Value.Substring(0, 1) == userRecords["Gender"].ToString())
                {
                    gender.ClearSelection();
                    gender.Items[i].Selected = true;
                }
            }

            if (RolesList.SelectedValue == "Students")
            {
                ProgramCode.Visible = true;
                position.Visible = false;
                for (int i = 0; i < ProgramCode.Items.Count; i++)
                {
                    if (ProgramCode.Items[i].Value == userRecords["ProgCode"].ToString())
                    {
                        ProgramCode.ClearSelection();
                        ProgramCode.Items[i].Selected = true;
                    }
                }
            }
            else
            {
                ProgramCode.Visible = false;
                position.Visible = true;
                for (int i = 0; i < position.Items.Count; i++)
                {
                    if (position.Items[i].Value == userRecords["Position"].ToString())
                    {
                        position.ClearSelection();
                        position.Items[i].Selected = true;
                    }
                }
            }
            CalendarUserControl.SelectedDate = String.Format("{0:yyy-MM-dd}", ((DateTime)userRecords["DateOfBirth"]));
            con.Close();
        }

    }
}
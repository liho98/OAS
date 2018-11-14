using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace OAS.Views.Administrator
{
    public partial class UsersAndRoles : System.Web.UI.Page
    {
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
    }
}
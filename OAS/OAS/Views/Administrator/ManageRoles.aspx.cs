using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace OAS.Views.Administrator
{
    public partial class ManageRoles : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
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

    }
}
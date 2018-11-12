using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace OAS.Views.Administrator
{
    public partial class CreateUserAccount : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                BindRolesToRolesList();

            }
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

        protected string GetErrorMessage(MembershipCreateStatus status)
        {
            switch (status)
            {
                case MembershipCreateStatus.DuplicateUserName:
                    return "User ID already exists. Please enter a different User ID.";

                case MembershipCreateStatus.DuplicateEmail:
                    return "An User ID for that email address already exists. Please enter a different email address.";

                case MembershipCreateStatus.InvalidPassword:
                    return "The password provided is invalid. Please enter a valid password value.";

                case MembershipCreateStatus.InvalidEmail:
                    return "The email address provided is invalid. Please check the value and try again.";

                case MembershipCreateStatus.InvalidAnswer:
                    return "The password retrieval answer provided is invalid. Please check the value and try again.";

                case MembershipCreateStatus.InvalidQuestion:
                    return "The password retrieval question provided is invalid. Please check the value and try again.";

                case MembershipCreateStatus.InvalidUserName:
                    return "The User ID provided is invalid. Please check the value and try again.";

                case MembershipCreateStatus.ProviderError:
                    return "The authentication provider returned an error. Please verify your entry and try again. If the problem persists, please contact your system administrator.";

                case MembershipCreateStatus.UserRejected:
                    return "The user creation request has been canceled. Please verify your entry and try again. If the problem persists, please contact your system administrator.";

                default:
                    return "An unknown error occurred. Please verify your entry and try again. If the problem persists, please contact your system administrator.";
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

    }
}
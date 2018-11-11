using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace OAS.Others
{
    public partial class SignUp : System.Web.UI.Page
    {
        private static string conStr = ConfigurationManager.ConnectionStrings["oasDB"].ConnectionString;
        private SqlConnection con = new SqlConnection(conStr);

        protected void Page_Load(object sender, EventArgs e)
        {
            con.Open();
        }
        protected void SignUpButton_Click(object sender, EventArgs e)
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

                Response.Redirect("Login.aspx");
            }
            catch (MembershipCreateUserException ex)
            {
                invalidDetailsMessage.Text = GetErrorMessage(ex.StatusCode);
            }
            catch (HttpException ex)
            {
                invalidDetailsMessage.Text = ex.Message;
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
    }
}
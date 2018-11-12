using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace OAS
{
    public partial class SignUp : System.Web.UI.Page
    {
        private static string connectionString = ConfigurationManager.ConnectionStrings["oasDB"].ConnectionString;

        protected void Page_Load(object sender, EventArgs e)
        {
            selectBox.Attributes["style"] = "display:block!important;height:390px!important";
            userBox.Attributes["style"] = "display:none!important;";
        }

        protected void studentButton_OnClick(object sender, EventArgs e)
        {
            role.Text = "Students";
            selectBox.Attributes["style"] = "display:none!important;";
            userBox.Attributes["style"] = "display:block!important;height: 685px!important";
        }

        protected void lecturerButton_OnClick(object sender, EventArgs e)
        {
            role.Text = "Lecturers";
            selectBox.Attributes["style"] = "display:none!important;";
            userBox.Attributes["style"] = "display:block!important;height: 685px!important";
        }
        protected void returnLink_OnClick(object sender, EventArgs e)
        {
            invalidDetailsMessage.Text = "";
            selectBox.Attributes["style"] = "display:block!important;height:390px!important";
            userBox.Attributes["style"] = "display:none!important;";
        }
        protected void SignUpButton_Click(object sender, EventArgs e)
        {
            if (PasswordConfirmCompareValidator.IsValid)
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
                    //Get the UserId of the just-added user
                    newUser = Membership.GetUser(userID.Text);
                    Guid newUserId = (Guid)newUser.ProviderUserKey;

                    Roles.AddUserToRole(userID.Text, role.Text);

                    string dafaultImage = Server.MapPath("~/Content/images/userAvatar/default_user.gif");
                    byte[] imageBytes = System.IO.File.ReadAllBytes(dafaultImage);
                    String imageUrl = "data:" + System.IO.Path.GetExtension(dafaultImage) + ";base64," + Convert.ToBase64String(imageBytes);

                    string insertSql = "INSERT INTO UserProfiles(UserId, FirstName, LastName, Gender, ContactNo, DateOfBirth, Status, Position, ProgCode, Image)" +
                        "VALUES(@UserId, @FirstName, @LastName, @Gender, @ContactNo, @DateOfBirth, @Status, @Position, @ProgCode, @Image)";

                    using (SqlConnection con = new SqlConnection(connectionString))
                    {
                        con.Open();
                        char sex;
                        if (gender.SelectedValue == "Male") { sex = 'M'; } else { sex = 'F'; }
                        SqlCommand sqlCommand = new SqlCommand(insertSql, con);
                        sqlCommand.Parameters.AddWithValue("@UserId", newUserId);
                        sqlCommand.Parameters.AddWithValue("@FirstName", firstName.Text);
                        sqlCommand.Parameters.AddWithValue("@LastName", lastName.Text);
                        sqlCommand.Parameters.AddWithValue("@Gender", sex);
                        sqlCommand.Parameters.AddWithValue("@ContactNo", contactNo.Text);
                        sqlCommand.Parameters.AddWithValue("@DateOfBirth", dateOfBirth.Text);
                        sqlCommand.Parameters.AddWithValue("@Status", DBNull.Value);
                        sqlCommand.Parameters.AddWithValue("@Position", position.Text);
                        sqlCommand.Parameters.AddWithValue("@ProgCode", ProgramCode.Text);
                        sqlCommand.Parameters.AddWithValue("@Image", Encoding.Default.GetBytes(imageUrl));
                        sqlCommand.ExecuteNonQuery();
                        con.Close();
                    }
                    invalidDetailsMessage.ForeColor = System.Drawing.Color.Green;
                    invalidDetailsMessage.Text = "You have registered successfully.";
                    selectBox.Attributes["style"] = "display:none!important;";
                    userBox.Attributes["style"] = "display:block!important;height: 685px!important";

                    userID.Text = "";
                    firstName.Text = "";
                    lastName.Text = "";
                    email.Text = "";
                    gender.ClearSelection();
                    contactNo.Text = "";
                    dateOfBirth.Text = "";
                    ProgramCode.ClearSelection();
                    position.ClearSelection();

                }
                catch (MembershipCreateUserException ex)
                {
                    selectBox.Attributes["style"] = "display:none!important;";
                    userBox.Attributes["style"] = "display:block!important;height: 685px!important";
                    invalidDetailsMessage.ForeColor = System.Drawing.Color.Red;
                    invalidDetailsMessage.Text = GetErrorMessage(ex.StatusCode);
                }
                catch (HttpException ex)
                {
                    selectBox.Attributes["style"] = "display:none!important;";
                    userBox.Attributes["style"] = "display:block!important;height: 685px!important";
                    invalidDetailsMessage.ForeColor = System.Drawing.Color.Red;
                    invalidDetailsMessage.Text = ex.Message;
                }
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
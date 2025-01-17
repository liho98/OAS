﻿using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace OAS.Views.Administrator
{
    public partial class CreateUserAccount : System.Web.UI.Page
    {
        private static string connectionString = ConfigurationManager.ConnectionStrings["oasDB"].ConnectionString;

        protected void Page_Load(object sender, EventArgs e)
        {
            password.Attributes.Add("value", password.Text);
            confirmPassword.Attributes.Add("value", confirmPassword.Text);

            if (!Page.IsPostBack)
            {
                BindRolesToRolesList();

            }
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
        }
        protected void CreateAccountButton_Click(object sender, EventArgs e)
        {
            if (Page.IsValid)
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

                    //Get the UserId of the just-added user
                    newUser = Membership.GetUser(userID.Text);
                    Guid newUserId = (Guid)newUser.ProviderUserKey;

                    char sex;
                    string dafaultImage;
                    if (gender.SelectedValue == "Male")
                    {
                        sex = 'M';
                        dafaultImage = Server.MapPath("~/Content/images/userAvatar/defaultAvatarMale.jpg");
                    }
                    else
                    {
                        sex = 'F';
                        dafaultImage = Server.MapPath("~/Content/images/userAvatar/defaultAvatarFemale.jpg");
                    }

                    byte[] imageBytes = System.IO.File.ReadAllBytes(dafaultImage);
                    String imageUrl = "data:" + System.IO.Path.GetExtension(dafaultImage) + ";base64," + Convert.ToBase64String(imageBytes);

                    string insertSql = "INSERT INTO UserProfiles(UserId, FirstName, LastName, Gender, ContactNo, DateOfBirth, Status, Position, ProgCode, Image)" +
                        "VALUES(@UserId, @FirstName, @LastName, @Gender, @ContactNo, @DateOfBirth, @Status, @Position, @ProgCode, @Image)";

                    using (SqlConnection con = new SqlConnection(connectionString))
                    {
                        con.Open();
                        SqlCommand sqlCommand = new SqlCommand(insertSql, con);
                        sqlCommand.Parameters.AddWithValue("@UserId", newUserId);
                        sqlCommand.Parameters.AddWithValue("@FirstName", firstName.Text);
                        sqlCommand.Parameters.AddWithValue("@LastName", lastName.Text);
                        sqlCommand.Parameters.AddWithValue("@Gender", sex);
                        sqlCommand.Parameters.AddWithValue("@ContactNo", contactNo.Text);
                        sqlCommand.Parameters.AddWithValue("@DateOfBirth", CalendarUserControl.SelectedDate);
                        sqlCommand.Parameters.AddWithValue("@Status", "Good");
                        if (RolesList.SelectedValue == "Students")
                        {
                            sqlCommand.Parameters.AddWithValue("@ProgCode", ProgramCode.Text);
                            sqlCommand.Parameters.AddWithValue("@Position", DBNull.Value);
                        }
                        else
                        {
                            sqlCommand.Parameters.AddWithValue("@ProgCode", DBNull.Value);
                            sqlCommand.Parameters.AddWithValue("@Position", position.Text);
                        }
                        sqlCommand.Parameters.AddWithValue("@Image", Encoding.Default.GetBytes(imageUrl));
                        sqlCommand.ExecuteNonQuery();
                        con.Close();
                    }

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
                    return "Use 8 characters or more for your password, and must contain at least 1 non alphanumeric characters.";

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
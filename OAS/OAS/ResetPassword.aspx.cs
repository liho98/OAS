using OAS.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Security.Cryptography;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace OAS
{
    public partial class ResetPassword : System.Web.UI.Page
    {
        private static string connectionString = ConfigurationManager.ConnectionStrings["oasDB"].ConnectionString;

        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void submitButton_Click(object sender, EventArgs e)
        {
            try
            {
                //get the full URL
                Uri myUri = new Uri(Request.Url.AbsoluteUri);
                //get any parameters
                string encryptedStr = HttpUtility.ParseQueryString(myUri.Query).Get("token").Replace(" ", "+");
                string keyStr = HttpUtility.ParseQueryString(myUri.Query).Get("key").Replace(" ", "+");
                string IVStr = HttpUtility.ParseQueryString(myUri.Query).Get("IV").Replace(" ", "+");

                byte[] encrypted = Convert.FromBase64String(encryptedStr);
                byte[] key = Convert.FromBase64String(keyStr);
                byte[] IV = Convert.FromBase64String(IVStr);
                // Decrypt the bytes to a string.
                string roundtrip = AesCrypto.DecryptStringFromBytes_Aes(encrypted, key, IV);
                Guid providerUserKey = Guid.Parse(roundtrip);

                string selectSql = "Select * from [dbo].[Token] " +
                        "where UserId = @UserId;";

                using (SqlConnection con = new SqlConnection(connectionString))
                {
                    con.Open();
                    SqlCommand sqlCommand = new SqlCommand(selectSql, con);
                    sqlCommand.Parameters.AddWithValue("@UserId", providerUserKey);
                    SqlDataReader Token = sqlCommand.ExecuteReader();
                    Token.Read();

                    TimeSpan timeSpan = DateTime.Now.Subtract((DateTime)Token["IssuedDateTime"]);

                    if (timeSpan.TotalDays > 1 || Convert.ToBoolean(Token["IsValid"]) == false)
                    {
                        Message.ForeColor = System.Drawing.Color.Red;
                        Message.Text = "The token is expired or invalid.";
                        con.Close();
                        return;
                    }
                    con.Close();
                }

                MembershipUser user = Membership.GetUser(providerUserKey);

                user.ChangePassword(user.ResetPassword(), newPassword.Text);

                using (SqlConnection con = new SqlConnection(connectionString))
                {
                    con.Open();
                    SqlCommand updateCommand = new SqlCommand("UPDATE [dbo].[Token] SET IsValid = @IsValid WHERE UserId = @UserId", con);
                    updateCommand.Parameters.AddWithValue("@IsValid", Convert.ToByte(false));
                    updateCommand.Parameters.AddWithValue("@UserId", providerUserKey);
                    updateCommand.ExecuteNonQuery();
                    con.Close();
                }

                Message.ForeColor = System.Drawing.Color.Green;
                Message.Text = "You have recover your account successfully. Kindly click here to <a href=\"Login.aspx\" style=\"color:#0067b8;\">&nbsp;&nbsp;login</a>.";

            }
            catch (Exception ex)
            {
                Message.ForeColor = System.Drawing.Color.Red;
                Message.Text = "Use 8 characters or more for your password, and must contain at least 1 non alphanumeric characters. Or your account has been locked. ";
            }
        }
    }
}
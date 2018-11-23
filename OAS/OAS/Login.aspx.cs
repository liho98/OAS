using OAS.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Net.Mail;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace OAS
{
    public partial class Login : System.Web.UI.Page
    {
        private static string connectionString = ConfigurationManager.ConnectionStrings["oasDB"].ConnectionString;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                if (Request.IsAuthenticated && !string.IsNullOrEmpty(Request.QueryString["ReturnUrl"]))
                {
                    // This is an unauthorized, authenticated request...
                    FormsAuthentication.SignOut();
                    Response.Redirect("~/UnauthorizedAccess.aspx?ReturnUrl=" + Request.QueryString["ReturnUrl"]);
                }
            }

        }

        protected void loginButton_Click(object sender, EventArgs e)
        {
            FormsAuthentication.SignOut();

            if (IsValidEmail(emailID.Text))
            {
                string username = Membership.GetUserNameByEmail(emailID.Text);
                if (username != null)
                {
                    emailID.Text = username;
                }
            }

            // Validate the user against the Membership framework user store
            if (Membership.ValidateUser(emailID.Text, password.Text))
            {
                // Log the user into the site
                FormsAuthentication.RedirectFromLoginPage(emailID.Text, rememberMe.Checked);
            }
            // If we reach here, the user's credentials were invalid
            invalidCredentialsMessage.Visible = true;
            emailID.Text = "";
        }

        protected void NextButton_OnClick(object sender, EventArgs e)
        {
            FormsAuthentication.SignOut();
            Page.ClientScript.RegisterStartupScript(this.GetType(), "Registered Script", "swapDiv('forgotPassword');", true);

            if (EmailTextBox.Text != "")
            {
                string username = Membership.GetUserNameByEmail(EmailTextBox.Text);
                if (username != null)
                {
                    String firstName, resetLink;
                    MembershipUser user = Membership.GetUser(username);
                    string original = ((Guid)user.ProviderUserKey).ToString();
                    if (!IsLinkStillValid(original))
                    {
                        SqlConnection con = new SqlConnection(connectionString);

                        string selectSql = "Select FirstName " +
                                "from [dbo].[UserProfiles] Where UserId = '" + original + "'";

                        con.Open();
                        SqlCommand sqlCommand = new SqlCommand(selectSql, con);
                        SqlDataReader userRecords = sqlCommand.ExecuteReader();
                        userRecords.Read();
                        firstName = userRecords["FirstName"].ToString();
                        con.Close();

                        // Create a new instance of the AesCryptoServiceProvider
                        // class.  This generates a new key and initialization 
                        // vector (IV).
                        using (AesCryptoServiceProvider myAes = new AesCryptoServiceProvider())
                        {
                            // Encrypt the string to an array of bytes.
                            byte[] encrypted = AesCrypto.EncryptStringToBytes_Aes(original, myAes.Key, myAes.IV);

                            resetLink = Request.Url.Scheme + "://" + Request.Url.Authority + "/ResetPassword.aspx?token=" +
                                        Convert.ToBase64String(encrypted, Base64FormattingOptions.None) + "&key=" + Convert.ToBase64String(myAes.Key, Base64FormattingOptions.None) +
                                        "&IV=" + Convert.ToBase64String(myAes.IV, Base64FormattingOptions.None);
                            try
                            {
                                SendEmailService("Reset OAS Account Password", ResetPassEmailBodyHtml(firstName, resetLink), EmailTextBox.Text);

                                StatusLabel.ForeColor = System.Drawing.Color.Green;
                                StatusLabel.Text = "Thanks! If there's an account associated with this email, we'll send the password reset instructions immediately.";

                                string insertSql = "INSERT INTO Token(UserId, AccessToken, IssuedDateTime, IsValid)" +
                                    "VALUES(@UserId, @AccessToken, @IssuedDateTime, @IsValid)";

                                using (con = new SqlConnection(connectionString))
                                {
                                    con.Open();
                                    sqlCommand = new SqlCommand(insertSql, con);
                                    sqlCommand.Parameters.AddWithValue("@UserId", user.ProviderUserKey);
                                    sqlCommand.Parameters.AddWithValue("@AccessToken", Convert.ToBase64String(encrypted, Base64FormattingOptions.None));
                                    sqlCommand.Parameters.AddWithValue("@IssuedDateTime", DateTime.Now);
                                    sqlCommand.Parameters.AddWithValue("@IsValid", Convert.ToByte(true));
                                    sqlCommand.ExecuteNonQuery();
                                    con.Close();
                                }
                            }
                            catch (Exception ex)
                            {
                                StatusLabel.Text = ex.ToString();
                            }

                        }
                    }
                    else
                    {
                        StatusLabel.ForeColor = System.Drawing.Color.Red;
                        StatusLabel.Text = "A Reset link is send, please kindly check your Email.";
                    }
                }
                else
                {
                    StatusLabel.ForeColor = System.Drawing.Color.Red;
                    StatusLabel.Text = "Please enter the Email Address.";
                }
            }
        }

        private bool IsLinkStillValid(String userId)
        {
            string selectSql = "Select * from [dbo].[Token] " +
                    "where UserId = '" + userId + "'";
            bool returnVal;

            using (SqlConnection con = new SqlConnection(connectionString))
            {
                con.Open();
                SqlCommand sqlCommand = new SqlCommand(selectSql, con);
                SqlDataReader Token = sqlCommand.ExecuteReader();


                if (Token.Read() == true)
                {
                    TimeSpan timeSpan = DateTime.Now.Subtract((DateTime)Token["IssuedDateTime"]);

                    if (timeSpan.TotalDays > 1 || Convert.ToBoolean(Token["IsValid"]) == false)
                    {
                        returnVal = false;
                    }
                    else
                    {
                        returnVal = true;
                    }
                }
                else
                {
                    returnVal = false;
                }
                con.Close();
            }
            return returnVal;
        }

        private bool IsValidEmail(string email)
        {
            return new EmailAddressAttribute().IsValid(email);
        }

        private void SendEmailService(String subject, String body, String RecipientEmail)
        {
            MailMessage mailMessage = new MailMessage()
            {
                Subject = subject,
                Body = body,
                IsBodyHtml = true,
            };
            mailMessage.To.Add(new MailAddress(RecipientEmail));
            SmtpClient smtpClient = new SmtpClient();
            smtpClient.SendAsync(mailMessage, null);
        }

        private string ResetPassEmailBodyHtml(String firstName, String resetLink)
        {
            return "<!DOCTYPE html PUBLIC \"-//W3C//DTD XHTML 1.0 Transitional//EN\" \"http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd\"><html xmlns:v=\"urn:schemas-microsoft-com:vml\"><head> <meta http-equiv=\"Content-Type\" content=\"text/html; charset=UTF-8\" /> <meta name=\"viewport\" content=\"width=device-width; initial-scale=1.0; maximum-scale=1.0;\" /> <meta name=\"viewport\" content=\"width=600,initial-scale = 2.3,user-scalable=no\"/> <!--[if !mso]><!-- --> <link href=\'https://fonts.googleapis.com/css?family=Work+Sans:300,400,500,600,700\' rel=\"stylesheet\"/> <link href=\'https://fonts.googleapis.com/css?family=Quicksand:300,400,700\' rel=\"stylesheet\"/> <!--<![endif]--> <title>OAS</title> <style type=\"text/css\"> body { width: 100%; background-color: #ffffff; margin: 0; padding: 0; -webkit-font-smoothing: antialiased; mso-margin-top-alt: 0px; mso-margin-bottom-alt: 0px; mso-padding-alt: 0px 0px 0px 0px; } p, h1, h2, h3, h4 { margin-top: 0; margin-bottom: 0; padding-top: 0; padding-bottom: 0; } span.preheader { display: none; font-size: 1px; } html { width: 100%; } table { font-size: 14px; border: 0; } /* ----------- responsivity ----------- */ @media only screen and (max-width: 640px) { /*------ top header ------ */ .main-header { font-size: 20px !important; } .main-section-header { font-size: 28px !important; } .show { display: block !important; } .hide { display: none !important; } .align-center { text-align: center !important; } .no-bg { background: none !important; } /*----- main image -------*/ .main-image img { width: 440px !important; height: auto !important; } /* ====== divider ====== */ .divider img { width: 440px !important; } /*-------- container --------*/ .container590 { width: 440px !important; } .container580 { width: 400px !important; } .main-button { width: 220px !important; } /*-------- secions ----------*/ .section-img img { width: 320px !important; height: auto !important; } .team-img img { width: 100% !important; height: auto !important; } } @media only screen and (max-width: 479px) { /*------ top header ------ */ .main-header { font-size: 18px !important; } .main-section-header { font-size: 26px !important; } /* ====== divider ====== */ .divider img { width: 280px !important; } /*-------- container --------*/ .container590 { width: 280px !important; } .container590 { width: 280px !important; } .container580 { width: 260px !important; } /*-------- secions ----------*/ .section-img img { width: 280px !important; height: auto !important; } } </style> <!--[if gte mso 9]><style type=”text/css”> body { font-family: arial, sans-serif!important; } </style> <![endif]--></head><body class=\"respond\" leftmargin=\"0\" topmargin=\"0\" marginwidth=\"0\" marginheight=\"0\"> <!-- pre-header --> <table style=\"display: none!important;\"> <tr> <td> <div style=\"overflow: hidden; display: none; font-size: 1px; color: #ffffff; line-height: 1px; font-family: Arial; maxheight: 0px; max-width: 0px; opacity: 0;\"> Welcome to MDB! </div> </td> </tr> </table> <!-- pre-header end --> <!-- header --> <table border=\"0\" width=\"100%\" cellpadding=\"0\" cellspacing=\"0\" bgcolor=\"ffffff\"> <tr> <td align=\"center\"> <table border=\"0\" align=\"center\" width=\"590\" cellpadding=\"0\" cellspacing=\"0\" class=\"container590\"> <tr> <td height=\"25\" style=\"font-size: 25px; line-height: 25px;\">&nbsp;</td> </tr> <tr> <td align=\"center\"> <table border=\"0\" align=\"center\" width=\"590\" cellpadding=\"0\" cellspacing=\"0\" class=\"container590\"> <tr> <td align=\"center\" height=\"70\" style=\"height: 70px;\"> <a href=\"\" style=\"display: block; border-style: none !important; border: 0 !important;\"> <img width=\"100\" border=\"0\" style=\"display: block; width: 100px;\" src=\"http://funkyimg.com/i/2Nc5L.png\" alt=\"\" /></a> </td> </tr> <tr> <td align=\"center\"> <table width=\"360 \" border=\"0\" cellpadding=\"0\" cellspacing=\"0\" style=\"border-collapse: collapse; mso-table-lspace: 0pt; mso-table-rspace: 0pt;\" class=\"container590 hide\"> <tr> <td width=\"120\" align=\"center\" style=\"font-size: 14px; font-family: \'Work Sans\', Calibri, sans-serif; line-height: 24px;\"> <a href=\"\" style=\"color: #312c32; text-decoration: none;\"></a> </td> <td width=\"120\" align=\"center\" style=\"font-size: 14px; font-family: \'Work Sans\', Calibri, sans-serif; line-height: 24px;\"> <a href=\"\" style=\"color: #312c32; text-decoration: none;\"></a> </td> <td width=\"120\" align=\"center\" style=\"font-size: 14px; font-family: \'Work Sans\', Calibri, sans-serif; line-height: 24px;\"> <a href=\"\" style=\"color: #312c32; text-decoration: none;\"></a> </td> </tr> </table> </td> </tr> </table> </td> </tr> <tr> <td height=\"25\" style=\"font-size: 25px; line-height: 25px;\">&nbsp;</td> </tr> </table> </td> </tr> </table> <!-- end header --> <!-- big image section --> <table border=\"0\" width=\"100%\" cellpadding=\"0\" cellspacing=\"0\" bgcolor=\"ffffff\" class=\"bg_color\"> <tr> <td align=\"center\"> <table border=\"0\" align=\"center\" width=\"590\" cellpadding=\"0\" cellspacing=\"0\" class=\"container590\"> <tr> <td align=\"center\" style=\"color: #343434; font-size: 24px; font-family: Quicksand, Calibri, sans-serif; font-weight: 700; letter-spacing: 3px; line-height: 35px;\" class=\"main-header\"> <!-- section text ======--> <div style=\"line-height: 35px\"> Reset Your <span style=\"color: #5caad2;\">Password</span> </div> </td> </tr> <tr> <td height=\"10\" style=\"font-size: 10px; line-height: 10px;\">&nbsp;</td> </tr> <tr> <td align=\"center\"> <table border=\"0\" width=\"40\" align=\"center\" cellpadding=\"0\" cellspacing=\"0\" bgcolor=\"eeeeee\"> <tr> <td height=\"2\" style=\"font-size: 2px; line-height: 2px;\">&nbsp;</td> </tr> </table> </td> </tr> <tr> <td height=\"20\" style=\"font-size: 20px; line-height: 20px;\">&nbsp;</td> </tr> <tr> <td align=\"left\"> <table border=\"0\" width=\"590\" align=\"center\" cellpadding=\"0\" cellspacing=\"0\" class=\"container590\"> <tr> <td align=\"left\" style=\"color: #888888; font-size: 16px; font-family: \'Work Sans\', Calibri, sans-serif; line-height: 24px;\"> <!-- section text ======--> <p style=\"line-height: 24px; margin-bottom: 15px;\"> " + firstName + ", </p> <p style=\"line-height: 24px; margin-bottom: 15px;\"> You\'re receiving this e-mail because you requested a password reset for your OAS account. </p> <p style=\"line-height: 24px; margin-bottom: 20px;\"> Please tap the button below to choose a new password. </p> <table border=\"0\" align=\"center\" width=\"180\" cellpadding=\"0\" cellspacing=\"0\" bgcolor=\"5caad2\" style=\"margin-bottom: 20px;\"> <tr> <td height=\"10\" style=\"font-size: 10px; line-height: 10px;\">&nbsp;</td> </tr> <tr> <td align=\"center\" style=\"color: #ffffff; font-size: 14px; font-family: \'Work Sans\', Calibri, sans-serif; line-height: 22px; letter-spacing: 2px;\"> <!-- main section button --> <div style=\"line-height: 22px;\"> <a href=\"" + resetLink + "\" style=\"color: #ffffff; text-decoration: none;\">RESET ACCOUNT</a> </div> </td> </tr> <tr> <td height=\"10\" style=\"font-size: 10px; line-height: 10px;\">&nbsp;</td> </tr> </table> <br /> <p style=\"line-height: 24px\"> If you didn\'t request a password reset, let us know.<br/> The OAS team </p> </td> </tr> </table> </td> </tr> </table> </td> </tr> <tr> <td height=\"40\" style=\"font-size: 40px; line-height: 40px;\">&nbsp;</td> </tr> </table> <!-- end section --> <!-- contact section --> <table border=\"0\" width=\"100%\" cellpadding=\"0\" cellspacing=\"0\" bgcolor=\"ffffff\" class=\"bg_color\"> <tr> <td height=\"60\" style=\"font-size: 60px; line-height: 60px;\">&nbsp;</td> </tr> <tr> <td align=\"center\"> <table border=\"0\" align=\"center\" width=\"590\" cellpadding=\"0\" cellspacing=\"0\" class=\"container590 bg_color\"> <tr> <td align=\"center\"> <table border=\"0\" align=\"center\" width=\"590\" cellpadding=\"0\" cellspacing=\"0\" class=\"container590 bg_color\"> <tr> <td> <table border=\"0\" width=\"300\" align=\"left\" cellpadding=\"0\" cellspacing=\"0\" style=\"border-collapse: collapse; mso-table-lspace: 0pt; mso-table-rspace: 0pt;\" class=\"container590\"> <tr> <td height=\"25\" style=\"font-size: 25px; line-height: 25px;\">&nbsp;</td> </tr> <tr> <td align=\"left\" style=\"color: #888888; font-size: 14px; font-family: \'Work Sans\', Calibri, sans-serif; line-height: 23px;\" class=\"text_color\"> <div style=\"color: #333333; font-size: 14px; font-family: \'Work Sans\', Calibri, sans-serif; font-weight: 600; mso-line-height-rule: exactly; line-height: 23px;\"> Email us: <br /> <a href=\"mailto:\" style=\"color: #888888; font-size: 14px; font-family: \'Hind Siliguri\', Calibri, Sans-serif; font-weight: 400;\">OAS.edu.my@gmail.com</a> </div> </td> </tr> </table> <table border=\"0\" width=\"2\" align=\"left\" cellpadding=\"0\" cellspacing=\"0\" style=\"border-collapse: collapse; mso-table-lspace: 0pt; mso-table-rspace: 0pt;\" class=\"container590\"> <tr> <td width=\"2\" height=\"10\" style=\"font-size: 10px; line-height: 10px;\"></td> </tr> </table> <table border=\"0\" width=\"200\" align=\"right\" cellpadding=\"0\" cellspacing=\"0\" style=\"border-collapse: collapse; mso-table-lspace: 0pt; mso-table-rspace: 0pt;\" class=\"container590\"> <tr> <td class=\"hide\" height=\"45\" style=\"font-size: 45px; line-height: 45px;\">&nbsp;</td> </tr> <tr> <td height=\"15\" style=\"font-size: 15px; line-height: 15px;\">&nbsp;</td> </tr> <tr> <td> <table border=\"0\" align=\"right\" cellpadding=\"0\" cellspacing=\"0\"> <tr> <td> <a href=\"https://www.facebook.com/liho.98\" style=\"display: block; border-style: none !important; border: 0 !important;\"> <img width=\"24\" border=\"0\" style=\"display: block;\" src=\"http://i.imgur.com/Qc3zTxn.png\" alt=\"\"></a> </td> <td>&nbsp;&nbsp;&nbsp;&nbsp;</td> <td> <a href=\"https://twitter.com/liho_98\" style=\"display: block; border-style: none !important; border: 0 !important;\"> <img width=\"24\" border=\"0\" style=\"display: block;\" src=\"http://i.imgur.com/RBRORq1.png\" alt=\"\"></a> </td> <td>&nbsp;&nbsp;&nbsp;&nbsp;</td> <td> <a href=\"https://plus.google.com/100956812200203645080\" style=\"display: block; border-style: none !important; border: 0 !important;\"> <img width=\"24\" border=\"0\" style=\"display: block;\" src=\"http://i.imgur.com/Wji3af6.png\" alt=\"\"></a> </td> </tr> </table> </td> </tr> </table> </td> </tr> </table> </td> </tr> </table> </td> </tr> <tr> <td height=\"60\" style=\"font-size: 60px; line-height: 60px;\">&nbsp;</td> </tr> </table> <!-- end section --> <!-- footer ====== --> <table border=\"0\" width=\"100%\" cellpadding=\"0\" cellspacing=\"0\" bgcolor=\"f4f4f4\"> <tr> <td height=\"25\" style=\"font-size: 25px; line-height: 25px;\">&nbsp;</td> </tr> <tr> <td align=\"center\"> <table border=\"0\" align=\"center\" width=\"590\" cellpadding=\"0\" cellspacing=\"0\" class=\"container590\"> <tr> <td> <table border=\"0\" align=\"left\" cellpadding=\"0\" cellspacing=\"0\" style=\"border-collapse: collapse; mso-table-lspace: 0pt; mso-table-rspace: 0pt;\" class=\"container590\"> <tr> <td align=\"left\" style=\"color: #aaaaaa; font-size: 14px; font-family: \'Work Sans\', Calibri, sans-serif; line-height: 24px;\"> <div style=\"line-height: 24px;\"> <span style=\"color: #333333;\">OAS Online Assessment Site</span> </div> </td> </tr> </table> <table border=\"0\" align=\"left\" width=\"5\" cellpadding=\"0\" cellspacing=\"0\" style=\"border-collapse: collapse; mso-table-lspace: 0pt; mso-table-rspace: 0pt;\" class=\"container590\"> <tr> <td height=\"20\" width=\"5\" style=\"font-size: 20px; line-height: 20px;\">&nbsp;</td> </tr> </table> <table border=\"0\" align=\"right\" cellpadding=\"0\" cellspacing=\"0\" style=\"border-collapse: collapse; mso-table-lspace: 0pt; mso-table-rspace: 0pt;\" class=\"container590\"> <tr> <td align=\"center\"> <table align=\"center\" border=\"0\" cellpadding=\"0\" cellspacing=\"0\"> <tr> <td align=\"center\"> <a style=\"font-size: 14px; font-family: \'Work Sans\', Calibri, sans-serif; line-height: 24px; color: #5caad2; text-decoration: none; font-weight: bold;\" href=\"{{UnsubscribeURL}}\">UNSUBSCRIBE</a> </td> </tr> </table> </td> </tr> </table> </td> </tr> </table> </td> </tr> <tr> <td height=\"25\" style=\"font-size: 25px; line-height: 25px;\">&nbsp;</td> </tr> </table> <!-- end footer ====== --></body></html>";
        }
    }
}
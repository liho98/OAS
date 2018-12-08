using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace OAS.Views.Lecturer
{
    public partial class MarkAssignment : System.Web.UI.Page
    {

        private static string connectionString = ConfigurationManager.ConnectionStrings["oasDB"].ConnectionString;
        private String[] assessment = new String[10];
        private String[] studentAssessment = new String[4];
        private String[] questionAnswer = new String[4];
        private List<String[]> questionAnswerList = new List<String[]>();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["assessment"] != null && Session["studentAssessment"] != null)
            {
                assessment = (String[])Session["assessment"];
                Session["assessment"] = assessment;
                Session.Timeout = 1000;
                studentAssessment = (String[])Session["studentAssessment"];
                Session["studentAssessment"] = studentAssessment;
                Session.Timeout = 1000;

                getQuestionAnswerList(Guid.Parse(assessment[0]), (Guid)(Membership.GetUser(studentAssessment[0])).ProviderUserKey);
                createQuestionAnswerTable();
            }
        }

        private void createQuestionAnswerTable()
        {
            Table table = new System.Web.UI.WebControls.Table();
            TableRow tableRow;
            TableCell tableCell;
            Table table2;
            TableRow tableRow2;
            TableCell tableCell2;
            TextBox textBox;

            HtmlGenericControl htmlGenericControl; HtmlGenericControl htmlGenericControl2; HtmlGenericControl htmlGenericControl3;
            //LinkButton linkButton; HtmlGenericControl span;

            table.ID = "datatables";
            table.CssClass = "table table-bordered";
            table.Attributes.CssStyle.Add("width", "100%");

            tableRow = new TableRow();
            tableRow.TableSection = TableRowSection.TableHeader;
            tableCell = new TableCell();
            tableCell.Attributes.Add("style", "padding: 10px!important;");

            if (assessment[0] != null)
            {
                tableCell.Text = "Assessment : " + assessment[1] + " | Type : " + assessment[2].Trim();
            }
            tableRow.Cells.Add(tableCell);
            table.Rows.Add(tableRow);

            for (int i = 0; i < questionAnswerList.Count; i++)
            {
                tableRow = new TableRow();
                tableCell = new TableCell();

                htmlGenericControl = new HtmlGenericControl("div");
                htmlGenericControl.Attributes.Add("style", "background-color:rgba(0, 0, 0, 0.05);padding: 20px;");

                htmlGenericControl2 = new HtmlGenericControl("h6");
                htmlGenericControl2.InnerHtml = "Question Level : " + questionAnswerList[i][3] + "";
                htmlGenericControl2.Attributes.Add("style", "float:right;font-size: 14px;");
                htmlGenericControl.Controls.Add(htmlGenericControl2);
                htmlGenericControl2 = new HtmlGenericControl("h3");
                htmlGenericControl2.InnerHtml = "Question " + (i + 1);
                htmlGenericControl.Controls.Add(htmlGenericControl2);

                tableCell.Controls.Add(htmlGenericControl);

                htmlGenericControl = new HtmlGenericControl("div");
                htmlGenericControl.Attributes.Add("style", "background-color:#fff;padding: 20px;border: 1px solid rgba(0,0,0,0.2);");

                htmlGenericControl2 = new HtmlGenericControl("div");
                table2 = new System.Web.UI.WebControls.Table(); table2.Attributes.Add("style", "width:100%");
                tableRow2 = new TableRow();
                tableCell2 = new TableCell(); tableCell2.Attributes.Add("style", "background-color: #fff;border:none;width:20%");

                htmlGenericControl3 = new HtmlGenericControl("div");
                htmlGenericControl3.InnerHtml = "<p>Question Image</p><img style=\"width:70%\" src=\"" + questionAnswerList[i][2] + "\"/>";
                htmlGenericControl3.Attributes.Add("style", "background-color:;width:100%;");
                tableCell2.Controls.Add(htmlGenericControl3);
                tableRow2.Cells.Add(tableCell2);

                tableCell2 = new TableCell(); tableCell2.Attributes.Add("style", "background-color: #fff;border:none");
                htmlGenericControl3 = new HtmlGenericControl("div");
                htmlGenericControl3.InnerHtml += questionAnswerList[i][0];

                htmlGenericControl3.Attributes.Add("style", "background-color:;width:100%;");
                tableCell2.Controls.Add(htmlGenericControl3);
                tableRow2.Cells.Add(tableCell2);
                table2.Rows.Add(tableRow2);

                htmlGenericControl2.Controls.Add(table2);

                htmlGenericControl.Controls.Add(htmlGenericControl2);

                tableCell.Controls.Add(htmlGenericControl);
                tableRow.Cells.Add(tableCell);
                //table.Rows.Add(tableRow);


                //tableRow = new TableRow();
                //tableCell = new TableCell();

                htmlGenericControl = new HtmlGenericControl("h4");
                htmlGenericControl.Attributes.Add("style", "font-family: 'Segoe UI Emoji';padding:5px;font-size:13px;padding-left:20px");
                htmlGenericControl.InnerHtml = "Student's Answer";
                tableCell.Controls.Add(htmlGenericControl);

                htmlGenericControl = new HtmlGenericControl("div");
                htmlGenericControl.Attributes.Add("style", "padding: 20px;padding-top: 8px;");
                htmlGenericControl.InnerHtml = questionAnswerList[i][1];

                tableCell.Controls.Add(htmlGenericControl);

                tableRow.Cells.Add(tableCell);
                table.Rows.Add(tableRow);
            }

            MarkAssignmentTablePlaceHolder.Controls.Add(table);
        }

        private void getQuestionAnswerList(Guid assessmentId, Guid userId)
        {
            string selectSql = "Select QuestionText, AnswerText, q.Image, q.QuestionLevel From UserProfiles u, Assignment ass, Assessment a, Question q, Answer ans " +
                               "Where ans.QuestionId = q.QuestionId and q.AssessmentId = a.AssessmentId and a.AssessmentId = ass.AssessmentId " +
                               "and ass.UserId = u.UserId and u.UserId = ans.UserId and u.UserId = @UserId " +
                               "and a.AssessmentId = @AssessmentId ";

            using (SqlConnection con = new SqlConnection(connectionString))
            {
                con.Open();
                SqlCommand sqlCommand = new SqlCommand(selectSql, con);
                sqlCommand.Parameters.AddWithValue("@UserId", userId);
                sqlCommand.Parameters.AddWithValue("@AssessmentId", assessmentId);
                SqlDataReader questionAnswerRecords = sqlCommand.ExecuteReader();

                while (questionAnswerRecords.Read())
                {
                    questionAnswer = new String[4];
                    questionAnswer[0] = questionAnswerRecords["QuestionText"].ToString();
                    questionAnswer[1] = questionAnswerRecords["AnswerText"].ToString();
                    if (questionAnswerRecords["Image"] != System.DBNull.Value)
                    {
                        questionAnswer[2] = Encoding.Default.GetString((byte[])questionAnswerRecords["Image"]);
                    }
                    questionAnswer[3] = questionAnswerRecords["QuestionLevel"].ToString();
                    questionAnswerList.Add(questionAnswer);
                }

                con.Close();
            }
        }

        protected void SubmitButton_OnClick(object sender, EventArgs e)
        {
            if (Page.IsValid)
            {
                try
                {
                    SendEmailService("Your OAS Assessment Result", EmailBodyHtml(studentAssessment[1], Double.Parse(ScoreTextbox.Text), CommentTextbox.Text, assessment[1]), Membership.GetUser(studentAssessment[0]).Email);
                    saveStudentScore(Guid.Parse(assessment[0]), (Guid)(Membership.GetUser(studentAssessment[0])).ProviderUserKey, double.Parse(ScoreTextbox.Text));
                    ScriptManager.RegisterStartupScript(this, GetType(), "showalert", "javascript:alert('A result with score and comment will send to Student\\'s email.');" +
                        "window.location = '" + Request.Url.Scheme + "://" + Request.Url.Authority + "/Views/Profile.aspx';", true);
                }
                catch(Exception ex)
                { Message.Text = ex.ToString(); }
            }
            else
            {
                //ValidationSummary.ShowMessageBox = true;
                //ValidationSummary.ShowSummary = true;
            }

        }

        private void saveStudentScore(Guid AssessmentId, Guid userId, double score)
        {
            string updateSql = "UPDATE [dbo].[Assignment] SET Score = @Score Where AssessmentId = @AssessmentId and UserId = @UserId";

            using (SqlConnection con = new SqlConnection(connectionString))
            {
                con.Open();
                SqlCommand sqlCommand = new SqlCommand(updateSql, con);
                sqlCommand.Parameters.AddWithValue("@Score", score);
                sqlCommand.Parameters.AddWithValue("@AssessmentId", AssessmentId);
                sqlCommand.Parameters.AddWithValue("@UserId", userId);
                sqlCommand.ExecuteNonQuery();
                con.Close();
            }
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

        private string EmailBodyHtml(String firstName, Double score, String comment, String assessmentTitle)
        {
            String result = "";
            if (score >= 50)
            {
                result = "<!-- pass start section --> <p style=\"line-height: 24px; margin-bottom: 20px;\"> Congrats you have pass your Assessment (" + assessmentTitle + "), Keep it up. </p><table border=\"0\" align=\"center\" width=\"180\" cellpadding=\"0\" cellspacing=\"0\" bgcolor=\"5caad2\" style=\"margin-bottom: 20px;\"><tr><td height=\"10\" style=\"font-size: 10px; line-height: 10px;\">&nbsp;</td></tr><tr><td align=\"center\" style=\"color: #ffffff; font-size: 14px; font-family: \'Work Sans\', Calibri, sans-serif; line-height: 22px; letter-spacing: 2px;\"><!-- main section button --><div style=\"line-height: 22px;\"><a style=\"color: #ffffff; text-decoration: none;\">Score : " + score.ToString() + "%</a></div></td></tr><tr><td height=\"10\" style=\"font-size: 10px; line-height: 10px;\">&nbsp;</td></tr></table> ";
            }
            else
            {
                result = "<!-- fail start section --> <p style=\"line-height: 24px; margin-bottom: 20px;\">Oh my god, You have failed your Assessment (" + assessmentTitle + "), have fun repeat the course.</p><table border=\"0\" align=\"center\" width=\"180\" cellpadding=\"0\" cellspacing=\"0\" bgcolor=\"5caad2\" style=\"margin-bottom: 20px;\"><tr><td height=\"10\" style=\"background-color: red;font-size: 10px; line-height: 10px;\">&nbsp;</td></tr><tr><td align=\"center\" style=\"background-color: red;color: #ffffff; font-size: 14px; font-family: \'Work Sans\', Calibri, sans-serif; line-height: 22px; letter-spacing: 2px;\"><!-- main section button --><div style=\"line-height: 22px;\"><a style=\"color: #ffffff; text-decoration: none;\">Score : " + score.ToString() + "%</a></div></td></tr><tr><td height=\"10\" style=\"background-color: red;font-size: 10px; line-height: 10px;\">&nbsp;</td></tr></table> <!-- fail end section -->";
            }

            return "<!DOCTYPE html PUBLIC \"-//W3C//DTD XHTML 1.0 Transitional//EN\" \"http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd\"><htmlxmlns:v=\"urn:schemas-microsoft-com:vml\"><head><meta http-equiv=\"Content-Type\" content=\"text/html; charset=UTF-8\" /><meta name=\"viewport\" content=\"width=device-width; initial-scale=1.0; maximum-scale=1.0;\" /><meta name=\"viewport\" content=\"width=600,initial-scale = 2.3,user-scalable=no\"/><!--[if !mso]><!-- --><link href=\'https://fonts.googleapis.com/css?family=Work+Sans:300,400,500,600,700\' rel=\"stylesheet\"/><link href=\'https://fonts.googleapis.com/css?family=Quicksand:300,400,700\' rel=\"stylesheet\"/><!--<![endif]--><title>OAS</title><style type=\"text/css\"> body { width: 100%; background-color: #ffffff; margin: 0; padding: 0; -webkit-font-smoothing: antialiased; mso-margin-top-alt: 0px; mso-margin-bottom-alt: 0px; mso-padding-alt: 0px 0px 0px 0px; } p, h1, h2, h3, h4 { margin-top: 0; margin-bottom: 0; padding-top: 0; padding-bottom: 0; } span.preheader { display: none; font-size: 1px; } html { width: 100%; } table { font-size: 14px; border: 0; } /* ----------- responsivity ----------- */ @media only screen and (max-width: 640px) { /*------ top header ------ */ .main-header { font-size: 20px !important; } .main-section-header { font-size: 28px !important; } .show { display: block !important; } .hide { display: none !important; } .align-center { text-align: center !important; } .no-bg { background: none !important; } /*----- main image -------*/ .main-image img { width: 440px !important; height: auto !important; } /* ====== divider ====== */ .divider img { width: 440px !important; } /*-------- container --------*/ .container590 { width: 440px !important; } .container580 { width: 400px !important; } .main-button { width: 220px !important; } /*-------- secions ----------*/ .section-img img { width: 320px !important; height: auto !important; } .team-img img { width: 100% !important; height: auto !important; } } @media only screen and (max-width: 479px) { /*------ top header ------ */ .main-header { font-size: 18px !important; } .main-section-header { font-size: 26px !important; } /* ====== divider ====== */ .divider img { width: 280px !important; } /*-------- container --------*/ .container590 { width: 280px !important; } .container590 { width: 280px !important; } .container580 { width: 260px !important; } /*-------- secions ----------*/ .section-img img { width: 280px !important; height: auto !important; } } </style><!--[if gte mso 9]><style type=”text/css”> body { font-family: arial, sans-serif!important; } </style><![endif]--></head><body class=\"respond\" leftmargin=\"0\" topmargin=\"0\" marginwidth=\"0\" marginheight=\"0\"><!-- pre-header --><table style=\"display: none!important;\"><tr><td><div style=\"overflow: hidden; display: none; font-size: 1px; color: #ffffff; line-height: 1px; font-family: Arial; maxheight: 0px; max-width: 0px; opacity: 0;\"> Welcome to MDB! </div></td></tr></table><!-- pre-header end --><!-- header --><table border=\"0\" width=\"100%\" cellpadding=\"0\" cellspacing=\"0\" bgcolor=\"ffffff\"><tr><td align=\"center\"><table border=\"0\" align=\"center\" width=\"590\" cellpadding=\"0\" cellspacing=\"0\" class=\"container590\"><tr><td height=\"25\" style=\"font-size: 25px; line-height: 25px;\">&nbsp;</td></tr><tr><td align=\"center\"><table border=\"0\" align=\"center\" width=\"590\" cellpadding=\"0\" cellspacing=\"0\" class=\"container590\"><tr><td align=\"center\" height=\"70\" style=\"height: 70px;\"><a href=\"\" style=\"display: block; border-style: none !important; border: 0 !important;\"><img width=\"100\" border=\"0\" style=\"display: block; width: 100px;\" src=\"http://funkyimg.com/i/2Nc5L.png\" alt=\"\" /></a></td></tr><tr><td align=\"center\"><table width=\"360 \" border=\"0\" cellpadding=\"0\" cellspacing=\"0\" style=\"border-collapse: collapse; mso-table-lspace: 0pt; mso-table-rspace: 0pt;\" class=\"container590 hide\"><tr><td width=\"120\" align=\"center\" style=\"font-size: 14px; font-family: \'Work Sans\', Calibri, sans-serif; line-height: 24px;\"><a href=\"\" style=\"color: #312c32; text-decoration: none;\"></a></td><td width=\"120\" align=\"center\" style=\"font-size: 14px; font-family: \'Work Sans\', Calibri, sans-serif; line-height: 24px;\"><a href=\"\" style=\"color: #312c32; text-decoration: none;\"></a></td><td width=\"120\" align=\"center\" style=\"font-size: 14px; font-family: \'Work Sans\', Calibri, sans-serif; line-height: 24px;\"><a href=\"\" style=\"color: #312c32; text-decoration: none;\"></a></td></tr></table></td></tr></table></td></tr><tr><td height=\"25\" style=\"font-size: 25px; line-height: 25px;\">&nbsp;</td></tr></table></td></tr></table><!-- end header --><!-- big image section --><table border=\"0\" width=\"100%\" cellpadding=\"0\" cellspacing=\"0\" bgcolor=\"ffffff\" class=\"bg_color\"><tr><td align=\"center\"><table border=\"0\" align=\"center\" width=\"590\" cellpadding=\"0\" cellspacing=\"0\" class=\"container590\"><tr><td align=\"center\" style=\"color: #343434; font-size: 24px; font-family: Quicksand, Calibri, sans-serif; font-weight: 700; letter-spacing: 3px; line-height: 35px;\" class=\"main-header\"><!-- section text ======--><div style=\"line-height: 35px\">Your Assessment<span style=\"color: #5caad2;\"> Result</span></div></td></tr><tr><td height=\"10\" style=\"font-size: 10px; line-height: 10px;\">&nbsp;</td></tr><tr><td align=\"center\"><table border=\"0\" width=\"40\" align=\"center\" cellpadding=\"0\" cellspacing=\"0\" bgcolor=\"eeeeee\"><tr><td height=\"2\" style=\"font-size: 2px; line-height: 2px;\">&nbsp;</td></tr></table></td></tr><tr><td height=\"20\" style=\"font-size: 20px; line-height: 20px;\">&nbsp;</td></tr><tr><td align=\"left\"><table border=\"0\" width=\"590\" align=\"center\" cellpadding=\"0\" cellspacing=\"0\" class=\"container590\"><tr><td align=\"left\" style=\"color: #888888; font-size: 16px; font-family: \'Work Sans\', Calibri, sans-serif; line-height: 24px;\"><!-- section text ======--><p style=\"line-height: 24px; margin-bottom: 15px;\"> Hi, " + firstName + " </p><p style=\"line-height: 24px; margin-bottom: 15px;\"> You\'re receiving this e-mail because your lecturer has graded your Assessment result.</p> " + result + "<br /> <!-- comment start section --> <p style=\"line-height: 24px; margin-bottom: 20px;\">Lecturer\'s comment</p> <p style=\"line-height: 24px;padding:5px; margin-bottom: 20px;border:5px dotted rgba(0,0,0,0.1)\">" + comment + "</p> <!-- comment end section --><p style=\"line-height: 24px\"> If you have any problem, please send an e-mail to let us know.<br/> The OAS Lecturer team </p></td></tr></table></td></tr></table></td></tr><tr><td height=\"40\" style=\"font-size: 40px; line-height: 40px;\">&nbsp;</td></tr></table><!-- end section --><!-- contact section --><table border=\"0\" width=\"100%\" cellpadding=\"0\" cellspacing=\"0\" bgcolor=\"ffffff\" class=\"bg_color\"><tr><td height=\"60\" style=\"font-size: 60px; line-height: 60px;\">&nbsp;</td></tr><tr><td align=\"center\"><table border=\"0\" align=\"center\" width=\"590\" cellpadding=\"0\" cellspacing=\"0\" class=\"container590 bg_color\"><tr><td align=\"center\"><table border=\"0\" align=\"center\" width=\"590\" cellpadding=\"0\" cellspacing=\"0\" class=\"container590 bg_color\"><tr><td><table border=\"0\" width=\"300\" align=\"left\" cellpadding=\"0\" cellspacing=\"0\" style=\"border-collapse: collapse; mso-table-lspace: 0pt; mso-table-rspace: 0pt;\" class=\"container590\"><tr><td height=\"25\" style=\"font-size: 25px; line-height: 25px;\">&nbsp;</td></tr><tr><td align=\"left\" style=\"color: #888888; font-size: 14px; font-family: \'Work Sans\', Calibri, sans-serif; line-height: 23px;\" class=\"text_color\"><div style=\"color: #333333; font-size: 14px; font-family: \'Work Sans\', Calibri, sans-serif; font-weight: 600; mso-line-height-rule: exactly; line-height: 23px;\"> Email us: <br /><a href=\"mailto:\" style=\"color: #888888; font-size: 14px; font-family: \'Hind Siliguri\', Calibri, Sans-serif; font-weight: 400;\">OAS.edu.my@gmail.com</a></div></td></tr></table><table border=\"0\" width=\"2\" align=\"left\" cellpadding=\"0\" cellspacing=\"0\" style=\"border-collapse: collapse; mso-table-lspace: 0pt; mso-table-rspace: 0pt;\" class=\"container590\"><tr><td width=\"2\" height=\"10\" style=\"font-size: 10px; line-height: 10px;\"></td></tr></table><table border=\"0\" width=\"200\" align=\"right\" cellpadding=\"0\" cellspacing=\"0\" style=\"border-collapse: collapse; mso-table-lspace: 0pt; mso-table-rspace: 0pt;\" class=\"container590\"><tr><td class=\"hide\" height=\"45\" style=\"font-size: 45px; line-height: 45px;\">&nbsp;</td></tr><tr><td height=\"15\" style=\"font-size: 15px; line-height: 15px;\">&nbsp;</td></tr><tr><td><table border=\"0\" align=\"right\" cellpadding=\"0\" cellspacing=\"0\"><tr><td><a href=\"https://www.facebook.com/liho.98\" style=\"display: block; border-style: none !important; border: 0 !important;\"><img width=\"24\" border=\"0\" style=\"display: block;\" src=\"http://i.imgur.com/Qc3zTxn.png\" alt=\"\"></a></td><td>&nbsp;&nbsp;&nbsp;&nbsp;</td><td><a href=\"https://twitter.com/liho_98\" style=\"display: block; border-style: none !important; border: 0 !important;\"><img width=\"24\" border=\"0\" style=\"display: block;\" src=\"http://i.imgur.com/RBRORq1.png\" alt=\"\"></a></td><td>&nbsp;&nbsp;&nbsp;&nbsp;</td><td><a href=\"https://plus.google.com/100956812200203645080\" style=\"display: block; border-style: none !important; border: 0 !important;\"><img width=\"24\" border=\"0\" style=\"display: block;\" src=\"http://i.imgur.com/Wji3af6.png\" alt=\"\"></a></td></tr></table></td></tr></table></td></tr></table></td></tr></table></td></tr><tr><td height=\"60\" style=\"font-size: 60px; line-height: 60px;\">&nbsp;</td></tr></table><!-- end section --><!-- footer ====== --><table border=\"0\" width=\"100%\" cellpadding=\"0\" cellspacing=\"0\" bgcolor=\"f4f4f4\"><tr><td height=\"25\" style=\"font-size: 25px; line-height: 25px;\">&nbsp;</td></tr><tr><td align=\"center\"><table border=\"0\" align=\"center\" width=\"590\" cellpadding=\"0\" cellspacing=\"0\" class=\"container590\"><tr><td><table border=\"0\" align=\"left\" cellpadding=\"0\" cellspacing=\"0\" style=\"border-collapse: collapse; mso-table-lspace: 0pt; mso-table-rspace: 0pt;\" class=\"container590\"><tr><td align=\"left\" style=\"color: #aaaaaa; font-size: 14px; font-family: \'Work Sans\', Calibri, sans-serif; line-height: 24px;\"><div style=\"line-height: 24px;\"><span style=\"color: #333333;\">OAS Online Assessment Site</span></div></td></tr></table><table border=\"0\" align=\"left\" width=\"5\" cellpadding=\"0\" cellspacing=\"0\" style=\"border-collapse: collapse; mso-table-lspace: 0pt; mso-table-rspace: 0pt;\" class=\"container590\"><tr><td height=\"20\" width=\"5\" style=\"font-size: 20px; line-height: 20px;\">&nbsp;</td></tr></table><table border=\"0\" align=\"right\" cellpadding=\"0\" cellspacing=\"0\" style=\"border-collapse: collapse; mso-table-lspace: 0pt; mso-table-rspace: 0pt;\" class=\"container590\"><tr><td align=\"center\"><table align=\"center\" border=\"0\" cellpadding=\"0\" cellspacing=\"0\"><tr><td align=\"center\"><a style=\"font-size: 14px; font-family: \'Work Sans\', Calibri, sans-serif; line-height: 24px; color: #5caad2; text-decoration: none; font-weight: bold;\" href=\"{{UnsubscribeURL}}\">UNSUBSCRIBE</a></td></tr></table></td></tr></table></td></tr></table></td></tr><tr><td height=\"25\" style=\"font-size: 25px; line-height: 25px;\">&nbsp;</td></tr></table><!-- end footer ====== --></body></html>";
        }
    }
}
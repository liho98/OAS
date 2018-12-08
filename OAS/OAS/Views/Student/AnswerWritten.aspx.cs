using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace OAS.Views.Student
{
    public partial class AnswerWritten : System.Web.UI.Page
    {

        private static string connectionString = ConfigurationManager.ConnectionStrings["oasDB"].ConnectionString;
        private String[] question = new String[4];
        protected String[] assignment = new String[4];

        protected List<String[]> questionList = new List<String[]>();
        //private List<String> studentAnswerList = new List<String>();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["assignment"] != null)
            {
                assignment = (String[])Session["assignment"];
                if (checkIsAnswered(Guid.Parse(assignment[0])))
                {
                    ScriptManager.RegisterStartupScript(this, GetType(), "showalert", "alert('You have answered this assessment.\\nPlease wait the Lecturer to mark it.\\nYour result will send through to your OAS Email Account.');" +
                        "window.location = '" + Request.Url.Scheme + "://" + Request.Url.Authority + "/Views/Profile.aspx';", true);
                }
                getQuestion(Guid.Parse(assignment[0]));
                Session["assignment"] = assignment;

                if (Session["Timer"] == null)
                {
                    TimeSpan timeSpan = TimeSpan.FromMinutes(Convert.ToInt16(assignment[3]));
                    Timer.Interval = Convert.ToInt32((timeSpan.TotalMilliseconds + 1000));
                    ScriptManager.RegisterStartupScript(this, GetType(), "setTimerSession", "sessionStorage.setItem('timer', " + Convert.ToInt32(timeSpan.TotalSeconds + 1).ToString() + " );", true);
                    //Timer.Interval = 11000;
                    //ScriptManager.RegisterStartupScript(this, GetType(), "setTimerSession", "sessionStorage.setItem('timer', " + 11 + " );", true);
                    Session["Timer"] = timeSpan.Minutes;
                }
                ScriptManager.RegisterStartupScript(this, GetType(), "setTimer", "var minutesLabel = document.getElementById(\"minutes\");var secondsLabel = document.getElementById(\"seconds\");var totalSeconds = sessionStorage.getItem('timer');setInterval(setTime, 1000);function setTime(){if (totalSeconds > 0){totalSeconds--;sessionStorage.setItem('timer', totalSeconds);}secondsLabel.innerHTML = pad(totalSeconds % 60);minutesLabel.innerHTML = pad(parseInt(totalSeconds / 60));}function pad(val){var valString = val + \"\";if (valString.length < 2){return \"0\" + valString;}else{return valString;}}", true);

                Session.Timeout = Convert.ToInt16(assignment[3]);
            }

            if (ViewState["questionList"] != null)
            {
                questionList = ViewState["questionList"] as List<String[]>;
            }

            questionTable();

            if (!Page.IsPostBack)
            {
                Message.Text = (String)Request.QueryString["Message"];
            }
        }
        protected void Timer_Tick(object sender, EventArgs e)
        {
            if (Session["assignment"] != null)
            {
                assignment = (String[])Session["assignment"];
            }
            else
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "showalert", "alert('Session expired, returning back.');" +
                    "window.location = '" + Request.Url.Scheme + "://" + Request.Url.Authority + "/Views/Profile.aspx';", true);
            }
            TextBox textbox = new TextBox();
            for (int i = 0; i < questionList.Count; i++)
            {
                textbox = (TextBox)AnswerTablePlaceHolder.FindControl("editor" + i);
                saveStudentAnswer(Guid.Parse(questionList[i][3]), textbox.Text);
            }
            Session.Remove("Timer");
            ScriptManager.RegisterStartupScript(this, GetType(), "showalert", "alert('Times up your answer has been submitted.\\nPlease wait the Lecturer to mark it.\\nYour result will send through to your OAS Email Account.');" +
                 "location = '" + Request.Url.Scheme + "://" + Request.Url.Authority + "/Views/Profile.aspx';", true);
        }

        protected void SubmitButton_OnClick(object sender, EventArgs e)
        {
            bool isAllCheck = true;
            if (Session["assignment"] != null)
            {
                assignment = (String[])Session["assignment"];
            }
            else
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "showalert", "alert('Session expired, returning back.');" +
                    "window.location = '" + Request.Url.Scheme + "://" + Request.Url.Authority + "/Views/Profile.aspx';", true);
            }
            TextBox textbox = new TextBox();

            for (int i = 0; i < questionList.Count; i++)
            {
                textbox = (TextBox)AnswerTablePlaceHolder.FindControl("editor" + i);

                if (textbox.Text != "<p>&nbsp;</p>")
                {

                }
                else
                {
                    isAllCheck = false; break;
                }
            }

            if (isAllCheck == false)
            {
                Message.ForeColor = System.Drawing.Color.Red;
                Message.Text = "Please answer ALL the question given.";
            }
            else
            {
                Message.ForeColor = System.Drawing.Color.Green;
                for (int i = 0; i < questionList.Count; i++)
                {
                    textbox = (TextBox)AnswerTablePlaceHolder.FindControl("editor" + i);
                    saveStudentAnswer(Guid.Parse(questionList[i][3]), textbox.Text);
                }

                ScriptManager.RegisterStartupScript(this, GetType(), "showalert", "alert('You have submitted your answers.\\nPlease wait the Lecturer to mark it.\\nYour result will send through to your OAS Email Account.');" +
                     "window.location = '" + Request.Url.Scheme + "://" + Request.Url.Authority + "/Views/Profile.aspx';", true);
                Message.Text = "You have submitted your answers.";
            }

        }
        private void saveStudentAnswer(Guid questionId, String studentAnswer)
        {
            string insertSql = "INSERT INTO Answer(QuestionId, UserId, AnswerText) " +
                               "VALUES(@QuestionId, @UserId, @AnswerText )";

            using (SqlConnection con = new SqlConnection(connectionString))
            {
                con.Open();
                SqlCommand sqlCommand = new SqlCommand(insertSql, con);
                sqlCommand.Parameters.AddWithValue("@QuestionId", questionId);
                sqlCommand.Parameters.AddWithValue("@UserId", (Guid)(Membership.GetUser(HttpContext.Current.User.Identity.Name)).ProviderUserKey);
                sqlCommand.Parameters.AddWithValue("@AnswerText", studentAnswer);
                sqlCommand.ExecuteNonQuery();
                con.Close();
            }
        }

        private bool checkIsAnswered(Guid assessmentId)
        {
            bool isAnswered = false;
            string selectSql = "Select AnswerText From UserProfiles u, Assignment ass, Assessment a, Question q, Answer ans " +
            "Where ans.QuestionId = q.QuestionId and q.AssessmentId = a.AssessmentId and a.AssessmentId = ass.AssessmentId " +
            "and ass.UserId = u.UserId and u.UserId = ans.UserId and u.UserId = @UserId " +
            "and a.AssessmentId = @AssessmentId";

            using (SqlConnection con = new SqlConnection(connectionString))
            {
                con.Open();
                SqlCommand sqlCommand = new SqlCommand(selectSql, con);
                sqlCommand.Parameters.AddWithValue("@UserId", (Guid)(Membership.GetUser(HttpContext.Current.User.Identity.Name)).ProviderUserKey);
                sqlCommand.Parameters.AddWithValue("@AssessmentId", assessmentId);
                SqlDataReader answerRecords = sqlCommand.ExecuteReader();

                if (answerRecords.Read())
                {
                    if (answerRecords["AnswerText"] != System.DBNull.Value)
                    {
                        isAnswered = true;
                    }
                }
                con.Close();
            }
            return isAnswered;
        }

        private void getQuestion(Guid assessmentId)
        {
            questionList = new List<String[]>();

            string selectSql = "Select * From Question q, Assessment a Where q.AssessmentId = a.AssessmentId and a.AssessmentId = @AssessmentId ";

            using (SqlConnection con = new SqlConnection(connectionString))
            {
                con.Open();
                SqlCommand sqlCommand = new SqlCommand(selectSql, con);
                sqlCommand.Parameters.AddWithValue("@AssessmentId", assessmentId);
                SqlDataReader questionRecords = sqlCommand.ExecuteReader();

                while (questionRecords.Read())
                {
                    question = new String[4];
                    question[0] = questionRecords["QuestionText"].ToString();
                    question[1] = questionRecords["QuestionLevel"].ToString();
                    if (questionRecords["Image"] != System.DBNull.Value)
                    {
                        question[2] = Encoding.Default.GetString((byte[])questionRecords["Image"]);
                    }
                    question[3] = questionRecords["QuestionId"].ToString();

                    questionList.Add(question);
                }
                ViewState["questionList"] = questionList;
                con.Close();
            }
        }

        private void questionTable()
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

            if (assignment[0] != null)
            {
                tableCell.Text = "Assessment : " + assignment[1] + " | Type : " + assignment[2].Trim() + " | Duration/Time Left : <label id=\"minutes\">00</label>:<label id=\"seconds\">00</label>";
            }
            tableRow.Cells.Add(tableCell);
            table.Rows.Add(tableRow);

            for (int i = 0; i < questionList.Count; i++)
            {
                tableRow = new TableRow();
                tableCell = new TableCell();

                htmlGenericControl = new HtmlGenericControl("div");
                htmlGenericControl.Attributes.Add("style", "background-color:rgba(0, 0, 0, 0.05);padding: 20px;");

                htmlGenericControl2 = new HtmlGenericControl("h6");
                htmlGenericControl2.InnerHtml = "Question Level : " + questionList[i][1] + "";
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
                htmlGenericControl3.InnerHtml = "<p>Question Image</p><img style=\"width:70%\" src=\"" + questionList[i][2] + "\"/>";
                htmlGenericControl3.Attributes.Add("style", "background-color:;width:100%;");
                tableCell2.Controls.Add(htmlGenericControl3);
                tableRow2.Cells.Add(tableCell2);

                tableCell2 = new TableCell(); tableCell2.Attributes.Add("style", "background-color: #fff;border:none");
                htmlGenericControl3 = new HtmlGenericControl("div");
                htmlGenericControl3.InnerHtml += questionList[i][0];

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
                htmlGenericControl.InnerHtml = "Insert Your Answer";
                tableCell.Controls.Add(htmlGenericControl);

                textBox = new TextBox();
                textBox.ID = "editor" + i;
                textBox.TextMode = TextBoxMode.MultiLine;

                tableCell.Controls.Add(textBox);

                tableRow.Cells.Add(tableCell);
                table.Rows.Add(tableRow);
            }
            AnswerTablePlaceHolder.Controls.Add(table);
        }
    }
}
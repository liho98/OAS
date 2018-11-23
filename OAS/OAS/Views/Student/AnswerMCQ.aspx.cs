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
    public partial class AnswerMCQ : System.Web.UI.Page
    {

        private static string connectionString = ConfigurationManager.ConnectionStrings["oasDB"].ConnectionString;
        //private String[] assessment = new String[10];
        private String[] question = new String[4];
        private String[] option = new String[2];
        protected String[] assignment = new String[4];

        private List<String[]> questionList = new List<String[]>();
        private List<String[]> optionList = new List<String[]>();
        private List<String> studentAnswerList = new List<String>();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["assignment"] != null)
            {
                assignment = (String[])Session["assignment"];
                if (checkIsAnswered(Guid.Parse(assignment[0])))
                {
                    ScriptManager.RegisterStartupScript(this, GetType(), "showalert", "alert('You have answered this assessment.');" +
                        "window.location = '" + Request.Url.Scheme + "://" + Request.Url.Authority + "/Views/Profile.aspx';", true);
                }
                getQuestion(Guid.Parse(assignment[0]));
                Session["assignment"] = assignment;
                Session.Timeout = 1000;
            }
            //if (Session["assessment"] != null)
            //{
            //    assessment = Session["assessment"] as String[];
            //}
            if (ViewState["questionList"] != null)
            {
                questionList = ViewState["questionList"] as List<String[]>;
            }
            if (ViewState["optionList"] != null)
            {
                optionList = ViewState["optionList"] as List<String[]>;
            }

            questionTable();

            if (!Page.IsPostBack)
            {
                Message.Text = (String)Request.QueryString["Message"];
            }
        }

        protected void SubmitButton_OnClick(object sender, EventArgs e)
        {
            if (Session["assignment"] != null)
            {
                assignment = (String[])Session["assignment"];
            }

            bool isAllCheck = false;
            int correctAnsCount = 0;
            RadioButtonList radioButtonList = new RadioButtonList();

            for (int i = 0; i < questionList.Count; i++)
            {
                radioButtonList = (RadioButtonList)AnswerTablePlaceHolder.FindControl("OptionRadioButton" + i);

                getOption(Guid.Parse(questionList[i][3]));

                for (int j = 0; j < optionList.Count; j++)
                {
                    if (radioButtonList.SelectedIndex == -1)
                    {
                        isAllCheck = false; break;
                    }
                    else
                    {
                        isAllCheck = true;
                    }

                    if (optionList[j][0] != "<p>&nbsp;</p>")
                    {
                        if (optionList[j][0] == radioButtonList.SelectedValue && optionList[j][1] == "True")
                        {
                            correctAnsCount++;
                        }
                    }
                }

                if (isAllCheck == false)
                {
                    break;
                }
            }

            if (isAllCheck == false)
            {
                Message.ForeColor = System.Drawing.Color.Red;
                Message.Text = "Please answer ALL the question given.";
            }
            else
            {

                for (int i = 0; i < questionList.Count; i++)
                {
                    radioButtonList = (RadioButtonList)AnswerTablePlaceHolder.FindControl("OptionRadioButton" + i);
                    saveStudentAnswer(Guid.Parse(questionList[i][3]), radioButtonList.SelectedValue);
                }

                double score = Math.Round((((double)correctAnsCount / questionList.Count) * 100.0), 2);
                saveStudentScore(Guid.Parse(assignment[0]), score);

                Message.ForeColor = System.Drawing.Color.Green;
                ScriptManager.RegisterStartupScript(this, GetType(), "showalert", "alert('You have submitted your answers.\\nYou have correct " + correctAnsCount + " question out of " + questionList.Count + ".\\nHence your total score is " + score + "');" +
                    "window.location = '" + Request.Url.Scheme + "://" + Request.Url.Authority + "/Views/Profile.aspx';", true);
                //Message.Text = "You have submitted your answers.";
            }
        }

        private bool checkIsAnswered(Guid assessmentId)
        {
            bool isAnswered = false;
            string selectSql = "Select Score From Assignment Where AssessmentId = @AssessmentId and UserId = @UserId ";

            using (SqlConnection con = new SqlConnection(connectionString))
            {
                con.Open();
                SqlCommand sqlCommand = new SqlCommand(selectSql, con);
                sqlCommand.Parameters.AddWithValue("@AssessmentId", assessmentId);
                sqlCommand.Parameters.AddWithValue("@UserId", (Guid)(Membership.GetUser(HttpContext.Current.User.Identity.Name)).ProviderUserKey);
                SqlDataReader scoreRecords = sqlCommand.ExecuteReader();

                if (scoreRecords.Read())
                {
                    if (scoreRecords["Score"] != System.DBNull.Value)
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

        private void getOption(Guid questionId)
        {
            optionList = new List<String[]>();

            string selectSql = "Select * From [dbo].[Option] o, Question q Where o.QuestionId = q.QuestionId and q.QuestionId = @QuestionId  ORDER BY o.QuestionId";

            using (SqlConnection con = new SqlConnection(connectionString))
            {
                con.Open();
                SqlCommand sqlCommand = new SqlCommand(selectSql, con);
                sqlCommand.Parameters.AddWithValue("@QuestionId", questionId);
                SqlDataReader optionRecords = sqlCommand.ExecuteReader();

                while (optionRecords.Read())
                {
                    option = new String[2];
                    option[0] = optionRecords["OptionText"].ToString();
                    option[1] = optionRecords["IsCorrectAnswer"].ToString();

                    optionList.Add(option);
                }
                ViewState["optionList"] = optionList;
                con.Close();
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

        private void saveStudentScore(Guid AssessmentId, Double score)
        {
            string updateSql = "UPDATE [dbo].[Assignment] SET Score = @Score Where AssessmentId = @AssessmentId and UserId = @UserId";

            using (SqlConnection con = new SqlConnection(connectionString))
            {
                con.Open();
                SqlCommand sqlCommand = new SqlCommand(updateSql, con);
                sqlCommand.Parameters.AddWithValue("@Score", score);
                sqlCommand.Parameters.AddWithValue("@AssessmentId", AssessmentId);
                sqlCommand.Parameters.AddWithValue("@UserId", (Guid)(Membership.GetUser(HttpContext.Current.User.Identity.Name)).ProviderUserKey);
                sqlCommand.ExecuteNonQuery();
                con.Close();
            }
        }

        private void questionTable()
        {
            //ViewState["assessment"] = assessment;

            Table table = new System.Web.UI.WebControls.Table();
            TableRow tableRow;
            TableCell tableCell;
            Table table2;
            TableRow tableRow2;
            TableCell tableCell2;
            RadioButtonList radioButtonList;

            HtmlGenericControl htmlGenericControl; HtmlGenericControl htmlGenericControl2; HtmlGenericControl htmlGenericControl3; ; HtmlGenericControl htmlGenericControl4;
            //LinkButton linkButton; HtmlGenericControl span;

            table.ID = "datatables";
            table.CssClass = "table table-striped table-bordered";
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
                htmlGenericControl.Attributes.Add("style", "background-color:#fff;padding: 20px;border: 1px solid rgba(0,0,0,0.05);");

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

                getOption(Guid.Parse(questionList[i][3]));

                radioButtonList = new RadioButtonList();
                int count = 0;
                for (int j = 0; j < optionList.Count; j++)
                {
                    if (optionList[j][0] != "<p>&nbsp;</p>")
                    {
                        //htmlGenericControl4 = new HtmlGenericControl("div");
                        //htmlGenericControl4.ID = "OptionDiv" + i + j;
                        //htmlGenericControl4.Attributes.Add("class", "optionDiv");
                        //htmlGenericControl4.InnerHtml = "<span >&#" + (count + 9398) + " </span><span style=\"width:95%;line-height: 0;\"> " + optionList[j][0] + "</span>";
                        //radiobutton = new RadioButton();
                        radioButtonList.ID = "OptionRadioButton" + i;
                        radioButtonList.Items.Add(new ListItem("<span >&#" + (count + 9398) + " </span><span style=\"width:95%;float: right;\"> " + optionList[j][0] + "</span>", optionList[j][0])); radioButtonList.CssClass = "RadioButtonClass";
                        //radiobutton.ID = "OptionRadioButton" + i + j;
                        //radiobutton.CssClass = "RadioButtonClass";
                        //radiobutton.GroupName = "OptionRadioButtonGroup" + i;
                        //radiobutton.Attributes.Add("style", "all:inherit;cursor:pointer;display:inline-block");
                        //radiobutton.Text = "<span >&#" + (count + 9398) + " </span><span style=\"width:95%;line-height: 0;\"> " + optionList[j][0] + "</span>";
                        //htmlGenericControl4.Controls.Add(radioButtonList);

                        //linkButton = new LinkButton();
                        //linkButton.ID = "link"+i+j;
                        //linkButton.Text = questionList[0][3];
                        //linkButton.Attributes.Add("style", "all:inherit;cursor:pointer;display:inline-block");
                        //linkButton.Click += new EventHandler(this.saveStudentAnswer_OnClick);
                        //linkButton.Controls.Add(htmlGenericControl4);
                        htmlGenericControl3.Controls.Add(radioButtonList);

                        //htmlGenericControl3.InnerHtml += "<div class=\"optionDiv\">" + "<span >&#" + (count + 9398) + " </span><span style=\"width:95%\"> " + optionList[j][0] + "</span></div>";
                        count++;
                    }
                }

                htmlGenericControl3.Attributes.Add("style", "background-color:;width:100%;");
                tableCell2.Controls.Add(htmlGenericControl3);
                tableRow2.Cells.Add(tableCell2);
                table2.Rows.Add(tableRow2);

                htmlGenericControl2.Controls.Add(table2);

                htmlGenericControl.Controls.Add(htmlGenericControl2);

                tableCell.Controls.Add(htmlGenericControl);

                tableRow.Cells.Add(tableCell);
                table.Rows.Add(tableRow);
            }
            AnswerTablePlaceHolder.Controls.Add(table);
        }
    }
}
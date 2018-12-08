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
    public partial class ReviewMCQAnswer : System.Web.UI.Page
    {
        private static string connectionString = ConfigurationManager.ConnectionStrings["oasDB"].ConnectionString;
        private String[] question = new String[4];
        private String[] option = new String[2];
        protected String[] assignment = new String[4];

        private List<String[]> questionList = new List<String[]>();
        private List<String[]> optionList = new List<String[]>();
        private List<String> studentAnswerList = new List<String>();
        private String studentAnswer = String.Empty;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["assignment"] != null)
            {
                assignment = (String[])Session["assignment"];
                getQuestion(Guid.Parse(assignment[0]));
                Session["assignment"] = assignment;
            }
                if (ViewState["questionList"] != null)
            {
                questionList = ViewState["questionList"] as List<String[]>;
            }
            if (ViewState["optionList"] != null)
            {
                optionList = ViewState["optionList"] as List<String[]>;
            }

            questionTable();
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
        private void getStudentAnswerList(Guid assessmentId, Guid userId)
        {
            string selectSql = "Select AnswerText From UserProfiles u, Assignment ass, Assessment a, Question q, Answer ans " +
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
                    //studentAnswer = String.Empty;
                    studentAnswer = questionAnswerRecords["AnswerText"].ToString();
                    studentAnswerList.Add(studentAnswer);
                }

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

            int correctAnsCount = 0;
            if (assignment[0] != null)
            {
                getStudentAnswerList(Guid.Parse(assignment[0]), (Guid)(Membership.GetUser(HttpContext.Current.User.Identity.Name)).ProviderUserKey);
                for (int i = 0; i < questionList.Count; i++)
                {
                    getOption(Guid.Parse(questionList[i][3]));
                    for (int j = 0; j < optionList.Count; j++)
                    {
                        if (optionList[j][0] != "<p>&nbsp;</p>")
                        {
                            if (bool.Parse(optionList[j][1]) == true && studentAnswerList[i] == optionList[j][0])
                            {
                                correctAnsCount++;
                            }
                        }
                    }
                }
                double score = Math.Round((((double)correctAnsCount / questionList.Count) * 100.0), 2);
                tableCell.Text = "Assessment : " + assignment[1] + " | Type : " + assignment[2].Trim() + " | Duration : "+ assignment[3] + " mins | Score : " + score.ToString("0.00") + "% | Correct " + correctAnsCount +" out of "+ questionList.Count +" question(s)";
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
                string color = "";
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
                        radioButtonList.Enabled = false;

                        if (bool.Parse(optionList[j][1]) == false && studentAnswerList[i] == optionList[j][0])
                        {
                            color = "rgba(255, 0, 0, 0.22);";
                        }
                        else
                        {
                            color = "";
                        }
                        if (bool.Parse(optionList[j][1]) == true)
                        {
                            color = "rgba(185,246,202,0.35);";
                        }

                        radioButtonList.Items.Add(new ListItem("<div style=\"background-color: " + color + "\"><span>&#" + (count + 9398) + " </span><span style=\"width:95%;float: right;\"> " + optionList[j][0] + "</span></div>", optionList[j][0])); radioButtonList.CssClass = "RadioButtonClass";

                        htmlGenericControl3.Controls.Add(radioButtonList);
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
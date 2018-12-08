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
    public partial class ReviewWrittenAnswer : System.Web.UI.Page
    {
        private static string connectionString = ConfigurationManager.ConnectionStrings["oasDB"].ConnectionString;
        private String[] question = new String[4];
        protected String[] assignment = new String[4];

        protected List<String[]> questionList = new List<String[]>();
        private List<String> studentAnswerList = new List<String>();
        private String studentAnswer = String.Empty;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["assignment"] != null)
            {
                assignment = (String[])Session["assignment"];
                getQuestion(Guid.Parse(assignment[0]));
                Session["assignment"] = assignment;

                Session.Timeout = Convert.ToInt16(assignment[3]);
            }

            if (ViewState["questionList"] != null)
            {
                questionList = ViewState["questionList"] as List<String[]>;
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
        private double getScore(Guid assessmentId, Guid userId)
        {
            string selectSql = "Select Score From Assignment Where AssessmentId = @AssessmentId and UserId = @UserId ";
            double score = 0;
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                con.Open();
                SqlCommand sqlCommand = new SqlCommand(selectSql, con);
                sqlCommand.Parameters.AddWithValue("@AssessmentId", assessmentId);
                sqlCommand.Parameters.AddWithValue("@UserId", userId);
                SqlDataReader scoreRecords = sqlCommand.ExecuteReader();

                if (scoreRecords.Read())
                {
                    score = double.Parse(scoreRecords["Score"].ToString());
                }
                con.Close();
                return score;
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
                getStudentAnswerList(Guid.Parse(assignment[0]), (Guid)(Membership.GetUser(HttpContext.Current.User.Identity.Name)).ProviderUserKey);

                tableCell.Text = "Assessment : " + assignment[1] + " | Type : " + assignment[2].Trim() + " | Duration : " + assignment[3] + " mins | Score : " + getScore(Guid.Parse(assignment[0]), (Guid)(Membership.GetUser(HttpContext.Current.User.Identity.Name)).ProviderUserKey).ToString("0.00") + "%";
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
                htmlGenericControl.InnerHtml = "Your Submmited Answer";
                tableCell.Controls.Add(htmlGenericControl);

                htmlGenericControl = new HtmlGenericControl("div");
                htmlGenericControl.Attributes.Add("style", "padding: 20px;padding-top: 8px;");
                htmlGenericControl.InnerHtml = studentAnswerList[i];

                tableCell.Controls.Add(htmlGenericControl);

                tableRow.Cells.Add(tableCell);
                table.Rows.Add(tableRow);
            }
            AnswerTablePlaceHolder.Controls.Add(table);
        }
    }
}
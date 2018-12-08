using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace OAS.Views.Lecturer
{
    public partial class ManageQuestion : System.Web.UI.Page
    {

        private static string connectionString = ConfigurationManager.ConnectionStrings["oasDB"].ConnectionString;
        private String[] assessment = new String[10];
        private String[] question = new String[4];
        private String[] option = new String[2];
        private List<String[]> questionList = new List<string[]>();
        private List<String[]> optionList = new List<string[]>();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["assessment"] != null)
            {
                assessment = Session["assessment"] as String[];
            }
            if (ViewState["questionList"] != null)
            {
                questionList = ViewState["questionList"] as List<String[]>;
            }
            if (ViewState["optionList"] != null)
            {
                questionList = ViewState["optionList"] as List<String[]>;
            }

            if (PreviousPage != null && PreviousPage.IsCrossPagePostBack)
            {
                assessment = (String[])Session["assessmentList" + Request.QueryString["i"].ToString()];
                Session["assessmentList" + Request.QueryString["i"].ToString()] = assessment;
            }

            Session["assessment"] = assessment;
            Session.Timeout = 1000;
            assessment = Session["assessment"] as String[];
            if (assessment[0] != null)
            {
                getQuestion(Guid.Parse(assessment[0]));
            }
            questionTable();

            if (!Page.IsCallback)
            {
                Message.Text = (String)Request.QueryString["Message"];
            }
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

        private void deleteQuestion(String questionId)
        {
            string deleteSql = "Delete From Question Where QuestionId = @QuestionId";

            using (SqlConnection con = new SqlConnection(connectionString))
            {
                con.Open();
                SqlCommand sqlCommand = new SqlCommand(deleteSql, con);
                sqlCommand.Parameters.AddWithValue("@QuestionId", Guid.Parse(questionId));
                sqlCommand.ExecuteNonQuery();
                con.Close();
            }
            Message.Text = "Deleted successfully.";

            Response.Redirect(Request.Url.GetLeftPart(UriPartial.Path) + "?Message=" + Message.Text);

        }

        protected void removeQuestion_OnClick(object sender, EventArgs e)
        {
            LinkButton linkButton = sender as LinkButton;
            deleteQuestion(linkButton.Text);
        }

        private int getCountOfAssignedQustion(Guid assessmentId)
        {
            int count;
            string selectSql = "Select Count(UserId) as count from Assessment a, Assignment ass where a.AssessmentId = ass.AssessmentId and a.AssessmentId = @AssessmentId ";

            using (SqlConnection con = new SqlConnection(connectionString))
            {
                con.Open();
                SqlCommand sqlCommand = new SqlCommand(selectSql, con);
                sqlCommand.Parameters.AddWithValue("@AssessmentId", assessmentId);
                SqlDataReader countRecords = sqlCommand.ExecuteReader();
                countRecords.Read();
                count = (int)countRecords["count"];
                con.Close();
                return count;
            }
        }

        private void questionTable()
        {
            String addQuestionLink;
            ViewState["assessment"] = assessment;

            Table table = new System.Web.UI.WebControls.Table();
            TableRow tableRow;
            TableCell tableCell;
            Table table2;
            TableRow tableRow2;
            TableCell tableCell2;

            HtmlGenericControl htmlGenericControl; HtmlGenericControl htmlGenericControl2; HtmlGenericControl htmlGenericControl3;
            LinkButton linkButton; HtmlGenericControl span;

            table.ID = "datatables";
            table.CssClass = "table table-striped table-bordered";
            table.Attributes.CssStyle.Add("width", "100%");

            tableRow = new TableRow();
            tableRow.TableSection = TableRowSection.TableHeader;
            tableCell = new TableCell();
            tableCell.Attributes.Add("style", "padding: 10px!important;");

            if (assessment[0] != null)
            {
                if (assessment[2].Trim() == "Written")
                {
                    addQuestionLink = "CreateWrittenQuestion.aspx";
                }
                else
                {
                    addQuestionLink = "CreateMCQuestion.aspx";
                }
                tableCell.Text = "Assessment : " + assessment[1] + " | Type : " + assessment[2].Trim() + " | Access : " + assessment[3] + " | Duration : " + assessment[4] + " (mins)" +
                                 " | CreatedDate : " + assessment[6] + "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<a runat=\"server\" title=\"Add new question\" style=\"all:inherit;font-size:13px;width:auto;line-height:0;height:fit-content;cursor:pointer;border: 1px solid rgba(0,0,0,0.2);display:inline-block\" href=\"" + addQuestionLink + "\"> add </a>" +
                                 "<span style=\"float:right\">Assigned to " + getCountOfAssignedQustion(Guid.Parse(assessment[0])) + " Student(s).</span>";
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
                htmlGenericControl.Attributes.Add("style", "background-color:#fff;padding: 20px;");

                htmlGenericControl2 = new HtmlGenericControl("div");
                table2 = new System.Web.UI.WebControls.Table(); table2.Attributes.Add("style", "width:100%");
                tableRow2 = new TableRow();
                tableCell2 = new TableCell(); tableCell2.Attributes.Add("style", "background-color: #fff;border:none;width:20%");

                htmlGenericControl3 = new HtmlGenericControl("div");
                htmlGenericControl3.InnerHtml = "<p>Question Image</p><img style=\"width:70%\" src=\"" + questionList[i][2] + "\"/>";
                //htmlGenericControl3.InnerHtml = "<div style=\"background-color:rgba(0,0,0,0.05);width:70%;height:100px;background-size: cover;background-position: center;line-height: 6;background-image:url('"+questionList[i][2]+"')\">Question Image</div> ";
                htmlGenericControl3.Attributes.Add("style", "background-color:;width:100%;");
                tableCell2.Controls.Add(htmlGenericControl3);
                tableRow2.Cells.Add(tableCell2);

                tableCell2 = new TableCell(); tableCell2.Attributes.Add("style", "background-color: #fff;border:none");
                htmlGenericControl3 = new HtmlGenericControl("div");
                htmlGenericControl3.InnerHtml += questionList[i][0];

                getOption(Guid.Parse(questionList[i][3]));

                int count = 0;
                for (int j = 0; j < optionList.Count; j++)
                {
                    if (optionList[j][0] != "<p>&nbsp;</p>")
                    {
                        if (optionList[j][1] == "True")
                        {
                            htmlGenericControl3.InnerHtml += "<div style=\"background-color:rgba(185,246,202,0.35);\" class=\"optionDiv\">" + "<span>&#" + (count + 9398) + "; <span>" + optionList[j][0] + "</div>";
                        }
                        else
                        {
                            htmlGenericControl3.InnerHtml += "<div class=\"optionDiv\">" + "<span>&#" + (count + 9398) + "; <span>" + optionList[j][0] + "</div>";
                        }
                        count++;
                    }
                }

                htmlGenericControl3.Attributes.Add("style", "background-color:;width:100%;");
                tableCell2.Controls.Add(htmlGenericControl3);
                tableRow2.Cells.Add(tableCell2);
                table2.Rows.Add(tableRow2);

                htmlGenericControl2.Controls.Add(table2);

                htmlGenericControl.Controls.Add(htmlGenericControl2);

                htmlGenericControl2 = new HtmlGenericControl("hr");
                htmlGenericControl.Controls.Add(htmlGenericControl2);
                span = new HtmlGenericControl("span");
                span.InnerHtml = "Edit";
                linkButton = new LinkButton();
                linkButton.Text = questionList[i][3];
                linkButton.Attributes.Add("style", "all:inherit;cursor:pointer;border: 1px solid rgba(0,0,0,0.2);display:inline-block");
                //linkButton.Click += new EventHandler(this.editQuestion_OnClick);
                linkButton.OnClientClick = "return false;";
                linkButton.Controls.Add(span);
                htmlGenericControl.Controls.Add(linkButton);

                span = new HtmlGenericControl("span");
                span.InnerHtml = "Delete";
                linkButton = new LinkButton();
                linkButton.Text = questionList[i][3];
                linkButton.Attributes.Add("style", "all:inherit;cursor:pointer;border: 1px solid rgba(0,0,0,0.2);display:inline-block");
                // Register the event-handling method for the OnClientClick event. 
                linkButton.Click += new EventHandler(this.removeQuestion_OnClick);
                linkButton.OnClientClick = "return confirm('Are you sure to delete Question " + (i + 1) + "?');";
                linkButton.Controls.Add(span);

                htmlGenericControl.Controls.Add(linkButton);
                tableCell.Controls.Add(htmlGenericControl);

                tableRow.Cells.Add(tableCell);
                table.Rows.Add(tableRow);
            }
            QuestionTablePlaceHolder.Controls.Add(table);
        }
    }
}
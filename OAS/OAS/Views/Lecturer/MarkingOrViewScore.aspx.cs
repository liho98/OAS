using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace OAS.Views.Lecturer
{
    public partial class MarkingOrViewScore : System.Web.UI.Page
    {

        private static string connectionString = ConfigurationManager.ConnectionStrings["oasDB"].ConnectionString;
        private String[] assessment = new String[10];
        private String[] studentAssessment = new String[4];
        private List<String[]> studentAssessmentList = new List<String[]>();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["assessment"] != null)
            {
                assessment = (String[])Session["assessment"];
                Session["assessment"] = assessment;
                Session.Timeout = 1000;

                getAllStudentByAssessment(Guid.Parse(assessment[0]));
                createStudentAssessmentTable();
            }

        }

        private void createStudentAssessmentTable()
        {

            Table table = new System.Web.UI.WebControls.Table();
            TableRow tableRow;
            TableCell tableCell;
            LinkButton linkButton; HtmlGenericControl htmlGenericControl;

            table.ID = "datatables";
            table.CssClass = "table table-striped table-bordered";
            table.Attributes.CssStyle.Add("width", "100%");

            tableRow = new TableRow();
            tableRow.TableSection = TableRowSection.TableHeader;
            tableCell = new TableCell();
            tableCell.Text = "Assessment : " + assessment[1] + " | Type : " + assessment[2].Trim();
            tableCell.ColumnSpan = 6;
            tableCell.Attributes.Add("style", "padding: 5px;padding-left: 13px;");
            tableRow.Cells.Add(tableCell);

            table.Rows.Add(tableRow);

            tableRow = new TableRow();
            tableRow.TableSection = TableRowSection.TableHeader;

            tableCell = new TableCell();
            tableCell.Text = "Student ID";
            tableRow.Cells.Add(tableCell);
            tableCell = new TableCell();
            tableCell.Text = "Student Name";
            tableRow.Cells.Add(tableCell);
            tableCell = new TableCell();
            tableCell.Text = "Student Programme";
            tableRow.Cells.Add(tableCell);
            tableCell = new TableCell();
            tableCell.Text = "Score";
            tableRow.Cells.Add(tableCell);
            tableCell = new TableCell();
            tableCell.Text = "Status";
            tableRow.Cells.Add(tableCell);
            tableCell = new TableCell();
            tableCell.Text = "Action";
            tableRow.Cells.Add(tableCell);

            table.Rows.Add(tableRow);


            for (int i = 0; i < studentAssessmentList.Count; i++)
            {
                tableRow = new TableRow();
                tableCell = new TableCell();
                tableCell.Text = studentAssessmentList[i][0];
                tableRow.Cells.Add(tableCell);
                tableCell = new TableCell();
                tableCell.Text = studentAssessmentList[i][1];
                tableRow.Cells.Add(tableCell);
                tableCell = new TableCell();
                tableCell.Text = studentAssessmentList[i][2];
                tableRow.Cells.Add(tableCell);
                tableCell = new TableCell();
                if (studentAssessmentList[i][3] != "")
                {
                    tableCell.Text = studentAssessmentList[i][3];
                }
                else
                {
                    tableCell.Text = "-";
                }
                tableRow.Cells.Add(tableCell);
                tableCell = new TableCell();

                if (isScored(Guid.Parse(assessment[0]), ((Guid)(Membership.GetUser(studentAssessmentList[i][0]).ProviderUserKey)))
                    && checkIsAnswered(Guid.Parse(assessment[0]), ((Guid)(Membership.GetUser(studentAssessmentList[i][0]).ProviderUserKey))))
                {
                    tableCell.Text = "Assignment Marked";
                }
                else if (isScored(Guid.Parse(assessment[0]), ((Guid)(Membership.GetUser(studentAssessmentList[i][0]).ProviderUserKey))) != true
                    && checkIsAnswered(Guid.Parse(assessment[0]), ((Guid)(Membership.GetUser(studentAssessmentList[i][0]).ProviderUserKey))))
                {
                    tableCell.Text = "Pending for marking";
                }
                else
                {
                    tableCell.Text = "Pending for answer";
                }

                tableRow.Cells.Add(tableCell);
                tableCell = new TableCell();
                tableCell.Text = "-";

                if (isScored(Guid.Parse(assessment[0]), ((Guid)(Membership.GetUser(studentAssessmentList[i][0]).ProviderUserKey))) != true
                    && checkIsAnswered(Guid.Parse(assessment[0]), ((Guid)(Membership.GetUser(studentAssessmentList[i][0]).ProviderUserKey))))
                {
                    htmlGenericControl = new HtmlGenericControl("span");
                    htmlGenericControl.InnerHtml = "<i style=\"font-size: 20px;\" class=\"material-icons\">assessment</i>";
                    htmlGenericControl.Attributes.Add("style", "");

                    linkButton = new LinkButton();
                    linkButton = new LinkButton();
                    linkButton.Attributes.Add("style", "all:unset;color:#007bff;cursor:pointer");
                    linkButton.ToolTip = "Mark Assignment";
                    linkButton.Text = i.ToString();
                    // Register the event-handling method for the CheckedChanged event. 
                    linkButton.Click += new EventHandler(this.markAssignment_OnClick);

                    linkButton.Controls.Add(htmlGenericControl);

                    tableCell.Controls.Add(linkButton);
                }

                tableRow.Cells.Add(tableCell);

                table.Rows.Add(tableRow);
            }

            StudentAssessmentTablePlaceHolder.Controls.Add(table);
        }

        protected void markAssignment_OnClick(object sender, EventArgs e)
        {
            LinkButton linkButton = sender as LinkButton;
            Session["assessment"] = assessment;
            Session.Timeout = 1000;

            Session.Add("studentAssessment", studentAssessmentList[Convert.ToInt16(linkButton.Text)]);
            Session.Timeout = 1000;

            Response.Redirect("~/Views/Lecturer/MarkAssignment.aspx");
        }

        private void getAllStudentByAssessment(Guid assessmentId)
        {
            string selectSql = "Select UserName, CONCAT(FirstName + ' ', LastName) As [Name], ProgCode, Score From Assessment a, Assignment ass, " +
                               "UserProfiles u , aspnet_Users au Where a.AssessmentId = ass.AssessmentId and ass.UserId = u.UserId and " +
                               "u.UserId = au.UserId and A.AssessmentId = @AssessmentId";

            using (SqlConnection con = new SqlConnection(connectionString))
            {
                con.Open();
                SqlCommand sqlCommand = new SqlCommand(selectSql, con);
                sqlCommand.Parameters.AddWithValue("@AssessmentId", assessmentId);
                SqlDataReader studentAssessmentRecords = sqlCommand.ExecuteReader();

                while (studentAssessmentRecords.Read())
                {
                    studentAssessment = new String[4];
                    studentAssessment[0] = studentAssessmentRecords["UserName"].ToString();
                    studentAssessment[1] = studentAssessmentRecords["Name"].ToString();
                    studentAssessment[2] = studentAssessmentRecords["ProgCode"].ToString();
                    studentAssessment[3] = studentAssessmentRecords["Score"].ToString();

                    studentAssessmentList.Add(studentAssessment);
                }
                con.Close();
            }
        }

        private bool isScored(Guid assessmentId, Guid userId)
        {
            bool isScored = false;
            string selectSql = "Select Score From Assignment Where AssessmentId = @AssessmentId and UserId = @UserId ";

            using (SqlConnection con = new SqlConnection(connectionString))
            {
                con.Open();
                SqlCommand sqlCommand = new SqlCommand(selectSql, con);
                sqlCommand.Parameters.AddWithValue("@AssessmentId", assessmentId);
                sqlCommand.Parameters.AddWithValue("@UserId", userId);
                SqlDataReader scoreRecords = sqlCommand.ExecuteReader();

                if (scoreRecords.Read())
                {
                    if (scoreRecords["Score"] != System.DBNull.Value)
                    {
                        isScored = true;
                    }
                }
                con.Close();
            }
            return isScored;
        }

        private bool checkIsAnswered(Guid assessmentId, Guid userId)
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
                sqlCommand.Parameters.AddWithValue("@UserId", userId);
                sqlCommand.Parameters.AddWithValue("@AssessmentId", assessmentId);
                SqlDataReader answerRecords = sqlCommand.ExecuteReader();

                if (answerRecords.Read())
                {
                    isAnswered = true;
                }
                con.Close();
            }
            return isAnswered;
        }
    }
}
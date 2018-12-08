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
    public partial class ManageAssessment : System.Web.UI.Page
    {
        private static string connectionString = ConfigurationManager.ConnectionStrings["oasDB"].ConnectionString;
        private String[] assessment;
        private List<String[]> assessmentList = new List<String[]>();
        private String[] contributor;
        private List<String[]> contributorList = new List<String[]>();

        protected void Page_Load(object sender, EventArgs e)
        {
            Message.Text = (String)Request.QueryString["Message"];
            GetAllAssessmentToList(); AssessmentTable();
        }

        private void GetAllAssessmentToList()
        {
            string selectSql = "Select * From Assessment a, Contributor c, UserProfiles u Where a.AssessmentId = c.AssessmentId and " +
                               "c.UserId = u.UserId and u.UserId = @UserId";

            using (SqlConnection con = new SqlConnection(connectionString))
            {
                con.Open();
                SqlCommand sqlCommand = new SqlCommand(selectSql, con);
                sqlCommand.Parameters.AddWithValue("@UserId", ((Guid)(Membership.GetUser(HttpContext.Current.User.Identity.Name).ProviderUserKey)));
                SqlDataReader assessmentRecords = sqlCommand.ExecuteReader();

                while (assessmentRecords.Read())
                {
                    assessment = new String[10];
                    assessment[0] = assessmentRecords["AssessmentId"].ToString();
                    assessment[1] = assessmentRecords["AssessmentTitle"].ToString();
                    assessment[2] = assessmentRecords["AssessmentType"].ToString();
                    assessment[3] = assessmentRecords["AssessmentAccess"].ToString();
                    assessment[4] = assessmentRecords["AssessmentDuration"].ToString();
                    assessment[5] = assessmentRecords["AssessmentDesc"].ToString();
                    assessment[6] = assessmentRecords["CreatedDate"].ToString();
                    assessment[7] = "";
                    assessment[8] = "";
                    assessment[9] = assessmentRecords["isHost"].ToString();
                    assessmentList.Add(assessment);
                }
                con.Close();
            }
            selectSql = "Select a.AssessmentId, FirstName, LastName, isHost From Assessment a, Contributor c, UserProfiles u Where a.AssessmentId = c.AssessmentId and " +
                               "c.UserId = u.UserId";

            using (SqlConnection con = new SqlConnection(connectionString))
            {
                con.Open();
                SqlCommand sqlCommand = new SqlCommand(selectSql, con);
                SqlDataReader assessmentRecords = sqlCommand.ExecuteReader();

                while (assessmentRecords.Read())
                {
                    contributor = new String[4];
                    contributor[0] = assessmentRecords["AssessmentId"].ToString();
                    contributor[1] = assessmentRecords["FirstName"].ToString();
                    contributor[2] = assessmentRecords["LastName"].ToString();
                    contributor[3] = assessmentRecords["isHost"].ToString();
                    contributorList.Add(contributor);
                }
                for (int i = 0; i < assessmentList.Count; i++)
                {
                    for (int j = 0; j < contributorList.Count; j++)
                    {
                        if (assessmentList[i][0] == contributorList[j][0] && contributorList[j][3] == "True")
                        {
                            assessmentList[i][7] += (contributorList[j][1] + " " + contributorList[j][2]);
                        }
                        if (assessmentList[i][0] == contributorList[j][0] && contributorList[j][3] == "False")
                        {
                            assessmentList[i][8] += (contributorList[j][1] + " " + contributorList[j][2] + ", ");
                        }
                    }
                    if (assessmentList[i][8] != "")
                    {
                        assessmentList[i][8] = assessmentList[i][8].Remove(assessmentList[i][8].Length - 2, 2);
                    }
                }
                con.Close();
            }
        }

        private void AssessmentTable()
        {
            Table table = new System.Web.UI.WebControls.Table();
            TableRow tableRow;
            TableCell tableCell;
            HtmlGenericControl span;
            LinkButton linkButton;

            table.ID = "datatables";
            table.CssClass = "table table-striped table-bordered";
            table.Attributes.CssStyle.Add("width", "100%");

            tableRow = new TableRow();
            tableRow.TableSection = TableRowSection.TableHeader;

            tableCell = new TableCell();
            tableCell.Text = "Title";
            tableRow.Cells.Add(tableCell);
            tableCell = new TableCell();
            tableCell.Text = "Type";
            tableRow.Cells.Add(tableCell);
            tableCell = new TableCell();
            tableCell.Text = "Access";
            tableRow.Cells.Add(tableCell);
            tableCell = new TableCell();
            tableCell.Text = "Duration (mins)";
            tableRow.Cells.Add(tableCell);
            tableCell = new TableCell();
            tableCell.Text = "Description";
            tableRow.Cells.Add(tableCell);
            tableCell = new TableCell();
            tableCell.Text = "Created or Updated Date";
            tableRow.Cells.Add(tableCell);
            tableCell = new TableCell();
            tableCell.Text = "Contributor";
            tableRow.Cells.Add(tableCell);
            tableCell = new TableCell();
            tableCell.Text = "Action";
            tableRow.Cells.Add(tableCell);

            table.Rows.Add(tableRow);

            for (int i = 0; i < assessmentList.Count; i++)
            {
                tableRow = new TableRow();
                tableCell = new TableCell();
                tableCell.Text = assessmentList[i][1];
                tableRow.Cells.Add(tableCell);
                tableCell = new TableCell();
                tableCell.Text = assessmentList[i][2];
                tableRow.Cells.Add(tableCell);
                tableCell = new TableCell();
                tableCell.Text = assessmentList[i][3];
                tableRow.Cells.Add(tableCell);
                tableCell = new TableCell();
                tableCell.Text = assessmentList[i][4];
                tableRow.Cells.Add(tableCell);
                tableCell = new TableCell();
                tableCell.Text = assessmentList[i][5];
                tableRow.Cells.Add(tableCell);
                tableCell = new TableCell();
                tableCell.Text = assessmentList[i][6];
                tableRow.Cells.Add(tableCell);
                tableCell = new TableCell();
                tableCell.Text = "Host : " + assessmentList[i][7] + "<br/>Partner : " + assessmentList[i][8];
                tableRow.Cells.Add(tableCell);

                tableCell = new TableCell();
                span = new HtmlGenericControl("span");
                span.InnerHtml = "clear";
                span.Attributes["class"] = "material-icons hvr-icon";

                linkButton = new LinkButton();
                linkButton.ID = "removeAssessment" + i;
                linkButton.ToolTip = "Remove Assessment";
                linkButton.Text = assessmentList[i][0];
                if (assessmentList[i][9] == "True")
                {
                    linkButton.CssClass = "actionButton hvr-icon-pulse";
                    // Register the event-handling method for the OnClientClick event. 
                    linkButton.Click += new EventHandler(this.removeAssessment_OnClick);
                    linkButton.OnClientClick = "return confirm('Are you sure to delete this Assessment " + assessmentList[i][1] + "?');";
                }
                else
                {
                    linkButton.CssClass = "actionButton"; linkButton.Attributes.CssStyle.Add("cursor", "not-allowed!important");
                    linkButton.Attributes.Add("onClick", "return false;");
                }
                linkButton.Controls.Add(span);

                tableCell.Controls.Add(linkButton);

                span = new HtmlGenericControl("span");
                span.InnerHtml = "assignment";
                span.Attributes["class"] = "material-icons hvr-icon";

                linkButton = new LinkButton();
                linkButton.ID = "manageQuestion" + i;
                linkButton.ToolTip = "Manage Question";
                linkButton.Text = assessmentList[i][0];
                linkButton.CssClass = "actionButton hvr-icon-pulse";
                // Register the event-handling method for the OnClientClick event. 
                Session["assessmentList" + i] = assessmentList[i];
                linkButton.PostBackUrl = "~/Views/Lecturer/ManageQuestion.aspx?i=" + i;

                linkButton.Controls.Add(span);

                tableCell.Controls.Add(linkButton);

                span = new HtmlGenericControl("span");
                span.InnerHtml = "edit";
                span.Attributes["class"] = "material-icons hvr-icon";

                linkButton = new LinkButton();

                linkButton.ID = "editAssessment" + i;
                linkButton.ToolTip = "Update Assessment";
                //linkButton.Attributes["value"] = assessmentList[i][0]; 
                if (assessmentList[i][9] == "True")
                {
                    linkButton.Attributes["class"] = "actionButton hvr-icon-pulse";
                    // Register the event-handling method for the OnClientClick event. 
                    Session["assessmentList" + i] = assessmentList[i];
                    linkButton.PostBackUrl = "~/Views/Lecturer/UpdateAssessment.aspx?i=" + i;
                }
                else
                {
                    linkButton.Attributes["class"] = "actionButton"; linkButton.Attributes.CssStyle.Add("cursor", "not-allowed!important");
                    linkButton.Attributes.Add("onClick", "return false;");
                }
                linkButton.Controls.Add(span);
                tableCell.Controls.Add(linkButton);
                tableRow.Cells.Add(tableCell);

                table.Rows.Add(tableRow);
            }
            AssessmentTablePlaceHolder.Controls.Add(table);
        }

        protected void removeAssessment_OnClick(object sender, EventArgs e)
        {
            LinkButton linkButton = sender as LinkButton;

            string deleteSql = "Delete From Assessment Where AssessmentId = @AssessmentId";

            using (SqlConnection con = new SqlConnection(connectionString))
            {
                con.Open();
                SqlCommand sqlCommand = new SqlCommand(deleteSql, con);
                sqlCommand.Parameters.AddWithValue("@AssessmentId", linkButton.Text);
                sqlCommand.ExecuteNonQuery();
                con.Close();
            }
            Message.Text = "Deleted successfully.";

            Response.Redirect(Request.Url.GetLeftPart(UriPartial.Path) + "?Message=" + Message.Text);
        }
    }
}
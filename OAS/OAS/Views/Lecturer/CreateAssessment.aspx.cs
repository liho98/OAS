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

namespace OAS.Views.Lecturers
{
    public partial class CreateAssessment : System.Web.UI.Page
    {
        private static string connectionString = ConfigurationManager.ConnectionStrings["oasDB"].ConnectionString;
        private String[] contributor;
        private List<String[]> contributorList = new List<String[]>();
        private String[] student;
        private List<String[]> studentList = new List<String[]>();

        private List<String> selectedContributorList = new List<String>();
        private List<String> selectedStudentList = new List<String>();

        private String Msg;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (ViewState["selectedContributorList"] != null)
            {
                selectedContributorList = ViewState["selectedContributorList"] as List<String>;
            }
            if (ViewState["selectedStudentList"] != null)
            {
                selectedStudentList = ViewState["selectedStudentList"] as List<String>;
            }

            if (Request.QueryString["Msg"] != null)
            {
                Msg = Request.QueryString["Msg"] as String;
                MessageLabel.ForeColor = System.Drawing.Color.Green;
                MessageLabel.Text = Msg;
            }

            if (Page.IsPostBack)
            {
                MessageLabel.Text = "";
            }

            GetAllLecturerToList();
            ContributorTable();
            GetAllStudentToList(); StudentTable();
        }
        protected void AssessmentAccess_OnChanged(object sender, EventArgs e)
        {
            if (AccessmentAccessRadioList.SelectedValue == "Private")
            {
                Page.ClientScript.RegisterStartupScript(this.GetType(), "Registered Script", "swapDiv('popupBox');", true);
                ViewState["selectedStudentList"] = selectedStudentList;
            }
        }
        protected void Select_OnClick(object sender, EventArgs e)
        {
            Page.ClientScript.RegisterStartupScript(this.GetType(), "Registered Script", "swapDiv('return');", true);
        }

        protected void CreateButton_OnClick(object sender, EventArgs e)
        {
            try
            {
                Guid assessmentId = Guid.NewGuid();
                string insertSql = "INSERT INTO Assessment(AssessmentId, AssessmentTitle, AssessmentType, AssessmentAccess, AssessmentDuration, AssessmentDesc, CreatedDate)" +
                    "VALUES(@AssessmentId, @AssessmentTitle, @AssessmentType, @AssessmentAccess, @AssessmentDuration, @AssessmentDesc, @CreateDate)";

                using (SqlConnection con = new SqlConnection(connectionString))
                {
                    con.Open();
                    SqlCommand sqlCommand = new SqlCommand(insertSql, con);
                    sqlCommand.Parameters.AddWithValue("@AssessmentId", assessmentId);
                    sqlCommand.Parameters.AddWithValue("@AssessmentTitle", TitleTextBox.Text);
                    sqlCommand.Parameters.AddWithValue("@AssessmentType", AccessmentTypeRadioList.SelectedValue);
                    sqlCommand.Parameters.AddWithValue("@AssessmentAccess", AccessmentAccessRadioList.SelectedValue);
                    sqlCommand.Parameters.AddWithValue("@AssessmentDuration", Convert.ToInt16(DurationDropDownList.SelectedValue.Substring(0,2)));
                    sqlCommand.Parameters.AddWithValue("@AssessmentDesc", DescriptionTextArea.Text);
                    sqlCommand.Parameters.AddWithValue("@CreateDate", DateTime.Now);
                    sqlCommand.ExecuteNonQuery();
                    con.Close();
                }

                AssignAssessment(assessmentId);

                insertSql = "INSERT INTO Contributor(UserId, AssessmentId, isHost)" +
                    "VALUES(@UserId, @AssessmentId, @isHost)";

                using (SqlConnection con = new SqlConnection(connectionString))
                {

                    for (int i = 0; i < selectedContributorList.Count; i++)
                    {
                        con.Open();
                        SqlCommand sqlCommand = new SqlCommand(insertSql, con);

                        sqlCommand.Parameters.AddWithValue("@UserId", (Guid)(Membership.GetUser(selectedContributorList[i])).ProviderUserKey);
                        sqlCommand.Parameters.AddWithValue("@AssessmentId", assessmentId);
                        if (selectedContributorList[i].ToLower() == HttpContext.Current.User.Identity.Name.ToLower())
                        {
                            sqlCommand.Parameters.AddWithValue("@isHost", Convert.ToByte(true));
                        }
                        else
                        {
                            sqlCommand.Parameters.AddWithValue("@isHost", Convert.ToByte(false));

                        }
                        sqlCommand.ExecuteNonQuery();
                        con.Close();
                    }

                }

                Response.Redirect(Request.Url.GetLeftPart(UriPartial.Path) + "?Msg=Assessment created successfully.");
            }
            catch (Exception ex)
            {
                MessageLabel.ForeColor = System.Drawing.Color.Red;
                MessageLabel.Text = "Assessment cannot be created. Check again your details." + ex.ToString();
            }
        }
        private void AssignAssessment(Guid assessmentId)
        {
            String insertSql = "INSERT INTO Assignment(AssessmentId, UserId, Score)" +
                "VALUES(@AssessmentId, @UserId, @Score)";
            SqlCommand sqlCommand;

            using (SqlConnection con = new SqlConnection(connectionString))
            {
                if (AccessmentAccessRadioList.SelectedValue == "Private")
                {
                    for (int i = 0; i < selectedStudentList.Count; i++)
                    {
                        con.Open();
                        sqlCommand = new SqlCommand(insertSql, con);

                        sqlCommand.Parameters.AddWithValue("@AssessmentId", assessmentId);
                        sqlCommand.Parameters.AddWithValue("@UserId", (Guid)(Membership.GetUser(selectedStudentList[i])).ProviderUserKey);
                        sqlCommand.Parameters.AddWithValue("@Score", System.DBNull.Value);

                        sqlCommand.ExecuteNonQuery();
                        con.Close();
                    }
                }
                else
                {
                    for (int i = 0; i < studentList.Count; i++)
                    {
                        con.Open();
                        sqlCommand = new SqlCommand(insertSql, con);

                        sqlCommand.Parameters.AddWithValue("@AssessmentId", assessmentId);
                        sqlCommand.Parameters.AddWithValue("@UserId", (Guid)(Membership.GetUser(studentList[i][0])).ProviderUserKey);
                        sqlCommand.Parameters.AddWithValue("@Score", System.DBNull.Value);

                        sqlCommand.ExecuteNonQuery();
                        con.Close();
                    }
                }
            }
        }

        protected void ContributorCheckBox_OnChanged(object sender, EventArgs e)
        {
            CheckBox CheckBox = sender as CheckBox;
            if (CheckBox.Checked)
            {

                selectedContributorList.Add(CheckBox.Text);
            }
            else
            {
                selectedContributorList.Remove(CheckBox.Text);
            }
        }

        protected void GetAllLecturerToList()
        {
            string selectSql = "Select FirstName,LastName, UserName From UserProfiles up, aspnet_Users au, aspnet_UsersInRoles ur, aspnet_Roles ar " +
                    "where up.UserId = au.UserId and au.UserId = ur.UserId and ur.RoleId = ar.RoleId and (ar.RoleName = 'Lecturers' or ar.RoleName = 'Administrators')";

            using (SqlConnection con = new SqlConnection(connectionString))
            {
                con.Open();
                SqlCommand sqlCommand = new SqlCommand(selectSql, con);
                SqlDataReader lecturerRecords = sqlCommand.ExecuteReader();

                while (lecturerRecords.Read())
                {
                    contributor = new String[2];
                    contributor[0] = lecturerRecords["UserName"].ToString();
                    contributor[1] = lecturerRecords["FirstName"].ToString() + " " + lecturerRecords["LastName"].ToString();
                    contributorList.Add(contributor);
                }
                con.Close();
            }
        }

        protected void ContributorTable()
        {
            Table table = new System.Web.UI.WebControls.Table();
            TableRow tableRow;
            TableCell tableCell;
            CheckBox checkBox;

            table.ID = "datatables";
            table.CssClass = "table table-striped table-bordered";
            table.Attributes.CssStyle.Add("width", "100%");

            tableRow = new TableRow();
            tableRow.TableSection = TableRowSection.TableHeader;

            tableCell = new TableCell();
            tableCell.Text = "Lecturer ID";
            tableRow.Cells.Add(tableCell);
            tableCell = new TableCell();
            tableCell.Text = "Lecturer Name";
            tableRow.Cells.Add(tableCell);
            tableCell = new TableCell();
            tableRow.Cells.Add(tableCell);
            table.Rows.Add(tableRow);

            for (int i = 0; i < contributorList.Count; i++)
            {
                tableRow = new TableRow();
                tableCell = new TableCell();
                tableCell.Text = contributorList[i][0];
                tableRow.Cells.Add(tableCell);
                tableCell = new TableCell();
                tableCell.Text = contributorList[i][1];
                tableRow.Cells.Add(tableCell);

                tableCell = new TableCell();
                checkBox = new CheckBox();
                checkBox.ID = "LecturerCheckBox" + i;
                checkBox.Text = contributorList[i][0]; checkBox.LabelAttributes.CssStyle.Add("display", "none");
                checkBox.Attributes.Add("value", contributorList[i][0]);
                checkBox.AutoPostBack = true;
                if (contributorList[i][0].ToLower() == HttpContext.Current.User.Identity.Name.ToLower())
                {
                    checkBox.Checked = true;
                    checkBox.Enabled = false;
                    checkBox.InputAttributes.CssStyle.Add("cursor", "not-allowed");
                    if (!selectedContributorList.Contains(contributorList[i][0]))
                    {
                        selectedContributorList.Add(contributorList[i][0]);
                        ViewState["selectedContributorList"] = selectedContributorList;
                    }
                }
                // Register the event-handling method for the CheckedChanged event. 
                checkBox.CheckedChanged += new EventHandler(this.ContributorCheckBox_OnChanged);
                tableCell.Controls.Add(checkBox);
                tableRow.Cells.Add(tableCell);

                table.Rows.Add(tableRow);

            }
            ContributorTablePlaceHolder.Controls.Add(table);
        }

        protected void GetAllStudentToList()
        {
            string selectSql = "Select FirstName,LastName, UserName From UserProfiles up, aspnet_Users au, aspnet_UsersInRoles ur, aspnet_Roles ar " +
                    "where up.UserId = au.UserId and au.UserId = ur.UserId and ur.RoleId = ar.RoleId and ar.RoleName = 'Students'";

            using (SqlConnection con = new SqlConnection(connectionString))
            {
                con.Open();
                SqlCommand sqlCommand = new SqlCommand(selectSql, con);
                SqlDataReader studentRecords = sqlCommand.ExecuteReader();

                while (studentRecords.Read())
                {
                    student = new String[2];
                    student[0] = studentRecords["UserName"].ToString();
                    student[1] = studentRecords["FirstName"].ToString() + " " + studentRecords["LastName"].ToString();
                    studentList.Add(student);
                }
                con.Close();
            }
        }
        protected void StudentCheckBox_OnChanged(object sender, EventArgs e)
        {
            Page.ClientScript.RegisterStartupScript(this.GetType(), "Registered Script", "swapDiv('popupBox');", true);

            CheckBox CheckBox = sender as CheckBox;
            if (CheckBox.Checked)
            {
                selectedStudentList.Add(CheckBox.Text);
            }
            else
            {
                selectedStudentList.Remove(CheckBox.Text);
            }
        }

        protected void StudentTable()
        {
            Table table = new System.Web.UI.WebControls.Table();
            TableRow tableRow;
            TableCell tableCell;
            CheckBox checkBox;

            table.ID = "datatables2";
            table.CssClass = "table table-striped table-bordered";
            table.Attributes.CssStyle.Add("width", "100%");

            tableRow = new TableRow();
            tableRow.TableSection = TableRowSection.TableHeader;

            tableCell = new TableCell();
            tableCell.Text = "Student ID";
            tableRow.Cells.Add(tableCell);
            tableCell = new TableCell();
            tableCell.Text = "Student Name";
            tableRow.Cells.Add(tableCell);
            tableCell = new TableCell();
            tableRow.Cells.Add(tableCell);
            table.Rows.Add(tableRow);

            for (int i = 0; i < studentList.Count; i++)
            {
                tableRow = new TableRow();
                tableCell = new TableCell();
                tableCell.Text = studentList[i][0];
                tableRow.Cells.Add(tableCell);
                tableCell = new TableCell();
                tableCell.Text = studentList[i][1];
                tableRow.Cells.Add(tableCell);

                tableCell = new TableCell();
                checkBox = new CheckBox();
                checkBox.ID = "StudentCheckBox" + i;
                checkBox.Text = studentList[i][0]; checkBox.LabelAttributes.CssStyle.Add("display", "none");
                checkBox.Attributes.Add("value", studentList[i][0]);
                checkBox.AutoPostBack = true;

                // Register the event-handling method for the CheckedChanged event. 
                checkBox.CheckedChanged += new EventHandler(this.StudentCheckBox_OnChanged);
                tableCell.Controls.Add(checkBox);
                tableRow.Cells.Add(tableCell);

                table.Rows.Add(tableRow);
            }
            StudentTablePlaceHolder.Controls.Add(table);
        }

    }
}
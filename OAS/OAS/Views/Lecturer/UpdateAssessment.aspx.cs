using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace OAS.Views.Lecturer
{
    public partial class UpdateAssessment : System.Web.UI.Page
    {
        private static string connectionString = ConfigurationManager.ConnectionStrings["oasDB"].ConnectionString;
        private String[] contributor;
        private List<String[]> contributorList = new List<String[]>();
        private String[] student;
        private List<String[]> studentList = new List<String[]>();

        private List<String> selectedContributorList = new List<String>();
        private List<String> selectedStudentList = new List<String>();
        private List<String> newSelectedStudentList = new List<String>();
        private List<String> tempList = new List<String>();

        private String Msg;

        String[] assessment; String x;

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
            if (ViewState["newSelectedStudentList"] != null)
            {
                newSelectedStudentList = ViewState["newSelectedStudentList"] as List<String>;
            }
            if (ViewState["assessment"] != null)
            {
                assessment = ViewState["assessment"] as String[];
            }

            GetAllLecturerToList();
            ContributorTable();
            GetAllStudentToList();
            StudentTable();

            if (PreviousPage != null && PreviousPage.IsCrossPagePostBack)
            {
                assessment = (String[])Session["assessmentList" + Request.QueryString["i"].ToString()]; Session.Remove("assessmentList" + Request.QueryString["i"].ToString());
                TitleTextBox.Text = assessment[1];
                AccessmentTypeRadioList.SelectedValue = assessment[2];
                AccessmentAccessRadioList.SelectedValue = assessment[3];
                DurationDropDownList.SelectedValue = assessment[4] + " (mins)";
                DescriptionTextArea.Text = assessment[5];
                getAssessmentDetails(assessment[0]);
                ViewState["assessment"] = assessment;
                //ViewState["selectedStudentList"] = selectedStudentList;
            }
            AccessmentTypeRadioList.Enabled = false;
            AccessmentTypeRadioList.ToolTip = "You are not able to chnage the Assessment type. Since there might have question is create for the particular type.";

            if (Request.QueryString["Msg"] != null)
            {
                Msg = Request.QueryString["Msg"] as String;
                MessageLabel.ForeColor = System.Drawing.Color.Green;
                MessageLabel.Text = Msg;
            }

            if (Page.IsPostBack)
            {
                //MessageLabel.Text = "";
            }
        }

        private void getAssessmentDetails(String assessmentId)
        {
            string selectSql = "Select au.UserName, isHost From Assessment a, Contributor c, UserProfiles u, " +
                    "aspnet_Users au Where a.AssessmentId = c.AssessmentId and a.AssessmentId = '" + assessmentId + "' and " +
                    "c.UserId = u.UserId and u.UserId = au.UserId";

            using (SqlConnection con = new SqlConnection(connectionString))
            {
                con.Open();
                SqlCommand sqlCommand = new SqlCommand(selectSql, con);
                SqlDataReader ContributorRecords = sqlCommand.ExecuteReader();

                while (ContributorRecords.Read())
                {
                    if (ContributorRecords["isHost"].ToString() == "False")
                    {
                        selectedContributorList.Add(ContributorRecords["UserName"].ToString());
                    }
                }
                con.Close();
            }
            //ViewState["selectedContributorList"] = selectedContributorList;

            selectSql = "Select au.UserName From Assessment a, Assignment ass, UserProfiles up, aspnet_Users au " +
                "Where a.AssessmentId = ass.AssessmentId and ass.AssessmentId = '" + assessmentId + "' " +
                "and ass.UserId = up.UserId and up.UserId = au.UserId";

            using (SqlConnection con = new SqlConnection(connectionString))
            {
                con.Open();
                SqlCommand sqlCommand = new SqlCommand(selectSql, con);
                SqlDataReader AssignmentRecords = sqlCommand.ExecuteReader();

                while (AssignmentRecords.Read())
                {
                    selectedStudentList.Add(AssignmentRecords["UserName"].ToString());
                }
                con.Close();
            }
            newSelectedStudentList = selectedStudentList;
            ViewState["newSelectedStudentList"] = newSelectedStudentList;
            ViewState["selectedStudentList"] = selectedStudentList;
            setContributorCheckBox(); setStudentCheckBox();
        }
        private void setContributorCheckBox()
        {
            CheckBox ContributorCheckBox;
            for (int i = 0; i < contributorList.Count; i++)
            {
                ContributorCheckBox = (CheckBox)ContributorTablePlaceHolder.FindControl("LecturerCheckBox" + i);
                for (int j = 0; j < selectedContributorList.Count; j++)
                {
                    if (contributorList[i][0] == selectedContributorList[j])
                    {
                        ContributorCheckBox.Checked = true;
                        break;
                    }
                }
            }
        }
        private void setStudentCheckBox()
        {
            CheckBox StudentCheckBox;
            for (int i = 0; i < studentList.Count; i++)
            {
                StudentCheckBox = (CheckBox)StudentTablePlaceHolder.FindControl("StudentCheckBox" + i);
                for (int j = 0; j < selectedStudentList.Count; j++)
                {
                    if (studentList[i][0] == selectedStudentList[j])
                    {
                        StudentCheckBox.Checked = true;
                        break;
                    }
                }
            }
        }

        protected void AssessmentAccess_OnChanged(object sender, EventArgs e)
        {
            if (AccessmentAccessRadioList.SelectedValue == "Private")
            {
                Page.ClientScript.RegisterStartupScript(this.GetType(), "Registered Script", "swapDiv('popupBox');", true);
                //ViewState["selectedStudentList"] = selectedStudentList;
            }
        }
        protected void Select_OnClick(object sender, EventArgs e)
        {
            Page.ClientScript.RegisterStartupScript(this.GetType(), "Registered Script", "swapDiv('return');", true);
        }

        protected void updateButton_OnClick(object sender, EventArgs e)
        {
            try
            {

                Guid assessmentId = Guid.Parse(assessment[0]);
                string updateSql = "UPDATE [dbo].[Assessment] SET AssessmentTitle = @AssessmentTitle, AssessmentType = @AssessmentType, " +
                        "AssessmentAccess = @AssessmentAccess, AssessmentDuration = @AssessmentDuration, AssessmentDesc = @AssessmentDesc, " +
                        "CreatedDate = @CreatedDate WHERE AssessmentId = @AssessmentId";

                using (SqlConnection con = new SqlConnection(connectionString))
                {
                    con.Open();
                    SqlCommand sqlCommand = new SqlCommand(updateSql, con);
                    sqlCommand.Parameters.AddWithValue("@AssessmentTitle", TitleTextBox.Text);
                    sqlCommand.Parameters.AddWithValue("@AssessmentType", AccessmentTypeRadioList.SelectedValue);
                    sqlCommand.Parameters.AddWithValue("@AssessmentAccess", AccessmentAccessRadioList.SelectedValue);
                    sqlCommand.Parameters.AddWithValue("@AssessmentDuration", Convert.ToInt16(DurationDropDownList.SelectedValue.Substring(0, 2)));
                    sqlCommand.Parameters.AddWithValue("@AssessmentDesc", DescriptionTextArea.Text);
                    sqlCommand.Parameters.AddWithValue("@CreatedDate", DateTime.Now);
                    sqlCommand.Parameters.AddWithValue("@AssessmentId", assessmentId);
                    sqlCommand.ExecuteNonQuery();
                    con.Close();
                }

                tempList = selectedStudentList.Except(newSelectedStudentList).ToList();

                for (int i = 0; i < tempList.Count; i++)
                {
                    deleteUsersFromAnswer(assessmentId.ToString(), ((Guid)(Membership.GetUser(tempList[i])).ProviderUserKey).ToString());
                    deleteUsersFromAssignment(assessmentId, (Guid)(Membership.GetUser(tempList[i])).ProviderUserKey);
                }
                AssignAssessment(assessmentId);

                deleteUsersFromContributor(assessmentId);

                string insertSql = "INSERT INTO Contributor(UserId, AssessmentId, isHost)" +
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

                Response.Redirect(Request.Url.GetLeftPart(UriPartial.Path) + "?Msg=Assessment updated successfully.");
            }
            catch (Exception ex)
            {
                MessageLabel.ForeColor = System.Drawing.Color.Red;
                MessageLabel.Text = "Assessment cannot be updated. Check again your details." + ex.ToString();
            }
        }

        private void deleteUsersFromAssignment(Guid assessmentId, Guid userId)
        {
            string deleteSql = "Delete From Assignment Where AssessmentId = @AssessmentId and UserId = @UserId";

            using (SqlConnection con = new SqlConnection(connectionString))
            {
                con.Open();
                SqlCommand sqlCommand = new SqlCommand(deleteSql, con);
                sqlCommand.Parameters.AddWithValue("@AssessmentId", assessmentId);
                sqlCommand.Parameters.AddWithValue("@UserId", userId);
                sqlCommand.ExecuteNonQuery();
                con.Close();
            }
        }

        private void deleteUsersFromAnswer(String assessmentId, String userId)
        {
            string deleteSql = "Delete from Answer where QuestionId in ( " +
            "Select q.QuestionId from UserProfiles up, Assignment ass, Answer ans, Assessment a, Question q " +
            "Where up.UserId = ass.UserId and up.UserId = ans.UserId and ass.AssessmentId = a.AssessmentId " +
            "and q.AssessmentId = a.AssessmentId and q.QuestionId = ans.QuestionId and " +
            "up.UserId = '" + userId + "' " +
            "and ass.AssessmentId = '" + assessmentId + "') and UserId = '" + userId + "'";

            using (SqlConnection con = new SqlConnection(connectionString))
            {
                con.Open();
                SqlCommand sqlCommand = new SqlCommand(deleteSql, con);
                sqlCommand.ExecuteNonQuery();
                con.Close();
            }
        }

        private void deleteUsersFromContributor(Guid assessmentId)
        {
            string deleteSql = "Delete From Contributor Where AssessmentId = @AssessmentId";

            using (SqlConnection con = new SqlConnection(connectionString))
            {
                con.Open();
                SqlCommand sqlCommand = new SqlCommand(deleteSql, con);
                sqlCommand.Parameters.AddWithValue("@AssessmentId", assessmentId);
                sqlCommand.ExecuteNonQuery();
                con.Close();
            }
        }

        private void AssignAssessment(Guid assessmentId)
        {
            String insertSql = "INSERT INTO Assignment(AssessmentId, UserId, Score)" +
                "VALUES(@AssessmentId, @UserId, @Score)";
            SqlCommand sqlCommand;

            tempList = newSelectedStudentList.Except(selectedStudentList).ToList();

            using (SqlConnection con = new SqlConnection(connectionString))
            {
                if (AccessmentAccessRadioList.SelectedValue == "Private")
                {
                    for (int i = 0; i < tempList.Count; i++)
                    {
                        con.Open();
                        sqlCommand = new SqlCommand(insertSql, con);

                        sqlCommand.Parameters.AddWithValue("@AssessmentId", assessmentId);
                        sqlCommand.Parameters.AddWithValue("@UserId", (Guid)(Membership.GetUser(tempList[i])).ProviderUserKey);
                        sqlCommand.Parameters.AddWithValue("@Score", System.DBNull.Value);

                        sqlCommand.ExecuteNonQuery();
                        con.Close();
                    }
                }
                else
                {
                    tempList.Clear();
                    for (int i = 0; i < studentList.Count; i++)
                    {
                        tempList.Add(studentList[i][0]);
                    }

                    tempList = tempList.Except(selectedStudentList).ToList();

                    for (int i = 0; i < tempList.Count; i++)
                    {
                        con.Open();
                        sqlCommand = new SqlCommand(insertSql, con);

                        sqlCommand.Parameters.AddWithValue("@AssessmentId", assessmentId);
                        sqlCommand.Parameters.AddWithValue("@UserId", (Guid)(Membership.GetUser(tempList[i])).ProviderUserKey);
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
                newSelectedStudentList.Add(CheckBox.Text);
            }
            else
            {
                newSelectedStudentList.Remove(CheckBox.Text);
            }
            ViewState["newSelectedStudentList"] = newSelectedStudentList;
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
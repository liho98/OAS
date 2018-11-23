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

namespace OAS.Views
{
    public partial class Profile : System.Web.UI.Page
    {

        private static string connectionString = ConfigurationManager.ConnectionStrings["oasDB"].ConnectionString;
        //Get the UserId of the just-added user
        Guid UserId = (Guid)(Membership.GetUser(HttpContext.Current.User.Identity.Name.ToUpper())).ProviderUserKey;
        private String[] assignment = new String[4];
        private List<String[]> assignmentList = new List<String[]>();
        private String[] assessment;
        private List<String[]> assessmentList = new List<String[]>();
        private String[] contributor;
        private List<String[]> contributorList = new List<String[]>();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                ViewState.Clear();
                getProfile();
                message.Text = (String)Request.QueryString["Message"];
            }
            if (User.IsInRole("Students"))
            {
                getAssignment();
                createAssignmentTable();
            }
            else
            {
                GetAllAssessmentToList();
                createAssessmentTable();
            }
        }

        private void getProfile()
        {
            SqlConnection con = new SqlConnection(connectionString);

            string selectSql = "Select FirstName,LastName,Gender,ContactNo,DateOfBirth,Status,Position,ProgCode,Image, Email " +
                    "from [dbo].[UserProfiles] u, [dbo].[aspnet_Membership] m where u.UserId = '" + UserId.ToString() + "' and m.UserId = '" + UserId.ToString() + "'";

            con.Open();
            SqlCommand sqlCommand = new SqlCommand(selectSql, con);
            SqlDataReader userRecords = sqlCommand.ExecuteReader();
            String title;

            userRecords.Read();
            if (userRecords["Gender"].ToString() == "M")
            {
                title = "Mr. ";
            }
            else
            {
                title = "Ms. ";
            }

            String showRoles = "";
            String[] userRoles;
            userRoles = Roles.GetRolesForUser(User.Identity.Name);
            for (int j = 0; j < userRoles.Length; j++)
            {
                if (userRoles.Length != 0)
                {
                    userRoles[j] = userRoles[j].Remove(userRoles[j].Length - 1, 1);
                }
                showRoles = showRoles + userRoles[j] + ", ";
            }
            if (showRoles.Length != 0)
            {
                showRoles = showRoles.Remove(showRoles.Length - 2, 2);
            }

            Role.InnerText = "Role : " + showRoles;
            Name.InnerText = title + userRecords["FirstName"].ToString() + " " + userRecords["LastName"].ToString();
            StatusText.InnerText = userRecords["Status"].ToString();

            userAvartar.Attributes.CssStyle.Add("background-image", "url('" + Encoding.Default.GetString((byte[])userRecords["Image"]) + "')");
            UserIdText.Text = HttpContext.Current.User.Identity.Name.ToUpper();
            FirstNametext.Text = userRecords["FirstName"].ToString();
            LastNameText.Text = userRecords["LastName"].ToString();
            EmailText.Text = userRecords["Email"].ToString();
            PhoneText.Text = userRecords["ContactNo"].ToString();

            for (int i = 0; i < ProgrammeDropDownList.Items.Count; i++)
            {
                if (ProgrammeDropDownList.Items[i].Value == userRecords["ProgCode"].ToString())
                {
                    ProgrammeDropDownList.Items[i].Selected = true;
                }
            }
            for (int i = 0; i < PositionDropDownList.Items.Count; i++)
            {
                if (PositionDropDownList.Items[i].Value == userRecords["Position"].ToString())
                {
                    PositionDropDownList.Items[i].Selected = true;
                }
            }
            DateOfBirthText.Text = String.Format("{0:yyy-MM-dd}", ((DateTime)userRecords["DateOfBirth"]));
            con.Close();
        }
        protected void editProfileButton_OnClick(object sender, EventArgs e)
        {
            message.Text = "";
            if (editProfileButton.Text == "Edit Profile")
            {
                editProfileButton.Text = "Cancel Edit";

                updateButton.Visible = true;
                FirstNametext.Enabled = true;
                FirstNametext.Focus();
                LastNameText.Enabled = true;
                EmailText.Enabled = true;
                PhoneText.Enabled = true;
                //ProgrammeDropDownList.Enabled = true;
                //PositionDropDownList.Enabled = true;
                DateOfBirthText.Enabled = true;
                changePhoto.Visible = true;

                FirstNametext.Attributes["class"] = "inputBox";
                LastNameText.Attributes["class"] = "inputBox";
                EmailText.Attributes["class"] = "inputBox";
                PhoneText.Attributes["class"] = "inputBox";
                //ProgrammeDropDownList.Attributes["class"] = "inputBox";
                //PositionDropDownList.Attributes["class"] = "inputBox";
                DateOfBirthText.Attributes["class"] = "inputBox";
            }
            else
            {
                editProfileButton.Text = "Edit Profile";

                updateButton.Visible = false;
                UserIdText.Enabled = false;
                FirstNametext.Enabled = false;
                LastNameText.Enabled = false;
                EmailText.Enabled = false;
                PhoneText.Enabled = false;
                //ProgrammeDropDownList.Enabled = false;
                //PositionDropDownList.Enabled = false;
                DateOfBirthText.Enabled = false;
                changePhoto.Visible = false;
            }
            Page.ClientScript.RegisterStartupScript(this.GetType(), "Registered Script", "swapDiv('About');", true);
        }
        protected void updateProfileButton_OnClick(object sender, EventArgs e)
        {
            /*
            string updateSql = "UPDATE [dbo].[UserProfiles] SET FirstName = @FirstName, LastName = @LastName, " +
                    "ContactNo = @ContactNo, ProgCode = @ProgCode, Position = @Position," +
                    "DateOfBirth = @DateOfBirth WHERE UserId = @UserId";
            */
            /*
            String imageUrl = "data:" + fileMimeType + ";base64," + Convert.ToBase64String(userAvatarUpload.FileBytes);
            updateCommand.Parameters.AddWithValue("@FirstName", FirstNametext.Text);
            updateCommand.Parameters.AddWithValue("@LastName", LastNameText.Text);
            updateCommand.Parameters.AddWithValue("@ContactNo", PhoneText.Text);
            updateCommand.Parameters.AddWithValue("@ProgCode", ProgrammeDropDownList.SelectedValue);
            updateCommand.Parameters.AddWithValue("@Position", PositionDropDownList.SelectedValue);
            updateCommand.Parameters.AddWithValue("@Image", Encoding.Default.GetBytes(imageUrl));
            updateCommand.Parameters.AddWithValue("@DateOfBirth", DateOfBirthText.Text);
            updateCommand.Parameters.AddWithValue("@UserId", UserId);
            */

            string fileName = userAvatarUpload.PostedFile.FileName.ToLower();
            string fileExtension = System.IO.Path.GetExtension(fileName);
            string fileMimeType = userAvatarUpload.PostedFile.ContentType;

            string[] matchExtension = { ".jpg", ".jpeg", ".png", ".gif" };
            string[] matchMimeType = { "image/jpg", "image/jpeg", "image/png", "image/gif" };

            ViewState.Clear();

            try
            {
                String updateSql = "";
                SqlConnection con = new SqlConnection(connectionString);

                if (userAvatarUpload.HasFile)
                {
                    if (matchExtension.Contains(fileExtension) && matchMimeType.Contains(fileMimeType))
                    {
                        String imageUrl = "data:" + fileMimeType + ";base64," + Convert.ToBase64String(userAvatarUpload.FileBytes);

                        updateSql = "UPDATE [dbo].[UserProfiles] SET FirstName = '" + FirstNametext.Text + "', LastName = '" + LastNameText.Text + "', " +
                            "ContactNo = '" + PhoneText.Text + "', " +
                            "DateOfBirth = '" + DateOfBirthText.Text + "'," +
                            "Image = '" + imageUrl + "' WHERE UserId = '" + UserId.ToString() + "'";
                    }
                    else
                    {
                        message.Text = "Upload status: Only jpg, jpeg, png or gif file is accepted!";
                        return;
                    }
                }
                else
                {
                    updateSql = "UPDATE [dbo].[UserProfiles] SET FirstName = '" + FirstNametext.Text + "', LastName = '" + LastNameText.Text + "', " +
                        "ContactNo = '" + PhoneText.Text + "', " +
                      "DateOfBirth = '" + DateOfBirthText.Text + "' WHERE UserId = '" + UserId.ToString() + "'";
                }
                // GetUser() without parameter returns the current logged in user.
                MembershipUser user = Membership.GetUser();
                user.Email = EmailText.Text;
                Membership.UpdateUser(user);

                con.Open();
                SqlCommand updateCommand = new SqlCommand(updateSql, con);
                updateCommand.ExecuteNonQuery();
                con.Close();

                message.Text = "You have successfully updated your account.";
            }
            catch (Exception ex)
            {
                message.Text = ex.Message;
            }
            Response.Redirect(Request.Url.GetLeftPart(UriPartial.Path) + "?Message=" + message.Text);
        }
        protected void changePassButton_OnClick(object sender, EventArgs e)
        {
            try
            {
                // Update the password.
                if (Membership.Provider.ChangePassword(User.Identity.Name, currentPasswordText.Text, newPasswordText.Text))
                {
                    message.Text = "Password has changed successfully.";
                    return;
                }
            }
            catch { }
            message.Text = "Password change failed. Please re-enter your credential and try again.";
        }

        private String getLecturerName(String assessmentId)
        {
            string selectSql = "Select CONCAT(FirstName + ' ', LastName) As [Name] From Assessment a, Contributor c, UserProfiles u " +
                               "where a.AssessmentId = c.AssessmentId and c.UserId = u.UserId and c.isHost = 'True' and a.AssessmentId = '" + assessmentId + "'";
            string lecturerName = "";
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                con.Open();
                SqlCommand sqlCommand = new SqlCommand(selectSql, con);
                SqlDataReader lecturerNameRecords = sqlCommand.ExecuteReader();

                if (lecturerNameRecords.Read())
                {
                    lecturerName = lecturerNameRecords["Name"].ToString();
                }
                con.Close();
            }
            return lecturerName;
        }

        private void getAssignment()
        {
            string selectSql = "Select a.AssessmentId, a.AssessmentTitle, a.AssessmentType, a.AssessmentDuration from Assessment a, Assignment ass, UserProfiles u " +
            "where a.AssessmentId = ass.AssessmentId and ass.UserId = u.UserId and u.UserId = @UserId";

            using (SqlConnection con = new SqlConnection(connectionString))
            {
                con.Open();
                SqlCommand sqlCommand = new SqlCommand(selectSql, con);
                sqlCommand.Parameters.AddWithValue("@UserId", (Guid)(Membership.GetUser(HttpContext.Current.User.Identity.Name.ToUpper())).ProviderUserKey);
                SqlDataReader assignmentRecords = sqlCommand.ExecuteReader();
                while (assignmentRecords.Read())
                {
                    assignment = new String[4];
                    assignment[0] = assignmentRecords["AssessmentId"].ToString();
                    assignment[1] = assignmentRecords["AssessmentTitle"].ToString();
                    assignment[2] = assignmentRecords["AssessmentType"].ToString();
                    assignment[3] = assignmentRecords["AssessmentDuration"].ToString();
                    assignmentList.Add(assignment);
                }
                Session.Add("assignmentList", assignmentList); Session.Timeout = 1000;
                con.Close();
            }
        }
        private void createAssignmentTable()
        {
            if (Session["assignmentList"] != null)
            {
                assignmentList = (List<String[]>)Session["assignmentList"];
            }

            Table table = new System.Web.UI.WebControls.Table();
            TableRow tableRow;
            TableCell tableCell; LinkButton linkButton; HtmlGenericControl span;

            table.ID = "datatables";
            table.CssClass = "table table-striped table-bordered";
            table.Attributes.CssStyle.Add("width", "100%");

            for (int i = 0; i < assignmentList.Count; i++)
            {
                tableRow = new TableRow();
                tableCell = new TableCell();

                span = new HtmlGenericControl("div");
                span.Attributes.Add("style", "padding-bottom: 10px");
                span.InnerHtml = "Lecturer " + getLecturerName(assignmentList[i][0]) + " has assigned Assessment " + assignmentList[i][1] + " | " + assignmentList[i][2].Trim() + " question to you.";

                linkButton = new LinkButton();
                linkButton.Text = i.ToString();
                linkButton.Attributes.Add("style", "all:unset;cursor:pointer;padding:10px;padding-top:20px;padding-bottom:20px;display:block");
                // Register the event-handling method for the CheckedChanged event. 
                linkButton.Click += new EventHandler(this.sendAssignment_OnClick);

                linkButton.Controls.Add(span);

                if (checkIsAnswered(Guid.Parse(assignmentList[i][0])) && isScored(Guid.Parse(assignmentList[i][0])))
                {
                    span = new HtmlGenericControl("span");
                    span.Attributes.Add("style", "float:right");
                    span.InnerHtml = "Score : " + getScore(Guid.Parse(assignmentList[i][0]));
                    linkButton.Controls.Add(span);
                }

                tableCell.Controls.Add(linkButton);
                tableRow.Cells.Add(tableCell);
                table.Rows.Add(tableRow);
            }

            TimelinePlaceHolder.Controls.Add(table);
        }

        protected void sendAssignment_OnClick(object sender, EventArgs e)
        {
            LinkButton linkButton = sender as LinkButton;

            Session.Add("assignment", assignmentList[Convert.ToInt16(linkButton.Text)]);
            Session.Timeout = 1000;

            if (assignmentList[Convert.ToInt16(linkButton.Text)][2].Trim() == "Written")
            {
                Response.Redirect("~/Views/Student/AnswerWritten.aspx");
            }
            else
            {
                Response.Redirect("~/Views/Student/AnswerMCQ.aspx");
            }

        }

        private bool isScored(Guid assessmentId)
        {
            bool isScored = false;
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
                        isScored = true;
                    }
                }
                con.Close();
            }
            return isScored;
        }

        private Double getScore(Guid assessmentId)
        {
            Double score = 0;
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
                        score = (Double)scoreRecords["Score"];
                    }
                }
                con.Close();
            }
            return score;
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
                    isAnswered = true;
                }
                con.Close();
            }
            return isAnswered;
        }

        private void GetAllAssessmentToList()
        {
            string selectSql = "Select * From Assessment a, Contributor c, UserProfiles u Where a.AssessmentId = c.AssessmentId and " +
                               "c.UserId = u.UserId and u.UserId = '" + ((Guid)(Membership.GetUser(HttpContext.Current.User.Identity.Name).ProviderUserKey)).ToString() + "' ORDER BY CreatedDate DESC ";

            using (SqlConnection con = new SqlConnection(connectionString))
            {
                con.Open();
                SqlCommand sqlCommand = new SqlCommand(selectSql, con);
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
                Session.Add("assessmentList", assessmentList); Session.Timeout = 1000;
                con.Close();
            }
        }

        private void createAssessmentTable()
        {
            if (Session["assessmentList"] != null)
            {
                assessmentList = (List<String[]>)Session["assessmentList"];
            }

            Table table = new System.Web.UI.WebControls.Table();
            TableRow tableRow;
            TableCell tableCell; LinkButton linkButton; HtmlGenericControl span;

            table.ID = "datatables";
            table.CssClass = "table table-striped table-bordered";
            table.Attributes.CssStyle.Add("width", "100%");

            for (int i = 0; i < assessmentList.Count; i++)
            {
                tableRow = new TableRow();
                tableCell = new TableCell();

                span = new HtmlGenericControl("div");
                span.Attributes.Add("style", "padding-bottom: 10px");
                span.InnerHtml = "You have created an Assessment <span style=\"font-weight:bold;color:black!important\">" + assessmentList[i][1] + " | " + assessmentList[i][2].Trim() + "</span> and has assigned to " + getCountOfAssignedQustion(Guid.Parse(assessmentList[i][0])) + " student(s).";

                linkButton = new LinkButton();
                linkButton.Text = i.ToString();
                linkButton.ToolTip = "Mark Answer or View Score";
                linkButton.Attributes.Add("style", "all:unset;cursor:pointer;padding:10px;padding-top:20px;padding-bottom:20px;display:block");
                // Register the event-handling method for the CheckedChanged event. 
                linkButton.Click += new EventHandler(this.ViewAllStudentAssigned_OnClick);

                linkButton.Controls.Add(span);

                span = new HtmlGenericControl("span");
                span.Attributes.Add("style", "float:right");
                span.InnerHtml = assessmentList[i][6];
                linkButton.Controls.Add(span);

                tableCell.Controls.Add(linkButton);
                tableRow.Cells.Add(tableCell);
                table.Rows.Add(tableRow);
            }

            TimelinePlaceHolder.Controls.Add(table);
        }

        protected void ViewAllStudentAssigned_OnClick(object sender, EventArgs e)
        {
            LinkButton linkButton = sender as LinkButton;

            Session.Add("assessment", assessmentList[Convert.ToInt16(linkButton.Text)]);
            Session.Timeout = 1000;

            Response.Redirect("~/Views/Lecturer/MarkingOrViewScore.aspx");
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
    }
}
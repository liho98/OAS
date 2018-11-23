using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace OAS.Views.Lecturer
{
    public partial class CreateWrittenQuestion : System.Web.UI.Page
    {
        private static string connectionString = ConfigurationManager.ConnectionStrings["oasDB"].ConnectionString;
        private String[] assessment = new String[10];
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["assessment"] != null)
            {
                assessment = Session["assessment"] as String[]; 
            }

            if (!Page.IsPostBack)
            {

                //MessageLabel.Text = (String)Request.QueryString["Message"];
            }
        }
        protected void SubmitButton_OnClick(object sender, EventArgs e)
        {

            string fileName = ImageUpload.PostedFile.FileName.ToLower();
            string fileExtension = System.IO.Path.GetExtension(fileName);
            string fileMimeType = ImageUpload.PostedFile.ContentType;

            string[] matchExtension = { ".jpg", ".jpeg", ".png", ".gif" };
            string[] matchMimeType = { "image/jpg", "image/jpeg", "image/png", "image/gif" };

            //ViewState.Clear();

            try
            {
                if (ImageUpload.HasFile)
                {
                    if (matchExtension.Contains(fileExtension) && matchMimeType.Contains(fileMimeType))
                    {
                        String imageUrl = "data:" + fileMimeType + ";base64," + Convert.ToBase64String(ImageUpload.FileBytes);

                        string insertSql = "INSERT INTO Question(QuestionText, QuestionLevel, Image, AssessmentId) " +
                                           "VALUES(@QuestionText, @QuestionLevel, @Image, @AssessmentId)";
                        using (SqlConnection con = new SqlConnection(connectionString))
                        {
                            con.Open();
                            SqlCommand sqlCommand = new SqlCommand(insertSql, con);
                            sqlCommand.Parameters.AddWithValue("@QuestionText", editor.Text);
                            sqlCommand.Parameters.AddWithValue("@QuestionLevel", LevelDropDownList.SelectedValue);
                            sqlCommand.Parameters.AddWithValue("@Image", Encoding.Default.GetBytes(imageUrl));
                            sqlCommand.Parameters.AddWithValue("@AssessmentId", Guid.Parse(assessment[0]));
                            sqlCommand.ExecuteNonQuery();
                            con.Close();
                        }
                    }
                    else
                    {
                        MessageLabel.Text = "Upload status: Only jpg, jpeg, png or gif file is accepted!";
                        return;
                    }
                }
                else
                {
                    string insertSql = "INSERT INTO Question(QuestionText, QuestionLevel, AssessmentId) " +
                                       "VALUES(@QuestionText, @QuestionLevel, @AssessmentId)";
                    using (SqlConnection con = new SqlConnection(connectionString))
                    {
                        con.Open();
                        SqlCommand sqlCommand = new SqlCommand(insertSql, con);
                        sqlCommand.Parameters.AddWithValue("@QuestionText", editor.Text);
                        sqlCommand.Parameters.AddWithValue("@QuestionLevel", LevelDropDownList.SelectedValue);
                        sqlCommand.Parameters.AddWithValue("@AssessmentId", Guid.Parse(assessment[0]));
                        sqlCommand.ExecuteNonQuery();
                        con.Close();
                    }
                }

                MessageLabel.Text = "You have successfully create a question for Assessment .";
            }
            catch (Exception ex)
            {
                MessageLabel.Text = ex.Message;
            }
            //Response.Redirect(Request.Url.GetLeftPart(UriPartial.Path) + "?Message=" + MessageLabel.Text);
        }
    }
}
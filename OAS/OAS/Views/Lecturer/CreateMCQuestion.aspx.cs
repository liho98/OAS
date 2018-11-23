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
    public partial class CreateMCQuestion : System.Web.UI.Page
    {

        private static string connectionString = ConfigurationManager.ConnectionStrings["oasDB"].ConnectionString;
        private string[] matchExtension = { ".jpg", ".jpeg", ".png", ".gif" };
        private string[] matchMimeType = { "image/jpg", "image/jpeg", "image/png", "image/gif" };
        private String[] assessment = new String[10];

        protected void Page_Load(object sender, EventArgs e)
        {
            MCQTable();

            if (Session["assessment"] != null)
            {
                assessment = Session["assessment"] as String[];
            }

            if (!Page.IsPostBack)
            {
                MessageLabel.Text = (String)Request.QueryString["Message"];
            }
        }

        protected void SubmitButton_OnClick(object sender, EventArgs e)
        {
            FileUpload ImageFileUpload; TextBox textbox; RadioButton radioButton;
            Guid questionId = Guid.NewGuid();

            for (int i = 0; i < 5; i++)
            {
                ImageFileUpload = (FileUpload)MCQTablePlaceholder.FindControl("ImageFileUpload" + i);
                textbox = (TextBox)MCQTablePlaceholder.FindControl("editor" + i);

                if (i == 0)
                {
                    InsertMCQuestion(questionId, ImageFileUpload, textbox);
                }
                if (i > 0)
                {
                    radioButton = (RadioButton)MCQTablePlaceholder.FindControl("AnswerRadioButton" + i);
                    InsertMCQOption(questionId, ImageFileUpload, textbox, radioButton);
                }
            }
            MessageLabel.Text = "You have successfully create a question for Assessment .";

            Response.Redirect(Request.Url.GetLeftPart(UriPartial.Path) + "?Message=" + MessageLabel.Text);
        }

        private void InsertMCQuestion(Guid questionId, FileUpload ImageFileUpload, TextBox textbox)
        {
            string fileName, fileExtension, fileMimeType;

            fileName = ImageFileUpload.PostedFile.FileName.ToLower();
            fileExtension = System.IO.Path.GetExtension(fileName);
            fileMimeType = ImageFileUpload.PostedFile.ContentType;

            if (ImageFileUpload.HasFile)
            {
                if (matchExtension.Contains(fileExtension) && matchMimeType.Contains(fileMimeType))
                {
                    String imageUrl = "data:" + fileMimeType + ";base64," + Convert.ToBase64String(ImageFileUpload.FileBytes);

                    string insertSql = "INSERT INTO Question(QuestionId, QuestionText, QuestionLevel, Image, AssessmentId) " +
                                       "VALUES(@QuestionId, @QuestionText, @QuestionLevel, @Image, @AssessmentId)";
                    using (SqlConnection con = new SqlConnection(connectionString))
                    {
                        con.Open();
                        SqlCommand sqlCommand = new SqlCommand(insertSql, con);
                        sqlCommand.Parameters.AddWithValue("@QuestionId", questionId);
                        sqlCommand.Parameters.AddWithValue("@QuestionText", textbox.Text);
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
                string insertSql = "INSERT INTO Question(QuestionId, QuestionText, QuestionLevel, AssessmentId) " +
                                   "VALUES(@QuestionId, @QuestionText, @QuestionLevel, @AssessmentId)";
                using (SqlConnection con = new SqlConnection(connectionString))
                {
                    con.Open();
                    SqlCommand sqlCommand = new SqlCommand(insertSql, con);
                    sqlCommand.Parameters.AddWithValue("@QuestionId", questionId);
                    sqlCommand.Parameters.AddWithValue("@QuestionText", textbox.Text);
                    sqlCommand.Parameters.AddWithValue("@QuestionLevel", LevelDropDownList.SelectedValue);
                    sqlCommand.Parameters.AddWithValue("@AssessmentId", Guid.Parse(assessment[0]));
                    sqlCommand.ExecuteNonQuery();
                    con.Close();
                }
            }
        }

        private void InsertMCQOption(Guid questionId, FileUpload ImageFileUpload, TextBox textbox, RadioButton radioButton)
        {
            string insertSql;
            string fileName, fileExtension, fileMimeType;

            fileName = ImageFileUpload.PostedFile.FileName.ToLower();
            fileExtension = System.IO.Path.GetExtension(fileName);
            fileMimeType = ImageFileUpload.PostedFile.ContentType;

            if (ImageFileUpload.HasFile)
            {
                if (matchExtension.Contains(fileExtension) && matchMimeType.Contains(fileMimeType))
                {
                    String imageUrl = "data:" + fileMimeType + ";base64," + Convert.ToBase64String(ImageFileUpload.FileBytes);

                    insertSql = "INSERT INTO [dbo].[Option](OptionText, isCorrectAnswer, Image, QuestionId) " +
                                       "VALUES(@OptionText, @isCorrectAnswer, @Image, @QuestionId)";
                    using (SqlConnection con = new SqlConnection(connectionString))
                    {
                        con.Open();
                        SqlCommand sqlCommand = new SqlCommand(insertSql, con);
                        sqlCommand.Parameters.AddWithValue("@OptionText", textbox.Text);
                        sqlCommand.Parameters.AddWithValue("@isCorrectAnswer", radioButton.Checked);
                        sqlCommand.Parameters.AddWithValue("@QuestionId", questionId);
                        sqlCommand.Parameters.AddWithValue("@Image", Encoding.Default.GetBytes(imageUrl));
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
                insertSql = "INSERT INTO [dbo].[Option](OptionText, isCorrectAnswer, QuestionId) " +
                                   "VALUES(@OptionText, @isCorrectAnswer, @QuestionId)";
                using (SqlConnection con = new SqlConnection(connectionString))
                {
                    con.Open();
                    SqlCommand sqlCommand = new SqlCommand(insertSql, con);
                    sqlCommand.Parameters.AddWithValue("@OptionText", textbox.Text);
                    sqlCommand.Parameters.AddWithValue("@isCorrectAnswer", radioButton.Checked);
                    sqlCommand.Parameters.AddWithValue("@QuestionId", questionId);
                    sqlCommand.ExecuteNonQuery();
                    con.Close();
                }
            }
        }

        private void MCQTable()
        {
            Table table = new System.Web.UI.WebControls.Table();
            TableRow tableRow;
            TableCell tableCell;
            HtmlGenericControl htmlGenericControl; HtmlGenericControl htmlGenericControl2;
            FileUpload fileUpload; TextBox textbox; RadioButton radioButton;

            table.ID = "MCQTable";
            table.Attributes.Add("style", "width: 100%; height: 100%; table-layout: fixed");

            for (int i = 0; i < 5; i++)
            {
                tableRow = new TableRow();
                tableCell = new TableCell();
                tableCell.Attributes.CssStyle.Add("width", "20%");
                htmlGenericControl = new HtmlGenericControl("h4");
                if (i == 0)
                {
                    htmlGenericControl.InnerHtml = "Question Image";
                }
                else
                {
                    htmlGenericControl.InnerHtml = "No." + i + ") Answer Option " + Convert.ToChar(i + 64);
                }
                htmlGenericControl.Attributes.Add("style", "font-family: 'Segoe UI Emoji';");
                tableCell.Controls.Add(htmlGenericControl);
                htmlGenericControl = new HtmlGenericControl("div");
                htmlGenericControl.Attributes.Add("style", "border: 1px solid rgba(0,0,0,0.2); text-align: center; width: 100%; height: 100%; margin: auto; background-color: ");
                htmlGenericControl2 = new HtmlGenericControl("div");
                htmlGenericControl2.ID = "imageUploadId" + i;
                htmlGenericControl2.Attributes.Add("style", "width: 100%; height: 100%; background-color: #d7d7d7; background-size: cover; background-position: center; background-image: url('../../Content/images/gif/gif1.gif')");
                htmlGenericControl.Controls.Add(htmlGenericControl2);
                tableCell.Controls.Add(htmlGenericControl2);
                tableRow.Cells.Add(tableCell);

                tableCell = new TableCell();
                htmlGenericControl = new HtmlGenericControl("h4");
                if (i == 0)
                {
                    htmlGenericControl.InnerHtml = "Insert your Question";
                    htmlGenericControl.Attributes.Add("style", "font-family: 'Segoe UI Emoji';");
                    tableCell.Controls.Add(htmlGenericControl);
                }
                else
                {
                    htmlGenericControl.InnerHtml = "Insert your Answer";
                    htmlGenericControl.Attributes.Add("style", "font-family: 'Segoe UI Emoji';float:left");
                    tableCell.Controls.Add(htmlGenericControl);

                    htmlGenericControl = new HtmlGenericControl("h5");
                    htmlGenericControl.Attributes.Add("style", "font-family: 'Segoe UI Emoji';float:right");

                    htmlGenericControl2 = new HtmlGenericControl("label");
                    htmlGenericControl2.Attributes.Add("for", "contentBody_AnswerRadioButton" + i);
                    htmlGenericControl2.InnerHtml = "Is Correct Answer?";
                    radioButton = new RadioButton();
                    radioButton.ID = "AnswerRadioButton" + i;
                    if (i == 1)
                    {
                        radioButton.Checked = true;
                    }
                    radioButton.GroupName = "Answer";
                    htmlGenericControl2.Controls.Add(radioButton);
                    htmlGenericControl.Controls.Add(htmlGenericControl2);
                    tableCell.Controls.Add(htmlGenericControl);
                }

                htmlGenericControl = new HtmlGenericControl("div");
                htmlGenericControl.ID = "scrollEditor" + i;
                htmlGenericControl.Attributes.Add("style", "border: 1px solid rgba(0,0,0,0.2); width: 100%; height: 100%; overflow: scroll");
                htmlGenericControl.Attributes.Add("onscroll", "scrollEditorFunc()");
                fileUpload = new FileUpload();
                fileUpload.ID = "ImageFileUpload" + i;
                fileUpload.Attributes.Add("style", "display: none;");
                htmlGenericControl.Controls.Add(fileUpload);
                htmlGenericControl2 = new HtmlGenericControl("label");
                htmlGenericControl2.ID = "showUpload" + i;
                htmlGenericControl2.Attributes.Add("title", "Insert Image");
                htmlGenericControl2.Attributes.Add("for", "contentBody_ImageFileUpload" + i);
                htmlGenericControl2.Attributes.Add("style", "background-color: rgba(0,0,0,0.2); cursor: pointer; position: absolute; transform: translate(965%,10%); z-index: 1; color: #fff; width: 33px; height: 33px;");
                htmlGenericControl.Controls.Add(htmlGenericControl2);
                textbox = new TextBox();
                textbox.ID = "editor" + i;
                textbox.TextMode = TextBoxMode.MultiLine;
                htmlGenericControl.Controls.Add(textbox);
                tableCell.Controls.Add(htmlGenericControl);

                tableRow.Cells.Add(tableCell);
                table.Rows.Add(tableRow);

                if (i == 0)
                {
                    tableRow = new TableRow();
                    tableRow.Attributes.Add("style", "padding-bottom: 0px!important;height: 1%!important;margin: 0!important;");
                    tableCell = new TableCell();
                    tableCell.ColumnSpan = 2;
                    tableCell.Attributes.Add("style", "padding-bottom:0!important");
                    htmlGenericControl = new HtmlGenericControl("hr");
                    htmlGenericControl.Attributes.Add("style", "");
                    tableCell.Controls.Add(htmlGenericControl);
                    tableRow.Cells.Add(tableCell);
                    table.Rows.Add(tableRow);
                }
            }
            MCQTablePlaceholder.Controls.Add(table);
        }
    }
}
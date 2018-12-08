using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace OAS.MasterPage
{
    public partial class MainSite : System.Web.UI.MasterPage
    {
        protected String firstName, userAvatar;

        protected void Page_Load(object sender, EventArgs e)
        {
            this.Page.Header.DataBind();
            if (System.Web.HttpContext.Current.User.Identity.IsAuthenticated)
            {
                try
                {
                    string connectionString = ConfigurationManager.ConnectionStrings["oasDB"].ConnectionString;
                    //Get the UserId of the just-added user
                    Guid UserId = (Guid)(Membership.GetUser(HttpContext.Current.User.Identity.Name)).ProviderUserKey;
                    SqlConnection con = new SqlConnection(connectionString);

                    string selectSql = "Select FirstName, Image " +
                            "from [dbo].[UserProfiles] Where UserId = @UserId;";

                    con.Open();
                    SqlCommand sqlCommand = new SqlCommand(selectSql, con);
                    sqlCommand.Parameters.AddWithValue("@UserId", UserId);
                    SqlDataReader userRecords = sqlCommand.ExecuteReader();
                    userRecords.Read();
                    firstName = userRecords["FirstName"].ToString();
                    userAvatar = Encoding.Default.GetString((byte[])userRecords["Image"]);
                    con.Close();
                }
                catch
                {
                    FormsAuthentication.RedirectToLoginPage();
                }
            }
        }
        protected void SignOut_OnClick(object sender, EventArgs e)
        {
            FormsAuthentication.SignOut();
            Response.Redirect("~/Default.aspx");
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace OAS
{
    public partial class Login : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                if (Request.IsAuthenticated && !string.IsNullOrEmpty(Request.QueryString["ReturnUrl"]))
                {
                    // This is an unauthorized, authenticated request...
                    FormsAuthentication.SignOut();
                    Response.Redirect("~/UnauthorizedAccess.aspx?ReturnUrl=" + Request.QueryString["ReturnUrl"]);
                }
            }

        }

        protected void loginButton_Click(object sender, EventArgs e)
        {
            FormsAuthentication.SignOut();
            // Validate the user against the Membership framework user store
            if (Membership.ValidateUser(emailID.Text, password.Text))
            {
                // Log the user into the site
                FormsAuthentication.RedirectFromLoginPage(emailID.Text, rememberMe.Checked);
            }
            // If we reach here, the user's credentials were invalid
            invalidCredentialsMessage.Visible = true;
        }
    }
}
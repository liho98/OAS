<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage/MainSite.Master" AutoEventWireup="true" CodeBehind="testing.aspx.cs" Inherits="OAS.Others.testing" %>

<%@ Import Namespace="System.Data" %>
<%@ Import Namespace="System.Data.SqlClient" %>
<%@ Import Namespace="System.IO" %>

<asp:Content ID="head" ContentPlaceHolderID="head" runat="server">
    <!-- Overide the #contentBody height value default is 1.8 -->
    <script>
        var path = window.location.protocol + "//" + window.location.host;

        $.when(
            $.getScript(path + "/Scripts/js/dynamicSetHeight.js"),
            $.Deferred(function (deferred) {
                $(deferred.resolve);
            })
        ).done(function () {
            //place your code here, the scripts are all loaded
            dynamicSetHeight(1);
            setLeftTriangle();
        });

        $(window).on('resize', function () {
            var instance = $.fn.deviceDetector;
            if (instance.isDesktop()) {
                dynamicSetHeight(1);
            }
            if ($(window).outerWidth() > 958) {
                setLeftTriangle();
            } else {
                document.getElementById("triangle-div").style.left = "";
            }
        });
    </script>

    <style>
        video {
            opacity: 0.6;
        }

        .background-image {
            filter: brightnes(250%);
            background-image: url('../../../Content/images/background_images/login_signup_bg.jpg');
            background-size: cover;
            background-position: center;
        }
    </style>
</asp:Content>

<asp:Content ID="videoSource" ContentPlaceHolderID="videoSource" runat="server">
    <source runat="server" type="video/mp4" src="~/Content/videos/bg_video2.mp4">
</asp:Content>

<asp:Content ID="backgoundText" ContentPlaceHolderID="backgoundText" runat="server">
    <!-- background text -->
    <div style="position: fixed; top: 14%; width: 100%; color: #fff">
        <div id="sloganDiv" style="text-align: center;">
            <div style="margin: 20px;">
                <span id="slogan">Testing</span>
            </div>
        </div>
    </div>
</asp:Content>

<asp:Content ID="contentBody" ContentPlaceHolderID="contentBody" runat="server">

    <script runat="server">
        private static string conStr = ConfigurationManager.ConnectionStrings["oasDB"].ConnectionString;
        private SqlConnection con = new SqlConnection(conStr);

        void Page_Load(Object sender, EventArgs e)
        {
            con.Open();
        }
        protected void UploadButton_Click(object sender, EventArgs e)
        {
            string fileName = FileUploadControl.PostedFile.FileName.ToLower();
            string fileExtension = System.IO.Path.GetExtension(fileName);
            string fileMimeType = FileUploadControl.PostedFile.ContentType;

            string[] matchExtension = { ".jpg", ".jpeg", ".png", ".gif" };
            string[] matchMimeType = { "image/jpg", "image/jpeg", "image/png", "image/gif" };

                String selectStr = "select UserId from [dbo].[aspnet_Users] where UserName = 1;";

                //SqlCommand selectCmd = new SqlCommand(selectStr, con);
                //SqlDataReader userRecords = selectCmd.ExecuteReader();
                  

            if (FileUploadControl.HasFile)
            {
                try
                {
                    if (matchExtension.Contains(fileExtension) && matchMimeType.Contains(fileMimeType))
                    {
                        String imageUrl = "data:" + fileMimeType + ";base64," + Convert.ToBase64String(FileUploadControl.FileBytes);
                        String updateStr = "UPDATE [dbo].[UserProfiles] SET Image = '" + imageUrl + "' WHERE UserId = '9762b7c6-9d78-468c-bd9a-7d39f1d319f4';";

                        SqlCommand updateCmd = new SqlCommand(updateStr, con);
                        updateCmd.ExecuteNonQuery();

                        //FileUploadControl.SaveAs(Server.MapPath(@"UserImages/" + fileName));
                        // FileUploadControl.SaveAs(Server.MapPath("~/") + fileName);
                        StatusLabel.Text = "Upload status: File uploaded!";
                    }
                    else
                    {
                        //Please choose only jpg, jpeg, png or gif file.
                        StatusLabel.Text = "Upload status: Only jpg, jpeg, png or gif file is accepted!";
                    }
                }
                catch (Exception ex)
                {
                    StatusLabel.Text = "Upload status: The file could not be uploaded. The following error occured: " + ex.Message;
                }
            }
        }

    </script>

    <!-- content start -->
    <div id="bodyContent" style="background-color: #fff; border-bottom: 1px solid rgb(205,205,205); z-index: 1; color: #5f5d5d">
        <div class="content">

            <asp:FileUpload ID="FileUploadControl" runat="server" />
            <asp:Button runat="server" ID="UploadButton" Text="Upload" OnClick="UploadButton_Click" />
            <br />
            <br />
            <asp:Label runat="server" ID="StatusLabel" Text="Upload status: " />


            <%-- String damn = "suck you"; %>
            <%= damn %><br />
            <%: damn --%>
            <% 
                  con.Close();        con.Open();
                String selectStr = "select * from [dbo].[UserProfiles];";

                SqlCommand selectCmd = new SqlCommand(selectStr, con);
                SqlDataReader userRecords = selectCmd.ExecuteReader();

                while (userRecords.Read())
                {
                    if (userRecords["Image"] != System.DBNull.Value)
                    {
                        // + "<img style=\"width: 100px;height:100px\" src=\"" + Encoding.Default.GetString((byte[]) userRecords["userImage"])+"\"/><br/>"
                        //Response.Write(userRecords["userID"].ToString() + "<img style=\"width: 300px;height:200px\" src=\"" + Encoding.Default.GetString((byte[])userRecords["Image"]) + "\"/><br/>");
                        Response.Write(userRecords["userID"].ToString() + "<img style=\"width: 300px;height:200px\" src=\"" + Encoding.Default.GetString((byte[])userRecords["Image"]) + "\"/><br/>");
                        //Response.Write(userRecords["userID"].ToString() + "<img style=\"width: 300px;height:200px\" src=\"" + Convert.ToBase64String((byte[])userRecords["Image"]) + "\"/><br/>");

                    }
                }

            %>
        </div>
    </div>
    <!-- content end -->
    <%
        con.Close();
    %>
</asp:Content>

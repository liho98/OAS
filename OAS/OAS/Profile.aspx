<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage/MainSite.Master" AutoEventWireup="true" CodeBehind="Profile.aspx.cs" Inherits="OAS.Profile" %>

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

        $.when(
            $.getScript(path + "/Scripts/js/jquery.device.detector.js"),
            $.Deferred(function (deferred) {
                $(deferred.resolve);
            })
        ).done(function () {
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
        });
    </script>

    <style>
        video {
            opacity: 0.6;
        }

        .background-image {
            filter: brightnes(250%);
            background-size: cover;
            background-position: center;
        }

        .box-wrapper {
            background-color: #fff;
            position: absolute;
            top: 43%;
            right: 0;
            bottom: 0;
            left: 50%;
            width: 70%;
            height: 90%;
            transform: translate(-50%, -50%);
            user-select: none;
        }

        .box {
            background-color: #fff;
            width: 75%;
            height: 75%;
            position: absolute;
            top: 50%;
            right: 0;
            bottom: 0;
            left: 50%;
            transform: translate(-50%, -50%);
        }
    </style>
</asp:Content>

<asp:Content ID="videoSource" ContentPlaceHolderID="videoSource" runat="server">
    <source runat="server" type="video/mp4" src="~/Content/videos/bg_video2.mp4">
    <script>
        var path = window.location.protocol + "//" + window.location.host;
        document.getElementsByClassName("background-image")[0].style.backgroundImage = "url('" + path + "/Content/images/background_images/login_signup_bg.jpg')";
    </script>
</asp:Content>

<asp:Content ID="backgoundText" ContentPlaceHolderID="backgoundText" runat="server">
    <!-- background text -->
    <div style="position: fixed; top: 14%; width: 100%; color: #fff">
        <div id="sloganDiv" style="text-align: center;">
            <div style="margin: 20px;">
                <span id="slogan">Profile</span>
            </div>
        </div>
    </div>
</asp:Content>

<asp:Content ID="contentBody" ContentPlaceHolderID="contentBody" runat="server">
    <!-- content start -->
    <div id="bodyContent" style="background-color: #fff; background: linear-gradient(to bottom, rgba(0,0,0,0.1) 10%, #ffffff 100%);">
        <div class="content">

            <div class="box-wrapper">
                <div class="box" style="width: 100%; height: 100%">

                    <div class="leftPanel" style="width:40%;height:100%;background-color:aquamarine;float:left">
                        <div class="profile-img" style="text-align: center; width: 220px; height: 300px; margin: 25%">

                            <img style="width: 100%;" src="Content/images/background_images/IMG_3034.jpg" alt="">
                            <asp:FileUpload ID="userAvatarUpload" runat="server" Style="display: none;" />
                            <label for="contentBody_userAvatarUpload" style="background-color: #212529b8; color: #fff; width: 100%; line-height: 35px; display: inline-block; margin-top: -64px; vertical-align: middle;">
                                Change Photo
                            </label>
                        </div>
                        <div>
                            <table>
                                <tr>
                                    <td></td>
                                </tr>
                            </table>
                        </div>
                    </div>
                    <div class="rightPanel" style="width:60%;height:100%;background-color:blanchedalmond;float:right">

                    </div>

                </div>
            </div>



        </div>
    </div>
    <!-- content end -->

</asp:Content>

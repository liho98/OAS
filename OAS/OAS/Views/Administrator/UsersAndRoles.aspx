<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage/MainSite.Master" AutoEventWireup="true" CodeBehind="UsersAndRoles.aspx.cs" Inherits="OAS.Views.Administrator.UsersAndRoles" %>

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
            dynamicSetHeight(0.7);
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
                    dynamicSetHeight(0.7);
                }
                if ($(window).outerWidth() > 958) {
                    setLeftTriangle();
                } else {
                    document.getElementById("triangle-div").style.left = "";
                }
            });
        });

        function searchUserFunction() {
            var input, filter, table, tr, td, i;
            input = document.getElementById("searchUserIDInput");
            filter = input.value.toUpperCase();
            table = document.getElementsByClassName("roleTable");
            tr = table[0].getElementsByTagName("tr");
            for (i = 1; i < tr.length; i++) {
                td = tr[i].getElementsByTagName("td")[1];
                if (td) {
                    if (td.innerHTML.toUpperCase().indexOf(filter) > -1) {
                        tr[i].style.display = "";
                    } else {
                        tr[i].style.display = "none";
                    }
                }
            }
        }
        function searchRoleFunction() {
            var input, filter, table, tr, td, innerTD, i, j;
            input = document.getElementById("searchRoleInput");
            filter = input.value.toUpperCase();
            table = document.getElementsByClassName("roleTable");
            tr = table[0].getElementsByTagName("tr");

            td = tr[0].getElementsByTagName("td");

            for (i = 2; i < td.length; i++) {
                if (td[i]) {
                    if (td[i].innerHTML.toUpperCase().indexOf(filter) > -1) {
                        for (j = 0; j < tr.length; j++) {
                            innerTD = tr[j].getElementsByTagName("td")[i];
                            innerTD.style.display = "";
                        }
                    } else {

                        for (j = 0; j < tr.length; j++) {
                            innerTD = tr[j].getElementsByTagName("td")[i];
                            innerTD.style.display = "none";
                        }
                    }
                }

            }
        }

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

        .roleDiv::-webkit-scrollbar {
            width: 5px;
            height: 5px;
        }

        .roleDiv::-webkit-scrollbar-track {
            -webkit-box-shadow: inset 0 0 5px rgba(0,0,0,0);
            background-color: white;
        }

        .roleDiv::-webkit-scrollbar-thumb {
            background-color: rgba(100,100,100,1);
            outline: 1px solid slategrey;
        }

        input[type=submit], button {
            border-color: #0067b8;
            background-color: #0067b8;
            color: #fff;
            padding: 4px 12px 4px 12px;
            border-width: 1px;
            border-style: solid;
            cursor: pointer;
            text-overflow: ellipsis;
            touch-action: manipulation;
            display: inline-block;
            -webkit-appearance: button;
            bottom: 0;
            right: 0;
        }

        #contentBody_ActionStatusByUser, #contentBody_ActionStatusByRole {
            font-weight: 500;
            font-size: 11px;
            letter-spacing: 0.5px;
            display: inline-block;
            margin-top: 15px;
        }

        #bodyContent {
            background-color: #fff;
            z-index: 1;
        }

        #searchUserIDInput, #searchRoleInput {
            background-image: url('../../../Content/images/icons_logos/searchicon.png');
            background-size: 12px;
            background-position: 10px 6px;
            background-repeat: no-repeat;
            width: 65%;
            font-size: 12px;
            padding: 5px 10px 5px 40px;
            display: block;
            border: none;
            border-bottom: 1px solid #ddd;
            margin: 0;
            outline: none;
            -webkit-transition: all 1s linear;
            transition: all 1s linear;
        }

        #searchRoleInput {
            float: right;
        }

            #searchUserIDInput:focus, #searchRoleInput:focus {
                border-color: rgb(102,102,255);
            }

        .tableContent {
            table-layout: fixed;
            width: 80%;
            height: 280px;
            margin: auto;
            margin-top: 100px;
        }

            .tableContent td {
                background-color: rgba(240,240,240,1);
            }

        .roleDiv {
            height: 200px;
            display: block;
            width: 100%;
            overflow: scroll;
            white-space: nowrap;
            border: 1px solid #ddd;
        }

        .roleTable {
            border-width: 0;
            border-collapse: collapse;
            text-align: left;
            width: 100%;
        }

            .roleTable tr {
                border-bottom: 1px solid #ddd;
            }

            .roleTable td, .roleTable thead td {
                padding: 10px;
            }

                .roleTable thead td, .roleTable thead td:hover {
                    background-color: #fff;
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
                <span id="slogan">Manage Roles</span>
            </div>
        </div>
    </div>
</asp:Content>

<script runat="server">


</script>

<asp:Content ID="contentBody" ContentPlaceHolderID="contentBody" runat="server">
    <!-- content start -->
    <div id="bodyContent">
        <div class="content">
            <table class="tableContent">
                <tr>
                    <td>
                        <h4>Assign Roles to Users</h4>
                        <table style="width: 100%; table-layout: fixed">
                            <tr>
                                <td>
                                    <input type="text" id="searchUserIDInput" onkeyup="searchUserFunction()" placeholder="Search for User..." title="Type in an ID" onkeydown="if (event.keyCode == 13) return false /">
                                </td>
                                <td>
                                    <input type="text" id="searchRoleInput" onkeyup="searchRoleFunction()" placeholder="Search for Role..." title="Type in a role" onkeydown="if (event.keyCode == 13) return false" />
                                </td>
                            </tr>
                        </table>
                        <div id="RoleTable" runat="server" class="roleDiv">
                        </div>
                        <div style="height: 40px; width: 100%;">
                            <asp:Label ID="ActionStatusByUser" runat="server"></asp:Label>
                        </div>
                    </td>
                </tr>
            </table>
        </div>
    </div>
    <!-- content end -->
</asp:Content>

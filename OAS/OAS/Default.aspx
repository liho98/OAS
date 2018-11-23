<%@ Page Title="OAS | Homepage" Language="C#" MasterPageFile="~/MasterPage/MainSite.Master" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="OAS.Default" %>

<asp:Content ID="head" ContentPlaceHolderID="head" runat="server">
    <script>
        var path = window.location.protocol + "//" + window.location.host;

        $.when(
            $.getScript(path + "/Scripts/js/dynamicSetHeight.js"),
            $.Deferred(function (deferred) {
                $(deferred.resolve);
            })
        ).done(function () {
            //place your code here, the scripts are all loaded
            dynamicSetHeight(1.8);
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
                    dynamicSetHeight(1.8);
                }
                if ($(window).outerWidth() > 958) {
                    setLeftTriangle();
                } else {
                    document.getElementById("triangle-div").style.left = "";
                }
            });
        });
    </script>
</asp:Content>

<asp:Content ID="videoSource" ContentPlaceHolderID="videoSource" runat="server">
    <source runat="server" type="video/mp4" src="~/Content/videos/bg_video1.mp4">
</asp:Content>

<asp:Content ID="bottomHeader" ContentPlaceHolderID="bottomHeader" runat="server">
    <div id="bottom-header">
        <div id="navigation">
            <div style="max-width: 1200px; margin: auto">
                <table cellspacing="20" style="width: 100%; margin: auto; table-layout: fixed;">
                    <tr>
                        <td>
                            <div class="menu">
                                <a href="#" title="Courses">Courses</a>
                            </div>

                            <div>
                                <div class="submenu" onclick="subButton(0)"><span>Courses Intro</span><span class="material-icons add">&#xE145;</span></div>
                                <div class="sub-sub-menu">
                                    <a href="#" class="sub-sub-air-tag">Courses Intro</a>
                                </div>
                            </div>
                        </td>
                        <td>
                            <div class="menu">
                                <a href="#" title="Quiz">Quiz</a>
                            </div>

                            <div class="submenu" onclick="subButton(1)"><span>Quiz Intro</span><span class="material-icons add">&#xE145;</span></div>
                            <div class="sub-sub-menu">
                                <a href="#" class="sub-sub-air-tag">Quiz Intro</a>
                            </div>
                        </td>
                        <td>
                            <div class="menu">
                                <a href="#" title="Assessments">Assessments</a>
                            </div>

                            <div class="submenu" onclick="subButton(2)"><span>Assessments Intro</span><span class="material-icons add">&#xE145;</span></div>
                            <div class="sub-sub-menu">
                                <a href="#" class="sub-sub-air-tag">Assessments Intro</a>
                            </div>
                        </td>
                        <td>
                            <div class="menu">
                                <a href="#" title="Exams">Exams</a>
                            </div>

                            <div class="submenu" onclick="subButton(3)"><span>Exams Intro</span><span class="material-icons add">&#xE145;</span></div>
                            <div class="sub-sub-menu">
                                <a href="#" class="sub-sub-air-tag">Exams Intro</a>
                            </div>
                        </td>
                        <td>
                            <div class="menu">
                                <a href="#" title="Other">Other</a>
                            </div>

                            <div class="submenu" onclick="subButton(4)"><span>Share &#38; Learn</span><span class="material-icons add">&#xE145;</span></div>
                            <div class="sub-sub-menu">
                                <a href="#" class="sub-sub-air-tag">Other</a>
                            </div>
                            <div class="submenu block">
                                <!--Block-->
                            </div>
                        </td>
                    </tr>
                </table>
            </div>
        </div>
        <hr id="separatorMenu" />
    </div>
</asp:Content>

<asp:Content ID="backgoundText" ContentPlaceHolderID="backgoundText" runat="server">
    <!-- background text -->
    <div style="position: fixed; bottom: 0; width: 100%; color: #fff">
        <div id="sloganDiv">
            <div style="margin: 20px;">
                <span id="slogan">Keep Learning. </span>
                <img id="gif1" src="/Content/images/gif/gif3.gif">
                <span id="sub-slogan-text">Curiosity is the wick in the candle of learning.</span>
            </div>
        </div>
    </div>
</asp:Content>

<asp:Content ID="slideshowContent" ContentPlaceHolderID="slideshowContent" runat="server">
    <!-- slideshow start -->
    <div id="swipe" class="slide-bg-container">

        <div class="slideshow">
            <div class="slidetext">
                <article>
                    <h1>Future Literature</h1>
                    <h4></h4>
                    <p>A beautiful young man in a library travels in a virtual futuristic world with augmented reality. Concept: educational, future, library, and immersive technology.</p>
                    <br />
                    <a href="#">Explore Events &nbsp;&#62;&#62;</a>
                </article>
            </div>
            <div class="slide-bg" style="background-image: url('/Content/images/slideshow_index_page/slide1.jpg');"></div>
        </div>

        <div class="slideshow">
            <div class="slidetext">
                <article style="max-width: 500px; float: right">
                    <h1>Build better learning products. Faster!</h1>
                    <h4></h4>
                    <p>World-class technology for building and delivering online assessments at scale. Save time, money and resources by integrating Learnosity into your product.</p>
                    <br />
                    <a href="#">Explore Events &nbsp;&#62;&#62;</a>
                </article>
            </div>
            <div class="slide-bg" style="background-image: url('/Content/images/slideshow_index_page/slide2.jpg');"></div>
        </div>

        <div class="slideshow">
            <div class="slidetext">
                <article style="max-width: 500px">
                    <h1>Future of education is creativity, social-emotional learning: new book</h1>
                    <h4></h4>
                    <p>The book "Pushing the Limits" is shown on a desk in a Toronto area school on Monday Aug. 28, 2017. The key to ensuring our kids are prepared for the unpredictable world that awaits is to make sure today's schools allow them to take risks, try new things and learn how to adapt to change, says educator Nancy Steinhauer.</p>
                    <br />
                    <a href="#">Explore Events &nbsp;&#62;&#62;</a>
                </article>
                <br />
            </div>
            <div class="slide-bg" style="background-image: url('/Content/images/slideshow_index_page/slide3.jpg');"></div>
        </div>

        <div class="slideshow">
            <div class="slidetext">
                <article style="max-width: 450px; float: right">
                    <h1>Foreign Exchange Strategies for Education Sector</h1>
                    <h4></h4>
                    <p>Education sector is a global marketplace that leaves Education Institutions exposed to constantly shifting foreign exchange rates. As budget cuts become more prevalent and government funding dwindles, finance departments face increased pressure to closely monitor costs.</p>
                    <br />
                    <a href="#">Explore Events &nbsp;&#62;&#62;</a>
                </article>
            </div>
            <div class="slide-bg" style="background-image: url('/Content/images/slideshow_index_page/slide4.jpg');"></div>
        </div>

        <div class="slideshow">
            <div class="slidetext">
                <article>
                    <h1>Always moving forward</h1>
                    <h4></h4>
                    <p>Innovation is at the core of what we do. Backed by a dedicated team of talented developers, we focus our brainpower on constantly evolving our technology so that you always stay ahead of the curve.</p>
                    <br />
                    <a href="#">Explore Events &nbsp;&#62;&#62;</a>
                </article>
            </div>
            <div class="slide-bg" style="background-image: url('/Content/images/slideshow_index_page/slide5.png');"></div>
        </div>

        <a class="prev" onclick="plusSlides(-1)">&#10094;</a>
        <a class="next" onclick="plusSlides(1)">&#10095;</a>
    </div>

    <script defer id="slideshowJs"></script>
    <script>
        var path = window.location.protocol + "//" + window.location.host;
        document.getElementById("slideshowJs").src = path + "/Scripts/js/slideshow.js";
    </script>

    <!-- slideshow end -->
</asp:Content>

<asp:Content ID="contentBody" ContentPlaceHolderID="contentBody" runat="server">
    <!-- content start -->
    <div id="bodyContent">
        <div class="content">

            <div class="subContent color1">
                <table>
                    <tr>
                        <td>
                            <h4>The new standard in assessment</h4>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <h3>Get your product to market at record speed by leveraging an infrastructure that’s powerful, flexible, and designed to perform at scale.</h3>
                        </td>
                    </tr>
                    <tr>
                        <!--td><a href="#">Join&nbsp;Society</a></td-->
                    </tr>
                </table>
            </div>

            <div class="subContent color2">
                <table>
                    <tr>
                        <td>
                            <h4>Save time, reduce costs</h4>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <h3>Our team of talented engineers have spent years creating a world-leading assessment solution that you can start using right away, saving you development time and resources.</h3>
                        </td>
                    </tr>
                    <tr>
                        <!--td><a href="">Be&nbsp;Our&nbsp;Facilitator</a></td-->
                    </tr>
                </table>
            </div>

            <div class="subContent color1">
                <table>
                    <tr>
                        <td>
                            <h4>Seamless integration</h4>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <h3>With a trailblazing suite of APIs for faster integration, adding interactive assessment capabilities into your digital product has never been so easy.</h3>
                        </td>
                    </tr>
                    <tr>
                        <!--td><a href="">Be&nbsp;Our&nbsp;Sponsor</a></td-->
                    </tr>
                </table>
            </div>

            <div class="subContent color2">
                <table>
                    <tr>
                        <td>
                            <h4>Always moving forward</h4>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <h3>Innovation is at the core of what we do. Backed by a dedicated team of talented developers, we focus our brainpower on constantly evolving our technology so that you always stay ahead of the <span style="color: #01CBE1;">curve</span>.</h3>
                        </td>
                    </tr>
                    <tr>
                        <!--td><a href="">Donate&nbsp;Now</a></td-->
                    </tr>
                </table>
            </div>

        </div>
    </div>
    <!-- content end -->
</asp:Content>

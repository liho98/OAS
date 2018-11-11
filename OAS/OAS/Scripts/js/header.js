window.onload = function () {
    onScroll();
};

window.onscroll = function () {
    onScroll();
};

var path = window.location.protocol + "//" + window.location.host;

var dot = document.getElementById("spin");
dot.style.transition = "none";
dot.style.transform = "rotate(0deg)";

var nav = document.getElementById("navigation");
var bottom_header = document.getElementById("bottom-header");
var menuLength = document.querySelectorAll(".menu").length;
var sub = document.getElementsByClassName("sub-sub-air-tag");

var media = window.matchMedia("(max-width: 959px)"); //If [device width] is less than or equal to 959px 
resizeMenu(media); // Call listener function at run time
media.addListener(resizeMenu); // Attach listener function on state changes			

var isMenuOpen = false;
var isFirst = true;
var isButton = false;
var scrollToTop = false;
var showWhiteHeight = 0;

var sideMenu = document.getElementById("triangle-div");
var isSideMenuOpen = false;

function onScroll() {
    showWhiteHeight = Math.ceil(window.outerHeight * 0.3);

    if (document.body.scrollTop > showWhiteHeight || document.documentElement.scrollTop > showWhiteHeight) {
        if (!(media.matches) && isButton == false && (isMenuOpen == true || isFirst == true)) {
            //closeDot();
        }
        document.getElementById("header").style.cssText = "background-color:white;color:#5f5d5d;transition: background-color 0.5s ease-in-out,color .5s ease-in-out;-webkit-backface-visibility: hidden;-webkit-transform: translate3d(0, 0, 0);";//avoid css jitter
        document.getElementById("top-header").className = "function1";
        document.getElementById("separator").style.display = "block";
        document.getElementById("icon").src = path + "/Content/images/icons_logos/oas_blue.png";
        if (bottom_header) {
            bottom_header.style.cssText = "display: none;";
        }
    } else {
        if (!(media.matches)) {
            transparentMenu();
            //openDot();
            if (bottom_header) {
                bottom_header.style.cssText = "display: block;";
            }
            isButton = false;
        }
        document.getElementById("header").style.cssText = "background-color:transparent;color:white;transition: background-color 0.5s ease-in-out,color .5s ease-in-out;-webkit-backface-visibility: hidden;-webkit-transform: translate3d(0, 0, 0);";
        document.getElementById("top-header").className = "";
        document.getElementById("separator").style.display = "none";
        document.getElementById("icon").src = path + "/Content/images/icons_logos/oas_white.png";
    }
    isFirst = false;
}

function resizeMenu(media) {
    showWhiteHeight = Math.ceil(window.outerHeight * 0.3);

    if (media.matches) { // If media query matches
        //closeDot();
        if (bottom_header) {
            bottom_header.style.cssText = "display: none;";
        }
    } else {
        if (document.querySelectorAll(".block")[0]) {
            document.querySelectorAll(".block")[0].style.display = "none";
        }

        //initial run, meaning this function's operation will run the most begining
        if (document.body.scrollTop <= showWhiteHeight && document.documentElement.scrollTop <= showWhiteHeight) {
            transparentMenu();
            //openDot();
            if (bottom_header) {
                bottom_header.style.cssText = "display: block;";
            }
        } else {
            if (isMenuOpen == true) {
                whiteMenu();
                //nav.style.position = "relative";
                //openDot();
                if (bottom_header) {
                    bottom_header.style.cssText = "display: block;";
                }
            }
        }
    }
}

function clickMenu() {
    if (isSideMenuOpen == false) {
        openDot();
    } else {
        closeDot();
    }
}

function openDot() {
    // Force the browser recalculate the styles
    dot.getBoundingClientRect();
    dot.style.transition = "all 1s";
    dot.style.transform = "rotate(450deg)";
    sideMenu.style.display = "block";
    isSideMenuOpen = true;
}

function closeDot() {
    dot.getBoundingClientRect();
    dot.style.transition = "all 1s";
    dot.style.transform = "rotate(0deg)";
    sideMenu.style.display = "none";
    isSideMenuOpen = false;
}

//color function
function whiteMenu() {

    if (nav) {
        nav.style.cssText = "background-color: white;color:#5f5d5d;height: 100%;width: 100%;left:0";
        document.getElementById("separatorMenu").style.cssText = "border: 0.5px rgba(222,222,222,1) solid;display: block;margin:0";

        for (var i = 0; i < menuLength; i++) {
            nav.getElementsByClassName("menu")[i].style.cssText = "border-bottom-color: #5f5d5d;";
            document.querySelectorAll(".menu > a")[i].style.color = "#5f5d5d";
            document.querySelectorAll(".submenu > span")[i].style.color = "#5f5d5d";
        }
        for (var j = 0; j < sub.length; j++) {
            sub[j].style.color = "#5f5d5d";
        }
    }
}
//color function
function transparentMenu() {

    if (nav) {
        nav.style.cssText = "background-color: transparent;color:white;";
        document.getElementById("separatorMenu").style.cssText = "display: none;";

        for (var i = 0; i < menuLength; i++) {
            nav.getElementsByClassName("menu")[i].style.cssText = "border-bottom-color: white";
            document.querySelectorAll(".menu > a")[i].style.color = "white";
            document.querySelectorAll(".submenu > span")[i].style.color = "white";
        }
        for (var j = 0; j < sub.length; j++) {
            sub[j].style.color = "#fff";
        }
    }
}

//sub sub-menu (5 colors only, and keep the btnOrder in ascending order sequentially start from 0)
var noOfSubMenu = document.querySelectorAll(".submenu").length;
var subSubMenu = document.querySelectorAll(".sub-sub-menu");
var add = document.querySelectorAll(".add"); //add is a + symbol, and will be rotated to x when it is clicked by user

var isSubMenuOpen = new Array(noOfSubMenu);
for (var i = 0; i < isSubMenuOpen.length; i++) {
    isSubMenuOpen[i] = false;
}

function subButton(btnOrder) {

    for (var i = 0; i < noOfSubMenu; i++) {

        if (isSubMenuOpen[i] == false && btnOrder == i) {
            openSubMenu(btnOrder);
            break;
        }
        if (isSubMenuOpen[i] == true && btnOrder == i) {
            closeSubMenu(btnOrder);
            break;
        }
    }
}

function openSubMenu(btnOrder) {

    var temp = (btnOrder + 1) % 5;

    for (var i = 0; i < subSubMenu.length; i++) {

        if (btnOrder == i) {
            subSubMenu[i].style.cssText = "max-height: 1000px;transition:max-height 1s ease-in-out;";

            if (temp == 1) {
                subSubMenu[i].style.borderLeft = "5px rgb(204, 255, 255) solid";
            } else if (temp == 2) {
                subSubMenu[i].style.borderLeft = "5px rgb(187, 255, 153) solid";
            } else if (temp == 3) {
                subSubMenu[i].style.borderLeft = "5px rgb(255, 179, 204) solid";
            } else if (temp == 4) {
                subSubMenu[i].style.borderLeft = "5px rgb(204, 153, 255) solid";
            } else if (temp == 0) {
                subSubMenu[i].style.borderLeft = "5px rgb(255, 204, 153) solid";
            }
        }
    }

    add[btnOrder].style.cssText = "transition:all 1s;transform:rotate(225deg)";
    isSubMenuOpen[btnOrder] = true;
}

function closeSubMenu(btnOrder) {

    var temp = (btnOrder + 1) % 5;

    for (var i = 0; i < subSubMenu.length; i++) {
        if (btnOrder == i) {
            subSubMenu[i].style.cssText = "max-height: 0;transition:max-height 0.3s ease-in-out;";

            if (temp == 1) {
                subSubMenu[i].style.borderLeft = "5px rgb(204, 255, 255) solid";
            } else if (temp == 2) {
                subSubMenu[i].style.borderLeft = "5px rgb(187, 255, 153) solid";
            } else if (temp == 3) {
                subSubMenu[i].style.borderLeft = "5px rgb(255, 179, 204) solid";
            } else if (temp == 4) {
                subSubMenu[i].style.borderLeft = "5px rgb(204, 153, 255) solid";
            } else if (temp == 0) {
                subSubMenu[i].style.borderLeft = "5px rgb(255, 204, 153) solid";
            }
        }
    }

    add[btnOrder].style.cssText = "transition:all 1s;transform:rotate(0deg)";
    isSubMenuOpen[btnOrder] = false;
}

function scrollBackTop() {
    document.querySelector('body').scrollIntoView({ behavior: "smooth", block: "start", inline: "nearest" });
    scrollToTop = true;
}
//set height for background
function dynamicSetHeight(bodyHeight) {
    windowHeight = $(window).outerHeight(true);

    $('.background').css('margin-top', windowHeight);
    $('.background-image').css('height', windowHeight);

    var tempHeight, adjustedHeight;

    tempHeight = Math.ceil(windowHeight * 0.9);
    adjustedHeight = tempHeight;

    if ($('.slide-bg-container').length) {

        $('.slide-bg-container').css('top', windowHeight);
        $('.slide-bg-container').css('height', adjustedHeight);
        /* sum of previous 2 height = next adjusted height */
        adjustedHeight += windowHeight;

    } else {
        tempHeight = Math.ceil(windowHeight * 0.4);
        adjustedHeight = tempHeight;
    }

    tempHeight = Math.ceil(windowHeight * bodyHeight);
    $('#bodyContent').css('top', adjustedHeight);
    $('#bodyContent').css('height', tempHeight);

    /* sum of previous 2 height = next adjusted height */
    adjustedHeight += tempHeight;

    $('.subContent').css('height', Math.ceil(tempHeight / 4));

    $('#footerDiv').css('top', adjustedHeight);
    $('#footerDiv').css('height', 400);

    /* sum of previous 2 height = next adjusted height */
    adjustedHeight += 400;
}
function setLeftTriangle() {
    var element = document.getElementById('spin');
    var elem = document.getElementById("triangle-div");
    var positionInfo = element.getBoundingClientRect();    
    elem.style.left = (positionInfo.left - 193)+"px";  
}

$( window ).on( "orientationchange", function() {
    setHeight();
    var elem = document.getElementById("triangle-div");  
    if(window.orientation == 0) // Portrait
    {
        elem.style.height = "500px";   
    }
    else // Landscape
    {
        elem.style.height = "300px";   
    }    
});
var slideIndex = 1;
var timer = null;

showSlides(slideIndex);

function plusSlides(n) {
    clearTimeout(timer);
    showSlides(slideIndex += n);
}

function showSlides(n) {
    var i;
    var slides = document.getElementsByClassName("slideshow");

    if (n == undefined) {
        n = ++slideIndex
    }
    if (n > slides.length) {
        slideIndex = 1
    }
    if (n < 1) {
        slideIndex = slides.length
    }

    for (i = 0; i < slides.length; i++) {
        slides[i].style.cssText = "width:0;height:0;visibility:hidden;opacity:0";
    }

    slides[slideIndex - 1].style.cssText = "width:110%;height:110%;visibility:visible;opacity:1";
    timer = setTimeout(showSlides, 15000);
}

//swipe using touch event handler
var swipe = document.getElementById("swipe");
var initialX = null;
var initialY = null;

//touch event handle
document.addEventListener('touchstart', handleTouchStart, false);
document.addEventListener('touchmove', handleTouchMove, false);

function handleTouchStart(e) {
    initialX = e.touches[0].clientX;
    initialY = e.touches[0].clientY;
};

function handleTouchMove(e) {
    if (initialX === null) {
        return;
    }
    if (initialY === null) {
        return;
    }

    var diffX = initialX - e.touches[0].clientX;
    var diffY = initialY - e.touches[0].clientY;

    if (Math.abs(diffX) > Math.abs(diffY)) {
        // sliding horizontally
        if (diffX > 5) {
            // swiped left
            clearTimeout(timer);
            showSlides(slideIndex += 1);
        } else if (diffX < -5) {
            // swiped right
            clearTimeout(timer);
            showSlides(slideIndex -= 1);
        }
    } else {
        // sliding vertically
        if (diffY > 0) {
            // swiped up
        } else {
            // swiped down
        }
    }
    initialX = null;
    initialY = null;
    //e.preventDefault();                                                  
};

// pointer event handle the future
swipe.addEventListener("pointerdown", function (e) {
    // Process the event
    switch (e.pointerType) {
        case "mouse":
            initialX = e.clientX;
            initialY = e.clientY;
            break;
        case "pen":
            /* pen/stylus input detected */
            break;
        case "touch":
            break;
        default:
        /* pointerType is empty (could not be detected)
         or UA-specific custom type */
    }

});

swipe.addEventListener("pointerup", function (e) {
    // Process the event
    switch (e.pointerType) {
        case "mouse":
            if (initialX === null) {
                return;
            }
            if (initialY === null) {
                return;
            }

            var diffX = initialX - e.clientX;
            var diffY = initialY - e.clientY;

            if (Math.abs(diffX) > Math.abs(diffY)) {
                // sliding horizontally
                if (diffX > 220) {
                    // swiped left
                    clearTimeout(timer);
                    showSlides(slideIndex += 1);
                } else if (diffX < -220) {
                    // swiped right
                    clearTimeout(timer);
                    showSlides(slideIndex -= 1);
                }
            } else {
                // sliding vertically
                if (diffY > 0) {
                    // swiped up
                } else {
                    // swiped down
                }
            }
            initialX = null;
            initialY = null;
            //e.preventDefault();
            break;
        case "pen":
            /* pen/stylus input detected */
            break;
        case "touch":
            break;
        default:
        /* pointerType is empty (could not be detected)
         or UA-specific custom type */
    }
});
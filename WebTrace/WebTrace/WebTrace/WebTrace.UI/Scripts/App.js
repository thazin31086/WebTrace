$("#webtrace").steps({
    headerTag: "h2",
    bodyTag: "section",
    transitionEffect: "slideLeft",
    stepsOrientation: "horizontal"
});


$(".draggable").draggable();
$(".droppable").droppable({
    drop: function (event, ui) {
        $(this)
            .addClass("ui-state-highlight")
            .find("p")
            .html("Dropped!");
    }
});
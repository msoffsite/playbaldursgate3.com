$(function () {
    populateWatchVideosLink();

    $(".carousel-indicators :button").on("click", function () {
        populateWatchVideosLink();
    });

    $(".carousel-control-next").on("click", function () {
        populateWatchVideosLink();
    });

    $(".carousel-control-prev").on("click", function () {
        populateWatchVideosLink();
    });

    $(".hide_title_description").on("click", function () {
        var currentTitleDescription = $(".carousel-item.active .playlist-title-description");
        currentTitleDescription.hide();
        showCarouselControls();
    });

    $(".title-description").on("click", function () {
        var currentTitleDescription = $(".carousel-item.active .playlist-title-description");
        if (currentTitleDescription.is(":visible")) {
            currentTitleDescription.hide();
            showCarouselControls();
        }
        else {
            currentTitleDescription.show();
            hideCarouselControls();
        }
    });

    function hideCarouselControls() {
        $(".carousel-control-next").hide();
        $(".carousel-control-prev").hide();
        $(".carousel-indicators").hide();
    }

    function populateWatchVideosLink() {
        setTimeout(() => {
            var playListId = $(".carousel-item.active .playlist-id").data("playlist-id");
            $("#watch_videos").attr("href", "/Videos/" + playListId);
        },
            1500
        );
        
    }

    function showCarouselControls() {
        $(".carousel-control-next").show();
        $(".carousel-control-prev").show();
        $(".carousel-indicators").show();
    }
});
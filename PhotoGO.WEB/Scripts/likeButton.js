$(function () {
    if ($(".like-button").size() > 0) {
        $(".like-button").on("click", function () {
            var btn = $(this);
            $.get(btn.data("link"), function (data) {
                var counter = btn.find("span");
                var before = $(counter).text();
                var color;

                if (before < data) {
                    color = '#cf0020';
                }
                else {
                    color = '#000000';

                    if (document.getElementById("fav") !== null)
                        $(btn).parentsUntil(".col-md-2").remove();
                    
                }

                $(counter).fadeOut(function () {
                    btn.find("i").css('color', color);
                    $(this).text(data);
                    $(this).fadeIn();
                });
            });            
        });
    }
});
$(function () {

    if ($(".like-button").size() > 0) {
        var postClient = $.connection.postHub;

        $(".like-button").on("click", function () {
            var btn = $(this);
            $.get(btn.data("link"), function (data) {
                var counter = btn.find("span");
                var before = $(counter).text();
                if (before < data) {
                    btn.find("i").css('color', '#cf0020');
                }
                else {
                    btn.find("i").css('color', '#000000');
                }
                $(counter).fadeOut(function () {
                    $(this).text(data);
                    $(this).fadeIn();
                });
            })            
        });

        $.connection.hub.start();
    }

});
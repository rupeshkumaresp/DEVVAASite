(function($) {
    $(document).ready(function() {
        var str = location.href.toLowerCase();
        $("#cssmenu li a").each(function() {
            if (str.indexOf($(this).attr("href").toLowerCase()) > -1) {
                $("li.active").removeClass("active");
                $(this).parent().addClass("active");
            }
        });
        $("li.active").parents().each(function() {
            if ($(this).is("li")) {
                $(this).addClass("active");
            }
        });
    });
})(jQuery);
$(document).ready(function () {
    $("#btnOpen").click(function () {
        $(".overlay,.popup").fadeIn();
        StartProgressBar();
        CallApi();
    });

    function StartProgressBar() {
        var currentDate = new Date();
        var second = currentDate.getUTCSeconds();
        if (second < 10) {
            second = "0" + second;
        }

        $(".progress-bar").css('width', second + '%');
        setTimeout(function () {
            StartProgressBar()
        },500);

    }

    // close progress bar
    function CloseProgressBar() {
        $("#Fade_area").removeAttr("style");
        $("#myModal").removeAttr("style");
    }

    function CallApi() {
        $.ajax({
            url: "",
            type: "post",
            contenteType: "application/json",
            success: function () {
                    setTimeout(function () {
                    CloseProgressBar()
                },10000);
            },
            error: function () {
                setTimeout(function () {
                    CloseProgressBar()
                }, 10000);
            }
        });
    }
});
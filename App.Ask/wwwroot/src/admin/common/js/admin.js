$(function () {
    $(".navbar-toggler").click(function() {
        $(".app-sidebar").toggleClass("toggle");
        $(".app-page").toggleClass("toggle");
        $(".app-navbar").toggleClass("toggle");
    });
});

$(function () {
    $(".metismenu").metisMenu();
});

$.ajaxSetup({
    crossDomain: true,
    xhrFields: {
        withCredentials: true
    }
});
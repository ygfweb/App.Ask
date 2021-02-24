$(document).ready(function(){
    $('#scroll').click(function(){
        $("html, body").animate({ scrollTop: 0 }, 600);
        return false;
    });
    if (document.body.clientWidth > 800) {
        $("#cs_box").removeClass("d-none");
    }
    $(".cs_close").click(function () {
        $("#cs_box").remove();
    })
});
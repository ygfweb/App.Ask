$(function () {
    $("#code_image").on("click",function () {
        $("#code_image").attr("src","/CodeImage?r="+Math.random());
    });   
});


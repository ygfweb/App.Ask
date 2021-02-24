$(function () {
    $("#code_image").on("click", function () {
        $("#code_image").attr("src", "/CodeImage?r=" + Math.random());
    });
});


function onBegin() {
    $("#btn_submit").attr("disabled","disabled");
    $("#spinner-wrapper").removeClass("d-none");
}

function onComplete() {
    $("#spinner-wrapper").addClass("d-none");
    $("#btn_submit").removeAttr("disabled");
}

function onSuccess(context) {
    if (context.Success){
        window.location.href = "/";
    }else {
        $("#register_form").data('validator').showErrors(context.ErrorMessages);
        $("#code_image").attr("src","/CodeImage?r="+Math.random());
    }
}


$(function () {
    $("#send_email").click(function () {
        $("#spinner-wrapper").removeClass("d-none");
        $("#send_email").attr("disabled","disabled");

        function beforeSend(){
            _showSpinner();
        }
        function complete(){
            $("#spinner-wrapper").addClass("d-none");
            $("#send_email").removeAttr("disabled");
        }
        function error(msg){
            BootstrapDialog.alert(msg);
        }
        function success(data){
            if (data.Success) {
                swal({
                    title: "发送成功",
                    text: "验证码已发送到您的邮箱，请检查您的邮箱。如果未收到邮件，请查看邮件是否被纳入垃圾箱中。",
                    icon: "success",
                    button: "确 定",
                });
            } else {
                let v =  $("#register_form").validate();
                v.showErrors(data.ErrorMessages);
                if (data.Message){
                    BootstrapDialog.alert(data.Message);
                } 
            }
        }
        
        let form = document.getElementById("register_form");
        let data = new FormData(form);
        _sendAjaxPost("/Account/SendEmail",data,beforeSend,error,success,complete); 
    });
});
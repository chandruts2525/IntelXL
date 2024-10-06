$(document).ready(function () {

    $('.learn, .learning-slide').hover(
        function () {
            $('.learning-slide').stop().slideDown(200);
        },
        function () {
            $('.learning-slide').stop().slideUp(200);
        }
    );

    var currentUrl = window.location.pathname;
    if (currentUrl === '/Membership/Inspriation') {
        $('.ins-btn').addClass('active');
    }
    if (currentUrl === '/Membership/Index') {
        $('.join-btn').addClass('active');
    }

    $(".sign_in_btn").on('click', function () {
        $(".form-control").removeClass("is-invalid");
        $(".loginerror ,.error-msg").text("");

        var isValid = true;

        $(".log-control").each(function () {
            if ($(this).val().trim() === "") {
                $(this).addClass("is-invalid");
                isValid = false;
            }
        });
        if (!isValid) {
            $(".loginerror").text("* fields are required.");
        }
        else {
            if (!emailvalidation($(".username-login").val())) {
                $(".loginerror").text("Enter a valid email address.");
            }
            else {
                let username = $('.username-login').val().trim();
                let password = $('.password-login').val().trim();
                if (username && password) {
                    $('.sign_in_btn').hide();
                    $("#loginmodelbtn").show();
                    validateUser(username, password);
                }
            }
        }
    });
    $(document).on('click', '#logoutbtn', function () {
        window.location.href = "/User/LogOut";
    });
    $(document).on('click', '#loginbtn', function () {
        CurrentPath = window.location.pathname + window.location.search;
        let username = $('.user').val().trim();
        let password = $('.password').val().trim();
        if (username && password) {
            if (!emailvalidation(username)) {
                $("#LoginModal").modal("show");
                $(".error-msg").text("Enter a valid email.");
            }
            else {
                $('#loginbtn').hide();
                $("#loging").show();
                validateUser(username, password);
            }
        }
        else {
            $("#LoginModal").modal("show");
        }

    });
    $(document).on('click', '.reg-btn', function () {
        let role_id = $(this).data("role");
        $("#registerModal").find("#hdnRole").val(role_id);
        $("#LoginModal").modal("hide");
        if (role_id == 1)
            $("#registerModal").find(".modal-title").text("Sign up as a Student");
        else
            $("#registerModal").find(".modal-title").text("Sign up as a Tutor");
        $("#registerModal").modal("show");
    });
    $('#show-password').change(function () {
        if ($(this).is(':checked')) {
            $('#password, #confirm-password').attr("type", "text");
        }
        else {
            $('#password, #confirm-password').attr("type", "password");
        }
    });

    //reset login/register modal
    $('#LoginModal,#registerModal').on('hidden.bs.modal', function () {
        $('.sign_in_btn').prop('disabled', false);
        $('input').removeClass('is-invalid');
        $(".loginerror ,.error-msg").text("");
        $('.username-login').val('');
        $('.password-login').val('');
        $(".errorfield").text("");
        $("#registrationform")[0].reset();
    });
});
var CurrentPath ="";
//Register Validations
function validateUser(emailId, password) {
    let isPersist = $(".remember").is(':checked');
    /*  let url = window.location.pathname + window.location.search;  */
    $('.sign_in_btn').prop('disabled', true);
    $.ajax({
        url: "/User/ValidateUser?userId=" + emailId.toLowerCase() + "&password=" + password,
        type: "POST",
        success: function (response) {
            if (!response.userName) {
                $('.sign_in_btn').prop('disabled', false);
                $('.error-msg').text("Email id or password is invalid!");
                $("#LoginModal").modal("show");
                $('#loginbtn,.sign_in_btn').show();
                $("#loging ,#loginmodelbtn").hide();
            }
            else {
                window.location.href = "/User/Signin?emailId=" + emailId.toLowerCase() + "&password=" + password + "&returnUrl=" + encodeURIComponent(CurrentPath) + "&isPersist=" + isPersist;
            }

        }, error: function (xhr, status, error) {
            alert("Unable to login");
            console.error("Error: " + error);
            $('#loginbtn,.sign_in_btn').show();
            $("#loging ,#loginmodelbtn").hide();
        }
    });
}
function emailvalidation(email) {
    var emailReg = /^([a-zA-Z0-9_.+-])+\@(([a-zA-Z0-9-])+\.)+([a-zA-Z0-9]{2,4})+$/;
    if (!emailReg.test(email)) {
        return false;
    } else {
        return true;
    }
}
function isPasswordValid(password) {
    var regex = /^(?=.*\d)(?=.*[a-z])(?=.*[A-Z])(?=.*[!@#$%^&*_-]).{8,20}$/;
    return regex.test(password);
}
$(document).ready(function () {
    $('.reg-control,.log-control').on('input', function () {
        if ($(this).val().trim() === "") {
            $(this).addClass("is-invalid");
        } else {
            $(this).removeClass('is-invalid');
        }
    });
    $('#register-button').click(function () {
        $(".form-control").removeClass("is-invalid");
        $(".errorfield").text("");

        var isValid = true;

        // Check if any of the required fields is empty
        $(".reg-control").each(function () {
            if ($(this).val().trim() === "") {
                $(this).addClass("is-invalid");
                isValid = false;
            }
        });

        if (!isValid) {
            $(".errorfield").text("* fields are required.");
        }
        else {
            if (!emailvalidation($("#email").val())) {
                $(".errorfield").text("Enter a valid email.");
            }
            else {
                $(this).hide();
                $('#regmodelbtn').show();
                $.get(window.location.origin + "/User/IsUserExist?emailId=" + $("#email").val(), function (response) {
                    if (response) {
                        $(".errorfield").text("Email id already exists");
                    }
                    else {
                        let password = $('#password').val();
                        let confirmPassword = $('#confirm-password').val();
                        if (!isPasswordValid(password)) {
                            $(".errorfield").text("Password must have at least one uppercase letter, one lowercase letter, one symbol, and one number.");
                        }
                        else if (password !== confirmPassword) {
                            $(".errorfield").text("Password and confirm password must match");
                        }
                        else {
                            $("#registrationform").submit();
                        }
                    }
                    $("#register-button").show();
                    $('#regmodelbtn').hide();
                });
            }
        }
    });
});
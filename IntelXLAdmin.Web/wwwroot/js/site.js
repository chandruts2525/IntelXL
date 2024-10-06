$(document).ready(function () {   
    $('#show-password').change(function () {
        if ($(this).is(':checked')) {
            $('#password, #confirm-password').attr("type", "text");
        }
        else {
            $('#password, #confirm-password').attr("type", "password");
        }
    });
    $('#register-button').click(function () {       
        $(".form-control").removeClass("is-invalid");
        $(".error-info").text("");

        var isValid = true;
          
        // Check if any of the required fields is empty
        $(".reg-form").each(function () {
            if ($(this).val().trim() === "") {
                $(this).addClass("is-invalid");
                isValid = false;
            }
        });
        if (!($(".select-position").val())) {
            $(".select-position").addClass("is-invalid");
            isValid = false;
        }
        if (!isValid) {
            $(".error-info").text("* fields are required.");
        }
        else {
            if (!emailvalidation($("#email").val())) {
                $(".error-info").text("Enter a valid email address.");
            }
            else {
                $.get(window.location.origin + "/Users/IsUserExist?emailId=" + $("#email").val(), function (response) {
                    if (response) {
                        $(".error-info").text("Email id already exists");
                    }
                    else {
                        let password = $('#password').val();
                        let confirmPassword = $('#confirm-password').val();
                        if (!isPasswordValid(password)) {
                            $(".error-info").text("Password must have at least one uppercase letter, one lowercase letter, one symbol, and one number.");
                        }
                        else if (password !== confirmPassword) {
                            $(".error-info").text("Password and confirm password must match");
                        }
                        else {
                            Register();                           
                        }
                    }
                });
            }
        }
    });
});

function Register() {
    var form = $('#registrationform');
    var formData = new FormData(form[0]); 
    $.ajax({
        url: '/Users/Registration',
        type: 'POST',
        data: formData,
        processData: false,
        contentType: false,
        success: function (response) {           
            if (response) {
                alert("registration Successful")
                window.location.href ='/Users/Register'
            }
            else (
                alert("registration failed")
            )
        },
        error: function (error) {          
            console.log(error);
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
// Function: showtoast
// Parameters:
//   - type: String (success, error, info, warning)
//   - message: String (The message to be displayed in the toast)
// Description: Displays a toast notification with the specified type and message.
function  showtoast(type, message) {
    toastr.options = {
        "closeButton": true,
        "newestOnTop": true,
        "progressBar": true,
        "positionClass": "toast-top-center",
        "timeOut": "5000",
    };
    $('.toast-message').css('white-space', 'nowrap');
    toastr[type](message);
}

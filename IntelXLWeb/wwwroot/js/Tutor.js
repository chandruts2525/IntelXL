$(document).ready(function () {
    //$('#show-password').change(function () {
    //    if ($(this).is(':checked')) {
    //        $('#password, #confirm-password').attr("type", "text");
    //    }
    //    else {
    //        $('#password, #confirm-password').attr("type", "password");
    //    }
    //});
    //$('#register-button').click(function () {
    //    $(".form-control").removeClass("is-invalid");
    //    $(".error-info").text("");

    //    var isValid = true;

    //    // Check if any of the required fields is empty
    //    $(".reg-form").each(function () {
    //        if ($(this).val().trim() === "") {
    //            $(this).addClass("is-invalid");
    //            isValid = false;
    //        }
    //    });       
    //    if (!isValid) {
    //        $(".error-info").text("* fields are required.");
    //    }
    //    else {
    //        if (!emailvalidation($("#email").val())) {
    //            $(".error-info").text("Enter a valid email address.");
    //        }
    //        else {
    //            $.get(window.location.origin + "/Tutors/IsTutorExist?emailId=" + $("#email").val(), function (response) {
    //                if (response) {
    //                    $(".error-info").text("Email id already exists");
    //                }
    //                else {
    //                    let password = $('#password').val();
    //                    let confirmPassword = $('#confirm-password').val();
    //                    if (!isPasswordValid(password)) {
    //                        $(".error-info").text("Password must have at least one uppercase letter, one lowercase letter, one symbol, and one number.");
    //                    }
    //                    else if (password !== confirmPassword) {
    //                        $(".error-info").text("Password and confirm password must match");
    //                    }
    //                    else {
    //                        $("#registrationform").submit();
    //                       // Register();
    //                    }
    //                }
    //            });
    //        }
    //    }
    //});
    $(".ix-availability").on('click', function () {
        $(this).css({
            "border": "1.5px solid #006af5",
            "background-color": "#fff",
            "position": "absolute",
            "z-index": 2
        });
        if ($(".overlay").length === 0) {
            $(".ix-time-pop").show();
            var overlay = $("<div class='overlay'></div>");
            overlay.css({
                "position": "absolute",
                "top": 0,
                "left": 0,
                "width": "100%",
                "height": "100%",
                "background-color": "#000",
                "opacity": "0.5",
                "z-index": 1
            });

            $("main").append(overlay);
        }       
    });
    //$("#priceRange").ionRangeSlider({
    //    type: "double",
    //    grid: true,
    //    min: 0,
    //    max: 3000,
    //    from: 0,
    //    to: 3000,
    //    step: 1,
    //    onFinish: function (data) {
    //        $("#pricerangespan").text("₹" + data.from + "-₹" + data.to);
    //        $("#minValue").text(data.from);
    //        $("#maxValue").text(data.to == 3000 ? "3000+" : data.to);
    //    },
    //});
    $('#birthCountry,#Country').select2({
        placeholder: 'Select countries',
        // templateSelection: customTemplateSelection
    }).on('select2:select', function (e) {
        var data = e.params.data;
        console.log(data);
    });
    var inputField = $("#language");
    var autosuggestContainer = $("#autosuggest");

    // Sample suggestions
    var suggestions = ["English", "Spanish", "French", "German", "Chinese", "Japanese"];

    inputField.on("input focus", function () {
        inputField.val("");
        showSuggestions();
    });

    function showSuggestions() {
        var inputValue = inputField.val().toLowerCase();
        var matchingSuggestions = $.grep(suggestions, function (suggestion) {
            return suggestion.toLowerCase().includes(inputValue);
        });

        if (matchingSuggestions.length > 0) {
            autosuggestContainer.html("");
            $.each(matchingSuggestions, function (index, suggestion) {
                var suggestionElement = $("<div>").text(suggestion);
                suggestionElement.on("click", function () {
                    inputField.val(suggestion);
                    autosuggestContainer.hide();
                });
                autosuggestContainer.append(suggestionElement);
            });

            autosuggestContainer.show();
        } else {
            autosuggestContainer.hide();
        }
    }

    $(document).on("click", function (event) {
        if (!inputField.is(event.target) && !autosuggestContainer.is(event.target) && autosuggestContainer.has(event.target).length === 0) {
            autosuggestContainer.hide();
        }
    });
});

//function emailvalidation(email) {
//    var emailReg = /^([a-zA-Z0-9_.+-])+\@(([a-zA-Z0-9-])+\.)+([a-zA-Z0-9]{2,4})+$/;
//    if (!emailReg.test(email)) {
//        return false;
//    } else {
//        return true;
//    }
//}
//function isPasswordValid(password) {
//    var regex = /^(?=.*\d)(?=.*[a-z])(?=.*[A-Z])(?=.*[!@#$%^&*_-]).{8,20}$/;
//    return regex.test(password);
//}
function showtoast(type, message) {
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
$(document).on('click', function (event) {
    if ($(event.target).closest('.overlay').length) {
        $(".overlay").remove();
        $(".ix-time-pop,.range-select").hide();
        $(".ix-availability,.ix-pricerange").css({
            "border": "1.5px solid #c5c5c5",
            "position": "unset",
        });
    }
});
//$(".ix-pricerange").on('click', function () {
//    $(this).css({
//        "border": "1.5px solid #006af5",
//        "background-color": "#fff",
//        "width": "100%",
//        "position": "absolute",
//        "z-index": 2
//    });
//    $(".range-select").show();
//    var overlay = $("<div class='overlay'></div>");
//    overlay.css({
//        "position": "absolute",
//        "top": 0,
//        "left": 0,
//        "width": "100%",
//        "height": "100%",
//        "background-color": "#000",
//        "opacity": "0.5",
//        "z-index": 1
//    });
//    if ($(".overlay").length === 0) {
//        $("main").append(overlay);
//    }
//});
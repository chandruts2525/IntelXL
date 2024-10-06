
$(document).ready(function () {

    //Remove after mobile subscription
    if (isSignedIn == 'True') {
        $("#LoginModal").modal("show");
    }

    const incrementButton = $('.incre-btn');
    const decrementButton = $('.decre-btn');
    const childCountSpan = $('.child-count');

    incrementButton.click(function () {
        let currentCount = parseInt(childCountSpan.text());
        if (currentCount < 10) {
            childCountSpan.text(currentCount + 1);
            decrementButton.prop('disabled', false);
        }
        // Disable the increment button if the count reaches 10
        if (parseInt(childCountSpan.text()) === 10) {
            incrementButton.prop('disabled', true);
        }
    });

    decrementButton.click(function () {
        let currentCount = parseInt(childCountSpan.text());
        if (currentCount > 1) {
            childCountSpan.text(currentCount - 1);
            incrementButton.prop('disabled', false);
        }
        // Disable the decrement button if the count is 1
        if (parseInt(childCountSpan.text()) === 1) {
            decrementButton.prop('disabled', true);
        }
    });
    $(document).on('click', '.package-selection-card', function () {
        $('.package-selection-card-btn').text("Select");
        $(this).find('.package-selection-card-btn').text("Selected");
        $(this).find('input').prop("checked", true);
    });
    $(document).on('change', '.paymentPlan', function () {
        $('.subscription-flow-radio-button-field').removeClass('active');
        $(this).closest('label').addClass('active');
        let currentPlan = $(this).val();
        let monthlyPrice = $('.hdnprice').map(function () {
            return parseFloat($(this).val());
        }).get();
        if (currentPlan === 'month') {
            $('.subscription-price .amt').each(function (index) {
                $(this).text(monthlyPrice[index]+".00" + '/mo');
            });
        }
        else {
            let yearlyPrice = monthlyPrice.map(function (val) {
                let total = val * 12;
                return Math.round(total);
            });
            $('.subscription-price .amt').each(function (index) {
                $(this).text(yearlyPrice[index]+".00" + '/yr');
            });
        }
    });
    $("#course-selection").on('change', function () {
        let v = $(this).val();
        $(".product-package-fieldset .sub-content").html("");
        $("#class-selection").prop("disabled", false);
        $("#class-selection").empty();
        $("#class-selection").append($("<option selected disabled>Select here...</option>"));
        $.ajax({
            type: "GET",
            url: "/Membership/GetAllById",
            data: {id:v},
            success: function (response) {
                if (response.length !== 0) {
                    $.each(response, function (index, value) {
                        $("#class-selection").append($("<option value=" + value.classId + ">" + value.className + "</option>"));
                    });
                }
            },
            error: function (xhr, status, error) {
                console.error("Error: " + error);
            }
        });
    });
    $("#class-selection").on('change', function () {
        let course_val = $('#course-selection').val();
        let class_val = $(this).val();
        $(".loading").show();
        $(".sub-content").html("");
        $.ajax({
            type: "GET",
            url: "/Membership/GetSubscriptions",
            data: { courseId: course_val, classId: class_val },
            success: function (response) {
                $(".loading").hide();
                $(".product-package-fieldset .sub-content").html(response);
               
            },
            error: function (xhr, status, error) {
                console.error("Error: " + error);
            }
        });
    });
    //$(document).on('click', '.package-selection-card-btn', function () {
    //    let id = $(this).find('.SubscriptionId').val();
    //    let paymentPlan = $('.paymentPlan:checked').val(); 
    //    if (id) {
    //        window.location.href = "/Payment/Subscribe?id=" + id + "&plan=" + paymentPlan;
    //    } 
    //});
});
//function Subscribe() {
//    let id = $('.SubscriptionId:checked').val();
//    let paymentPlan = $('.paymentPlan:checked').val();   
//    if (id) {
//        window.location.href = "/Payment/Subscribe?id=" + id + "&plan=" + paymentPlan;
//    }    
//}
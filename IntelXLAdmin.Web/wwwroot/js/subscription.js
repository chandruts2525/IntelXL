$.ajax({
    url: "/Question/GetAllCourses",
    type: "GET",
    success: function (data) {
        if (data.length !== 0) {
            $.each(data, function (index, value) {                
                $("#courseSub").append($("<option value=" + value.courseId + ">" + value.courseName + "</option>"));
            });
        }
    },
    error: function (error) {
        console.log("failed to load dropdown items");
    }
});
$(document).ready(function () {
    let beforeedit = "";
    let afteredit = "";
    let hasChanged = false;  
    let currentlyEditingItem = null;
    $(document).on("click", ".edit-button, .cancel-button", function () { 
        if (currentlyEditingItem && currentlyEditingItem !== $(this).closest(".subscription-card")) {
            cancelEditing(currentlyEditingItem);
        }
        beforeedit = JSON.stringify($(this).closest("form").serializeArray());
        let item = $(this).closest(".subscription-card");

        // Update the currently editing item
        currentlyEditingItem = item;
        item.find('.edt-save-button').prop('disabled', true);
        item.find(".edit-value").toggle();
        item.find(".currency-edit").toggle();
        item.find(".display-value").toggle();
        item.find(".edit-button").toggle();
        item.find(".delete-button").toggle();
        item.find(".edt-save-button").toggle();
        item.find(".cancel-button").toggle();
        $('input').removeClass('is-invalid');
    });

    $(document).on("click", ".cancel-button", function () {
        cancelEditing($(this).closest(".subscription-card"));
    });

    // Function to cancel editing on a specific item
    function cancelEditing(item) {
        item.find('.edt-save-button').prop('disabled', false);
        item.find(".edit-value").toggle();
        item.find(".currency-edit").toggle();
        item.find(".display-value").toggle();
        item.find(".edit-button").toggle();
        item.find(".delete-button").toggle();
        item.find(".edt-save-button").toggle();
        item.find(".cancel-button").toggle();

        $('input').removeClass('is-invalid');

        currentlyEditingItem = null;
    }
    $(document).on('input', '.editSubscription input,textarea,select', function () {
        afteredit = JSON.stringify($(this).closest("form").find(':input').serializeArray());

        if (afteredit.trim() !== beforeedit.trim()) {
            hasChanged = true;
            $(this).closest("form").find('.edt-save-button').prop('disabled', false);
        } else {
            hasChanged = false;
            $(this).closest("form").find('.edt-save-button').prop('disabled', true);
        }
    });

    $(document).on("click", ".edt-save-button ", function () {
       
        isValid = true;
        let form = $(this).closest(".editSubscription");
        form.find('.edt-save-button').prop('disabled', true);
        form.find(".edit-required").each(function () {
            if ($(this).val().trim() === "") {
                $(this).addClass("is-invalid");
                isValid = false;
            }
        });
        if (!isValid) {
            return false;
        }
        else {
            $(".edit-required").removeClass("is-invalid");           
            let formData = new FormData(form[0]);
            $.ajax({
                url: "/Subscriptions/EditSubscription",
                type: "PUT",
                data: formData,
                processData: false,
                contentType: false,
                success: function (response) {
                    if (response) {
                        showtoast("success", "Edit saved Successfully");
                        setTimeout(function () {
                            window.location.href = "/Subscriptions/Index";
                        }, 5000);                         
                       
                    }
                    else {
                        showtoast("error", "Something went wrong. Please try again");
                        form.find('.edt-save-button').prop('disabled', false);
                    }
                },
                error: function (error) {
                    showtoast("error", "Something went wrong. Please try again");
                    form.find('.edt-save-button').prop('disabled', false);
                }
            });
        }
       
    });

    $("#courseSub").on("change", function () {
        $("#classSub").prop("disabled", false);
        $("#classSub").empty();
        $("#classSub").append($("<option selected disabled>Select here...</option>"));
        let courseId = $(this).val(); 
        $.ajax({
            url: "/Classes/GetAllById",
            type: "GET",
            data: { id: courseId },
            success: function (data) {              
                if (data.length !== 0) {
                    $.each(data, function (index, value) {
                        $("#classSub").append($("<option value=" + value.classId + ">" + value.className + "</option>"));
                    });
                }
            },
            error: function (error) {
                console.log("failed to load dropdown items");
            }
        });
    });

    //Delete Subscription
    let Subscription_id;
    let current;
    $(document).on("click", ".delete-button", function () {
        //Getting current div and its id  
        current = $(this).closest(".editSubscription");
        Subscription_id = current.find(".hdnSubscriptionId").val();
        $('#delDiscardModal').modal('show')       
    });

    $(document).on('click', '#ok-btnDiscard', function (event) {
        $('#delDiscardModal').modal('hide');
        //Ajax request to delete
        $.ajax({
            url: "/Subscriptions/Delete",
            data: { id: Subscription_id },
            success: function (response) {
                if (response) {
                    showtoast("success", "Subscription deleted Successfully");
                    //Remove the deleted one from list
                    current.slideUp(100, function () {
                        $(this).remove();
                        if ($(".editSubscription").length == 0) {
                            window.location.href = "/Subscriptions/Index";
                        }
                    });
                }
                else {
                    showtoast("error", "Something went wrong. Please try again");
                }
            },
            error: function (error) {
                showtoast("error", "Something went wrong. Please try again");
            }
        });
    });
});
//All scripts for Tutor details getting modal
$(document).ready(function () {
    $('#ix-activationModal').on('show.bs.modal', function () {
        showTab(currentTab); // Display the current tab       
    });
    $('#ix-activationModal').on('hidden.bs.modal', function () {
        currentTab = 0;
        $(".tab").hide();
        $("#ix-multipageform").trigger("reset");
    });
});
function showTab(n) {
    // This function will display the specified tab of the form...
    var $x = $(".tab");
    $x.eq(n).show();

    //... and fix the Previous/Next buttons:
    if (n === 0) {
        $("#prevBtn").hide();
    } else {
        $("#prevBtn").show();
    }

    if (n === ($x.length - 1)) {
        $("#nextBtn").text("Submit");
    } else {
        $("#nextBtn").text("Next");
    }
    fixStepIndicator(n)
}
function nextPrev(n) {
    var $x = $(".tab");
   if (n == 1 && !validateForm()) return false;
    $x.eq(currentTab).hide();
    currentTab = currentTab + n;
    if (currentTab == 3) {
        showTab(currentTab);        
        saveCertification(function (response) {
            if (response) {
                showtoast("info", "Progress Saved");
                showTab(currentTab);
            } else {
                showtoast("error", "Something went wrong try again!");
            }
        });
    }
    else if (currentTab == 4) {
        saveTimeInfo(function (response) {
            if (response) {
                showtoast("info", "Progress Saved");    
                showTab(currentTab);
            } else {
                showtoast("error", "Something went wrong try again!");               
            }
        });
    }
    else if (currentTab >= $x.length) {
        //submit data to database
        submitForm();
    }
    else {
        showTab(currentTab);
    }
}
//Function to store certificate
function saveCertification(callback) {
    let certificateName = $("#certificatename").val();
    let subjectName = $("#subjects").val();
    let description = $("#certificatedescription").val();
    let issuedBy = $("#issuedby").val();
    let startYear = $("#startyear").val();
    let endYear = $("#endyear").val();  
    if (endYear == "present") {
        endYear = new Date().getFullYear();
    }
    let yearsOfStudy = startYear + "-" + endYear;
    let certificateUrl = $("#certificateUrl").val();
    let userId = $("#hdnTutorId").val();
    let certificationData = {
        certificateName: certificateName,
        subject: subjectName,
        description: description,
        issuedBy: issuedBy,
        yearsOfStudy: yearsOfStudy,
        certificateUrl: certificateUrl,
        appUserId: userId
    };
    $.ajax({
        type: "POST",
        url: "/Tutors/AddCertification",
        data: JSON.stringify(certificationData),
        contentType: "application/json",
        success: function (data) {
            callback(data);
        },
        error: function (error) {
            callback(error);
        }
    });
}
//function for save timing
function saveTimeInfo(callback) {
    let tutorId = $("#hdnTutorId").val();
    let timeconfig = [];

    $('.ix-day-checkbox:checked').each(function () {
        let component = $(this).data("component");
        let rows = $(".ix-" + component).find(".row");
        let d = $(this).val();
        let daySlots = [];

        rows.each(function (i, v) {
            let s = $(v).find(".start-time").val();
            let e = $(v).find(".end-time").val();

            daySlots.push({
                FromTimeId: s,
                ToTimeId: e
            });
        });

        timeconfig.push({
            dayId: d,
            timeSlots: daySlots
        });
    });
    $.ajax({
        type: "POST",
        url: "/Tutors/AddTimeConfig",
        data: JSON.stringify({ tutorId: tutorId, timeconfigs: timeconfig }),
        contentType: "application/json",
        success: function (data) {
            callback(data);
        },
        error: function (error) {
            callback(error);
        }
    });
}

function fixStepIndicator(n) {
    $(".step").removeClass("is-active");
    $(".step").eq(n).addClass("is-active");
}
function submitForm() {
    var form = $('#ix-multipageform');
    var formData = new FormData(form[0]);
    $.ajax({
        url: '/Tutors/AddDetails',
        type: 'POST',
        data: formData,
        processData: false,
        contentType: false,
        success: function (response) {
            if (response) {
                showtoast("success", "Your account will be activated soon...");
                $("#ix-activationModal").modal("hide");
                $(".ix-account-action").remove();
            }
            else {
                $("#ix-activationModal").modal("hide");
                showtoast("error", "Something went wrong try again later");
            }
        }, error: function (error) {
            console.log(error);
        }
    });
}
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
function validateForm() {
    var x, y, i, valid = true;
    x = $(".tab");
    y = x.eq(currentTab).find(".input-validation");

    y.each(function () {
        if ($(this).val() === "") {
            $(this).addClass("is-invalid");
            $(".img-error").css("visibility", "visible");
            $(".certificate-error").css("visibility", "visible");
            valid = false;
        }
        else {
            $(this).removeClass("is-invalid");
            $(".img-error").css("visibility", "hidden");
            $(".certificate-error").css("visibility", "hidden");
        }
    });

    if (valid) {
        $(".step").eq(currentTab).addClass("finish");
    }
    return valid;
}

//Scripts for availablitity tab page
let time_array = [];

$(document).ready(function () {
    let defaultStartTime = 14;
    let defaultEndTime = 15;
    $.ajax({
        url: '/Tutors/GetAllTiming',
        type: 'GET',
        success: function (result) {
            if (result.length!=0) {
                $.each(result, function (i, v) {
                    time_array.push(v);
                    let startOption = $("<option value=" + v.timingId + ">" + v.timing + "</option>");
                    startOption.attr("selected", v.timingId === defaultStartTime);
                    $(".start-time").append(startOption);
                    if (v.timingId > defaultEndTime) {
                        let endOption = $("<option value=" + v.timingId + ">" + v.timing + "</option>");
                        endOption.attr("selected", v.timingId === defaultEndTime);
                        $(".end-time").append(endOption);
                    }
                });
            }
            else {
                console.log("Something went wrong!")
            }
        }, error: function (error) {
            console.log(error);
        }
    });
    $.ajax({
        url: '/Tutors/GetAllDays',
        type: 'GET',
        success: function (result) {
            if (result.length != 0) {
                $.each(result, function (i, v) {
                    let domEle = '<input type="checkbox" class="form-check-input ix-day-checkbox" id="' + v.dayName + 'Checkbox" checked data-component="' + v.dayName.toLowerCase() + '"value="' + v.dayId +'">';
                    domEle += '<label class="form-check-label" for="' + v.dayName + 'Checkbox">' + v.dayName + '</label>';
                    $(".day-checkbox").eq(i).html(domEle);
                });
            }
            else {
                console.log("Something went wrong!")
            }
        }, error: function (error) {
            console.log(error);
        }
    });
    $(document).on('click','.ix-day-checkbox', function () {

        let current_component = $(this).data("component");
        if ($(this).is(':checked')) {
            $(".ix-" + current_component).show();
        } else {
            $(".ix-" + current_component).hide();
        }
    });
    $(".add-moreslot").on("click", function () {
        let current_link = $(this).data("link");
        //Getting rows and count
        let x = $(".ix-" + current_link).find(".row")
        let c = x.length - 1;
        //Getting value of last end time
        let v = x.eq(c).find(".end-time").val();
        //Getting and adding newelement
        let new_element = $(adjuststartTime(htmlElement(), v));
        $(".ix-" + current_link + " .ix-slots").append(new_element);
    });
    $(document).on('change', '.start-time', function () {
        let currentElement = $(this).closest(".row").find('.end-time');
        let data = $(this).val();
        //Clear the current element options
        currentElement.empty();
        //Calling function to change End Time
        adjustEndTime(currentElement, data)
    });
    $(document).on('change', '.end-time', function () {
        let nextEle= $(this).closest(".row").next(".row");
        let v = $(this).val();
        let j = parseInt(v) + 1;
        nextEle.find(".start-time,.end-time").empty();//Clear the options
        for (let i = v; i < time_array.length-1; i++) {
            //adding options for endtime based on selected starttime
            nextEle.find(".start-time").append($("<option value=" + time_array[i].timingId + ">" + time_array[i].timing + "</option>"))
            if (j < time_array.length) {
                nextEle.find(".end-time").append($("<option value=" + time_array[j].timingId + ">" + time_array[j].timing + "</option>"));
                j++;
            }
        }
    });
    $('#ix-activationModal').on('hidden.bs.modal', function () {
        $("#ix-multipageform")[0].reset();
        $('.file-input').val('');
        $('.selected-files').hide();
        $('#videoContainer').html('<video src="" controls class="Video-src"></video>');
    });
});
//function for create dropdownelement
function htmlElement() {
    let html = "";
    html += '<div class="row">';
    html += ' <div class="form-group select-wrap col-6">';
    html += ' <select class="form-select form-control time-select tutor-select start-time"></select>';
    html += ' </div>';
    html += ' <div class="form-group select-wrap col-6">';
    html += ' <select class="form-select form-control time-select tutor-select end-time"></select>';
    html += ' </div>';
    html += '</div>';
    return html;
}
function adjuststartTime(e, value) {
    element = $(e)
    if (value == time_array.length || !value) {
        element.find("option").remove();
    }
    else {
        let j = parseInt(value) + 1;
        element.find(".start-time, .end-time").empty(); // Clear existing options
        //Adding new optionts
        for (let i = value; i < time_array.length - 1; i++) {
            element.find(".start-time").append($("<option value=" + time_array[i].timingId + ">" + time_array[i].timing + "</option>"));
            if (j < time_array.length) {
                element.find(".end-time").append($("<option value=" + time_array[j].timingId + ">" + time_array[j].timing + "</option>"));
                j++;
            }
        }
    }
    return element;
}
function adjustEndTime(element, value) {
    for (let i = value; i < time_array.length; i++) {
        //adding options for endtime based on selected starttime
        element.append($("<option value=" + time_array[i].timingId + ">" + time_array[i].timing + "</option>"))
    }
}
//Profile photo and video upload
$(document).on('change', "#images", function () {
    const fileInput = this.files[0];
    if (fileInput) {
        $("#nextBtn").prop("disabled", true);
        $(".upload-progress").show();
        /* uploadToBlob(fileInput);*/
        uploadToBlob(fileInput, function (response) {
            if (response) {
                console.log(response)
                $("#profileUrl").val(response)
                showtoast("success", "Profile uploaded");
            } else {
                showtoast("error", "Something went wrong, try again!");
            }
            $("#nextBtn").prop("disabled", false);
            $(".upload-progress").hide();
        });
    }
});
$(document).on('change', "#certificate", function () {
    const fileInput = this.files[0];
    if (fileInput) {
        uploadToBlob(fileInput, function (response) {
            if (response) {
                console.log(response)
                $("#certificateUrl").val(response)
                showtoast("success", "Profile uploaded");
            } else {
                showtoast("error", "Something went wrong, try again!");
            }
        });
    }
});

function uploadToBlob(file,callback) {
    // Create FormData object
    var formData = new FormData();
    formData.append('file', file);

    $.ajax({
        type: 'POST',
        url: '/Tutors/Uploadfiles',
        data: formData,
        contentType: false, // Set to false when using FormData
        processData: false,
        success: function (response) {          
            callback(response);          
            //console.log('File uploaded successfully.');
        },
        error: function (error) {
            console.error('File upload failed.', error);
            callback(error);     
        }
    });
}











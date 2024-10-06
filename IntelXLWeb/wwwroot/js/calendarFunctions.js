$(document).ready(function () {
    $('#date').datepicker({
        format: 'dd-mm-yyyy',
        startDate: '+1d',
        minDate: 1
    });
    $.ajax({
        url: '/Tutors/GetAllTiming',
        type: 'GET',
        success: function (result) {
            if (result.length != 0) {
                $.each(result, function (i, v) {
                    time_array.push(v);                    
                });             
            }
            else {
                console.log("Something went wrong!")
            }
        }, error: function (error) {
            console.log(error);
        }
    });

    $("#date").on('change', function () {
        $('.time-select').empty();
        $('.time-select').append($('<option selected disabled>Select here</option>'));
        var dateObject = new Date($(this).val());
        // Get the day of the week (0 = Sunday, 1 = Monday, ..., 6 = Saturday)
        var dayOfWeek = dateObject.getDay();   
        let startIndex;
        let endIndex; 
        
        $.each(events, function (i, v) {
            if (v.daysOfWeek == dayOfWeek) {
                $.each(time_array, function (index, value) {
                    if (value.timing == v.startTime) {
                        startIndex = index;
                        timeConfigId = v.id;
                    }
                    else if (value.timing == v.endTime) {
                        endIndex = index;
                    }                  
                });
                //appending option availavle on that selected day
                for (var i = startIndex; i <= endIndex; i++) {
                    $('.time-select').append($('<option>', {
                        value: time_array[i].timingId,
                        text: time_array[i].timing
                    }));
                }
            }            
        });
        if (startIndex === undefined || endIndex === undefined) {
            alert("Tutor not available on this day.");
        }
    });

    $(".confirm-time button").on('click', function () {
        var requiredTime = $("input[name='required-time']:checked").val();
        if (!$(".date-select").val() || !$(".time-select").val()) {
            alert("select date and time");
        }
        else {
            var timeId = $(".time-select").val();
            var date = $(".date-select").val();
            var timingObject = $.grep(time_array, function (e) { return e.timingId == timeId; });           
            var combinedDateTimeString = date + " " + timingObject[0].timing;            
            // Create a JavaScript Date object using the combined string
            var scheduledDate = new Date(combinedDateTimeString);
            // Format the date in the desired format (yyyy-MM-dd HH:mm)
            var formattedScheduledDate = scheduledDate.toISOString().slice(0, 16).replace("T", " ");
            //Creating object to store 
            var studentTutorScheduleObject = {
                tutorTimingConfigId: timeConfigId,
                fromTimeId: parseInt(timeId),
                toTimeId: (requiredTime === '20') ? parseInt(timeId) + 1 : parseInt(timeId) + 2,
                scheduledDate: formattedScheduledDate,
            };
            console.log(studentTutorScheduleObject);
            var scheduleObjectString = encodeURIComponent(JSON.stringify(studentTutorScheduleObject));
            window.location.href = "/Tutors/CreateSchedule/" + tutorId + '?schedulestr=' + scheduleObjectString;
        }       
    });
}); 
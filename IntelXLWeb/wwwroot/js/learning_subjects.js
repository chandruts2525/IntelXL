//Getting data

$.ajax({
    url: "/Learning/GetSubTopicsByClass",
    type: "Get",
    data: { courseId: course_id, classId: class_id, subName: subject_name},
    success: function (result) {
        $('main').html(result)
    },
    error: function (xhr, status, error) {
        console.error("Error: " + error);
    }
});
$(document).on('click', '.subject-navigator', function (event) {  
    event.preventDefault();
    //Data to pass to the controller.
    var courseId = 1;
    var sub_name = $(this).data("name");  
    // Create the URL with query parameters.
    var url = '/Learning/SubTopicsByClass?courseId=' + course_id + '&classId=' + class_id + '&subName=' + sub_name;

    // Navigate to the new URL.
    window.location.href = url;
});
$(document).on('change', '.previous-year', function (event) {      
    let year = $(this).val().trim()
    $("#previous-practice-modal").modal("show");
    $('#previous-practice-modal').on('click', '#practice-ques', function () {       
        window.location.href = '/Learning/PracticePreviousQuestions?subjectId=' + sub_id + '&year=' + year;     
    });
});
$(document).on('change', '.probable-year', function (event) {      
    let year = $(this).val().trim()
    $("#probable-practice-modal").modal("show");    
    $('#probable-practice-modal').on('click', '#probable-ques', function () {
        window.location.href = '/Learning/PracticeProbableQuestions?subjectId=' + sub_id + '&year=' + year;     
    });
});

$(document).on('hidden.bs.modal', '#previous-practice-modal, #probable-practice-modal', function () {
    $('.previous-year').prop("selectedIndex", 0);
    $('.probable-year').prop("selectedIndex", 0);
});

$(document).ready(function () {
    getClasses(course_id)
    selectedId = course_id;
});

var selectedId = 1;
//Getting class based on subscription course 
function getClasses(c_id) {
    $('.home-animation').show();
    $(".class-wrap").html("");
    let html = '';

    //Array of claess to apply border, text color   
    let styles = [
        { class: "lkgcard", color: "#10a0b6" }, { class: "Ukgcard", color: "#f39317" }, { class: "ClassI", color: "#0b863c" }, { class: "ClassII", color: "#d74726" }, { class: "ClassIII", color: "#00a1de" },
        { class: "ClassIV", color: "#8d33aa" }, { class: "ClassV", color: "#00b971" }, { class: "ClassVI", color: "#e9681b" }, { class: "ClassVII", color: "#0b863c" }, { class: "lkgcard", color: "#10a0b6" }, { class: "Ukgcard", color: "#f39317" }, { class: "ClassI", color: "#0b863c" }, { class: "ClassII", color: "#d74726" }, { class: "ClassIII", color: "#00a1de" },
        { class: "ClassIV", color: "#8d33aa" }, { class: "ClassV", color: "#00b971" }, { class: "ClassVI", color: "#e9681b" }, { class: "ClassVII", color: "#0b863c" }];
    $.ajax({
        type: "GET",
        url: "/Learning/GetClasses",
        data: { id: c_id },
        success: function (response) {
            $(".course-btn-container, .ix_languageSelection").css("visibility", "visible");
            if (response && response.length > 0) {
                $.each(response, function (index, value) {
                    let questionCount = Math.round((value.totalQuestionsCount) / 100) * 100;
                    let countDiv = "";
                    if (questionCount != 0) {
                        countDiv = '</span><span style="float: right; color: #52b700;">' + questionCount * multiplyBy + '+ Questions</span>';
                    }
                    else {
                        countDiv = '</span><span style="float: right; color: #52b700;">No Questions</span>';
                    }
                    //Setting data in the card
                    html += '<div class="col-lg-4 col-sm-6 mt-3">';
                    html += '<div class="card">';
                    html += '<div class="card-body m-2 ' + styles[index].class + '">';
                    html += '<a class="d-flex justify-content-between align-items-center grade" data-classname="' + value.className + '" href="#">';
                    html += '<input type="hidden" class="hdnclass" value=' + value.classId + '>';
                    html += '<span class="h5 card-title" style="color:' + styles[index].color + ';font-size:25px;">' + value.className + '</span>' + countDiv + '';
                    html += '<div class="rightarrow" style="color:' + styles[index].color + ';"><i class="fa-solid fa-chevron-right fa-lg"></i></div>';
                    html += '</a>';
                    html += '<div class="class-details">';
                    html += '<p class="card-text"  style="font-size:12px;"> ' + value.description + '</p>';
                    html += '<hr/>';

                    $.each(value.subjectMasters, function (i, v) {
                        html += '<div class="d-flex justify-content-between pb-2">';
                        html += '<div class="subName"style="font-size:12px;">' + v.subjectName + ' </div>';
                        html += '<div class="text-end"><a class="skill-link" href="#" data-name="' + v.subjectName + '" style="">' + v.subTopicCount + ' skills <i class="fa-solid fa-angles-right fa-sm mt-2"></i></a> </div>';
                        html += '</div>';
                    });
                    html += '</div>';
                    html += '</div>';
                    html += '</div>';
                    html += '</div>';
                });
            }
            else {
                html += '<div class="justify-content-center d-flex" style="color:#b7b7b7;"><h4 class="mb-3 mx-2 ">No data available</h4></div>';
            }
            $('.home-animation').hide();
            $(".class-wrap").html(html);
        }
    });
}
$(document).on('click', '.courseSelection', function (event) {
    selectedId = $(this).find('input').val()
    $(".courseSelection").find('div').removeClass("course-active")
    $(this).find('div').addClass("course-active")
    getClasses(selectedId)
});

$(document).on('click', '.grade', function (event) {
    event.preventDefault();
    var sub_name = $(this).closest(".card").find(".subName:first").text();
    var class_id = $(this).find('.hdnclass').val();
    window.location.href = "/Learning/SubTopicsByClass?courseId=" + selectedId + '&classId=' + class_id + '&subName=' + sub_name;
});
$(document).on('click', '.skill-link', function (event) {
    event.preventDefault();
    var sub_name = $(this).data('name');
    var class_id = $(this).closest(".card").find('.hdnclass').val();
    window.location.href = "/Learning/SubTopicsByClass?courseId=" + selectedId + '&classId=' + class_id + '&subName=' + sub_name;
});
$(document).on('change', '.ix_language', function () {
    let selectdValue = $(this).val();
    $.ajax({
        type: "GET",
        url: "/Home/GetCourses",
        data: { id: selectdValue },
        success: function (response) {
            if (response.length > 0) {
                let currentCourseId = response[0].courseId;
                selectedId = currentCourseId;
                getClasses(currentCourseId)
                let html = '<div class="d-flex course-div align-items-center">';
                $.each(response, function (index, value) {
                    let active = (index === 0) ? "course-active" : "";
                    html += ' <div class="btn-inner text-center py-2 my-1 mx-2">';
                    html += ' <a href="#" class="courseSelection">';
                    html += ' <input type="hidden" id="hdnCourseId" value="' + value.courseId + '" />';
                    html += '  <div type="button" class="btn course-btn text-nowrap ' + active + ' px-4">' + value.courseName + '</div>';
                    html += ' </a>';
                    html += '  </div>';
                })
                html += '  </div>';
                $(".course-bg-div").html(html);
            }
            else {
                $(".course-btn-container .course-bg-div").html('<div class="justify-content-center d-flex" style="color:#b7b7b7;"><h4 class="my-3 mx-2 ">No data available for this language</h4></div>');
                $(".class-wrap").html('<div class="justify-content-center d-flex" style="color:#b7b7b7;"><h4 class="mb-3 mx-2 "></h4></div>');
            }

        },
        error: function () {
            console.error('Error fetching courses:', error);
        }
    });
});
var baseUrl = window.location.protocol + "//" + window.location.host + window.location.pathname;
history.pushState(null, null, baseUrl);
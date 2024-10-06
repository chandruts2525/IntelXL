var option = "";
$(document).ready(function () {
    $.ajax({
        type: "GET",
        url: "/SubTopics/GetById",
        data: { id: subTitleId },
        success: function (data) {
            updateTopic(data[0].topicId)
            $.each(data, function (i, v) {
                var $option = $('<option>', {
                    value: v.subTopicId,
                    text: v.subTopic
                });
                if (v.subTopicId == subTitleId) {
                    $option.attr("selected", "selected");
                }
                $('#subtopic').append($option);
            });
        },
        error: function (xhr, status, error) {
            console.log("Error: " + error);
        }
    });
    updateBreadcrumb()
    $('.square-radio').click(function () {
        questionType = $(this).val();
        if (questionType == 1) {
            $(".note-pad").show();
            $(".note-editor").hide();
        }
        else {
            $(".note-pad").hide();
            $(".note-editor").show();
        }
    });
    $('.question-cancel-btn').click(function () {
        window.scrollTo(0, 0);
        setTimeout(function () {
            location.reload();
        }, 500);

    });

    //Choice create button
    $('.option-create-btn').click(function () {
        $(".Question-warning").hide();
        $(".option-warning").hide();
        $(".answer-warning").hide();
        if (optionsArray.length >= 5) {
            $(".option-warning").text("Maximum number of choices is 5");
            $(".option-warning").show();
            return false;
        }
        if (questionType == 1) {
            option = $('.ans-text-type-1').val().trim();
        }
        else {
            option = $('.ans-text').val().trim();
        }
        //const option = $('.optionTxtArea').val().trim();

        if (!option) {
            $(".option-warning").text("Choice couldn't be empty");
            $(".option-warning").show();
            return false;
        }

        if (optionsArray.includes(option)) {
            $(".option-warning").text("Choices couldn't be same");
            $(".option-warning").show();
            return false;
        }

        const iscorrectAns = $('.isCorrectAns').is(':checked');

        if (iscorrectAns) {
            if (answer) {
                $(".option-warning").text("Multiple choices couldn't be the answer");
                $(".option-warning").show();
                return false;
            }
            answer = option;
        }

        optionsArray.push(option);

        createOptionElement(option, iscorrectAns);

        //Choice input to default state
        $('.optionTxtArea,.ans-text-type-1').val('');
        $('.isCorrectAns').prop('checked', false);
    })

    //Remove choice
    $(document).on('click', '.optionRemove', function () {
        const optionEle = $(this).closest('.row');

        if (optionEle.find('input').is(':checked')) {
            answer = '';
        }
        optionsArray.pop(optionEle.find('.optionText').text());

        optionEle.remove();
    })
   
    //Question submit to add
        $('.question-add-btn').click(function () {
            $("#subtopic,.year-input").removeClass("is-invalid");
            if (!subTitleId) {
                $("#subtopic").addClass("is-invalid");
            showtoast("error", "The subtopic couldn't be empty");
            return false;
        }
        let questionTxt = "";
        $(".Question-warning").hide();
        $(".option-warning").hide();
        $(".answer-warning").hide();

        if (questionType == 1) {
            questionTxt = $('.qus-text-type-1').val().trim();
            answerDetail = $('.ans-detail-type-1').val().trim();
        }
        else {
            questionTxt = $('.qus-text').val().trim();
            answerDetail = $('.ans_detail_txt').val().trim();
        }

        //const questionTxt = $('.qus-text').val().trim();

        if (!questionTxt) {
            window.scrollTo(0, 0);
            $(".Question-warning").text("Question Text couldn't be empty");
            $(".Question-warning").show();
            return false;
        }

        if (optionsArray.length < 2) {
            window.scrollTo(0, 0);
            $(".option-warning").text("Number of choices must be 2");
            $(".option-warning").show();
            return false;
        }

        if (!answer) {
            window.scrollTo(0, 0);
            $(".option-warning").text("Select one of the choices as answer");
            $(".option-warning").show();
            return false;
        }

        //answerDetail = $('.ans_detail_txt').val().trim();

        if (!answerDetail) {
            $(".answer-warning").text("Answer detail couldn't be empty");
            $(".answer-warning").show();
            return false;
            }  
            var isPrevious = $('.previous-question').is(':checked');
            var previousYear;
            var previousYearString = "";
            if (isPrevious) {
                previousYear = $('.previous-year').val();
                previousYearString = previousYear.join(',');
            }
            if (isPrevious && previousYearString == '') {
                $(".previous-year").closest(".multiselect-native-select").find(".multiselect").addClass("is-invalid");
                return false;
            }
            var isProbable = $('.probable-question').is(':checked');
            var probableYear;
            var probableYearString = "";
            if (isProbable) {
                probableYear = $('.probable-year').val();
                probableYearString = probableYear.join(',');
            }
            if (isProbable && previousYearString == '') {
                $(".probable-year").closest(".multiselect-native-select").find(".multiselect").addClass("is-invalid");
                return false;
            }
            $(this).hide();
            $("#adding").show();
        let question = {
            subTopicId: subTitleId,
            question: questionTxt,
            questionId: quesId,
            answerId: ansId,
            questionType: questionType,
            isPreviousYearQuestion: isPrevious,
            previousYear: previousYearString,
            isProbable: isProbable,
            probableYear: probableYearString,
            choiceMasters: optionsArray.map(function (value, index) {
                if (index < choices.length) {
                    return {
                        choice: value,
                      /*  choiceId: choices[index].ChoiceId,*/
                        QuestionId: choices[index].QuestionId
                    };
                } else {                    
                    return {
                        choice: value,
                        choiceId: 0,
                        QuestionId: quesId
                    };
                }
            }),            
            Answer: {
                AnswerId: ansId,
                Answer: answer,
                Description: answerDetail
            }            
            }
        $.ajax({
            type: "PUT",
            url: "/Question/EditQuestion",
            data: JSON.stringify(question),
            contentType: "application/json",
            success: function (data) {
                if (data) {
                    showtoast("success", "Question saved Successfully");
                    setTimeout(function () {
                        window.location.href = "/Question/AllQuestions?id=" + subTitleId;
                    }, 5000);
                }

                else {
                    showtoast("error", "Something went wrong. Please try again");
                }
                // $('html, body').scrollTop(0);
                $(".question-add-btn").show();
                $("#adding").hide();
            },
            error: function (error) {
                showtoast("error", "Something went wrong. Please try again");
                $('html, body').scrollTop(0);
                $(".question-add-btn").show();
                $("#adding").hide();
            }
        });   
        })
    $('.previous-question').click(function () {
        if ($('.previous-question').is(':checked')) {
            $('.year-div').show();
        }
        else {
            $('.year-div').hide();
        }
    });
    $('.probable-question').click(function () {
        if ($('.probable-question').is(':checked')) {
            $('.probable-year-div').show();
        }
        else {
            $('.probable-year-div').hide();
        }
    });
})
$(document).on('change', '#subtopic', function () {
    
    let newId = parseInt($(this).val());
    subTitleId = newId;
    //let newText = $(this).find("option:selected").text()
    //$(".subtopic-bread-crumb").html(newText);
    //localStorage.setItem('Subtopic', newText);
    /* var currentUrl = window.location.href;
     var updatedUrl = currentUrl.replace(/(\?|&)id=([^&]*)/, '$1id=' + newId);
     history.pushState({ path: updatedUrl }, '', updatedUrl);*/
});
$(document).on('change', '#topic', function () {
    let topicid = parseInt($(this).val());
    $('#subtopic').empty().removeClass("is-invalid");
    $.ajax({
        type: "GET",
        url: "/SubTopics/GetAllById",
        data: { id: topicid },
        success: function (data) {
           
            if (data.length != 0) {
                $.each(data, function (i, v) {
                    if (v.status) {
                        var $option = $('<option>', {
                            value: v.subTopicId,
                            text: v.subTopic
                        });
                        if (v.subTopicId == subTitleId) {
                            $option.attr("selected", "selected");
                        }
                        $('#subtopic').append($option);
                    }
                });
            }
            else {
                $('#subtopic').append(`<option selected disabled>Select here...</option>`);
            }
            $('#subtopic').trigger('change');
        },
        error: function (xhr, status, error) {
            console.log("Error: " + error);
        }
    });
});
function createOptionElement(option, iscorrectAns) {

    const color = (iscorrectAns) ? 'green' : 'red';

    let optionDiv = '<div class="row mx-0">';
    optionDiv += '<div class="col-1 pe-0 text-md-center py-1">';
    optionDiv += '<div style="width: 15px; height: 15px; background-color: ' + color + '; border-radius: 50%;"></div>';
    optionDiv += '</div>';
    optionDiv += '<div class="col flex-grow-1 optionText">' + option + '</div>';
    optionDiv += '<div class="col-1 w-10 text-danger text-end"><i class="fa-solid fa-xmark optionRemove"></i></div>';
    optionDiv += '<input type="checkbox" style="display: none" ' + (iscorrectAns ? 'checked' : '') + '/>';
    optionDiv += '<hr class="my-1" style="color: ' + color + '"/>';
    optionDiv += '</div>';
    $('.options').append(optionDiv);
    $('.answer .note-editable').empty();
}
function updateTopic(id) {
    $.ajax({
        type: "GET",
        url: "/Topics/GetById",
        data: { id: id },
        success: function (data) {
            $.each(data, function (i, v) {
                if (v.status) {
                    var $option = $('<option>', {
                        value: v.topicId,
                        text: v.topic
                    });
                    if (v.topicId == id) {
                        $option.attr("selected", "selected");
                    }
                    $('#topic').append($option);
                }
            });
        },
        error: function (xhr, status, error) {
            console.log("Error: " + error);
        }
    });
}
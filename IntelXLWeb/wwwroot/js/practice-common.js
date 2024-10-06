$(document).ready(function () {   
    //Practice question submit
    $(".prac_submit").click(function () {
        questionCount()
        if ($('input[name="choice_check"]').is(':checked')) {            
            const selectedAnswer = $('input[name="choice_check"]:checked').siblings('label').text();
            //Verify answer 
            if (selectedAnswer == answer) {
                answeredStatus = 1;               
                SaveExamStatus();                
                //score += scoreIncrement;
                //correctAnsCount++;

                if (ansCount >= count) {
                    //show model here
                    $("#indicationModel").modal("show");
                    //window.location.href = `/Learning/Certificate?score=${Math.ceil(score)}&time=${$('.time-status b').text()}&ans=${correctAnsCount}&subTopicId=${subTopicId}&noOfQuestions=${questionsLength}`;
                }
                else {                   
                    if (questionsLength > questionIndex)                        
                        showQuestion(questions[questionIndex]);                    
                }
                //uncheck the option 
                $('input[name=choice_check]').prop('checked', false);
            }
            else {
                answeredStatus = 3;               
                SaveExamStatus();
                wrongAnswer();
                //$("#prac-modal-submit").trigger("click");
            }
        }
        else {
            answeredStatus = 2;            
            SaveExamStatus();
            $("#practiceModal").modal("show");
        }        
    });
    $("#prac-modal-submit").click(function () {  
        wrongAnswer();
        questionCount()
        $("#practiceModal").modal("hide");
        //complete practicing
        //if (ansCount >= questionsLength) {
        //    window.location.href = `/Learning/Certificate?score=${Math.ceil(score)}&time=${$('.time-status b').text()}&ans=${correctAnsCount}&subTopicId=${subTopicId}&noOfQuestions=${questionsLength}`;
        //}

        ////show next question
        //if (questionsLength > questionIndex)
        //    showQuestion(questions[questionIndex]);

        //$('.prac_ans_container').hide();
        //$('.prac_gotit').hide();
        //$('.prac_submit').fadeIn();

        ////uncheck the option 
        //$('input[name=choice_check]').prop('checked', false);
    });
    //After answer detail ( Got it button click)
    $(".prac_gotit").click(function () {
        
        //complete practicing
        if (ansCount >= count) {
            $("#indicationModel").modal("show");
            //window.location.href = `/Learning/Certificate?score=${Math.ceil(score)}&time=${$('.time-status b').text()}&ans=${correctAnsCount}&subTopicId=${subTopicId}&noOfQuestions=${questionsLength}`;
        }

        //show next question
        if (questionsLength > questionIndex)
            showQuestion(questions[questionIndex]);

        $('.prac_ans_container').hide();
        $('.prac_gotit').hide();
        $('.prac_submit').fadeIn();

        //uncheck the option 
        $('input[name=choice_check]').prop('checked', false);
    });
    $(document).on('click', '.practice-again', function (e) {
        e.preventDefault();
        window.location.href = "/Learning/PracticeAgainByYear?userId=" + userId + "&subjectId=" + subjectId + "&year=" + year + "&isPrevious=" + isPrevious
    });
    $(document).on('click', '.practice-wrong', function (e) {
        e.preventDefault();
        window.location.reload();
    });
});


// Initialization
let questions = [];
let questionsLength = 0;
let questionIndex = 0;
let answer = "";
let ansCount = 0;
//let correctAnsCount = 0;
//let score = 0;
let maxFreeCount = 0;
let scoreIncrement = 0;
let practiceQuesLength = 0;
let isSubscribed = false;
var questionId = 0;
var practiceType = "previous";
var count = 0;

//show question to practice
function showQuestion(question) {
    $(".prac_ques_container").hide();
    subTopicId = question.subTopicId
    questionId = question.questionId;
    questionIndex++;
    //Assign question   
    $('.prac_ques').html(question.question);

    let html = "";
    question.choiceMasters.forEach((choice, i) => {
        //let cl = '.prac_choice' + (i + 1);
        //$('.prac_choice' + (i + 1)).html(choice.choice)
        html += `<div class="form-check my-3 py-2 ${choice.choiceId}">
                        <input id="qus-choice${i + 1}" type="radio" name="choice_check" class="form-check-input" value="" />
                        <label for="qus-choice${i + 1}" class="form-check-label prac_choice${i + 1}">${choice.choice}</label>
                    </div>`
    });
    $('.prac_choices').html(html)
    //Assign answer
    answer = $('<div/>').html(question.answer.answer).text();
    console.log('answer : ' + answer)

    $('.prac_ans').html(answer);
    $('.prac_ans_detail').html(question.answer.description);

    $(".prac_ques_container").fadeIn();

    if (questionIndex == 1) {
        $(".prac_loading").remove();
        startTimer();
    }
}

function startTimer() {
    const timeStatus = $('.time-status b');
    let hours = 0, minutes = 0, seconds = 0;

    const updateTimer = () => {
        timeStatus.text(`${String(hours).padStart(2, '0')} : ${String(minutes).padStart(2, '0')} : ${String(seconds).padStart(2, '0')}`);
    };

    timerInterval = setInterval(() => {
        seconds++;
        if (seconds === 60) {
            seconds = 0;
            minutes++;
        }
        if (minutes === 60) {
            minutes = 0;
            hours++;
        }
        updateTimer();
    }, 1000);
}

// Function to stop and reset the timer
function resetTimer() {
    clearInterval(timerInterval); // Stop the current timer interval
    $('.time-status b').text('00 : 00 : 00'); // Reset the timer display
}

function startCountdown() {
    let count = 2;
    let countdownInterval = setInterval(function () {
        if (count) {
            $(".last-count b").text(count);
            count--;
        }
        else {
            clearInterval(countdownInterval);
            $(".last3-qus").hide();
            //show next question
            if (questionsLength > questionIndex)
                showQuestion(questions[questionIndex]);
        }
    }, 1000);
}

function wrongAnswer() {   
    $('.prac_submit').hide();
    $('.prac_ans_container, .prac_gotit').fadeIn();
}
function questionCount() {
    $(".completed_count").text(++ansCount);
}

function SaveExamStatus() {
    if (isPrevious != "True") {
        practiceType = "probable";
    }
    
    var values = {
        questionId: questionId,
        answeredStatus: answeredStatus,
        subtopicId: subTopicId,
        subjectId: subjectId,
        practiceType: practiceType,
        yearOfQuestion: year
    };    
    $.ajax({
        type: "POST",
        url: "/Learning/UserExamEntry",
        data: { userExam: values },
        Success: function (response) {
            if (response) {

            }
            else {
                console.log("Error in create entry response");
            }
        },
        error: function (error) {
            console.log("Error in create entry" + error);
        }
    });
}


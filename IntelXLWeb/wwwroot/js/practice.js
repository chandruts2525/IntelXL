$(document).ready(function () {
    //Practice question submit
    $(".prac_submit").click(function () {
        //var currentLimit = parseInt(getCookie('dailylimit')) || 0;
        //if (currentLimit <= maxFreeCount && !isSubscribed) {
        //    // Increment the value
        //    currentLimit++;
        //    var expirationDate = new Date();
        //    // Set the expiration time to tomorrow morning
        //    //expirationDate.setTime(expirationDate.getTime() + 10 * 60 * 1000); // 10 minutes in milliseconds
        //    expirationDate.setHours(24, 0, 0, 0);
        //    // Set the updated value back to the cookie
        //    document.cookie = 'dailylimit=' + currentLimit + '; expires=' + expirationDate.toUTCString() + '; path=/';
        //}
        //checking is answer selected
        if ($('input[name="choice_check"]').is(':checked')) {

            questionCount()

            const selectedAnswer = $('input[name="choice_check"]:checked').siblings('label').text();

            //Verify answer 
            if (selectedAnswer == answer) {
                answeredStatus = 1;
                $(".prac_ques_container").hide()

                //getting random appriciation message
                let message = getMessage();
                $(".msg-success b").text(`${message}!`);

                score += scoreIncrement;
                correctAnsCount++;
                $(".smart-score-value b").text(Math.ceil(score));

                //Entering into chalenge
                if (ansCount == practiceQuesLength) {
                    $(".last3-qus").show();
                    startCountdown();
                }
                else {
                    $(".success-msg").fadeIn(500);
                    setTimeout(function () {
                        //complete practicing
                        if (ansCount >= questionsLength) {
                            if (isSubscribed) {
                                window.location.href = `/Learning/Certificate?score=${Math.ceil(score)}&time=${$('.time-status b').text()}&ans=${correctAnsCount}&subTopicId=${subTopicId}&noOfQuestions=${questionsLength}&classId=${classId}`;
                            }
                            else {
                                $('.cancel-modal').html(`<a class="backToTopics" href="#">Back to topics</a>`);
                                $(".cross").remove();
                                $("#subscribeAlertModel").modal("show");
                            }
                        }
                        else {
                            $(".success-msg").fadeOut(function () {
                                //show next question
                                if (questionsLength > questionIndex)
                                    showQuestion(questions[questionIndex]);
                            });
                        }
                    }, 500);
                }
                //uncheck the option 
                $('input[name=choice_check]').prop('checked', false);
            }
            else {
                answeredStatus = 3;
                wrongAnswer();
            }

        }
        else {
            answeredStatus = 2;
            $("#practiceModal").modal("show");
        }
        if (isSubscribed) {
            SaveExamStatus();
        }
    });
    // 
    $("#prac-modal-submit").click(function () {
        wrongAnswer();
        questionCount()
        $("#practiceModal").modal("hide");
    });


    //After answer detail ( Got it button click)
    $(".prac_gotit").click(function () {

        //complete practicing
        if (ansCount >= questionsLength) {
            if (isSubscribed) {
                window.location.href = `/Learning/Certificate?score=${Math.ceil(score)}&time=${$('.time-status b').text()}&ans=${correctAnsCount}&subTopicId=${subTopicId}&noOfQuestions=${questionsLength}&classId=${classId}`;
            }
            else {
                $('.cancel-modal').html(`<a class="backToTopics" href="#">Back to topics</a>`);
                $(".cross").remove();
                $("#subscribeAlertModel").modal("show");
            }
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
});
//$.ajax({
//    url: '/Learning/GetFreeCount',
//    type: 'GET',
//    success: function (response) {
//        maxFreeCount = response.count;
//    },
//    error: function (error) {
//        maxFreeCount = 5;
//        console.error('Error fetching count:', error);
//    }
//});

// Initialization
let questions = [];
let questionsLength = 0;
let questionIndex = 0;
let answer = "";
let ansCount = 0;
let correctAnsCount = 0;
let score = 0;
let maxFreeCount = 0;

let scoreIncrement = 0;
let practiceQuesLength = 0;
let isSubscribed = false;

// Ajax call to get practice questions
$.ajax({
    type: "GET",
    url: "/Learning/GetPracticeQuestions?subTopicId=" + subTopicId + "&classId=" + classId + "&userId=" + userId,
    success: function (data) {
        questionsLength = data?.questions?.length;
        isSubscribed = data?.hasSubscription;
        maxFreeCount = data.trialCount;
        if (questionsLength > 3) {
            questions = data.questions;
            showQuestion(questions[0]);

            scoreIncrement = 100 / questionsLength;
            practiceQuesLength = questionsLength - Math.floor(questionsLength / 3);
        }
        else {
            $(".prac_loading").remove();
            if (data.count != 0 && data.questions.length < data.count) {
                $(".prac_emtyqns").html(`<div class="text-secondary"><h2>You have practiced all Question! <a class="practice-again" href="" style="color:#0d6efd;text-decoration: underline;">Practice again</a></h2></div>`).show();
            }
            else {
                $(".prac_emtyqns").show();
            }
            console.log('No questions availble to practice.');
        }
        if (!isSubscribed) {
            $("#subscribeAlertModel").modal("show");
        }
    },
    error: function (error) {
        console.error('Error fetching questions:', error);
    }
});

//function getCookie(name) {
//    var match = document.cookie.match(new RegExp('(^| )' + name + '=([^;]+)'));
//    if (match) return match[2];
//}
//show question to practice
function showQuestion(question) {
    questionId = question.questionId;
    questionIndex++;
    //Assign question
    $('.prac_ques').html(question.question);
    //Assign choices
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
    //console.log('answer : ' + answer)

    $('.prac_ans').html(answer);
    $('.prac_ans_detail').html(question.answer.description);

    $(".prac_ques_container").fadeIn();

    if (questionIndex == 1) {
        $(".prac_loading").remove();
        startTimer();
    }

    //Restict without authorization
    if (!isSubscribed && ansCount >= maxFreeCount) {
        /*if (!isSubscribed && ((parseInt(getCookie('dailylimit'))) >= maxFreeCount)) {*/
        $(".practice-side").css("pointer-events", "none");
        /*  $("#LoginModal").modal("show");*/
        $(".membershiptoast").show();
        $(".removeablebtns").remove();
        /* $(".error-msg").text("To continue you must have subscription");*/
        return false;
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


function getMessage() {
    var messages = ["Super", "Excellent", "Outstanding", "Marvelous", "Fantastic", "Incredible",
        "Terrific", "Awesome", "Wonderful", "Brilliant", "Superior", "Impressive"
    ];
    var randomMessage = messages[Math.floor(Math.random() * messages.length)];
    return randomMessage;
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
    //if (score < 3)
    //    score = 0
    //else
    //    score -= 3;
    //$(".smart-score-value b").text(score);

    $('.prac_submit').hide();
    $('.prac_ans_container, .prac_gotit').fadeIn();
}
function questionCount() {
    //ansCount++;
    $(".qus-ans-count b").text(++ansCount);
}

function SaveExamStatus() {
    var values = {
        questionId: questionId,
        answeredStatus: answeredStatus,
        subtopicId: subTopicId,
        practiceType: "practice"
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

//Validating trial Expiration
$(document).ready(function () {
    //................Trial period verification..................
    //if (userId != 0) {
    //    $.ajax({
    //        type: "GET",
    //        url: "/User/GetuserDetail",
    //        data: { userId: userId },
    //        success: function (response) {
    //            var registeredDate = new Date(response.updatedDttm);
    //            var currentDateTime = new Date();
    //            var oneMonthLater = new Date(registeredDate);
    //            oneMonthLater.setMonth(oneMonthLater.getMonth() + 1);
    //            if (oneMonthLater < currentDateTime && classId == 0) {
    //                $(".practice-side").css("pointer-events", "none");  
    //                $(".membershiptoast p").text("Your trial has Expired");
    //                $(".membershiptoast").show();
    //                $(".removeablebtns").remove();                   
    //                return false;
    //            }             
    //        },
    //        error: function (error) {
    //            console.log("Error in getting user Detail" + error);
    //        }
    //    });
    //}   
    $(document).on('click', '.practice-again', function (e) {
        e.preventDefault();
        window.location.href = "/Learning/PracticeAgain?subTopicId=" + subTopicId + "&userId=" + userId + "&classId=" + classId;
    });

    $(document).on('click', '#subscribeBtn', function (e) {
        e.preventDefault();
        window.location.href = "/Membership/Index";
    });
    $(document).on('click', '.backToTopics', function (event) {
        event.preventDefault(); 
        window.history.back();
    });
});
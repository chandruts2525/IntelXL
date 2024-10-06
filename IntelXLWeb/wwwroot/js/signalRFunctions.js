var isArchived = false;
$(document).ready(function () {
    var connection = new signalR.HubConnectionBuilder().withUrl("/chatHub").build();
    connection.on("ReceiveMessage", function (chatModel) {
        console.log(chatModel);
        if ($(".chat-conversation").is(':visible') && $("#hdnData").val() == chatModel.fromId) {
            $("#chatBox").append($(receiveElement(chatModel.message, chatModel.fileName, chatModel.mediaUrl)));
            var chatBox = $("#chat-body");
            chatBox.scrollTop(chatBox[0].scrollHeight);
            $(".typing-indicator").hide();
        }
        else if ($(".chat-inbox").is(':visible')) {
            UpdateList();
        }
        else {
            console.log("No active conversation");
            $("#notification-badge").show();
        }
    });
    connection.on("TypingIndication", function (fromId, message) {
        if ($(".chat-conversation").is(':visible') && $("#hdnData").val() == fromId && message) {
            $(".typing-indicator").show();
        }
        else {
            $(".typing-indicator").hide();
        }
    });
    $("#message").on('input', function () {
        let m = $("#message").val().trim();
        var v = parseInt(selectedUserId);
        connection.invoke("TypingIndication", v, m)
            .then(() => {
                // Handle success if needed
            })
            .catch(err => console.error(err));
    });
    $("#sendMsg").click(function () {
        var message = $("#message").val();
        var toUserId = parseInt(selectedUserId);
        var convoId = $("#ConvoId").val();
        var fileName = $("#hdnfileName").val();
        var fileUrl = $("#fileUrl").val();
        if (message.trim() || fileUrl) {
            connection.invoke("SendToUser", toUserId, message, convoId, fileName, fileUrl)
                .then(() => {
                    $("#chatBox").append($(sendElement(message, fileName, fileUrl)));
                    var chatBox = $("#chat-body");
                    chatBox.scrollTop(chatBox[0].scrollHeight);
                    $(".uploaded-file").html("");
                    $("#fileUrl").val("");
                })
                .catch(err => console.error(err));
        }
        $("#message").val("").focus();
    });
    connection.start().then(function () {
        console.log("Connection established!");
    }).catch(function (err) {
        console.error(err.toString());
    });

    //Chat
    $(".chat-btn").on('click', function () {
        $("#chatModal").modal({
            backdrop: false
        }).modal("show");
        $('.wrapper').css('display', 'block');
        if ($(".chat-conversation").is(':visible')) {
            $('.button-back').css('display', 'block');
        } else {
            $('.button-back').css('display', 'none');
        }
        $.ajax({
            url: '/Chats/GetChatsList',
            type: 'GET',
            success: function (response) {
                $(".inbox_chat").html(response);
            },
            error: function (xhr, status, error) {
                console.error("Error: " + error);
            }
        });
    });
    $(".chat-close").on('click', function () {
        $('.chat-conversation').css('display', 'none');
        $('.chat-inbox').css('display', 'block');
        $('.chat-form').css('display', 'none');
        $("#message").val("");
        let idVal = 0;
        if (selectedUserId) {
            idVal = parseInt(selectedUserId);
        }
        connection.invoke("TypingIndication", idVal, "")
            .then(() => {
                // Handle success if needed
            })
            .catch(err => console.error(err));
    });
    getunreadCount();
});
$(document).ready(function () {
    $('#message').keypress(function (e) {
        if (e.which === 13) {
            e.preventDefault();
            $('#sendMsg').click();
        }
    });
});
//Chat window
$(document).on('click', '.chat_list', function () {
    $('.chat-conversation').css('display', 'block').html($(messageLoading()));
    $('.chat-inbox').css('display', 'none');
    $('.chat-form').css('display', 'block');
    if ($(".chat-conversation").is(':visible')) {

        $('.button-back').css('display', 'block');
    } else {
        $('.button-back').css('display', 'none');
    }
   /* if ($(this).hasClass("unread")) {*/
        let fromid = $(this).data("id");
        updateReadStatus(fromid);
    /*}*/
    $(".wrapper .chat-header h3").text($(this).data("name"));
    selectedUserId = $(this).data("id");
    $("#hdnData").val(selectedUserId);
    getMessages(selectedUserId)
});

$(document).on('click', '.button-back', function () {
    $('.chat-conversation').css('display', 'none');
    $('.chat-inbox').css('display', 'block');
    $('.chat-form').css('display', 'none');
    $('.chat-conversation').html($(messageLoading()));
    $(".wrapper .chat-header h3").text("Messages");
    $(".uploaded-file").html("");
    $("#fileUrl").val("");
    if ($(".chat-conversation").is(':visible')) {
        $('.button-back').css('display', 'block');
    }
    else {
        $('.button-back').css('display', 'none');
    }
    // Trigger the click event of chat-btn
    $(".chat-btn").click();
});

function receiveElement(message, name, url) {
    let attachmentEle = "";
    if (url) {
        attachmentEle = attachmentDom(name, url);
    }
    let receiveDom = `
        <div class="message-container">
            <div class="img-container">
                <img src="/images/child.png" alt="Avatar" class="avatar me-2">
                <div class="message">
                    <p>${message}</p>
                    <div>${attachmentEle}</div>
                </div>
            </div>
        </div>`;
    return receiveDom;
}

function sendElement(message, name, url) {
    let element = "";
    if (url) {
        element = attachmentDom(name, url);
    }
    let sendDom = `
        <div class="message-container">
            <div class="img-container" style="float: right;">
                <div class="message darker">
                     <p>${message}</p>   
                      <div>${element}</div>
                </div>
                <img src="/images/child.png" alt="Avatar" class="avatar right">
            </div>
        </div>`;
    return sendDom;
}
function attachmentDom(name, url) {
    let ele = `<div>
        <div class="d-flex justify-content-between me-3 border border-1 rounded-1 px-2" style="background-color:#fff">
        <a href="${url}" class=" text-decoration-none  d-flex justify-content-between align-items-center" target="_blank">
        <div class="text-muted"><i class="fa-solid fa-file"></i></div>
        <div class="p-1 text-black">
        <div class="document-name">${name}</div>
        </div>
        </a>
        </div>
        </div>`;
    return ele;
}
function getMessages(fromId) {
    $.ajax({
        url: '/Chats/GetMessages',
        type: 'GET',
        data: { fromUserId: fromId },
        success: function (response) {
            $(".chat-conversation").html($(response));
            var chatBox = $("#chat-body");
            chatBox.scrollTop(chatBox[0].scrollHeight);
        },
        error: function (xhr, status, error) {
            console.error("Error: " + error);
        }
    });
}
function getunreadCount() {
    $.ajax({
        url: '/Chats/GetUnreadCount',
        type: 'GET',
        success: function (response) {
            if (response != 0) {
                $("#notification-badge").show();
            } else {
                $("#notification-badge").hide();
            }
        },
        error: function (xhr, status, error) {
            console.error("Error: " + error);
        }
    });
}
function updateReadStatus(fromid) {
    $.ajax({
        url: '/Chats/UpdateReadStatus',
        type: 'PUT',
        data: { fromId: fromid },
        success: function (response) {
            if (response)
                getunreadCount();
        },
        error: function (xhr, status, error) {
            console.error("Error: " + error);
        }
    });
}

function UpdateList() {
    $.ajax({
        url: '/Chats/GetChatsList',
        type: 'GET',
        data: { isArchived: isArchived },
        success: function (response) {
            if (isArchived) {
                $("#Archived").html(response)
            }
            else {
                $(".inbox_chat").html(response);
            }
        },
        error: function (xhr, status, error) {
            console.error("Error: " + error);
        }
    });
}
//Change Tabs
$(document).ready(function () {
    $('#myTab a').click(function (e) {
        e.preventDefault();
        $(this).tab('show');
        if (e.target.id === 'unread-tab') {
            let unreadChats = $(".inbox_chat > .chat_list.unread").clone();
            $("#Unread").html(`<div class="d-flex justify-content-center text-muted mt-5">You’ve read all your messages</div>`);
            if (unreadChats.length != 0) {
                $("#Unread").html(unreadChats);
            }
        }
        else if (e.target.id === 'archived-tab') {
            getArchivedChats();
        }
        else {
            isArchived = false;
            UpdateList();
        }
    });
});
function getArchivedChats() {
    $.ajax({
        url: '/Chats/GetArchivedChats',
        type: 'GET',
        success: function (response) {
            $("#Archived").html(response)
        },
        error: function (xhr, status, error) {
            console.error("Error: " + error);
        }
    });
}
function messageLoading() {
    let loadingDom = `
        <div class="mt-4">
             <div class="">
                  <div class="d-flex justify-content-end">                                          
                       <div class="me-2 p-2" style="border: 1px solid #ddd;border-radius: 5px;">                                               
                          <div class="msg-loading animated-background float-end"style="border-radius:5px;"></div>
                          <div><div class="name-loading animated-background float-end  mt-2"style="border-radius:5px;"></div></div>
                       </div>
                       <div class="img-loading animated-background" style="border-radius:50%;"></div>
                   </div>
              </div>
        </div>   
        <div class="mt-4">
         <div class="">
            <div class="d-flex">
                <div class="img-loading animated-background" style="border-radius: 50%;"></div>
                <div class="ms-2 p-2" style="border: 1px solid #ddd; border-radius: 5px;">
                    <div class="msg-loading animated-background" style="border-radius: 5px;"></div>
                    <div class="name-loading animated-background mt-2" style="border-radius: 5px;"></div>
                </div>
              </div>
            </div>
        </div>`;
    return loadingDom;
}

//Attachment
$(document).on('change', "#attachment", function () {
    $(this).prop('disabled', true);
    const fileInput = this.files[0];
    if (fileInput) {
        uploadToBlob(fileInput, function (response) {
            if (response) {
                $("#fileUrl").val(response);
                $("#hdnfileName").val(fileInput.name);
                /* <div class="text-muted px-1"><i class="fa-solid fa-file"></i></div>*/
                let fileDom = `<div class="" style="background-color:#f5f5f5">               
                <div class="d-flex justify-content-between align-items-center px-3">
                <div style="color:#0d6efd;overflow: hidden; text-overflow: ellipsis; white-space: nowrap; max-width: 90%;">${fileInput.name}</div>
                <div class="float-end text-danger remove-attachment" style="cursor: pointer;font-weight: bold;">&times;</div></div></div>`;
                $(".uploaded-file").append(fileDom);
            } else {
            }
        });
    }
});
//Remove Attachment
$(document).on('click', ".remove-attachment", function () {
    $("#attachment").prop('disabled', false);
    $(".uploaded-file").html("");
    $("#fileUrl").val("");
});
function uploadToBlob(file, callback) {
    // Create FormData object
    var formData = new FormData();
    formData.append('file', file);

    $.ajax({
        type: 'POST',
        url: '/Chats/UploadAttachments',
        data: formData,
        contentType: false, // Set to false when using FormData
        processData: false,
        success: function (response) {
            callback(response);
        },
        error: function (error) {
            console.error('File upload failed.', error);
            callback(error);
        }
    });
}

//Archived and un Archived
$(document).on('click', '.ellipsis', function (e) {
    e.stopPropagation();
    var $archiveBtn = $(this).siblings('.archive-btn');

    $('.archive-btn').not($archiveBtn).hide();
    $archiveBtn.toggle();

    $('.ellipsis').not(this).removeClass("toggleellipsis");
    $(this).addClass("toggleellipsis");

    $('.chat_list').not($(this).closest('.chat_list')).removeClass("toggleBackground");
    $(this).closest('.chat_list').addClass("toggleBackground");
});

//$(document).on('click', function (e) {
//    if (!$('.ellipsis').is(e.target) && !$('.archive-btn').has(e.target).length) {
//        $('.archive-btn').hide();
//    }
//});
$(document).on('click', '#archive-btn', function (event) {
    event.stopPropagation();
    let chatId = $(this).data("id");
    let submitUrl = '/Chats/AddToArchived';
    if (!$(this).hasClass("archive")) {
        submitUrl = '/Chats/RemoveFromArchived';
        isArchived = true;
    }
    $.ajax({
        url: submitUrl,
        type: 'PUT',
        data: { fromId: chatId },
        success: function (response) {
            if (response)
                UpdateList();
        },
        error: function (xhr, status, error) {
            console.error("Error: " + error);
        }
    });
});
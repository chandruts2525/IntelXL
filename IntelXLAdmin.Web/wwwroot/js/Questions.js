$(document).ready(function () { 
    var getUrl = '';
    var submitUrl = '';
    var target = '';
    let selectedValue = 0;
    var targetDropdown = "";
    let selectedDropDown = "";
    let dropDownOptions = [];
    let dropDownIds = ["courses", "classes", "subjects", "units", "topics", "subtopics"];
    let lang_drpdown = [];
    let activestatus = true;
    let orders = [];
    //DropDown Functionalities  
    $("#courses, #classes, #subjects, #units, #topics").on("change", function () {
        var changedId = this.id;
        const index = dropDownIds.indexOf(changedId);
        let resetDropdown = dropDownIds.slice(index + 1);
        let disableDropdown = dropDownIds.slice(index + 2);
        let btnenable = dropDownIds.slice(0, index);
        $('#selectRow button').prop('disabled', true);
        $.each(disableDropdown, function (index, value) {
            $("#" + value).prop('disabled', true);
        });
        $.each(resetDropdown, function (index, value) {
            $("#" + value).empty();
            $("#" + value).append($("<option selected disabled>Select here...</option>"));
        });
        $(".warning-msg").text("");
        let sourceDropdown = $(this);
        let DropDownName = sourceDropdown.data("target");
        targetDropdown = $("#" + DropDownName);
        sourceDropdown.closest('#selectRow').find("button").prop('disabled', false);
        targetDropdown.closest('#selectRow').find("button").prop('disabled', false);
        $(".all-btn, .add-qus").prop('disabled', true)
        targetDropdown.empty();
        targetDropdown.prop('disabled', false);
        let getUrl = "/" + DropDownName + "/GetAllById";
        selectedValue = sourceDropdown.val();
        if (selectedValue) {
            updateClassesDropdown(getUrl, selectedValue)
        }
        $.each(btnenable, function (index, value) {
            $("#" + value).closest('#selectRow').find("button").prop('disabled', false);
        });

    });
    $("#subtopics").on("change", function () {
        if ($("#subtopics").val()) {
            $(".all-btn, .add-qus").prop('disabled', false)
        }
    });
    function updateClassesDropdown(uri, data) {
        $.ajax({
            url: uri,
            type: "POST",
            data: { id: data },
            success: function (result) {              
                targetDropdown.append($("<option selected disabled>Select here...</option>"));
                if (result.length !== 0) {
                    $.each(result, function (index, value) {
                        let status = (value.status) ? "" : " (Inactive)";
                        //Updating data to dropdown
                        if (status == "") {
                            if (value.className)
                                targetDropdown.append($("<option value=" + value.classId + ">" + value.className + "<span class=''>" + status + "</span></option>"));
                            else if (value.subjectName)
                                targetDropdown.append($("<option value=" + value.subjectId + ">" + value.subjectName + "<span class=''>" + status + "</span></option>"));
                            else if (value.unitName)
                                targetDropdown.append($("<option value=" + value.unitId + ">" + value.unitName + "<span class=''>" + status + "</span></option>"));
                            else if (value.topic)
                                targetDropdown.append($("<option value=" + value.topicId + ">" + value.topic + "<span class=''>" + status + "</span></option>"));
                            else
                                targetDropdown.append($("<option value=" + value.subTopicId + ">" + value.subTopic + "<span class=''>" + status + "</span></option>"));
                        }                        
                    });
                }
            },
            error: function (xhr, status, error) {
                console.error("Error: " + error);
            }
        });
    }

    function getForm(formDiv, keyValue, KeyId) {
        let name = getName(formDiv);
        let langid=$(".ix_language").val();
        let sampleform = '<div class="d-flex justify-content-between align-items-center">';
        sampleform += '<h2 class="modal-title mb-0" id="exampleModalLabel" style="color: mediumpurple;"> ' + formDiv + ' </h2>';
        sampleform += '<button type="button" class="btn-close" data-bs-dismiss="modal" aria-label="Close"></button>';
        sampleform += '</div>';
        sampleform += '<form id="addNewForm">';
        sampleform += '<div class="mt-lg-5">';
        sampleform += '<label for="course-name" class="form-label"><b> ' + formDiv + '<span class="text-danger">*</span></b></label>';
        sampleform += '<input type="text" class="form-control" id="inputValue" placeholder="Enter a ' + formDiv + '" name="' + name + '" style="width: 100%; border-color: mediumpurple;">';
        sampleform += '</div>';
        if (formDiv == "Class") {
            sampleform += '<div class="mt-2">';
            sampleform += '<label for="course-description" class="form-label"><b> ' + formDiv + 'Description <span class="text-danger">*</span></b></label>';
            sampleform += '<textarea type="text" class="form-control" name="Description" id="classDescription" placeholder="Enter Description" style="width: 100%; border-color: mediumpurple;">';
            sampleform += '</textarea>'
            sampleform += '</div>';
        }

        sampleform += '<div class="text-danger p-2" id="error-info"></div>';
        sampleform += '<div class="mt-5 d-flex justify-content-end">';
        sampleform += '<button type="button" class="btn btn-primary" id="submitButton" style="width: 110px; background-color: mediumpurple; box-shadow: none; border-color: mediumpurple;">Submit</button>';
        sampleform += '<input type="hidden" name="' + KeyId + '" value="' + keyValue + '"/>';
        if (formDiv == "Course") {
            sampleform += '<input type="hidden" name="LanguageId" value="' + langid + '"/>';
        }
        sampleform += '</form>';
        return sampleform;
    }

    function getName(value) {
        let result = '';
        switch (value) {
            case 'Course':
                result = "CourseName";
                break;
            case 'Class':
                result = "ClassName";
                break;
            case 'Subject':
                result = "SubjectName";
                break;
            case 'Unit':
                result = "UnitName";
                break;
            case 'Topic':
                result = "Topic";
                break;
            case 'SubTopic':
                result = "SubTopic";
                break;
            default:
                result = "";
                break;
        } return result;
    }
    $(".otherCourse").on('click', function (event) {
        dropDownOptions = [];
        $(".warning-msg").text("");
        submitUrl = "/Course/AddCourse";
        target = $(this.closest('#selectRow')).find('select');
        $(target).find('option:not(:disabled)').each(function () {
            dropDownOptions.push($(this).text().toLowerCase().trim());
        });
        $("#myModal .add-new-container").html(getForm("Course", 0));
        $("#myModal").modal("show");
    });

    $(".otherClass").on('click', function () {
        dropDownOptions = [];
        if (!($('#courses').val())) {
            $(".warning-msg").text("Select any Course!");
            return false;
        }
        submitUrl = "/Classes/AddClass";
        target = $(this.closest('#selectRow')).find('select');
        $(target).find('option:not(:disabled)').each(function () {
            dropDownOptions.push($(this).text().toLowerCase().trim());
        });
        let keyValue = $("#courses").val();
        $("#myModal .add-new-container").html(getForm("Class", keyValue, "CourseId"));
        $("#myModal").modal("show");
    });

    $(".otherSubject").on('click', function () {
        dropDownOptions = [];
        if (!($('#classes').val())) {
            $(".warning-msg").text("Select any Class!");
            return false;
        }
        submitUrl = "/Subjects/AddSubject";
        target = $(this.closest('#selectRow')).find('select');
        $(target).find('option:not(:disabled)').each(function () {
            dropDownOptions.push($(this).text().toLowerCase().trim());
        });
        let keyValue = $("#classes").val();
        $("#myModal .add-new-container").html(getForm("Subject", keyValue, "ClassId"));
        $("#myModal").modal("show");
    });

    $(".otherUnit").on('click', function () {
        dropDownOptions = [];
        if (!($('#subjects').val())) {
            $(".warning-msg").text("Select any Subject!");
            return false;
        }
        submitUrl = "/Units/AddUnit";
        target = $(this.closest('#selectRow')).find('select');
        $(target).find('option:not(:disabled)').each(function () {
            dropDownOptions.push($(this).text().toLowerCase().trim());
        });
        let keyValue = $("#subjects").val();
        $("#myModal .add-new-container").html(getForm("Unit", keyValue, "SubjectId"));
        $("#myModal").modal("show");
    });

    $(".otherTopic").on('click', function () {
        dropDownOptions = [];
        if (!($('#units').val())) {
            $(".warning-msg").text("Select any Unit!");
            return false;
        }
        submitUrl = "/Topics/AddTopic";
        target = $(this.closest('#selectRow')).find('select');
        $(target).find('option:not(:disabled)').each(function () {
            dropDownOptions.push($(this).text().toLowerCase().trim());
        });
        let keyValue = $("#units").val();
        $("#myModal .add-new-container").html(getForm("Topic", keyValue, "UnitId"));
        $("#myModal").modal("show");
    });
    $(".otherSubtopic").on('click', function () {
        dropDownOptions = [];
        if (!($('#topics').val())) {
            $(".warning-msg").text("Select any Topic!");
            return false;
        }
        submitUrl = "/SubTopics/AddSubTopic";
        target = $(this.closest('#selectRow')).find('select');
        $(target).find('option:not(:disabled)').each(function () {
            dropDownOptions.push($(this).text().toLowerCase().trim());
        });
        let keyValue = $("#topics").val();
        $("#myModal .add-new-container").html(getForm("SubTopic", keyValue, "TopicId"));
        $("#myModal").modal("show");
    });

    //Create new entry
    $(document).on("click", "#submitButton", function () {
        var form = $('#addNewForm');
        var formData = new FormData(form[0]);
        $("#error-info").text("");
        let key = formData.keys().next().value;
        if (key) {
            let value = formData.get(key);           
            var isExists = dropDownOptions.includes(value.toLowerCase().trim());
            if (!value.trim() || (form.find("#classDescription").length != 0 && !$("#classDescription").val())) {
                $("#error-info").text("*fields are required");
                return false;
            }
            else if (isExists) {
                $("#error-info").text("Already Exists");
                return false;
            }
        }
        $.ajax({
            url: submitUrl,
            type: "POST",
            data: formData,
            processData: false,
            contentType: false,
            success: function (result) {
                if (result.id) {
                    target.append($("<option value='" + result.id + "'>" + result.value + "</option>"));
                }
                $("#myModal").modal("hide");
            },
            error: function (xhr, status, error) {
                console.log("Error: " + error);
            }
        });
    });
    $('#myModal').on('hidden.bs.modal', function () {
        $("#inputValue").val("");
    });


    //Edit Functionalities
    $(document).on("click", ".getAll", function () {
        dropDownOptions = [];
        orders = [];
        var currentRow = $(this.closest('#selectRow'));
        var selectedValue = currentRow.prev('#selectRow').find('select').val();
        selectedDropDown = currentRow.find('select');
        $(selectedDropDown).find('option:not(:disabled)').each(function () {
            dropDownOptions.push($(this).text().toLowerCase());
        });
        let title = $(this).data('target');
        let tableContent = gettable(title);
        let editUri = '';
        if (title === "Course") {
           selectedValue = $(".ix_language").val();
            editUri = "/Course/GetAllById";
        } else if (selectedValue) {
            editUri = "/" + title + "/GetAllById";
        } else {
            return false;
        }
        $.ajax({
            url: editUri,
            type: "Get",
            data: { id: selectedValue},
            success: function (result) {
                if (result.length !== 0) {
                    //$.each(result, function (index, value) {
                    //    orders.push(value.order)
                    //});
                    $.each(result, function (index, value) {    
                        activestatus = value.status;
                        if (value.courseName)
                            tableContent += getTableData(value.courseId, value.courseName, /*value.order, */"CourseId", "courseName");
                        else if (value.className)
                            tableContent += getTableData(value.classId, value.className, /*value.order, */"ClassId", "ClassName", value.description);
                        else if (value.subjectName)
                            tableContent += getTableData(value.subjectId, value.subjectName, /*value.order, */"SubjectId", "SubjectName");
                        else if (value.unitName)
                            tableContent += getTableData(value.unitId, value.unitName, /*value.order, */"UnitId", "UnitName");
                        else if (value.topic)
                            tableContent += getTableData(value.topicId, value.topic, /*value.order,*/ "TopicId", "Topic");
                        else
                            tableContent += getTableData(value.subTopicId, value.subTopic, /*value.order,*/ "SubTopicId", "SubTopic");
                    });
                    tableContent += '</div></div></div>';
                }
                else {
                    tableContent += '<div class="my-3 d-flex justify-content-center text-muted" style="font-size: 24px;">No Data Available</div>';
                }
                $("#EditModal .edit-container").html(tableContent);
                $("#EditModal").modal("show");
            },
            error: function (xhr, status, error) {
                console.error("Error: " + error);
            }
        });
    });

    function gettable(title) {
        let html = '<div class="modal-header px-3 p-2" style="background-color:#008080;color:#fff;">';
        html += '<h5 class="modal-title" id = "staticBackdropLabel">' + title + '</h5>';
        html += '<button type="button" class="btn-close btn-close-white" data-bs-dismiss="modal" aria-label="Close"></button>';
        html += ' </div>';
        html += '<div class="modal-body" style="max-height: 927px; overflow-y: auto; min-height:590px;scrollbar-width: thin;" title=' + title + '>';
        html += '<div class="container-fluid p-0">';
        html += '<header class="edit-header ps-2 d-flex" ><b>' + title + ' Name </b></header>';
        return html;
    }

    function getTableData(id, name, /*order,*/ inputId, inputName, description) {
        let content = '<div class="d-flex row course-edit" style="display: flex; align-items: center;">';
        content += '<form id="formDiv" class="courseform p-0" style=" height:100%;">';
        content += '<input type="hidden" class="hdnId" name="' + inputId + '"value="' + id + '"/>';
        content += '<div style="flex: 1; display: flex; justify-content: space-between; align-items: center;">';
        content += '<div class="course-name py-2 d-flex">';
        content += '<div style="display: inline-block;">' + name + '</div>';
        content += '<input class="form-control input-sm edit-input pt-sm-1 pb-sm-1" id="CourseName" type="text"  name="' + inputName + '" value="' + name + '"  style="display: none; width: 100%;">';
        if (inputName !== "ClassName") {
            content += '<span class="text-danger text-nowrap ps-1 empty-alert" style="display:none">*Required</span>';
        }
        content += '</div>';
        content += '<div class="bttn" style="display: flex; align-items: center;">';
        //content += '<div class=""><select class="form-select text-secondary order-change"data-val=' + order +'>';

        //$.each(orders, function (i, v) {
        //    var $option = $('<option>', {
        //        value: v,
        //        text: v
        //    });
        //    if (v == order) {
        //        $option.attr("selected", "selected");
        //    }
        //    content += $option.prop('outerHTML'); // Append the HTML of the option element
        //});

        //content += '</select></div>'; 
        content += '<div class="btn edit-btn" style="display: inline-block;"><i class="fa-solid fa-pencil" style="color: blue;"></i></div>';
        content += '<div class="btn btn-sm save-btn" style="display: none;"><i class="fa-solid fa-check" style="color:#198754; font-size:large;"></i></div>';
        if (activestatus)
            content += '<div class="del-restore"><div class="btn btn-sm delete-btn" style="display: inline-block;"><i class="fa-solid fa-trash-can alert-danger"></i></div></div>';
        else
            content += '<div del-restore><div class="btn btn-sm restore-btn" style="display: inline-block;"><i class="fas fa-undo-alt"></i></div></div>';        
        content += '<div class="btn btn-sm cancel-btn" style="display: none;"><i class="fa-solid fa-xmark" style="color:black;font-size:large;"></i></div>';
        content += '</div>';
        content += '</div>';
        if (inputName == "ClassName") {
            content += '<div class="course-name">';
            content += '<label for="ClassTextarea"><b>Description:</b></label>';
            content += '<div class="py-1" style="max-width:650px;">' + description + '</div>';           
            content += '<textarea class="form-control my-2 edit-text pt-sm-1 pb-sm-1" name="Description" id="ClassTextarea" style="display: none; width: 100%;"> ' + description + '</textarea>';
            content += '<span class="text-danger text-nowrap ps-1 empty-alert" style="display:none">All fields are required</span>';
            content += '</div>';
        }
        content += '</form>';
        content += '</div>';
        return content;
    }

    var text = "";
    var description = "";
    $(document).on("click", ".edit-btn, .cancel-btn", function () {
        var row = $(this).closest(".row");
        row.find(".course-name div").toggle();
        row.find(".edit-input").toggle();
        row.find(".edit-text").toggle();
        row.find(".edit-btn").toggle();
        row.find(".save-btn").toggle();
        row.find(".delete-btn").toggle();
        row.find(".restore-btn").toggle();
        row.find(".cancel-btn").toggle();
        row.find(".empty-alert").hide();
        if ($(this).hasClass("edit-btn")) {
            text = row.find('input[type="text"]').val();
            description = row.find('#ClassTextarea').val();
        }
        else {
            row.find('input[type="text"]').val(text);
            row.find('#ClassTextarea').val(description);
        }

    });

    $(document).on("click", ".save-btn", function () {
        let hasChanged = false;
        var form = $(this).closest(".row");
        var titleValue = $(this).closest(".modal-body").attr("title");
        var id = form.find('input[type="hidden"]').val();
        var name = form.find('input[type="text"]').val();
        let class_description = form.find('#ClassTextarea').val();
        if (text !== name) {
            hasChanged = true;
        }
        var isExists = dropDownOptions.includes(name.toLowerCase().trim());
        if (!name.trim() || (class_description !== undefined && class_description.trim() === '')) {
            //if (!name.trim()) {
            form.find(".empty-alert").text("Fields required").show();
            //form.find("#ClassTextarea").addClass("is-invalid");
            return false;
        }
        else if (isExists && hasChanged) {
            form.find(".empty-alert").text("Already Exists").show();
            return false;
        }
        else {
            var formData = new FormData();
            let input_id = form.find('input[type="hidden"]').attr('name');
            let input_name = form.find('input[type="text"]').attr('name');
            formData.append(input_id, id);
            formData.append(input_name, name);
            formData.append("description", class_description);
            //var postUri = "/" + titleValue + "/Update";
            if (titleValue == "Language") {
                var postUri = "/LanguageOfInstructions/Update";
            }
            else {
                var postUri = "/" + titleValue + "/Update";
            } 
            $.post({
                url: postUri,
                data: formData,
                processData: false,
                contentType: false,
                success: function (response) {
                    if (response) {
                        window.location.href = "/Question/Index";
                    }
                },
                error: function (error) {
                    console.error("Error:", error);
                }
            });
        }
    });
    //Delete 
    $(document).on("click", ".delete-btn", function () {
        //Getting the current clicked data
        let current = $(this).closest(".row");
        let current_title = $(this).closest(".modal-body").attr("title");
        let current_id = current.find('.hdnId').val();
        let deleteUri = "/" + current_title + "/Delete";
        if (current_title == "Language") {
            deleteUri = "/LanguageOfInstructions/Delete";
        }
        
        $.ajax({
            url: deleteUri,
            data: {id: current_id},
            success: function (response) {
                if (response) {
                    //Remove the deleted one from list
                    //current.slideUp(100, function () {
                    //    $(this).remove();
                    //});
                    //Remove the deleted one from Dropdown
                    /* selectedDropDown.find("option[value='" + current_id + "']").remove();*/
                    showtoast("success", "Deleted successfully"); 
                    current.find(".del-restore").html('<div class="btn btn-sm restore-btn" style="display: inline-block;"><i class="fas fa-undo-alt"></i></div>');
                }
                else {
                    showtoast("error", "Something went wrong!.Faild to delete");   
                }
            },
            error: function (error) {
                console.error("Error:", error);
                showtoast("error", "Something went wrong!.Failed to delete");  
            }
        });
    });
    //Restore/Activate
    $(document).on("click", ".restore-btn", function () {
        //Getting the current clicked data
        let current = $(this).closest(".row");
        let current_title = $(this).closest(".modal-body").attr("title");
        let current_id = current.find('.hdnId').val();
        let restoreUri = "/" + current_title + "/Restore";
        if (current_title == "Language") {
            restoreUri = "/LanguageOfInstructions/Restore";
        }
       
        $.ajax({
            url: restoreUri,
            data: { id: current_id },
            success: function (response) {
                if (response) {
                    showtoast("success", "Restored successfully");  
                    current.find(".del-restore").html('<div class="btn btn-sm delete-btn" style="display: inline-block;"><i class="fa-solid fa-trash-can alert-danger"></i>');
                }
                else {
                    showtoast("error", "Something went wrong!. Please try again");                  
                }
            },
            error: function (xhr, status, error) {                
                showtoast("error", "Something went wrong!. Please try again");     
            }
        });
    });
  
     //LanguageDropDown Functionalities 
    $(".ix_language").on("change", function () {    
        let lan_id = $(this).val();  
        $("#courses").empty();
        $("#courses").append($("<option selected disabled>Select here...</option>"));
        let resetDropdown = dropDownIds.slice(1);       
        $.each(resetDropdown, function (index, value) {
            $("#" + value).prop('disabled', true);
            $("#" + value).empty();
            $("#" + value).append($("<option selected disabled>Select here...</option>"));
        });
        $('#selectRow button').prop('disabled', true);
        $.ajax({
            url: "/Question/GetCourses",
            type: "GET",
            data: { id: lan_id },
            success: function (result) {
                $("#courses").prop('disabled', false);
                $("#courses").closest('#selectRow').find("button").prop('disabled', false);
                $.each(result, function (index, value) {
                    let active_status = (value.status) ? "" : " (Inactive)"; 
                    if (active_status == "") {
                        $("#courses").append($("<option value=" + value.courseId + ">" + value.courseName + "<span class=''>" + active_status + "</span></option>"));
                    }
                });
            },
            error: function (xhr, status, error) {
                console.error("Error: " + error);
            }
        })
    });
    $(".ix_addlanguage").on('click', function () {
       lang_drpdown= [];
        let lang_target = $(this.closest('#ix_language')).find('select');
        $(lang_target).find('option:not(:disabled)').each(function () {
            lang_drpdown.push($(this).text().toLowerCase().trim());
        });        
        let lang_form = '<div class="d-flex justify-content-between align-items-center">';
        lang_form += '<h2 class="modal-title mb-0" id="exampleModalLabel" style="color: mediumpurple;">Language</h2>';
        lang_form += '<button type="button" class="btn-close" data-bs-dismiss="modal" aria-label="Close"></button>';
        lang_form += '</div>';
        lang_form += '<form id="addNewLanguage">';
        lang_form += '<div class="mt-lg-5">';
        lang_form += '<label for="Language" class="form-label"><b>Language<span class="text-danger">*</span></b></label>';
        lang_form += '<input type="text" class="form-control" id="inputValue" placeholder="Enter a Language" name="Language" style="width: 100%; border-color: mediumpurple;">';
        lang_form += '</div>';     
        lang_form += '<div class="text-danger p-2" id="error-info"></div>';
        lang_form += '<div class="mt-5 d-flex justify-content-end">';
        lang_form += '<button type="button" class="btn btn-primary" id="add_lang" style="width: 110px; background-color: mediumpurple; box-shadow: none; border-color: mediumpurple;">Submit</button>';       
        lang_form += '</form>';
        $("#myModal .add-new-container").html(lang_form);
        $("#myModal").modal("show");
    });
    $(document).on("click", "#add_lang", function () {
        var form = $('#addNewLanguage');
        var formData = new FormData(form[0]);
        $("#error-info").text("");
        var languageValue = formData.get('Language');       
        var isExists = lang_drpdown.includes(languageValue.toLowerCase().trim());
            if (!languageValue.trim()) {
                $("#error-info").text("*fields are required");
                return false;
            }
            else if (isExists) {
                $("#error-info").text("Already Exists");
                return false;
            }
      
        $.ajax({
            url: "/LanguageOfInstructions/AddLanguage",
            type: "POST",
            data: formData,
            processData: false,
            contentType: false,
            success: function (result) {
                if (result.id) {
                    $(".ix_language").append($("<option value='" + result.id + "'>" + result.value + "</option>"));
                }
                $("#myModal").modal("hide");
            },
            error: function (xhr, status, error) {
                console.log("Error: " + error);
            }
        });
    });
    $(document).on("click", ".ix_editlanguage", function () {
        lang_drpdown = [];
        let lang_target = $(".ix_addlanguage").closest('#ix_language').find('select');
        $(lang_target).find('option:not(:disabled)').each(function () {
            lang_drpdown.push($(this).text().toLowerCase().trim());
        }); 
        let lang_table = gettable("Language");
        $.ajax({
            url: "/Question/GetLanguages",
            type: "GET",           
            success: function (result) { 
                if (result.length !== 0) {
                    //$.each(result, function (index, value) {                       
                    //    orders.push(value.order)                       
                    //});
                    $.each(result, function (index, value) {
                        activestatus = value.status;
                        lang_table += getTableData(value.languageId, value.language, /*value.order,*/ "LanguageId", "Language");
                    });
                    lang_table += '</div></div></div>';
                }
                else {
                    lang_table += '<div class="my-3 d-flex justify-content-center text-muted" style="font-size: 24px;">No Data Available</div>';
                }
                $("#EditModal .edit-container").html(lang_table);
                $("#EditModal").modal("show");
            },
            error: function (xhr, status, error) {
                console.log("Error: " + error);
            }
        });
        
    });
    //$(document).on('change', '.order-change', function () {
    //    let current_val = $(this).data("val");
    //    let new_val = $(this).val();
    //    console.log(current_val)
    //    console.log(new_val)
    //});
    
});
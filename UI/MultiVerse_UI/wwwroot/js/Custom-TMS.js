
var RightsList = JSON.parse($("#RightsList").val());
SetPageRights();
CKEDITOR.replace('txtComments', {
    height: 100,
    extraPlugins: 'colorbutton',
    colorButton_colors: 'FF0000,00FF00,0000FF,FFFF00,FF00FF,00FFFF,000000,FFFFFF,808080,C0C0C0,800000,008000,000080,FFFF80,FF80FF,80FFFF,800080,008080,808000,FF8080'
});
CKEDITOR.replace('txtNote', {
    height: 100,
    extraPlugins: 'colorbutton',
    colorButton_colors: 'FF0000,00FF00,0000FF,FFFF00,FF00FF,00FFFF,000000,FFFFFF,808080,C0C0C0,800000,008000,000080,FFFF80,FF80FF,80FFFF,800080,008080,808000,FF8080'
});
//$("#txtStartDate_1").kendoDatePicker({
//    animation: false,
//    format: "MM/dd/yyyy",
//    open: function (e) {

//        var clearbuttonContainer = e.sender.dateView.calendar.element.find(".clear-button-container");

//        if (!clearbuttonContainer.length) {
//            // Create a container for custom buttons
//            clearbuttonContainer = $('<div class="k-footer clear-button-container"></div>');

//            // Create custom button (Clear Date) and append it to the container
//            var clearbutton = $('<button tabindex="-1" class="k-button custom-clear-button k-flex k-button-md k-button-flat k-button-flat-primary k-rounded-md" title="Clear Date"><span style="color:red" class="k-button-text">Clear Date</span></button>');
//            clearbuttonContainer.append(clearbutton);

//            // Append the custom buttons container to the DatePicker popup
//            e.sender.dateView.calendar.element.append(clearbuttonContainer);

//            // Handle click events for the custom button
//            clearbutton.on("click", function () {
//                // Clear the selected date
//                e.sender.value(new Date());
//                e.sender.value(null);
//                // Close the DatePicker
//                e.sender.close();
//            });
//        }
//    }
//});
//$("#txtEndDate_1").kendoDatePicker({
//    animation: false,
//    format: "MM/dd/yyyy",
//    open: function (e) {

//        var clearbuttonContainer = e.sender.dateView.calendar.element.find(".clear-button-container");

//        if (!clearbuttonContainer.length) {
//            // Create a container for custom buttons
//            clearbuttonContainer = $('<div class="k-footer clear-button-container"></div>');

//            // Create custom button (Clear Date) and append it to the container
//            var clearbutton = $('<button tabindex="-1" class="k-button custom-clear-button k-flex k-button-md k-button-flat k-button-flat-primary k-rounded-md" title="Clear Date"><span style="color:red" class="k-button-text">Clear Date</span></button>');
//            clearbuttonContainer.append(clearbutton);

//            // Append the custom buttons container to the DatePicker popup
//            e.sender.dateView.calendar.element.append(clearbuttonContainer);

//            // Handle click events for the custom button
//            clearbutton.on("click", function () {
//                // Clear the selected date
//                e.sender.value(new Date());
//                e.sender.value(null);
//                // Close the DatePicker
//                e.sender.close();
//            });
//        }
//    }
//});
//$("#txtReviewDate_1").kendoDatePicker({
//    animation: false,
//    format: "MM/dd/yyyy",
//    open: function (e) {

//        var clearbuttonContainer = e.sender.dateView.calendar.element.find(".clear-button-container");

//        if (!clearbuttonContainer.length) {
//            // Create a container for custom buttons
//            clearbuttonContainer = $('<div class="k-footer clear-button-container"></div>');

//            // Create custom button (Clear Date) and append it to the container
//            var clearbutton = $('<button tabindex="-1" class="k-button custom-clear-button k-flex k-button-md k-button-flat k-button-flat-primary k-rounded-md" title="Clear Date"><span style="color:red" class="k-button-text">Clear Date</span></button>');
//            clearbuttonContainer.append(clearbutton);

//            // Append the custom buttons container to the DatePicker popup
//            e.sender.dateView.calendar.element.append(clearbuttonContainer);

//            // Handle click events for the custom button
//            clearbutton.on("click", function () {
//                // Clear the selected date
//                e.sender.value(new Date());
//                e.sender.value(null);
//                // Close the DatePicker
//                e.sender.close();
//            });
//        }
//    }
//});
//$("#ETADate_1").kendoDatePicker({
//    animation: false,
//    format: "MM/dd/yyyy",
//    open: function (e) {

//        var clearbuttonContainer = e.sender.dateView.calendar.element.find(".clear-button-container");

//        if (!clearbuttonContainer.length) {
//            // Create a container for custom buttons
//            clearbuttonContainer = $('<div class="k-footer clear-button-container"></div>');

//            // Create custom button (Clear Date) and append it to the container
//            var clearbutton = $('<button tabindex="-1" class="k-button custom-clear-button k-flex k-button-md k-button-flat k-button-flat-primary k-rounded-md" title="Clear Date"><span style="color:red" class="k-button-text">Clear Date</span></button>');
//            clearbuttonContainer.append(clearbutton);

//            // Append the custom buttons container to the DatePicker popup
//            e.sender.dateView.calendar.element.append(clearbuttonContainer);

//            // Handle click events for the custom button
//            clearbutton.on("click", function () {
//                // Clear the selected date
//                e.sender.value(new Date());
//                e.sender.value(null);
//                // Close the DatePicker
//                e.sender.close();
//            });
//        }
//    }
//});
$("#selectCategory_1").select2();
$("#selectPriority_1").select2();
$("#selectStatus_1").select2();
$("#selectApplicationURL_1").select2();
$("#selectLeadAssignTo_1").select2();
 

//if (RightsList.IsView) {
//    TMS_Load_Data();
//}

function SetPageRights() {
    if (RightsList.IsAdd) {
        $('#TasksModalButton').show();
    }
}

var itemsPerPage;
var totalItems = 0;
var currentPage = 1;
// var rowCounter = 1;
var totalRows = 1;

//Load Data UI TMS Function Start
function TMS_Load_Data() {

   
    var kendogridid = 'tmsgrid';
    var pagesize = 5;
    $("#PageLoader").show();
    $("#" + kendogridid).empty();
    var TMSfieldstype = {
        // T1: { type: KendoFilterTypes.String, srtype: SRVTypes.String },
        // T2: { type: KendoFilterTypes.String, srtype: SRVTypes.String },
        // T3: { type: KendoFilterTypes.String, srtype: SRVTypes.String },
    }
    var _griddatasource = GetGridDataSourceAjaxCommon(kendogridid, pagesize, "/TMS/Get_TMS_List", true, true, true, TMSfieldstype, false, false);
    dataSource = _griddatasource
    $("#" + kendogridid).kendoGrid({
        dataSource: dataSource,
        responsive: false,
        pageable: {
            alwaysVisible: true,
            refresh: true,
            pageSizes: [5, 10, 15, 20, 30],
            messages: {
                //display: "{0} - {1} of {2} items &nbsp;&nbsp;|&nbsp;&nbsp; Export:&nbsp;<img id='" + kendogridid + "_exportExcel' title='Click here to export to excel file' src='/icon/excel_icon.jpg' style='width:23px; height:23px; cursor:pointer' onclick='onClickExportExcel(this)' /> ",
                empty: "No data found",
            }
        },
        sortable: false,
        resizable: false,
        reorderable: false,
        noRecords: true,
        filterable: false,
        customfixheader: false, //custom fields for onKendoDataBound function
        pageloaderid: "PageLoader", //custom fields for onKendoDataBound function
        isheaderhide: true,
        dataBound: onKendoDataBound,
        dataBinding: function (e) {
            $("#PageLoader").hide();
        },
        columns: [
            {
                field: "T1",
                width: "33.33%",
                template: function (item) {
                    var html = Get_TMS_Html(item.T1)
                    return html;
                }
            },
            {
                field: "T2",
                width: "33.33%",
                template: function (item) {
                    var html = Get_TMS_Html(item.T2)
                    return html;
                }
            },
            {
                field: "T3",
                width: "33.33%",
                template: function (item) {
                    var html = Get_TMS_Html(item.T3)
                    return html;
                }
            },
        ],
        detailInit: function (e) {
            $("#PageLoader").hide();

        }
    });

}


function Get_TMS_Html(task) {
    var html = '';

    if (task.T_ID ==0 || task==null) {
        return html;
    }

    html = "<div class='content'>";
    html += "<div class='card'>";
    html += "<div class='card-body'>";

    html += "<div class='d-flex align-items-center justify-content-between'>";

    html += "<div class='lesson_name'>";
    html += "<span class='small text-muted project_name fw-bold'> " + task.TaskName + " </span>";
    html += "<h6 class='mb-0 fw-bold fs-6 mb-2'> " + task.Task_Item + " </h6>";
    html += "</div>";

    html += "<div class='btn-group' role = 'group' aria-label='Basic outlined example'>";
    html += "<button type='button' class='btn btn-outline-secondary' onclick='AddOrEditTasksModal(\"" + task.Encrypted_T_ID + "\",\"" + task.Encrypted_TD_ID + "\")'> <i class='fa fa-edit text-success' > </i></button > ";
    html += "<button type='button' class='btn btn-outline-secondary' onclick='RemoveTasks(\"" + task.Encrypted_T_ID + "\",\"" + task.Encrypted_TD_ID + "\");'><i class='fa fa-trash text-danger'></i></button>";
    html += "</div>";

    html += "</div>";

    html += "<div class='d-flex align-items-center'>";
    html += "<div class='avatar-list avatar-list-stacked pt-2'>";
    html += "<span class='btn btn-success'> " + task.Status + " </span>";
    html += "</div>";
    html += "</div>";
    html += "<div class='row g-2 pt-4'>";
    html += "<div class='col-6'>";
    html += "<div class='d-flex align-items-center cursor-pointer'>";
    html += "<i class='fa fa-paperclip'></i>";
    html += "<span class='ms-2' onclick='GetAttachmnets(\"" + task.Encrypted_T_ID + "\",\"" + task.Encrypted_TD_ID + "\");'>" + task.TotalAttachments + " Attach</span>";
    html += "</div>";
    html += "</div>";
    html += "<div class='col-6'>";
    html += "<div class='d-flex align-items-center cursor-pointer'>";
    html += "<i class='fa fa-hourglass-start'></i>";
    html += "<span class='ms-2'>" + task.ETA + "</span>";
    html += "</div>";
    html += "</div>";
    // html += "<div class='col-6'>";
    // html += "<div class='d-flex align-items-center cursor-pointer' data-bs-toggle='modal' data-bs-target='#AddOrEditMembers'>";
    // html += "<i class='fa fa-plus-circle'></i>";
    // html += "<span class='ms-2'>Add Memebers</span>";
    // html += "</div>";
    // html += "</div>";
    html += "<div class='col-6'>";
    html += "<div class='d-flex align-items-center cursor-pointer' onclick='GetMemebers(\"" + task.Encrypted_TD_ID + "\");'>";
    html += "<i class='fa fa-users'></i>";
    html += "<span class='ms-2'>" + task.TotalMemebers + " Members </span>";
    html += "</div>";
    html += "</div>";
    html += "<div class='col-6'>";
    html += "<div class='d-flex align-items-center cursor-pointer' onclick='GetCommentsModal(\"" + task.Encrypted_TD_ID + "\");'>";
    html += "<i class='fa fa-comment'></i>";
    html += "<span class='ms-2'>" + task.TotalComments + "</span>";
    html += "</div>";
    html += "</div>";
    html += "</div>";
    html += "<div class='dividers-block'></div>";
    html += "<div class='d-flex align-items-center justify-content-between mb-2'>";
    html += "<h4 class='small fw-bold mb-0'> Progress </h4>";
    html += "<span class='small light-danger-bg p-1 rounded'><i class='fa fa - clock'></i>" + task.LeftDays + " Days Left</span>";
    html += "</div>";
    html += "<div class='progress' style='height: 8px;'>";

    html += "</div>";
    html += "</div>";
    html += "</div>";


    return html;
}
// Load Data UI TMS Function End

// Tasks Function Start
function AddOrEditTasksModal(T_ID, TD_ID) {

    ResetAddOrEditTasksModal();
    if (T_ID != "" && TD_ID != "") {
        $("#AddOrEditTasksModal .modal-title").text("Update Task");
        $(".btnAddOrEdit").text("UPDATE");
        $('#txtTaskName').prop('disabled', true);
        CKEDITOR.instances['txtNote'].setReadOnly(true);
        $('#selectApplication').prop('disabled', true);

        GetTaskJson(T_ID, TD_ID)
    } else {
        $("#AddOrEditTasksModal .modal-title").text("Create Task");
        $(".btnAddOrEdit").text("CREATE");
        $('#txtTaskName').prop('disabled', false);
        CKEDITOR.instances['txtNote'].setReadOnly(false);
        $('#selectApplication').prop('disabled', false);
        
    }

    $('#AddOrEditTasksModal').modal({
        backdrop: true
    });
    $('#AddOrEditTasksModal').modal('show');
    $('html, body').css('overflow', 'hidden');
    $('.select2 .select2-selection').css('height', '38px');
    $('.select2-container--default .select2-selection--single .select2-selection__rendered').css('line-height', '38px');

}
function ResetAddOrEditTasksModal() {

    $('.select2 .select2-selection').css('height', '28px');
    $('.select2-container--default .select2-selection--single .select2-selection__rendered').css('line-height', '28px');

    $("#selectApplication").val("");
    $("#txtTaskName").val("");
    $("#hiddenT_ID").val("");
    $("#modalBuildCode_1").attr("data-value", "");
    $("#modalBuildCode_1").text("[Select option]");

    $('html, body').css('overflow', '');

    CKEDITOR.instances['txtNote'].setData('');
    CKEDITOR.instances['txtDetails'].setData('');
    var index = 0;
    $("#tblItemDetail tbody tr").each(function () {
        if (index < 2) {
            // Reset the first two rows
            $(this).find("select, input[type='text'], input[type='date']").val("");
            $(this).find(".lblTaskAttachments").text("0");
            $(this).find(".txtTaskAttachments").val(null);
            $(this).find(".txtHiddenTaskDetail").val(null);
            $(this).find(".txtTaskDetail").val(null);
            $(this).find(".txtTaskDetail").attr("placeholder", "Add");
            $(this).find(".txtTD_ID").text("0");
            $('#selectCategory_1').val('').trigger('change');
            $('#selectPriority_1').val('').trigger('change');
            $('#selectStatus_1').val('').trigger('change');
            $('#selectLeadAssignTo_1').val('').trigger('change');
            $('#selectApplicationURL_1').val('').trigger('change');
            $('#txtCustomPageName_1').val('');
           
            ResetFormData(formData);
        } else {
            // Remove rows after the first two
            $(this).remove();
        }
        index += 1;
    });
    $(".btnRemoveRow").hide();
    Attachments = [];

}

var formData = new FormData();
function AddOrEditDynamicTasks() {
    var T_ID = $("#hiddenT_ID").val();
    if (T_ID === "" || isNaN(T_ID)) {
        T_ID = 0;
    }
    var applicationID = parseInt($("#selectApplication").val())
    if (applicationID == "" || isNaN(applicationID) || applicationID == null) {
        applicationID = 0;
    }
    if (applicationID == 0) {
        toastr.error('Please Select Application');
        return;
    }
    var note = CKEDITOR.instances['txtNote'].getData();


    var getTaskItemJson = GetTaskItemJson();
    if (getTaskItemJson.length == 0) {
        return true;
    }

    var ObjJson = {
        T_ID: parseInt(T_ID),
        Application_ID: applicationID,
        TaskName: $("#txtTaskName").val(),
        Note: note,
        Active: true,
        TaskItems: getTaskItemJson,
    };

    // Create FormData object

    formData.append('jsonData', JSON.stringify(ObjJson));


    requiredFields = ['Application_ID', 'TaskName'];
    if (!validateRequiredFields(ObjJson, requiredFields)) {
        return;
    }

    // Hide modal
    $("#AddOrEditTasksModal").modal('hide');
    $("#PageLoader").show();
    $('html, body').css('overflow', '');
    AddEditAjaxFileUpload(formData, '/TMS/AddOrEdit_TMS', TMS_Load_Data, undefined)

}
var appendSelectOptionNo = 0;
function LoadBuildCodeNameForCardView(P_ID,element) {
    if ((P_ID == "" && RightsList.IsTaskDetailListAdd == true) || (P_ID !== "" && RightsList.IsTaskDetailListEdit == true)) {
        appendSelectOptionNo = 0;
        var selecRowId = element.id;
        if (selecRowId !== null || selecRowId !== undefined || selecRowId !== "") {
            appendSelectOptionNo = selecRowId.split('_')[1];
            $("#modalBuildCode_" + appendSelectOptionNo).attr("data-value", "");
            $("#modalBuildCode_" + appendSelectOptionNo).text("[Select option]");
        }
        GetResponseAjaxCommon(P_ID, "/TMS/GetBuildCodeNameDropdown", HandleSelectOptionForCardView);
    } else {
        WarningMessage('No Rights', "You Don't Have Rights To Access.", true, "Ok");
        $("#PageLoader").hide();
        return;
    }
}
function HandleSelectOptionForCardView(Response) {
    var options = '<div class="value" data-value="" onclick="DynamicSelectOption(this)">[Select option]</div>';
    for (var i = 0; i < Response.length; i++) {
        options += '<div class="value" data-value="' + Response[i].code + '" onclick="DynamicSelectOption(this)">' + Response[i].name + '</div>';
    }
    $('#selectoption_' + appendSelectOptionNo).html(options);
}
function AddEditAjaxFileUpload(FromData, url, successCallback, refreshgridid = undefined, callbackParams = []) {
    $.ajax({
        url: url,
        type: 'POST',
        data: FromData,
        processData: false,
        contentType: false,
        success: function (response) {

            var res = JSON.parse(response);
            if (response == "Session is Expired") {
                SessionMessage(true);
                $("#PageLoader").hide();
                return;
            }
            if (res.ReturnCode === true) {
                if (successCallback && typeof successCallback === 'function') {
                    resetmodal('dynamic-modal1');

                    if (callbackParams && callbackParams.length > 0) {
                        successCallback.apply(null, callbackParams);
                        toastr.success(res.ReturnText);
                    } else {
                        successCallback();
                        toastr.success(res.ReturnText);
                    }
                }
                else {
                    resetmodal('dynamic-modal1');
                    RefreshGridData(refreshgridid);
                    toastr.success(res.ReturnText);
                }
            } else {
                toastr.error(res.ReturnText);
            }
            $("#PageLoader").hide();
            ResetFormData(FromData)
        },
        failure: function (response) {
            $("#PageLoader").hide();
            ResetFormData(FromData)
        },
        error: function (response) {
            $("#PageLoader").hide();
            ResetFormData(FromData)
        }

    });
}

function ResetFormData(removeFormData) {
    for (var pair of removeFormData.keys()) {
        removeFormData.delete(pair);
    }
    Attachments = [];
}

function GetTaskItemJson() {
    var TaskItems = [];

    var index = 0;
    $("#tblItemDetail tbody tr").each(function (index) {
        if (index > 0) {
            var RowNo = parseInt($(this).children()[1].textContent);
            var TD_ID = $(this).children()[10].textContent;
            if (TD_ID.trim() === "") {
                TD_ID = 0;
            } else {
                TD_ID = parseInt(TD_ID);
            }
            var applicationURLID = 0
            var pageNameValue = $(this).children()[8].firstElementChild.value;
            if (pageNameValue.trim() != "") {
                applicationURLID = parseInt(pageNameValue)
            }
            var children = $(Row_1).children();
            var grandchildren = children.children();

            var taskCategoryMTVID = 0
            var checkTaskCategoryValue = $(this).children()[2].firstElementChild.value;
            if (checkTaskCategoryValue != null && checkTaskCategoryValue.trim() !== "") {
                taskCategoryMTVID = parseInt(checkTaskCategoryValue);
            }

            var priorityMTVCode = 0
            var checkPriorityMTVCodeValue = $(this).children()[6].firstElementChild.value
            if (checkPriorityMTVCodeValue != null && checkPriorityMTVCodeValue.trim() !== "") {
                priorityMTVCode = parseInt(checkPriorityMTVCodeValue);
            }

            var statusMTVCode = 0;
            var checkStatusMTVCodeValue = $(this).children()[7].firstElementChild.value;
            if (checkStatusMTVCodeValue != null && checkStatusMTVCodeValue.trim() !== "") {
                statusMTVCode = parseInt(checkStatusMTVCodeValue);
            }
            if (taskCategoryMTVID == 0) {

                toastr.error("Please select Task Category of Row No " + RowNo);
                TaskItems = []

                return true
            }
            if (priorityMTVCode == 0) {

                toastr.error("Please select Priority Row No " + RowNo);
                TaskItems = []
                return true
            }

            if (statusMTVCode == 0) {

                toastr.error("Please select Status Row No " + RowNo);
                TaskItems = []
                return true
            }


            // var currentTime = new Date().toTimeString().split(' ')[0];
            var taskItem = {
                RowNo: RowNo,
                TD_ID: TD_ID,
                TaskCategory_MTV_ID: taskCategoryMTVID,
                TaskItemName: $(this).children()[3].firstElementChild.value,
                StartDate: $(this).children()[4].firstElementChild.firstElementChild.value,
                EndDate: $(this).children()[5].firstElementChild.firstElementChild.value,
                Priority_MTV_Code: priorityMTVCode,
                Status_MTV_Code: statusMTVCode,
                Application_URL: applicationURLID,
                //BUILDCODE: $(this).children()[11].find('.dynamic-select-selected').attr('data-value'),
                BUILDCODE: $(this).children().eq(11).find('.dynamic-select-selected').attr('data-value'),
                Review_Date: $(this).children()[12].firstElementChild.firstElementChild.value,
                ETA_Date: $(this).children()[13].firstElementChild.firstElementChild.value,
                LeadAssignTo: $(this).children()[14].firstElementChild.value,
                CustomPageName: $(this).children()[15].firstElementChild.value,
                TaskDetail: $(this).children()[17].firstElementChild.value,
                IsPrivate: false,
                Active: true,
                Attachments: []
            };
            console.log(taskItem);

            if (Attachments.length > 0) {
                for (var i = 0; i < Attachments.length; i++) {
                    var attachment = Attachments[i];
                    var AttachRowNo = attachment.RowNo;
                    var FileName = attachment.FileName;
                    var FileExt = attachment.FileExt;
                    var OriginalFileName = attachment.OriginalFileName;
                    var File = attachment.File;


                    var AttachObj = {
                        RowNo: AttachRowNo,
                        TA_ID: 0,
                        OriginalFileName: OriginalFileName,
                        FileName: FileName,
                        FileExt: FileExt,
                        Path: "",
                        DocumentType_MTV_ID: 178100,
                        AttachmentType_MTV_ID: 179100,
                        Active: true,
                        REFID1: 0,
                        REFID2: TD_ID,
                        REFID3: 0,
                        REFID4: 0
                    }

                    if (RowNo === AttachRowNo) {
                        taskItem.Attachments.push(AttachObj);
                    }
                }
            }

            TaskItems.push(taskItem);
        }
        index += 1
    });

    return TaskItems;
}
function RemoveTasks(T_ID, TD_ID) {
    if (RightsList.IsDelete == false) {
        WarningMessage('No Rights', "You Don't Have Rights To Access.", true, "Ok");
        $("#PageLoader").hide();
        return;
    }
    $('#modal-confirmation-message').html('');
    var message = "<h3 class='text-center'>Are you sure you want to Remove Task?</h3>";
    $('#modal-confirmation-message').append(message);
    $('#modal-confirmation-div').modal({
        backdrop: false
    });
    $('#modal-confirmation-yes').removeAttr('disabled');
    $('#modal-confirmation-div').modal('show');
    $('#modal-confirmation-yes').off('click').on('click', function () {
        var ObjJson = new Object();
        ObjJson.T_ID = T_ID;
        ObjJson.TD_ID = TD_ID;
        var JsonData = JSON.stringify(ObjJson);
        requiredFields = ['T_ID', 'TD_ID'];
        if (!validateRequiredFields(ObjJson, requiredFields)) {
            return;
        }
        AddEditRemoveAjaxCommon(JsonData, '/TMS/Remove_TMS', TMS_Load_Data, false, undefined);
    });
}

function GetTaskJson(T_ID, TD_ID) {
    if (T_ID != "" && TD_ID != "") {
        var ObjJson = new Object();
        ObjJson.T_ID = T_ID;
        ObjJson.TD_ID = TD_ID;
        //var JsonData = JSON.stringify(ObjJson);
        $("#PageLoader").show();
        GetResponseAjaxCommon(ObjJson, "/TMS/GetTasksDetails_Json", putTaskDataFromJson);
    }

}
function putTaskDataFromJson(json) {
    $("#PageLoader").hide();
    var jsonObject = $.parseJSON(json);
    var taskItems = jsonObject.hasOwnProperty("TaskItems") && jsonObject.TaskItems.length > 0 ? jsonObject.TaskItems : null;

    if (jsonObject != null) {
        $('#txtTaskName').val(jsonObject.TaskName);
        CKEDITOR.instances['txtNote'].setData(jsonObject.Note);
        $('#selectApplication').val(jsonObject.Application_ID);
        $('#hiddenT_ID').val(jsonObject.T_ID)

    }
    if (taskItems != null || taskItems != undefined) {
        for (var taskCount = 0; taskCount < taskItems.length; taskCount++) {
            var rowCount = taskCount + 1;
            var attachment = taskItems[taskCount].hasOwnProperty("Attachments") && taskItems[taskCount].Attachments.length > 0 ? taskItems[taskCount].Attachments : null;
            var attachmentCount = 0;
            if (attachment != null) {
                attachmentCount = attachment.length
            }
            //$('#selectCategory_' + rowCount).val(taskItems[taskCount].TaskCategory).select
            $('#selectCategory_' + rowCount).val(taskItems[taskCount].TaskCategory).trigger('change');

            $('#txtItemName_' + rowCount).val(taskItems[taskCount].Item)
            $('#txtStartDate_' + rowCount).val(taskItems[taskCount].Task_Start_Date)
            $('#txtEndDate_' + rowCount).val(taskItems[taskCount].Task_End_Date)
            $('#selectPriority_' + rowCount).val(taskItems[taskCount].Priority_MTV_Code).trigger('change');
            $('#selectStatus_' + rowCount).val(taskItems[taskCount].Status_MTV_Code).trigger('change');
            $('#selectApplicationURL_' + rowCount).val(taskItems[taskCount].Application_URL).trigger('change');
            $('#txtTaskDetail_' + rowCount).attr("placeholder", "View");
            $('#txtHiddenTaskDetail_' + rowCount).val(taskItems[taskCount].Item_Detail)
            $('#txtTD_ID_' + rowCount).text(taskItems[taskCount].TD_ID)
            $('#lblTaskAttachments_' + rowCount).text(attachmentCount)
            $('#txtBuildCode_' + rowCount).val(taskItems[taskCount].BUILDCODE)
            $('#txtReviewDate_' + rowCount).val(taskItems[taskCount].Review_Date)
            $('#ETADate_' + rowCount).val(taskItems[taskCount].ETA_Date)
            $('#selectLeadAssignTo_' + rowCount).val(taskItems[taskCount].LeadAssignTo).trigger('change');
            $("#modalBuildCode_1").attr("data-value", "" + taskItems[taskCount].BUILDCODE +"");
            $("#modalBuildCode_1").html(taskItems[taskCount].BUILDCODEName);



            // if (task != taskItems.length) {
            //     AddNewRowItemDetail();
            // }
        }
    }

}

function AddOrEditTaskDetails() {
    if (RightsList.IsAdd == false || RightsList.IsEdit == false) {
        WarningMessage('No Rights', "You Don't Have Rights To Access.", true, "Ok");
        $("#PageLoader").hide();
        return; s
    }
    var getDetailsText = String(CKEDITOR.instances['txtDetails'].getData());
    $('#' + detailsId).attr("placeholder", "View");
    var numericPart = detailsId.substring("txtTaskDetail_".length);
    var hiddenDetailsId = "txtHiddenTaskDetail_" + numericPart;
    $('#' + hiddenDetailsId).val(getDetailsText);
    $("#DetailsModal").modal('hide');
}
// Tasks Function End

// Task Item Detail Function Start

function findMaxTextInTd(tableId, tdClass) {

    var maxInt = -Infinity;


    $('#' + tableId + ' td.' + tdClass).each(function () {

        var text = parseInt($(this).text().trim());

        if (!isNaN(text) && text > maxInt) {
            maxInt = text;
        }
    });

    // Return the maximum integer found
    return maxInt;
}

function AddNewRowItemDetail(this_) {
    totalRows = $('#tblItemDetail tbody tr').length;
    const newRow = $('#tblItemDetail #Row_0').clone();
    var rowCounter = findMaxTextInTd('tblItemDetail', 'rowNo');
    rowCounter += 1;
    newRow.attr('id', 'Row_' + rowCounter);
    newRow.find('#txtRowNumber_0').text(totalRows);
    newRow.find('[id^="btnAddNewRow_"]').attr('id', 'btnAddNewRow_' + rowCounter);
    newRow.find('[id^="btnRemoveRow_"]').attr('id', 'btnRemoveRow_' + rowCounter);
    newRow.find('[id^="txtRowNumber_"]').attr('id', 'txtRowNumber_' + rowCounter);
    newRow.find('[id^="selectCategory_"]').attr('id', 'selectCategory_' + rowCounter);
    newRow.find('[id^="txtItemName_"]').attr('id', 'txtItemName_' + rowCounter);
    newRow.find('[id^="txtStartDate_"]').attr('id', 'txtStartDate_' + rowCounter);
    newRow.find('[id^="txtEndDate_"]').attr('id', 'txtEndDate_' + rowCounter);
    newRow.find('[id^="selectPriority_"]').attr('id', 'selectPriority_' + rowCounter);
    newRow.find('[id^="selectStatus_"]').attr('id', 'selectStatus_' + rowCounter);
    newRow.find('[id^="selectApplicationURL_"]').attr('id', 'selectApplicationURL_' + rowCounter);
    newRow.find('[id^="lblTaskAttachments_"]').attr('id', 'lblTaskAttachments_' + rowCounter);
    newRow.find('[id^="txtTaskAttachments_"]').attr('id', 'txtTaskAttachments_' + rowCounter);
    newRow.find('[id^="txtTaskDetail_"]').attr('id', 'txtTaskDetail_' + rowCounter);
    newRow.find('[id^="txtHiddenTaskDetail_"]').attr('id', 'txtHiddenTaskDetail_' + rowCounter);
    newRow.find('[id^="txtReviewDate_"]').attr('id', 'txtReviewDate_' + rowCounter);
    newRow.find('[id^="ETADate_"]').attr('id', 'ETADate_' + rowCounter);
    newRow.find('[id^="txtTD_ID_"]').attr('id', 'txtTD_ID_' + rowCounter);
    newRow.find('[id^="selectoption_"]').attr('id', 'selectoption_' + rowCounter);
    newRow.find('[id^="modalBuildCode_"]').attr('id', 'modalBuildCode_' + rowCounter);
    newRow.find('[id^="dynamic-custom-select_"]').attr('id', 'dynamic-custom-select_' + rowCounter);
    newRow.find('[id^="selectLeadAssignTo_"]').attr('id', 'selectLeadAssignTo_' + rowCounter);
    newRow.find('[id^="txtCustomPageName_"]').attr('id', 'txtCustomPageName_' + rowCounter);
    newRow.css('display', 'table-row');

    $('#tblItemDetail tbody').append(newRow);
    $("#Row_0").hide();
    if (rowCounter > 1) {
        $("#tblItemDetail tbody tr .btnRemoveRow").show();
    }

    var CategorySelect = $("#selectCategory_" + rowCounter);
    if (CategorySelect.data('select2')) {
        CategorySelect.select2('destroy');
    }
    CategorySelect.select2();

    var prioritySelect = $("#selectPriority_" + rowCounter);
    if (prioritySelect.data('select2')) {
        prioritySelect.select2('destroy');
    }
    prioritySelect.select2();

    var statusSelect = $("#selectStatus_" + rowCounter);
    if (statusSelect.data('select2')) {
        statusSelect.select2('destroy');
    }
    statusSelect.select2();

    var applicationSelect = $("#selectApplicationURL_" + rowCounter);
    if (applicationSelect.data('select2')) {
        applicationSelect.select2('destroy');
    }
    applicationSelect.select2();

    var selectLeadAssignTo = $("#selectLeadAssignTo_" + rowCounter);
    if (selectLeadAssignTo.data('select2')) {
        selectLeadAssignTo.select2('destroy');
    }
    selectLeadAssignTo.select2();


    $("#txtStartDate_" + rowCounter).kendoDatePicker({
        changeMonth: true,
        changeYear: true,
        altFormat: "dd/mm/yyyy",
    });

    $("#txtEndDate_" + rowCounter).kendoDatePicker({
        changeMonth: true,
        changeYear: true,
        altFormat: "dd/mm/yyyy",
    });

    $("#txtReviewDate_" + rowCounter).kendoDatePicker({
        changeMonth: true,
        changeYear: true,
        altFormat: "dd/mm/yyyy",
    });
    $("#ETADate_" + rowCounter).kendoDatePicker({
        changeMonth: true,
        changeYear: true,
        altFormat: "dd/mm/yyyy",

    });

    $('.select2 .select2-selection').css('height', '38px');
    $('.select2-container--default .select2-selection--single .select2-selection__rendered').css('line-height', '38px');
}
function RemoveRowItemDetail(this_) {
    if (!RightsList.IsDelete) {
        WarningMessage('No Rights', "You Don't Have Rights To Access.", true, "Ok");
        $("#PageLoader").hide();
        return;
    }
    if ($("#" + this_.id).attr("disabled") == "disabled") {
        return;
    }
    $("#" + this_.id).attr("disabled", "disabled");
    $('#PageLoader').show();

    var id = this_.id;
    var replaceID = id.replace('btnRemoveRow_', '');
    const removedID = parseInt(replaceID);
    $('#' + id).closest('tr').remove();
    totalRows -= 1;
    if (totalRows <= 1) {
        $(".btnRemoveRow").hide();
    }
    resetRowNumbers(removedID);
    removeDeletedRowFile(removedID)

    $('#PageLoader').hide();
    $("#" + this_.id).removeAttr("disabled");
}
function resetRowNumbers(removedID) {
    const rows = $('[id^="txtRowNumber_"]');
    for (let index = 0; index < rows.length; index++) {
        const currentId = parseInt(rows[index].id.split('_')[1]);
        if ($(rows[index]).text() !== '') {
            const value = parseInt($(rows[index]).text());
            if (value >= removedID) {
                const newId = value - 1;
                $(rows[index]).text(newId);
                // If you want to update other IDs based on this change, do it here.
            }
        }
    }
}

// Task Item Detail Function End

// Members Function Start
function GetMemebers(TD_ID) {
    $("#txtTD_ID").val(TD_ID);
    GetResponseAjaxCommon(TD_ID, "/TMS/Get_TMS_Memebers_List", handleTaskMemebersResponse);

    var memebersModal = $('#AddOrEditMembers');
    if (!memebersModal.hasClass('show')) {
        memebersModal.modal({
            backdrop: false
        });
        memebersModal.modal('show');
    }
}
function handleTaskMemebersResponse(res) {
    $("#selectAssignToTypeGrid").val("")
    $("#selectMemebers").val("")

    $(".memeber_box").html('');
    res.forEach(function (item) {
        var html = Get_TMS_Memebrs_Hmtl(item);
        $(".memeber_box").append(html);
    });
}
function Get_TMS_Memebrs_Hmtl(item) {
    var html = '';
    html = "<li class='list-group-item py-3 text-center text-md-start'>";
    html += "<div class='d-flex align-items-center flex-column flex-sm-column flex-md-column flex-lg-row'>";
    html += "<div class='flex-fill ms-3 text-truncate'>";
    html += "<h6 class='mb-0 fw-bold'>" + item.AssignedTo + "</h6>";
    html += "<span class='text-muted'>" + item.Email + "</span>";
    html += "</div>";
    html += "<div class='members-action'>";
    html += "<span class='members-role'>" + item.RoleName + "</span>";
    html += "<div class='btn-group'>";
    html += "<a href='#' class='btn bg-transparent dropdown-toggle' data-bs-toggle='dropdown' aria-expanded='false'>";
    html += "<i class='fa fa-settings fs-6'></i>";
    html += "</a>";
    html += "<ul class='dropdown-menu dropdown-menu-end'>";
    html += "<li onclick='RemoveMemebers(" + item.TATM_ID + ", " + item.TD_ID + ");'><a class='dropdown-item' href='#'><i class='fa fa-trash fs-6 me-2'></i>Remove Memeber</a></li>";
    html += "</ul>";
    html += "</div>";
    html += "</div>";
    html += "</div>";
    html += "</li>";
    return html;
}
function AddMemebers() {
    var ObjJson = new Object();
    ObjJson.Decrypted_TD_ID = $("#txtTD_ID").val();
    ObjJson.AssignToType_MTV_CODE = $("#selectAssignToTypeGrid option:selected").val();
    ObjJson.AssignedTo = $("#selectMemebers option:selected").val();
    ObjJson.Active = true;
    var JsonData = JSON.stringify(ObjJson);
    requiredFields = ['Decrypted_TD_ID', 'AssignedTo','AssignToType_MTV_CODE'];
    if (!validateRequiredFields(ObjJson, requiredFields)) {
        return;
    }
    var funcParms = [ObjJson.Decrypted_TD_ID];
    AddEditRemoveAjaxCommon(JsonData, '/TMS/AddTaskAssignedToMapFromCardView', GetMemebers, false, undefined, funcParms);
}

function GetAssignToUserForCardView(AssignToType_MTV_CODE) {
    if ((AssignToType_MTV_CODE == "" && RightsList.IsTaskAssignedToListAdd == true) || (AssignToType_MTV_CODE !== "" && RightsList.IsTaskAssignedToListEdit == true)) {
        {
            GetDropDownListCommonAjax(AssignToType_MTV_CODE, "/TMS/GetAssignToDropdown", "selectMemebers");
        }

    } else {
        WarningMessage('No Rights', "You Don't Have Rights To Access.", true, "Ok");
        $("#PageLoader").hide();
        return;
    }
}

function RemoveMemebers(TATM_ID, TD_ID) {
    var funcParms = [TD_ID];
    AddEditRemoveAjaxCommon(JSON.stringify(TATM_ID), "/TMS/Remove_TaskAssignedToMap", GetMemebers, false, undefined, funcParms);
}
// Members Function End

// Attachments Function Start
var Attachments = [];
var idenfiedAttachmetNo = 1;
function handleFileInputChange(this_) {
    var fileInput = this_;

    var fileLabel = this_.previousElementSibling;
    var previousCount = parseInt(fileLabel.textContent)
    var files = $("#" + this_.id).prop('files');
    var rowNoID = this_.id.split('_')[1];
    var files1 = fileInput.files[0];
    //var rowNo = parseInt($(this_).parent().parent().siblings()[1].textContent);
    var rowNo = parseInt($('#txtRowNumber_' + rowNoID).text())
    var fileCount = files.length;
    // var validExtensions = ['jpg', 'jpeg', 'png', 'gif'];
    var totalAttachmentCount = previousCount + fileCount


    var fileInput = $("#" + this_.id)[0]; // Assuming file input has id 'txtTaskAttachments_1'
    if (fileInput.files.length > 0) {
        for (var i = 0; i < fileInput.files.length; i++) {
            var modifiedFile = files[i];
            var modifiedFileExt = getFileExtension(modifiedFile.name);
            var fileNameWithoutExt = getFileNameWithoutExtension(modifiedFile.name)
            var newFileName = fileNameWithoutExt + '_' + rowNo + '.' + modifiedFileExt;
            formData.append('files', fileInput.files[i], newFileName);
        }
    }
    if (fileCount > 0) {
        var count = 0;
        for (let i = 0; i < fileInput.files.length; i++) {
            var file = files[i];
            var fileExtension = "";
            fileExtension = getFileExtension(file.name);;
            var NameWithoutExt = getFileNameWithoutExtension(file.name)
            var newFileName = NameWithoutExt + '_' + rowNo + '.' + fileExtension;

                const attachment = {
                    RowNo: rowNo,
                    FileName: "",
                    OriginalFileName: file.name,
                    FileExt: fileExtension,
                    NewFileName: newFileName
                };
                Attachments.push(attachment);
             
        }
    }
    fileLabel.textContent = totalAttachmentCount;
}


function removeDeletedRowFile(rowNo) {
    // Filter Attachments to get files associated with the given row number
    var filesToRemove = Attachments.filter(attachment => attachment.RowNo === rowNo);

    // Get files from FormData
    var filesinFormData = formData.getAll('files');
     
    for (var i = 0; i < filesToRemove.length; i++) {
        var fileToRemove = filesToRemove[i];

        // Removing from Attachments
        var indexAttachment = Attachments.findIndex(attachment => attachment.RowNo === rowNo);
        if (indexAttachment !== -1) {
            Attachments.splice(indexAttachment, 1);
        }

        // Removing from filesinFormData
        var indexFormData = filesinFormData.findIndex(file => file.name === fileToRemove.NewFileName);
        if (indexFormData !== -1) {
            filesinFormData.splice(indexFormData, 1);
        }
    }


    // Update FormData with modified files list
    formData.delete('files');
    filesinFormData.forEach(file => {
        formData.append('files', file);
    });


}


function AttachmentFileCount(this_) {
    var fileInput = this_.querySelector('.txtTaskAttachments');
    fileInput.click();
}
function GetAttachmnets(T_ID, TD_ID) {
    if (TD_ID !== "") {
        $('#txtmodalTD_ID').text(TD_ID);
        $("#txtmodalT_ID").text(T_ID);
        GetResponseAjaxCommon(TD_ID, "/TMS/Get_TMS_Attachment_List", handleAttachementsResponse);
        $('#imageGallery').empty();
        $("#PageLoader").show();
    }
}
function handleAttachementsResponse(res) {
    //$(".memeber_box").html('');
    var modalTD_ID = 0;


    res.forEach(function (item) {
        // Call the function to display the image
        modalTD_ID = item.Encrypted_TD_ID;
        AttachmentsCard(item.OriginalFileName, item.FileExt, item.FileName, item.Encrypted_TA_ID, item.Encrypted_TD_ID, item.FullPath);
    });
    $("#PageLoader").hide();
    //$('#txtmodalTD_ID').text(modalTD_ID);

    var attachmentsModal = $('#AttachmentModal');
    if (!attachmentsModal.hasClass('show')) {
        attachmentsModal.modal({
            backdrop: false
        });
        attachmentsModal.modal('show');
        $('#AttachmentModal').css("display", "block");
        $('html, body').css('overflow', 'hidden');
    }
}
function AttachmentsCard(originalFileName, fileExtension, fileName, TA_ID, TD_ID, filePath) {
    var html = '';
    html += "<div class='col-xl-3 col-lg-3 col-md-3 col-sm-3 gallery-card'>";

    // Checking file extension to determine appropriate icon or preview
    if (fileExtension.toLowerCase() === 'jpg' || fileExtension.toLowerCase() === 'jpeg' || fileExtension.toLowerCase() === 'png' || fileExtension.toLowerCase() === 'gif') {
        // If image file
        html += "<a href='" + filePath + "' target='_blank'  >";
        html += "<img src='" + filePath + "' />";
        html += "</a>";
    } else if (fileExtension.toLowerCase() === 'pdf') {
        // If PDF file
        html += "<a href='" + filePath + "' target='_blank'  >";
        html += "<img src='../icon/pdf-icon.png' />";
        html += "</a>";
    } else if (fileExtension.toLowerCase() === 'xls' || fileExtension.toLowerCase() === 'xlsx') {
        // If Excel file
        html += "<a href='" + filePath + "' target='_blank'  >";
        html += "<img src='../icon/excel_icon.jpg' />";
        html += "</a>";
    } else if (fileExtension.toLowerCase() === 'doc' || fileExtension.toLowerCase() === 'docx') {
        // If Word file
        html += "<a href='" + filePath + "' target='_blank'  >";
        html += "<img src='../icon/word_icon.png' />";
        html += "</a>";
    } else {
        // For other file types, you can choose to display a generic icon or handle differently
        html += "<a href='" + filePath + "' target='_blank'  >";
        html += "<i class='fa imageiconsize'>&#xf15b;</i>";
        html += "</a>";
    }

    html += "<div class='attachment-info'>";
    html += "<div class='attachment-name'>" + originalFileName + "</div>";
    html += "<div class='closestyle' onclick='RemoveAttachment(\"" + TA_ID + "\",\"" + TD_ID + "\")'><i class='fas'>&#xf00d;</i></div>";
    html += "</div>";
    html += "</div>";

    $('#imageGallery').append(html);
}


function RemoveAttachment(TA_ID, TD_ID) {
    var confirmed = confirm("Are you sure to remove this Attachment?");
    if (!confirmed) {
        return;
    }
    $("#PageLoader").show();
    var T_ID = $('#txtmodalT_ID').text();
    var funcParms = [T_ID, TD_ID];
    AddEditRemoveAjaxCommon(JSON.stringify(TA_ID), "/TMS/RemoveAttachments", GetAttachmnets, false, undefined, funcParms);
}

function handleFileFromModal(element) {
    $("#PageLoader").show();
    var ModalAttachment = [];
    var TD_TD = $('#txtmodalTD_ID').text();
    var T_TD = $('#txtmodalT_ID').text();

    var files = $("#" + element.id).prop('files');
    var fileCount = files.length;
    // var validExtensions = ['jpg', 'jpeg', 'png', 'gif'];
    let count = 0;
    modalAttachementFrom = new FormData();
    var fileInput = $("#" + element.id)[0]; // Assuming file input has id 'txtTaskAttachments_1'
    if (fileInput.files.length > 0) {
        for (var i = 0; i < fileInput.files.length; i++) {
            count++;
            const file = files[i];
            var fileExt = getFileExtension(file.name);
            const attachment = {
                RowNo: count,
                FileName: file.name,
                OriginalFileName: file.name,
                FileExt: fileExt,
                Path: "",
                DocumentType_MTV_ID: 178100,
                AttachmentType_MTV_ID: 179100,
                Active: true,
                REFID1: 0,
                REFID2: 0,
                REFID3: 0,
                REFID4: 0,
                Decrypted_T_ID: T_TD,
                Decrypted_TD_ID: TD_TD
            };
            ModalAttachment.push(attachment);
            var fileNameWithoutExt = getFileNameWithoutExtension(file.name)
            var newFileName = fileNameWithoutExt + '_' + count + '.' + fileExt;
            modalAttachementFrom.append('files', fileInput.files[i], newFileName);

        }
    }


    // var ObjectJson = new Object();
    // ObjectJson.ModalAttachment = ModalAttachment;
    var modalAttachmentJosn = JSON.stringify(ModalAttachment);
    modalAttachementFrom.append("ModalAttachment", modalAttachmentJosn)
    var funcParms = [T_TD, TD_TD];
    AddEditAjaxFileUpload(modalAttachementFrom, '/TMS/AddModalAttachment', GetAttachmnets, undefined, funcParms);



}



// Attachments Function End

// Comments Function Start
function GetCommentsModal(TD_ID) {
    var editor = CKEDITOR.instances.txtComments;
    editor.setData('');
    $("#txtCommentsTD_ID").val(TD_ID);
    GetResponseAjaxCommon(TD_ID, "/TMS/Get_TMS_Comments_List", handleTaskCommentsResponse);

    var commentBox = document.getElementById("commentBox");
    commentBox.scrollTop = commentBox.scrollHeight;

    var commentsModal = $('#CommentsModal');
    if (!commentsModal.hasClass('show')) {
        commentsModal.modal({
            backdrop: false
        });
        commentsModal.modal('show');
    }
}
function handleTaskCommentsResponse(res) {
    $(".CommentsSection").html('');
    res.forEach(function (item) {
        var html = Get_TMS_Comments_Hmtl(item);
        $(".CommentsSection").append(html);
    });
}
function Get_TMS_Comments_Hmtl(item) {
    var backColor = '';
    var textAlignment = '';
    var memebrActive = '';
    if (item.MemeberActive) {
        memebrActive = 'green'
        textAlignment = 'text-right'
        backColor = '#ccf7ff'
    } else {
        memebrActive = 'grey'
        textAlignment = 'text-left'
        backColor = '#f1f1f1'
    }
    var html = '';
    html = "<div class='comment mb-3' style='background:" + backColor + "'>";
    html += "<div class='user-banner'>";
    if (item.MemeberActive) {
        html += "<span>" + item.CommentAgo + "</span>";
    }
    html += "<div class='user'>";
    html += "<div class='avatar' style='background-color: #fff5e9;border-color: #ffe0bd;color: #f98600;'>" + item.ShortCommentBy + "<span class='stat " + memebrActive + "'></span></div>";
    html += "<h5 style='margin-bottom: -2px;'>" + item.CommentBy + "</h5>";
    html += "</div>";
    if (!item.MemeberActive) {
        html += "<span>" + item.CommentAgo + "</span>";
    }
    html += "</div>";
    html += "<div class='content " + textAlignment + "'>";
    html += item.CommentText;
    html += "</div>";
    html += "</div>";
    return html;
}
function AddComments() {
    var editor = CKEDITOR.instances.txtComments;
    var ObjJson = new Object();
    ObjJson.TC_ID = 0;
    ObjJson.Decrypted_TD_ID = $("#txtCommentsTD_ID").val();
    ObjJson.CommentText = editor.getData();
    ObjJson.Active = true;
    var JsonData = JSON.stringify(ObjJson);
    requiredFields = ['Decrypted_TD_ID', 'CommentText'];
    if (!validateRequiredFields(ObjJson, requiredFields)) {
        return;
    }
    var funcParms = [ObjJson.Decrypted_TD_ID];
    AddEditRemoveAjaxCommon(JsonData, "/TMS/AddOrEdit_TaskComments", GetCommentsModal, false, undefined, funcParms);

    editor.setData('');
}
// Comments Function End

// Task Detail Modal Function Start
CKEDITOR.replace('txtDetails', {
    height: 100,
    extraPlugins: 'colorbutton',
    colorButton_colors: 'FF0000,00FF00,0000FF,FFFF00,FF00FF,00FFFF,000000,FFFFFF,808080,C0C0C0,800000,008000,000080,FFFF80,FF80FF,80FFFF,800080,008080,808000,FF8080'
});
var detailsId = "";
function handlesDetailsModal(this_) {
    detailsId = $(this_).attr("id");
    var placeholderDetailsValue = $('#' + detailsId).attr("placeholder");
    var tdId = $('#' + detailsId).closest('td').next('.text-center.d-none').attr('id');
    var checkTD_ID = parseInt($('#' + tdId).text());
    if (checkTD_ID > 0 || placeholderDetailsValue == 'View') {
        $("#DetailsModal .modal-title").text("Update Details");
        $("#btnAddOrEditDetails").text("UPDATE");
        var numericPart = detailsId.substring("txtTaskDetail_".length);
        var hiddenDetailsIdForShow = "txtHiddenTaskDetail_" + numericPart;;
        var getDetailsValue = $('#' + hiddenDetailsIdForShow).val();
        if (getDetailsValue !== null || getDetailsValue !== undefined) {
            CKEDITOR.instances['txtDetails'].setData(getDetailsValue);
        }
    } else {
        $("#DetailsModal .modal-title").text("Add Details");
        $("#btnAddOrEditDetails").text("Add");
        CKEDITOR.instances['txtDetails'].setData('');
    }

    var detailsModal = $('#DetailsModal');
    if (!detailsModal.hasClass('show')) {
        detailsModal.modal({
            backdrop: false
        });
        detailsModal.modal('show');
        $('html, body').css('overflow', 'hidden');
    }
}
// Task Detail Modal Function End

// Common Function Start
function getFileNameWithoutExtension(fileName) {
    return fileName.replace(/\.[^/.]+$/, "");
}
function getFileExtension(fileName) {
    return fileName.split('.').pop();
}

function AttachmentModalClose() {

    $('html, body').css('overflow', '');
}

function dynamicToggleDropdown(element) {
    var $this = $(element);

    var $dropdown = $this.next('.dynamic-select-items');
    $('.dynamic-select-items').not($dropdown).hide();
    $dropdown.toggle();
    $dropdown.css({
        "z-index": "3" , 
        "width": "max-content" ,
        "margin": "1px" 
    });
    $this.toggleClass('select-arrow-active');
}

function DynamicSelectOption(element) {
    var $this = $(element);
    var selected = $this.html();
    var value = $this.data("value");
    var parent = $this.parent();
    parent.prev('.dynamic-select-selected').html(selected);
    parent.prev('.dynamic-select-selected').attr("data-value", value);
    parent.hide();
    parent.prev('.dynamic-select-selected').removeClass('select-arrow-active');

}


// function GetFileFromBase64(file, fileExt, fileName, TA_ID, TD_ID) {
//     const binaryData = atob(file);
//     const arrayBuffer = new ArrayBuffer(binaryData.length);
//     const uint8Array = new Uint8Array(arrayBuffer);
//     for (let i = 0; i < binaryData.length; i++) {
//         uint8Array[i] = binaryData.charCodeAt(i);
//     }
//     const fileBlob = new Blob([uint8Array], { type: `application/${fileExt}` });
//     AttachmentsCard(fileBlob, fileExt, fileName, TA_ID);
// }

//function onCustomFilterValidate(id) {
//    var resultjson = { issuccess: false, customfilterjson: [] };
//    resultjson = onCustomFilter(false, id);
//    if (resultjson.issuccess) {
//        RefreshGridData(id);
//    }
//}
//var lastcustomreportFilterObjectList = [];
//function onCustomFilter(IsGetLastSaved, id) {

//    IsGetLastSaved = (IsGetLastSaved == undefined ? false : IsGetLastSaved);
//    var customreportFilterObjectList = [];
//    var resultjson = { issuccess: false, customfilterjson: customreportFilterObjectList };
//    var reportFilterObject = { code: "", name: "", isFilterApplied: false, reportFilterObjectArry: [] };
//    var filterobject = new Object();

//    var specificreportFilterObjectList = [];
//    var specificreportFilterObject = { code: "", name: "", isFilterApplied: false, reportFilterObjectArry: [] };
//    var specificfilterobject = new Object();

//    if (IsGetLastSaved) {
//        if (lastcustomreportFilterObjectList.length > 0) {
//            for (var i = 0; i <= lastcustomreportFilterObjectList.length - 1; i++) {
//                if (lastcustomreportFilterObjectList[i].id == id) {
//                    resultjson = { issuccess: true, customfilterjson: lastcustomreportFilterObjectList[i].customfilterjson };
//                    break;
//                }
//            }
//        }
//        return resultjson;
//    }

//    if ($('#txtdate').val() != "" && $('#txtenddate').val() != "") {
//        SetCustomFilterValue(customreportFilterObjectList, reportFilterObject, filterobject, "and", $('#txtdate').val(), KendoGridFilterType.isequalorgreather, KendoFilterTypes.Date, SRVTypes.Date, false, 0, "Created_On_Date", "Created_On_Date", false);
//        SetCustomFilterValue(customreportFilterObjectList, reportFilterObject, filterobject, "and", $('#txtenddate').val(), KendoGridFilterType.isequalorless, KendoFilterTypes.Date, SRVTypes.Date, false, 0, "Created_On_Date", "Created_On_Date", true);

//        onSpecificFilter(id, specificreportFilterObjectList, specificreportFilterObject, specificfilterobject, "and", $('#txtdate').val()
//            , KendoGridFilterType.isequalorgreather, KendoFilterTypes.Date, SRVTypes.Date, false, 0, "Created_On_Date", "Created_On_Date", false);
//        onSpecificFilter(id, specificreportFilterObjectList, specificreportFilterObject, specificfilterobject, "and", $('#txtenddate').val()
//            , KendoGridFilterType.isequalorless, KendoFilterTypes.Date, SRVTypes.Date, false, 0, "Created_On_Date", "Created_On_Date", true);
//    }

//    if ($('#txtOrigHub').val() != "") {
//        SetCustomFilterValue(customreportFilterObjectList, reportFilterObject, filterobject, "and", $('#txtOrigHub').val(), KendoGridFilterType.inlistfilter, KendoFilterTypes.String, SRVTypes.UpperString, false, 0, "OrigHub", "OrigHub", false);
//    }

//    if (specificreportFilterObjectList.length > 0) {
//        for (var i = 0; i < specificreportFilterObjectList.length; i++) {
//            customreportFilterObjectList.push(specificreportFilterObjectList[i]);
//        }
//    }

//    if (lastcustomreportFilterObjectList.length > 0) {
//        for (var i = 0; i <= lastcustomreportFilterObjectList.length - 1; i++) {
//            if (lastcustomreportFilterObjectList[i].id == id) {
//                resultjson = { issuccess: true, customfilterjson: lastcustomreportFilterObjectList[i].customfilterjson };
//                break;
//            }
//        }
//    }

//    var isexistscustomfilterjson = false;
//    if (lastcustomreportFilterObjectList.length > 0) {
//        for (var i = 0; i <= lastcustomreportFilterObjectList.length - 1; i++) {
//            if (lastcustomreportFilterObjectList[i].id == id) {
//                var lastcustomreportFilterObject = new Object();
//                lastcustomreportFilterObject.id = id;
//                lastcustomreportFilterObject.customfilterjson = customreportFilterObjectList;
//                lastcustomreportFilterObjectList[i] = lastcustomreportFilterObject;
//                isexistscustomfilterjson = true;
//                break;
//            }
//        }
//    }
//    if (isexistscustomfilterjson == false) {
//        var lastcustomreportFilterObject = new Object();
//        lastcustomreportFilterObject.id = id;
//        lastcustomreportFilterObject.customfilterjson = customreportFilterObjectList;
//        lastcustomreportFilterObjectList.push(lastcustomreportFilterObject);
//    }

//    resultjson = { issuccess: true, customfilterjson: customreportFilterObjectList };

//    return resultjson;
//}
//function onSpecificFilter(id, specificreportFilterObjectList, specificreportFilterObject, specificfilterobject, logic, value, filtertype, fieldtype, srvfieldtype, islist, listtype, code, name, isprevioussamecode) {
//    if (code == 'Created_On_Date') {
//        var dateObject = new Date(value)
//        dateObject.setDate(dateObject.getDate() + (isprevioussamecode == false ? -1 : 1));
//        var newvalue = formatDateToDateTime(dateObject, false);
//        SetCustomFilterValue(specificreportFilterObjectList, specificreportFilterObject, specificfilterobject, logic, newvalue, filtertype, fieldtype, srvfieldtype, islist, listtype, "UTC_Created_On_Date", "UTC_Created_On_Date", isprevioussamecode);
//    }
//}
//function onReset() {
//    $("#dvProg").show();
//    $('#txtdate').val($("#originalstartdate").val());
//    $('#txtOrigHub').html($("#hublist").html());
//    $('#txtClientID').html($("#clientlist").html())
//    //RefreshGridData("ReturnDetails");
//    $("#dvProg").hide();
//}
// Common Function End

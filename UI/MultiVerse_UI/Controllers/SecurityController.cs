using Data.DataAccess;
using Data.Dtos;
using Microsoft.AspNetCore.Mvc;
using MultiVerse_UI.Extensions;
using MultiVerse_UI.Models;
using Newtonsoft.Json;
using OfficeOpenXml;
using Services.AccountServices;
using System.Data;
using static Data.Dtos.CustomClasses;
using static MultiVerse_UI.Models.ModalDtos;
using static MultiVerse_UI.Models.MVCAppEnum;

namespace MultiVerse_UI.Controllers
{
    public class SecurityController : Controller
    {
        #region Controller Constructor
        private IConfiguration _config;
        private IHttpContextAccessor _httpContextAccessor;
        private readonly IAccountService account;
        private PublicClaimObjects? _PublicClaimObjects;
        private readonly string _bodystring = "";
        public SecurityController(IConfiguration config, IHttpContextAccessor httpContextAccessor, IAccountService account)
        {
            this._config = config;
            this._httpContextAccessor = httpContextAccessor;
            this.account = account;
            this._bodystring = StaticPublicObjects.ado.GetRequestBodyString().Result;
            this._PublicClaimObjects = StaticPublicObjects.ado.GetPublicClaimObjects();
        }
        #endregion Controller Constructor

        #region User Setup
        [CustomPageSetup]
        public IActionResult UserSetup()
        {
            bool IsView = StaticPublicObjects.ado.P_Is_Has_Right_From_Username_And_PR_ID_From_Memory(_PublicClaimObjects!.username, RightsList_ID.Page_Setup_View);
            if (IsView)
            {
                bool IsAdd = StaticPublicObjects.ado.P_Is_Has_Right_From_Username_And_PR_ID_From_Memory(_PublicClaimObjects.username, RightsList_ID.User_Setup_Add);
                bool IsEdit = StaticPublicObjects.ado.P_Is_Has_Right_From_Username_And_PR_ID_From_Memory(_PublicClaimObjects.username, RightsList_ID.User_Setup_Edit);
                bool IsDelete = StaticPublicObjects.ado.P_Is_Has_Right_From_Username_And_PR_ID_From_Memory(_PublicClaimObjects.username, RightsList_ID.User_Setup_Delete);
                ViewBag.RightsListObj = new { IsView = IsView, IsAdd = IsAdd, IsEdit = IsEdit, IsDelete = IsDelete };
                ViewBag.RightsList = JsonConvert.SerializeObject(ViewBag.RightsListObj);

                DataSet DS = new DataSet();
                (string Name, object Value)[] ParamsNameValues = new (string, object)[1];
                ParamsNameValues[0] = ("Username", _PublicClaimObjects.username);
                DS = StaticPublicObjects.ado.ExecuteStoreProcedureDS("P_Get_UserSetup_Dropdown_Lists", ParamsNameValues);
                ViewBag.RolesList = JsonConvert.SerializeObject(DS.Tables[0]);
                ViewBag.ApplicationAccessList = JsonConvert.SerializeObject(DS.Tables[1]);
                ViewBag.BlockList = JsonConvert.SerializeObject(DS.Tables[2]);
                ViewBag.UserTypeList = JsonConvert.SerializeObject(DS.Tables[3]);
                return View();
            }
            else
            {
                string ID = "";
                Exception exception = new Exception("You Don't Have Rights To Access User Setup");
                ID = _httpContextAccessor.HttpContext!.Session.SetupSessionError("No Rights", "/Security/UserSetup", "User Setup", exception);
                return Redirect($"/Error/Index?ID={ID}");
            }
        }

        [CheckSessionExpiration]
        [HttpPost]
        public IActionResult GetUsersList([FromBody] ReportParams _ReportParams)
        {
            ReportResponsePageSetup reportResponse = new ReportResponsePageSetup();
            try
            {
                if (StaticPublicObjects.ado.P_Is_Has_Right_From_Username_And_PR_ID_From_Memory(_PublicClaimObjects!.username, RightsList_ID.Page_Setup_View))
                {
                    List<Dynamic_SP_Params> List_Dynamic_SP_Params = new List<Dynamic_SP_Params>();
                    ModalFunctions.GetKendoFilter(ref _ReportParams, ref List_Dynamic_SP_Params, _PublicClaimObjects, true);

                    List<P_Users_Result> ResultList = StaticPublicObjects.ado.P_Get_Generic_List_SP<P_Users_Result>("P_Get_Users_List", ref List_Dynamic_SP_Params);

                    reportResponse.TotalRowCount = ModalFunctions.GetValueFromReturnParameter<long>(List_Dynamic_SP_Params, "TotalRowCount", typeof(long));
                    reportResponse.ResultData = ResultList;
                    reportResponse.response_code = true;
                }
                else
                {
                    reportResponse.TotalRowCount = 0;
                    reportResponse.ResultData = null;
                    reportResponse.response_code = false;
                }
            }
            catch (Exception ex)
            {
                StaticPublicObjects.logFile.ErrorLog(FunctionName: "GetUsersList", SmallMessage: ex.Message, Message: ex.ToString());
                reportResponse.TotalRowCount = 0;
                reportResponse.ResultData = null;
                reportResponse.response_code = false;
            }
            return Globals.GetAjaxJsonReturn(reportResponse);
        }

        [CheckSessionExpiration]
        [HttpPost]
        public string GetAddEditUserModal([FromBody] string Encrypted_USER_ID)
        {
            string HtmlString = "";
            DataSet DS = new DataSet();
            int Modal_ID = Crypto.DecryptNumericToStringWithOutNull(Encrypted_USER_ID);

            bool IsAdd = StaticPublicObjects.ado.P_Is_Has_Right_From_Username_And_PR_ID_From_Memory(_PublicClaimObjects!.username, RightsList_ID.Page_Setup_Add);
            bool IsEdit = StaticPublicObjects.ado.P_Is_Has_Right_From_Username_And_PR_ID_From_Memory(_PublicClaimObjects.username, RightsList_ID.Page_Setup_Edit);
            if ((Modal_ID == 0 && IsAdd == false) || (Modal_ID > 0 && IsEdit == false))
                return "No Rights";

            GetModalDetail getModalDetail = new GetModalDetail();
            List<ModalBodyTypeInfo> List_ModalBodyTypeInfo = new List<ModalBodyTypeInfo>();
            ModalBodyTypeInfo modalBodyTypeInfo = new ModalBodyTypeInfo();
            List<Dynamic_SP_Params> _parms = new List<Dynamic_SP_Params>();

            P_AddOrEdit_User_Response ModalEdit = new P_AddOrEdit_User_Response();
            if (Modal_ID > 0)
            {
                _parms = new List<Dynamic_SP_Params>();
                _parms.Add(new Dynamic_SP_Params { ParameterName = "ModalID", Val = Modal_ID });
                ModalEdit = StaticPublicObjects.ado.P_Get_Generic_SP<P_AddOrEdit_User_Response>("SELECT [User_ID],UserName,Email,TelegramUserName,TelegramID,FirstName,LastName,PasswordHash,PasswordSalt,PasswordExpiryDateTime,UserType_MTV_CODE,BlockType_MTV_CODE,IsApproved,IsTempPassword FROM [dbo].[T_Users] WITH (NOLOCK) WHERE [User_ID] = @ModalID", ref _parms, false);
            }

            _parms = new List<Dynamic_SP_Params>();
            _parms.Add(new Dynamic_SP_Params { ParameterName = "MT_ID", Val = 1 });
            List<SelectDropDownList> UserTypeList = StaticPublicObjects.ado.Get_DropDownList_Result("SELECT code = MTV_CODE, [name] = [Name] FROM [dbo].[T_Master_Type_Value] WITH (NOLOCK) WHERE MT_ID = @MT_ID ORDER BY [Sort_]", _parms);

            _parms = new List<Dynamic_SP_Params>();
            _parms.Add(new Dynamic_SP_Params { ParameterName = "MT_ID", Val = 2 });
            List<SelectDropDownList> BlockTypeList = StaticPublicObjects.ado.Get_DropDownList_Result("SELECT code = MTV_CODE, [name] = [Name] FROM [dbo].[T_Master_Type_Value] WITH (NOLOCK) WHERE MT_ID = @MT_ID ORDER BY [Sort_]", _parms);

            List<SelectDropDownList> ApplicationList = StaticPublicObjects.ado.Get_DropDownList_Result("SELECT code = App_ID, [name] = [App_Name] FROM [dbo].[T_Application] WITH (NOLOCK)");
            List<SelectDropDownList> RoleList = StaticPublicObjects.ado.Get_DropDownList_Result("SELECT code = R_ID, [name] = [RoleName] FROM [dbo].[T_Roles] WITH (NOLOCK)");

            getModalDetail.getmodelsize = GetModalSize.modal_lg;
            getModalDetail.modaltitle = (Modal_ID == 0 ? "Add New User" : "Edit User");
            getModalDetail.modalfootercancelbuttonname = "Cancel";
            getModalDetail.modalfootersuccessbuttonname = (Modal_ID == 0 ? "Add" : "Update");
            getModalDetail.modalBodyTypeInfos = new List<ModalBodyTypeInfo>();

            getModalDetail.onclickmodalsuccess = "AddOrEditUser()";

            modalBodyTypeInfo = new ModalBodyTypeInfo();
            modalBodyTypeInfo.ModelBodyType = GetModelBodyType.TRInput;
            modalBodyTypeInfo.GetInputTypeString = GetInputStringType.text;
            modalBodyTypeInfo.LabelName = "User ID";
            modalBodyTypeInfo.PlaceHolder = "User ID";
            modalBodyTypeInfo.id = "User_ID";
            modalBodyTypeInfo.IsRequired = true;
            if (ModalEdit.User_ID > 0)
            {
                modalBodyTypeInfo.value = ModalEdit.User_ID;
            }
            modalBodyTypeInfo.AttributeList = new List<AttributeList>();
            modalBodyTypeInfo.AttributeList.Add(new AttributeList { Name = "readonly", Value = "readonly" });
            if (ModalEdit.User_ID > 0)
                List_ModalBodyTypeInfo.Add(modalBodyTypeInfo);

            modalBodyTypeInfo = new ModalBodyTypeInfo();
            modalBodyTypeInfo.ModelBodyType = GetModelBodyType.TRInput;
            modalBodyTypeInfo.GetInputTypeString = GetInputStringType.text;
            modalBodyTypeInfo.ClassName = "form-control";
            modalBodyTypeInfo.LabelName = "User Name";
            modalBodyTypeInfo.PlaceHolder = "User Name";
            modalBodyTypeInfo.id = "UserName";
            modalBodyTypeInfo.IsRequired = true;
            if (ModalEdit.UserName != "")
            {
                modalBodyTypeInfo.value = ModalEdit.UserName;
            }
            modalBodyTypeInfo.AttributeList = new List<AttributeList>();
            modalBodyTypeInfo.AttributeList.Add(new AttributeList { Name = "onfocus", Value = "validate(this);" });
            modalBodyTypeInfo.AttributeList.Add(new AttributeList { Name = "onkeydown", Value = "validate(this);" });
            modalBodyTypeInfo.AttributeList.Add(new AttributeList { Name = "onchange", Value = "validate(this);" });
            modalBodyTypeInfo.AttributeList.Add(new AttributeList { Name = "autocomplete", Value = "off" });
            List_ModalBodyTypeInfo.Add(modalBodyTypeInfo);

            modalBodyTypeInfo = new ModalBodyTypeInfo();
            modalBodyTypeInfo.ModelBodyType = GetModelBodyType.TRInput;
            modalBodyTypeInfo.GetInputTypeString = GetInputStringType.text;
            modalBodyTypeInfo.ClassName = "form-control";
            modalBodyTypeInfo.LabelName = "Password";
            modalBodyTypeInfo.PlaceHolder = "Password";
            modalBodyTypeInfo.id = "Password";
            modalBodyTypeInfo.IsRequired = true;
            modalBodyTypeInfo.value = "";
            modalBodyTypeInfo.AttributeList = new List<AttributeList>();
            modalBodyTypeInfo.AttributeList.Add(new AttributeList { Name = "onfocus", Value = "validate(this);" });
            modalBodyTypeInfo.AttributeList.Add(new AttributeList { Name = "onkeydown", Value = "validate(this);" });
            modalBodyTypeInfo.AttributeList.Add(new AttributeList { Name = "onchange", Value = "validate(this);" });
            modalBodyTypeInfo.AttributeList.Add(new AttributeList { Name = "autocomplete", Value = "off" });
            if (ModalEdit.User_ID == 0)
                List_ModalBodyTypeInfo.Add(modalBodyTypeInfo);

            modalBodyTypeInfo = new ModalBodyTypeInfo();
            modalBodyTypeInfo.ModelBodyType = GetModelBodyType.TRInput;
            modalBodyTypeInfo.GetInputTypeString = GetInputStringType.text;
            modalBodyTypeInfo.ClassName = "form-control";
            modalBodyTypeInfo.LabelName = "Confirm Password";
            modalBodyTypeInfo.PlaceHolder = "Confirm Password";
            modalBodyTypeInfo.id = "ConfirmPassword";
            modalBodyTypeInfo.IsRequired = true;
            modalBodyTypeInfo.value = "";
            modalBodyTypeInfo.AttributeList = new List<AttributeList>();
            modalBodyTypeInfo.AttributeList.Add(new AttributeList { Name = "onfocus", Value = "validate(this);" });
            modalBodyTypeInfo.AttributeList.Add(new AttributeList { Name = "onkeydown", Value = "validate(this);" });
            modalBodyTypeInfo.AttributeList.Add(new AttributeList { Name = "onchange", Value = "validate(this);" });
            modalBodyTypeInfo.AttributeList.Add(new AttributeList { Name = "autocomplete", Value = "off" });
            if (ModalEdit.User_ID == 0)
                List_ModalBodyTypeInfo.Add(modalBodyTypeInfo);

            modalBodyTypeInfo = new ModalBodyTypeInfo();
            modalBodyTypeInfo.ModelBodyType = GetModelBodyType.TRInput;
            modalBodyTypeInfo.GetInputTypeString = GetInputStringType.text;
            modalBodyTypeInfo.ClassName = "form-control";
            modalBodyTypeInfo.LabelName = "Email";
            modalBodyTypeInfo.PlaceHolder = "Email";
            modalBodyTypeInfo.id = "Email";
            modalBodyTypeInfo.IsRequired = true;
            if (ModalEdit.Email != "")
            {
                modalBodyTypeInfo.value = ModalEdit.Email;
            }
            modalBodyTypeInfo.AttributeList = new List<AttributeList>();
            modalBodyTypeInfo.AttributeList.Add(new AttributeList { Name = "onfocus", Value = "validate(this);" });
            modalBodyTypeInfo.AttributeList.Add(new AttributeList { Name = "onkeydown", Value = "validate(this);" });
            modalBodyTypeInfo.AttributeList.Add(new AttributeList { Name = "onchange", Value = "validate(this);" });
            modalBodyTypeInfo.AttributeList.Add(new AttributeList { Name = "autocomplete", Value = "off" });
            List_ModalBodyTypeInfo.Add(modalBodyTypeInfo);

            modalBodyTypeInfo = new ModalBodyTypeInfo();
            modalBodyTypeInfo.ModelBodyType = GetModelBodyType.TRInput;
            modalBodyTypeInfo.GetInputTypeString = GetInputStringType.text;
            modalBodyTypeInfo.ClassName = "form-control";
            modalBodyTypeInfo.LabelName = "Telegram Name";
            modalBodyTypeInfo.PlaceHolder = "Telegram User Name";
            modalBodyTypeInfo.id = "TelegramUserName";
            modalBodyTypeInfo.IsRequired = false;
            if (ModalEdit.TelegramUserName != "")
            {
                modalBodyTypeInfo.value = ModalEdit.TelegramUserName!;
            }
            modalBodyTypeInfo.AttributeList = new List<AttributeList>();
            modalBodyTypeInfo.AttributeList.Add(new AttributeList { Name = "autocomplete", Value = "off" });
            List_ModalBodyTypeInfo.Add(modalBodyTypeInfo);

            modalBodyTypeInfo = new ModalBodyTypeInfo();
            modalBodyTypeInfo.ModelBodyType = GetModelBodyType.TRInput;
            modalBodyTypeInfo.GetInputTypeString = GetInputStringType.text;
            modalBodyTypeInfo.ClassName = "form-control";
            modalBodyTypeInfo.LabelName = "Telegram ID";
            modalBodyTypeInfo.PlaceHolder = "Telegram ID";
            modalBodyTypeInfo.id = "TelegramID";
            modalBodyTypeInfo.IsRequired = false;
            if (ModalEdit.TelegramID != "")
            {
                modalBodyTypeInfo.value = ModalEdit.TelegramID!;
            }
            modalBodyTypeInfo.AttributeList = new List<AttributeList>();
            modalBodyTypeInfo.AttributeList.Add(new AttributeList { Name = "autocomplete", Value = "off" });
            List_ModalBodyTypeInfo.Add(modalBodyTypeInfo);

            modalBodyTypeInfo = new ModalBodyTypeInfo();
            modalBodyTypeInfo.ModelBodyType = GetModelBodyType.TRInput;
            modalBodyTypeInfo.GetInputTypeString = GetInputStringType.text;
            modalBodyTypeInfo.ClassName = "form-control";
            modalBodyTypeInfo.LabelName = "First Name";
            modalBodyTypeInfo.PlaceHolder = "First Name";
            modalBodyTypeInfo.id = "FirstName";
            modalBodyTypeInfo.IsRequired = false;
            if (ModalEdit.FirstName != "")
            {
                modalBodyTypeInfo.value = ModalEdit.FirstName;
            }
            modalBodyTypeInfo.AttributeList = new List<AttributeList>();
            modalBodyTypeInfo.AttributeList.Add(new AttributeList { Name = "autocomplete", Value = "off" });
            List_ModalBodyTypeInfo.Add(modalBodyTypeInfo);

            modalBodyTypeInfo = new ModalBodyTypeInfo();
            modalBodyTypeInfo.ModelBodyType = GetModelBodyType.TRInput;
            modalBodyTypeInfo.GetInputTypeString = GetInputStringType.text;
            modalBodyTypeInfo.ClassName = "form-control";
            modalBodyTypeInfo.LabelName = "Last Name";
            modalBodyTypeInfo.PlaceHolder = "Last Name";
            modalBodyTypeInfo.id = "LastName";
            modalBodyTypeInfo.IsRequired = false;
            if (ModalEdit.LastName != "")
            {
                modalBodyTypeInfo.value = ModalEdit.LastName;
            }
            modalBodyTypeInfo.AttributeList = new List<AttributeList>();
            modalBodyTypeInfo.AttributeList.Add(new AttributeList { Name = "autocomplete", Value = "off" });
            List_ModalBodyTypeInfo.Add(modalBodyTypeInfo);

            modalBodyTypeInfo = new ModalBodyTypeInfo();
            modalBodyTypeInfo.ModelBodyType = GetModelBodyType.TRInput;
            modalBodyTypeInfo.GetInputTypeString = GetInputStringType.date;
            modalBodyTypeInfo.ClassName = "form-control";
            modalBodyTypeInfo.LabelName = "Password Expiry";
            modalBodyTypeInfo.PlaceHolder = "Password Expiry";
            modalBodyTypeInfo.id = "PasswordExpiryDateTime";
            modalBodyTypeInfo.IsRequired = false;
            if (ModalEdit.PasswordExpiryDateTime!.ToString() != "")
            {
                modalBodyTypeInfo.value = ModalEdit.PasswordExpiryDateTime;
            }
            modalBodyTypeInfo.AttributeList = new List<AttributeList>();
            modalBodyTypeInfo.AttributeList.Add(new AttributeList { Name = "autocomplete", Value = "off" });
            List_ModalBodyTypeInfo.Add(modalBodyTypeInfo);

            modalBodyTypeInfo = new ModalBodyTypeInfo();
            modalBodyTypeInfo.ModelBodyType = GetModelBodyType.TRselect;
            modalBodyTypeInfo.GetInputTypeString = GetInputStringType.text;
            modalBodyTypeInfo.ClassName = "form-control";
            modalBodyTypeInfo.LabelName = "Application";
            modalBodyTypeInfo.id = "Application";
            modalBodyTypeInfo.IsRequired = false;
            modalBodyTypeInfo.isselect = false;
            modalBodyTypeInfo.ismultiselect = true;
            if (!string.IsNullOrWhiteSpace(ModalEdit.UserType_MTV_CODE))
            {
                modalBodyTypeInfo.IsSelectOption = true;
                modalBodyTypeInfo.value = ModalEdit.UserType_MTV_CODE;
            }
            modalBodyTypeInfo.selectLists = ApplicationList;
            modalBodyTypeInfo.AttributeList = new List<AttributeList>();
            modalBodyTypeInfo.AttributeList.Add(new AttributeList { Name = "multiple", Value = "multiple" });
            modalBodyTypeInfo.AttributeList.Add(new AttributeList { Name = "autocomplete", Value = "off" });
            List_ModalBodyTypeInfo.Add(modalBodyTypeInfo);

            modalBodyTypeInfo = new ModalBodyTypeInfo();
            modalBodyTypeInfo.ModelBodyType = GetModelBodyType.TRselect;
            modalBodyTypeInfo.GetInputTypeString = GetInputStringType.text;
            modalBodyTypeInfo.ClassName = "form-control";
            modalBodyTypeInfo.LabelName = "Role";
            modalBodyTypeInfo.id = "Role";
            modalBodyTypeInfo.IsRequired = false;
            if (!string.IsNullOrWhiteSpace(ModalEdit.UserType_MTV_CODE))
            {
                modalBodyTypeInfo.IsSelectOption = true;
                modalBodyTypeInfo.value = ModalEdit.UserType_MTV_CODE;
            }
            modalBodyTypeInfo.selectLists = RoleList;
            modalBodyTypeInfo.AttributeList = new List<AttributeList>();
            modalBodyTypeInfo.AttributeList.Add(new AttributeList { Name = "onfocus", Value = "validate(this);" });
            modalBodyTypeInfo.AttributeList.Add(new AttributeList { Name = "onkeydown", Value = "validate(this);" });
            modalBodyTypeInfo.AttributeList.Add(new AttributeList { Name = "onchange", Value = "validate(this);" });
            modalBodyTypeInfo.AttributeList.Add(new AttributeList { Name = "autocomplete", Value = "off" });
            List_ModalBodyTypeInfo.Add(modalBodyTypeInfo);

            modalBodyTypeInfo = new ModalBodyTypeInfo();
            modalBodyTypeInfo.ModelBodyType = GetModelBodyType.TRselect;
            modalBodyTypeInfo.GetInputTypeString = GetInputStringType.text;
            modalBodyTypeInfo.ClassName = "form-control";
            modalBodyTypeInfo.LabelName = "User Type";
            modalBodyTypeInfo.id = "UserType_MTV_CODE";
            modalBodyTypeInfo.IsRequired = false;
            if (!string.IsNullOrWhiteSpace(ModalEdit.UserType_MTV_CODE))
            {
                modalBodyTypeInfo.IsSelectOption = true;
                modalBodyTypeInfo.value = ModalEdit.UserType_MTV_CODE;
            }
            modalBodyTypeInfo.selectLists = UserTypeList;
            modalBodyTypeInfo.AttributeList = new List<AttributeList>();
            modalBodyTypeInfo.AttributeList.Add(new AttributeList { Name = "onfocus", Value = "validate(this);" });
            modalBodyTypeInfo.AttributeList.Add(new AttributeList { Name = "onkeydown", Value = "validate(this);" });
            modalBodyTypeInfo.AttributeList.Add(new AttributeList { Name = "onchange", Value = "validate(this);" });
            modalBodyTypeInfo.AttributeList.Add(new AttributeList { Name = "autocomplete", Value = "off" });
            List_ModalBodyTypeInfo.Add(modalBodyTypeInfo);

            modalBodyTypeInfo = new ModalBodyTypeInfo();
            modalBodyTypeInfo.ModelBodyType = GetModelBodyType.TRselect;
            modalBodyTypeInfo.GetInputTypeString = GetInputStringType.text;
            modalBodyTypeInfo.ClassName = "form-control";
            modalBodyTypeInfo.LabelName = "Block Type";
            modalBodyTypeInfo.id = "BlockType_MTV_CODE";
            modalBodyTypeInfo.IsRequired = false;
            if (!string.IsNullOrWhiteSpace(ModalEdit.BlockType_MTV_CODE))
            {
                modalBodyTypeInfo.IsSelectOption = true;
                modalBodyTypeInfo.value = ModalEdit.BlockType_MTV_CODE;
            }
            modalBodyTypeInfo.selectLists = BlockTypeList;
            modalBodyTypeInfo.AttributeList = new List<AttributeList>();
            modalBodyTypeInfo.AttributeList.Add(new AttributeList { Name = "onfocus", Value = "validate(this);" });
            modalBodyTypeInfo.AttributeList.Add(new AttributeList { Name = "onkeydown", Value = "validate(this);" });
            modalBodyTypeInfo.AttributeList.Add(new AttributeList { Name = "onchange", Value = "validate(this);" });
            modalBodyTypeInfo.AttributeList.Add(new AttributeList { Name = "autocomplete", Value = "off" });
            List_ModalBodyTypeInfo.Add(modalBodyTypeInfo);

            modalBodyTypeInfo = new ModalBodyTypeInfo();
            modalBodyTypeInfo.ModelBodyType = GetModelBodyType.TRCheckBox;
            modalBodyTypeInfo.LabelName = "IsApproved";
            modalBodyTypeInfo.IsRequired = false;
            modalBodyTypeInfo.GetInputTypeString = GetInputStringType.checkbox;
            modalBodyTypeInfo.PlaceHolder = "IsApproved";
            modalBodyTypeInfo.id = "IsApproved";
            modalBodyTypeInfo.value = "";
            if (ModalEdit.User_ID > 0)
            {
                modalBodyTypeInfo.ischecked = ModalEdit.IsApproved;
            }
            else
            {
                modalBodyTypeInfo.ischecked = false;
            }
            List_ModalBodyTypeInfo.Add(modalBodyTypeInfo);

            modalBodyTypeInfo = new ModalBodyTypeInfo();
            modalBodyTypeInfo.ModelBodyType = GetModelBodyType.TRCheckBox;
            modalBodyTypeInfo.LabelName = "Is TempPassword";
            modalBodyTypeInfo.IsRequired = false;
            modalBodyTypeInfo.GetInputTypeString = GetInputStringType.checkbox;
            modalBodyTypeInfo.PlaceHolder = "Is TempPassword";
            modalBodyTypeInfo.id = "IsTempPassword";
            modalBodyTypeInfo.value = "";
            if (ModalEdit.User_ID > 0)
            {
                modalBodyTypeInfo.ischecked = ModalEdit.IsTempPassword;
            }
            else
            {
                modalBodyTypeInfo.ischecked = false;
            }
            List_ModalBodyTypeInfo.Add(modalBodyTypeInfo);

            getModalDetail.modalBodyTypeInfos = List_ModalBodyTypeInfo;
            HtmlString = ModalFunctions.GetModalWithBody(getModalDetail);
            return HtmlString;
        }

        [CheckSessionExpiration]
        [HttpPost]
        public IActionResult AddOrEdit_User([FromBody] P_AddOrEdit_User_Request req)
        {
            bool IsAdd = StaticPublicObjects.ado.P_Is_Has_Right_From_Username_And_PR_ID_From_Memory(_PublicClaimObjects!.username, RightsList_ID.User_Setup_Add);
            bool IsEdit = StaticPublicObjects.ado.P_Is_Has_Right_From_Username_And_PR_ID_From_Memory(_PublicClaimObjects.username, RightsList_ID.User_Setup_Edit);
            if ((IsAdd == false) || (IsEdit == false))
                return Content(JsonConvert.SerializeObject("No Rights"));

            P_ReturnMessage_Result response = StaticPublicObjects.ado.P_AddOrEdit_User(req);
            if (response.ReturnCode == false)
                StaticPublicObjects.logFile.ErrorLog(FunctionName: "AddOrEdit_User", SmallMessage: response.ReturnText!, Message: response.ReturnText!);

            return Content(JsonConvert.SerializeObject(response));
        }

        [CheckSessionExpiration]
        [HttpPost]
        public IActionResult Remove_User([FromBody] string Ery_User_ID)
        {
            bool IsDelete = StaticPublicObjects.ado.P_Is_Has_Right_From_Username_And_PR_ID_From_Memory(_PublicClaimObjects!.username, RightsList_ID.User_Setup_Delete);
            if (IsDelete)
            {
                int User_ID = Crypto.DecryptNumericToStringWithOutNull(Ery_User_ID);
                P_ReturnMessage_Result response = StaticPublicObjects.ado.P_SP_Remove_Generic_Result("T_Users", "User_ID", User_ID);
                if (response.ReturnCode == false)
                    StaticPublicObjects.logFile.ErrorLog(FunctionName: "Remove_User", SmallMessage: response.ReturnText!, Message: response.ReturnText!);
                return Content(JsonConvert.SerializeObject(response));
            }
            else
            {
                return Content(JsonConvert.SerializeObject("No Rights"));
            }
        }
        #endregion User Setup

        #region Page Setup
        [CustomPageSetup]
        public IActionResult PageSetup()
        {
            bool IsView = StaticPublicObjects.ado.P_Is_Has_Right_From_Username_And_PR_ID_From_Memory(_PublicClaimObjects!.username, RightsList_ID.Page_Setup_View);
            if (IsView)
            {
                bool IsAdd = StaticPublicObjects.ado.P_Is_Has_Right_From_Username_And_PR_ID_From_Memory(_PublicClaimObjects.username, RightsList_ID.Page_Setup_Add);
                bool IsEdit = StaticPublicObjects.ado.P_Is_Has_Right_From_Username_And_PR_ID_From_Memory(_PublicClaimObjects.username, RightsList_ID.Page_Setup_Edit);
                bool IsDelete = StaticPublicObjects.ado.P_Is_Has_Right_From_Username_And_PR_ID_From_Memory(_PublicClaimObjects.username, RightsList_ID.Page_Setup_Delete);
                ViewBag.RightsListObj = new { IsView = IsView, IsAdd = IsAdd, IsEdit = IsEdit, IsDelete = IsDelete };
                ViewBag.RightsList = JsonConvert.SerializeObject(ViewBag.RightsListObj);


                DataSet DS = new DataSet();
                (string Name, object Value)[] ParamsNameValues = new (string, object)[1];
                ParamsNameValues[0] = ("Username", _PublicClaimObjects.username);
                DS = StaticPublicObjects.ado.ExecuteStoreProcedureDS("P_Get_Page_Setup_DropDown_Lists", ParamsNameValues);
                ViewBag.PageGroupList = JsonConvert.SerializeObject(DS.Tables[0]);
                ViewBag.PageList = JsonConvert.SerializeObject(DS.Tables[1]);
                ViewBag.ApplicationList = JsonConvert.SerializeObject(DS.Tables[2]);
                return View();
            }
            else
            {
                string ID = "";
                Exception exception = new Exception("You Don't Have Rights To Access Page Setup");
                ID = _httpContextAccessor.HttpContext!.Session.SetupSessionError("No Rights", "/Security/PageSetup", "Page Setup", exception);
                return Redirect($"/Error/Index?ID={ID}");
            }
        }

        [CheckSessionExpiration]
        [HttpPost]
        public IActionResult GetPageGroupsList([FromBody] ReportParams _ReportParams)
        {
            ReportResponsePageSetup reportResponse = new ReportResponsePageSetup();
            try
            {
                if (StaticPublicObjects.ado.P_Is_Has_Right_From_Username_And_PR_ID_From_Memory(_PublicClaimObjects!.username, RightsList_ID.Page_Setup_View))
                {
                    List<Dynamic_SP_Params> List_Dynamic_SP_Params = new List<Dynamic_SP_Params>();
                    ModalFunctions.GetKendoFilter(ref _ReportParams, ref List_Dynamic_SP_Params, _PublicClaimObjects, true);

                    List<P_PageGroup_Result> ResultList = StaticPublicObjects.ado.P_Get_Generic_List_SP<P_PageGroup_Result>("P_Get_PageGroup_List", ref List_Dynamic_SP_Params);

                    reportResponse.TotalRowCount = ModalFunctions.GetValueFromReturnParameter<long>(List_Dynamic_SP_Params, "TotalRowCount", typeof(long));
                    reportResponse.ResultData = ResultList;
                    reportResponse.response_code = true;
                }
                else
                {
                    reportResponse.TotalRowCount = 0;
                    reportResponse.ResultData = null;
                    reportResponse.response_code = false;
                }
            }
            catch (Exception ex)
            {
                StaticPublicObjects.logFile.ErrorLog(FunctionName: "GetPageGroupsList", SmallMessage: ex.Message, Message: ex.ToString());
                reportResponse.TotalRowCount = 0;
                reportResponse.ResultData = null;
                reportResponse.response_code = false;
            }
            return Globals.GetAjaxJsonReturn(reportResponse);
        }

        [CheckSessionExpiration]
        [HttpPost]
        public IActionResult GetPagesList([FromBody] ReportParams _ReportParams)
        {
            ReportResponsePageSetup reportResponse = new ReportResponsePageSetup();
            try
            {
                if (StaticPublicObjects.ado.P_Is_Has_Right_From_Username_And_PR_ID_From_Memory(_PublicClaimObjects!.username, RightsList_ID.Page_Setup_View))
                {
                    List<Dynamic_SP_Params> List_Dynamic_SP_Params = new List<Dynamic_SP_Params>();
                    ModalFunctions.GetKendoFilter(ref _ReportParams, ref List_Dynamic_SP_Params, _PublicClaimObjects, true);

                    List<P_Page_Result> ResultList = StaticPublicObjects.ado.P_Get_Generic_List_SP<P_Page_Result>("P_Get_Page_List", ref List_Dynamic_SP_Params);

                    reportResponse.TotalRowCount = ModalFunctions.GetValueFromReturnParameter<long>(List_Dynamic_SP_Params, "TotalRowCount", typeof(long));
                    reportResponse.ResultData = ResultList;
                    reportResponse.response_code = true;
                }
                else
                {
                    reportResponse.TotalRowCount = 0;
                    reportResponse.ResultData = null;
                    reportResponse.response_code = false;
                }
            }
            catch (Exception ex)
            {
                StaticPublicObjects.logFile.ErrorLog(FunctionName: "GetPagesList", SmallMessage: ex.Message, Message: ex.ToString());
                reportResponse.TotalRowCount = 0;
                reportResponse.ResultData = null;
                reportResponse.response_code = false;
            }
            return Globals.GetAjaxJsonReturn(reportResponse);
        }

        [CheckSessionExpiration]
        [HttpPost]
        public string GetAddEditPageGroupModal([FromBody] string Encrypted_PG_ID)
        {
            string HtmlString = "";
            int PG_ID = Crypto.DecryptNumericToStringWithOutNull(Encrypted_PG_ID);

            bool IsAdd = StaticPublicObjects.ado.P_Is_Has_Right_From_Username_And_PR_ID_From_Memory(_PublicClaimObjects!.username, RightsList_ID.Page_Setup_Add);
            bool IsEdit = StaticPublicObjects.ado.P_Is_Has_Right_From_Username_And_PR_ID_From_Memory(_PublicClaimObjects.username, RightsList_ID.Page_Setup_Edit);
            if ((PG_ID == 0 && IsAdd == false) || (PG_ID > 0 && IsEdit == false))
                return "No Rights";


            GetModalDetail getModalDetail = new GetModalDetail();
            List<ModalBodyTypeInfo> List_ModalBodyTypeInfo = new List<ModalBodyTypeInfo>();
            ModalBodyTypeInfo modalBodyTypeInfo = new ModalBodyTypeInfo();

            P_AddOrEdit_PageGroup_Response PageGroupEdit = new P_AddOrEdit_PageGroup_Response();
            if (PG_ID > 0)
            {
                List<Dynamic_SP_Params> List_Dynamic_SP_Params = new List<Dynamic_SP_Params>();
                Dynamic_SP_Params Dynamic_SP_Params = new Dynamic_SP_Params();
                Dynamic_SP_Params.ParameterName = "PG_ID";
                Dynamic_SP_Params.Val = PG_ID;
                List_Dynamic_SP_Params.Add(Dynamic_SP_Params);
                PageGroupEdit = StaticPublicObjects.ado.ExecuteSelectSQLMap<P_AddOrEdit_PageGroup_Response>("SELECT PG_ID, PageGroupName, IsHide, IsActive Active FROM [dbo].[T_Page_Group] with (nolock) WHERE PG_ID = @PG_ID", false, 0, ref List_Dynamic_SP_Params);
            }

            getModalDetail.getmodelsize = GetModalSize.modal_md;
            getModalDetail.modaltitle = (PG_ID == 0 ? "Add New Page Group" : "Edit Page Group");
            getModalDetail.modalfootercancelbuttonname = "Cancel";
            getModalDetail.modalfootersuccessbuttonname = (PG_ID == 0 ? "Add" : "Update");
            getModalDetail.modalBodyTypeInfos = new List<ModalBodyTypeInfo>();

            getModalDetail.onclickmodalsuccess = "AddOrEditPageGroup()";

            modalBodyTypeInfo = new ModalBodyTypeInfo();
            modalBodyTypeInfo.ModelBodyType = GetModelBodyType.TRInput;
            modalBodyTypeInfo.LabelName = "Page Group ID";
            modalBodyTypeInfo.IsRequired = true;
            modalBodyTypeInfo.IsHidden = true;
            modalBodyTypeInfo.GetInputTypeString = GetInputStringType.text;
            modalBodyTypeInfo.PlaceHolder = "Page Group ID";
            modalBodyTypeInfo.id = "modalpagegroupid";
            if (PageGroupEdit.PG_ID > 0)
            {
                modalBodyTypeInfo.value = PageGroupEdit.PG_ID;
            }
            else
            {
                modalBodyTypeInfo.value = "";
            }
            modalBodyTypeInfo.ClassName = "form-control";
            modalBodyTypeInfo.AttributeList = new List<AttributeList>
            {
                new AttributeList { Name = "readonly", Value = "readonly" }
            };
            if (PageGroupEdit.PG_ID > 0)
                List_ModalBodyTypeInfo.Add(modalBodyTypeInfo);

            modalBodyTypeInfo = new ModalBodyTypeInfo();
            modalBodyTypeInfo.ModelBodyType = GetModelBodyType.TRInput;
            modalBodyTypeInfo.LabelName = "Page Group Name";
            modalBodyTypeInfo.IsRequired = true;
            modalBodyTypeInfo.GetInputTypeString = GetInputStringType.text;
            modalBodyTypeInfo.PlaceHolder = "Page Group Name";
            modalBodyTypeInfo.id = "modalpagegroupname";
            if (PageGroupEdit.PageGroupName != "")
            {
                modalBodyTypeInfo.value = PageGroupEdit.PageGroupName;
            }
            else
            {
                modalBodyTypeInfo.value = "";
            }
            modalBodyTypeInfo.ClassName = "form-control";
            modalBodyTypeInfo.AttributeList = new List<AttributeList>
            {
                new AttributeList(){Name = "onfocus", Value = "validate(this)"},
                new AttributeList(){Name = "onkeydown", Value = "validate(this)"},
                new AttributeList(){Name = "autocomplete", Value = "off"}
            };
            List_ModalBodyTypeInfo.Add(modalBodyTypeInfo);

            modalBodyTypeInfo = new ModalBodyTypeInfo();
            modalBodyTypeInfo.ModelBodyType = GetModelBodyType.TRCheckBox;
            modalBodyTypeInfo.LabelName = "Is Hide";
            modalBodyTypeInfo.IsRequired = false;
            modalBodyTypeInfo.GetInputTypeString = GetInputStringType.checkbox;
            modalBodyTypeInfo.PlaceHolder = "Is Hide";
            modalBodyTypeInfo.id = "modalpagegroupishide";
            modalBodyTypeInfo.value = "";
            if (PageGroupEdit.IsHide)
            {
                modalBodyTypeInfo.ischecked = PageGroupEdit.IsHide;
            }
            else
            {
                modalBodyTypeInfo.ischecked = false;
            }
            List_ModalBodyTypeInfo.Add(modalBodyTypeInfo);

            modalBodyTypeInfo = new ModalBodyTypeInfo();
            modalBodyTypeInfo.ModelBodyType = GetModelBodyType.TRCheckBox;
            modalBodyTypeInfo.LabelName = "Is Active";
            modalBodyTypeInfo.IsRequired = false;
            modalBodyTypeInfo.GetInputTypeString = GetInputStringType.checkbox;
            modalBodyTypeInfo.PlaceHolder = "Is Active";
            modalBodyTypeInfo.id = "modalpgisactive";
            modalBodyTypeInfo.value = "";
            if (PageGroupEdit.PG_ID > 0)
            {
                modalBodyTypeInfo.ischecked = PageGroupEdit.Active;
            }
            else
            {
                modalBodyTypeInfo.ischecked = true;
            }
            List_ModalBodyTypeInfo.Add(modalBodyTypeInfo);

            getModalDetail.modalBodyTypeInfos = List_ModalBodyTypeInfo;

            HtmlString = ModalFunctions.GetModalWithBody(getModalDetail);
            return HtmlString;
        }

        [CheckSessionExpiration]
        [HttpPost]
        public string GetAddEditPageModal([FromBody] string Encrypted_P_ID)
        {
            string HtmlString = "";
            DataSet DS = new DataSet();
            int P_ID = Crypto.DecryptNumericToStringWithOutNull(Encrypted_P_ID);

            bool IsAdd = StaticPublicObjects.ado.P_Is_Has_Right_From_Username_And_PR_ID_From_Memory(_PublicClaimObjects!.username, RightsList_ID.Page_Setup_Add);
            bool IsEdit = StaticPublicObjects.ado.P_Is_Has_Right_From_Username_And_PR_ID_From_Memory(_PublicClaimObjects.username, RightsList_ID.Page_Setup_Edit);
            if ((P_ID == 0 && IsAdd == false) || (P_ID > 0 && IsEdit == false))
                return "No Rights";

            GetModalDetail getModalDetail = new GetModalDetail();
            List<ModalBodyTypeInfo> List_ModalBodyTypeInfo = new List<ModalBodyTypeInfo>();
            ModalBodyTypeInfo modalBodyTypeInfo = new ModalBodyTypeInfo();

            P_AddOrEdit_Page_Response PageEdit = new P_AddOrEdit_Page_Response();
            if (P_ID > 0)
            {
                List<Dynamic_SP_Params> List_Dynamic_SP_Params = new List<Dynamic_SP_Params>();
                Dynamic_SP_Params Dynamic_SP_Params = new Dynamic_SP_Params();
                Dynamic_SP_Params.ParameterName = "P_ID";
                Dynamic_SP_Params.Val = P_ID;
                List_Dynamic_SP_Params.Add(Dynamic_SP_Params);
                PageEdit = StaticPublicObjects.ado.ExecuteSelectSQLMap<P_AddOrEdit_Page_Response>("SELECT P_ID, PG_ID, PageName, PageURL, Application_MTV_ID, IsHide, IsActive Active FROM [dbo].[T_Page] with (nolock) WHERE P_ID = @P_ID", false, 0, ref List_Dynamic_SP_Params);
            }

            List<SelectDropDownList> PageGroupList = StaticPublicObjects.ado.Get_DropDownList_Result("SELECT PG_ID code, PageGroupName name FROM [dbo].[T_Page_Group] with (nolock) WHERE isActive = 1 ORDER BY Sort_");
            List<SelectDropDownList> ApplicationList = StaticPublicObjects.ado.Get_DropDownList_Result("SELECT code = App_ID, [name] = [App_Name] FROM [dbo].[T_Application] WITH (NOLOCK)");

            getModalDetail.getmodelsize = GetModalSize.modal_md;
            getModalDetail.modaltitle = (P_ID == 0 ? "Add New Page" : "Edit Page");
            getModalDetail.modalfootercancelbuttonname = "Cancel";
            getModalDetail.modalfootersuccessbuttonname = (P_ID == 0 ? "Add" : "Update");
            getModalDetail.modalBodyTypeInfos = new List<ModalBodyTypeInfo>();

            getModalDetail.onclickmodalsuccess = "AddOrEditPage()";

            modalBodyTypeInfo = new ModalBodyTypeInfo();
            modalBodyTypeInfo.ModelBodyType = GetModelBodyType.TRInput;
            modalBodyTypeInfo.LabelName = "Page ID";
            modalBodyTypeInfo.IsRequired = true;
            modalBodyTypeInfo.GetInputTypeString = GetInputStringType.text;
            modalBodyTypeInfo.PlaceHolder = "Page ID";
            modalBodyTypeInfo.id = "modalpageid";
            if (PageEdit.P_ID > 0)
            {
                modalBodyTypeInfo.value = PageEdit.P_ID;
            }
            else
            {
                modalBodyTypeInfo.value = "";
            }
            modalBodyTypeInfo.AttributeList = new List<AttributeList>
            {
                new AttributeList { Name = "readonly", Value = "readonly" }
            };
            if (PageEdit.P_ID > 0)
                List_ModalBodyTypeInfo.Add(modalBodyTypeInfo);

            modalBodyTypeInfo = new ModalBodyTypeInfo();
            modalBodyTypeInfo.ModelBodyType = GetModelBodyType.TRselect;
            modalBodyTypeInfo.LabelName = "Application";
            modalBodyTypeInfo.IsRequired = true;
            modalBodyTypeInfo.GetInputTypeString = GetInputStringType.text;
            modalBodyTypeInfo.id = "modalApplication_MTV_ID";
            if (PageEdit.Application_MTV_ID > 0)
            {
                modalBodyTypeInfo.IsSelectOption = true;
                modalBodyTypeInfo.value = PageEdit.Application_MTV_ID;
            }
            else
            {
                modalBodyTypeInfo.value = "";
            }
            modalBodyTypeInfo.selectLists = ApplicationList;
            modalBodyTypeInfo.ClassName = "form-control";
            modalBodyTypeInfo.AttributeList = new List<AttributeList>
            {
                new AttributeList(){Name = "onfocus", Value = "validate(this)"},
                new AttributeList(){Name = "onkeydown", Value = "validate(this)"},
                new AttributeList(){Name = "onchange", Value = "validate(this)"},
                new AttributeList(){Name = "autocomplete", Value = "off"}
            };
            List_ModalBodyTypeInfo.Add(modalBodyTypeInfo);

            modalBodyTypeInfo = new ModalBodyTypeInfo();
            modalBodyTypeInfo.ModelBodyType = GetModelBodyType.TRselect;
            modalBodyTypeInfo.LabelName = "Page Group Name";
            modalBodyTypeInfo.IsRequired = true;
            modalBodyTypeInfo.GetInputTypeString = GetInputStringType.text;
            modalBodyTypeInfo.id = "modalSelectPageGroupName";
            if (PageEdit.PG_ID > 0)
            {
                modalBodyTypeInfo.IsSelectOption = true;
                modalBodyTypeInfo.value = PageEdit.PG_ID;
            }
            else
            {
                modalBodyTypeInfo.value = "";
            }
            modalBodyTypeInfo.selectLists = PageGroupList;
            modalBodyTypeInfo.ClassName = "form-control";
            modalBodyTypeInfo.AttributeList = new List<AttributeList>
            {
                new AttributeList(){Name = "onfocus", Value = "validate(this)"},
                new AttributeList(){Name = "onkeydown", Value = "validate(this)"},
                new AttributeList(){Name = "onchange", Value = "validate(this)"},
                new AttributeList(){Name = "autocomplete", Value = "off"}
            };
            List_ModalBodyTypeInfo.Add(modalBodyTypeInfo);

            modalBodyTypeInfo = new ModalBodyTypeInfo();
            modalBodyTypeInfo.ModelBodyType = GetModelBodyType.TRInput;
            modalBodyTypeInfo.LabelName = "Page Name";
            modalBodyTypeInfo.IsRequired = true;
            modalBodyTypeInfo.GetInputTypeString = GetInputStringType.text;
            modalBodyTypeInfo.PlaceHolder = "Page Name";
            modalBodyTypeInfo.id = "modalpagename";
            if (PageEdit.PageName != "")
            {
                modalBodyTypeInfo.value = PageEdit.PageName;
            }
            else
            {
                modalBodyTypeInfo.value = "";
            }
            modalBodyTypeInfo.ClassName = "form-control";
            modalBodyTypeInfo.AttributeList = new List<AttributeList>
            {
                new AttributeList(){Name = "onfocus", Value = "validate(this)"},
                new AttributeList(){Name = "onkeydown", Value = "validate(this)"},
                new AttributeList(){Name = "onchange", Value = "validate(this)"},
                new AttributeList(){Name = "autocomplete", Value = "off"}
            };
            List_ModalBodyTypeInfo.Add(modalBodyTypeInfo);

            modalBodyTypeInfo = new ModalBodyTypeInfo();
            modalBodyTypeInfo.ModelBodyType = GetModelBodyType.TRInput;
            modalBodyTypeInfo.LabelName = "Page URL";
            modalBodyTypeInfo.IsRequired = true;
            modalBodyTypeInfo.GetInputTypeString = GetInputStringType.text;
            modalBodyTypeInfo.PlaceHolder = "Page URL";
            modalBodyTypeInfo.id = "modalpageurl";
            if (PageEdit.PageUrl != "")
            {
                modalBodyTypeInfo.value = PageEdit.PageUrl;
            }
            else
            {
                modalBodyTypeInfo.value = "";
            }
            modalBodyTypeInfo.ClassName = "form-control";
            modalBodyTypeInfo.AttributeList = new List<AttributeList>
            {
                new AttributeList(){Name = "onfocus", Value = "validate(this)"},
                new AttributeList(){Name = "onkeydown", Value = "validate(this)"},
                new AttributeList(){Name = "onchange", Value = "validate(this)"},
                new AttributeList(){Name = "autocomplete", Value = "off"}
            };
            List_ModalBodyTypeInfo.Add(modalBodyTypeInfo);

            modalBodyTypeInfo = new ModalBodyTypeInfo();
            modalBodyTypeInfo.ModelBodyType = GetModelBodyType.TRCheckBox;
            modalBodyTypeInfo.LabelName = "Is Hide";
            modalBodyTypeInfo.IsRequired = false;
            modalBodyTypeInfo.GetInputTypeString = GetInputStringType.checkbox;
            modalBodyTypeInfo.PlaceHolder = "Is Hide";
            modalBodyTypeInfo.id = "modalpageishide";
            modalBodyTypeInfo.value = "";
            if (PageEdit.IsHide)
            {
                modalBodyTypeInfo.ischecked = PageEdit.IsHide;
            }
            else
            {
                modalBodyTypeInfo.ischecked = false;
            }
            List_ModalBodyTypeInfo.Add(modalBodyTypeInfo);

            modalBodyTypeInfo = new ModalBodyTypeInfo();
            modalBodyTypeInfo.ModelBodyType = GetModelBodyType.TRCheckBox;
            modalBodyTypeInfo.LabelName = "Is Active";
            modalBodyTypeInfo.IsRequired = false;
            modalBodyTypeInfo.GetInputTypeString = GetInputStringType.checkbox;
            modalBodyTypeInfo.PlaceHolder = "Is Active";
            modalBodyTypeInfo.id = "modalpisactive";
            modalBodyTypeInfo.value = "";
            if (PageEdit.Active)
            {
                modalBodyTypeInfo.ischecked = PageEdit.Active;
            }
            else
            {
                modalBodyTypeInfo.ischecked = true;
            }
            List_ModalBodyTypeInfo.Add(modalBodyTypeInfo);

            getModalDetail.modalBodyTypeInfos = List_ModalBodyTypeInfo;
            HtmlString = ModalFunctions.GetModalWithBody(getModalDetail);
            return HtmlString;
        }

        [CheckSessionExpiration]
        [HttpPost]
        public IActionResult AddOrEdit_PageGroup([FromBody] P_AddOrEdit_PageGroup_Response res)
        {
            bool IsAdd = StaticPublicObjects.ado.P_Is_Has_Right_From_Username_And_PR_ID_From_Memory(_PublicClaimObjects!.username, RightsList_ID.Page_Setup_Add);
            bool IsEdit = StaticPublicObjects.ado.P_Is_Has_Right_From_Username_And_PR_ID_From_Memory(_PublicClaimObjects.username, RightsList_ID.Page_Setup_Edit);
            if ((IsAdd == false) || (IsEdit == false))
                return Content(JsonConvert.SerializeObject("No Rights"));

            P_ReturnMessage_Result response = StaticPublicObjects.ado.P_SP_MultiParm_Result("P_AddOrEdit_PageGroup", res, _PublicClaimObjects.username);
            if (response.ReturnCode == false)
                StaticPublicObjects.logFile.ErrorLog(FunctionName: "AddOrEdit_PageGroup", SmallMessage: response.ReturnText!, Message: response.ReturnText!);
            return Content(JsonConvert.SerializeObject(response));
        }

        [CheckSessionExpiration]
        [HttpPost]
        public IActionResult AddOrEdit_Page([FromBody] P_AddOrEdit_Page_Response res)
        {
            bool IsAdd = StaticPublicObjects.ado.P_Is_Has_Right_From_Username_And_PR_ID_From_Memory(_PublicClaimObjects!.username, RightsList_ID.Page_Setup_Add);
            bool IsEdit = StaticPublicObjects.ado.P_Is_Has_Right_From_Username_And_PR_ID_From_Memory(_PublicClaimObjects.username, RightsList_ID.Page_Setup_Edit);
            if ((IsAdd == false) || (IsEdit == false))
                return Content(JsonConvert.SerializeObject("No Rights"));

            P_ReturnMessage_Result response = StaticPublicObjects.ado.P_SP_MultiParm_Result("P_AddOrEdit_Page", res, _PublicClaimObjects.username);
            if (response.ReturnCode == false)
                StaticPublicObjects.logFile.ErrorLog(FunctionName: "AddOrEdit_Page", SmallMessage: response.ReturnText!, Message: response.ReturnText!);
            return Content(JsonConvert.SerializeObject(response));
        }

        [CheckSessionExpiration]
        [HttpPost]
        public IActionResult Remove_PageGroup([FromBody] string Ery_PG_ID)
        {
            bool IsDelete = StaticPublicObjects.ado.P_Is_Has_Right_From_Username_And_PR_ID_From_Memory(_PublicClaimObjects!.username, RightsList_ID.Page_Setup_Delete);
            if (IsDelete)
            {
                int PG_ID = Crypto.DecryptNumericToStringWithOutNull(Ery_PG_ID);
                P_ReturnMessage_Result response = StaticPublicObjects.ado.P_SP_Remove_Generic_Result("T_Page_Group", "PG_ID", PG_ID);
                if (response.ReturnCode == false)
                    StaticPublicObjects.logFile.ErrorLog(FunctionName: "Remove_PageGroup", SmallMessage: response.ReturnText!, Message: response.ReturnText!);
                return Content(JsonConvert.SerializeObject(response));
            }
            else
            {
                return Content(JsonConvert.SerializeObject("No Rights"));
            }
        }

        [CheckSessionExpiration]
        [HttpPost]
        public IActionResult Remove_Page([FromBody] string Ery_P_ID)
        {
            bool IsDelete = StaticPublicObjects.ado.P_Is_Has_Right_From_Username_And_PR_ID_From_Memory(_PublicClaimObjects!.username, RightsList_ID.Page_Setup_Delete);
            if (IsDelete)
            {
                int P_ID = Crypto.DecryptNumericToStringWithOutNull(Ery_P_ID);
                P_ReturnMessage_Result response = StaticPublicObjects.ado.P_SP_Remove_Generic_Result("T_Page", "P_ID", P_ID);
                if (response.ReturnCode == false)
                    StaticPublicObjects.logFile.ErrorLog(FunctionName: "Remove_Page", SmallMessage: response.ReturnText!, Message: response.ReturnText!);
                return Content(JsonConvert.SerializeObject(response));
            }
            else
            {
                return Content(JsonConvert.SerializeObject("No Rights"));
            }
        }

        [CheckSessionExpiration]
        [HttpPost]
        public string Get_PageGroup_Sorting()
        {
            string query = "SELECT ROW_NUMBER() OVER (ORDER BY Sort_) New_Sort_Value, PG_ID Sort_ID, PageGroupName Sort_Text, Sort_ Old_Sort_Value FROM [dbo].[T_Page_Group] with (nolock) WHERE isActive = 1 ORDER BY Sort_";
            string result = ModalFunctions.GetSortingModelWithData(query);
            return result;
        }

        [CheckSessionExpiration]
        [HttpPost]
        public string Get_Page_Sorting([FromBody] string Ery_PG_ID)
        {
            int PG_ID = Crypto.DecryptNumericToStringWithOutNull(Ery_PG_ID);
            string query = "SELECT ROW_NUMBER() OVER (ORDER BY Sort_) New_Sort_Value, P_ID Sort_ID, PageName Sort_Text, Sort_ Old_Sort_Value FROM [dbo].[T_Page] with (nolock) WHERE PG_ID = " + PG_ID + " AND isActive = 1 ORDER BY Sort_";
            string result = ModalFunctions.GetSortingModelWithData(query);
            return result;
        }

        [CheckSessionExpiration]
        [HttpPost]
        public IActionResult Update_PageGroup_Sorting([FromBody] List<Sorting_Result> res)
        {
            var json = JsonConvert.SerializeObject(res);
            P_ReturnMessage_Result response = StaticPublicObjects.ado.P_SP_SingleParm_Result("P_Update_PageGroup_Sorting", "json", json, _PublicClaimObjects!.username);
            if (response.ReturnCode == false)
                StaticPublicObjects.logFile.ErrorLog(FunctionName: "Update_PageGroup_Sorting", SmallMessage: response.ReturnText!, Message: response.ReturnText!);
            return Content(JsonConvert.SerializeObject(response));
        }

        [CheckSessionExpiration]
        [HttpPost]
        public IActionResult Update_Page_Sorting([FromBody] List<Sorting_Result> res)
        {
            var json = JsonConvert.SerializeObject(res);
            P_ReturnMessage_Result response = StaticPublicObjects.ado.P_SP_SingleParm_Result("P_Update_Page_Sorting", "json", json, _PublicClaimObjects!.username);
            if (response.ReturnCode == false)
                StaticPublicObjects.logFile.ErrorLog(FunctionName: "Update_Page_Sorting", SmallMessage: response.ReturnText!, Message: response.ReturnText!);
            return Content(JsonConvert.SerializeObject(response));
        }
        #endregion  Page Setup

        #region Role Setup
        [CustomPageSetupAttribute]
        public IActionResult RoleSetup()
        {
            bool IsView = StaticPublicObjects.ado.P_Is_Has_Right_From_Username_And_PR_ID_From_Memory(_PublicClaimObjects!.username, RightsList_ID.Role_Setup_View);
            if (IsView)
            {
                bool IsAdd = StaticPublicObjects.ado.P_Is_Has_Right_From_Username_And_PR_ID_From_Memory(_PublicClaimObjects.username, RightsList_ID.Role_Setup_Add);
                bool IsEdit = StaticPublicObjects.ado.P_Is_Has_Right_From_Username_And_PR_ID_From_Memory(_PublicClaimObjects.username, RightsList_ID.Role_Setup_Edit);
                bool IsDelete = StaticPublicObjects.ado.P_Is_Has_Right_From_Username_And_PR_ID_From_Memory(_PublicClaimObjects.username, RightsList_ID.Role_Setup_Delete);
                ViewBag.RightsListObj = new { IsView = IsView, IsAdd = IsAdd, IsEdit = IsEdit, IsDelete = IsDelete };
                ViewBag.RightsList = JsonConvert.SerializeObject(ViewBag.RightsListObj);
                DataSet DS = new DataSet();
                (string Name, object Value)[] ParamsNameValues = new (string, object)[1];
                ParamsNameValues[0] = ("Username", _PublicClaimObjects.username);
                DS = StaticPublicObjects.ado.ExecuteStoreProcedureDS("[P_Get_Role_Setup_DropDown_Lists]", ParamsNameValues);
                ViewBag.RoleList = JsonConvert.SerializeObject(DS.Tables[0]);
                ViewBag.RoleGroupList = JsonConvert.SerializeObject(DS.Tables[1]);
                return View();
            }
            else
            {
                string ID = "";
                Exception exception = new Exception("You Don't Have Rights To Access Role Setup");
                ID = _httpContextAccessor.HttpContext!.Session.SetupSessionError("No Rights", "/Security/PageSetup", "Role Setup", exception);
                return Redirect($"/Error/Index?ID={ID}");
            }
        }

        [HttpPost]
        public IActionResult GetFilterData_Role_List([FromBody] ReportParams _ReportParams)
        {
            ReportResponse reportResponse = new ReportResponse();
            try
            {
                if (StaticPublicObjects.ado.P_Is_Has_Right_From_Username_And_PR_ID_From_Memory(_PublicClaimObjects!.username, RightsList_ID.Role_Setup_View))
                {
                    List<Dynamic_SP_Params> List_Dynamic_SP_Params = new List<Dynamic_SP_Params>();
                    ModalFunctions.GetKendoFilter(ref _ReportParams, ref List_Dynamic_SP_Params, _PublicClaimObjects, true);

                    List<P_Role_Result> ResultList = StaticPublicObjects.ado.P_Get_Generic_List_SP<P_Role_Result>("P_Get_Roles_List", ref List_Dynamic_SP_Params);

                    reportResponse.TotalRowCount = ModalFunctions.GetValueFromReturnParameter<long>(List_Dynamic_SP_Params, "TotalRowCount", typeof(long));
                    reportResponse.ResultData = ResultList;
                    reportResponse.response_code = true;
                }
                else
                {
                    reportResponse.TotalRowCount = 0;
                    reportResponse.ResultData = null;
                    reportResponse.response_code = false;
                }
            }
            catch (Exception ex)
            {
                StaticPublicObjects.logFile.ErrorLog(FunctionName: "GetFilterData_Role_List", SmallMessage: ex.Message, Message: ex.ToString());
                reportResponse.TotalRowCount = 0;
                reportResponse.ResultData = null;
                reportResponse.response_code = false;
            }
            return Globals.GetAjaxJsonReturn(reportResponse);
        }
        [HttpPost]
        public IActionResult GetFilterData_RoleGroup_List([FromBody] ReportParams _ReportParams)
        {
            ReportResponse reportResponse = new ReportResponse();
            try
            {
                if (StaticPublicObjects.ado.P_Is_Has_Right_From_Username_And_PR_ID_From_Memory(_PublicClaimObjects.username, RightsList_ID.Role_Setup_View))
                {
                    List<Dynamic_SP_Params> List_Dynamic_SP_Params = new List<Dynamic_SP_Params>();
                    ModalFunctions.GetKendoFilter(ref _ReportParams, ref List_Dynamic_SP_Params, _PublicClaimObjects, true);

                    List<P_RoleGroup_Result> ResultList = StaticPublicObjects.ado.P_Get_Generic_List_SP<P_RoleGroup_Result>("P_Get_Roles_Group_List", ref List_Dynamic_SP_Params);

                    reportResponse.TotalRowCount = ModalFunctions.GetValueFromReturnParameter<long>(List_Dynamic_SP_Params, "TotalRowCount", typeof(long));
                    reportResponse.ResultData = ResultList;
                    reportResponse.response_code = true;
                }
                else
                {
                    reportResponse.TotalRowCount = 0;
                    reportResponse.ResultData = null;
                    reportResponse.response_code = false;
                }
            }
            catch (Exception ex)
            {
                StaticPublicObjects.logFile.ErrorLog(FunctionName: "GetFilterData_RoleGroup_List", SmallMessage: ex.Message, Message: ex.ToString());
                reportResponse.TotalRowCount = 0;
                reportResponse.ResultData = null;
                reportResponse.response_code = false;
            }
            return Globals.GetAjaxJsonReturn(reportResponse);
        }
        [HttpPost]
        public IActionResult GetFilterData_RoleGroupMap_List([FromBody] ReportParams _ReportParams)
        {
            ReportResponse reportResponse = new ReportResponse();
            try
            {
                if (StaticPublicObjects.ado.P_Is_Has_Right_From_Username_And_PR_ID_From_Memory(_PublicClaimObjects.username, RightsList_ID.Role_Setup_View))
                {
                    List<Dynamic_SP_Params> List_Dynamic_SP_Params = new List<Dynamic_SP_Params>();
                    ModalFunctions.GetKendoFilter(ref _ReportParams, ref List_Dynamic_SP_Params, _PublicClaimObjects, true);

                    List<P_RoleGroupMap_Result> ResultList = StaticPublicObjects.ado.P_Get_Generic_List_SP<P_RoleGroupMap_Result>("P_Get_RolesGroupMap_List", ref List_Dynamic_SP_Params);

                    reportResponse.TotalRowCount = ModalFunctions.GetValueFromReturnParameter<long>(List_Dynamic_SP_Params, "TotalRowCount", typeof(long));
                    reportResponse.ResultData = ResultList;
                    reportResponse.response_code = true;
                }
                else
                {
                    reportResponse.TotalRowCount = 0;
                    reportResponse.ResultData = null;
                    reportResponse.response_code = false;
                }
            }
            catch (Exception ex)
            {
                StaticPublicObjects.logFile.ErrorLog(FunctionName: "GetFilterData_RoleGroupMap_List", SmallMessage: ex.Message, Message: ex.ToString());
                reportResponse.TotalRowCount = 0;
                reportResponse.ResultData = null;
                reportResponse.response_code = false;
            }
            return Globals.GetAjaxJsonReturn(reportResponse);
        }


        [HttpPost]
        public string GetAddEditRoleSetupModal([FromBody] int RoleID)
        {
            string HtmlString = "";

            bool IsAdd = StaticPublicObjects.ado.P_Is_Has_Right_From_Username_And_PR_ID_From_Memory(_PublicClaimObjects.username, RightsList_ID.Role_Setup_Add);
            bool IsEdit = StaticPublicObjects.ado.P_Is_Has_Right_From_Username_And_PR_ID_From_Memory(_PublicClaimObjects.username, RightsList_ID.Role_Setup_Edit);

            if ((RoleID == 0 && IsAdd == false) || (RoleID > 0 && IsEdit == false))
                return "No Rights";


            GetModalDetail getModalDetail = new GetModalDetail();
            List<ModalBodyTypeInfo> List_ModalBodyTypeInfo = new List<ModalBodyTypeInfo>();
            ModalBodyTypeInfo modalBodyTypeInfo = new ModalBodyTypeInfo();

            P_AddOrEdit_Role_Response RoleEdit = new P_AddOrEdit_Role_Response();
            if (RoleID > 0)
            {
                List<Dynamic_SP_Params> List_Dynamic_SP_Params = new List<Dynamic_SP_Params>();
                Dynamic_SP_Params Dynamic_SP_Params = new Dynamic_SP_Params();
                Dynamic_SP_Params.ParameterName = "R_ID";
                Dynamic_SP_Params.Val = RoleID;
                List_Dynamic_SP_Params.Add(Dynamic_SP_Params);
                RoleEdit = StaticPublicObjects.ado.ExecuteSelectSQLMap<P_AddOrEdit_Role_Response>("SELECT R_ID RoleID, RoleName, IsActive Active FROM [dbo].[T_Roles] with (nolock) WHERE R_ID = @R_ID", false, 0, ref List_Dynamic_SP_Params);
            }

            getModalDetail.getmodelsize = GetModalSize.modal_md;
            getModalDetail.modaltitle = (RoleID == 0 ? "Add New Role" : "Edit Role");
            getModalDetail.modalfootercancelbuttonname = "Cancel";
            getModalDetail.modalfootersuccessbuttonname = (RoleID == 0 ? "Add" : "Update");
            getModalDetail.modalBodyTypeInfos = new List<ModalBodyTypeInfo>();

            getModalDetail.onclickmodalsuccess = "AddOrEditRole()";

            modalBodyTypeInfo = new ModalBodyTypeInfo();
            modalBodyTypeInfo.ModelBodyType = GetModelBodyType.TRInput;
            modalBodyTypeInfo.LabelName = "Role ID";
            modalBodyTypeInfo.IsRequired = false;
            modalBodyTypeInfo.GetInputTypeString = GetInputStringType.number;
            modalBodyTypeInfo.PlaceHolder = "Role ID";
            modalBodyTypeInfo.id = "modalroleid";
            if (RoleEdit.RoleID > 0)
            {
                modalBodyTypeInfo.value = RoleEdit.RoleID;
            }
            else
            {
                modalBodyTypeInfo.value = "";
            }
            modalBodyTypeInfo.ClassName = "form-control";
            modalBodyTypeInfo.AttributeList = new List<AttributeList>
            {
                new AttributeList { Name = "readonly", Value = "readonly" }
            };
            if (RoleEdit.RoleID > 0)
                List_ModalBodyTypeInfo.Add(modalBodyTypeInfo);

            modalBodyTypeInfo = new ModalBodyTypeInfo();
            modalBodyTypeInfo.ModelBodyType = GetModelBodyType.TRInput;
            modalBodyTypeInfo.LabelName = "Role Name";
            modalBodyTypeInfo.IsRequired = true;
            modalBodyTypeInfo.GetInputTypeString = GetInputStringType.text;
            modalBodyTypeInfo.PlaceHolder = "Role Name";
            modalBodyTypeInfo.id = "modalrolename";
            if (RoleEdit.RoleName != "")
            {
                modalBodyTypeInfo.value = RoleEdit.RoleName;
            }
            else
            {
                modalBodyTypeInfo.value = "";
            }
            modalBodyTypeInfo.ClassName = "form-control";
            modalBodyTypeInfo.AttributeList = new List<AttributeList>
            {
                new AttributeList(){Name = "onfocus", Value = "validate(this)"},
                new AttributeList(){Name = "onkeydown", Value = "validate(this)"},
                new AttributeList(){Name = "onchange", Value = "validate(this)"},
                new AttributeList(){Name = "autocomplete", Value = "off"}
            };
            List_ModalBodyTypeInfo.Add(modalBodyTypeInfo);

            modalBodyTypeInfo = new ModalBodyTypeInfo();
            modalBodyTypeInfo.ModelBodyType = GetModelBodyType.TRCheckBox;
            modalBodyTypeInfo.LabelName = "Is Active";
            modalBodyTypeInfo.IsRequired = false;
            if (RoleEdit.RoleID > 0)
            {
                modalBodyTypeInfo.ischecked = RoleEdit.Active;
            }
            else
            {
                modalBodyTypeInfo.ischecked = true;
            }
            modalBodyTypeInfo.GetInputTypeString = GetInputStringType.checkbox;
            modalBodyTypeInfo.PlaceHolder = "Is Active";
            modalBodyTypeInfo.id = "modalroleisactive";
            modalBodyTypeInfo.value = "";
            List_ModalBodyTypeInfo.Add(modalBodyTypeInfo);

            getModalDetail.modalBodyTypeInfos = List_ModalBodyTypeInfo;

            HtmlString = ModalFunctions.GetModalWithBody(getModalDetail);

            return HtmlString;
        }
        [HttpPost]
        public string GetAddEditRoleGroupModal([FromBody] int RoleGroupID)
        {
            string HtmlString = "";

            bool IsAdd = StaticPublicObjects.ado.P_Is_Has_Right_From_Username_And_PR_ID_From_Memory(_PublicClaimObjects.username, RightsList_ID.Role_Setup_Add);
            bool IsEdit = StaticPublicObjects.ado.P_Is_Has_Right_From_Username_And_PR_ID_From_Memory(_PublicClaimObjects.username, RightsList_ID.Role_Setup_Edit);

            if ((RoleGroupID == 0 && IsAdd == false) || (RoleGroupID > 0 && IsEdit == false))
                return "No Rights";

            GetModalDetail getModalDetail = new GetModalDetail();
            List<ModalBodyTypeInfo> List_ModalBodyTypeInfo = new List<ModalBodyTypeInfo>();
            ModalBodyTypeInfo modalBodyTypeInfo = new ModalBodyTypeInfo();

            P_AddOrEdit_Role_Group_Response RoleGroupEdit = new P_AddOrEdit_Role_Group_Response();
            if (RoleGroupID > 0)
            {
                List<Dynamic_SP_Params> List_Dynamic_SP_Params = new List<Dynamic_SP_Params>();
                Dynamic_SP_Params Dynamic_SP_Params = new Dynamic_SP_Params();
                Dynamic_SP_Params.ParameterName = "RG_ID";
                Dynamic_SP_Params.Val = RoleGroupID;
                List_Dynamic_SP_Params.Add(Dynamic_SP_Params);
                RoleGroupEdit = StaticPublicObjects.ado.ExecuteSelectSQLMap<P_AddOrEdit_Role_Group_Response>("SELECT RG_ID RoleGroupID, RoleGroupName, IsActive Active FROM [dbo].[T_Role_Group] with (nolock) WHERE RG_ID = @RG_ID", false, 0, ref List_Dynamic_SP_Params);
            }

            getModalDetail.getmodelsize = GetModalSize.modal_md;
            getModalDetail.modaltitle = (RoleGroupID == 0 ? "Add New Role Group" : "Edit Role Group");
            getModalDetail.modalfootercancelbuttonname = "Cancel";
            getModalDetail.modalfootersuccessbuttonname = (RoleGroupID == 0 ? "Add" : "Update");
            getModalDetail.modalBodyTypeInfos = new List<ModalBodyTypeInfo>();

            getModalDetail.onclickmodalsuccess = "AddOrEdit_Role_Group()";

            modalBodyTypeInfo = new ModalBodyTypeInfo();
            modalBodyTypeInfo.ModelBodyType = GetModelBodyType.TRInput;
            modalBodyTypeInfo.LabelName = "Role Group ID";
            modalBodyTypeInfo.IsRequired = false;
            modalBodyTypeInfo.GetInputTypeString = GetInputStringType.number;
            modalBodyTypeInfo.PlaceHolder = "Role Group ID";
            modalBodyTypeInfo.id = "modalrolegroupid";
            if (RoleGroupEdit.RoleGroupID > 0)
            {
                modalBodyTypeInfo.value = RoleGroupEdit.RoleGroupID;
            }
            else
            {
                modalBodyTypeInfo.value = "";
            }
            modalBodyTypeInfo.ClassName = "form-control";
            modalBodyTypeInfo.AttributeList = new List<AttributeList>
            {
                new AttributeList { Name = "readonly", Value = "readonly" }
            };
            if (RoleGroupEdit.RoleGroupID > 0)
                List_ModalBodyTypeInfo.Add(modalBodyTypeInfo);

            modalBodyTypeInfo = new ModalBodyTypeInfo();
            modalBodyTypeInfo.ModelBodyType = GetModelBodyType.TRInput;
            modalBodyTypeInfo.LabelName = "Role Group Name";
            modalBodyTypeInfo.IsRequired = true;
            modalBodyTypeInfo.GetInputTypeString = GetInputStringType.text;
            modalBodyTypeInfo.PlaceHolder = "Role Group Name";
            modalBodyTypeInfo.id = "modalrolegroupname";
            if (RoleGroupEdit.RoleGroupName != "")
            {
                modalBodyTypeInfo.value = RoleGroupEdit.RoleGroupName;
            }
            else
            {
                modalBodyTypeInfo.value = "";
            }
            modalBodyTypeInfo.ClassName = "form-control";
            modalBodyTypeInfo.AttributeList = new List<AttributeList>
            {
                new AttributeList(){Name = "onfocus", Value = "validate(this)"},
                new AttributeList(){Name = "onkeydown", Value = "validate(this)"},
                new AttributeList(){Name = "onchange", Value = "validate(this)"},
                new AttributeList(){Name = "autocomplete", Value = "off"}
            };
            List_ModalBodyTypeInfo.Add(modalBodyTypeInfo);

            modalBodyTypeInfo = new ModalBodyTypeInfo();
            modalBodyTypeInfo.ModelBodyType = GetModelBodyType.TRCheckBox;
            modalBodyTypeInfo.LabelName = "Is Active";
            modalBodyTypeInfo.IsRequired = false;
            if (RoleGroupEdit.RoleGroupID > 0)
            {
                modalBodyTypeInfo.ischecked = RoleGroupEdit.Active;
            }
            else
            {
                modalBodyTypeInfo.ischecked = true;
            }
            modalBodyTypeInfo.GetInputTypeString = GetInputStringType.checkbox;
            modalBodyTypeInfo.PlaceHolder = "Is Active";
            modalBodyTypeInfo.id = "modalrolegroupisactive";
            modalBodyTypeInfo.value = "";
            List_ModalBodyTypeInfo.Add(modalBodyTypeInfo);

            getModalDetail.modalBodyTypeInfos = List_ModalBodyTypeInfo;

            HtmlString = ModalFunctions.GetModalWithBody(getModalDetail);

            return HtmlString;
        }
        [HttpPost]
        public string GetAddEditRoleGroupMappingModal([FromBody] int RoleGroupMappingID)
        {
            string HtmlString = "";

            bool IsAdd = StaticPublicObjects.ado.P_Is_Has_Right_From_Username_And_PR_ID_From_Memory(_PublicClaimObjects.username, RightsList_ID.Role_Setup_Add);
            bool IsEdit = StaticPublicObjects.ado.P_Is_Has_Right_From_Username_And_PR_ID_From_Memory(_PublicClaimObjects.username, RightsList_ID.Role_Setup_Edit);

            if ((RoleGroupMappingID == 0 && IsAdd == false) || (RoleGroupMappingID > 0 && IsEdit == false))
                return "No Rights";

            GetModalDetail getModalDetail = new GetModalDetail();
            List<ModalBodyTypeInfo> List_ModalBodyTypeInfo = new List<ModalBodyTypeInfo>();
            ModalBodyTypeInfo modalBodyTypeInfo = new ModalBodyTypeInfo();

            P_AddOrEdit_Role_Group_Mapping_Response RoleGroupMappingEdit = new P_AddOrEdit_Role_Group_Mapping_Response();
            if (RoleGroupMappingID > 0)
            {
                List<Dynamic_SP_Params> List_Dynamic_SP_Params = new List<Dynamic_SP_Params>();
                Dynamic_SP_Params Dynamic_SP_Params = new Dynamic_SP_Params();
                Dynamic_SP_Params.ParameterName = "RGM_ID";
                Dynamic_SP_Params.Val = RoleGroupMappingID;
                List_Dynamic_SP_Params.Add(Dynamic_SP_Params);
                RoleGroupMappingEdit = StaticPublicObjects.ado.ExecuteSelectSQLMap<P_AddOrEdit_Role_Group_Mapping_Response>("SELECT RGM_ID RoleGroupMappingID, R_ID RoleID, RG_ID RoleGroupID, IsActive Active FROM [dbo].[T_Role_Group_Mapping] with (nolock) WHERE RGM_ID = @RGM_ID", false, 0, ref List_Dynamic_SP_Params);
            }

            List<Dynamic_SP_Params> RoleGroup_Params = null;
            List<SelectDropDownList> List_RoleGroup_SelectDropDownList = StaticPublicObjects.ado.ExecuteSelectSQLMapList<SelectDropDownList>("SELECT code = RG_ID, name = RoleGroupName FROM [dbo].[T_Role_Group] with (nolock) WHERE isActive = 1", false, 0, ref RoleGroup_Params);
            List<SelectDropDownList> List_Role_SelectDropDownList = new List<SelectDropDownList>();
            if (RoleGroupMappingID > 0)
            {
                List_Role_SelectDropDownList = StaticPublicObjects.ado.ExecuteSelectSQLMapList<SelectDropDownList>("SELECT code = R_ID, name = RoleName FROM [dbo].[T_Roles] with (nolock) WHERE isActive = 1", false, 0, ref RoleGroup_Params);
            }

            getModalDetail.getmodelsize = GetModalSize.modal_md;
            getModalDetail.modaltitle = (RoleGroupMappingID == 0 ? "Add New Role Group Mapping" : "Edit Role Group Mapping");
            getModalDetail.modalfootercancelbuttonname = "Cancel";
            getModalDetail.modalfootersuccessbuttonname = (RoleGroupMappingID == 0 ? "Add" : "Update");
            getModalDetail.modalBodyTypeInfos = new List<ModalBodyTypeInfo>();

            getModalDetail.onclickmodalsuccess = "AddOrEdit_Role_Group_Mapping()";

            modalBodyTypeInfo = new ModalBodyTypeInfo();
            modalBodyTypeInfo.ModelBodyType = GetModelBodyType.TRInput;
            modalBodyTypeInfo.LabelName = "Role Group Mapping ID";
            modalBodyTypeInfo.IsRequired = false;
            modalBodyTypeInfo.GetInputTypeString = GetInputStringType.number;
            modalBodyTypeInfo.PlaceHolder = "Role Group Mapping ID";
            modalBodyTypeInfo.id = "modalrolegroupmaingid";
            if (RoleGroupMappingEdit.RoleGroupMappingID > 0)
            {
                modalBodyTypeInfo.value = RoleGroupMappingEdit.RoleGroupMappingID;
            }
            else
            {
                modalBodyTypeInfo.value = "";
            }
            modalBodyTypeInfo.ClassName = "form-control";
            modalBodyTypeInfo.AttributeList = new List<AttributeList>
            {
                new AttributeList { Name = "readonly", Value = "readonly" },
            };
            if (RoleGroupMappingEdit.RoleGroupMappingID > 0)
                List_ModalBodyTypeInfo.Add(modalBodyTypeInfo);

            modalBodyTypeInfo = new ModalBodyTypeInfo();
            modalBodyTypeInfo.ModelBodyType = GetModelBodyType.TRselect;
            modalBodyTypeInfo.LabelName = "Role Group Name";
            modalBodyTypeInfo.IsRequired = true;
            modalBodyTypeInfo.GetInputTypeString = GetInputStringType.text;
            modalBodyTypeInfo.PlaceHolder = "Role Group Name";
            modalBodyTypeInfo.id = "modalrgmrolegroupname";
            if (RoleGroupMappingEdit.RoleGroupID > 0)
            {
                modalBodyTypeInfo.IsSelectOption = true;
                modalBodyTypeInfo.value = RoleGroupMappingEdit.RoleGroupID;
            }
            else
            {
                modalBodyTypeInfo.value = "";
            }
            modalBodyTypeInfo.selectLists = List_RoleGroup_SelectDropDownList;
            modalBodyTypeInfo.ClassName = "form-control";
            if (RoleGroupMappingID > 0)
            {
                modalBodyTypeInfo.AttributeList = new List<AttributeList>
                {
                    new AttributeList(){Name = "onfocus", Value = "validate(this)"},
                    new AttributeList(){Name = "onkeydown", Value = "validate(this)"},
                    new AttributeList { Name = "onchange", Value = "validate(this)" },
                    new AttributeList(){Name = "autocomplete", Value = "off"}
                };
            }
            else
            {
                modalBodyTypeInfo.AttributeList = new List<AttributeList>
                {
                    new AttributeList(){Name = "onfocus", Value = "validate(this)"},
                    new AttributeList(){Name = "onkeydown", Value = "validate(this)"},
                    new AttributeList { Name = "onchange", Value = "Load_Roles_By_RoleGroup_Dropdown(this.value)" },
                    new AttributeList(){Name = "autocomplete", Value = "off"}
                };
            }
            List_ModalBodyTypeInfo.Add(modalBodyTypeInfo);

            modalBodyTypeInfo = new ModalBodyTypeInfo();
            modalBodyTypeInfo.ModelBodyType = GetModelBodyType.TRselect;
            modalBodyTypeInfo.LabelName = "Role Name";
            modalBodyTypeInfo.IsRequired = true;
            modalBodyTypeInfo.GetInputTypeString = GetInputStringType.text;
            modalBodyTypeInfo.PlaceHolder = "Role Name";
            modalBodyTypeInfo.id = "modalrgmrolename";
            if (RoleGroupMappingEdit.RoleID > 0)
            {
                modalBodyTypeInfo.IsSelectOption = true;
                modalBodyTypeInfo.value = RoleGroupMappingEdit.RoleID;
            }
            else
            {
                modalBodyTypeInfo.value = "";
            }
            modalBodyTypeInfo.selectLists = List_Role_SelectDropDownList;
            modalBodyTypeInfo.ClassName = "form-control";
            modalBodyTypeInfo.AttributeList = new List<AttributeList>
            {
                new AttributeList(){Name = "onfocus", Value = "validate(this)"},
                new AttributeList(){Name = "onkeydown", Value = "validate(this)"},
                new AttributeList(){Name = "onchange", Value = "validate(this)"},
                new AttributeList(){Name = "autocomplete", Value = "off"}
            };
            List_ModalBodyTypeInfo.Add(modalBodyTypeInfo);

            modalBodyTypeInfo = new ModalBodyTypeInfo();
            modalBodyTypeInfo.ModelBodyType = GetModelBodyType.TRCheckBox;
            modalBodyTypeInfo.LabelName = "Is Active";
            modalBodyTypeInfo.IsRequired = false;
            if (RoleGroupMappingEdit.RoleGroupMappingID > 0)
            {
                modalBodyTypeInfo.ischecked = RoleGroupMappingEdit.Active;
            }
            else
            {
                modalBodyTypeInfo.ischecked = true;
            }
            modalBodyTypeInfo.GetInputTypeString = GetInputStringType.checkbox;
            modalBodyTypeInfo.PlaceHolder = "Is Active";
            modalBodyTypeInfo.id = "modalrolegroupmappingisactive";
            modalBodyTypeInfo.value = "";
            List_ModalBodyTypeInfo.Add(modalBodyTypeInfo);

            getModalDetail.modalBodyTypeInfos = List_ModalBodyTypeInfo;

            HtmlString = ModalFunctions.GetModalWithBody(getModalDetail);

            return HtmlString;
        }


        [HttpPost]
        public IActionResult AddOrEdit_Role([FromBody] P_AddOrEdit_Role_Response res)
        {
            bool IsAdd = StaticPublicObjects.ado.P_Is_Has_Right_From_Username_And_PR_ID_From_Memory(_PublicClaimObjects!.username, RightsList_ID.Role_Setup_Add);
            bool IsEdit = StaticPublicObjects.ado.P_Is_Has_Right_From_Username_And_PR_ID_From_Memory(_PublicClaimObjects.username, RightsList_ID.Role_Setup_Edit);

            if ((IsAdd == false) || (IsEdit == false))
                return Content(JsonConvert.SerializeObject("No Rights"));


            P_ReturnMessage_Result response = StaticPublicObjects.ado.P_SP_MultiParm_Result("P_AddOrEdit_Roles", res, _PublicClaimObjects.username);
            if (response.ReturnCode == false)
                StaticPublicObjects.logFile.ErrorLog(FunctionName: "AddOrEdit_Role", SmallMessage: response.ReturnText!, Message: response.ReturnText!);
            return Content(JsonConvert.SerializeObject(response));
        }
        [HttpPost]
        public IActionResult AddOrEdit_Role_Group([FromBody] P_AddOrEdit_Role_Group_Response res)
        {
            bool IsAdd = StaticPublicObjects.ado.P_Is_Has_Right_From_Username_And_PR_ID_From_Memory(_PublicClaimObjects.username, RightsList_ID.Role_Setup_Add);
            bool IsEdit = StaticPublicObjects.ado.P_Is_Has_Right_From_Username_And_PR_ID_From_Memory(_PublicClaimObjects.username, RightsList_ID.Role_Setup_Edit);

            if ((IsAdd == false) || (IsEdit == false))
                return Content(JsonConvert.SerializeObject("No Rights"));

            P_ReturnMessage_Result response = StaticPublicObjects.ado.P_SP_MultiParm_Result("P_AddOrEdit_Role_Group", res, _PublicClaimObjects.username);
            if (response.ReturnCode == false)
                StaticPublicObjects.logFile.ErrorLog(FunctionName: "AddOrEdit_Role_Group", SmallMessage: response.ReturnText!, Message: response.ReturnText!);
            return Content(JsonConvert.SerializeObject(response));
        }
        [HttpPost]
        public IActionResult AddOrEdit_Role_Group_Mapping([FromBody] P_AddOrEdit_Role_Group_Mapping_Response res)
        {
            bool IsAdd = StaticPublicObjects.ado.P_Is_Has_Right_From_Username_And_PR_ID_From_Memory(_PublicClaimObjects.username, RightsList_ID.Role_Setup_Add);
            bool IsEdit = StaticPublicObjects.ado.P_Is_Has_Right_From_Username_And_PR_ID_From_Memory(_PublicClaimObjects.username, RightsList_ID.Role_Setup_Edit);

            if ((IsAdd == false) || (IsEdit == false))
                return Content(JsonConvert.SerializeObject("No Rights"));

            P_ReturnMessage_Result response = StaticPublicObjects.ado.P_SP_MultiParm_Result("P_AddOrEdit_Role_Group_Mapping", res, _PublicClaimObjects.username);
            if (response.ReturnCode == false)
                StaticPublicObjects.logFile.ErrorLog(FunctionName: "AddOrEdit_Role_Group_Mapping", SmallMessage: response.ReturnText!, Message: response.ReturnText!);
            return Content(JsonConvert.SerializeObject(response));
        }


        [HttpPost]
        public IActionResult Remove_Role([FromBody] int RoleID)
        {
            bool IsDelete = StaticPublicObjects.ado.P_Is_Has_Right_From_Username_And_PR_ID_From_Memory(_PublicClaimObjects!.username, RightsList_ID.Role_Setup_Delete);
            if (IsDelete)
            {
                P_ReturnMessage_Result response = StaticPublicObjects.ado.P_SP_Remove_Generic_Result("T_Roles", "R_ID", RoleID);
                if (response.ReturnCode == false)
                    StaticPublicObjects.logFile.ErrorLog(FunctionName: "Remove_Role", SmallMessage: response.ReturnText!, Message: response.ReturnText!);
                return Content(JsonConvert.SerializeObject(response));
            }
            else
            {
                return Content(JsonConvert.SerializeObject("No Rights"));
            }
        }
        [HttpPost]
        public IActionResult Remove_Role_Group([FromBody] int RoleGroupID)
        {
            bool IsDelete = StaticPublicObjects.ado.P_Is_Has_Right_From_Username_And_PR_ID_From_Memory(_PublicClaimObjects!.username, RightsList_ID.Role_Setup_Delete);
            if (IsDelete)
            {
                P_ReturnMessage_Result response = StaticPublicObjects.ado.P_SP_Remove_Generic_Result("T_Role_Group", "RoleGroupID", RoleGroupID);
                if (response.ReturnCode == false)
                    StaticPublicObjects.logFile.ErrorLog(FunctionName: "Remove_Role_Group", SmallMessage: response.ReturnText!, Message: response.ReturnText!);
                return Content(JsonConvert.SerializeObject(response));
            }
            else
            {
                return Content(JsonConvert.SerializeObject("No Rights"));
            }
        }
        [HttpPost]
        public IActionResult Remove_Role_Group_Mapping([FromBody] int RoleGroupMappingID)
        {
            bool IsDelete = StaticPublicObjects.ado.P_Is_Has_Right_From_Username_And_PR_ID_From_Memory(_PublicClaimObjects!.username, RightsList_ID.Role_Setup_Delete);
            if (IsDelete)
            {
                P_ReturnMessage_Result response = StaticPublicObjects.ado.P_SP_Remove_Generic_Result("T_Role_Group_Mapping", "RGM_ID", RoleGroupMappingID);
                if (response.ReturnCode == false)
                    StaticPublicObjects.logFile.ErrorLog(FunctionName: "Remove_Role_Group_Mapping", SmallMessage: response.ReturnText!, Message: response.ReturnText!);
                return Content(JsonConvert.SerializeObject(response));
            }
            else
            {
                return Content(JsonConvert.SerializeObject("No Rights"));
            }
        }


        [HttpPost]
        public IActionResult Get_Roles_By_RoleGroup_Dropdown([FromBody] int RG_ID)
        {
            var result = ModalFunctions.GetDropDownListByID(RG_ID, "R_ID", "RoleName", "T_Roles", "T_Role_Group_Mapping", "RG_ID");
            return Content(JsonConvert.SerializeObject(result));
        }
        [HttpPost]
        public IActionResult Get_Roles_By_Department_Dropdown([FromBody] int D_ID)
        {
            var result = ModalFunctions.GetDropDownListByID(D_ID, "R_ID", "RoleName", "T_Roles", "T_Department_Role_Mapping", "D_ID");
            return Content(JsonConvert.SerializeObject(result));
        }
        #endregion  Role Setup

        #region Rights Setup
        [CustomPageSetup]
        public IActionResult RightsSetup()
        {
            bool IsView = StaticPublicObjects.ado.P_Is_Has_Right_From_Username_And_PR_ID_From_Memory(_PublicClaimObjects.username, RightsList_ID.Right_Setup_View);
            if (IsView)
            {
                bool IsAdd = StaticPublicObjects.ado.P_Is_Has_Right_From_Username_And_PR_ID_From_Memory(_PublicClaimObjects.username, RightsList_ID.Right_Setup_Add);
                bool IsEdit = StaticPublicObjects.ado.P_Is_Has_Right_From_Username_And_PR_ID_From_Memory(_PublicClaimObjects.username, RightsList_ID.Right_Setup_Edit);
                bool IsDelete = StaticPublicObjects.ado.P_Is_Has_Right_From_Username_And_PR_ID_From_Memory(_PublicClaimObjects.username, RightsList_ID.Right_Setup_Delete);
                ViewBag.RightsListObj = new { IsView = IsView, IsAdd = IsAdd, IsEdit = IsEdit, IsDelete = IsDelete };
                ViewBag.RightsList = JsonConvert.SerializeObject(ViewBag.RightsListObj);
                DataSet DS = new DataSet();
                (string Name, object Value)[] ParamsNameValues = new (string, object)[1];
                ParamsNameValues[0] = ("Username", _PublicClaimObjects.username);
                DS = StaticPublicObjects.ado.ExecuteStoreProcedureDS("[P_Get_Rights_Setup_DropDown_Lists]", ParamsNameValues);
                ViewBag.RRoleList = JsonConvert.SerializeObject(DS.Tables[0]);
                ViewBag.PageList = JsonConvert.SerializeObject(DS.Tables[1]);
                ViewBag.PageRightList = JsonConvert.SerializeObject(DS.Tables[2]);
                return View();
            }
            else
            {
                string ID = "";
                Exception exception = new Exception("You Don't Have Rights To Access Rigths Setup");
                ID = _httpContextAccessor.HttpContext.Session.SetupSessionError("No Rights", "/Security/PageSetup", "Rights Setup", exception);
                return Redirect($"/Error/Index?ID={ID}");
            }
        }

        [HttpPost]
        public IActionResult GetFilterData_PageRights_List([FromBody] ReportParams _ReportParams)
        {
            ReportResponse reportResponse = new ReportResponse();
            try
            {
                if (StaticPublicObjects.ado.P_Is_Has_Right_From_Username_And_PR_ID_From_Memory(_PublicClaimObjects.username, RightsList_ID.Right_Setup_View))
                {
                    List<Dynamic_SP_Params> List_Dynamic_SP_Params = new List<Dynamic_SP_Params>();
                    ModalFunctions.GetKendoFilter(ref _ReportParams, ref List_Dynamic_SP_Params, _PublicClaimObjects, true);

                    List<P_PageRight_Result> ResultList = StaticPublicObjects.ado.P_Get_Generic_List_SP<P_PageRight_Result>("P_Get_PageRight_List", ref List_Dynamic_SP_Params);

                    reportResponse.TotalRowCount = ModalFunctions.GetValueFromReturnParameter<long>(List_Dynamic_SP_Params, "TotalRowCount", typeof(long));
                    reportResponse.ResultData = ResultList;
                    reportResponse.response_code = true;
                }
                else
                {
                    reportResponse.TotalRowCount = 0;
                    reportResponse.ResultData = null;
                    reportResponse.response_code = false;
                }
            }
            catch (Exception ex)
            {
                StaticPublicObjects.logFile.ErrorLog(FunctionName: "GetFilterData_PageRights_List", SmallMessage: ex.Message, Message: ex.ToString());
                reportResponse.TotalRowCount = 0;
                reportResponse.ResultData = null;
                reportResponse.response_code = false;
            }
            return Globals.GetAjaxJsonReturn(reportResponse);
        }
        [HttpPost]
        public IActionResult GetFilterData_RolePageRights_List([FromBody] ReportParams _ReportParams)
        {
            ReportResponse reportResponse = new ReportResponse();
            try
            {
                if (StaticPublicObjects.ado.P_Is_Has_Right_From_Username_And_PR_ID_From_Memory(_PublicClaimObjects.username, RightsList_ID.Right_Setup_View))
                {
                    List<Dynamic_SP_Params> List_Dynamic_SP_Params = new List<Dynamic_SP_Params>();
                    ModalFunctions.GetKendoFilter(ref _ReportParams, ref List_Dynamic_SP_Params, _PublicClaimObjects, true);

                    List<P_RolePageRight_Result> ResultList = StaticPublicObjects.ado.P_Get_Generic_List_SP<P_RolePageRight_Result>("P_Get_RolePageRightMap_List", ref List_Dynamic_SP_Params);

                    reportResponse.TotalRowCount = ModalFunctions.GetValueFromReturnParameter<long>(List_Dynamic_SP_Params, "TotalRowCount", typeof(long));
                    reportResponse.ResultData = ResultList;
                    reportResponse.response_code = true;
                }
                else
                {
                    reportResponse.TotalRowCount = 0;
                    reportResponse.ResultData = null;
                    reportResponse.response_code = false;
                }
            }
            catch (Exception ex)
            {
                StaticPublicObjects.logFile.ErrorLog(FunctionName: "GetFilterData_RolePageRights_List", SmallMessage: ex.Message, Message: ex.ToString());
                reportResponse.TotalRowCount = 0;
                reportResponse.ResultData = null;
                reportResponse.response_code = false;
            }
            return Globals.GetAjaxJsonReturn(reportResponse);
        }
        [HttpPost]
        public IActionResult GetFilterData_UserRoleMap_List([FromBody] ReportParams _ReportParams)
        {
            ReportResponse reportResponse = new ReportResponse();
            try
            {
                if (StaticPublicObjects.ado.P_Is_Has_Right_From_Username_And_PR_ID_From_Memory(_PublicClaimObjects.username, RightsList_ID.Right_Setup_View))
                {
                    List<Dynamic_SP_Params> List_Dynamic_SP_Params = new List<Dynamic_SP_Params>();
                    ModalFunctions.GetKendoFilter(ref _ReportParams, ref List_Dynamic_SP_Params, _PublicClaimObjects, true);

                    List<P_UserRoleMap_Result> ResultList = StaticPublicObjects.ado.P_Get_Generic_List_SP<P_UserRoleMap_Result>("P_Get_UserRoleMap_List", ref List_Dynamic_SP_Params);

                    reportResponse.TotalRowCount = ModalFunctions.GetValueFromReturnParameter<long>(List_Dynamic_SP_Params, "TotalRowCount", typeof(long));
                    reportResponse.ResultData = ResultList;
                    reportResponse.response_code = true;
                }
                else
                {
                    reportResponse.TotalRowCount = 0;
                    reportResponse.ResultData = null;
                    reportResponse.response_code = false;
                }
            }
            catch (Exception ex)
            {
                StaticPublicObjects.logFile.ErrorLog(FunctionName: "GetFilterData_DepartmentRoleMap_List", SmallMessage: ex.Message, Message: ex.ToString());
                reportResponse.TotalRowCount = 0;
                reportResponse.ResultData = null;
                reportResponse.response_code = false;
            }
            return Globals.GetAjaxJsonReturn(reportResponse);
        }


        [HttpPost]
        public string GetAddEditPageRightsModal([FromBody] int PR_ID)
        {
            string HtmlString = "";

            bool IsAdd = StaticPublicObjects.ado.P_Is_Has_Right_From_Username_And_PR_ID_From_Memory(_PublicClaimObjects.username, RightsList_ID.Right_Setup_Add);
            bool IsEdit = StaticPublicObjects.ado.P_Is_Has_Right_From_Username_And_PR_ID_From_Memory(_PublicClaimObjects.username, RightsList_ID.Right_Setup_Edit);
            if ((PR_ID == 0 && IsAdd == false) || (PR_ID > 0 && IsEdit == false))
                return "No Rights";

            GetModalDetail getModalDetail = new GetModalDetail();
            List<ModalBodyTypeInfo> List_ModalBodyTypeInfo = new List<ModalBodyTypeInfo>();
            ModalBodyTypeInfo modalBodyTypeInfo = new ModalBodyTypeInfo();

            P_AddOrEdit_PageRights_Response PageRightEdit = new P_AddOrEdit_PageRights_Response();
            if (PR_ID > 0)
            {
                List<Dynamic_SP_Params> List_Dynamic_SP_Params = new List<Dynamic_SP_Params>();
                Dynamic_SP_Params Dynamic_SP_Params = new Dynamic_SP_Params();
                Dynamic_SP_Params.ParameterName = "PR_ID";
                Dynamic_SP_Params.Val = PR_ID;
                List_Dynamic_SP_Params.Add(Dynamic_SP_Params);
                PageRightEdit = StaticPublicObjects.ado.ExecuteSelectSQLMap<P_AddOrEdit_PageRights_Response>("SELECT PR_ID, P_ID, PR_CODE, PageRightName, PageRightType_MTV_CODE PageRightType, IsHide, IsActive Active FROM [dbo].[T_Page_Rights] with (nolock) WHERE PR_ID = @PR_ID", false, 0, ref List_Dynamic_SP_Params);
            }

            List<SelectDropDownList> PageList = StaticPublicObjects.ado.Get_DropDownList_Result("SELECT P_ID code, PageName name FROM [dbo].[T_Page] with (nolock) WHERE isActive = 1 ORDER BY Sort_");
            List<Dynamic_SP_Params> PageRightTypeList_Parm = new List<Dynamic_SP_Params>
            {
                new Dynamic_SP_Params(){ ParameterName = "MT_ID", Val = 133 }
            };
            List<SelectDropDownList> PageRightTypeList = StaticPublicObjects.ado.Get_DropDownList_Result("SELECT MTV_CODE code, Name name FROM[dbo].[T_Master_Type_Value] with(nolock) Where MT_ID = @MT_ID ORDER BY Sort_", PageRightTypeList_Parm);

            getModalDetail.getmodelsize = GetModalSize.modal_md;
            getModalDetail.modaltitle = (PR_ID == 0 ? "Add Page Rights Setup" : "Edit Add Page Rights Setup");
            getModalDetail.modalfootercancelbuttonname = "Cancel";
            getModalDetail.modalfootersuccessbuttonname = (PR_ID == 0 ? "Add" : "Update");
            getModalDetail.modalBodyTypeInfos = new List<ModalBodyTypeInfo>();

            getModalDetail.onclickmodalsuccess = "AddOrEditPageRights()";

            modalBodyTypeInfo = new ModalBodyTypeInfo();
            modalBodyTypeInfo.ModelBodyType = GetModelBodyType.TRInput;
            modalBodyTypeInfo.LabelName = "Page Right ID";
            modalBodyTypeInfo.IsRequired = true;
            modalBodyTypeInfo.GetInputTypeString = GetInputStringType.text;
            modalBodyTypeInfo.PlaceHolder = "Page Right ID";
            modalBodyTypeInfo.id = "modalPageRightId";
            if (PageRightEdit.PR_ID > 0)
            {
                modalBodyTypeInfo.value = PageRightEdit.PR_ID;
            }
            else
            {
                modalBodyTypeInfo.value = "";
            }
            modalBodyTypeInfo.ClassName = "form-control";
            modalBodyTypeInfo.AttributeList = new List<AttributeList>
            {
                new AttributeList { Name = "readonly", Value = "readonly" }
            };
            if (PageRightEdit.PR_ID > 0)
                List_ModalBodyTypeInfo.Add(modalBodyTypeInfo);

            modalBodyTypeInfo = new ModalBodyTypeInfo();
            modalBodyTypeInfo.ModelBodyType = GetModelBodyType.TRselect;
            modalBodyTypeInfo.LabelName = "Page Name";
            modalBodyTypeInfo.IsRequired = true;
            modalBodyTypeInfo.GetInputTypeString = GetInputStringType.text;
            modalBodyTypeInfo.id = "modalSelectPageName";
            if (PageRightEdit.P_ID > 0)
            {
                modalBodyTypeInfo.IsSelectOption = true;
                modalBodyTypeInfo.value = PageRightEdit.P_ID;
            }
            else
            {
                modalBodyTypeInfo.value = "";
            }
            modalBodyTypeInfo.selectLists = PageList;
            modalBodyTypeInfo.ClassName = "form-control";
            modalBodyTypeInfo.AttributeList = new List<AttributeList>
            {
                new AttributeList(){Name = "onfocus", Value = "validate(this)"},
                new AttributeList(){Name = "onkeydown", Value = "validate(this)"},
                new AttributeList(){Name = "onchange", Value = "validate(this)"},
                new AttributeList(){Name = "autocomplete", Value = "off"}
            };
            List_ModalBodyTypeInfo.Add(modalBodyTypeInfo);

            modalBodyTypeInfo = new ModalBodyTypeInfo();
            modalBodyTypeInfo.ModelBodyType = GetModelBodyType.TRInput;
            modalBodyTypeInfo.LabelName = "PR Code";
            modalBodyTypeInfo.IsRequired = true;
            modalBodyTypeInfo.GetInputTypeString = GetInputStringType.text;
            modalBodyTypeInfo.PlaceHolder = "PR Code";
            modalBodyTypeInfo.id = "modalPageRightCode";
            if (PageRightEdit.PR_CODE != "")
            {
                modalBodyTypeInfo.value = PageRightEdit.PR_CODE;
            }
            else
            {
                modalBodyTypeInfo.value = "";
            }
            modalBodyTypeInfo.ClassName = "form-control";
            modalBodyTypeInfo.AttributeList = new List<AttributeList>
            {
                new AttributeList(){Name = "onfocus", Value = "validate(this)"},
                new AttributeList(){Name = "onkeydown", Value = "validate(this)"},
                new AttributeList(){Name = "onchange", Value = "validate(this)"},
                new AttributeList(){Name = "autocomplete", Value = "off"}
            };
            List_ModalBodyTypeInfo.Add(modalBodyTypeInfo);

            modalBodyTypeInfo = new ModalBodyTypeInfo();
            modalBodyTypeInfo.ModelBodyType = GetModelBodyType.TRInput;
            modalBodyTypeInfo.LabelName = "Page Right Name";
            modalBodyTypeInfo.IsRequired = true;
            modalBodyTypeInfo.GetInputTypeString = GetInputStringType.text;
            modalBodyTypeInfo.PlaceHolder = "Page Right Name";
            modalBodyTypeInfo.id = "modalPageRightName";
            if (PageRightEdit.PageRightName != "")
            {
                modalBodyTypeInfo.value = PageRightEdit.PageRightName;
            }
            else
            {
                modalBodyTypeInfo.value = "";
            }
            modalBodyTypeInfo.ClassName = "form-control";
            modalBodyTypeInfo.AttributeList = new List<AttributeList>
            {
                new AttributeList(){Name = "onfocus", Value = "validate(this)"},
                new AttributeList(){Name = "onkeydown", Value = "validate(this)"},
                new AttributeList(){Name = "onchange", Value = "validate(this)"},
                new AttributeList(){Name = "autocomplete", Value = "off"}
            };
            List_ModalBodyTypeInfo.Add(modalBodyTypeInfo);

            modalBodyTypeInfo = new ModalBodyTypeInfo();
            modalBodyTypeInfo.ModelBodyType = GetModelBodyType.TRselect;
            modalBodyTypeInfo.LabelName = "Page Right Type";
            modalBodyTypeInfo.IsRequired = true;
            modalBodyTypeInfo.GetInputTypeString = GetInputStringType.text;
            modalBodyTypeInfo.id = "modalPageRightType";
            if (PageRightEdit.PageRightType != "")
            {
                modalBodyTypeInfo.IsSelectOption = true;
                modalBodyTypeInfo.value = PageRightEdit.PageRightType;
            }
            else
            {
                modalBodyTypeInfo.value = "";
            }
            modalBodyTypeInfo.selectLists = PageRightTypeList;
            modalBodyTypeInfo.ClassName = "form-control";
            modalBodyTypeInfo.AttributeList = new List<AttributeList>
            {
                new AttributeList(){Name = "onfocus", Value = "validate(this)"},
                new AttributeList(){Name = "onkeydown", Value = "validate(this)"},
                new AttributeList(){Name = "onchange", Value = "validate(this)"},
                new AttributeList(){Name = "autocomplete", Value = "off"}
            };
            List_ModalBodyTypeInfo.Add(modalBodyTypeInfo);

            modalBodyTypeInfo = new ModalBodyTypeInfo();
            modalBodyTypeInfo.ModelBodyType = GetModelBodyType.TRCheckBox;
            modalBodyTypeInfo.LabelName = "Is Hide";
            modalBodyTypeInfo.IsRequired = false;
            modalBodyTypeInfo.GetInputTypeString = GetInputStringType.checkbox;
            modalBodyTypeInfo.PlaceHolder = "Is Hide";
            modalBodyTypeInfo.id = "modalprishide";
            if (PageRightEdit.PR_ID > 0)
            {
                modalBodyTypeInfo.ischecked = PageRightEdit.IsHide;
            }
            else
            {
                modalBodyTypeInfo.ischecked = false;
            }
            List_ModalBodyTypeInfo.Add(modalBodyTypeInfo);

            modalBodyTypeInfo = new ModalBodyTypeInfo();
            modalBodyTypeInfo.ModelBodyType = GetModelBodyType.TRCheckBox;
            modalBodyTypeInfo.LabelName = "Is Active";
            modalBodyTypeInfo.IsRequired = false;
            modalBodyTypeInfo.GetInputTypeString = GetInputStringType.checkbox;
            modalBodyTypeInfo.PlaceHolder = "Is Active";
            modalBodyTypeInfo.id = "modalprisactive";
            modalBodyTypeInfo.value = "";
            if (PageRightEdit.PR_ID > 0)
            {
                modalBodyTypeInfo.ischecked = PageRightEdit.Active;
            }
            else
            {
                modalBodyTypeInfo.ischecked = true;
            }
            List_ModalBodyTypeInfo.Add(modalBodyTypeInfo);

            getModalDetail.modalBodyTypeInfos = List_ModalBodyTypeInfo;

            HtmlString = ModalFunctions.GetModalWithBody(getModalDetail);

            return HtmlString;
        }
        [HttpPost]
        public IActionResult GetAddEditRolePageRightsModal([FromBody] int RoleID)
        {
            bool IsAdd = StaticPublicObjects.ado.P_Is_Has_Right_From_Username_And_PR_ID_From_Memory(_PublicClaimObjects.username, RightsList_ID.Right_Setup_Add);
            bool IsEdit = StaticPublicObjects.ado.P_Is_Has_Right_From_Username_And_PR_ID_From_Memory(_PublicClaimObjects.username, RightsList_ID.Right_Setup_Edit);
            if ((IsAdd == false) || (IsEdit == false))
                return Content(JsonConvert.SerializeObject("No Rights"));

            List<Dynamic_SP_Params> _parms = new List<Dynamic_SP_Params>()
            {
                new Dynamic_SP_Params {ParameterName = "RoleID", Val = RoleID},
            };
            string json = StaticPublicObjects.ado.P_Get_MultiParm_String_Result("P_Get_Role_Rights_Json", _parms);
            P_AddOrEdit_RolePageRights_TreeView treeViewResult = null!;
            if (json != "")
            {
                treeViewResult = new P_AddOrEdit_RolePageRights_TreeView();
                treeViewResult = JsonConvert.DeserializeObject<P_AddOrEdit_RolePageRights_TreeView>(json)!;
            }
            return Content(JsonConvert.SerializeObject(treeViewResult));
        }
        [HttpPost]
        public IActionResult GetAddEditUserRoleMappingModal([FromBody] string UserName)
        {
            bool IsAdd = StaticPublicObjects.ado.P_Is_Has_Right_From_Username_And_PR_ID_From_Memory(_PublicClaimObjects!.username, RightsList_ID.Right_Setup_Add);
            bool IsEdit = StaticPublicObjects.ado.P_Is_Has_Right_From_Username_And_PR_ID_From_Memory(_PublicClaimObjects.username, RightsList_ID.Right_Setup_Edit);

            if ((IsAdd == false) || (IsEdit == false))
                return Content(JsonConvert.SerializeObject("No Rights"));

            int RoleID = StaticPublicObjects.ado.Get_RoleID_From_UserName(UserName);

            List<Dynamic_SP_Params> _parms = new List<Dynamic_SP_Params>()
            {
                new Dynamic_SP_Params {ParameterName = "RoleID", Val = RoleID}
            };
            string json = StaticPublicObjects.ado.P_Get_MultiParm_String_Result("P_Get_Role_Rights_Json", _parms);
            P_AddOrEdit_RolePageRights_TreeView treeViewResult = null!;
            if (json != "")
            {
                treeViewResult = new P_AddOrEdit_RolePageRights_TreeView();
                treeViewResult = JsonConvert.DeserializeObject<P_AddOrEdit_RolePageRights_TreeView>(json)!;
            }
            return Content(JsonConvert.SerializeObject(treeViewResult));
        }

        [HttpPost]
        public string GetAddEditUserRoleMappingModal1([FromBody] string Ery_URM_ID)
        {
            int URM_ID = Crypto.DecryptNumericToStringWithOutNull(Ery_URM_ID);
            string HtmlString = "";
            DataSet DS = new DataSet();

            GetModalDetail getModalDetail = new GetModalDetail();
            List<ModalBodyTypeInfo> List_ModalBodyTypeInfo = new List<ModalBodyTypeInfo>();
            ModalBodyTypeInfo modalBodyTypeInfo = new ModalBodyTypeInfo();

            P_AddOrEdit_UserRoleMapping_Response_1 UserRoleMappingEdit = new P_AddOrEdit_UserRoleMapping_Response_1();
            if (URM_ID > 0)
            {
                List<Dynamic_SP_Params> List_Dynamic_SP_Params = new List<Dynamic_SP_Params>();
                Dynamic_SP_Params Dynamic_SP_Params = new Dynamic_SP_Params();
                Dynamic_SP_Params.ParameterName = "URM_ID";
                Dynamic_SP_Params.Val = URM_ID;
                List_Dynamic_SP_Params.Add(Dynamic_SP_Params);
                UserRoleMappingEdit = StaticPublicObjects.ado.ExecuteSelectSQLMap<P_AddOrEdit_UserRoleMapping_Response_1>("SELECT URM_ID, USERNAME UNAME, ROLE_ID R_ID, IsGroupRoleID, IsActive Active FROM [dbo].[T_User_Role_Mapping] with (nolock) WHERE URM_ID = @URM_ID", false, 0, ref List_Dynamic_SP_Params);
            }

            List<SelectDropDownList> UserList = new List<SelectDropDownList>();
            List<SelectDropDownList> RoleList = new List<SelectDropDownList>();
            RoleList = StaticPublicObjects.ado.Get_DropDownList_Result("SELECT R_ID code, RoleName name FROM [dbo].[T_Roles] with (nolock) WHERE isActive = 1 ORDER BY Sort_");

            getModalDetail.getmodelsize = GetModalSize.modal_md;
            getModalDetail.modaltitle = (URM_ID == 0 ? "Add User Role Mapping Setup" : "Edit Add User Role Mapping Setup");
            getModalDetail.modalfootercancelbuttonname = "Cancel";
            getModalDetail.modalfootersuccessbuttonname = (URM_ID == 0 ? "Add" : "Update");
            getModalDetail.modalBodyTypeInfos = new List<ModalBodyTypeInfo>();

            getModalDetail.onclickmodalsuccess = "AddOrEditUserRoleMapping1()";

            modalBodyTypeInfo = new ModalBodyTypeInfo();
            modalBodyTypeInfo.ModelBodyType = GetModelBodyType.TRInput;
            modalBodyTypeInfo.LabelName = "User Role Mapping ID";
            modalBodyTypeInfo.IsRequired = true;
            modalBodyTypeInfo.IsHidden = true;
            modalBodyTypeInfo.GetInputTypeString = GetInputStringType.text;
            modalBodyTypeInfo.PlaceHolder = "User Role Mapping ID";
            modalBodyTypeInfo.id = "modalUserRoleMappingID";
            if (UserRoleMappingEdit.URM_ID > 0)
            {
                modalBodyTypeInfo.value = UserRoleMappingEdit.URM_ID;
            }
            else
            {
                modalBodyTypeInfo.value = "";
            }
            modalBodyTypeInfo.ClassName = "form-control";
            modalBodyTypeInfo.AttributeList = new List<AttributeList>
        {
            new AttributeList { Name = "readonly", Value = "readonly" }
        };
            if (UserRoleMappingEdit.URM_ID > 0)
                List_ModalBodyTypeInfo.Add(modalBodyTypeInfo);

            modalBodyTypeInfo = new ModalBodyTypeInfo();
            modalBodyTypeInfo.ModelBodyType = GetModelBodyType.TRCheckBox;
            modalBodyTypeInfo.LabelName = "Is Group Role ID";
            modalBodyTypeInfo.IsRequired = false;
            modalBodyTypeInfo.GetInputTypeString = GetInputStringType.checkbox;
            modalBodyTypeInfo.PlaceHolder = "Is Group Role ID";
            modalBodyTypeInfo.id = "modalIsGroupRoleID";
            if (UserRoleMappingEdit.R_ID > 0)
            {
                modalBodyTypeInfo.ischecked = UserRoleMappingEdit.IsGroupRoleID;
            }
            else
            {
                modalBodyTypeInfo.ischecked = false;
            }
            modalBodyTypeInfo.AttributeList = new List<AttributeList>
        {
            new AttributeList() { Name = "onchange", Value = "Load_Users_By_GroupRole_Dropdown(this.checked)" }
        };
            List_ModalBodyTypeInfo.Add(modalBodyTypeInfo);

            modalBodyTypeInfo = new ModalBodyTypeInfo();
            modalBodyTypeInfo.ModelBodyType = GetModelBodyType.TRselect;
            modalBodyTypeInfo.LabelName = "Role Name";
            modalBodyTypeInfo.IsRequired = true;
            modalBodyTypeInfo.GetInputTypeString = GetInputStringType.text;
            modalBodyTypeInfo.id = "modalurmSelectRoleName";
            if (UserRoleMappingEdit.R_ID > 0)
            {
                modalBodyTypeInfo.IsSelectOption = true;
                modalBodyTypeInfo.value = UserRoleMappingEdit.R_ID;
            }
            else
            {
                modalBodyTypeInfo.value = "";
            }
            modalBodyTypeInfo.selectLists = RoleList;
            modalBodyTypeInfo.ClassName = "form-control";
            if (URM_ID > 0)
            {
                modalBodyTypeInfo.AttributeList = new List<AttributeList>
            {
                new AttributeList(){Name = "onfocus", Value = "validate(this)"},
                new AttributeList(){Name = "onkeydown", Value = "validate(this)"},
                new AttributeList(){ Name = "onchange", Value = "validate(this)" },
                new AttributeList(){Name = "autocomplete", Value = "off"}
            };
            }
            else
            {
                modalBodyTypeInfo.AttributeList = new List<AttributeList>
            {
                new AttributeList(){Name = "onfocus", Value = "validate(this)"},
                new AttributeList(){Name = "onkeydown", Value = "validate(this)"},
                new AttributeList { Name = "onchange", Value = "Load_Users_By_Role_Dropdown(this.value)" },
                new AttributeList(){Name = "autocomplete", Value = "off"}
            };
            }
            List_ModalBodyTypeInfo.Add(modalBodyTypeInfo);

            if (UserRoleMappingEdit.URM_ID > 0)
            {
                modalBodyTypeInfo = new ModalBodyTypeInfo();
                modalBodyTypeInfo.ModelBodyType = GetModelBodyType.TRInput;
                modalBodyTypeInfo.LabelName = "User Name";
                modalBodyTypeInfo.IsRequired = true;
                modalBodyTypeInfo.GetInputTypeString = GetInputStringType.text;
                modalBodyTypeInfo.id = "modalurmUserName";
                modalBodyTypeInfo.value = UserRoleMappingEdit.UNAME;
                modalBodyTypeInfo.ClassName = "form-control";
                modalBodyTypeInfo.AttributeList = new List<AttributeList>
            {
                new AttributeList(){Name = "disabled", Value = "disabled"},
                new AttributeList(){Name = "readonly", Value = "readonly"},
                new AttributeList(){Name = "autocomplete", Value = "off"}
            };
                List_ModalBodyTypeInfo.Add(modalBodyTypeInfo);
            }
            else
            {
                modalBodyTypeInfo = new ModalBodyTypeInfo();
                modalBodyTypeInfo.ModelBodyType = GetModelBodyType.TRselect;
                modalBodyTypeInfo.LabelName = "User Name";
                modalBodyTypeInfo.IsRequired = true;
                modalBodyTypeInfo.GetInputTypeString = GetInputStringType.text;
                modalBodyTypeInfo.id = "modalurmUserName";

                if (UserRoleMappingEdit.UNAME != "")
                {
                    modalBodyTypeInfo.value = UserRoleMappingEdit.UNAME;
                }
                else
                {
                    modalBodyTypeInfo.value = "";
                }

                modalBodyTypeInfo.selectLists = UserList;
                modalBodyTypeInfo.ClassName = "form-control";
                modalBodyTypeInfo.AttributeList = new List<AttributeList>
            {
                new AttributeList(){Name = "onfocus", Value = "validate(this)"},
                new AttributeList(){Name = "onkeydown", Value = "validate(this)"},
                new AttributeList(){Name = "onchange", Value = "validate(this)"},
                new AttributeList(){Name = "autocomplete", Value = "off"}
            };
                List_ModalBodyTypeInfo.Add(modalBodyTypeInfo);
            }

            modalBodyTypeInfo = new ModalBodyTypeInfo();
            modalBodyTypeInfo.ModelBodyType = GetModelBodyType.TRCheckBox;
            modalBodyTypeInfo.LabelName = "Is Active";
            modalBodyTypeInfo.IsRequired = false;
            modalBodyTypeInfo.ischecked = true;
            modalBodyTypeInfo.GetInputTypeString = GetInputStringType.checkbox;
            modalBodyTypeInfo.PlaceHolder = "Is Active";
            modalBodyTypeInfo.id = "modalurmisactive";
            modalBodyTypeInfo.value = "";
            if (UserRoleMappingEdit.R_ID > 0)
            {
                modalBodyTypeInfo.ischecked = UserRoleMappingEdit.Active;
            }
            else
            {
                modalBodyTypeInfo.ischecked = true;
            }
            List_ModalBodyTypeInfo.Add(modalBodyTypeInfo);

            getModalDetail.modalBodyTypeInfos = List_ModalBodyTypeInfo;

            HtmlString = ModalFunctions.GetModalWithBody(getModalDetail);

            return HtmlString;
        }


        [HttpPost]
        public IActionResult AddOrEdit_PageRights([FromBody] P_AddOrEdit_PageRights_Response res)
        {
            bool IsAdd = StaticPublicObjects.ado.P_Is_Has_Right_From_Username_And_PR_ID_From_Memory(_PublicClaimObjects.username, RightsList_ID.Right_Setup_Add);
            bool IsEdit = StaticPublicObjects.ado.P_Is_Has_Right_From_Username_And_PR_ID_From_Memory(_PublicClaimObjects.username, RightsList_ID.Right_Setup_Edit);
            if ((IsAdd == false) || (IsEdit == false))
                return Content(JsonConvert.SerializeObject("No Rights"));


            P_ReturnMessage_Result response = StaticPublicObjects.ado.P_SP_MultiParm_Result("P_AddOrEdit_Page_Rights", res, _PublicClaimObjects.username);
            if (response.ReturnCode == false)
                StaticPublicObjects.logFile.ErrorLog(FunctionName: "AddOrEdit_PageRights", SmallMessage: response.ReturnText!, Message: response.ReturnText!);
            return Content(JsonConvert.SerializeObject(response));
        }

        [HttpPost]
        public IActionResult AddOrEdit_RolePageRights([FromBody] P_AddOrEdit_RolePageRights_TreeView res)
        {
            bool IsAdd = StaticPublicObjects.ado.P_Is_Has_Right_From_Username_And_PR_ID_From_Memory(_PublicClaimObjects!.username, RightsList_ID.Right_Setup_Add);
            bool IsEdit = StaticPublicObjects.ado.P_Is_Has_Right_From_Username_And_PR_ID_From_Memory(_PublicClaimObjects.username, RightsList_ID.Right_Setup_Edit);
            if ((IsAdd == false) || (IsEdit == false))
                return Content(JsonConvert.SerializeObject("No Rights"));

            List<P_AddOrEdit_RolePageRights_Response> RPRM_Response_List = res.pageGroupInfo
                .SelectMany(pg => pg.pageInfo
                .SelectMany(p => p.pageRightsInfo
                .Select(pr => new P_AddOrEdit_RolePageRights_Response
                {
                    R_ID = res.R_ID,
                    IsRightActive = true,
                    Active = true,
                    PR_ID = pr.PR_ID
                }))).ToList();
            var json = JsonConvert.SerializeObject(RPRM_Response_List);
            P_ReturnMessage_Result response = StaticPublicObjects.ado.P_SP_SingleParm_Result("P_AddOrEdit_RolePageRight_Json", "json", json, _PublicClaimObjects.username);
            if (response.ReturnCode == false)
                StaticPublicObjects.logFile.ErrorLog(FunctionName: "AddOrEdit_RolePageRights", SmallMessage: response.ReturnText!, Message: response.ReturnText!);
            return Content(JsonConvert.SerializeObject(response));
        }
        [HttpPost]
        public IActionResult Sync_RolePageRights([FromBody] P_Sync_RolePageRights_TreeView res)
        {
            bool IsAdd = StaticPublicObjects.ado.P_Is_Has_Right_From_Username_And_PR_ID_From_Memory(_PublicClaimObjects!.username, RightsList_ID.Right_Setup_Add);
            bool IsEdit = StaticPublicObjects.ado.P_Is_Has_Right_From_Username_And_PR_ID_From_Memory(_PublicClaimObjects.username, RightsList_ID.Right_Setup_Edit);
            if ((IsAdd == false) || (IsEdit == false))
            {
                return Content(JsonConvert.SerializeObject("No Rights"));
            }
            else
            {
                bool? ActiveRights = null;
                if (res.Active == false)
                {
                    res.Active = null;
                }

                P_ReturnMessage_Result response = StaticPublicObjects.ado.P_SP_MultiParm_Result("P_Sync_RolePageRights", res, _PublicClaimObjects.username);
                if (response.ReturnCode == false)
                    StaticPublicObjects.logFile.ErrorLog(FunctionName: "Sync_RolePageRights", SmallMessage: response.ReturnText!, Message: response.ReturnText!);
                return Content(JsonConvert.SerializeObject(response));
            }
        }

        [HttpPost]
        public IActionResult AddOrEdit_UserRoleMap([FromBody] P_AddOrEdit_UserRolePageRights_TreeView res)
        {
            bool IsAdd = StaticPublicObjects.ado.P_Is_Has_Right_From_Username_And_PR_ID_From_Memory(_PublicClaimObjects!.username, RightsList_ID.Right_Setup_Add);
            bool IsEdit = StaticPublicObjects.ado.P_Is_Has_Right_From_Username_And_PR_ID_From_Memory(_PublicClaimObjects.username, RightsList_ID.Right_Setup_Edit);
            if ((IsAdd == false) || (IsEdit == false))
                return Content(JsonConvert.SerializeObject("No Rights"));

            List<P_AddOrEdit_UserRoleMapping_Response> RPRM_Response_List = res.pageGroupInfo!
               .SelectMany(pg => pg.pageInfo!
               .SelectMany(p => p.pageRightsInfo!
               .Select(pr => new P_AddOrEdit_UserRoleMapping_Response
               {
                   R_ID = res.R_ID,
                   IsRightActive = true,
                   Active = true,
                   PR_ID = pr.PR_ID,
                   UserName = res.UserName,
               }))).ToList();

            var json = JsonConvert.SerializeObject(RPRM_Response_List);

            P_ReturnMessage_Result response = StaticPublicObjects.ado.P_AddOrEdit_User_Role_Map(json);
            if (response.ReturnCode == false)
                StaticPublicObjects.logFile.ErrorLog(FunctionName: "AddOrEdit_UserRoleMap", SmallMessage: response.ReturnText!, Message: response.ReturnText!);

            return Content(JsonConvert.SerializeObject(response));
        }
        [HttpPost]
        public IActionResult Sync_UserRolePageRights([FromBody] P_Sync_UserRolePageRights_TreeView req)
        {
            bool IsAdd = StaticPublicObjects.ado.P_Is_Has_Right_From_Username_And_PR_ID_From_Memory(_PublicClaimObjects!.username, RightsList_ID.Right_Setup_Add);
            bool IsEdit = StaticPublicObjects.ado.P_Is_Has_Right_From_Username_And_PR_ID_From_Memory(_PublicClaimObjects.username, RightsList_ID.Right_Setup_Edit);
            if ((IsAdd == false) || (IsEdit == false))
            {
                return Content(JsonConvert.SerializeObject("No Rights"));
            }
            else
            {
                bool? ActiveRights = null;
                if (req.Active == false)
                {
                    req.Active = null;
                }

                P_Sync_RolePageRights_TreeView res = new P_Sync_RolePageRights_TreeView();
                res.RoleID = StaticPublicObjects.ado.Get_RoleID_From_UserName(req.UserName!);
                res.RoleIDCompare = StaticPublicObjects.ado.Get_RoleID_From_UserName(req.UserNameCompare!);
                res.CopyR_ID = req.CopyR_ID;
                res.CopyPG_ID = req.CopyPG_ID;
                res.CopyP_ID = req.CopyP_ID;
                res.Active = req.Active;

                P_ReturnMessage_Result response = StaticPublicObjects.ado.P_SP_MultiParm_Result("P_Sync_RolePageRights", res, _PublicClaimObjects.username);
                if (response.ReturnCode == false)
                    StaticPublicObjects.logFile.ErrorLog(FunctionName: "Sync_RolePageRights", SmallMessage: response.ReturnText!, Message: response.ReturnText!);
                return Content(JsonConvert.SerializeObject(response));
            }
        }

        [HttpPost]
        public ActionResult AddOrEdit_UserRoleMap1([FromBody] P_AddOrEdit_UserRoleMapping_Response_1 response)
        {
            P_ReturnMessage_Result submitRoleResponse = new P_ReturnMessage_Result();
            submitRoleResponse = StaticPublicObjects.ado.P_SP_MultiParm_Result("P_AddOrEdit_User_Role_Map_1", response, _PublicClaimObjects.username, "");

            if (submitRoleResponse.ReturnCode == false)
                StaticPublicObjects.logFile.ErrorLog(FunctionName: "AddOrEdit_PageRights", SmallMessage: submitRoleResponse.ReturnText, Message: submitRoleResponse.ReturnText);

            return Content(JsonConvert.SerializeObject(submitRoleResponse));
        }


        [HttpPost]
        public IActionResult Remove_PageRights([FromBody] int PR_ID)
        {
            bool IsDelete = StaticPublicObjects.ado.P_Is_Has_Right_From_Username_And_PR_ID_From_Memory(_PublicClaimObjects!.username, RightsList_ID.Right_Setup_Delete);
            if (IsDelete)
            {
                P_ReturnMessage_Result response = StaticPublicObjects.ado.P_SP_Remove_Generic_Result("T_Page_Rights", "PR_ID", PR_ID);
                if (response.ReturnCode == false)
                    StaticPublicObjects.logFile.ErrorLog(FunctionName: "Remove_PageRights", SmallMessage: response.ReturnText!, Message: response.ReturnText!);
                return Content(JsonConvert.SerializeObject(response));
            }
            else { return Content(JsonConvert.SerializeObject("No Rights")); }
        }
        [HttpPost]
        public IActionResult Remove_RolePageRights([FromBody] int RPRM_ID)
        {
            bool IsDelete = StaticPublicObjects.ado.P_Is_Has_Right_From_Username_And_PR_ID_From_Memory(_PublicClaimObjects!.username, RightsList_ID.Right_Setup_Delete);
            if (IsDelete)
            {
                P_ReturnMessage_Result response = StaticPublicObjects.ado.P_SP_Remove_Generic_Result("T_Role_Page_Rights_Mapping", "RPRM_ID", RPRM_ID);
                if (response.ReturnCode == false)
                    StaticPublicObjects.logFile.ErrorLog(FunctionName: "Remove_RolePageRights", SmallMessage: response.ReturnText!, Message: response.ReturnText!);
                return Content(JsonConvert.SerializeObject(response));
            }
            else
            {
                return Content(JsonConvert.SerializeObject("No Rights"));
            }
        }
        [HttpPost]
        public IActionResult Remove_UserRoleMap([FromBody] int URM_ID)
        {
            bool IsDelete = StaticPublicObjects.ado.P_Is_Has_Right_From_Username_And_PR_ID_From_Memory(_PublicClaimObjects!.username, RightsList_ID.Right_Setup_Delete);
            if (IsDelete)
            {
                P_ReturnMessage_Result response = StaticPublicObjects.ado.P_SP_Remove_Generic_Result("T_User_Role_Mapping", "URM_ID", URM_ID);
                if (response.ReturnCode == false)
                    StaticPublicObjects.logFile.ErrorLog(FunctionName: "Remove_UserRoleMap", SmallMessage: response.ReturnText!, Message: response.ReturnText!);
                return Content(JsonConvert.SerializeObject(response));
            }
            else
            {
                return Content(JsonConvert.SerializeObject("No Rights"));
            }
        }


        [HttpPost]
        public IActionResult Get_Roles_Dropdown()
        {
            List<SelectDropDownList> result = StaticPublicObjects.ado.Get_DropDownList_Result("SELECT code = R_ID, name = RoleName FROM [dbo].[T_Roles] with (nolock) WHERE R_ID NOT IN (1) ORDER BY Sort_");
            return Content(JsonConvert.SerializeObject(result));
        }

        [HttpPost]
        public ActionResult Get_Users_By_Role_Dropdown([FromBody] int R_ID)
        {
            List<Dynamic_SP_Params> parms = new List<Dynamic_SP_Params>()
            {
                 new Dynamic_SP_Params {ParameterName = "R_ID", Val = R_ID}
            };
            List<SelectDropDownList> result = StaticPublicObjects.ado.ExecuteSelectSQLMapList<SelectDropDownList>("SELECT code = r.USERNAME, name = r.USERNAME FROM [dbo].[T_Users] r with (nolock) INNER JOIN [dbo].[T_User_Role_Mapping] urm with (nolock) ON r.USERNAME = urm.USERNAME WHERE urm.ROLE_ID = @R_ID and urm.IsActive = 1 and r.IsActive = 1", false, 0, ref parms);
            return Content(JsonConvert.SerializeObject(result));
        }

        [HttpPost]
        public IActionResult Get_Users_By_GroupRole_Dropdown([FromBody] bool IsGroupRole)
        {
            List<SelectDropDownList> list = new List<SelectDropDownList>();

            return Content(JsonConvert.SerializeObject(list));
        }

        [HttpPost]
        public IActionResult Get_Users_Dropdown()
        {
            var result = ModalFunctions.GetDropDownListCommon("USERNAME", "USERNAME", "T_Users", true);
            return Content(JsonConvert.SerializeObject(result));
        }
        [HttpPost]
        public IActionResult Get_Application_Dropdown()
        {
            List<Dynamic_SP_Params> _parms = new List<Dynamic_SP_Params>() {
                new Dynamic_SP_Params { ParameterName = "MT_ID", Val = 148}
            };
            List<SelectDropDownList> result = StaticPublicObjects.ado.Get_DropDownList_Result("SELECT code = MTV_ID, name = [Name] FROM [dbo].[T_Master_Type_Value] with (nolock) WHERE MT_ID = @MT_ID ORDER BY Sort_", _parms);
            return Content(JsonConvert.SerializeObject(result));
        }

        //[HttpPost]
        //public IActionResult ExportExcel_RolePageRights()
        //{
        //    List<P_Get_Role_All_Right_Combinations> result = new List<P_Get_Role_All_Right_Combinations>();
        //    try
        //    {
        //        result = StaticPublicObjects.ado.P_Get_Role_All_Right_Combinations();
        //    }
        //    catch (Exception ex)
        //    {
        //        StaticPublicObjects.logFile.ErrorLog(FunctionName: "ExportExcel_RolePageRights", SmallMessage: ex.Message, Message: ex.Message);
        //    }
        //    return Content(JsonConvert.SerializeObject(result));
        //}
        [HttpPost]
        public IActionResult ImportExcel_RolePageRights([FromForm] IFormFile ImportFile)
        {
            P_ReturnMessage_Result result = new P_ReturnMessage_Result();
            try
            {
                if (ImportFile == null || ImportFile.Length == 0)
                {
                    result.ReturnCode = false;
                    result.ReturnText = "No file uploaded.";
                    return Content(JsonConvert.SerializeObject(result));
                }

                var stream = new MemoryStream();
                ImportFile.CopyTo(stream);

                var package = new ExcelPackage(stream);

                ExcelWorksheet worksheet = package.Workbook.Worksheets[0];
                int rowCount = worksheet.Dimension.Rows;
                int colCount = worksheet.Dimension.Columns;

                List<P_Get_Role_All_Right_Combinations_Import> res = new List<P_Get_Role_All_Right_Combinations_Import>();

                for (int row = 2; row <= rowCount; row++)
                {
                    P_Get_Role_All_Right_Combinations_Import item = new P_Get_Role_All_Right_Combinations_Import();
                    item.Role_ID = Convert.ToInt32(worksheet.Cells[row, 1].Value)!;
                    item.Role_Name = Convert.ToString(worksheet.Cells[row, 2].Value)!;
                    item.Right_ID = Convert.ToInt32(worksheet.Cells[row, 3].Value)!;
                    item.Right_Name = Convert.ToString(worksheet.Cells[row, 4].Value)!;
                    item.Right_Type = Convert.ToString(worksheet.Cells[row, 5].Value)!;
                    item.Page_Name = Convert.ToString(worksheet.Cells[row, 6].Value)!;
                    item.PageGroup_Name = Convert.ToString(worksheet.Cells[row, 7].Value)!;
                    item.Application_Name = Convert.ToString(worksheet.Cells[row, 8].Value)!;
                    item.IsRightActive = Convert.ToBoolean(worksheet.Cells[row, 9].Value)!;
                    res.Add(item);
                }

                var Json = JsonConvert.SerializeObject(res);
                result = StaticPublicObjects.ado.P_SP_SingleParm_Result("P_ImportExcel_RolePageRights", "Json", Json, _PublicClaimObjects!.username);
            }
            catch (Exception ex)
            {
                result.ReturnCode = false;
                result.ReturnText = "Internal Error";
                StaticPublicObjects.logFile.ErrorLog(FunctionName: "ExportExcel_RolePageRights", SmallMessage: ex.Message, Message: ex.Message);
            }
            return Content(JsonConvert.SerializeObject(result));
        }
        #endregion  Rights Setup

        #region Master Setup
        [CustomPageSetupAttribute]
        public IActionResult MasterSetup()
        {
            bool IsView = StaticPublicObjects.ado.P_Is_Has_Right_From_Username_And_PR_ID_From_Memory(_PublicClaimObjects.username, RightsList_ID.Master_Setup_View);
            if (IsView)
            {
                bool IsAdd = StaticPublicObjects.ado.P_Is_Has_Right_From_Username_And_PR_ID_From_Memory(_PublicClaimObjects.username, RightsList_ID.Master_Setup_Add);
                bool IsEdit = StaticPublicObjects.ado.P_Is_Has_Right_From_Username_And_PR_ID_From_Memory(_PublicClaimObjects.username, RightsList_ID.Master_Setup_Edit);
                bool IsDelete = StaticPublicObjects.ado.P_Is_Has_Right_From_Username_And_PR_ID_From_Memory(_PublicClaimObjects.username, RightsList_ID.Master_Setup_Delete);
                ViewBag.RightsListObj = new { IsView = IsView, IsAdd = IsAdd, IsEdit = IsEdit, IsDelete = IsDelete };
                ViewBag.RightsList = JsonConvert.SerializeObject(ViewBag.RightsListObj);
                DataSet DS = new DataSet();
                (string Name, object Value)[] ParamsNameValues = new (string, object)[1];
                ParamsNameValues[0] = ("Username", _PublicClaimObjects.username);
                DS = StaticPublicObjects.ado.ExecuteStoreProcedureDS("P_Get_Master_Setup_DropDown_Lists", ParamsNameValues);
                ViewBag.MasterTypeList = JsonConvert.SerializeObject(DS.Tables[0]);
                ViewBag.MasterTypeValueList = JsonConvert.SerializeObject(DS.Tables[1]);

                return View();
            }
            else
            {
                string ID = "";
                Exception exception = new Exception("You Don't Have Rights To Access Master Setup");
                ID = _httpContextAccessor.HttpContext.Session.SetupSessionError("No Rights", "/Security/MasterSetup", "Master Setup", exception);
                return Redirect($"/Error/Index?ID={ID}");
            }
        }
        [HttpPost]
        public IActionResult GetFilterData_MT_List([FromBody] ReportParams _ReportParams)
        {
            ReportResponse reportResponse = new ReportResponse();
            try
            {
                if (StaticPublicObjects.ado.P_Is_Has_Right_From_Username_And_PR_ID_From_Memory(_PublicClaimObjects.username, RightsList_ID.Page_Setup_View))
                {
                    List<Dynamic_SP_Params> List_Dynamic_SP_Params = new List<Dynamic_SP_Params>();
                    ModalFunctions.GetKendoFilter(ref _ReportParams, ref List_Dynamic_SP_Params, _PublicClaimObjects, true);

                    List<P_MT_Result> ResultList = StaticPublicObjects.ado.P_Get_Generic_List_SP<P_MT_Result>("P_Get_MasterType_List", ref List_Dynamic_SP_Params);

                    reportResponse.TotalRowCount = ModalFunctions.GetValueFromReturnParameter<long>(List_Dynamic_SP_Params, "TotalRowCount", typeof(long));
                    reportResponse.ResultData = ResultList;
                    reportResponse.response_code = true;
                }
                else
                {
                    reportResponse.TotalRowCount = 0;
                    reportResponse.ResultData = null;
                    reportResponse.response_code = false;
                }
            }
            catch (Exception ex)
            {
                StaticPublicObjects.logFile.ErrorLog(FunctionName: "GetFilterData_MT_List", SmallMessage: ex.Message, Message: ex.ToString());
                reportResponse.TotalRowCount = 0;
                reportResponse.ResultData = null;
                reportResponse.response_code = false;
            }
            return Globals.GetAjaxJsonReturn(reportResponse);
        }
        [HttpPost]
        public IActionResult GetFilterData_MTV_List([FromBody] ReportParams _ReportParams)
        {
            ReportResponse reportResponse = new ReportResponse();
            try
            {
                if (StaticPublicObjects.ado.P_Is_Has_Right_From_Username_And_PR_ID_From_Memory(_PublicClaimObjects.username, RightsList_ID.Page_Setup_View))
                {
                    List<Dynamic_SP_Params> List_Dynamic_SP_Params = new List<Dynamic_SP_Params>();
                    ModalFunctions.GetKendoFilter(ref _ReportParams, ref List_Dynamic_SP_Params, _PublicClaimObjects, true);

                    List<P_MTV_Result> ResultList = StaticPublicObjects.ado.P_Get_Generic_List_SP<P_MTV_Result>("P_Get_MasterTypeValue_List", ref List_Dynamic_SP_Params);

                    reportResponse.TotalRowCount = ModalFunctions.GetValueFromReturnParameter<long>(List_Dynamic_SP_Params, "TotalRowCount", typeof(long));
                    reportResponse.ResultData = ResultList;
                    reportResponse.response_code = true;
                }
                else
                {
                    reportResponse.TotalRowCount = 0;
                    reportResponse.ResultData = null;
                    reportResponse.response_code = false;
                }
            }
            catch (Exception ex)
            {
                StaticPublicObjects.logFile.ErrorLog(FunctionName: "GetFilterData_MTV_List", SmallMessage: ex.Message, Message: ex.ToString());
                reportResponse.TotalRowCount = 0;
                reportResponse.ResultData = null;
                reportResponse.response_code = false;
            }
            return Globals.GetAjaxJsonReturn(reportResponse);
        }


        [HttpPost]
        public string GetAddEditMTModal([FromBody] int MT_ID)
        {
            string HtmlString = "";

            bool IsAdd = StaticPublicObjects.ado.P_Is_Has_Right_From_Username_And_PR_ID_From_Memory(_PublicClaimObjects.username, RightsList_ID.Master_Setup_Add);
            bool IsEdit = StaticPublicObjects.ado.P_Is_Has_Right_From_Username_And_PR_ID_From_Memory(_PublicClaimObjects.username, RightsList_ID.Master_Setup_Edit);
            if ((MT_ID == 0 && IsAdd == false) || (MT_ID > 0 && IsEdit == false))
                return "No Rights";

            GetModalDetail getModalDetail = new GetModalDetail();
            List<ModalBodyTypeInfo> List_ModalBodyTypeInfo = new List<ModalBodyTypeInfo>();
            ModalBodyTypeInfo modalBodyTypeInfo = new ModalBodyTypeInfo();

            P_AddOrEdit_MT_Response MTEdit = new P_AddOrEdit_MT_Response();
            if (MT_ID > 0)
            {
                List<Dynamic_SP_Params> List_Dynamic_SP_Params = new List<Dynamic_SP_Params>();
                Dynamic_SP_Params Dynamic_SP_Params = new Dynamic_SP_Params();
                Dynamic_SP_Params.ParameterName = "MT_ID";
                Dynamic_SP_Params.Val = MT_ID;
                List_Dynamic_SP_Params.Add(Dynamic_SP_Params);
                MTEdit = StaticPublicObjects.ado.ExecuteSelectSQLMap<P_AddOrEdit_MT_Response>("SELECT MT_ID, MasterTypeName = Name, Description, IsActive Active FROM [dbo].[T_Master_Type] with (nolock) WHERE MT_ID = @MT_ID ORDER BY [Name]", false, 0, ref List_Dynamic_SP_Params);
            }

            getModalDetail.getmodelsize = GetModalSize.modal_md;
            getModalDetail.modaltitle = (MT_ID == 0 ? "Add New Master Type" : "Edit Master Type");
            getModalDetail.modalfootercancelbuttonname = "Cancel";
            getModalDetail.modalfootersuccessbuttonname = (MT_ID == 0 ? "Add" : "Update");
            getModalDetail.modalBodyTypeInfos = new List<ModalBodyTypeInfo>();

            getModalDetail.onclickmodalsuccess = "AddOrEditMT()";

            modalBodyTypeInfo = new ModalBodyTypeInfo();
            modalBodyTypeInfo.ModelBodyType = GetModelBodyType.TRInput;
            modalBodyTypeInfo.LabelName = "Master Type ID";
            modalBodyTypeInfo.IsRequired = true;
            modalBodyTypeInfo.GetInputTypeString = GetInputStringType.text;
            modalBodyTypeInfo.PlaceHolder = "Master Type ID";
            modalBodyTypeInfo.id = "modalMT_ID";
            if (MTEdit.MT_ID > 0)
            {
                modalBodyTypeInfo.value = MTEdit.MT_ID;
            }
            else
            {
                modalBodyTypeInfo.value = "";
            }
            modalBodyTypeInfo.ClassName = "form-control";
            modalBodyTypeInfo.AttributeList = new List<AttributeList>
            {
                new AttributeList { Name = "readonly", Value = "readonly" }
            };
            if (MTEdit.MT_ID > 0)
                List_ModalBodyTypeInfo.Add(modalBodyTypeInfo);

            modalBodyTypeInfo = new ModalBodyTypeInfo();
            modalBodyTypeInfo.ModelBodyType = GetModelBodyType.TRInput;
            modalBodyTypeInfo.LabelName = "Master Type Name";
            modalBodyTypeInfo.IsRequired = true;
            modalBodyTypeInfo.GetInputTypeString = GetInputStringType.text;
            modalBodyTypeInfo.PlaceHolder = "Master Type Name";
            modalBodyTypeInfo.id = "modalMasterTypeName";
            if (MTEdit.MasterTypeName != "")
            {
                modalBodyTypeInfo.value = MTEdit.MasterTypeName;
            }
            else
            {
                modalBodyTypeInfo.value = "";
            }
            modalBodyTypeInfo.ClassName = "form-control";
            modalBodyTypeInfo.AttributeList = new List<AttributeList>
            {
                new AttributeList(){Name = "onfocus", Value = "validate(this)"},
                new AttributeList(){Name = "onkeydown", Value = "validate(this)"},
                new AttributeList(){Name = "autocomplete", Value = "off"}
            };
            List_ModalBodyTypeInfo.Add(modalBodyTypeInfo);

            modalBodyTypeInfo = new ModalBodyTypeInfo();
            modalBodyTypeInfo.ModelBodyType = GetModelBodyType.TRTextArea;
            modalBodyTypeInfo.LabelName = "Description";
            modalBodyTypeInfo.IsRequired = false;
            modalBodyTypeInfo.GetInputTypeString = GetInputStringType.text;
            modalBodyTypeInfo.PlaceHolder = "Description";
            modalBodyTypeInfo.id = "modalDescription";
            if (MTEdit.Description != "")
            {
                modalBodyTypeInfo.value = MTEdit.Description;
            }
            else
            {
                modalBodyTypeInfo.value = "";
            }
            modalBodyTypeInfo.ClassName = "form-control";
            modalBodyTypeInfo.AttributeList = new List<AttributeList>
            {
                new AttributeList(){Name = "autocomplete", Value = "off"}
            };
            List_ModalBodyTypeInfo.Add(modalBodyTypeInfo);

            modalBodyTypeInfo = new ModalBodyTypeInfo();
            modalBodyTypeInfo.ModelBodyType = GetModelBodyType.TRCheckBox;
            modalBodyTypeInfo.LabelName = "Is Active";
            modalBodyTypeInfo.IsRequired = false;
            modalBodyTypeInfo.GetInputTypeString = GetInputStringType.checkbox;
            modalBodyTypeInfo.PlaceHolder = "Is Active";
            modalBodyTypeInfo.id = "modalMTisActive";
            modalBodyTypeInfo.value = "";
            if (MTEdit.MT_ID > 0)
            {
                modalBodyTypeInfo.ischecked = MTEdit.Active;
            }
            else
            {
                modalBodyTypeInfo.ischecked = true;
            }
            List_ModalBodyTypeInfo.Add(modalBodyTypeInfo);

            getModalDetail.modalBodyTypeInfos = List_ModalBodyTypeInfo;

            HtmlString = ModalFunctions.GetModalWithBody(getModalDetail);
            return HtmlString;
        }
        [HttpPost]
        public string GetAddEditMTVModal([FromBody] int MTV_ID)
        {
            string HtmlString = "";

            bool IsAdd = StaticPublicObjects.ado.P_Is_Has_Right_From_Username_And_PR_ID_From_Memory(_PublicClaimObjects.username, RightsList_ID.Master_Setup_Add);
            bool IsEdit = StaticPublicObjects.ado.P_Is_Has_Right_From_Username_And_PR_ID_From_Memory(_PublicClaimObjects.username, RightsList_ID.Master_Setup_Edit);
            if ((MTV_ID == 0 && IsAdd == false) || (MTV_ID > 0 && IsEdit == false))
                return "No Rights";

            DataSet DS = new DataSet();
            GetModalDetail getModalDetail = new GetModalDetail();
            List<ModalBodyTypeInfo> List_ModalBodyTypeInfo = new List<ModalBodyTypeInfo>();
            ModalBodyTypeInfo modalBodyTypeInfo = new ModalBodyTypeInfo();

            P_AddOrEdit_MTV_Response MTVEdit = new P_AddOrEdit_MTV_Response();
            List<SelectDropDownList> MTVDropDownList = new List<SelectDropDownList>();
            List<SelectDropDownList> SubMTVDropDownList = new List<SelectDropDownList>();
            if (MTV_ID > 0)
            {
                List<Dynamic_SP_Params> List_Dynamic_SP_Params = new List<Dynamic_SP_Params>();
                Dynamic_SP_Params Dynamic_SP_Params = new Dynamic_SP_Params();
                Dynamic_SP_Params.ParameterName = "MTV_ID";
                Dynamic_SP_Params.Val = MTV_ID;
                List_Dynamic_SP_Params.Add(Dynamic_SP_Params);
                MTVEdit = StaticPublicObjects.ado.ExecuteSelectSQLMap<P_AddOrEdit_MTV_Response>("SELECT MTV_ID, MT_ID, MTV_CODE, MasterTypeValueName = Name, Sub_MTV_ID ,IsActive Active FROM [dbo].[T_Master_Type_Value] with (nolock) WHERE MTV_ID = @MTV_ID ORDER BY [Name]", false, 0, ref List_Dynamic_SP_Params);

                List<Dynamic_SP_Params> mtv_parms = new List<Dynamic_SP_Params>()
                {
                    new Dynamic_SP_Params {ParameterName = "MT_ID", Val = MTVEdit.MT_ID}
                };
                MTVDropDownList = StaticPublicObjects.ado.Get_DropDownList_Result("SELECT code = MTV_ID , name = Name FROM [dbo].[T_Master_Type_Value] with (nolock) WHERE isActive = 1 AND MT_ID = @MT_ID ORDER BY [Name]", mtv_parms);
            }

            List<SelectDropDownList> MTList = StaticPublicObjects.ado.Get_DropDownList_Result("SELECT code = MT_ID , name = Name FROM [dbo].[T_Master_Type] with (nolock) WHERE isActive = 1 ORDER BY [Name]");

            getModalDetail.getmodelsize = GetModalSize.modal_md;
            getModalDetail.modaltitle = (MTV_ID == 0 ? "Add New Master Type Value" : "Edit Master Type Value");
            getModalDetail.modalfootercancelbuttonname = "Cancel";
            getModalDetail.modalfootersuccessbuttonname = (MTV_ID == 0 ? "Add" : "Update");
            getModalDetail.modalBodyTypeInfos = new List<ModalBodyTypeInfo>();

            getModalDetail.onclickmodalsuccess = "AddOrEditMTV()";

            modalBodyTypeInfo = new ModalBodyTypeInfo();
            modalBodyTypeInfo.ModelBodyType = GetModelBodyType.TRInput;
            modalBodyTypeInfo.LabelName = "Master Type Value ID";
            modalBodyTypeInfo.IsRequired = true;
            modalBodyTypeInfo.IsHidden = true;
            modalBodyTypeInfo.GetInputTypeString = GetInputStringType.text;
            modalBodyTypeInfo.PlaceHolder = "Master Type Value ID";
            modalBodyTypeInfo.id = "modalMTV_ID";
            if (MTVEdit.MTV_ID > 0)
            {
                modalBodyTypeInfo.value = MTVEdit.MTV_ID;
            }
            else
            {
                modalBodyTypeInfo.value = "";
            }
            modalBodyTypeInfo.AttributeList = new List<AttributeList>
            {
                new AttributeList { Name = "readonly", Value = "readonly" }
            };
            if (MTVEdit.MTV_ID > 0)
                List_ModalBodyTypeInfo.Add(modalBodyTypeInfo);

            modalBodyTypeInfo = new ModalBodyTypeInfo();
            modalBodyTypeInfo.ModelBodyType = GetModelBodyType.TRselect;
            modalBodyTypeInfo.LabelName = "Master Type Name";
            modalBodyTypeInfo.IsRequired = true;
            modalBodyTypeInfo.GetInputTypeString = GetInputStringType.text;
            modalBodyTypeInfo.id = "modalSelectMasterTypeName";
            if (MTVEdit.MT_ID > 0)
            {
                modalBodyTypeInfo.IsSelectOption = true;
                modalBodyTypeInfo.value = MTVEdit.MT_ID;
            }
            else
            {
                modalBodyTypeInfo.value = "";
            }
            modalBodyTypeInfo.selectLists = MTList;
            modalBodyTypeInfo.ClassName = "form-control select2";
            modalBodyTypeInfo.AttributeList = new List<AttributeList>
            {
                new AttributeList(){Name = "onfocus", Value = "validate(this)"},
                new AttributeList(){Name = "onkeydown", Value = "validate(this)"},
                new AttributeList(){Name = "onchange", Value = "validate(this)"},
                new AttributeList(){Name = "autocomplete", Value = "off"}
            };
            List_ModalBodyTypeInfo.Add(modalBodyTypeInfo);

            modalBodyTypeInfo = new ModalBodyTypeInfo();
            modalBodyTypeInfo.ModelBodyType = GetModelBodyType.TRInput;
            modalBodyTypeInfo.LabelName = "Master Type Value Code";
            modalBodyTypeInfo.IsRequired = false;
            modalBodyTypeInfo.GetInputTypeString = GetInputStringType.text;
            modalBodyTypeInfo.PlaceHolder = "Master Type Value Code";
            modalBodyTypeInfo.id = "modalMasterTypeValueCode";
            if (MTVEdit.MT_ID > 0)
            {
                modalBodyTypeInfo.value = MTVEdit.MTV_CODE;
            }
            else
            {
                modalBodyTypeInfo.value = "";
            }
            modalBodyTypeInfo.ClassName = "form-control";
            modalBodyTypeInfo.AttributeList = new List<AttributeList>
            {
                new AttributeList(){Name = "autocomplete", Value = "off"}
            };
            List_ModalBodyTypeInfo.Add(modalBodyTypeInfo);

            modalBodyTypeInfo = new ModalBodyTypeInfo();
            modalBodyTypeInfo.ModelBodyType = GetModelBodyType.TRInput;
            modalBodyTypeInfo.LabelName = "Master Type Value Name";
            modalBodyTypeInfo.IsRequired = true;
            modalBodyTypeInfo.GetInputTypeString = GetInputStringType.text;
            modalBodyTypeInfo.PlaceHolder = "Master Type Value Name";
            modalBodyTypeInfo.id = "modalMasterTypeValueName";
            if (MTVEdit.MT_ID > 0)
            {
                modalBodyTypeInfo.value = MTVEdit.MasterTypeValueName;
            }
            else
            {
                modalBodyTypeInfo.value = "";
            }
            modalBodyTypeInfo.ClassName = "form-control";
            List_ModalBodyTypeInfo.Add(modalBodyTypeInfo);


            modalBodyTypeInfo = new ModalBodyTypeInfo();
            modalBodyTypeInfo.ModelBodyType = GetModelBodyType.TRselect;
            modalBodyTypeInfo.LabelName = "Sub Master Type Name";
            modalBodyTypeInfo.IsRequired = false;
            modalBodyTypeInfo.GetInputTypeString = GetInputStringType.text;
            modalBodyTypeInfo.id = "modalMasterTypeNameForFilter";
            int Sub_MT_ID = 0;
            if (MTVEdit.Sub_MTV_ID > 0)
            {
                object Sub_MT_ID_obj;
                (string Name, object Value)[] ParamsNameValues = new (string, object)[1];
                ParamsNameValues[0] = ("Sub_MTV_ID", MTVEdit.Sub_MTV_ID);
                Sub_MT_ID_obj = StaticPublicObjects.ado.ExecuteSelectObj("select MT_ID from [dbo].[T_Master_Type_Value] mtv with (nolock) where mtv.MTV_ID = @Sub_MTV_ID ORDER BY [MT_ID]", ParamsNameValues);
                if (Sub_MT_ID_obj != null)
                {
                    Sub_MT_ID = Convert.ToInt32(Sub_MT_ID_obj);
                }
            }
            if (Sub_MT_ID > 0)
            {
                modalBodyTypeInfo.IsSelectOption = true;
                modalBodyTypeInfo.value = Sub_MT_ID;
            }
            else
            {
                modalBodyTypeInfo.value = "";
            }
            modalBodyTypeInfo.selectLists = MTList;
            modalBodyTypeInfo.ClassName = "form-control select2";
            modalBodyTypeInfo.AttributeList = new List<AttributeList>
            {
                new AttributeList {Name = "onchange", Value = "MTVDynamicDropDown(this.value)"},
            };
            List_ModalBodyTypeInfo.Add(modalBodyTypeInfo);

            modalBodyTypeInfo = new ModalBodyTypeInfo();
            modalBodyTypeInfo.ModelBodyType = GetModelBodyType.TRselect;
            modalBodyTypeInfo.LabelName = "Sub MTV Name";
            modalBodyTypeInfo.IsRequired = false;
            modalBodyTypeInfo.GetInputTypeString = GetInputStringType.text;
            modalBodyTypeInfo.PlaceHolder = "Sub MTV Name";
            modalBodyTypeInfo.id = "modalsubmtvname";
            if (MTVEdit.Sub_MTV_ID > 0)
            {
                modalBodyTypeInfo.value = MTVEdit.Sub_MTV_ID;
                List<Dynamic_SP_Params> sub_mtv_parms = new List<Dynamic_SP_Params>()
                {
                    new Dynamic_SP_Params {ParameterName = "Sub_MT_ID", Val = Sub_MT_ID}
                };
                SubMTVDropDownList = StaticPublicObjects.ado.Get_DropdownList_MT_ID(Sub_MT_ID, _PublicClaimObjects.username);
            }
            else
            {
                modalBodyTypeInfo.value = "";
            }
            modalBodyTypeInfo.selectLists = SubMTVDropDownList;
            modalBodyTypeInfo.ClassName = "form-control";
            List_ModalBodyTypeInfo.Add(modalBodyTypeInfo);

            modalBodyTypeInfo = new ModalBodyTypeInfo();
            modalBodyTypeInfo.ModelBodyType = GetModelBodyType.TRCheckBox;
            modalBodyTypeInfo.LabelName = "Is Active";
            modalBodyTypeInfo.IsRequired = false;
            modalBodyTypeInfo.GetInputTypeString = GetInputStringType.checkbox;
            modalBodyTypeInfo.PlaceHolder = "Is Active";
            modalBodyTypeInfo.id = "modalMTVisActive";
            modalBodyTypeInfo.value = "";
            if (MTVEdit.MTV_ID > 0)
            {
                modalBodyTypeInfo.ischecked = MTVEdit.Active;
            }
            else
            {
                modalBodyTypeInfo.ischecked = true;
            }
            List_ModalBodyTypeInfo.Add(modalBodyTypeInfo);

            getModalDetail.modalBodyTypeInfos = List_ModalBodyTypeInfo;
            HtmlString = ModalFunctions.GetModalWithBody(getModalDetail);
            return HtmlString;
        }


        [HttpPost]
        public IActionResult AddOrEdit_MT([FromBody] P_AddOrEdit_MT_Response res)
        {
            bool IsAdd = StaticPublicObjects.ado.P_Is_Has_Right_From_Username_And_PR_ID_From_Memory(_PublicClaimObjects.username, RightsList_ID.Master_Setup_Add);
            bool IsEdit = StaticPublicObjects.ado.P_Is_Has_Right_From_Username_And_PR_ID_From_Memory(_PublicClaimObjects.username, RightsList_ID.Master_Setup_Edit);
            if ((IsAdd == false) || (IsEdit == false))
                return Content(JsonConvert.SerializeObject("No Rights"));

            P_ReturnMessage_Result response = StaticPublicObjects.ado.P_SP_MultiParm_Result("P_AddOrEdit_MasterType", res, _PublicClaimObjects.username, "");
            if (response.ReturnCode == false)
                StaticPublicObjects.logFile.ErrorLog(FunctionName: "AddOrEdit_MT", SmallMessage: response.ReturnText!, Message: response.ReturnText!);
            return Content(JsonConvert.SerializeObject(response));
        }
        [HttpPost]
        public IActionResult AddOrEdit_MTV([FromBody] P_AddOrEdit_MTV_Response res)
        {
            bool IsAdd = StaticPublicObjects.ado.P_Is_Has_Right_From_Username_And_PR_ID_From_Memory(_PublicClaimObjects.username, RightsList_ID.Master_Setup_Add);
            bool IsEdit = StaticPublicObjects.ado.P_Is_Has_Right_From_Username_And_PR_ID_From_Memory(_PublicClaimObjects.username, RightsList_ID.Master_Setup_Edit);
            if ((IsAdd == false) || (IsEdit == false))
                return Content(JsonConvert.SerializeObject("No Rights"));

            P_ReturnMessage_Result response = StaticPublicObjects.ado.P_SP_MultiParm_Result("P_AddOrEdit_MasterTypeValue", res, _PublicClaimObjects.username);
            if (response.ReturnCode == false)
                StaticPublicObjects.logFile.ErrorLog(FunctionName: "AddOrEdit_MTV", SmallMessage: response.ReturnText!, Message: response.ReturnText!);
            return Content(JsonConvert.SerializeObject(response));
        }


        [HttpPost]
        public IActionResult Remove_MT([FromBody] int MT_ID)
        {
            bool IsDelete = StaticPublicObjects.ado.P_Is_Has_Right_From_Username_And_PR_ID_From_Memory(_PublicClaimObjects!.username, RightsList_ID.Master_Setup_Delete);
            if (IsDelete)
            {
                P_ReturnMessage_Result response = StaticPublicObjects.ado.P_SP_Remove_Generic_Result("T_Master_Type", "MT_ID", MT_ID);
                if (response.ReturnCode == false)
                    StaticPublicObjects.logFile.ErrorLog(FunctionName: "Remove_MT", SmallMessage: response.ReturnText!, Message: response.ReturnText!);
                return Content(JsonConvert.SerializeObject(response));
            }
            else
            {
                return Content(JsonConvert.SerializeObject("No Rights"));
            }
        }
        [HttpPost]
        public IActionResult Remove_MTV([FromBody] int MTV_ID)
        {
            bool IsDelete = StaticPublicObjects.ado.P_Is_Has_Right_From_Username_And_PR_ID_From_Memory(_PublicClaimObjects!.username, RightsList_ID.Master_Setup_Delete);
            if (IsDelete)
            {
                P_ReturnMessage_Result response = StaticPublicObjects.ado.P_SP_Remove_Generic_Result("T_Master_Type_Value", "MTV_ID", MTV_ID);
                if (response.ReturnCode == false)
                    StaticPublicObjects.logFile.ErrorLog(FunctionName: "Remove_MTV", SmallMessage: response.ReturnText!, Message: response.ReturnText!);
                return Content(JsonConvert.SerializeObject(response));
            }
            else
            {
                return Content(JsonConvert.SerializeObject("No Rights"));
            }
        }


        [HttpPost]
        public IActionResult DynamicMTVDropDown([FromBody] int MT_ID)
        {
            List<Dynamic_SP_Params> parms = new List<Dynamic_SP_Params>()
            {
                new Dynamic_SP_Params {ParameterName = "MT_ID", Val = MT_ID}
            };
            List<SelectDropDownList> result = StaticPublicObjects.ado.Get_DropDownList_Result("SELECT code = MTV_ID , name = Name FROM [dbo].[T_Master_Type_Value] with (nolock) WHERE isActive = 1 AND MT_ID = @MT_ID ORDER BY [Name]", parms);
            return Content(JsonConvert.SerializeObject(result));
        }
        #endregion  Master Setup

        #region Audit History Setup
        [CustomPageSetupAttribute]
        public IActionResult AuditHistory()
        {
            bool IsView = StaticPublicObjects.ado.P_Is_Has_Right_From_Username_And_PR_ID_From_Memory(_PublicClaimObjects.username, RightsList_ID.Audit_History_View);
            if (IsView)
            {
                ViewBag.RightsListObj = new { IsView = IsView };
                ViewBag.RightsList = JsonConvert.SerializeObject(ViewBag.RightsListObj);
                DataSet DS = new DataSet();
                (string Name, object Value)[] ParamsNameValues = new (string, object)[1];
                ParamsNameValues[0] = ("Username", _PublicClaimObjects.username);
                DS = StaticPublicObjects.ado.ExecuteStoreProcedureDS("P_Get_Audit_History_DropDown_Lists", ParamsNameValues);
                ViewBag.AuditTypeList = JsonConvert.SerializeObject(DS.Tables[0]);
                ViewBag.AuditSourceList = JsonConvert.SerializeObject(DS.Tables[1]);

                return View();
            }
            else
            {
                string ID = "";
                Exception exception = new Exception("You Don't Have Rights To Access Audit History Setup");
                ID = _httpContextAccessor.HttpContext!.Session.SetupSessionError("No Rights", "/Security/AuditHistory", "Audit History", exception);
                return Redirect($"/Error/Index?ID={ID}");
            }
        }
        [HttpPost]
        public IActionResult GetFilterData_AuditHistory_List([FromBody] ReportParams _ReportParams)
        {
            ReportResponse reportResponse = new ReportResponse();
            try
            {
                if (StaticPublicObjects.ado.P_Is_Has_Right_From_Username_And_PR_ID_From_Memory(_PublicClaimObjects!.username, RightsList_ID.Audit_History_View))
                {
                    List<Dynamic_SP_Params> List_Dynamic_SP_Params = new List<Dynamic_SP_Params>();
                    ModalFunctions.GetKendoFilter(ref _ReportParams, ref List_Dynamic_SP_Params, _PublicClaimObjects, true);

                    List<P_AuditHistory_Result> ResultList = StaticPublicObjects.ado.P_Get_Generic_List_SP<P_AuditHistory_Result>("P_Get_AuditHistory_List", ref List_Dynamic_SP_Params);

                    reportResponse.TotalRowCount = ModalFunctions.GetValueFromReturnParameter<long>(List_Dynamic_SP_Params, "TotalRowCount", typeof(long));
                    reportResponse.ResultData = ResultList;
                    reportResponse.response_code = true;
                }
                else
                {
                    reportResponse.TotalRowCount = 0;
                    reportResponse.ResultData = null;
                    reportResponse.response_code = false;
                }
            }
            catch (Exception ex)
            {
                StaticPublicObjects.logFile.ErrorLog(FunctionName: "GetFilterData_AuditHistory_List", SmallMessage: ex.Message, Message: ex.ToString());
                reportResponse.TotalRowCount = 0;
                reportResponse.ResultData = null;
                reportResponse.response_code = false;
            }
            return Globals.GetAjaxJsonReturn(reportResponse);
        }

        [HttpPost]
        public IActionResult GetFilterData_AuditColumn_List([FromBody] ReportParams _ReportParams)
        {
            ReportResponse reportResponse = new ReportResponse();
            try
            {
                if (StaticPublicObjects.ado.P_Is_Has_Right_From_Username_And_PR_ID_From_Memory(_PublicClaimObjects.username, RightsList_ID.Audit_History_View))
                {
                    List<Dynamic_SP_Params> List_Dynamic_SP_Params = new List<Dynamic_SP_Params>();
                    ModalFunctions.GetKendoFilter(ref _ReportParams, ref List_Dynamic_SP_Params, _PublicClaimObjects, true);

                    List<P_AuditColumn_Result> ResultList = StaticPublicObjects.ado.P_Get_Generic_List_SP<P_AuditColumn_Result>("P_Get_AuditColumn_List", ref List_Dynamic_SP_Params);

                    reportResponse.TotalRowCount = ModalFunctions.GetValueFromReturnParameter<long>(List_Dynamic_SP_Params, "TotalRowCount", typeof(long));
                    reportResponse.ResultData = ResultList;
                    reportResponse.response_code = true;
                }
                else
                {
                    reportResponse.TotalRowCount = 0;
                    reportResponse.ResultData = null;
                    reportResponse.response_code = false;
                }
            }
            catch (Exception ex)
            {
                StaticPublicObjects.logFile.ErrorLog(FunctionName: "GetFilterData_AuditColumn_List", SmallMessage: ex.Message, Message: ex.ToString());
                reportResponse.TotalRowCount = 0;
                reportResponse.ResultData = null;
                reportResponse.response_code = false;
            }
            return Globals.GetAjaxJsonReturn(reportResponse);
        }
        #endregion Audit History Setup
    }
}

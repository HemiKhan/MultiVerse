
using Data.DataAccess;

namespace Data.Dtos
{
    #region User Setup
    public class P_Users_Result
    {
        public int RowNo { get; set; } = 0;
        public int User_ID { get; set; } = 0;
        public string Encrypted_USER_ID
        {
            get
            {
                return Crypto.EncryptNumericToStringWithOutNull(User_ID);
            }
        }
        public int R_ID { get; set; } = 0;
        public string AppName { get; set; } = "";
        public string RoleName { get; set; } = "";
        public string UserName { get; set; } = "";
        public string Email { get; set; } = "";
        public string FirstName { get; set; } = "";
        public string LastName { get; set; } = "";
        public string UserType { get; set; } = "";
        public string PasswordExpiry { get; set; } = "";
        public string TelegramID { get; set; } = "";
        public string TelegramUserName { get; set; } = "";
        public string BlockType { get; set; } = "";
        public bool IsApproved { get; set; }
        public bool IsGroupRoleID { get; set; }
        public bool IsTempPassword { get; set; }
        public bool IsActive { get; set; } = false;
    }
    public class P_AddOrEdit_User_Request
    {
        public int User_ID { get; set; } = 0;

        private string _USERNAME = "";
        public string UserName
        {
            get
            {
                return this._USERNAME;
            }
            set
            {
                this._USERNAME = (value == null ? "" : value.ToUpper());
            }
        }

        private string _Email = "";
        public string Email
        {
            get
            {
                return this._Email;
            }
            set
            {
                this._Email = (value == null ? "" : value.ToLower());
            }
        }

        public string TelegramUserName { get; set; } = default!;
        public string TelegramID { get; set; } = default!;

        private string _FirstName = "";
        public string FirstName
        {
            get
            {
                return this._FirstName;
            }
            set
            {
                this._FirstName = (value == null ? "" : value);
            }
        }

        private string _LastName = "";
        public string LastName
        {
            get
            {
                return this._LastName;
            }
            set
            {
                this._LastName = (value == null ? "" : value);
            }
        }

        public string Password { get; set; } = default!;
        public string ConfirmPassword { get; set; } = default!;
        public DateTime PasswordExpiryDateTime { get; set; } = default!;
        public string UserType_MTV_CODE { get; set; } = default!;
        public string BlockType_MTV_CODE { get; set; } = default!;
        public bool IsApproved { get; set; } = false;
        public bool IsTempPassword { get; set; } = false;
    }
    public class P_AddOrEdit_User_Response
    {
        public int User_ID { get; set; } = 0;

        private string _USERNAME = "";
        public string UserName
        {
            get
            {
                return this._USERNAME;
            }
            set
            {
                this._USERNAME = (value == null ? "" : value.ToUpper());
            }
        }

        private string _Email = "";
        public string Email
        {
            get
            {
                return this._Email;
            }
            set
            {
                this._Email = (value == null ? "" : value.ToLower());
            }
        }

        public string TelegramUserName { get; set; } = default!;
        public string TelegramID { get; set; } = default!;

        private string _FirstName = "";
        public string FirstName
        {
            get
            {
                return this._FirstName;
            }
            set
            {
                this._FirstName = (value == null ? "" : value);
            }
        }

        private string _LastName = "";
        public string LastName
        {
            get
            {
                return this._LastName;
            }
            set
            {
                this._LastName = (value == null ? "" : value);
            }
        }

        public string PasswordHash { get; set; } = default!;
        public string PasswordSalt { get; set; } = default!;
        public DateTime PasswordExpiryDateTime { get; set; } = default!;
        public string UserType_MTV_CODE { get; set; } = default!;
        public string BlockType_MTV_CODE { get; set; } = default!;
        public bool IsApproved { get; set; } = false;
        public bool IsTempPassword { get; set; } = false;
    }
    public class P_Get_User_By_ID
    {
        public int USER_ID { get; set; } = 0;

        private string _USERNAME = "";
        public string USERNAME
        {
            get
            {
                return this._USERNAME;
            }
            set
            {
                this._USERNAME = (value == null ? "" : value.ToUpper());
            }
        }

        public string UserType_MTV_CODE { get; set; } = default!;
        public string UserType { get; set; } = default!;
        public string Password { get; set; } = default!;
        public string ConfirmPassword { get; set; } = default!;
        public int D_ID { get; set; }
        public string DepartmentName { get; set; }
        public string Designation { get; set; } = default!;

        private string _FirstName = "";
        public string FirstName
        {
            get
            {
                return this._FirstName;
            }
            set
            {
                this._FirstName = (value == null ? "" : value);
            }
        }

        private string _MiddleName = "";
        public string MiddleName
        {
            get
            {
                return this._MiddleName;
            }
            set
            {
                this._MiddleName = (value == null ? "" : value);
            }
        }

        private string _LastName = "";
        public string LastName
        {
            get
            {
                return this._LastName;
            }
            set
            {
                this._LastName = (value == null ? "" : value);
            }
        }

        public string Company { get; set; } = default!;
        public string Address { get; set; } = default!;
        public string Address2 { get; set; } = default!;
        public string City { get; set; } = default!;
        public string State { get; set; } = default!;
        public string ZipCode { get; set; } = default!;
        public string Country { get; set; } = default!;

        private string _Email = "";
        public string Email
        {
            get
            {
                return this._Email;
            }
            set
            {
                this._Email = (value == null ? "" : value.ToLower());
            }
        }

        public string Mobile { get; set; } = default!;
        public string Phone { get; set; } = default!;
        public string PhoneExt { get; set; } = default!;
        public int SecurityQuestion_MTV_ID { get; set; }
        public string Question { get; set; } = default!;
        public string EncryptedAnswer { get; set; } = default!;
        public int TIMEZONE_ID { get; set; }
        public string TIMEZONE { get; set; } = default!;
        public bool IsApproved { get; set; } = false;
        public int BlockType_MTV_ID { get; set; }
        public string BlockType { get; set; } = default!;
        public bool IsAPIUser { get; set; } = false;
        public bool IsActive { get; set; } = true;
    }
    public class P_ReturnMessageForJson_Result
    {
        public bool ReturnCode { get; set; } = false;
        public string? ReturnText { get; set; } = "";
        public string? Execution_Error { get; set; } = "";
        public string? Error_Text { get; set; } = "";

    }
    public class P_Get_SearchUsersName
    {
        public int USER_ID { get; set; }
        public string Eny_UserID
        {
            get
            {
                return Crypto.EncryptNumericToStringWithOutNull(USER_ID);
            }
        }


        public string USERNAME { get; set; }
    }
    #endregion User Setup

    #region Page Setup
    public class P_PageGroup_Result
    {
        public int RowNo { get; set; }
        public int PG_ID { get; set; }
        public string Ery_PG_ID
        {
            get
            {
                return Crypto.EncryptNumericToStringWithOutNull(PG_ID);
            }
        }
        public string PageGroupName { get; set; }
        public int Sort_ { get; set; }
        public bool IsHide { get; set; }
        public bool IsActive { get; set; }
    }
    public class P_Page_Result
    {
        public int RowNo { get; set; }
        public int P_ID { get; set; }
        public string Ery_P_ID
        {
            get
            {
                return Crypto.EncryptNumericToStringWithOutNull(P_ID);
            }
        }
        public int PG_ID { get; set; }
        public string Ery_PG_ID
        {
            get
            {
                return Crypto.EncryptNumericToStringWithOutNull(PG_ID);
            }
        }
        public int Application_MTV_ID { get; set; }
        public string PageName { get; set; }
        public string PageGroupName { get; set; }
        public string PageURL { get; set; }
        public string Application { get; set; }
        public int Sort_ { get; set; }
        public bool IsHide { get; set; }
        public bool IsActive { get; set; }
    }
    public class P_AddOrEdit_PageGroup_Response
    {
        public int PG_ID { get; set; }
        public string PageGroupName { get; set; }
        public bool IsHide { get; set; }
        public bool Active { get; set; }
    }
    public class P_AddOrEdit_Page_Response
    {
        public int P_ID { get; set; }
        public int PG_ID { get; set; }
        public string PageName { get; set; }
        public string PageUrl { get; set; }
        public int Application_MTV_ID { get; set; }
        public bool IsHide { get; set; }
        public bool Active { get; set; }
    }

    public class P_PageChart_Response
    {
        public int? AppID { get; set; }
        public string? UserName { get; set; }
        public int? RoleID { get; set; }
    }
    public class P_Get_PageChart_TreeView
    {
        public int App_ID { get; set; }
        public string Application { get; set; }
        public bool IsAllChecked
        {
            get
            {
                bool Ret = false;
                if (pageGroupInfo != null)
                {
                    if (pageGroupInfo.Count > 0)
                    {
                        Ret = true;
                        for (int i = 0; i <= pageGroupInfo.Count - 1; i++)
                        {
                            if (pageGroupInfo[i].IsAllChecked == false && pageGroupInfo[i].IsChildExists)
                            {
                                Ret = false;
                                break;
                            }
                        }
                    }
                }
                return Ret;
            }
        }
        public bool IsChildExists
        {
            get
            {
                bool Ret = false;
                if (pageGroupInfo != null)
                {
                    if (pageGroupInfo.Count > 0)
                        for (int i = 0; i <= pageGroupInfo.Count - 1; i++)
                        {
                            if (pageGroupInfo[i].IsChildExists)
                            {
                                Ret = true;
                                break;
                            }
                        }
                }
                return Ret;
            }
        }
        public List<PageGroupInfo> pageGroupInfo { get; set; }
    }
    #endregion Page Setup

    #region RoleSetup
    public class P_Role_Result
    {
        public int rowno { get; set; }
        public int R_ID { get; set; }
        public string RoleName { get; set; }
        public int Sort_ { get; set; }
        public int IsActive { get; set; }

    }
    public class P_RoleGroup_Result
    {
        public int rowno { get; set; }
        public int RG_ID { get; set; }
        public string RoleGroupName { get; set; }
        public int Sort_ { get; set; }
        public int IsActive { get; set; }

    }
    public class P_RoleGroupMap_Result
    {
        public int rowno { get; set; }
        public int RGM_ID { get; set; }
        public int R_ID { get; set; }
        public int RG_ID { get; set; }
        public string RoleName { get; set; }
        public string RoleGroupName { get; set; }
        public int IsActive { get; set; }
    }
    public class P_DepartmentRoleMap_Result
    {
        public int rowno { get; set; }
        public int DRM_ID { get; set; }
        public int R_ID { get; set; }
        public int D_ID { get; set; }
        public string RoleName { get; set; }
        public string DepartmentName { get; set; }
        public int IsActive { get; set; }
    }
    public class P_AddOrEdit_Role_Response
    {
        public int RoleID { get; set; }
        public string RoleName { get; set; }
        public bool Active { get; set; }
    }
    public class P_AddOrEdit_Role_Group_Response
    {
        public int RoleGroupID { get; set; }
        public string RoleGroupName { get; set; }
        public bool Active { get; set; }
    }
    public class P_AddOrEdit_Role_Group_Mapping_Response
    {
        public int RoleGroupMappingID { get; set; }
        public int RoleID { get; set; }
        public int RoleGroupID { get; set; }
        public bool Active { get; set; }
    }
    public class P_AddOrEdit_Department_Role_Mapping_Response
    {
        public int DepartmentRoleMappingID { get; set; }
        public int RoleID { get; set; }
        public int DepartmentID { get; set; }
        public bool Active { get; set; }
    }
    #endregion RoleSetup

    #region RightsSetup
    public class P_PageRight_Result
    {
        public int rowno { get; set; }
        public int PR_ID { get; set; }
        public int P_ID { get; set; }
        public string PageName { get; set; }
        public string PR_CODE { get; set; }
        public string PageRightName { get; set; }
        public string PageRightType_MTV_CODE { get; set; }
        public int Sort_ { get; set; }
        public bool IsHide { get; set; }
        public bool IsActive { get; set; }

    }
    public class P_RolePageRight_Result
    {
        public int rowno { get; set; }
        public int RPRM_ID { get; set; }
        public int R_ID { get; set; }
        public int PR_ID { get; set; }
        public string RoleName { get; set; }
        public string PageRightName { get; set; }
        public bool IsRightActive { get; set; }
        public bool IsActive { get; set; }
    }
    public class P_UserRoleMap_Result
    {
        public int rowno { get; set; }
        public int URM_ID { get; set; }
        public string Ery_URM_ID
        {
            get
            {
                return Crypto.EncryptNumericToStringWithOutNull(URM_ID);
            }
        }
        public int R_ID { get; set; }
        public string USERNAME { get; set; }
        public string RoleName { get; set; }
        public bool IsGroupRoleID { get; set; }
        public bool IsActive { get; set; }
    }
    public class P_AddOrEdit_PageRights_Response
    {
        public int PR_ID { get; set; }
        public int P_ID { get; set; }
        public string PR_CODE { get; set; }
        public string PageRightName { get; set; }
        public string PageRightType { get; set; }
        public bool IsHide { get; set; }
        public bool Active { get; set; }
    }
    public class P_AddOrEdit_RolePageRights_Response
    {
        public int RPRM_ID { get; set; }
        public int R_ID { get; set; }
        public int PR_ID { get; set; }
        public bool IsRightActive { get; set; }
        public bool Active { get; set; }
    }

    public class P_AddOrEdit_RolePageRights_TreeView
    {
        public int R_ID { get; set; }
        public string RoleName { get; set; }
        public bool IsAllChecked
        {
            get
            {
                bool Ret = false;
                if (pageGroupInfo != null)
                {
                    if (pageGroupInfo.Count > 0)
                    {
                        Ret = true;
                        for (int i = 0; i <= pageGroupInfo.Count - 1; i++)
                        {
                            if (pageGroupInfo[i].IsAllChecked == false && pageGroupInfo[i].IsChildExists)
                            {
                                Ret = false;
                                break;
                            }
                        }
                    }
                }
                return Ret;
            }
        }
        public bool IsChildExists
        {
            get
            {
                bool Ret = false;
                if (pageGroupInfo != null)
                {
                    if (pageGroupInfo.Count > 0)
                        for (int i = 0; i <= pageGroupInfo.Count - 1; i++)
                        {
                            if (pageGroupInfo[i].IsChildExists)
                            {
                                Ret = true;
                                break;
                            }
                        }
                }
                return Ret;
            }
        }
        public List<PageGroupInfo> pageGroupInfo { get; set; }
    }
    public class P_AddOrEdit_UserRolePageRights_TreeView
    {
        public int R_ID { get; set; }
        public string? RoleName { get; set; }
        public string? UserName { get; set; }
        public bool IsAllChecked
        {
            get
            {
                bool Ret = false;
                if (pageGroupInfo != null)
                {
                    if (pageGroupInfo.Count > 0)
                    {
                        Ret = true;
                        for (int i = 0; i <= pageGroupInfo.Count - 1; i++)
                        {
                            if (pageGroupInfo[i].IsAllChecked == false && pageGroupInfo[i].IsChildExists)
                            {
                                Ret = false;
                                break;
                            }
                        }
                    }
                }
                return Ret;
            }
        }
        public bool IsChildExists
        {
            get
            {
                bool Ret = false;
                if (pageGroupInfo != null)
                {
                    if (pageGroupInfo.Count > 0)
                        for (int i = 0; i <= pageGroupInfo.Count - 1; i++)
                        {
                            if (pageGroupInfo[i].IsChildExists)
                            {
                                Ret = true;
                                break;
                            }
                        }
                }
                return Ret;
            }
        }
        public List<PageGroupInfo>? pageGroupInfo { get; set; }
    }
    public class P_Sync_RolePageRights_TreeView
    {
        public int RoleID { get; set; }
        public int RoleIDCompare { get; set; }
        public int CopyR_ID { get; set; }
        public int CopyPG_ID { get; set; }
        public int CopyP_ID { get; set; }
        public bool? Active { get; set; }
    }
    public class P_Sync_UserRolePageRights_TreeView
    {
        public string? UserName { get; set; }
        public string? UserNameCompare { get; set; }
        public int CopyR_ID { get; set; }
        public int CopyPG_ID { get; set; }
        public int CopyP_ID { get; set; }
        public bool? Active { get; set; }
    }
    public class PageGroupInfo
    {
        public int PG_ID { get; set; }
        public string? PageGroupName { get; set; }
        public int PGSort_ { get; set; }
        public bool IsAllChecked
        {
            get
            {
                bool Ret = false;
                if (pageInfo != null)
                {
                    if (pageInfo.Count > 0)
                    {
                        Ret = true;
                        for (int i = 0; i <= pageInfo.Count - 1; i++)
                        {
                            if (pageInfo[i].IsAllChecked == false && pageInfo[i].IsChildExists)
                            {
                                Ret = false;
                                break;
                            }
                        }
                    }
                }
                return Ret;
            }
        }
        public bool IsChildExists
        {
            get
            {
                bool Ret = false;
                if (pageInfo != null)
                {
                    if (pageInfo.Count > 0)
                        for (int i = 0; i <= pageInfo.Count - 1; i++)
                        {
                            if (pageInfo[i].IsChildExists)
                            {
                                Ret = true;
                                break;
                            }
                        }
                }
                return Ret;
            }
        }
        public List<PageInfo>? pageInfo { get; set; }
    }
    public class PageInfo
    {
        public int P_ID { get; set; }
        public string? PageName { get; set; }
        public int PageAppCode { get; set; }
        public int PSort_ { get; set; }
        public bool IsAllChecked
        {
            get
            {
                bool Ret = false;
                if (pageRightsInfo != null)
                {
                    if (pageRightsInfo.Count > 0)
                    {
                        Ret = true;
                        for (int i = 0; i <= pageRightsInfo.Count - 1; i++)
                        {
                            if (pageRightsInfo[i].IsRightActive == false)
                            {
                                Ret = false;
                                break;
                            }
                        }
                    }
                }
                return Ret;
            }
        }
        public bool IsChildExists
        {
            get
            {
                bool Ret = false;
                if (pageRightsInfo != null)
                {
                    if (pageRightsInfo.Count > 0)
                        Ret = true;
                }
                return Ret;
            }
        }
        public List<PageRightInfo>? pageRightsInfo { get; set; }
    }
    public class PageRightInfo
    {
        public int PR_ID { get; set; }
        public string PR_CODE { get; set; }
        public string PageRightName { get; set; }
        public string PageRightType_MTV_CODE { get; set; }
        public int PRSort_ { get; set; }
        public bool IsRightActive { get; set; }
    }
    public class P_AddOrEdit_UserRoleMapping_Response : P_AddOrEdit_RolePageRights_Response
    {
        public string? UserName { get; set; }
    }

    public class P_AddOrEdit_UserRoleMapping_Response_1
    {
        public int URM_ID { get; set; }
        public int R_ID { get; set; }
        public string? UNAME { get; set; } = string.Empty;
        public bool IsGroupRoleID { get; set; }
        public bool Active { get; set; }
    }

    public class P_Get_Role_All_Right_Combinations
    {
        public string Role_ID { get; set; } = string.Empty;
        public string Role_Name { get; set; } = string.Empty;
        public string Right_ID { get; set; } = string.Empty;
        public string Right_Name { get; set; } = string.Empty;
        public string Right_Type { get; set; } = string.Empty;
        public string Page_Name { get; set; } = string.Empty;
        public string PageGroup_Name { get; set; } = string.Empty;
        public string Application_Name { get; set; } = string.Empty;
        public string IsRightActive { get; set; } = string.Empty;
    }
    public class P_Get_Role_All_Right_Combinations_Import
    {
        public int Role_ID { get; set; } = 0;
        public string Role_Name { get; set; } = string.Empty;
        public int Right_ID { get; set; } = 0;
        public string Right_Name { get; set; } = string.Empty;
        public string Right_Type { get; set; } = string.Empty;
        public string Page_Name { get; set; } = string.Empty;
        public string PageGroup_Name { get; set; } = string.Empty;
        public string Application_Name { get; set; } = string.Empty;
        public bool IsRightActive { get; set; } = false;
    }
    #endregion RightsSetup

    #region MasterSetup
    public class P_MT_Result
    {
        public int rowno { get; set; }
        public int MT_ID { get; set; }
        public string MasterTypeName { get; set; }
        public string Description { get; set; }
        public bool IsActive { get; set; }
    }
    public class P_AddOrEdit_MT_Response
    {
        public int MT_ID { get; set; }
        public string MasterTypeName { get; set; }
        public string Description { get; set; }
        public bool Active { get; set; }
    }
    public class P_MTV_Result
    {
        public int rowno { get; set; }
        public int MTV_ID { get; set; }
        public int MT_ID { get; set; }
        public string? MTV_CODE { get; set; }
        public string MasterType { get; set; }
        public string MasterTypeValue { get; set; }
        public int Sort_ { get; set; }
        public int Sub_MTV_ID { get; set; }
        public string? Sub_MTV_Name { get; set; }
        public bool IsActive { get; set; }
    }
    public class P_AddOrEdit_MTV_Response
    {
        public int MTV_ID { get; set; }
        public int MT_ID { get; set; }
        public string MTV_CODE { get; set; }
        public string MasterTypeValueName { get; set; }
        public int Sub_MTV_ID { get; set; }
        public bool Active { get; set; }
    }
    #endregion MasterSetup

    #region AuditHistory
    public class P_AuditHistory_Result
    {
        public int rowno { get; set; }
        public int AH_ID { get; set; }
        public int AC_ID { get; set; }
        public int AuditType_MTV_ID { get; set; }
        public int Source_MTV_ID { get; set; }
        public string TableName { get; set; }
        public string DbName { get; set; }
        public string ColumnName { get; set; }
        public string REF_NO { get; set; }
        public string MasterTypeValueAudit { get; set; }
        public string RefNo1 { get; set; }
        public string RefNo2 { get; set; }
        public string RefNo3 { get; set; }
        public string OldValueHidden { get; set; }
        public string NewValueHidden { get; set; }
        public string OldValue { get; set; }
        public string NewValue { get; set; }
        public string Reason { get; set; }
        public bool IsAuto { get; set; }
        public string MasterTypeValueSource { get; set; }
        public string TriggerDebugInfo { get; set; }
        public string ChangedBy { get; set; }
        private string _ChangedOn = "";
        public DateTime ChangedOn
        {
            get
            {
                return Convert.ToDateTime(this._ChangedOn);
            }
            set
            {
                this._ChangedOn = Globals.ConvertDBNulltoNullIfExistsDateTime(value, true) ?? "";
            }
        }
        public string TimeZoneAbbr { get; set; } = "";
    }

    public class P_AuditColumn_Result
    {
        public int rowno { get; set; }
        public int AC_ID { get; set; }
        public string TableName { get; set; }
        public string DbName { get; set; }
        public string Name { get; set; }
        public bool IsPublic { get; set; }


    }
    #endregion AuditHistory
}

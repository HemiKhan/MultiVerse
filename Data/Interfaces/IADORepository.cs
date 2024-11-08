using Data.DataAccess;
using Data.Dtos;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using System.Data;
using System.Security.Claims;

namespace Data.Interfaces
{
    public interface IADORepository
    {
        #region General
        public bool GetIsLoginCaptchaEnabled();
        public string GetApplicationEnvironment();
        public bool GetIsSingleSignOn();
        public List<string> GetAllowedFileExtension();
        public string GetEdiFabricSerialKey();
        public string GetFileServerPath();
        public string GetPublicFileSrvPath();
        public IHttpContextAccessor GetIHttpContextAccessor();
        public IConfiguration GetIConfiguration();
        public string GetAPIUserName();
        public string GetAPIPassword();
        public HttpClient SetDefaultWebBasedHeaders();
        public void RemoveMemoryCache(string subtype, MemoryCacheValueType? _MemoryCacheValueType = null);
        public bool IsValidToken(PublicClaimObjects _PublicClaimObjects, int Seconds);
        public void UpdateTokenKeyCacheTime(PublicClaimObjects _PublicClaimObjects, int Seconds);
        public void AddTokenKeyCacheTime(bool iswebtoken, bool issinglesignon, string jit, string key, int Seconds);
        public void RemoveTokenKeyFromCache(PublicClaimObjects _PublicClaimObjects);
        public AllowedDomainListModel GetAllowedRemoteDomain();
        public AllowedMobileAppKeysModel GetAllowedMobileAppKeys();
        public AllowedWebAppKeysModel GetAllowedWebAppKeys();
        public AllowedDomainExcludingSubDomainListModel GetAllowedRemoteDomainExcludingSubDomain();
        public List<string>? GetAllowedRemoteDomainList();
        public List<string>? GetAllowedRemoteDomainExcludingSubDomainList();
        public bool IsAllowAnonymousMethods();
        public bool IsAllowedDomain();
        public bool IsAllowedDomainExcludingSubDomain();
        public PublicClaimObjects GetPublicClaimObjects();
        public Task<string> GetRequestBodyString();
        public ClaimsPrincipal? GetUserClaim();
        public string GetVirtualDirectory();
        public string GetSwaggerHiddenVersion();
        public string GetAdminSwaggerKey();
        public bool GetIsCachingEnabled();
        public bool IsDevelopment();
        public string GetSubDomain();
        public string GetHostNameExcludingSubDomain();
        public string GetHostName();
        public string GetRemoteDomainExcludingSubDomain();
        public string GetRemoteDomain();
        public string GetRemoteRefererURL();
        public string GetRemoteURL();
        public string GetHostURL();
        public bool IsSwaggerCall();
        public bool IsSwaggerCallAdmin();
        public void P_CacheEntry_IU(string cacheKey, string cacheValue, DateTime? expirationTime, int? applicationID = null);
        public void P_CacheEntry_Delete(string cacheKey);
        public DataRow? P_Get_CacheEntry(string cacheKey);
        public DataTable P_Add_Session_History(int? WebUserID, string Username, string SessionID, int DeviceTypeID, DateTime LoginTime, int ApplicationID, bool IsSuccess, string Latitude, string Longitude);
        public string GetLocalIPAddress();
        public string GetRequestPath();
        public int GetMethodIDFromPath();
        public DataRow? P_Get_T_Config_Detail(string Config_Key, MemoryCacheValueType? _MemoryCacheValueType = null);
        public DataRow? P_Get_API_User_Map_Request_Limit(int UserID, int MethodID, string? Username, MemoryCacheValueType? _MemoryCacheValueType = null);
        public DataTable P_Get_API_User_Map(int UserID, int APIID, string? Username, MemoryCacheValueType? _MemoryCacheValueType = null);
        public DataRow? P_Get_API_RemoteDomain_IP_Request_Limit(bool Is_RemoteDomain, int MethodID, MemoryCacheValueType? _MemoryCacheValueType = null);
        public DataRow? P_Get_API_RemoteDomain_IP_WhiteListing(bool Is_RemoteDomain, int APIID, MemoryCacheValueType? _MemoryCacheValueType = null);
        public DataTable P_Get_List_By_ID(int MT_ID, string SELLER_KEY, string? SELLER_CODE = null, string? SELLER_NAME = null, MemoryCacheValueType? _MemoryCacheValueType = null);
        public DataTable P_Get_List_By_ID_2(int MT_ID, string? Username = null, MemoryCacheValueType? _MemoryCacheValueType = null);
        public DataTable P_Get_Role_Rights_From_RoleID(int ROLE_ID, bool IsGroupRoleID, MemoryCacheValueType? _MemoryCacheValueType = null);
        public DataTable P_Get_Role_Rights_From_RoleID_And_P_ID(int ROLE_ID, bool IsGroupRoleID, int P_ID, MemoryCacheValueType? _MemoryCacheValueType = null);
        public DataTable P_Get_Role_Rights_From_RoleID_And_PR_ID(int ROLE_ID, bool IsGroupRoleID, int PR_ID, MemoryCacheValueType? _MemoryCacheValueType = null);
        public DataTable P_Get_Role_Rights_From_RoleID_And_PageRightType_MTV_CODE(int ROLE_ID, bool IsGroupRoleID, string PageRightType_MTV_CODE, MemoryCacheValueType? _MemoryCacheValueType = null);
        public DataTable P_Get_Role_Rights_From_Username(string Username, MemoryCacheValueType? _MemoryCacheValueType = null);
        public DataTable P_Get_Role_Rights_From_Username_And_P_ID(string Username, int P_ID, MemoryCacheValueType? _MemoryCacheValueType = null);
        public DataTable P_Get_Role_Rights_From_Username_And_PR_ID(string Username, int PR_ID, MemoryCacheValueType? _MemoryCacheValueType = null);
        public DataTable P_Get_Role_Rights_From_Username_And_PageRightType_MTV_CODE(string Username, string PageRightType_MTV_CODE, MemoryCacheValueType? _MemoryCacheValueType = null);
        public bool P_Is_Has_Right_From_RoleID_And_PR_ID_From_Memory(int ROLE_ID, bool IsGroupRoleID, int PR_ID);
        public bool P_Is_Has_Right_From_RoleID_And_PageRightType_MTV_CODE_From_Memory(int ROLE_ID, bool IsGroupRoleID, string PageRightType_MTV_CODE);
        public bool P_Is_Has_Right_From_Username_And_PR_ID_From_Memory(string Username, int PR_ID);
        public bool P_Is_Has_Right_From_Username_And_PageRightType_MTV_CODE_From_Memory(string Username, string PageRightType_MTV_CODE);
        public bool P_Is_Has_Right_From_RoleID_And_PR_ID(int ROLE_ID, bool IsGroupRoleID, int PR_ID = 0, MemoryCacheValueType? _MemoryCacheValueType = null);
        public bool P_Is_Has_Right_From_RoleID_And_PageRightType_MTV_CODE(int ROLE_ID, bool IsGroupRoleID, string PageRightType_MTV_CODE = "", MemoryCacheValueType? _MemoryCacheValueType = null);
        public bool P_Is_Has_Right_From_Username_And_PR_ID(string Username, int PR_ID = 0, MemoryCacheValueType? _MemoryCacheValueType = null);
        public bool P_Is_Has_Right_From_Username_And_PageRightType_MTV_CODE(string Username, string PageRightType_MTV_CODE = "", MemoryCacheValueType? _MemoryCacheValueType = null);
        #endregion General

        #region DB
        public int ExecuteNONQuery(string Query, ref List<Dynamic_SP_Params> dynamic_SP_Params, string Database_Name = AppEnum.Database_Name.MultiVerseDB, int CommandTimeOut = 0, string Config_Key = "");
        public int ExecuteNONQuery(string Query, string Database_Name = AppEnum.Database_Name.MultiVerseDB, int CommandTimeOut = 0, string Config_Key = "");
        public int ExecuteNONQuery(string Query, string Database_Name = AppEnum.Database_Name.MultiVerseDB, int CommandTimeOut = 0, string Config_Key = "", params (string Name, object Value)[] ParamsNameValues);
        public int ExecuteNONQuery(string Query, params (string Name, object Value)[] ParamsNameValues);
        public DataSet ExecuteSelectDS(string Query, ref List<Dynamic_SP_Params> dynamic_SP_Params, bool Read_Only = false, string Database_Name = AppEnum.Database_Name.MultiVerseDB, int CommandTimeOut = 0, string Config_Key = "");
        public DataSet ExecuteSelectDS(string Query, bool Read_Only = false, string Database_Name = AppEnum.Database_Name.MultiVerseDB, int CommandTimeOut = 0, string Config_Key = "");
        public DataSet ExecuteSelectDS(string Query, bool Read_Only = false, string Database_Name = AppEnum.Database_Name.MultiVerseDB, int CommandTimeOut = 0, string Config_Key = "", params (string Name, object Value)[] ParamsNameValues);
        public DataSet ExecuteSelectDS(string Query, params (string Name, object Value)[] ParamsNameValues);
        public DataTable ExecuteSelectDT(string Query, ref List<Dynamic_SP_Params> dynamic_SP_Params, bool Read_Only = false, string Database_Name = AppEnum.Database_Name.MultiVerseDB, int CommandTimeOut = 0, string Config_Key = "");
        public DataTable ExecuteSelectDT(string Query, bool Read_Only = false, string Database_Name = AppEnum.Database_Name.MultiVerseDB, int CommandTimeOut = 0, string Config_Key = "");
        public DataTable ExecuteSelectDT(string Query, bool Read_Only = false, string Database_Name = AppEnum.Database_Name.MultiVerseDB, int CommandTimeOut = 0, string Config_Key = "", params (string Name, object Value)[] ParamsNameValues);
        public DataTable ExecuteSelectDT(string Query, params (string Name, object Value)[] ParamsNameValues);
        public DataRow? ExecuteSelectDR(string Query, ref List<Dynamic_SP_Params> dynamic_SP_Params, bool Read_Only = false, string Database_Name = AppEnum.Database_Name.MultiVerseDB, int CommandTimeOut = 0, string Config_Key = "");
        public DataRow? ExecuteSelectDR(string Query, bool Read_Only = false, string Database_Name = AppEnum.Database_Name.MultiVerseDB, int CommandTimeOut = 0, string Config_Key = "");
        public DataRow? ExecuteSelectDR(string Query, bool Read_Only = false, string Database_Name = AppEnum.Database_Name.MultiVerseDB, int CommandTimeOut = 0, string Config_Key = "", params (string Name, object Value)[] ParamsNameValues);
        public DataRow? ExecuteSelectDR(string Query, params (string Name, object Value)[] ParamsNameValues);
        public object? ExecuteSelectObj(string Query, ref List<Dynamic_SP_Params> dynamic_SP_Params, bool Read_Only = false, string Database_Name = AppEnum.Database_Name.MultiVerseDB, int CommandTimeOut = 0, string Config_Key = "");
        public object? ExecuteSelectObj(string Query, bool Read_Only = false, string Database_Name = AppEnum.Database_Name.MultiVerseDB, int CommandTimeOut = 0, string Config_Key = "");
        public object? ExecuteSelectObj(string Query, bool Read_Only = false, string Database_Name = AppEnum.Database_Name.MultiVerseDB, int CommandTimeOut = 0, string Config_Key = "", params (string Name, object Value)[] ParamsNameValues);
        public object? ExecuteSelectObj(string Query, params (string Name, object Value)[] ParamsNameValues);
        public int ExecuteStoreProcedureNONQuery(string SPName, ref List<Dynamic_SP_Params> dynamic_SP_Params, string Database_Name = AppEnum.Database_Name.MultiVerseDB, int CommandTimeOut = 0, string Config_Key = "");
        public int ExecuteStoreProcedureNONQuery(string SPName, string Database_Name = AppEnum.Database_Name.MultiVerseDB, int CommandTimeOut = 0, string Config_Key = "");
        public int ExecuteStoreProcedureNONQuery(string Query, string Database_Name = AppEnum.Database_Name.MultiVerseDB, int CommandTimeOut = 0, string Config_Key = "", params (string Name, object Value)[] ParamsNameValues);
        public int ExecuteStoreProcedureNONQuery(string Query, params (string Name, object Value)[] ParamsNameValues);
        public DataSet ExecuteStoreProcedureDS(string SPName, ref List<Dynamic_SP_Params> dynamic_SP_Params, bool Read_Only = false, string Database_Name = AppEnum.Database_Name.MultiVerseDB, int CommandTimeOut = 0, string Config_Key = "");
        public DataSet ExecuteStoreProcedureDS(string SPName, bool Read_Only = false, string Database_Name = AppEnum.Database_Name.MultiVerseDB, int CommandTimeOut = 0, string Config_Key = "");
        public DataSet ExecuteStoreProcedureDS(string Query, bool Read_Only = false, string Database_Name = AppEnum.Database_Name.MultiVerseDB, int CommandTimeOut = 0, string Config_Key = "", params (string Name, object Value)[] ParamsNameValues);
        public DataSet ExecuteStoreProcedureDS(string Query, params (string Name, object Value)[] ParamsNameValues);
        public DataTable ExecuteStoreProcedureDT(string SPName, ref List<Dynamic_SP_Params> dynamic_SP_Params, bool Read_Only = false, string Database_Name = AppEnum.Database_Name.MultiVerseDB, int CommandTimeOut = 0, string Config_Key = "");
        public DataTable ExecuteStoreProcedureDT(string SPName, bool Read_Only = false, string Database_Name = AppEnum.Database_Name.MultiVerseDB, int CommandTimeOut = 0, string Config_Key = "");
        public DataTable ExecuteStoreProcedureDT(string Query, bool Read_Only = false, string Database_Name = AppEnum.Database_Name.MultiVerseDB, int CommandTimeOut = 0, string Config_Key = "", params (string Name, object Value)[] ParamsNameValues);
        public DataTable ExecuteStoreProcedureDT(string Query, params (string Name, object Value)[] ParamsNameValues);
        public DataRow? ExecuteStoreProcedureDR(string SPName, ref List<Dynamic_SP_Params> dynamic_SP_Params, bool Read_Only = false, string Database_Name = AppEnum.Database_Name.MultiVerseDB, int CommandTimeOut = 0, string Config_Key = "", bool IsSP = true);
        public DataRow? ExecuteStoreProcedureDR(string SPName, bool Read_Only = false, string Database_Name = AppEnum.Database_Name.MultiVerseDB, int CommandTimeOut = 0, string Config_Key = "");
        public DataRow? ExecuteStoreProcedureDR(string Query, bool Read_Only = false, string Database_Name = AppEnum.Database_Name.MultiVerseDB, int CommandTimeOut = 0, string Config_Key = "", params (string Name, object Value)[] ParamsNameValues);
        public DataRow? ExecuteStoreProcedureDR(string Query, params (string Name, object Value)[] ParamsNameValues);
        public object? ExecuteStoreProcedureObj(string SPName, ref List<Dynamic_SP_Params> dynamic_SP_Params, bool Read_Only = false, string Database_Name = AppEnum.Database_Name.MultiVerseDB, int CommandTimeOut = 0, string Config_Key = "");
        public object? ExecuteStoreProcedureObj(string SPName, bool Read_Only = false, string Database_Name = AppEnum.Database_Name.MultiVerseDB, int CommandTimeOut = 0, string Config_Key = "");
        public object? ExecuteStoreProcedureObj(string Query, bool Read_Only = false, string Database_Name = AppEnum.Database_Name.MultiVerseDB, int CommandTimeOut = 0, string Config_Key = "", params (string Name, object Value)[] ParamsNameValues);
        public object? ExecuteStoreProcedureObj(string Query, params (string Name, object Value)[] ParamsNameValues);
        public T ExecuteSelectSQLMap<T>(string Query, bool IsSP, int CommandTimeOut, ref List<Dynamic_SP_Params> List_Dynamic_SP_Params, bool Read_Only = false, string Database_Name = AppEnum.Database_Name.MultiVerseDB, string Config_Key = "") where T : new();
        public T ExecuteSelectSQLMap<T>(string Query, bool IsSP, int CommandTimeOut, bool Read_Only = false, string Database_Name = AppEnum.Database_Name.MultiVerseDB, string Config_Key = "", params (string Name, object Value)[] ParamsNameValues) where T : new();
        public T ExecuteSelectSQLMap<T>(string Query, bool IsSP, int CommandTimeOut, bool Read_Only = false, string Database_Name = AppEnum.Database_Name.MultiVerseDB, string Config_Key = "") where T : new();
        public List<T> ExecuteSelectSQLMapList<T>(string Query, bool IsSP, int CommandTimeOut, ref List<Dynamic_SP_Params> List_Dynamic_SP_Params, bool Read_Only = false, string Database_Name = AppEnum.Database_Name.MultiVerseDB, string Config_Key = "") where T : new();
        public List<T> ExecuteSelectSQLMapList<T>(string Query, bool IsSP, int CommandTimeOut, bool Read_Only = false, string Database_Name = AppEnum.Database_Name.MultiVerseDB, string Config_Key = "", params (string Name, object Value)[] ParamsNameValues) where T : new();
        public List<T> ExecuteSelectSQLMapList<T>(string Query, bool IsSP, int CommandTimeOut, bool Read_Only = false, string Database_Name = AppEnum.Database_Name.MultiVerseDB, string Config_Key = "") where T : new();
        public void ExecuteSelectSQLMapMultiple(string Query, bool IsSP, bool IsList, int CommandTimeOut, ref List<Dynamic_SP_Params> List_Dynamic_SP_Params, ref List<object> listofobject, bool Read_Only = false, string Database_Name = AppEnum.Database_Name.MultiVerseDB, string Config_Key = "");
        public void ExecuteSelectSQLMapMultiple(string Query, bool IsSP, bool IsList, int CommandTimeOut, ref List<object> listofobject, bool Read_Only = false, string Database_Name = AppEnum.Database_Name.MultiVerseDB, string Config_Key = "", params (string Name, object Value)[] ParamsNameValues);
        public void ExecuteSelectSQLMapMultiple(string Query, bool IsSP, bool IsList, int CommandTimeOut, ref List<object> listofobject, bool Read_Only = false, string Database_Name = AppEnum.Database_Name.MultiVerseDB, string Config_Key = "");
        public DataRow P_Common_DR_Procedure(string Query, ref List<Dynamic_SP_Params> List_Dynamic_SP_Params);
        public P_ReturnMessage_Result P_SP_MultiParm_Result<T>(string Query, T res, string USERNAME, string IP = "");
        public P_ReturnMessageWithObj_Result P_SP_MultiParmWithObj_Result<T>(string Query, T res, string USERNAME, string ObjFieldName, string IP = "");
        public P_ReturnMessage_Result P_SP_SingleParm_Result(string Query, string parmName, object parmValue, string USERNAME, string IP = "");
        public string P_Get_SingleParm_String_Result(string Query, string parmName, object parmValue);
        public string P_Get_MultiParm_String_Result(string Query, List<Dynamic_SP_Params> List_Dynamic_SP_Params);
        public List<SelectDropDownList> Get_DropDownList_Result(string Query, List<Dynamic_SP_Params> List_Dynamic_SP_Params = null!, bool IsSP = false);
        public T P_AddEditRemove_SP<T>(string Query, List<Dynamic_SP_Params> List_Dynamic_SP_Params) where T : new();
        public T P_Get_Generic_SP<T>(string Query, ref List<Dynamic_SP_Params> List_Dynamic_SP_Params, bool IsSP = true) where T : new();
        public P_ReturnMessage_Result P_SP_Remove_Generic_Result(string TableName, string ColumnName, object ColumnValue);
        public List<T> P_Get_Generic_List_SP<T>(string Query, ref List<Dynamic_SP_Params> List_Dynamic_SP_Params, bool IsSP = true) where T : new();
        public string P_Get_SingleValue_String_SP(string Query, string parmName, object parmValue);
        public List<SelectDropDownList> Get_DropdownList_MT_ID(int MT_ID, string UserName);
        public List<SelectDropDownListWithEncryptionString> Get_DropdownList_MT_ID_With_Encryption(int MT_ID, string UserName, bool IsCodeRequired);
        public List<object> Get_SingleParm_Object_Result(string Query, List<Dynamic_SP_Params> _Params, bool IsSP = false);
        public object? Get_SingleRow_Object_Result(string Query, List<Dynamic_SP_Params> _Params, bool IsSP = false);
        public string Get_SingleRow_String_Result(string Query, List<Dynamic_SP_Params> _Params, bool IsSP = false);
        public P_ReturnMessage_Result P_ExecuteProc_Result(string Query, List<Dynamic_SP_Params> dynamic_SP_Params_list, bool IsSP = true);
        public P_ReturnMessage_Result P_ExcuteDynamic_Result<T>(string Query, T res);
        #endregion DB     

        #region User
        public Task<P_UserLoginPasswordModel> GetUserLoginCredentials(string UserName, CancellationToken cancellationToken);
        public P_Get_User_Info P_Get_User_Info(string UserName, int ApplicationID, MemoryCacheValueType? _MemoryCacheValueType = null);
        public P_ReturnMessage_Result P_AddOrEdit_User_Role_Map(string Json);
        public int Get_RoleID_From_UserName(string UserName);
        public P_ReturnMessage_Result P_AddOrEdit_User(P_AddOrEdit_User_Request req);

        #endregion User
    }
}

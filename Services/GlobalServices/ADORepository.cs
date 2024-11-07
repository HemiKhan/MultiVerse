using Dapper;
using Data.DataAccess;
using Data.Dtos;
using Data.Interfaces;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using Microsoft.VisualBasic;
using System.Data;
using System.Reflection;
using System.Security.Claims;
using static Data.Dtos.AppEnum;
using static Data.Dtos.CustomClasses;

namespace Services.GlobalServices
{
    public class ADORepository : IADORepository
    {
        #region Contructor
        private readonly DbStringCollection _dbStringCollection1;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly string _virtualdirectory;
        private readonly string _adminswaggerkey;
        private readonly AllowedMobileAppKeysModel _allowedMobileAppKeys;
        private readonly AllowedWebAppKeysModel _allowedWebAppKeys;
        private readonly bool _iscachingenable;
        private readonly bool _issinglesignon;
        private readonly string _swaggerhiddenversion;
        private readonly IMemoryCache _cache;
        private readonly List<string> _subdomain;
        private readonly AllowedDomainListModel _alloweddomain;
        private readonly AllowedDomainExcludingSubDomainListModel _alloweddomainexcludingsubdomain;
        private readonly string _apiusername;
        private readonly string _apipassword;
        private readonly int _executiontimeout;
        private readonly List<string> _allowanonymousmethods;
        private readonly string _FileServerPath;
        private readonly string _EdiFabricSerialKey;
        private readonly string _ClaimFileSrvPath;
        private readonly string _PublicFileSrvPath;
        private readonly string _defaultdomain;
        private readonly List<string> _AllowedFileExtensionFilePath;
        private readonly IConfiguration _iconfig;
        private readonly string _ApplicationEnvironment;
        private readonly bool _IsLoginCaptchaEnabled;
        public ADORepository(DbStringCollection dbStringCollection, IWebHostEnvironment env, IHttpContextAccessor httpContextAccessor, IConfiguration iconfig, IMemoryCache cache, AllowedDomainListModel alloweddomain, AllowedDomainExcludingSubDomainListModel alloweddomainexcludingsubdomain, AllowedMobileAppKeysModel allowedMobileAppKeys, AllowedWebAppKeysModel allowedWebAppKeys)
        {
            this._dbStringCollection1 = dbStringCollection;
            this._httpContextAccessor = httpContextAccessor;
            this._cache = cache;
            this._virtualdirectory = iconfig.GetValue<string>("VirtualDirectory")!.ToLower();
            this._adminswaggerkey = iconfig.GetValue<string>("AdminSwaggerKey")!.ToLower();
            this._allowedMobileAppKeys = allowedMobileAppKeys;
            this._allowedWebAppKeys = allowedWebAppKeys;
            this._swaggerhiddenversion = iconfig.GetValue<string>("SwaggerHiddenVersion")!.ToLower();
            this._subdomain = iconfig.GetSection("SubDomain").GetChildren().Select(x => x.Value).ToList()!;
            this._iscachingenable = iconfig.GetValue<bool>("IsCachingEnabled");
            this._issinglesignon = iconfig.GetValue<bool>("IsSingleSignOn");
            this._alloweddomain = alloweddomain;
            this._alloweddomainexcludingsubdomain = alloweddomainexcludingsubdomain;
            this._apiusername = iconfig.GetValue<string>("APIUserName")!;
            this._apipassword = iconfig.GetValue<string>("APIPassword")!;
            this._executiontimeout = iconfig.GetValue<int>("ExecutionTimeOut");
            this._allowanonymousmethods = iconfig.GetSection("AllowAnonymousMethods").GetChildren().Select(x => x.Value!.ToLower()).ToList();
            this._FileServerPath = iconfig.GetValue<string>("FileServerPath")!;
            this._EdiFabricSerialKey = iconfig.GetValue<string>("EdiFabricSerialKey")!;
            this._ClaimFileSrvPath = iconfig.GetValue<string>("ClaimFileSrvPath")!;
            this._PublicFileSrvPath = iconfig.GetValue<string>("PublicFileSrvPath")!;
            this._defaultdomain = iconfig.GetValue<string>("DefaultDomain")!;
            this._ApplicationEnvironment = iconfig.GetValue<string>("ApplicationEnvironment")!;
            this._AllowedFileExtensionFilePath = iconfig.GetSection("FileExtensionAllowed").GetChildren().Select(x => x.Value!.ToLower()).ToList();
            this._IsLoginCaptchaEnabled = iconfig.GetValue<bool>("IsLoginCaptchaEnabled");
            this._iconfig = iconfig;
        }
        #endregion Contructor

        #region General
        public bool GetIsLoginCaptchaEnabled()
        {
            return _IsLoginCaptchaEnabled;
        }
        public string GetApplicationEnvironment()
        {
            return _ApplicationEnvironment;
        }
        public bool GetIsSingleSignOn()
        {
            return _issinglesignon;
        }
        public List<string> GetAllowedFileExtension()
        {
            return _AllowedFileExtensionFilePath.ToList();
        }
        public string GetEdiFabricSerialKey()
        {
            return _EdiFabricSerialKey;
        }
        public string GetFileServerPath()
        {
            return _FileServerPath;
        }
        public string GetClaimFileSrvPath()
        {
            return _ClaimFileSrvPath;
        }
        public string GetPublicFileSrvPath()
        {
            return _PublicFileSrvPath;
        }
        public IHttpContextAccessor GetIHttpContextAccessor()
        {
            return _httpContextAccessor;
        }
        public IConfiguration GetIConfiguration()
        {
            return _iconfig;
        }
        public string GetAPIUserName()
        {
            return _apiusername;
        }
        public string GetAPIPassword()
        {
            return _apipassword;
        }
        public HttpClient SetDefaultWebBasedHeaders()
        {
            HttpClient client = new HttpClient();
            {
                string originHeader = string.IsNullOrEmpty(_httpContextAccessor?.HttpContext?.Request?.Headers?.Origin) == false ? _httpContextAccessor?.HttpContext?.Request?.Headers?.Origin : _httpContextAccessor?.HttpContext?.Request?.Headers?.Host;
                string webkey = "";
                if (GetAllowedWebAppKeys().AllowedWebAppKeys.Length > 0)
                    webkey = GetAllowedWebAppKeys().AllowedWebAppKeys[0];
                if (!string.IsNullOrEmpty(originHeader))
                    client.DefaultRequestHeaders.Add("Origin", originHeader);
                if (!string.IsNullOrEmpty(webkey))
                    client.DefaultRequestHeaders.Add("WebKey", webkey);
            }
            return client;
        }
        public void RemoveMemoryCache(string subtype, MemoryCacheValueType? _MemoryCacheValueType = null)
        {
            string paravalue = $"PKey:{subtype}".ToLower();
            _MemoryCacheValueType = (_MemoryCacheValueType == null ? new MemoryCacheValueType() : _MemoryCacheValueType);
            _MemoryCacheValueType._GetMemoryCacheValueType.setkeyparavalues = paravalue;
            _MemoryCacheValueType._GetMemoryCacheValueType.subtype = subtype;
            _MemoryCacheValueType._SetMemoryCacheValueType.subtype = subtype;
            MemoryCaches.RemoveCacheValue(_MemoryCacheValueType, _cache);

        }
        public bool IsValidToken(PublicClaimObjects _PublicClaimObjects, int Seconds)
        {
            bool Ret = true;
            bool isalloweddomain = IsAllowedDomain();
            bool isalloweddomainexcludingsubdomain = IsAllowedDomainExcludingSubDomain();
            if ((isalloweddomain && _PublicClaimObjects.iswebtoken && IsSwaggerCall() == false) || (isalloweddomainexcludingsubdomain && _PublicClaimObjects.iswebtoken && IsSwaggerCall() == false))
            {
                string cachekey = $"{_PublicClaimObjects.jit}{_PublicClaimObjects.key}";
                Ret = MemoryCaches.GetCacheValue(cachekey, _cache, Seconds);
            }
            else if ((isalloweddomain && _PublicClaimObjects.iswebtoken == false) || (isalloweddomainexcludingsubdomain && _PublicClaimObjects.iswebtoken == false))
                Ret = false;
            else if ((isalloweddomain == false && _PublicClaimObjects.iswebtoken) || (isalloweddomainexcludingsubdomain == false && _PublicClaimObjects.iswebtoken))
                Ret = false;

            return Ret;
        }
        public void UpdateTokenKeyCacheTime(PublicClaimObjects _PublicClaimObjects, int Seconds)
        {
            bool isalloweddomain = IsAllowedDomain();
            bool isalloweddomainexcludingsubdomain = IsAllowedDomainExcludingSubDomain();
            if ((isalloweddomain && _PublicClaimObjects.iswebtoken && IsSwaggerCall() == false) || (isalloweddomainexcludingsubdomain && _PublicClaimObjects.iswebtoken && IsSwaggerCall() == false))
            {
                string cachekey = $"{_PublicClaimObjects.jit}{_PublicClaimObjects.key}";
                MemoryCaches.SetTokenCacheValue(cachekey, Seconds, _cache);
            }
        }
        public void AddTokenKeyCacheTime(bool iswebtoken, bool issinglesignon, string jit, string key, int Seconds)
        {
            bool isalloweddomain = IsAllowedDomain();
            bool isalloweddomainexcludingsubdomain = IsAllowedDomainExcludingSubDomain();
            if ((isalloweddomain && issinglesignon == false && IsSwaggerCall() == false) || (isalloweddomainexcludingsubdomain && issinglesignon && IsSwaggerCall() == false))
            {
                string cachekey = $"{jit}{key}";
                MemoryCaches.SetTokenCacheValue(cachekey, Seconds, _cache);
            }
        }
        public void RemoveTokenKeyFromCache(PublicClaimObjects _PublicClaimObjects)
        {
            string cachekey = $"{_PublicClaimObjects.jit}{_PublicClaimObjects.key}";
            MemoryCaches.RemoveCacheValue(cachekey, _cache, true);
        }
        public AllowedDomainListModel GetAllowedRemoteDomain()
        {
            return this._alloweddomain;
        }
        public AllowedMobileAppKeysModel GetAllowedMobileAppKeys()
        {
            return this._allowedMobileAppKeys;
        }
        public AllowedWebAppKeysModel GetAllowedWebAppKeys()
        {
            return this._allowedWebAppKeys;
        }
        public AllowedDomainExcludingSubDomainListModel GetAllowedRemoteDomainExcludingSubDomain()
        {
            return this._alloweddomainexcludingsubdomain;
        }
        public List<string>? GetAllowedRemoteDomainList()
        {
            return GetAllowedRemoteDomain()?.AllowedRemoteDomainAngular?.ToList();
        }
        public List<string>? GetAllowedRemoteDomainExcludingSubDomainList()
        {
            return GetAllowedRemoteDomainExcludingSubDomain()?.AllowedRemoteDomainAngular?.ToList();
        }
        public bool IsAllowAnonymousMethods()
        {
            bool Ret = false;
            string? ReqPath = GetRequestPath();
            if (_allowanonymousmethods != null && ReqPath != null)
            {
                if (ReqPath != "" && _allowanonymousmethods.Count > 0)
                {
                    if (_allowanonymousmethods.Contains(ReqPath))
                        Ret = true;
                }
            }
            return Ret;
        }
        public bool IsAllowedDomain()
        {
            bool Ret = false;
            string _localhost = "localhost";
            if (GetRemoteDomain() != "")
            {
                if (GetAllowedRemoteDomainList() != null)
                {
                    string webkey = GetRemoteDomainWebKey();
                    if (webkey != "")
                    {
                        if (GetAllowedWebAppKeys().AllowedWebAppKeys.Contains(webkey))
                        {
                            if (GetAllowedRemoteDomainList().Contains(GetRemoteDomain() + "/"))
                                Ret = true;
                            else if (Microsoft.VisualBasic.Strings.Left(GetRemoteDomain().Replace("https://", "").Replace("http://", ""), Microsoft.VisualBasic.Strings.Len(_localhost)) == _localhost)
                                Ret = true;
                        }
                    }
                }
            }
            return Ret;
        }
        public bool IsAllowedDomainExcludingSubDomain()
        {
            bool Ret = false;
            string _localhost = "localhost";
            if (GetRemoteDomainExcludingSubDomain() != "")
            {
                if (GetAllowedRemoteDomainExcludingSubDomainList() != null)
                {
                    string webkey = GetRemoteDomainWebKey();
                    if (webkey != "")
                    {
                        if (GetAllowedWebAppKeys().AllowedWebAppKeys.Contains(webkey))
                        {
                            if (GetAllowedRemoteDomainExcludingSubDomainList().Contains(GetRemoteDomainExcludingSubDomain()))
                                Ret = true;
                            else if (Microsoft.VisualBasic.Strings.Left(GetRemoteDomainExcludingSubDomain().Replace("https://", "").Replace("http://", ""), Microsoft.VisualBasic.Strings.Len(_localhost)) == _localhost)
                                Ret = true;
                        }
                    }
                }
            }
            return Ret;
        }
        public PublicClaimObjects GetPublicClaimObjects()
        {
            PublicClaimObjects _PublicClaimObjects;
            _PublicClaimObjects = _httpContextAccessor?.HttpContext?.Items["PublicClaimObjects"] as PublicClaimObjects;
            if (_PublicClaimObjects == null)
                _PublicClaimObjects = new PublicClaimObjects();
            return _PublicClaimObjects;
        }
        public async Task<string> GetRequestBodyString()
        {
            string RequestBodyString = "";
            try
            {
                if (_httpContextAccessor?.HttpContext != null)
                {
                    _httpContextAccessor.HttpContext.Request.EnableBuffering();

                    using (var memoryStream = new MemoryStream())
                    {
                        await _httpContextAccessor.HttpContext.Request.Body.CopyToAsync(memoryStream);
                        memoryStream.Seek(0, SeekOrigin.Begin);

                        using (var reader = new StreamReader(memoryStream))
                        {
                            RequestBodyString = await reader.ReadToEndAsync();
                        }
                    }

                    // Reset the position of the original request body stream
                    _httpContextAccessor.HttpContext.Request.Body.Position = 0;
                }

            }
            catch (Exception ex)
            {
                StaticPublicObjects.logFile.ErrorLog(FunctionName: "GetRequestBodyString", SmallMessage: ex.Message, Message: ex.ToString());
                //StaticPublicObjects.logger.Error(ex, ex.Message);
            }
            return RequestBodyString;
        }
        public ClaimsPrincipal? GetUserClaim()
        {
            ClaimsPrincipal? User = _httpContextAccessor?.HttpContext?.User;
            return User;
        }
        public string GetVirtualDirectory()
        {
            return _virtualdirectory;
        }
        public string GetSwaggerHiddenVersion()
        {
            return _swaggerhiddenversion;
        }
        public string GetAdminSwaggerKey()
        {
            return _adminswaggerkey;
        }
        public bool GetIsCachingEnabled()
        {
            return _iscachingenable;
        }
        public bool IsDevelopment()
        {
            bool Ret = false;
            try
            {
                string hostname = GetHostName();
                if (hostname == "localhost")
                    Ret = true;
            }
            catch (Exception ex)
            {
                StaticPublicObjects.logFile.ErrorLog(FunctionName: "IsDevelopment", SmallMessage: ex.Message, Message: ex.ToString());
            }
            return Ret;
        }
        public string GetHostName()
        {
            string? hostname = "";
            try
            {
                hostname = _httpContextAccessor?.HttpContext?.Request?.Host.Host;
                hostname = (hostname == null ? "" : hostname.ToLower());
            }
            catch (Exception ex)
            {
                StaticPublicObjects.logFile.ErrorLog(FunctionName: "GetHostName", SmallMessage: ex.Message, Message: ex.ToString());
            }
            return hostname;
        }
        public string? GetSubDomain()
        {
            string? hostnamesubdomain = "";
            try
            {
                if (StaticPublicObjects.subdomain == null)
                {
                    hostnamesubdomain = GetHostName();
                    if (hostnamesubdomain == "")
                        return null;

                    if (hostnamesubdomain != "")
                    {
                        if (_subdomain.Count > 0)
                        {
                            for (int i = 0; i < _subdomain.Count; i++)
                            {
                                if (hostnamesubdomain.Contains($"{_subdomain[i].ToLower()}."))
                                {
                                    if (hostnamesubdomain.Split('.')[0] == $"{_subdomain[i].ToLower()}")
                                    {
                                        hostnamesubdomain = _subdomain[i].ToLower();
                                        StaticPublicObjects.subdomain = hostnamesubdomain;
                                        break;
                                    }
                                }
                            }
                        }
                        else
                        {
                            hostnamesubdomain = "";
                        }
                    }
                }
                else
                {
                    hostnamesubdomain = StaticPublicObjects.subdomain;
                }
            }
            catch (Exception ex)
            {
                StaticPublicObjects.logFile.ErrorLog(FunctionName: "GetSubDomain", SmallMessage: ex.Message, Message: ex.ToString());
            }
            return hostnamesubdomain;
        }
        public string? GetRemoteSubDomain()
        {
            string? remotesubdomain = "";
            try
            {
                remotesubdomain = GetRemoteDomain();
                if (remotesubdomain != "")
                {
                    remotesubdomain.Replace("https://", "").Replace("http://", "");
                    remotesubdomain = remotesubdomain.Split('/')[0].Split('.')[0];
                }
            }
            catch (Exception ex)
            {
                StaticPublicObjects.logFile.ErrorLog(FunctionName: "GetSubDomain", SmallMessage: ex.Message, Message: ex.ToString());
            }
            return remotesubdomain;
        }
        public string GetHostNameExcludingSubDomain()
        {
            string? hostnameexcludingsubdomain = "";
            try
            {
                hostnameexcludingsubdomain = GetHostName();
                if (hostnameexcludingsubdomain != "")
                {
                    string subdomain = GetSubDomain();
                    if (subdomain != "")
                    {
                        hostnameexcludingsubdomain = hostnameexcludingsubdomain.Replace(subdomain + ".", "");
                    }
                    if (subdomain.ToLower() == "localhost" || hostnameexcludingsubdomain.ToLower() == "localhost")
                        hostnameexcludingsubdomain = _defaultdomain;
                }
            }
            catch (Exception ex)
            {
                StaticPublicObjects.logFile.ErrorLog(FunctionName: "GetHostNameExcludingSubDomain", SmallMessage: ex.Message, Message: ex.ToString());
            }
            return hostnameexcludingsubdomain;
        }
        public string GetRemoteDomainExcludingSubDomain()
        {
            string remotedomainexcludingsubdomain = "";
            try
            {
                remotedomainexcludingsubdomain = GetRemoteDomain().Replace("https://", "").Replace("http://", "");
                if (remotedomainexcludingsubdomain != "")
                {
                    string subdomain = GetRemoteSubDomain();
                    if (subdomain != "")
                    {
                        remotedomainexcludingsubdomain = remotedomainexcludingsubdomain.Replace(subdomain + ".", "");
                    }
                    if (subdomain.ToLower() == "localhost" || remotedomainexcludingsubdomain.ToLower() == "localhost")
                        remotedomainexcludingsubdomain = _defaultdomain;
                }
            }
            catch (Exception ex)
            {
                StaticPublicObjects.logFile.ErrorLog(FunctionName: "GetRemoteDomainExcludingSubDomain", SmallMessage: ex.Message, Message: ex.ToString());
            }
            return remotedomainexcludingsubdomain;
        }
        public string GetRemoteDomain()
        {
            string? remotedomain = "";
            try
            {
                if (_httpContextAccessor?.HttpContext?.Request?.Method.ToUpper() != "GET")
                {
                    remotedomain = _httpContextAccessor?.HttpContext?.Request?.Headers?.Origin.ToString().ToLower();
                    if (string.IsNullOrEmpty(remotedomain) == false)
                    {
                        remotedomain = _httpContextAccessor?.HttpContext?.Request?.Headers?.Origin.ToString().ToLower();
                        if (Strings.Left(remotedomain, 8) == "https://")
                            remotedomain = Strings.Mid(remotedomain, 9, Strings.Len(remotedomain));
                        else if (Strings.Left(remotedomain, 7) == "http://")
                            remotedomain = Strings.Mid(remotedomain, 8, Strings.Len(remotedomain));
                    }
                    else
                    {
                        remotedomain = _httpContextAccessor?.HttpContext?.Request?.Headers["Origin"];
                        remotedomain = (remotedomain == null ? "" : remotedomain.ToLower());
                    }
                }
                else
                {
                    remotedomain = _httpContextAccessor?.HttpContext?.Request?.Host.Value;

                }
                remotedomain = (remotedomain == null ? "" : remotedomain.ToLower());
            }
            catch (Exception ex)
            {
                StaticPublicObjects.logFile.ErrorLog(FunctionName: "GetRemoteDomain", SmallMessage: ex.Message, Message: ex.ToString());
            }
            return remotedomain;
        }
        public string GetRemoteDomainWebKey()
        {
            string? remotedomainwebkey = "";
            try
            {
                remotedomainwebkey = _httpContextAccessor?.HttpContext?.Request?.Headers["WebKey"];
                remotedomainwebkey = (remotedomainwebkey == null ? "" : remotedomainwebkey.ToLower());
            }
            catch (Exception ex)
            {
                StaticPublicObjects.logFile.ErrorLog(FunctionName: "GetRemoteDomainWebKey", SmallMessage: ex.Message, Message: ex.ToString());
            }
            return remotedomainwebkey;
        }
        public string GetRemoteRefererURL()
        {
            string? RefererURL = "";
            try
            {
                string RemoteDomain = "";
                RemoteDomain = _httpContextAccessor?.HttpContext?.Request?.Headers?.Origin.ToString().ToLower();
                if (RemoteDomain != "")
                {
                    RefererURL = _httpContextAccessor?.HttpContext?.Request?.Headers?.Referer.ToString().ToLower();
                    RefererURL = (RefererURL == null ? "" : RefererURL.ToLower());
                    if (RefererURL != "")
                    {
                        if (Strings.Len(RefererURL) > Strings.Len(RemoteDomain))
                        {
                            RefererURL = Strings.Mid(RefererURL, Strings.Len(RemoteDomain) + 1, Strings.Len(RefererURL));
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                StaticPublicObjects.logFile.ErrorLog(FunctionName: "GetRemoteRefererURL", SmallMessage: ex.Message, Message: ex.ToString());
            }
            return RefererURL;
        }
        public string GetRemoteURL()
        {
            string? remoteurl = "";
            try
            {
                remoteurl = _httpContextAccessor?.HttpContext?.Request?.Headers?.Referer.ToString();
                remoteurl = (remoteurl == null ? "" : (remoteurl.ToString() == "null" ? "" : remoteurl.ToString().ToLower()));
            }
            catch (Exception ex)
            {
                StaticPublicObjects.logFile.ErrorLog(FunctionName: "GetRemoteURL", SmallMessage: ex.Message, Message: ex.ToString());
            }
            return remoteurl;
        }
        public string GetHostURL()
        {
            string? hosturl = "";
            try
            {
                hosturl = _httpContextAccessor?.HttpContext?.Request?.Headers?.Referer.ToString();
                hosturl = (hosturl == null ? "" : (hosturl.ToString() == "null" ? "" : hosturl.ToString().ToLower()));
            }
            catch (Exception ex)
            {
                StaticPublicObjects.logFile.ErrorLog(FunctionName: "GetHostURL", SmallMessage: ex.Message, Message: ex.ToString());
            }
            return hosturl;
        }
        public bool IsSwaggerCall()
        {
            bool Ret = false;
            try
            {
                string hostname = GetHostName();
                if (hostname != null)
                {
                    string hosturl = GetHostURL();//.Replace(_httpContextAccessor.HttpContext.Request.Headers.Origin.ToString() + "/", "").ToLower();
                    //Ret = (hosturl == _virtualdirectory + "swagger/ui/index.html" ? true : false);
                    Ret = (hosturl.IndexOf("/swagger/") >= 0 && hosturl.IndexOf("/index.html") >= 0 ? true : false);
                }
            }
            catch (Exception ex)
            {
                StaticPublicObjects.logFile.ErrorLog(FunctionName: "IsSwaggerCall", SmallMessage: ex.Message, Message: ex.ToString());
            }
            return Ret;
        }
        public bool IsSwaggerCallAdmin()
        {
            bool Ret = false;
            try
            {
                if (IsSwaggerCall())
                {
                    string hosturl = GetHostURL();
                    Ret = (hosturl.IndexOf($"id={_adminswaggerkey}") >= 0 ? true : false);
                }
            }
            catch (Exception ex)
            {
                StaticPublicObjects.logFile.ErrorLog(FunctionName: "IsSwaggerCallAdmin", SmallMessage: ex.Message, Message: ex.ToString());
            }
            return Ret;
        }
        public void P_CacheEntry_IU(string cacheKey, string cacheValue, DateTime? expirationTime, int? applicationID = null)
        {
            try
            {

                if (cacheKey == "")
                    return;
                else if (expirationTime == null)
                    return;

                DateTime dateTime = (DateTime)expirationTime;

                List<Dynamic_SP_Params> List_Dynamic_SP_Params = new List<Dynamic_SP_Params>();
                Dynamic_SP_Params dynamic_SP_Params = new Dynamic_SP_Params();

                dynamic_SP_Params = new Dynamic_SP_Params();
                dynamic_SP_Params.ParameterName = "applicationID";
                if (applicationID == null)
                    dynamic_SP_Params.Val = (GetPublicClaimObjects().iswebtoken == true ? AppEnum.ApplicationId.CareerPortalAppID : AppEnum.ApplicationId.AppID);
                else
                    dynamic_SP_Params.Val = (int)applicationID;
                List_Dynamic_SP_Params.Add(dynamic_SP_Params);

                dynamic_SP_Params = new Dynamic_SP_Params();
                dynamic_SP_Params.ParameterName = "cacheKey";
                dynamic_SP_Params.Val = cacheKey;
                List_Dynamic_SP_Params.Add(dynamic_SP_Params);

                dynamic_SP_Params = new Dynamic_SP_Params();
                dynamic_SP_Params.ParameterName = "cacheValue";
                dynamic_SP_Params.Val = cacheValue;
                List_Dynamic_SP_Params.Add(dynamic_SP_Params);

                dynamic_SP_Params = new Dynamic_SP_Params();
                dynamic_SP_Params.ParameterName = "expirationTime";
                dynamic_SP_Params.Val = dateTime.ToString("yyyy-MM-dd HH:mm:ss.fffffff");
                List_Dynamic_SP_Params.Add(dynamic_SP_Params);

                ExecuteStoreProcedureNONQuery("P_CacheEntry_IU", ref List_Dynamic_SP_Params);
            }
            catch (Exception ex)
            {
                StaticPublicObjects.logFile.ErrorLog(FunctionName: "P_CacheEntry_IU", SmallMessage: ex.Message, Message: ex.ToString());
            }
        }
        public void P_CacheEntry_Delete(string cacheKey)
        {
            try
            {
                if (cacheKey == "")
                    return;

                List<Dynamic_SP_Params> List_Dynamic_SP_Params = new List<Dynamic_SP_Params>();
                Dynamic_SP_Params dynamic_SP_Params = new Dynamic_SP_Params();

                dynamic_SP_Params = new Dynamic_SP_Params();
                dynamic_SP_Params.ParameterName = "applicationID";
                dynamic_SP_Params.Val = (GetPublicClaimObjects().iswebtoken == true ? AppEnum.ApplicationId.CareerPortalAppID : AppEnum.ApplicationId.AppID);
                List_Dynamic_SP_Params.Add(dynamic_SP_Params);

                dynamic_SP_Params = new Dynamic_SP_Params();
                dynamic_SP_Params.ParameterName = "cacheKey";
                dynamic_SP_Params.Val = cacheKey;
                List_Dynamic_SP_Params.Add(dynamic_SP_Params);

                ExecuteStoreProcedureNONQuery("P_CacheEntry_Delete", ref List_Dynamic_SP_Params);
            }
            catch (Exception ex)
            {
                StaticPublicObjects.logFile.ErrorLog(FunctionName: "P_CacheEntry_Delete", SmallMessage: ex.Message, Message: ex.ToString());
            }
        }
        public DataRow? P_Get_CacheEntry(string cacheKey)
        {
            DataRow? DR = null;
            try
            {
                if (cacheKey == "")
                    return DR;

                List<Dynamic_SP_Params> List_Dynamic_SP_Params = new List<Dynamic_SP_Params>();
                Dynamic_SP_Params dynamic_SP_Params = new Dynamic_SP_Params();

                dynamic_SP_Params = new Dynamic_SP_Params();
                dynamic_SP_Params.ParameterName = "applicationID";
                dynamic_SP_Params.Val = (GetPublicClaimObjects().iswebtoken == true ? AppEnum.ApplicationId.CareerPortalAppID : AppEnum.ApplicationId.AppID);
                List_Dynamic_SP_Params.Add(dynamic_SP_Params);

                dynamic_SP_Params = new Dynamic_SP_Params();
                dynamic_SP_Params.ParameterName = "cacheKey";
                dynamic_SP_Params.Val = cacheKey;
                List_Dynamic_SP_Params.Add(dynamic_SP_Params);

                DR = ExecuteStoreProcedureDR("P_Get_CacheEntry", ref List_Dynamic_SP_Params);
            }
            catch (Exception ex)
            {
                DR = null;
                StaticPublicObjects.logFile.ErrorLog(FunctionName: "P_Get_CacheEntry", SmallMessage: ex.Message, Message: ex.ToString());
            }
            return DR;
        }
        public DataTable P_Add_Session_History(int? WebUserID, string Username, string SessionID, int DeviceTypeID, DateTime LoginTime, int ApplicationID, bool IsSuccess, string Latitude, string Longitude)
        {
            List<Dynamic_SP_Params> Dynamic_SP_Params_List = new List<Dynamic_SP_Params>();
            Dynamic_SP_Params Dynamic_SP_Params;

            Dynamic_SP_Params = new Dynamic_SP_Params();
            Dynamic_SP_Params.ParameterName = "WebUserID";
            Dynamic_SP_Params.Val = WebUserID;
            Dynamic_SP_Params_List.Add(Dynamic_SP_Params);

            Dynamic_SP_Params = new Dynamic_SP_Params();
            Dynamic_SP_Params.ParameterName = "Username";
            Dynamic_SP_Params.Val = Username;
            Dynamic_SP_Params_List.Add(Dynamic_SP_Params);

            Dynamic_SP_Params = new Dynamic_SP_Params();
            Dynamic_SP_Params.ParameterName = "SessionID";
            Dynamic_SP_Params.Val = SessionID;
            Dynamic_SP_Params_List.Add(Dynamic_SP_Params);

            Dynamic_SP_Params = new Dynamic_SP_Params();
            Dynamic_SP_Params.ParameterName = "DeviceTypeID";
            Dynamic_SP_Params.Val = DeviceTypeID;
            Dynamic_SP_Params_List.Add(Dynamic_SP_Params);

            Dynamic_SP_Params = new Dynamic_SP_Params();
            Dynamic_SP_Params.ParameterName = "LoginTime";
            Dynamic_SP_Params.Val = LoginTime;
            Dynamic_SP_Params_List.Add(Dynamic_SP_Params);

            Dynamic_SP_Params = new Dynamic_SP_Params();
            Dynamic_SP_Params.ParameterName = "Latitude";
            Dynamic_SP_Params.Val = Latitude;
            Dynamic_SP_Params_List.Add(Dynamic_SP_Params);

            Dynamic_SP_Params = new Dynamic_SP_Params();
            Dynamic_SP_Params.ParameterName = "Longitude";
            Dynamic_SP_Params.Val = Longitude;
            Dynamic_SP_Params_List.Add(Dynamic_SP_Params);

            Dynamic_SP_Params = new Dynamic_SP_Params();
            Dynamic_SP_Params.ParameterName = "LocalIPAddress";
            Dynamic_SP_Params.Val = GetLocalIPAddress();
            Dynamic_SP_Params_List.Add(Dynamic_SP_Params);

            Dynamic_SP_Params = new Dynamic_SP_Params();
            Dynamic_SP_Params.ParameterName = "InternetAddress";
            Dynamic_SP_Params.Val = GetLocalIPAddress();
            Dynamic_SP_Params_List.Add(Dynamic_SP_Params);

            Dynamic_SP_Params = new Dynamic_SP_Params();
            Dynamic_SP_Params.ParameterName = "ApplicationID";
            Dynamic_SP_Params.Val = ApplicationID;
            Dynamic_SP_Params_List.Add(Dynamic_SP_Params);

            Dynamic_SP_Params = new Dynamic_SP_Params();
            Dynamic_SP_Params.ParameterName = "IsSuccess";
            Dynamic_SP_Params.Val = IsSuccess;
            Dynamic_SP_Params_List.Add(Dynamic_SP_Params);

            return ExecuteStoreProcedureDT("P_Add_Session_History", ref Dynamic_SP_Params_List);
        }
        public string GetLocalIPAddress()
        {
            string? Return = "";
            Return = this._httpContextAccessor?.HttpContext?.Connection?.RemoteIpAddress?.ToString();
            return (Return == null ? "" : Return);
        }
        public string GetRequestPath()
        {
            string? Return = "";
            Return = this._httpContextAccessor?.HttpContext?.Request?.Path.ToString();
            return Return = (Return == null ? "" : Return.ToLower().Replace("/v1", "").Replace("/" + GetSwaggerHiddenVersion(), ""));
        }
        public int GetMethodIDFromPath()
        {
            string _Path = GetRequestPath();
            int MethodID = 0;
            if (_Path == "/api/account/signinasync")
                MethodID = AppEnum.APIMethods.SignInAsync;
            else if (_Path == "/api/account/refreshtoken")
                MethodID = AppEnum.APIMethods.RefreshToken;
            else if (_Path == "/api/account/resetmemorycache")
                MethodID = AppEnum.APIMethods.ResetMemoryCache;

            return MethodID;
        }
        public DataRow? P_Get_T_Config_Detail(string Config_Key, MemoryCacheValueType? _MemoryCacheValueType = null)
        {
            string paravalue = $"Config_Key:{Config_Key}".ToLower();
            _MemoryCacheValueType = (_MemoryCacheValueType == null ? new MemoryCacheValueType() : _MemoryCacheValueType);
            _MemoryCacheValueType._GetMemoryCacheValueType.setkeyparavalues = paravalue;
            _MemoryCacheValueType._GetMemoryCacheValueType.subtype = CacheSubType.P_Get_T_Config_Detail;
            _MemoryCacheValueType._SetMemoryCacheValueType.subtype = CacheSubType.P_Get_T_Config_Detail;
            DataRow? result = null;
            if (MemoryCaches.GetCacheValue(_MemoryCacheValueType, ref result, _cache, _iscachingenable))
                return result;

            List<Dynamic_SP_Params> Dynamic_SP_Params_List = new List<Dynamic_SP_Params>();
            Dynamic_SP_Params Dynamic_SP_Params;

            Dynamic_SP_Params = new Dynamic_SP_Params();
            Dynamic_SP_Params.ParameterName = "Config_Key";
            Dynamic_SP_Params.Val = Config_Key;
            Dynamic_SP_Params_List.Add(Dynamic_SP_Params);

            result = ExecuteStoreProcedureDR("P_Get_T_Config_Detail", ref Dynamic_SP_Params_List);

            MemoryCaches.SetCacheValue(_MemoryCacheValueType, result, _cache, _iscachingenable);

            return result;
        }
        public DataRow? P_Get_API_User_Map_Request_Limit(int UserID, int MethodID, string? Username, MemoryCacheValueType? _MemoryCacheValueType = null)
        {
            string paravalue = $"UserID:{UserID}|MethodID:{MethodID}|Username:{(Username == null ? "" : Username)}".ToLower();
            _MemoryCacheValueType = (_MemoryCacheValueType == null ? new MemoryCacheValueType() : _MemoryCacheValueType);
            _MemoryCacheValueType._GetMemoryCacheValueType.setkeyparavalues = paravalue;
            _MemoryCacheValueType._GetMemoryCacheValueType.subtype = CacheSubType.P_Get_API_User_Map_Request_Limit;
            _MemoryCacheValueType._SetMemoryCacheValueType.subtype = CacheSubType.P_Get_API_User_Map_Request_Limit;
            DataRow? result = null;
            if (MemoryCaches.GetCacheValue(_MemoryCacheValueType, ref result, _cache, _iscachingenable))
                return result;

            List<Dynamic_SP_Params> Dynamic_SP_Params_List = new List<Dynamic_SP_Params>();
            Dynamic_SP_Params Dynamic_SP_Params;

            Dynamic_SP_Params = new Dynamic_SP_Params();
            Dynamic_SP_Params.ParameterName = "UserID";
            Dynamic_SP_Params.Val = UserID;
            Dynamic_SP_Params_List.Add(Dynamic_SP_Params);

            Dynamic_SP_Params = new Dynamic_SP_Params();
            Dynamic_SP_Params.ParameterName = "MethodID";
            Dynamic_SP_Params.Val = MethodID;
            Dynamic_SP_Params_List.Add(Dynamic_SP_Params);

            Dynamic_SP_Params = new Dynamic_SP_Params();
            Dynamic_SP_Params.ParameterName = "Username";
            Dynamic_SP_Params.Val = (Username == null ? DBNull.Value : Username.ToUpper());
            Dynamic_SP_Params_List.Add(Dynamic_SP_Params);

            result = ExecuteStoreProcedureDR("P_Get_API_User_Map_Request_Limit", ref Dynamic_SP_Params_List);

            MemoryCaches.SetCacheValue(_MemoryCacheValueType, result, _cache, _iscachingenable);

            return result;
        }
        public DataTable P_Get_API_User_Map(int UserID, int APIID, string? Username, MemoryCacheValueType? _MemoryCacheValueType = null)
        {
            string paravalue = $"UserID:{UserID}|APIID:{APIID}|Username:{(Username == null ? "" : Username)}".ToLower();
            _MemoryCacheValueType = (_MemoryCacheValueType == null ? new MemoryCacheValueType() : _MemoryCacheValueType);
            _MemoryCacheValueType._GetMemoryCacheValueType.setkeyparavalues = paravalue;
            _MemoryCacheValueType._GetMemoryCacheValueType.subtype = CacheSubType.P_Get_API_User_Map;
            _MemoryCacheValueType._SetMemoryCacheValueType.subtype = CacheSubType.P_Get_API_User_Map;
            DataTable result = new DataTable();
            if (MemoryCaches.GetCacheValue(_MemoryCacheValueType, ref result, _cache, _iscachingenable))
                return result;

            List<Dynamic_SP_Params> Dynamic_SP_Params_List = new List<Dynamic_SP_Params>();
            Dynamic_SP_Params Dynamic_SP_Params;

            Dynamic_SP_Params = new Dynamic_SP_Params();
            Dynamic_SP_Params.ParameterName = "UserID";
            Dynamic_SP_Params.Val = UserID;
            Dynamic_SP_Params_List.Add(Dynamic_SP_Params);

            Dynamic_SP_Params = new Dynamic_SP_Params();
            Dynamic_SP_Params.ParameterName = "APIID";
            Dynamic_SP_Params.Val = APIID;
            Dynamic_SP_Params_List.Add(Dynamic_SP_Params);

            Dynamic_SP_Params = new Dynamic_SP_Params();
            Dynamic_SP_Params.ParameterName = "Username";
            Dynamic_SP_Params.Val = (Username == null ? DBNull.Value : Username.ToUpper());
            Dynamic_SP_Params_List.Add(Dynamic_SP_Params);

            result = ExecuteStoreProcedureDT("P_Get_API_User_Map", ref Dynamic_SP_Params_List);

            MemoryCaches.SetCacheValue(_MemoryCacheValueType, result, _cache, _iscachingenable);

            return result;
        }
        public DataRow? P_Get_API_RemoteDomain_IP_Request_Limit(bool Is_RemoteDomain, int MethodID, MemoryCacheValueType? _MemoryCacheValueType = null)
        {
            DataRow? result = null;
            if (Is_RemoteDomain && GetRemoteDomain().ToLower() == "")
                return result;

            string paravalue = $"RemoteDomain_Or_IP:{(Is_RemoteDomain == true ? GetRemoteDomain().ToLower() : GetLocalIPAddress())}|Is_RemoteDomain:{Is_RemoteDomain}|MethodID:{MethodID}".ToLower();
            _MemoryCacheValueType = (_MemoryCacheValueType == null ? new MemoryCacheValueType() : _MemoryCacheValueType);
            _MemoryCacheValueType._GetMemoryCacheValueType.setkeyparavalues = paravalue;
            _MemoryCacheValueType._GetMemoryCacheValueType.subtype = CacheSubType.P_Get_API_RemoteDomain_IP_Request_Limit;
            _MemoryCacheValueType._SetMemoryCacheValueType.subtype = CacheSubType.P_Get_API_RemoteDomain_IP_Request_Limit;
            if (MemoryCaches.GetCacheValue(_MemoryCacheValueType, ref result, _cache, _iscachingenable))
                return result;

            List<Dynamic_SP_Params> Dynamic_SP_Params_List = new List<Dynamic_SP_Params>();
            Dynamic_SP_Params Dynamic_SP_Params;

            Dynamic_SP_Params = new Dynamic_SP_Params();
            Dynamic_SP_Params.ParameterName = "RemoteDomain_Or_IP";
            Dynamic_SP_Params.Val = (Is_RemoteDomain == true ? GetRemoteDomain().ToLower() : GetLocalIPAddress());
            Dynamic_SP_Params_List.Add(Dynamic_SP_Params);

            Dynamic_SP_Params = new Dynamic_SP_Params();
            Dynamic_SP_Params.ParameterName = "MethodID";
            Dynamic_SP_Params.Val = MethodID;
            Dynamic_SP_Params_List.Add(Dynamic_SP_Params);

            Dynamic_SP_Params = new Dynamic_SP_Params();
            Dynamic_SP_Params.ParameterName = "Is_RemoteDomain";
            Dynamic_SP_Params.Val = Is_RemoteDomain;
            Dynamic_SP_Params_List.Add(Dynamic_SP_Params);

            result = ExecuteStoreProcedureDR("P_Get_API_RemoteDomain_IP_Request_Limit", ref Dynamic_SP_Params_List);

            MemoryCaches.SetCacheValue(_MemoryCacheValueType, result, _cache, _iscachingenable);

            return result;
        }
        public DataRow? P_Get_API_RemoteDomain_IP_WhiteListing(bool Is_RemoteDomain, int APIID, MemoryCacheValueType? _MemoryCacheValueType = null)
        {
            string paravalue = $"|RemoteDomain_Or_IP:{(Is_RemoteDomain == true ? GetRemoteDomain().ToLower() : GetLocalIPAddress())}|Is_RemoteDomain:{Is_RemoteDomain}|APIID:{APIID}".ToLower();
            _MemoryCacheValueType = (_MemoryCacheValueType == null ? new MemoryCacheValueType() : _MemoryCacheValueType);
            _MemoryCacheValueType._GetMemoryCacheValueType.setkeyparavalues = paravalue;
            _MemoryCacheValueType._GetMemoryCacheValueType.subtype = CacheSubType.P_Get_API_RemoteDomain_IP_WhiteListing;
            _MemoryCacheValueType._SetMemoryCacheValueType.subtype = CacheSubType.P_Get_API_RemoteDomain_IP_WhiteListing;
            DataRow? result = null;
            if (MemoryCaches.GetCacheValue(_MemoryCacheValueType, ref result, _cache, _iscachingenable))
                return result;

            List<Dynamic_SP_Params> Dynamic_SP_Params_List = new List<Dynamic_SP_Params>();
            Dynamic_SP_Params Dynamic_SP_Params;

            Dynamic_SP_Params = new Dynamic_SP_Params();
            Dynamic_SP_Params.ParameterName = "RemoteDomain_Or_IP";
            Dynamic_SP_Params.Val = (Is_RemoteDomain == true ? GetRemoteDomain().ToLower() : GetLocalIPAddress());
            Dynamic_SP_Params_List.Add(Dynamic_SP_Params);

            Dynamic_SP_Params = new Dynamic_SP_Params();
            Dynamic_SP_Params.ParameterName = "APIID";
            Dynamic_SP_Params.Val = APIID;
            Dynamic_SP_Params_List.Add(Dynamic_SP_Params);

            Dynamic_SP_Params = new Dynamic_SP_Params();
            Dynamic_SP_Params.ParameterName = "Is_RemoteDomain";
            Dynamic_SP_Params.Val = Is_RemoteDomain;
            Dynamic_SP_Params_List.Add(Dynamic_SP_Params);

            result = ExecuteStoreProcedureDR("P_Get_API_RemoteDomain_IP_WhiteListing", ref Dynamic_SP_Params_List);

            MemoryCaches.SetCacheValue(_MemoryCacheValueType, result, _cache, _iscachingenable);

            return result;
        }
        public DataTable P_Get_List_By_ID(int MT_ID, string SELLER_KEY, string? SELLER_CODE = null, string? SELLER_NAME = null, MemoryCacheValueType? _MemoryCacheValueType = null)
        {
            string TempSELLER_CODE = (SELLER_CODE == null ? "" : SELLER_CODE);
            string TempSELLER_NAME = (SELLER_NAME == null ? "" : SELLER_NAME);
            string paravalue = $"MT_ID:{MT_ID}|SELLER_KEY:{SELLER_KEY}|SELLER_CODE:{TempSELLER_CODE}|SELLER_NAME:{TempSELLER_NAME}".ToLower();
            _MemoryCacheValueType = (_MemoryCacheValueType == null ? new MemoryCacheValueType() : _MemoryCacheValueType);
            _MemoryCacheValueType._GetMemoryCacheValueType.setkeyparavalues = paravalue;
            _MemoryCacheValueType._GetMemoryCacheValueType.subtype = CacheSubType.P_Get_List_By_ID;
            _MemoryCacheValueType._SetMemoryCacheValueType.subtype = CacheSubType.P_Get_List_By_ID;
            DataTable result = new DataTable();
            if (MemoryCaches.GetCacheValue(_MemoryCacheValueType, ref result, _cache, _iscachingenable))
                return result;

            List<Dynamic_SP_Params> Dynamic_SP_Params_List = new List<Dynamic_SP_Params>();
            Dynamic_SP_Params Dynamic_SP_Params;

            Dynamic_SP_Params = new Dynamic_SP_Params();
            Dynamic_SP_Params.ParameterName = "MT_ID";
            Dynamic_SP_Params.Val = MT_ID;
            Dynamic_SP_Params_List.Add(Dynamic_SP_Params);

            Dynamic_SP_Params = new Dynamic_SP_Params();
            Dynamic_SP_Params.ParameterName = "SELLER_KEY";
            Dynamic_SP_Params.Val = SELLER_KEY;
            Dynamic_SP_Params_List.Add(Dynamic_SP_Params);

            Dynamic_SP_Params = new Dynamic_SP_Params();
            Dynamic_SP_Params.ParameterName = "SELLER_CODE";
            Dynamic_SP_Params.Val = SELLER_CODE;
            Dynamic_SP_Params_List.Add(Dynamic_SP_Params);

            Dynamic_SP_Params = new Dynamic_SP_Params();
            Dynamic_SP_Params.ParameterName = "SELLER_NAME";
            Dynamic_SP_Params.Val = SELLER_NAME;
            Dynamic_SP_Params_List.Add(Dynamic_SP_Params);

            result = ExecuteStoreProcedureDT("P_Get_List_By_ID", ref Dynamic_SP_Params_List);

            MemoryCaches.SetCacheValue(_MemoryCacheValueType, result, _cache, _iscachingenable);

            return result;
        }
        public DataTable P_Get_List_By_ID_2(int MT_ID, string? Username = null, MemoryCacheValueType? _MemoryCacheValueType = null)
        {
            string TempUsername = (Username == null ? "" : Username);
            string paravalue = $"MT_ID:{MT_ID}|Username:{TempUsername}".ToLower();
            _MemoryCacheValueType = (_MemoryCacheValueType == null ? new MemoryCacheValueType() : _MemoryCacheValueType);
            _MemoryCacheValueType._GetMemoryCacheValueType.setkeyparavalues = paravalue;
            _MemoryCacheValueType._GetMemoryCacheValueType.subtype = CacheSubType.P_Get_List_By_ID_2;
            _MemoryCacheValueType._SetMemoryCacheValueType.subtype = CacheSubType.P_Get_List_By_ID_2;
            DataTable result = new DataTable();
            if (MemoryCaches.GetCacheValue(_MemoryCacheValueType, ref result, _cache, _iscachingenable))
                return result;

            List<Dynamic_SP_Params> Dynamic_SP_Params_List = new List<Dynamic_SP_Params>();
            Dynamic_SP_Params Dynamic_SP_Params;

            Dynamic_SP_Params = new Dynamic_SP_Params();
            Dynamic_SP_Params.ParameterName = "MT_ID";
            Dynamic_SP_Params.Val = MT_ID;
            Dynamic_SP_Params_List.Add(Dynamic_SP_Params);

            Dynamic_SP_Params = new Dynamic_SP_Params();
            Dynamic_SP_Params.ParameterName = "Username";
            Dynamic_SP_Params.Val = Username;
            Dynamic_SP_Params_List.Add(Dynamic_SP_Params);

            result = ExecuteStoreProcedureDT("P_Get_List_By_ID_2", ref Dynamic_SP_Params_List);

            MemoryCaches.SetCacheValue(_MemoryCacheValueType, result, _cache, _iscachingenable);

            return result;
        }
        public DataTable P_Get_Role_Rights_From_RoleID(int ROLE_ID, bool IsGroupRoleID, int P_ID = 0, int PR_ID = 0, string PageRightType_MTV_CODE = "", MemoryCacheValueType? _MemoryCacheValueType = null)
        {
            string paravalue = $"ROLE_ID:{ROLE_ID}|IsGroupRoleID:{IsGroupRoleID}|P_ID:{P_ID}|PR_ID:{PR_ID}|PageRightType_MTV_CODE:{PageRightType_MTV_CODE}".ToLower();
            _MemoryCacheValueType = (_MemoryCacheValueType == null ? new MemoryCacheValueType() : _MemoryCacheValueType);
            _MemoryCacheValueType._GetMemoryCacheValueType.setkeyparavalues = paravalue;
            _MemoryCacheValueType._GetMemoryCacheValueType.subtype = CacheSubType.P_Get_Role_Rights_From_RoleID;
            _MemoryCacheValueType._SetMemoryCacheValueType.subtype = CacheSubType.P_Get_Role_Rights_From_RoleID;
            DataTable result = new DataTable();
            if (MemoryCaches.GetCacheValue(_MemoryCacheValueType, ref result, _cache, _iscachingenable))
                return result;

            List<Dynamic_SP_Params> Dynamic_SP_Params_List = new List<Dynamic_SP_Params>();
            Dynamic_SP_Params Dynamic_SP_Params;

            Dynamic_SP_Params = new Dynamic_SP_Params();
            Dynamic_SP_Params.ParameterName = "ROLE_ID";
            Dynamic_SP_Params.Val = ROLE_ID;
            Dynamic_SP_Params_List.Add(Dynamic_SP_Params);

            Dynamic_SP_Params = new Dynamic_SP_Params();
            Dynamic_SP_Params.ParameterName = "IsGroupRoleID";
            Dynamic_SP_Params.Val = IsGroupRoleID;
            Dynamic_SP_Params_List.Add(Dynamic_SP_Params);

            Dynamic_SP_Params = new Dynamic_SP_Params();
            Dynamic_SP_Params.ParameterName = "P_ID";
            Dynamic_SP_Params.Val = P_ID;
            Dynamic_SP_Params_List.Add(Dynamic_SP_Params);

            Dynamic_SP_Params = new Dynamic_SP_Params();
            Dynamic_SP_Params.ParameterName = "PR_ID";
            Dynamic_SP_Params.Val = PR_ID;
            Dynamic_SP_Params_List.Add(Dynamic_SP_Params);

            Dynamic_SP_Params = new Dynamic_SP_Params();
            Dynamic_SP_Params.ParameterName = "PageRightType_MTV_CODE";
            Dynamic_SP_Params.Val = PageRightType_MTV_CODE;
            Dynamic_SP_Params_List.Add(Dynamic_SP_Params);

            result = ExecuteStoreProcedureDT("P_Get_Role_Rights_From_RoleID", ref Dynamic_SP_Params_List);

            MemoryCaches.SetCacheValue(_MemoryCacheValueType, result, _cache, _iscachingenable);

            return result;
        }
        public DataTable P_Get_Role_Rights_From_RoleID(int ROLE_ID, bool IsGroupRoleID, MemoryCacheValueType? _MemoryCacheValueType = null)
        {
            return P_Get_Role_Rights_From_RoleID(ROLE_ID, IsGroupRoleID, 0, 0, "", _MemoryCacheValueType);
        }
        public DataTable P_Get_Role_Rights_From_RoleID_And_P_ID(int ROLE_ID, bool IsGroupRoleID, int P_ID, MemoryCacheValueType? _MemoryCacheValueType = null)
        {
            return P_Get_Role_Rights_From_RoleID(ROLE_ID, IsGroupRoleID, P_ID, 0, "", _MemoryCacheValueType);
        }
        public DataTable P_Get_Role_Rights_From_RoleID_And_PR_ID(int ROLE_ID, bool IsGroupRoleID, int PR_ID, MemoryCacheValueType? _MemoryCacheValueType = null)
        {
            return P_Get_Role_Rights_From_RoleID(ROLE_ID, IsGroupRoleID, 0, PR_ID, "", _MemoryCacheValueType);
        }
        public DataTable P_Get_Role_Rights_From_RoleID_And_PageRightType_MTV_CODE(int ROLE_ID, bool IsGroupRoleID, string PageRightType_MTV_CODE, MemoryCacheValueType? _MemoryCacheValueType = null)
        {
            return P_Get_Role_Rights_From_RoleID(ROLE_ID, IsGroupRoleID, 0, 0, PageRightType_MTV_CODE, _MemoryCacheValueType);
        }
        public DataTable P_Get_Role_Rights_From_Username(string Username, int P_ID = 0, int PR_ID = 0, string PageRightType_MTV_CODE = "", MemoryCacheValueType? _MemoryCacheValueType = null)
        {
            string paravalue = $"Username:{Username}|P_ID:{P_ID}|PR_ID:{PR_ID}|PageRightType_MTV_CODE:{PageRightType_MTV_CODE}".ToLower();
            _MemoryCacheValueType = (_MemoryCacheValueType == null ? new MemoryCacheValueType() : _MemoryCacheValueType);
            _MemoryCacheValueType._GetMemoryCacheValueType.setkeyparavalues = paravalue;
            _MemoryCacheValueType._GetMemoryCacheValueType.subtype = CacheSubType.P_Get_Role_Rights_From_Username;
            _MemoryCacheValueType._SetMemoryCacheValueType.subtype = CacheSubType.P_Get_Role_Rights_From_Username;
            DataTable result = new DataTable();
            if (MemoryCaches.GetCacheValue(_MemoryCacheValueType, ref result, _cache, _iscachingenable))
                return result;

            List<Dynamic_SP_Params> Dynamic_SP_Params_List = new List<Dynamic_SP_Params>();
            Dynamic_SP_Params Dynamic_SP_Params;

            Dynamic_SP_Params = new Dynamic_SP_Params();
            Dynamic_SP_Params.ParameterName = "Username";
            Dynamic_SP_Params.Val = Username.ToUpper();
            Dynamic_SP_Params_List.Add(Dynamic_SP_Params);

            Dynamic_SP_Params = new Dynamic_SP_Params();
            Dynamic_SP_Params.ParameterName = "P_ID";
            Dynamic_SP_Params.Val = P_ID;
            Dynamic_SP_Params_List.Add(Dynamic_SP_Params);

            Dynamic_SP_Params = new Dynamic_SP_Params();
            Dynamic_SP_Params.ParameterName = "PR_ID";
            Dynamic_SP_Params.Val = PR_ID;
            Dynamic_SP_Params_List.Add(Dynamic_SP_Params);

            Dynamic_SP_Params = new Dynamic_SP_Params();
            Dynamic_SP_Params.ParameterName = "PageRightType_MTV_CODE";
            Dynamic_SP_Params.Val = PageRightType_MTV_CODE;
            Dynamic_SP_Params_List.Add(Dynamic_SP_Params);

            result = ExecuteStoreProcedureDT("P_Get_Role_Rights_From_Username", ref Dynamic_SP_Params_List);

            MemoryCaches.SetCacheValue(_MemoryCacheValueType, result, _cache, _iscachingenable);

            return result;
        }
        public DataTable P_Get_Role_Rights_From_Username(string Username, MemoryCacheValueType? _MemoryCacheValueType = null)
        {
            return P_Get_Role_Rights_From_Username(Username, 0, 0, "", _MemoryCacheValueType);
        }
        public DataTable P_Get_Role_Rights_From_Username_And_P_ID(string Username, int P_ID, MemoryCacheValueType? _MemoryCacheValueType = null)
        {
            return P_Get_Role_Rights_From_Username(Username, P_ID, 0, "", _MemoryCacheValueType);
        }
        public DataTable P_Get_Role_Rights_From_Username_And_PR_ID(string Username, int PR_ID, MemoryCacheValueType? _MemoryCacheValueType = null)
        {
            return P_Get_Role_Rights_From_Username(Username, 0, PR_ID, "", _MemoryCacheValueType);
        }
        public DataTable P_Get_Role_Rights_From_Username_And_PageRightType_MTV_CODE(string Username, string PageRightType_MTV_CODE, MemoryCacheValueType? _MemoryCacheValueType = null)
        {
            return P_Get_Role_Rights_From_Username(Username, 0, 0, PageRightType_MTV_CODE, _MemoryCacheValueType);
        }
        public bool P_Is_Has_Right_From_RoleID_And_PR_ID_From_Memory(int ROLE_ID, bool IsGroupRoleID, int PR_ID)
        {
            bool result = false;
            DataTable DT = new DataTable();
            DT = P_Get_Role_Rights_From_RoleID(ROLE_ID, IsGroupRoleID, null);
            if (DT.Rows.Count > 0)
            {
                List<DataRow> DR = DT.Select($"PR_ID = {PR_ID}").AsEnumerable().ToList();
                if (DR.Count > 0)
                {
                    result = Convert.ToBoolean(DR[0]["IsRightActive"]);
                }
            }
            return result;
        }
        public bool P_Is_Has_Right_From_RoleID_And_PageRightType_MTV_CODE_From_Memory(int ROLE_ID, bool IsGroupRoleID, string PageRightType_MTV_CODE)
        {
            bool result = false;
            DataTable DT = new DataTable();
            DT = P_Get_Role_Rights_From_RoleID(ROLE_ID, IsGroupRoleID, null);
            if (DT.Rows.Count > 0)
            {
                List<DataRow> DR = DT.Select($"PageRightType_MTV_CODE = '{PageRightType_MTV_CODE}'").AsEnumerable().ToList();
                if (DR.Count > 0)
                {
                    result = Convert.ToBoolean(DR[0]["IsRightActive"]);
                }
            }
            return result;
        }
        public bool P_Is_Has_Right_From_Username_And_PR_ID_From_Memory(string Username, int PR_ID)
        {
            bool result = false;
            DataTable DT = new DataTable();
            DT = P_Get_Role_Rights_From_Username(Username, null);
            if (DT.Rows.Count > 0)
            {
                List<DataRow> DR = DT.Select($"PR_ID = {PR_ID}").AsEnumerable().ToList();
                if (DR.Count > 0)
                {
                    result = Convert.ToBoolean(DR[0]["IsRightActive"]);
                }
            }
            return result;
        }
        public bool P_Is_Has_Right_From_Username_And_PageRightType_MTV_CODE_From_Memory(string Username, string PageRightType_MTV_CODE)
        {
            bool result = false;
            DataTable DT = new DataTable();
            DT = P_Get_Role_Rights_From_Username(Username, null);
            if (DT.Rows.Count > 0)
            {
                List<DataRow> DR = DT.Select($"PageRightType_MTV_CODE = '{PageRightType_MTV_CODE}'").AsEnumerable().ToList();
                if (DR.Count > 0)
                {
                    result = Convert.ToBoolean(DR[0]["IsRightActive"]);
                }
            }
            return result;
        }
        public bool P_Is_Has_Right_From_RoleID(int ROLE_ID, bool IsGroupRoleID, int PR_ID = 0, string PageRightType_MTV_CODE = "", MemoryCacheValueType? _MemoryCacheValueType = null)
        {
            string paravalue = $"ROLE_ID:{ROLE_ID}|IsGroupRoleID:{IsGroupRoleID}|PR_ID:{PR_ID}|PageRightType_MTV_CODE:{PageRightType_MTV_CODE}".ToLower();
            _MemoryCacheValueType = (_MemoryCacheValueType == null ? new MemoryCacheValueType() : _MemoryCacheValueType);
            _MemoryCacheValueType._GetMemoryCacheValueType.setkeyparavalues = paravalue;
            _MemoryCacheValueType._GetMemoryCacheValueType.subtype = CacheSubType.P_Get_Role_Rights_From_RoleID;
            _MemoryCacheValueType._SetMemoryCacheValueType.subtype = CacheSubType.P_Get_Role_Rights_From_RoleID;
            bool result = false;
            if (MemoryCaches.GetCacheValue(_MemoryCacheValueType, ref result, _cache, _iscachingenable))
                return result;

            List<Dynamic_SP_Params> Dynamic_SP_Params_List = new List<Dynamic_SP_Params>();
            Dynamic_SP_Params Dynamic_SP_Params;

            Dynamic_SP_Params = new Dynamic_SP_Params();
            Dynamic_SP_Params.ParameterName = "ROLE_ID";
            Dynamic_SP_Params.Val = ROLE_ID;
            Dynamic_SP_Params_List.Add(Dynamic_SP_Params);

            Dynamic_SP_Params = new Dynamic_SP_Params();
            Dynamic_SP_Params.ParameterName = "IsGroupRoleID";
            Dynamic_SP_Params.Val = IsGroupRoleID;
            Dynamic_SP_Params_List.Add(Dynamic_SP_Params);

            Dynamic_SP_Params = new Dynamic_SP_Params();
            Dynamic_SP_Params.ParameterName = "PR_ID";
            Dynamic_SP_Params.Val = PR_ID;
            Dynamic_SP_Params_List.Add(Dynamic_SP_Params);

            Dynamic_SP_Params = new Dynamic_SP_Params();
            Dynamic_SP_Params.ParameterName = "PageRightType_MTV_CODE";
            Dynamic_SP_Params.Val = PageRightType_MTV_CODE;
            Dynamic_SP_Params_List.Add(Dynamic_SP_Params);

            result = Convert.ToBoolean(ExecuteStoreProcedureObj("P_Is_Has_Right_From_RoleID", ref Dynamic_SP_Params_List));

            MemoryCaches.SetCacheValue(_MemoryCacheValueType, result, _cache, _iscachingenable);

            return result;
        }
        public bool P_Is_Has_Right_From_RoleID_And_PR_ID(int ROLE_ID, bool IsGroupRoleID, int PR_ID = 0, MemoryCacheValueType? _MemoryCacheValueType = null)
        {
            return P_Is_Has_Right_From_RoleID(ROLE_ID, IsGroupRoleID, PR_ID, "", _MemoryCacheValueType);
        }
        public bool P_Is_Has_Right_From_RoleID_And_PageRightType_MTV_CODE(int ROLE_ID, bool IsGroupRoleID, string PageRightType_MTV_CODE = "", MemoryCacheValueType? _MemoryCacheValueType = null)
        {
            return P_Is_Has_Right_From_RoleID(ROLE_ID, IsGroupRoleID, 0, PageRightType_MTV_CODE, _MemoryCacheValueType);
        }
        public bool P_Is_Has_Right_From_Username(string Username, int PR_ID = 0, string PageRightType_MTV_CODE = "", MemoryCacheValueType? _MemoryCacheValueType = null)
        {
            string paravalue = $"Username:{Username}|PR_ID:{PR_ID}|PageRightType_MTV_CODE:{PageRightType_MTV_CODE}".ToLower();
            _MemoryCacheValueType = (_MemoryCacheValueType == null ? new MemoryCacheValueType() : _MemoryCacheValueType);
            _MemoryCacheValueType._GetMemoryCacheValueType.setkeyparavalues = paravalue;
            _MemoryCacheValueType._GetMemoryCacheValueType.subtype = CacheSubType.P_Get_Role_Rights_From_RoleID;
            _MemoryCacheValueType._SetMemoryCacheValueType.subtype = CacheSubType.P_Get_Role_Rights_From_RoleID;
            bool result = false;
            if (MemoryCaches.GetCacheValue(_MemoryCacheValueType, ref result, _cache, _iscachingenable))
                return result;

            List<Dynamic_SP_Params> Dynamic_SP_Params_List = new List<Dynamic_SP_Params>();
            Dynamic_SP_Params Dynamic_SP_Params;

            Dynamic_SP_Params = new Dynamic_SP_Params();
            Dynamic_SP_Params.ParameterName = "Username";
            Dynamic_SP_Params.Val = Username;
            Dynamic_SP_Params_List.Add(Dynamic_SP_Params);

            Dynamic_SP_Params = new Dynamic_SP_Params();
            Dynamic_SP_Params.ParameterName = "PR_ID";
            Dynamic_SP_Params.Val = PR_ID;
            Dynamic_SP_Params_List.Add(Dynamic_SP_Params);

            Dynamic_SP_Params = new Dynamic_SP_Params();
            Dynamic_SP_Params.ParameterName = "PageRightType_MTV_CODE";
            Dynamic_SP_Params.Val = PageRightType_MTV_CODE;
            Dynamic_SP_Params_List.Add(Dynamic_SP_Params);

            result = Convert.ToBoolean(ExecuteStoreProcedureObj("P_Is_Has_Right_From_Username", ref Dynamic_SP_Params_List));

            MemoryCaches.SetCacheValue(_MemoryCacheValueType, result, _cache, _iscachingenable);

            return result;
        }
        public bool P_Is_Has_Right_From_Username_And_PR_ID(string Username, int PR_ID = 0, MemoryCacheValueType? _MemoryCacheValueType = null)
        {
            return P_Is_Has_Right_From_Username(Username, PR_ID, "", _MemoryCacheValueType);
        }
        public bool P_Is_Has_Right_From_Username_And_PageRightType_MTV_CODE(string Username, string PageRightType_MTV_CODE = "", MemoryCacheValueType? _MemoryCacheValueType = null)
        {
            return P_Is_Has_Right_From_Username(Username, 0, PageRightType_MTV_CODE, _MemoryCacheValueType);
        }
        #endregion

        #region DB  
        public string GetDB(string Database_Name = "")
        {
            string connectionString = GetDBConnectionString(Database_Name);
            return connectionString;
        }
        public string GetDBConnectionString(string Database_Name = "")
        {
            string connectionString = "";
            if (Database_Name == AppEnum.Database_Name.MultiVerseDB)
            {
                connectionString = _dbStringCollection1.MultiVerse_ConnectionModel!.ConnectionString; 
            }
            else
            {
                connectionString = _dbStringCollection1.Default_ConnectionModel!.ConnectionString; 
            }
            return connectionString;
        }
        public object? ExecuteSQL(string Query, bool IsSP, int CommandTimeOut, ref List<Dynamic_SP_Params> List_Dynamic_SP_Params, bool Read_Only, string Database_Name, bool IsReturnRecord, string Config_Key)
        {
            if (CommandTimeOut <= 0)
                CommandTimeOut = _executiontimeout;
            object? result = null;
            DataSet dataSet = new DataSet();
            string ConnectionString;
            ConnectionString = GetDB(Database_Name);
            bool IsOutPutParaExists = false;
            using (var connection = new SqlConnection(ConnectionString))
            {
                connection.Open();
                try
                {
                    if (IsReturnRecord)
                    {
                        result = dataSet;
                    }

                    if (IsReturnRecord)
                    {
                        var adapter = new SqlDataAdapter(Query, connection);
                        if (IsSP)
                            adapter.SelectCommand.CommandType = CommandType.StoredProcedure;
                        else
                            adapter.SelectCommand.CommandType = CommandType.Text;
                        adapter.SelectCommand.CommandTimeout = CommandTimeOut;
                        if (List_Dynamic_SP_Params != null)
                        {
                            for (int i = 0; i <= List_Dynamic_SP_Params.Count - 1; i++)
                            {
                                List_Dynamic_SP_Params[i].Val = (List_Dynamic_SP_Params[i].Val == null ? DBNull.Value : List_Dynamic_SP_Params[i].Val);
                                SqlParameter sqlParam = new SqlParameter("@" + List_Dynamic_SP_Params[i].ParameterName, List_Dynamic_SP_Params[i].GetValueType);
                                sqlParam.Value = List_Dynamic_SP_Params[i].Val;
                                if (List_Dynamic_SP_Params[i].Size > 0)
                                    sqlParam.Size = List_Dynamic_SP_Params[i].Size;
                                sqlParam.Direction = List_Dynamic_SP_Params[i].IsInputType ? ParameterDirection.Input : ParameterDirection.Output;
                                IsOutPutParaExists = ((IsOutPutParaExists == false && List_Dynamic_SP_Params[i].IsInputType == false) ? true : IsOutPutParaExists);
                                adapter.SelectCommand.Parameters.Add(sqlParam);
                            }
                        }

                        dataSet = new DataSet();
                        adapter.Fill(dataSet);
                        result = dataSet;
                        if (IsOutPutParaExists)
                        {
                            for (int i = 0; i <= List_Dynamic_SP_Params.Count - 1; i++)
                            {
                                if (List_Dynamic_SP_Params[i].IsInputType == false)
                                    List_Dynamic_SP_Params[i].Val = adapter.SelectCommand.Parameters["@" + List_Dynamic_SP_Params[i].ParameterName].Value;
                            }
                        }
                    }
                    else
                    {
                        var command = new SqlCommand(Query, connection);
                        if (IsSP)
                            command.CommandType = CommandType.StoredProcedure;
                        else
                            command.CommandType = CommandType.Text;
                        command.CommandTimeout = CommandTimeOut;
                        if (List_Dynamic_SP_Params != null)
                        {
                            for (int i = 0; i <= List_Dynamic_SP_Params.Count - 1; i++)
                            {
                                List_Dynamic_SP_Params[i].Val = (List_Dynamic_SP_Params[i].Val == null ? DBNull.Value : List_Dynamic_SP_Params[i].Val);
                                SqlParameter sqlParam = new SqlParameter("@" + List_Dynamic_SP_Params[i].ParameterName, List_Dynamic_SP_Params[i].GetValueType);
                                sqlParam.Value = List_Dynamic_SP_Params[i].Val;
                                if (List_Dynamic_SP_Params[i].Size > 0)
                                    sqlParam.Size = List_Dynamic_SP_Params[i].Size;
                                sqlParam.Direction = List_Dynamic_SP_Params[i].IsInputType ? ParameterDirection.Input : ParameterDirection.Output;
                                IsOutPutParaExists = ((IsOutPutParaExists == false && List_Dynamic_SP_Params[i].IsInputType == false) ? true : IsOutPutParaExists);
                                command.Parameters.Add(sqlParam);
                            }
                        }
                        result = command.ExecuteNonQuery();
                        if (IsOutPutParaExists)
                        {
                            for (int i = 0; i <= List_Dynamic_SP_Params.Count - 1; i++)
                            {
                                if (List_Dynamic_SP_Params[i].IsInputType == false)
                                    List_Dynamic_SP_Params[i].Val = command.Parameters["@" + List_Dynamic_SP_Params[i].ParameterName].Value;
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    StaticPublicObjects.logFile.ErrorLog(FunctionName: "ExecuteSQL", SmallMessage: ex.Message, Message: ex.ToString());
                    throw new Exception("Internal Server Error");
                }
                connection.Close();
            }
            return result;
        }
        public int ExecuteNONQuery(string Query, ref List<Dynamic_SP_Params> dynamic_SP_Params, string Database_Name = AppEnum.Database_Name.MultiVerseDB, int CommandTimeOut = 0, string Config_Key = "")
        {
            object? return_obj = null;
            int return_int = 0;
            return_obj = ExecuteSQL(Query, false, CommandTimeOut, ref dynamic_SP_Params, false, Database_Name, false, Config_Key);
            if (return_obj != null)
            {
                if (Information.IsNumeric(return_obj))
                    return return_int = Convert.ToInt32(return_obj);
            }
            return return_int;
        }
        public int ExecuteNONQuery(string Query, string Database_Name = AppEnum.Database_Name.MultiVerseDB, int CommandTimeOut = 0, string Config_Key = "")
        {
            List<Dynamic_SP_Params> dynamic_SP_Params = null;
            return ExecuteNONQuery(Query, ref dynamic_SP_Params, Database_Name, CommandTimeOut, Config_Key);
        }
        public int ExecuteNONQuery(string Query, string Database_Name = AppEnum.Database_Name.MultiVerseDB, int CommandTimeOut = 0, string Config_Key = "", params (string Name, object Value)[] ParamsNameValues)
        {
            List<Dynamic_SP_Params> List_dynamic_SP_Params = new List<Dynamic_SP_Params>();
            Dynamic_SP_Params dynamic_SP_Params = new Dynamic_SP_Params();

            if ((ParamsNameValues != null))
            {
                if ((ParamsNameValues.Length > 0))
                {
                    for (var i = 0; i <= ParamsNameValues.Length - 1; i++)
                    {
                        dynamic_SP_Params = new Dynamic_SP_Params();
                        dynamic_SP_Params.ParameterName = ParamsNameValues[i].Name;
                        dynamic_SP_Params.Val = ParamsNameValues[i].Value;
                        List_dynamic_SP_Params.Add(dynamic_SP_Params);
                    }
                }
            }

            return (int)ExecuteSQL(Query, false, CommandTimeOut, ref List_dynamic_SP_Params, false, Database_Name, false, Config_Key);
        }
        public int ExecuteNONQuery(string Query, params (string Name, object Value)[] ParamsNameValues)
        {
            string Database_Name = AppEnum.Database_Name.MultiVerseDB;
            int CommandTimeOut = 0;
            string Config_Key = "";
            List<Dynamic_SP_Params> dynamic_SP_Params = new List<Dynamic_SP_Params>();
            Dynamic_SP_Params dynamic_SP_Params_item = new Dynamic_SP_Params();

            if ((ParamsNameValues != null))
            {
                if ((ParamsNameValues.Length > 0))
                {
                    for (var i = 0; i <= ParamsNameValues.Length - 1; i++)
                    {
                        dynamic_SP_Params_item = new Dynamic_SP_Params();
                        dynamic_SP_Params_item.ParameterName = ParamsNameValues[i].Name;
                        dynamic_SP_Params_item.Val = ParamsNameValues[i].Value;
                        dynamic_SP_Params.Add(dynamic_SP_Params_item);
                    }
                }
            }

            object? return_obj = null;
            int return_int = 0;
            return_obj = ExecuteSQL(Query, false, CommandTimeOut, ref dynamic_SP_Params, false, Database_Name, false, Config_Key);
            if (return_obj != null)
            {
                if (Information.IsNumeric(return_obj))
                    return return_int = Convert.ToInt32(return_obj);
            }
            return return_int;
        }
        public DataSet ExecuteSelectDS(string Query, ref List<Dynamic_SP_Params> dynamic_SP_Params, bool Read_Only = false, string Database_Name = AppEnum.Database_Name.MultiVerseDB, int CommandTimeOut = 0, string Config_Key = "")
        {
            DataSet dataSet = new DataSet();
            object? return_obj = null;
            return_obj = ExecuteSQL(Query, false, CommandTimeOut, ref dynamic_SP_Params, Read_Only, Database_Name, true, Config_Key);
            if (return_obj != null)
            {
                return dataSet = (DataSet)return_obj;
            }
            return dataSet;
        }
        public DataSet ExecuteSelectDS(string Query, bool Read_Only = false, string Database_Name = AppEnum.Database_Name.MultiVerseDB, int CommandTimeOut = 0, string Config_Key = "")
        {
            List<Dynamic_SP_Params> dynamic_SP_Params = null;
            return ExecuteSelectDS(Query, ref dynamic_SP_Params, Read_Only, Database_Name, CommandTimeOut, Config_Key);
        }
        public DataSet ExecuteSelectDS(string Query, bool Read_Only = false, string Database_Name = AppEnum.Database_Name.MultiVerseDB, int CommandTimeOut = 0, string Config_Key = "", params (string Name, object Value)[] ParamsNameValues)
        {
            List<Dynamic_SP_Params> List_dynamic_SP_Params = new List<Dynamic_SP_Params>();
            Dynamic_SP_Params dynamic_SP_Params = new Dynamic_SP_Params();

            if ((ParamsNameValues != null))
            {
                if ((ParamsNameValues.Length > 0))
                {
                    for (var i = 0; i <= ParamsNameValues.Length - 1; i++)
                    {
                        dynamic_SP_Params = new Dynamic_SP_Params();
                        dynamic_SP_Params.ParameterName = ParamsNameValues[i].Name;
                        dynamic_SP_Params.Val = ParamsNameValues[i].Value;
                        List_dynamic_SP_Params.Add(dynamic_SP_Params);
                    }
                }
            }

            DataSet dataSet = new DataSet();
            dataSet = (DataSet)ExecuteSQL(Query, false, CommandTimeOut, ref List_dynamic_SP_Params, Read_Only, Database_Name, true, Config_Key);
            return dataSet;
        }
        public DataSet ExecuteSelectDS(string Query, params (string Name, object Value)[] ParamsNameValues)
        {
            bool Read_Only = false;
            string Database_Name = AppEnum.Database_Name.MultiVerseDB;
            int CommandTimeOut = 0;
            string Config_Key = "";
            List<Dynamic_SP_Params> List_dynamic_SP_Params = new List<Dynamic_SP_Params>();
            Dynamic_SP_Params dynamic_SP_Params = new Dynamic_SP_Params();

            if ((ParamsNameValues != null))
            {
                if ((ParamsNameValues.Length > 0))
                {
                    for (var i = 0; i <= ParamsNameValues.Length - 1; i++)
                    {
                        dynamic_SP_Params = new Dynamic_SP_Params();
                        dynamic_SP_Params.ParameterName = ParamsNameValues[i].Name;
                        dynamic_SP_Params.Val = ParamsNameValues[i].Value;
                        List_dynamic_SP_Params.Add(dynamic_SP_Params);
                    }
                }
            }

            DataSet dataSet = new DataSet();
            dataSet = (DataSet)ExecuteSQL(Query, false, CommandTimeOut, ref List_dynamic_SP_Params, Read_Only, Database_Name, true, Config_Key);
            return dataSet;
        }
        public DataTable ExecuteSelectDT(string Query, ref List<Dynamic_SP_Params> dynamic_SP_Params, bool Read_Only = false, string Database_Name = AppEnum.Database_Name.MultiVerseDB, int CommandTimeOut = 0, string Config_Key = "")
        {
            DataSet dataSet = new DataSet();
            DataTable dataTable = new DataTable();
            object? return_obj = null;
            return_obj = ExecuteSQL(Query, false, CommandTimeOut, ref dynamic_SP_Params, Read_Only, Database_Name, true, Config_Key);
            if (return_obj != null)
            {
                dataSet = (DataSet)return_obj;
                if (dataSet.Tables.Count > 0)
                {
                    dataTable = dataSet.Tables[0];
                }
            }
            return dataTable;
        }
        public DataTable ExecuteSelectDT(string Query, bool Read_Only = false, string Database_Name = AppEnum.Database_Name.MultiVerseDB, int CommandTimeOut = 0, string Config_Key = "")
        {
            List<Dynamic_SP_Params> dynamic_SP_Params = null;
            return ExecuteSelectDT(Query, ref dynamic_SP_Params, Read_Only, Database_Name, CommandTimeOut, Config_Key);
        }
        public DataTable ExecuteSelectDT(string Query, bool Read_Only = false, string Database_Name = AppEnum.Database_Name.MultiVerseDB, int CommandTimeOut = 0, string Config_Key = "", params (string Name, object Value)[] ParamsNameValues)
        {
            List<Dynamic_SP_Params> List_dynamic_SP_Params = new List<Dynamic_SP_Params>();
            Dynamic_SP_Params dynamic_SP_Params = new Dynamic_SP_Params();

            if ((ParamsNameValues != null))
            {
                if ((ParamsNameValues.Length > 0))
                {
                    for (var i = 0; i <= ParamsNameValues.Length - 1; i++)
                    {
                        dynamic_SP_Params = new Dynamic_SP_Params();
                        dynamic_SP_Params.ParameterName = ParamsNameValues[i].Name;
                        dynamic_SP_Params.Val = ParamsNameValues[i].Value;
                        List_dynamic_SP_Params.Add(dynamic_SP_Params);
                    }
                }
            }

            DataSet dataSet = new DataSet();
            DataTable dataTable = new DataTable();
            dataSet = (DataSet)ExecuteSQL(Query, false, CommandTimeOut, ref List_dynamic_SP_Params, Read_Only, Database_Name, true, Config_Key);
            if (dataSet.Tables.Count > 0)
            {
                dataTable = dataSet.Tables[0];
            }
            return dataTable;
        }
        public DataTable ExecuteSelectDT(string Query, params (string Name, object Value)[] ParamsNameValues)
        {
            bool Read_Only = false;
            string Database_Name = AppEnum.Database_Name.MultiVerseDB;
            int CommandTimeOut = 0;
            string Config_Key = "";
            List<Dynamic_SP_Params> List_dynamic_SP_Params = new List<Dynamic_SP_Params>();
            Dynamic_SP_Params dynamic_SP_Params = new Dynamic_SP_Params();

            if ((ParamsNameValues != null))
            {
                if ((ParamsNameValues.Length > 0))
                {
                    for (var i = 0; i <= ParamsNameValues.Length - 1; i++)
                    {
                        dynamic_SP_Params = new Dynamic_SP_Params();
                        dynamic_SP_Params.ParameterName = ParamsNameValues[i].Name;
                        dynamic_SP_Params.Val = ParamsNameValues[i].Value;
                        List_dynamic_SP_Params.Add(dynamic_SP_Params);
                    }
                }
            }

            DataSet dataSet = new DataSet();
            DataTable dataTable = new DataTable();
            dataSet = (DataSet)ExecuteSQL(Query, false, CommandTimeOut, ref List_dynamic_SP_Params, Read_Only, Database_Name, true, Config_Key);
            if (dataSet.Tables.Count > 0)
            {
                dataTable = dataSet.Tables[0];
            }
            return dataTable;
        }
        public DataRow? ExecuteSelectDR(string Query, ref List<Dynamic_SP_Params> dynamic_SP_Params, bool Read_Only = false, string Database_Name = AppEnum.Database_Name.MultiVerseDB, int CommandTimeOut = 0, string Config_Key = "")
        {
            DataSet dataSet = new DataSet();
            DataRow? dataRow = null;
            object? return_obj = null;
            return_obj = ExecuteSQL(Query, false, CommandTimeOut, ref dynamic_SP_Params, Read_Only, Database_Name, true, Config_Key);
            if (return_obj != null)
            {
                dataSet = (DataSet)return_obj;
                if (dataSet.Tables.Count > 0)
                {
                    if (dataSet.Tables[0].Rows.Count > 0)
                        dataRow = dataSet.Tables[0].Rows[0];
                }
            }
            return dataRow;
        }
        public DataRow? ExecuteSelectDR(string Query, bool Read_Only = false, string Database_Name = AppEnum.Database_Name.MultiVerseDB, int CommandTimeOut = 0, string Config_Key = "")
        {
            List<Dynamic_SP_Params> dynamic_SP_Params = null;
            return ExecuteSelectDR(Query, ref dynamic_SP_Params, Read_Only, Database_Name, CommandTimeOut, Config_Key);
        }
        public DataRow? ExecuteSelectDR(string Query, bool Read_Only = false, string Database_Name = AppEnum.Database_Name.MultiVerseDB, int CommandTimeOut = 0, string Config_Key = "", params (string Name, object Value)[] ParamsNameValues)
        {
            List<Dynamic_SP_Params> List_dynamic_SP_Params = new List<Dynamic_SP_Params>();
            Dynamic_SP_Params dynamic_SP_Params = new Dynamic_SP_Params();

            if ((ParamsNameValues != null))
            {
                if ((ParamsNameValues.Length > 0))
                {
                    for (var i = 0; i <= ParamsNameValues.Length - 1; i++)
                    {
                        dynamic_SP_Params = new Dynamic_SP_Params();
                        dynamic_SP_Params.ParameterName = ParamsNameValues[i].Name;
                        dynamic_SP_Params.Val = ParamsNameValues[i].Value;
                        List_dynamic_SP_Params.Add(dynamic_SP_Params);
                    }
                }
            }

            DataSet dataSet = new DataSet();
            DataRow? dataRow = null;
            dataSet = (DataSet)ExecuteSQL(Query, false, CommandTimeOut, ref List_dynamic_SP_Params, Read_Only, Database_Name, true, Config_Key);
            if (dataSet.Tables.Count > 0)
            {
                if (dataSet.Tables[0].Rows.Count > 0)
                    dataRow = dataSet.Tables[0].Rows[0];
            }
            return dataRow;
        }
        public DataRow? ExecuteSelectDR(string Query, params (string Name, object Value)[] ParamsNameValues)
        {
            bool Read_Only = false;
            string Database_Name = AppEnum.Database_Name.MultiVerseDB;
            int CommandTimeOut = 0;
            string Config_Key = "";
            List<Dynamic_SP_Params> List_dynamic_SP_Params = new List<Dynamic_SP_Params>();
            Dynamic_SP_Params dynamic_SP_Params = new Dynamic_SP_Params();

            if ((ParamsNameValues != null))
            {
                if ((ParamsNameValues.Length > 0))
                {
                    for (var i = 0; i <= ParamsNameValues.Length - 1; i++)
                    {
                        dynamic_SP_Params = new Dynamic_SP_Params();
                        dynamic_SP_Params.ParameterName = ParamsNameValues[i].Name;
                        dynamic_SP_Params.Val = ParamsNameValues[i].Value;
                        List_dynamic_SP_Params.Add(dynamic_SP_Params);
                    }
                }
            }

            DataSet dataSet = new DataSet();
            DataRow? dataRow = null;
            dataSet = (DataSet)ExecuteSQL(Query, false, CommandTimeOut, ref List_dynamic_SP_Params, Read_Only, Database_Name, true, Config_Key);
            if (dataSet.Tables.Count > 0)
            {
                if (dataSet.Tables[0].Rows.Count > 0)
                    dataRow = dataSet.Tables[0].Rows[0];
            }
            return dataRow;
        }
        public object? ExecuteSelectObj(string Query, ref List<Dynamic_SP_Params> dynamic_SP_Params, bool Read_Only = false, string Database_Name = AppEnum.Database_Name.MultiVerseDB, int CommandTimeOut = 0, string Config_Key = "")
        {
            object? result = null;
            DataRow? dataRow;
            dataRow = ExecuteSelectDR(Query, ref dynamic_SP_Params, Read_Only, Database_Name, CommandTimeOut, Config_Key);
            if (dataRow != null)
            {
                if (Information.IsDBNull(dataRow[0]) == false)
                    result = dataRow[0];
            }
            return result;
        }
        public object? ExecuteSelectObj(string Query, bool Read_Only = false, string Database_Name = AppEnum.Database_Name.MultiVerseDB, int CommandTimeOut = 0, string Config_Key = "")
        {
            List<Dynamic_SP_Params> dynamic_SP_Params = null;
            return ExecuteSelectDR(Query, ref dynamic_SP_Params, Read_Only, Database_Name, CommandTimeOut, Config_Key);
        }
        public object? ExecuteSelectObj(string Query, bool Read_Only = false, string Database_Name = AppEnum.Database_Name.MultiVerseDB, int CommandTimeOut = 0, string Config_Key = "", params (string Name, object Value)[] ParamsNameValues)
        {
            List<Dynamic_SP_Params> List_dynamic_SP_Params = new List<Dynamic_SP_Params>();
            Dynamic_SP_Params dynamic_SP_Params = new Dynamic_SP_Params();

            if ((ParamsNameValues != null))
            {
                if ((ParamsNameValues.Length > 0))
                {
                    for (var i = 0; i <= ParamsNameValues.Length - 1; i++)
                    {
                        dynamic_SP_Params = new Dynamic_SP_Params();
                        dynamic_SP_Params.ParameterName = ParamsNameValues[i].Name;
                        dynamic_SP_Params.Val = ParamsNameValues[i].Value;
                        List_dynamic_SP_Params.Add(dynamic_SP_Params);
                    }
                }
            }

            object? result = null;
            DataRow? dataRow = null;
            dataRow = ExecuteSelectDR(Query, ref List_dynamic_SP_Params, Read_Only, Database_Name, CommandTimeOut, Config_Key);
            if (dataRow != null)
            {
                if (Information.IsDBNull(dataRow[0]) == false)
                    result = dataRow[0];
            }
            return result;
        }
        public object? ExecuteSelectObj(string Query, params (string Name, object Value)[] ParamsNameValues)
        {
            bool Read_Only = false;
            string Database_Name = AppEnum.Database_Name.MultiVerseDB;
            int CommandTimeOut = 0;
            string Config_Key = "";
            List<Dynamic_SP_Params> List_dynamic_SP_Params = new List<Dynamic_SP_Params>();
            Dynamic_SP_Params dynamic_SP_Params = new Dynamic_SP_Params();

            if ((ParamsNameValues != null))
            {
                if ((ParamsNameValues.Length > 0))
                {
                    for (var i = 0; i <= ParamsNameValues.Length - 1; i++)
                    {
                        dynamic_SP_Params = new Dynamic_SP_Params();
                        dynamic_SP_Params.ParameterName = ParamsNameValues[i].Name;
                        dynamic_SP_Params.Val = ParamsNameValues[i].Value;
                        List_dynamic_SP_Params.Add(dynamic_SP_Params);
                    }
                }
            }

            object? result = null;
            DataRow? dataRow = null;
            dataRow = ExecuteSelectDR(Query, ref List_dynamic_SP_Params, Read_Only, Database_Name, CommandTimeOut, Config_Key);
            if (dataRow != null)
            {
                if (Information.IsDBNull(dataRow[0]) == false)
                    result = dataRow[0];
            }
            return result;
        }
        public object? ExecuteSelectObjMainDB(string Query, ref List<Dynamic_SP_Params> dynamic_SP_Params, string Database_Name = AppEnum.Database_Name.MultiVerseDB, int CommandTimeOut = 0, string Config_Key = "")
        {
            object? result = null;
            result = ExecuteSelectObj(Query, ref dynamic_SP_Params, false, Database_Name, CommandTimeOut, Config_Key);
            return result;
        }
        public object? ExecuteSelectObjMainDB(string Query, string Database_Name = AppEnum.Database_Name.MultiVerseDB, int CommandTimeOut = 0, string Config_Key = "")
        {
            List<Dynamic_SP_Params> dynamic_SP_Params = null;
            return ExecuteSelectObj(Query, ref dynamic_SP_Params, false, Database_Name, CommandTimeOut, Config_Key);
        }
        public object? ExecuteSelectObjMainDB(string Query, string Database_Name = AppEnum.Database_Name.MultiVerseDB, int CommandTimeOut = 0, string Config_Key = "", params (string Name, object Value)[] ParamsNameValues)
        {
            List<Dynamic_SP_Params> List_dynamic_SP_Params = new List<Dynamic_SP_Params>();
            Dynamic_SP_Params dynamic_SP_Params = new Dynamic_SP_Params();

            if ((ParamsNameValues != null))
            {
                if ((ParamsNameValues.Length > 0))
                {
                    for (var i = 0; i <= ParamsNameValues.Length - 1; i++)
                    {
                        dynamic_SP_Params = new Dynamic_SP_Params();
                        dynamic_SP_Params.ParameterName = ParamsNameValues[i].Name;
                        dynamic_SP_Params.Val = ParamsNameValues[i].Value;
                        List_dynamic_SP_Params.Add(dynamic_SP_Params);
                    }
                }
            }

            object? result = null;
            result = ExecuteSelectObj(Query, ref List_dynamic_SP_Params, false, Database_Name, CommandTimeOut, Config_Key);
            return result;
        }
        public object? ExecuteSelectObjMainDB(string Query, params (string Name, object Value)[] ParamsNameValues)
        {
            string Database_Name = AppEnum.Database_Name.MultiVerseDB;
            int CommandTimeOut = 0;
            string Config_Key = "";
            List<Dynamic_SP_Params> List_dynamic_SP_Params = new List<Dynamic_SP_Params>();
            Dynamic_SP_Params dynamic_SP_Params = new Dynamic_SP_Params();

            if ((ParamsNameValues != null))
            {
                if ((ParamsNameValues.Length > 0))
                {
                    for (var i = 0; i <= ParamsNameValues.Length - 1; i++)
                    {
                        dynamic_SP_Params = new Dynamic_SP_Params();
                        dynamic_SP_Params.ParameterName = ParamsNameValues[i].Name;
                        dynamic_SP_Params.Val = ParamsNameValues[i].Value;
                        List_dynamic_SP_Params.Add(dynamic_SP_Params);
                    }
                }
            }

            object? result = null;
            result = ExecuteSelectObj(Query, ref List_dynamic_SP_Params, false, Database_Name, CommandTimeOut, Config_Key);
            return result;
        }
        public int ExecuteStoreProcedureNONQuery(string SPName, ref List<Dynamic_SP_Params> dynamic_SP_Params, string Database_Name = AppEnum.Database_Name.MultiVerseDB, int CommandTimeOut = 0, string Config_Key = "")
        {
            object? return_obj = null;
            int return_int = 0;
            return_obj = ExecuteSQL(SPName, true, CommandTimeOut, ref dynamic_SP_Params, false, Database_Name, false, Config_Key);
            if (return_obj != null)
            {
                if (Information.IsNumeric(return_obj))
                    return return_int = Convert.ToInt32(return_obj);
            }
            return return_int;
        }
        public int ExecuteStoreProcedureNONQuery(string SPName, string Database_Name = AppEnum.Database_Name.MultiVerseDB, int CommandTimeOut = 0, string Config_Key = "")
        {
            List<Dynamic_SP_Params> dynamic_SP_Params = null;
            return ExecuteStoreProcedureNONQuery(SPName, ref dynamic_SP_Params, Database_Name, CommandTimeOut, Config_Key);
        }
        public int ExecuteStoreProcedureNONQuery(string Query, string Database_Name = AppEnum.Database_Name.MultiVerseDB, int CommandTimeOut = 0, string Config_Key = "", params (string Name, object Value)[] ParamsNameValues)
        {
            List<Dynamic_SP_Params> List_dynamic_SP_Params = new List<Dynamic_SP_Params>();
            Dynamic_SP_Params dynamic_SP_Params = new Dynamic_SP_Params();

            if ((ParamsNameValues != null))
            {
                if ((ParamsNameValues.Length > 0))
                {
                    for (var i = 0; i <= ParamsNameValues.Length - 1; i++)
                    {
                        dynamic_SP_Params = new Dynamic_SP_Params();
                        dynamic_SP_Params.ParameterName = ParamsNameValues[i].Name;
                        dynamic_SP_Params.Val = ParamsNameValues[i].Value;
                        List_dynamic_SP_Params.Add(dynamic_SP_Params);
                    }
                }
            }
            return (int)ExecuteSQL(Query, true, CommandTimeOut, ref List_dynamic_SP_Params, false, Database_Name, false, Config_Key);
        }
        public int ExecuteStoreProcedureNONQuery(string Query, params (string Name, object Value)[] ParamsNameValues)
        {
            string Database_Name = AppEnum.Database_Name.MultiVerseDB;
            int CommandTimeOut = 0;
            string Config_Key = "";
            List<Dynamic_SP_Params> List_dynamic_SP_Params = new List<Dynamic_SP_Params>();
            Dynamic_SP_Params dynamic_SP_Params = new Dynamic_SP_Params();

            if ((ParamsNameValues != null))
            {
                if ((ParamsNameValues.Length > 0))
                {
                    for (var i = 0; i <= ParamsNameValues.Length - 1; i++)
                    {
                        dynamic_SP_Params = new Dynamic_SP_Params();
                        dynamic_SP_Params.ParameterName = ParamsNameValues[i].Name;
                        dynamic_SP_Params.Val = ParamsNameValues[i].Value;
                        List_dynamic_SP_Params.Add(dynamic_SP_Params);
                    }
                }
            }
            return (int)ExecuteSQL(Query, true, CommandTimeOut, ref List_dynamic_SP_Params, false, Database_Name, false, Config_Key);
        }
        public DataSet ExecuteStoreProcedureDS(string SPName, ref List<Dynamic_SP_Params> dynamic_SP_Params, bool Read_Only = false, string Database_Name = AppEnum.Database_Name.MultiVerseDB, int CommandTimeOut = 0, string Config_Key = "")
        {
            DataSet dataSet = new DataSet();
            object? return_obj = null;
            return_obj = ExecuteSQL(SPName, true, CommandTimeOut, ref dynamic_SP_Params, Read_Only, Database_Name, true, Config_Key);
            if (return_obj != null)
            {
                return dataSet = (DataSet)return_obj;
            }
            return dataSet;
        }
        public DataSet ExecuteStoreProcedureDS(string SPName, bool Read_Only = false, string Database_Name = AppEnum.Database_Name.MultiVerseDB, int CommandTimeOut = 0, string Config_Key = "")
        {
            List<Dynamic_SP_Params> dynamic_SP_Params = null;
            return ExecuteStoreProcedureDS(SPName, ref dynamic_SP_Params, Read_Only, Database_Name, CommandTimeOut, Config_Key);
        }
        public DataSet ExecuteStoreProcedureDS(string Query, bool Read_Only = false, string Database_Name = AppEnum.Database_Name.MultiVerseDB, int CommandTimeOut = 0, string Config_Key = "", params (string Name, object Value)[] ParamsNameValues)
        {
            List<Dynamic_SP_Params> List_dynamic_SP_Params = new List<Dynamic_SP_Params>();
            Dynamic_SP_Params dynamic_SP_Params = new Dynamic_SP_Params();

            if ((ParamsNameValues != null))
            {
                if ((ParamsNameValues.Length > 0))
                {
                    for (var i = 0; i <= ParamsNameValues.Length - 1; i++)
                    {
                        dynamic_SP_Params = new Dynamic_SP_Params();
                        dynamic_SP_Params.ParameterName = ParamsNameValues[i].Name;
                        dynamic_SP_Params.Val = ParamsNameValues[i].Value;
                        List_dynamic_SP_Params.Add(dynamic_SP_Params);
                    }
                }
            }

            DataSet dataSet = new DataSet();
            dataSet = (DataSet)ExecuteSQL(Query, true, CommandTimeOut, ref List_dynamic_SP_Params, Read_Only, Database_Name, true, Config_Key);
            return dataSet;
        }
        public DataSet ExecuteStoreProcedureDS(string Query, params (string Name, object Value)[] ParamsNameValues)
        {
            bool Read_Only = false;
            string Database_Name = AppEnum.Database_Name.MultiVerseDB;
            int CommandTimeOut = 0;
            string Config_Key = "";
            List<Dynamic_SP_Params> List_dynamic_SP_Params = new List<Dynamic_SP_Params>();
            Dynamic_SP_Params dynamic_SP_Params = new Dynamic_SP_Params();

            if ((ParamsNameValues != null))
            {
                if ((ParamsNameValues.Length > 0))
                {
                    for (var i = 0; i <= ParamsNameValues.Length - 1; i++)
                    {
                        dynamic_SP_Params = new Dynamic_SP_Params();
                        dynamic_SP_Params.ParameterName = ParamsNameValues[i].Name;
                        dynamic_SP_Params.Val = ParamsNameValues[i].Value;
                        List_dynamic_SP_Params.Add(dynamic_SP_Params);
                    }
                }
            }

            DataSet dataSet = new DataSet();
            dataSet = (DataSet)ExecuteSQL(Query, true, CommandTimeOut, ref List_dynamic_SP_Params, Read_Only, Database_Name, true, Config_Key);
            return dataSet;
        }
        public DataTable ExecuteStoreProcedureDT(string SPName, ref List<Dynamic_SP_Params> dynamic_SP_Params, bool Read_Only = false, string Database_Name = AppEnum.Database_Name.MultiVerseDB, int CommandTimeOut = 0, string Config_Key = "")
        {
            DataSet dataSet = new DataSet();
            DataTable dataTable = new DataTable();
            object? return_obj = null;
            return_obj = ExecuteSQL(SPName, true, CommandTimeOut, ref dynamic_SP_Params, Read_Only, Database_Name, true, Config_Key);
            if (return_obj != null)
            {
                dataSet = (DataSet)return_obj;
                if (dataSet.Tables.Count > 0)
                {
                    dataTable = dataSet.Tables[0];
                }
            }
            return dataTable;
        }
        public DataTable ExecuteStoreProcedureDT(string SPName, bool Read_Only = false, string Database_Name = AppEnum.Database_Name.MultiVerseDB, int CommandTimeOut = 0, string Config_Key = "")
        {
            List<Dynamic_SP_Params> dynamic_SP_Params = null;
            return ExecuteStoreProcedureDT(SPName, ref dynamic_SP_Params, Read_Only, Database_Name, CommandTimeOut, Config_Key);
        }
        public DataTable ExecuteStoreProcedureDT(string Query, bool Read_Only = false, string Database_Name = AppEnum.Database_Name.MultiVerseDB, int CommandTimeOut = 0, string Config_Key = "", params (string Name, object Value)[] ParamsNameValues)
        {
            List<Dynamic_SP_Params> List_dynamic_SP_Params = new List<Dynamic_SP_Params>();
            Dynamic_SP_Params dynamic_SP_Params = new Dynamic_SP_Params();

            if ((ParamsNameValues != null))
            {
                if ((ParamsNameValues.Length > 0))
                {
                    for (var i = 0; i <= ParamsNameValues.Length - 1; i++)
                    {
                        dynamic_SP_Params = new Dynamic_SP_Params();
                        dynamic_SP_Params.ParameterName = ParamsNameValues[i].Name;
                        dynamic_SP_Params.Val = ParamsNameValues[i].Value;
                        List_dynamic_SP_Params.Add(dynamic_SP_Params);
                    }
                }
            }

            DataSet dataSet = new DataSet();
            DataTable dataTable = new DataTable();
            dataSet = (DataSet)ExecuteSQL(Query, true, CommandTimeOut, ref List_dynamic_SP_Params, Read_Only, Database_Name, true, Config_Key);
            if (dataSet.Tables.Count > 0)
            {
                dataTable = dataSet.Tables[0];
            }
            return dataTable;
        }
        public DataTable ExecuteStoreProcedureDT(string Query, params (string Name, object Value)[] ParamsNameValues)
        {
            bool Read_Only = false;
            string Database_Name = AppEnum.Database_Name.MultiVerseDB;
            int CommandTimeOut = 0;
            string Config_Key = "";
            List<Dynamic_SP_Params> List_dynamic_SP_Params = new List<Dynamic_SP_Params>();
            Dynamic_SP_Params dynamic_SP_Params = new Dynamic_SP_Params();

            if ((ParamsNameValues != null))
            {
                if ((ParamsNameValues.Length > 0))
                {
                    for (var i = 0; i <= ParamsNameValues.Length - 1; i++)
                    {
                        dynamic_SP_Params = new Dynamic_SP_Params();
                        dynamic_SP_Params.ParameterName = ParamsNameValues[i].Name;
                        dynamic_SP_Params.Val = ParamsNameValues[i].Value;
                        List_dynamic_SP_Params.Add(dynamic_SP_Params);
                    }
                }
            }

            DataSet dataSet = new DataSet();
            DataTable dataTable = new DataTable();
            dataSet = (DataSet)ExecuteSQL(Query, true, CommandTimeOut, ref List_dynamic_SP_Params, Read_Only, Database_Name, true, Config_Key);
            if (dataSet.Tables.Count > 0)
            {
                dataTable = dataSet.Tables[0];
            }
            return dataTable;
        }
        public DataRow? ExecuteStoreProcedureDR(string SPName, ref List<Dynamic_SP_Params> dynamic_SP_Params, bool Read_Only = false, string Database_Name = AppEnum.Database_Name.MultiVerseDB, int CommandTimeOut = 0, string Config_Key = "", bool isSP = true)
        {
            DataSet dataSet = new DataSet();
            DataRow? dataRow = null;
            object? return_obj = null;
            return_obj = ExecuteSQL(SPName, isSP, CommandTimeOut, ref dynamic_SP_Params, Read_Only, Database_Name, true, Config_Key);
            if (return_obj != null)
            {
                dataSet = (DataSet)return_obj;
                if (dataSet.Tables.Count > 0)
                {
                    if (dataSet.Tables[0].Rows.Count > 0)
                        dataRow = dataSet.Tables[0].Rows[0];
                }
            }
            return dataRow;
        }
        public DataRow? ExecuteStoreProcedureDR(string SPName, bool Read_Only = false, string Database_Name = AppEnum.Database_Name.MultiVerseDB, int CommandTimeOut = 0, string Config_Key = "")
        {
            List<Dynamic_SP_Params> dynamic_SP_Params = null;
            return ExecuteStoreProcedureDR(SPName, ref dynamic_SP_Params, Read_Only, Database_Name, CommandTimeOut, Config_Key);
        }
        public DataRow? ExecuteStoreProcedureDR(string Query, bool Read_Only = false, string Database_Name = AppEnum.Database_Name.MultiVerseDB, int CommandTimeOut = 0, string Config_Key = "", params (string Name, object Value)[] ParamsNameValues)
        {
            List<Dynamic_SP_Params> List_dynamic_SP_Params = new List<Dynamic_SP_Params>();
            Dynamic_SP_Params dynamic_SP_Params = new Dynamic_SP_Params();

            if ((ParamsNameValues != null))
            {
                if ((ParamsNameValues.Length > 0))
                {
                    for (var i = 0; i <= ParamsNameValues.Length - 1; i++)
                    {
                        dynamic_SP_Params = new Dynamic_SP_Params();
                        dynamic_SP_Params.ParameterName = ParamsNameValues[i].Name;
                        dynamic_SP_Params.Val = ParamsNameValues[i].Value;
                        List_dynamic_SP_Params.Add(dynamic_SP_Params);
                    }
                }
            }

            DataSet dataSet = new DataSet();
            DataRow? dataRow = null;
            dataSet = (DataSet)ExecuteSQL(Query, true, CommandTimeOut, ref List_dynamic_SP_Params, Read_Only, Database_Name, true, Config_Key);
            if (dataSet.Tables.Count > 0)
            {
                if (dataSet.Tables[0].Rows.Count > 0)
                    dataRow = dataSet.Tables[0].Rows[0];
            }
            return dataRow;
        }
        public DataRow? ExecuteStoreProcedureDR(string Query, params (string Name, object Value)[] ParamsNameValues)
        {
            bool Read_Only = false;
            string Database_Name = AppEnum.Database_Name.MultiVerseDB;
            int CommandTimeOut = 0;
            string Config_Key = "";
            List<Dynamic_SP_Params> List_dynamic_SP_Params = new List<Dynamic_SP_Params>();
            Dynamic_SP_Params dynamic_SP_Params = new Dynamic_SP_Params();

            if ((ParamsNameValues != null))
            {
                if ((ParamsNameValues.Length > 0))
                {
                    for (var i = 0; i <= ParamsNameValues.Length - 1; i++)
                    {
                        dynamic_SP_Params = new Dynamic_SP_Params();
                        dynamic_SP_Params.ParameterName = ParamsNameValues[i].Name;
                        dynamic_SP_Params.Val = ParamsNameValues[i].Value;
                        List_dynamic_SP_Params.Add(dynamic_SP_Params);
                    }
                }
            }

            DataSet dataSet = new DataSet();
            DataRow? dataRow = null;
            dataSet = (DataSet)ExecuteSQL(Query, true, CommandTimeOut, ref List_dynamic_SP_Params, Read_Only, Database_Name, true, Config_Key);
            if (dataSet.Tables.Count > 0)
            {
                if (dataSet.Tables[0].Rows.Count > 0)
                    dataRow = dataSet.Tables[0].Rows[0];
            }
            return dataRow;
        }
        public object? ExecuteStoreProcedureObj(string SPName, ref List<Dynamic_SP_Params> dynamic_SP_Params, bool Read_Only = false, string Database_Name = AppEnum.Database_Name.MultiVerseDB, int CommandTimeOut = 0, string Config_Key = "")
        {
            object? result = null;
            DataRow? dataRow;
            dataRow = ExecuteStoreProcedureDR(SPName, ref dynamic_SP_Params, Read_Only, Database_Name, CommandTimeOut, Config_Key);
            if (dataRow != null)
            {
                if (Information.IsDBNull(dataRow[0]) == false)
                    result = dataRow[0];
            }
            return result;
        }
        public object? ExecuteStoreProcedureObj(string SPName, bool Read_Only = false, string Database_Name = AppEnum.Database_Name.MultiVerseDB, int CommandTimeOut = 0, string Config_Key = "")
        {
            List<Dynamic_SP_Params> dynamic_SP_Params = null;
            return ExecuteStoreProcedureObj(SPName, ref dynamic_SP_Params, Read_Only, Database_Name, CommandTimeOut, Config_Key);
        }
        public object? ExecuteStoreProcedureObj(string Query, bool Read_Only = false, string Database_Name = AppEnum.Database_Name.MultiVerseDB, int CommandTimeOut = 0, string Config_Key = "", params (string Name, object Value)[] ParamsNameValues)
        {
            List<Dynamic_SP_Params> List_dynamic_SP_Params = new List<Dynamic_SP_Params>();
            Dynamic_SP_Params dynamic_SP_Params = new Dynamic_SP_Params();

            if ((ParamsNameValues != null))
            {
                if ((ParamsNameValues.Length > 0))
                {
                    for (var i = 0; i <= ParamsNameValues.Length - 1; i++)
                    {
                        dynamic_SP_Params = new Dynamic_SP_Params();
                        dynamic_SP_Params.ParameterName = ParamsNameValues[i].Name;
                        dynamic_SP_Params.Val = ParamsNameValues[i].Value;
                        List_dynamic_SP_Params.Add(dynamic_SP_Params);
                    }
                }
            }

            object result = null;
            DataRow? dataRow;
            dataRow = ExecuteStoreProcedureDR(Query, ref List_dynamic_SP_Params, Read_Only, Database_Name, CommandTimeOut, Config_Key);
            if (dataRow != null)
            {
                if (Information.IsDBNull(dataRow[0]) == false)
                    result = dataRow[0];
            }
            return result;
        }
        public object? ExecuteStoreProcedureObj(string Query, params (string Name, object Value)[] ParamsNameValues)
        {
            bool Read_Only = false;
            string Database_Name = AppEnum.Database_Name.MultiVerseDB;
            int CommandTimeOut = 0;
            string Config_Key = "";
            List<Dynamic_SP_Params> List_dynamic_SP_Params = new List<Dynamic_SP_Params>();
            Dynamic_SP_Params dynamic_SP_Params = new Dynamic_SP_Params();

            if ((ParamsNameValues != null))
            {
                if ((ParamsNameValues.Length > 0))
                {
                    for (var i = 0; i <= ParamsNameValues.Length - 1; i++)
                    {
                        dynamic_SP_Params = new Dynamic_SP_Params();
                        dynamic_SP_Params.ParameterName = ParamsNameValues[i].Name;
                        dynamic_SP_Params.Val = ParamsNameValues[i].Value;
                        List_dynamic_SP_Params.Add(dynamic_SP_Params);
                    }
                }
            }

            object result = null;
            DataRow? dataRow;
            dataRow = ExecuteStoreProcedureDR(Query, ref List_dynamic_SP_Params, Read_Only, Database_Name, CommandTimeOut, Config_Key);
            if (dataRow != null)
            {
                if (Information.IsDBNull(dataRow[0]) == false)
                    result = dataRow[0];
            }
            return result;
        }
        public object? ExecuteStoreProcedureObjMainDB(string SPName, ref List<Dynamic_SP_Params> dynamic_SP_Params, string Database_Name = AppEnum.Database_Name.MultiVerseDB, int CommandTimeOut = 0, string Config_Key = "")
        {
            object? result = null;
            result = ExecuteStoreProcedureObj(SPName, ref dynamic_SP_Params, false, Database_Name, CommandTimeOut, Config_Key);
            return result;
        }
        public object? ExecuteStoreProcedureObjMainDB(string SPName, string Database_Name = AppEnum.Database_Name.MultiVerseDB, int CommandTimeOut = 0, string Config_Key = "")
        {
            List<Dynamic_SP_Params> dynamic_SP_Params = null;
            return ExecuteStoreProcedureObjMainDB(SPName, ref dynamic_SP_Params, Database_Name, CommandTimeOut, Config_Key);
        }
        public object? ExecuteStoreProcedureObjMainDB(string Query, string Database_Name = AppEnum.Database_Name.MultiVerseDB, int CommandTimeOut = 0, string Config_Key = "", params (string Name, object Value)[] ParamsNameValues)
        {
            List<Dynamic_SP_Params> List_dynamic_SP_Params = new List<Dynamic_SP_Params>();
            Dynamic_SP_Params dynamic_SP_Params = new Dynamic_SP_Params();

            if ((ParamsNameValues != null))
            {
                if ((ParamsNameValues.Length > 0))
                {
                    for (var i = 0; i <= ParamsNameValues.Length - 1; i++)
                    {
                        dynamic_SP_Params = new Dynamic_SP_Params();
                        dynamic_SP_Params.ParameterName = ParamsNameValues[i].Name;
                        dynamic_SP_Params.Val = ParamsNameValues[i].Value;
                        List_dynamic_SP_Params.Add(dynamic_SP_Params);
                    }
                }
            }

            object? result = null;
            result = ExecuteStoreProcedureDR(Query, ref List_dynamic_SP_Params, false, Database_Name, CommandTimeOut, Config_Key);
            return result;
        }
        public object? ExecuteStoreProcedureObjMainDB(string Query, params (string Name, object Value)[] ParamsNameValues)
        {
            string Database_Name = AppEnum.Database_Name.MultiVerseDB;
            int CommandTimeOut = 0;
            string Config_Key = "";
            List<Dynamic_SP_Params> List_dynamic_SP_Params = new List<Dynamic_SP_Params>();
            Dynamic_SP_Params dynamic_SP_Params = new Dynamic_SP_Params();

            if ((ParamsNameValues != null))
            {
                if ((ParamsNameValues.Length > 0))
                {
                    for (var i = 0; i <= ParamsNameValues.Length - 1; i++)
                    {
                        dynamic_SP_Params = new Dynamic_SP_Params();
                        dynamic_SP_Params.ParameterName = ParamsNameValues[i].Name;
                        dynamic_SP_Params.Val = ParamsNameValues[i].Value;
                        List_dynamic_SP_Params.Add(dynamic_SP_Params);
                    }
                }
            }

            object? result = null;
            result = ExecuteStoreProcedureDR(Query, ref List_dynamic_SP_Params, false, Database_Name, CommandTimeOut, Config_Key);
            return result;
        }
        public T ExecuteSelectSQLMap<T>(string Query, bool IsSP, int CommandTimeOut, ref List<Dynamic_SP_Params> List_Dynamic_SP_Params, bool Read_Only = false, string Database_Name = AppEnum.Database_Name.MultiVerseDB, string Config_Key = "") where T : new()
        {
            bool IsList = false;
            if (CommandTimeOut <= 0)
                CommandTimeOut = 2000;
            T result = new T();
            string ConnectionString;
            ConnectionString = GetDB(Database_Name);
            bool IsOutPutParaExists = false;
            using (var connection = new SqlConnection(ConnectionString))
            {
                connection.Open();
                try
                {
                    var spParam = new DynamicParameters();
                    if (List_Dynamic_SP_Params != null)
                    {
                        for (int i = 0; i <= List_Dynamic_SP_Params.Count - 1; i++)
                        {
                            //List_Dynamic_SP_Params[i].Val = (List_Dynamic_SP_Params[i].Val == null ? DBNull.Value : List_Dynamic_SP_Params[i].Val);
                            if (List_Dynamic_SP_Params[i].Size > 0)
                            {
                                spParam.Add(name: "@" + List_Dynamic_SP_Params[i].ParameterName
                                    , value: List_Dynamic_SP_Params[i].Val
                                    , size: List_Dynamic_SP_Params[i].Size
                                    , direction: List_Dynamic_SP_Params[i].IsInputType ? ParameterDirection.Input : ParameterDirection.Output);
                            }
                            else
                            {
                                spParam.Add(name: "@" + List_Dynamic_SP_Params[i].ParameterName
                                    , value: List_Dynamic_SP_Params[i].Val
                                    , direction: List_Dynamic_SP_Params[i].IsInputType ? ParameterDirection.Input : ParameterDirection.Output);
                            }
                            if (List_Dynamic_SP_Params[i].IsInputType == false && IsOutPutParaExists == false)
                                IsOutPutParaExists = true;
                        }
                        result = connection.Query<T>(Query, param: spParam, commandType: (IsSP ? CommandType.StoredProcedure : CommandType.Text), commandTimeout: CommandTimeOut).FirstOrDefault();
                    }
                    else
                    {
                        result = connection.Query<T>(Query, commandType: (IsSP ? CommandType.StoredProcedure : CommandType.Text), commandTimeout: CommandTimeOut).FirstOrDefault();
                    }
                    if (IsOutPutParaExists)
                    {
                        for (int i = 0; i <= List_Dynamic_SP_Params.Count - 1; i++)
                        {
                            if (List_Dynamic_SP_Params[i].IsInputType == false)
                                List_Dynamic_SP_Params[i].Val = spParam.Get<object>("@" + List_Dynamic_SP_Params[i].ParameterName);
                        }
                    }
                }
                catch (Exception ex)
                {
                    StaticPublicObjects.logFile.ErrorLog(FunctionName: "ExecuteSelectSQLMap", SmallMessage: ex.Message, Message: ex.ToString());
                    throw new Exception("Internal Server Error");
                }

                connection.Close();
            }
            return result;
        }
        public T ExecuteSelectSQLMap<T>(string Query, bool IsSP, int CommandTimeOut, bool Read_Only = false, string Database_Name = AppEnum.Database_Name.MultiVerseDB, string Config_Key = "", params (string Name, object Value)[] ParamsNameValues) where T : new()
        {
            List<Dynamic_SP_Params> List_Dynamic_SP_Params = null;

            if ((ParamsNameValues != null))
            {
                if ((ParamsNameValues.Length > 0))
                {
                    List_Dynamic_SP_Params = new List<Dynamic_SP_Params>();
                    Dynamic_SP_Params dynamic_SP_Params = new Dynamic_SP_Params();
                    for (var i = 0; i <= ParamsNameValues.Length - 1; i++)
                    {
                        dynamic_SP_Params = new Dynamic_SP_Params();
                        dynamic_SP_Params.ParameterName = ParamsNameValues[i].Name;
                        dynamic_SP_Params.Val = ParamsNameValues[i].Value;
                        List_Dynamic_SP_Params.Add(dynamic_SP_Params);
                    }
                }
            }

            return ExecuteSelectSQLMap<T>(Query, IsSP, CommandTimeOut, ref List_Dynamic_SP_Params, Read_Only, Database_Name, Config_Key);
        }
        public T ExecuteSelectSQLMap<T>(string Query, bool IsSP, int CommandTimeOut, bool Read_Only = false, string Database_Name = AppEnum.Database_Name.MultiVerseDB, string Config_Key = "") where T : new()
        {
            List<Dynamic_SP_Params> List_Dynamic_SP_Params = null;
            return ExecuteSelectSQLMap<T>(Query, IsSP, CommandTimeOut, ref List_Dynamic_SP_Params, Read_Only, Database_Name, Config_Key);
        }
        public List<T> ExecuteSelectSQLMapList<T>(string Query, bool IsSP, int CommandTimeOut, ref List<Dynamic_SP_Params> List_Dynamic_SP_Params, bool Read_Only = false, string Database_Name = AppEnum.Database_Name.MultiVerseDB, string Config_Key = "") where T : new()
        {
            bool IsList = true;
            if (CommandTimeOut <= 0)
                CommandTimeOut = 2000;
            List<T> result = new List<T>();
            string ConnectionString;
            ConnectionString = GetDB(Database_Name);
            bool IsOutPutParaExists = false;
            using (var connection = new SqlConnection(ConnectionString))
            {
                connection.Open();
                try
                {
                    var spParam = new DynamicParameters();
                    if (List_Dynamic_SP_Params != null)
                    {
                        for (int i = 0; i <= List_Dynamic_SP_Params.Count - 1; i++)
                        {
                            //List_Dynamic_SP_Params[i].Val = (List_Dynamic_SP_Params[i].Val == null ? DBNull.Value : List_Dynamic_SP_Params[i].Val);
                            if (List_Dynamic_SP_Params[i].Size > 0)
                            {
                                spParam.Add(name: "@" + List_Dynamic_SP_Params[i].ParameterName
                                    , value: List_Dynamic_SP_Params[i].Val
                                    , size: List_Dynamic_SP_Params[i].Size
                                    , direction: List_Dynamic_SP_Params[i].IsInputType ? ParameterDirection.Input : ParameterDirection.Output);
                            }
                            else
                            {
                                spParam.Add(name: "@" + List_Dynamic_SP_Params[i].ParameterName
                                    , value: List_Dynamic_SP_Params[i].Val
                                    , direction: List_Dynamic_SP_Params[i].IsInputType ? ParameterDirection.Input : ParameterDirection.Output);
                            }
                            if (List_Dynamic_SP_Params[i].IsInputType == false && IsOutPutParaExists == false)
                                IsOutPutParaExists = true;
                        }
                        result = connection.Query<T>(Query, param: spParam, commandType: (IsSP ? CommandType.StoredProcedure : CommandType.Text), commandTimeout: CommandTimeOut).ToList<T>();
                    }
                    else
                    {
                        result = connection.Query<T>(Query, commandType: (IsSP ? CommandType.StoredProcedure : CommandType.Text), commandTimeout: CommandTimeOut).ToList<T>();
                    }
                    if (IsOutPutParaExists)
                    {
                        for (int i = 0; i <= List_Dynamic_SP_Params.Count - 1; i++)
                        {
                            if (List_Dynamic_SP_Params[i].IsInputType == false)
                                List_Dynamic_SP_Params[i].Val = spParam.Get<object>("@" + List_Dynamic_SP_Params[i].ParameterName);
                        }
                    }
                }
                catch (Exception ex)
                {
                    StaticPublicObjects.logFile.ErrorLog(FunctionName: "ExecuteSelectSQLMapList", SmallMessage: ex.Message, Message: ex.ToString());
                    throw new Exception("Internal Server Error");
                }

                connection.Close();
            }
            return result;
        }
        public List<T> ExecuteSelectSQLMapList<T>(string Query, bool IsSP, int CommandTimeOut, bool Read_Only = false, string Database_Name = AppEnum.Database_Name.MultiVerseDB, string Config_Key = "", params (string Name, object Value)[] ParamsNameValues) where T : new()
        {
            List<Dynamic_SP_Params> List_Dynamic_SP_Params = null;

            if ((ParamsNameValues != null))
            {
                if ((ParamsNameValues.Length > 0))
                {
                    List_Dynamic_SP_Params = new List<Dynamic_SP_Params>();
                    Dynamic_SP_Params dynamic_SP_Params = new Dynamic_SP_Params();
                    for (var i = 0; i <= ParamsNameValues.Length - 1; i++)
                    {
                        dynamic_SP_Params = new Dynamic_SP_Params();
                        dynamic_SP_Params.ParameterName = ParamsNameValues[i].Name;
                        dynamic_SP_Params.Val = ParamsNameValues[i].Value;
                        List_Dynamic_SP_Params.Add(dynamic_SP_Params);
                    }
                }
            }

            return ExecuteSelectSQLMapList<T>(Query, IsSP, CommandTimeOut, ref List_Dynamic_SP_Params, Read_Only, Database_Name, Config_Key);
        }
        public List<T> ExecuteSelectSQLMapList<T>(string Query, bool IsSP, int CommandTimeOut, bool Read_Only = false, string Database_Name = AppEnum.Database_Name.MultiVerseDB, string Config_Key = "") where T : new()
        {
            List<Dynamic_SP_Params> List_Dynamic_SP_Params = null;
            return ExecuteSelectSQLMapList<T>(Query, IsSP, CommandTimeOut, ref List_Dynamic_SP_Params, Read_Only, Database_Name, Config_Key);
        }
        public void ExecuteSelectSQLMapMultiple(string Query, bool IsSP, bool IsList, int CommandTimeOut, ref List<Dynamic_SP_Params> List_Dynamic_SP_Params, ref List<object> listofobject, bool Read_Only = false, string Database_Name = AppEnum.Database_Name.MultiVerseDB, string Config_Key = "")
        {
            if (CommandTimeOut <= 0)
                CommandTimeOut = 2000;
            SqlMapper.GridReader result;
            string ConnectionString;
            ConnectionString = GetDB(Database_Name);
            bool IsOutPutParaExists = false;
            using (var connection = new SqlConnection(ConnectionString))
            {
                connection.Open();
                try
                {
                    var spParam = new DynamicParameters();
                    if (List_Dynamic_SP_Params != null)
                    {
                        for (int i = 0; i <= List_Dynamic_SP_Params.Count - 1; i++)
                        {
                            //List_Dynamic_SP_Params[i].Val = (List_Dynamic_SP_Params[i].Val == null ? DBNull.Value : List_Dynamic_SP_Params[i].Val);
                            if (List_Dynamic_SP_Params[i].Size > 0)
                            {
                                spParam.Add(name: "@" + List_Dynamic_SP_Params[i].ParameterName
                                    , value: List_Dynamic_SP_Params[i].Val
                                    , size: List_Dynamic_SP_Params[i].Size
                                    , direction: List_Dynamic_SP_Params[i].IsInputType ? ParameterDirection.Input : ParameterDirection.Output);
                            }
                            else
                            {
                                spParam.Add(name: "@" + List_Dynamic_SP_Params[i].ParameterName
                                    , value: List_Dynamic_SP_Params[i].Val
                                    , direction: List_Dynamic_SP_Params[i].IsInputType ? ParameterDirection.Input : ParameterDirection.Output);
                            }
                            if (List_Dynamic_SP_Params[i].IsInputType == false && IsOutPutParaExists == false)
                                IsOutPutParaExists = true;
                        }
                        result = connection.QueryMultiple(Query, param: spParam, commandType: (IsSP ? CommandType.StoredProcedure : CommandType.Text), commandTimeout: CommandTimeOut);
                    }
                    else
                        result = connection.QueryMultiple(Query, commandType: (IsSP ? CommandType.StoredProcedure : CommandType.Text), commandTimeout: CommandTimeOut);

                    for (int i = 0; i <= listofobject.Count - 1; i++)
                    {
                        listofobject[i] = result.Read<dynamic>().ToList();
                    }

                    if (IsOutPutParaExists)
                    {
                        for (int i = 0; i <= List_Dynamic_SP_Params.Count - 1; i++)
                        {
                            if (List_Dynamic_SP_Params[i].IsInputType == false)
                                List_Dynamic_SP_Params[i].Val = spParam.Get<object>("@" + List_Dynamic_SP_Params[i].ParameterName);
                        }
                    }
                }
                catch (Exception ex)
                {
                    StaticPublicObjects.logFile.ErrorLog(FunctionName: "ExecuteSelectSQLMapMultiple", SmallMessage: ex.Message, Message: ex.ToString());
                    throw new Exception("Internal Server Error");
                }

                connection.Close();
            }
        }
        public void ExecuteSelectSQLMapMultiple(string Query, bool IsSP, bool IsList, int CommandTimeOut, ref List<object> listofobject, bool Read_Only = false, string Database_Name = AppEnum.Database_Name.MultiVerseDB, string Config_Key = "", params (string Name, object Value)[] ParamsNameValues)
        {
            List<Dynamic_SP_Params> List_Dynamic_SP_Params = null;

            if ((ParamsNameValues != null))
            {
                if ((ParamsNameValues.Length > 0))
                {
                    List_Dynamic_SP_Params = new List<Dynamic_SP_Params>();
                    Dynamic_SP_Params dynamic_SP_Params = new Dynamic_SP_Params();
                    for (var i = 0; i <= ParamsNameValues.Length - 1; i++)
                    {
                        dynamic_SP_Params = new Dynamic_SP_Params();
                        dynamic_SP_Params.ParameterName = ParamsNameValues[i].Name;
                        dynamic_SP_Params.Val = ParamsNameValues[i].Value;
                        List_Dynamic_SP_Params.Add(dynamic_SP_Params);
                    }
                }
            }

            ExecuteSelectSQLMapMultiple(Query, IsSP, IsList, CommandTimeOut, ref List_Dynamic_SP_Params, ref listofobject, Read_Only, Database_Name, Config_Key);
        }
        public void ExecuteSelectSQLMapMultiple(string Query, bool IsSP, bool IsList, int CommandTimeOut, ref List<object> listofobject, bool Read_Only = false, string Database_Name = AppEnum.Database_Name.MultiVerseDB, string Config_Key = "")
        {
            List<Dynamic_SP_Params> List_Dynamic_SP_Params = null;
            ExecuteSelectSQLMapMultiple(Query, IsSP, IsList, CommandTimeOut, ref List_Dynamic_SP_Params, ref listofobject, Read_Only, Database_Name, Config_Key);
        }

        public DataRow P_Common_DR_Procedure(string Query, ref List<Dynamic_SP_Params> List_Dynamic_SP_Params)
        {
            DataRow result = null;
            result = ExecuteStoreProcedureDR(Query, ref List_Dynamic_SP_Params)!;
            return result;
        }
        public P_ReturnMessage_Result P_SP_MultiParm_Result<T>(string Query, T res, string USERNAME, string IP = "")
        {
            P_ReturnMessage_Result result = new P_ReturnMessage_Result();
            List<Dynamic_SP_Params> dynamic_SP_Params_list = new List<Dynamic_SP_Params>();

            PropertyInfo[] properties = typeof(T).GetProperties();
            foreach (var property in properties)
            {
                if (property.GetCustomAttribute<ExcludeFromDynamicSPParamsAttribute>() == null) // Check if the property does not have the custom attribute
                {
                    Dynamic_SP_Params dynamic_SP_Params = new Dynamic_SP_Params();
                    dynamic_SP_Params.ParameterName = property.Name;
                    dynamic_SP_Params.Val = property.GetValue(res);
                    dynamic_SP_Params_list.Add(dynamic_SP_Params);
                }
            }
            dynamic_SP_Params_list.Add(new Dynamic_SP_Params { ParameterName = "Username", Val = USERNAME });
            dynamic_SP_Params_list.Add(new Dynamic_SP_Params { ParameterName = "IPAddress", Val = IP });

            DataRow DR = ExecuteStoreProcedureDR(Query, ref dynamic_SP_Params_list);
            result.ReturnCode = Convert.ToBoolean(DR["Return_Code"]);
            result.ReturnText = DR["Return_Text"].ToString();
            return result;
        }
        public P_ReturnMessageWithObj_Result P_SP_MultiParmWithObj_Result<T>(string Query, T res, string USERNAME, string ObjFieldName, string IP = "")
        {
            P_ReturnMessageWithObj_Result result = new P_ReturnMessageWithObj_Result();
            List<Dynamic_SP_Params> dynamic_SP_Params_list = new List<Dynamic_SP_Params>();

            PropertyInfo[] properties = typeof(T).GetProperties();
            foreach (var property in properties)
            {
                if (property.GetCustomAttribute<ExcludeFromDynamicSPParamsAttribute>() == null) // Check if the property does not have the custom attribute
                {
                    Dynamic_SP_Params dynamic_SP_Params = new Dynamic_SP_Params();
                    dynamic_SP_Params.ParameterName = property.Name;
                    dynamic_SP_Params.Val = property.GetValue(res);
                    dynamic_SP_Params_list.Add(dynamic_SP_Params);
                }
            }
            dynamic_SP_Params_list.Add(new Dynamic_SP_Params { ParameterName = "Username", Val = USERNAME });
            dynamic_SP_Params_list.Add(new Dynamic_SP_Params { ParameterName = "IPAddress", Val = IP });

            DataRow DR = ExecuteStoreProcedureDR(Query, ref dynamic_SP_Params_list);
            result.ReturnCode = Convert.ToBoolean(DR["Return_Code"]);
            result.ReturnText = DR["Return_Text"].ToString();
            if (Information.IsDBNull(DR[ObjFieldName]) == false)
                result.ID = DR[ObjFieldName];
            return result;
        }
        public P_ReturnMessage_Result P_SP_SingleParm_Result(string Query, string parmName, object parmValue, string USERNAME, string IP = "")
        {
            P_ReturnMessage_Result result = new P_ReturnMessage_Result();
            List<Dynamic_SP_Params> dynamic_SP_Params_list = new List<Dynamic_SP_Params>();
            dynamic_SP_Params_list.Add(new Dynamic_SP_Params { ParameterName = parmName, Val = parmValue });
            //dynamic_SP_Params_list.Add(new Dynamic_SP_Params { ParameterName = "Username", Val = USERNAME });
            //dynamic_SP_Params_list.Add(new Dynamic_SP_Params { ParameterName = "IPAddress", Val = IP });

            DataRow DR = ExecuteStoreProcedureDR(Query, ref dynamic_SP_Params_list);
            result.ReturnCode = Convert.ToBoolean(DR["Return_Code"]);
            result.ReturnText = DR["Return_Text"].ToString();
            return result;
        }
        public string P_Get_SingleParm_String_Result(string Query, string parmName, object parmValue)
        {
            List<Dynamic_SP_Params> dynamic_SP_Params_list = new List<Dynamic_SP_Params>();
            Dynamic_SP_Params dynamic_SP_Params = new Dynamic_SP_Params();
            dynamic_SP_Params = new Dynamic_SP_Params();
            dynamic_SP_Params.ParameterName = parmName;
            dynamic_SP_Params.Val = parmValue;
            dynamic_SP_Params_list.Add(dynamic_SP_Params);
            object result = null;
            result = ExecuteStoreProcedureObj(Query, ref dynamic_SP_Params_list)!;
            return (result == null ? "" : result.ToString()!);
        }
        public string P_Get_MultiParm_String_Result(string Query, List<Dynamic_SP_Params> List_Dynamic_SP_Params)
        {
            object result = null;
            result = ExecuteStoreProcedureObj(Query, ref List_Dynamic_SP_Params)!;
            return (result == null ? "" : result.ToString()!);
        }
        public List<SelectDropDownList> Get_DropDownList_Result(string Query, List<Dynamic_SP_Params> List_Dynamic_SP_Params = null, bool isSP = false)
        {
            List<SelectDropDownList> result = ExecuteSelectSQLMapList<SelectDropDownList>(Query, isSP, 0, ref List_Dynamic_SP_Params);
            return result;
        }
        public T P_AddEditRemove_SP<T>(string Query, List<Dynamic_SP_Params> List_Dynamic_SP_Params) where T : new()
        {
            T result = new T();
            DataRow DR = ExecuteStoreProcedureDR(Query, ref List_Dynamic_SP_Params)!;
            if (DR != null)
            {
                var properties = typeof(T).GetProperties();
                foreach (var property in properties)
                {
                    if (DR.Table.Columns.Contains(property.Name))
                    {
                        object value = DR[property.Name];
                        property.SetValue(result, value == DBNull.Value ? null : value, null);
                    }
                }
            }
            return result;
        }
        public T P_Get_Generic_SP<T>(string Query, ref List<Dynamic_SP_Params> List_Dynamic_SP_Params, bool IsSP = true) where T : new()
        {
            return ExecuteSelectSQLMap<T>(Query, IsSP, 0, ref List_Dynamic_SP_Params);
        }
        public P_ReturnMessage_Result P_SP_Remove_Generic_Result(string TableName, string ColumnName, object ColumnValue)
        {
            P_ReturnMessage_Result result = new P_ReturnMessage_Result();
            List<Dynamic_SP_Params> dynamic_SP_Params_list = new List<Dynamic_SP_Params>();
            dynamic_SP_Params_list.Add(new Dynamic_SP_Params { ParameterName = "TableName", Val = TableName });
            dynamic_SP_Params_list.Add(new Dynamic_SP_Params { ParameterName = "ColumnName", Val = ColumnName });
            dynamic_SP_Params_list.Add(new Dynamic_SP_Params { ParameterName = "ColumnValue", Val = ColumnValue });
            dynamic_SP_Params_list.Add(new Dynamic_SP_Params { ParameterName = "Username", Val = GetPublicClaimObjects().username });
            dynamic_SP_Params_list.Add(new Dynamic_SP_Params { ParameterName = "IPAddress", Val = "" });

            DataRow DR = ExecuteStoreProcedureDR("P_Remove_Generic", ref dynamic_SP_Params_list);
            result.ReturnCode = Convert.ToBoolean(DR["Return_Code"]);
            result.ReturnText = DR["Return_Text"].ToString();
            return result;
        }

        public List<T> P_Get_Generic_List_SP<T>(string Query, ref List<Dynamic_SP_Params> List_Dynamic_SP_Params, bool IsSP = true) where T : new()
        {
            return ExecuteSelectSQLMapList<T>(Query, IsSP, 1000, ref List_Dynamic_SP_Params);
        }
        public string P_Get_SingleValue_String_SP(string Query, string parmName, object parmValue)
        {
            List<Dynamic_SP_Params> dynamic_SP_Params_list = new List<Dynamic_SP_Params>();
            Dynamic_SP_Params dynamic_SP_Params = new Dynamic_SP_Params();
            dynamic_SP_Params = new Dynamic_SP_Params();
            dynamic_SP_Params.ParameterName = parmName;
            dynamic_SP_Params.Val = parmValue;
            dynamic_SP_Params_list.Add(dynamic_SP_Params);
            object result = null;
            result = ExecuteStoreProcedureObj(Query, ref dynamic_SP_Params_list)!;
            return (result == null ? "" : result.ToString()!);
        }
        public List<SelectDropDownList> Get_DropdownList_MT_ID(int MT_ID, string UserName)
        {
            List<SelectDropDownList> lists = new List<SelectDropDownList>();

            DataTable dt = new DataTable();
            dt = P_Get_List_By_ID_2(MT_ID, UserName);

            foreach (DataRow row in dt.Rows)
            {
                SelectDropDownList item = new SelectDropDownList
                {
                    code = row["MTV_CODE"],
                    name = row["SubName"].ToString()!
                };

                lists.Add(item);
            }

            return lists;
        }
        public List<SelectDropDownListWithEncryptionString> Get_DropdownList_MT_ID_With_Encryption(int MT_ID, string UserName, bool IsCodeRequired)
        {
            List<SelectDropDownListWithEncryptionString> lists = new List<SelectDropDownListWithEncryptionString>();

            DataTable dt = new DataTable();
            dt = P_Get_List_By_ID_2(MT_ID, UserName);

            foreach (DataRow row in dt.Rows)
            {
                SelectDropDownListWithEncryptionString item = new SelectDropDownListWithEncryptionString
                {
                    code = (IsCodeRequired ? row["MTV_ID"].ToString() : row["MTV_CODE"].ToString()),
                    name = row["SubName"].ToString()
                };

                lists.Add(item);
            }

            return lists;
        }
        public List<object> Get_SingleParm_Object_Result(string Query, List<Dynamic_SP_Params> _Params, bool IsSP = false)
        {
            List<object> result = StaticPublicObjects.ado.P_Get_Generic_List_SP<object>(Query, ref _Params, IsSP);
            return result;
        }
        public object? Get_SingleRow_Object_Result(string Query, List<Dynamic_SP_Params> _Params, bool IsSP = false)
        {
            object? result = null;
            DataRow? dataRow;
            dataRow = ExecuteStoreProcedureDR(Query, ref _Params, false, AppEnum.Database_Name.MultiVerseDB, 0, "", IsSP);
            if (dataRow != null)
            {
                if (Information.IsDBNull(dataRow[0]) == false)
                    result = dataRow[0];
            }
            return result;
        }
        public string Get_SingleRow_String_Result(string Query, List<Dynamic_SP_Params> _Params, bool IsSP = false)
        {
            object? result = null;
            result = Get_SingleRow_Object_Result(Query, _Params, IsSP);
            return (result == null ? "" : result.ToString()!);
        }
        public P_ReturnMessage_Result P_ExecuteProc_Result(string Query, List<Dynamic_SP_Params> dynamic_SP_Params_list, bool IsSP = true)
        {
            P_ReturnMessage_Result result = new P_ReturnMessage_Result();
            DataRow DR = ExecuteStoreProcedureDR(Query, ref dynamic_SP_Params_list, false, AppEnum.Database_Name.MultiVerseDB, 0, "", IsSP)!;
            result.ReturnCode = Convert.ToBoolean(DR["Return_Code"]);
            result.ReturnText = DR["Return_Text"].ToString();
            return result;
        }

        #endregion DB

        #region User
        public async Task<P_UserLoginPasswordModel> GetUserLoginCredentials(string UserName, CancellationToken cancellationToken)
        {
            List<Dynamic_SP_Params> parms = new List<Dynamic_SP_Params>()
            {
                new Dynamic_SP_Params {ParameterName = "UserName", Val = UserName.ToUpper()}
            };
            string query = "SELECT PasswordHash,PasswordSalt FROM [dbo].[T_Users] WITH (NOLOCK) WHERE IsActive = 1 AND UPPER(UserName) = @UserName";
            var result = P_Get_Generic_SP<P_UserLoginPasswordModel>(query, ref parms, false);
            return result;

        }
        public P_Get_User_Info P_Get_User_Info(string UserName, int ApplicationID, MemoryCacheValueType? _MemoryCacheValueType = null)
        {
            List<Dynamic_SP_Params> parms = new List<Dynamic_SP_Params>()
            {
                new Dynamic_SP_Params {ParameterName = "UserName", Val = UserName},
                new Dynamic_SP_Params {ParameterName = "ApplicationID", Val = ApplicationID},
            };
            P_Get_User_Info result = ExecuteSelectSQLMap<P_Get_User_Info>("P_Get_User_Info", true, 0, ref parms);
            return result;
        }
        #endregion User
    }
}

using AutoMapper;
using Data.DataAccess;
using Data.Dtos;
using Data.Interfaces;
using Microsoft.Extensions.Caching.Memory;
using Services.GlobalServices;
using System.Data;
using static Data.Dtos.AppEnum;

namespace MultiVerse_API.Extensions
{
    public class CustomRateLimit
    {
        public static bool VerifyCustomRateLimit(int MethodID, ref APILimitResponse response, bool IsIgnoreSubDomainCheck = false, bool IsIgnoreDomainCheck = false)
        {
            PublicClaimObjects _PublicClaimObjects = StaticPublicObjects.ado.GetPublicClaimObjects();
            response = new APILimitResponse();
            IADORepository ado = StaticPublicObjects.ado;
            IMapper map = StaticPublicObjects.map;
            IMemoryCache cache = StaticPublicObjects.memorycache;
            string username = _PublicClaimObjects.username;
            APILimitRequest? _APILimitRequest = new APILimitRequest();
            //APIRemoteDomainLimitRequest? _APIRemoteDomainLimitRequest = new APIRemoteDomainLimitRequest();

            if (_PublicClaimObjects.username != "")
            {
                if ((_PublicClaimObjects.P_Get_User_Info_Class!.IsAdmin)
                    || (_PublicClaimObjects.P_Get_User_Info_Class.IsAdmin && StaticPublicObjects.ado.IsAllowedDomain() == false)
                    || (StaticPublicObjects.ado.IsAllowedDomain() && StaticPublicObjects.ado.GetSubDomain() != "localhost" && IsIgnoreSubDomainCheck == false)
                    || (StaticPublicObjects.ado.IsAllowedDomainExcludingSubDomain() && StaticPublicObjects.ado.GetSubDomain() != "localhost" && IsIgnoreDomainCheck == false))
                {
                    response.response_code = true;
                    return response.response_code;
                }
            }

            {
                DataRow? APIRemoteDomain_DR = null;
                APIRemoteDomain_DR = ado.P_Get_API_RemoteDomain_IP_Request_Limit(true, MethodID);
                if (APIRemoteDomain_DR != null)
                {
                    _APILimitRequest = new APILimitRequest();
                    _APILimitRequest = (APIRemoteDomain_DR == null ? null : map.Map<APILimitRequest>(APIRemoteDomain_DR));
                    VerifyCustomRateLimitStatus(ref response, ref _APILimitRequest, cache, _PublicClaimObjects, false, true);
                    if (response.statuscode != 0)
                        return response.response_code;
                }
            }

            {
                DataRow? APIIP_DR = null;
                APIIP_DR = ado.P_Get_API_RemoteDomain_IP_Request_Limit(false, MethodID);
                if (APIIP_DR != null)
                {
                    _APILimitRequest = new APILimitRequest();
                    _APILimitRequest = (APIIP_DR == null ? null : map.Map<APILimitRequest>(APIIP_DR));
                    VerifyCustomRateLimitStatus(ref response, ref _APILimitRequest, cache, _PublicClaimObjects, true, false);
                    if (response.statuscode != 0)
                        return response.response_code;
                }
            }

            DataTable APIUserMap_DT = new DataTable();
            int UserID = 0;
            if (_PublicClaimObjects.username != "")
                APIUserMap_DT = ado.P_Get_API_User_Map(UserID, APIType.MultiVerseAPI, _PublicClaimObjects.username);
            if (APIUserMap_DT.Rows.Count > 0)
            {
                List<DataRow> APIUserMap_DR;
                APIUserMap_DR = APIUserMap_DT.Select($"MethodID={MethodID}").AsEnumerable().ToList();
                if (APIUserMap_DR.Count > 0)
                {
                    {
                        DataRow? APIUserMapRequestLimit_DR = null;
                        APIUserMapRequestLimit_DR = ado.P_Get_API_User_Map_Request_Limit(UserID, MethodID, _PublicClaimObjects.username);
                        _APILimitRequest = new APILimitRequest();
                        _APILimitRequest = (APIUserMapRequestLimit_DR == null ? null : map.Map<APILimitRequest>(APIUserMapRequestLimit_DR));
                        VerifyCustomRateLimitStatus(ref response, ref _APILimitRequest, cache, _PublicClaimObjects, false, false);
                        if (response.statuscode != 0)
                            return response.response_code;
                    }
                }
                else
                {
                    response.errormsg = $"You Don't Have Access to This Method";
                    response.errorcode = ErrorList.ErrorListNoRight.ErrorCode;
                    response.statuscode = ErrorList.ErrorListNoRight.StatusCode;
                    return response.response_code;
                }
            }
            else if (StaticPublicObjects.ado.IsAllowAnonymousMethods() == false && _PublicClaimObjects.username != "")
            {
                response.errormsg = $"You Don't Have Access to This API";
                response.errorcode = ErrorList.ErrorListNoRight.ErrorCode;
                response.statuscode = ErrorList.ErrorListNoRight.StatusCode;
                return response.response_code;
            }

            response.response_code = true;
            return response.response_code;
        }
        private static void VerifyCustomRateLimitStatus(ref APILimitResponse response, ref APILimitRequest? request, IMemoryCache cache, PublicClaimObjects _PublicClaimObjects, bool IsIP, bool IsRemoteDomain)
        {
            if (request != null)
            {
                int GetUsedRequest = 0;
                int ReqInAllowed = 0;
                string CacheKey = "";
                string ReqTypeName = request.ReqTypeName;
                string AdditionaCacheKeyInfo = "";
                if (IsIP)
                    AdditionaCacheKeyInfo = StaticPublicObjects.ado.GetLocalIPAddress();
                if (IsRemoteDomain)
                    AdditionaCacheKeyInfo = StaticPublicObjects.ado.GetRemoteDomain();
                CacheKey = MemoryCaches.GetCacheKey(_PublicClaimObjects.path, ReqTypeName, (AdditionaCacheKeyInfo == "" ? _PublicClaimObjects.username : ""), AdditionaCacheKeyInfo);
                MemoryCaches.GetSetRequestLimitCacheValue(CacheKey, ref GetUsedRequest, cache, request.ReqLimitsTime, true);
                if (GetUsedRequest > request.ReqLimits)
                {
                    response.errormsg = $"Limit is Reached. {request.ReqLimits} Request{(request.ReqLimits > 1 ? "s are" : " is")} Allowed in {ReqTypeName}";
                    response.errorcode = ErrorList.ErrorListLimitReached.ErrorCode;
                    response.statuscode = ErrorList.ErrorListLimitReached.StatusCode;
                }
                else
                {
                    if (request.ReqOtherType_IsErrorEnabled == true || request.ReqOtherType_IsWarningEnabled == true)
                    {
                        GetUsedRequest = 0;
                        ReqTypeName = "Second";
                        ReqInAllowed = request.ReqInSecond;
                        SetCacheofRateLimit(ref response, request, cache, _PublicClaimObjects, ref GetUsedRequest, ref ReqTypeName, IsIP, IsRemoteDomain, ReqInAllowed);

                        GetUsedRequest = 0;
                        ReqTypeName = "Minute";
                        ReqInAllowed = request.ReqInMinute;
                        SetCacheofRateLimit(ref response, request, cache, _PublicClaimObjects, ref GetUsedRequest, ref ReqTypeName, IsIP, IsRemoteDomain, ReqInAllowed);

                        GetUsedRequest = 0;
                        ReqTypeName = "Hour";
                        ReqInAllowed = request.ReqInHour;
                        SetCacheofRateLimit(ref response, request, cache, _PublicClaimObjects, ref GetUsedRequest, ref ReqTypeName, IsIP, IsRemoteDomain, ReqInAllowed);

                        GetUsedRequest = 0;
                        ReqTypeName = "Day";
                        ReqInAllowed = request.ReqInDay;
                        SetCacheofRateLimit(ref response, request, cache, _PublicClaimObjects, ref GetUsedRequest, ref ReqTypeName, IsIP, IsRemoteDomain, ReqInAllowed);

                        GetUsedRequest = 0;
                        ReqTypeName = "";
                        if (response.statuscode == 0)
                        {
                            response.response_code = true;
                            response.statuscode = 200;
                        }
                    }
                    else
                    {
                        response.response_code = true;
                        response.statuscode = 200;
                    }
                }

            }
            else
            {
                response.errormsg = $"Limit is Not Set For This Method. Please Contact Metro.";
                response.statuscode = 429;
            }
        }
        private static void SetCacheofRateLimit(ref APILimitResponse response, APILimitRequest? request, IMemoryCache cache, PublicClaimObjects _PublicClaimObjects, ref int GetUsedRequest, ref string ReqTypeName, bool IsIP, bool IsRemoteDomain, int ReqInAllowed)
        {
            if (request.ReqTypeName.ToLower() != ReqTypeName.ToLower() && response.statuscode == 0)
            {
                string CacheKey = "";
                string AdditionaCacheKeyInfo = "";
                if (IsIP)
                    AdditionaCacheKeyInfo = StaticPublicObjects.ado.GetLocalIPAddress();
                if (IsRemoteDomain)
                    AdditionaCacheKeyInfo = StaticPublicObjects.ado.GetRemoteDomain();
                CacheKey = MemoryCaches.GetCacheKey(_PublicClaimObjects.path, ReqTypeName, (AdditionaCacheKeyInfo == "" ? _PublicClaimObjects.username : ""), AdditionaCacheKeyInfo);
                MemoryCaches.GetSetRequestLimitCacheValue(CacheKey, ref GetUsedRequest, cache, 1, true);

                if (GetUsedRequest > ReqInAllowed && request.ReqOtherType_IsErrorEnabled)
                {
                    response.errormsg = $"Limit is Reached. {ReqInAllowed} Request{(ReqInAllowed > 1 ? "s are" : " is")} Allowed in {ReqTypeName}";
                    response.errorcode = ErrorList.ErrorListLimitReached.ErrorCode;
                    response.statuscode = ErrorList.ErrorListLimitReached.StatusCode;
                }
                else if (GetUsedRequest > ReqInAllowed && request.ReqOtherType_IsWarningEnabled)
                {
                    response.warningmsg = $"Limit is Reached. {ReqInAllowed} Request{(ReqInAllowed > 1 ? "s are" : " is")} Allowed in {ReqTypeName}";
                    //response.statuscode = 200;
                }
            }
        }
    }
    public class CustomWhiteListing
    {
        public static bool VerifyWhiteListing(int MethodID, ref APIWhiteListingResponse response)
        {
            response = new APIWhiteListingResponse();
            IADORepository ado = StaticPublicObjects.ado;
            IMapper map = StaticPublicObjects.map;
            IMemoryCache cache = StaticPublicObjects.memorycache;
            APIRemoteDomainWhiteListingRequest? _APIRemoteDomainWhiteListingRequest = new APIRemoteDomainWhiteListingRequest();

            {
                DataRow? APIRemoteDomain_DR = null;
                APIRemoteDomain_DR = ado.P_Get_API_RemoteDomain_IP_WhiteListing(true, AppEnum.ApplicationId.AppID);
                if (APIRemoteDomain_DR != null)
                {
                    _APIRemoteDomainWhiteListingRequest = new APIRemoteDomainWhiteListingRequest();
                    _APIRemoteDomainWhiteListingRequest = (APIRemoteDomain_DR == null ? null : map.Map<APIRemoteDomainWhiteListingRequest>(APIRemoteDomain_DR));
                    VerifyCustomWhiteListingStatus(ref response, ref _APIRemoteDomainWhiteListingRequest, cache);
                    if (response.statuscode != 0)
                        return response.response_code;
                }
            }

            {
                DataRow? APIIP_DR = null;
                APIIP_DR = ado.P_Get_API_RemoteDomain_IP_WhiteListing(false, MethodID);
                if (APIIP_DR != null)
                {
                    _APIRemoteDomainWhiteListingRequest = new APIRemoteDomainWhiteListingRequest();
                    _APIRemoteDomainWhiteListingRequest = (APIIP_DR == null ? null : map.Map<APIRemoteDomainWhiteListingRequest>(APIIP_DR));
                    VerifyCustomWhiteListingStatus(ref response, ref _APIRemoteDomainWhiteListingRequest, cache);
                    if (response.statuscode != 0)
                        return response.response_code;
                }
            }
            response.response_code = true;

            return response.response_code;
        }
        private static void VerifyCustomWhiteListingStatus(ref APIWhiteListingResponse response, ref APIRemoteDomainWhiteListingRequest? request, IMemoryCache cache)
        {
            if (request != null)
            {
                if (request.IsBlackList)
                {
                    string name = (request.Is_RemoteDomain == true ? "RemoteDomain" : "IP Address");
                    response.errormsg = $"This {name} is Blocked";
                    response.statuscode = 401;
                }
                else if (request.IsWhiteList == false)
                {
                    string name = (request.Is_RemoteDomain == true ? "RemoteDomain" : "IP Address");
                    response.errormsg = $"This {name} is Not WhiteListed";
                    response.statuscode = 401;
                }
            }
        }
    }
}

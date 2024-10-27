using Data.DataAccess;
using Data.Dtos;
using Data.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Newtonsoft.Json;

namespace MultiVerse_API.Extensions
{
    public class OnChallenge
    {
        public static void GetOnChallenge(JwtBearerChallengeContext c, ref string responsetext, string SwaggerHiddenVersion)
        {
            APILimitResponse _APILimitResponse = new APILimitResponse();
            string ErrorMessage = c.Error == null ? "Token Is Required" : c.Error;
            string ErrorCode = c.Error == null ? ErrorList.ErrorListTokenRequired.ErrorCode : ErrorList.ErrorListInvalidToken.ErrorCode;
            int StatusCode = c.Error == null ? ErrorList.ErrorListTokenRequired.StatusCode : ErrorList.ErrorListInvalidToken.StatusCode;
            string _Path = c.Request.Path.Value.ToLower().Replace("/v1", "").Replace("/v2", "").Replace("/" + SwaggerHiddenVersion.ToLower(), "");
            int MethodID = StaticPublicObjects.ado.GetMethodIDFromPath();
            string source_ = AppEnum.APISourceName.GetLogAllFailedRequest + (StaticPublicObjects.ado.IsSwaggerCall() ? "-SW" : "");
            string RequestString = "";
            string RefNo = "";
            if (c.AuthenticateFailure != null)
            {
                ErrorMessage = ErrorMessage.ToLower() == "invalid_token" ? "Token Expired" : ErrorMessage;
                ErrorCode = (ErrorMessage == "Token Expired" ? ErrorList.ErrorListInvalidToken.ErrorCode : ErrorCode);
            }

            GetResponseText(_Path, ErrorCode, StatusCode, ErrorMessage, MethodID, source_, ref responsetext, RequestString, RefNo);
        }
        public static void GetResponseText(string _Path, string ErrorCode, int StatusCode, string ErrorMessage, int MethodID, string source_, ref string responsetext, string RequestString, string RefNo)
        {
            JsonSerializerSettings _JsonSerializerSettings = Globals.GetCustomJsonDefaultSetting(AppEnum.JsonIgnorePropertyType.HideProperty, false, false);
            DateTime LogStartTime = DateTime.UtcNow;
            DateTime LogEndTime = DateTime.UtcNow;
            string ResponseJson = "";
            if (_Path == "/api/account/resetmemorycache")
            {
                APILimitResponse response = new APILimitResponse();
                response.errormsg = ErrorMessage;
                response.errorcode = ErrorCode;
                ResponseJson = Globals.GetResponseJson(response, _JsonSerializerSettings);
                //StaticPublicObjects.ado.P_API_Request_Log_Insert_2("", MethodID, RequestString, null, response, source_, RefNo, StatusCode, response.response_code, ErrorCode, response.errormsg, ResponseJson);
                responsetext = ResponseJson;
            }
            else if (_Path == "/api/account/signinasync")
            {
                AccountSignInResDTO response = new AccountSignInResDTO();
                response.ErrorMsg = ErrorMessage;
                response.ErrorCode = ErrorCode;
                ResponseJson = Globals.GetResponseJson(response, _JsonSerializerSettings);
                //StaticPublicObjects.ado.P_API_Request_Log_Insert_2("", MethodID, RequestString, null, response, source_, RefNo, StatusCode, response.ResponseCode, ErrorCode, response.ErrorMsg, ResponseJson);
                responsetext = ResponseJson;
            }
            else if (_Path == "/api/account/refreshtoken")
            {
                RefreshTokenResDTO response = new RefreshTokenResDTO();
                response.ErrorMsg = ErrorMessage;
                response.ErrorCode = ErrorCode;
                ResponseJson = Globals.GetResponseJson(response, _JsonSerializerSettings);
                //StaticPublicObjects.ado.P_API_Request_Log_Insert_2("", MethodID, RequestString, null, response, source_, RefNo, StatusCode, response.ResponseCode, ErrorCode, response.ErrorMsg, ResponseJson);
                responsetext = ResponseJson;
            }
            else
            {
                APILimitResponse response = new APILimitResponse();
                response.errormsg = ErrorMessage;
                response.errorcode = ErrorCode;
                ResponseJson = Globals.GetResponseJson(response, _JsonSerializerSettings);
                //StaticPublicObjects.ado.P_API_Request_Log_Insert_2("", MethodID, RequestString, null, response, source_, RefNo, StatusCode, response.response_code, ErrorCode, response.errormsg, ResponseJson);
                responsetext = ResponseJson;
            }
        }
    }
}

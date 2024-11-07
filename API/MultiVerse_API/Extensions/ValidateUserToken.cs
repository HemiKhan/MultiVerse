using Data.DataAccess;
using Data.Dtos;

namespace MultiVerse_API.Extensions
{
    public class ValidateUserToken
    {
        public static APILimitResponse GetValidateUserToken(HttpContext context, ref PublicClaimObjects _PublicClaimObjects, ref string responsetext)
        {
            APILimitResponse Ret = new APILimitResponse();
            
            //APILimitResponse _APILimitResponse = new APILimitResponse();
            string ErrorMessage = "";
            string ErrorCode = "";
            int StatusCode = 0;
            string _Path = StaticPublicObjects.ado.GetRequestPath();
            int MethodID = StaticPublicObjects.ado.GetMethodIDFromPath();
            string source_ = AppEnum.APISourceName.GetValidateUserToken + (StaticPublicObjects.ado.IsSwaggerCall() ? "-SW" : "");
            string RefNo = "";

            if (StaticPublicObjects.ado.IsAllowAnonymousMethods() && _PublicClaimObjects.username == "")
            {
                Ret.response_code = true;
                return Ret;
            }
            else
            {
                if (_PublicClaimObjects.P_Get_User_Info_Class!.IsBlocked)
                {
                    ErrorMessage = "This User is Blocked";
                    ErrorCode = ErrorList.ErrorListBlock.ErrorCode;
                    StatusCode = ErrorList.ErrorListBlock.StatusCode;
                    Ret.response_code = false;
                }
                //else if (_PublicClaimObjects.P_Get_User_Info_Class.IsApplicationAccessAllowed == false && _PublicClaimObjects.P_Get_User_Info_Class.IsAPIAccessAllowed == false && _PublicClaimObjects.P_Get_User_Info_Class.IsAdmin == false)
                //{
                //    ErrorMessage = "You Don't Have Access to This Application5";
                //    ErrorCode = ErrorList.ErrorListBlock.ErrorCode;
                //    StatusCode = ErrorList.ErrorListBlock.StatusCode;
                //    Ret.response_code = false;
                //}
                else if (_PublicClaimObjects.P_Get_User_Info_Class.encrypted_key != _PublicClaimObjects.key)
                {
                    ErrorMessage = "Invalid Token1";
                    ErrorCode = ErrorList.ErrorListInvalidToken.ErrorCode;
                    StatusCode = ErrorList.ErrorListInvalidToken.StatusCode;
                    Ret.response_code = false;
                }
                else if (StaticPublicObjects.ado.IsValidToken(_PublicClaimObjects, AppEnum.WebTokenExpiredTime.Seconds) == false && StaticPublicObjects.ado.GetSubDomain() != "localhost")
                {
                    ErrorMessage = "Invalid Token2";
                    ErrorCode = ErrorList.ErrorListInvalidToken.ErrorCode;
                    StatusCode = ErrorList.ErrorListInvalidToken.StatusCode;
                    Ret.response_code = false;
                }
                else
                {
                    Ret.response_code = true;
                    StaticPublicObjects.ado.UpdateTokenKeyCacheTime(_PublicClaimObjects, AppEnum.WebTokenExpiredTime.Seconds); //20 Minutes
                    return Ret;
                }
            }

            Ret.statuscode = StatusCode;
            Ret.errorcode = ErrorCode;
            Ret.errormsg = ErrorMessage;

            if (Ret.response_code == false)
            {
                string RequestString = StaticPublicObjects.ado.GetRequestBodyString().Result;
                OnChallenge.GetResponseText(_Path, ErrorCode, StatusCode, ErrorMessage, MethodID, source_, ref responsetext, RequestString, RefNo);
            }

            return Ret;
        }
    }
}

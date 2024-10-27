using Data.DataAccess;
using Data.Dtos;

namespace MultiVerse_API.Extensions
{
    public class LogAllFailedRequest
    {
        public static void GetLogAllFailedRequest(APIWhiteListingResponse whitelistingresponse, HttpContext context, PublicClaimObjects _PublicClaimObjects, ref string responsetext)
        {
            string ErrorMessage = whitelistingresponse.errormsg;
            string ErrorCode = whitelistingresponse.errorcode;
            int StatusCode = whitelistingresponse.statuscode;
            string _Path = StaticPublicObjects.ado.GetRequestPath();
            int MethodID = StaticPublicObjects.ado.GetMethodIDFromPath();
            string source_ = AppEnum.APISourceName.GetLogAllFailedRequest + (_PublicClaimObjects.isswaggercall ? "-SW" : "");
            string RequestString = StaticPublicObjects.ado.GetRequestBodyString().Result;
            string RefNo = "";

            OnChallenge.GetResponseText(_Path, ErrorCode, StatusCode, ErrorMessage, MethodID, source_, ref responsetext, RequestString, RefNo);
        }
    }
}

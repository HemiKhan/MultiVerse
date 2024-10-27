using MultiVerse_UI.Extensions;
using Data.DataAccess;
using Data.Dtos;
using Microsoft.VisualBasic;
using System.IdentityModel.Tokens.Jwt;
using static MultiVerse_UI.Models.MVCAppEnum;

namespace MultiVerse_UI.Models
{
    public class UserLogic_Security
    {
        public static bool P_Is_Has_Right_From_Username_And_PR_ID_From_Memory(string username, int PR_ID, bool IsApplicant)
        {
            bool ResultCode = false;
            if (IsApplicant == false)
                ResultCode = StaticPublicObjects.ado.P_Is_Has_Right_From_Username_And_PR_ID_From_Memory(username, PR_ID);
            else
            {
                if (PR_ID == RightsList_ID.Job_Setup_View
                    || PR_ID == RightsList_ID.Job_Setup_Add
                    || PR_ID == RightsList_ID.Job_Setup_Edit
                    || PR_ID == RightsList_ID.Job_Setup_Delete)
                    return true;

            }
            return ResultCode;

        }
        public P_Get_User_Info? CurrentUser
        {
            get
            {
                P_Get_User_Info? Ret = null;
                var claim = StaticPublicObjects.ado.GetPublicClaimObjects();
                if (claim != null)
                {
                    Ret = claim.P_Get_User_Info_Class;
                }
                return Ret;
            }
        }
        public static PublicClaimObjects? GetPublicClaimObjectsFromSession
        {
            get
            {
                return StaticPublicObjects.ado.GetPublicClaimObjects();
            }
        }
        public static P_Get_User_Info? CurrentUserStatic
        {
            get
            {
                P_Get_User_Info? Ret = null;
                var claim = StaticPublicObjects.ado.GetPublicClaimObjects();
                if (claim != null)
                {
                    Ret = claim.P_Get_User_Info_Class;
                }
                return Ret;
            }
        }
        public static void SignInAjax()
        {
            IHttpContextAccessor _HttpContextAccessor = StaticPublicObjects.ado.GetIHttpContextAccessor();
            if (_HttpContextAccessor.HttpContext.Request.Cookies.GetCookie_CookiesAccepted() == false)
                return;

            string Encryptedappenv = Crypto.EncryptString(StaticPublicObjects.ado.GetApplicationEnvironment(), true);
            string sso = StaticPublicObjects.ado.GetIsSingleSignOn() ? "sso" : "";
            string appid = AppEnum.ApplicationId.CareerPortalAppID.ToString();

            var username = Convert.ToString(_HttpContextAccessor.HttpContext.Request.Cookies[sso + "user" + Encryptedappenv]);
            var jwtToken = Convert.ToString(_HttpContextAccessor.HttpContext.Request.Cookies[sso + "jwtToken" + Encryptedappenv]);
            var refreshToken = Convert.ToString(_HttpContextAccessor.HttpContext.Request.Cookies[sso + "refreshToken" + Encryptedappenv]);
            if (!string.IsNullOrWhiteSpace(username) && !string.IsNullOrWhiteSpace(jwtToken) && !string.IsNullOrWhiteSpace(refreshToken))
            {
                try
                {
                    username = Crypto.DecryptString(username, true);
                    jwtToken = Crypto.DecryptString(jwtToken, true);
                    refreshToken = Crypto.DecryptString(refreshToken, true);
                }
                catch (Exception ex)
                {
                    StaticPublicObjects.logFile.ErrorLog(FunctionName: "MasterPage Decrypt", SmallMessage: ex.Message, Message: ex.ToString());
                    username = "";
                    jwtToken = "";
                    refreshToken = "";
                }
            }
            if (!string.IsNullOrWhiteSpace(username) && !string.IsNullOrWhiteSpace(jwtToken) && !string.IsNullOrWhiteSpace(refreshToken))
            {
                UserLogic_Security.SignInAjax();
                bool IsValidToken = false;// StaticPublicObjects.ado.GetUserPOMSSSOAPIIsTokenValid(jwtToken);
                bool IsLogin = false;
                if (IsValidToken == false)
                {
                    //RefreshTokenRequestDTO refreshTokenRequestDTO = new RefreshTokenRequestDTO();
                    //RefreshTokenResDTO? refreshTokenResDTO = new RefreshTokenResDTO();

                    //refreshTokenRequestDTO.token = jwtToken;
                    //refreshTokenRequestDTO.refreshToken = refreshToken;
                    //refreshTokenRequestDTO.userID = username.ToUpper();
                    //if (StaticPublicObjects.ado.GetIsSingleSignOn())
                    //    refreshTokenResDTO = StaticPublicObjects.ado.GetUserPOMSSSOAPITokenFromRefreshToken(refreshTokenRequestDTO);
                    //else
                    //    refreshTokenResDTO = StaticPublicObjects.ado.GetPOMSAPITokenFromRefreshToken(refreshTokenRequestDTO);

                    //if (refreshTokenResDTO != null)
                    //{
                    //    if (refreshTokenResDTO.ResponseCode)
                    //    {
                    //        PublicClaimObjects _publicClaimObjects = new PublicClaimObjects();
                    //        AccountController accountController = new AccountController(StaticPublicObjects.ado.GetIConfiguration(), _HttpContextAccessor, null);
                    //        accountController.SetPublicClaimObjectAfterLogin(ref _publicClaimObjects, refreshTokenResDTO.JWToken, "", refreshTokenRequestDTO.refreshToken);
                    //        _HttpContextAccessor.HttpContext.Session.Remove("RedirectURL");
                    //        IsLogin = true;
                    //        _publicClaimObjects = _HttpContextAccessor.HttpContext.Session.GetObject<PublicClaimObjects>("PublicClaimObjects");
                    //        _HttpContextAccessor.HttpContext.Items["PublicClaimObjects"] = _publicClaimObjects;
                    //    }
                    //}
                }
                else
                {
                    PublicClaimObjects _publicClaimObjects = new PublicClaimObjects();
                    //AccountController accountController = new AccountController(StaticPublicObjects.ado.GetIConfiguration(), _HttpContextAccessor, null);
                    //accountController.SetPublicClaimObjectAfterLogin(ref _publicClaimObjects, jwtToken, "", refreshToken);
                    _HttpContextAccessor.HttpContext.Session.Remove("RedirectURL");
                    IsLogin = true;
                    _publicClaimObjects = _HttpContextAccessor.HttpContext.Session.GetObject<PublicClaimObjects>("PublicClaimObjects");
                    _HttpContextAccessor.HttpContext.Items["PublicClaimObjects"] = _publicClaimObjects;

                }
                if (IsLogin == false)
                {
                    _HttpContextAccessor.HttpContext.Session.Clear();
                    var options = new CookieOptions
                    {
                        Expires = DateTimeOffset.UtcNow.AddDays(-1),
                        HttpOnly = true,
                        Secure = true,
                    };
                    if (StaticPublicObjects.ado.GetIsSingleSignOn())
                    {
                        options.Domain = (StaticPublicObjects.ado.IsDevelopment() ? "." + "localhost" : "." + StaticPublicObjects.ado.GetHostNameExcludingSubDomain());
                        options.Path = "/";
                    }
                    foreach (var cookie in _HttpContextAccessor.HttpContext.Request.Cookies.Keys)
                    {
                        if ((((Strings.Left(cookie.ToLower(), 3) == "sso" && sso == "sso") || sso == "")
                                    && Strings.Right(cookie.ToLower(), Strings.Len(Encryptedappenv)) == Encryptedappenv.ToLower())
                                    || cookie.ToLower().Contains(appid))
                        {
                            _HttpContextAccessor.HttpContext.Response.Cookies.Append(cookie, "", options);
                            //_HttpContextAccessor.HttpContext.Response.Cookies.Delete(cookie);
                        }
                    }
                }
            }
        }
        public static void SetPublicClaimObjectFromToken(ref PublicClaimObjects _PublicClaimObjectsNew, string JWToken)
        {

            if (JWToken != "")
            {
                //ClaimsPrincipal User_ = POMS.APIService.Controllers.AccountController.GetPrincipalFromExpiredToken(JWToken, POMS.Domain.Common.AppEnum.ApplicationId.POMSMVCAppID, StaticPublicObjects.ado.GetIConfiguration());

                var handler = new JwtSecurityTokenHandler();
                var token = handler.ReadJwtToken(JWToken);

                _PublicClaimObjectsNew = new PublicClaimObjects();
                _PublicClaimObjectsNew.username = (token?.Claims.FirstOrDefault(claim => claim.Type == "username")?.Value == null ? "" : token?.Claims.FirstOrDefault(claim => claim.Type == "username")?.Value.ToString())!;
                _PublicClaimObjectsNew.jit = (token?.Claims.FirstOrDefault(claim => claim.Type == "jit")?.Value == null ? "" : token?.Claims.FirstOrDefault(claim => claim.Type == "jit")?.Value.ToString())!;
                _PublicClaimObjectsNew.key = (token?.Claims.FirstOrDefault(claim => claim.Type == "key")?.Value == null ? "" : token?.Claims.FirstOrDefault(claim => claim.Type == "key")?.Value.ToString())!;
                _PublicClaimObjectsNew.iswebtoken = (token?.Claims.FirstOrDefault(claim => claim.Type == "isweb")?.Value == null ? false : Convert.ToBoolean(token?.Claims.FirstOrDefault(claim => claim.Type == "isweb")?.Value.ToString()));
                _PublicClaimObjectsNew.issinglesignon = (token?.Claims.FirstOrDefault(claim => claim.Type == "issinglesignon")?.Value == null ? false : Convert.ToBoolean(token?.Claims.FirstOrDefault(claim => claim.Type == "issinglesignon")?.Value.ToString()));

                _PublicClaimObjectsNew.isdevelopment = StaticPublicObjects.ado.IsDevelopment();
                _PublicClaimObjectsNew.isallowedremotedomain = StaticPublicObjects.ado.IsAllowedDomain();
                _PublicClaimObjectsNew.appsettingfilename = (_PublicClaimObjectsNew.isdevelopment == true ? "appsettings.Development.json" : "appsettings.json");
                _PublicClaimObjectsNew.isswaggercall = false;
                _PublicClaimObjectsNew.isswaggercalladmin = false;
                _PublicClaimObjectsNew.path = (StaticPublicObjects.ado == null ? "" : StaticPublicObjects.ado.GetRequestPath());
                _PublicClaimObjectsNew.hostname = (StaticPublicObjects.ado == null ? "" : StaticPublicObjects.ado.GetHostName());
                _PublicClaimObjectsNew.hosturl = (StaticPublicObjects.ado == null ? "" : StaticPublicObjects.ado.GetHostURL());
                _PublicClaimObjectsNew.remotedomain = (StaticPublicObjects.ado == null ? "" : StaticPublicObjects.ado.GetRemoteDomain());
                _PublicClaimObjectsNew.remoteurl = (StaticPublicObjects.ado == null ? "" : StaticPublicObjects.ado.GetRemoteURL());
                //_PublicClaimObjectsNew.P_Get_User_Info_Class = (StaticPublicObjects.ado == null ? null : StaticPublicObjects.ado.P_Get_User_Info_Class(_PublicClaimObjectsNew.username, AppEnum.ApplicationId.AppID));

            }
        }
    }
}

using MultiVerse_UI.Extensions;
using Data.DataAccess;
using Data.Dtos;
using Data.Models;
using Microsoft.VisualBasic;
using Newtonsoft.Json;
using System.IdentityModel.Tokens.Jwt;

namespace MultiVerse_UI.Models
{
    public class ProgramSetPublicClaimObjects
    {
        public static void SetPublicClaimObjectsVerifyToken(ref HttpContext context, string sso, string Encryptedappenv, ref string jwtToken, ref bool IsRunVerifyToken, ref bool IsValidToken, ref string refreshToken, ref string user, ref bool RememberMe, ref CookieOptions? cookietimeObj, ref DateTime? lastaccessdatetime, ref PublicClaimObjects _PublicClaimObjects, bool IsForceRun = false, bool IsRunRefreshToken = false)
        {
            IsRunVerifyToken = false;
            if (context.Session.GetBool("istokenverify") == null || IsForceRun)
            {
                IsValidToken = false;
                context.Session.SetBool("istokenverify", true);
                IsRunVerifyToken = true;
                //IsValidToken = StaticPublicObjects.ado.GetUserPOMSSSOAPIIsTokenValid(jwtToken);
                context.Session.SetDateTime("validtokendatetime", DateTime.UtcNow);
                user = "";
                JwtSecurityTokenHandler handler = new JwtSecurityTokenHandler();
                JwtSecurityToken? token = null;
                if (jwtToken != "")
                {
                    token = new JwtSecurityToken();
                    token = handler.ReadJwtToken(jwtToken);
                    var userClaim = token?.Claims.FirstOrDefault(claim => claim.Type == "username");
                    user = userClaim?.Value ?? "";
                }

                if (IsValidToken && user != "" && IsRunRefreshToken == false)
                {
                    bool cookieRememberMe = false;
                    string cookierm = context.Request.Cookies.GetCookie(sso + "rm" + Encryptedappenv) ?? "";
                    if (cookierm == "")
                        cookierm = context.Session.GetStringValue("rm") ?? "";
                    if (cookierm != "")
                        cookieRememberMe = Convert.ToBoolean(Crypto.DecryptString(cookierm, true));
                    CookieOptions options = new CookieOptions();
                    if (cookietimeObj != null)
                        options = cookietimeObj;
                    else
                    {
                        options = new CookieOptions
                        {
                            Domain = (StaticPublicObjects.ado.IsDevelopment() ? "." + "localhost" : "." + StaticPublicObjects.ado.GetHostNameExcludingSubDomain()),
                            Path = "/",
                            Expires = (cookieRememberMe ? DateTimeOffset.UtcNow.AddDays(AppEnum.WebRememberMeTokenExpiredTime.Days) : DateTimeOffset.UtcNow.AddMinutes(AppEnum.WebTokenExpiredTime.Minutes)),
                            HttpOnly = true,
                            Secure = true,
                        };
                    }
                    if (cookierm != "")
                    {
                        context.Response.Cookies.Append(sso + "rm" + Encryptedappenv, cookierm, options);
                        context.Session.SetStringValue("rm", cookierm);
                    }
                    if (user != "")
                    {
                        context.Response.Cookies.Append(sso + "user" + Encryptedappenv, Crypto.EncryptString(user, true), options);
                        context.Session.SetStringValue("user", Crypto.EncryptString(user, true));
                    }
                    if (jwtToken != "")
                    {
                        context.Response.Cookies.Append(sso + "jwtToken" + Encryptedappenv, Crypto.EncryptString(jwtToken, true), options);
                        context.Session.SetStringValue("jwtToken", Crypto.EncryptString(jwtToken, true));
                    }
                    //if (cookieRememberMe)
                    {
                        if (refreshToken != "")
                        {
                            context.Response.Cookies.Append(sso + "refreshToken" + Encryptedappenv, Crypto.EncryptString(refreshToken, true), options);
                            context.Session.SetStringValue("refreshToken", Crypto.EncryptString(refreshToken, true));
                        }
                        {
                            context.Response.Cookies.Append(sso + "cookietime" + Encryptedappenv, Crypto.EncryptString(Newtonsoft.Json.JsonConvert.SerializeObject(options), true), options);
                            context.Session.SetStringValue("cookietime", Crypto.EncryptString(JsonConvert.SerializeObject(cookietimeObj), true));
                        }
                    }
                    context.Response.Cookies.Append(sso + "lastaccess" + Encryptedappenv, Crypto.EncryptString(DateTime.UtcNow.AddMinutes(AppEnum.WebTokenExpiredTime.Minutes).ToString("yyyy-MM-dd HH:mm:ss.fff"), true), new CookieOptions
                    {
                        Domain = (StaticPublicObjects.ado.IsDevelopment() ? "." + "localhost" : "." + StaticPublicObjects.ado.GetHostNameExcludingSubDomain()),
                        Path = "/",
                        Expires = DateTimeOffset.UtcNow.AddMinutes(AppEnum.WebTokenExpiredTime.Minutes),
                        HttpOnly = true,
                        Secure = true,
                    });
                    UserLogic_Security.SetPublicClaimObjectFromToken(ref _PublicClaimObjects, jwtToken);
                    context.Session.SetObject<PublicClaimObjects>("PublicClaimObjects", _PublicClaimObjects);
                    string GUID_ = context.Session.GetStringValue("FileGUID") ?? Guid.NewGuid().ToString().ToLower();
                    context.Session.SetStringValue("FileGUID", GUID_);
                    context.Session.SetDateTime("validtokendatetime", DateTime.UtcNow);
                }
                else if (jwtToken != "")
                {
                    var expClaim = token?.Claims.FirstOrDefault(claim => claim.Type == "exp");
                    if (expClaim != null && long.TryParse(expClaim.Value, out long expUnixTime))
                    {
                        DateTime expDateTime = DateTimeOffset.FromUnixTimeSeconds(expUnixTime).DateTime;
                        if (expDateTime < DateTime.UtcNow)
                        {
                            if ((jwtToken != "" && refreshToken != "" && user != "" && _PublicClaimObjects != null && context.Session.GetBool("GetToken") == null) || IsRunRefreshToken)
                            {
                                context.Session.SetBool("GetToken", true);
                                RefreshTokenRequestDTO _RefreshTokenRequestDTO = new RefreshTokenRequestDTO();
                                _RefreshTokenRequestDTO.userID = user;
                                _RefreshTokenRequestDTO.refreshToken = refreshToken;
                                _RefreshTokenRequestDTO.token = jwtToken;
                                RefreshTokenResDTO? _RefreshTokenResDTO = new RefreshTokenResDTO(); //StaticPublicObjects.ado.GetUserPOMSSSOAPITokenFromRefreshToken(_RefreshTokenRequestDTO);
                                IsValidToken = _RefreshTokenResDTO.ResponseCode;
                                if (IsValidToken)
                                {
                                    jwtToken = _RefreshTokenResDTO.JWToken;
                                    CookieOptions options = new CookieOptions();
                                    if (cookietimeObj != null)
                                        options = cookietimeObj;
                                    else
                                    {
                                        options = new CookieOptions
                                        {
                                            Domain = (StaticPublicObjects.ado.IsDevelopment() ? "." + "localhost" : "." + StaticPublicObjects.ado.GetHostNameExcludingSubDomain()),
                                            Path = "/",
                                            Expires = (RememberMe ? DateTimeOffset.UtcNow.AddDays(AppEnum.WebRememberMeTokenExpiredTime.Days) : DateTimeOffset.UtcNow.AddMinutes(AppEnum.WebTokenExpiredTime.Minutes)),
                                            HttpOnly = true,
                                            Secure = true,
                                        };
                                    }
                                    if (jwtToken != "")
                                    {
                                        context.Response.Cookies.Append(sso + "jwtToken" + Encryptedappenv, Crypto.EncryptString(jwtToken, true), options);
                                        context.Session.SetStringValue("jwtToken", Crypto.EncryptString(jwtToken, true));
                                    }
                                    context.Response.Cookies.Append(sso + "user" + Encryptedappenv, Crypto.EncryptString(user, true), options);
                                    context.Session.SetStringValue("user", Crypto.EncryptString(user, true));
                                    context.Response.Cookies.Append(sso + "lastaccess" + Encryptedappenv, Crypto.EncryptString(DateTime.UtcNow.AddMinutes(AppEnum.WebTokenExpiredTime.Minutes).ToString("yyyy-MM-dd HH:mm:ss.fff"), true), new CookieOptions
                                    {
                                        Domain = (StaticPublicObjects.ado.IsDevelopment() ? "." + "localhost" : "." + StaticPublicObjects.ado.GetHostNameExcludingSubDomain()),
                                        Path = "/",
                                        Expires = DateTimeOffset.UtcNow.AddMinutes(AppEnum.WebTokenExpiredTime.Minutes),
                                        HttpOnly = true,
                                        Secure = true,
                                    });
                                    UserLogic_Security.SetPublicClaimObjectFromToken(ref _PublicClaimObjects, jwtToken);
                                    context.Session.SetObject<PublicClaimObjects>("PublicClaimObjects", _PublicClaimObjects);
                                    string GUID_ = context.Session.GetStringValue("FileGUID") ?? Guid.NewGuid().ToString().ToLower();
                                    context.Session.SetStringValue("FileGUID", GUID_);
                                    context.Session.SetDateTime("validtokendatetime", DateTime.UtcNow);
                                }
                                context.Session.Remove("GetToken");
                            }
                        }
                    }
                }
                context.Session.Remove("istokenverify");
            }
        }
        public static void SetPublicClaimObjectsRemoveCookie(ref HttpContext context, ref DateTime? lastaccessdatetime, ref PublicClaimObjects _PublicClaimObjects, string sso, string Encryptedappenv, string appid, bool isremoveallcookies = true, List<string>? liststring = null)
        {
            var options = new CookieOptions
            {
                Domain = (StaticPublicObjects.ado.IsDevelopment() ? "." + "localhost" : "." + StaticPublicObjects.ado.GetHostNameExcludingSubDomain()),
                Path = "/",
                Expires = DateTimeOffset.UtcNow.AddDays(-1),
                HttpOnly = true,
                Secure = true,
            };
            if (isremoveallcookies)
            {
                context.Response.Cookies.SetCookie(sso + "rm" + Encryptedappenv, "", options);
                context.Response.Cookies.SetCookie(sso + "user" + Encryptedappenv, "", options);
                context.Response.Cookies.SetCookie(sso + "refreshToken" + Encryptedappenv, "", options);
                context.Response.Cookies.SetCookie(sso + "cookietime" + Encryptedappenv, "", options);
                context.Response.Cookies.SetCookie(sso + "jwtToken" + Encryptedappenv, "", options);

                foreach (var cookie in context.Request.Cookies.Keys)
                {
                    if ((((Strings.Left(cookie.ToLower(), 3) == "sso" && sso == "sso") || sso == "")
                        && Strings.Right(cookie.ToLower(), Strings.Len(Encryptedappenv)) == Encryptedappenv.ToLower())
                        || cookie.ToLower().Contains(appid))
                    {
                        context.Response.Cookies.Append(cookie, "", options);
                        //context.Response.Cookies.Delete(cookie);
                    }
                }
                lastaccessdatetime = null;
                _PublicClaimObjects = null;
                string FileGUID = context.Session.GetStringValue("FileGUID") ?? "";
                context.Session.Clear();
                if (FileGUID != "")
                    context.Session.SetStringValue("FileGUID", FileGUID);
            }
            else
            {
                for (int i = 0; i <= liststring?.Count - 1; i++)
                {
                    context.Response.Cookies.SetCookie(liststring?[i], "", options);
                }
                lastaccessdatetime = null;
                _PublicClaimObjects = null;
                string FileGUID = context.Session.GetStringValue("FileGUID") ?? "";
                context.Session.Clear();
                if (FileGUID != "")
                    context.Session.SetStringValue("FileGUID", FileGUID);
            }
        }
        public static void SetPublicClaimObjectsFromCookie(ref HttpContext context, ref PublicClaimObjects _PublicClaimObjects)
        {
            if (context.Request.RouteValues.Count > 0)
            {
                if (_PublicClaimObjects != null)
                {
                    try
                    {
                        _PublicClaimObjects.requeststarttime = Convert.ToDateTime(context.Items["RequestStartTime"]);
                    }
                    catch (Exception ex)
                    {
                        StaticPublicObjects.logFile.ErrorLog(FunctionName: "SetPublicClaimObjectsFromCookie RequestStartTime", SmallMessage: ex.Message, Message: ex.ToString());
                    }
                }

                bool CookiesAccepted = context.Request.Cookies.GetCookie_CookiesAccepted();
                bool IsSingleSignOn = StaticPublicObjects.ado.GetIsSingleSignOn();
                string Encryptedappenv = Crypto.EncryptString(StaticPublicObjects.ado.GetApplicationEnvironment(), true);
                string sso = IsSingleSignOn ? "sso" : "";
                string appid = AppEnum.ApplicationId.CareerPortalAppID.ToString();
                bool IsRunVerifyToken = false;
                bool IsValidToken = true;
                DateTime Currentdatetime = DateTime.UtcNow;

                if (IsSingleSignOn)
                {
                    bool isvalidatetoken = false;
                    string RememberMeString = context.Request.Cookies.GetCookie_RMString(sso, Encryptedappenv);
                    bool RememberMe = context.Request.Cookies.GetCookie_RM(sso, Encryptedappenv);
                    string user = context.Request.Cookies.GetCookie_User(sso, Encryptedappenv);
                    string jwtToken = context.Request.Cookies.GetCookie_JWTToken(sso, Encryptedappenv);
                    string refreshToken = context.Request.Cookies.GetCookie_RefreshToken(sso, Encryptedappenv);
                    CookieOptions? cookietimeObj = new CookieOptions();
                    cookietimeObj = context.Request.Cookies.GetCookie_CookieTime(sso, Encryptedappenv);
                    DateTime? lastaccessdatetime = context.Request.Cookies.GetCookie_LastAccess(sso, Encryptedappenv);
                    CookieOptions lastaccessCookieOptions = new CookieOptions
                    {
                        Domain = (StaticPublicObjects.ado.IsDevelopment() ? "." + "localhost" : "." + StaticPublicObjects.ado.GetHostNameExcludingSubDomain()),
                        Path = "/",
                        Expires = DateTimeOffset.UtcNow.AddMinutes(AppEnum.WebTokenExpiredTime.Minutes),
                        HttpOnly = true,
                        Secure = true,
                    };

                    if (CookiesAccepted == false)
                    {
                        SetPublicClaimObjectsRemoveCookie(ref context, ref lastaccessdatetime, ref _PublicClaimObjects, sso, Encryptedappenv, appid, isremoveallcookies: true, liststring: null);
                        return;
                    }

                    if (_PublicClaimObjects != null)
                    {
                        if (jwtToken == "" && context.Session.GetStringValue("jwtToken") != null)
                        {
                            context.Response.Cookies.SetCookie(sso + "jwtToken" + Encryptedappenv, context.Session.GetStringValue("jwtToken"), lastaccessCookieOptions);
                            jwtToken = context.Session.GetJWTToken();
                            isvalidatetoken = true;
                            //SetPublicClaimObjectsVerifyToken(ref context, sso, Encryptedappenv, ref jwtToken, ref IsRunVerifyToken, ref IsValidToken, ref refreshToken, ref user, ref RememberMe, ref cookietimeObj, ref lastaccessdatetime, ref _PublicClaimObjects, true);
                        }

                        if (user == "" && context.Session.GetStringValue("user") != null)
                        {
                            context.Response.Cookies.SetCookie(sso + "user" + Encryptedappenv, context.Session.GetStringValue("user"), lastaccessCookieOptions);
                            user = context.Session.GetUser();
                            isvalidatetoken = true;
                            //SetPublicClaimObjectsVerifyToken(ref context, sso, Encryptedappenv, ref jwtToken, ref IsRunVerifyToken, ref IsValidToken, ref refreshToken, ref user, ref RememberMe, ref cookietimeObj, ref lastaccessdatetime, ref _PublicClaimObjects, true);
                        }

                    }

                    if ((_PublicClaimObjects?.username ?? "").ToUpper() != (context.Session.GetStringValue("User_Name") ?? "").ToUpper() && (context.Session.GetStringValue("User_Name") ?? "") != "")
                    {
                        isvalidatetoken = true;
                        _PublicClaimObjects = null;
                        string FileGUID = context.Session.GetStringValue("FileGUID") ?? "";
                        context.Session.Clear();
                        if (FileGUID != "")
                        {
                            context.Session.SetStringValue("FileGUID", FileGUID);
                        }
                    }
                    else if ((_PublicClaimObjects?.username ?? "").ToUpper() != (user ?? "").ToUpper() && (_PublicClaimObjects?.username ?? "") != "")
                    {
                        isvalidatetoken = true;
                        _PublicClaimObjects = null;
                        string FileGUID = context.Session.GetStringValue("FileGUID") ?? "";
                        context.Session.Clear();
                        if (FileGUID != "")
                        {
                            context.Session.SetStringValue("FileGUID", FileGUID);
                        }
                    }
                    else if ((jwtToken ?? "") != (Crypto.DecryptString(context.Session.GetStringValue("jwtToken"), true) ?? "") && (context.Session.GetStringValue("jwtToken") ?? "") != "")
                    {
                        isvalidatetoken = true;
                        _PublicClaimObjects = null;
                        string FileGUID = context.Session.GetStringValue("FileGUID") ?? "";
                        context.Session.Clear();
                        if (FileGUID != "")
                            context.Session.SetStringValue("FileGUID", FileGUID);
                    }

                    if (IsSingleSignOn)
                    {
                        if (_PublicClaimObjects != null)
                        {
                            if (RememberMeString != "" && context.Session.GetStringValue("rm") == null)
                                context.Session.SetStringValue("rm", Crypto.EncryptString(RememberMe.ToString(), true));

                            if (user != "" && context.Session.GetStringValue("user") == null)
                                context.Session.SetStringValue("user", Crypto.EncryptString(user, true));

                            if (jwtToken != "" && context.Session.GetStringValue("jwtToken") == null)
                                context.Session.SetStringValue("jwtToken", Crypto.EncryptString(jwtToken, true));

                            if (refreshToken != "" && context.Session.GetStringValue("refreshToken") == null)
                                context.Session.SetStringValue("refreshToken", Crypto.EncryptString(refreshToken, true));

                            if (cookietimeObj != null && context.Session.GetStringValue("cookietime") == null)
                                context.Session.SetStringValue("cookietime", Crypto.EncryptString(JsonConvert.SerializeObject(cookietimeObj), true));

                            if (RememberMeString == "" && context.Session.GetStringValue("rm") != null)
                            {
                                context.Response.Cookies.SetCookie(sso + "rm" + Encryptedappenv, context.Session.GetStringValue("rm"), lastaccessCookieOptions);
                                RememberMe = context.Session.GetRememberMe();
                                RememberMeString = Crypto.EncryptString(RememberMe.ToString(), true);
                                isvalidatetoken = true;
                            }

                            if (user == "" && context.Session.GetStringValue("user") != null)
                            {
                                context.Response.Cookies.SetCookie(sso + "user" + Encryptedappenv, context.Session.GetStringValue("user"), lastaccessCookieOptions);
                                user = context.Session.GetUser();
                                isvalidatetoken = true;
                            }

                            if (refreshToken == "" && context.Session.GetStringValue("refreshToken") != null)
                            {
                                context.Response.Cookies.SetCookie(sso + "refreshToken" + Encryptedappenv, context.Session.GetStringValue("refreshToken"), lastaccessCookieOptions);
                                refreshToken = context.Session.GetRefreshToken();
                                isvalidatetoken = true;
                            }

                            if (cookietimeObj == null && context.Session.GetStringValue("cookietime") != null)
                            {
                                context.Response.Cookies.SetCookie(sso + "cookietime" + Encryptedappenv, context.Session.GetStringValue("cookietime"), lastaccessCookieOptions);
                                cookietimeObj = context.Session.GetCookieTime();
                                isvalidatetoken = true;
                            }

                            if (jwtToken == "" && context.Session.GetStringValue("jwtToken") != null)
                            {
                                context.Response.Cookies.SetCookie(sso + "jwtToken" + Encryptedappenv, context.Session.GetStringValue("jwtToken"), lastaccessCookieOptions);
                                jwtToken = context.Session.GetJWTToken();
                                isvalidatetoken = true;
                                //SetPublicClaimObjectsVerifyToken(ref context, sso, Encryptedappenv, ref jwtToken, ref IsRunVerifyToken, ref IsValidToken, ref refreshToken, ref user, ref RememberMe, ref cookietimeObj, ref lastaccessdatetime, ref _PublicClaimObjects, true);
                            }
                        }

                        if (isvalidatetoken)
                        {
                            SetPublicClaimObjectsVerifyToken(ref context, sso, Encryptedappenv, ref jwtToken, ref IsRunVerifyToken, ref IsValidToken, ref refreshToken, ref user, ref RememberMe, ref cookietimeObj, ref lastaccessdatetime, ref _PublicClaimObjects, true);
                            if (IsRunVerifyToken)
                            {
                                if (IsValidToken == false)
                                {
                                    if (RememberMe == false)
                                    {
                                        SetPublicClaimObjectsRemoveCookie(ref context, ref lastaccessdatetime, ref _PublicClaimObjects, sso, Encryptedappenv, appid, isremoveallcookies: true, liststring: null);
                                    }
                                    else
                                    {
                                        SetPublicClaimObjectsRemoveCookie(ref context, ref lastaccessdatetime, ref _PublicClaimObjects, sso, Encryptedappenv, appid, isremoveallcookies: false, liststring: new List<string> { "lastaccess" });
                                    }
                                }
                            }
                        }
                        else
                        {
                            if (lastaccessdatetime != null)
                            {
                                if (lastaccessdatetime > DateTime.UtcNow && jwtToken != "")
                                {
                                    context.Response.Cookies.Append(sso + "lastaccess" + Encryptedappenv, Crypto.EncryptString(DateTime.UtcNow.AddMinutes(AppEnum.WebTokenExpiredTime.Minutes).ToString("yyyy-MM-dd HH:mm:ss.fff"), true), lastaccessCookieOptions);
                                }
                                else
                                {
                                    SetPublicClaimObjectsVerifyToken(ref context, sso, Encryptedappenv, ref jwtToken, ref IsRunVerifyToken, ref IsValidToken, ref refreshToken, ref user, ref RememberMe, ref cookietimeObj, ref lastaccessdatetime, ref _PublicClaimObjects);
                                    if (IsRunVerifyToken)
                                    {
                                        if (IsValidToken == false)
                                        {
                                            if (RememberMe == false)
                                            {
                                                SetPublicClaimObjectsRemoveCookie(ref context, ref lastaccessdatetime, ref _PublicClaimObjects, sso, Encryptedappenv, appid, isremoveallcookies: true, liststring: null);
                                            }
                                            else
                                            {
                                                SetPublicClaimObjectsRemoveCookie(ref context, ref lastaccessdatetime, ref _PublicClaimObjects, sso, Encryptedappenv, appid, isremoveallcookies: false, liststring: new List<string> { "lastaccess" });
                                            }
                                        }
                                    }
                                }
                            }
                            else
                            {
                                SetPublicClaimObjectsVerifyToken(ref context, sso, Encryptedappenv, ref jwtToken, ref IsRunVerifyToken, ref IsValidToken, ref refreshToken, ref user, ref RememberMe, ref cookietimeObj, ref lastaccessdatetime, ref _PublicClaimObjects);
                                if (IsRunVerifyToken)
                                {
                                    if (IsValidToken == false)
                                    {
                                        if (RememberMe == false)
                                        {
                                            SetPublicClaimObjectsRemoveCookie(ref context, ref lastaccessdatetime, ref _PublicClaimObjects, sso, Encryptedappenv, appid, isremoveallcookies: true, liststring: null);
                                        }
                                        else
                                        {
                                            SetPublicClaimObjectsRemoveCookie(ref context, ref lastaccessdatetime, ref _PublicClaimObjects, sso, Encryptedappenv, appid, isremoveallcookies: false, liststring: new List<string> { "lastaccess" });
                                        }
                                    }
                                }
                            }
                        }

                        if (context.Session.GetBool("istokenverify") == null)
                        {
                            DateTime? validtokendateTime = null;
                            validtokendateTime = context.Session.GetDateTime("validtokendatetime");
                            bool isvalidtokenapirun = true;
                            if (validtokendateTime != null)
                            {
                                if ((DateTime)validtokendateTime > DateTime.UtcNow.AddMinutes(-5))
                                {
                                    isvalidtokenapirun = false;
                                }
                            }
                            if (isvalidtokenapirun && IsValidToken)
                            {
                                SetPublicClaimObjectsVerifyToken(ref context, sso, Encryptedappenv, ref jwtToken, ref IsRunVerifyToken, ref IsValidToken, ref refreshToken, ref user, ref RememberMe, ref cookietimeObj, ref lastaccessdatetime, ref _PublicClaimObjects);
                                if (IsRunVerifyToken)
                                {
                                    if (IsValidToken == false)
                                    {
                                        if (RememberMe == false)
                                        {
                                            SetPublicClaimObjectsRemoveCookie(ref context, ref lastaccessdatetime, ref _PublicClaimObjects, sso, Encryptedappenv, appid, isremoveallcookies: true, liststring: null);
                                        }
                                        else
                                        {
                                            SetPublicClaimObjectsRemoveCookie(ref context, ref lastaccessdatetime, ref _PublicClaimObjects, sso, Encryptedappenv, appid, isremoveallcookies: false, liststring: new List<string> { "lastaccess" });
                                        }
                                    }
                                }
                            }
                        }

                        if (jwtToken != "" && refreshToken != "" && user != "" && _PublicClaimObjects == null && RememberMe && IsValidToken == false)
                        {
                            SetPublicClaimObjectsVerifyToken(ref context, sso, Encryptedappenv, ref jwtToken, ref IsRunVerifyToken, ref IsValidToken, ref refreshToken, ref user, ref RememberMe, ref cookietimeObj, ref lastaccessdatetime, ref _PublicClaimObjects, true, true);
                            if (IsRunVerifyToken)
                            {
                                if (IsValidToken == false)
                                {
                                    if (RememberMe == false)
                                    {
                                        SetPublicClaimObjectsRemoveCookie(ref context, ref lastaccessdatetime, ref _PublicClaimObjects, sso, Encryptedappenv, appid, isremoveallcookies: true, liststring: null);
                                    }
                                    else
                                    {
                                        SetPublicClaimObjectsRemoveCookie(ref context, ref lastaccessdatetime, ref _PublicClaimObjects, sso, Encryptedappenv, appid, isremoveallcookies: false, liststring: new List<string> { "lastaccess" });
                                    }
                                }
                            }
                        }

                        if (_PublicClaimObjects == null && context.Session.GetBool("GetToken") == null)
                        {
                            if (jwtToken != "" && IsValidToken)
                            {
                                SetPublicClaimObjectsVerifyToken(ref context, sso, Encryptedappenv, ref jwtToken, ref IsRunVerifyToken, ref IsValidToken, ref refreshToken, ref user, ref RememberMe, ref cookietimeObj, ref lastaccessdatetime, ref _PublicClaimObjects);
                                if (IsRunVerifyToken)
                                {
                                    if (IsValidToken == false || _PublicClaimObjects == null)
                                    {
                                        if (RememberMe == false)
                                        {
                                            SetPublicClaimObjectsRemoveCookie(ref context, ref lastaccessdatetime, ref _PublicClaimObjects, sso, Encryptedappenv, appid, isremoveallcookies: true, liststring: null);
                                        }
                                        else
                                        {
                                            SetPublicClaimObjectsRemoveCookie(ref context, ref lastaccessdatetime, ref _PublicClaimObjects, sso, Encryptedappenv, appid, isremoveallcookies: false, liststring: new List<string> { "lastaccess" });
                                        }
                                    }
                                }
                            }

                        }
                    }
                    else if (jwtToken != "" && refreshToken != "" && user != "" && _PublicClaimObjects == null && IsSingleSignOn == false && context.Session.GetBool("GetToken") == null)
                    {
                        context.Session.SetBool("GetToken", true);
                        UserLogic_Security.SignInAjax();
                        _PublicClaimObjects = context.Session.GetObject<PublicClaimObjects>("PublicClaimObjects");
                        context.Session.Remove("GetToken");
                    }
                }
            }
        }
    }
}

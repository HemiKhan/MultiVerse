using Data.DataAccess;
using Microsoft.VisualBasic;
using Newtonsoft.Json;

namespace MultiVerse_UI.Extensions
{
    public static class CookieService
    {
        // Get cookie value by name
        public static string? GetCookie(this IRequestCookieCollection _httpContextAccessor, string key)
        {
            _httpContextAccessor.TryGetValue(key, out string? value);
            return value;
        }
        public static string GetCookie_RMString(this IRequestCookieCollection _httpContextAccessor, string? sso, string? Encryptedappenv)
        {
            Encryptedappenv = Encryptedappenv ?? Crypto.EncryptString(StaticPublicObjects.ado.GetApplicationEnvironment(), true);
            sso = sso ?? (StaticPublicObjects.ado.GetIsSingleSignOn() ? "sso" : "");

            string key = sso + "rm" + Encryptedappenv;
            _httpContextAccessor.TryGetValue(key, out string? value);
            string RememberMeString = "";
            try
            {
                RememberMeString = Crypto.DecryptString((value ?? ""), true);
            }
            catch (Exception ex)
            {
                StaticPublicObjects.logFile.ErrorLog(FunctionName: "GetCookie_RM", SmallMessage: ex.Message, Message: ex.ToString());
            }
            return RememberMeString;
        }
        public static bool GetCookie_RM(this IRequestCookieCollection _httpContextAccessor, string? sso, string? Encryptedappenv)
        {
            bool RememberMe = false;
            try
            {
                string RememberMeString = GetCookie_RMString(_httpContextAccessor, sso, Encryptedappenv);
                RememberMe = string.IsNullOrEmpty(RememberMeString) == false ? Convert.ToBoolean(RememberMeString) : false;
            }
            catch (Exception ex)
            {
                StaticPublicObjects.logFile.ErrorLog(FunctionName: "GetCookie_RM", SmallMessage: ex.Message, Message: ex.ToString());
            }
            return RememberMe;
        }
        public static string GetCookie_User(this IRequestCookieCollection _httpContextAccessor, string? sso, string? Encryptedappenv)
        {
            Encryptedappenv = Encryptedappenv ?? Crypto.EncryptString(StaticPublicObjects.ado.GetApplicationEnvironment(), true);
            sso = sso ?? (StaticPublicObjects.ado.GetIsSingleSignOn() ? "sso" : "");

            string key = sso + "user" + Encryptedappenv;
            string user = "";
            _httpContextAccessor.TryGetValue(key, out string? value);
            try
            {
                user = Crypto.DecryptString((value ?? ""), true);
            }
            catch (Exception ex)
            {
                StaticPublicObjects.logFile.ErrorLog(FunctionName: "GetCookie_User", SmallMessage: ex.Message, Message: ex.ToString());
            }
            return user;
        }
        public static string GetCookie_JWTToken(this IRequestCookieCollection _httpContextAccessor, string? sso, string? Encryptedappenv)
        {
            Encryptedappenv = Encryptedappenv ?? Crypto.EncryptString(StaticPublicObjects.ado.GetApplicationEnvironment(), true);
            sso = sso ?? (StaticPublicObjects.ado.GetIsSingleSignOn() ? "sso" : "");

            string key = sso + "jwtToken" + Encryptedappenv;
            _httpContextAccessor.TryGetValue(key, out string? value);
            string jwtToken = "";
            try
            {
                jwtToken = Crypto.DecryptString((value ?? ""), true);
            }
            catch (Exception ex)
            {
                StaticPublicObjects.logFile.ErrorLog(FunctionName: "GetCookie_JWTToken", SmallMessage: ex.Message, Message: ex.ToString());
            }
            return jwtToken;
        }
        public static string GetCookie_RefreshToken(this IRequestCookieCollection _httpContextAccessor, string? sso, string? Encryptedappenv)
        {
            Encryptedappenv = Encryptedappenv ?? Crypto.EncryptString(StaticPublicObjects.ado.GetApplicationEnvironment(), true);
            sso = sso ?? (StaticPublicObjects.ado.GetIsSingleSignOn() ? "sso" : "");

            string key = sso + "refreshToken" + Encryptedappenv;
            _httpContextAccessor.TryGetValue(key, out string? value);
            string refreshToken = "";
            try
            {
                refreshToken = Crypto.DecryptString((value ?? ""), true);
            }
            catch (Exception ex)
            {
                StaticPublicObjects.logFile.ErrorLog(FunctionName: "GetCookie_RefreshToken", SmallMessage: ex.Message, Message: ex.ToString());
            }
            return refreshToken;
        }
        public static CookieOptions? GetCookie_CookieTime(this IRequestCookieCollection _httpContextAccessor, string? sso, string? Encryptedappenv)
        {
            Encryptedappenv = Encryptedappenv ?? Crypto.EncryptString(StaticPublicObjects.ado.GetApplicationEnvironment(), true);
            sso = sso ?? (StaticPublicObjects.ado.GetIsSingleSignOn() ? "sso" : "");

            string key = sso + "cookietime" + Encryptedappenv;
            _httpContextAccessor.TryGetValue(key, out string? value);
            CookieOptions? cookietimeObj = null;
            try
            {
                string cookietime = Crypto.DecryptString((value ?? ""), true);
                if (string.IsNullOrEmpty(cookietime) == false)
                {
                    cookietimeObj = new CookieOptions();
                    cookietimeObj = JsonConvert.DeserializeObject<CookieOptions>(cookietime);
                }
            }
            catch (Exception ex)
            {
                StaticPublicObjects.logFile.ErrorLog(FunctionName: "GetCookie_CookieTime", SmallMessage: ex.Message, Message: ex.ToString());
            }
            return cookietimeObj;
        }
        public static DateTime? GetCookie_LastAccess(this IRequestCookieCollection _httpContextAccessor, string? sso, string? Encryptedappenv)
        {
            Encryptedappenv = Encryptedappenv ?? Crypto.EncryptString(StaticPublicObjects.ado.GetApplicationEnvironment(), true);
            sso = sso ?? (StaticPublicObjects.ado.GetIsSingleSignOn() ? "sso" : "");

            string key = sso + "lastaccess" + Encryptedappenv;
            _httpContextAccessor.TryGetValue(key, out string? value);
            DateTime? lastaccessdatetime = null;
            try
            {
                string lastaccess = Crypto.DecryptString((value ?? ""), true);
                if (string.IsNullOrEmpty(lastaccess) == false)
                {
                    if (Information.IsDate(lastaccess))
                    {
                        lastaccessdatetime = Convert.ToDateTime(lastaccess);
                    }
                }
            }
            catch (Exception ex)
            {
                StaticPublicObjects.logFile.ErrorLog(FunctionName: "GetCookie_LastAccess", SmallMessage: ex.Message, Message: ex.ToString());
            }
            return lastaccessdatetime;
        }
        public static void SetCookie(this IResponseCookies _httpContextAccessor, string key, string value, CookieOptions? options = null)
        {
            if (options == null)
            {
                options = new CookieOptions { HttpOnly = true, Secure = true, Expires = DateTime.UtcNow.AddDays(1) };
            }
            _httpContextAccessor.Append(key, value, options);
        }
        public static bool GetCookie_CookiesAccepted(this IRequestCookieCollection _httpContextAccessor)
        {
            string key = "cookiesAccepted";
            bool CookiesAccepted = false;
            _httpContextAccessor.TryGetValue(key, out string? value);
            try
            {
                if (value != "")
                {
                    try
                    {
                        value = Crypto.DecryptString((value ?? ""), true);
                        CookiesAccepted = string.IsNullOrEmpty(value) == false ? Convert.ToBoolean(value) : false;
                    }
                    catch (Exception ex)
                    {
                        StaticPublicObjects.logFile.ErrorLog(FunctionName: "GetCookie_CookiesAccepted 1", SmallMessage: ex.Message, Message: ex.ToString());
                    }
                }
            }
            catch (Exception ex)
            {
                StaticPublicObjects.logFile.ErrorLog(FunctionName: "GetCookie_CookiesAccepted", SmallMessage: ex.Message, Message: ex.ToString());
            }
            return CookiesAccepted;
        }
        // Set cookie value with options
    }
}

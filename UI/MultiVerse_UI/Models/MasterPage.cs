using Data.DataAccess;
using Data.Dtos;

namespace MultiVerse_UI.Models
{
    public class MasterPage
    {
        public IHttpContextAccessor _HttpContextAccessor;
        public MasterPage()
        {
            _HttpContextAccessor = StaticPublicObjects.ado.GetIHttpContextAccessor();
        }
        public class MasterpageResponse
        {
            public bool IsSessionEnabled { get; set; }
            public string? RedirectURL { get; set; }
            public string? CurrentURL { get; set; }
        }
        public MasterpageResponse MasterPage_Code(MasterpageResponse aResponse)
        {
            MasterpageResponse Ret = new MasterpageResponse();
            Ret.IsSessionEnabled = true;
            var _publicClaimObjects = _HttpContextAccessor.HttpContext!.Session.GetObject<PublicClaimObjects>("PublicClaimObjects");
            string? CurrentUserName = _publicClaimObjects?.username;
            if (string.IsNullOrEmpty(CurrentUserName))
            {
                UserLogic_Security.SignInAjax();
                _publicClaimObjects = _HttpContextAccessor.HttpContext.Session.GetObject<PublicClaimObjects>("PublicClaimObjects");
                CurrentUserName = _publicClaimObjects?.username;
            }

            string CurrentURL = StaticPublicObjects.ado.GetRequestPath();
            if (string.IsNullOrEmpty(CurrentUserName))
            {
                Ret.IsSessionEnabled = false;
                Ret.CurrentURL = CurrentURL;
                Ret.RedirectURL = "~/Account/Login?RedirectURL=" + Crypto.EncryptString(CurrentURL, true);
                return Ret;
            }
            string URL;
            URL = Crypto.EncryptString(CurrentURL, true);
            string Encryptedappenv = Crypto.EncryptString(StaticPublicObjects.ado.GetApplicationEnvironment(), true);
            string sso = StaticPublicObjects.ado.GetIsSingleSignOn() ? "sso" : "";
            string appid = AppEnum.ApplicationId.CareerPortalAppID.ToString();

            _HttpContextAccessor.HttpContext.Response.Cookies.Append(sso + "URLCookie" + appid + Encryptedappenv + Crypto.EncryptString(CurrentUserName, true), URL, new CookieOptions { HttpOnly = true, Secure = true, Expires = DateTime.UtcNow.AddDays(1) });

            return Ret;
        }

    }
}

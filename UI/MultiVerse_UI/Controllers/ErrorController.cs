using Data.DataAccess;
using Data.Dtos;
using Microsoft.AspNetCore.Mvc;

namespace MultiVerse_UI.Controllers
{
    public class ErrorController : Controller
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private PublicClaimObjects? _PublicClaimObjects
        {
            get
            {
                return StaticPublicObjects.ado.GetPublicClaimObjects();
            }
        }
        public ErrorController(IHttpContextAccessor httpContextAccessor)
        {
            this._httpContextAccessor = httpContextAccessor;
        }
        public IActionResult Index()
        {
            try
            {
                if (_httpContextAccessor?.HttpContext.Session.GetString("FileGUID") == null)
                {
                    _httpContextAccessor?.HttpContext.Session.SetStringValue("FileGUID", Guid.NewGuid().ToString().ToLower());
                }
                ViewBag.GUID = _httpContextAccessor?.HttpContext?.Session.GetStringValue("FileGUID");
            }
            catch (Exception ex)
            {
                ViewBag.GUID = Guid.NewGuid().ToString().ToLower();
                StaticPublicObjects.logFile.ErrorLog(FunctionName: "Error Index", SmallMessage: ex.Message, Message: ex.ToString());
            }
            int StatusCode = 500;
            string id = "";
            string errorpagename = "";
            string previouspageurl = "";
            string previouspagename = "";
            Exception? exception = null;
            string errormsg = "Internal Server Error";
            try
            {
                if (_httpContextAccessor?.HttpContext?.Response?.StatusCode == 404)
                {
                    string Encryptedappenv = Crypto.EncryptString(StaticPublicObjects.ado.GetApplicationEnvironment(), true);
                    string sso = StaticPublicObjects.ado.GetIsSingleSignOn() ? "sso" : "";
                    string appid = AppEnum.ApplicationId.CareerPortalAppID.ToString();

                    string? url = Request.Cookies[sso + "URLCookie" + appid + Encryptedappenv + Crypto.EncryptString(_PublicClaimObjects?.username, true)];
                    if (url != null)
                        url = Crypto.DecryptString(url, true);
                    else
                        url = "/Account/Home";
                    StatusCode = 404;
                    errormsg = "Page Not Found";
                    errorpagename = "Not Found";
                    previouspageurl = url;
                    previouspagename = url;
                }
            }
            catch (Exception ex)
            {
            }
            try
            {
                id = _httpContextAccessor?.HttpContext?.Request?.Query["ID"].ToString() ?? "";

                if (id != "")
                {

                    errorpagename = _httpContextAccessor?.HttpContext?.Session.GetStringValue("ErrorPageName" + id) ?? "";
                    _httpContextAccessor?.HttpContext?.Session.Remove("ErrorPageName" + id);
                    errorpagename = errorpagename == "" ? "Error" : errorpagename;

                    previouspageurl = _httpContextAccessor?.HttpContext?.Session.GetStringValue("PreviousUrl" + id) ?? "";
                    _httpContextAccessor?.HttpContext?.Session.Remove("PreviousUrl" + id);
                    previouspagename = _httpContextAccessor?.HttpContext?.Session.GetStringValue("PreviousName" + id) ?? "";
                    _httpContextAccessor?.HttpContext?.Session.Remove("PreviousName" + id);
                    exception = _httpContextAccessor?.HttpContext?.Session.GetObject<Exception>("Exception" + id);
                    _httpContextAccessor?.HttpContext?.Session.Remove("Exception" + id);

                    if (exception != null)
                    {
                        errormsg = exception.Message;
                        if (errormsg == "Internal Server Error" && errorpagename == "Error")
                            errorpagename = "Server Error";

                        //_httpContextAccessor?.HttpContext?.Session.Remove("Exception" + id);
                    }
                }
            }
            catch (Exception ex)
            {
                StaticPublicObjects.logFile.ErrorLog(FunctionName: "Error Index 1", SmallMessage: ex.Message, Message: ex.ToString());
            }
            ViewBag.ErrorPageName = errorpagename;
            ViewBag.PreviousPageURL = previouspageurl;
            ViewBag.PreviousPageName = previouspagename;
            ViewBag.ErrorMsg = errormsg;
            return View();
        }
    }
}

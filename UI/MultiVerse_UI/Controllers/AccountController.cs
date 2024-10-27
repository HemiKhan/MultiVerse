using Data.DataAccess;
using Data.Dtos;
using Data.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Services.AccountServices;

namespace MultiVerse_UI.Controllers
{
    public class AccountController : Controller
    {
        #region Constructor
        private IConfiguration _config;
        private IHttpContextAccessor _httpContextAccessor;
        private readonly bool issinglesignon;
        private PublicClaimObjects? _PublicClaimObjects;
        private readonly string _bodystring = "";
        private readonly IAccountService srv;
        public AccountController(IConfiguration config, IHttpContextAccessor httpContextAccessor, IAccountService srv)
        {
            this._config = config;
            this._httpContextAccessor = httpContextAccessor;
            this._bodystring = StaticPublicObjects.ado.GetRequestBodyString().Result;
            this.issinglesignon = StaticPublicObjects.ado.GetIsSingleSignOn();
            this._PublicClaimObjects = StaticPublicObjects.ado.GetPublicClaimObjects();
            this.srv = srv;
        }
        #endregion Constructor

        #region Home
        //[CustomPageSetupAttribute]
        public IActionResult Home()
        {
            return View();
        }
        #endregion Home

        #region Login
        public IActionResult Login()
        {
            string GUID_ = Guid.NewGuid().ToString().ToLower();
            ViewBag.GUID = GUID_;
            return View();
        }
        [HttpPost]
        public IActionResult Login([FromBody] LoginRequestDto req)
        {
            try
            {
                string ReturnURL = "";
                P_ReturnMessage_Result response = srv.SignIn(req);
                if (response.ReturnCode)
                {
                    PublicClaimObjects _PublicClaimObjectsNew = new PublicClaimObjects();
                    _PublicClaimObjectsNew.username = req.UserID;
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
                    HttpContext.Session.SetObject<PublicClaimObjects>("PublicClaimObjects", _PublicClaimObjectsNew);

                    string GUID_ = Guid.NewGuid().ToString().ToLower();
                    HttpContext.Session.SetStringValue("FileGUID", GUID_);
                    ViewBag.GUID = GUID_;

                    ReturnURL = "/PatientReport/Index";
                }
                else
                {
                    ReturnURL = "/Account/Login";
                }
                return Content(JsonConvert.SerializeObject(ReturnURL));
            }
            catch (Exception ex)
            {
                StaticPublicObjects.logFile.ErrorLog(FunctionName: "Login", SmallMessage: ex.Message, Message: ex.ToString());
                throw new Exception("Internal Server Error");
            }
        }
        #endregion Login

        #region Logout
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return Redirect("/Account/Login");
        }
        #endregion Logout
    }
}

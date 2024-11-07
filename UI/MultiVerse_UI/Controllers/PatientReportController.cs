using MultiVerse_UI.Models;
using Data.DataAccess;
using Data.Dtos;
using Data.Models;
using Microsoft.AspNetCore.Mvc;
using Services.PatientReportServices;
using static Data.Dtos.CustomClasses;

namespace MultiVerse_UI.Controllers
{
    public class PatientReportController : Controller
    {
        #region Constructor
        private IConfiguration _config;
        private IHttpContextAccessor _httpContextAccessor;
        private readonly bool issinglesignon;
        private PublicClaimObjects? _PublicClaimObjects;
        private readonly string _bodystring = "";
        private readonly IPatientReportService srv;
        public PatientReportController(IConfiguration config, IHttpContextAccessor httpContextAccessor, IPatientReportService srv)
        {
            this._config = config;
            this._httpContextAccessor = httpContextAccessor;
            this._bodystring = StaticPublicObjects.ado.GetRequestBodyString().Result;
            this.issinglesignon = StaticPublicObjects.ado.GetIsSingleSignOn();
            this._PublicClaimObjects = StaticPublicObjects.ado.GetPublicClaimObjects();
            this.srv = srv;
        }
        #endregion Constructor

        public IActionResult Index()
        {
            ViewBag.UserName = _PublicClaimObjects.username;
            ViewBag.GUID = Guid.NewGuid().ToString().ToLower();
            return View();
        }

        [HttpPost]
        public IActionResult GetFilterData_PatientReport_List([FromBody] ReportParams _ReportParams)
        {
            ReportResponse reportResponse = new ReportResponse();
            try
            {
                List<Dynamic_SP_Params> List_Dynamic_SP_Params = new List<Dynamic_SP_Params>();
                ModalFunctions.GetKendoFilter(ref _ReportParams, ref List_Dynamic_SP_Params, _PublicClaimObjects, true);

                List<P_Get_PatientReport_List> ResultList = StaticPublicObjects.ado.P_Get_Generic_List_SP<P_Get_PatientReport_List>("P_Get_PatientReport_List", ref List_Dynamic_SP_Params);

                reportResponse.TotalRowCount = ModalFunctions.GetValueFromReturnParameter<long>(List_Dynamic_SP_Params, "TotalRowCount", typeof(long));
                reportResponse.ResultData = ResultList;
                reportResponse.response_code = true;
            }
            catch (Exception ex)
            {
                StaticPublicObjects.logFile.ErrorLog(FunctionName: "GetFilterData_PatientReport_List", SmallMessage: ex.Message, Message: ex.ToString());
                reportResponse.TotalRowCount = 0;
                reportResponse.ResultData = null;
                reportResponse.response_code = false;
            }
            return Globals.GetAjaxJsonReturn(reportResponse);
        }
        [HttpPost]
        public IActionResult GetFilterData_ReportTemplate_List([FromBody] ReportParams _ReportParams)
        {
            ReportResponse reportResponse = new ReportResponse();
            try
            {
                List<Dynamic_SP_Params> List_Dynamic_SP_Params = new List<Dynamic_SP_Params>();
                ModalFunctions.GetKendoFilter(ref _ReportParams, ref List_Dynamic_SP_Params, _PublicClaimObjects, true);

                List<P_Get_PatientReport_List> ResultList = StaticPublicObjects.ado.P_Get_Generic_List_SP<P_Get_PatientReport_List>("P_Get_ReportTemplate_List", ref List_Dynamic_SP_Params);

                reportResponse.TotalRowCount = ModalFunctions.GetValueFromReturnParameter<long>(List_Dynamic_SP_Params, "TotalRowCount", typeof(long));
                reportResponse.ResultData = ResultList;
                reportResponse.response_code = true;
            }
            catch (Exception ex)
            {
                StaticPublicObjects.logFile.ErrorLog(FunctionName: "GetFilterData_ReportTemplate_List", SmallMessage: ex.Message, Message: ex.ToString());
                reportResponse.TotalRowCount = 0;
                reportResponse.ResultData = null;
                reportResponse.response_code = false;
            }
            return Globals.GetAjaxJsonReturn(reportResponse);
        }
    }
}

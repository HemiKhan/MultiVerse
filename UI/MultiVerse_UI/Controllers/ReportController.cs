using MultiVerse_UI.Models;
using Data.DataAccess;
using Data.Dtos;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualBasic;
using Newtonsoft.Json;
using System.Data;
using excel = OfficeOpenXml;
using static Data.Dtos.AppEnum;
using static Data.Dtos.CustomClasses;

namespace MultiVerse_UI.Controllers
{
    public class ReportController : Controller
    {
        private IWebHostEnvironment _env;
        private IHttpContextAccessor _httpContextAccessor;
        private PublicClaimObjects? _PublicClaimObjects
        {
            get
            {
                return StaticPublicObjects.ado.GetPublicClaimObjects();
            }
        }
        private readonly string _bodystring = "";
        public ReportController(IWebHostEnvironment env, IHttpContextAccessor httpContextAccessor)
        {
            this._env = env;
            this._httpContextAccessor = httpContextAccessor;
            this._bodystring = StaticPublicObjects.ado.GetRequestBodyString().Result;
        }
        public IActionResult Index()
        {
            return View();
        }

        #region Grid Report Template
        [HttpPost]
        public IActionResult GetReportTemplateModal(string GRGUID, string UGRCGUID)
        {
            GRGUID = GRGUID ?? "";
            UGRCGUID = UGRCGUID ?? "";
            List<Dynamic_SP_Params> List_Dynamic_SP_Params = new List<Dynamic_SP_Params>();
            Dynamic_SP_Params Dynamic_SP_Params = new Dynamic_SP_Params();

            Dynamic_SP_Params = new Dynamic_SP_Params();
            Dynamic_SP_Params.ParameterName = "GRL_ID";
            Dynamic_SP_Params.Val = 0;
            List_Dynamic_SP_Params.Add(Dynamic_SP_Params);

            Dynamic_SP_Params = new Dynamic_SP_Params();
            Dynamic_SP_Params.ParameterName = "UGRTL_ID";
            Dynamic_SP_Params.Val = 0;
            List_Dynamic_SP_Params.Add(Dynamic_SP_Params);

            Dynamic_SP_Params = new Dynamic_SP_Params();
            Dynamic_SP_Params.ParameterName = "GRGUID";
            Dynamic_SP_Params.Val = (UGRCGUID == "" ? GRGUID : "");
            List_Dynamic_SP_Params.Add(Dynamic_SP_Params);

            Dynamic_SP_Params = new Dynamic_SP_Params();
            Dynamic_SP_Params.ParameterName = "UGRCGUID";
            Dynamic_SP_Params.Val = UGRCGUID;
            List_Dynamic_SP_Params.Add(Dynamic_SP_Params);

            Dynamic_SP_Params = new Dynamic_SP_Params();
            Dynamic_SP_Params.ParameterName = "Username";
            Dynamic_SP_Params.Val = _PublicClaimObjects.username;
            List_Dynamic_SP_Params.Add(Dynamic_SP_Params);

            //Dynamic_SP_Params = new Dynamic_SP_Params();
            //Dynamic_SP_Params.ParameterName = "UserType_MTV_CODE";
            //Dynamic_SP_Params.Val = _PublicClaimObjects.P_Get_User_Info_Class.UserTypeMTVCode;
            //List_Dynamic_SP_Params.Add(Dynamic_SP_Params);

            DataTable DT = StaticPublicObjects.ado.ExecuteStoreProcedureDT("P_Get_User_Grid_Report_Columns_List", ref List_Dynamic_SP_Params, true);
            int UGRTL_ID = 0;
            int GRLID = 0;

            if (DT.Rows.Count == 0)
                return Globals.GetAjaxJsonReturn(Json(new { result = "" }));

            if (UGRCGUID != "")
                UGRTL_ID = Convert.ToInt32(DT.Rows[0]["UGRTL_ID"]);

            GRLID = Convert.ToInt32(DT.Rows[0]["GRLID"]);

            string htmlstring = "";

            htmlstring += "<div class=\"modal-dialog modal-md modal-dialog-scrollable\">";
            htmlstring += "<div class=\"modal-content\">";

            {
                htmlstring += "<div class=\"modal-header modal-colored-header Theme-Header\">";
                htmlstring += "<h4 class=\"modal-title\" id=\"title_modal\">Add New Master Type</h4>";
                htmlstring += "<button type=\"button\" class=\"btn-close\" data-bs-dismiss=\"modal\" onclick=\"resetmodal('dynamic-modal1');\" aria-label=\"Close\"></button>";
                htmlstring += "</div>";
            }

            {
                htmlstring += "<div class=\"modal-body col-lg-12 col-md-12 col-sm-12 col-xs-12\">";
                {
                    htmlstring += "<div class=\"form-group\">";
                    {
                        htmlstring += "<div class=\"col-lg-12 col-md-12 col-sm-12 col-xs-12 row\">";
                        {
                            {
                                htmlstring += "<div class=\"col-lg-4 col-md-5 col-sm-12 col-xs-12\">";
                                htmlstring += "<label>Report Name: <span style=\"color: red\"> *</span></label>";
                                htmlstring += "</div>";
                            }
                            {
                                htmlstring += "<div class=\"col-lg-8 col-md-7 col-sm-12 col-xs-12\">";
                                htmlstring += $"<input type=\"text\" id=\"ReportName\" autocomplete=\"off\" class=\"form-control w-100\" placeholder=\"Enter Report Template Name\" value =\"{(UGRTL_ID == 0 ? "" : DT.Rows[0]["UGRCName"].ToString())}\" name =\"ReportName\">";
                                htmlstring += "</div>";
                            }
                        }
                        htmlstring += "</div>";
                    }
                    {
                        htmlstring += "<div class=\"col-lg-12 col-md-12 col-sm-12 col-xs-12\">";
                        {
                            htmlstring += "<table style=\"border: 0;\">";
                            {
                                htmlstring += "<tbody>";
                                {
                                    htmlstring += "<tr>";
                                    {
                                        htmlstring += "<td>";
                                        htmlstring += "<select id=\"ddl_a_coulmn\" size=\"25\" style=\"width: 200px;height: 400px\" multiple=\"multiple\">";
                                        for (int i = 0; i <= DT.Rows.Count - 1; i++)
                                        {
                                            if (Convert.ToBoolean(DT.Rows[i]["IsHidden"]) == false && Convert.ToBoolean(DT.Rows[i]["IsChecked"]) == false)
                                            {
                                                htmlstring += $"<option value=\"{DT.Rows[i]["GRCID"].ToString()}\">{DT.Rows[i]["Name"].ToString()}</option>";
                                            }
                                        }
                                        htmlstring += "</select>";
                                        htmlstring += "</td>";
                                    }
                                    {
                                        htmlstring += "<td>";
                                        {
                                            htmlstring += "<table>";
                                            {
                                                htmlstring += "<tbody>";
                                                {
                                                    htmlstring += "<tr>";
                                                    htmlstring += "<td>";
                                                    htmlstring += "<img style=\"cursor: pointer\" src=\"/img/next.png\" class=\"hnd\" title=\"Add Column\" onclick=\"AddColumn();\">";
                                                    htmlstring += "</td>";
                                                    htmlstring += "</tr>";
                                                }
                                                {
                                                    htmlstring += "<tr>";
                                                    htmlstring += "<td>";
                                                    htmlstring += "<img style=\"cursor: pointer\" src=\"/img/previous.png\" class=\"hnd\" title=\"Remove Column\" onclick=\"RemoveColumn();\">";
                                                    htmlstring += "</td>";
                                                    htmlstring += "</tr>";
                                                }
                                                htmlstring += "</tbody>";
                                            }
                                            htmlstring += "</table>";
                                        }
                                        htmlstring += "</td>";
                                    }
                                    {
                                        htmlstring += "<td>";
                                        htmlstring += "<select id=\"ddl_sel_coulmn\" size=\"5\" style=\"width: 200px;height: 400px\" multiple=\"multiple\">";
                                        for (int i = 0; i <= DT.Rows.Count - 1; i++)
                                        {
                                            if (Convert.ToBoolean(DT.Rows[i]["IsChecked"]))
                                            {
                                                htmlstring += $"<option canremoved=\"{!Convert.ToBoolean(DT.Rows[i]["IsHidden"])}\" value=\"{DT.Rows[i]["GRCID"].ToString()}\">{DT.Rows[i]["Name"].ToString()}</option>";
                                            }
                                        }
                                        htmlstring += "</select>";
                                        htmlstring += "</td>";
                                    }
                                    {
                                        htmlstring += "<td>";
                                        {
                                            htmlstring += "<table>";
                                            {
                                                htmlstring += "<tbody>";
                                                {
                                                    htmlstring += "<tr>";
                                                    htmlstring += "<td>";
                                                    htmlstring += "<img style=\"cursor: pointer\" src=\"/img/up.png\" class=\"hnd\" onclick=\"ColumnUp()\">";
                                                    htmlstring += "</td>";
                                                    htmlstring += "</tr>";
                                                }
                                                {
                                                    htmlstring += "<tr>";
                                                    htmlstring += "<td>";
                                                    htmlstring += "<img style=\"cursor: pointer\" src=\"/img/down.png\" class=\"hnd\" onclick=\"ColumnDown()\">";
                                                    htmlstring += "</td>";
                                                    htmlstring += "</tr>";
                                                }
                                                htmlstring += "</tbody>";
                                            }
                                            htmlstring += "</table>";
                                        }
                                        htmlstring += "</td>";
                                    }
                                    htmlstring += "</tr>";
                                }
                                htmlstring += "</tbody>";
                            }
                            htmlstring += "</table>";
                        }
                        htmlstring += "</div>";
                    }
                    htmlstring += "</div>";
                }
                htmlstring += "</div>";
            }

            {
                htmlstring += "<div class=\"modal-footer\">";
                htmlstring += "<button type=\"button\" class=\"btn btn-light-danger text-danger font-weight-medium\" data-bs-dismiss=\"modal\" onclick=\"resetmodal('dynamic-modal1');\">Cancel</button>";
                htmlstring += $"<button type=\"button\" class=\"btn Theme-button font-weight-medium\" onclick=\"AddUpdateReportTemplate(this,'{Crypto.EncryptNumericToStringWithOutNull(GRLID)}','{Crypto.EncryptNumericToStringWithOutNull(UGRTL_ID)}');\">{(UGRTL_ID == 0 ? "Add" : "Update")}</button>";
                htmlstring += "</div>";
            }
            htmlstring += "</div>";
            htmlstring += "</div>";

            //return htmlstring;
            return Globals.GetAjaxJsonReturn(Json(new { result = htmlstring }));
        }
        [HttpPost]
        public IActionResult AddUpdateReportTemplate(string GRL_ID, string UGRTL_ID, string Name, string pJson)
        {
            List<Dynamic_SP_Params> List_Dynamic_SP_Params = new List<Dynamic_SP_Params>();
            Dynamic_SP_Params Dynamic_SP_Params = new Dynamic_SP_Params();

            Dynamic_SP_Params = new Dynamic_SP_Params();
            Dynamic_SP_Params.ParameterName = "GRL_ID";
            Dynamic_SP_Params.Val = Crypto.DecryptNumericToStringWithOutNull(GRL_ID);
            List_Dynamic_SP_Params.Add(Dynamic_SP_Params);

            Dynamic_SP_Params = new Dynamic_SP_Params();
            Dynamic_SP_Params.ParameterName = "UGRTL_ID";
            Dynamic_SP_Params.Val = Crypto.DecryptNumericToStringWithOutNull(UGRTL_ID);
            List_Dynamic_SP_Params.Add(Dynamic_SP_Params);

            Dynamic_SP_Params = new Dynamic_SP_Params();
            Dynamic_SP_Params.ParameterName = "Name";
            Dynamic_SP_Params.Val = Name;
            List_Dynamic_SP_Params.Add(Dynamic_SP_Params);

            Dynamic_SP_Params = new Dynamic_SP_Params();
            Dynamic_SP_Params.ParameterName = "Username";
            Dynamic_SP_Params.Val = _PublicClaimObjects.username;
            List_Dynamic_SP_Params.Add(Dynamic_SP_Params);

            Dynamic_SP_Params = new Dynamic_SP_Params();
            Dynamic_SP_Params.ParameterName = "AddedBy";
            Dynamic_SP_Params.Val = _PublicClaimObjects.username;
            List_Dynamic_SP_Params.Add(Dynamic_SP_Params);

            Dynamic_SP_Params = new Dynamic_SP_Params();
            Dynamic_SP_Params.ParameterName = "Json";
            Dynamic_SP_Params.Val = (pJson == null ? null : pJson);// JsonConvert.SerializeObject(pJson));
            List_Dynamic_SP_Params.Add(Dynamic_SP_Params);

            DataRow? DR = StaticPublicObjects.ado.ExecuteStoreProcedureDR("P_Grid_Report_Template_IU", ref List_Dynamic_SP_Params);
            bool _ReturnCode = false;
            string _ReturnText = "";
            string GUID = "";

            if (DR == null)
                _ReturnText = "Error";
            else
            {
                _ReturnCode = Convert.ToBoolean(DR["ReturnCode"]);
                _ReturnText = DR["ReturnText"].ToString();
                GUID = DR["GUID_"].ToString();
            }

            return Globals.GetAjaxJsonReturn(Json(new { ReturnCode = _ReturnCode, ReturnText = _ReturnText, GUID_ = GUID }));
        }
        [HttpPost]
        public IActionResult DeleteReportTemplate(string UGRCGUID)
        {
            List<Dynamic_SP_Params> List_Dynamic_SP_Params = new List<Dynamic_SP_Params>();
            Dynamic_SP_Params Dynamic_SP_Params = new Dynamic_SP_Params();

            Dynamic_SP_Params = new Dynamic_SP_Params();
            Dynamic_SP_Params.ParameterName = "UGRCGUID";
            Dynamic_SP_Params.Val = UGRCGUID;
            List_Dynamic_SP_Params.Add(Dynamic_SP_Params);

            Dynamic_SP_Params = new Dynamic_SP_Params();
            Dynamic_SP_Params.ParameterName = "Username";
            Dynamic_SP_Params.Val = _PublicClaimObjects.username;
            List_Dynamic_SP_Params.Add(Dynamic_SP_Params);

            Dynamic_SP_Params = new Dynamic_SP_Params();
            Dynamic_SP_Params.ParameterName = "AddedBy";
            Dynamic_SP_Params.Val = _PublicClaimObjects.username;
            List_Dynamic_SP_Params.Add(Dynamic_SP_Params);

            DataRow? DR = StaticPublicObjects.ado.ExecuteStoreProcedureDR("P_Grid_Report_Template_Delete", ref List_Dynamic_SP_Params);
            bool _ReturnCode = false;
            string _ReturnText = "";

            if (DR == null)
                _ReturnText = "Error";
            else
            {
                _ReturnCode = Convert.ToBoolean(DR["ReturnCode"]);
                _ReturnText = DR["ReturnText"].ToString();
            }
            return Globals.GetAjaxJsonReturn(Json(new { ReturnCode = _ReturnCode, ReturnText = _ReturnText }));
        }
        #endregion Grid Report Template

        #region ExcelExport_Reports
        public ActionResult Grid_ExportInExcel(string SPName, string FileName, bool IsUserNameSet = true, List<Dynamic_SP_Params>? List_Dynamic_SP_Params = null)
        {
            try
            {
                ReportParams _ReportParams = new ReportParams();
                List<ReportGridColumns> reportGridColumns = new List<ReportGridColumns>();
                var formdata_reportParams = Request.Form["reportParams"];
                var formdata_gridColumns = Request.Form["gridColumns"];
                bool IsAllPages = true;
                if (formdata_reportParams.Count == 0)
                {
                    string ID = "";
                    Exception exception = new Exception("Internal Server Error");
                    ID = HttpContext.Session.SetupSessionError("File Download Error", "", "", exception);
                    return Redirect($"/Error/Index?ID={ID}");
                }
                else
                {
                    _ReportParams = JsonConvert.DeserializeObject<ReportParams>(formdata_reportParams[0]);
                }
                if (formdata_gridColumns.Count != 0)
                {
                    reportGridColumns = JsonConvert.DeserializeObject<List<ReportGridColumns>>(formdata_gridColumns[0]);
                }
                IsAllPages = Convert.ToBoolean(Request.Form["IsAllPages"]);
                if (IsAllPages)
                {
                    _ReportParams.PageIndex = 0;
                    _ReportParams.PageSize = 0;
                }

                if (List_Dynamic_SP_Params == null)
                    List_Dynamic_SP_Params = new List<Dynamic_SP_Params>();
                ModalFunctions.GetKendoFilter(ref _ReportParams, ref List_Dynamic_SP_Params, _PublicClaimObjects, IsUserNameSet);

                DataTable DT = StaticPublicObjects.ado.ExecuteStoreProcedureDT(SPName, ref List_Dynamic_SP_Params);

                if (SPName == "P_CP_Get_Jobs_List")
                {
                    for (int i = 0; i <= DT.Rows.Count - 1; i++)
                    {
                        DT.Rows[i]["JobUrl"] = Crypto.EncryptNumericToStringWithOutNull(Globals.ConvertDBNulltoNullIfExistsInt(DT.Rows[i]["J_ID"]));
                    }
                }

                DataTable ColumnDT = DT.Clone();

                List<SetColumnType> ListsetColumnType = new List<SetColumnType>();

                if (reportGridColumns.Count > 0)
                {
                    bool IsShowInExcel = false;
                    int Position = 0;
                    for (int ii = 0; ii <= reportGridColumns.Count - 1; ii++)
                    {
                        string ColumnName = "";
                        IsShowInExcel = false;
                        for (int i = 0; i <= ColumnDT.Columns.Count - 1; i++)
                        {
                            if (ColumnDT.Columns[i].ColumnName.ToLower() == reportGridColumns[ii].field.ToLower())
                            {
                                ColumnName = ColumnDT.Columns[i].ColumnName;
                                IsShowInExcel = reportGridColumns[ii].showinexcel;
                                if (IsShowInExcel)
                                {
                                    DT.Columns[ColumnName].SetOrdinal(Position);
                                    Position += 1;
                                    if (reportGridColumns[ii].excelcolumntype != ExcelColumnType.General)
                                    {
                                        SetColumnType setColumnType = new SetColumnType();
                                        setColumnType.columnindex = Position;
                                        setColumnType.columnname = DT.Columns[ColumnName].ColumnName;
                                        setColumnType.columntype = reportGridColumns[ii].excelcolumntype;
                                        ListsetColumnType.Add(setColumnType);
                                    }
                                    DT.Columns[ColumnName].ColumnName = reportGridColumns[ii].title;
                                }
                                break;
                            }
                        }
                    }
                    for (int i = 0; i <= ColumnDT.Columns.Count - 1; i++)
                    {
                        IsShowInExcel = false;
                        for (int ii = 0; ii <= reportGridColumns.Count - 1; ii++)
                        {
                            if (ColumnDT.Columns[i].ColumnName.ToLower() == reportGridColumns[ii].field.ToLower() && reportGridColumns[ii].showinexcel)
                                IsShowInExcel = true;
                        }
                        if (IsShowInExcel == false)
                            DT.Columns.Remove(ColumnDT.Columns[i].ColumnName);
                    }
                }

                return ExportToExcel(FileName, "Sheet1", DT, ListsetColumnType);
            }
            catch (Exception ex)
            {
                StaticPublicObjects.logFile.ErrorLog(ParameterDetail: SPName, FunctionName: "Grid_ExportInExcel", SmallMessage: ex.Message, Message: ex.ToString());
                string ID = "";
                Exception exception = new Exception("Unable To Download File");
                ID = HttpContext.Session.SetupSessionError("File Download Error", "", "", exception);
                return Redirect($"/Error/Index?ID={ID}");
            }
        }
        #endregion ExcelExport_Reports

        #region ExcelExport 
        public ActionResult ExportToExcel(string FileName, string SheetName, DataTable DT, List<SetColumnType> setColumnTypes = null, bool IsDeleteExistingFiles = true)
        {
            FileName = FileName.Replace("/", "_").Replace("\\", "_").Replace(",", "-").Replace("&", "and").Replace(":", " ").Replace("*", " ").Replace("?", " ").Replace("\"", " ").Replace("<", " ").Replace(">", " ").Replace("|", " ");
            excel.ExcelPackage pck = new excel.ExcelPackage();

            try
            {
                var ws = pck.Workbook.Worksheets.Add(SheetName);
                ws.Cells["A1"].LoadFromDataTable(DT, true);
                ws.SelectedRange[1, 1, (DT.Rows.Count + 1), (DT.Columns.Count + 1)].AutoFitColumns();
                for (int colno = 1; colno <= ws.Dimension.Columns; colno++)
                {
                    if (ws.Column(colno).Width > 100)
                    {
                        ws.Column(colno).Width = 100;
                    }
                }
                if (setColumnTypes != null)
                {
                    for (int coltype = 0; coltype <= setColumnTypes.Count - 1; coltype++)
                    {
                        try
                        {
                            int columnIndex = setColumnTypes[coltype].columnindex;
                            string columnName = setColumnTypes[coltype].columnname;
                            if (columnIndex == 0 && columnName != "")
                            {
                                columnIndex = ws.Cells["1:1"].First(c => c.Text == columnName).Start.Column;
                            }
                            if (columnIndex != 0)
                            {
                                ws.Column(columnIndex).Style.Numberformat.Format = setColumnTypes[coltype].columntype; // Change the format as needed
                            }
                        }
                        catch (Exception ex1)
                        {
                            StaticPublicObjects.logFile.ErrorLog(ParameterDetail: FileName + " - " + setColumnTypes[coltype].columnname + " - " + " - " + setColumnTypes[coltype].columnindex, FunctionName: "ExportToExcel", SmallMessage: ex1.Message, Message: ex1.ToString());
                        }
                    }
                }

                ws.Cells[1, 1, 1, (DT.Columns.Count + 1)].Style.Font.Bold = true;
                var excel = pck.GetAsByteArray();

                string Folder = Guid.NewGuid().ToString().ToLower();
                if ((_PublicClaimObjects?.username ?? "") != "")
                {
                    Folder = _PublicClaimObjects?.username.ToLower();
                }
                else if (HttpContext.Session.GetStringValue("SGUID") != null)
                {
                    Folder = HttpContext.Session.GetStringValue("SGUID");
                }
                if (HttpContext.Session.GetStringValue("SGUID") == null)
                {
                    HttpContext.Session.SetStringValue("SGUID", Folder);
                }

                FileInfo FileInfo_ = new FileInfo(Path.Combine(_env.WebRootPath, "Data", "Export", Folder, FileName + ".xlsx"));
                if (IsDeleteExistingFiles)
                {
                    if (FileInfo_.Directory.Exists)
                    {
                        FileInfo_.Directory.Delete(true);
                    }
                }
                if (FileInfo_.Directory.Exists == false)
                {
                    FileInfo_.Directory.Create();
                }

                System.IO.File.WriteAllBytes(FileInfo_.ToString(), excel);
                pck.Dispose();

                string filePath = FileInfo_.ToString();

                Response.Clear();
                Response.Headers.Append("content-disposition", ("attachment; filename=" + Path.GetFileName(filePath)));
                MemoryStream memoryStream = new MemoryStream(excel);
                memoryStream.Seek(0, SeekOrigin.Begin);
                return new FileStreamResult(memoryStream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet");

            }
            catch (Exception ex)
            {
                StaticPublicObjects.logFile.ErrorLog(FunctionName: "ExportToExcel", SmallMessage: ex.Message, Message: ex.ToString());
                string ID = "";
                Exception exception = new Exception("Unable To Download File");
                ID = HttpContext.Session.SetupSessionError("File Download Error", "", "", exception);
                return Redirect($"/Error/Index?ID={ID}");
            }
            finally
            {
                //pck.Dispose();
            }
        }
        public ActionResult ExportToExcelDS(string FileName, DataSet data, List<SetColumnType> setColumnTypes = null, bool IsDeleteExistingFiles = true)
        {
            FileName = FileName.Replace("/", "_").Replace("\\", "_").Replace(",", "-").Replace("&", "and").Replace(":", " ").Replace("*", " ").Replace("?", " ").Replace("\"", " ").Replace("<", " ").Replace(">", " ").Replace("|", " ");
            excel.ExcelPackage pck = new excel.ExcelPackage();
            excel.ExcelWorksheet ws;
            try
            {

                for (int i = 0; i <= data.Tables.Count - 1; i++)
                {
                    ws = pck.Workbook.Worksheets.Add("Sheet " + i + 1);
                    ws.Cells["A1"].LoadFromDataTable(data.Tables[i], true);
                    ws.SelectedRange[1, 1, (data.Tables[i].Rows.Count + 1), (data.Tables[i].Columns.Count + 1)].AutoFitColumns();
                    for (int colno = 1; colno <= ws.Dimension.Columns; colno++)
                    {
                        if (ws.Column(colno).Width > 100)
                        {
                            ws.Column(colno).Width = 100;
                        }
                    }
                    if (setColumnTypes != null)
                    {
                        for (int coltype = 0; coltype <= setColumnTypes.Count - 1; coltype++)
                        {
                            try
                            {
                                int columnIndex = setColumnTypes[coltype].columnindex;
                                string columnName = setColumnTypes[coltype].columnname;
                                if (columnIndex == 0 && columnName != "")
                                {
                                    columnIndex = ws.Cells["1:1"].First(c => c.Text == columnName).Start.Column;
                                }
                                if (columnIndex != 0)
                                {
                                    ws.Column(columnIndex).Style.Numberformat.Format = setColumnTypes[coltype].columntype; // Change the format as needed
                                }
                            }
                            catch (Exception ex1)
                            {
                                StaticPublicObjects.logFile.ErrorLog(ParameterDetail: FileName + " - " + setColumnTypes[coltype].columnname + " - " + " - " + setColumnTypes[coltype].columnindex, FunctionName: "ExportToExcelDS", SmallMessage: ex1.Message, Message: ex1.ToString());
                            }
                        }
                    }

                    ws.Cells[1, 1, 1, (data.Tables[i].Columns.Count + 1)].Style.Font.Bold = true;
                }
                var excel = pck.GetAsByteArray();

                string Folder = Guid.NewGuid().ToString().ToLower();
                if ((_PublicClaimObjects?.username ?? "") != "")
                {
                    Folder = _PublicClaimObjects?.username.ToLower();
                }
                else if (HttpContext.Session.GetStringValue("SGUID") != null)
                {
                    Folder = HttpContext.Session.GetStringValue("SGUID");
                }
                if (HttpContext.Session.GetStringValue("SGUID") == null)
                {
                    HttpContext.Session.SetStringValue("SGUID", Folder);
                }

                HttpContext.Session.SetStringValue("SGUID", Folder);
                FileInfo FileInfo_ = new FileInfo(Path.Combine(_env.WebRootPath, "Data", "Export", Folder, FileName + ".xlsx"));
                if (IsDeleteExistingFiles)
                {
                    if (FileInfo_.Directory.Exists)
                    {
                        FileInfo_.Directory.Delete(true);
                    }
                }
                if (FileInfo_.Directory.Exists == false)
                {
                    FileInfo_.Directory.Create();
                }

                System.IO.File.WriteAllBytes(FileInfo_.ToString(), excel);
                pck.Dispose();

                string filePath = FileInfo_.ToString();

                Response.Clear();
                Response.Headers.Append("content-disposition", ("attachment; filename=" + Path.GetFileName(filePath)));
                MemoryStream memoryStream = new MemoryStream(excel);
                memoryStream.Seek(0, SeekOrigin.Begin);
                return new FileStreamResult(memoryStream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet");
            }
            catch (Exception ex)
            {
                StaticPublicObjects.logFile.ErrorLog(FunctionName: "ExportToExcelDS", SmallMessage: ex.Message, Message: ex.ToString());
                string ID = "";
                Exception exception = new Exception("Unable To Download File");
                ID = HttpContext.Session.SetupSessionError("File Download Error", "", "", exception);
                return Redirect($"/Error/Index?ID={ID}");
            }
            finally
            {
                //pck.Dispose();
            }
        }
        public ActionResult ReDownloadExportFile()
        {
            try
            {
                string Folder = Guid.NewGuid().ToString().ToLower();
                if ((_PublicClaimObjects?.username ?? "") != "")
                {
                    Folder = _PublicClaimObjects?.username.ToLower();
                }
                else if (HttpContext.Session.GetStringValue("SGUID") != null)
                {
                    Folder = HttpContext.Session.GetStringValue("SGUID");
                }
                if (HttpContext.Session.GetStringValue("SGUID") == null)
                {
                    HttpContext.Session.SetStringValue("SGUID", Folder);
                }

                HttpContext.Session.SetStringValue("SGUID", Folder);
                FileInfo FileInfo_ = new FileInfo(Path.Combine(_env.WebRootPath, "Data", "Export", Folder, "test" + ".txt"));
                if (FileInfo_.Directory.Exists)
                {
                    for (int i = 0; i <= FileInfo_.Directory.GetFiles().Length - 1; i++)
                    {
                        if (i == FileInfo_.Directory.GetFiles().Length - 1 && (FileInfo_.Directory.GetFiles()[i].CreationTimeUtc >= DateTime.UtcNow.AddDays(-1)))
                        {
                            Response.Clear();
                            Response.ContentType = HttpContext.Response.ContentType;
                            Response.Headers.Append("content-disposition", ("attachment; filename=" + FileInfo_.Directory.GetFiles()[i].Name));
                            HttpContext.Response.WriteAsync(FileInfo_.Directory.GetFiles()[i].FullName);
                            return new EmptyResult();

                        }
                        {
                            FileInfo_.Directory.GetFiles()[i].Delete();
                        }
                    }
                }
                {
                    return new EmptyResult();
                }
            }
            catch (Exception ex)
            {

                StaticPublicObjects.logFile.ErrorLog(FunctionName: "ReDownloadExportFile", SmallMessage: ex.Message, Message: ex.ToString());
                return new EmptyResult();
            }
            finally
            {
            }
        }
        public DownloadLastFile GetDownloadLastFile()
        {
            // Anyother Code in WebFunction.aspx.vb
            DownloadLastFile Ret = new DownloadLastFile();
            Ret.fileext = "";
            Ret.filename = "";
            Ret.isfileavailable = false;
            Ret.filedatetime = "";
            try
            {
                string Folder = Guid.NewGuid().ToString().ToLower();
                if ((_PublicClaimObjects?.username ?? "") != "")
                {
                    Folder = _PublicClaimObjects?.username.ToLower();
                }
                else if (HttpContext.Session.GetStringValue("SGUID") != null)
                {
                    Folder = HttpContext.Session.GetStringValue("SGUID");
                }
                if (HttpContext.Session.GetStringValue("SGUID") == null)
                {
                    HttpContext.Session.SetStringValue("SGUID", Folder);
                }

                FileInfo FileInfo_ = new FileInfo(Path.Combine(_env.WebRootPath, "Data", "Export", Folder, "test" + ".txt"));
                if (FileInfo_.Directory.Exists)
                {
                    for (int i = 0; i <= FileInfo_.Directory.GetFiles().Length - 1; i++)
                    {
                        if (i == FileInfo_.Directory.GetFiles().Length - 1 && (FileInfo_.Directory.GetFiles()[i].CreationTimeUtc >= DateTime.UtcNow.AddDays(-1)))
                        {
                            Ret.filename = Path.GetFileNameWithoutExtension(FileInfo_.Directory.GetFiles()[i].Name);
                            Ret.fileext = FileInfo_.Directory.GetFiles()[i].Extension;
                            Ret.isfileavailable = true;
                            Ret.filedatetime = string.Format(" File Created: {0} UTC; {1} Hours Ago", Strings.Format(FileInfo_.Directory.GetFiles()[i].CreationTimeUtc, "MM/dd/yyyy hh:mm:ss"), Math.Round(((((DateTime.UtcNow - FileInfo_.Directory.GetFiles()[i].CreationTimeUtc).TotalSeconds) / 60) / 60), 2));
                        }


                        {
                            FileInfo_.Directory.GetFiles()[i].Delete();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                StaticPublicObjects.logFile.ErrorLog(FunctionName: "GetDownloadLastFile", SmallMessage: ex.Message, Message: ex.ToString());
                Ret.fileext = "";
                Ret.filename = "";
                Ret.isfileavailable = false;
                Ret.filedatetime = "";
            }
            finally
            {
            }
            return Ret;
        }
        #endregion  ExcelExport

        #region Career
        [HttpPost]
        public ActionResult ExportInExcel_JobDetail()
        {
            return Grid_ExportInExcel("P_CP_Get_Jobs_List", "JobDetail");
        }
        #endregion


    }
}

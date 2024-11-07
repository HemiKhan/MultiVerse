using Data.Dtos;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualBasic;
using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Globalization;
using System.IdentityModel.Tokens.Jwt;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using static Data.Dtos.AppEnum;
using static Data.Dtos.CustomClasses;

namespace Data.DataAccess
{
    public class Globals
    {
        public static int GetRemainingSecondsInDay(DateTime utcdateTime)
        {
            // Calculate the remaining time until the end of the day
            DateTime endOfDay = utcdateTime.Date.AddDays(1); // Start of the next day
            TimeSpan remainingTime = endOfDay - utcdateTime;

            // Return the remaining seconds
            return (int)remainingTime.TotalSeconds;
        }
        public static int GetRemainingSecondsInHour(DateTime utcdateTime)
        {
            // Calculate the remaining time until the end of the day
            DateTime endOfHour = utcdateTime.Date.AddHours(1); // Start of the next day
            TimeSpan remainingTime = endOfHour - utcdateTime;

            // Return the remaining seconds
            return (int)remainingTime.TotalSeconds;
        }
        public static ContentResult GetJsonReturn(object? _Data, int _StatusCode = 200, JsonSerializerSettings? _JsonSerializerSettings = null, bool isexcludefilebase64field = false, string _ContentType = "application/json", bool isoriginalmembername = false)
        {
            return Globals.GetContentResult(Globals.GetResponseJson(_Data, _JsonSerializerSettings, isexcludefilebase64field, isoriginalmembername), _StatusCode, _JsonSerializerSettings, _ContentType);
        }
        public static ContentResult GetAjaxJsonReturn(object? _Data, int _StatusCode = 200, JsonSerializerSettings? _JsonSerializerSettings = null, bool isexcludefilebase64field = false, string _ContentType = "application/json", bool isoriginalmembername = true)
        {
            return Globals.GetContentResult(Globals.GetResponseJson(_Data, _JsonSerializerSettings, isexcludefilebase64field, isoriginalmembername), _StatusCode, _JsonSerializerSettings, _ContentType);
        }
        /// <summary>
        /// Get Content Result
        /// </summary>
        /// <param name="json">Json string.</param>
        /// <param name="_StatusCode">Result StatusCode. Default value 200 OK Response</param>
        /// <param name="_JsonSerializerSettings">JsonSerialization Setting. In case of null it will be consider *CustomContractResolverHideProperty* or CustomContractResolverNone if call is from swaggeradmin.</param>
        /// <param name="_ContentType">Result ContentType. Default value *application/json*</param>
        /// <returns>New ContentResult will be returned.</returns>
        public static ContentResult GetContentResult(string json, int _StatusCode = 200, JsonSerializerSettings? _JsonSerializerSettings = null, string _ContentType = "application/json")
        {
            ContentResult _ContentResult = new ContentResult
            {
                Content = json,
                ContentType = _ContentType,
                StatusCode = _StatusCode
            };
            return _ContentResult;
        }
        /// <summary>
        /// Get Content Result
        /// </summary>
        /// <param name="_Data">Result Object. In case of null it will be consider empty string.</param>
        /// <param name="_JsonSerializerSettings">JsonSerialization Setting. In case of null it will be consider *CustomContractResolverHideProperty* or CustomContractResolverNone if call is from swaggeradmin.</param>
        /// <returns>Response Json Will Be Returned</returns>
        public static string GetResponseJson(object? _Data, JsonSerializerSettings? _JsonSerializerSettings = null, bool isexcludefilebase64field = false, bool isoriginalmembername = false)
        {
            if (_JsonSerializerSettings == null)
            {
                int Type_ = AppEnum.JsonIgnorePropertyType.HideProperty;
                if (StaticPublicObjects.ado.IsSwaggerCallAdmin() || (StaticPublicObjects.ado.IsAllowedDomain() && StaticPublicObjects.ado.IsSwaggerCall() == false))
                {
                    Type_ = AppEnum.JsonIgnorePropertyType.None;
                }
                _JsonSerializerSettings = GetCustomJsonDefaultSetting(Type_, isoriginalmembername, isexcludefilebase64field);
            }

            string json = (_Data == null ? "" : JsonConvert.SerializeObject(_Data, _JsonSerializerSettings));
            return json;
        }
        public static string GetRequestBodyHash(string body)
        {
            // hash the request body to generate a cache key
            // for simplicity, we're using SHA512 here, but you might want to use a stronger hash algorithm
            using (var _SHA512 = SHA512.Create())
            {
                //var serializedBody = JsonConvert.SerializeObject(body);
                var hash = _SHA512.ComputeHash(Encoding.UTF8.GetBytes(body));
                return BitConverter.ToString(hash).Replace("-", "").ToLower();
            }
        }
        public static DateTime? GetTokenExpiryTime(string bearerToken)
        {
            var handler = new JwtSecurityTokenHandler();
            var token = handler.ReadJwtToken(bearerToken);

            // Extract "exp" claim from token's payload
            var expiryTime = token.Claims.FirstOrDefault(c => c.Type == "exp")?.Value;

            if (!string.IsNullOrEmpty(expiryTime) && long.TryParse(expiryTime, out var epochTime))
            {
                // Convert epoch time to DateTime
                return DateTimeOffset.FromUnixTimeSeconds(epochTime).DateTime;
            }

            return null; // Return null if "exp" claim is not found or invalid
        }
        public static List<T> GetObjectListFromDataTable<T>(ref DataTable DT) where T : new()
        {
            List<T> _List = new List<T>();
            T item = new T();
            for (int i = 0; i <= DT.Rows.Count - 1; i++)
            {
                item = new T();
                item = StaticPublicObjects.map.Map<T>(DT.Rows[i]);
                _List.Add(item);
            }
            return _List;
        }
        public static object? ConvertDBNulltoNullIfExists(object? obj)
        {
            object? Ret = obj;
            if (Information.IsDBNull(Ret))
                Ret = null;
            return Ret;
        }
        public static string? ConvertDBNulltoNullIfExistsString(object? obj)
        {
            string? Ret = null;
            if (Information.IsDBNull(obj))
                obj = null;
            if (obj != null)
                Ret = obj.ToString();

            return Ret;
        }
        public static int? ConvertDBNulltoNullIfExistsInt(object? obj)
        {
            int? Ret = null;
            if (Information.IsDBNull(obj))
                obj = null;
            if (obj != null)
            {
                if (obj.ToString() != "")
                    Ret = Convert.ToInt32(Convert.ToDouble(obj));
            }

            return Ret;
        }
        public static double? ConvertDBNulltoNullIfExistsDouble(object? obj)
        {
            double? Ret = null;
            if (Information.IsDBNull(obj))
                obj = null;
            if (obj != null)
            {
                if (obj.ToString() != "")
                    Ret = Convert.ToDouble(obj);
            }

            return Ret;
        }
        public static bool? ConvertDBNulltoNullIfExistsBool(object? obj)
        {
            bool? Ret = null;
            if (Information.IsDBNull(obj))
                obj = null;
            if (obj != null)
            {
                if (obj.ToString() != "")
                    Ret = Convert.ToBoolean(obj);
            }

            return Ret;
        }
        public static string? ConvertDBNulltoNullIfExistsDate(object? obj, bool isyearfirst = false)
        {
            string? Ret = null;
            if (Information.IsDBNull(obj))
                obj = null;
            if (obj != null)
            {
                if (obj.ToString() != "")
                {
                    if (Information.IsDate(obj))
                    {
                        DateTime dateTime = Convert.ToDateTime(obj);
                        if (dateTime.Year <= 2000)
                        {
                            return Ret;
                        }
                        if (isyearfirst)
                            Ret = dateTime.ToString("yyyy-MM-dd");
                        else
                            Ret = dateTime.ToString("MM-dd-yyyy");
                    }
                }
            }

            return Ret;
        }
        public static string? ConvertDBNulltoNullIfExistsDateTime(object? obj, bool isyearfirst = false)
        {
            string? Ret = null;
            if (Information.IsDBNull(obj))
                obj = null;
            if (obj != null)
            {
                if (obj.ToString() != "")
                {
                    if (Information.IsDate(obj) == false && obj.ToString().Length >= 20)
                    {
                        obj = Strings.Left(obj.ToString(), obj.ToString().Length - 3).Trim().ToString();
                    }
                    DateTime dateTime = Convert.ToDateTime(obj);
                    if (dateTime.Year <= 2000)
                    {
                        return Ret;
                    }
                    if (isyearfirst)
                        Ret = dateTime.ToString("yyyy-MM-dd hh:mm:ss tt");
                    else
                        Ret = dateTime.ToString("MM-dd-yyyy hh:mm:ss tt");
                }
            }
            return Ret;
        }
        public static string? ConvertDBNulltoNullIfExists12HourTime(object? obj)
        {
            string? Ret = null;
            if (Information.IsDBNull(obj))
                obj = null;
            if (obj != null)
            {
                if (obj.ToString() != "")
                    Ret = Convert.ToDateTime(obj).ToString("hh:mm tt");
            }

            return Ret;
        }
        public static string ConvertNulltoEmptyString(string? obj)
        {
            string Ret = "";
            if (obj != null)
                Ret = obj.ToString();

            return Ret;
        }
        public static bool IsAnyListValueExists(string val, List<string>? listval)
        {
            bool result = false;
            if (val != "" && listval != null)
            {
                for (int i = 0; i <= listval.Count - 1; i++)
                {
                    if (val.ToLower().Contains(listval[i].ToLower()))
                        return result = true;
                }
            }
            return result;
        }
        public static List<string>? GetListofStringsFromString(List<string>? ReturnListString, string addstring, string separater)
        {
            List<string>? Ret = ReturnListString;
            if (addstring != "")
            {
                Ret = (Ret == null ? new List<string>() : Ret);
                List<string> manualPOMStext = new List<string>();
                manualPOMStext = addstring.Split(separater).ToList();
                for (int az = 0; az < manualPOMStext.Count; az++)
                    Ret.Add(manualPOMStext[az].Trim());
            }
            return Ret;
        }
        public static List<string>? GetListofStringsFromString(object? addstring, string separater)
        {
            List<string>? Ret = new List<string>();
            addstring = (Information.IsDBNull(addstring) ? null : addstring);
            if (addstring == null)
                Ret = null;
            else if (addstring.ToString() == "")
                Ret = null;
            else if (addstring.ToString() != "")
            {
                List<string>? TempList = new List<string>();
                TempList = addstring.ToString().Split(separater).ToList();
                foreach (string TempString in TempList)
                    Ret.Add(TempString.Trim());
            }
            return Ret;
        }
        public static DateTime GetCurrentUTCDateTimeFromEST(string ESTDateTime)
        {
            return TimeZoneInfo.ConvertTime(Convert.ToDateTime(ESTDateTime), TimeZoneInfo.FindSystemTimeZoneById("Eastern Standard Time"), TimeZoneInfo.FindSystemTimeZoneById("UTC"));
        }
        public static DateTime GetCurrentESTDateTime()
        {
            return TimeZoneInfo.ConvertTime(DateTime.UtcNow, TimeZoneInfo.FindSystemTimeZoneById("UTC"), TimeZoneInfo.FindSystemTimeZoneById("Eastern Standard Time"));
        }
        public static string GetCurrentESTDateTimeToString(bool IsIncludeTime)
        {
            DateTime Ret = GetCurrentESTDateTime();
            if (IsIncludeTime)
                return Ret.ToString("yyyy-MM-dd HH:mm:ss.fff");
            else
                return Ret.ToString("yyyy-MM-dd");
        }
        public static JsonSerializerSettings GetCustomJsonDefaultSetting(int Type_, bool isoriginalmembername, bool isexcludefilebase64field)
        {
            JsonSerializerSettings settings = new JsonSerializerSettings();
            if (Type_ == JsonIgnorePropertyType.Standard)
                settings.ContractResolver = new CustomContractResolverStandard(true, isoriginalmembername, isexcludefilebase64field);
            else if (Type_ == JsonIgnorePropertyType.None)
                settings.ContractResolver = new CustomContractResolverNone(isoriginalmembername, isexcludefilebase64field);
            else if (Type_ == JsonIgnorePropertyType.HideProperty)
                settings.ContractResolver = new CustomContractResolverHideProperty(false, isoriginalmembername, isexcludefilebase64field);
            return settings;
        }
        public static double GetMiles(double oLat, double oLon, double dLat, double dLon)
        {
            double Miles;

            double SinDLat;
            double SinOLat;
            double CosDLat;
            double CosOLat;
            double CosOLon;
            double CosDLon;

            SinDLat = Math.Sin(Radians(dLat));
            SinOLat = Math.Sin(Radians(oLat));
            CosDLat = Math.Cos(Radians(dLat));
            CosOLat = Math.Cos(Radians(oLat));
            CosOLon = Radians(oLon);
            CosDLon = Radians(dLon);

            Miles = Math.Acos((SinDLat * SinOLat) + ((CosDLat * CosOLat) * Math.Cos(CosOLon - CosDLon))) * 3959;
            return Miles;
        }
        public static object? GetValueFromReturnFieldDynamicCondition(List<Dynamic_SP_Params> Dynamic_SP_Params_List, string fieldname, bool IsReturnParam = true)
        {
            object? obj = "";
            if (Dynamic_SP_Params_List != null)
            {
                if (Dynamic_SP_Params_List.Count > 0)
                {
                    for (int i = 0; i <= Dynamic_SP_Params_List.Count - 1; i++)
                    {
                        if (Dynamic_SP_Params_List[i].ParameterName == fieldname && IsReturnParam == true && Dynamic_SP_Params_List[i].IsInputType == false)
                        {
                            obj = Dynamic_SP_Params_List[i].Val;
                            break;
                        }
                    }
                }
            }

            return obj;
        }
        private static double Radians(double v)
        {
            double pi;
            pi = 22 / 7.00;
            return (v * pi) / 180.00;
        }
        public static string GetStringJoin(List<string>? strings, string joiner)
        {
            string Ret = "";
            if (strings != null)
            {
                List<string> NewList = new List<string>();
                for (int i = 0; i <= strings.Count - 1; i++)
                {
                    if (strings[i].Trim() != "")
                        NewList.Add(strings[i].Trim());
                }
                if (NewList.Count > 0)
                    Ret = string.Join(joiner, NewList);
            }
            return Ret;
        }
        public static string GetStringJoin(string[]? strings, string joiner)
        {
            string Ret = "";
            if (strings != null)
            {
                List<string> NewList = new List<string>();
                for (int i = 0; i <= strings.Length - 1; i++)
                {
                    if (strings[i].Trim() != "")
                        NewList.Add(strings[i].Trim());
                }
                if (NewList.Count > 0)
                    Ret = string.Join(joiner, NewList);
            }
            return Ret;
        }
        public static bool IsPasswordValid(string password, int passwordlength)
        {
            // Define regular expressions for each criterion
            var hasCapitalLetter = new Regex(@"[A-Z]").IsMatch(password);
            var hasSmallLetter = new Regex(@"[a-z]").IsMatch(password);
            var hasSpecialCharacter = new Regex(@"[!@#$%^&*()_+{}\[\]:;<>,.?~\\/\-=]").IsMatch(password);
            var hasNumber = new Regex(@"\d").IsMatch(password);
            var haslength = password.Length < passwordlength ? false : true;

            // Check if all criteria are met
            return hasCapitalLetter && hasSmallLetter && hasSpecialCharacter && hasNumber && haslength;
        }
        public static bool IsValidEmail(string originalemail)
        {
            bool isvalidemail = false;
            if (string.IsNullOrWhiteSpace(originalemail))
                return false;

            try
            {
                string[] emailobj;
                emailobj = originalemail.Split(';');
                // Normalize the domain
                foreach (string s in emailobj)
                {
                    string email = s;
                    email = Regex.Replace(email, @"(@)(.+)$", DomainMapper,
                                      RegexOptions.None, TimeSpan.FromMilliseconds(200));

                    // Examines the domain part of the email and normalizes it.
                    string DomainMapper(Match match)
                    {
                        // Use IdnMapping class to convert Unicode domain names.
                        var idn = new IdnMapping();

                        // Pull out and process domain name (throws ArgumentException on invalid)
                        string domainName = idn.GetAscii(match.Groups[2].Value);

                        return match.Groups[1].Value + domainName;
                    }
                    isvalidemail = Regex.IsMatch(email,
                    @"^[^@\s]+@[^@\s]+\.[^@\s]+$",
                    RegexOptions.IgnoreCase, TimeSpan.FromMilliseconds(250));
                    if (isvalidemail == false)
                        break;
                }
            }
            catch (RegexMatchTimeoutException e)
            {
                StaticPublicObjects.logFile.ErrorLog(FunctionName: "IsValidEmail", SmallMessage: e.Message, Message: e.ToString());
                return false;
            }
            catch (ArgumentException e)
            {
                StaticPublicObjects.logFile.ErrorLog(FunctionName: "IsValidEmail 1", SmallMessage: e.Message, Message: e.ToString());
                return false;
            }

            try
            {
                return isvalidemail;
            }
            catch (RegexMatchTimeoutException e)
            {
                StaticPublicObjects.logFile.ErrorLog(FunctionName: "IsValidEmail 2", SmallMessage: e.Message, Message: e.ToString());
                return false;
            }
        }
        public static void SetReportFilterClause(ref ReportParams reportParams)
        {
            string FilterClause = " ";
            for (int i = 0; i <= reportParams.ReportFilterObjectList.Count - 1; i++)
            {
                List<SQLReportFilterObjectArry> ListSQLReportFilterObjectArry = new List<SQLReportFilterObjectArry>();
                string TempFilterClause = " ";
                for (int z = 0; z <= reportParams.ReportFilterObjectList[i].reportFilterObjectArry.Count - 1; z++)
                {
                    string setCode = reportParams.ReportFilterObjectList[i].Code;
                    if (z > 0)
                    {
                        setCode = reportParams.ReportFilterObjectList[i].reportFilterObjectArry[z].Code;
                    }
                    SQLReportFilterObjectArry sQLReportFilterObjectArry = new SQLReportFilterObjectArry();
                    GetSQLReportFilterOperator(ref sQLReportFilterObjectArry, reportParams.ReportFilterObjectList[i].reportFilterObjectArry[z]);
                    ListSQLReportFilterObjectArry.Add(sQLReportFilterObjectArry);
                    string code = setCode;
                    if (reportParams.ReportFilterObjectList[i].SRFieldType == KendoGridFilterSRFieldType.LowerString && reportParams.ReportFilterObjectList[i].reportFilterObjectArry[z].Value.ToString() != "")
                    {
                        code = $"lower({setCode})";
                    }
                    else if (reportParams.ReportFilterObjectList[i].SRFieldType == KendoGridFilterSRFieldType.UpperString && reportParams.ReportFilterObjectList[i].reportFilterObjectArry[z].Value.ToString() != "")
                    {
                        code = $"upper({setCode})";
                    }
                    else if (reportParams.ReportFilterObjectList[i].SRFieldType == KendoGridFilterSRFieldType.Date)
                    {
                        code = $"cast({setCode} as date)";
                    }
                    if (reportParams.ReportFilterObjectList[i].reportFilterObjectArry[z].Type == KendoGridFilterType.isnullorempty || reportParams.ReportFilterObjectList[i].reportFilterObjectArry[z].Type == KendoGridFilterType.isnotnullorempty)
                        code = $"isnull({code},'')";

                    if (z == 0)
                    {
                        TempFilterClause = $" {code} {sQLReportFilterObjectArry.Type} {sQLReportFilterObjectArry.Value} ";
                    }
                    else
                    {
                        TempFilterClause += $" {reportParams.ReportFilterObjectList[i].reportFilterObjectArry[z].Logic} {code} {sQLReportFilterObjectArry.Type} {sQLReportFilterObjectArry.Value} ";
                    }
                }
                if (TempFilterClause.Trim() != "")
                    FilterClause += $" AND ({TempFilterClause}) ";

                reportParams.ReportFilterObjectList[i].sQLReportFilterObjectArry = ListSQLReportFilterObjectArry;
            }
            reportParams.FilterClause = FilterClause;

        }
        public static void GetSQLReportFilterOperator(ref SQLReportFilterObjectArry sQLReportFilterObjectArry, ReportFilterObjectArry reportFilterObjectArry)
        {
            sQLReportFilterObjectArry = new SQLReportFilterObjectArry();
            object val = reportFilterObjectArry.Value;
            if (reportFilterObjectArry.SRFieldType == KendoGridFilterSRFieldType.Date)
            {
                if (reportFilterObjectArry.Value.ToString() == "")
                    val = "1900-01-01";
                else if (Information.IsDate(reportFilterObjectArry.Value))
                    val = Convert.ToDateTime(reportFilterObjectArry.Value.ToString()).ToString("yyyy-MM-dd");
            }
            else if (reportFilterObjectArry.SRFieldType == KendoGridFilterSRFieldType.Datetime)
            {
                if (reportFilterObjectArry.Value.ToString() == "")
                    val = "1900-01-01";
                else if (Information.IsDate(reportFilterObjectArry.Value))
                    val = Convert.ToDateTime(reportFilterObjectArry.Value.ToString()).ToString("yyyy-MM-dd HH:mm:ss");
            }
            else if (reportFilterObjectArry.SRFieldType == KendoGridFilterSRFieldType.Float)
            {
                if (reportFilterObjectArry.Value.ToString() == "")
                    val = 0.00;
                else if (Information.IsNumeric(reportFilterObjectArry.Value))
                    val = Convert.ToDouble(reportFilterObjectArry.Value.ToString());
            }
            else if (reportFilterObjectArry.SRFieldType == KendoGridFilterSRFieldType.Int)
            {
                if (reportFilterObjectArry.Value.ToString() == "")
                    val = 0;
                else if (Information.IsNumeric(reportFilterObjectArry.Value))
                    val = Convert.ToInt64(reportFilterObjectArry.Value.ToString());
            }
            else if (reportFilterObjectArry.SRFieldType == KendoGridFilterSRFieldType.LowerString)
            {
                if (reportFilterObjectArry.Value.ToString() == "")
                    val = "";
                else
                    val = reportFilterObjectArry.Value.ToString().ToLower();
            }
            else if (reportFilterObjectArry.SRFieldType == KendoGridFilterSRFieldType.UpperString)
            {
                if (reportFilterObjectArry.Value.ToString() == "")
                    val = "";
                else
                    val = reportFilterObjectArry.Value.ToString().ToUpper();
            }
            else if (reportFilterObjectArry.SRFieldType == KendoGridFilterSRFieldType.Boolean)
            {
                if (Convert.ToBoolean(reportFilterObjectArry.Value.ToString()))
                    val = 1;
                else
                    val = 0;
            }

            if (reportFilterObjectArry.Type == KendoGridFilterType.inlistfilter || reportFilterObjectArry.Type == KendoGridFilterType.notinlistfilter)
            {
                if (val.ToString() == "")
                    val = "''";
                else
                    val = SetOrders(val.ToString(), true);
            }

            if (reportFilterObjectArry.Type == KendoGridFilterType.contains)
            {
                sQLReportFilterObjectArry.Type = " like ";
                sQLReportFilterObjectArry.Value = $"'%{val}%'";
            }
            else if (reportFilterObjectArry.Type == KendoGridFilterType.doesnotcontain)
            {
                sQLReportFilterObjectArry.Type = " not like ";
                sQLReportFilterObjectArry.Value = $"'%{val}%'";
            }
            else if (reportFilterObjectArry.Type == KendoGridFilterType.notequal)
            {
                sQLReportFilterObjectArry.Type = " not in ";
                sQLReportFilterObjectArry.Value = $"('{val}')";
            }
            else if (reportFilterObjectArry.Type == KendoGridFilterType.equal && reportFilterObjectArry.FieldType != KendoGridFilterFieldType.Boolean && reportFilterObjectArry.FieldType != KendoGridFilterFieldType.Number)
            {
                sQLReportFilterObjectArry.Type = " in ";
                sQLReportFilterObjectArry.Value = $"('{val}')";
            }
            else if (reportFilterObjectArry.Type == KendoGridFilterType.equal && reportFilterObjectArry.FieldType == KendoGridFilterFieldType.Number)
            {
                sQLReportFilterObjectArry.Type = " in ";
                sQLReportFilterObjectArry.Value = $"({val})";
            }
            else if (reportFilterObjectArry.Type == KendoGridFilterType.equal && reportFilterObjectArry.FieldType == KendoGridFilterFieldType.Boolean)
            {
                sQLReportFilterObjectArry.Type = " = ";
                sQLReportFilterObjectArry.Value = val;
            }
            else if (reportFilterObjectArry.Type == KendoGridFilterType.startswith)
            {
                sQLReportFilterObjectArry.Type = " like ";
                sQLReportFilterObjectArry.Value = $"'{val}%'";
            }
            else if (reportFilterObjectArry.Type == KendoGridFilterType.endswith)
            {
                sQLReportFilterObjectArry.Type = " like ";
                sQLReportFilterObjectArry.Value = $"'%{val}'";
            }
            else if (reportFilterObjectArry.Type == KendoGridFilterType.isnull)
            {
                sQLReportFilterObjectArry.Type = " is ";
                sQLReportFilterObjectArry.Value = $"null";
            }
            else if (reportFilterObjectArry.Type == KendoGridFilterType.isnotnull)
            {
                sQLReportFilterObjectArry.Type = " is not ";
                sQLReportFilterObjectArry.Value = $"null";
            }
            else if (reportFilterObjectArry.Type == KendoGridFilterType.orderno)
            {
                sQLReportFilterObjectArry.Type = " = ";
                sQLReportFilterObjectArry.Value = (reportFilterObjectArry.FieldType == KendoGridFilterFieldType.Number ? val : $"'{val}'");
            }
            else if (reportFilterObjectArry.Type == KendoGridFilterType.isempty)
            {
                sQLReportFilterObjectArry.Type = " = ";
                sQLReportFilterObjectArry.Value = $"''";
            }
            else if (reportFilterObjectArry.Type == KendoGridFilterType.isnotempty)
            {
                sQLReportFilterObjectArry.Type = " <> ";
                sQLReportFilterObjectArry.Value = $"''";
            }
            else if (reportFilterObjectArry.Type == KendoGridFilterType.isnullorempty)
            {
                sQLReportFilterObjectArry.Type = " = ";
                sQLReportFilterObjectArry.Value = $"''";
            }
            else if (reportFilterObjectArry.Type == KendoGridFilterType.isnotnullorempty)
            {
                sQLReportFilterObjectArry.Type = " <> ";
                sQLReportFilterObjectArry.Value = $"''";
            }
            else if (reportFilterObjectArry.Type == KendoGridFilterType.isequalorgreather)
            {
                sQLReportFilterObjectArry.Type = " >= ";
                sQLReportFilterObjectArry.Value = (reportFilterObjectArry.FieldType == KendoGridFilterFieldType.Number ? val : $"'{val}'");
            }
            else if (reportFilterObjectArry.Type == KendoGridFilterType.greather)
            {
                sQLReportFilterObjectArry.Type = " > ";
                sQLReportFilterObjectArry.Value = (reportFilterObjectArry.FieldType == KendoGridFilterFieldType.Number ? val : $"'{val}'");
            }
            else if (reportFilterObjectArry.Type == KendoGridFilterType.isequalorless)
            {
                sQLReportFilterObjectArry.Type = " <= ";
                sQLReportFilterObjectArry.Value = (reportFilterObjectArry.FieldType == KendoGridFilterFieldType.Number ? val : $"'{val}'");
            }
            else if (reportFilterObjectArry.Type == KendoGridFilterType.less)
            {
                sQLReportFilterObjectArry.Type = " < ";
                sQLReportFilterObjectArry.Value = (reportFilterObjectArry.FieldType == KendoGridFilterFieldType.Number ? val : $"'{val}'");
            }
            else if (reportFilterObjectArry.Type == KendoGridFilterType.notinlistfilter)
            {
                sQLReportFilterObjectArry.Type = " not in ";
                sQLReportFilterObjectArry.Value = $"({val})";
            }
            else if (reportFilterObjectArry.Type == KendoGridFilterType.inlistfilter)
            {
                sQLReportFilterObjectArry.Type = " in ";
                sQLReportFilterObjectArry.Value = $"({val})";
            }
        }
        public static void GetKendoFilter(ref ReportParams _ReportParams, ref List<Dynamic_SP_Params> List_Dynamic_SP_Params, P_Get_User_Info userinfo)
        {
            ReportResponse reportResponse = new ReportResponse();
            SetReportFilterClause(ref _ReportParams);

            int pagesize = _ReportParams.PageSize;
            int pageindex = _ReportParams.PageIndex;
            string sortExpression = _ReportParams.SortExpression;

            List_Dynamic_SP_Params = new List<Dynamic_SP_Params>();
            Dynamic_SP_Params Dynamic_SP_Params = new Dynamic_SP_Params();

            Dynamic_SP_Params = new Dynamic_SP_Params();
            Dynamic_SP_Params.ParameterName = "PageIndex";
            Dynamic_SP_Params.Val = pageindex;
            List_Dynamic_SP_Params.Add(Dynamic_SP_Params);

            Dynamic_SP_Params = new Dynamic_SP_Params();
            Dynamic_SP_Params.ParameterName = "PageSize";
            Dynamic_SP_Params.Val = pagesize;
            List_Dynamic_SP_Params.Add(Dynamic_SP_Params);

            Dynamic_SP_Params = new Dynamic_SP_Params();
            Dynamic_SP_Params.ParameterName = "SortExpression";
            Dynamic_SP_Params.Val = sortExpression;
            List_Dynamic_SP_Params.Add(Dynamic_SP_Params);

            Dynamic_SP_Params = new Dynamic_SP_Params();
            Dynamic_SP_Params.ParameterName = "FilterClause";
            Dynamic_SP_Params.Val = _ReportParams.FilterClause;
            List_Dynamic_SP_Params.Add(Dynamic_SP_Params);

            Dynamic_SP_Params = new Dynamic_SP_Params();
            Dynamic_SP_Params.ParameterName = "TotalRowCount";
            Dynamic_SP_Params.Val = 0;
            Dynamic_SP_Params.IsInputType = false;
            List_Dynamic_SP_Params.Add(Dynamic_SP_Params);

            //Dynamic_SP_Params = new Dynamic_SP_Params();
            //Dynamic_SP_Params.ParameterName = "TimeZoneID";
            //Dynamic_SP_Params.Val = userinfo.TimeZoneID;
            //List_Dynamic_SP_Params.Add(Dynamic_SP_Params);

            Dynamic_SP_Params = new Dynamic_SP_Params();
            Dynamic_SP_Params.ParameterName = "FilterObject";
            Dynamic_SP_Params.Val = JsonConvert.SerializeObject(_ReportParams.ReportFilterObjectList);
            List_Dynamic_SP_Params.Add(Dynamic_SP_Params);

            Dynamic_SP_Params = new Dynamic_SP_Params();
            Dynamic_SP_Params.ParameterName = "ColumnObject";
            Dynamic_SP_Params.Val = JsonConvert.SerializeObject(_ReportParams.ReportColumnObjectList);
            List_Dynamic_SP_Params.Add(Dynamic_SP_Params);

        }
        public static string SetOrders(string StrOrder, bool Seperation_Logic_Is_Text)
        {
            StrOrder = StrOrder.Replace("'", "");
            StrOrder = StrOrder.Replace(',', Strings.Chr(13));
            StrOrder = StrOrder.Replace(' ', Strings.Chr(13));
            StrOrder = StrOrder.Replace(Strings.Chr(10), Strings.Chr(13));
            string StrRet = "";
            string[] A;
            A = StrOrder.Split(Strings.Chr(13));
            foreach (var OrderNo in A)
            {
                if (OrderNo.Trim() != "")
                {
                    if (StrRet == "")
                    {
                        if (Seperation_Logic_Is_Text)
                            StrRet = string.Format("'{0}'", OrderNo.Trim());
                        else
                            StrRet = string.Format("{0}", OrderNo.Trim());
                    }
                    else if (Seperation_Logic_Is_Text)
                        StrRet = string.Format("{0},'{1}'", StrRet, OrderNo.Trim());
                    else
                        StrRet = string.Format("{0},{1}", StrRet, OrderNo.Trim());
                }
            }
            if (!Seperation_Logic_Is_Text)
            {
            }
            return StrRet;
        }
        public static List<ReportFilterDropDownList> GetReportFilterDropDownListFromDataTable(DataTable DT, ref List<ReportFilterDropDownList> reportFilterDropDownLists, string TextFieldColName = "text", string ValueFieldColName = "value", bool IsAddedSelectOption = true, bool IsAddedSelectAll = false)
        {
            reportFilterDropDownLists = new List<ReportFilterDropDownList>();
            if (IsAddedSelectOption)
            {
                ReportFilterDropDownList reportFilterDropDown = new ReportFilterDropDownList();
                reportFilterDropDown.text = "[Select Option]";
                reportFilterDropDown.value = "";
                reportFilterDropDownLists.Add(reportFilterDropDown);
            }
            if (DT.Rows.Count > 0)
            {
                if (IsAddedSelectAll)
                {
                    ReportFilterDropDownList reportFilterDropDown = new ReportFilterDropDownList();
                    reportFilterDropDown.text = "All";
                    reportFilterDropDown.value = "ALL";
                    reportFilterDropDownLists.Add(reportFilterDropDown);
                }
                for (var i = 0; i <= DT.Rows.Count - 1; i++)
                {
                    ReportFilterDropDownList reportFilterDropDown = new ReportFilterDropDownList();
                    reportFilterDropDown.text = DT.Rows[i][TextFieldColName].ToString();
                    reportFilterDropDown.value = DT.Rows[i][ValueFieldColName];
                    reportFilterDropDownLists.Add(reportFilterDropDown);
                }
            }
            return reportFilterDropDownLists;
        }
        public static void SetKendoOptionDataFilter(ref kendo_option_data_filter kendo_Option_Data_Filter, kendo_option_data_filter_filters kendo_Option_Data_Filter_Filters, string logic = "and")
        {
            //kendo_option_data_filter kendo_Option_Data_Filter = new kendo_option_data_filter();
            kendo_option_data_filter2 kendo_Option_Data_Filter2 = new kendo_option_data_filter2();
            kendo_Option_Data_Filter2.logic = logic;
            //kendo_option_data_filter_filters kendo_Option_Data_Filter_Filters = new kendo_option_data_filter_filters();
            kendo_Option_Data_Filter2.filters.Add(kendo_Option_Data_Filter_Filters);
            kendo_Option_Data_Filter.filters.Add(kendo_Option_Data_Filter2);


        }
        public static List<ValidationResult>? GetDistinctValidationResult(ref List<ValidationResult>? results, bool IsIncludeIgnoreError = false)
        {
            List<ValidationResult>? Ret = new List<ValidationResult>();
            if (results != null)
            {
                if (results.Count > 0)
                {
                    for (int i = 0; i <= results.Count - 1; i++)
                    {
                        string ErrorMsg = results[i].ErrorMessage.Trim();
                        if ((Strings.Left(ErrorMsg.ToLower(), 12) == "ignore error" && IsIncludeIgnoreError))
                        {
                            ErrorMsg = Strings.Mid(ErrorMsg, 15).Trim();
                            Ret.Add(new ValidationResult(ErrorMsg));
                        }
                        else if (Strings.Left(ErrorMsg.ToLower(), 12) != "ignore error" && IsIncludeIgnoreError == false)
                        {
                            Ret.Add(new ValidationResult(ErrorMsg));
                        }
                    }
                    if (Ret.Count > 1)
                    {
                        var ErrorsComparer = new DynamicEqualityComparer<ValidationResult>(c => c.ErrorMessage);
                        Ret = Ret.Distinct(ErrorsComparer).ToList();
                    }
                }
                else
                    Ret = results;
            }
            else
                Ret = results;

            return Ret;
        }
        public static void ValidateChildObjects(object obj, ref List<ValidationResult> results, ref bool isValid)
        {
            var resultstemp = new List<ValidationResult>();
            var context = new ValidationContext(obj);

            // Validate the current object
            var isValid2 = Validator.TryValidateObject(obj, context, resultstemp, true);

            if (isValid2 == false)
            {
                foreach (var validationResult in resultstemp)
                {
                    results.Add(validationResult);
                }
            }

            if (isValid == true && isValid2 == false)
                isValid = isValid2;

            // If the current object is not valid, stop further validation for this branch
            if (!isValid)
            {
                foreach (var property in obj.GetType().GetProperties())
                {
                    // Check if the property is an object and not a primitive type or string
                    if (property != null)
                    {
                        if (property.PropertyType.IsClass)
                        {
                            GetCustomAttributeValidationResult(property, obj, ref results);
                        }
                    }
                }
                return;
            }

            // Validate child properties recursively
            foreach (var property in obj.GetType().GetProperties())
            {
                // Check if the property is an object and not a primitive type or string
                if (property != null)
                {
                    if (property.PropertyType.IsClass)
                    {
                        if (property.PropertyType != typeof(string))
                        {
                            // Check if the property has parameters (indexers)
                            if (property.GetIndexParameters().Length == 0)
                            {
                                var childObject = property.GetValue(obj);

                                // Recursively validate child objects
                                if (childObject != null)
                                {
                                    ValidateChildObjects(childObject, ref results, ref isValid);
                                }
                            }
                        }

                        GetCustomAttributeValidationResult(property, obj, ref results);
                    }
                }
            }
        }
        public static void GetCustomAttributeValidationResult(PropertyInfo property, object obj, ref List<ValidationResult> results)
        {
            // Check for custom validation attributes
            var customAttributes = property.GetCustomAttributes(typeof(CustomValidationAttribute2), true);

            foreach (var attribute in customAttributes)
            {
                if (attribute is CustomValidationAttribute2 customValidationAttribute)
                {
                    // Get the current property value
                    var propertyValue = property.GetValue(obj);

                    // Check if customValidationAttribute is not null
                    if (customValidationAttribute != null)
                    {
                        // Get the Validate method using reflection with parameter types
                        var validateMethod = customValidationAttribute.GetType().GetMethod("Validate",
                            new[] { propertyValue?.GetType() ?? typeof(object), typeof(ValidationContext) });

                        // Check if the Validate method exists
                        if (validateMethod != null)
                        {
                            // Parameters for the Validate method (adjust as needed)
                            var parameters = new object[] { propertyValue, new ValidationContext(obj) };

                            try
                            {
                                // Invoke the Validate method
                                var validationResult = (ValidationResult)validateMethod.Invoke(customValidationAttribute, parameters);

                                // Check the result of the custom validation
                                if (validationResult != ValidationResult.Success)
                                {
                                    // Handle the validation failure (add to results list)
                                    results.Add(validationResult);
                                }
                            }
                            catch (Exception ex)
                            {
                                results.Add(new ValidationResult(ex.InnerException?.Message));
                            }

                        }
                        else
                        {
                            // Handle the case where the Validate method is not found
                            // You might want to log or throw an exception depending on your requirements
                        }
                    }
                }
            }
        }
        public static T? GetValueFromReturnParameter<T>(List<Dynamic_SP_Params> List_Dynamic_SP_Params, string ParameterName, Type type_)
        {
            object? returnvalue = null;
            for (int i = 0; i <= List_Dynamic_SP_Params.Count - 1; i++)
            {
                if (type_ == typeof(int))
                {
                    if (List_Dynamic_SP_Params[i].ParameterName == ParameterName && Information.IsNumeric(List_Dynamic_SP_Params[i].Val))
                        returnvalue = Convert.ToInt32(List_Dynamic_SP_Params[i].Val);
                }
                else if (type_ == typeof(long))
                {
                    if (List_Dynamic_SP_Params[i].ParameterName == ParameterName && Information.IsNumeric(List_Dynamic_SP_Params[i].Val))
                        returnvalue = Convert.ToInt64(List_Dynamic_SP_Params[i].Val);
                }
                else if (type_ == typeof(string))
                {
                    if (List_Dynamic_SP_Params[i].ParameterName == ParameterName && List_Dynamic_SP_Params[i].Val != null)
                        returnvalue = List_Dynamic_SP_Params[i].Val.ToString();
                }
                else
                {
                    if (List_Dynamic_SP_Params[i].ParameterName == ParameterName)
                        returnvalue = List_Dynamic_SP_Params[i].Val;
                }
            }
            return (T?)returnvalue;
        }
    }
    public class GlobalsFunctions
    {
        public static int GetSeconds(int seconds = 0, int minutes = 0, int hours = 0)
        {
            return (seconds + (minutes * 60) + (hours * 60 * 60));
        }
        public static string GetTypeString(List<string>? _List)
        {
            string Ret = "";
            if (_List != null)
            {
                if (_List.Count > 0)
                {
                    for (int i = 0; i <= _List.Count - 1; i++)
                    {
                        Ret += "type:" + _List[i] + "|";
                    }
                }
            }
            return Ret;
        }
        public static bool GetDefaultSetCache(string? subtype)
        {
            bool DefaultValue = false;
            subtype = (subtype == null ? "" : subtype);
            bool RetValue = DefaultValue;

            if (subtype == CacheSubType.P_Get_API_User_Map_Request_Limit)
                RetValue = true;
            else if (subtype == CacheSubType.P_Get_API_User_Map)
                RetValue = true;
            else if (subtype == CacheSubType.P_Get_API_RemoteDomain_IP_Request_Limit)
                RetValue = true;
            else if (subtype == CacheSubType.P_Get_User_Info)
                RetValue = true;
            else if (subtype == CacheSubType.P_Get_API_RemoteDomain_IP_WhiteListing)
                RetValue = true;
            else if (subtype == CacheSubType.P_Get_T_Config_Detail)
                RetValue = true;
            else if (subtype == CacheSubType.P_Get_List_By_ID)
                RetValue = true;
            else if (subtype == CacheSubType.P_Get_Client_Service_Type_List)
                RetValue = true;
            else if (subtype == CacheSubType.P_Get_Order_Client_Identifier_Fields_List)
                RetValue = true;
            else if (subtype == CacheSubType.P_Get_Client_Special_Service_List)
                RetValue = true;
            else if (subtype == CacheSubType.P_Get_Client_Ignore_API_Errors_List)
                RetValue = true;
            else if (subtype == CacheSubType.P_Get_Seller_All_MappingList_ReturnJson)
                RetValue = true;
            else if (subtype == CacheSubType.P_Get_SellToClientList)
                RetValue = true;
            else if (subtype == CacheSubType.P_Get_Price_Key)
                RetValue = true;
            else if (subtype == CacheSubType.P_Get_Role_Rights_From_RoleID)
                RetValue = true;
            else if (subtype == CacheSubType.P_Get_Role_Rights_From_Username)
                RetValue = true;
            else if (subtype == CacheSubType.P_Is_Has_Right_From_RoleID)
                RetValue = true;
            else if (subtype == CacheSubType.P_Is_Has_Right_From_Username)
                RetValue = true;
            else if (subtype == CacheSubType.P_Get_User_Order_Detail_Grid_List)
                RetValue = true;
            else if (subtype == CacheSubType.P_Get_Pages_Info_By_User)
                RetValue = true;
            else if (subtype == CacheSubType.P_GetDBServerForDataRead_2)
                RetValue = true;
            else if (subtype == CacheSubType.GetQuoteAPIToken)
                RetValue = true;
            else if (subtype == CacheSubType.P_Get_Order_GUID_By_OrderID)
                RetValue = true;
            else if (subtype == CacheSubType.P_Get_OrderAccess_By_GUID)
                RetValue = true;
            else if (subtype == CacheSubType.P_Get_OrderDetail_Assignment_By_GUID)
                RetValue = false;
            else if (subtype == CacheSubType.P_Get_OrderDetail_BasicSummary_By_GUID)
                RetValue = false;
            else if (subtype == CacheSubType.P_Get_OrderDetail_ClientIdentifier_By_GUID)
                RetValue = false;
            else if (subtype == CacheSubType.P_Get_OrderDetail_SpecialServices_By_GUID)
                RetValue = false;
            else if (subtype == CacheSubType.P_Get_OrderDetail_AccessLog_By_GUID)
                RetValue = false;
            else if (subtype == CacheSubType.P_Get_OrderDetail_ItemDetail_By_GUID)
                RetValue = false;
            else if (subtype == CacheSubType.P_Get_OrderDetail_InvoiceHeader_By_GUID)
                RetValue = false;
            else if (subtype == CacheSubType.P_Get_OrderDetail_InvoiceLine_By_GUID)
                RetValue = false;
            else if (subtype == CacheSubType.P_Get_OrderDetail_Comments_By_GUID)
                RetValue = false;
            else if (subtype == CacheSubType.P_Get_OrderDetail_Events_By_GUID)
                RetValue = false;
            else if (subtype == CacheSubType.P_Get_OrderDetail_Documents_By_GUID)
                RetValue = false;
            else if (subtype == CacheSubType.P_Get_OrderDetail_ScanHistory_By_GUID)
                RetValue = false;
            else if (subtype == CacheSubType.P_Get_OrderDetail_Manifest_By_GUID)
                RetValue = false;
            else if (subtype == CacheSubType.P_Get_OrderDetail_POPPOD_By_GUID)
                RetValue = false;
            else if (subtype == CacheSubType.P_Get_OrderDetail_ChangeLog_By_GUID)
                RetValue = false;
            else if (subtype == CacheSubType.P_Get_Seller_Import_Fields_Name_Setting)
                RetValue = false;
            else if (subtype == CacheSubType.GoogleMapsAPICall)
                RetValue = true;
            else if (subtype == CacheSubType.P_Get_SellerBillTo_From_Pinnacle_User)
                RetValue = true;
            else if (subtype == CacheSubType.P_Get_ShipmentType_From_ServiceLevel)
                RetValue = true;
            else if (subtype == CacheSubType.P_Get_Search_Sku_List)
                RetValue = true;
            else if (subtype == CacheSubType.GoogleAddressValidationAPICall)
                RetValue = true;
            else if (subtype == CacheSubType.P_Get_Google_Address_Validation_Json)
                RetValue = true;
            else if (subtype == CacheSubType.P_Get_SellerAddressList)
                RetValue = true;
            else if (subtype == CacheSubType.P_Get_AddressList)
                RetValue = true;
            else if (subtype == CacheSubType.P_Get_Import_Order_File_Source_Setup)
                RetValue = true;
            else if (subtype == CacheSubType.P_Get_List_By_ID_2)
                RetValue = true;
            else if (subtype == CacheSubType.P_Get_Sub_OrderType_List)
                RetValue = true;
            else if (subtype == CacheSubType.GetReportingAPIToken)
                RetValue = true;
            else if (subtype == CacheSubType.GetUserPOMSAPIToken)
                RetValue = true;
            else if (subtype == CacheSubType.GetAdminPOMSAPIToken)
                RetValue = true;

            return RetValue;
        }
        public static bool GetDefaultSetSQLCache(string? subtype)
        {
            bool DefaultValue = false;
            subtype = (subtype == null ? "" : subtype);
            bool RetValue = DefaultValue;

            if (subtype == CacheSubType.P_Get_API_User_Map_Request_Limit)
                RetValue = false;
            else if (subtype == CacheSubType.P_Get_API_User_Map)
                RetValue = false;
            else if (subtype == CacheSubType.P_Get_API_RemoteDomain_IP_Request_Limit)
                RetValue = false;
            else if (subtype == CacheSubType.P_Get_User_Info)
                RetValue = false;
            else if (subtype == CacheSubType.P_Get_API_RemoteDomain_IP_WhiteListing)
                RetValue = false;
            else if (subtype == CacheSubType.P_Get_T_Config_Detail)
                RetValue = false;
            else if (subtype == CacheSubType.P_Get_List_By_ID)
                RetValue = false;
            else if (subtype == CacheSubType.P_Get_Client_Service_Type_List)
                RetValue = false;
            else if (subtype == CacheSubType.P_Get_Order_Client_Identifier_Fields_List)
                RetValue = false;
            else if (subtype == CacheSubType.P_Get_Client_Special_Service_List)
                RetValue = false;
            else if (subtype == CacheSubType.P_Get_Client_Ignore_API_Errors_List)
                RetValue = false;
            else if (subtype == CacheSubType.P_Get_Seller_All_MappingList_ReturnJson)
                RetValue = false;
            else if (subtype == CacheSubType.P_Get_SellToClientList)
                RetValue = false;
            else if (subtype == CacheSubType.P_Get_Price_Key)
                RetValue = false;
            else if (subtype == CacheSubType.P_Get_Role_Rights_From_RoleID)
                RetValue = false;
            else if (subtype == CacheSubType.P_Get_Role_Rights_From_Username)
                RetValue = false;
            else if (subtype == CacheSubType.P_Is_Has_Right_From_RoleID)
                RetValue = false;
            else if (subtype == CacheSubType.P_Is_Has_Right_From_Username)
                RetValue = false;
            else if (subtype == CacheSubType.P_Get_User_Order_Detail_Grid_List)
                RetValue = false;
            else if (subtype == CacheSubType.P_Get_Pages_Info_By_User)
                RetValue = false;
            else if (subtype == CacheSubType.P_GetDBServerForDataRead_2)
                RetValue = false;
            else if (subtype == CacheSubType.GetQuoteAPIToken)
                RetValue = false;
            else if (subtype == CacheSubType.P_Get_Order_GUID_By_OrderID)
                RetValue = false;
            else if (subtype == CacheSubType.P_Get_OrderAccess_By_GUID)
                RetValue = false;
            else if (subtype == CacheSubType.P_Get_OrderDetail_Assignment_By_GUID)
                RetValue = false;
            else if (subtype == CacheSubType.P_Get_OrderDetail_BasicSummary_By_GUID)
                RetValue = false;
            else if (subtype == CacheSubType.P_Get_OrderDetail_ClientIdentifier_By_GUID)
                RetValue = false;
            else if (subtype == CacheSubType.P_Get_OrderDetail_SpecialServices_By_GUID)
                RetValue = false;
            else if (subtype == CacheSubType.P_Get_OrderDetail_AccessLog_By_GUID)
                RetValue = false;
            else if (subtype == CacheSubType.P_Get_OrderDetail_ItemDetail_By_GUID)
                RetValue = false;
            else if (subtype == CacheSubType.P_Get_OrderDetail_InvoiceHeader_By_GUID)
                RetValue = false;
            else if (subtype == CacheSubType.P_Get_OrderDetail_InvoiceLine_By_GUID)
                RetValue = false;
            else if (subtype == CacheSubType.P_Get_OrderDetail_Comments_By_GUID)
                RetValue = false;
            else if (subtype == CacheSubType.P_Get_OrderDetail_Events_By_GUID)
                RetValue = false;
            else if (subtype == CacheSubType.P_Get_OrderDetail_Documents_By_GUID)
                RetValue = false;
            else if (subtype == CacheSubType.P_Get_OrderDetail_ScanHistory_By_GUID)
                RetValue = false;
            else if (subtype == CacheSubType.P_Get_OrderDetail_Manifest_By_GUID)
                RetValue = false;
            else if (subtype == CacheSubType.P_Get_OrderDetail_POPPOD_By_GUID)
                RetValue = false;
            else if (subtype == CacheSubType.P_Get_OrderDetail_ChangeLog_By_GUID)
                RetValue = false;
            else if (subtype == CacheSubType.P_Get_Seller_Import_Fields_Name_Setting)
                RetValue = false;
            else if (subtype == CacheSubType.GoogleMapsAPICall)
                RetValue = false;
            else if (subtype == CacheSubType.P_Get_SellerBillTo_From_Pinnacle_User)
                RetValue = false;
            else if (subtype == CacheSubType.P_Get_ShipmentType_From_ServiceLevel)
                RetValue = false;
            else if (subtype == CacheSubType.P_Get_Search_Sku_List)
                RetValue = false;
            else if (subtype == CacheSubType.GoogleAddressValidationAPICall)
                RetValue = false;
            else if (subtype == CacheSubType.P_Get_Google_Address_Validation_Json)
                RetValue = false;
            else if (subtype == CacheSubType.P_Get_SellerAddressList)
                RetValue = false;
            else if (subtype == CacheSubType.P_Get_AddressList)
                RetValue = false;
            else if (subtype == CacheSubType.P_Get_Import_Order_File_Source_Setup)
                RetValue = false;
            else if (subtype == CacheSubType.P_Get_List_By_ID_2)
                RetValue = false;
            else if (subtype == CacheSubType.P_Get_Sub_OrderType_List)
                RetValue = false;
            else if (subtype == CacheSubType.GetReportingAPIToken)
                RetValue = false;
            else if (subtype == CacheSubType.GetUserPOMSAPIToken)
                RetValue = false;
            else if (subtype == CacheSubType.GetAdminPOMSAPIToken)
                RetValue = false;

            return RetValue;
        }
        public static bool GetDefaultGetCache(string? subtype)
        {
            bool DefaultValue = false;
            subtype = (subtype == null ? "" : subtype);
            bool RetValue = DefaultValue;

            if (subtype == CacheSubType.P_Get_API_User_Map_Request_Limit)
                RetValue = true;
            else if (subtype == CacheSubType.P_Get_API_User_Map)
                RetValue = true;
            else if (subtype == CacheSubType.P_Get_API_RemoteDomain_IP_Request_Limit)
                RetValue = true;
            else if (subtype == CacheSubType.P_Get_User_Info)
                RetValue = true;
            else if (subtype == CacheSubType.P_Get_API_RemoteDomain_IP_WhiteListing)
                RetValue = true;
            else if (subtype == CacheSubType.P_Get_T_Config_Detail)
                RetValue = true;
            else if (subtype == CacheSubType.P_Get_List_By_ID)
                RetValue = true;
            else if (subtype == CacheSubType.P_Get_Client_Service_Type_List)
                RetValue = true;
            else if (subtype == CacheSubType.P_Get_Order_Client_Identifier_Fields_List)
                RetValue = true;
            else if (subtype == CacheSubType.P_Get_Client_Special_Service_List)
                RetValue = true;
            else if (subtype == CacheSubType.P_Get_Client_Ignore_API_Errors_List)
                RetValue = true;
            else if (subtype == CacheSubType.P_Get_Seller_All_MappingList_ReturnJson)
                RetValue = true;
            else if (subtype == CacheSubType.P_Get_SellToClientList)
                RetValue = true;
            else if (subtype == CacheSubType.P_Get_Price_Key)
                RetValue = true;
            else if (subtype == CacheSubType.P_Get_Role_Rights_From_RoleID)
                RetValue = true;
            else if (subtype == CacheSubType.P_Get_Role_Rights_From_Username)
                RetValue = true;
            else if (subtype == CacheSubType.P_Is_Has_Right_From_RoleID)
                RetValue = true;
            else if (subtype == CacheSubType.P_Is_Has_Right_From_Username)
                RetValue = true;
            else if (subtype == CacheSubType.P_Get_User_Order_Detail_Grid_List)
                RetValue = true;
            else if (subtype == CacheSubType.P_Get_Pages_Info_By_User)
                RetValue = true;
            else if (subtype == CacheSubType.P_GetDBServerForDataRead_2)
                RetValue = true;
            else if (subtype == CacheSubType.GetQuoteAPIToken)
                RetValue = true;
            else if (subtype == CacheSubType.P_Get_Order_GUID_By_OrderID)
                RetValue = true;
            else if (subtype == CacheSubType.P_Get_OrderAccess_By_GUID)
                RetValue = true;
            else if (subtype == CacheSubType.P_Get_OrderDetail_Assignment_By_GUID)
                RetValue = false;
            else if (subtype == CacheSubType.P_Get_OrderDetail_BasicSummary_By_GUID)
                RetValue = false;
            else if (subtype == CacheSubType.P_Get_OrderDetail_ClientIdentifier_By_GUID)
                RetValue = false;
            else if (subtype == CacheSubType.P_Get_OrderDetail_SpecialServices_By_GUID)
                RetValue = false;
            else if (subtype == CacheSubType.P_Get_OrderDetail_AccessLog_By_GUID)
                RetValue = false;
            else if (subtype == CacheSubType.P_Get_OrderDetail_ItemDetail_By_GUID)
                RetValue = false;
            else if (subtype == CacheSubType.P_Get_OrderDetail_InvoiceHeader_By_GUID)
                RetValue = false;
            else if (subtype == CacheSubType.P_Get_OrderDetail_InvoiceLine_By_GUID)
                RetValue = false;
            else if (subtype == CacheSubType.P_Get_OrderDetail_Comments_By_GUID)
                RetValue = false;
            else if (subtype == CacheSubType.P_Get_OrderDetail_Events_By_GUID)
                RetValue = false;
            else if (subtype == CacheSubType.P_Get_OrderDetail_Documents_By_GUID)
                RetValue = false;
            else if (subtype == CacheSubType.P_Get_OrderDetail_ScanHistory_By_GUID)
                RetValue = false;
            else if (subtype == CacheSubType.P_Get_OrderDetail_Manifest_By_GUID)
                RetValue = false;
            else if (subtype == CacheSubType.P_Get_OrderDetail_POPPOD_By_GUID)
                RetValue = false;
            else if (subtype == CacheSubType.P_Get_OrderDetail_ChangeLog_By_GUID)
                RetValue = false;
            else if (subtype == CacheSubType.P_Get_Seller_Import_Fields_Name_Setting)
                RetValue = false;
            else if (subtype == CacheSubType.GoogleMapsAPICall)
                RetValue = true;
            else if (subtype == CacheSubType.P_Get_SellerBillTo_From_Pinnacle_User)
                RetValue = true;
            else if (subtype == CacheSubType.P_Get_ShipmentType_From_ServiceLevel)
                RetValue = true;
            else if (subtype == CacheSubType.P_Get_Search_Sku_List)
                RetValue = true;
            else if (subtype == CacheSubType.GoogleAddressValidationAPICall)
                RetValue = true;
            else if (subtype == CacheSubType.P_Get_Google_Address_Validation_Json)
                RetValue = true;
            else if (subtype == CacheSubType.P_Get_SellerAddressList)
                RetValue = true;
            else if (subtype == CacheSubType.P_Get_AddressList)
                RetValue = true;
            else if (subtype == CacheSubType.P_Get_Import_Order_File_Source_Setup)
                RetValue = true;
            else if (subtype == CacheSubType.P_Get_List_By_ID_2)
                RetValue = true;
            else if (subtype == CacheSubType.P_Get_Sub_OrderType_List)
                RetValue = true;
            else if (subtype == CacheSubType.GetReportingAPIToken)
                RetValue = true;
            else if (subtype == CacheSubType.GetUserPOMSAPIToken)
                RetValue = true;
            else if (subtype == CacheSubType.GetAdminPOMSAPIToken)
                RetValue = true;

            return RetValue;
        }
        public static bool GetDefaultGetSQLCache(string? subtype)
        {
            bool DefaultValue = false;
            subtype = (subtype == null ? "" : subtype);
            bool RetValue = DefaultValue;

            if (subtype == CacheSubType.P_Get_API_User_Map_Request_Limit)
                RetValue = false;
            else if (subtype == CacheSubType.P_Get_API_User_Map)
                RetValue = false;
            else if (subtype == CacheSubType.P_Get_API_RemoteDomain_IP_Request_Limit)
                RetValue = false;
            else if (subtype == CacheSubType.P_Get_User_Info)
                RetValue = false;
            else if (subtype == CacheSubType.P_Get_API_RemoteDomain_IP_WhiteListing)
                RetValue = false;
            else if (subtype == CacheSubType.P_Get_T_Config_Detail)
                RetValue = false;
            else if (subtype == CacheSubType.P_Get_List_By_ID)
                RetValue = false;
            else if (subtype == CacheSubType.P_Get_Client_Service_Type_List)
                RetValue = false;
            else if (subtype == CacheSubType.P_Get_Order_Client_Identifier_Fields_List)
                RetValue = false;
            else if (subtype == CacheSubType.P_Get_Client_Special_Service_List)
                RetValue = false;
            else if (subtype == CacheSubType.P_Get_Client_Ignore_API_Errors_List)
                RetValue = false;
            else if (subtype == CacheSubType.P_Get_Seller_All_MappingList_ReturnJson)
                RetValue = false;
            else if (subtype == CacheSubType.P_Get_SellToClientList)
                RetValue = false;
            else if (subtype == CacheSubType.P_Get_Price_Key)
                RetValue = false;
            else if (subtype == CacheSubType.P_Get_Role_Rights_From_RoleID)
                RetValue = false;
            else if (subtype == CacheSubType.P_Get_Role_Rights_From_Username)
                RetValue = false;
            else if (subtype == CacheSubType.P_Is_Has_Right_From_RoleID)
                RetValue = false;
            else if (subtype == CacheSubType.P_Is_Has_Right_From_Username)
                RetValue = false;
            else if (subtype == CacheSubType.P_Get_User_Order_Detail_Grid_List)
                RetValue = false;
            else if (subtype == CacheSubType.P_Get_Pages_Info_By_User)
                RetValue = false;
            else if (subtype == CacheSubType.P_GetDBServerForDataRead_2)
                RetValue = false;
            else if (subtype == CacheSubType.GetQuoteAPIToken)
                RetValue = false;
            else if (subtype == CacheSubType.P_Get_Order_GUID_By_OrderID)
                RetValue = false;
            else if (subtype == CacheSubType.P_Get_OrderAccess_By_GUID)
                RetValue = false;
            else if (subtype == CacheSubType.P_Get_OrderDetail_Assignment_By_GUID)
                RetValue = false;
            else if (subtype == CacheSubType.P_Get_OrderDetail_BasicSummary_By_GUID)
                RetValue = false;
            else if (subtype == CacheSubType.P_Get_OrderDetail_ClientIdentifier_By_GUID)
                RetValue = false;
            else if (subtype == CacheSubType.P_Get_OrderDetail_SpecialServices_By_GUID)
                RetValue = false;
            else if (subtype == CacheSubType.P_Get_OrderDetail_AccessLog_By_GUID)
                RetValue = false;
            else if (subtype == CacheSubType.P_Get_OrderDetail_ItemDetail_By_GUID)
                RetValue = false;
            else if (subtype == CacheSubType.P_Get_OrderDetail_InvoiceHeader_By_GUID)
                RetValue = false;
            else if (subtype == CacheSubType.P_Get_OrderDetail_InvoiceLine_By_GUID)
                RetValue = false;
            else if (subtype == CacheSubType.P_Get_OrderDetail_Comments_By_GUID)
                RetValue = false;
            else if (subtype == CacheSubType.P_Get_OrderDetail_Events_By_GUID)
                RetValue = false;
            else if (subtype == CacheSubType.P_Get_OrderDetail_Documents_By_GUID)
                RetValue = false;
            else if (subtype == CacheSubType.P_Get_OrderDetail_ScanHistory_By_GUID)
                RetValue = false;
            else if (subtype == CacheSubType.P_Get_OrderDetail_Manifest_By_GUID)
                RetValue = false;
            else if (subtype == CacheSubType.P_Get_OrderDetail_POPPOD_By_GUID)
                RetValue = false;
            else if (subtype == CacheSubType.P_Get_OrderDetail_ChangeLog_By_GUID)
                RetValue = false;
            else if (subtype == CacheSubType.P_Get_Seller_Import_Fields_Name_Setting)
                RetValue = false;
            else if (subtype == CacheSubType.GoogleMapsAPICall)
                RetValue = false;
            else if (subtype == CacheSubType.P_Get_SellerBillTo_From_Pinnacle_User)
                RetValue = false;
            else if (subtype == CacheSubType.P_Get_ShipmentType_From_ServiceLevel)
                RetValue = false;
            else if (subtype == CacheSubType.P_Get_Search_Sku_List)
                RetValue = false;
            else if (subtype == CacheSubType.GoogleAddressValidationAPICall)
                RetValue = false;
            else if (subtype == CacheSubType.P_Get_Google_Address_Validation_Json)
                RetValue = false;
            else if (subtype == CacheSubType.P_Get_SellerAddressList)
                RetValue = false;
            else if (subtype == CacheSubType.P_Get_AddressList)
                RetValue = false;
            else if (subtype == CacheSubType.P_Get_Import_Order_File_Source_Setup)
                RetValue = false;
            else if (subtype == CacheSubType.P_Get_List_By_ID_2)
                RetValue = false;
            else if (subtype == CacheSubType.P_Get_Sub_OrderType_List)
                RetValue = false;
            else if (subtype == CacheSubType.GetReportingAPIToken)
                RetValue = false;
            else if (subtype == CacheSubType.GetUserPOMSAPIToken)
                RetValue = false;
            else if (subtype == CacheSubType.GetAdminPOMSAPIToken)
                RetValue = false;

            return RetValue;
        }
        public static bool GetDefaultRemoveCache(string? subtype)
        {
            bool DefaultValue = false;
            subtype = (subtype == null ? "" : subtype);
            bool RetValue = DefaultValue;

            if (subtype == CacheSubType.P_Get_API_User_Map_Request_Limit)
                RetValue = false;
            else if (subtype == CacheSubType.P_Get_API_User_Map)
                RetValue = false;
            else if (subtype == CacheSubType.P_Get_API_RemoteDomain_IP_Request_Limit)
                RetValue = false;
            else if (subtype == CacheSubType.P_Get_User_Info)
                RetValue = false;
            else if (subtype == CacheSubType.P_Get_API_RemoteDomain_IP_WhiteListing)
                RetValue = false;
            else if (subtype == CacheSubType.P_Get_T_Config_Detail)
                RetValue = false;
            else if (subtype == CacheSubType.P_Get_List_By_ID)
                RetValue = false;
            else if (subtype == CacheSubType.P_Get_Client_Service_Type_List)
                RetValue = false;
            else if (subtype == CacheSubType.P_Get_Order_Client_Identifier_Fields_List)
                RetValue = false;
            else if (subtype == CacheSubType.P_Get_Client_Special_Service_List)
                RetValue = false;
            else if (subtype == CacheSubType.P_Get_Client_Ignore_API_Errors_List)
                RetValue = false;
            else if (subtype == CacheSubType.P_Get_Seller_All_MappingList_ReturnJson)
                RetValue = false;
            else if (subtype == CacheSubType.P_Get_SellToClientList)
                RetValue = true;
            else if (subtype == CacheSubType.P_Get_Price_Key)
                RetValue = true;
            else if (subtype == CacheSubType.P_Get_Role_Rights_From_RoleID)
                RetValue = false;
            else if (subtype == CacheSubType.P_Get_Role_Rights_From_Username)
                RetValue = false;
            else if (subtype == CacheSubType.P_Is_Has_Right_From_RoleID)
                RetValue = false;
            else if (subtype == CacheSubType.P_Is_Has_Right_From_Username)
                RetValue = false;
            else if (subtype == CacheSubType.P_Get_User_Order_Detail_Grid_List)
                RetValue = false;
            else if (subtype == CacheSubType.P_Get_Pages_Info_By_User)
                RetValue = false;
            else if (subtype == CacheSubType.P_GetDBServerForDataRead_2)
                RetValue = false;
            else if (subtype == CacheSubType.GetQuoteAPIToken)
                RetValue = false;
            else if (subtype == CacheSubType.P_Get_Order_GUID_By_OrderID)
                RetValue = false;
            else if (subtype == CacheSubType.P_Get_OrderAccess_By_GUID)
                RetValue = false;
            else if (subtype == CacheSubType.P_Get_OrderDetail_Assignment_By_GUID)
                RetValue = false;
            else if (subtype == CacheSubType.P_Get_OrderDetail_BasicSummary_By_GUID)
                RetValue = false;
            else if (subtype == CacheSubType.P_Get_OrderDetail_ClientIdentifier_By_GUID)
                RetValue = false;
            else if (subtype == CacheSubType.P_Get_OrderDetail_SpecialServices_By_GUID)
                RetValue = false;
            else if (subtype == CacheSubType.P_Get_OrderDetail_AccessLog_By_GUID)
                RetValue = false;
            else if (subtype == CacheSubType.P_Get_OrderDetail_ItemDetail_By_GUID)
                RetValue = false;
            else if (subtype == CacheSubType.P_Get_OrderDetail_InvoiceHeader_By_GUID)
                RetValue = false;
            else if (subtype == CacheSubType.P_Get_OrderDetail_InvoiceLine_By_GUID)
                RetValue = false;
            else if (subtype == CacheSubType.P_Get_OrderDetail_Comments_By_GUID)
                RetValue = false;
            else if (subtype == CacheSubType.P_Get_OrderDetail_Events_By_GUID)
                RetValue = false;
            else if (subtype == CacheSubType.P_Get_OrderDetail_Documents_By_GUID)
                RetValue = false;
            else if (subtype == CacheSubType.P_Get_OrderDetail_ScanHistory_By_GUID)
                RetValue = false;
            else if (subtype == CacheSubType.P_Get_OrderDetail_Manifest_By_GUID)
                RetValue = false;
            else if (subtype == CacheSubType.P_Get_OrderDetail_POPPOD_By_GUID)
                RetValue = false;
            else if (subtype == CacheSubType.P_Get_OrderDetail_ChangeLog_By_GUID)
                RetValue = false;
            else if (subtype == CacheSubType.P_Get_Seller_Import_Fields_Name_Setting)
                RetValue = false;
            else if (subtype == CacheSubType.GoogleMapsAPICall)
                RetValue = false;
            else if (subtype == CacheSubType.P_Get_SellerBillTo_From_Pinnacle_User)
                RetValue = false;
            else if (subtype == CacheSubType.P_Get_ShipmentType_From_ServiceLevel)
                RetValue = false;
            else if (subtype == CacheSubType.P_Get_Search_Sku_List)
                RetValue = false;
            else if (subtype == CacheSubType.GoogleAddressValidationAPICall)
                RetValue = false;
            else if (subtype == CacheSubType.P_Get_Google_Address_Validation_Json)
                RetValue = false;
            else if (subtype == CacheSubType.P_Get_SellerAddressList)
                RetValue = false;
            else if (subtype == CacheSubType.P_Get_AddressList)
                RetValue = false;
            else if (subtype == CacheSubType.P_Get_Import_Order_File_Source_Setup)
                RetValue = false;
            else if (subtype == CacheSubType.P_Get_List_By_ID_2)
                RetValue = false;
            else if (subtype == CacheSubType.P_Get_Sub_OrderType_List)
                RetValue = false;
            else if (subtype == CacheSubType.GetReportingAPIToken)
                RetValue = false;
            else if (subtype == CacheSubType.GetUserPOMSAPIToken)
                RetValue = false;
            else if (subtype == CacheSubType.GetAdminPOMSAPIToken)
                RetValue = false;

            return RetValue;
        }
        public static bool GetDefaultRemoveSQLCache(string? subtype)
        {
            bool DefaultValue = false;
            subtype = (subtype == null ? "" : subtype);
            bool RetValue = DefaultValue;

            if (subtype == CacheSubType.P_Get_API_User_Map_Request_Limit)
                RetValue = false;
            else if (subtype == CacheSubType.P_Get_API_User_Map)
                RetValue = false;
            else if (subtype == CacheSubType.P_Get_API_RemoteDomain_IP_Request_Limit)
                RetValue = false;
            else if (subtype == CacheSubType.P_Get_User_Info)
                RetValue = false;
            else if (subtype == CacheSubType.P_Get_API_RemoteDomain_IP_WhiteListing)
                RetValue = false;
            else if (subtype == CacheSubType.P_Get_T_Config_Detail)
                RetValue = false;
            else if (subtype == CacheSubType.P_Get_List_By_ID)
                RetValue = false;
            else if (subtype == CacheSubType.P_Get_Client_Service_Type_List)
                RetValue = false;
            else if (subtype == CacheSubType.P_Get_Order_Client_Identifier_Fields_List)
                RetValue = false;
            else if (subtype == CacheSubType.P_Get_Client_Special_Service_List)
                RetValue = false;
            else if (subtype == CacheSubType.P_Get_Client_Ignore_API_Errors_List)
                RetValue = false;
            else if (subtype == CacheSubType.P_Get_Seller_All_MappingList_ReturnJson)
                RetValue = false;
            else if (subtype == CacheSubType.P_Get_SellToClientList)
                RetValue = false;
            else if (subtype == CacheSubType.P_Get_Price_Key)
                RetValue = false;
            else if (subtype == CacheSubType.P_Get_Role_Rights_From_RoleID)
                RetValue = false;
            else if (subtype == CacheSubType.P_Get_Role_Rights_From_Username)
                RetValue = false;
            else if (subtype == CacheSubType.P_Is_Has_Right_From_RoleID)
                RetValue = false;
            else if (subtype == CacheSubType.P_Is_Has_Right_From_Username)
                RetValue = false;
            else if (subtype == CacheSubType.P_Get_User_Order_Detail_Grid_List)
                RetValue = false;
            else if (subtype == CacheSubType.P_Get_Pages_Info_By_User)
                RetValue = false;
            else if (subtype == CacheSubType.P_GetDBServerForDataRead_2)
                RetValue = false;
            else if (subtype == CacheSubType.GetQuoteAPIToken)
                RetValue = false;
            else if (subtype == CacheSubType.P_Get_Order_GUID_By_OrderID)
                RetValue = false;
            else if (subtype == CacheSubType.P_Get_OrderAccess_By_GUID)
                RetValue = false;
            else if (subtype == CacheSubType.P_Get_OrderDetail_Assignment_By_GUID)
                RetValue = false;
            else if (subtype == CacheSubType.P_Get_OrderDetail_BasicSummary_By_GUID)
                RetValue = false;
            else if (subtype == CacheSubType.P_Get_OrderDetail_ClientIdentifier_By_GUID)
                RetValue = false;
            else if (subtype == CacheSubType.P_Get_OrderDetail_SpecialServices_By_GUID)
                RetValue = false;
            else if (subtype == CacheSubType.P_Get_OrderDetail_AccessLog_By_GUID)
                RetValue = false;
            else if (subtype == CacheSubType.P_Get_OrderDetail_ItemDetail_By_GUID)
                RetValue = false;
            else if (subtype == CacheSubType.P_Get_OrderDetail_InvoiceHeader_By_GUID)
                RetValue = false;
            else if (subtype == CacheSubType.P_Get_OrderDetail_InvoiceLine_By_GUID)
                RetValue = false;
            else if (subtype == CacheSubType.P_Get_OrderDetail_Comments_By_GUID)
                RetValue = false;
            else if (subtype == CacheSubType.P_Get_OrderDetail_Events_By_GUID)
                RetValue = false;
            else if (subtype == CacheSubType.P_Get_OrderDetail_Documents_By_GUID)
                RetValue = false;
            else if (subtype == CacheSubType.P_Get_OrderDetail_ScanHistory_By_GUID)
                RetValue = false;
            else if (subtype == CacheSubType.P_Get_OrderDetail_Manifest_By_GUID)
                RetValue = false;
            else if (subtype == CacheSubType.P_Get_OrderDetail_POPPOD_By_GUID)
                RetValue = false;
            else if (subtype == CacheSubType.P_Get_OrderDetail_ChangeLog_By_GUID)
                RetValue = false;
            else if (subtype == CacheSubType.P_Get_Seller_Import_Fields_Name_Setting)
                RetValue = false;
            else if (subtype == CacheSubType.GoogleMapsAPICall)
                RetValue = false;
            else if (subtype == CacheSubType.P_Get_SellerBillTo_From_Pinnacle_User)
                RetValue = false;
            else if (subtype == CacheSubType.P_Get_ShipmentType_From_ServiceLevel)
                RetValue = false;
            else if (subtype == CacheSubType.P_Get_Search_Sku_List)
                RetValue = false;
            else if (subtype == CacheSubType.GoogleAddressValidationAPICall)
                RetValue = false;
            else if (subtype == CacheSubType.P_Get_Google_Address_Validation_Json)
                RetValue = false;
            else if (subtype == CacheSubType.P_Get_SellerAddressList)
                RetValue = false;
            else if (subtype == CacheSubType.P_Get_AddressList)
                RetValue = false;
            else if (subtype == CacheSubType.P_Get_Import_Order_File_Source_Setup)
                RetValue = false;
            else if (subtype == CacheSubType.P_Get_List_By_ID_2)
                RetValue = false;
            else if (subtype == CacheSubType.P_Get_Sub_OrderType_List)
                RetValue = false;
            else if (subtype == CacheSubType.GetReportingAPIToken)
                RetValue = false;
            else if (subtype == CacheSubType.GetUserPOMSAPIToken)
                RetValue = false;
            else if (subtype == CacheSubType.GetAdminPOMSAPIToken)
                RetValue = false;

            return RetValue;
        }
        public static int GetDefaultCacheExpirySeconds(string? subtype)
        {
            int DefaultValue = 60;
            subtype = (subtype == null ? "" : subtype);
            int RetValue = DefaultValue;

            if (subtype == CacheSubType.P_Get_API_User_Map_Request_Limit)
                RetValue = GetSeconds(hours: 1);
            else if (subtype == CacheSubType.P_Get_API_User_Map)
                RetValue = GetSeconds(hours: 1);
            else if (subtype == CacheSubType.P_Get_API_RemoteDomain_IP_Request_Limit)
                RetValue = GetSeconds(hours: 6);
            else if (subtype == CacheSubType.P_Get_User_Info)
                RetValue = GetSeconds(hours: 1);
            else if (subtype == CacheSubType.P_Get_API_RemoteDomain_IP_WhiteListing)
                RetValue = GetSeconds(hours: 6);
            else if (subtype == CacheSubType.P_Get_T_Config_Detail)
                RetValue = GetSeconds(hours: 1);
            else if (subtype == CacheSubType.P_Get_List_By_ID)
                RetValue = GetSeconds(hours: 3);
            else if (subtype == CacheSubType.P_Get_Client_Service_Type_List)
                RetValue = GetSeconds(hours: 3);
            else if (subtype == CacheSubType.P_Get_Order_Client_Identifier_Fields_List)
                RetValue = GetSeconds(hours: 3);
            else if (subtype == CacheSubType.P_Get_Client_Special_Service_List)
                RetValue = GetSeconds(hours: 3);
            else if (subtype == CacheSubType.P_Get_Client_Ignore_API_Errors_List)
                RetValue = GetSeconds(hours: 3);
            else if (subtype == CacheSubType.P_Get_Seller_All_MappingList_ReturnJson)
                RetValue = GetSeconds(minutes: 10);
            else if (subtype == CacheSubType.P_Get_SellToClientList)
                RetValue = GetSeconds(minutes: 10);
            else if (subtype == CacheSubType.P_Get_Price_Key)
                RetValue = GetSeconds(minutes: 10);
            else if (subtype == CacheSubType.P_Get_Role_Rights_From_RoleID)
                RetValue = GetSeconds(hours: 6);
            else if (subtype == CacheSubType.P_Get_Role_Rights_From_Username)
                RetValue = GetSeconds(minutes: 20);
            else if (subtype == CacheSubType.P_Is_Has_Right_From_RoleID)
                RetValue = GetSeconds(hours: 6);
            else if (subtype == CacheSubType.P_Is_Has_Right_From_Username)
                RetValue = GetSeconds(hours: 6);
            else if (subtype == CacheSubType.P_Get_User_Order_Detail_Grid_List)
                RetValue = GetSeconds(hours: 6);
            else if (subtype == CacheSubType.P_Get_Pages_Info_By_User)
                RetValue = GetSeconds(hours: 6);
            else if (subtype == CacheSubType.P_GetDBServerForDataRead_2)
                RetValue = GetSeconds(minutes: 5);
            else if (subtype == CacheSubType.GetQuoteAPIToken)
                RetValue = GetSeconds(hours: 24);
            else if (subtype == CacheSubType.P_Get_Order_GUID_By_OrderID)
                RetValue = GetSeconds(minutes: 15);
            else if (subtype == CacheSubType.P_Get_OrderAccess_By_GUID)
                RetValue = GetSeconds(minutes: 15);
            else if (subtype == CacheSubType.P_Get_OrderDetail_Assignment_By_GUID)
                RetValue = GetSeconds(minutes: 15);
            else if (subtype == CacheSubType.P_Get_OrderDetail_BasicSummary_By_GUID)
                RetValue = GetSeconds(minutes: 15);
            else if (subtype == CacheSubType.P_Get_OrderDetail_ClientIdentifier_By_GUID)
                RetValue = GetSeconds(minutes: 15);
            else if (subtype == CacheSubType.P_Get_OrderDetail_SpecialServices_By_GUID)
                RetValue = GetSeconds(minutes: 15);
            else if (subtype == CacheSubType.P_Get_OrderDetail_AccessLog_By_GUID)
                RetValue = GetSeconds(minutes: 15);
            else if (subtype == CacheSubType.P_Get_OrderDetail_ItemDetail_By_GUID)
                RetValue = GetSeconds(minutes: 15);
            else if (subtype == CacheSubType.P_Get_OrderDetail_InvoiceHeader_By_GUID)
                RetValue = GetSeconds(minutes: 15);
            else if (subtype == CacheSubType.P_Get_OrderDetail_InvoiceLine_By_GUID)
                RetValue = GetSeconds(minutes: 15);
            else if (subtype == CacheSubType.P_Get_OrderDetail_Comments_By_GUID)
                RetValue = GetSeconds(minutes: 15);
            else if (subtype == CacheSubType.P_Get_OrderDetail_Events_By_GUID)
                RetValue = GetSeconds(minutes: 15);
            else if (subtype == CacheSubType.P_Get_OrderDetail_Documents_By_GUID)
                RetValue = GetSeconds(minutes: 15);
            else if (subtype == CacheSubType.P_Get_OrderDetail_ScanHistory_By_GUID)
                RetValue = GetSeconds(minutes: 15);
            else if (subtype == CacheSubType.P_Get_OrderDetail_Manifest_By_GUID)
                RetValue = GetSeconds(minutes: 15);
            else if (subtype == CacheSubType.P_Get_OrderDetail_POPPOD_By_GUID)
                RetValue = GetSeconds(minutes: 15);
            else if (subtype == CacheSubType.P_Get_OrderDetail_ChangeLog_By_GUID)
                RetValue = GetSeconds(minutes: 15);
            else if (subtype == CacheSubType.P_Get_Seller_Import_Fields_Name_Setting)
                RetValue = GetSeconds(minutes: 15);
            else if (subtype == CacheSubType.GoogleMapsAPICall)
                RetValue = GetSeconds(hours: 3);
            else if (subtype == CacheSubType.P_Get_SellerBillTo_From_Pinnacle_User)
                RetValue = GetSeconds(hours: 3);
            else if (subtype == CacheSubType.P_Get_ShipmentType_From_ServiceLevel)
                RetValue = GetSeconds(hours: 6);
            else if (subtype == CacheSubType.P_Get_Search_Sku_List)
                RetValue = GetSeconds(minutes: 5);
            else if (subtype == CacheSubType.GoogleAddressValidationAPICall)
                RetValue = GetSeconds(hours: 3);
            else if (subtype == CacheSubType.P_Get_Google_Address_Validation_Json)
                RetValue = GetSeconds(hours: 1);
            else if (subtype == CacheSubType.P_Get_SellerAddressList)
                RetValue = GetSeconds(minutes: 5);
            else if (subtype == CacheSubType.P_Get_AddressList)
                RetValue = GetSeconds(minutes: 5);
            else if (subtype == CacheSubType.P_Get_Import_Order_File_Source_Setup)
                RetValue = GetSeconds(minutes: 5);
            else if (subtype == CacheSubType.P_Get_List_By_ID_2)
                RetValue = GetSeconds(hours: 1);
            else if (subtype == CacheSubType.P_Get_Sub_OrderType_List)
                RetValue = GetSeconds(hours: 1);
            else if (subtype == CacheSubType.GetReportingAPIToken)
                RetValue = GetSeconds(hours: 24);
            else if (subtype == CacheSubType.GetUserPOMSAPIToken)
                RetValue = GetSeconds(hours: 24);
            else if (subtype == CacheSubType.GetAdminPOMSAPIToken)
                RetValue = GetSeconds(hours: 24);

            return RetValue;
        }
        public static List<string>? GetCacheType(string? subtype)
        {
            subtype = (subtype == null ? "" : subtype);
            List<string>? RetValue = new List<string>();

            if (subtype == CacheSubType.P_Get_API_User_Map_Request_Limit)
            {
                RetValue.Add(CacheType.TAPIUserMapRequestLimit);
            }
            else if (subtype == CacheSubType.P_Get_API_User_Map)
            {
                RetValue.Add(CacheType.MetropolitanUserAPIMap);
            }
            else if (subtype == CacheSubType.P_Get_API_RemoteDomain_IP_Request_Limit)
            {
                RetValue.Add(CacheType.TAPIRemoteDomainIPRequestLimit);
            }
            else if (subtype == CacheSubType.P_Get_User_Info)
            {
                RetValue.Add(CacheType.MetropolitanAdditionalApprovers);
                RetValue.Add(CacheType.TUsers);
                RetValue.Add(CacheType.MetropolitanWebUserLogin);
                RetValue.Add(CacheType.MetropolitanTimeZone);
            }
            else if (subtype == CacheSubType.P_Get_API_RemoteDomain_IP_WhiteListing)
            {
                RetValue.Add(CacheType.TApiRemoteDomainIPWhiteListing);
            }
            else if (subtype == CacheSubType.P_Get_T_Config_Detail)
            {
                RetValue.Add(CacheType.TConfig);
            }
            else if (subtype == CacheSubType.P_Get_List_By_ID)
            {
                RetValue.Add(CacheType.MetropolitanCustomer);
                RetValue.Add(CacheType.TMasterTypeValue);
            }
            else if (subtype == CacheSubType.P_Get_Client_Service_Type_List)
            {
                RetValue.Add(CacheType.MetropolitanCustomer);
                RetValue.Add(CacheType.TMasterTypeValue);
                RetValue.Add(CacheType.TServiceType);
                RetValue.Add(CacheType.TSubServiceType);
                RetValue.Add(CacheType.TClientServiceType);
                RetValue.Add(CacheType.TServiceTypeDetail);
            }
            else if (subtype == CacheSubType.P_Get_Order_Client_Identifier_Fields_List)
            {
                RetValue.Add(CacheType.MetropolitanCustomer);
                RetValue.Add(CacheType.TOrderIdentifierFields);
                RetValue.Add(CacheType.TOrderClientIdentifierFields);
            }
            else if (subtype == CacheSubType.P_Get_Client_Special_Service_List)
            {
                RetValue.Add(CacheType.MetropolitanCustomer);
                RetValue.Add(CacheType.TServiceLevelSpecialService);
                RetValue.Add(CacheType.TSpecialServicesList);
                RetValue.Add(CacheType.TClientServiceLevelSpecialService);
            }
            else if (subtype == CacheSubType.P_Get_Client_Ignore_API_Errors_List)
            {
                RetValue.Add(CacheType.MetropolitanCustomer);
                RetValue.Add(CacheType.TIgnoreAPIErrorsList);
                RetValue.Add(CacheType.TClientIgnoreAPIErrorsList);
            }
            else if (subtype == CacheSubType.P_Get_Seller_All_MappingList_ReturnJson)
            {
                RetValue.Add(CacheType.MetropolitanCustomer);
                RetValue.Add(CacheType.TSellerList);
                RetValue.Add(CacheType.TSellerBillToMapping);
                RetValue.Add(CacheType.TSellerPartnerMapping);
                RetValue.Add(CacheType.TSellerPartnerList);
                RetValue.Add(CacheType.TSellerTariffMapping);
                RetValue.Add(CacheType.TTariffList);
            }
            else if (subtype == CacheSubType.P_Get_SellToClientList)
            {
                RetValue.Add(CacheType.TSellerList);
            }
            else if (subtype == CacheSubType.P_Get_Price_Key)
            {
                RetValue.Add(CacheType.MetropolitanCustomer);
                RetValue.Add(CacheType.TSellerList);
                RetValue.Add(CacheType.TSellerBillToMapping);
                RetValue.Add(CacheType.TSellerPartnerMapping);
                RetValue.Add(CacheType.TSellerPartnerList);
                RetValue.Add(CacheType.TSellerTariffMapping);
                RetValue.Add(CacheType.TTariffList);
                RetValue.Add(CacheType.TSellerAllMappingPriceKey);
                RetValue.Add(CacheType.QuotesTPriceKey);
            }
            else if (subtype == CacheSubType.P_Get_Role_Rights_From_RoleID)
            {
                RetValue.Add(CacheType.TGetRoleRightsFromRoleID);
            }
            else if (subtype == CacheSubType.P_Get_Role_Rights_From_Username)
            {
                RetValue.Add(CacheType.TGetRoleRightsFromUsername);
            }
            else if (subtype == CacheSubType.P_Is_Has_Right_From_RoleID)
            {
                RetValue.Add(CacheType.TIsHasRightFromRoleID);
            }
            else if (subtype == CacheSubType.P_Is_Has_Right_From_Username)
            {
                RetValue.Add(CacheType.TIsHasRightFromUsername);
            }
            else if (subtype == CacheSubType.P_Get_User_Order_Detail_Grid_List)
            {
                RetValue.Add(CacheType.TGetUserOrderDetailGridList);
            }
            else if (subtype == CacheSubType.P_Get_Pages_Info_By_User)
            {
                RetValue.Add(CacheType.TGetPagesInfoByUser);
            }
            else if (subtype == CacheSubType.P_GetDBServerForDataRead_2)
            {
                RetValue.Add(CacheType.ReadOnlyServer);
            }
            else if (subtype == CacheSubType.GetQuoteAPIToken)
            {
                RetValue.Add(CacheType.TGetQuoteAPIToken);
            }
            else if (subtype == CacheSubType.P_Get_Order_GUID_By_OrderID)
            {
                RetValue.Add(CacheType.TGetOrderGUIDByOrderID);
            }
            else if (subtype == CacheSubType.P_Get_OrderAccess_By_GUID)
            {
                RetValue.Add(CacheType.TGetOrderAccessByGUID);
            }
            else if (subtype == CacheSubType.P_Get_OrderDetail_Assignment_By_GUID)
            {
                RetValue.Add(CacheType.TGetOrderDetailAssignmentByGUID);
            }
            else if (subtype == CacheSubType.P_Get_OrderDetail_BasicSummary_By_GUID)
            {
                RetValue.Add(CacheType.TGetOrderDetailBasicSummaryByGUID);
            }
            else if (subtype == CacheSubType.P_Get_OrderDetail_ClientIdentifier_By_GUID)
            {
                RetValue.Add(CacheType.TGetOrderDetailClientIdentifierByGUID);
            }
            else if (subtype == CacheSubType.P_Get_OrderDetail_SpecialServices_By_GUID)
            {
                RetValue.Add(CacheType.TGetOrderDetailSpecialServicesByGUID);
            }
            else if (subtype == CacheSubType.P_Get_OrderDetail_AccessLog_By_GUID)
            {
                RetValue.Add(CacheType.TGetOrderDetailAccessLogByGUID);
            }
            else if (subtype == CacheSubType.P_Get_OrderDetail_ItemDetail_By_GUID)
            {
                RetValue.Add(CacheType.TGetOrderDetailItemDetailByGUID);
            }
            else if (subtype == CacheSubType.P_Get_OrderDetail_InvoiceHeader_By_GUID)
            {
                RetValue.Add(CacheType.TGetOrderDetailInvoiceHeaderByGUID);
            }
            else if (subtype == CacheSubType.P_Get_OrderDetail_InvoiceLine_By_GUID)
            {
                RetValue.Add(CacheType.TGetOrderDetailInvoiceLineByGUID);
            }
            else if (subtype == CacheSubType.P_Get_OrderDetail_Comments_By_GUID)
            {
                RetValue.Add(CacheType.TGetOrderDetailCommentsByGUID);
            }
            else if (subtype == CacheSubType.P_Get_OrderDetail_Events_By_GUID)
            {
                RetValue.Add(CacheType.TGetOrderDetailEventsByGUID);
            }
            else if (subtype == CacheSubType.P_Get_OrderDetail_Documents_By_GUID)
            {
                RetValue.Add(CacheType.TGetOrderDetailDocumentsByGUID);
            }
            else if (subtype == CacheSubType.P_Get_OrderDetail_ScanHistory_By_GUID)
            {
                RetValue.Add(CacheType.TGetOrderDetailScanHistoryByGUID);
            }
            else if (subtype == CacheSubType.P_Get_OrderDetail_Manifest_By_GUID)
            {
                RetValue.Add(CacheType.TGetOrderDetailManifestByGUID);
            }
            else if (subtype == CacheSubType.P_Get_OrderDetail_POPPOD_By_GUID)
            {
                RetValue.Add(CacheType.TGetOrderDetailPOPPODByGUID);
            }
            else if (subtype == CacheSubType.P_Get_OrderDetail_ChangeLog_By_GUID)
            {
                RetValue.Add(CacheType.TGetOrderDetailChangeLogByGUID);
            }
            else if (subtype == CacheSubType.P_Get_Seller_Import_Fields_Name_Setting)
            {
                RetValue.Add(CacheType.TGetSellerImportFieldsNameSetting);
            }
            else if (subtype == CacheSubType.GoogleMapsAPICall)
            {
                RetValue.Add(CacheType.TGoogleAddressValidationAPICall);
            }
            else if (subtype == CacheSubType.P_Get_SellerBillTo_From_Pinnacle_User)
            {
                RetValue.Add(CacheType.TGetSellerBillToFromPinnacleUser);
            }
            else if (subtype == CacheSubType.P_Get_ShipmentType_From_ServiceLevel)
            {
                RetValue.Add(CacheType.TGetShipmentTypeFromServiceLevel);
            }
            else if (subtype == CacheSubType.P_Get_Search_Sku_List)
            {
                RetValue.Add(CacheType.TGetSearchSkuList);
            }
            else if (subtype == CacheSubType.GoogleAddressValidationAPICall)
            {
                RetValue.Add(CacheType.TGoogleAddressValidationAPICall);
            }
            else if (subtype == CacheSubType.P_Get_Google_Address_Validation_Json)
            {
                RetValue.Add(CacheType.TGoogleAddressValidationAPICall);
            }
            else if (subtype == CacheSubType.P_Get_SellerAddressList)
            {
                RetValue.Add(CacheType.TGetSellerAddressList);
            }
            else if (subtype == CacheSubType.P_Get_AddressList)
            {
                RetValue.Add(CacheType.TGetAddressList);
            }
            else if (subtype == CacheSubType.P_Get_Import_Order_File_Source_Setup)
            {
                RetValue.Add(CacheType.TGetImportOrderFileSourceSetup);
            }
            else if (subtype == CacheSubType.P_Get_List_By_ID_2)
            {
                RetValue.Add(CacheType.TMasterTypeValue);
            }
            else if (subtype == CacheSubType.P_Get_Sub_OrderType_List)
            {
                RetValue.Add(CacheType.TSubOrderTypeValue);
            }
            else if (subtype == CacheSubType.GetReportingAPIToken)
            {
                RetValue.Add(CacheType.TGetReportingAPIToken);
            }
            else if (subtype == CacheSubType.GetUserPOMSAPIToken)
            {
                RetValue.Add(CacheType.TGetUserPOMSAPIToken);
            }
            else if (subtype == CacheSubType.GetAdminPOMSAPIToken)
            {
                RetValue.Add(CacheType.TGetAdminPOMSAPIToken);
            }

            if (RetValue.Count == 0)
                RetValue = null;
            return RetValue;
        }
        public static List<string>? GetCacheSubType(string? type)
        {
            type = (type == null ? "" : type);
            List<string>? RetValue = new List<string>();

            if (type == CacheType.TAPIUserMapRequestLimit)
            {
                RetValue.Add(CacheSubType.P_Get_API_User_Map_Request_Limit);
            }
            else if (type == CacheType.MetropolitanUserAPIMap)
            {
                RetValue.Add(CacheSubType.P_Get_API_User_Map);
            }
            else if (type == CacheType.TAPIRemoteDomainIPRequestLimit)
            {
                RetValue.Add(CacheSubType.P_Get_API_RemoteDomain_IP_Request_Limit);
            }
            else if (type == CacheType.MetropolitanAdditionalApprovers)
            {
                RetValue.Add(CacheSubType.P_Get_User_Info);
            }
            else if (type == CacheType.TUsers)
            {
                RetValue.Add(CacheSubType.P_Get_User_Info);
            }
            else if (type == CacheType.MetropolitanWebUserLogin)
            {
                RetValue.Add(CacheSubType.P_Get_User_Info);
            }
            else if (type == CacheType.MetropolitanTimeZone)
            {
                RetValue.Add(CacheSubType.P_Get_User_Info);
            }
            else if (type == CacheType.TApiRemoteDomainIPWhiteListing)
            {
                RetValue.Add(CacheSubType.P_Get_API_RemoteDomain_IP_WhiteListing);
            }
            else if (type == CacheType.TConfig)
            {
                RetValue.Add(CacheSubType.P_Get_T_Config_Detail);
            }
            else if (type == CacheType.TMasterTypeValue)
            {
                RetValue.Add(CacheSubType.P_Get_List_By_ID);
                RetValue.Add(CacheSubType.P_Get_List_By_ID_2);
                RetValue.Add(CacheSubType.P_Get_Client_Service_Type_List);
            }
            else if (type == CacheType.MetropolitanCustomer)
            {
                RetValue.Add(CacheSubType.P_Get_List_By_ID);
                RetValue.Add(CacheSubType.P_Get_Client_Service_Type_List);
                RetValue.Add(CacheSubType.P_Get_Order_Client_Identifier_Fields_List);
                RetValue.Add(CacheSubType.P_Get_Client_Special_Service_List);
                RetValue.Add(CacheSubType.P_Get_Client_Ignore_API_Errors_List);
                RetValue.Add(CacheSubType.P_Get_Seller_All_MappingList_ReturnJson);
                RetValue.Add(CacheSubType.P_Get_Price_Key);
            }
            else if (type == CacheType.TServiceType)
            {
                RetValue.Add(CacheSubType.P_Get_Client_Service_Type_List);
            }
            else if (type == CacheType.TSubServiceType)
            {
                RetValue.Add(CacheSubType.P_Get_Client_Service_Type_List);
            }
            else if (type == CacheType.TClientServiceType)
            {
                RetValue.Add(CacheSubType.P_Get_Client_Service_Type_List);
            }
            else if (type == CacheType.TServiceTypeDetail)
            {
                RetValue.Add(CacheSubType.P_Get_Client_Service_Type_List);
            }
            else if (type == CacheType.TOrderIdentifierFields)
            {
                RetValue.Add(CacheSubType.P_Get_Order_Client_Identifier_Fields_List);
            }
            else if (type == CacheType.TOrderClientIdentifierFields)
            {
                RetValue.Add(CacheSubType.P_Get_Order_Client_Identifier_Fields_List);
            }
            else if (type == CacheType.TServiceLevelSpecialService)
            {
                RetValue.Add(CacheSubType.P_Get_Client_Special_Service_List);
            }
            else if (type == CacheType.TSpecialServicesList)
            {
                RetValue.Add(CacheSubType.P_Get_Client_Special_Service_List);
            }
            else if (type == CacheType.TClientServiceLevelSpecialService)
            {
                RetValue.Add(CacheSubType.P_Get_Client_Special_Service_List);
            }
            else if (type == CacheType.TIgnoreAPIErrorsList)
            {
                RetValue.Add(CacheSubType.P_Get_Client_Ignore_API_Errors_List);
            }
            else if (type == CacheType.TClientIgnoreAPIErrorsList)
            {
                RetValue.Add(CacheSubType.P_Get_Client_Ignore_API_Errors_List);
            }
            else if (type == CacheType.TSellerList)
            {
                RetValue.Add(CacheSubType.P_Get_Seller_All_MappingList_ReturnJson);
                RetValue.Add(CacheSubType.P_Get_SellToClientList);
                RetValue.Add(CacheSubType.P_Get_Price_Key);
            }
            else if (type == CacheType.TSellerBillToMapping)
            {
                RetValue.Add(CacheSubType.P_Get_Seller_All_MappingList_ReturnJson);
                RetValue.Add(CacheSubType.P_Get_Price_Key);
            }
            else if (type == CacheType.TSellerPartnerMapping)
            {
                RetValue.Add(CacheSubType.P_Get_Seller_All_MappingList_ReturnJson);
                RetValue.Add(CacheSubType.P_Get_Price_Key);
            }
            else if (type == CacheType.TSellerPartnerList)
            {
                RetValue.Add(CacheSubType.P_Get_Seller_All_MappingList_ReturnJson);
                RetValue.Add(CacheSubType.P_Get_Price_Key);
            }
            else if (type == CacheType.TSellerTariffMapping)
            {
                RetValue.Add(CacheSubType.P_Get_Seller_All_MappingList_ReturnJson);
                RetValue.Add(CacheSubType.P_Get_Price_Key);
            }
            else if (type == CacheType.TTariffList)
            {
                RetValue.Add(CacheSubType.P_Get_Seller_All_MappingList_ReturnJson);
                RetValue.Add(CacheSubType.P_Get_Price_Key);
            }
            else if (type == CacheType.TSellerAllMappingPriceKey)
            {
                RetValue.Add(CacheSubType.P_Get_Price_Key);
            }
            else if (type == CacheType.QuotesTPriceKey)
            {
                RetValue.Add(CacheType.TGetRoleRightsFromRoleID);
            }
            else if (type == CacheSubType.P_Get_Role_Rights_From_RoleID)
            {
                RetValue.Add(CacheType.TIsHasRightFromRoleID);
            }
            else if (type == CacheType.TGetRoleRightsFromUsername)
            {
                RetValue.Add(CacheSubType.P_Get_Role_Rights_From_Username);
            }
            else if (type == CacheSubType.P_Is_Has_Right_From_RoleID)
            {
                RetValue.Add(CacheType.TIsHasRightFromRoleID);
            }
            else if (type == CacheSubType.P_Is_Has_Right_From_Username)
            {
                RetValue.Add(CacheSubType.P_Is_Has_Right_From_Username);
            }
            else if (type == CacheType.TGetUserOrderDetailGridList)
            {
                RetValue.Add(CacheSubType.P_Get_User_Order_Detail_Grid_List);
            }
            else if (type == CacheType.TGetPagesInfoByUser)
            {
                RetValue.Add(CacheSubType.P_Get_Pages_Info_By_User);
            }
            else if (type == CacheType.ReadOnlyServer)
            {
                RetValue.Add(CacheSubType.P_GetDBServerForDataRead_2);
            }
            else if (type == CacheType.TGetQuoteAPIToken)
            {
                RetValue.Add(CacheSubType.GetQuoteAPIToken);
            }
            else if (type == CacheType.TGetOrderGUIDByOrderID)
            {
                RetValue.Add(CacheSubType.P_Get_Order_GUID_By_OrderID);
            }
            else if (type == CacheType.TGetOrderAccessByGUID)
            {
                RetValue.Add(CacheSubType.P_Get_OrderAccess_By_GUID);
            }
            else if (type == CacheType.TGetOrderDetailAssignmentByGUID)
            {
                RetValue.Add(CacheSubType.P_Get_OrderDetail_Assignment_By_GUID);
            }
            else if (type == CacheType.TGetOrderDetailBasicSummaryByGUID)
            {
                RetValue.Add(CacheSubType.P_Get_OrderDetail_BasicSummary_By_GUID);
            }
            else if (type == CacheType.TGetOrderDetailClientIdentifierByGUID)
            {
                RetValue.Add(CacheSubType.P_Get_OrderDetail_ClientIdentifier_By_GUID);
            }
            else if (type == CacheType.TGetOrderDetailClientIdentifierByGUID)
            {
                RetValue.Add(CacheSubType.P_Get_OrderDetail_SpecialServices_By_GUID);
            }
            else if (type == CacheType.TGetOrderDetailAccessLogByGUID)
            {
                RetValue.Add(CacheSubType.P_Get_OrderDetail_AccessLog_By_GUID);
            }
            else if (type == CacheType.TGetOrderDetailItemDetailByGUID)
            {
                RetValue.Add(CacheSubType.P_Get_OrderDetail_ItemDetail_By_GUID);
            }
            else if (type == CacheType.TGetOrderDetailInvoiceHeaderByGUID)
            {
                RetValue.Add(CacheSubType.P_Get_OrderDetail_InvoiceHeader_By_GUID);
            }
            else if (type == CacheType.TGetOrderDetailInvoiceLineByGUID)
            {
                RetValue.Add(CacheSubType.P_Get_OrderDetail_InvoiceLine_By_GUID);
            }
            else if (type == CacheType.TGetOrderDetailCommentsByGUID)
            {
                RetValue.Add(CacheSubType.P_Get_OrderDetail_Comments_By_GUID);
            }
            else if (type == CacheType.TGetOrderDetailEventsByGUID)
            {
                RetValue.Add(CacheSubType.P_Get_OrderDetail_Events_By_GUID);
            }
            else if (type == CacheType.TGetOrderDetailDocumentsByGUID)
            {
                RetValue.Add(CacheSubType.P_Get_OrderDetail_Documents_By_GUID);
            }
            else if (type == CacheType.TGetOrderDetailScanHistoryByGUID)
            {
                RetValue.Add(CacheSubType.P_Get_OrderDetail_ScanHistory_By_GUID);
            }
            else if (type == CacheType.TGetOrderDetailManifestByGUID)
            {
                RetValue.Add(CacheSubType.P_Get_OrderDetail_Manifest_By_GUID);
            }
            else if (type == CacheType.TGetOrderDetailPOPPODByGUID)
            {
                RetValue.Add(CacheSubType.P_Get_OrderDetail_POPPOD_By_GUID);
            }
            else if (type == CacheType.TGetOrderDetailChangeLogByGUID)
            {
                RetValue.Add(CacheSubType.P_Get_OrderDetail_ChangeLog_By_GUID);
            }
            else if (type == CacheType.TGetSellerImportFieldsNameSetting)
            {
                RetValue.Add(CacheSubType.P_Get_Seller_Import_Fields_Name_Setting);
            }
            else if (type == CacheType.TGoogleAddressValidationAPICall)
            {
                RetValue.Add(CacheSubType.GoogleMapsAPICall);
            }
            else if (type == CacheType.TGetSellerBillToFromPinnacleUser)
            {
                RetValue.Add(CacheSubType.P_Get_SellerBillTo_From_Pinnacle_User);
            }
            else if (type == CacheType.TGetShipmentTypeFromServiceLevel)
            {
                RetValue.Add(CacheSubType.P_Get_ShipmentType_From_ServiceLevel);
            }
            else if (type == CacheType.TGetSearchSkuList)
            {
                RetValue.Add(CacheSubType.P_Get_Search_Sku_List);
            }
            else if (type == CacheType.TGoogleAddressValidationAPICall)
            {
                RetValue.Add(CacheSubType.GoogleAddressValidationAPICall);
            }
            else if (type == CacheType.TGoogleAddressValidationAPICall)
            {
                RetValue.Add(CacheSubType.P_Get_Google_Address_Validation_Json);
            }
            else if (type == CacheType.TGetSellerAddressList)
            {
                RetValue.Add(CacheSubType.P_Get_SellerAddressList);
            }
            else if (type == CacheType.TGetAddressList)
            {
                RetValue.Add(CacheSubType.P_Get_AddressList);
            }
            else if (type == CacheType.TGetImportOrderFileSourceSetup)
            {
                RetValue.Add(CacheSubType.P_Get_Import_Order_File_Source_Setup);
            }
            else if (type == CacheType.TSubOrderTypeValue)
            {
                RetValue.Add(CacheSubType.P_Get_Sub_OrderType_List);
            }
            else if (type == CacheType.TGetReportingAPIToken)
            {
                RetValue.Add(CacheSubType.GetReportingAPIToken);
            }
            else if (type == CacheType.TGetUserPOMSAPIToken)
            {
                RetValue.Add(CacheSubType.GetUserPOMSAPIToken);
            }
            else if (type == CacheType.TGetAdminPOMSAPIToken)
            {
                RetValue.Add(CacheSubType.GetAdminPOMSAPIToken);
            }

            if (RetValue.Count == 0)
                RetValue = null;
            return RetValue;
        }
    }
}

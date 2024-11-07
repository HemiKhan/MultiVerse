using AutoMapper;
using Data.Dtos;
using Data.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Serilog.Context;

namespace Data.DataAccess
{
    public static class StaticPublicObjects
    {
        private static ILogFile _logFile;
        public static ILogFile logFile
        {
            get
            {
                return _logFile;
            }
            set
            {
                if (_logFile == null) _logFile = value;
            }
        }
        private static ILogger _logger;
        public static ILogger logger
        {
            get
            {
                return _logger;
            }
            set
            {
                if (_logger == null) _logger = value;
            }
        }
        private static IADORepository _ado;
        public static IADORepository ado
        {
            get
            {
                return _ado;
            }
            set
            {
                if (_ado == null) _ado = value;
            }
        }
        public static string? subdomain { get; set; } = null;
        private static IMapper _map;
        public static IMapper map
        {
            get
            {
                return _map;
            }
            set
            {
                if (_map == null) _map = value;
            }
        }
        private static IMemoryCache _memorycache;
        public static IMemoryCache memorycache
        {
            get
            {
                return _memorycache;
            }
            set
            {
                if (_memorycache == null) _memorycache = value;
            }
        }
        public static string? virtualdirectory { get; set; } = null;
        public static string? adminswaggerkey { get; set; } = null;
        public static bool iscachingenable { get; set; } = false;
        public static string? swaggerhiddenversion { get; set; } = null;
        public static AllowedDomainListModel? allowdomainlist { get; set; } = null;
        public static AllowedDomainExcludingSubDomainListModel? allowdomainexcludingsubdomainlist { get; set; } = null;

    }
    public class PublicClaimObjects
    {
        public DateTime requeststarttime { get; set; } = DateTime.UtcNow;
        private string _username = "";
        public string username
        {
            get
            {
                return _username;
            }
            set
            {
                string Ret = "";
                if (value != null)
                    Ret = value.ToUpper();
                _username = Ret;
            }
        }
        public string jit { get; set; } = "";
        public string key { get; set; } = "";
        public bool iswebtoken { get; set; } = false;
        public bool isdevelopment { get; set; } = false;
        private string _appsettingfilename = "appsettings.json";
        public string appsettingfilename
        {
            get
            {
                return _appsettingfilename;
            }
            set
            {
                string Ret = "appsettings.json";
                if (value == "appsettings.Development.json")
                    Ret = value;
                _appsettingfilename = Ret;
            }
        }
        public bool isswaggercall { get; set; } = false;
        public bool isswaggercalladmin { get; set; } = false;
        public string path { get; set; } = "";
        public string hostname { get; set; } = "";
        public string hosturl { get; set; } = "";
        public string remotedomain { get; set; } = "";
        public P_Get_User_Info? P_Get_User_Info { get; set; } = null;
    }
    public class Error
    {
        public int StatusCode { get; set; }
        public string Message { get; set; }
        public override string ToString() => JsonConvert.SerializeObject(this);
    }
    public class LoggingMiddleware
    {
        private readonly RequestDelegate _next;

        public LoggingMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            using (LogContext.PushProperty("Url", context.Request.GetDisplayUrl()))
            {
                await _next(context);
            }
        }
    }
}

using Data.Dtos;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;

namespace Data.Swaggers
{
    public class RestrictDomainFilter : Attribute, IActionFilter
    {
        private readonly AllowedDomainListModel _config;
        private readonly string _param;
        private readonly HttpContextAccessor _httpContextAccessor;
        public RestrictDomainFilter(string param, AllowedDomainListModel config, HttpContextAccessor httpContextAccessor)
        {
            _config = config;
            _param = param;
            _httpContextAccessor = httpContextAccessor;
        }

        public void OnActionExecuting(ActionExecutingContext context)
        {
            string? remoteurl = "";
            try
            {
                remoteurl = _httpContextAccessor?.HttpContext?.Request?.Headers?.Referer.ToString();
                remoteurl = (remoteurl == null ? "" : (remoteurl.ToString() == "null" ? "" : remoteurl.ToString().ToLower()));
            }
            catch (Exception ex)
            {
                //StaticPublicObjects.logFile.ErrorLog(FunctionName: "OnActionExecuting", SmallMessage: ex.Message, Message: ex.ToString());
            }

            try
            {

                if ((_param == "CreateOrder") && !_config.AllowedRemoteDomainAngular.Contains(remoteurl, StringComparer.OrdinalIgnoreCase))
                {
                    context.Result = new BadRequestObjectResult("RemoteDomain is not allowed");
                }
            }
            catch (Exception ex)
            {
                //StaticPublicObjects.logFile.ErrorLog(FunctionName: "OnActionExecuting 1", SmallMessage: ex.Message, Message: ex.ToString());
            }
        }

        public void OnActionExecuted(ActionExecutedContext context)
        {
            // Do something after the action has been executed
        }
    }
}

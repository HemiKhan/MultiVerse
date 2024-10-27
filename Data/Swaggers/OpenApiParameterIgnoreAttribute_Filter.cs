using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Reflection;
using System.Runtime.Serialization;

namespace Data.Swaggers
{
    public class MySwaggerSchemaFilter : ISchemaFilter
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly string _swaggeradminkey;
        private readonly string _swaggerhiddenversion;
        private readonly List<string> _adminswaggeripwhitelist;
        public MySwaggerSchemaFilter(IHttpContextAccessor httpContextAccessor, IConfiguration iconfig)
        {
            this._httpContextAccessor = httpContextAccessor;
            this._swaggeradminkey = iconfig.GetValue<string>("AdminSwaggerKey").ToLower();
            this._swaggerhiddenversion = iconfig.GetValue<string>("SwaggerHiddenVersion").ToLower();
            this._adminswaggeripwhitelist = iconfig.GetSection("AdminSwaggerIPWhiteList").GetChildren().Select(x => x.Value).ToList();
        }
        public void Apply(OpenApiSchema schema, SchemaFilterContext context)
        {
            if (schema?.Properties == null)
            {
                return;
            }

            bool isadmin = (_httpContextAccessor.HttpContext.Request.Headers.Referer.ToString().ToLower().IndexOf("id=" + _swaggeradminkey) >= 0 ? true : false);

            if (isadmin)
                isadmin = (_adminswaggeripwhitelist.Contains(_httpContextAccessor.HttpContext.Connection.RemoteIpAddress.ToString()));

            var ignoreDataMemberProperties = context.Type.GetProperties()
                .Where(t => t.GetCustomAttribute<IgnoreDataMemberAttribute>() != null);

            foreach (var ignoreDataMemberProperty in ignoreDataMemberProperties)
            {
                var propertyToHide = schema.Properties.Keys
                    .SingleOrDefault(x => x.ToLower() == ignoreDataMemberProperty.Name.ToLower());

                if (propertyToHide != null)
                {
                    if (isadmin)
                    { }
                    else
                        schema.Properties.Remove(propertyToHide);
                }
            }
        }
    }
}

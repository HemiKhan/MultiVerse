using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using Data.Dtos;
using Asp.Versioning.ApiExplorer;

namespace MultiVerse_API.Extentions.SwaggerAndApiVersioning
{
    public class FilterRoutesDocumentFilter : IDocumentFilter
	{
        private readonly IHttpContextAccessor  _httpContextAccessor;
        private readonly string _swaggeradminkey;
        private readonly string _swaggerhiddenversion;
        private readonly IApiVersionDescriptionProvider _provider;
        private readonly List<string> _adminswaggeripwhitelist;
        private readonly SwaggerHideURLs _swaggerhideurls;
        //private readonly ApiVersionDescription _description;
        public FilterRoutesDocumentFilter(IHttpContextAccessor httpContextAccessor, IConfiguration iconfig, IApiVersionDescriptionProvider provider, SwaggerHideURLs swaggerhideurls)
        {
            this._httpContextAccessor = httpContextAccessor;
            //this._description = description;
            this._swaggeradminkey = iconfig.GetValue<string>("AdminSwaggerKey")!.ToLower();
            this._swaggerhiddenversion = iconfig.GetValue<string>("SwaggerHiddenVersion")!.ToLower();
            this._provider = provider;
            this._swaggerhideurls = swaggerhideurls;
            this._adminswaggeripwhitelist = iconfig.GetSection("AdminSwaggerIPWhiteList").GetChildren().Select(x => x.Value).ToList()!;
        }
        #region Summary 
        // as we have two routes for every end point so swagger will show both routes for every end point
        // but in our case we dont want to show both routes in swagger we want to show only path with version in swaggr
        //but other simple path can be hit from postman but wont be shown in swagger 
        //so here we are filtering all available routes and only show routes that contain version in its path
        #endregion

        public void Apply(OpenApiDocument swaggerDoc, DocumentFilterContext context)
		{
            bool isadmin = (_httpContextAccessor.HttpContext!.Request.Headers.Referer.ToString().IndexOf("id=" + _swaggeradminkey) >= 0 ? true : false); ;

            if (isadmin)
                isadmin = (_adminswaggeripwhitelist.Contains(_httpContextAccessor.HttpContext.Connection.RemoteIpAddress!.ToString()));

            string[]? VersionsList = null;
            if (swaggerDoc.Info.Version.Split('.')[0] == "1")
                VersionsList = _swaggerhideurls.Version1;
            else if (swaggerDoc.Info.Version.Split('.')[0] == "78652140")
                VersionsList = _swaggerhideurls.Version78652140;

            var nonMobileRoutes = swaggerDoc.Paths
                .Where(x => x.Key.ToLower().Contains("/api/v" + swaggerDoc.Info.Version.Split('.')[0].ToString() + "/"))
                .ToList();
            nonMobileRoutes.ForEach(x => { swaggerDoc.Paths.Remove(x.Key); });
            if (VersionsList != null)
            {
                for (int i = 0; i <= VersionsList.Length - 1; i++)
                {
                    var removenonMobileRoutes = swaggerDoc.Paths
                    .Where(x => x.Key.ToLower().Equals("/api" + VersionsList[i].ToLower()))
                    .ToList();
                    removenonMobileRoutes.ForEach(x => { swaggerDoc.Paths.Remove(x.Key); });
                }
                
            }

            if (swaggerDoc.Info.Version.Split('.')[0] == _swaggerhiddenversion && isadmin == false)
                swaggerDoc.Paths.Clear();
            
            if (swaggerDoc.Info.Version.Split('.')[0] == _swaggerhiddenversion)
                swaggerDoc.Info.Description = $"Code To Cure All APIs.";
            else
                swaggerDoc.Info.Description = $"Code To Cure API Version {swaggerDoc.Info.Version.Split('.')[0]}.";

            swaggerDoc.Info.Description += " <br><strong>Only TLS1.2 Protocol is Supported.</strong>";

            if (_provider.ApiVersionDescriptions.Count > 0)
            {
                for (var i = 0; i <= _provider.ApiVersionDescriptions.Count - 1; i++)
                {
                    if (_provider.ApiVersionDescriptions[i].IsDeprecated && _provider.ApiVersionDescriptions[i].ApiVersion.ToString() == swaggerDoc.Info.Version)
                        swaggerDoc.Info.Description += " <br><span>This API version has been deprecated.</span>";
                }

            }

            if (isadmin)
                swaggerDoc.Info.Description += " <br><span>YOU ARE USING ADMIN LINK.</span>";

            var VersionList = _provider.ApiVersionDescriptions.ToList();
            if (VersionList.Count > 1 && swaggerDoc.Info.Extensions.Count == 0 )
            {
                swaggerDoc.Info.Description += " <br><br><strong>Following Versions are Available. You Can Use Below Version to Explore Swagger Documentation.</strong>";
                swaggerDoc.Info.Description += " <div><ul>";
                for (var i = 0; i <= VersionList.Count - 1; i++)
                {
                    if (VersionList[i].ApiVersion.MajorVersion.ToString() == _swaggerhiddenversion && isadmin)
                        swaggerDoc.Info.Description += $" <li>All Admin APIs List = v{VersionList[i].ApiVersion.MajorVersion.ToString()}</li>";
                    else if (VersionList[i].ApiVersion.MajorVersion.ToString() != _swaggerhiddenversion)
                        swaggerDoc.Info.Description += $" <li>Version {VersionList[i].ApiVersion.MajorVersion.ToString()} = v{VersionList[i].ApiVersion.MajorVersion.ToString()}</li>";
                }
                swaggerDoc.Info.Description += "</ul></div>";
            }

        }
	}
}

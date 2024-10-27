using Asp.Versioning.ApiExplorer;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace MultiVerse_API.Extentions.SwaggerAndApiVersioning
{
	public class ConfigureSwaggerOptions : IConfigureNamedOptions<SwaggerGenOptions>
	{
        private readonly IApiVersionDescriptionProvider _provider;
        public ConfigureSwaggerOptions(IHttpContextAccessor httpContextAccessor, IConfiguration iconfig, IApiVersionDescriptionProvider provider)
        {
            this._provider = provider;
        }
        public void Configure(string name, SwaggerGenOptions options)
		{
			Configure(options);
		}

		public void Configure(SwaggerGenOptions options)
		{
			// add swagger document for every API version discovered
			foreach (var description in _provider.ApiVersionDescriptions)
			{
				options.SwaggerDoc(
					description.GroupName,
					CreateVersionInfo(description));
			}
		}

		private OpenApiInfo CreateVersionInfo(ApiVersionDescription description)
		{
			var info = new OpenApiInfo()
			{
				Title = "MultiVerse API",
				Version = description.ApiVersion.ToString(),
                Description = "",
                    Contact = new OpenApiContact
                    {
                        Name = "Multi Verse",
                        Email = string.Empty,
                    },
                    License = new OpenApiLicense
                    {
                        Name = "Use under LICX",
                    }
            };

            return info;
		}
	}
}

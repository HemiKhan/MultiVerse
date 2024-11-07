using Microsoft.OpenApi.Models;
using Data.Swaggers;
using Asp.Versioning;

namespace MultiVerse_API.Extentions.SwaggerAndApiVersioning
{
    public static class SwaggerConfiguration_Extention
    {
        public static void ConfigureSwagger(this IServiceCollection services)
        {
            //API versioning 
            services.AddApiVersioning(options =>
            {
                options.AssumeDefaultVersionWhenUnspecified = true; // set this to true if you want to set to consider default api version when api version is not mentioned
                options.DefaultApiVersion = new ApiVersion(1, 0); // default api version
                options.ReportApiVersions = false;
            }).AddApiExplorer(
                options =>
                {
                    options.GroupNameFormat = "'v'VVV";
                    options.SubstituteApiVersionInUrl = true;
                }
            );


            //Swagger
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen(c =>
            {
                c.OperationFilter<RemoveVersionParameterFilter>();
                c.OperationFilter<AddAuthorizationHeaderFilter>();
                c.EnableAnnotations();
                c.SchemaFilter<SwaggerSchemaExampleFilter>();
                c.SchemaFilter<MySwaggerSchemaFilter>();
                c.DocumentFilter<FilterRoutesDocumentFilter>();
                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme()
                {
                    Name = "Authorization",
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Bearer",
                    BearerFormat = "JWT",
                    In = ParameterLocation.Header,
                    Description = "JWT Authorization header using the Bearer scheme. \r\n\r\n Enter 'Bearer' [space] and then your token in the text input below.\r\n\r\nExample: \"Bearer 12345abcdef\"",
                });
                c.AddSecurityRequirement(new OpenApiSecurityRequirement
                                {
                                {
                                      new OpenApiSecurityScheme
                                        {
                                            Reference = new OpenApiReference
                                            {
                                                Type = ReferenceType.SecurityScheme,
                                                Id = "Bearer"
                                            }
                                        },
                                        new string[] {}

                                }
                                });
                c.ResolveConflictingActions(apiDesc => apiDesc.First());
            });
            services.ConfigureOptions<ConfigureSwaggerOptions>();
            services.AddSwaggerGenNewtonsoftSupport();
        }
    }
}

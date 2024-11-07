using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Newtonsoft.Json;
using Serilog;
using Serilog.Events;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using AutoMapper;
using Microsoft.Extensions.Caching.Memory;
using Asp.Versioning.ApiExplorer;
using MultiVerse_API.Extensions;
using MultiVerse_API.Extentions.SwaggerAndApiVersioning;
using Data.Dtos;
using Data.AppContext;
using Data.Interfaces;
using MultiVerse_API.Helper;
using Data.DataAccess;

internal class Program
{
    private static void Main(string[] args)
    {
        Log.Logger = new LoggerConfiguration()
    .Enrich.FromLogContext()
    .WriteTo.File(
    path: AppDomain.CurrentDomain.BaseDirectory + "ErrorLog\\log-.txt",
    outputTemplate: "Timestamp: {Timestamp:yyyy-MM-dd HH:mm:ss.fff zzz} Level: [{Level:u3}] Url: {Url}{NewLine}Msg: {Message:1j}{NewLine}Exception: {Exception}{NewLine}{NewLine}",
    rollingInterval: RollingInterval.Hour,
    restrictedToMinimumLevel: LogEventLevel.Warning
    ).CreateLogger();

        var builder = WebApplication.CreateBuilder(args);

        builder.Host.UseSerilog();

        builder.WebHost.ConfigureKestrel(options =>
        {
            options.AddServerHeader = false;
            options.ConfigureHttpsDefaults(httpsOptions =>
            {
                httpsOptions.SslProtocols = System.Security.Authentication.SslProtocols.Tls12;
            });
        });

        builder.Services.AddMemoryCache();

        //builder.Services.AddFluentValidationAutoValidation();
        //builder.Services.AddValidatorsFromAssemblyContaining<Program>();

        builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

        string SwaggerHiddenVersion = builder.Configuration.GetSection("SwaggerHiddenVersion").Value!;

        // Add services to the container.
        builder.Services.AddAuthentication(option =>
        {
            option.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            option.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;

        })
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,//if False then that the expired token is considered valid
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = builder.Configuration["Jwt:Issuer"],
                    ValidAudience = builder.Configuration["Jwt:Issuer"],
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]!))//key must be min 16 chars
                };
                options.Events = new JwtBearerEvents
                {
                    OnAuthenticationFailed = c =>
                    {
                        if (c.Exception.GetType() == typeof(SecurityTokenExpiredException))
                        {
                            return Task.CompletedTask;
                        }
                        else
                        {
                            return Task.CompletedTask;
                        }
                    },
                    OnChallenge = c =>
                    {
                        string responsetext = "";
                        OnChallenge.GetOnChallenge(c, ref responsetext, SwaggerHiddenVersion);
                        c.HandleResponse();
                        c.Response.StatusCode = 401;
                        c.Response.ContentType = "application/json";
                        return c.Response.WriteAsync(responsetext);
                    }
                };
            });

        builder.Services.AddCors(options =>
        {
            options.AddPolicy("AnyOrigin", builder =>
            {
                builder
                    .AllowAnyOrigin()
                    .AllowAnyMethod()
                    .AllowAnyHeader();
            });
        });

        builder.Services.AddControllers().ConfigureApiBehaviorOptions(options =>
        {
            options.SuppressModelStateInvalidFilter = true;
        });

        // using AutoMapper
        builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

        builder.Services.AddControllers().AddNewtonsoftJson(options =>
            {
                options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
            });

        builder.Services.AddSingleton<JsonSerializerSettings>(options =>
            options.GetRequiredService<IOptions<MvcNewtonsoftJsonOptions>>().Value.SerializerSettings);

        //dbConnection Get from appsetting.json
        DbStringCollection dbStringCollection = new DbStringCollection()
        {
            Default_ConnectionModel = builder.Configuration.GetSection(nameof(Default_ConnectionModel)).Get<Default_ConnectionModel>(),
            MultiVerse_ConnectionModel = builder.Configuration.GetSection(nameof(MultiVerse_ConnectionModel)).Get<MultiVerse_ConnectionModel>(),
        };
        builder.Services.AddSingleton(dbStringCollection);
        builder.Services.AddSignalR();

        builder.Services.ApplicationDI(dbStringCollection);

        //Restric Domain Attribute Model
        AllowedDomainListModel allowedDomainListModel = builder.Configuration.GetSection("AllowedDomains").Get<AllowedDomainListModel>()!;
        builder.Services.AddSingleton(allowedDomainListModel);
        AllowedDomainExcludingSubDomainListModel allowedDomainExcludingSubDomainListModel = builder.Configuration.GetSection("AllowedDomainsExcludingSubDomain").Get<AllowedDomainExcludingSubDomainListModel>()!;
        builder.Services.AddSingleton(allowedDomainExcludingSubDomainListModel);
        AllowedMobileAppKeysModel allowedMobileAppKeysModel = builder.Configuration.GetSection("AllowedMobileAppKeysList").Get<AllowedMobileAppKeysModel>()!;
        builder.Services.AddSingleton(allowedMobileAppKeysModel);
        AllowedWebAppKeysModel allowedWebAppKeysModel = builder.Configuration.GetSection("AllowedWebAppKeysList").Get<AllowedWebAppKeysModel>()!;
        builder.Services.AddSingleton(allowedWebAppKeysModel);

        SwaggerHideURLs swaggerHideURLs = builder.Configuration.GetSection("SwaggerHideURLs").Get<SwaggerHideURLs>()!;
        builder.Services.AddSingleton(swaggerHideURLs);

        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        builder.Services.ConfigureSwagger();

        var app = builder.Build();

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
        }
        string virDir = builder.Configuration.GetSection("VirtualDirectory").Value!;

        app.UseStaticFiles();

        app.UseMiddleware<RequestStartMiddleware>();
        app.UseMiddleware<LoggingMiddleware>();
        app.UseMiddleware<CustomContractResolverMiddleware>();
        app.UseSwagger();
        app.UseSwaggerUI(options =>
        {
            //// incase of single api version use below code 
            //options.DefaultModelsExpandDepth(-1);
            //options.SwaggerEndpoint("v1/swagger.json", "MultiVerse API v1");

            ////incase of multiple api versions use this 
            IApiVersionDescriptionProvider provider = app.Services.GetRequiredService<IApiVersionDescriptionProvider>();
            foreach (var description in provider.ApiVersionDescriptions)
            {
                if (description.GroupName != "v" + SwaggerHiddenVersion)
                    options.SwaggerEndpoint($"{description.GroupName}/swagger.json", "MultiVerse" + description.GroupName);
            }
            options.DefaultModelsExpandDepth(-1);
        });

        app.UseExceptionHandler(error =>
        {
            error.Run(async context =>
            {
                context.Response.StatusCode = StatusCodes.Status500InternalServerError;
                context.Response.ContentType = "application/json";
                var contextFeature = context.Features.Get<IExceptionHandlerFeature>();
                if (contextFeature != null)
                {
                    Log.Error(contextFeature.Error, $"{contextFeature.Error.Message}");
                    Log.CloseAndFlush();
                    string responsetext = "";
                    string ErrorMessage = ErrorList.ErrorListInternalServerError.ErrorMsg;
                    string ErrorCode = ErrorList.ErrorListInternalServerError.ErrorCode;
                    int StatusCode = ErrorList.ErrorListInternalServerError.StatusCode;
                    string _Path = StaticPublicObjects.ado.GetRequestPath();
                    int MethodID = StaticPublicObjects.ado.GetMethodIDFromPath();
                    string source_ = AppEnum.APISourceName.GetLogAllFailedRequest;
                    string RequestString = "";
                    string RefNo = "";
                    OnChallenge.GetResponseText(_Path, ErrorCode, StatusCode, ErrorMessage, MethodID, source_, ref responsetext, RequestString, RefNo);
                    StaticPublicObjects.logFile.ErrorLog(FunctionName: "Global Error; Path" + _Path, SmallMessage: contextFeature.Error.Message, Message: contextFeature.Error.ToString());
                    await context.Response.WriteAsync(responsetext);
                }
            });

        });

        app.UseHttpsRedirection();
        app.UseCors("AnyOrigin");
        app.UseAuthentication();
        app.UseAuthorization();
        app.MapControllers();

        using (var serviceScope = app.Services.CreateScope())
        {
            var serviceProvider = serviceScope.ServiceProvider;

            try
            {
                var logFile = serviceProvider.GetService<ILogFile>();
                var ado = serviceProvider.GetService<IADORepository>();
                var map = serviceProvider.GetService<IMapper>();
                var memorycache = serviceProvider.GetService<IMemoryCache>();

                SetPublicObjectsAPI.SetStaticPublicObjects(logFile!, ado!, map!, memorycache!);
            }
            catch (Exception ex)
            {
                StaticPublicObjects.logFile.ErrorLog(FunctionName: "Program", SmallMessage: ex.Message, Message: ex.ToString());
            }
        }

        app.Use(async (context, next) =>
        {
            if (context.Response.HasStarted == false)
            {
                // Get the serializer settings from the service provider
                var serializerSettings = (JsonSerializerSettings)context.RequestServices.GetService(typeof(JsonSerializerSettings))!;

                // Modify the serializer settings based on some logic
                if (StaticPublicObjects.ado.IsSwaggerCallAdmin())
                {
                    serializerSettings.ContractResolver = new CustomContractResolverNone(false, false);
                }
                else
                {
                    serializerSettings.ContractResolver = new CustomContractResolverHideProperty(true);
                }
            }

            PublicClaimObjects _PublicClaimObjects = SetPublicObjects.SetPublicClaimObjects();
            context.Items["PublicClaimObjects"] = _PublicClaimObjects;
            try
            {
                _PublicClaimObjects.requeststarttime = Convert.ToDateTime(context.Items["RequestStartTime"]);
            }
            catch (Exception ex)
            {
                StaticPublicObjects.logFile.ErrorLog(FunctionName: "Program Set RequestStartTime", SmallMessage: ex.Message, Message: ex.ToString());
            }

            APILimitResponse Ret = new APILimitResponse();
            string responsetext = "";
            Ret = ValidateUserToken.GetValidateUserToken(context, ref _PublicClaimObjects, ref responsetext);

            if (Ret.response_code == false)
            {
                context.Response.StatusCode = Ret.statuscode;
                context.Response.ContentType = "application/json";
                context.Response.WriteAsync(responsetext);
                return;
            }

            APIWhiteListingResponse whitelistingresponse = new APIWhiteListingResponse();
            CustomWhiteListing.VerifyWhiteListing(AppEnum.ApplicationId.AppID, ref whitelistingresponse);

            if (whitelistingresponse.response_code == false)
            {
                responsetext = "";
                LogAllFailedRequest.GetLogAllFailedRequest(whitelistingresponse, context, _PublicClaimObjects, ref responsetext);
                context.Response.StatusCode = 401;
                context.Response.ContentType = "application/json";
                context.Response.WriteAsync(responsetext);
                return;
            }

            // Call the next middleware in the pipeline
            await next.Invoke();
        });

        //app.MapHub<ChatHub>("/chatHub");

        app.Run();

    }
}
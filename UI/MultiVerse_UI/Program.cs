using AutoMapper;
using MultiVerse_UI.Models;
using Data.DataAccess;
using Data.Dtos;
using Data.Interfaces;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Serilog.Events;
using Serilog;
using Data.AppContext;
using MultiVerse_UI.Extensions;

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

        builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

        string SwaggerHiddenVersion = builder.Configuration.GetSection("SwaggerHiddenVersion").Value!;

        // Add services to the container.

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

        builder.Services.AddControllersWithViews();
        builder.Services.AddDistributedMemoryCache();

        builder.Services.AddSession(options =>
        {
            options.IdleTimeout = TimeSpan.FromMinutes(AppEnum.WebTokenExpiredTime.Minutes);
            options.Cookie.HttpOnly = true;
            options.Cookie.IsEssential = true;
        });

        builder.Services.AddControllers().ConfigureApiBehaviorOptions(options =>
        {
            options.SuppressModelStateInvalidFilter = true;
        });

        // using AutoMapper
        builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

        builder.Services.AddControllers(
            ).AddNewtonsoftJson(options =>
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

        //DI Resolver for Database Connection
        builder.Services.ApplicationDI(dbStringCollection);
        builder.Services.AddOtherServices();

        //Restric Domain Attribute Model
        AllowedDomainListModel allowedDomainListModel = builder.Configuration.GetSection("AllowedDomains").Get<AllowedDomainListModel>();
        builder.Services.AddSingleton(allowedDomainListModel);
        AllowedDomainExcludingSubDomainListModel allowedDomainExcludingSubDomainListModel = builder.Configuration.GetSection("AllowedDomainsExcludingSubDomain").Get<AllowedDomainExcludingSubDomainListModel>();
        builder.Services.AddSingleton(allowedDomainExcludingSubDomainListModel);
        AllowedMobileAppKeysModel allowedMobileAppKeysModel = builder.Configuration.GetSection("AllowedMobileAppKeysList").Get<AllowedMobileAppKeysModel>();
        builder.Services.AddSingleton(allowedMobileAppKeysModel);
        AllowedWebAppKeysModel allowedWebAppKeysModel = builder.Configuration.GetSection("AllowedWebAppKeysList").Get<AllowedWebAppKeysModel>();
        builder.Services.AddSingleton(allowedWebAppKeysModel);

        SwaggerHideURLs swaggerHideURLs = builder.Configuration.GetSection("SwaggerHideURLs").Get<SwaggerHideURLs>();
        builder.Services.AddSingleton(swaggerHideURLs);

        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        //builder.Services.ConfigureSwagger();

        var app = builder.Build();

        // Configure the HTTP request pipeline.
        //if (app.Environment.IsDevelopment())
        //{
        //    app.UseDeveloperExceptionPage();
        //}
        string virDir = builder.Configuration.GetSection("VirtualDirectory").Value;

        app.UseSession();
        app.UseStaticFiles();

        app.UseMiddleware<RequestStartMiddleware>();
        app.UseMiddleware<LoggingMiddleware>();
        app.UseMiddleware<CustomContractResolverMiddleware>();

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
                    string _Path = StaticPublicObjects.ado.GetRequestPath();
                    StaticPublicObjects.logFile.ErrorLog(FunctionName: "Global Error; Path" + _Path, SmallMessage: contextFeature.Error.Message, Message: contextFeature.Error.ToString());

                    string ID = "";
                    Exception exception = new Exception("Internal Server Error");
                    string CurrentURL = StaticPublicObjects.ado.GetRequestPath();
                    ID = context.Session.SetupSessionError("Error", "~/" + CurrentURL, CurrentURL, exception);
                    context.Response.Redirect($"/Error/Index?ID={ID}");
                }
            });

        });

        if (app.Environment.IsDevelopment())
        {
            //app.UseExceptionHandler("/Home/Error");
            app.UseStatusCodePagesWithReExecute("/Error/Index", "?statusCode={0}"); // Handles 404 errors
            // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
            app.UseHsts();
        }
        else //if (app.Environment.IsDevelopment())
        {
            //app.UseExceptionHandler("/Home/Error");
            app.UseStatusCodePagesWithReExecute("/Error/Index", "?statusCode={0}"); // Handles 404 errors
            // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
            app.UseHsts();
        }
        app.UseHttpsRedirection();
        app.UseCors("AnyOrigin");

        // Apply middleware for brute-force prevention
        //app.UseMiddleware<BruteForceMiddleware>();

        app.UseRouting();
        app.UseAuthentication();
        app.UseAuthorization();
        app.MapControllerRoute(
            name: "default",
            pattern: "{controller=Account}/{action=Login}/{id?}");

        using (var serviceScope = app.Services.CreateScope())
        {
            var serviceProvider = serviceScope.ServiceProvider;

            try
            {
                var logFile = serviceProvider.GetService<ILogFile>();
                var ado = serviceProvider.GetService<IADORepository>();
                var map = serviceProvider.GetService<IMapper>();
                var memorycache = serviceProvider.GetService<IMemoryCache>();

                SetPublicObjects.SetStaticPublicObjects(logFile, ado, map, memorycache);
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
                PublicClaimObjects _PublicClaimObjects = new PublicClaimObjects();
                _PublicClaimObjects = context.Session.GetObject<PublicClaimObjects>("PublicClaimObjects");
                ProgramSetPublicClaimObjects.SetPublicClaimObjectsFromCookie(ref context, ref _PublicClaimObjects);
                context.Items["PublicClaimObjects"] = _PublicClaimObjects;
            }
            // Call the next middleware in the pipeline
            await next.Invoke();
        });

        //app.MapHub<SessionHub>("/sessionHub");

        app.Run();

    }
}
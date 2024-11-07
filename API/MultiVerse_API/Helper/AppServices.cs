using Data.AppContext;
using Data.Dtos;
using Data.Interfaces;
using Microsoft.EntityFrameworkCore;
using Services.GlobalServices;

namespace MultiVerse_API.Helper
{
    public static class AppServices
    {
        public static void ApplicationDI(this IServiceCollection services, DbStringCollection dbStringCollection)
        {
            services.AddDbContext<IMultiVerse_DB_Context_10, MultiVerse_DB_Context_10>(op => op.UseSqlServer(dbStringCollection.MultiVerse_ConnectionModel!.ConnectionString, optionBuilder => optionBuilder.MigrationsAssembly("Data")));
            services.AddScoped<IADORepository, ADORepository>();
            services.AddScoped<ILogFile, LogFile>();
        }
    }
}

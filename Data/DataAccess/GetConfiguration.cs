using Microsoft.Extensions.Configuration;

namespace Data.DataAccess
{
    public class GetConfiguration
    {
        private readonly IConfiguration iconfig;
        public GetConfiguration(PublicClaimObjects _PublicClaimObjects)
        {
            iconfig = new ConfigurationBuilder().AddJsonFile(_PublicClaimObjects.appsettingfilename).Build();
        }
        public string GetConfigValue(string fieldname)
        {
            return iconfig.GetValue<string>(fieldname)!;
        }
    }
}

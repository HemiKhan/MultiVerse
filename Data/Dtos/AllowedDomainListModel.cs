namespace Data.Dtos
{
    public class AllowedDomainListModel
    {
        public string[] AllowedRemoteDomainAngular { get; set; }
    }
    public class AllowedDomainExcludingSubDomainListModel
    {
        public string[] AllowedRemoteDomainAngular { get; set; }
    }
    public class AllowedMobileAppKeysModel
    {
        public string[] AllowedMobileAppKeys { get; set; }
    }
    public class AllowedWebAppKeysModel
    {
        public string[] AllowedWebAppKeys { get; set; }
    }
    public class SwaggerHideURLs
    {
        public string[] Version1 { get; set; }
        public string[] Version78652140 { get; set; }
    }
}

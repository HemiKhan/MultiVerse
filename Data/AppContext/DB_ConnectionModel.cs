namespace Data.AppContext
{
    public class Default_ConnectionModel : GeneralDatabaseConnectionModel
    {
        public override string ConnectionString
        {
            get
            {
                return $"Data Source={Data_Source};Initial Catalog={Initial_Catalog};Integrated Security={Integrated_Security};TrustServerCertificate={TrustServerCertificate};MultipleActiveResultSets={MultipleActiveResultSets}";
            }
        }
    }

    public class MultiVerse_ConnectionModel : GeneralDatabaseConnectionModel
    {
        public override string ConnectionString
        {
            get
            {
                return $"Data Source={Data_Source};Initial Catalog={Initial_Catalog};Integrated Security={Integrated_Security};TrustServerCertificate={TrustServerCertificate};MultipleActiveResultSets={MultipleActiveResultSets}";
            }
        }
    }

    public class GeneralDatabaseConnectionModel
    {
        public string? Data_Source { get; set; }
        public string? Initial_Catalog { get; set; }
        public string? User_Id { get; set; }
        public string? Password { get; set; }
        public bool? Integrated_Security { get; set; }
        public bool? TrustServerCertificate { get; set; }
        public bool? MultipleActiveResultSets { get; set; }
        public virtual string? ConnectionString { get; }
    }
}

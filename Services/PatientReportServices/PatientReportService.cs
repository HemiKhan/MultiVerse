using Data.DataAccess;

namespace Services.PatientReportServices
{
    public class PatientReportService : IPatientReportService
    {
        private readonly IServiceProvider _serviceProvider;
        private PublicClaimObjects? _PublicClaimObjects;

        public PatientReportService(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
            this._PublicClaimObjects = StaticPublicObjects.ado.GetPublicClaimObjects();
        }
    }
}

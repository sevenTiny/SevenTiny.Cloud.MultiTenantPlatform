using SevenTiny.Bantina;
using SevenTiny.Cloud.MultiTenant.Domain.Entity;
using SevenTiny.Cloud.MultiTenant.Domain.RepositoryContract;
using SevenTiny.Cloud.MultiTenant.Domain.ServiceContract;

namespace SevenTiny.Cloud.MultiTenant.Domain.Service
{
    internal class CloudApplicationService : CommonServiceBase<CloudApplication>, ICloudApplicationService
    {
        public CloudApplicationService(ICloudApplicationRepository applicationRepository) : base(applicationRepository)
        {
            _applicationRepository = applicationRepository;
        }

        readonly ICloudApplicationRepository _applicationRepository;

        public new Result Update(CloudApplication application)
        {
            return base.UpdateWithOutCode(application, target =>
            {
                target.Icon = application.Icon;
            });
        }
    }
}

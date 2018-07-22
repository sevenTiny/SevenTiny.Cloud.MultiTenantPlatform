using SevenTiny.Cloud.MultiTenantPlatform.CloudModel;

namespace SevenTiny.Cloud.MultiTenantPlatform.Application.ServiceContract
{
    public interface IObjectDataService
    {
        void Insert(ObjectData objectData);
        void Update(ObjectData objectData);
    }
}

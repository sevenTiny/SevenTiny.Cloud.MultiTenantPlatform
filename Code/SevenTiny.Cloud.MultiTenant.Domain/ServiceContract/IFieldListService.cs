using SevenTiny.Cloud.MultiTenant.Domain.Entity;
using SevenTiny.Cloud.MultiTenant.Domain.Repository;
using SevenTiny.Cloud.MultiTenant.Infrastructure.ValueObject;

namespace SevenTiny.Cloud.MultiTenant.Domain.ServiceContract
{
    public interface IFieldListService : IMetaObjectManageRepository<FieldList>
    {
    }
}

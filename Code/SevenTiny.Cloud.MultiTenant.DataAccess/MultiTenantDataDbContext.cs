using MongoDB.Bson;
using MongoDB.Driver;
using SevenTiny.Bantina.Bankinate;
using SevenTiny.Cloud.MultiTenant.Infrastructure.Configs;

namespace SevenTiny.Cloud.MultiTenant.DataAccess
{
    public class MultiTenantDataDbContext : MongoDbContext<MultiTenantDataDbContext>
    {
        public MultiTenantDataDbContext() : base(ConnectionStringsConfig.Instance.mongodb39911)
        {
        }
    }
}

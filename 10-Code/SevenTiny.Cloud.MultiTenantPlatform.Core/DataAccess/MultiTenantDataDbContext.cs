using MongoDB.Bson;
using MongoDB.Driver;
using SevenTiny.Bantina.Bankinate;
using SevenTiny.Cloud.MultiTenantPlatform.Infrastructure.Configs;

namespace SevenTiny.Cloud.MultiTenantPlatform.Core.DataAccess
{
    public class MultiTenantDataDbContext : MongoDbContext<MultiTenantDataDbContext>
    {
        public MultiTenantDataDbContext() : base(ConnectionStringsConfig.Instance.Config.mongodb39911)
        {
        }
    }
}

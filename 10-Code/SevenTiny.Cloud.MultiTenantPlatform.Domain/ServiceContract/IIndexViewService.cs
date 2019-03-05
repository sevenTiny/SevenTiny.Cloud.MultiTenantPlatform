using SevenTiny.Cloud.MultiTenantPlatform.Domain.Entity;
using SevenTiny.Cloud.MultiTenantPlatform.Domain.Repository;
using SevenTiny.Cloud.MultiTenantPlatform.Domain.UIMetaData.IndexPage;

namespace SevenTiny.Cloud.MultiTenantPlatform.Domain.ServiceContract
{
    public interface IIndexViewService : IMetaObjectManageRepository<IndexView>
    {
        /// <summary>
        /// 构建视图页组件
        /// </summary>
        /// <param name="indexView"></param>
        /// <returns></returns>
        IndexPageComponent GetIndexPageComponentByIndexView(IndexView indexView);
    }
}

using SevenTiny.Cloud.MultiTenantPlatform.Core.Entity;
using SevenTiny.Cloud.MultiTenantPlatform.Core.Repository;
using SevenTiny.Cloud.MultiTenantPlatform.UI.UIMetaData.IndexPage;

namespace SevenTiny.Cloud.MultiTenantPlatform.Core.ServiceContract
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

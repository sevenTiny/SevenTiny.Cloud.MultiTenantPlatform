using SevenTiny.Cloud.MultiTenant.Core.Entity;
using SevenTiny.Cloud.MultiTenant.Core.Repository;
using SevenTiny.Cloud.MultiTenant.UI.UIMetaData.IndexPage;

namespace SevenTiny.Cloud.MultiTenant.Core.ServiceContract
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

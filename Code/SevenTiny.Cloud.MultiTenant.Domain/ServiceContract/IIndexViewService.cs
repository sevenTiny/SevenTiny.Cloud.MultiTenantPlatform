using SevenTiny.Cloud.MultiTenant.Domain.Entity;
using SevenTiny.Cloud.MultiTenant.Domain.Repository;
using SevenTiny.Cloud.MultiTenant.UI.UIMetaData.IndexPage;

namespace SevenTiny.Cloud.MultiTenant.Domain.ServiceContract
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

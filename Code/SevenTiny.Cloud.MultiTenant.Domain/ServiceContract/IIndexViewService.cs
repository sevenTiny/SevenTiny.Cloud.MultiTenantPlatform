using SevenTiny.Cloud.MultiTenant.Domain.Entity;
using SevenTiny.Cloud.MultiTenant.Domain.Repository;
using SevenTiny.Cloud.MultiTenant.Domain.ValueObject.UIMetaData.IndexPage;

namespace SevenTiny.Cloud.MultiTenant.Domain.ServiceContract
{
    public interface IIndexViewService : IMetaObjectCommonServiceBase<IndexView>
    {
        /// <summary>
        /// 构建视图页组件
        /// </summary>
        /// <param name="indexView"></param>
        /// <returns></returns>
        IndexPageComponent GetIndexPageComponentByIndexView(IndexView indexView);
    }
}

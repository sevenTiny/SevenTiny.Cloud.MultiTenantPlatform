using SevenTiny.Bantina;
using SevenTiny.Cloud.MultiTenant.Domain.DbContext;
using SevenTiny.Cloud.MultiTenant.Domain.Entity;
using SevenTiny.Cloud.MultiTenant.Domain.Repository;
using SevenTiny.Cloud.MultiTenant.Domain.RepositoryContract;
using SevenTiny.Cloud.MultiTenant.Domain.ServiceContract;
using System;
using System.Linq;

namespace SevenTiny.Cloud.MultiTenant.Domain.Service
{
    internal class SearchConditionService : MetaObjectCommonServiceBase<SearchCondition>, ISearchConditionService
    {
        public SearchConditionService(ISearchConditionRepository searchConditionRepository) : base(searchConditionRepository)
        {
        }

        /// <summary>
        /// 更新对象
        /// </summary>
        /// <param name="metaField"></param>
        public new Result Update(SearchCondition searchCondition)
        {
            return base.UpdateWithOutCode(searchCondition);
        }

        ///// <summary>
        ///// 根据id删除配置字段，校验是否被引用
        ///// </summary>
        ///// <param name="id"></param>
        //public new Result<SearchCondition> Delete(int id)
        //{
        //    if (dbContext.Queryable<CloudInterface>().Where(t => t.SearchConditionId == id).Any())
        //    {
        //        //存在引用关系，先删除引用该数据的数据
        //        return Result<SearchCondition>.Error("存在引用关系，先删除引用该数据的数据");
        //    }
        //    else
        //    {
        //        base.Delete(id);
        //        return Result<SearchCondition>.Success();
        //    }
        //}

        //public new void DeleteByMetaObjectId(int metaObjectId)
        //{
        //    var searchContions = base.GetEntitiesByMetaObjectId(metaObjectId);
        //    TransactionHelper.Transaction(() =>
        //    {
        //        if (searchContions != null && searchContions.Any())
        //        {
        //            //删除字段配置下的所有字段
        //            foreach (var item in searchContions)
        //            {
        //                dbContext.Delete<SearchConditionNode>(t => t.SearchConditionId == item.Id);
        //            }
        //        }
        //        //删除字段配置
        //        base.DeleteByMetaObjectId(metaObjectId);
        //    });
        //}
    }
}

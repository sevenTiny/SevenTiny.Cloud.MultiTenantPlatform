using SevenTiny.Bantina;
using SevenTiny.Cloud.MultiTenantPlatform.Core.DataAccess;
using SevenTiny.Cloud.MultiTenantPlatform.Core.Entity;
using SevenTiny.Cloud.MultiTenantPlatform.Core.Repository;
using SevenTiny.Cloud.MultiTenantPlatform.Core.ServiceContract;
using System;
using System.Linq;

namespace SevenTiny.Cloud.MultiTenantPlatform.Core.Service
{
    public class SearchConditionService : MetaObjectManageRepository<SearchCondition>, ISearchConditionService
    {
        public SearchConditionService(
            MultiTenantPlatformDbContext multiTenantPlatformDbContext
            ) : base(multiTenantPlatformDbContext)
        {
            dbContext = multiTenantPlatformDbContext;
        }

        readonly MultiTenantPlatformDbContext dbContext;

        /// <summary>
        /// 更新对象
        /// </summary>
        /// <param name="metaField"></param>
        public new Result Update(SearchCondition searchCondition)
        {
            SearchCondition myfield = GetById(searchCondition.Id);
            if (myfield != null)
            {
                //编码不允许修改
                myfield.Name = searchCondition.Name;
                myfield.Group = searchCondition.Group;
                myfield.SortNumber = searchCondition.SortNumber;
                myfield.Description = searchCondition.Description;
                myfield.ModifyBy = -1;
                myfield.ModifyTime = DateTime.Now;
            }
            base.Update(myfield);
            return Result.Success();
        }

        /// <summary>
        /// 根据id删除配置字段，校验是否被引用
        /// </summary>
        /// <param name="id"></param>
        public new Result<InterfaceAggregation> Delete(int id)
        {
            if (dbContext.Queryable<InterfaceAggregation>().Where(t => t.SearchConditionId == id).Any())
            {
                //存在引用关系，先删除引用该数据的数据
                return Result<InterfaceAggregation>.Error("存在引用关系，先删除引用该数据的数据");
            }
            else
            {
                base.Delete(id);
                return Result<InterfaceAggregation>.Success();
            }
        }

        public new void DeleteByMetaObjectId(int metaObjectId)
        {
            var searchContions = base.GetEntitiesByMetaObjectId(metaObjectId);
            TransactionHelper.Transaction(() =>
            {
                if (searchContions != null && searchContions.Any())
                {
                    //删除字段配置下的所有字段
                    foreach (var item in searchContions)
                    {
                        dbContext.Delete<SearchConditionAggregation>(t => t.SearchConditionId == item.Id);
                    }
                }
                //删除字段配置
                base.DeleteByMetaObjectId(metaObjectId);
            });
        }
    }
}

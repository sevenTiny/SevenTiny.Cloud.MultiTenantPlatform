using SevenTiny.Bantina;
using SevenTiny.Cloud.MultiTenantPlatform.Domain.Entity;
using SevenTiny.Cloud.MultiTenantPlatform.Domain.Repository;
using SevenTiny.Cloud.MultiTenantPlatform.Domain.ServiceContract;
using SevenTiny.Cloud.MultiTenantPlatform.Domain.ValueObject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SevenTiny.Cloud.MultiTenantPlatform.Domain.Service
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
        public new ResultModel Update(SearchCondition searchCondition)
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
            return ResultModel.Success();
        }

        /// <summary>
        /// 根据id删除配置字段，校验是否被引用
        /// </summary>
        /// <param name="id"></param>
        public new ResultModel Delete(int id)
        {
            if (dbContext.QueryExist<InterfaceAggregation>(t => t.FieldListId == id))
            {
                //存在引用关系，先删除引用该数据的数据
                return ResultModel.Error("存在引用关系，先删除引用该数据的数据");
            }
            else
            {
                base.Delete(id);
                return ResultModel.Success();
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

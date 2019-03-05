using SevenTiny.Bantina;
using SevenTiny.Cloud.MultiTenantPlatform.Core.Entity;
using SevenTiny.Cloud.MultiTenantPlatform.Core.Enum;
using SevenTiny.Cloud.MultiTenantPlatform.Core.Repository;
using SevenTiny.Cloud.MultiTenantPlatform.Core.ServiceContract;
using SevenTiny.Cloud.MultiTenantPlatform.Core.ValueObject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SevenTiny.Cloud.MultiTenantPlatform.Core.Service
{
    public class FieldListService : MetaObjectManageRepository<FieldList>, IFieldListService
    {
        public FieldListService(MultiTenantPlatformDbContext multiTenantPlatformDbContext) : base(multiTenantPlatformDbContext)
        {
            dbContext = multiTenantPlatformDbContext;
        }

        readonly MultiTenantPlatformDbContext dbContext;

        /// <summary>
        /// 更新对象
        /// </summary>
        /// <param name="metaField"></param>
        public new ResultModel Update(FieldList interfaceField)
        {
            FieldList myfield = GetById(interfaceField.Id);
            if (myfield != null)
            {
                //编码不允许修改
                myfield.Name = interfaceField.Name;
                myfield.Group = interfaceField.Group;
                myfield.SortNumber = interfaceField.SortNumber;
                myfield.Description = interfaceField.Description;
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
            var fieldLists = base.GetEntitiesByMetaObjectId(metaObjectId);
            
            TransactionHelper.Transaction(() =>
            {
                if (fieldLists!=null && fieldLists.Any())
                {
                    //删除字段配置下的所有字段
                    foreach (var item in fieldLists)
                    {
                        dbContext.Delete<FieldListAggregation>(t => t.FieldListId == item.Id);
                    }
                }
                //删除字段配置
                base.DeleteByMetaObjectId(metaObjectId);
            });
        }
    }
}

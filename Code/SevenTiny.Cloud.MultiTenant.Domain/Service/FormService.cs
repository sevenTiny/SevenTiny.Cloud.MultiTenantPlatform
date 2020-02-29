using SevenTiny.Bantina;
using SevenTiny.Cloud.MultiTenant.Domain.DataAccess;
using SevenTiny.Cloud.MultiTenant.Domain.Entity;
using SevenTiny.Cloud.MultiTenant.Domain.Repository;
using SevenTiny.Cloud.MultiTenant.Domain.ServiceContract;
using System;
using System.Linq;

namespace SevenTiny.Cloud.MultiTenant.Domain.Service
{
    public class FormService : MetaObjectManageRepository<Form>, IFormService
    {
        public FormService(MultiTenantPlatformDbContext multiTenantPlatformDbContext) : base(multiTenantPlatformDbContext)
        {
            dbContext = multiTenantPlatformDbContext;
        }

        readonly MultiTenantPlatformDbContext dbContext;

        /// <summary>
        /// 更新对象
        /// </summary>
        /// <param name="metaField"></param>
        public new Result<Form> Update(Form entity)
        {
            Form myfield = GetById(entity.Id);
            if (myfield != null)
            {
                //编码不允许修改
                myfield.Name = entity.Name;
                myfield.Group = entity.Group;
                myfield.SortNumber = entity.SortNumber;
                myfield.Description = entity.Description;
                myfield.ModifyBy = -1;
                myfield.ModifyTime = DateTime.Now;
            }
            return base.Update(myfield);
        }

        /// <summary>
        /// 根据id删除配置字段，校验是否被引用
        /// </summary>
        /// <param name="id"></param>
        public new Result<Form> Delete(int id)
        {
            if (dbContext.Queryable<InterfaceAggregation>().Where(t => t.FormId == id).Any())
            {
                //存在引用关系，先删除引用该数据的数据
                return Result<Form>.Error("存在引用关系，先删除引用该数据的数据");
            }
            else
            {
                return base.Delete(id);
            }
        }

        public new void DeleteByMetaObjectId(int metaObjectId)
        {
            var fieldLists = base.GetEntitiesByMetaObjectId(metaObjectId);

            TransactionHelper.Transaction(() =>
            {
                if (fieldLists != null && fieldLists.Any())
                {
                    //删除字段配置下的所有字段
                    foreach (var item in fieldLists)
                    {
                        dbContext.Delete<FieldListMetaField>(t => t.FieldListId == item.Id);
                    }
                }
                //删除字段配置
                base.DeleteByMetaObjectId(metaObjectId);
            });
        }
    }
}

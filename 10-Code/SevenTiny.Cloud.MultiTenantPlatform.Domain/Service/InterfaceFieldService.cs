using SevenTiny.Bantina;
using SevenTiny.Cloud.MultiTenantPlatform.Domain.Entity;
using SevenTiny.Cloud.MultiTenantPlatform.Domain.Enum;
using SevenTiny.Cloud.MultiTenantPlatform.Domain.Repository;
using SevenTiny.Cloud.MultiTenantPlatform.Domain.ServiceContract;
using SevenTiny.Cloud.MultiTenantPlatform.Domain.ValueObject;
using System;
using System.Collections.Generic;
using System.Text;

namespace SevenTiny.Cloud.MultiTenantPlatform.Domain.Service
{
    public class InterfaceFieldService : MetaObjectManageRepository<InterfaceField>, IInterfaceFieldService
    {
        public InterfaceFieldService(MultiTenantPlatformDbContext multiTenantPlatformDbContext) : base(multiTenantPlatformDbContext)
        {
            dbContext = multiTenantPlatformDbContext;
        }

        readonly MultiTenantPlatformDbContext dbContext;

        /// <summary>
        /// 检查是否有相同名称的编码或名称
        /// </summary>
        /// <param name="metaObjectId"></param>
        /// <param name="code"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        public ResultModel CheckSameCodeOrName(int metaObjectId, InterfaceField interfaceField)
        {
            var obj = dbContext.QueryOne<InterfaceField>(t => t.MetaObjectId == metaObjectId && t.Id != interfaceField.Id && (t.Code.Equals(interfaceField.Code) || t.Name.Equals(interfaceField.Name)));
            if (obj != null)
            {
                if (obj.Code.Equals(interfaceField.Code))
                    return ResultModel.Error($"编码[{obj.Code}]已存在", interfaceField);
                else if (obj.Name.Equals(interfaceField.Name))
                    return ResultModel.Error($"名称[{obj.Name}]已存", interfaceField);
            }
            return ResultModel.Success();
        }

        /// <summary>
        /// 更新对象
        /// </summary>
        /// <param name="metaField"></param>
        public new void Update(InterfaceField interfaceField)
        {
            InterfaceField myfield = GetById(interfaceField.Id);
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
        }

        /// <summary>
        /// 根据id删除配置字段
        /// 同时删除相关的对象
        /// </summary>
        /// <param name="id"></param>
        public new void Delete(int id)
        {
            TransactionHelper.Transaction(() =>
            {
                //clear fields first
                dbContext.Delete<InterfaceAggregation>(t => t.InterfaceFieldId == id);
                //delete field config
                Delete(id);
            });
        }
    }
}

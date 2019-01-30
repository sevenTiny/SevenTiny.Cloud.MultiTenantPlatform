using SevenTiny.Cloud.MultiTenantPlatform.Domain.Entity;
using SevenTiny.Cloud.MultiTenantPlatform.Domain.Repository;
using SevenTiny.Cloud.MultiTenantPlatform.Domain.ServiceContract;
using SevenTiny.Cloud.MultiTenantPlatform.Domain.ValueObject;
using System;

namespace SevenTiny.Cloud.MultiTenantPlatform.Domain.Service
{
    public class TriggerScriptService : MetaObjectManageRepository<TriggerScript>, ITriggerScriptService
    {
        public TriggerScriptService(MultiTenantPlatformDbContext multiTenantPlatformDbContext) : base(multiTenantPlatformDbContext)
        {
            dbContext = multiTenantPlatformDbContext;
        }

        MultiTenantPlatformDbContext dbContext;

        /// <summary>
        /// 更新对象
        /// </summary>
        /// <param name="triggerScript"></param>
        public new ResultModel Update(TriggerScript triggerScript)
        {
            TriggerScript myfield = GetById(triggerScript.Id);
            if (myfield != null)
            {
                //编码不允许修改
                //脚本类型不允许修改
                myfield.Name = triggerScript.Name;
                myfield.Group = triggerScript.Group;
                myfield.SortNumber = triggerScript.SortNumber;
                myfield.Description = triggerScript.Description;
                myfield.ModifyBy = -1;
                myfield.ModifyTime = DateTime.Now;
            }
            base.Update(myfield);
            return ResultModel.Success();
        }
    }
}

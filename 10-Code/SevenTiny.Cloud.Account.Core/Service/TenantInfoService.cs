using SevenTiny.Bantina;
using SevenTiny.Bantina.Validation;
using SevenTiny.Cloud.Account.Core.DataAccess;
using SevenTiny.Cloud.Account.Core.Entity;
using SevenTiny.Cloud.Account.Core.Repository;
using SevenTiny.Cloud.Account.Core.ServiceContract;
using System;

namespace SevenTiny.Cloud.Account.Core.Service
{
    public class TenantInfoService : CommonInfoRepository<TenantInfo>, ITenantInfoService
    {
        AccountDbContext _dbContext;
        public TenantInfoService(AccountDbContext accountDbContext) : base(accountDbContext)
        {
            _dbContext = accountDbContext;
        }

        public bool Exist(int tenantId)
        {
            return _dbContext.Queryable<TenantInfo>().Where(t => t.Id == tenantId).Any();
        }

        public new Result<TenantInfo> Add(TenantInfo entity)
        {
            return Result<TenantInfo>.Success("add Succeed!")
                .ContinueAssert(!string.IsNullOrEmpty(entity.RegisterEmail), "邮箱不能为空")
                .ContinueAssert(entity.RegisterEmail.IsEmail(), "邮箱格式不正确")
                //register
                .Continue(re =>
                {
                    return base.Add(entity);
                });
        }

        public new Result<TenantInfo> Update(TenantInfo entity)
        {
            TenantInfo old = GetById(entity.Id);
            if (old != null)
            {
                old.TenantName = entity.TenantName;
                //old.OperatorName = entity.OperatorName;
                //old.RegisterEmail = entity.RegisterEmail;
                old.RegisterPhone = entity.RegisterPhone;
                old.RegisterName = entity.RegisterName;
                //old.IsActive = entity.IsActive;

                old.Group = entity.Group;
                old.SortNumber = entity.SortNumber;
                old.Description = entity.Description;
                old.ModifyBy = entity.ModifyBy;
                old.ModifyTime = DateTime.Now;
            }
            return base.Update(old);
        }
    }
}

using SevenTiny.Bantina;
using SevenTiny.Bantina.Validation;
using SevenTiny.Cloud.Account.Core.DataAccess;
using SevenTiny.Cloud.Account.Core.Entity;
using SevenTiny.Cloud.Account.Core.Enum;
using SevenTiny.Cloud.Account.Core.Repository;
using SevenTiny.Cloud.Account.Core.ServiceContract;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SevenTiny.Cloud.Account.Core.Service
{
    public class TenantApplicationLicenseService : CommonInfoRepository<TenantApplicationLicense>, ITenantApplicationLicenseService
    {
        AccountDbContext _dbContext;
        MultiTenantPlatformDbContext _MultiTenantPlatformDbContext;
        ITenantInfoService _tenantInfoService;
        public TenantApplicationLicenseService(AccountDbContext accountDbContext,
            MultiTenantPlatformDbContext multiTenantPlatformDbContext,
            ITenantInfoService tenantInfoService) : base(accountDbContext)
        {
            _dbContext = accountDbContext;
            _MultiTenantPlatformDbContext = multiTenantPlatformDbContext;
            _tenantInfoService = tenantInfoService;
        }

        public Dictionary<int, string> GetApplicationIdNameDic()
        {
            var list = _MultiTenantPlatformDbContext.Queryable<Application>().Where(t => t.IsDeleted == (int)TrueFalse.False).ToList();
            if (list != null && list.Any())
            {
                return list.ToDictionary(k => k.Id, v => v.Name);
            }
            return new Dictionary<int, string>();
        }

        public List<TenantApplicationLicense> GetUnDeletedEntitiesByTenantId(int tenantId)
        {
            return _dbContext.Queryable<TenantApplicationLicense>().Where(t => t.IsDeleted == (int)TrueFalse.False && t.TenantId == tenantId).ToList();
        }

        public List<TenantApplicationLicense> GetUnDeletedValidityEntitiesByTenantId(int tenantId)
        {
            //var dt = DateTime.Now;
            //return _dbContext.Queryable<TenantApplicationLicense>().Where(t => t.IsDeleted == (int)TrueFalse.False && t.TenantId == tenantId && t.StartTime <= dt && t.ExpirationTime >= dt).ToList();
            var param = new Dictionary<string, object>()
            {
                { "@tIsDeleted",(int)TrueFalse.False},
                { "@tTenantId",tenantId},
                { "@tStartTime",DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss")},
                { "@tExpirationTime",DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss")},
            };
            return _dbContext.Queryable("SELECT * FROM TenantApplicationLicense t  WHERE ( 1=1 )  AND  ((((t.IsDeleted = @tIsDeleted)  AND  (t.TenantId = @tTenantId))  AND  (t.StartTime <= @tStartTime))  AND  (t.ExpirationTime >= @tExpirationTime))", param).ToList<TenantApplicationLicense>();
        }

        public new Result<TenantApplicationLicense> Add(TenantApplicationLicense entity)
        {
            return Result<TenantApplicationLicense>.Success("Add success")
                .ContinueAssert(entity.TenantId > 0, "租户Id不正确")
                .Continue(re =>
                {
                    if (!_tenantInfoService.Exist(entity.TenantId))
                    {
                        return Result<TenantApplicationLicense>.Error("租户不存在");
                    }
                    return re;
                })
                .Continue(re =>
                {
                    var applicationDic = GetApplicationIdNameDic();
                    if (!applicationDic.ContainsKey(entity.ApplicationId))
                    {
                        return Result<TenantApplicationLicense>.Error("应用不存在");
                    }
                    entity.ApplicationName = applicationDic[entity.ApplicationId];
                    return re;
                })
                .Continue(re =>
                {
                    bool exist = _dbContext.Queryable<TenantApplicationLicense>().Where(t => t.TenantId == entity.TenantId && t.ApplicationId == entity.ApplicationId).Any();
                    if (exist)
                    {
                        return Result<TenantApplicationLicense>.Error("该授权已存在");
                    }
                    return re;
                })
                .Continue(re =>
                {
                    return base.Add(entity);
                });
        }
        public new Result<TenantApplicationLicense> Update(TenantApplicationLicense entity)
        {
            TenantApplicationLicense old = GetById(entity.Id);
            if (old != null)
            {
                old.StartTime = entity.StartTime;
                old.ExpirationTime = entity.ExpirationTime;
                old.IsEnable = entity.IsEnable;
                //可以进行修改租户显示的应用名称
                old.ApplicationName = entity.ApplicationName;

                old.Group = entity.Group;
                old.SortNumber = entity.SortNumber;
                old.Description = entity.Description;
                old.ModifyBy = entity.ModifyBy;
                old.ModifyTime = DateTime.Now;
            }
            return base.Update(old);
        }

        /// <summary>
        /// 启用应用（租户管理员对租户内应用启用停止）
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Result EnableApplication(int id)
        {
            return Result.Success("启用成功")
                .Continue(re =>
                {
                    TenantApplicationLicense old = GetById(id);
                    if (old == null)
                        return Result.Error("错误的授权Id");

                    old.IsEnable = (int)TrueFalse.True;
                    return re;
                });
        }
        /// <summary>
        /// 停止应用（租户管理员对租户内应用启用停止）
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Result DisableApplication(int id)
        {
            return Result.Success("停用成功")
                .Continue(re =>
                {
                    TenantApplicationLicense old = GetById(id);
                    if (old == null)
                        return Result.Error("错误的授权Id");

                    old.IsEnable = (int)TrueFalse.False;
                    return re;
                });
        }
    }
}

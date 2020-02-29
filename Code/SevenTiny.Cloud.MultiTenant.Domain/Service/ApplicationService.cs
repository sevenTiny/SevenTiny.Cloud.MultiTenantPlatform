using SevenTiny.Bantina;
using SevenTiny.Cloud.MultiTenant.Domain.DataAccess;
using SevenTiny.Cloud.MultiTenant.Domain.Entity;
using SevenTiny.Cloud.MultiTenant.Domain.Repository;
using SevenTiny.Cloud.MultiTenant.Domain.ServiceContract;
using SevenTiny.Cloud.MultiTenant.Infrastructure.ValueObject;
using System;
using System.Linq;

namespace SevenTiny.Cloud.MultiTenant.Domain.Service
{
    public class ApplicationService : CommonInfoRepository<Application>, IApplicationService
    {
        public ApplicationService(
            MultiTenantPlatformDbContext multiTenantPlatformDbContext,
            IMetaObjectService _metaObjectService
            ) : base(multiTenantPlatformDbContext)
        {
            dbContext = multiTenantPlatformDbContext;
            metaObjectService = _metaObjectService;
        }

        MultiTenantPlatformDbContext dbContext;
        readonly IMetaObjectService metaObjectService;

        public new Result Update(Application application)
        {
            if (dbContext.Queryable<Application>().Where(t => t.Id != application.Id && t.Name == application.Name).Any())
            {
                return Result.Error("该名称已存在");
            }

            var app = GetById(application.Id);
            if (app != null)
            {
                app.Name = application.Name;
                app.Icon = application.Icon;
                app.Group = application.Group;
                app.SortNumber = application.SortNumber;
                app.Description = application.Description;
                app.ModifyBy = -1;
                app.ModifyTime = DateTime.Now;

                base.Update(app);
            }
            return Result.Success();
        }

        public new Result Delete(int id)
        {
            var metaObjects = dbContext.Queryable<MetaObject>().Where(t => t.ApplicationId == id).ToList();
            if (metaObjects != null && metaObjects.Any())
            {
                TransactionHelper.Transaction(() =>
                {
                    //删除应用下的所有对象
                    foreach (var item in metaObjects)
                    {
                        metaObjectService.Delete(item.Id);
                    }
                });
            }
            base.Delete(id);
            return Result.Success();
        }

        public new Result<Application> Add(Application entity)
        {
            //check metaobject of name or code exist?
            Application obj = dbContext.Queryable<Application>().Where(t => t.Code.Equals(entity.Code) || t.Name.Equals(entity.Name)).ToOne();
            if (obj != null)
            {
                if (obj.Code.Equals(entity.Code))
                {
                    return Result<Application>.Error("Code Has Been Exist！", entity);
                }
                if (obj.Name.Equals(entity.Name))
                {
                    return Result<Application>.Error("Name Has Been Exist！", entity);
                }
            }

            base.Add(entity);

            return Result<Application>.Success();
        }
    }
}

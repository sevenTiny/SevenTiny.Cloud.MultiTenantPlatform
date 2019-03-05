using SevenTiny.Bantina;
using SevenTiny.Cloud.MultiTenantPlatform.Core.Entity;
using SevenTiny.Cloud.MultiTenantPlatform.Core.Repository;
using SevenTiny.Cloud.MultiTenantPlatform.Core.ServiceContract;
using SevenTiny.Cloud.MultiTenantPlatform.Core.ValueObject;
using System;
using System.Linq;

namespace SevenTiny.Cloud.MultiTenantPlatform.Core.Service
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

        public new ResultModel Update(Application application)
        {
            if (dbContext.QueryExist<Application>(t => t.Id != application.Id && t.Name == application.Name))
            {
                return ResultModel.Error("该名称已存在");
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
            return ResultModel.Success();
        }

        public new ResultModel Delete(int id)
        {
            var metaObjects = dbContext.QueryList<MetaObject>(t => t.ApplicationId == id);
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
            return ResultModel.Success();
        }

        public new ResultModel Add(Application entity)
        {
            //check metaobject of name or code exist?
            Application obj = dbContext.QueryOne<Application>(t => t.Code.Equals(entity.Code) || t.Name.Equals(entity.Name));
            if (obj != null)
            {
                if (obj.Code.Equals(entity.Code))
                {
                    return ResultModel.Error("Code Has Been Exist！", entity);
                }
                if (obj.Name.Equals(entity.Name))
                {
                    return ResultModel.Error("Name Has Been Exist！", entity);
                }
            }

            base.Add(entity);

            return ResultModel.Success();
        }
    }
}

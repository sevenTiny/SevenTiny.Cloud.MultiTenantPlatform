using SevenTiny.Bantina;
using SevenTiny.Cloud.MultiTenant.Core.DataAccess;
using SevenTiny.Cloud.MultiTenant.Core.Entity;
using SevenTiny.Cloud.MultiTenant.Core.Enum;
using SevenTiny.Cloud.MultiTenant.Core.Repository;
using SevenTiny.Cloud.MultiTenant.Core.ServiceContract;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SevenTiny.Cloud.MultiTenant.Core.Service
{
    public class MenueService : CommonInfoRepository<Menue>, IMenueService
    {
        public MenueService(
            MultiTenantPlatformDbContext multiTenantPlatformDbContext
            ) : base(multiTenantPlatformDbContext)
        {
            dbContext = multiTenantPlatformDbContext;
        }

        MultiTenantPlatformDbContext dbContext;

        public new Result Update(Menue obj)
        {
            var app = GetById(obj.Id);
            if (app != null)
            {
                app.Name = obj.Name;
                app.Icon = obj.Icon;
                app.LinkType = obj.LinkType;
                app.Address = obj.Address;

                app.Group = obj.Group;
                app.SortNumber = obj.SortNumber;
                app.Description = obj.Description;
                app.ModifyBy = -1;
                app.ModifyTime = DateTime.Now;

                base.Update(app);
            }
            return Result.Success();
        }

        public new Result Delete(int id)
        {
            base.Delete(id);
            return Result.Success();
        }

        public new Result<Menue> Add(Menue entity)
        {
            return Result<Menue>.Success()
                .Continue(re =>
                {
                    //check metaobject of name or code exist?
                    Menue obj = dbContext.Queryable<Menue>().Where(t => t.Code.Equals(entity.Code)).ToOne();
                    if (obj != null)
                    {
                        if (obj.Code.Equals(entity.Code))
                        {
                            return Result<Menue>.Error("Code Has Been Exist！", entity);
                        }
                    }
                    return re;
                })
                .Continue(re =>
                {
                    entity.Code = $"{entity.ApplicationCode}.Menue.{entity.Code}";
                    return re;
                })
                .Continue(re =>
                {
                    return base.Add(entity);
                });
        }

        public Result<List<Menue>> AnalysisMenueTree()
        {
            //var menues = dbContext.Queryable<Menue>().ToList();
            //var roots = menues?.Where(t => t.ParentId == -1)?.ToList();
            //if (roots != null && roots.Any())
            //{
            //    foreach (var item in roots)
            //    {
            //        item.Children = menues.Where(t => t.ParentId == item.Id).ToList();
            //    }
            //}
            return Result<List<Menue>>.Success("", null);
        }

        public List<Menue> GetUnDeletedEntitiesByApplicationId(int applicationId)
        {
            return dbContext.Queryable<Menue>().Where(t => t.ApplicationId == applicationId && t.IsDeleted == (int)IsDeleted.UnDeleted).ToList();
        }
    }
}

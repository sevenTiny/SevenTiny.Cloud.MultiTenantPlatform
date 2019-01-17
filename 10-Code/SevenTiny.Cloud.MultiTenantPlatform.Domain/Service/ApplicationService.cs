using SevenTiny.Cloud.MultiTenantPlatform.Domain.Entity;
using SevenTiny.Cloud.MultiTenantPlatform.Domain.Repository;
using SevenTiny.Cloud.MultiTenantPlatform.Domain.ServiceContract;
using System;

namespace SevenTiny.Cloud.MultiTenantPlatform.Domain.Service
{
    public class ApplicationService : CommonInfoRepository<Application>, IApplicationService
    {
        public ApplicationService(MultiTenantPlatformDbContext multiTenantPlatformDbContext) : base(multiTenantPlatformDbContext)
        {
            dbContext = multiTenantPlatformDbContext;
        }

        MultiTenantPlatformDbContext dbContext;


        public bool ExistForSameName(string name)
            => dbContext.QueryExist<Application>(t => t.Name.Equals(name));

        public bool ExistForSameNameAndNotSameId(string name, int id)
            => dbContext.QueryExist<Application>(t => t.Name.Equals(name) && t.Id != id);

        public new void Update(Application application)
        {
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

                dbContext.Update(t => t.Id == application.Id, app);
            }
        }

        public new void Delete(int id) { }
    }
}

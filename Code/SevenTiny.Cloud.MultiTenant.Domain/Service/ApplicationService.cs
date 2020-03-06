using SevenTiny.Bantina;
using SevenTiny.Cloud.MultiTenant.Domain.DbContext;
using SevenTiny.Cloud.MultiTenant.Domain.Entity;
using SevenTiny.Cloud.MultiTenant.Domain.Repository;
using SevenTiny.Cloud.MultiTenant.Domain.RepositoryContract;
using SevenTiny.Cloud.MultiTenant.Domain.ServiceContract;
using SevenTiny.Cloud.MultiTenant.Infrastructure.ValueObject;
using System;
using System.Linq;

namespace SevenTiny.Cloud.MultiTenant.Domain.Service
{
    internal class ApplicationService : CommonServiceBase<Application>, IApplicationService
    {
        public ApplicationService(IApplicationRepository applicationRepository) : base(applicationRepository)
        {
            _applicationRepository = applicationRepository;
        }

        readonly IApplicationRepository _applicationRepository;

        public Result Update(Application application)
        {
            if (_applicationRepository.CheckExistSameNameWithNotSameId(application.Id, application.Name))
            {
                return Result.Error("该名称已存在");
            }

            var app = _applicationRepository.GetById(application.Id);
            if (app != null)
            {
                app.Name = application.Name;
                app.Icon = application.Icon;
                app.Group = application.Group;
                app.SortNumber = application.SortNumber;
                app.Description = application.Description;
                app.ModifyBy = -1;
                app.ModifyTime = DateTime.Now;

                _applicationRepository.Update(app);
            }
            return Result.Success();
        }

        public Result Delete(int id)
        {
            var metaObjects = dbContext.Queryable<MetaObject>().Where(t => t.ApplicationId == id).ToList();
            if (metaObjects != null && metaObjects.Any())
            {
                TransactionHelper.Transaction(() =>
                {
                    //删除应用下的所有对象
                    foreach (var item in metaObjects)
                    {
                        _metaObjectService.Delete(item.Id);
                    }
                });
            }
            base.Delete(id);
            return Result.Success();
        }
    }
}

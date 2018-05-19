using Microsoft.AspNetCore.Mvc;
using SevenTiny.Cloud.MultiTenantPlatform.DomainModel.Enums;
using SevenTiny.Cloud.MultiTenantPlatform.DomainModel.Repository;
using SevenTiny.Cloud.MultiTenantPlatform.Web.Models;
using System;
using System.Collections.Generic;

namespace SevenTiny.Cloud.MultiTenantPlatform.Web.Controllers
{
    public class ApplicationController : Controller
    {
        private readonly IRepository<DomainModel.Entities.Application> _applicationRepository;

        public ApplicationController(IRepository<DomainModel.Entities.Application> applicationRepository)
        {
            this._applicationRepository = applicationRepository;
        }

        public IActionResult Select()
        {
            List<DomainModel.Entities.Application> list = _applicationRepository.GetList(t => t.IsDeleted == (int)IsDeleted.NotDeleted);
            return View(list);
        }

        public IActionResult List()
        {
            List<DomainModel.Entities.Application> list = _applicationRepository.GetList(t => t.IsDeleted == (int)IsDeleted.NotDeleted);
            return View(list);
        }

        public IActionResult DeleteList()
        {
            List<DomainModel.Entities.Application> list = _applicationRepository.GetList(t => t.IsDeleted == (int)IsDeleted.Deleted);
            return View(list);
        }

        public IActionResult Add()
        {
            DomainModel.Entities.Application application = new DomainModel.Entities.Application();
            application.Icon = "cloud";
            return View(new ActionResultModel<DomainModel.Entities.Application>(true, string.Empty, application));
        }

        public IActionResult AddLogic(DomainModel.Entities.Application application)
        {
            if (string.IsNullOrEmpty(application.Name))
            {
                return View("Add", new ActionResultModel<DomainModel.Entities.Application>(false, "Application Name Can Not Be Null！", application));
            }
            if (string.IsNullOrEmpty(application.Code))
            {
                return View("Add", new ActionResultModel<DomainModel.Entities.Application>(false, "Application Code Can Not Be Null！", application));
            }
            _applicationRepository.Add(application);
            return RedirectToAction("List");
        }


        public IActionResult Update(int id)
        {
            var application = _applicationRepository.GetEntity(t => t.Id == id);
            return View(new ActionResultModel<DomainModel.Entities.Application>(true, string.Empty, application));

        }

        public IActionResult UpdateLogic(DomainModel.Entities.Application application)
        {
            if (application.Id == 0)
            {
                return View("Update", new ActionResultModel<DomainModel.Entities.Application>(false, "Application Id Can Not Be Null！", application));
            }
            if (string.IsNullOrEmpty(application.Name))
            {
                return View("Update", new ActionResultModel<DomainModel.Entities.Application>(false, "Application Name Can Not Be Null！", application));
            }
            if (string.IsNullOrEmpty(application.Code))
            {
                return View("Update", new ActionResultModel<DomainModel.Entities.Application>(false, "Application Code Can Not Be Null！", application));
            }
            DomainModel.Entities.Application app = _applicationRepository.GetEntity(t => t.Id == application.Id);
            if (app != null)
            {
                app.Name = application.Name;
                app.Icon = application.Icon;
                app.Group = application.Group;
                app.SortNumber = application.SortNumber;
                app.Description = application.Description;
                app.ModifyBy = -1;
                app.ModifyTime = DateTime.Now;
            }
            _applicationRepository.Update(t => t.Id == application.Id, app);
            return RedirectToAction("List");
        }

        public IActionResult Delete(int id)
        {
            DomainModel.Entities.Application application = _applicationRepository.GetEntity(t => t.Id == id);
            _applicationRepository.LogicDelete(t => t.Id == id, application);
            return JsonResultModel.Success("删除成功");
        }

        public IActionResult Recover(int id)
        {
            DomainModel.Entities.Application application = _applicationRepository.GetEntity(t => t.Id == id);
            _applicationRepository.Recover(t => t.Id == id, application);
            return JsonResultModel.Success("恢复成功");
        }
    }
}
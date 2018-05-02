using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SevenTiny.Cloud.MultiTenantPlatform.DomainModel.Entities;
using domain = SevenTiny.Cloud.MultiTenantPlatform.DomainModel;
using app = SevenTiny.Cloud.MultiTenantPlatform.Application;
using repository = SevenTiny.Cloud.MultiTenantPlatform.DomainModel.Repository;
using SevenTiny.Cloud.MultiTenantPlatform.Web.Models;

namespace SevenTiny.Cloud.MultiTenantPlatform.Web.Controllers
{
    public class ApplicationController : Controller
    {
        public IActionResult List()
        {
            List<domain.Entities.Application> list = repository.ApplicationRepository.GetApplicationList();
            if (list!=null)
            {
                list = list.OrderByDescending(t => t.SortNumber).ThenByDescending(t=>t.CreateTime).ToList();
            }
            return View(list);
        }
        public IActionResult Add()
        {
            return View();
        }

        public IActionResult AddLogic(domain.Entities.Application application)
        {
            if (string.IsNullOrEmpty(application.Name))
            {
                return AddFaild(application, "Application Name Can Not Be Null！");
            }
            if (string.IsNullOrEmpty(application.Code))
            {
                return AddFaild(application, "Application Code Can Not Be Null！");
            }
            repository.ApplicationRepository.AddApplication(application);
            return RedirectToAction("List");
        }

        public IActionResult AddFaild(domain.Entities.Application application,string msg)
        {
            return View("Add", new ActionResultModel<domain.Entities.Application>(false, "添加失败!", application));
        }

        public IActionResult Update()
        {
            return View();
        }
        public IActionResult Delete()
        {
            return View();
        }
    }
}
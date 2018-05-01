using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SevenTiny.Cloud.MultiTenantPlatform.DomainModel.Entities;
using domain = SevenTiny.Cloud.MultiTenantPlatform.DomainModel;
using app = SevenTiny.Cloud.MultiTenantPlatform.Application;
using repository = SevenTiny.Cloud.MultiTenantPlatform.DomainModel.Repository;

namespace SevenTiny.Cloud.MultiTenantPlatform.Web.Controllers
{
    public class ApplicationController : Controller
    {
        public IActionResult List()
        {
            List<domain.Entities.Application> list = repository.ApplicationRepository.GetApplicationList();
            return View(list);
        }
        public IActionResult Add()
        {
            return View();
        }

        public IActionResult AddApplication(domain.Entities.Application application)
        {
            repository.ApplicationRepository.AddApplication(application);
            return View("/Application/List");
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
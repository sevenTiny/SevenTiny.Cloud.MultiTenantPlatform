using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SevenTiny.Cloud.MultiTenantPlatform.DomainModel.Entities;
using SevenTiny.Cloud.MultiTenantPlatform.DomainModel.Enums;
using SevenTiny.Cloud.MultiTenantPlatform.DomainModel.Repository;
using SevenTiny.Cloud.MultiTenantPlatform.Web.Models;

namespace SevenTiny.Cloud.MultiTenantPlatform.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly IRepository<MetaObject> _metaObjectRepository;

        public HomeController(IRepository<MetaObject> metaObjectRepository)
        {
            this._metaObjectRepository = metaObjectRepository;
        }

        public IActionResult Index()
        {
            return Redirect("/Home/Application");
        }

        //切换应用的公共入口
        public IActionResult Application(string app)
        {
            if (string.IsNullOrEmpty(app))
            {
                return Redirect("/Application/Select");
            }
            HttpContext.Session.SetString("ApplicationCode", app);
            ViewData["Application"] = app;
            ViewData["MetaObjects"] = _metaObjectRepository.GetList(t => t.IsDeleted == (int)IsDeleted.NotDeleted);
            return View();
        }

       

        public IActionResult Welcome()
        {
            return View();
        }


        public IActionResult About()
        {
            ViewData["Message"] = "Your application description page.";

            return View();
        }

        public IActionResult Contact()
        {
            ViewData["Message"] = "Your contact page.";

            return View();
        }

        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}

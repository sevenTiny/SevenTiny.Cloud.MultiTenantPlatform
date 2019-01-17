using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SevenTiny.Cloud.MultiTenantPlatform.Domain.ServiceContract;
using SevenTiny.Cloud.MultiTenantPlatform.Web.Models;
using System.Diagnostics;

namespace SevenTiny.Cloud.MultiTenantPlatform.Web.Controllers
{
    public class HomeController : Controller
    {
        public HomeController(IApplicationService _applicationService, IMetaObjectService _metaObjectService)
        {
            applicationService = _applicationService;
            metaObjectService = _metaObjectService;
        }

        IApplicationService applicationService;
        IMetaObjectService metaObjectService;

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
            var application = applicationService.GetByCode(app);

            HttpContext.Session.SetInt32("ApplicationId", application.Id);
            HttpContext.Session.SetString("ApplicationCode", application.Code);

            ViewData["Application"] = app;
            ViewData["MetaObjects"] = metaObjectService.GetMetaObjectsUnDeletedByApplicationId(application.Id);

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

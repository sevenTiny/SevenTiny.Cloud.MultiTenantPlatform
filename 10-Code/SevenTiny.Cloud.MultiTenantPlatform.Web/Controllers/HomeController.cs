using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SevenTiny.Cloud.MultiTenantPlatform.DomainModel.Enums;
using SevenTiny.Cloud.MultiTenantPlatform.DomainModel.RepositoryInterface;
using SevenTiny.Cloud.MultiTenantPlatform.Web.Models;
using System.Diagnostics;

namespace SevenTiny.Cloud.MultiTenantPlatform.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly IMetaObjectRepository _metaObjectRepository;
        private readonly IApplicationRepository _applicationRepository;

        public HomeController(IMetaObjectRepository metaObjectRepository, IApplicationRepository applicationRepository)
        {
            this._metaObjectRepository = metaObjectRepository;
            this._applicationRepository = applicationRepository;
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
            DomainModel.Entities.Application application = _applicationRepository.GetEntity(t => t.Code.Equals(app));
            HttpContext.Session.SetInt32("ApplicationId", application.Id);
            ViewData["Application"] = app;
            ViewData["MetaObjects"] = _metaObjectRepository.GetList(t => t.ApplicationId == application.Id && t.IsDeleted == (int)IsDeleted.NotDeleted);
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

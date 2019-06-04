using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SevenTiny.Cloud.MultiTenantPlatform.Web.Models;
using System.Diagnostics;

namespace Seventiny.Cloud.DevelopmentWeb.Controllers
{
    public class HomeController : ControllerBase
    {
        public HomeController() { }

        public IActionResult Index()
        {
            return Redirect("/Application/Select");
        }

        public IActionResult Welcome(int applicationId, string applicationCode)
        {
            SetApplictionSession(applicationId, applicationCode);
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

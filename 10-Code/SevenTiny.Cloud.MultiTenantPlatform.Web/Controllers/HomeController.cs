using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SevenTiny.Cloud.MultiTenantPlatform.Web.Models;

namespace SevenTiny.Cloud.MultiTenantPlatform.Web.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index(string App)
        {
            if (string.IsNullOrEmpty(App))
            {
                return Redirect("/Home/Index?App=7TinyCloud Demo");
            }
            
            HttpContext.Session.SetString("Application", App);
            ViewData["Application"]=App;
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

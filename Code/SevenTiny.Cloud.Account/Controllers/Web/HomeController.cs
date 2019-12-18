using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SevenTiny.Cloud.Account.Core.Enum;
using SevenTiny.Cloud.Account.Models;

namespace SevenTiny.Cloud.Account.Controllers
{
    public class HomeController : WebControllerBase
    {
        [Authorize("Administrator")]
        public IActionResult Index()
        {
            ViewData["TenantName"] = CurrentTenantName;
            ViewData["UserName"] = CurrentUserName;
            ViewData["Identity"] = CurrentIdentity;
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        public IActionResult Welcome()
        {
            return View();
        }

        public IActionResult HTTP401()
        {
            return View();
        }

        public IActionResult HTTP403()
        {
            return View();
        }

        public IActionResult ErrorPage(ErrorType errorType)
        {
            ViewData["ErrorType"] = errorType;
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}

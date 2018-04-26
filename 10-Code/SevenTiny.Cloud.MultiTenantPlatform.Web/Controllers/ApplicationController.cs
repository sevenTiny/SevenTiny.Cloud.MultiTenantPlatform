using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SevenTiny.Cloud.MultiTenantPlatform.Model.Entities;

namespace SevenTiny.Cloud.MultiTenantPlatform.Web.Controllers
{
    public class ApplicationController : Controller
    {
        public IActionResult List()
        {
            List<Application> applicationList = new List<Application>
            {
                new Application
                {
                    Id=1001,
                    Name="图书管理",
                    Code="Book",
                    Description="关于图书的管理系统",
                    CreateBy=100101,
                    CreateTime=DateTimeOffset.Now,
                    IsDeleted=0
                },
                new Application
                {
                    Id=1002,
                    Name="人员管理",
                    Code="User",
                    Description="关于人员管理",
                    CreateBy=100101,
                    CreateTime=DateTimeOffset.Now,
                    IsDeleted=0
                },
                new Application
                {
                    Id=1003,
                    Name="后勤管理",
                    Code="Back",
                    Description="关于后勤管理",
                    CreateBy=100101,
                    CreateTime=DateTimeOffset.Now,
                    IsDeleted=0
                }
            };

            return View(applicationList);
        }
        public IActionResult Add()
        {
            return View();
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
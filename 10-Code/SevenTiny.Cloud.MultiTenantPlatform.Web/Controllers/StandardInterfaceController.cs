using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SevenTiny.Cloud.MultiTenantPlatform.Model.Entities;

namespace SevenTiny.Cloud.MultiTenantPlatform.Web.Controllers
{
    public class StandardInterfaceController : Controller
    {
        public IActionResult List()
        {
            List<StandardInterface> standardInterfaces = new List<StandardInterface>
            {
                new StandardInterface
                {
                    Id=10001,
                    Name="图书列表",
                    Code="BookList",
                    Description="关于图书的管理系统",
                    TableListId=101,
                    CreateBy=100101,
                    CreateTime=DateTimeOffset.Now,
                    IsDeleted=0
                },
                new StandardInterface
                {
                    Id=1002,
                    Name="人员管理列表",
                    Code="UserList",
                    Description="关于人员管理",
                    TableListId=102,
                    CreateBy=100101,
                    CreateTime=DateTimeOffset.Now,
                    IsDeleted=0
                },
                new StandardInterface
                {
                    Id=1003,
                    Name="后勤管理列表",
                    Code="BackList",
                    Description="关于后勤管理",
                    TableListId=103,
                    CreateBy=100101,
                    CreateTime=DateTimeOffset.Now,
                    IsDeleted=0
                }
            };

            return View(standardInterfaces);
        }
    }
}
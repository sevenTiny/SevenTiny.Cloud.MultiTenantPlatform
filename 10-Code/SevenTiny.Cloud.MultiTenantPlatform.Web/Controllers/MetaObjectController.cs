using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SevenTiny.Cloud.MultiTenantPlatform.DomainModel.Entities;
using SevenTiny.Cloud.MultiTenantPlatform.DomainModel.Repository;

namespace SevenTiny.Cloud.MultiTenantPlatform.Web.Controllers
{
    public class MetaObjectController : Controller
    {
        public IActionResult List()
        {
            return View();

            //IList<MetaObject> metaObjects = MetaObjectRepository.GetMetaObjects();
           // return View(metaObjects);
        }

        public IActionResult Add()
        {
            return View();
        }
    }
}
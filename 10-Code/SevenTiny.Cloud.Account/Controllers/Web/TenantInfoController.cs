using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SevenTiny.Cloud.Account.Core.ServiceContract;

namespace SevenTiny.Cloud.Account.Controllers
{
    [Authorize("Administrator")]
    public class TenantInfoController : WebControllerBase
    {
        ITenantInfoService _tenantInfoService;
        public TenantInfoController(
            ITenantInfoService tenantInfoService
            )
        {
            _tenantInfoService = tenantInfoService;
        }
        public IActionResult List()
        {
            var list = _tenantInfoService.GetEntitiesUnDeleted();
            return View(list);
        }
    }
}
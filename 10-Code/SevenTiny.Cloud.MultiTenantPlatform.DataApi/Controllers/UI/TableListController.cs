using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace SevenTiny.Cloud.MultiTenantPlatform.DataApi.Controllers
{
    [Route("api/UI/[controller]")]
    [ApiController]
    public class TableListController : ControllerBase
    {
        public string Get()
        {
            return "123";
        }
    }
}
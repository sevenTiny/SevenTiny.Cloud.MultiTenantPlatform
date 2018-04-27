using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace SevenTiny.Cloud.MultiTenantPlatform.DataApi.Controllers
{
    [Produces("application/json")]
    [Route("api/CloudData")]
    public class CloudDataController : Controller
    {
        // GET api/values/5
        [HttpGet]
        [HttpGet("{tenant:int}/{api:int}")]
        public string Get(int tenant, int api)
        {
            return $"tenant id:{tenant},api:{api}";
        }
    }
}
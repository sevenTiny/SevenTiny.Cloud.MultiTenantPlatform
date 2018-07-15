using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using SevenTiny.Cloud.MultiTenantPlatform.DataApi.Models;

namespace SevenTiny.Cloud.MultiTenantPlatform.DataApi.Controllers
{
    [Produces("application/json")]
    [Route("api/CloudData")]
    public class CloudDataController : Controller
    {
        // GET api/values/5
        [HttpGet]
        [HttpGet("{tenantId:int}/{api:int}")]
        public string Get(int tenantId, int api)
        {
            return $"tenantId:{tenantId},api:{api}";
        }

        public IActionResult Get(int tenantId, QueryArgs queryArgs)
        {
            try
            {
                //1.args check
                if (queryArgs == null)
                {
                    throw new ArgumentNullException("queryArgs can not be null!");
                }
                queryArgs.QueryArgsCheck();


                //todo:query and result

                return JsonResultModel.Success("");
                
            }
            catch (ArgumentException argEx)
            {
                return JsonResultModel.Error(argEx.Message);
            }
            catch (Exception ex)
            {
                return JsonResultModel.Error(JsonConvert.SerializeObject(ex));
            }
        }

        public IActionResult Post()
        {
            return null;
        }

        public IActionResult Update()
        {
            return null;
        }
        public IActionResult Delete()
        {
            return null;
        }
    }
}
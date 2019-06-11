using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Seventiny.Cloud.DataApi.Models;
using SevenTiny.Cloud.Infrastructure.Context;
using SevenTiny.Cloud.MultiTenantPlatform.Core.ValueObject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Seventiny.Cloud.DataApi.Controllers
{
    public abstract class ApiControllerBase : ControllerBase
    {
        /// <summary>
        /// 设置Session中ApplicationCode
        /// </summary>
        /// <param name="applicationCode"></param>
        protected void SetApplictionCodeToSession(string applicationCode)
        {
            HttpContext.Session.SetString("ApplicationCode", applicationCode);
        }

        /// <summary>
        /// 当前应用编码
        /// </summary>
        protected string CurrentApplicationCode
        {
            get
            {
                var applicationCode = HttpContext.Session.GetString("ApplicationCode");

                if (string.IsNullOrEmpty(applicationCode))
                    throw new ArgumentNullException("ApplicationCode is null");

                return applicationCode;
            }
        }
        /// <summary>
        /// 请求上下文信息
        /// </summary>
        private ApplicationContext _applicationContext;
        protected ApplicationContext CurrentApplicationContext
        {
            get
            {
                if (_applicationContext == null)
                {
                    _applicationContext = new ApplicationContext
                    {
                        ApplicationCode = CurrentApplicationCode,
                        TenantId = HttpContext.Session.GetInt32("TenantId").Value,
                        UserId = HttpContext.Session.GetInt32("UserId").Value,
                        UserEmail = HttpContext.Session.GetString("UserEmail")
                    };
                }
                return _applicationContext;
            }
        }
    }
}

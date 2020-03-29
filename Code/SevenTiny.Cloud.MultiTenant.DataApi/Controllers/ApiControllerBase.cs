using Microsoft.AspNetCore.Mvc;
using SevenTiny.Cloud.MultiTenantPlatform.DataApi.Models;
using SevenTiny.Cloud.MultiTenant.Infrastructure.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SevenTiny.Cloud.MultiTenantPlatform.DataApi.Controllers
{
    public abstract class ApiControllerBase : ControllerBase
    {
        private string _applicationCode;
        /// <summary>
        /// 设置Session中ApplicationCode
        /// </summary>
        /// <param name="applicationCode"></param>
        protected void SetApplictionCodeToSession(string applicationCode)
        {
            _applicationCode = applicationCode;
        }

        /// <summary>
        /// 当前应用编码
        /// </summary>
        protected string CurrentApplicationCode
        {
            get
            {
                if (string.IsNullOrEmpty(_applicationCode))
                    throw new ArgumentNullException("ApplicationCode is null");

                return _applicationCode;
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
                        TenantId = Convert.ToInt32(HttpContext.Items["TenantId"]),
                        UserId = Convert.ToInt32(HttpContext.Items["UserId"]),
                        UserEmail = Convert.ToString(HttpContext.Items["UserEmail"])
                    };

                    //check
                    if (_applicationContext.TenantId <= 0)
                    {
                        //throw new ArgumentNullException("TenantId", "TenantId incorrect! It could be the result of an identity failure");
                    }
                    if (_applicationContext.UserId <= 0)
                    {
                        //throw new ArgumentNullException("UserId", "UserId incorrect! It could be the result of an identity failure");
                    }
                }
                return _applicationContext;
            }
        }
    }
}

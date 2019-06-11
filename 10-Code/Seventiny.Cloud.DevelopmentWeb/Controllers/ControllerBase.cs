using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SevenTiny.Cloud.Infrastructure.Context;
using System;

namespace Seventiny.Cloud.DevelopmentWeb.Controllers
{
    /// <summary>
    /// 控制器基类
    /// </summary>
    public class ControllerBase : Controller
    {
        protected void SetApplictionSession(int applicationId, string applicationCode)
        {
            HttpContext.Session.SetInt32("ApplicationId", applicationId);
            HttpContext.Session.SetString("ApplicationCode", applicationCode);
        }

        protected void SetMetaObjectSession(int metaObjectId, string metaObjectCode)
        {
            HttpContext.Session.SetInt32("MetaObjectId", metaObjectId);
            HttpContext.Session.SetString("MetaObjectCode", metaObjectCode);
        }

        /// <summary>
        /// 当前应用Id
        /// </summary>
        protected int CurrentApplicationId
        {
            get
            {
                //先将应用名赋值到ViewData
                string appName = CurrentApplicationCode;
                var applicationId = HttpContext.Session.GetInt32("ApplicationId");

                if (applicationId == null)
                    Response.Redirect("/Application/Select");

                return applicationId ?? throw new ArgumentNullException("ApplicationId is null,please select application first!");
            }
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
                    Response.Redirect("/Application/Select");

                ViewData["Application"] = applicationCode ?? throw new ArgumentNullException("ApplicationCode is null,please select application first!");
                return applicationCode;
            }
        }

        /// <summary>
        /// 当前对象Id
        /// </summary>
        protected int CurrentMetaObjectId
        {
            get
            {
                return HttpContext.Session.GetInt32("MetaObjectId") ?? throw new ArgumentNullException("MetaObjectId is null,please select MetaObject first!"); ;
            }
        }

        /// <summary>
        /// 当前对象编码
        /// </summary>
        protected string CurrentMetaObjectCode
        {
            get
            {
                return HttpContext.Session.GetString("MetaObjectCode") ?? throw new ArgumentNullException("MetaObjectCode is null,please select MetaObject first!"); ;
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
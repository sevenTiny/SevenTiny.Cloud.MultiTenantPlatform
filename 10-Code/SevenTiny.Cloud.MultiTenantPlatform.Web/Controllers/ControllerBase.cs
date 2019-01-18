using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;

namespace SevenTiny.Cloud.MultiTenantPlatform.Web.Controllers
{
    /// <summary>
    /// 控制器基类
    /// </summary>
    public class ControllerBase : Controller
    {
        /// <summary>
        /// 当前应用Id
        /// </summary>
        protected int CurrentApplicationId
        {
            get
            {
                //先将应用名赋值到ViewData
                string appName = CurrentApplicationCode;
                return HttpContext.Session.GetInt32("ApplicationId") ?? throw new ArgumentNullException("ApplicationId is null,please select application first!");
            }
        }
        /// <summary>
        /// 当前应用编码
        /// </summary>
        protected string CurrentApplicationCode
        {
            get
            {
                string appName = HttpContext.Session.GetString("ApplicationCode") ?? throw new ArgumentNullException("ApplicationCode is null,please select application first!");
                ViewData["Application"] = appName;
                return appName;
            }
        }
    }
}
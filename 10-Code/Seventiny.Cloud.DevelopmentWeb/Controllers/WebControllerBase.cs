using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Seventiny.Cloud.DevelopmentWeb.Filters;
using Seventiny.Cloud.DevelopmentWeb.Helpers;
using SevenTiny.Cloud.Infrastructure.Configs;
using SevenTiny.Cloud.Infrastructure.Const;
using SevenTiny.Cloud.Infrastructure.Context;
using SevenTiny.Cloud.MultiTenantPlatform.Core.Enum;
using System;
using System.Linq;

namespace Seventiny.Cloud.DevelopmentWeb.Controllers
{
    /// <summary>
    /// 控制器基类
    /// </summary>
    [DevelopmentAuthFilter]
    //[Authorize]
    public class WebControllerBase : Controller
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
                        TenantId = CurrentTenantId,
                        UserId = CurrentUserId,
                        UserEmail = CurrentUserEmail
                    };
                }
                return _applicationContext;
            }
        }


        /// <summary>
        /// 从Token串中获取参数
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        private string GetArgumentFromToken(string key)
        {
            return HttpContext.GetArgumentFromToken(key);
        }

        protected int CurrentTenantId
        {
            get
            {
                var result = Convert.ToInt32(GetArgumentFromToken(AccountConst.KEY_TenantId));

                if (result <= 0)
                    Response.Redirect("/UserAccount/Login");

                return result;
            }
        }

        protected string CurrentTenantName
        {
            get
            {
                var result = GetArgumentFromToken(AccountConst.KEY_TenantName);
                return result ?? CurrentTenantId.ToString();
            }
        }

        protected int CurrentUserId
        {
            get
            {
                var result = Convert.ToInt32(GetArgumentFromToken(AccountConst.KEY_UserId));

                if (result <= 0)
                    Response.Redirect($"{UrlsConfig.Instance.Account}/UserAccount/Login?_redirectUrl={UrlsConfig.Instance.DevelopmentWebUrl}/Home/Index");

                return result;
            }
        }

        protected string CurrentUserEmail
        {
            get
            {
                var result = GetArgumentFromToken(AccountConst.KEY_UserEmail);

                if (string.IsNullOrEmpty(result))
                    Response.Redirect("/UserAccount/Login");

                return result;
            }
        }

        protected string CurrentUserName
        {
            get
            {
                var result = GetArgumentFromToken(AccountConst.KEY_UserName);
                return result ?? CurrentUserEmail;
            }
        }

        protected int CurrentIdentity
        {
            get
            {
                var result = Convert.ToInt32(GetArgumentFromToken(AccountConst.KEY_SystemIdentity));
                return result;
            }
        }

        /// <summary>
        /// 将用户信息存到ViewData里面用于页面展示
        /// </summary>
        protected void SetUserInfoToViewData()
        {
            ViewData["UserIdentity"] = SystemIdentityTranslator.ToChinese(CurrentIdentity);
            ViewData["UserName"] = CurrentUserName;
        }
    }
}
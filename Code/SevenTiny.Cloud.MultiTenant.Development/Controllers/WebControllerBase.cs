using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SevenTiny.Cloud.MultiTenant.Development.Filters;
using SevenTiny.Cloud.MultiTenant.Development.Helpers;
using SevenTiny.Cloud.MultiTenant.Infrastructure.Configs;
using SevenTiny.Cloud.MultiTenant.Infrastructure.Const;
using SevenTiny.Cloud.MultiTenant.Infrastructure.Context;
using SevenTiny.Cloud.MultiTenant.Domain.Enum;
using System;
using System.Linq;

namespace SevenTiny.Cloud.MultiTenant.Development.Controllers
{
    /// <summary>
    /// 控制器基类
    /// </summary>
    [DevelopmentAuthFilter]
    //[Authorize]
    public class WebControllerBase : Controller
    {
        protected void SetApplictionSession(Guid applicationId, string applicationCode)
        {
            HttpContext.Session.SetString("ApplicationId", applicationId.ToString());
            HttpContext.Session.SetString("ApplicationCode", applicationCode);
        }

        protected void SetMetaObjectSession(Guid metaObjectId, string metaObjectCode)
        {
            HttpContext.Session.SetString("MetaObjectId", metaObjectId.ToString());
            HttpContext.Session.SetString("MetaObjectCode", metaObjectCode);
        }

        /// <summary>
        /// 当前应用Id
        /// </summary>
        protected Guid CurrentApplicationId
        {
            get
            {
                var applicationId = HttpContext.Session.GetString("ApplicationId");

                if (string.IsNullOrEmpty(applicationId))
                {
                    Response.Redirect("/CloudApplication/Select");
                }

                return Guid.Parse(applicationId);
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
                    Response.Redirect("/CloudApplication/Select");

                ViewData["Application"] = applicationCode ?? throw new ArgumentNullException("ApplicationCode is null,please select application first!");
                return applicationCode;
            }
        }

        /// <summary>
        /// 当前对象Id
        /// </summary>
        protected Guid CurrentMetaObjectId
        {
            get
            {
                var id = HttpContext.Session.GetString("MetaObjectId");

                if (string.IsNullOrEmpty(id))
                    throw new ArgumentNullException("MetaObjectId is null,please select MetaObject first!");

                return Guid.Parse(id);
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
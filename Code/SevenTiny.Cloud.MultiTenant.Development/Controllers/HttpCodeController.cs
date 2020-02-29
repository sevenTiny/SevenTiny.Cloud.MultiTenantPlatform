using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SevenTiny.Cloud.MultiTenant.Development.Helpers;
using SevenTiny.Cloud.MultiTenant.Infrastructure.Configs;
using SevenTiny.Cloud.MultiTenant.Infrastructure.Const;

namespace SevenTiny.Cloud.MultiTenant.Development.Controllers
{
    /// <summary>
    /// 该控制器集成Base，因为不受权限控制
    /// </summary>
    public class HttpCodeController : Controller
    {
        /// <summary>
        /// 从Token串中获取参数
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        private string GetArgumentFromToken(string key)
        {
            return HttpContext.GetArgumentFromToken(key);
        }
        protected int CurrentUserId
        {
            get
            {
                var result = Convert.ToInt32(GetArgumentFromToken(AccountConst.KEY_UserId));

                if (result <= 0)
                    Response.Redirect("/UserAccount/Login");

                return result;
            }
        }

        protected string CurrentUserEmail
        {
            get
            {
                var result = GetArgumentFromToken(AccountConst.KEY_UserEmail);

                if (string.IsNullOrEmpty(result))
                    Response.Redirect($"{UrlsConfig.Instance.Account}/UserAccount/Login?_redirectUrl={UrlsConfig.Instance.DevelopmentWebUrl}/Home/Index");

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

        public IActionResult Http401()
        {
            return View();
        }

        public IActionResult Http403()
        {
            ViewData["UserName"] = CurrentUserName;
            return View();
        }
    }
}
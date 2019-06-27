using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SevenTiny.Cloud.Infrastructure.Const;
using System;
using System.Linq;

namespace SevenTiny.Cloud.Account.Controllers
{
    public class WebControllerBase : Controller
    {
        /// <summary>
        /// 从Token串中获取参数
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        private string GetArgumentFromToken(string key)
        {
            var auth = HttpContext.AuthenticateAsync()?.Result?.Principal?.Claims;
            return auth?.FirstOrDefault(t => t.Type.Equals(key))?.Value;
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
    }
}

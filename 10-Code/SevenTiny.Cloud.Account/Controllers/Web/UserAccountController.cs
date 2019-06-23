using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SevenTiny.Bantina;
using SevenTiny.Cloud.Account.AuthManagement;
using SevenTiny.Cloud.Account.Core.Const;
using SevenTiny.Cloud.Account.Core.Entity;
using SevenTiny.Cloud.Account.Core.ServiceContract;
using SevenTiny.Cloud.Account.DTO;
using SevenTiny.Cloud.Account.Models;

namespace SevenTiny.Cloud.Account.Controllers
{
    public class UserAccountController : WebControllerBase
    {
        IUserAccountService _userAccountService;
        public UserAccountController(
            IUserAccountService userAccountService
            )
        {
            _userAccountService = userAccountService;
        }
        [AllowAnonymous]
        public IActionResult Login()
        {
            return View();
        }

        [AllowAnonymous]
        public IActionResult LoginLogic(LoginModel loginModel)
        {
            var loginResult = Result<UserAccount>.Success("登陆成功")
                .ContinueAssert(!string.IsNullOrEmpty(loginModel.Email), "email can not be null")
                .ContinueAssert(!string.IsNullOrEmpty(loginModel.Password), "password can not be null")
                .ContinueAssert(!string.IsNullOrEmpty(loginModel.RedirectUrl), "redirectUrl can not be null")
                .Continue(re => _userAccountService.LoginByEmail(new UserAccount { Email = loginModel.Email, Password = loginModel.Password }));

            if (loginResult.IsSuccess)
            {
                //将租户Id存入Session
                SetTenantIdToSession(loginResult.Data.TenantId);
                SetTenantIdToSession(loginResult.Data.Id);
                //get token
                var token = TokenManagement.GetToken(loginResult.Data).Data;
                //set token to cookie
                Response.Cookies.Append(AccountConst.KEY_ACCESSTOKEN, token);
                //concat url
                string redireUrl = loginModel.RedirectUrl;
                if (redireUrl.Contains('?'))
                    redireUrl = $"{redireUrl}&{AccountConst.KEY_ACCESSTOKEN}={token}";
                else
                    redireUrl = $"{redireUrl}?{AccountConst.KEY_ACCESSTOKEN}={token}";
                return Redirect(redireUrl);
            }
            return View("Login", loginResult.ToResponseModel());
        }

        [AllowAnonymous]
        public IActionResult LogOut()
        {
            //clear cookie
            Response.Cookies.Delete(AccountConst.KEY_ACCESSTOKEN);
            return Redirect("/UserAccount/Login");
        }

        [AllowAnonymous]
        public IActionResult SignUp()
        {
            return View();
        }

        [AllowAnonymous]
        public IActionResult SignUpLogic(UserAccount userAccount)
        {
            return Content("不支持用户主动注册，请联系管理员添加账户！");
        }

        [Authorize("Administrator")]
        public IActionResult AddAccount()
        {
            return View();
        }

        [Authorize("Administrator")]
        public IActionResult AddAccountLogic(UserAccount userAccount)
        {
            var result = Result.Success("添加成功")
                .Continue(re =>
                {
                    //赋值当前租户Id
                    userAccount.TenantId = CurrentTenantId;
                    //注册用户
                    return _userAccountService.SignUpByEmail(userAccount);
                });
            return View();
        }

        [Authorize("Administrator")]
        public IActionResult List()
        {
            var list = _userAccountService.GetUserAccountsByTenantId(CurrentTenantId);
            return View(list);
        }

        [Authorize]
        public IActionResult EditByUser(int userId)
        {
            return View();
        }

        [Authorize]
        public IActionResult EditByUserLogic()
        {
            return View();
        }

        [Authorize("Administrator")]
        public IActionResult EditByAdministrator(int userId)
        {
            return View();
        }

        [Authorize("Administrator")]
        public IActionResult EditByAdministratorLogic()
        {
            return View();
        }

        [Authorize("Administrator")]
        public IActionResult Delete(int id)
        {
            //永久删除
            _userAccountService.Delete(id);
            return JsonResultModel.Success("删除成功");
        }
    }
}
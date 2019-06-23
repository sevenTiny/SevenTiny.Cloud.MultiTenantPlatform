using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SevenTiny.Bantina;
using SevenTiny.Bantina.Validation;
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
        TokenManagement _tokenManagement;
        public UserAccountController(
            IUserAccountService userAccountService,
            TokenManagement tokenManagement
            )
        {
            _userAccountService = userAccountService;
            _tokenManagement = tokenManagement;
        }
        [AllowAnonymous]
        public IActionResult Login()
        {
            var httpCode = Convert.ToString(Request.Query["httpCode"]);
            if (httpCode != null && httpCode.Equals(401.ToString()))
            {
                return View(ResponseModel.Error("身份认证失败，请重新登陆!"));
            }
            return View();
        }

        [AllowAnonymous]
        public IActionResult LoginLogic(LoginModel loginModel)
        {
            var userAccount = new UserAccount { Email = loginModel.Email, Password = loginModel.Password };
            var loginResult = Result<UserAccount>.Success("登陆成功", userAccount)
                .ContinueAssert(!string.IsNullOrEmpty(loginModel.RedirectUrl), "redirectUrl can not be null")
                .Continue(re => _userAccountService.LoginByEmail(userAccount));

            if (loginResult.IsSuccess)
            {
                //get token
                var token = _tokenManagement.GetToken(loginResult.Data).Data;
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

            //将上次的值提供到前端
            loginResult.Data = userAccount;
            return View("Login", loginResult.ToResponseModel());
        }

        [AllowAnonymous]
        public IActionResult Logout()
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
            return View(ResponseModel.Success(new UserAccount()));
        }

        [Authorize("Administrator")]
        public IActionResult AddAccountLogic(UserAccount userAccount)
        {
            var result = Result.Success("添加成功")
                .Continue(re =>
                {
                    //赋值当前租户Id
                    userAccount.TenantId = CurrentTenantId;
                    //默认账号密码都是123456
                    userAccount.Password = "123456";
                    userAccount.CreateBy = CurrentUserId;
                    //注册用户
                    return _userAccountService.SignUpByEmail(userAccount);
                });

            if (result.IsSuccess)
            {
                return Redirect("/UserAccount/List");
            }
            return View(ResponseModel.Error(result.Message, userAccount));
        }

        [Authorize("Administrator")]
        public IActionResult List()
        {
            var list = _userAccountService.GetUserAccountsByTenantId(CurrentTenantId).Data;
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
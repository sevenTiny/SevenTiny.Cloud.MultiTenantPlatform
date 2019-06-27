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
using SevenTiny.Cloud.Account.Core.Entity;
using SevenTiny.Cloud.Account.Core.Enum;
using SevenTiny.Cloud.Account.Core.ServiceContract;
using SevenTiny.Cloud.Account.DTO;
using SevenTiny.Cloud.Account.Models;
using SevenTiny.Cloud.Infrastructure.Const;

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
            var httpCode = Convert.ToString(Request.Query["_httpCode"]);
            //传递跳转链接
            string redirectUrl = Convert.ToString(Request.Query["_redirectUrl"]);
            //如果没传该参数，得到错误
            if (string.IsNullOrEmpty(redirectUrl))
                return Redirect("/Home/ErrorPage?errorType=" + (int)ErrorType.NoRedirect);

            ViewData["RedirectUrl"] = redirectUrl;
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
                Response.Cookies.Append(AccountConst.KEY_AccessToken, token);
                //concat url
                string redireUrl = loginModel.RedirectUrl;
                if (redireUrl.Contains('?'))
                    redireUrl = $"{redireUrl}&{AccountConst.KEY_AccessToken}={token}";
                else
                    redireUrl = $"{redireUrl}?{AccountConst.KEY_AccessToken}={token}";
                return Redirect(redireUrl);
            }

            //将上次的值提供到前端
            loginResult.Data = userAccount;
            ViewData["RedirectUrl"] = loginModel.RedirectUrl;
            return View("Login", loginResult.ToResponseModel());
        }

        [AllowAnonymous]
        public IActionResult Logout()
        {
            //clear cookie
            Response.Cookies.Delete(AccountConst.KEY_AccessToken);
            //传递跳转链接
            string redirectUrl = Convert.ToString(Request.Query["_redirectUrl"]);
            return Redirect($"/UserAccount/Login?_redirectUrl={redirectUrl}");
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
        public IActionResult EditAccount(int id)
        {
            var entity = _userAccountService.GetById(id);
            return View(ResponseModel.Success(entity));
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
        public IActionResult EditAccountLogic(UserAccount userAccount)
        {
            var result = Result.Success("修改成功")
                .Continue(re =>
                {
                    return _userAccountService.Update(userAccount);
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
        public IActionResult LogicDelete(int id)
        {
            _userAccountService.LogicDelete(id);
            return JsonResultModel.Success("删除成功");
        }
    }
}
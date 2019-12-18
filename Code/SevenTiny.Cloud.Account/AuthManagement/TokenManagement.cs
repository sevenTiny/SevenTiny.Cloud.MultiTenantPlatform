using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using SevenTiny.Bantina;
using SevenTiny.Cloud.Account.Core.Entity;
using SevenTiny.Cloud.Account.Core.ServiceContract;
using SevenTiny.Cloud.Infrastructure.Configs;
using SevenTiny.Cloud.Infrastructure.Const;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace SevenTiny.Cloud.Account.AuthManagement
{
    public class TokenManagement
    {
        ITenantInfoService _tenantInfoService;
        IUserAppLicenseService _userAppLicenseService;
        public TokenManagement(
            ITenantInfoService tenantInfoService,
            IUserAppLicenseService userAppLicenseService
            )
        {
            _tenantInfoService = tenantInfoService;
            _userAppLicenseService = userAppLicenseService;
        }

        public Result<string> GetToken(UserAccount userAccount)
        {
            //将租户Id存入Session
            var tenantInfo = _tenantInfoService.GetById(userAccount.TenantId);
            var license = _userAppLicenseService.GetTenantApplicationLicenseDtos(userAccount.TenantId);
            // push the user’s name into a claim, so we can identify the user later on.
            var claims = new[]
            {
                //租户Id
                new Claim(AccountConst.KEY_TenantId, userAccount.TenantId.ToString()),
                //租户名称
                new Claim(AccountConst.KEY_TenantName, tenantInfo.TenantName),
                //用户Id
                new Claim(AccountConst.KEY_UserId,userAccount.Id.ToString()),
                //用户邮箱
                new Claim(AccountConst.KEY_UserEmail,userAccount.Email),
                //用户名
                new Claim(AccountConst.KEY_UserName,userAccount.Name),
                //用户系统身份
                new Claim(AccountConst.KEY_SystemIdentity,userAccount.SystemIdentity.ToString()),
                //开发态权限
                new Claim(AccountConst.KEY_HasDevelopmentSystemPermission,userAccount.HasDevelopmentSystemPermission.ToString()),
                //实施态权限
                new Claim(AccountConst.KEY_HasSettingSystemPermission,userAccount.HasSettingSystemPermission.ToString()),
                //官方Web站点权限
                new Claim(AccountConst.KEY_HasOfficialSystemPermission,userAccount.HasOfficialSystemPermission.ToString()),
                //用户应用授权信息
                new Claim(AccountConst.KEY_UserIdentityLicense,JsonConvert.SerializeObject(license)),
            };
            //sign the token using a secret key.This secret will be shared between your API and anything that needs to check that the token is legit.
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(AccountConfig.Instance.SecurityKey));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            //.NET Core’s JwtSecurityToken class takes on the heavy lifting and actually creates the token.
            var token = new JwtSecurityToken(
                issuer: AccountConfig.Instance.TokenIssuer,
                audience: AccountConfig.Instance.TokenAudience,
                claims: claims,
                expires: DateTime.Now.AddMinutes(AccountConfig.Instance.TokenExpiredMinutesTimeSpan),
                signingCredentials: creds);

            string tokenStr = new JwtSecurityTokenHandler().WriteToken(token);

            return Result<string>.Success("get token success", tokenStr);
        }
    }
}

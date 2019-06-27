using Microsoft.IdentityModel.Tokens;
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
        public TokenManagement(
            ITenantInfoService tenantInfoService
            )
        {
            _tenantInfoService = tenantInfoService;
        }

        public Result<string> GetToken(UserAccount userAccount)
        {
            //将租户Id存入Session
            var tenantInfo = _tenantInfoService.GetById(userAccount.TenantId);
            // push the user’s name into a claim, so we can identify the user later on.
            var claims = new[]
            {
                //new Claim(JwtRegisteredClaimNames.Nbf,$"{new DateTimeOffset(DateTime.Now).ToUnixTimeSeconds()}") ,
                //new Claim (JwtRegisteredClaimNames.Exp,$"{new DateTimeOffset(DateTime.Now.AddSeconds(100)).ToUnixTimeSeconds()}"),
                new Claim(AccountConst.KEY_TENANTID, userAccount.TenantId.ToString()),
                new Claim(AccountConst.KEY_TENANTNAME, tenantInfo.TenantName),
                new Claim(AccountConst.KEY_USERID,userAccount.Id.ToString()),
                new Claim(AccountConst.KEY_USEREMAIL,userAccount.Email),
                new Claim(AccountConst.KEY_USERNAME,userAccount.Name),
                new Claim(AccountConst.KEY_SYSTEMIDENTITY,userAccount.SystemIdentity.ToString()),
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

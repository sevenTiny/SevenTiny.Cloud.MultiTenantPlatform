using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SevenTiny.Cloud.Account.Core.Const;

namespace SevenTiny.Cloud.Account.Controllers
{
    public class WebControllerBase : Controller
    {
        protected void SetTenantIdToSession(int tenantId)
        {
            HttpContext.Session.SetInt32(AccountConst.KEY_TENANTID, tenantId);
        }

        protected void SetUserIdToSession(int userId)
        {
            HttpContext.Session.SetInt32(AccountConst.KEY_USERID, userId);
        }

        protected int CurrentTenantId
        {
            get
            {
                var tenantId = HttpContext.Session.GetInt32(AccountConst.KEY_TENANTID);

                if (tenantId == null || tenantId.Value <= 0)
                    Response.Redirect("/UserAccount/Login");

                return tenantId.Value;
            }
        }

        protected int CurrentUserId
        {
            get
            {
                var tenantId = HttpContext.Session.GetInt32(AccountConst.KEY_USERID);

                if (tenantId == null || tenantId.Value <= 0)
                    Response.Redirect("/UserAccount/Login");

                return tenantId.Value;
            }
        }
    }
}

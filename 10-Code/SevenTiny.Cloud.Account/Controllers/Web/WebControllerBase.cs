using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace SevenTiny.Cloud.Account.Controllers
{
    public class WebControllerBase : Controller
    {
        protected int CurrentTenantId
        {
            get
            {
                var tenantId = HttpContext.Session.GetInt32("TenantId");

                if (tenantId == null || tenantId.Value <= 0)
                    Response.Redirect("/UserAccount/Login");

                return tenantId.Value;
            }
        }
    }
}

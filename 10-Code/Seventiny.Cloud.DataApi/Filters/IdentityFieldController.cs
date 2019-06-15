using Microsoft.AspNetCore.Mvc;
using SevenTiny.Cloud.MultiTenantPlatform.Core.ServiceContract;

namespace SevenTiny.Cloud.MultiTenantPlatform.Web.Controllers
{
    public class IdentityFieldController : ControllerBase
    {
        readonly IIdentityFieldService identityFieldService;
        readonly IFieldListService fieldListService;
        readonly IMetaFieldService metaFieldService;
        readonly IInterfaceAggregationService interfaceAggregationService;
        //readonly IFieldListAggregationService fieldAggregationService;

        public IdentityFieldController(IFieldListService _fieldListService, IMetaFieldService _metaFieldService, 
            IInterfaceAggregationService _interfaceAggregationService,  
            IIdentityFieldService _identityFieldService)
        {
            identityFieldService = _identityFieldService;
            fieldListService = _fieldListService;
            metaFieldService = _metaFieldService;
            interfaceAggregationService = _interfaceAggregationService;
            //fieldAggregationService = _fieldAggregationService;
        }

        // GET
        public IActionResult List()
        {
            return null;
            //return View(identityFieldService.GetEntitiesUnDeletedByMetaObjectId(CurrentMetaObjectId));
        }
    }
}
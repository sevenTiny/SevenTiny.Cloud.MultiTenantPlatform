using SevenTiny.Cloud.MultiTenantPlatform.Application.ServiceContract;
using SevenTiny.Cloud.MultiTenantPlatform.DomainModel.Entities;
using SevenTiny.Cloud.MultiTenantPlatform.DomainModel.RepositoryContract;

namespace SevenTiny.Cloud.MultiTenantPlatform.Application.Service
{
    public class InterfaceAggregationService : IInterfaceAggregationService
    {
        private readonly IConditionAggregationRepository _conditionAggregationRepository;
        private readonly IInterfaceAggregationRepository _interfaceAggregationRepository;
        public InterfaceAggregationService(IConditionAggregationRepository conditionAggregationRepository, IInterfaceAggregationRepository interfaceAggregationRepository)
        {
            this._conditionAggregationRepository = conditionAggregationRepository;
            _interfaceAggregationRepository = interfaceAggregationRepository;
        }

        public ConditionAggregation GetConditionAggregationByInterfaceAggregationCode(string interfaceAggregationCode)
        {
            var interfaceAggregation = _interfaceAggregationRepository.GetInterfaceAggregationByCode(interfaceAggregationCode);
            if (interfaceAggregation == null)
            {
                return null;
            }
            return _conditionAggregationRepository.GetConditionAggregationById(interfaceAggregation.InterfaceSearchConditionId);
        }

        public ConditionAggregation GetConditionAggregationByInterfaceAggregationId(int interfaceAggregationId)
        {
            var interfaceAggregation = _interfaceAggregationRepository.GetInterfaceAggregationById(interfaceAggregationId);
            if (interfaceAggregation==null)
            {
                return null;
            }
            return _conditionAggregationRepository.GetConditionAggregationById(interfaceAggregation.InterfaceSearchConditionId);
        }
    }
}

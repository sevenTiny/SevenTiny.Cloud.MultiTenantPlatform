using SevenTiny.Bantina;
using SevenTiny.Bantina.Validation;
using SevenTiny.Cloud.MultiTenant.Domain.DbContext;
using SevenTiny.Cloud.MultiTenant.Domain.Entity;
using SevenTiny.Cloud.MultiTenant.Domain.Enum;
using SevenTiny.Cloud.MultiTenant.Domain.Repository;
using SevenTiny.Cloud.MultiTenant.Domain.RepositoryContract;
using SevenTiny.Cloud.MultiTenant.Domain.ServiceContract;
using System;
using System.Collections.Generic;
using System.Text;

namespace SevenTiny.Cloud.MultiTenant.Domain.Service
{
    internal class DataSourceService : CommonServiceBase<DataSource>, IDataSourceService
    {
        public DataSourceService(IDataSourceRepository sourceRepository) : base(sourceRepository)
        {
            _sourceRepository = sourceRepository;
        }

        IDataSourceRepository _sourceRepository;

        /// <summary>
        /// 更新对象
        /// </summary>
        /// <param name="entity"></param>
        public new Result Update(DataSource entity)
        {
            return UpdateWithOutCode(entity, target =>
            {
                target.Language = entity.Language;
                target.Script = entity.Script;
            });
        }

        public List<DataSource> GetListByApplicationIdAndDataSourceType(Guid applicationId, DataSourceType dataSourceType)
        {
            return _sourceRepository.GetListByApplicationIdAndDataSourceType(applicationId, dataSourceType);
        }
    }
}

using SevenTiny.Bantina;
using SevenTiny.Cloud.MultiTenant.Domain.Entity;
using System;
using System.Collections.Generic;
using System.Text;

namespace SevenTiny.Cloud.MultiTenant.Domain.RepositoryContract
{
    internal interface ICloudInterfaceRepository : IMetaObjectCommonRepositoryBase<CloudInterface>
    {
        bool CheckFormIdExist(Guid formId);
    }
}

using SevenTiny.Cloud.MultiTenant.Domain.Entity;
using System;
using System.Collections.Generic;

namespace SevenTiny.Cloud.MultiTenant.Domain.RepositoryContract
{
    public interface IMetaFieldRepository : IMetaObjectCommonRepositoryBase<MetaField>
    {
        List<MetaField> GetSystemAndCustomListUnDeleted(Guid metaObjectId);
    }
}

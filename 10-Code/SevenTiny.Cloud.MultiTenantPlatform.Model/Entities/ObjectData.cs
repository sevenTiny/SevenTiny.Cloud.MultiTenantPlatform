using System;
using System.Collections.Generic;
using System.Linq;

namespace SevenTiny.Cloud.MultiTenantPlatform.Model.Entities
{
    /// <summary>
    /// query object data
    /// </summary>
    public class ObjectData: EntityInfo
    {
        /// <summary>
        /// tenant id,means the data belong to the tenant
        /// </summary>
        public int TenantId { get; set; }
        /// <summary>
        /// user id,means the data belong to the user
        /// </summary>
        public int UserId { get; set; }
        /// <summary>
        /// meta object data(key,type,value)
        /// </summary>
        public Data[] MetaObjects { get; set; }

        public string MetaDataName { get; set; }
        public Guid MetaDataId { get; set; }
        /// <summary>
        /// get MetaObject by propertyName which same as DataKey
        /// </summary>
        /// <param name="fieldKey"></param>
        /// <returns></returns>
        public object this[string fieldKey]
            => this.MetaFields.ContainsKey(fieldKey) ? this.MetaFields[fieldKey].DataValue : null;

        private Dictionary<string, Data> _MetaFields { get; set; }
        /// <summary>
        /// MetaObjects Dictionary type
        /// </summary>
        public Dictionary<string, Data> MetaFields
        {
            get
            {
                if (this._MetaFields == null)
                {
                    this._MetaFields = this.MetaObjects.ToDictionary(t => t.DataKey, v => v);
                }
                return this._MetaFields;
            }
        }

        public int FieldsCount => MetaObjects.Count();
    }
}

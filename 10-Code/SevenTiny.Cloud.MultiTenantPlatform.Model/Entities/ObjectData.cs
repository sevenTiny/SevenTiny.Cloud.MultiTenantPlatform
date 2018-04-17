using System.Collections.Generic;
using System.Linq;

namespace SevenTiny.Cloud.MultiTenantPlatform.Model.Entities
{
    public class ObjectData
    {
        public MetaObject[] MetaObjects { get; set; }
        public string MetaDataName { get; set; }
        public string MetaDataId { get; set; }
        /// <summary>
        /// get MetaObject by propertyName which same as DataKey
        /// </summary>
        /// <param name="propertyName"></param>
        /// <returns></returns>
        public object this[string propertyName]
            => this.MetaObjectDictionary.ContainsKey(propertyName) ? this.MetaObjectDictionary[propertyName] : null;
        private Dictionary<string, MetaObject> _MetaObjectDictionary { get; set; }
        public Dictionary<string, MetaObject> MetaObjectDictionary
        {
            get
            {
                if (this._MetaObjectDictionary == null)
                {
                    this._MetaObjectDictionary = this.MetaObjects.ToDictionary(t => t.DataKey, v => v);
                }
                return this._MetaObjectDictionary;
            }
        }
        public void Update(string propertyName, object metaDataValue)
        {
            if (this.MetaObjectDictionary.ContainsKey(propertyName))
            {
                this.MetaObjectDictionary[propertyName].DataValue = metaDataValue;
            }
        }
        public void Updata(string propertyName, MetaObject metaObject)
        {
            if (this.MetaObjectDictionary.ContainsKey(propertyName))
            {
                this.MetaObjectDictionary[propertyName] = metaObject;
            }
        }
    }
}

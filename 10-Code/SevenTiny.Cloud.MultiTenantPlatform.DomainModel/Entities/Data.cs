using SevenTiny.Cloud.MultiTenantPlatform.Model.Enums;

namespace SevenTiny.Cloud.MultiTenantPlatform.DomainModel.Entities
{
    /// <summary>
    /// MetaData
    /// </summary>
    public class Data
    {
        public Data(string dataKey, DataType dataType, object dataValue)
        {
            this.DataKey = dataKey;
            this.DataType = dataType;
            this.DataValue = dataValue;
        }
        public string DataKey { get; set; }
        public DataType DataType { get; set; }
        public object DataValue { get; set; }
    }
}

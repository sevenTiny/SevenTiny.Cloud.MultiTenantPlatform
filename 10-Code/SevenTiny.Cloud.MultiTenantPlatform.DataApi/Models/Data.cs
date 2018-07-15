namespace SevenTiny.Cloud.MultiTenantPlatform.DataApi.Models
{
    /// <summary>
    /// MetaData
    /// </summary>
    public class Data
    {
        public Data(string dataKey, DataType dataType, object dataValue)
        {
            this.DataKey = dataKey;
            this.DataType = (int)dataType;
            this.DataValue = dataValue;
        }
        public string DataKey { get; set; }
        public int DataType { get; set; }
        public object DataValue { get; set; }
    }
}

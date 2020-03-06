//using Newtonsoft.Json;
//using System;
//using System.Collections.Generic;
//using System.Linq;

//namespace SevenTiny.Cloud.MultiTenant.Domain.ValueObject
//{
//    /// <summary>
//    /// query object data
//    /// </summary>
//    public class ObjectData : CommonInfo
//    {
//        //public ObjectData(string metaObjectCode)
//        //{
//        //    if (string.IsNullOrEmpty(metaObjectCode))
//        //    {
//        //        throw new ArgumentNullException("metaObjectCode can not be null!");
//        //    }
//        //    var metaFields = GetMetaFieldsByMetaObjectCode(metaObjectCode);
//        //    List<Data> dataList = new List<Data>();
//        //    foreach (var item in metaFields)
//        //    {
//        //        dataList.Add(new Data(item.Code, EnumsTranslaterUseInProgram.ToDataType(item.FieldType), null));
//        //    }
//        //    this.DataArray = dataList.ToArray();
//        //}

//        //private List<MetaField> GetMetaFieldsByMetaObjectCode(string metaObjectCode)
//        //{
//        //    //tode:there should be cache!!!
//        //    using (var db = new MultiTenantPlatformDbContext())
//        //    {
//        //        var metaObject = db.QueryOne<MetaObject>(t => t.Code.Equals(metaObjectCode));
//        //        this.MetaObjectId = metaObject.Id;
//        //        this.MetaObjectCode = metaObject.Code;
//        //        this.MetaObjectName = metaObject.Name;
//        //        var metaFields = db.QueryList<MetaField>(t => t.MetaObjectId == metaObject.Id);
//        //        return metaFields;
//        //    }
//        //}


//        /// <summary>
//        /// tenant id,means the data belong to the tenant
//        /// </summary>
//        public int TenantId { get; set; } = -1;
//        /// <summary>
//        /// user id,means the data belong to the user
//        /// </summary>
//        public int UserId { get; set; } = -1;
//        /// <summary>
//        /// meta object data(key,type,value)
//        /// </summary>
//        public Data[] DataArray { get; set; }

//        public int MetaObjectId { get; set; }
//        public string MetaObjectCode { get; set; }
//        public string MetaObjectName { get; set; }
//        /// <summary>
//        /// get MetaObject by propertyName which same as DataKey
//        /// </summary>
//        /// <param name="fieldKey"></param>
//        /// <returns></returns>
//        public object this[string fieldKey]
//        {
//            get { return this.MetaFields.ContainsKey(fieldKey) ? this.MetaFields[fieldKey].DataValue : null; }
//            set
//            {
//                if (MetaFields.ContainsKey(fieldKey))
//                {
//                    DataType type = EnumsTranslaterUseInProgram.ToDataType(MetaFields[fieldKey].DataType);
//                    //check type
//                    switch (type)
//                    {
//                        case DataType.Unknown:
//                            break;
//                        case DataType.Number:
//                            if (!(value is int))
//                            {
//                                throw new ArgumentException($"value type mismatch.({value} is not '{nameof(DataType.Number)}')");
//                            }
//                            else
//                            {
//                                if (value is int intValue)
//                                {
//                                    if (intValue < 0)
//                                    {
//                                        throw new ArgumentException($"value type mismatch.({value} is not '{nameof(DataType.Number)}')");
//                                    }
//                                }
//                            }
//                            break;
//                        case DataType.Text:
//                            break;
//                        case DataType.DateTime:
//                            if (!(value is DateTime || value is DateTimeOffset))
//                            {
//                                throw new ArgumentException($"value type mismatch.({value} is not correct of '{nameof(DataType.DateTime)}')");
//                            }
//                            break;
//                        case DataType.Date:
//                            if (!(value is DateTime || value is DateTimeOffset))
//                            {
//                                throw new ArgumentException($"value type mismatch.({value} is not correct of '{nameof(DataType.Date)}')");
//                            }
//                            break;
//                        case DataType.Boolean:
//                            if (!(value is int || value is bool || value is string))
//                            {
//                                throw new ArgumentException($"value type mismatch.({value} is not correct of '{nameof(DataType.Boolean)}')");
//                            }
//                            else
//                            {
//                                if (value is int intValue)
//                                {
//                                    if (intValue == 0)
//                                    {
//                                        MetaFields[fieldKey].DataValue = false;
//                                    }
//                                    else if (intValue == 1)
//                                    {
//                                        MetaFields[fieldKey].DataValue = true;
//                                    }
//                                    else
//                                    {
//                                        throw new ArgumentException($"value type mismatch.({value} is not correct of '{nameof(DataType.Boolean)}')");
//                                    }
//                                }
//                                else if (value is string strValue)
//                                {
//                                    if (strValue.ToLower().Equals("true"))
//                                    {
//                                        MetaFields[fieldKey].DataValue = true;
//                                    }
//                                    else if (strValue.ToLower().Equals("false"))
//                                    {
//                                        MetaFields[fieldKey].DataValue = false;
//                                    }
//                                    else
//                                    {
//                                        throw new ArgumentException($"value type mismatch.({value} is not correct of '{nameof(DataType.Boolean)}')");
//                                    }
//                                }
//                            }
//                            break;
//                        case DataType.Int:
//                            if (!(value is int))
//                            {
//                                throw new ArgumentException($"value type mismatch.({value} is not '{nameof(DataType.Int)}')");
//                            }
//                            break;
//                        case DataType.Long:
//                            if (!(value is long))
//                            {
//                                throw new ArgumentException($"value type mismatch.({value} is not '{nameof(DataType.Long)}')");
//                            }
//                            break;
//                        case DataType.Double:
//                            if (!(value is double))
//                            {
//                                throw new ArgumentException($"value type mismatch.({value} is not '{nameof(DataType.Double)}')");
//                            }
//                            break;
//                        case DataType.DataSource:
//                            break;
//                        case DataType.StandradDate:
//                            if (!(value is DateTime || value is DateTimeOffset))
//                            {
//                                throw new ArgumentException($"value type mismatch.({value} is not correct of '{nameof(DataType.Date)}')");
//                            }
//                            break;
//                        case DataType.StandradDateTime:
//                            if (!(value is DateTime || value is DateTimeOffset))
//                            {
//                                throw new ArgumentException($"value type mismatch.({value} is not correct of '{nameof(DataType.Date)}')");
//                            }
//                            break;
//                        default:
//                            break;
//                    }
//                    MetaFields[fieldKey].DataValue = value;
//                }
//            }
//        }

//        private Dictionary<string, Data> _MetaFields { get; set; }
//        /// <summary>
//        /// MetaObjects Dictionary type
//        /// </summary>
//        [JsonIgnore]
//        public Dictionary<string, Data> MetaFields
//        {
//            get
//            {
//                if (this._MetaFields == null)
//                {
//                    this._MetaFields = this.DataArray?.ToDictionary(t => t.DataKey, v => v);
//                }
//                return this._MetaFields;
//            }
//        }

//        public int FieldsCount => DataArray.Count();
//    }
//}

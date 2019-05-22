using Microsoft.CodeAnalysis;
using MongoDB.Bson;
using MongoDB.Driver;
using Newtonsoft.Json;
using SevenTiny.Cloud.MultiTenantPlatform.Core.DataAccess;
using SevenTiny.Cloud.MultiTenantPlatform.Infrastructure.Logging;
using SevenTiny.Cloud.MultiTenantPlatform.UIModel.UIMetaData;
using System;
using System.Collections.Generic;
using System.IO;

namespace SevenTiny.Cloud.MultiTenantPlatform.TriggerScriptEngine.Base
{
    internal class ReferenceProvider
    {
        /// <summary>
        /// 引入公共的命名空间
        /// 这段代码在保存触发器脚本时自动添加到脚本头（最前面的引用中）
        /// </summary>
        internal static string GeneralUsing
            => @"
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SevenTiny.Cloud.MultiTenantPlatform.Core.DataAccess;
using SevenTiny.Cloud.MultiTenantPlatform.UIModel.UIMetaData;
using SevenTiny.Cloud.MultiTenantPlatform.UIModel.UIMetaData.ListView;
using SevenTiny.Cloud.MultiTenantPlatform.UIModel.UIMetaData.UserInfo;
using SevenTiny.Cloud.MultiTenantPlatform.Infrastructure.Logging;
using logger = SevenTiny.Cloud.MultiTenantPlatform.Infrastructure.Logging.Logger;
                ";
        /// <summary>
        /// 公共导入程序集,加触发器脚本的公共引用时，要在这里补充
        /// </summary>
        internal static string[] GeneralRefrences
            => new string[] {
                "System",
                "System.Text",
                "System.Linq",
                "System.Collections.Generic",
                "SevenTiny.Cloud.MultiTenantPlatform.Core",
                "Newtonsoft.Json",
                "SevenTiny.Cloud.MultiTenantPlatform.Infrastructure",
                "MongoDB.Bson",
                "MongoDB.Driver"
            };

        //返回常用的元数据引用
        internal static MetadataReference[] GetGeneralMetadataReferences()
        {
            List<MetadataReference> metadataReferences = new List<MetadataReference>();
            //从类型导入
            Type[] types = new[] {
                typeof(object),
                typeof(JsonConvert),//newtonsoft.json
                typeof(BsonDocument),//mongodb
                typeof(MongoDB.Driver.Collation),//mongodb
                typeof(FilterDefinition<BsonDocument>),//mongodb
                //business reference
                typeof(Logger),//infrastructure
                typeof(MultiTenantDataDbContext),//domain
                typeof(BaseComponent)//UIModel
            };
            foreach (var item in types)
            {
                try
                {
                    metadataReferences.Add(MetadataReference.CreateFromFile(item.Assembly.Location));
                }
                catch (Exception) { }
            }
            //从dll直接导入(这里都是.netstandard的系统dll)
            string[] Dlls = new[] {
                "netstandard.dll",
                "System.dll",
                "System.Runtime.dll",
                "System.Linq.dll",
                "System.Collections.dll"
            };
            string dllLocation = typeof(object).Assembly.Location;
            string dllPath = dllLocation.Substring(0, dllLocation.LastIndexOf("\\"));
            foreach (var item in Dlls)
            {
                try
                {
                    metadataReferences.Add(MetadataReference.CreateFromFile(Path.Combine(dllPath, item)));
                }
                catch (Exception) { }
            }

            return metadataReferences.ToArray();
        }
    }
}

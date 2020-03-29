using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace SevenTiny.Cloud.MultiTenant.Domain.ValueObject.UIMetaData.ListView
{
    public class ColumnCmpData
    {
        //
        // Summary:
        //     连接URL
        [JsonProperty("linkType", Order = 1, NullValueHandling = NullValueHandling.Ignore)]
        public string LinkType { get; set; }
        //
        // Summary:
        //     有权限显示
        [JsonProperty("canShow")]
        public bool CanShow { get; set; }
        //
        // Summary:
        //     是否是下载链接
        [JsonProperty("isDownload", Order = 1)]
        public bool IsDownload { get; set; }
        [JsonProperty("controlPack", Order = 1, NullValueHandling = NullValueHandling.Ignore)]
        public string ControlPack { get; set; }
        [JsonProperty("dataType", Order = 1, NullValueHandling = NullValueHandling.Ignore)]
        public int DataType { get; set; }
        [JsonProperty("metaFieldPrecision", Order = 1, NullValueHandling = NullValueHandling.Ignore)]
        public int MetaFieldPrecision { get; set; }
        [JsonProperty("precision", Order = 1, NullValueHandling = NullValueHandling.Ignore)]
        public int Precision { get; set; }
        [JsonProperty("formatType", Order = 1, NullValueHandling = NullValueHandling.Ignore)]
        public string FormatType { get; set; }
        [JsonProperty("sortField", Order = 1, NullValueHandling = NullValueHandling.Ignore)]
        public string SortField { get; set; }
        [JsonProperty("sortable", Order = 1)]
        public bool Sortable { get; set; }
        //
        // Summary:
        //     是否可以显示
        [JsonProperty("visible", Order = 1)]
        public bool Visible { get; set; }
        //
        // Summary:
        //     当前列在行的位置索引
        [JsonProperty("showIndex", Order = 1)]
        public int ShowIndex { get; set; }
        //
        // Summary:
        //     打开方式
        [JsonProperty("OpenMode", Order = 1, NullValueHandling = NullValueHandling.Ignore)]
        public string OpenMode { get; set; }
        //
        // Summary:
        //     是否是可树形展开列
        [JsonProperty("isTreeNodeColumn")]
        public bool IsTreeNodeColumn { get; set; }
        //
        // Summary:
        //     连接URL
        [JsonProperty("linkURL", Order = 1, NullValueHandling = NullValueHandling.Ignore)]
        public string LinkURL { get; set; }
        //
        // Summary:
        //     是否是冻结列
        [JsonProperty("frozen", Order = 1)]
        public bool IsFrozenColumn { get; set; }
        //
        // Summary:
        //     是否可以编辑
        [JsonProperty("editable", Order = 1)]
        public bool IsEditable { get; set; }
        //
        // Summary:
        //     是否需要渲染为链接
        [JsonProperty("isLink", Order = 1)]
        public bool IsLink { get; set; }
        //
        // Summary:
        //     前端类名
        [JsonProperty("className", Order = 1, NullValueHandling = NullValueHandling.Ignore)]
        public string CssClass { get; set; }
        //
        // Summary:
        //     名称
        [JsonProperty("name", Order = 1)]
        public string Name { get; set; }
        //
        // Summary:
        //     对齐方式
        [JsonProperty("alignment", Order = 1)]
        public string Alignment { get; set; }
        [JsonProperty("width", Order = 1)]
        public string Width { get; set; }
        //
        // Summary:
        //     标题
        [JsonProperty("title", Order = 1)]
        public string Title { get; set; }
        //
        // Summary:
        //     列表类型
        [JsonProperty("columnType", Order = 1, NullValueHandling = NullValueHandling.Ignore)]
        public string ColumnType { get; set; }
        //
        // Summary:
        //     类型
        [JsonProperty("type", Order = 1, NullValueHandling = NullValueHandling.Ignore)]
        public string Type { get; set; }
        //
        // Summary:
        //     有权限编辑
        [JsonProperty("canEdit")]
        public bool CanEdit { get; set; }
    }
}
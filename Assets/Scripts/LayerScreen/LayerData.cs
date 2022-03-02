/***********************************************
Copyright (C) 2018 The Company Name
File Name:           LayerData.cs
Author:              #AuthorName
CreateTime:          #CreateTime
User:                layer.json数据类
***********************************************/

using System.Collections.Generic;

/// <summary>
/// layer.json数据类
/// </summary>
public class LayerData
{

    /// <summary>
    /// 图层划分的种类(专业和非专业)
    /// </summary>
    public List<LayerTypeItem> LayerType { get; set; }

    /// <summary>
    /// 图层信息集合
    /// </summary>
    public List<LayerListItem> LayerList { get; set; }

    public class LayerTypeItem
    {
        /// <summary>
        /// 图层划分种类序号
        /// </summary>
        public int layerIndex { get; set; }
        /// <summary>
        /// 图层划分种类标识
        /// </summary>
        public string layerName { get; set; }
    }

    public class LayerListItem
    {
        /// <summary>
        /// 节点标识
        /// </summary>
        public int id { get; set; }
        /// <summary>
        /// 节点名称
        /// </summary>
        public string nodeName { get; set; }
        /// <summary>
        /// 节点类型(图层划分类型)
        /// </summary>
        public string nodeType { get; set; }
        /// <summary>
        /// 父节点标识
        /// </summary>
        public int parentId { get; set; }
        /// <summary>
        /// 所在父物体下序号
        /// </summary>
        public int index { get; set; }
        /// <summary>
        /// 详细信息
        /// </summary>
        public Dictionary<string, object> details { get; set; }
    }
}

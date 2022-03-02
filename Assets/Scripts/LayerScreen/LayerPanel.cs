/***********************************************
Copyright (C) 2018 The Company Name
File Name:           LayerPanel.cs
Author:              #AuthorName
CreateTime:          #CreateTime
User:                图层面板管理
***********************************************/

using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;

public class LayerPanel : MonoBehaviour
{
    public static LayerPanel Instance { get; private set;}

    public LayerPanelItem itemPerfab;

    /// <summary>
    /// 树形图根节点
    /// </summary>
    public Transform treeParent;

    private LayerData layerData = new LayerData();

    /// <summary>
    /// 所有的图层划分类型数组
    /// </summary>
    private string[] layerTypeArray;

    /// <summary>
    /// 统计的json节点信息
    /// </summary>
    private Dictionary<string, List<LayerData.LayerListItem>> layerDic = new Dictionary<string, List<LayerData.LayerListItem>>();

    /// <summary>
    /// 统计的场景内节点信息
    /// </summary>
    private Dictionary<string, List<LayerPanelItem>> itemDic = new Dictionary<string, List<LayerPanelItem>>();

    /// <summary>
    /// 树形图根节点
    /// </summary>
    private LayerPanelItem rootNode;

    /// <summary>
    /// 当前图层面板选中的筛选方式
    /// </summary>
    private string nowLayerType = "";

    private RectTransform contentRect;

    void Awake()
    {
        Instance = this;

        contentRect = treeParent.GetComponent<RectTransform>();
    }

    void Start()
    {
        GetLayerData();

        StatisticsAllTypeLayers();

        CreateLayerTreeItems();

        SetLayerTreeParent();

        ChangeLayerTree(0);
    }

    void FixedUpdate()
    {
        if (ProjectConfig.modelLoadEnded == true)
        {
            ProjectConfig.modelLoadEnded = false;

            ContentItemsGameObjects();
        }
    }

    /// <summary>
    /// 读取图层json数据
    /// </summary>
    void GetLayerData()
    {
        string json = "";
        using (FileStream fs = new FileStream(ProjectConfig.localPath + ProjectConfig.layerTxt, FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.ReadWrite))
        {
            using (StreamReader sr = new StreamReader(fs, Encoding.UTF8))
            {
                json = sr.ReadToEnd().ToString();
                layerData = JsonConvert.DeserializeObject<LayerData>(json);

                layerTypeArray = new string[layerData.LayerType.Count];
                for (int i = 0; i < layerData.LayerType.Count; i++)
                {
                    layerTypeArray[i] = layerData.LayerType[i].layerName;
                }

                nowLayerType = layerTypeArray[0];
            }
        }
    }

    /// <summary>
    /// 根据图层划分种类统计节点信息
    /// </summary>
    void StatisticsAllTypeLayers()
    {
        //统计字典的初始化
        for (int i = 0; i < layerTypeArray.Length; i++)
        {
            if (!layerDic.ContainsKey(layerTypeArray[i]))
            {
                layerDic.Add(layerTypeArray[i], new List<LayerData.LayerListItem>());
            }
        }

        //字典填充
        for (int i = 0; i < layerData.LayerList.Count; i++)
        {
            //根节点
            if (layerData.LayerList[i].nodeType == "root")
            {
                layerDic.Add("root", new List<LayerData.LayerListItem>() { layerData.LayerList[i] });
            }
            //普通节点
            else if (layerDic.ContainsKey(layerData.LayerList[i].nodeType))
            {
                layerDic[layerData.LayerList[i].nodeType].Add(layerData.LayerList[i]);
            }
        }
    }

    /// <summary>
    /// 实例化所有节点
    /// </summary>
    void CreateLayerTreeItems()
    {
        foreach (var item in layerDic)
        {
            itemDic.Add(item.Key, new List<LayerPanelItem>());

            if (item.Key == "root" && rootNode == null)
            {
                rootNode = treeParent.gameObject.AddComponent<LayerPanelItem>();

                rootNode.nodeData = item.Value[0];
            }
            else 
            {
                for (int i = 0; i < item.Value.Count; i++)
                {
                    LayerPanelItem node = Instantiate(itemPerfab).GetComponent<LayerPanelItem>();

                    node.InitParamaters(item.Value[i]);

                    itemDic[item.Key].Add(node);
                }
            }
        }
    }

    /// <summary>
    /// 对所有节点的父子关系进行整理
    /// </summary>
    void SetLayerTreeParent()
    {
        foreach (var item in itemDic)
        {
            if (item.Key == "root") continue;

            List<LayerPanelItem> nodeList = item.Value;

            for (int i = 0; i < nodeList.Count; i++)
            {
                int parentID = nodeList[i].nodeData.parentId;

                if (parentID == -1)
                {
                    nodeList[i].transform.SetParent(rootNode.transform);

                    nodeList[i].layerIndex = 0;
                }
                else
                {
                    var parentNode = nodeList.Find(s => s.nodeData.id == parentID);

                    if (parentNode != null)
                    {
                        parentNode.SetChildren(nodeList[i]);

                        nodeList[i].layerIndex = parentNode.layerIndex + 1;
                    }
                }
            }
        }
    }

    /// <summary>
    /// 更改图层面板的筛选类型
    /// </summary>
    /// <param name="layerTypeIndex"></param>
    public void ChangeLayerTree(int layerTypeIndex)
    {
        nowLayerType = layerTypeArray[layerTypeIndex];

        foreach (var item in itemDic)
        {
            for (int i = 0; i < item.Value.Count; i++)
            {
                item.Value[i].gameObject.SetActive(item.Key == nowLayerType);
            }
        }
    }

    public void ChangeContentSize(float changeHeight)
    {
        contentRect.sizeDelta = new Vector2(contentRect.sizeDelta.x, contentRect.sizeDelta.y + changeHeight);
    }

    public void ContentItemsGameObjects()
    {
        foreach (var item in itemDic)
        {
            for (int i = 0; i < item.Value.Count; i++)
            {
                item.Value[i].GetChildrenGameObject();

                Debug.Log("节点: " + item.Value[i].nodeData.nodeName + "开始关联模型");
            }
        }
    }
}

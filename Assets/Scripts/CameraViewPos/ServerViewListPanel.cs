/***********************************************
Copyright (C) 2018 The Company Name
File Name:           ServerViewListPanel.cs
Author:              #AuthorName
CreateTime:          #CreateTime
User:                自定义视角列表(保存在服务器上)
***********************************************/

using Newtonsoft.Json;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ServerViewListPanel : MonoBehaviour
{
    public GameObject serverItemPerfab;

    public RectTransform content;

    private List<ServerViewListItem> itemList = new List<ServerViewListItem>();

    private List<ServerViewListItem> hideItemList = new List<ServerViewListItem>();

    private MaintenancePageListData serverData;

    private float originalHeight, itemHeight, spacing, nowcontentHeight;

    void Awake()
    {
        originalHeight = content.rect.height;

        itemHeight = serverItemPerfab.GetComponent<RectTransform>().rect.height;

        spacing = content.GetComponent<VerticalLayoutGroup>().spacing;
    }

    void OnEnable()
    {
        GetServerViewListData();
    }

    /// <summary>
    /// 获取服务器数据
    /// </summary>
    private void GetServerViewListData()
    {
        string URL = string.Format(ProjectConfig.maintenanceListUrl, "1", "8");

        Dictionary<string, string> data = new Dictionary<string, string>();
        data.Add("loginToken", ProjectConfig.LoginToken);
        data.Add("apiUrl", "api_viewPoint");

        Debug.Log("获取自定义视角，url:" + URL);

        StartCoroutine(NetWorkManager.InternalGet(URL, data, RefreshViewList));
    }

    /// <summary>
    /// 刷新列表
    /// </summary>
    private void RefreshViewList(string receiveStr)
    {
        serverData = JsonConvert.DeserializeObject<MaintenancePageListData>(receiveStr);

        if (serverData.data.resultList.Count != itemList.Count)
        {
            if (serverData.data.resultList.Count > itemList.Count)
            {
                for (int i = itemList.Count; i < serverData.data.resultList.Count; i++)
                {
                    CreateItem();
                }
            }
            else
            {
                for (int i = itemList.Count - 1; i >= serverData.data.resultList.Count; i--)
                {
                    HideItem(i);
                }
            }
        }

        for (int i = 0; i < itemList.Count; i++)
        {
            itemList[i].gameObject.SetActive(true);

            itemList[i].InitParamaters(serverData.data.resultList[i]);
        }

        ResizeContentSize();
    }

    /// <summary>
    /// 补充元素
    /// </summary>
    private void CreateItem()
    {
        if (hideItemList.Count != 0)
        {
            ServerViewListItem item = hideItemList[0];
            itemList.Add(item);
            hideItemList.RemoveAt(0);
        }
        else
        {
            ServerViewListItem item = Instantiate(serverItemPerfab, content).GetComponent<ServerViewListItem>();
            itemList.Add(item);
        }
    }

    /// <summary>
    /// 删除多余元素
    /// </summary>
    /// <param name="index"></param>
    private void HideItem(int index)
    {
        hideItemList.Add(itemList[index]);
        itemList[index].gameObject.SetActive(false);
        itemList.RemoveAt(index);
    }

    /// <summary>
    /// 调整滑动框大小
    /// </summary>
    private void ResizeContentSize()
    {
        nowcontentHeight = (itemHeight + spacing) * itemList.Count - spacing;

        content.sizeDelta = new Vector2(content.sizeDelta.x, nowcontentHeight > originalHeight ? nowcontentHeight : originalHeight);
    }

    void OnDestory()
    {
        hideItemList.Clear();
    }
}

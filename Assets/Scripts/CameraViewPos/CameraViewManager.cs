/***********************************************
Copyright (C) 2018 The Company Name
File Name:           CameraViewListPanel.cs
Author:              #AuthorName
CreateTime:          #CreateTime
User:                视角列表管理
***********************************************/

using UnityEngine;

public class CameraViewManager : MonoBehaviour
{
    internal static CameraViewManager instance;

    public LocalViewListPanel localViewPanel;

    public ServerViewListPanel serverViewPanel;

    public CamerViewMsgPanel msgPanel;

    void Awake()
    {
        if (instance == null) instance = this;

        ProjectConfig.sunTrans = FindObjectOfType<Light>().transform;
    }

    /// <summary>
    /// 展开自定义视角的详细信息
    /// </summary>
    /// <param name="serverViewItem"></param>
    public void OpenServerViewMsgPanel(ServerViewListItem serverViewItem)
    {
        msgPanel.gameObject.SetActive(true);

        msgPanel.InitViewMsg(serverViewItem.viewData);
    }
}



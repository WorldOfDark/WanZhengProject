/***********************************************
Copyright (C) 2018 The Company Name
File Name:           CamerViewMsgPanel.cs
Author:              #AuthorName
CreateTime:          #CreateTime
User:                视角节点详细信息展示面板
***********************************************/

using System;
using UnityEngine;
using UnityEngine.UI;

public class CamerViewMsgPanel : MonoBehaviour
{
    public Text personName, viewName, viewTip, viewType;

    public Image viewScreen;

    private MaintenancePageListData.BeforeImgListItem BeforeImg;

    public void InitViewMsg(MaintenancePageListData.ResultListItem result)
    {
        viewName.text = result.problemName;

        viewTip.text = result.desc;

        viewType.text = result.problemType;

        personName.text = result.informUserName;

        BeforeImg = result.beforeImgList[0];

        GetViewScreenImage();
    }

    private void GetViewScreenImage()
    {
        string url = string.Format("http://192.168.1.211:8038/download/{0}", BeforeImg.filePath);

        StartCoroutine(NetWorkManager.InternalGet(url, (byte[] bytes) => { viewScreen.sprite = Util.ByteToSprite(bytes); }));
    }
}

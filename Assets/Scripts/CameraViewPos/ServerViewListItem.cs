/***********************************************
Copyright (C) 2018 The Company Name
File Name:           CameraViewListItem.cs
Author:              #AuthorName
CreateTime:          #CreateTime
User:                服务器视角看板列表元素
***********************************************/

using UnityEngine;
using UnityEngine.UI;
using static MaintenancePageListData;

public class ServerViewListItem : MonoBehaviour
{
    public ResultListItem viewData;

    public Button button;

    public Text nameText;

    public void InitParamaters(ResultListItem dataInfo)
    {
        viewData = dataInfo;

        nameText.text = dataInfo.problemName;

        button.onClick.AddListener(() => { CameraViewManager.instance.OpenServerViewMsgPanel(this); });
    }
}

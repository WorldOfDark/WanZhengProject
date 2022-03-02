/***********************************************
Copyright (C) 2018 The Company Name
File Name:           LocalViewListPanel.cs
Author:              #AuthorName
CreateTime:          #CreateTime
User:                本地视角列表
***********************************************/

using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LocalViewListPanel : MonoBehaviour
{
    public GameObject localItemPerfab;

    public RectTransform content;

    public List<ViewAndLight> localViews = new List<ViewAndLight>();

    private float originalHeight, itemHeight, spacing;

    void Awake()
    {
        originalHeight = content.rect.height;

        itemHeight = localItemPerfab.GetComponent<RectTransform>().rect.height;

        spacing = content.GetComponent<VerticalLayoutGroup>().spacing;
    }

    void Start()
    {
        for (int i = 0; i < localViews.Count; i++)
        {
            InitItem(localViews[i]);
        }

        content.sizeDelta = new Vector2(content.sizeDelta.x, (itemHeight + spacing) * localViews.Count - spacing > originalHeight ? (itemHeight + spacing) * localViews.Count - spacing : originalHeight);
    }

    void InitItem(ViewAndLight viewItem)
    {
        GameObject item = Instantiate(localItemPerfab, content);

        item.GetComponent<Text>().text = viewItem.viewTrans.name;

        Button itemBtn = item.GetComponent<Button>();
        itemBtn.onClick.AddListener(() =>
        {
            CameraMoveLogic.MoveCamera(viewItem.viewTrans, new Vector3(viewItem.lightAngleX, viewItem.lightAngleY, 0));
        });
    }
}

[System.Serializable]
public class ViewAndLight
{
    public Transform viewTrans;

    public float lightAngleX;

    public float lightAngleY;
}

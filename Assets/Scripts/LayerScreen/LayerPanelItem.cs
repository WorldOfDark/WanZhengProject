/***********************************************
Copyright (C) 2018 The Company Name
File Name:           LayerPanelItem.cs
Author:              #AuthorName
CreateTime:          #CreateTime
User:                
***********************************************/

using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LayerPanelItem : MonoBehaviour
{
    public Toggle dropdownToggle, activeToggle;

    public Text nameText;

    public Transform childrenParent;

    public LayerData.LayerListItem nodeData;

    public int layerIndex = -1;

    private RectTransform selfRect;

    private float width, height;

    private float childrenReduceWidth = 20.0f;

    private List<LayerPanelItem> childrenList = new List<LayerPanelItem>();

    private List<string> bundleNames = new List<string>();

    public List<GameObject> releateObjs = new List<GameObject>();

    void Awake()
    {
        selfRect = GetComponent<RectTransform>();
    }

    void OnEnable()
    {
        if (layerIndex > 0)
        {
            dropdownToggle.gameObject.SetActive(childrenList.Count != 0);
        }
    }

    public void InitParamaters(LayerData.LayerListItem item)
    {
        width = transform.GetComponent<RectTransform>().rect.width;

        height = transform.GetComponent<RectTransform>().rect.height;

        nodeData = item;

        nameText.text = item.nodeName;

        GetBundelList();
    }

    public void SetChildren(LayerPanelItem children)
    {
        childrenList.Add(children);

        children.transform.SetParent(childrenParent);

        children.GetComponent<RectTransform>().sizeDelta = new Vector2(width - childrenReduceWidth, height);

        childrenParent.GetComponent<RectTransform>().sizeDelta = new Vector2(width,height * childrenList.Count);
    }

    public void OpenAndHideChidlrens(bool value)
    {
        childrenParent.gameObject.SetActive(value);

        if (value)
        {
            selfRect.sizeDelta = new Vector2(width, (childrenList.Count + 1) * height + 5);

            LayerPanel.Instance.ChangeContentSize(childrenList.Count * height + 5);
        }
        else
        {
            selfRect.sizeDelta = new Vector2(width, height);

            LayerPanel.Instance.ChangeContentSize((childrenList.Count * height + 5) * -1);
        }
    }

    public void ChangeChildrenActive(bool value)
    {
        for (int i = 0; i < releateObjs.Count; i++)
        {
            releateObjs[i].SetActive(value);
        }
    }

    public void GetChildrenGameObject()
    {
        var selfObjs = AssetBundleManager.instance.GetReleatedObjs(bundleNames);

        if (selfObjs.Count != 0)
        {
            for (int j = 0; j < selfObjs.Count; j++)
            {
                if (!releateObjs.Contains(selfObjs[j]))
                {
                    releateObjs.Add(selfObjs[j]);
                }
            }
        }
        

        for (int i = 0; i < childrenList.Count; i++)
        {
            var objs = AssetBundleManager.instance.GetReleatedObjs(childrenList[i].bundleNames);

            if (objs.Count == 0) continue;

            for (int j = 0; j < objs.Count; j++)
            {
                releateObjs.Add(objs[j]);
            }
        }
    }

    /// <summary>
    /// 获取节点内关联的AB包名称集合
    /// </summary>
    /// <param name="node"></param>
    /// <returns></returns>
    public void GetBundelList()
    {
        if (nodeData.details != null)
        {
            object data = GetDetailsValueData(nodeData.details, "assetsBundleNames");
            if (data != null)
            {
                Newtonsoft.Json.Linq.JArray jsonData = data as Newtonsoft.Json.Linq.JArray;
                bundleNames = jsonData.ToObject<List<string>>();
            }
        }
    }

    public static object GetDetailsValueData(Dictionary<string, object> details, string key)
    {
        if (details.ContainsKey(key))
        {
            return details[key];
        }
        else
        {
            return null;
        }
    }
}

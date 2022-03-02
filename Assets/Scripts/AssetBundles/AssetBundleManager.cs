/***********************************************
Copyright (C) 2018 The Company Name
File Name:           AssetBundleManager.cs
Author:              #AuthorName
CreateTime:          #CreateTime
User:                AB包资源更新的整流程控制
***********************************************/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AssetBundleManager : MonoBehaviour
{
    public static AssetBundleManager instance;

    private static AssetBundle manifesAB;

    private static AssetBundleManifest manifest;

    /// <summary>
    /// 资源加载队列
    /// </summary>
    private Queue<IEnumerator> loadQuence = new Queue<IEnumerator>();

    /// <summary>
    /// ab包名称+实体对象
    /// </summary>
    private Dictionary<string, List<GameObject>> bundleObjDic = new Dictionary<string, List<GameObject>>();

    void Awake()
    {
        instance = this;
    }

    void Start()
    {
        UpdateAssetBundles.Instance.StartUpdate();
    }

    void FixedUpdate()
    {
        if (ProjectConfig.updateProgress == 1.0f)
        {
            ProjectConfig.updateProgress = 0.0f;

            LoadAllAssetBundles(transform);

            StartCoroutine(WaitForLoadEnded());
        }
    }

    #region 资源加载
    private void LoadAllAssetBundles(Transform root)
    {
        if (manifesAB == null)
        {
            manifesAB = AssetBundle.LoadFromFile(ProjectConfig.localPath + ProjectConfig.abMainfest);
            if (manifest == null)
            {
                manifest = manifesAB.LoadAsset<AssetBundleManifest>("AssetBundleManifest");
            }
        }

        if (manifest != null)
        {
            string[] abArray = manifest.GetAllAssetBundles();

            Debug.Log("开始进行资源加载");

            for (int i = 0; i < abArray.Length; i++)
            {
                loadQuence.Enqueue(LoadAssetBundle(abArray[i], transform));
            }
        }
    }

    IEnumerator LoadAssetBundle(string abName,Transform root)
    {
        AssetBundleCreateRequest ab = AssetBundle.LoadFromFileAsync(ProjectConfig.localPath + abName);

        yield return ab;

        GameObject[] a = ab.assetBundle.LoadAllAssets<GameObject>();

        List<GameObject> list = new List<GameObject>();

        foreach (GameObject go in a)
        {
            if (go != null)
            {
                GameObject NObj = GameObject.Instantiate(go, root);
                if (NObj.name.Contains("(Clone)"))
                {
                    NObj.name = NObj.name.Replace("(Clone)", "");
                    list.Add(NObj);
                }
            }
        }

        bundleObjDic.Add(abName, list);
    }

    IEnumerator WaitForLoadEnded()
    {
        while (loadQuence.Count > 0)
        {
            yield return loadQuence.Dequeue();
        }

        ProjectConfig.modelLoadEnded = true;

        Debug.Log("资源加载完成");
    }
    #endregion

    #region 外部调用
    /// <summary>
    /// 图层面板元素获取自身相关的场景对象
    /// </summary>
    /// <param name="bundleNames">场景对象对应的AB包名称</param>
    /// <returns></returns>
    public List<GameObject> GetReleatedObjs(List<string> bundleName)
    {
        List<GameObject> objs = new List<GameObject>();

        for (int i = 0; i < bundleName.Count; i++)
        {
            if (!bundleObjDic.ContainsKey(bundleName[i]))
            {
                continue; 
            }

            for (int j = 0; j < bundleObjDic[bundleName[i]].Count; j++)
            {
                if (objs.Contains(bundleObjDic[bundleName[i]][j]))
                {
                    continue;
                }

                objs.Add(bundleObjDic[bundleName[i]][j]);
            }
        }

        return objs;
    }
    #endregion
}

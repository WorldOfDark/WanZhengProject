/***********************************************
Copyright (C) 2018 The Company Name
File Name:           UpdateAssetBundles.cs
Author:              #AuthorName
CreateTime:          #CreateTime
User:                本地资源更新
***********************************************/

using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

public class UpdateAssetBundles : MonoBehaviour
{
    public static UpdateAssetBundles Instance;

    private List<string> updateFileList = new List<string>();

    private Dictionary<string, string> serverVersionDic = new Dictionary<string, string>();

    private Dictionary<string, string> localVersionDic = new Dictionary<string, string>();

    private Queue<IEnumerator> updateQuences = new Queue<IEnumerator>();

    private bool serverVersionDownloaded = false;

    private bool localVersionDownloaded = false;

    #region 更新面板
    public Text abCount, abNowIndex, abNowTotalSize, abNowSize;
    public Slider slider;
    #endregion

    void Awake()
    {
        Instance = this;
    }

    void FixedUpdate()
    {
        slider.value = ProjectConfig.nowDownloadProgress;

        if (serverVersionDownloaded && localVersionDownloaded)
        {
            serverVersionDownloaded = false;

            localVersionDownloaded = false;

            GetUpdateFileList();

            FillUpdateQuence();

            if (updateFileList.Count > 0)
            {
                StartCoroutine(WaitForUpdateEnd());
            }
            else
            {
                Debug.Log("没有文件需要更新");

                ProjectConfig.updateProgress = 1.0f;
            }
        }
    }

    public void StartUpdate()
    {
        GetServerVersionTxt();

        GetLocalVersionTxt();
    }

    /// <summary>
    /// 获取所有需要更新的文件名称
    /// </summary>
    private void GetUpdateFileList()
    {
        Debug.Log("开始整理需要更新的模型文件");

        foreach (var item in serverVersionDic)
        {
            if (localVersionDic.ContainsKey(item.Key) && localVersionDic[item.Key] == item.Value)
            {
                continue;
            }

            if (localVersionDic.ContainsKey(item.Key))
            {
                DeletOldFile(item.Key);
            }

            updateFileList.Add(item.Key);
            Debug.Log(item.Key + "文件需要更新");
        }

        if (updateFileList.Count == 0.0f)
        {
            ProjectConfig.updateProgress = 1.0f;
        }
        else
        {
            ProjectConfig.updateProgress = 0.0f;
        }
    }

    /// <summary>
    /// 填充更新队列
    /// </summary>
    private void FillUpdateQuence()
    {
        abCount.text = updateFileList.Count.ToString();

        abNowIndex.text = "0";

        abNowSize.text = "0.00MB";

        abNowTotalSize.text = "0.00MB";

        for (int i = 0; i < updateFileList.Count; i++)
        {
            int index = i + 1;
            string fileName_update = updateFileList[i];
            updateQuences.Enqueue(NetWorkManager.InternalGet(ProjectConfig.serverPath, index, fileName_update, (string fileName, byte[] bytes) =>
            {
                SaveAssetBundles.SaveFile(ProjectConfig.localPath, fileName, bytes);
            }));
        }

        updateQuences.Enqueue(NetWorkManager.InternalGet(ProjectConfig.serverPath, -1 ,ProjectConfig.versionTxt, (string fileName, byte[] bytes) =>
        {
            SaveAssetBundles.SaveFile(ProjectConfig.localPath, ProjectConfig.versionTxt, bytes);
        }));
    }

    /// <summary>
    /// 获取服务器版本文件
    /// </summary>
    private void GetServerVersionTxt()
    {
        Debug.Log("开始获取服务器版本文件,地址:" + ProjectConfig.serverPath + ProjectConfig.versionTxt);

        StartCoroutine(NetWorkManager.InternalGet(ProjectConfig.serverPath, ProjectConfig.versionTxt, (string str) =>
        {
            serverVersionDownloaded = true;
            Debug.Log("获取远端版本文件失败: " + str);
        }, (byte[] bytes) =>
        {
            if (bytes == null || bytes.Length == 0)
            {
                localVersionDic.Clear();
            }
            else
            {
                string receiveStr = Encoding.UTF8.GetString(bytes);
                string[] receiveArray = receiveStr.Split('\n');
                for (int i = 0; i < receiveArray.Length - 1; i++)
                {
                    serverVersionDic.Add(receiveArray[i].Split(',')[0], receiveArray[i].Split(',')[1]);

                    ProjectConfig.assetBundleList.Add(receiveArray[i].Split(',')[0]);
                }
            }

            serverVersionDownloaded = true;

            Debug.Log("已获取服务器版本文件");
        }));
    }

    /// <summary>
    /// 获取本地版本文件
    /// </summary>
    private void GetLocalVersionTxt()
    {
        Debug.Log("开始获取本地版本文件,地址：" + ProjectConfig.localPath + ProjectConfig.versionTxt);

        StartCoroutine(NetWorkManager.InternalGet(ProjectConfig.localPath, ProjectConfig.versionTxt, (string str) =>
        {
            localVersionDownloaded = true;
            Debug.Log("获取本地版本文件失败: " + str);
        }, (byte[] bytes) =>
        {
            if (bytes == null || bytes.Length == 0)
            {
                localVersionDic.Clear();
            }
            else
            {
                string receiveStr = Encoding.UTF8.GetString(bytes);
                string[] receiveArray = receiveStr.Split('\n');
                for (int i = 0; i < receiveArray.Length - 1; i++)
                {
                    localVersionDic.Add(receiveArray[i].Split(',')[0], receiveArray[i].Split(',')[1]);
                }
            }

            localVersionDownloaded = true;

            Debug.Log("已获取本地版本文件");
        }));
    }

    /// <summary>
    /// 删除旧版本文件
    /// </summary>
    /// <param name="fileName"></param>
    private void DeletOldFile(string fileName)
    {
        if (File.Exists(ProjectConfig.localPath + fileName))
        {
            File.Delete(ProjectConfig.localPath + fileName);
        }
    }

    IEnumerator WaitForUpdateEnd()
    {
        Debug.Log("开始更新文件");

        while (updateQuences.Count > 0)
        {
            yield return updateQuences.Dequeue();
        }

        Debug.Log("资源更新完成");

        //ProjectConfig.updateProgress = 1;

        updateQuences.Clear();
        updateFileList.Clear();
    }
}

/***********************************************
Copyright (C) 2018 The Company Name
File Name:           DownloadAssetBundles.cs
Author:              #AuthorName
CreateTime:          #CreateTime
User:                网络请求相关
***********************************************/

using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;

public class NetWorkManager
{
    /// <summary>
    /// 下载文件（AB包文件更新）
    /// </summary>
    /// <param name="url">文件地址</param>
    /// <param name="EndAction">下载后的事件调用</param>
    /// <returns></returns>
    public static IEnumerator InternalGet(string url, int fileIndex,string fileName, Action<string, byte[]> EndAction)
    {
        using (UnityWebRequest request = UnityWebRequest.Get(url + fileName))
        {
            ProjectConfig.nowDownloadIndex = fileIndex;

            yield return request.SendWebRequest();

            while (!request.isDone)
            {
                ProjectConfig.nowDownloadProgress = request.downloadProgress;
            }

            if (request.isHttpError)
            {
                Debug.LogError(request.error);
            }
            else
            {
                EndAction?.Invoke(fileName, request.downloadHandler.data);
            }
        }
    }

    /// <summary>
    /// 下载文件
    /// </summary>
    /// <param name="url">服务器地址</param>
    /// <param name="fileName">文件名称</param>
    /// <param name="fallCallback">失败回调</param>
    /// <param name="successCallback">成功回调</param>
    /// <returns></returns>
    public static IEnumerator InternalGet(string url, string fileName, Action<string> fallCallback, Action<byte[]> successCallback)
    {
        using (UnityWebRequest request = UnityWebRequest.Get(url + fileName))
        {
            yield return request.SendWebRequest();
            if (request.isHttpError)
            {
                fallCallback?.Invoke(request.error);
            }
            else
            {
                successCallback?.Invoke(request.downloadHandler.data);
            }
        }
    }

    /// <summary>
    /// POST请求
    /// </summary>
    /// <param name="url">目标地址</param>
    /// <param name="parameters">参数字典</param>
    /// <param name="contentType">连接类型</param>
    /// <param name="action">连接回调</param>
    /// <returns></returns>
    public static IEnumerator InternalPost(string url, IDictionary<string, string> parameters, string contentType, Action<string> action)
    {
        bool first = true;
        StringBuilder buffer = new StringBuilder();
        foreach (string key in parameters.Keys)
        {

            if (!first)
            {
                buffer.AppendFormat("&{0}={1}", key, parameters[key]);
            }
            else
            {
                buffer.AppendFormat("{0}={1}", key, parameters[key]);
                first = false;
            }
        }

        string postData = buffer.ToString();

        using (UnityWebRequest request = UnityWebRequest.Post(url, postData))
        {
            byte[] postBytes = Encoding.UTF8.GetBytes(postData);
            request.uploadHandler = new UploadHandlerRaw(postBytes);
            request.downloadHandler = new DownloadHandlerBuffer();
            request.SetRequestHeader("Content-Type", contentType);

            yield return request.SendWebRequest();

            if (request.isNetworkError)
            {
                Debug.Log(request.error);
            }
            else
            {
                action?.Invoke(request.downloadHandler.text);
            }
        }
    }

    /// <summary>
    /// POST请求
    /// </summary>
    /// <param name="url">目标地址</param>
    /// <param name="parameters">参数字典</param>
    /// <param name="contentType">连接类型</param>
    /// <param name="action">连接回调</param>
    /// <returns></returns>
    public static IEnumerator InternalPost(string url, Dictionary<string, string> headerDic, byte[] parameters, Action<string> action)
    {
        using (UnityWebRequest request = UnityWebRequest.Post(url, parameters.ToString()))
        {
            request.uploadHandler = new UploadHandlerRaw(parameters);
            request.downloadHandler = new DownloadHandlerBuffer();

            foreach (var item in headerDic)
            {
                request.SetRequestHeader(item.Key, item.Value);
            }

            yield return request.SendWebRequest();

            if (request.isNetworkError)
            {
                Debug.Log(request.error);
            }
            else
            {
                action?.Invoke(request.downloadHandler.text);
            }
        }
    }

    /// <summary>
    /// Get请求
    /// </summary>
    /// <param name="url">目标地址</param>
    /// <param name="action">连接回调</param>
    /// <returns></returns>
    public static IEnumerator InternalGet(string url, Action<string> action)
    {
        using (UnityWebRequest request = UnityWebRequest.Get(url))
        {
            yield return request.SendWebRequest();
            if (request.isNetworkError || request.isHttpError)
            {
                Debug.Log("网络地址请求失败,网址:" + url);
            }
            else
            {
                action?.Invoke(request.downloadHandler.text);
            }
        }
    }

    /// <summary>
    /// Get请求
    /// </summary>
    /// <param name="url">目标地址</param>
    /// <param name="action">连接回调</param>
    /// <returns></returns>
    public static IEnumerator InternalGet(string url, Action<byte[]> action)
    {
        using (UnityWebRequest request = UnityWebRequest.Get(url))
        {
            yield return request.SendWebRequest();
            if (request.isNetworkError || request.isHttpError)
            {
                Debug.Log("网络地址请求失败,网址:" + url);
            }
            else
            {
                action?.Invoke(request.downloadHandler.data);
            }
        }
    }

    /// <summary>
    /// Get请求
    /// </summary>
    /// <param name="url">目标地址</param>
    /// <param name="action">连接回调</param>
    /// <returns></returns>
    public static IEnumerator InternalGet(string url, Dictionary<string, string> data, Action<string> action)
    {
        using (UnityWebRequest request = UnityWebRequest.Get(url))
        {
            foreach (var item in data)
            {
                request.SetRequestHeader(item.Key, item.Value);
            }

            yield return request.SendWebRequest();
            if (request.isNetworkError || request.isHttpError)
            {
                Debug.Log("网络地址请求失败,网址:" + url);
            }
            else
            {
                action?.Invoke(request.downloadHandler.text);
            }
        }
    }
}

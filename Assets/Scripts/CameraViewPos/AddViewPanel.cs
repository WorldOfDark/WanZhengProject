/***********************************************
Copyright (C) 2018 The Company Name
File Name:           AddViewPanel.cs
Author:              #AuthorName
CreateTime:          #CreateTime
User:                视角添加视图
***********************************************/

using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Newtonsoft.Json;
using System.Security.Cryptography;
using System.Text;
using System;

public class AddViewPanel : MonoBehaviour
{
    public InputField viewName, viewTip;

    public Dropdown viewPersonal, viewType;

    public RawImage viewScreen;

    public Button submitBtn, editorImageBtn, editorEndBtn;

    private List<string> typeList = new List<string>() { "修改", "协同", "维修", "置换", "保养" };

    private List<string> personList = new List<string>();

    private MaintenancePersonData personData;

    private WWWForm form;

    void Awake()
    {
        FillPersonDropDown();
    }

    void Start()
    {
        form = new WWWForm();

        submitBtn.onClick.AddListener(() => { UpLoadCustomView(); });

        editorImageBtn.onClick.AddListener(() => { EditorScreen(); });

        editorEndBtn.onClick.AddListener(() => { EditorScreenEnd(); });
    }

    void OnEnable()
    {
        viewScreen.texture = Util.CaptureCamera(Camera.main, new Rect(0, 0, Screen.width, Screen.height));
    }

    public void InitParamaters(MaintenancePageListData.ResultListItem result = null,Texture sprite = null)
    {
        if (result == null) return;

        viewName.text = result.problemName;

        viewTip.text = result.desc;

        viewType.value = GetTypeValue(result.problemType);

        viewPersonal.value = GetTypeValue(result.informUserName);

        viewScreen.texture = sprite;
    }

    private void FillPersonDropDown()
    {
        Dictionary<string, string> data = new Dictionary<string, string>();
        data.Add("Content-Type", "application/json");
        data.Add("loginToken", ProjectConfig.LoginToken);
        data.Add("apiUrl", "api_viewPoint");
        data.Add("source", "unity");

        StartCoroutine(NetWorkManager.InternalGet(ProjectConfig.personListUrl, data, (string receiveStr) => {
            personData = JsonConvert.DeserializeObject<MaintenancePersonData>(receiveStr);

            List<string> staffsList = new List<string>();
            for (int i = 0; i < personData.data.Count; i++)
            {
                if (!string.IsNullOrEmpty(personData.data[i].name))
                {
                    personList.Add(personData.data[i].name);
                }
            }

            viewPersonal.ClearOptions();

            viewPersonal.AddOptions(personList);
        }));
    }

    private int GetTypeValue(string typeStr)
    {
        var result = typeList.Find(s => s == typeStr);
        return result == null ? 0 : typeList.IndexOf(result);
    }

    #region 图片上传
    private void UpLoadCustomView()
    {
        string locationName = viewName.text;

        string presionID = GetPersonID(viewPersonal.captionText.text);

        string node = viewTip.text;

        byte[] picture = Util.TextureToByte(viewScreen.texture);

        string problemType = viewType.captionText.text;

        string locationInfo = GetLocationInfo(locationName, Camera.main.transform.position, Camera.main.transform.eulerAngles);

        FillUpLoadMsg(locationInfo, locationName, node, picture, problemType, presionID);

        CheckFileMd5(picture);
    }

    private string GetLocationInfo(string posName, Vector3 pos, Vector3 angle)
    {
        ViewPosListDataInfo cameraPosStruct = new ViewPosListDataInfo();
        cameraPosStruct.posName = posName;
        cameraPosStruct.pos = new ViewPosListDataInfo.Pos();
        cameraPosStruct.pos.x = pos.x;
        cameraPosStruct.pos.y = pos.y;
        cameraPosStruct.pos.z = pos.z;
        cameraPosStruct.angle = new ViewPosListDataInfo.Angle();
        cameraPosStruct.angle.x = angle.x;
        cameraPosStruct.angle.y = angle.y;
        cameraPosStruct.angle.z = angle.z;

        cameraPosStruct.light_AngleX = ProjectConfig.sunTrans.eulerAngles.x;
        cameraPosStruct.light_AngleY = ProjectConfig.sunTrans.eulerAngles.y;

        JsonSerializerSettings jsonSerializerSettings = new JsonSerializerSettings();
        return JsonConvert.SerializeObject(cameraPosStruct, null, jsonSerializerSettings);
    }

    /// <summary>
    /// 图片上传准备
    /// </summary>
    /// <param name="viewpointInfo"></param>
    /// <param name="locationName"></param>
    /// <param name="note"></param>
    /// <param name="picture"></param>
    /// <param name="problemType"></param>
    /// <param name="persionID"></param>
    private void FillUpLoadMsg(string viewpointInfo, string locationName, string note, byte[] picture, string problemType, string persionID)
    {
        if (form.data.Length > 0)
        {
            form = null;
            form = new WWWForm();
        }

        //将信息转成 byte[] 再存储进form中
        form.AddField("content", viewpointInfo);
        form.AddField("problemName", locationName);
        form.AddField("desc", note);
        form.AddField("problemType", problemType);
        form.AddField("projectUuid", ProjectConfig.projectID);
        form.AddField("informUserUuid", persionID);

        form.AddBinaryData("beforeImg", picture);
    }

    /// <summary>
    /// 文件校验
    /// </summary>
    /// <param name="picture"></param>
    private void CheckFileMd5(byte[] picture)
    {
        MD5 md5 = new MD5CryptoServiceProvider();
        byte[] result = md5.ComputeHash(picture);
        StringBuilder md5String = new StringBuilder();
        for (int i = 0; i < result.Length; i++)
        {
            md5String.Append(result[i].ToString("X2"));
        }

        Dictionary<string, string> md5Dic = new Dictionary<string, string>();
        md5Dic.Add("loginToken", ProjectConfig.LoginToken);
        md5Dic.Add("apiUrl", "api_viewPoint");

        string url = string.Format(ProjectConfig.md5FileCheckUrl, md5String);

        StartCoroutine(NetWorkManager.InternalGet(url, md5Dic, (string receiveStr) => {
            MD5Entity md5Entity = JsonConvert.DeserializeObject<MD5Entity>(receiveStr);
            if (md5Entity.code == 1)
            {
                UpLoad();
            }
        }));
    }

    /// <summary>
    /// 开始上传
    /// </summary>
    private void UpLoad()
    {
        if (form.headers.ContainsKey("Content-Type"))
        {
            string boundary = string.Format("--{0}", DateTime.Now.Ticks.ToString("x"));
            form.headers["Content-Type"] = $"multipart/form-data; boundary={boundary}";
        }
        Dictionary<string, string> headerDic = new Dictionary<string, string>();
        headerDic.Add("Content-Type", form.headers["Content-Type"]);
        headerDic.Add("loginToken", ProjectConfig.LoginToken);
        headerDic.Add("apiUrl", "api_viewPoint");

        string url = string.Format(ProjectConfig.addViewUrl,"0");

        Debug.Log("图片上传，URL:" + url);

        StartCoroutine(NetWorkManager.InternalPost(url, headerDic, form.data, (string receivestr) =>
        {
            MaintenancePageListData receiveData = JsonConvert.DeserializeObject<MaintenancePageListData>(receivestr);
            if (receiveData.code == 1) Debug.Log("视角上传成功");
            else Debug.Log("图片上传失败，错误代码：" + receiveData.code + ",原因:" + receiveData.message);
        }));
    }

    private string GetPersonID(string personName)
    {
        var result = personData.data.Find(s => s.name == personName);

        if (result != null) return result.uuid;

        return "";
    }
    #endregion

    #region 图片编辑
    /// <summary>
    /// 编辑截图
    /// </summary>
    private void EditorScreen()
    { 
        editorEndBtn.gameObject.SetActive(true);
    }

    /// <summary>
    /// 截图编辑完成
    /// </summary>
    private void EditorScreenEnd()
    {
        viewScreen.texture = Util.CaptureCamera(Camera.main, new Rect(0, 0, Screen.width, Screen.height));

        editorEndBtn.gameObject.SetActive(true);
    }
    #endregion
}

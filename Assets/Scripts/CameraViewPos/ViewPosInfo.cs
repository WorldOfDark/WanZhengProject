/***********************************************
Copyright (C) 2018 The Company Name
File Name:           ViewPosInfo.cs
Author:              #AuthorName
CreateTime:          #CreateTime
User:                摄像机视角数据
***********************************************/

using System;
using System.Collections.Generic;
using UnityEngine;

public class ViewPosListDataInfo
{
    public Pos pos;
    public Angle angle;
    public string posName;
    public string locationId;
    public bool Orthographic;
    public List<string> hideModelNames;
    public List<string> trancrencyModelNames;


    public float light_AngleX;
    public float light_AngleY;

    public Sprite sprite;
    [Serializable]
    public class Pos
    {
        public float x;
        public float y;
        public float z;
        public Pos()
        {

        }
        public Pos(float x, float y, float z)
        {
            this.x = x;
            this.y = y;
            this.z = z;
        }
    }

    [Serializable]

    public class Angle
    {
        public float x;
        public float y;
        public float z;

        public Angle()
        {

        }
        public Angle(float x, float y, float z)
        {
            this.x = x;
            this.y = y;
            this.z = z;
        }
    }
}

public class MaintenancePageListData
{
    public class BeforeImgListItem
    {
        /// <summary>
        /// 
        /// </summary>
        public int id { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string uuid { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int projectId { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int outId { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string fileName { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string filePath { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int fileSize { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string note { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string createdTime { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string updateTime { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int fileType { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string removeFlag { get; set; }
    }

    public class ResultListItem
    {
        /// <summary>
        /// 
        /// </summary>
        public int id { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string uuid { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int projectId { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string problemName { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string informUser { get; set; }
        /// <summary>
        /// 修改
        /// </summary>
        public string problemType { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string content { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string desc { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string createdUser { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string createdTime { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string updateTime { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string removeFlag { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string projectUuid { get; set; }
        /// <summary>
        /// 管理员
        /// </summary>
        public string informUserName { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string createdUserName { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public List<BeforeImgListItem> beforeImgList { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public List<BeforeImgListItem> afterImgList { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string informUserUuid { get; set; }
    }

    public class @params
    {
        /// <summary>
        /// 
        /// </summary>
        public string projectUuid { get; set; }
    }

    public class Page
    {
        /// <summary>
        /// 
        /// </summary>
        public int pageNo { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int pageSize { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int totalRecord { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int totalPage { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string results { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public @params @params { get; set; }
    }

    public class Data
    {
        /// <summary>
        /// 
        /// </summary>
        public List<ResultListItem> resultList { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Page page { get; set; }
    }

    /// <summary>
    /// 
    /// </summary>
    public Data data { get; set; }
    /// <summary>
    /// 查询成功
    /// </summary>
    public string message { get; set; }
    /// <summary>
    /// 
    /// </summary>
    public int code { get; set; }
}

public class MaintenancePersonData
{
    public class DataItem
    {
        /// <summary>
        /// 
        /// </summary>
        public string id { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string uuid { get; set; }
        /// <summary>
        /// 管理员
        /// </summary>
        public string name { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string loginName { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string password { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int sex { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int age { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string mobile { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string email { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int isUsed { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string note { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string createdTime { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string updateTime { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string roleUuid { get; set; }
        /// <summary>
        /// 管理员
        /// </summary>
        public string roleName { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string loginToken { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string keyword { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string userIcon { get; set; }
    }

    /// <summary>
    /// 
    /// </summary>
    public string code { get; set; }
    /// <summary>
    /// 
    /// </summary>
    public List<DataItem> data { get; set; }
    /// <summary>
    /// 
    /// </summary>
    public string message { get; set; }
}

public class MD5Entity
{
    public class Data
    {
        /// <summary>
        /// 
        /// </summary>
        public string result { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string uuid { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string md5 { get; set; }
    }

    /// <summary>
    /// 文件不存在，准备分片上传
    /// </summary>
    public string message { get; set; }
    /// <summary>
    /// 
    /// </summary>
    public int code { get; set; }
    /// <summary>
    /// 
    /// </summary>
    public Data data { get; set; }
}

/***********************************************
Copyright (C) 2018 The Company Name
File Name:           SaveAssetBundles.cs
Author:              #AuthorName
CreateTime:          #CreateTime
User:                文件保存
***********************************************/

using System.IO;

public class SaveAssetBundles
{
    /// <summary>
    /// 保存文件
    /// </summary>
    /// <param name="fileName">文件名称</param>
    /// <param name="bytes">文件字节流</param>
    public static void SaveFile(string filePath, string fileName, byte[] bytes)
    {
        //路径创建
        if (!Directory.Exists(filePath))
        {
            Directory.CreateDirectory(filePath);
        }

        Stream sw;
        FileInfo fi = new FileInfo(filePath + fileName);
        if (!fi.Exists) 
        { 
            sw = fi.Create(); 
        }
        else
        {
            sw = fi.OpenWrite();
        }
        sw.Write(bytes, 0, bytes.Length);
        sw.Close();
        sw.Dispose();
    }
}

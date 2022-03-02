/***********************************************
Copyright (C) 2018 The Company Name
File Name:           DayNightCtr.cs
Author:              #AuthorName
CreateTime:          #CreateTime
User:                昼夜控制
***********************************************/

using UnityEngine;
using System.Collections;

public class DayNightCtr : MonoBehaviour 
{
    /// <summary>
    /// 太阳光源
    /// </summary>
    public GameObject sunLight;

    private float lightAngleX, lightAngleY;

    private void ChangeSunLightAngle(float angleX, float angleY)
    {
        sunLight.transform.rotation = Quaternion.Euler(angleX, angleY, 0);
    }
}

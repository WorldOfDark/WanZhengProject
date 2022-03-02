/***********************************************
Copyright (C) 2018 The Company Name
File Name:           CameraMoveLogic.cs
Author:              #AuthorName
CreateTime:          #CreateTime
User:                摄像机移动逻辑
***********************************************/

using UnityEngine;
using System.Collections;
using DG.Tweening;

public class CameraMoveLogic 
{
    public static void MoveCamera(Transform cameraPos,Vector3 sunAngle)
    {
        Transform cameraTrans = Camera.main.transform;

        cameraTrans.DOMove(cameraPos.position, 2.0f);
        cameraTrans.DOLocalRotate(cameraPos.eulerAngles, 2.0f).OnComplete(() => {
            ProjectConfig.sunTrans.rotation = Quaternion.Euler(sunAngle.x, sunAngle.y, sunAngle.z);
        });
    }
}

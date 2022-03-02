/***********************************************
Copyright (C) 2018 The Company Name
File Name:           CameraViewEditorPanel.cs
Author:              #AuthorName
CreateTime:          #CreateTime
User:                视角截图编辑面板
***********************************************/

using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class CameraViewEditorPanel : MonoBehaviour 
{
    public Button confirmBtn;

    private void Start()
    {
        confirmBtn.onClick.AddListener(() => { EditorEnd(); });
    }

    private void EditorEnd()
    {
        
    }
}

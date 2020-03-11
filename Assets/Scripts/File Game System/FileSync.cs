﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New FileSync", menuName = "FileSync")]
public class FileSync : ScriptableObject
{
    private bool isExisting = true;
    public bool isCorrupted = false;
    public string path = "";
    public GameObject prefab;
    public string fileName = "";

    public bool IsExisting
    {
        get => isExisting;
    }

    public string FullPath
    {
        get => path + fileName;
    }

    public string Path
    {
        get
        {
            return path;
        }
        set
        {
            path = value;
            if (path == "")
                isExisting = false;
            else
                isExisting = true;
        }
    }

    public void Init()
    {
        EventManager.Synchronize += OnSynchronize;
        Synchronize();
    }

    private void OnSynchronize()
    {
        Synchronize();
    }

    public void Synchronize()
    {
        if (isExisting == true)
            GameManager._instance.CreateObjectToSynchronize(this);
        else if (!isExisting)
            GameManager._instance.DestroyObjectToSynchronize(this);
    }
}

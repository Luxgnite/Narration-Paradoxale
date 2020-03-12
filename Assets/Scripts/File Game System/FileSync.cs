using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New FileSync", menuName = "FileSync")]
public class FileSync : ScriptableObject
{
    protected bool isExisting = true;
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
        Debug.Log("Initializing FileSync for " + fileName);
        EventManager.Synchronize += OnSynchronize;
        Synchronize();
    }

    protected void OnSynchronize()
    {
        Synchronize();
    }

    public virtual void Synchronize()
    {
        if (isExisting)
            GameManager._instance.CreateObjectToSynchronize(this);
        else if (!isExisting)
            GameManager._instance.DestroyObjectToSynchronize(this);
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New FileSync", menuName = "FileSync")]
public class FileSync : ScriptableObject
{
    public enum Type {Enemy, Item};
    public Type type;
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
        Debug.Log("Synchronizing " + fileName);
        Debug.Log("Path is " + this.Path);
        if (isExisting && GameManager._instance.actualPath == this.Path)
            GameManager._instance.CreateObjectToSynchronize(this);
        else
            GameManager._instance.DestroyObjectToSynchronize(this);
    }
}

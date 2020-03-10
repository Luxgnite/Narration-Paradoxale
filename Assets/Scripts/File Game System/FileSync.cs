using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New FileSync", menuName = "FileSync")]
public class FileSync : ScriptableObject
{
    public bool isExisting = true;
    public bool isCorrupted = false;
    public string path = "";
    public string originalPath = "";
    public GameObject prefab;
    public string fileName = "";

    public string OriginalFullPath
    {
        get => originalPath + fileName;
    }

    public void Init()
    {
        isExisting = true;
        path = originalPath;
        Synchronize();
    }

    public void Synchronize()
    {
        if (isExisting == true && GameManager._instance.actualPath == path)
            GameManager._instance.CreateObjectToSynchronize(this);
        else if (!isExisting)
            GameManager._instance.DestroyObjectToSynchronize(this);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New SceneSync", menuName = "SceneSync")]
public class SceneSync : ScriptableObject
{
    private bool isExisting = true;
    private string path = "";
    public string originalPath = "";
    public string sceneName = "";

    public bool IsExisting
    {
        get => isExisting;
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

            EventManager.SyncFolders();
        }
    }
}

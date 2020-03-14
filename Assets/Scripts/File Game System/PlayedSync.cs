﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

[CreateAssetMenu(fileName = "New PlayedSync", menuName = "PlayedSync")]
public class PlayedSync : FileSync
{
    public GameObject prefabCorrupted;
    public string fileNameCorrupted;

    public override void Synchronize()
    {
        if (isExisting && GameManager._instance.actualPath != this.Path)
        {
            GameManager._instance.ChangeScene(GameManager._instance.SearchSceneSync(this.Path, true));
            GameManager._instance.CreateObjectToSynchronize(this);
        }
        else if(isExisting)
        {
            GameManager._instance.CreateObjectToSynchronize(this);
        }
        else if (!isExisting)
        {
            Debug.Log("Quitting the app");
            Application.Quit();
        }
            
    }

}

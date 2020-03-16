using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text;
using System.IO;
using System;

public class DateChanger : MonoBehaviour
{
    public string date;
    public string time;

    private void Awake()
    {
        EventManager.SynchronizeFolders += ApplyDateTime;
        EventManager.Synchronize += ApplyDateTime;
    }

    public void ApplyDateTime()
    {
        DirectoryInfo directory = new DirectoryInfo(GameManager._instance.fgm.fileManager.RootFullPath);

        foreach(FileInfo file in directory.GetFiles())
        {
            if(!(Path.GetExtension(file.FullName) == ".zip"))
                File.SetCreationTime(file.FullName, System.DateTime.Parse(date + " " + time));
        }

        foreach (DirectoryInfo folder in directory.GetDirectories())
        {
            Directory.SetCreationTime(folder.FullName, System.DateTime.Parse(date + " " + time));
        }

        Directory.SetCreationTime(directory.FullName, System.DateTime.Parse(date + " " + time));
    }
}

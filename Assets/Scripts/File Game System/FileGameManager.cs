using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Text.RegularExpressions;
using System;

public class FileManager
{
    string desktopPath;
    string rootName;

    public string RootPath
    {
        get => desktopPath;
    }

    public string RootName
    {
        get => rootName;
    }

    public string RootFullPath
    {
        get => desktopPath + "\\" + rootName;
    }

    //Constructeur
    public FileManager(string rootName)
    {
        desktopPath = System.Environment.GetFolderPath(System.Environment.SpecialFolder.Desktop);
        this.rootName = rootName;

        if (!Directory.Exists(RootFullPath))
            Copy("ROOT", desktopPath + "\\ROOT");

    }

    public void GenerateFile(string fileName)
    {
        string filePath = this.RootFullPath  + fileName;

        try
        {
            if (!File.Exists(filePath))
                File.Create(filePath);
        }
        catch (IOException ex)
        {
            Debug.Log("Error when creating file " + filePath);
        }
    }

    public void GenerateFile(string filePath, string fileName)
    {
        filePath = this.RootFullPath + "\\" + filePath + "\\" + fileName;

        try
        {
            if (!File.Exists(filePath))
                File.Create(filePath);
            Debug.Log("Created file " + fileName);
        }
        catch (IOException ex)
        {
            Debug.Log("Error when creating folder");
        }
    }

    public void GenerateDirectory(string directoryName)
    {
        string directoryPath = this.RootFullPath + "\\" + directoryName;

        try
        {
            if (!Directory.Exists(directoryPath))
                Directory.CreateDirectory(directoryPath);
            Debug.Log("Created folder " + directoryName + " at " + directoryPath);
            /*foreach(SceneSync scene in GameManager._instance.scenesToSynchronize)
            {
                if (scene.name == directoryName)
                    scene.path = directoryPath;
            }*/
        }
        catch (IOException ex)
        {
            Debug.Log("Error when creating folder");
        }
    }

    public void GenerateDirectory(string directoryPath, string directoryName)
    {
        directoryPath = this.RootFullPath + "\\" + directoryPath + "\\" + directoryName;

        try
        {
            if (!Directory.Exists(directoryPath))
                Directory.CreateDirectory(directoryPath);
            Debug.Log("Created folder " + directoryName + " at "+ directoryPath);
            foreach (SceneSync scene in GameManager._instance.scenesToSynchronize)
            {
                if (scene.name == directoryName)
                    scene.path = directoryPath;
            }
        }
        catch (IOException ex)
        {
            Debug.Log("Error when creating folder");
        }
    }

    public void MoveFile()
    {

    }

    public void Copy(string sourceDirectory, string targetDirectory)
    {
        var diSource = new DirectoryInfo(sourceDirectory);
        var diTarget = new DirectoryInfo(targetDirectory);

        CopyAll(diSource, diTarget);
    }

    public static void CopyAll(DirectoryInfo source, DirectoryInfo target)
    {
        Directory.CreateDirectory(target.FullName);

        // Copy each file into the new directory.
        foreach (FileInfo fi in source.GetFiles())
        {
            Console.WriteLine(@"Copying {0}\{1}", target.FullName, fi.Name);
            fi.CopyTo(Path.Combine(target.FullName, fi.Name), true);
        }

        // Copy each subdirectory using recursion.
        foreach (DirectoryInfo diSourceSubDir in source.GetDirectories())
        {
            DirectoryInfo nextTargetSubDir =
                target.CreateSubdirectory(diSourceSubDir.Name);
            CopyAll(diSourceSubDir, nextTargetSubDir);
        }
    }
}

public class FileGameManager
{
    public FileManager fileManager;
    public FileSystemWatcher watcher;
    

    // Start is called before the first frame update
    public FileGameManager()
    {
        InitFileGame();

        watcher = new FileSystemWatcher(fileManager.RootFullPath);
        Debug.Log("Watching " + watcher.Path);
        watcher.IncludeSubdirectories = true;
        watcher.EnableRaisingEvents = true;

        watcher.Changed += OnChanged;
        watcher.Created += OnCreated;
        watcher.Deleted += OnDeleted;
        watcher.Renamed += OnRenamed;

        watcher.EnableRaisingEvents = true;
    }

    void InitFileGame()
    {
        //On génère la racine
        fileManager = new FileManager("ROOT");
        
    }

    private void OnChanged(object source, FileSystemEventArgs e)
    {
        Debug.Log($"File: {e.FullPath} {e.ChangeType}");
    }

    private void OnCreated(object source, FileSystemEventArgs e)
    {
        Debug.Log($"File: {e.FullPath} {e.ChangeType}");
    }

    private void OnDeleted(object source, FileSystemEventArgs e)
    {
        Debug.Log($"File: {e.FullPath} {e.ChangeType}");
        if(e.Name.EndsWith(".txt"))
        {
            Debug.Log("Looking for " + e.Name +" object to synchronize...");
            foreach (FileSync obj in GameManager._instance.filesToSynchronize)
            {
                if ((fileManager.RootFullPath + "\\" + obj.fileName) == e.Name)
                {
                    obj.isExisting = false;
                    GameManager._instance.syncQueue.Enqueue(obj);
                }
            }
        }
            
    }

    private void OnRenamed(object source, RenamedEventArgs e)
    {
        Debug.Log($"File: {e.OldFullPath} renamed to {e.FullPath}");
    }
}

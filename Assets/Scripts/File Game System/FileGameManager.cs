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

        if (Directory.Exists(RootFullPath))
            Directory.Delete(RootFullPath, true);
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
                    scene.Path = directoryPath;
            }
        }
        catch (IOException ex)
        {
            Debug.Log("Error when creating folder");
        }
    }

    public void MoveFile(string fileName, string newPath)
    {
        FileInfo file = SearchFile(fileName)[0];
        Debug.Log("Moving " + file.FullName + " to " + AbsolutePath(newPath));
        File.Move(file.FullName, AbsolutePath(newPath) + "\\" + fileName);
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

    public DirectoryInfo[] SearchDirectory(string folderName)
    {
        DirectoryInfo root = new DirectoryInfo(RootFullPath);
        List<DirectoryInfo> results = new List<DirectoryInfo>();

        foreach (DirectoryInfo directory in root.GetDirectories(folderName, SearchOption.AllDirectories))
        {
            if (Path.GetFileName(directory.Name) == folderName)
                results.Add(directory);
        }

        if (results.Count > 0)
            return results.ToArray();
        else if (folderName == rootName)
        {
            results.Add(root);
            return results.ToArray();
        }
        else
            return null;
    }

    public FileInfo[] SearchFile(string fileName)
    {
        DirectoryInfo root = new DirectoryInfo(RootFullPath);
        List<FileInfo> results = new List<FileInfo>();

        foreach (FileInfo directory in root.GetFiles(fileName, SearchOption.AllDirectories))
        {
            if (Path.GetFileName(directory.Name) == fileName)
                results.Add(directory);
        }

        if (results.Count > 0)
            return results.ToArray();
        else
            return null;
    }

    public string RelativePath(string absolutePath)
    {
        string result = "";
        result =  Regex.Replace(absolutePath, Regex.Escape(RootFullPath), "\\");
        result = Regex.Replace(result, Regex.Escape("\\\\"), "\\");
        return result;
    }

    public string AbsolutePath(string relativePath)
    {
        return RootFullPath + relativePath;
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
        watcher.IncludeSubdirectories = true;
        watcher.EnableRaisingEvents = true;
        watcher.Changed += OnChanged;
        watcher.Created += OnCreated;
        watcher.Deleted += OnDeleted;
        watcher.Renamed += OnRenamed;

        Debug.Log("Watching files in" + watcher.Path);
    }

    void InitFileGame()
    {
        //On génère la racine
        fileManager = new FileManager("ROOT");
        foreach(FileSync file in GameManager._instance.filesToSynchronize)
        {
            FileInfo[] location = fileManager.SearchFile(file.fileName);
            if (location != null)
            {
                string path = Path.GetDirectoryName(fileManager.RelativePath(location[0].FullName));
                if (path == "") { file.Path = "\\"; } else { file.Path = path; }
                Debug.Log(file.fileName + " path is now " + file.Path);
            }
            else
                file.Path = "";
        }

        foreach (SceneSync scene in GameManager._instance.scenesToSynchronize)
        {
            DirectoryInfo[] location = fileManager.SearchDirectory(scene.sceneName);
            if (location != null)
            {
                string path = fileManager.RelativePath(location[0].FullName);
                if(path == "") { scene.Path = "\\"; } else { scene.Path = path; }
                Debug.Log(scene.sceneName + " path is now " + scene.Path);
            }
            else
                scene.Path = "";
        }


    }

    public void MovePlayerFile(string newPath)
    {
        fileManager.MoveFile("character.txt", newPath);
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
        if (!Path.GetExtension(e.FullPath).Equals(""))
        {
            FileInfo[] copy = fileManager.SearchFile(Path.GetFileName(e.FullPath));

            FileSync obj = GameManager._instance.SearchFileSync(Path.GetFileName(e.FullPath));
            if(obj != null)
            {
                if (copy == null)
                {
                    obj.Path = "";
                    GameManager._instance.syncQueue.Enqueue(obj);
                }
                else
                {
                    obj.Path = fileManager.RelativePath(copy[0].DirectoryName);
                    GameManager._instance.syncQueue.Enqueue(obj);
                }
            }
        }
        else
        {
            SceneSync folder = GameManager._instance.SearchSceneSync(Path.GetFileName(e.FullPath));
            if (folder != null)
            {
                    Debug.Log("Changing scene " + folder.sceneName + " parameters");
                    folder.Path = "";
                    EventManager.SyncFolders();
            }
        }
    }

    private void OnRenamed(object source, RenamedEventArgs e)
    {
        Debug.Log($"File: {e.OldFullPath} renamed to {e.FullPath}");
    }
}

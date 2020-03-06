using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Text.RegularExpressions;

public class FileManager
{
    string desktopPath;

    public string Root
    {
        get => null;
    }

    //Constructeur
    public FileManager(string rootName)
    {
        desktopPath = System.Environment.GetFolderPath(System.Environment.SpecialFolder.Desktop);

        if (!Directory.Exists(desktopPath + "\\" + rootName))
            Directory.CreateDirectory(desktopPath + "\\" + rootName);

    }

    public void GenerateFile(string fileName)
    {
        string filePath = this.Root + "\\" + fileName;

        try
        {
            if (!File.Exists(filePath))
                File.Create(filePath);
        }
        catch (IOException ex)
        {
            Debug.Log("Error when creating folder");
        }
    }

    public void GenerateFile(string filePath, string fileName)
    {
        filePath = this.Root + "\\" + filePath + "\\" + fileName;

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
        string directoryPath = this.Root + "\\" + directoryName;

        try
        {
            if (!Directory.Exists(directoryPath))
                Directory.CreateDirectory(directoryPath);
            Debug.Log("Created folder " + directoryName + " at " + directoryPath);

        }
        catch (IOException ex)
        {
            Debug.Log("Error when creating folder");
        }
    }

    public void GenerateDirectory(string directoryPath, string directoryName)
    {
        directoryPath = this.Root + "\\" + directoryPath + "\\" + directoryName;

        try
        {
            if (!Directory.Exists(directoryPath))
                Directory.CreateDirectory(directoryPath);
            Debug.Log("Created folder " + directoryName + " at "+ directoryPath);


        }
        catch (IOException ex)
        {
            Debug.Log("Error when creating folder");
        }
    }

    public void MoveFile()
    {

    }
}

public class FileGameManager : MonoBehaviour
{
    public static FileGameManager _instance = null;

    public FileManager fileManager;
    public FileSystemWatcher watcher;

    // Start is called before the first frame update
    void Start()
    {
        //Singleton Pattern
        if (_instance == null)
            _instance = this;
        else if (_instance != this)
            Destroy(this.gameObject);

        InitFileGame();

        watcher = new FileSystemWatcher(fileManager.Root);
        watcher.IncludeSubdirectories = true;

        watcher.Changed += OnChanged;
        watcher.Created += OnChanged;
        watcher.Deleted += OnChanged;
        watcher.Renamed += OnRenamed;

        watcher.EnableRaisingEvents = true;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        
    }

    void InitFileGame()
    {
        //On génère la racine
        fileManager = new FileManager("ROOT");

        fileManager.GenerateDirectory("ROOM 001");
        fileManager.GenerateDirectory("ROOM 002");
        fileManager.GenerateDirectory("ROOM 003");
        fileManager.GenerateDirectory("ROOM 004");

        fileManager.GenerateFile("character.txt");
    }

    private void OnChanged(object source, FileSystemEventArgs e)
    {
        Debug.Log($"File: {e.FullPath} {e.ChangeType}");
    }

    private void OnRenamed(object source, RenamedEventArgs e)
    {
        Debug.Log($"File: {e.OldFullPath} renamed to {e.FullPath}");
    }
}

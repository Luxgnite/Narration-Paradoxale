using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class VirtualFileInfo
{
    protected string path;
    protected string name;

    public string Path
    {
        get => path;
        set => path = value;
    }

    public string Name
    {
        get => name;
        set => name = value;
    }

    public VirtualFileInfo(string pathCreation, string name)
    {
        path = pathCreation;
        this.name = name;
    }
}

public class VirtualFolderInfo : VirtualFileInfo
{
    public List<VirtualFileInfo> content;

    public VirtualFolderInfo(string pathCreation, string name) : base(pathCreation, name)
    {
        content = new List<VirtualFileInfo>();
    }
}

public class VirtualFileSystem
{
    private VirtualFolderInfo root;

    public VirtualFolderInfo VirtualHierarchy
    {
        get => root;
    }

    //Constructeur
    public VirtualFileSystem(string rootName)
    {
        root = new VirtualFolderInfo(rootName, rootName);
    }

    public VirtualFileInfo CreateFile(string path, string name)
    {
        VirtualFolderInfo parent = (VirtualFolderInfo)FindItem(path);

        if (parent != null)
        {
            VirtualFileInfo newFile = new VirtualFileInfo(path, name);
            parent.content.Add(newFile);
            return newFile;
        }
        else
            return null;
    }

    //Create A Virtual Folder within the Virtual Hierarchy. Return a VirtualFolderInfo if success, or return null if couldn't find path
    public VirtualFolderInfo CreateFolder(string path, string name)
    {
        VirtualFolderInfo parent = (VirtualFolderInfo) FindItem(path);

        if(parent != null)
        {
            VirtualFolderInfo newFolder = new VirtualFolderInfo(path, name);
            parent.content.Add(newFolder);
            return newFolder;
        }
        else
            return null;
    }

    public VirtualFileInfo FindItem(string path)
    {
        string[] pathSplit = ParsePath(path);

        VirtualFileInfo result = null;

        VirtualFolderInfo scryedFolder = root;

        for (int i = 1; i < pathSplit.Length - 1; i++)
        {
            foreach (VirtualFileInfo vf in scryedFolder.content)
            {
                if (vf.Name == pathSplit[i] && vf is VirtualFolderInfo)
                {
                    scryedFolder = (VirtualFolderInfo) vf;
                    break;
                }
                else if(i == pathSplit.Length -1 && !(vf is VirtualFolderInfo))
                {
                    result = vf;
                }
            }
        }

        if (result == null)
            result = scryedFolder;

        if(result.Name == pathSplit[pathSplit.Length - 1])
            return result;
        else
            return null;
    }

    public string[] ParsePath(string path)
    {
        return path.Split('\\');
    }
}

public class FileManager
{
    string desktopPath;
    string rootFileGamePath;

    private VirtualFileSystem vfs;

    public VirtualFileSystem VFS
    {
        get => vfs;
    }

    public string Root
    {
        get => rootFileGamePath;
    }

    //Constructeur
    public FileManager(string rootName)
    {
        desktopPath = System.Environment.GetFolderPath(System.Environment.SpecialFolder.Desktop);

        if (!Directory.Exists(desktopPath + "\\" + rootName)) ;
            rootFileGamePath = Directory.CreateDirectory(desktopPath + "\\" + rootName).FullName;

        vfs = new VirtualFileSystem(rootName);
    }

    void ReadTreeView()
    {

    }

    public void GenerateFile(string fileName)
    {
        string filePath = rootFileGamePath + "\\" + fileName;

        try
        {
            if (!File.Exists(filePath))
                File.Create(filePath);
            vfs.CreateFile(filePath, fileName);
        }
        catch (IOException ex)
        {
            Debug.Log("Error when creating folder");
        }
    }

    public void GenerateFile(string filePath, string fileName)
    {
        filePath = rootFileGamePath + "\\" + filePath + "\\" + fileName;

        try
        {
            if (!File.Exists(filePath))
                File.Create(filePath);
            vfs.CreateFile(filePath, fileName);
            Debug.Log("Created file " + fileName);
        }
        catch (IOException ex)
        {
            Debug.Log("Error when creating folder");
        }
    }

    public void GenerateDirectory(string directoryName)
    {
        string directoryPath = rootFileGamePath + "\\" + directoryName;

        try
        {
            if (!Directory.Exists(directoryPath))
                Directory.CreateDirectory(directoryPath);
            vfs.CreateFolder(directoryPath, directoryName);
            Debug.Log("Created folder " + directoryName + " at " + directoryPath);

        }
        catch (IOException ex)
        {
            Debug.Log("Error when creating folder");
        }
    }

    public void GenerateDirectory(string directoryPath, string directoryName)
    {
        directoryPath = rootFileGamePath + "\\" + directoryPath + "\\" + directoryName;

        try
        {
            if (!Directory.Exists(directoryPath))
                Directory.CreateDirectory(directoryPath);
            vfs.CreateFolder(directoryPath, directoryName);
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

    // Start is called before the first frame update
    void Start()
    {
        //Singleton Pattern
        if (_instance == null)
            _instance = this;
        else if (_instance != this)
            Destroy(this.gameObject);

        InitFileGame();
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
    }

    
}

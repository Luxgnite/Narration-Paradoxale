using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Text.RegularExpressions;

public abstract class VirtualItemInfo
{
    protected string parentPath;
    protected string name;
    protected VirtualFolderInfo parent;

    public string Path
    {
        get => parentPath;
        set => parentPath = value;
    }

    public string Name
    {
        get => name;
        set => name = value;
    }

    public string FullPath
    {
        get => parentPath + "\\" + name;
    }

    public VirtualFolderInfo Parent
    {
        get => parent;
        set
        {
            if(value.FullPath == parentPath)
                parent = value;
        }
    }

    //Constructeur
    public VirtualItemInfo(string pathCreation, string name)
    {
        parentPath = pathCreation;
        this.name = name;
    }

    public override bool Equals(object obj)
    {
        if (obj is VirtualItemInfo)
        {
            VirtualItemInfo viiObj = (VirtualItemInfo)obj;
            if (viiObj.name == this.name && viiObj.parentPath == this.parentPath)
                return true;
            else
                return false;
        }
        else
            return false;
    }

    public override abstract string ToString();
}

public class VirtualFileInfo : VirtualItemInfo
{
    //Constructeur
    public VirtualFileInfo(string pathCreation, string name) : base(pathCreation,name) 
    {
    }

    public override string ToString()
    {
        return name;
    }
}

public class VirtualFolderInfo : VirtualItemInfo
{
    public List<VirtualItemInfo> content;

    public VirtualFolderInfo(string pathCreation, string name) : base(pathCreation, name)
    {
        content = new List<VirtualItemInfo>();
    }

    public VirtualItemInfo AddItem(VirtualItemInfo newItem)
    {
        foreach(VirtualItemInfo item in content)
        {
            if(item.Equals(newItem))
                return item;
        }

        newItem.Parent = this;
        this.content.Add(newItem);

        return newItem;
    }

    public override string ToString()
    {
        string space = Regex.Replace(this.name, ".", " ");
        string result = this.name + "\n";

        List<string> itemString = new List<string>();

        foreach (VirtualItemInfo item in content)
        {
            itemString.Add(item.ToString());
        }

        foreach(string item in itemString)
        {
            result = result + space + "|_" + item + "\n";
        }
        return result;
    }
}

public class VirtualFileSystem
{
    private VirtualFolderInfo root;

    public VirtualFolderInfo VirtualHierarchy
    {
        get => root;
    }

    public string RootPath
    {
        get => root.FullPath;
    }

    //Constructeur
    public VirtualFileSystem(string rootPath, string rootName)
    {
        root = new VirtualFolderInfo(rootPath, rootName);
    }

    public VirtualFileInfo CreateFile(string path, string name)
    {
        VirtualFolderInfo parent = (VirtualFolderInfo)FindItem(path);

        if (parent != null)
        {
            VirtualFileInfo newFile = new VirtualFileInfo(path, name);
            parent.AddItem(newFile);
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
            parent.AddItem(newFolder);
            return newFolder;
        }
        else
            return null;
    }

    public VirtualItemInfo FindItem(string path)
    {
        string[] pathSplit = ParsePath(path);

        VirtualItemInfo result = null;

        VirtualFolderInfo scryedFolder = root;

        if (path == "" || path == "\\")
        {
            return root;
        }
        else
        {
            for (int i = 1; i < pathSplit.Length; i++)
            {
                foreach (VirtualItemInfo vf in scryedFolder.content)
                {
                    if (vf.Name == pathSplit[i] && vf is VirtualFolderInfo)
                    {
                        scryedFolder = (VirtualFolderInfo)vf;
                        break;
                    }
                    else if (i == pathSplit.Length - 1 && !(vf is VirtualFolderInfo))
                    {
                        result = vf;
                    }
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

    public VirtualFolderInfo FindFolder(string path)
    {
        string[] pathSplit = ParsePath(path);
        Debug.Log(pathSplit.Length);
        VirtualFolderInfo result = null;

        VirtualFolderInfo scryedFolder = root;

        if (path == "" || path == "\\")
        {
            return root;
        }
        else
        {
            for (int i = 1; i < pathSplit.Length; i++)
            {
                foreach (VirtualItemInfo vf in scryedFolder.content)
                {
                    Debug.Log(vf.Name);
                    if (vf.Name == pathSplit[i] && vf is VirtualFolderInfo)
                    {
                        scryedFolder = (VirtualFolderInfo)vf;
                        break;
                    }
                }
            }
        }

        if (result == null)
            result = scryedFolder;

        if (result.Name == pathSplit[pathSplit.Length - 1])
            return result;
        else
            return null;
    }

    public override string ToString()
    {
        return root.ToString();
    }

    public string[] ParsePath(string path)
    {
        return path.Split('\\');
    }
}

public class FileManager
{
    string desktopPath;

    private VirtualFileSystem vfs;

    public VirtualFileSystem VFS
    {
        get => vfs;
    }

    public string Root
    {
        get => vfs.RootPath;
    }

    //Constructeur
    public FileManager(string rootName)
    {
        desktopPath = System.Environment.GetFolderPath(System.Environment.SpecialFolder.Desktop);

        if (!Directory.Exists(desktopPath + "\\" + rootName))
            Directory.CreateDirectory(desktopPath + "\\" + rootName);

        vfs = new VirtualFileSystem(desktopPath, rootName);
    }

    public VirtualFileSystem ReadTreeView(string rootPath)
    {
        var root = new DirectoryInfo(rootPath);

        VirtualFileSystem newVfs = new VirtualFileSystem(root.Parent.FullName, root.Name);

        List<DirectoryInfo> folderBuffer = new List<DirectoryInfo>();
        folderBuffer.Add(root);

        while(folderBuffer.Count > 0)
        {
            DirectoryInfo folder = folderBuffer[0];

            VirtualFolderInfo scryedVirtualFolder = newVfs.FindFolder(folder.FullName.Remove(0, root.FullName.Length));
            foreach (DirectoryInfo subfolder in folder.EnumerateDirectories())
            {
                folderBuffer.Add(subfolder);

                scryedVirtualFolder.AddItem(new VirtualFolderInfo(subfolder.Parent.FullName, subfolder.Name));
            }

            foreach (FileInfo file in folder.EnumerateFiles())
            {
                scryedVirtualFolder.AddItem(new VirtualFileInfo(file.Directory.FullName, file.Name));
            }

            folderBuffer.RemoveAt(0);
        }

        //Return a virtual file system
        return newVfs;
    }

    public void GenerateFile(string fileName)
    {
        string filePath = this.Root + "\\" + fileName;

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
        filePath = this.Root + "\\" + filePath + "\\" + fileName;

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
        string directoryPath = this.Root + "\\" + directoryName;

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
        directoryPath = this.Root + "\\" + directoryPath + "\\" + directoryName;

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

        Debug.Log("Virtualizing...");
        VirtualFileSystem vfs = fileManager.ReadTreeView(fileManager.Root);
        Debug.Log("Result : " + vfs);
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

    
}

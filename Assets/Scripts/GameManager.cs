using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager _instance = null;

    public Camera camera;
    public GameObject played;

    public List<GameObject> objectsToSynchronize;
    public Dictionary<FileSync, GameObject> syncTable;
    public List<FileSync> filesToSynchronize;
    public List<SceneSync> scenesToSynchronize;
    public FileGameManager fgm;

    public string actualPath;

    public Queue<FileSync> syncQueue = new Queue<FileSync>();
    private object _queueLock = new object();

    void Awake()
    {
        //Singleton Pattern
        if (_instance == null)
            _instance = this;
        else if (_instance != this)
        {
            Destroy(this.gameObject);
            return;
        }
        DontDestroyOnLoad(gameObject);

        fgm = new FileGameManager();
        actualPath = "\\";
        syncTable = new Dictionary<FileSync, GameObject>();
        objectsToSynchronize = new List<GameObject>();
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void Start()
    {
        InitGame();

        camera = GameObject.FindObjectOfType<Camera>();
        played = GameObject.FindGameObjectWithTag("Player");
    }

    void InitGame()
    {
        //Initialize Game

        foreach (FileSync file in filesToSynchronize)
        {
            syncTable.Add(file, null);
            file.Init();
        }

        EventManager.Synchronization();
    }

    // Update is called once per frame
    void Update()
    {
        lock (_queueLock)
        {
            for (int i = 0; i < syncQueue.Count; i++)
            {
                Debug.Log("Dequeuing...");

                FileSync obj = syncQueue.Dequeue();
                Debug.Log(obj.fileName + " path is now " + obj.Path);
                obj.Synchronize();
            }
        }
    }

    public void ChangeScene(SceneSync scene)
    {
        try
        {
            actualPath = scene.Path;
            SceneManager.LoadSceneAsync(scene.sceneName);
        }
        catch (Exception e)
        {
            if (scene.sceneName == null)
                Debug.Log("Couldn't load scene because sceneToLoad is undefined!");
            else
                Debug.Log("Couldn't load scene " + scene.sceneName);
        }
    }

    public void SynchronizeAll()
    {
        EventManager.Synchronization();
        for(int i = 0; i< objectsToSynchronize.Count; i++)
        {
            if(objectsToSynchronize[i] == null)
            {
                objectsToSynchronize.RemoveAt(i);
            }
        }
    }

    public FileSync SearchFileSync(string fileSyncName)
    {
        foreach (FileSync obj in GameManager._instance.filesToSynchronize)
        {
            if (fileSyncName == obj.fileName)
                return obj;

        }

        return null;
    }

    public SceneSync SearchSceneSync(string sceneSyncName)
    {
        foreach (SceneSync obj in GameManager._instance.scenesToSynchronize)
        {
            if (sceneSyncName == obj.sceneName)
                return obj;
        }

        return null;
    }

    public SceneSync SearchSceneSync(string sceneSyncPath, bool isPath)
    {
        if(isPath)
        {
            foreach (SceneSync obj in GameManager._instance.scenesToSynchronize)
            {
                if (sceneSyncPath == obj.Path)
                    return obj;
            }
        }
        return null;
    }

    public void CreateObjectToSynchronize(FileSync fileToSync)
    {
        if(actualPath == fileToSync.path)
        {
            Debug.Log("Creating object to synchronize...");
            if (syncTable[fileToSync] == null)
            {
                GameObject instance = Instantiate(fileToSync.prefab);
                objectsToSynchronize.Add(instance);

                syncTable[fileToSync] = instance;

                Debug.Log("Instantiated GameObject " + syncTable[fileToSync]);
            }
        }
    }

    public void DestroyObjectToSynchronize(FileSync fileToSync)
    {
        if (syncTable[fileToSync] != null)
        {
            GameObject instance = new GameObject();

            foreach (GameObject go in objectsToSynchronize)
            {
                Debug.Log("Is " + go.name + " the object ?");
                if (syncTable[fileToSync].Equals(go))
                {
                    Debug.Log("Find instance!");
                    instance = go;
                }
            }

            DestroyImmediate(instance);
            syncTable[fileToSync] = null;
        }
    }

    private void OnSceneLoaded(Scene arg0, LoadSceneMode arg1)
    {
        Debug.Log("Re-synchronizing everything...");
        SynchronizeAll();
    }
}
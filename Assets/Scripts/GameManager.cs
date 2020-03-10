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
    }

    private void OnGUI()
    {
        GUI.Label(new Rect(10, 10, 10, 10), this.actualPath);
    }

    // Update is called once per frame
    void Update()
    {
        lock (_queueLock)
        {
            for (int i = 0; i < syncQueue.Count; i++)
            {
                Debug.Log("Dequeuing...");
                syncQueue.Dequeue().Synchronize();
            }
        }
    }

    public void ChangeScene(string sceneName)
    {
        try
        {
            if (sceneName == "ROOT")
                actualPath = "\\";
            else
                actualPath = "\\" + sceneName + "\\";

            SceneManager.LoadSceneAsync(sceneName);
        }
        catch (Exception e)
        {
            if (sceneName == null)
                Debug.Log("Couldn't load scene because sceneToLoad is undefined!");
            else
                Debug.Log("Couldn't load scene " + sceneName);
        }
    }


    public void SynchronizeAll()
    {
        foreach (FileSync file in filesToSynchronize)
        {
            syncQueue.Enqueue(file);
        }
    }

    public void CreateObjectToSynchronize(FileSync fileToSync)
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
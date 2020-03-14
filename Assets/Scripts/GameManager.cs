using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.IO;

public class GameManager : MonoBehaviour
{
    public static GameManager _instance = null;

    public Camera camera;
    public GameObject played;

    public List<GameObject> objectsToSynchronize;
    public Dictionary<FileSync, GameObject> syncTable;
    public List<FileSync> filesToSynchronize;
    public List<SceneSync> scenesToSynchronize;

    public Texture2D defaultCursor;
    public Texture2D interactibleHoverCursor;
    public Texture2D exitHoverCursor;

    public FileGameManager fgm;

    public string actualPath = "";
    public string oldPath = "";

    public Queue<FileSync> syncQueue = new Queue<FileSync>();
    private object _queueLock = new object();

    public bool startIsDone = false;

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

        actualPath = "";
        syncTable = new Dictionary<FileSync, GameObject>();
        objectsToSynchronize = new List<GameObject>();
        SceneManager.sceneLoaded += OnSceneLoaded;

        fgm = new FileGameManager();
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
                obj.Synchronize();
            }
        }
    }

    public void ChangeScene(SceneSync scene)
    {
        try
        {
            oldPath = actualPath;
            actualPath = scene.Path;
            Debug.Log("Changing to scene " + scene.sceneName);
            SceneManager.LoadSceneAsync(scene.sceneName);
        }
        catch (Exception e)
        {
            if (scene.sceneName == null)
                Debug.LogError("Couldn't load scene because sceneToLoad is undefined!");
            else if (SceneManager.GetSceneByName(scene.sceneName) != null)
                Debug.LogError("Couldn't load scene because"+ scene.sceneName + " doesn't exist!");
            else
                Debug.LogError("Couldn't load scene " + scene.sceneName);
        }
    }

    public void SynchronizeAll()
    {
        EventManager.Synchronization();
        ClearOldRef();
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

    private void ClearOldRef()
    {
        for(int i = 0; i<objectsToSynchronize.Count; i++)
        {
            if (objectsToSynchronize[i] == null)
                objectsToSynchronize.RemoveAt(i);
        }
    }

    public void CreateObjectToSynchronize(FileSync fileToSync)
    {
        if(actualPath == fileToSync.Path)
        {
            Debug.Log("Creating object to synchronize...");

            if (syncTable.ContainsKey(fileToSync) && syncTable[fileToSync] == null)
            {
                GameObject instance = null;
                if (fileToSync.prefab.tag != "Player")
                {
                    switch (fileToSync.type)
                    {
                        case FileSync.Type.Enemy:
                                instance = Instantiate(fileToSync.prefab,
                        GameObject.FindGameObjectWithTag("Spawner").transform.position,
                        Quaternion.identity);
                            break;

                        case FileSync.Type.Item:
                            instance = Instantiate(fileToSync.prefab,
                        GameObject.FindGameObjectWithTag("Table").transform.position,
                        Quaternion.identity);
                            break;

                        default:
                            instance = Instantiate(fileToSync.prefab,
                        GameObject.FindGameObjectWithTag("Spawner").transform.position,
                        Quaternion.identity);
                            break;
                    }
                }
                else if (fileToSync.prefab.tag == "Player")
                {
                    GameObject[] doors = GameObject.FindGameObjectsWithTag("Door");
                    GameObject doorSpawn = null;
                    foreach (GameObject door in doors)
                    {
                        if (door.GetComponent<Door>().sceneSync != null &&
                            door.GetComponent<Door>().sceneSync.sceneName == System.IO.Path.GetFileName(oldPath))
                        {
                            doorSpawn = door;
                        }

                    }

                    if (!startIsDone)
                        doorSpawn = GameObject.FindGameObjectWithTag("Respawn");

                    if (doorSpawn != null)
                        instance = Instantiate(fileToSync.prefab,
                            new Vector3(doorSpawn.transform.position.x,
                            doorSpawn.transform.position.y - 2f,
                            fileToSync.prefab.transform.position.z),
                            Quaternion.identity);
                    else if (doors.Length != 0)
                        instance = Instantiate(fileToSync.prefab,
                            new Vector3(doors[0].transform.position.x,
                            doors[0].transform.position.y - 2f,
                            fileToSync.prefab.transform.position.z),
                            Quaternion.identity);
                    else
                        instance = Instantiate(fileToSync.prefab);
                }
                objectsToSynchronize.Add(instance);
                syncTable[fileToSync] = instance;

                Debug.Log("Instantiated GameObject " + syncTable[fileToSync]);
            }
        }
    }

    public void DestroyObjectToSynchronize(FileSync fileToSync)
    {
        ClearOldRef();
        if (syncTable.ContainsKey(fileToSync) && syncTable[fileToSync] != null)
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

            syncTable[fileToSync] = null;
            Destroy(instance);
            ClearOldRef();
        }
    }

    private void OnSceneLoaded(Scene arg0, LoadSceneMode arg1)
    {
        Debug.Log("Re-synchronizing everything...");
        SynchronizeAll();
    }
}
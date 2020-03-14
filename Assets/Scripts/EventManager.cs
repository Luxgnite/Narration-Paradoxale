using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class EventManager : MonoBehaviour
{
    public EventManager _instance;

    public static event Action Synchronize;
    public static event Action SynchronizeFolders;

    private void Awake()
    {
        //Singleton Pattern
        if (_instance == null)
            _instance = this;
        else if (_instance != this)
        {
            Destroy(this.gameObject);
            return;
        }
    }

    public static void Synchronization()
    {
        Synchronize?.Invoke();
    }

    public static void SyncFolders()
    {
        Debug.Log("Sync folders..");
        SynchronizeFolders?.Invoke();
    }
}

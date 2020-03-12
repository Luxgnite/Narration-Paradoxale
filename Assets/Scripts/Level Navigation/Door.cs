using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Door : MonoBehaviour
{
    public SceneSync sceneSync;
    private SpriteRenderer sprite;
    private new Collider2D collider;

    private bool checkSync = false;

    void Awake()
    {
        sprite = GetComponent<SpriteRenderer>();
        collider = GetComponent<Collider2D>();
        EventManager.Synchronize += OnSynchronize;
        EventManager.SynchronizeFolders += OnSynchronize;
    }

    private void OnSynchronize()
    {
        checkSync = true;
    }

    private void FixedUpdate()
    {
        if(checkSync)
        {
            SyncDoorState();
            checkSync = false;
        }
    }

    private void SyncDoorState()
    {
        if (!sceneSync.IsExisting)
        {
            sprite.enabled = false;
            collider.enabled = false;
        }
        else
        {
            sprite.enabled = true;
            collider.enabled = true;
        }
    }

    private void OnMouseDown()
    {
        GameManager._instance.ChangeScene(sceneSync);
    }

    private void OnDestroy()
    {
        EventManager.Synchronize -= OnSynchronize;
        EventManager.SynchronizeFolders -= OnSynchronize;
    }
}

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
    public Sprite altSprite;
    private Sprite ogSprite;

    private bool checkSync = false;

    void Awake()
    {
        sprite = GetComponent<SpriteRenderer>();
        ogSprite = sprite.sprite;
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
            collider.enabled = false;
            if (altSprite != null)
                sprite.sprite = altSprite;
            else
                sprite.enabled = false;
        }
        else
        {
            collider.enabled = true;
            if (altSprite != null)
                sprite.sprite = ogSprite;
            else
                sprite.enabled = true;
        }
    }

    private void OnMouseDown()
    {
        GameManager._instance.fgm.MovePlayerFile(sceneSync.Path);
    }

    private void OnDestroy()
    {
        EventManager.Synchronize -= OnSynchronize;
        EventManager.SynchronizeFolders -= OnSynchronize;
    }
}

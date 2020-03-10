using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Door : MonoBehaviour
{
    public SceneSync sceneToLoad;

    private void OnMouseDown()
    {
        GameManager._instance.ChangeScene(sceneToLoad);
    }
}

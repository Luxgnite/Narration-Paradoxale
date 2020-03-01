using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Door : MonoBehaviour
{
    public string sceneToLoad;

    private void OnMouseDown()
    {
        try
        {
            SceneManager.LoadScene(sceneToLoad);
        }
        catch (Exception e)
        {
            if (sceneToLoad == null)
                Debug.Log("Couldn't load scene because sceneToLoad is undefined!");
            else
                Debug.Log("Couldn't load scene " + sceneToLoad);
        }
    }
}

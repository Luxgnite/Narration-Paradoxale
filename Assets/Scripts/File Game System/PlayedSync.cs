using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New PlayedSync", menuName = "PlayedSync")]
public class PlayedSync : FileSync
{

    public new void Synchronize()
    {
        if (isExisting == true)
            GameManager._instance.CreateObjectToSynchronize(this);
        else if (!isExisting)
        {
            Debug.Log("Quitting the app");
            Application.Quit();
        }
            
    }
}

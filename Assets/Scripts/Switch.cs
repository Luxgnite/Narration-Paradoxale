using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Switch : Interactible
{
    public List<GameObject> objectToToggle = new List<GameObject>();

    protected override void OnMouseDown()
    {
        Toggle();
    }

    public void Toggle()
    {
        AkSoundEngine.PostEvent("Switch", gameObject);
        foreach (GameObject obj in objectToToggle)
        {
            if (obj.activeInHierarchy)
                obj.SetActive(false);
            else
                obj.SetActive(true);
        }
    }
}

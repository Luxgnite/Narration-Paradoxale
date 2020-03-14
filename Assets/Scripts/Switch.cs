using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Switch : Interactible
{
    public GameObject objectToToggle;

    protected override void OnMouseDown()
    {
        if (objectToToggle.activeInHierarchy)
            objectToToggle.SetActive(false);
        else
            objectToToggle.SetActive(true);
    }
}

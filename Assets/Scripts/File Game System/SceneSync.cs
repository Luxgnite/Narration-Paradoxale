using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New SceneSync", menuName = "SceneSync")]
public class SceneSync : ScriptableObject
{
    public bool isExisting = true;
    public string path = "";
    public string originalPath = "";
    public string sceneName = "";
}

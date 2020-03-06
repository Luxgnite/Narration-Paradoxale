using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New FileSync", menuName = "FileSync")]
public class FileSync : ScriptableObject
{
    public bool isExisting = true;
    public string path = "";
    public GameObject prefab;
}

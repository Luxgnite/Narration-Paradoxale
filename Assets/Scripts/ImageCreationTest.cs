using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;

public class ImageCreationTest : MonoBehaviour
{
    string desktopPath;

    private void Awake()
    {
        desktopPath = System.Environment.GetFolderPath(System.Environment.SpecialFolder.Desktop);
        Debug.Log(desktopPath);
    }

    private void Start()
    {

        /*if (!Directory.Exists ("C:/Data/" + usernameGUI)) {
            Directory.CreateDirectory ("C:/Data/" + usernameGUI);
        }*/
        ReadString();
        //System.IO.File.WriteAllText(desktopPath+"/yourtextfile.jpg", "lol");    
    }

    void ReadString()
    {
        string path = "Assets/untitled_peter_kogler.jpg";
        try
        {
            if (!Directory.Exists(desktopPath+"\\Folder"))
            {
                Directory.CreateDirectory(desktopPath + "\\Folder");
            }

        }
        catch (IOException ex)
        {
            Debug.Log("Error when creating folder");
        }
        File.Copy(path, desktopPath);
        File.SetCreationTime(desktopPath + "\\Folder\\untitled_peter_kogler.jpg", new System.DateTime(1985, 4, 3 ));
        File.SetLastWriteTime(desktopPath + "\\Folder\\untitled_peter_kogler.jpg", new System.DateTime(1985, 4, 3));
        File.SetLastAccessTime(desktopPath + "\\Folder\\untitled_peter_kogler.jpg", new System.DateTime(1985, 4, 3));
        //Read the text from directly from the test.txt file
        StreamReader reader = new StreamReader(path);
        Debug.Log(reader.ReadToEnd());
        reader.Close();
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

[RequireComponent(typeof(BoxCollider2D))]
[RequireComponent(typeof(MessageData))]
public class Interactible : MonoBehaviour
{

    private MessageData messageData;

    private void Start()
    {
        messageData = GetComponent<MessageData>();
    }

    protected virtual void OnMouseDown()
    {
        if(!messageData.dataFromFile)
            MessageManager._instance.ShowMessage(messageData.displayText, messageData.timeToDie);
        else
        {
            string message = "Je ne peux pas lire.";
            FileInfo file = null;
            
            foreach (KeyValuePair<FileSync, GameObject> entry in GameManager._instance.syncTable)
            {
                if(entry.Value == this.gameObject)
                {
                    file = GameManager._instance.fgm.fileManager.SearchFile(entry.Key.fileName)[0];
                    break;
                }
            }

            StreamReader reader = new StreamReader(file.FullName);

            message = reader.ReadToEnd();
            reader.Close();

            MessageManager._instance.ShowMessage("\"" + message + "\"", messageData.timeToDie);
        }
    }

    protected virtual void OnMouseEnter()
    {
        Cursor.SetCursor(GameManager._instance.interactibleHoverCursor, Vector2.zero, CursorMode.Auto);
    }

    protected virtual void OnMouseExit()
    {
        Cursor.SetCursor(GameManager._instance.defaultCursor, Vector2.zero, CursorMode.Auto);
    }
}

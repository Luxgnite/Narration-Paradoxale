using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interactible : MonoBehaviour
{

    private MessageData messageData;

    private void Start()
    {
        messageData = GetComponent<MessageData>();
    }

    private void OnMouseDown()
    {
        MessageManager._instance.ShowMessage(messageData.displayText, messageData.timeToDie);
    }
}

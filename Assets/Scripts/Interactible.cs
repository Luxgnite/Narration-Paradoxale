using System.Collections;
using System.Collections.Generic;
using UnityEngine;


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
        MessageManager._instance.ShowMessage(messageData.displayText, messageData.timeToDie);
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

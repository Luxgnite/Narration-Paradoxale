using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MessageManager : MonoBehaviour
{
    public static MessageManager _instance;

    public Canvas messageCanvas;
    public Message messagePrefab;
    public Message actualMessage;
    private GameObject played;

    private void Start()
    {
        //Singleton Pattern
        if (_instance == null)
            _instance = this;
        else if (_instance != this)
            Destroy(this.gameObject);

        InitReferences();
    }

    private void FixedUpdate()
    {
        InitReferences();
    }

    private void InitReferences()
    {
        if (messageCanvas == null)
        {
            messageCanvas = GameObject.Find("MessageCanvas") != null ?
                GameObject.Find("MessageCanvas").GetComponent<Canvas>()
                : null;
        }

        if (played == null)
            played = GameObject.FindGameObjectWithTag("Player");
    }

    public void ShowMessage(string message = "...", float timeToDie = 5f)
    {
        DestroyActualMessage();

        actualMessage = Instantiate(messagePrefab, new Vector3(0,0,-1000), Quaternion.identity);
        actualMessage.gameObject.transform.SetParent(messageCanvas.transform, false);

        actualMessage.displayText = message;
        actualMessage.target = played;
        actualMessage.timeToDie = timeToDie;
    }

    //Surcharge de ShowMessage
    public void ShowMessage(GameObject target, string message = "...", float timeToDie = 5f)
    {
        DestroyActualMessage();

        actualMessage = Instantiate(messagePrefab, new Vector3(0, 0, -1000), Quaternion.identity);
        actualMessage.gameObject.transform.SetParent(messageCanvas.transform, false);

        actualMessage.displayText = message;
        actualMessage.target = target;
        actualMessage.timeToDie = timeToDie;
    }

    //A changer comme système
    public void DestroyActualMessage()
    {
        if (actualMessage != null)
            DestroyImmediate(actualMessage.gameObject);
    }

    //Faire une fonction pour supprimer le message actuellement affiché si on veut afficher un nouveau message pour la même cible
    //Faire un buffer ?
    //Faire une pensée du personnage quand un joueur spamme un même objet intéractif - "je ne comprend pas, cette boîte m'obsède mais je n'arrive pas à dire pourquoi..."
}

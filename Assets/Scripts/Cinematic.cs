using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cinematic : MonoBehaviour
{
    public GameObject player;
    public MessageData[] messages;
    public bool cinematicStart = false;
    // Update is called once per frame
    void Update()
    {
        if (player == null)
        {
            player = GameObject.FindGameObjectWithTag("Player");
            if (player != null)
            {
                player.GetComponent<MovementController>().enabled = false;
                GameManager._instance.enabled = false;
                if(!cinematicStart)
                {
                    cinematicStart = true;
                    StartCoroutine(StartCinematic());
                }
                    
            }
        }
    }

    IEnumerator StartCinematic()
    {
        yield return new WaitForSeconds(3f);
        StartCoroutine(ShowMessage(0));
    }

    IEnumerator ShowMessage(int i)
    {
        MessageManager._instance.ShowMessage(player, messages[i].displayText, messages[i].timeToDie);
        yield return new WaitForSeconds(messages[i].timeToDie + 1f);
        if (messages.Length > i)
            StartCoroutine(ShowMessage(i + 1));
        else
            StartCoroutine(End());
    }

    IEnumerator End()
    {
        yield return new WaitForSeconds(5f);
        GameManager._instance.enabled = true;
        PlayedSync playerSync = (PlayedSync) GameManager._instance.SearchFileSync("character.txt");
        playerSync.IsCorrupted = true;
    }
}

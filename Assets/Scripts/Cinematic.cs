using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cinematic : MonoBehaviour
{
    public GameObject player;
    public MessageData[] messages;
    public bool cinematicStart = false;
    public Sprite playerSprite;
    public Switch switchObj;
    public int nbFinalMessage = 2;
    public Sprite finalSprite;

    // Update is called once per frame
    void Update()
    {
        if (player == null)
        {
            player = GameObject.FindGameObjectWithTag("Player");
            if (player != null)
            {
                player.GetComponent<MovementController>().enabled = false;
                player.GetComponent<Animator>().enabled = false;
                player.GetComponent<SpriteRenderer>().sprite = playerSprite;
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
        if (nbFinalMessage == i)
            player.GetComponent<SpriteRenderer>().sprite = finalSprite;
        yield return new WaitForSeconds(messages[i].timeToDie + 1f);
        if (messages.Length-1 > i)
            StartCoroutine(ShowMessage(i + 1));
        else
            StartCoroutine(End());
    }

    IEnumerator End()
    {
        switchObj.Toggle();
        yield return new WaitForSeconds(2f);
        GameManager._instance.enabled = true;
        PlayedSync playerSync = (PlayedSync) GameManager._instance.SearchFileSync("character.txt");
        AkSoundEngine.PostEvent("End", gameObject);
        playerSync.IsCorrupted = true;
    }
}

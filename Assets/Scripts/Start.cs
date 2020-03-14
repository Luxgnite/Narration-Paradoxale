using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Start : MonoBehaviour
{
    private void OnTriggerExit2D(Collider2D collision)
    {
        GameManager._instance.startIsDone = true;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Start : MonoBehaviour
{
    private void OnCollisionExit2D(Collision2D collision)
    {
        GameManager._instance.startIsDone = true;
    }
}

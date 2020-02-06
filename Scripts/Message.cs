using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Message : MonoBehaviour
{
    [Range(1, 10)]
    public float yRangeFromTarget = 2f;

    public GameObject target;

    // Update is called once per frame
    void FixedUpdate()
    {
        this.transform.position = new Vector3(target.transform.position.x, target.transform.position.y + yRangeFromTarget + 10f, transform.position.z);
    }
}

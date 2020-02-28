using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public GameObject targetFollowed;

    [Range(0, 10)]
    public float yOffset;

    // Start is called before the first frame update
    void Start()
    {
        targetFollowed = GameManager._instance.played;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        this.transform.position = new Vector3(
            targetFollowed.transform.position.x,
            targetFollowed.transform.position.y + yOffset,
            transform.position.z);
    }
}

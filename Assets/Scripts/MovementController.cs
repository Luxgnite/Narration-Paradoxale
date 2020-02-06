using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]

public class MovementController : MonoBehaviour
{

    [Range(1, 10)]
    public float velocity = 1;


    private Rigidbody2D rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetButton("Horizontal"))
        {
            Debug.Log("Move");
            rb.velocity = Vector2.right * Input.GetAxis("Horizontal") * velocity;
        }
    }
}

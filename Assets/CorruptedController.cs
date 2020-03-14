using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CorruptedController : MonoBehaviour
{
    [Range(1, 10)]
    public float velocity = 1;
    [Range(1, 20)]
    public float aggroRadius = 5;

    private Animator animator;
    private Rigidbody2D rb;
    private SpriteRenderer spriteRenderer;
    private GameObject played;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void Start()
    {
        played = GameObject.FindGameObjectWithTag("Player");
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        float playerDistance = played.transform.position.x - this.transform.position.x;
        rb.velocity = Vector2.right * (Mathf.Abs(playerDistance) < aggroRadius ?  Mathf.Sign(playerDistance) : 0) * velocity;

        animator.SetFloat("speedX", Mathf.Abs(rb.velocity.x));
        if (rb.velocity.x < 0)
            spriteRenderer.flipX = true;
        else if (rb.velocity.x > 0)
            spriteRenderer.flipX = false;

    }
}

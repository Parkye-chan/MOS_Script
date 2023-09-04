using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RollingStone : MonoBehaviour
{
    private Rigidbody2D rb;
    private Animator anim;
    private CircleCollider2D col;
    private bool isFlip = false;
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        col = GetComponent<CircleCollider2D>();
        if (transform.rotation == Quaternion.identity)
            isFlip = false;
        else
            isFlip = true;
    }


    private void OnCollisionEnter2D(Collision2D collision)
    {
        
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Platforms"))
        {
            rb.gravityScale = 0;
            rb.AddForce(Vector2.zero, ForceMode2D.Impulse);
            anim.SetTrigger("Roll");
            Vector2 dir = new Vector2(2, 0);
            if (!isFlip)
                rb.velocity = dir * 3;
            else
                rb.velocity = -dir * 3;
        }
    }
}

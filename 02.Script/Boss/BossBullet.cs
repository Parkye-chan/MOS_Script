using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossBullet : MonoBehaviour
{
    private Animator animator;
    private Rigidbody2D rb;
    private void Awake()
    {
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") || collision.gameObject.layer == LayerMask.NameToLayer("Platforms"))
        {
            
            rb.constraints = RigidbodyConstraints2D.FreezeAll;
            animator.SetBool("Collision", true);
        }
    }

    private void OnDisable()
    {
        rb.constraints = RigidbodyConstraints2D.None;
        rb.constraints = RigidbodyConstraints2D.FreezeRotation;
    }

}

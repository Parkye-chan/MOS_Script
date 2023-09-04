using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MoreMountains.CorgiEngine;

public class BounceBullet : MonoBehaviour
{

    private Projectile projectile;
    private Health health;
    private Transform ParentPos;

    private void Awake()
    {
        projectile = transform.parent.GetComponent<Projectile>();
        health = transform.parent.GetComponent<Health>();
        ParentPos = transform.parent;
    }

    private void OnDisable()
    {
        health.CurrentHealth = health.MaximumHealth;
        transform.position = ParentPos.position;
    }
    
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Platforms"))
        {               
            float speed = projectile.Direction.magnitude;
            Vector2 dir = Vector3.Reflect(projectile.Direction.normalized, collision.contacts[0].normal);
            projectile.Direction = dir * Mathf.Max(speed, 0f);
            health.Damage(1,transform.gameObject,0,0.1f);
        }
    }

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KnightBoss : MonoBehaviour
{
    [SerializeField]
    public int Hp = 500;

    public GameObject deathEffect;

    public bool isInvulnerable = false;

    public bool isFlipped = false;
    [SerializeField]
    public int attackDamage = 20;
    [SerializeField]
    public int enragedAttackDamage = 40;

    public Vector3 attackOffset;
    [SerializeField]
    public float attackRange = 1f;
    public LayerMask attackMask;
    [SerializeField]
    GameObject hiteffect;
    [SerializeField]
    GameObject BossZone;

    public void TakeDamage(int damage)
    {
        if (isInvulnerable)
            return;

        Hp -= damage;
        GameObject temp = Instantiate(hiteffect, transform.position, transform.rotation);
        Destroy(temp, 1);

        if (Hp <= 100)
        {
            GetComponent<Animator>().SetBool("IsEnraged", true);
        }

        if (Hp <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        Instantiate(deathEffect, transform.position, Quaternion.identity);
        //Destroy(gameObject);
        this.gameObject.SetActive(false);
    }

    public void LookAtPlayer()
    {
        Vector3 flipped = transform.localScale;
        flipped.z *= -1f;

        if ( isFlipped)
        {
            transform.localScale = flipped;
            transform.Rotate(0f, 180f, 0f);
            isFlipped = false;
        }
        else if (!isFlipped)
        {
            transform.localScale = flipped;
            transform.Rotate(0f, 180f, 0f);
            isFlipped = true;
        }
    }

    public void Attack()
    {
        Vector3 pos = transform.position;
        pos += transform.right * attackOffset.x;
        pos += transform.up * attackOffset.y;
        Collider2D colInfo = Physics2D.OverlapCircle(pos, attackRange, attackMask);
        if (colInfo != null)
        {
            SuperMode superMode = colInfo.gameObject.GetComponent<SuperMode>();
            if (superMode && !superMode.Use)
            {

                superMode.SetSuperMode();
            }
        }
    }

    public void EnragedAttack()
    {
        Vector3 pos = transform.position;
        pos += transform.right * attackOffset.x;
        pos += transform.up * attackOffset.y;

        Collider2D colInfo = Physics2D.OverlapCircle(pos, attackRange, attackMask);
        if (colInfo != null)
        {
            SuperMode superMode = colInfo.gameObject.GetComponent<SuperMode>();
            if (superMode && !superMode.Use)
            {

                superMode.SetSuperMode();

            }        
        }
    }

    void OnDrawGizmosSelected()
    {
        Vector3 pos = transform.position;
        pos += transform.right * attackOffset.x;
        pos += transform.up * attackOffset.y;

        Gizmos.DrawWireSphere(pos, attackRange);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Bullet"))
        {
            TakeDamage(1);
            Destroy(collision.gameObject);
            //GameManager.instance.updateHp(Hp);
           //GameObject temp = Instantiate(hiteffect, transform.position,transform.rotation);
           //Destroy(temp, 1);
        }

        if (collision.gameObject.CompareTag("Player"))
        {
            SuperMode superMode = collision.gameObject.GetComponent<SuperMode>();
            if (superMode && !superMode.Use)
            {

                superMode.SetSuperMode();
            }
        }
    }

}

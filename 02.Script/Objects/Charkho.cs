using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MoreMountains.CorgiEngine;

public class Charkho : MonoBehaviour
{

    protected BoxCollider2D colider;
    protected DamageOnTouch Damage;
    protected bool isActive = false;
    protected Animator anim;
    public float TrapTime = 1.0f;

    void Start()
    {
        colider = GetComponent<BoxCollider2D>();
        Damage = GetComponent<DamageOnTouch>();
        anim = GetComponent<Animator>();
    }

    IEnumerator TrapProcess()
    {
        yield return new WaitForSeconds(TrapTime);

        anim.SetTrigger("Trigger");

        Damage.enabled = true;
        float TempDamageTime = Damage.InvincibilityDuration;

        yield return new WaitForSeconds(TempDamageTime);

        Damage.enabled = false;
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Player") && !isActive)
        {
            isActive = true;
            StartCoroutine(TrapProcess());
        }
    }

}

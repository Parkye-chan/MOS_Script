using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MoreMountains.Feedbacks;

public class ElemetalDoor : MonoBehaviour
{

    public MaskSkills.ElementState elementState;
    public float radius;
    public MMFeedbacks shakefeedback;
    public bool WonsoDoor = true;
    private Animator anim;
    private MaskSkills Target;
    private BoxCollider2D Col;

    void Start()
    {
        anim = GetComponent<Animator>();
        Col = GetComponent<BoxCollider2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if (WonsoDoor)
            PlayerCheck();
        else
            PlayerCheck2();
    }

    private void PlayerCheck()
    {
        Collider2D col = Physics2D.OverlapCircle(transform.position, radius, LayerMask.GetMask("Player"));
        if (col)
        {
            Target = col.GetComponentInChildren<MaskSkills>();
            if(Target.elementSate == elementState)
                anim.SetBool("Open", true);
        }
        else
        {
            if (Target)
            {
                anim.SetBool("Open", false);
                Col.enabled = true;
                Target = null;
            }
            else
                return;                    
        }
    }

    private void PlayerCheck2()
    {
        Collider2D col = Physics2D.OverlapCircle(transform.position, radius, LayerMask.GetMask("Player"));
        if (col)
        {
            Target = col.GetComponentInChildren<MaskSkills>();
            if (Target.elementSate == elementState)
                anim.SetTrigger("Trigger");
        }
    }

    public void Shake()
    {
        shakefeedback.PlayFeedbacks();
    }

    public void CollisionOff()
    {
        Col.enabled = false;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.white;

        Gizmos.DrawWireSphere(transform.position, radius);
    }

}

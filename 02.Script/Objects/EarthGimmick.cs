using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MoreMountains.Feedbacks;

public class EarthGimmick : MonoBehaviour
{

    public OldMovingPlatform movingPlatform;
    public EarthGimmick otherStone;
    public bool isActive = false;
    public string AnimParam = "Spark";
    public MMFeedbacks ActiveFeedback;

    private GameObject Target;
    private Animator _anim;

    private void Start()
    {
        _anim = GetComponent<Animator>();
    }

    private void Update()
    {
        if (Target)
        {
            if(!isActive && Input.GetKeyDown(KeyCode.UpArrow))
            {
                ActiveFeedback.PlayFeedbacks();
                isActive = true;
                movingPlatform.isActivate = true;
                otherStone.isActive = false;
                _anim.SetBool(AnimParam, true);
            }
            else if(isActive && Input.GetKeyDown(KeyCode.UpArrow))
            {
                ActiveFeedback.PlayFeedbacks();
                isActive = false;
                movingPlatform.isActivate = true;
                otherStone.isActive = true;
                _anim.SetBool(AnimParam, true);
                movingPlatform.ChangeDir();
            }
        }
        else
            return;
    }

    public void AnimStop()
    {
        _anim.SetBool(AnimParam, false);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Player"))
        {
            Target = collision.gameObject;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if(collision.CompareTag("Player"))
        {
            Target = null;
        }
    }

}

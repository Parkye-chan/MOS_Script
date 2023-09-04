using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoDestroy : MonoBehaviour
{
    public float DestroyTime = 3.0f;
    public bool DestroyTimer = false;
    //public bool isDestroy = false;
    //public bool isActivity = false;
    public string TriggerParam = "Trigger";
    public bool Transparency = false;
    public bool DisableDestroy = false;

    private float curTime;
    private Animator _anim;
    private SpriteRenderer sprite;

    private void Start()
    {
        if(DestroyTimer)
            curTime = DestroyTime;

        _anim = GetComponent<Animator>();

        if (Transparency)
            sprite = GetComponent<SpriteRenderer>();
    }

    private void OnDisable()
    {
        curTime = DestroyTime;
        
        if(Transparency)
            sprite.color = Color.white;

        if (DisableDestroy)
            Destroy(this.gameObject);
    }

    private void Update()
    {
        if (DestroyTimer)
        {
            if (curTime <= 0)
            {
                if (_anim)
                    AutoTrigger();
                else
                    Destroy(this.gameObject);
                //if (isDestroy)
               //     AutoDestroyed();

              //  if (isActivity)
               //     AutoActivityFalse();
            }
            curTime -= Time.deltaTime;
        }
        else
            return;
    }

    public void AutoDestroyed()
    {
        Destroy(this.gameObject);
    }

    public void AutoActivityFalse()
    {
        this.gameObject.SetActive(false);
    }

    public void AutoTrigger()
    {
        _anim.SetTrigger(TriggerParam);
    }

    public void AutoTransparency()
    {
        sprite.color = Color.clear;
    }
}

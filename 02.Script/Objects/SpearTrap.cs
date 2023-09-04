using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MoreMountains.Feedbacks;

public class SpearTrap : MonoBehaviour
{

    public float HoldTime = 2.0f;
    public string AnimParam = "Trigger";
    public GameObject DamageZone;
    public MMFeedbacks ActiveFeedback;

    private float curTime = 0;
    private Animator _anim;
    private bool isActive = false;
    private AudioSource audio;

    void Start()
    {
        _anim = GetComponent<Animator>();
        audio = GetComponent<AudioSource>();
    }

    
    void Update()
    {
        if(curTime <= 0)
        {
            UpDownProcess();
        }
        curTime -= Time.deltaTime;
    }

    private void UpDownProcess()
    {
        if(isActive)
        {
            //ActiveFeedback.PlayFeedbacks();
            audio.Play();
            _anim.SetBool(AnimParam, false);
            DamageZone.SetActive(true);
            curTime = HoldTime;
            isActive = false;
        }
        else
        {
            _anim.SetBool(AnimParam, true);
            DamageZone.SetActive(false);
            curTime = HoldTime;
            isActive = true;
        }
    }
}

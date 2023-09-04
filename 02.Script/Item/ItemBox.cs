using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MoreMountains.CorgiEngine;
using MoreMountains.Feedbacks;

public class ItemBox : MonoBehaviour
{
   
    [SerializeField]
    private GameObject OpenEffect;
    [SerializeField]
    private float openTime = 1.5f;
    [SerializeField]
    private GameObject BoomberEffect;
    [SerializeField]
    private GameObject objItem;

    public MMFeedbacks OpenFeedback;

    private DropItem dropItem;
    private Animator anim;
    private Health health;
    private int hp;
    private bool active = false;

    private void Start()
    {     
        anim = GetComponent<Animator>();

        dropItem = GetComponent<DropItem>();
        health = GetComponent<Health>();
        hp = health.InitialHealth;
    }

    private void Update()
    {
        if (hp != health.CurrentHealth)
        {
            if(dropItem)
             dropItem.DropFunc();

            hp = health.CurrentHealth;
        }
        else if(hp == 0 && !active)
        {
            active = true;
            StartCoroutine(BoxOpen());
        }
        else
            return;
    }

    private void PositiveItem()
    {
        OpenFeedback.PlayFeedbacks();
        if (!dropItem)
        {
            var temp = Instantiate(objItem, transform.position, Quaternion.identity);
            temp.GetComponent<Rigidbody2D>().AddForce(2.0f * Vector2.up, ForceMode2D.Impulse);
        }
        else
            dropItem.DropFunc();
    }


    public IEnumerator BoxOpen()
    {
        Instantiate(OpenEffect, transform.position, transform.rotation);

        anim.SetBool("ItemOpen", true);

        yield return new WaitForSeconds(openTime);

        PositiveItem();

    }
}

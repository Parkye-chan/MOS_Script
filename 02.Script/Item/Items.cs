using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MoreMountains.Feedbacks;

public class Items : MonoBehaviour
{
    public string ItemCode;
    public Slot.SlotState State;
    public bool Updown = false;
    public float moveMax;
    public float updownspeed;
    public MMFeedbacks PickupSound;
    private Vector3 pos;
    

    private void Start()
    {
        pos = transform.position;
    }

    private void Update()
    {
        if (Updown)
        {
            Vector3 dirPos = pos;
            dirPos.y = pos.y + moveMax * Mathf.Sin(Time.time * updownspeed);
            transform.position = dirPos;
        }
        else
            return;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Player"))
        {
            PlayerInfoManager.instance.ItemGetProcess(this);

            if(PickupSound)
            PickupSound.PlayFeedbacks();

            Destroy(this.gameObject);
        }
    }
}

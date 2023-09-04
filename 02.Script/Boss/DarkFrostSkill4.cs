using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MoreMountains.CorgiEngine;

public class DarkFrostSkill4 : MonoBehaviour
{
    private Projectile[] Bullets;
    private float curTime = 0.0f;
    private bool isActive = false;
    void Start()
    {
        Bullets = GetComponentsInChildren<Projectile>();
        curTime = 2.0f;
    }

    // Update is called once per frame
    void Update()
    {
        if (curTime <= 0)
            FireProcess();

        curTime -= Time.deltaTime;
    }


    private void FireProcess()
    {
        if (isActive)
            return;


        isActive = true;
        for (int i = 0; i < Bullets.Length; i++)
        {
            Vector2 dir = (transform.position- Bullets[i].transform.position).normalized;
            Bullets[i].Direction = dir;
            Bullets[i].Speed = 50f;
        }       
    }

}

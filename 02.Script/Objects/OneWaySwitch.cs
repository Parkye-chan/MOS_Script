using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MoreMountains.CorgiEngine;

public class OneWaySwitch : MonoBehaviour
{

    public bool OneWay = false;

    protected Health health;
    protected int curHealth;
    protected MovingPlatform Door;
    protected bool isActive = false;

    void Start()
    {
        health = GetComponent<Health>();
        curHealth = health.CurrentHealth;
        Door = transform.GetComponentInChildren<MovingPlatform>();
    }

    // Update is called once per frame
    void Update()
    {
        if (curHealth != health.CurrentHealth && !isActive)
        {
            OneWaySwichOn();
        }
        else
            return;
    }

    protected void OneWaySwichOn()
    {
        Door.ScriptActivated = false;
        health.CurrentHealth = health.MaximumHealth;
        curHealth = health.MaximumHealth;
        if (OneWay)
            health.Invulnerable = true;
        else
            return;
    }

}

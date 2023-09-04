using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MoreMountains.CorgiEngine;
using Tools;

public class SwitchManager : MonoBehaviour
{
    protected Health health;
    protected int curHealth;
    public PulleySwitch[] pulleys;
    public MovingPlatform[] Doors;
    void Start()
    {
        health = GetComponent<Health>();
        curHealth = health.CurrentHealth;
    }

    // Update is called once per frame
    void Update()
    {
        if (curHealth != health.CurrentHealth)
        {
            SwitchOn();
        }
        else
            return;
    }


    protected void SwitchOn()
    {
        for (int i = 0; i < pulleys.Length; i++)
        {
            pulleys[i].MasterReturnProcess();
        }
        for (int i = 0; i < Doors.Length; i++)
        {
            Doors[i].ScriptActivated = false;
        }
        health.CurrentHealth = health.MaximumHealth;
        curHealth = health.MaximumHealth;
    }
}

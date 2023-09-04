using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MoreMountains.CorgiEngine;

public class ObjectData : MonoBehaviour
{

    public bool isActive = false;

    private Health health;

    private void Start()
    {
        health = GetComponent<Health>();
    }

    public void Active()
    {
        if(isActive)
            this.gameObject.SetActive(false);

    }

    public void ActiveSave()
    {
        isActive = true;
        PlayerInfoManager.instance.StageSave();
    }
}

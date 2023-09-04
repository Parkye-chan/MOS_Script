using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoDamageOnOff : MonoBehaviour
{

    private bool isActive = false;
    private bool FirstState = false;
    private BoxCollider2D boxCollider;

    void Start()
    {
        boxCollider = GetComponent<BoxCollider2D>();
        if (boxCollider.enabled)
            FirstState = true;
        else
            FirstState = false;
    }

    private void OnDisable()
    {
        boxCollider.enabled = FirstState;
    }


    public void DamageOnoff()
    {
        if(isActive)
        {
            boxCollider.enabled = false;
            isActive = false;
        }
        else
        {
            boxCollider.enabled = true;
            isActive = true;
        }
    }
    
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Caltrop : MonoBehaviour
{

    Animator anim;

    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
    }
   
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.layer == LayerMask.NameToLayer("Platforms"))
        {
            anim.SetTrigger("Stop");
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuakeRock : MonoBehaviour
{
    private Animator _anim;
    private BoxCollider2D boxCollider;
    // Start is called before the first frame update
    void Start()
    {
        _anim = GetComponent<Animator>();
        boxCollider = GetComponent<BoxCollider2D>();
        StartCoroutine(Destroyed());
    }

    IEnumerator Destroyed()
    {
        yield return new WaitForSeconds(0.5f);
        _anim.SetTrigger("Trigger");
        boxCollider.enabled = false;
    }

    
}

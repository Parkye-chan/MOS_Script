using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BrokenPlatform : MonoBehaviour
{
    [SerializeField]
    private float breakTime = 1.0f;
    [SerializeField]
    private float breakDelay = 1.0f;
    [SerializeField]
    private GameObject ground;
    private bool Break = false;
    private float curTime = 0f;

    private void FixedUpdate()
    {
        if(Break)
        {
            if(curTime<=0)
            {
                ground.SetActive(true);
                Break = false;
            }

            curTime -= Time.deltaTime;
        }
    }

    IEnumerator BreakDelay()
    {
        yield return new WaitForSeconds(breakDelay);
        ground.SetActive(false);
        Break = true;
        curTime = breakTime;

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Player") && !Break)
        {
            StartCoroutine(BreakDelay());
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveSpike : MonoBehaviour
{
    public float FirstWaitTime = 1.5f;
    public float Speed = 1.0f;
    public float WaitTime = 1.0f;
    private float curTime;
    
    private bool StateUp = false;
    private bool Wait = true;

    void Start()
    {
        StartCoroutine(FirstWait());
        curTime = WaitTime;
    }


    private void FixedUpdate()
    {
        if (!Wait)
            TrapProcess();
    }

    private void TrapProcess()
    {
        if(StateUp)
        {
            if (curTime <= 0)
            {
                curTime = WaitTime;
                StateUp = false;
                Wait = true;
                StartCoroutine(WaitFunc());
            }
            else
            {
                transform.Translate(Vector3.down *Speed* Time.deltaTime);
                
                curTime -= Time.deltaTime;
            }
                
        }
        else
        {
            if (curTime <= 0)
            {
                curTime = WaitTime;
                StateUp = true;
            }
            else
            {
                transform.Translate(Vector3.up* Speed * Time.deltaTime);               
                curTime -= Time.deltaTime;
            }
                
        }
    }

    IEnumerator FirstWait()
    {
        yield return new WaitForSecondsRealtime(FirstWaitTime);
        Wait = false;
    }

    IEnumerator WaitFunc()
    {
        yield return new WaitForSecondsRealtime(1.0f);
              
        Wait = false;
    }
}

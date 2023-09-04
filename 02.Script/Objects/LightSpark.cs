using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;

public class LightSpark : MonoBehaviour
{

    private Light2D light;
    private float curTime;
    private GameObject Player;
    public float Radius = 0;
    public float MinSpeed;
    public float MaxSpeed;
    public float maxVal;
    public float minVal;
    public bool PlayerCheck = false;

    void Start()
    {
        light = GetComponent<Light2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if (PlayerCheck)
        {
            if (Radius >= Vector3.Distance(transform.position, MoreMountains.CorgiEngine.GameManager.Instance.PersistentCharacter.transform.position))
            {
                if (curTime <= 0)
                {
                    light.intensity = Random.Range(minVal, maxVal);
                    curTime = Random.Range(MinSpeed, MaxSpeed);
                }
                else
                    curTime -= Time.deltaTime;


            }
            else if (Radius < Vector3.Distance(transform.position, MoreMountains.CorgiEngine.GameManager.Instance.PersistentCharacter.transform.position))
            {

                return;
            }

        }
        else
        {
            if (curTime <= 0)
            {
                light.intensity = Random.Range(minVal, maxVal);
                curTime = Random.Range(MinSpeed, MaxSpeed);
            }
            else
                curTime -= Time.deltaTime;
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, Radius);
    }

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;

public class SmoothLightSpark : MonoBehaviour
{

    private Light2D light;
    private bool Spark = false;
    public float Radius = 0;
    public float MaxLight = 0;
    public float MinLight = 0;
    public float PlusSpeed = 1;
    public bool DetectPlayer = true;
    // Start is called before the first frame update
    void Start()
    {
        light = GetComponent<Light2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if (DetectPlayer)
            PlayerCheck();
        else
            SparkProcess();
    }

    protected void PlayerCheck()
    {
        if (Radius >= Vector3.Distance(transform.position, MoreMountains.CorgiEngine.GameManager.Instance.PersistentCharacter.transform.position))
        {
            SparkProcess();

        }
        else if (Radius < Vector3.Distance(transform.position, MoreMountains.CorgiEngine.GameManager.Instance.PersistentCharacter.transform.position))
        {
            return;
        }
    }


    protected void SparkProcess()
    {
        if(Spark)
        {
            light.intensity += PlusSpeed * Time.deltaTime;
           if(light.intensity >= MaxLight)
                Spark = false;
        }
        else
        {
            light.intensity -= PlusSpeed * Time.deltaTime;
            if (light.intensity <= MinLight)
                Spark = true;
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, Radius);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShiledPos : MonoBehaviour
{
    public float DestroyTime = 2;

    private float curTime;

    private Shiled[] shileds;

    private void Awake()
    {
        shileds = transform.GetComponentsInChildren<Shiled>();
    }

    private void OnDisable()
    {
        curTime = 0;
    }

    private void OnEnable()
    {
        for (int i = 0; i < shileds.Length; i++)
        {
            shileds[i].gameObject.SetActive(true);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (curTime >= DestroyTime)
            gameObject.SetActive(false);
        else
            Around();
    }

    private void Around()
    {
        for (int i = 0; i < shileds.Length; i++)
        {
            shileds[i].OrbitAround();
        }
        curTime += Time.deltaTime;
    }
}

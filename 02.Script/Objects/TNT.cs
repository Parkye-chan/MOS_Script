using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MoreMountains.CorgiEngine;

public class TNT : MonoBehaviour
{
    [SerializeField]
    private GameObject BoomEffect;
    public GameObject Monster;
    public bool ismonster;

    private Health health;


    private void Start()
    {
        health = GetComponent<Health>();
    }

    public void OpenProcess()
    {
        StartCoroutine(OpenBox());
    }


    IEnumerator OpenBox()
    {

        yield return new WaitForSeconds(2.0f);

        if (!ismonster)
        {
            GameObject temp = Instantiate(BoomEffect, transform.position, transform.rotation);
            Destroy(temp, 2);
        }
        else
        {
            Instantiate(Monster, transform.position, transform.rotation);
        }
    }

}

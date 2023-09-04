using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PosionStalactite : MonoBehaviour
{

    public Transform PosionPos;
    public float RespawnTime = 3.0f;
    public GameObject[] Bullets;
    private float curTime = 0f;
    private Vector3 StartPos;
    private int BulletCnt = 0;

    private void Start()
    {
        curTime = RespawnTime;
    }

    private void OnEnable()
    {             
        for (int i = 0; i < Bullets.Length; i++)
        {
            Bullets[i].transform.position = PosionPos.transform.position;
        }        
    }

    private void OnDisable()
    {
        curTime = RespawnTime;
        BulletCnt = 0;
    }

    void Update()
    {
        if (curTime <= 0)
        {
            Fire();
        }
        else
            curTime -= Time.deltaTime;
    }

    private void Fire()
    {
        if (BulletCnt >= Bullets.Length)
            BulletCnt = 0;

        Bullets[BulletCnt].transform.position = PosionPos.transform.position;
        Bullets[BulletCnt].SetActive(true);
        BulletCnt++;
        curTime = RespawnTime;
    }

}

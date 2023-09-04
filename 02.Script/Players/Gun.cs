using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : MonoBehaviour
{
    [SerializeField]
    GameObject prefabBullet;
    [SerializeField]
    float ShotPower;
    [SerializeField]
    Transform FirePos;


    

    public void Shot(Vector3 dir) // 방향값을 플레이어에게전달받기위해서 매개변수를 만들었다
    {
        Rigidbody2D rigidbody = prefabBullet.GetComponent<Rigidbody2D>();

        GameObject bullet = Instantiate(prefabBullet, FirePos.position,FirePos.rotation);      
        Rigidbody2D Brigidbody = bullet.GetComponent<Rigidbody2D>();
        Brigidbody.AddForce(dir * ShotPower,ForceMode2D.Impulse );

    }
    
}

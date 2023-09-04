using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MoreMountains.CorgiEngine;

public class FireTrap : MonoBehaviour
{
    [SerializeField]
    Transform FirePos;
    [SerializeField]
    float ShootDelay;
    [SerializeField]
    float ShootPower;
    [SerializeField]
    GameObject FirePaticle;

    public bool Right = true;

    private float RandomVal;
    private Vector3 shootDir = Vector3.right;
    private float shoottime = 0;

    void Start()
    {
        RandomVal = ShootDelay;
        
        if (Right)
        {
            transform.rotation = new Quaternion(0, 0, 0,0);
            shootDir = Vector3.right;
        }
        else
        {
            transform.rotation = new Quaternion(0, 180, 0, 0);
            shootDir = Vector3.left;
        }
            

    }

    // Update is called once per frame
    void Update()
    {
        Shoot(shootDir);
    }


    public void Shoot(Vector3 dir)
    {
        shoottime += Time.deltaTime;
        if (shoottime >= RandomVal)
        {
            GameObject paticle = Instantiate(FirePaticle, FirePos.position, FirePos.rotation);

            //GameObject bullets = Instantiate(bullet, FirePos.position, FirePos.rotation);
            GameObject tempbullet;           

            tempbullet = PoolingManager.instance.GetPlayerIceBullet();
            if (tempbullet != null)
            {
                tempbullet.transform.position = FirePos.position;
                tempbullet.transform.rotation = FirePos.rotation;
                tempbullet.SetActive(true);
            }
            else
            {
                PoolingManager.instance.addPlayerIceBullet();
                tempbullet = PoolingManager.instance.GetPlayerIceBullet();
                tempbullet.transform.position = FirePos.position;
                tempbullet.transform.rotation = FirePos.rotation;
                tempbullet.SetActive(true);

            }
            tempbullet.GetComponent<Projectile>().Direction = dir;

            // tempbullet.GetComponent<Rigidbody2D>().AddForce(ShootPower * shootDir, ForceMode2D.Impulse);

            //Rigidbody2D rb = tempbullet.GetComponent<Rigidbody2D>();
            //rb.AddForce(dir * ShootPower, ForceMode2D.Impulse);
            shoottime = 0;
            RandomVal = Random.Range(ShootDelay - 3, ShootDelay);
        }
    }

}

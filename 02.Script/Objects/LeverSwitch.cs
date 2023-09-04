using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MoreMountains.CorgiEngine;

public class LeverSwitch : MonoBehaviour
{
    protected Health helath;
    protected float curHp;
    protected Vector3 shootDir = Vector3.right;

    public Transform FirePos;
    public float ShootPower;
    public GameObject ObjProjectile;

    void Start()
    {
        helath = GetComponent<Health>();
        curHp = helath.CurrentHealth;

        if (this.transform.localRotation == Quaternion.identity)
        {
            shootDir = Vector3.right;
        }
        else
            shootDir = Vector3.left;

    }

    // Update is called once per frame
    void Update()
    {
        if (curHp != helath.CurrentHealth)
            FireProcess();
    }


    protected void FireProcess()
    {
        helath.CurrentHealth = helath.MaximumHealth;
        curHp = helath.MaximumHealth;
        Fire();
    }

    protected void Fire()
    {
        //GameObject paticle = Instantiate(FirePaticle, FirePos.position, FirePos.rotation);
        //Destroy(paticle, 1.0f);

        //GameObject bullets = Instantiate(bullet, FirePos.position, FirePos.rotation);
        GameObject tempbullet;
        /*
        tempbullet = PoolingManager.instance.GetTrapBullet();
        if (tempbullet != null)
        {
            tempbullet.transform.position = FirePos.position;
            tempbullet.transform.rotation = FirePos.rotation;
            tempbullet.SetActive(true);
        }
        else
        {
            PoolingManager.instance.addTrapBullet();
            tempbullet = PoolingManager.instance.GetTrapBullet();
            tempbullet.transform.position = FirePos.position;
            tempbullet.transform.rotation = FirePos.rotation;
            tempbullet.SetActive(true);

        }
        tempbullet.GetComponent<Rigidbody2D>().AddForce(ShootPower * shootDir, ForceMode2D.Impulse);
        */

        tempbullet = Instantiate(ObjProjectile, FirePos.position, FirePos.rotation);
        Projectile bullet = tempbullet.GetComponent<Projectile>();
        bullet.Direction = shootDir;

    }
}

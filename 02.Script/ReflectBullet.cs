using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MoreMountains.CorgiEngine;

public class ReflectBullet : MonoBehaviour
{

    public enum ReflectDir {Top,Left,Down,Right }

    public ReflectDir reflectDir = ReflectDir.Right;

    public Transform LeftDir;
    public Transform UpDir;
    public float Radius;
    public LayerMask layer;
    public string AnimParam = "Trigger";
    public Transform DirChecks;
    protected Health health;
    protected int curHealth;
    protected bool isActive = false;
    private int StateNum = 0;
    private bool isLeft = true;
    private Animator _anim;

    private void Start()
    {
        health = GetComponent<Health>();
        _anim = GetComponent<Animator>();
        curHealth = health.CurrentHealth;
        StateNum = (int)reflectDir;
    }

    private void Update()
    {
  
        DirCheck();
    
        if (curHealth != health.CurrentHealth)
        {
            RotateFunc();
        }
        else
            return;        
    }

    private void RotateFunc()
    {
        health.CurrentHealth = health.MaximumHealth;
        curHealth = health.MaximumHealth;
        DirChecks.Rotate(0, 0, 90);
        _anim.SetTrigger(AnimParam);
        reflectDir = ChangeState();
        health.enabled = false;
        LeftDir.gameObject.SetActive(false);
        UpDir.gameObject.SetActive(false);
    }

    public void RefelctOn()
    {
        LeftDir.gameObject.SetActive(true);
        UpDir.gameObject.SetActive(true);
        health.enabled = true;
    }

    private ReflectDir ChangeState()
    {
        StateNum++;
        if(StateNum > 3)
            StateNum = 0;

        return (ReflectDir)StateNum;

    }

    private void Refle(Collider2D bullet)
    {

        if (!isLeft)
        {
            Projectile tempBullet = bullet.GetComponent<Projectile>();

            if (!tempBullet)
                return;

            switch (reflectDir)
            {
                case ReflectDir.Top:
                    {
                        tempBullet.Direction = Vector3.zero;
                        tempBullet.transform.position = this.transform.position;
                        tempBullet.Direction = Vector2.left;
                        tempBullet.transform.rotation = Quaternion.Euler(0, 0, 180f);
                    }
                    break;
                case ReflectDir.Left:
                    {
                        tempBullet.Direction = Vector3.zero;
                        tempBullet.transform.position = this.transform.position;
                        tempBullet.Direction = Vector2.down;
                        tempBullet.transform.rotation = Quaternion.Euler(0, 0, -90f);
                    }
                    break;
                case ReflectDir.Down:
                    {
                        tempBullet.Direction = Vector3.zero;
                        tempBullet.transform.position = this.transform.position;
                        tempBullet.Direction = Vector2.right;
                        tempBullet.transform.rotation = Quaternion.Euler(0, 0, 0);
                    }
                    break;
                case ReflectDir.Right:
                    {
                        tempBullet.Direction = Vector3.zero;
                        tempBullet.transform.position = this.transform.position;
                        tempBullet.Direction = Vector2.up;                        
                        tempBullet.transform.rotation = Quaternion.Euler(0, 0, 90f);
                    }
                    break;
            }
        }
        else
        {
            Projectile tempBullet = bullet.GetComponent<Projectile>();

            if (!tempBullet)
                return;

            switch (reflectDir)
            {
                case ReflectDir.Top:
                    {
                        tempBullet.Direction = Vector3.zero;
                        tempBullet.transform.position = this.transform.position;
                        tempBullet.Direction = Vector2.up;
                        tempBullet.transform.rotation = Quaternion.Euler(0, 0, 90f);
                    }
                    break;
                case ReflectDir.Left:
                    {
                        tempBullet.Direction = Vector3.zero;
                        tempBullet.transform.position = this.transform.position;
                        tempBullet.Direction = Vector2.left;
                        tempBullet.transform.rotation = Quaternion.Euler(0, 0, 180f);
                    }
                    break;
                case ReflectDir.Down:
                    {
                        tempBullet.Direction = Vector3.zero;
                        tempBullet.transform.position = this.transform.position;
                        tempBullet.Direction = Vector2.down;
                        tempBullet.transform.rotation = Quaternion.Euler(0, 0, -90f);
                    }
                    break;
                case ReflectDir.Right:
                    {
                        tempBullet.Direction = Vector3.zero;
                        tempBullet.transform.position = this.transform.position;
                        tempBullet.Direction = Vector2.right;
                        tempBullet.transform.rotation = Quaternion.Euler(0, 0, 0);
                    }
                    break;
            }
        }     
    }
    private void DirCheck()
    {
        Collider2D colInfo = Physics2D.OverlapCircle(LeftDir.position, Radius, layer);
        Collider2D colInfo2 = Physics2D.OverlapCircle(UpDir.position, Radius, layer);

        if (colInfo)
        {
            isLeft = true;
        }
        else if (colInfo2)
        {
            isLeft = false;
        }

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.layer == LayerMask.NameToLayer("Projectiles"))
        {
            Refle(collision);
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.white;

        Gizmos.DrawWireSphere(LeftDir.position, Radius);
        Gizmos.DrawWireSphere(UpDir.position, Radius);
    }
}

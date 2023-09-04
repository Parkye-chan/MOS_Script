using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{

    private float destroyTime = 10;

    Vector3 vStart;
    [SerializeField]
    public float fRange;
    [SerializeField]
    public GameObject effect;
    [HideInInspector]
    public int Damage = 0;

    [HideInInspector]
    public bool fire = false;

    private Animator anim;
    // Start is called before the first frame update
    private void OnEnable()
    {
        vStart = this.transform.position;
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 vCurPos = this.transform.position;
        // float fCurDist = Vector3.Distance(vStart, vCurPos);
        Vector3 vCurDist = vCurPos - vStart;
        float fCurDist = vCurDist.magnitude; //거리값 계산

        if(fCurDist >= fRange)
        {
            // GetComponent<Rigidbody2D>().velocity = Vector3.zero; // 리지드 바디 힘 없애기
            //Destroy(gameObject);
            gameObject.SetActive(false);
        }
    }

    private void OnDisable()
    {
        disableFunc();
    }

    public void disableFunc()
    {
        fire = false;
        this.transform.position = Vector3.zero;
        this.transform.rotation = Quaternion.identity;
        Rigidbody2D rigidbody = this.GetComponent<Rigidbody2D>();
        rigidbody.Sleep();
    }

    IEnumerator AutoDestroy(GameObject bullet)
    {
        
        yield return new WaitForSeconds(destroyTime);
        bullet.SetActive(false);
        bullet.transform.position = Vector3.zero;
        bullet.transform.rotation = Quaternion.identity;
    }
    /*

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            
            //Destroy(gameObject);
            if (effect)
            {
                //GameObject temp = Instantiate(effect, transform.position, transform.rotation);
               // GameObject temp = PoolingManager.instance.GetPlayerEffect();
                if(temp != null)
                {
                    temp.transform.position = transform.position;
                    temp.transform.rotation = transform.rotation;
                    temp.SetActive(true);
                }
                //Destroy(temp, 1);
                //StartCoroutine(AutoDestroy(temp));
            }
            gameObject.SetActive(false);
        }

        else if (collision.gameObject.CompareTag("Enemy"))
        {

            GameObject objPlayer = GameManager.instance.player.gameObject;
            //Status target = collision.gameObject.GetComponent<Status>();

            Monster target = collision.gameObject.GetComponent<Monster>();
            if (objPlayer)
            {
                Status me = objPlayer.GetComponent<Status>();
                //SuperMode superMode = collision.GetComponent<SuperMode>();
                if (target && me)
                {
                    //me.Attack(target);
                    //superMode.SetSuperMode();
                    if (Damage != 0)
                        target.TakeDamage(Damage);
                    else
                        target.TakeDamage(me.m_nStr);

                    Damage = 0;
                    //Destroy(this.gameObject);
                    
                    if (effect)
                    {
                        //GameObject temp = Instantiate(effect, transform.position, transform.rotation);
                        //GameObject temp = PoolingManager.instance.GetPlayerEffect();
                        if (temp != null)
                        {
                            temp.transform.position = transform.position;
                            temp.transform.rotation = transform.rotation;
                            temp.SetActive(true);
                        }
                        //Destroy(temp, 1);
                       // StartCoroutine(AutoDestroy(temp));
                    }
                    gameObject.SetActive(false);
                }

            }
        }
        else
        {
            Debug.Log(collision.gameObject.name);
            gameObject.SetActive(false);
        }
        /*
        else if(collision.gameObject.CompareTag("Player"))
        {
            SuperMode superMode = collision.gameObject.GetComponent<SuperMode>();
            if (superMode && !superMode.Use)
            {
                
                
                collision.gameObject.GetComponent<Status>().m_nHP -= BulletDamage ;
                Debug.Log(BulletDamage);
                Destroy(gameObject);
                collision.gameObject.GetComponent<Animator>().SetTrigger("Hit");
                //GameObject hiteffect = collision.gameObject.GetComponent<PlayerCtrl>().Melle_effect;
                //Instantiate(hiteffect, collision.transform);
                //Destroy(hiteffect, 1);
                GameObject temp = Instantiate(effect, transform.position, transform.rotation);
                Destroy(temp, 1);
                GameManager.instance.hitHpbar();
                superMode.SetSuperMode();
            }
            else if(superMode.Use)
            {
                Destroy(gameObject);
                GameObject temp = Instantiate(effect, transform.position, transform.rotation);
                Destroy(temp, 1);
            }
        }
        */
    }
/*
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            //Destroy(gameObject);
           
            if (effect)
            {
                //GameObject temp = Instantiate(effect, transform.position, transform.rotation);
               // GameObject temp = PoolingManager.instance.GetPlayerEffect();
                if (temp != null)
                {
                    temp.transform.position = transform.position;
                    temp.transform.rotation = transform.rotation;
                    temp.SetActive(true);
                }
                //Destroy(temp, 1);
                //StartCoroutine(AutoDestroy(temp));
            }
            gameObject.SetActive(false);
        }

       else if (collision.gameObject.CompareTag("Enemy"))
        {

            GameObject objPlayer = GameManager.instance.player.gameObject;
            Status target = collision.gameObject.GetComponent<Status>();

            if (objPlayer)
            {
                Status me = objPlayer.GetComponent<Status>();
                //SuperMode superMode = collision.GetComponent<SuperMode>();
                if (target && me)
                {
                    me.Attack(target);
                    //superMode.SetSuperMode();
                    //Destroy(this.gameObject);                    
                    if (effect)
                    {
                        //GameObject temp = Instantiate(effect, transform.position, transform.rotation);
                       // GameObject temp = PoolingManager.instance.GetPlayerEffect();
                        if (temp != null)
                        {
                            temp.transform.position = transform.position;
                            temp.transform.rotation = transform.rotation;
                            temp.SetActive(true);
                        }
                        //Destroy(temp, 1);
                       // StartCoroutine(AutoDestroy(temp));
                    }
                    gameObject.SetActive(false);
                }

            }
        }
        else if(collision.gameObject.CompareTag("Fire"))
        {
            fire = true;
            anim.SetBool("Fire", true);
        }
        //else if(!collision.gameObject.CompareTag("Player"))
        else
        {
            gameObject.SetActive(false);
        }
    }
}
*/
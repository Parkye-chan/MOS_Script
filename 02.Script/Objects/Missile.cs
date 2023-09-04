using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MoreMountains.CorgiEngine;

public class Missile : MonoBehaviour
{
    [SerializeField]
    float Radius;
    [SerializeField]
    float speed;
    Transform Enemy;
    Vector3 targetPos;

    public float DetectTime = 0.5f;
    public Transform sunrise;
    public Transform sunset;
    public float journeyTime = 1.0F;
    public float reduceHeight = 1f;
    private float startTime;
    private float fmove;
    private Rigidbody2D rb;
    private Projectile projectile;    
    //감지시간이 지나면 방향전환 x

    private void Awake()
    {
        //startTime = Time.time;
        rb = GetComponent<Rigidbody2D>();
        projectile = GetComponent<Projectile>();
    }

    void Update()
    {
        if (startTime < DetectTime)
        {
            EnnemyCheck();
            startTime += Time.deltaTime;
        }
        else
            return;
    }

    private void OnDisable()
    {
        transform.position = Vector3.zero;
        transform.rotation = Quaternion.identity;
        Enemy = null;
        fmove = 0;
        projectile.Direction = Vector2.left;
        startTime = 0;
    }

    private void Tracke()
    {
        //GetComponent<Rigidbody2D>().gravityScale = 0;
        Vector3 center = (sunrise.position + targetPos) * 0.5F; //Center 값만큼 위로 올라간다.
        center -= new Vector3(0, 1f * reduceHeight, 0); //y값을 높이면 높이가 낮아진다.
        Vector3 riseRelCenter = sunrise.position - center;
        Vector3 setRelCenter = targetPos - center;
        float fracComplete = (Time.time - startTime) / journeyTime;
        //transform.position = Vector3.Slerp(riseRelCenter, setRelCenter, fracComplete);
        fmove += Time.deltaTime * speed;
        transform.position = Vector3.Slerp(riseRelCenter, setRelCenter, fmove);
        transform.position += center;
    }

    private void ChangeDir()
    {
        Vector2 target = (targetPos - transform.position).normalized;
        projectile.Direction = target;
        transform.rotation = Quaternion.Euler(0, 0, Enemy.rotation.eulerAngles.z);
        float angle = Mathf.Atan2(target.y,target.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        //Vector2 newPos = Vector2.MoveTowards(transform.position, targetPos, speed * Time.deltaTime);
        //rb.MovePosition(newPos);
    }

    private void EnnemyCheck()
    {
       Collider2D target = Physics2D.OverlapCircle(transform.position, Radius,LayerMask.GetMask("Enemies"));
        if(target)
        {
            if (!Enemy)
            {
                Enemy = target.transform;
                sunrise = this.transform;
                targetPos = Enemy.position;
                // StartCoroutine(Slerp());
                //rb.velocity = Vector3.zero;
                ChangeDir();
            }
            else
                return;                          
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.GetMask("Enemies"))
        {
            //Destroy(gameObject);
            //gameObject.SetActive(false);
        }

        else if (collision.gameObject.CompareTag("Enemies"))
        {
            Debug.Log("attack");
            //gameObject.SetActive(false);
            /*
            GameObject objPlayer = GameManager.instance.player.gameObject;
            Status target = collision.gameObject.GetComponent<Status>();
            if (objPlayer)
            {
                Status me = objPlayer.GetComponent<Status>();
               
                if (target && me)
                {
                    me.Attack(target);                   
                    gameObject.SetActive(false);
                }

            }*/
        }
    }

 
        IEnumerator Slerp()
        {
            while (true)
            {
                transform.position = Vector3.Slerp(transform.position, Enemy.position, speed*Time.deltaTime);
            transform.rotation = Quaternion.Slerp(transform.rotation, Enemy.rotation, Time.deltaTime);   
            // if (transform.position == transform.position)
               // {
               //     break;
               // }
                yield return null;
            }
        }
    

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, Radius);
    }
}

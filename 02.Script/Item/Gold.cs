using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MoreMountains.CorgiEngine;

public class Gold : MonoBehaviour
{

    [SerializeField]
    float Radius;
    [SerializeField]
    float speed;
    Transform Player;
    public Transform sunrise;
    public Transform sunset;
    public float journeyTime = 1.0F;
    private float startTime;
    public float reduceHeight = 1f;
    private float curTime = 0f;
    public float magnetTime = 0f;
    float fmove;
    public float moveMax;
    public float updownspeed;
    public bool isMove = false;
    Vector3 pos;
    private void Start()
    {
        startTime = Time.time;
        pos = transform.position;
    }

    private void OnDisable()
    {
        fmove = 0;
    }

    void Update()
    {
        PlayerCheck();
        updownmove();
    }

    private void FixedUpdate()
    {
        if (Player)
        {

            curTime += Time.deltaTime;
            if (curTime >= magnetTime)
            {

                isMove = false;
                //Tracke();
                GetComponent<Rigidbody2D>().gravityScale = 0;
                Vector3 center = (sunrise.position + Player.position) * 0.5F; //Center 값만큼 위로 올라간다.
                center -= new Vector3(0, 1f * reduceHeight, 0); //y값을 높이면 높이가 낮아진다.
                Vector3 riseRelCenter = sunrise.position - center;
                Vector3 setRelCenter = Player.position - center;
                float fracComplete = (Time.time - startTime) / journeyTime;
                //transform.position = Vector3.Slerp(riseRelCenter, setRelCenter, fracComplete);
                fmove += Time.deltaTime * speed;
                transform.position = Vector3.Slerp(riseRelCenter, setRelCenter, fmove);
                transform.position += center;

            }
        }
        else
            return;

    }

    private void updownmove()
    {
        if (isMove)
        {
            Vector3 dirPos = pos;
            dirPos.y = pos.y + moveMax * Mathf.Sin(Time.time * updownspeed);
            transform.position = dirPos;
        }
        else
            return;
    }

    private void PlayerCheck()
    {
        Collider2D target = Physics2D.OverlapCircle(transform.position, Radius, LayerMask.GetMask("Player"));
        if (target)
        {
            if (!Player)
            {
                Player = target.transform;
                sunrise = this.transform;
                // StartCoroutine(Slerp());
            }
        }
        else
        {
            Player = null;
            curTime = 0;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.CompareTag("Player"))
        {
            LevelManager.Instance.PointUpdate();
            collision.GetComponent<Health>().GetHealth(1, collision.gameObject);
            InfoManager.instance.UpdatePotionBar(LevelManager.Instance.PotionNum);
            Destroy(this.gameObject);
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, Radius);
    }
}

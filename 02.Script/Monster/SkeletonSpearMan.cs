using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MoreMountains.CorgiEngine;
using MoreMountains.Feedbacks;

public class SkeletonSpearMan : MonoBehaviour
{
    

    private enum MonsterState
    {
        IDLE,
        WALK,
        DEAD,
        SKILL01,
        TRACKE
    }


    public float WalkSpeed = 3.0f;
    public float CoolTime = 1.5f;
    public float TrackeSpeed = 5.0f;
    public GameObject Model;
    public string WalkParam = "Walk";
    public string AttackParam = "Attack";
    public string DeadParam = "Dead";
    public GameObject DamageZone;
    public MMFeedbacks AttackFeedbacks;

    private MonsterState curState = MonsterState.IDLE;
    private float curtime = 0f;
    private Health health;
    private MonsterFOV FOV;
    private Vector2 Dir = Vector2.left;
    private Animator _anim;
    private Character character;
    private Rigidbody2D rb;
    private int slowCnt = 0;
    private float MovementSpeed = 0;

    void Start()
    {
        health = GetComponent<Health>();
        FOV = GetComponent<MonsterFOV>();
        rb = GetComponent<Rigidbody2D>();
        _anim = Model.GetComponent<Animator>();
        DataLoad();
    }

    private void OnEnable()
    {
        curtime = CoolTime;
        curState = MonsterState.IDLE;
    }

    private void OnDisable()
    {
        Dir = Vector2.left;
        slowCnt = 0;
        MovementSpeed = WalkSpeed;
    }

    // Update is called once per frame
    void Update()
    {

        switch (curState)
        {
            case MonsterState.IDLE:
                {
                    if(curtime <= 0)
                    {
                        Think();
                    }
                    curtime -= Time.deltaTime;
                }
                break;
            case MonsterState.WALK:
                {
                    transform.Translate(Vector2.left * MovementSpeed * Time.deltaTime);
                    //rb.velocity = Dir * WalkSpeed * Time.deltaTime;
                    /*Vector2 frontVec = new Vector2(transform.position.x + Dir.x * GroundFrontDist, transform.position.y - GroundCheckPivot);

                    Debug.DrawRay(frontVec, Vector3.down, Color.red);
                    RaycastHit2D rayhit = Physics2D.Raycast(frontVec, Vector3.down, 1, obstacleLayer);

                    if (rayhit.collider == null)*/
                    if (!FOV.GroundCheck() || FOV.WallCheck())
                    {
                        Turn();
                        Setstate(MonsterState.IDLE);
                    }

                    if(FOV.TargetDetect())
                    {
                        curtime = 5.0f;
                        Setstate(MonsterState.TRACKE);
                    }

                    curtime -= Time.deltaTime;
                }
                break;
            case MonsterState.DEAD:
                {

                }
                break;
            case MonsterState.SKILL01:
                {
                    if(curtime <= 0)
                    {
                        rb.velocity = Vector3.zero;
                        _anim.SetTrigger(AttackParam);
                        curtime = CoolTime;
                        AttackFeedbacks.PlayFeedbacks();
                        Setstate(MonsterState.IDLE);
                    }
                    curtime -= Time.deltaTime;
                }
                break;
            case MonsterState.TRACKE:
                {
                    transform.Translate(Vector2.left * TrackeSpeed * Time.deltaTime);
                    //rb.velocity = Dir * TrackeSpeed * Time.deltaTime;

                    if (FOV.WallCheck())
                    {
                        Turn();
                        Setstate(MonsterState.WALK);
                    }

                    if (FOV.AttackRange())
                    {
                        AttackProcess();
                    }

                    if(curtime <= 0)
                    {
                        curtime = CoolTime;
                        Setstate(MonsterState.IDLE);
                    }

                    curtime -= Time.deltaTime;
                }
                break;
        }
    }

    private void Think()
    {
        if(FOV.TargetDetect())
        {
            //rb.velocity = Vector3.zero;
            curtime = 5.0f;
            Setstate(MonsterState.TRACKE);
        }
        else
        {
            WalkProcess();
        }
    }

    private void AttackProcess()
    {
        rb.velocity = Vector3.zero;
        _anim.SetBool(WalkParam, false);
        curtime = 1.0f;
        Setstate(MonsterState.SKILL01);
    }

    private void Turn()
    {
        FOV.RayDir = -FOV.RayDir;
        //Dir = -Dir;
        transform.Rotate(0, 180, 0);
    }

    private void WalkProcess()
    {
        _anim.SetBool(WalkParam,true);
        Setstate(MonsterState.WALK);
    }

    private void TrackeProcess()
    {
        Setstate(MonsterState.TRACKE);
    }

    private void Setstate(MonsterState state)
    {
        if (curState == state)
            return;
        else
            curState = state;
    }

    public void DeadFunc()
    {
        StopAllCoroutines();
        _anim.SetTrigger(DeadParam);
        Setstate(MonsterState.DEAD);
    }

    public void DamageZoneOnOff()
    {
        if (DamageZone.activeSelf)
            DamageZone.SetActive(false);
        else
            DamageZone.SetActive(true);
    }

    public void SlowFunc()
    {
        slowCnt++;
        if (slowCnt > 1)
        {
            StartCoroutine(SlowProcess());
            slowCnt = 0;
        }
    }

    private void DataLoad()
    {
        WalkSpeed = (float)GetComponent<MonsterDataLoad>().Speed;
        MovementSpeed = WalkSpeed;
    }

    IEnumerator SlowProcess()
    {
        MovementSpeed = WalkSpeed * 0.5f;
        yield return new WaitForSecondsRealtime(5.0f);
        MovementSpeed = WalkSpeed;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Projectile projectile = collision.GetComponent<Projectile>();
        if (projectile)
        {            
            if (projectile.Slow)
                SlowFunc();
        }
    }

}

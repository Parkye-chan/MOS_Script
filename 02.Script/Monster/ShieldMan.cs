using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MoreMountains.CorgiEngine;
using MoreMountains.Feedbacks;

public class ShieldMan : MonoBehaviour
{
    private enum MonsterState
    {
        IDLE,
        WALK,
        DEAD,
        SKILL01,
        DASH
    }

    public float WalkSpeed = 3.0f;
    public float CoolTime = 3.0f;
    public float DashSpeed = 10.0f;
    public GameObject Model;
    public string WalkParam = "Walk";
    public string AttackParam = "Attack";
    public string SkillParam = "Skill01";
    public string DeadParam = "Dead";
    public MMFeedbacks ReadyFeedback;
    public MMFeedbacks AttackFeedback;

    private MonsterState curState = MonsterState.IDLE;
    private float curtime = 0f;
    private Health health;
    private MonsterFOV FOV;
    private Vector3 Dir = Vector3.left;
    private Animator _anim;
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
                    if (curtime <= 0)
                        Think();


                    curtime -= Time.deltaTime;
                }
                break;
            case MonsterState.WALK:
                {
                    transform.Translate(Vector2.left * MovementSpeed * Time.deltaTime);
                    if (!FOV.GroundCheck() || FOV.WallCheck())
                    {
                        Turn();
                        Setstate(MonsterState.IDLE);
                    }

                    if(FOV.TargetDetect())
                    {
                        AttackProcess();
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
                    if (curtime <= 0)
                    {
                        curtime = CoolTime;
                        _anim.SetBool(AttackParam, false);
                        _anim.SetBool(SkillParam, true);
                        AttackFeedback.PlayFeedbacks();
                        Setstate(MonsterState.DASH);
                    }
                    curtime -= Time.deltaTime;
                }
                break;
            case MonsterState.DASH:
                {
                    transform.Translate(Vector2.left * DashSpeed * Time.deltaTime);

                    if (FOV.GroundCheck())
                    {
                        if(FOV.WallCheck())
                        {
                            _anim.SetBool(SkillParam, false);
                            Turn();
                            rb.velocity = Vector3.zero;
                            Setstate(MonsterState.IDLE);
                        }
                        else if(curtime <= 0)
                        {
                            _anim.SetBool(SkillParam, false);
                            Turn();
                            rb.velocity = Vector3.zero;
                            Setstate(MonsterState.IDLE);
                        }
                    }
                    else
                    {
                        _anim.SetBool(SkillParam, false);
                        Turn();
                        rb.velocity = Vector3.zero;
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
            AttackProcess();
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
        _anim.SetBool(AttackParam, true);
        curtime = 0.5f;
        ReadyFeedback.PlayFeedbacks();
        Setstate(MonsterState.SKILL01);
    }

    private void Turn()
    {
        curtime = CoolTime;
        FOV.RayDir = -FOV.RayDir;
        Dir = -Dir;
        transform.Rotate(0, 180, 0);
    }

    private void WalkProcess()
    {
        _anim.SetBool(WalkParam, true);
        Setstate(MonsterState.WALK);
    }

    private void TrackeProcess()
    {
        Setstate(MonsterState.DASH);
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


    public void SlowFunc()
    {
        slowCnt++;
        if (slowCnt > 1)
        {
            StartCoroutine(SlowProcess());
            slowCnt = 0;
        }
    }

    IEnumerator SlowProcess()
    {
        MovementSpeed = WalkSpeed * 0.5f;
        yield return new WaitForSecondsRealtime(5.0f);
        MovementSpeed = WalkSpeed;
    }

    private void DataLoad()
    {
        WalkSpeed = (float)GetComponent<MonsterDataLoad>().Speed;
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

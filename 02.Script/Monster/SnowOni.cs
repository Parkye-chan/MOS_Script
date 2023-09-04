using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MoreMountains.CorgiEngine;
using MoreMountains.Feedbacks;

public class SnowOni : MonoBehaviour
{
    private enum MonsterState
    {
        IDLE,
        WALK,
        DEAD,
        SKILL01
    }


    public float WalkSpeed = 5.0f;
    public float CoolTime = 1.5f;
    public float TrackeSpeed = 5.0f;
    public GameObject Model;
    public string WalkParam = "Walk";
    public string AttackParam = "Attack";
    public string DeadParam = "Dead";
    public Transform FirePos;
    public GameObject bullet;
    public MMFeedbacks AttackFeedbacks;
    public MMFeedbacks AttackFeedbacks2;

    private MonsterState curState = MonsterState.WALK;
    private float curtime = 0f;
    private Health health;
    private MonsterFOV FOV;
    private Vector2 Dir = Vector2.left;
    private Animator _anim;
    private Rigidbody2D rb;
    private Transform Target;
    private bool isFlipped = false;
    private int slowCnt = 0;
    private float MovementSpeed = 0;

    void Start()
    {
        Init();
        DataLoad();
    }

    private void OnDisable()
    {
        Target = null;
        isFlipped = false;
        MovementSpeed = WalkSpeed;
        Dir = Vector2.left;
        slowCnt = 0;
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
                        curtime = 1.8f;                       
                        Setstate(MonsterState.SKILL01);
                    }
                    curtime -= Time.deltaTime;
                }
                break;
            case MonsterState.WALK:
                {
                    transform.Translate(Vector2.left * MovementSpeed * Time.deltaTime);
                    _anim.SetBool(WalkParam, true);

                    if (FOV.TargetDetect())
                    {
                        Target = FOV.target;
                        _anim.SetBool(WalkParam, false);
                        curtime = 0.3f;
                        Setstate(MonsterState.IDLE);
                    }
                    else if (FOV.WallCheck() || !FOV.GroundCheck())
                    {
                        Turn();
                    }
                    
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
                        LookAtPlayer();
                        _anim.SetBool(AttackParam, true);
                        curtime = 1.2f;
                    }
                    else if (!FOV.TargetDetect())
                        Setstate(MonsterState.WALK);

                    curtime -= Time.deltaTime;
                }
                break;
        }
    }


    private void Init()
    {
        health = GetComponent<Health>();
        FOV = GetComponent<MonsterFOV>();
        rb = GetComponent<Rigidbody2D>();
        _anim = GetComponent<Animator>();
    }

    public void Fire()
    {
        LookAtPlayer();
        GameObject tempbullet = Instantiate(bullet, transform.position, transform.rotation);     
        Vector3 Firedir = (Target.position - transform.position).normalized;
        tempbullet.GetComponent<Projectile>().Direction = Firedir;

        _anim.SetBool(AttackParam, false);
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

    public void AttackFeedback1()
    {
        AttackFeedbacks.PlayFeedbacks();
    }

    public void AttackFeedback2()
    {
        AttackFeedbacks2.PlayFeedbacks();
    }

    private void LookAtPlayer()
    {
        Vector3 flipped = transform.localScale;
        flipped.z *= -1f;
        if (transform.position.x > Target.position.x && isFlipped)
        {
            transform.localScale = flipped;
            transform.Rotate(0f, 180f, 0f);
            isFlipped = false;
            Dir = -Dir;
            FOV.RayDir = -FOV.RayDir;
        }
        else if (transform.position.x < Target.position.x && !isFlipped)
        {
            transform.localScale = flipped;
            transform.Rotate(0f, 180f, 0f);
            isFlipped = true;
            Dir = -Dir;
            FOV.RayDir = -FOV.RayDir;
        }
    }

    private void Turn()
    {
        Vector3 flipped = transform.localScale;
        flipped.z *= -1f;
        if (isFlipped)
        {
            transform.localScale = flipped;
            isFlipped = false;
            FOV.RayDir = -FOV.RayDir;
            Dir = -Dir;
            transform.Rotate(0, 180, 0);
        }
        else
        {
            transform.localScale = flipped;
            isFlipped = true;
            FOV.RayDir = -FOV.RayDir;
            Dir = -Dir;
            transform.Rotate(0, 180, 0);
        }

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

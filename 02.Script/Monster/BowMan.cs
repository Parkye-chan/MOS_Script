using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MoreMountains.CorgiEngine;
using MoreMountains.Feedbacks;

public class BowMan : MonoBehaviour
{
    private enum MonsterState
    {
        IDLE,
        WALK,
        DEAD,
        SKILL01,
        TRACKE
    }


    public float WalkSpeed = 5.0f;
    public float CoolTime = 1.5f;
    public float TrackeSpeed = 5.0f;
    public GameObject Model;
    public string WalkParam = "Walk";
    public string AttackParam = "Attack";
    public string DeadParam = "Dead";
    public Transform FirePos;
    public MMFeedbacks AttackFeedback;

    private MonsterState curState = MonsterState.IDLE;
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
        slowCnt = 0;
        
    }

    // Update is called once per frame
    void Update()
    {
        switch (curState)
        {
            case MonsterState.IDLE:
                {
                    Think();
                }
                break;
            case MonsterState.WALK:
                {
                    if(curtime <= 0)
                    {
                        _anim.SetBool(WalkParam, false);
                        Setstate(MonsterState.IDLE);
                    }
                    else if(FOV.WallCheck() || !FOV.GroundCheck())
                    {
                        Turn();
                        _anim.SetBool(WalkParam, false);
                        Setstate(MonsterState.IDLE);
                    }
                    transform.Translate(Vector2.left * MovementSpeed * Time.deltaTime);
                    _anim.SetBool(WalkParam, true);

                    curtime -= Time.deltaTime;
                }
                break;
            case MonsterState.DEAD:
                {

                }
                break;
            case MonsterState.SKILL01:
                {
                    _anim.SetBool(AttackParam, true);
                }
                break;
            case MonsterState.TRACKE:
                break;
        }
    }


    private void Think()
    {
        if (FOV.TargetDetect())
        {
            AttackProcess();
        }
        else
        {
            WalkProcess();
        }
    }

    private void TestBtn()
    {
        LookAtPlayer();
    }

    public void Fire()
    {
        GameObject tempbullet = PoolingManager.instance.GetArrow();
        AttackFeedback.PlayFeedbacks();
        if (tempbullet == null)
        {
            PoolingManager.instance.addArrow();
            tempbullet = PoolingManager.instance.GetArrow();
        }

        tempbullet.transform.position = FirePos.position;
        tempbullet.transform.rotation = FirePos.rotation;
        tempbullet.GetComponent<Projectile>().Direction = Dir;
        tempbullet.SetActive(true);

        Turn();
        curtime = 2.0f;
        _anim.SetBool(AttackParam, false);
        Setstate(MonsterState.WALK);
    }

    private void AttackProcess()
    {
        _anim.SetBool(WalkParam, false);
        Target = FOV.target;
        LookAtPlayer();
        Setstate(MonsterState.SKILL01);
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

    private void WalkProcess()
    {
        _anim.SetBool(WalkParam, true);
        curtime = 2.0f;
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

    private void Init()
    {
        health = GetComponent<Health>();
        FOV = GetComponent<MonsterFOV>();
        rb = GetComponent<Rigidbody2D>();
        _anim = Model.GetComponent<Animator>();       
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

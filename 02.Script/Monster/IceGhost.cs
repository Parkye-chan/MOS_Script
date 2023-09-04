using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MoreMountains.CorgiEngine;
using MoreMountains.Feedbacks;

public class IceGhost : MonoBehaviour
{
    private enum MonsterState
    {
        IDLE,
        WALK,
        DEAD,
        SKILL01
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
    private bool isFlipped;
    private Transform Target;
    private int Attacknum = 0;

    void Start()
    {
        health = GetComponent<Health>();
        FOV = GetComponent<MonsterFOV>();
        rb = GetComponent<Rigidbody2D>();
        _anim = GetComponent<Animator>();
        DataLoad();
    }

    private void OnDisable()
    {
        Target = null;
        Dir = Vector2.left;
        isFlipped = false;
        MovementSpeed = WalkSpeed;
        slowCnt = 0;
        curState = MonsterState.IDLE;
    }

    private void Update()
    {
        switch (curState)
        {
            case MonsterState.IDLE:
                {

                    if (curtime <= 0)
                    {
                        _anim.SetBool(WalkParam, true);
                        Setstate(MonsterState.WALK);
                    }

                    if (!FOV.GroundCheck() || FOV.WallCheck())
                        Turn();

                    if (FOV.TargetDetect())
                    {
                        Target = FOV.target;
                        curtime = 0.3f;
                        _anim.SetBool(WalkParam, false);
                        Setstate(MonsterState.SKILL01);
                    }

                    curtime -= Time.deltaTime;
                }
                break;
            case MonsterState.WALK:
                {
                    transform.Translate(Vector2.left * MovementSpeed * Time.deltaTime);

                    if (!FOV.GroundCheck() || FOV.WallCheck())
                        Turn();

                    if (FOV.TargetDetect())
                    {
                        Target = FOV.target;
                        curtime = 0.3f;
                        _anim.SetBool(WalkParam, false);
                        Setstate(MonsterState.SKILL01);
                    }
                }
                break;
            case MonsterState.DEAD:
                break;
            case MonsterState.SKILL01:
                {
                    if(Attacknum < 2)
                    {
                        if (curtime <= 0)
                        {
                            LookAtPlayer();
                            AttackProcess();
                        }
                    }
                    else
                    {
                        Attacknum = 0;
                        Setstate(MonsterState.IDLE);
                    }
                    curtime -= Time.deltaTime;
                }
                break;
        }
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

    public void AttackFeedback()
    {
        AttackFeedbacks.PlayFeedbacks();
    }

    private void Turn()
    {
        //FOV.RayDir = -FOV.RayDir;
        //Dir = -Dir;
        //transform.Rotate(0, 180, 0);
        Vector3 flipped = transform.localScale;
        flipped.z *= -1f;
        if (isFlipped)
        {
            transform.localScale = flipped;
            transform.Rotate(0f, 180f, 0f);
            isFlipped = false;
            Dir = -Dir;
            FOV.RayDir = -FOV.RayDir;
        }
        else if (!isFlipped)
        {
            transform.localScale = flipped;
            transform.Rotate(0f, 180f, 0f);
            isFlipped = true;
            Dir = -Dir;
            FOV.RayDir = -FOV.RayDir;
        }
    }

    private void WalkProcess()
    {
        _anim.SetBool(WalkParam, true);
        Setstate(MonsterState.WALK);
    }

    private void AttackProcess()
    {

        rb.velocity = Vector3.zero;
        _anim.SetBool(AttackParam, true);
        curtime = 0.3f;

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
        {
            DamageZone.SetActive(false);
            _anim.SetBool(AttackParam, false);
            Attacknum++;
        }
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

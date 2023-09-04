using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MoreMountains.CorgiEngine;
using MoreMountains.Feedbacks;

public class RedReaper : MonoBehaviour
{
    public enum ReaperState
    {
        IDLE,
        SKILL01,
        SKILL02,
        SKILL03,
        JUMP,
        WALK,
        DEAD,
        APPEAR,
        SLEEP
    }


    public BossZone bossZone;
    public string AppearParam = "Appear";
    public string Skill01Param = "Skill01";
    public string Skill02Param = "Skill02";
    public string Skill03Param = "Skill03";
    public string Skill04Param = "Skill04";
    public string WalkParam = "Walk";
    public string Phase2Param = "2Phase";
    public string Skill02Ready = "Skill2Ready";
    public string Skill05Ready = "Skill5Ready";
    public string TriggerParam = "Trigger";
    public float Skill01Time = 4.0f;
    public float Skill02Time = 4.0f;
    public float Skill03Time = 6.0f;
    public float Skill04Time = 3.0f;
    public float Skill05Time = 6.0f;
    public float MoveSpeed = 5.0f;
    public float DashSpeed = 10.0f;
    public float Gravity = 40.0f;
    public Transform Target;
    public GameObject DashEffect;
    public GameObject[] AttackObj;
    public GameObject LandEffect;
    public GameObject NextPhaseModel;
    public GameObject SpawnEffect;
    public MMFeedbacks DashFeedback;
    public MMFeedbacks LandFeedback;
    public GameObject LandingEffect;

    private ReaperState state;
    private Health health;
    private Animator _anim;
    private BossFOV FOV;
    private float curTime = 0;
    private bool isFlipped = true;
    private Rigidbody2D rb;
    private int Skill02cnt = 0;
    private Vector2 JumpStartPos;
    private SpriteRenderer sprite;
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        health = GetComponent<Health>();
        FOV = GetComponent<BossFOV>();
        _anim = GetComponent<Animator>();
        curTime = 6.0f;
        DataLoad();
        state = ReaperState.APPEAR;
        sprite = GetComponent<SpriteRenderer>();
        health.Invulnerable = true;
        StartCoroutine(Pade());
    }

    // Update is called once per frame
    void Update()
    {
        switch (state)
        {
            case ReaperState.IDLE:
                {
                    if (HealthCheck(health.CurrentHealth))
                    {
                        rb.gravityScale = 0;
                        JumpStartPos = transform.position;
                        SetState(ReaperState.DEAD);
                    }

                    WalkProcess();

                    if (curTime <= 0)
                        Think();


                    curTime -= Time.deltaTime;
                }
                break;
            case ReaperState.SKILL01:
                {
                    if (FOV.WallCheck())
                    {
                        curTime = 2.0f;
                        LookAtPlayer();
                        StopAllCoroutines();
                        _anim.SetBool(Skill01Param, false);
                        SetState(ReaperState.IDLE);
                    }

                    transform.Translate(Vector2.right * DashSpeed * Time.deltaTime);
                }
                break;
            case ReaperState.SKILL02:
                {
                    if (curTime <= 0)
                    {
                        curTime = 2.0f;
                        //_anim.SetBool(Skill02Param, false);
                        SetState(ReaperState.IDLE);
                    }

                    curTime -= Time.deltaTime;
                }
                break;
            case ReaperState.SKILL03:
                {
                    if (FOV.GroundCheck())
                    {
                        rb.velocity = Vector3.zero;
                        rb.gravityScale = 1.0f;
                        _anim.SetBool(Skill03Param, false);
                        GameObject temp = Instantiate(LandEffect, transform.position, transform.rotation);
                        curTime = 2.0f;
                        SetState(ReaperState.IDLE);
                    }
                    else
                    {
                        //transform.Translate(Vector2.down * Gravity * Time.deltaTime);
                        rb.gravityScale = Gravity;
                    }
                }
                break;
            case ReaperState.JUMP:
                {
                    float Dist = Vector3.Distance(JumpStartPos, transform.position);

                    if(Dist >= 10)
                    {
                        rb.velocity = Vector3.zero;
                        rb.gravityScale = 1;
                        _anim.SetBool(Skill03Param, true);
                        _anim.SetBool(TriggerParam, false);
                        SetState(ReaperState.SKILL03);
                    }

                    transform.Translate(Vector2.up * 10.0f * Time.deltaTime);
                }
                break;
            case ReaperState.WALK:
                {
                    if (HealthCheck(health.CurrentHealth))
                    {
                        _anim.SetBool(WalkParam, false);
                        rb.gravityScale = 0;
                        JumpStartPos = transform.position;
                        SetState(ReaperState.DEAD);
                    }

                    IdleProcess();

                    if (curTime <= 0)
                        Think();


                    curTime -= Time.deltaTime;

                    transform.Translate(Vector2.right * MoveSpeed * Time.deltaTime);
                }
                break;
            case ReaperState.DEAD:
                {
                    LookAtPlayer();
                    float Dist = Vector3.Distance(JumpStartPos, transform.position);

                    if (Dist >= 15)
                    {
                        NextPhaseModel.SetActive(true);
                        NextPhaseModel.GetComponent<SuperRedReaper>().Target = Target;
                        NextPhaseModel.GetComponent<SuperRedReaper>().isFlipped = isFlipped;
                        NextPhaseModel.GetComponent<BossFOV>().target = Target;
                        NextPhaseModel.transform.parent = null;
                        rb.velocity = Vector3.zero;
                        Destroy(this.gameObject);
                    }

                    transform.Translate(Vector2.up * 5.0f * Time.deltaTime);
                }
                break;
            case ReaperState.APPEAR:
                {
                    if (curTime <= 0)
                    {
                        health.Invulnerable = false;
                        SetState(ReaperState.IDLE);
                    }

                    curTime -= Time.deltaTime;

                }
                break;
            case ReaperState.SLEEP:
                break;
        }
    }

    IEnumerator Pade()
    {
        
        yield return new WaitForSeconds(1.0f);
        Instantiate(SpawnEffect, transform.position + new Vector3(0,1.0f,0), transform.rotation);
        Color tempColor = sprite.color;
        while (tempColor.a < 1f)
        {
            tempColor.a += Time.deltaTime / 2.0f;
            sprite.color = tempColor;

            if (tempColor.a >= 1f) tempColor.a = 1f;

            yield return null;
        }
        sprite.color = tempColor;
    }

    private void Think()
    {
        _anim.SetBool(WalkParam, false);

        if (DistCheck() >= 17)
        {
            Skill01Process();
        }
        else if (DistCheck() > 7 && DistCheck() < 17)
        {
            Skill02Process();
        }
        else if (DistCheck() <= 7)
        {
            Skill03Process();
        }
    }

    private void WalkProcess()
    {
        if(DistCheck() >= 1.0f)
        {

            LookAtPlayer();
            _anim.SetBool(WalkParam,true);

            
            SetState(ReaperState.WALK);
        }
    }

    private void IdleProcess()
    {
        if (DistCheck() < 1.0f)
        {
            _anim.SetBool(WalkParam, false);
            SetState(ReaperState.IDLE);
        }
    }

    private float DistCheck()
    {
        Vector2 TargetX = new Vector2(Target.position.x, transform.position.y);

        float Dist = Vector2.Distance(transform.position, TargetX);


        return Dist;
        
    }

    private void Skill01Process()
    {
        LookAtPlayer();
        StartCoroutine(Skill01());
        _anim.SetBool(Skill01Param, true);
        SetState(ReaperState.SKILL01);
    }

    IEnumerator Skill01()
    {

        while (!FOV.WallCheck())
        {
            Instantiate(DashEffect, transform.position, transform.rotation);
            yield return new WaitForSeconds(0.2f);
        }

    }

    private void Skill02Process()
    {
        LookAtPlayer();
        //_anim.SetBool(Skill02Param, true);
        _anim.SetTrigger(Skill02Param);
        curTime = 3.0f;
        SetState(ReaperState.SKILL02);
    }

    private void Skill03Process()
    {
        LookAtPlayer();
        _anim.SetBool(TriggerParam,true);
        JumpStartPos = transform.position;
        rb.gravityScale = 0;
        SetState(ReaperState.JUMP);
    }


    public void Skill02Fire()
    {
        GameObject tempbullet = Instantiate(AttackObj[Skill02cnt], transform.position , transform.rotation);
        Vector3 dir = (Target.position - transform.position).normalized;

        if(transform.rotation == Quaternion.identity)
        {
            tempbullet.GetComponent<Projectile>().Direction = new Vector3(dir.magnitude, 0, 0);
            if (Skill02cnt == 1)
            {
                tempbullet.transform.rotation = Quaternion.Euler(new Vector3(tempbullet.transform.rotation.x, transform.rotation.eulerAngles.y, 45f));
                tempbullet.GetComponent<Projectile>().Direction = new Vector3(dir.magnitude, 1, 0);
            }
        }
        else
        {
            tempbullet.GetComponent<Projectile>().Direction = new Vector3(-dir.magnitude, 0, 0);
            if (Skill02cnt == 1)
            {
                tempbullet.transform.rotation = Quaternion.Euler(new Vector3(tempbullet.transform.rotation.x, transform.rotation.eulerAngles.y, 45f));
                tempbullet.GetComponent<Projectile>().Direction = new Vector3(-dir.magnitude, 1, 0);
            }
        }

        
        Skill02cnt++;
        if(Skill02cnt >2)
        {
            Skill02cnt = 0;
        }
    }

    public void DeadFunc()
    {
        StopAllCoroutines();
        //bossZone.Clear = true;
        //bossZone.DisappearDoor();
        _anim.SetTrigger("Dead");
        SetState(ReaperState.DEAD);
    }


    public void stateStart()
    {
        state = ReaperState.IDLE;
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
            FOV.RayDir = -FOV.RayDir;
        }
        else if (transform.position.x < Target.position.x && !isFlipped)
        {
            transform.localScale = flipped;
            transform.Rotate(0f, 180f, 0f);
            isFlipped = true;
            FOV.RayDir = -FOV.RayDir;
        }
    }

    public void DashFeedbacks()
    {
        DashFeedback.PlayFeedbacks();
    }

    public void LandFeedbacks()
    {
        LandFeedback.PlayFeedbacks();
        LandingEffect.SetActive(true);
    }

    private bool HealthCheck(int curHp)
    {
        if (curHp <= (health.MaximumHealth * 0.5f))
            return true;
        else
            return false;
    }

    private void SetState(ReaperState TargetState)
    {
        if (TargetState == state)
            return;
        else
            state = TargetState;
    }

    private void DataLoad()
    {
        MoveSpeed = (float)GetComponent<MonsterDataLoad>().Speed;
        // MovementSpeed = WalkSpeed;
    }


}

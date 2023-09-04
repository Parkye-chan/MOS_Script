using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MoreMountains.CorgiEngine;
using MoreMountains.Feedbacks;



public class ForestBoss : MonoBehaviour
{

    enum TigerState
    {
        IDLE = 0,
        WALK,
        JUMP,
        ATTACK,
        READY,
        EARTHQUAKE,
        CHANGE,
        LANDING,
        ULTIMATE,
        Dead
    }


    public MMFeedbacks Roarfeedback;
    public MMFeedbacks Landingfeedback;
    public MMFeedbacks Shakefeedback;
    public MMFeedbacks Walkfeedback;
    public MMFeedbacks Skill1Chargefeedback;
    public MMFeedbacks Skill1feedback;
    public MMFeedbacks Skill2feedback;
    public MMFeedbacks Skill3feedback;
    public MMFeedbacks Skill4Startfeedback;
    public MMFeedbacks Skill4FinalFeedback;
    public MMFeedbacks Phasefeedback;

    public float MoveSpeed = 10;
    public float MoveTime = 3;
    public float Skill01Time = 18;
    public float Skill02Time = 9;
    public float Skill03Time = 6;
    public float Skill04Time = 25;
    public float ShootPower = 3;
    public GameObject[] RockBullet;
    public Transform FirePos;
    public Transform Target;
    public BoxCollider2D Skill01CheckBox;
    public BoxCollider2D Skill03CheckBox;
    public BoxCollider2D[] PlayerCheckZone;
    public float CoolTime = 3;
    public BossZone bosszone;
    public GameObject sonicBooms;
    public float RayDist = 5.0f;
    public float JumpRayDist = 2.0f;
    public GameObject JumpEffect;
    public List<Transform> QuakePos = new List<Transform>();
    public GameObject BFAobject;
    public GameObject BFADamageZone;
    public Transform BFAjumpPos;
    public Transform[] JumpPos;
    public int vertexCount = 12;

    private int QuakeCnt = 0;
    private bool QuakeVal = false;
    private CorgiController controller;
    private Health health;
    private Animator _anim;
    private SpriteRenderer sprite;
    private bool GroundCheack = false;
    private float startTime;
    private TigerState state = TigerState.LANDING;
    private float curTime = 0;
    private Rigidbody2D rb;
    private bool isFlipped = false;
    private bool isBFA = false;
    private bool HitWall = false;
    private bool Phase2 = false;
    private int BFAcnt = 0;
    private float skill01curTime = 0;
    private float skill02curTime = 0;
    private float skill03curTime = 0;
    private float skill04curTime = 0;
    private Vector2 StartPos;
    private Vector2 curPos;
    private Vector2 RayDir = Vector2.left;
    private BFA[] skills;
    private Transform bullet;
    private float tx;
    private float ty;
    private float tz;
    private float v;
    public float JumpSpeed = 9.8f;
    private float elapsed_time;
    private float max_height;
    private float t;
    private Vector3 start_pos;
    private Vector3 end_pos;
    private float dat;
    private float targetYPos;

    private void Start()
    {
        //controller = GetComponent<CorgiController>();
        //controller.DefaultParameters.FallMultiplier = 3.5f;
        sprite = GetComponent<SpriteRenderer>();
        health = GetComponent<Health>();
        _anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        rb.gravityScale = 3.5f;
        skills = BFAobject.GetComponentsInChildren<BFA>();                            
        LookAtPlayer();
        targetYPos = 55.19f; //Target.position.y;
        if(PlayerInfoManager.instance.BossMode)
            targetYPos = -7.66f;

        DataLoad();
    }

    private void OnDisable()
    {
        if (state == TigerState.Dead)
        {
            bosszone.Clear = true;
            bosszone.DisappearDoor();
        }
    }

    private void Update()
    {
        if (Target.GetComponent<Character>().ConditionState.CurrentState != CharacterStates.CharacterConditions.Dead)
        {
            if (health.CurrentHealth <= 0)
            {
                StopAllCoroutines();
                BFADamageZone.SetActive(false);
                state = TigerState.Dead;
            }

            PlayerHealthCheack(Target);

            switch (state)
            {
                case TigerState.IDLE:
                    {                        
                        LookAtPlayer();
                        if (curTime <= 0)
                        {
                            Think();
                        }

                        curTime -= Time.deltaTime;
                        skill01curTime -= Time.deltaTime;
                        skill02curTime -= Time.deltaTime;
                        skill03curTime -= Time.deltaTime;
                        skill04curTime -= Time.deltaTime;

                    }
                    break;
                case TigerState.WALK:
                    {                       

                        HitWall = WallCheack();
                        curPos = transform.position;
                        float Dist = Vector2.Distance(StartPos, curPos);
                        Debug.Log(Dist);
                        Debug.DrawLine(StartPos, curPos);
                        if (Dist < 6)
                        {

                            //curTarget = (Target.position- transform.position).normalized;
                            // Vector2 target = new Vector2(Target.position.x, rb.position.y);
                            //Vector2 newPos = Vector2.MoveTowards(rb.position, curTarget, MoveSpeed * Time.fixedDeltaTime);
                            //Debug.Log("newpos : " + newPos);
                            //rb.MovePosition(newPos);

                            //Debug.Log(curTarget);
                            if (!HitWall)
                                transform.Translate(Vector2.left * MoveSpeed * Time.deltaTime);
                            else
                            {
                                if (PlayerCheack(Skill03CheckBox))
                                {
                                    _anim.SetBool("Walk", false);
                                    skill03curTime = Skill03Time;
                                    curTime = 3.0f;
                                    LookAtPlayer();
                                    _anim.SetBool("Jump", true);
                                    SetState(TigerState.ATTACK);
                                }                                
                            }                              
                        }
                        else
                        {
                            if(Skill03PlayerCheack())
                            {
                                LookAtPlayer();
                                _anim.SetBool("Walk", false);
                                SetState(TigerState.IDLE);
                            }
                            else
                            {
                                if (PlayerCheack(Skill03CheckBox))
                                {
                                    _anim.SetBool("Walk", false);
                                    skill03curTime = Skill03Time;
                                    curTime = 3.0f;
                                    LookAtPlayer();
                                    _anim.SetBool("Jump", true);
                                    SetState(TigerState.ATTACK);
                                }
                                else
                                {
                                    if (skill02curTime <= 0)
                                    {
                                        _anim.SetBool("Walk", false);
                                        skill02curTime = Skill02Time;
                                        curTime = 4.0f;
                                        QuakeCnt = 0;
                                        SetState(TigerState.EARTHQUAKE);
                                    }
                                    else
                                    {
                                        LookAtPlayer();
                                        _anim.SetBool("Walk", false);
                                        SetState(TigerState.IDLE);
                                    }
                                }
                            }                                                     
                        }

                        curTime -= Time.deltaTime;
                        skill01curTime -= Time.deltaTime;
                        skill02curTime -= Time.deltaTime;
                        skill03curTime -= Time.deltaTime;
                        skill04curTime -= Time.deltaTime;
                    }
                    break;
                case TigerState.JUMP:
                    {
                        _anim.SetTrigger("JumpAttack");
                        if (RayGroundCheck())
                        {
                            //controller.DefaultParameters.FallMultiplier = 3.5f;
                            rb.gravityScale = 3.5f;
                            rb.velocity = Vector3.zero;
                            StopAllCoroutines();
                            _anim.SetBool("Jump", false);
                            LandingShake();
                            LookAtPlayer();
                            SetState(TigerState.IDLE);
                        }
                        curTime -= Time.deltaTime;
                        skill01curTime -= Time.deltaTime;
                        skill02curTime -= Time.deltaTime;
                        skill03curTime -= Time.deltaTime;
                        skill04curTime -= Time.deltaTime;
                    }
                    break;
                case TigerState.READY:
                    {
                        
                        curTime -= Time.deltaTime;
                        skill01curTime -= Time.deltaTime;
                        skill02curTime -= Time.deltaTime;
                        skill03curTime -= Time.deltaTime;
                        skill04curTime -= Time.deltaTime;
                    }
                    break;
                case TigerState.ATTACK:
                    {

                        curTime -= Time.deltaTime;
                        skill01curTime -= Time.deltaTime;
                        skill02curTime -= Time.deltaTime;
                        skill03curTime -= Time.deltaTime;
                        skill04curTime -= Time.deltaTime;
                    }
                    break;
                case TigerState.EARTHQUAKE:
                    {
                        
                        if (QuakeCnt < 4)
                        {
                            _anim.SetBool("Skill02", true);
                        }
                        else
                            SetState(TigerState.IDLE);

                        curTime -= Time.deltaTime;
                        skill01curTime -= Time.deltaTime;
                        skill02curTime -= Time.deltaTime;
                        skill03curTime -= Time.deltaTime;
                        skill04curTime -= Time.deltaTime;
                    }
                    break;
                case TigerState.CHANGE:
                    {
                        if(curTime <= 0)
                        {
                            SetState(TigerState.IDLE);
                        }
                        curTime -= Time.deltaTime;
                        skill01curTime -= Time.deltaTime;
                        skill02curTime -= Time.deltaTime;
                        skill03curTime -= Time.deltaTime;
                        skill04curTime -= Time.deltaTime;
                    }
                    break;
                case TigerState.LANDING:
                    {
                        if (RayGroundCheck() && !GroundCheack )
                        {
                            _anim.SetBool("Ground", true);
                            GroundCheack = true;
                        }
                        else
                            return;
                    }
                    break;
                case TigerState.ULTIMATE:
                    {
                        if(RayGroundCheck())
                        {
                            if (isBFA)
                                return;
                            else
                            {
                                _anim.SetBool("BFAend", true);
                                Shake();
                                Skill4FinalFeedback.PlayFeedbacks();
                                StartCoroutine(BFAEnd());
                                isBFA = true;
                            }
                        }
                        curTime -= Time.deltaTime;
                        skill01curTime -= Time.deltaTime;
                        skill02curTime -= Time.deltaTime;
                        skill03curTime -= Time.deltaTime;
                        skill04curTime -= Time.deltaTime;
                    }
                    break;
                case TigerState.Dead:
                    {
                        return;
                    }
                    
            }
        }
    }

    public void Shake()
    {
        Roarfeedback.PlayFeedbacks();
    }

    public void LandingShake()
    {
        Landingfeedback.PlayFeedbacks();
        StartCoroutine(RoarStart());
    }

    private bool WallCheack()
    {
        RaycastHit2D hitInfo = Physics2D.Raycast(transform.position, RayDir, RayDist, LayerMask.GetMask("Platforms"));
        Debug.DrawRay(transform.position, RayDir * RayDist, Color.red);
        if (hitInfo)
        {
            return true;
        }
        return false;
    }

    public void ShockWaveSpawn()
    {
        GameObject temp = Instantiate(sonicBooms,FirePos.position, FirePos.rotation);
        Destroy(temp, 3.0f);
    }

    private bool HealthCheck(int curHp)
    {
        if (curHp <= (health.MaximumHealth * 0.5f))
        {
            
            return true;
        }       
        else
            return false;
    }

    private bool PlayerCheack(BoxCollider2D PlayerCheckBox)
    {
        RaycastHit2D hitInfo = Physics2D.BoxCast(PlayerCheckBox.bounds.center, PlayerCheckBox.bounds.size, 0f, Vector2.zero, 0, LayerMask.GetMask("Player"));
        if (hitInfo)
        {
            Target = hitInfo.transform;
            return true;
        }
        return false;
    }

    private bool Skill03PlayerCheack()
    {
        RaycastHit2D hitInfo = Physics2D.BoxCast(PlayerCheckZone[0].bounds.center, PlayerCheckZone[0].bounds.size, 0f, Vector2.zero, 0, LayerMask.GetMask("Player"));
        RaycastHit2D hitInfo2 = Physics2D.BoxCast(PlayerCheckZone[1].bounds.center, PlayerCheckZone[1].bounds.size, 0f, Vector2.zero, 0, LayerMask.GetMask("Player"));
        if (hitInfo || hitInfo2)
        {
            return true;
        }
        return false;
    }

    private void Think()
    {

        if(HealthCheck(health.CurrentHealth))
        {
            if (Phase2)
            {
                ThinkPhase2();
            }
            else
            {
                _anim.SetBool("Phase2", true);
                Phase2 = true;
                curTime = 4.0f;
                SetState(TigerState.CHANGE);
            }
        }
        else
        {
            if (skill01curTime <= 0)
            {
                Skill01Process();
            }
            else
            {
                if (skill03curTime <= 0)
                {
                    if(Skill03PlayerCheack())
                    {
                        if (skill02curTime <= 0)
                        {
                            Skill02Process();
                        }
                    }
                    else
                    {
                        Skill03Process();
                    }                    
                }
                else
                {
                    if (skill02curTime <= 0)
                    {
                        Skill02Process();
                    }
                    else
                    {
                        _anim.SetBool("Walk", false);
                        SetState(TigerState.IDLE);
                    }
                }
            }
        }
    }

    private void ThinkPhase2()
    {
        if(skill04curTime <= 0)
        {
            _anim.SetBool("BFA", true);
            skill04curTime = Skill04Time;
            curTime = 10.0f;
            isBFA = false;
        }
        else
        {
            if (skill01curTime <= 0)
            {
                Skill01Process();
            }
            else
            {
                if (skill03curTime <= 0)
                {
                    if (Skill03PlayerCheack())
                    {
                        if (skill02curTime <= 0)
                        {
                            Skill02Process();
                        }
                    }
                    else
                    {
                        Skill03Process();
                    }
                }
                else
                {
                    if (skill02curTime <= 0)
                    {
                        Skill02Process();
                    }
                    else
                    {
                        _anim.SetBool("Walk", false);
                        SetState(TigerState.IDLE);
                    }
                }
            }
        }
    }

    private void Skill01Process()
    {
        if (PlayerCheack(Skill01CheckBox))
        {
            skill01curTime = Skill01Time;
            _anim.SetBool("Walk", false);
            _anim.SetBool("Ready", true);
            StartCoroutine(Skill01());
            SetState(TigerState.READY);
        }
        else
        {
            Skill03Process();
        }
    }

    private void Skill02Process()
    {
        _anim.SetBool("Walk", false);
        skill02curTime = Skill02Time;
        curTime = 4.0f;
        QuakeCnt = 0;
        SetState(TigerState.EARTHQUAKE);
    }

    private void Skill03Process()
    {
        if (PlayerCheack(Skill03CheckBox))
        {
            _anim.SetBool("Walk", false);
            skill03curTime = Skill03Time;
            curTime = 3.0f;
            LookAtPlayer();
            _anim.SetBool("Jump", true);
            SetState(TigerState.ATTACK);
        }
        else
        {
            _anim.SetBool("Walk", true);
            StartPos = transform.position;
            SetState(TigerState.WALK);
        }
    }

    private void TestBtn()
    {
        skill01curTime = Skill01Time;
        _anim.SetBool("Ready", true);
        StartCoroutine(Skill01());
        SetState(TigerState.READY);
    }

    private void TestBtn2()
    {
        //controller.DefaultParameters.FallMultiplier = 0f;
        rb.gravityScale = 0f;
        GameObject temp = Instantiate(JumpEffect, transform.position, transform.rotation);
        temp.SetActive(true);
        Jump(transform, transform.position, Target.position + new Vector3(0, 2), JumpSpeed, JumpPos[1].position.y);
        StartCoroutine(JumpMove());
    }


    IEnumerator Skill01()
    {
        Skill1Chargefeedback.PlayFeedbacks();
        yield return new WaitForSeconds(2.0f);
        _anim.SetBool("Skill01", true);
        Skill1feedback.PlayFeedbacks();
        SetState(TigerState.ATTACK);
        yield return new WaitForSeconds(0.5f);
        StartCoroutine(Skill01B());   
    }

    IEnumerator Skill01B()
    {
        Skill1Chargefeedback.PlayFeedbacks();
        SetState(TigerState.READY);
        yield return new WaitForSeconds(2.0f);
        _anim.SetBool("Skill01", false);
        _anim.SetBool("Ready", false);
        Skill1feedback.PlayFeedbacks();
        yield return new WaitForSeconds(0.5f);
        SetState(TigerState.IDLE);
    }

    private void SetState(TigerState InputState)
    {
        if (state == InputState)
            return;

        state = InputState;
    }

    public void EarthQuake()
    {
        StartCoroutine(EarthQuakeFunc());
            
    }

    IEnumerator EarthQuakeFunc()
    {                
        QuakeCnt++;
        Shakefeedback.PlayFeedbacks();
        if (RockBullet.Length != 0)
        {
            for (int i = 0; i < QuakePos.Count; i++)
            {
                if(QuakeVal)
                {
                    if(i%2 == 0)
                    {
                        int RandomVal = Random.Range(0, RockBullet.Length);
                        GameObject temp = Instantiate(RockBullet[RandomVal], QuakePos[i].position, QuakePos[i].rotation);
                    }
                }
                else
                {
                    if (i % 2 != 0)
                    {
                        int RandomVal = Random.Range(0, RockBullet.Length);
                        GameObject temp = Instantiate(RockBullet[RandomVal], QuakePos[i].position, QuakePos[i].rotation);
                    }
                }
            }
            
        }
        yield return new WaitForSeconds(0.5f);

        if (QuakeVal)
            QuakeVal = false;
        else
            QuakeVal = true;

        if (QuakeCnt >= 4)
        {
            _anim.SetBool("Skill02", false);

            SetState(TigerState.IDLE);
            yield break;
        }

        _anim.SetTrigger("Skill02Repeat");
        
        
    }

    IEnumerator FireProcess(float time)
    {

        yield return new WaitForSeconds(time);

        curTime = 1;      
        state = TigerState.IDLE;

    }

    private void BFAProcess()
    {
        if (BFAcnt >= skills.Length)
            BFAcnt = 0;

        skills[BFAcnt].WarningFunc();
        BFAcnt++;
    }

    public IEnumerator BFAGroundtouch()
    {

        //controller.DefaultParameters.FallMultiplier = 3.5f;
        rb.gravityScale = 3.5f;
        sprite.color = Color.white;
        GetComponent<BoxCollider2D>().enabled = true;
        _anim.SetBool("BFA", false);
        transform.position = BFAjumpPos.transform.position;
        yield return new WaitForSeconds(0.1f);
        state = TigerState.ULTIMATE;
        
    }

    private IEnumerator BFAEnd()
    {
        BFADamageZone.SetActive(true);
        yield return new WaitForSeconds(0.5f);
        BFADamageZone.SetActive(false);
        yield return new WaitForSeconds(3.0f);
        _anim.SetBool("BFAend", false);
        SetState(TigerState.IDLE);
    }

    public void HideBoss()
    {
        sprite.color = Color.clear;
        GetComponent<BoxCollider2D>().enabled = false;
        //controller.DefaultParameters.FallMultiplier = 0f;
        rb.gravityScale = 0f;
        BFAProcess();
    }

    public void JumpFunc()
    {
        //controller.DefaultParameters.FallMultiplier = 0f;
        rb.gravityScale = 0f;
        GameObject temp = Instantiate(JumpEffect, transform.position, transform.rotation);
        temp.SetActive(true);
        Vector3 Ypos = new Vector3(Target.position.x, targetYPos, Target.position.z);
        Jump(transform,transform.position, Ypos + new Vector3(0,2),JumpSpeed, JumpPos[1].position.y);
        StartCoroutine(JumpMove());
        
    }

    IEnumerator JumpMove()
    {
        yield return new WaitForSeconds(0.2f);
        SetState(TigerState.JUMP);
    }

    private bool GroundCheck()
    {
        //RaycastHit2D hit = Physics2D.BoxCast(GroundCheckPos.bounds.center, GroundCheckPos.bounds.size, 0f, Vector2.down, 0, LayerMask.GetMask("Platforms"));        
        if (controller.State.IsGrounded)
        {
            
           return true;
            
        }
        else
        {
            return false;
        }
    }

    private bool RayGroundCheck()
    {
        RaycastHit2D hitInfo = Physics2D.Raycast(transform.position, Vector2.down, JumpRayDist, LayerMask.GetMask("Platforms"));
        Debug.DrawRay(transform.position, JumpRayDist * Vector2.down, Color.red);
        if (hitInfo)
        {
            return true;
        }
        return false;
    }

    private void PlayerHealthCheack(Transform Player)
    {
        if (Player == null)
            return;
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
            RayDir = Vector2.left;
        }
        else if (transform.position.x < Target.position.x && !isFlipped)
        {
            transform.localScale = flipped;
            transform.Rotate(0f, 180f, 0f);
            isFlipped = true;
            RayDir = Vector2.right;
        }
    }

    public void stateStart()
    {
        //controller.DefaultParameters.FallMultiplier = 1;
        rb.gravityScale = 1f;
        state = TigerState.IDLE;
    }

    IEnumerator RoarStart()
    {
        yield return new WaitForSecondsRealtime(1.0f);
        _anim.SetBool("Roar", true);
        yield return new WaitForSecondsRealtime(3.0f);
        _anim.SetBool("Stay", true);
        _anim.SetBool("Roar", false);
        _anim.SetBool("Ground", false);

    }

    public void Jump(Transform bullet, Vector3 startPos, Vector3 endPos, float g, float max_height)

    {
        start_pos = startPos;
        end_pos = endPos;
        this.JumpSpeed = g;
        this.max_height = max_height;
        this.bullet = bullet;
        this.bullet.position = start_pos;
        var dh = endPos.y - startPos.y;   
        var mh = max_height - startPos.y;
        ty = Mathf.Sqrt(2 * this.JumpSpeed * mh);
        float a = this.JumpSpeed;
        float b = -2 * ty;
        float c = 2 * dh;
        dat = (-b + Mathf.Sqrt(b * b - 4 * a * c)) / (2 * a);
        tx = -(startPos.x - endPos.x) / dat;
        tz = -(startPos.z - endPos.z) / dat;
        this.elapsed_time = 0;
        StartCoroutine(this.ShootImpl());
    }

    IEnumerator ShootImpl()
    {
        while (true)
        {
            this.elapsed_time += Time.deltaTime;
            var tx = start_pos.x + this.tx * elapsed_time;

            var ty = start_pos.y + this.ty * elapsed_time - 0.5f * JumpSpeed * elapsed_time * elapsed_time;

            var tz = start_pos.z + this.tz * elapsed_time;
            var tpos = new Vector3(tx, ty, tz);
            Debug.Log(tpos);
            bullet.transform.position = tpos;

            if (this.elapsed_time >= this.dat)
                break;



            yield return null;    
        }
    }

    private void DataLoad()
    {
        MoveSpeed = (float)GetComponent<MonsterDataLoad>().Speed;
        //MovementSpeed = MoveSpeed;
    }

    public void WalkFeedbackFunc()
    {
        Walkfeedback.PlayFeedbacks();
    }

    public void Skill3FeedbackFunc()
    {
        Skill3feedback.PlayFeedbacks();
    }

    public void Skill4StartFunc()
    {
        Skill4Startfeedback.PlayFeedbacks();
    }

    public void PhaseFeedbackFunc()
    {
        Phasefeedback.PlayFeedbacks();
    }

    /*
    private void OnGUI()
    {
        if (GUI.Button(new Rect(200, 0, 200, 80), "ATTACK"))
        {
            TestBtn();
        }
        if (GUI.Button(new Rect(200, 400, 200, 80), "REset"))
        {
            TestBtn2();
        }
    }*/

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawLine(JumpPos[0].position, JumpPos[1].position);

        Gizmos.color = Color.cyan;
        Gizmos.DrawLine(JumpPos[1].position, JumpPos[2].position);

        Gizmos.color = Color.red;
        for (float ratio = 0.5f/ vertexCount; ratio < 1; ratio += 1.0f/ vertexCount)
        {
            Gizmos.DrawLine(Vector3.Lerp(JumpPos[0].position, JumpPos[1].position, ratio)
                ,Vector3.Lerp(JumpPos[1].position, JumpPos[2].position, ratio));
        }

        Gizmos.DrawWireCube(Skill01CheckBox.bounds.center, Skill01CheckBox.bounds.size);

        Gizmos.DrawWireCube(Skill03CheckBox.bounds.center, Skill03CheckBox.bounds.size);

        Gizmos.DrawWireCube(PlayerCheckZone[0].bounds.center, PlayerCheckZone[0].bounds.size);
        Gizmos.DrawWireCube(PlayerCheckZone[1].bounds.center, PlayerCheckZone[1].bounds.size);
    }


}

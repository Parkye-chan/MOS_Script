using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MoreMountains.CorgiEngine;
using MoreMountains.Feedbacks;

public class Guardian : MonoBehaviour
{


    private enum GuardianState
    {
        IDLE,
        JUMP,
        SKILL1,
        SKILL2,
        ATTACK,
        WALK,
        WAIT,
        DEAD
    }

    public enum GuardianType
    {
        GUARDIAN_A,
        GUARDIAN_B,
        GUARDIAN_C
    }


    public BossZone bossZone;
    public string AppearParam = "Appear";
    public string Skill01Param = "Skill01";
    public string Skill02Param = "Skill02";
    public string AttackParam = "Attack";
    public string WalkParam = "Walk";
    public string JumpParam = "Jump";
    public string IdleParm = "Idle";
    public float Skill01Time = 10;
    public float Skill02Time = 10;
    public float SkillAttackTime = 6;
    public float MoveSpeed = 7;
    public float JumpPower = 1;
    public float Gravity = 40;
    public bool DirLeft = true;
    public Transform FirePos;
    public Transform Target;
    public GuardianType type;
    public BoxCollider2D AttackBox;
    public BoxCollider2D Skill1Box;
    public GameObject Skill1DamageZone;
    public GameObject Skill1DamageZone2;
    public BoxCollider2D Skill2Box;
    public GameObject Skill2DamageZone;
    public GameObject SkillAttackDamageZone;
    public GameObject SkillB_Bullet;
    public Transform[] JumpPos;
    public MMFeedbacks Skill01Feedback;
    public MMFeedbacks Skill02Feedback;
    public MMFeedbacks AttackFeedback;

    private GuardianState state;
    private Health health;
    private Animator _anim;
    private BossFOV FOV;
    private Vector3 JumpStartPos;
    private Rigidbody2D rb;
    private float curTime = 0;
    //private float Skill01curTime = 0;
    //private float Skill02curTime = 0;
    //private float Skill03curTime = 0;
    private bool isFlipped = false;
    private int UseSkill = 0;
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
    private Transform bullet;

    void Start()
    {
        Init();
    }

    // Update is called once per frame
    void Update()
    {

        switch (state)
        {
            case GuardianState.IDLE:
                {
                    LookAtPlayer();
                    if(curTime <= 0)
                        Think();

                    curTime -= Time.deltaTime;
                }
                break;
            case GuardianState.JUMP:
                {
                    float Dist = Vector3.Distance(JumpStartPos, transform.position);
                    rb.gravityScale = 0;
                    transform.Translate(Vector2.up * JumpPower * Time.deltaTime);
                    if (Dist >= 7)
                    {
                        rb.velocity = Vector3.zero;
                        rb.gravityScale = 1;
                        _anim.SetBool(JumpParam, false);
                        _anim.SetBool(Skill02Param,true);
                        SetState(GuardianState.SKILL2);
                    }

                    //rb.velocity = Vector2.up* JumpPower * Time.deltaTime;
                    
                }
                break;
            case GuardianState.SKILL1:
                {
                    if (curTime <= 0)
                    {
                        _anim.SetBool(Skill02Param, false);
                        _anim.SetBool(Skill01Param, false);
                        curTime = 1.0f;
                        SetState(GuardianState.IDLE);
                    }

                    curTime -= Time.deltaTime;
                }
                break;
            case GuardianState.SKILL2:
                {
                    if(FOV.GroundCheck())
                    {
                        _anim.SetBool(Skill02Param, false);
                        _anim.SetBool(Skill01Param, false);
                        rb.gravityScale = 1.0f;
                        SetState(GuardianState.IDLE);
                    }
                    else
                    {
                        //rb.velocity = Vector3.down* Gravity * Time.deltaTime;
                        //transform.Translate(Vector2.down * Gravity * Time.deltaTime);
                        rb.gravityScale = Gravity;
                    }
                }
                break;
            case GuardianState.ATTACK:
                {

                    switch (type)
                    {
                        case GuardianType.GUARDIAN_A:
                            {
                                transform.Translate(Vector2.left * 7.0f * Time.deltaTime);
                                curTime -= Time.deltaTime;

                                if (curTime <= 0)
                                {
                                    curTime = 3.0f;
                                    //UseSkill = 0;
                                    SetState(GuardianState.IDLE);
                                }
                            }
                            break;
                        case GuardianType.GUARDIAN_B:
                            {
                                
                                //UseSkill = 0;
                                curTime = 3.0f;
                                SetState(GuardianState.IDLE);
                            }
                            break;
                        case GuardianType.GUARDIAN_C:
                            {
                                float Dist = Vector3.Distance(JumpStartPos, transform.position);
                                rb.gravityScale = 0;

                                if (curTime > 0)
                                {
                                    curTime -= Time.deltaTime;
                                    return;
                                }

                                transform.Translate(Vector2.up * JumpPower * Time.deltaTime);

                                if (Dist >= 7.0f)
                                {
                                    rb.velocity = Vector3.zero;
                                    rb.gravityScale = 1;
                                    _anim.SetBool(JumpParam, false);
                                    _anim.SetBool(Skill02Param, true);
                                    SetState(GuardianState.SKILL2);
                                }

                                //rb.velocity = Vector2.up * JumpPower * Time.deltaTime;
                                
                            }
                            break;
                    }

                }
                break;
            case GuardianState.WALK:
                {
                    if (curTime <= 0)
                    {
                        StartCoroutine(WalkSkill01());
                    }
                    else
                    {
                        transform.Translate(Vector2.left * MoveSpeed * Time.deltaTime);
                        curTime -= Time.deltaTime;
                    }
                }
                break;
            case GuardianState.WAIT:
                {
                    if(FOV.LinePlayerCheck())
                    {
                        bossZone.SpawnDoor();
                        curTime = 0;
                        Target = FOV.target;
                        SetState(GuardianState.IDLE);
                    }
                    else
                        return;
                    
                }
                break;
            case GuardianState.DEAD:
                break;
        }
    }


    private void Init()
    {
        if (!bossZone)
        {
            health = GetComponent<Health>();
            _anim = GetComponent<Animator>();
            FOV = GetComponent<BossFOV>();
            rb = GetComponent<Rigidbody2D>();
            //Skill01curTime = 0;
            //Skill02curTime = 0;
            //Skill03curTime = 0;
            Target = MoreMountains.CorgiEngine.GameManager.Instance.PersistentCharacter.transform;
            curTime = 1.0f;
            SetState(GuardianState.IDLE);
        }
        else
        {
            health = GetComponent<Health>();
            _anim = GetComponent<Animator>();
            FOV = GetComponent<BossFOV>();
            rb = GetComponent<Rigidbody2D>();
            //Skill01curTime = 0;
            //Skill02curTime = 0;
            //Skill03curTime = 0;
            curTime = 0;
            if(DirLeft)
            {
                isFlipped = false;
            }
            else
            {
                isFlipped = true;
            }
            state = GuardianState.WAIT;
        }

        DataLoad();
    }

    private void Think()
    {
        if(UseSkill >= 3)
        {
            if(FOV.TargetDetect(AttackBox))
            {
                SkillAttackProcess();
            }
            else
            {
                if (FOV.TargetDetect(Skill1Box))
                {
                    Skill01Process();
                }
                else
                {
                    WalkProcess();
                }
            }
        }
        else
        {
            if(FOV.TargetDetect(Skill1Box))
            {
                Skill01Process();
            }
            else
            {
                WalkProcess();
            }
        }
    }

    IEnumerator WalkSkill01()
    {
        SetState(GuardianState.SKILL1);
        yield return new WaitForSeconds(0.3f);
        _anim.SetBool(WalkParam, false);
        if(FOV.TargetDetect(Skill1Box))
        {
            Skill01Process();
        }
        else
        {
            Skill02Process();
        }
    }

    IEnumerator Skill02()
    {
        yield return new WaitForSeconds(1.0f);
        curTime = 3.0f;
        if(FOV.WallCheck())
        {
            _anim.SetBool(JumpParam, true);
            JumpStartPos = transform.position;
            SetState(GuardianState.JUMP);
        }
        else
        {
            _anim.SetBool(JumpParam, true);
            Vector3 Ypos = new Vector3(Target.position.x, targetYPos, Target.position.z);
            JumpStartPos = transform.position;
            //Jump(transform, transform.position, Ypos + new Vector3(0, 2), JumpSpeed, JumpPos[1].position.y);
            Jump(transform, transform.position, JumpPos[2].position, JumpSpeed, JumpPos[1].position.y);
            SetState(GuardianState.JUMP);
        }
    }

    private void Skill01Process()
    {
        curTime = 2.0f;
        _anim.SetBool(Skill01Param,true);
        UseSkill++;
        SetState(GuardianState.SKILL1);
    }

    private void Skill02Process()
    {
        UseSkill++;
        StartCoroutine(Skill02());
    }

    private void SkillAttackProcess()
    {
        _anim.SetBool(AttackParam, true);
        UseSkill = 0;
        StartCoroutine(SkillAttack());
        SetState(GuardianState.SKILL1);
    }

    IEnumerator SkillAttack()
    {
        yield return new WaitForSeconds(0.65f);

        _anim.SetBool(AttackParam, false);
        curTime = SkillAttackTime;
        JumpStartPos = transform.position;
        SetState(GuardianState.ATTACK);


    }

    private void WalkProcess()
    {
        curTime = 1.0f;
        _anim.SetBool(WalkParam, true);
        SetState(GuardianState.WALK);
    }

    public void SkillBFire()
    {
        GameObject temp = Instantiate(SkillB_Bullet, FirePos.position, FirePos.rotation);
        Destroy(temp,3.0f);
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
            bullet.transform.position = tpos;

            if (this.elapsed_time >= this.dat)
            {
                _anim.SetBool(Skill02Param, true);
                break;
            }



            yield return null;
        }
        rb.gravityScale = 1.0f;
        _anim.SetBool(Skill02Param, false);
    }

    public void Skill1AttackZoneActive()
    {
        if (Skill1DamageZone.activeSelf)
            Skill1DamageZone.SetActive(false);
        else
            Skill1DamageZone.SetActive(true);
    }

    public void Skill1AttackZoneActive2()
    {
        if (Skill1DamageZone2.activeSelf)
        {
            Skill1DamageZone2.SetActive(false);
            _anim.SetBool(Skill01Param, false);
            SetState(GuardianState.IDLE);
        }
        else
            Skill1DamageZone2.SetActive(true);
    }

    public void Skill2AttackZoneActive()
    {
        if (Skill2DamageZone.activeSelf)
        {
            Skill2DamageZone.SetActive(false);
            _anim.SetBool(Skill02Param, false);
            SetState(GuardianState.IDLE);
        }
        else
            Skill2DamageZone.SetActive(true);
    }

    public void SkillAttackZoneActive()
    {
        if (SkillAttackDamageZone.activeSelf)
            SkillAttackDamageZone.SetActive(false);
        else
            SkillAttackDamageZone.SetActive(true);
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

    public void DeadFunc()
    {
        StopAllCoroutines();
        
        _anim.SetTrigger("Dead");
        if (bossZone)
        {
            bossZone.Clear = true;
            bossZone.DisappearDoor();
        }

        SetState(GuardianState.DEAD);
    }

    public void Skill01Feedbacks()
    {
        Skill01Feedback.PlayFeedbacks();
    }

    public void Skill02Feedbacks()
    {
        Skill02Feedback.PlayFeedbacks();
    }

    public void AttackFeedbacks()
    {
        AttackFeedback.PlayFeedbacks();
    }


    private void DataLoad()
    {
        MoveSpeed = (float)GetComponent<MonsterDataLoad>().Speed;
        //MovementSpeed = WalkSpeed;
    }

    private void SetState(GuardianState golemState)
    {
        if (golemState == state)
            return;
        else
            state = golemState;
    }
}

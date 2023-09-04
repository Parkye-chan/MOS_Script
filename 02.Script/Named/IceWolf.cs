using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MoreMountains.CorgiEngine;
using MoreMountains.Feedbacks;

public class IceWolf : MonoBehaviour
{
    private enum BossState
    {
        IDLE,
        SKILL1,
        SKILL2,
        SKILL3,
        WALK,
        WAIT,
        DEAD
    }

    public BossZone bossZone;
    public string Skill01ReadyParam = "Skill01Ready";
    public string Skill02ReadyParam = "Skill02Ready";
    public string Skill01Param = "Skill01";
    public string Skill02Param = "Skill02";
    public string Skill03Param = "Skill03";
    public string AttackParam = "Attack";
    public string WalkParam = "Walk";
    public string JumpParam = "Jump";
    public string IdleParm = "Idle";
    public float Skill01Time = 10;
    public float Skill02Time = 10;
    public float SkillAttackTime = 6;
    public float MoveSpeed = 8;
    public bool DirLeft = true;
    public Transform FirePos;
    public Transform Target;
    public BoxCollider2D AttackBox;
    public BoxCollider2D Skill1Box;
    public BoxCollider2D Skill2Box;
    public BoxCollider2D DetectBox;
    public GameObject Skill01DamageZone;
    public GameObject Skill02DamageZone;
    public GameObject[] Iceberg;
    public GameObject[] IcebergEffect;
    public GameObject Ring;
    public MMFeedbacks Skill01Feedbacks;
    public MMFeedbacks Skill02Feedbacks;
    public MMFeedbacks Skill03Feedbacks;
    public GameObject FangEffect;

    private BossState state;
    private Health health;
    private Animator _anim;
    private BossFOV FOV;
    private Rigidbody2D rb;
    private float curTime = 0;
    private float Skill01curTime = 4.0f;
    private float Skill02curTime = 5.0f;
    private float Skill03curTime = 5.0f;
    private bool isFlipped = false;
    private int UseSkill = 0;
    private GameObject Bullet;
    private int RandomVal = 0;

    void Start()
    {
        Init();
    }

    // Update is called once per frame
    void Update()
    {
        switch (state)
        {
            case BossState.IDLE:
                {

                    if (curTime <= 0)
                        Think();

                    LookAtPlayer();
                    curTime -= Time.deltaTime;
                }
                break;
            case BossState.SKILL1:
                {

                    curTime -= Time.deltaTime;
                }
                break;
            case BossState.SKILL2:
                {

                    curTime -= Time.deltaTime;
                }
                break;
            case BossState.SKILL3:
                {

                    curTime -= Time.deltaTime;
                }
                break;
            case BossState.WALK:
                {
                    if(FOV.TargetDetect(Skill1Box))
                    {
                        _anim.SetBool(WalkParam, false);
                        Skill01Process();
                    }
                    else
                    {
                        if(FOV.WallCheck())
                        {
                            _anim.SetBool(WalkParam, false);
                            Skill02Process();
                        }
                        else
                        {
                            if(curTime <= 0)
                            {
                                _anim.SetBool(WalkParam, false);
                                Skill02Process();
                            }
                        }
                    }

                    transform.Translate(Vector2.left * MoveSpeed * Time.deltaTime);
                    curTime -= Time.deltaTime;
                }
                break;
            case BossState.WAIT:
                {
                    if(FOV.TargetDetect(DetectBox))
                    {
                        Target = FOV.target;
                        bossZone.SpawnDoor();
                        SetState(BossState.IDLE);
                    }
                }
                break;
            case BossState.DEAD:
                {

                }
                break;
        }
    }

    private void Think()
    {
        if(UseSkill >= 3)
        {
            Skill03Process();
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

    private void Skill01Process()
    {   
        curTime = Skill01curTime;
        _anim.SetBool(Skill01Param, true);
        FangEffect.SetActive(true);
        UseSkill++;
        SetState(BossState.SKILL1);
    }

    private void Skill02Process()
    {
        curTime = Skill02curTime;
        _anim.SetBool(Skill02Param, true);
        SetState(BossState.SKILL2);
    }

    private void Skill03Process()
    {
        UseSkill = 0;
        curTime = Skill03curTime;
        _anim.SetBool(Skill03Param, true);
        StartCoroutine(Skill03());
        SetState(BossState.SKILL3);
    }

    IEnumerator Skill03()
    {
        Ring.SetActive(true);
        yield return new WaitForSeconds(2.0f);
        Ring.SetActive(false);
        _anim.SetBool(Skill03Param, false);
        SetState(BossState.IDLE);
    }

    private void WalkProcess()
    {
        curTime = 2.0f;
        _anim.SetBool(WalkParam,true);
        SetState(BossState.WALK);
    }

    public void Skill01DamageZoneOnOff()
    {
        if (Skill01DamageZone.activeSelf)
        {
            Skill01DamageZone.SetActive(false);
            _anim.SetBool(Skill01Param, false);
            SetState(BossState.IDLE);
        }
        else
            Skill01DamageZone.SetActive(true);
    }

    public void Skill02DamageZoneOnOff()
    {
        if (Skill02DamageZone.activeSelf)
        {
            Skill02DamageZone.SetActive(false);
            UseSkill++;
            _anim.SetBool(Skill02Param, false);
            SetState(BossState.IDLE);
        }
        else
        {
            if (Bullet != null)
            {
                Instantiate(IcebergEffect[RandomVal], Bullet.transform.position, Bullet.transform.rotation);
                Destroy(Bullet);
            }

            Skill02DamageZone.SetActive(true);
            RandomVal = Random.Range(0, Iceberg.Length);
            Vector3 SpawnPos = new Vector3(Target.transform.position.x, transform.position.y);
            Bullet = Instantiate(Iceberg[RandomVal], SpawnPos, Target.transform.rotation);
        }
    }

    public void DeadFunc()
    {
        StopAllCoroutines();

        bossZone.Clear = true;
        bossZone.DisappearDoor();
        _anim.SetTrigger("Dead");
        SetState(BossState.DEAD);
    }

    public void Skill01Feedback()
    {
        Skill01Feedbacks.PlayFeedbacks();
    }

    public void Skill02Feedback()
    {
        Skill02Feedbacks.PlayFeedbacks();
    }

    public void Skill03Feedback()
    {
        Skill03Feedbacks.PlayFeedbacks();
    }

    private void SetState(BossState TargetState)
    {
        if (TargetState == state)
            return;
        else
            state = TargetState;
    }

    private void DataLoad()
    {
        MoveSpeed = (float)GetComponent<MonsterDataLoad>().Speed;
        //MovementSpeed = WalkSpeed;
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


    private void Init()
    {
        health = GetComponent<Health>();
        FOV = GetComponent<BossFOV>();
        rb = GetComponent<Rigidbody2D>();
        _anim = GetComponent<Animator>();
        DataLoad();
        SetState(BossState.WAIT);
    }



}

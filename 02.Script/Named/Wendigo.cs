using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MoreMountains.CorgiEngine;
using MoreMountains.Feedbacks;

public class Wendigo : MonoBehaviour
{

    private enum BossState
    {
        IDLE,
        SKILL1,
        SKILL2,
        SKILL3,
        WALK,
        WAIT,
        SLEEP,
        DEAD
    }

    public BossZone bossZone;
    public string Skill01ReadyParam = "Ready";
    public string Skill01Param = "Skill01";
    public string Skill02Param = "Skill02";
    public string Skill03Param = "Skill03";
    public string WalkParam = "Walk";
    public string JumpParam = "Jump";
    public string IdleParm = "Idle";
    public float Skill01Time = 10;
    public float Skill02Time = 10;
    public float SkillAttackTime = 6;
    public float MoveSpeed = 7;
    public bool DirLeft = true;
    public Transform Target;
    public GameObject Skill01DamageZone;
    public BoxCollider2D Skill1Box;
    public BoxCollider2D TargetDetectBox;
    public Transform[] Skill02Pos;
    public GameObject WarningLines;
    public GameObject Layzers;
    public Transform[] IceBergPos;
    public GameObject[] IceBergs;
    public MMFeedbacks Skill01ReadyFeedbacks;
    public MMFeedbacks Skill01RunFeedbacks;
    public MMFeedbacks Skill01ReturnFeedbacks;
    public MMFeedbacks Skill02Feedbacks;
    public MMFeedbacks Skill03Feedbacks;
    public GameObject Skill03Effect;
    public GameObject Skill01Effect;

    private BossState state;
    private Health health;
    private Animator _anim;
    private BossFOV FOV;
    private Rigidbody2D rb;
    private float curTime = 0;
    private bool isFlipped = false;
    private int UseSkill = 0;
    private Transform Movetarget;

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
                    {
                        LookAtPlayer();
                        Think();
                    }


                    curTime -= Time.deltaTime;
                }
                break;
            case BossState.SKILL1:
                {
                    if(curTime <= 0)
                    {
                        UseSkill++;
                        _anim.SetBool(Skill01Param, false);
                        Skill01Effect.SetActive(false);
                        SetState(BossState.IDLE);
                    }
                    else if(FOV.WallCheck() || FOV.TargetDetect())
                    {
                        UseSkill++;
                        _anim.SetBool(Skill01Param, false);
                        curTime = 0.2f;
                        Skill01Effect.SetActive(false);
                        SetState(BossState.IDLE);
                    }
                    transform.Translate(Vector2.left * MoveSpeed * Time.deltaTime);
                    curTime -= Time.deltaTime;
                }
                break;
            case BossState.SKILL2:
                {
                    float Dist = Vector2.Distance(Movetarget.position, transform.position);
                    if(Dist <= 0.3f)
                    {
                        _anim.SetBool(WalkParam, false);
                        LookAtPlayer();
                        _anim.SetBool(Skill02Param, true);
                        StartCoroutine(Skill02());
                        SetState(BossState.WAIT);
                    }

                    transform.Translate(Vector2.left * MoveSpeed/2 * Time.deltaTime);

                }
                break;
            case BossState.WALK:
                {

                    if(FOV.TargetDetect(Skill1Box))
                    {
                        _anim.SetBool(WalkParam, false);
                        Skill01Process();
                        SetState(BossState.WAIT);
                    }
                    else
                    {
                        if (FOV.WallCheck())
                        {
                            curTime = 6.0f;
                            Skill02Process();
                            //SetState(BossState.SKILL2);
                        }
                        else
                        {
                            if (curTime <=0)
                            {
                                curTime = 6.0f;
                                Skill02Process();
                                //SetState(BossState.SKILL2);
                            }
                        }
                    }
                                                      
                    transform.Translate(Vector2.left * MoveSpeed/2 * Time.deltaTime);
                    curTime -= Time.deltaTime;
                }
                break;
            case BossState.SKILL3:
                {

                    curTime -= Time.deltaTime;
                }
                break;
            case BossState.WAIT:
                {
                    
                    curTime -= Time.deltaTime;
                }
                break;
            case BossState.SLEEP:
                {
                    if (FOV.TargetDetect(TargetDetectBox))
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
        if (UseSkill >= 4)
            Skill03Process();
        else
        {
            if(FOV.TargetDetect(Skill1Box))
            {
                Skill01Process();
            }
            else
            {
                WalkProcess();
                //Skill02Process();
            }
        }
    }

    public void Skill01DamageZoneOnOff()
    {
        if (Skill01DamageZone.activeSelf)
            Skill01DamageZone.SetActive(false);
        else
            Skill01DamageZone.SetActive(true);
    }

    private void Skill01Process()
    {
        _anim.SetBool(Skill01ReadyParam,true);
        StartCoroutine(Skill01());
    }

    IEnumerator Skill01()
    {
        SetState(BossState.WAIT);
        yield return new WaitForSeconds(1.0f);
        curTime = 2.0f;
        _anim.SetBool(Skill01ReadyParam, false);
        Skill01Effect.SetActive(true);
        _anim.SetBool(Skill01Param, true);
        SetState(BossState.SKILL1);
    }

    private void Skill02Process()
    {
        _anim.SetBool(WalkParam, true);
        float DistA = Vector2.Distance(Skill02Pos[0].position, transform.position);
        float DistB = Vector2.Distance(Skill02Pos[1].position, transform.position);

        Movetarget = DistA <= DistB ? Skill02Pos[0] : Skill02Pos[1];
        LookAtTarget(Movetarget);
        SetState(BossState.SKILL2);
    }

    IEnumerator Skill02()
    {
        int i = 0;
        yield return new WaitForSeconds(1.0f);

        i = Movetarget == Skill02Pos[0] ? 0 : 1;
        WarningLines.SetActive(true);
        yield return new WaitForSeconds(0.5f);
        WarningLines.SetActive(false);
        Layzers.SetActive(true);
        yield return new WaitForSeconds(2.0f);
        Layzers.SetActive(false);
        _anim.SetBool(Skill02Param, false);
        UseSkill++;
        SetState(BossState.IDLE);
    }

    private void WalkProcess()
    {
        _anim.SetBool(WalkParam, true);
        curTime = 10.0f;
        SetState(BossState.WALK);
    }

    private void Skill03Process()
    {
        curTime = 4.0f;
        _anim.SetBool(Skill03Param, true);
        StartCoroutine(Skill03());
        SetState(BossState.SKILL3);
    }

    IEnumerator Skill03()
    {
        List<GameObject> tempIceBerg = new List<GameObject>();
        yield return new WaitForSeconds(2.0f);
        Skill03Effect.SetActive(true);
        _anim.SetBool(Skill03Param, false);
        //IceBerg.SetActive(true);
        for (int i = 0; i < IceBergPos.Length; i++)
        {
            int RandomVal = Random.Range(0, IceBergs.Length);
            tempIceBerg.Add(Instantiate(IceBergs[RandomVal], IceBergPos[i].position, IceBergPos[i].rotation));
        }

        yield return new WaitForSeconds(0.3f);
        //IceBerg.SetActive(false);
        for (int i = 0; i < tempIceBerg.Count; i++)
        {
            tempIceBerg[i].GetComponent<Health>().Kill();
        }
        UseSkill = 0;
        SetState(BossState.IDLE);
    }

    public void DeadFunc()
    {
        StopAllCoroutines();

        bossZone.Clear = true;
        bossZone.DisappearDoor();
        _anim.SetTrigger("Dead");
        SetState(BossState.DEAD);
    }

    private void SetState(BossState TargetState)
    {
        if (TargetState == state)
            return;
        else
            state = TargetState;
    }

    public void Skill01ReadyFeedback()
    {
        Skill01ReadyFeedbacks.PlayFeedbacks();
    }

    public void Skill01RunFeedback()
    {
        Skill01RunFeedbacks.PlayFeedbacks();
    }

    public void Skill01ReturnFeedback()
    {
        Skill01ReturnFeedbacks.PlayFeedbacks();
    }

    public void Skill02Feedback()
    {
        Skill02Feedbacks.PlayFeedbacks();
    }

    public void Skill03Feedback()
    {
        Skill03Feedbacks.PlayFeedbacks();
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

    private void LookAtTarget(Transform Pos)
    {
        Vector3 flipped = transform.localScale;
        flipped.z *= -1f;
        if (transform.position.x > Pos.position.x && isFlipped)
        {
            transform.localScale = flipped;
            transform.Rotate(0f, 180f, 0f);
            isFlipped = false;
            FOV.RayDir = -FOV.RayDir;
        }
        else if (transform.position.x < Pos.position.x && !isFlipped)
        {
            transform.localScale = flipped;
            transform.Rotate(0f, 180f, 0f);
            isFlipped = true;
            FOV.RayDir = -FOV.RayDir;
        }
    }

    private void DataLoad()
    {
        MoveSpeed = (float)GetComponent<MonsterDataLoad>().Speed;
        //MovementSpeed = WalkSpeed;
    }

    private void Init()
    {
        health = GetComponent<Health>();
        FOV = GetComponent<BossFOV>();
        rb = GetComponent<Rigidbody2D>();
        _anim = GetComponent<Animator>();
        DataLoad();
        SetState(BossState.SLEEP);
    }

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MoreMountains.CorgiEngine;
using MoreMountains.Feedbacks;

public class SuperRedReaper : MonoBehaviour
{
    public enum SuperReaperState
    {
        IDLE,
        SKILL01,
        SKILL02,
        SKILL03,
        SKILL04,
        WALK,
        DEAD,
        APPEAR,
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
    public Transform Target;
    public GameObject Skill01Bullet;
    public List<GameObject> Skill02Pos = new List<GameObject>();
    public List<GameObject> Skill03Pos = new List<GameObject>();
    public GameObject[] StackGlow;
    public GameObject Skill03Marble;
    public GameObject Skill03Obj;
    public GameObject Skill04Obj;
    public Transform[] Skill02DistCheckPos;
    public GameObject BossClearObj;
    public MMFeedbacks FireFeedbacks;
    public MMFeedbacks IceFeedbacks;
    public MMFeedbacks EarthFeedbacks;
    public MMFeedbacks PoisonFeedbacks;
    public MMFeedbacks AppearFeedbacks;
    public MMFeedbacks AppearFeedbacks2;
    public MMFeedbacks AppearFeedbacks3;
    public MMFeedbacks DeathFeedbacks;
    public MMFeedbacks Death2Feedbacks;
    public GameObject SpawnEffect;

    private SuperReaperState state;
    private Health health;
    private Animator _anim;
    private BossFOV FOV;
    private float curTime = 0;
    public bool isFlipped = true;
    private Rigidbody2D rb;
    private int Stack = 0;
    private int StackCheck = 0;
    private int MaxHelath;
    private GameObject tempobj;

    private void Start()
    {
        Init();
    }

    private void Update()
    {
        switch (state)
        {
            case SuperReaperState.IDLE:
                {

                    HealthCheck(health.CurrentHealth);
                    WalkProcess();

                    if (curTime <= 0)
                        Think();

                    curTime -= Time.deltaTime;
                }
                break;
            case SuperReaperState.SKILL01:
                {
                    if (curTime <= 0)
                    {
                        curTime = 4.0f;
                        _anim.SetBool(Skill01Param, false);
                        SetState(SuperReaperState.IDLE);
                    }


                    curTime -= Time.deltaTime;
                }
                break;
            case SuperReaperState.SKILL02:
                {

                }
                break;
            case SuperReaperState.SKILL03:
                {
                    if(tempobj == null || !tempobj.activeSelf)
                    {
                        if(curTime <= 0)
                        Skill03();

                    }

                    curTime -= Time.deltaTime;
                }
                break;
            case SuperReaperState.SKILL04:
                {
                    if (curTime <= 0)
                    {
                        curTime = 4.0f;
                        _anim.SetBool(Skill04Param, false);
                        Skill04Obj.SetActive(false);
                        SetState(SuperReaperState.IDLE);
                    }

                    curTime -= Time.deltaTime;
                }
                break;
            case SuperReaperState.WALK:
                {
                    HealthCheck(health.CurrentHealth);
                    IdleProcess();

                    if (FOV.WallCheck())
                    {
                        Skill02Process();
                    }
                   
                    if (curTime <= 0)
                        Think();

                    curTime -= Time.deltaTime;

                    transform.Translate(Vector2.right * MoveSpeed * Time.deltaTime);

                }
                break;
            case SuperReaperState.DEAD:
                break;
            case SuperReaperState.APPEAR:
                {
                    if (curTime <= 0)
                    {
                        Vector3 dir = (Target.position - transform.position).normalized;
                        dir.y = 0;
                        FOV.RayDir = dir;
                        bossZone.BossObject = this.gameObject;
                        health.Invulnerable = false;
                        SetState(SuperReaperState.IDLE);
                    }

                    curTime -= Time.deltaTime;
                }
                break;
        }
    }


    private void Think()
    {
        if (FOV.WallCheck())
        {
            Skill02Process();
        }
        else
        {
            Skill01Process();
        }
    }

    private void Skill01Process()
    {
        _anim.SetBool(Skill01Param, true);
        curTime = 1.0f;
        SetState(SuperReaperState.SKILL01);
    }

    public void Skill01Fire()
    {
        GameObject tempBullet = Instantiate(Skill01Bullet, transform.position + new Vector3(0,-1,0), transform.rotation);
        Vector3 dir = (Target.position - transform.position).normalized;
        
        if (transform.rotation == Quaternion.identity)
        {
            tempBullet.GetComponent<Projectile>().Direction = new Vector3(dir.magnitude, 0, 0);
        }
        else
        {
            tempBullet.GetComponent<Projectile>().Direction = new Vector3(-dir.magnitude, 0, 0);
        }
    }

    private void Skill02Process()
    {

        //curTime = 2.0f;
        
        _anim.SetBool(Skill02Param, true);
        float DistA = Vector2.Distance(Skill02DistCheckPos[0].position, transform.position);
        float DistB = Vector2.Distance(Skill02DistCheckPos[1].position, transform.position);
        Transform Movetarget = DistA <= DistB ? Skill02DistCheckPos[0] : Skill02DistCheckPos[1];
        LookAtTarget(Movetarget);
        bool Left = false;
        if (Movetarget == Skill02DistCheckPos[0])
            Left = true;
        else
            Left = false;

        StartCoroutine(Skill02(Left));

        SetState(SuperReaperState.SKILL02);
        //SetState(SuperReaperState.SKILL02);
    }

    IEnumerator Skill02(bool Left)
    {
        
        if (Left)
        {
            int cnt = 0;
            while (cnt < Skill02Pos.Count)
            {
                Skill02Pos[cnt].SetActive(true);
                yield return new WaitForSeconds(0.2f);
                cnt++;
            }
        }
        else
        {
            int cnt = Skill02Pos.Count-1;
            while (cnt > 0)
            {
                Skill02Pos[cnt].SetActive(true);
                yield return new WaitForSeconds(0.2f);
                cnt--;
            }
        }

        curTime = 4.0f;
        _anim.SetBool(Skill02Param, false);
        SetState(SuperReaperState.IDLE);

    }

    private void Skill03Process()
    {
        _anim.SetBool(Skill03Param, true);
        curTime = 1.0f;
        SetState(SuperReaperState.SKILL03);
    }

    private void Skill03()
    {
        for (int i = 0; i < Skill03Pos.Count; i++)
        {
            Instantiate(Skill03Obj, Skill03Pos[i].transform.position, Skill03Pos[i].transform.rotation);
        }
        curTime = 4.0f;
        _anim.SetBool(Skill03Param, false);
        SetState(SuperReaperState.IDLE);
    }

    public void Skill03Fire()
    {
        tempobj = Instantiate(Skill03Marble, transform.position, transform.rotation);
        tempobj.GetComponent<Rigidbody2D>().AddForce(10.0f * Vector2.right);
    }

   
    private void Skill04Process()
    {
        _anim.SetBool(Skill04Param, true);
        curTime = 1.5f;
        Skill04Obj.SetActive(true);
        SetState(SuperReaperState.SKILL04);
    }


    private void WalkProcess()
    {
        if (DistCheck() >= 1.0f)
        {

            LookAtPlayer();
            _anim.SetBool(WalkParam, true);


            SetState(SuperReaperState.WALK);
        }
    }

    private void IdleProcess()
    {
        if (DistCheck() < 1.0f)
        {
            _anim.SetBool(WalkParam, false);
            SetState(SuperReaperState.IDLE);
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

    private float DistCheck()
    {
        Vector2 TargetX = new Vector2(Target.position.x, transform.position.y);
        float Dist = Vector2.Distance(transform.position, TargetX);

        return Dist;
    }

    public void DeadFunc()
    {
        StopAllCoroutines();
        bossZone.Clear = true;
        if(BossClearObj)
            BossClearObj.SetActive(true);

        bossZone.DisappearDoor();
        _anim.SetTrigger("Dead");
        SetState(SuperReaperState.DEAD);
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

    private void HealthCheck(int curHp)
    {
       if(MaxHelath != curHp)
        {
            StackCheck++;
            MaxHelath = curHp;
            if(StackCheck >= 2)
            {               
                StackCheck = 0;
                StackGlowProcess(Stack);
            }
        }
        
    }

    private void StackGlowProcess(int stack)
    {
        switch (stack)
        {
            case 0:
                {
                    StackGlow[stack].SetActive(true);
                    Stack++;
                }
                break;
            case 1:
                {
                    StackGlow[stack - 1].SetActive(false);
                    StackGlow[stack].SetActive(true);
                    Stack++;
                }
                break;
            case 2:
                {
                    StackGlow[stack - 1].SetActive(false);
                    int RandomVal = Random.Range(0, 2);
                    if (RandomVal == 0)
                    {
                        Skill03Process();
                        Stack = 0;
                    }
                    else
                    {
                        Skill04Process();
                        Stack = 0;
                    }
                }
                break;
        }
    }

    public void FireFeedback()
    {
        FireFeedbacks.PlayFeedbacks();
    }

    public void IceFeedback()
    {
        IceFeedbacks.PlayFeedbacks();
    }

    public void EarthFeedback()
    {
        EarthFeedbacks.PlayFeedbacks();
    }

    public void PoisonFeedback()
    {
        PoisonFeedbacks.PlayFeedbacks();
    }

    public void AppearFeedback()
    {
        AppearFeedbacks.PlayFeedbacks();
    }

    public void AppearFeedback2()
    {
        //SpawnEffect.SetActive(true);
        Destroy(SpawnEffect, 5);
       // AppearFeedbacks2.PlayFeedbacks();
    }

    public void AppearFeecback3()
    {
        AppearFeedbacks3.PlayFeedbacks();
    }

    public void DeathFeedback()
    {
        DeathFeedbacks.PlayFeedbacks();
    }

    public void DeathFeedback2()
    {
        Death2Feedbacks.PlayFeedbacks();
    }   

    private void Init()
    {
        rb = GetComponent<Rigidbody2D>();
        health = GetComponent<Health>();
        FOV = GetComponent<BossFOV>();
        _anim = GetComponent<Animator>();
        curTime = 4.0f;
        DataLoad();
        health.Invulnerable = true;
        MaxHelath = health.MaximumHealth;
        state = SuperReaperState.APPEAR;
    }

    private void DataLoad()
    {
        MoveSpeed = (float)GetComponent<MonsterDataLoad>().Speed;
        // MovementSpeed = WalkSpeed;
    }

    private void SetState(SuperReaperState TargetState)
    {
        if (TargetState == state)
            return;
        else
            state = TargetState;
    }

}

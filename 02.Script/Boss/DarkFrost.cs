using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MoreMountains.CorgiEngine;
using MoreMountains.Feedbacks;

public class DarkFrost : MonoBehaviour
{
    private enum DarkFrostState
    {
        IDLE,
        SKILL01,
        SKILL02,
        SKILL03,
        SKILL04,
        SKILL05,
        CHANGE,
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
    public string Skill05Param = "Skill05";
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
    public Transform[] MovePos;
    public GameObject Skill01Bullet;
    public GameObject Storm;
    public GameObject IceBreath;
    public GameObject Skill04Bullet;
    public GameObject Skill05Bulelt;
    public Transform[] Skill05Pos;
    public GameObject[] Skill05Effect;
    public GameObject TransParentEffect;
    public GameObject AppearEffect_1;
    public GameObject AppearEffect_2;
    public Transform BossPos;
    public MMFeedbacks WalkFeedback;
    public MMFeedbacks AppearShakeFeedback;
    public MMFeedbacks Skill01Feedbacks;
    public MMFeedbacks Skill02Feedbacks;
    public MMFeedbacks Skill03Feedbacks;

    private Rigidbody2D rb;
    private DarkFrostState state = DarkFrostState.SLEEP;
    private Health health;
    private Animator _anim;
    private BossFOV FOV;
    private float curTime = 0;
    private bool isFlipped = false;
    private bool Phase2 = false;
    private int UseSkillCnt = 0;
    private bool Left = true;
    private Vector3 StormPos;
    private SpriteRenderer sprite;
    private Vector3 startPos;
    private float[] RandomRange = new float[7] {-0.7f, -0.4f,-0.2f,0f,0.2f,0.4f,0.7f};
    private bool isSkill01 = false;

    void Start()
    {
        
        rb = GetComponent<Rigidbody2D>();
        health = GetComponent<Health>();
        FOV = GetComponent<BossFOV>();
        _anim = GetComponent<Animator>();
        sprite = GetComponent<SpriteRenderer>();
        curTime = 1.5f;
        UseSkillCnt = 1;
        DataLoad();
        state = DarkFrostState.SLEEP;
    }

    private void Update()
    {
        switch (state)
        {
            case DarkFrostState.IDLE:
                {
                    if (curTime <= 0)
                        Think();

                    Vector2 BossVector = new Vector2(transform.position.x, BossPos.position.y);

                    if (Left)
                    {
                        //float Dist = Vector2.Distance(MovePos[0].position, transform.position);
                        float Dist = Vector2.Distance(MovePos[0].position, BossVector);

                        if (Dist <= 0.1f)
                            Left = false;
                        LookAtTarget(MovePos[0]);
                        transform.Translate(Vector2.left * MoveSpeed  * Time.deltaTime);
                    }
                    else
                    {
                        //float Dist = Vector2.Distance(MovePos[1].position, transform.position);
                        float Dist = Vector2.Distance(MovePos[1].position, BossVector);

                        if (Dist <= 0.1f)
                            Left = true;
                        LookAtTarget(MovePos[1]);
                        transform.Translate(Vector2.left * MoveSpeed  * Time.deltaTime);
                    }

                    curTime -= Time.deltaTime;
                    
                }
                break;
            case DarkFrostState.SKILL01:
                {
                    if (curTime <= 0)
                    {
                        _anim.SetBool(Skill01Param, false);
                        SetState(DarkFrostState.IDLE);
                    }

                    curTime -= Time.deltaTime;
                }
                break;
            case DarkFrostState.SKILL02:
                {

                }
                break;
            case DarkFrostState.SKILL03:
                {
                    if (Left)
                    {

                        Vector2 BossVector = new Vector2(transform.position.x, BossPos.position.y);

                        //float Dist = Vector2.Distance(MovePos[0].position, transform.position);
                        float Dist = Vector2.Distance(MovePos[0].position, BossVector);

                        if (Dist <= 0.1f)
                            Left = false;
                        LookAtTarget(MovePos[0]);
                        transform.Translate(Vector2.left * MoveSpeed / 2 * Time.deltaTime);
                    }
                    else
                    {
                        float Dist = Vector2.Distance(MovePos[1].position, transform.position);
                        if (Dist <= 0.1f)
                            Left = true;
                        LookAtTarget(MovePos[1]);
                        transform.Translate(Vector2.left * MoveSpeed / 2 * Time.deltaTime);
                    }


                    curTime -= Time.deltaTime;
                }
                break;
            case DarkFrostState.SKILL04:
                {

                    if(curTime <= 0)
                    {
                        _anim.SetBool(Skill04Param, false);
                        SetState(DarkFrostState.IDLE);
                    }

                    curTime -= Time.deltaTime;
                }
                break;
            case DarkFrostState.SKILL05:
                {

                }
                break;
            case DarkFrostState.CHANGE:
                {
                    if (curTime <= 0)
                    {
                        SetState(DarkFrostState.IDLE);
                    }

                    curTime -= Time.deltaTime;
                }
                break;
            case DarkFrostState.DEAD:
                {

                }
                break;
            case DarkFrostState.APPEAR:
                {
                    float Dist = Vector2.Distance(BossPos.position, transform.position);

                    if (Dist < 0.1f)
                    {
                        _anim.SetBool(AppearParam, false);
                        transform.position = BossPos.position;
                        sprite.color = Color.white;
                        AppearEffect_1.SetActive(false);
                        AppearEffect_2.SetActive(false);
                        startPos = transform.position;
                        SetState(DarkFrostState.IDLE);
                    }

                    transform.Translate(Vector2.up * MoveSpeed * Time.deltaTime);
                    curTime -= Time.deltaTime;
                }
                break;
            case DarkFrostState.SLEEP:
                {
                    if (curTime <= 0)
                    {
                        sprite.color = new Color(sprite.color.r, sprite.color.g, sprite.color.b, 255f/ 255f);
                        Debug.Log(sprite.color);
                        SetState(DarkFrostState.APPEAR);
                    }

                    curTime -= Time.deltaTime;
                }
                break;
        }
    }

    private void Think()
    {
        if(HealthCheck(health.CurrentHealth))
        {
            Think2Phase();
        }
        else
        {
            switch (UseSkillCnt)
            {
                case 1:
                    {
                        Skill01Process();
                    }
                    break;
                case 2:
                    {
                        Skill02Process();
                    }
                    break;
                case 3:
                    {
                        Skill03Process();
                    }
                    break;
                case 4:
                    {
                        Skill04Process();
                    }
                    break;
                case 0:
                    {
                        if (!Phase2)
                            UseSkillCnt++;
                    }
                    break;
            }
        }
    }

    private void Think2Phase()
    {
        if(Phase2)
        {
            if(UseSkillCnt == 0)
            {
                curTime = Skill05Time;
                SetState(DarkFrostState.SKILL05);
                _anim.SetBool(Skill05Param, true);
                Skill05Process();
            }
            else
            {
                switch (UseSkillCnt)
                {
                    case 1:
                        {
                            Skill01Process();
                        }
                        break;
                    case 2:
                        {
                            Skill02Process();
                        }
                        break;
                    case 3:
                        {
                            Skill03Process();
                        }
                        break;
                    case 4:
                        {
                            Skill04Process();
                        }
                        break;
                }
            }
        }
        else
        {
            SetState(DarkFrostState.CHANGE);
            Phase2 = true;
            curTime = 5.0f;
            _anim.SetBool(Phase2Param, true);
            UseSkillCnt = 0;
        }
    }


    private void Skill01Process()
    {
        isSkill01 = true;
        StartCoroutine(Skill01());
        curTime = Skill01Time;
        SetState(DarkFrostState.SKILL01);
        
    }

    IEnumerator Skill01()
    {
        yield return new WaitForSeconds(1.0f);       
        _anim.SetBool(Skill01Param, true);
    }

    private void Skill02Process()
    {

        StartCoroutine(Skill02());
        SetState(DarkFrostState.SKILL02);
    }

    IEnumerator Skill02()
    {
        curTime = Skill02Time;
        _anim.SetBool(Skill02Ready, true);
        StormPos = new Vector3(Target.transform.position.x, transform.position.y);
        yield return new WaitForSeconds(1.0f);
        _anim.SetBool(Skill02Param, true);
        _anim.SetBool(Skill02Ready, false);
        health.Invulnerable = true;
        yield return new WaitForSeconds(1.0f);
        TransParentsOff();
        _anim.SetBool(Skill02Param, false);
        Instantiate(Storm, StormPos,Quaternion.identity);
        yield return new WaitForSeconds(0.5f);
        //_anim.SetBool(Skill02Param, false);
        UseSkillCnt++;
        SetState(DarkFrostState.IDLE);
    }

    private void Skill03Process()
    {
        StartCoroutine(Skill03());
        SetState(DarkFrostState.SKILL03);
    }

    IEnumerator Skill03()
    {
        curTime = Skill03Time;
        _anim.SetBool(Skill03Param, true);
        yield return new WaitForSeconds(0.5f);
        IceBreath.SetActive(true);
        yield return new WaitForSeconds(2.0f);       
        _anim.SetBool(Skill03Param, false);
        IceBreath.SetActive(false);
        //IceBreath.GetComponent<Animator>().SetTrigger(TriggerParam);
        UseSkillCnt++;
        SetState(DarkFrostState.IDLE);
    }

    private void Skill04Process()
    {
        curTime = Skill04Time;
        isSkill01 = false;
        _anim.SetBool(Skill04Param, true);
        SetState(DarkFrostState.SKILL04);
    }

    private void Skill05Process()
    {
        
        StartCoroutine(Skill05());
        
    }

    IEnumerator Skill05()
    {
        SetState(DarkFrostState.SKILL05);
        curTime = Skill05Time;
        _anim.SetBool(Skill05Ready, true);
        yield return new WaitForSeconds(1.0f);
        _anim.SetBool(Skill05Ready, false);
        _anim.SetBool(Skill05Param, true);
        health.Invulnerable = true;
        int MoveCnt = 0;
        while (MoveCnt < 5)
        {
            Skill05Effect[0].SetActive(true);
            for (int i = 0; i < Skill05Pos.Length; i++)
            {
                Vector2 dir = new Vector2(RandomRange[Random.Range(0, RandomRange.Length)], -1f);
                GameObject tempBullet = Instantiate(Skill05Bulelt, Skill05Pos[i].position, Skill05Pos[i].rotation);
                tempBullet.GetComponent<Projectile>().Direction = dir;
            }

            yield return new WaitForSeconds(0.3f);

            Skill05Effect[1].SetActive(true);

            for (int i = 0; i < Skill05Pos.Length; i++)
            {
                Vector2 dir = new Vector2(RandomRange[Random.Range(0, RandomRange.Length)], -1f);
                GameObject tempBullet = Instantiate(Skill05Bulelt, Skill05Pos[i].position, Skill05Pos[i].rotation);
                tempBullet.GetComponent<Projectile>().Direction = dir;
            }

            yield return new WaitForSeconds(0.3f);
            MoveCnt++;
        }

        transform.position = startPos;
        TransParentsOff();
        UseSkillCnt++;
        _anim.SetBool(Skill05Param, false);
    }

    public void Skill01Fire()
    {
        if (!isSkill01)
            return;

        Vector3 dir = (Target.position - transform.position).normalized;
        GameObject tempbullet = Instantiate(Skill01Bullet, transform.position, transform.rotation);      
        tempbullet.GetComponent<Projectile>().Direction = dir;
        UseSkillCnt++;
    }

    public void Skill04Fire()
    {
        if (isSkill01)
            return;

        GameObject tempbullet = Instantiate(Skill04Bullet, Target.position, Target.rotation);       
        UseSkillCnt = 0;

        
    }

    public void TransParents()
    {
        StartCoroutine(Trans());
        //sprite.color = Color.clear;
        //TransParentEffect.SetActive(true);
    }

    IEnumerator Trans()
    {
        sprite.color = Color.clear;
        TransParentEffect.SetActive(true);
        yield return new WaitForSeconds(0.4f);
        TransParentEffect.SetActive(false);
    }

    private void DataLoad()
    {
        MoveSpeed = (float)GetComponent<MonsterDataLoad>().Speed;
       // MovementSpeed = WalkSpeed;
    }

    

    public void TransParentsOff()
    {
        health.Invulnerable = false;
        sprite.color = Color.white;
        SetState(DarkFrostState.IDLE);
    }

    public void DeadFunc()
    {
        StopAllCoroutines();
        bossZone.Clear = true;
        bossZone.DisappearDoor();
        _anim.SetTrigger("Dead");
        SetState(DarkFrostState.DEAD);
    }

    public void Skill01Feedback()
    {
        Skill01Feedbacks.PlayFeedbacks();
    }

    public void WalkFeedbacks()
    {
        WalkFeedback.PlayFeedbacks();
    }

    public void AppearFeedback()
    {
        AppearShakeFeedback.PlayFeedbacks();
    }

    public void Skill02Feedback()
    {
        Skill02Feedbacks.PlayFeedbacks();
    }

    public void Skill03Feedback()
    {
        Skill03Feedbacks.PlayFeedbacks();
    }


    public void stateStart()
    {
        state = DarkFrostState.IDLE;
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
        }
        else if (transform.position.x < Target.position.x && !isFlipped)
        {
            transform.localScale = flipped;
            transform.Rotate(0f, 180f, 0f);
            isFlipped = true;
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

    private bool HealthCheck(int curHp)
    {
        if (curHp <= (health.MaximumHealth * 0.5f))
            return true;
        else
            return false;
    }

    private void SetState(DarkFrostState TargetState)
    {
        if (TargetState == state)
            return;
        else
            state = TargetState;
    }


}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MoreMountains.CorgiEngine;
using MoreMountains.Feedbacks;

public class BigSnake : MonoBehaviour
{
    private enum BigSnakeState
    {
        IDLE,
        SKILL01,
        SKILL02,
        SKILL03,
        SKILL04,
        DIE,
        CHANGE,
        APPEAR
    }



    public BossZone bossZone;
    public string AppearParam = "Appear";
    public string Skill01Param = "Skill01";
    public string Skill02Param = "Skill02";
    public string Skill03Param = "Skill03";
    public string Skill04Param = "Skill04";
    public string Phase2Param = "2Phase";
    public string IdleParm = "Idle";
    public string ReadyParam = "Ready";
    public string TriggerParam = "Trigger";
    public float Skill01Time = 19;
    public float Skill02Time = 14;
    public float Skill03Time = 9;
    public float Skill04Time = 40;
    public Transform Target;
    public BoxCollider2D PlayerCheckBoxSkill01A;
    public BoxCollider2D PlayerCheckBoxSkill01B;
    public BoxCollider2D PlayerCheckBoxSkill02;
    public Transform FirePos;
    public GameObject[] BressObj;
    public GameObject[] Tail;
    public MMFeedbacks ShakeFeedback;
    public MMFeedbacks Skill1ChargeFeedback;
    public MMFeedbacks Skill1Feedback;
    public MMFeedbacks Skill2ChargeFeedback;
    public MMFeedbacks Skill2Feedback;
    public MMFeedbacks Skill3Feedback;
    public MMFeedbacks Skill4StartFeedback;
    public MMFeedbacks AppearFeedback;
    public MMFeedbacks PhaseFeedback;
    public GameObject PosionOrb;
    public float MoveSpeed = 5.0f;
    public float RayDist = 2;
    public Transform[] PosionMistPos;
    public Transform BulletTarget;
    public GameObject MistObj;
    public GameObject[] SnakeHeads;
    public GameObject[] Beam;

    private BigSnakeState state = BigSnakeState.APPEAR;
    private Health health;
    private Animator _anim;
    private float curTime = 0;
    private float Skill01curTime = 0;
    private float Skill02curTime = 0;
    private float Skill03curTime = 0;
    private float Skill04curTime = 0;
    private bool isFlipped = false;
    private bool Phase2 = false;
    private GameObject MyBullet;
    private SpriteRenderer sprite;
    private Vector3 TaillAttackPos;

    void Start()
    {
        health = GetComponent<Health>();
        _anim = GetComponent<Animator>();
        sprite = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        switch (state)
        {
            case BigSnakeState.IDLE:
                {
                    if (curTime <= 0)
                    {
                        Think();
                    }
                    curTime -= Time.deltaTime;
                    Skill01curTime -= Time.deltaTime;
                    Skill02curTime -= Time.deltaTime;
                    Skill03curTime -= Time.deltaTime;
                    Skill04curTime -= Time.deltaTime;
                }
                break;
            case BigSnakeState.SKILL01:
                {
                    curTime -= Time.deltaTime;
                    Skill01curTime -= Time.deltaTime;
                    Skill02curTime -= Time.deltaTime;
                    Skill03curTime -= Time.deltaTime;
                    Skill04curTime -= Time.deltaTime;
                }
                break;
            case BigSnakeState.SKILL02:
                {
                    curTime -= Time.deltaTime;
                    Skill01curTime -= Time.deltaTime;
                    Skill02curTime -= Time.deltaTime;
                    Skill03curTime -= Time.deltaTime;
                    Skill04curTime -= Time.deltaTime;                   

                    if (MyBullet == null)
                        return;

                    if (MyBullet.activeSelf)
                        return;
                    else
                    {
                        StartCoroutine(Skill02());
                        SetState(BigSnakeState.IDLE);
                    }


                }
                break;
            case BigSnakeState.SKILL03:
                {
                    curTime -= Time.deltaTime;
                    Skill01curTime -= Time.deltaTime;
                    Skill02curTime -= Time.deltaTime;
                    Skill03curTime -= Time.deltaTime;
                    Skill04curTime -= Time.deltaTime;
                }
                break;
            case BigSnakeState.SKILL04:
                {
                    curTime -= Time.deltaTime;
                    Skill01curTime -= Time.deltaTime;
                    Skill02curTime -= Time.deltaTime;
                    Skill03curTime -= Time.deltaTime;
                    Skill04curTime -= Time.deltaTime;
                }
                break;
            case BigSnakeState.CHANGE:
                {
                    if (curTime <= 0)
                    {
                        SetState(BigSnakeState.IDLE);
                    }
                    curTime -= Time.deltaTime;
                    Skill01curTime -= Time.deltaTime;
                    Skill02curTime -= Time.deltaTime;
                    Skill03curTime -= Time.deltaTime;
                    Skill04curTime -= Time.deltaTime;
                }
                break;
            case BigSnakeState.DIE:
                {
                    curTime -= Time.deltaTime;
                    Skill01curTime -= Time.deltaTime;
                    Skill02curTime -= Time.deltaTime;
                    Skill03curTime -= Time.deltaTime;
                    Skill04curTime -= Time.deltaTime;
                }
                break;
            case BigSnakeState.APPEAR:
                {
                    
                }
                break;
        }
    }

    private void OnEnable()
    {
        AppearFeedback.PlayFeedbacks();
    }

    public void TestBtn()
    {
        Skill02Process();
    }

    private void Think()
    {
        if(HealthCheck(health.CurrentHealth))
        {
            if(Phase2)
            {
                Phase2Think();
            }
            else
            {
                PhaseFeedback.PlayFeedbacks();
                curTime = 5.0f;
                _anim.SetBool(Phase2Param,true);
                Phase2 = true;
                SetState(BigSnakeState.CHANGE);
            }
        }
        else
        {
            if(Skill01curTime <= 0)
            {
                if(PlayerCheack(PlayerCheckBoxSkill01A) || PlayerCheack(PlayerCheckBoxSkill01B))
                    Skill01Process();
                else
                {
                    if (PlayerCheack(PlayerCheckBoxSkill02))
                        Skill02Process();
                    else
                        Skill03Process();
                }
            }
            else
            {
                if(Skill02curTime <= 0)
                {
                    if(PlayerCheack(PlayerCheckBoxSkill02))
                        Skill02Process();
                    else
                        Skill03Process();
                }
                else
                {
                    Skill03Process();
                }
            }
        }
    }

    private void Phase2Think()
    {
        if(Skill04curTime <= 0)
        {
            Skill04Process();
        }
        else
        {
            if (Skill01curTime <= 0)
            {
                if (PlayerCheack(PlayerCheckBoxSkill01A) || PlayerCheack(PlayerCheckBoxSkill01B))
                    Skill01Process();
                else
                {
                    if (PlayerCheack(PlayerCheckBoxSkill02))
                        Skill02Process();
                    else
                        Skill03Process();
                }
            }
            else
            {
                if (Skill02curTime <= 0)
                {
                    if (PlayerCheack(PlayerCheckBoxSkill02))
                        Skill02Process();
                    else
                        Skill03Process();
                }
                else
                {
                    Skill03Process();
                }
            }
        }
    }

    private void Skill01Process()
    {
        Skill01curTime = Skill01Time;
        curTime = 12.0f;
        _anim.SetBool(Skill01Param, true);
        StartCoroutine(Skill01());
        SetState(BigSnakeState.SKILL01);
    }

    IEnumerator Skill01()
    {
        
        yield return new WaitForSeconds(0.5f);
        Skill1ChargeFeedback.PlayFeedbacks();
        BressObj[0].SetActive(true);
        Beam[0].SetActive(true);
        yield return new WaitForSeconds(1.0f);
        BressObj[0].SetActive(false);
        Beam[0].SetActive(false);
        BressObj[1].SetActive(true);
        Skill1Feedback.PlayFeedbacks();
        yield return new WaitForSeconds(2.0f);
        _anim.SetBool(ReadyParam, true);
        BressObj[1].GetComponent<Animator>().SetTrigger(TriggerParam);
        yield return new WaitForSeconds(0.5f);
        Skill1ChargeFeedback.PlayFeedbacks();
        BressObj[2].SetActive(true);
        Beam[1].SetActive(true);
        yield return new WaitForSeconds(1.0f);
        BressObj[2].SetActive(false);
        Beam[1].SetActive(false);
        BressObj[3].SetActive(true);
        Skill1Feedback.PlayFeedbacks();
        yield return new WaitForSeconds(2.0f);
        BressObj[3].GetComponent<Animator>().SetTrigger(TriggerParam);
        _anim.SetBool(Skill01Param, false);
        _anim.SetBool(ReadyParam, false);
        SetState(BigSnakeState.IDLE);
    }
    private void Skill02Process()
    {
        Skill02curTime = Skill02Time;
        curTime = 9.0f;
        _anim.SetBool(Skill02Param, true);
        SetState(BigSnakeState.SKILL02);
    }

    IEnumerator Skill02()
    {
        for (int i = 0; i < PosionMistPos.Length; i++)
        {
            Instantiate(MistObj, PosionMistPos[i].position,PosionMistPos[i].rotation);
        }

        yield return new WaitForSeconds(10.0f);
        MyBullet = null;
    }

    private void Skill03Process()
    {        
        curTime = 7.0f;
        //_anim.SetBool(Skill03Param, true);
        StartCoroutine(Skill03());
        SetState(BigSnakeState.SKILL03);
    }

    IEnumerator Skill03()
    {
        int cnt = 0;

        while(cnt < 5)
        {
            TaillAttackPos = Target.transform.position;
            yield return new WaitForSeconds(0.2f);
            Skill3Feedback.PlayFeedbacks();
            _anim.SetBool(Skill03Param, true);
            _anim.SetTrigger(TriggerParam);
            cnt++;
            yield return new WaitForSeconds(1.0f);
        }
        _anim.SetBool(Skill03Param, false);
        SetState(BigSnakeState.IDLE);
    }

    private void Skill04Process()
    {
        curTime = 15.0f;
        Skill04curTime = Skill04Time;
        _anim.SetBool(Skill04Param, true);
        SetState(BigSnakeState.SKILL04);
    }

    IEnumerator Skill04()
    {
        yield return new WaitForSeconds(1.0f);

        SnakeHeads[0].SetActive(true);

        yield return new WaitForSeconds(1.5f);

        SnakeHeads[1].SetActive(true);

        yield return new WaitForSeconds(1.5f);

        SnakeHeads[2].SetActive(true);

        yield return new WaitForSeconds(1.0f);

        sprite.color = Color.white;
        GetComponent<BoxCollider2D>().enabled = true;
        _anim.SetBool(Skill04Param, false);
        SetState(BigSnakeState.IDLE);
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

    public void Bossdisable()
    {
        sprite.color = Color.clear;
        GetComponent<BoxCollider2D>().enabled = false;
        StartCoroutine(Skill04());
    }

    public void DeadFunc()
    {
        StopAllCoroutines();
        for (int i = 0; i < BressObj.Length; i++)
        {
            BressObj[i].SetActive(false);
        }
        for (int i = 0; i < Tail.Length; i++)
        {
            Tail[i].SetActive(false);
        }
        bossZone.Clear = true;
        bossZone.DisappearDoor();
        _anim.SetTrigger("Dead");
        SetState(BigSnakeState.DIE);
    }

    public void TailAttack()
    {
        if(Phase2)
        {
            Tail[1].transform.position = new Vector3(TaillAttackPos.x, Tail[1].transform.position.y, Tail[1].transform.position.z);
            Tail[1].SetActive(true);
        }
        else
        {
            Tail[0].transform.position = new Vector3(TaillAttackPos.x, Tail[0].transform.position.y, Tail[0].transform.position.z);
            Tail[0].SetActive(true);
        }

    }

    public void SnakeBallFire()
    {
        Vector3 dir = (BulletTarget.position - FirePos.position).normalized;
        MyBullet = Instantiate(PosionOrb, FirePos.position, FirePos.rotation);
        Projectile tempBullet = MyBullet.GetComponent<Projectile>();
        tempBullet.Direction = dir;
        _anim.SetBool(Skill02Param, false);
    }

    public void Shake()
    {
        ShakeFeedback.PlayFeedbacks();
    }

    public void stateStart()
    {
        state = BigSnakeState.IDLE;
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

    private bool WallCheck(Vector2 RayDir)
    {
        RaycastHit2D hitInfo = Physics2D.Raycast(transform.position, RayDir, RayDist, LayerMask.GetMask("Platforms"));
        Debug.DrawRay(transform.position, RayDir * RayDist, Color.red);
        if (hitInfo)
        {
            return true;
        }
        return false;
    }

    private bool GroundCheck()
    {
        RaycastHit2D hitInfo = Physics2D.Raycast(transform.position, Vector2.down, RayDist, LayerMask.GetMask("Platforms"));
        Debug.DrawRay(transform.position, RayDist * Vector2.down, Color.red);
        if (hitInfo)
        {
            return true;
        }
        return false;
    }

    private bool HealthCheck(int curHp)
    {
        if (curHp <= (health.MaximumHealth * 0.5f))
            return true;       
        else
            return false;
    }

    private void SetState(BigSnakeState snakeState)
    {
        if (snakeState == state)
            return;
        else
            state = snakeState;
    }

    public void AppearFeedbackFunc()
    {
        AppearFeedback.PlayFeedbacks();
    }

    public void PhaseFeedbackFunc()
    {
        PhaseFeedback.PlayFeedbacks();
    }

    public void Skill2ChargeFeedbackFunc()
    {
        Skill2ChargeFeedback.PlayFeedbacks();
    }

    public void Skill4StartFeedbackFunc()
    {
        Skill4StartFeedback.PlayFeedbacks();
    }
    /*
    private void OnGUI()
    {
        if (GUI.Button(new Rect(200, 0, 200, 80), "skill"))
        {
            TestBtn();
        }
    }*/


}

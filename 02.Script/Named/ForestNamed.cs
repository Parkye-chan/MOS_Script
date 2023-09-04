using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MoreMountains.Feedbacks;

public class ForestNamed : MonoBehaviour
{
    
    enum NamedState
    {
        Stay,
        Walk,
        Ready,
        Charge,
        Tired,
        Stun,
        Die,
        Sleep
    };

    public MMFeedbacks StunFeedbacks;
    public float WaitTime = 3;
    public float StunTime = 3;
    public float TiredTime = 3;
    public Transform Target;
    public float RayDist = 5.0f;
    public float MoveSpeed;
    public BossZone bossZone;
    public float MinChargeDist = 15.0f;
    public float MaxChargeDist = 20.0f;
    public List<Transform> FirePos = new List<Transform>();
    public MMFeedbacks WalkFeedback;
    public MMFeedbacks ReadyFeedback;
    public MMFeedbacks Skill01Feedback;
    public MMFeedbacks Skill02Feedback;
    public GameObject RunEffect;

    private NamedState state = NamedState.Sleep;
    private float curTime = 0;
    private bool isFlipped;
    private Vector2 RayDir = Vector2.left;
    private int RotateCnt = 0;
    private Animator anim;
    private Vector3 curTarget;
    private Rigidbody2D rb;
    private bool RepeatCharge = false;

    void Start()
    {
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        DataLoad();
    }

    void Update()
    {
        switch (state)
        {
            case NamedState.Stay:
                {
                    if (curTime > 0)
                    {
                        curTime -= Time.deltaTime;
                    }
                    else
                    {
                        Think();
                    }
                }
                break;
            case NamedState.Ready:
                {
                    if (curTime > 0)
                    {
                        curTime -= Time.deltaTime;
                    }
                    else
                    {
                        if (!RepeatCharge)
                        {
                            anim.SetBool("Ready", false);
                            anim.SetBool("Walk", true);
                            StartCoroutine(UpdateMove(transform.localPosition, Target.position));
                            RunEffect.SetActive(true);
                            SetState(NamedState.Walk);
                        }
                        else
                        {                           
                            anim.SetBool("Ready", false);
                            RunEffect.SetActive(true);
                            SetState(NamedState.Charge);
                        }
                    }                    
                }
                break;
            case NamedState.Walk:
                {
                    if (WallCheck())
                    {
                        StopAllCoroutines();
                        curTime = StunTime;
                        anim.SetBool("Stun",true);
                        StunFeedbacks.PlayFeedbacks();
                        StoneShower();
                        RunEffect.SetActive(false);
                        SetState(NamedState.Stun);
                    }
                }
                break;
            case NamedState.Charge:
                {
                    if (WallCheck())
                    {
                        RotatePos();
                        RotateCnt++;                       
                    }
                    else if (RotateCnt >= 5)
                    {
                        anim.SetBool("Tired", true);
                        curTime = TiredTime;
                        RotateCnt = 0;
                        RepeatCharge = false;
                        Skill02Feedback.PlayFeedbacks();
                        RunEffect.SetActive(false);
                        SetState(NamedState.Tired);
                    }
                    else
                    {
                        transform.Translate(Vector2.left * MoveSpeed * Time.deltaTime);
                    }
                }
                break;
            case NamedState.Tired:
                {
                    if (curTime > 0)
                    {
                        curTime -= Time.deltaTime;
                    }
                    else
                    {
                        curTime = WaitTime;
                        anim.SetBool("Tired", false);
                        SetState(NamedState.Stay);
                    }
                }
                break;
            case NamedState.Stun:
                {
                    if (curTime > 0)
                    {
                        curTime -= Time.deltaTime;
                    }
                    else
                    {
                        anim.SetBool("Stun", false);
                        LookAtPlayer();
                        ChargeThink();
                    }
                }
                break;
            case NamedState.Die:
                {
                    anim.SetTrigger("Dead");
                    return;
                }
            case NamedState.Sleep:
                {
                    if (PlayerCheack())
                    {
                        bossZone.SpawnDoor();
                        SetState(NamedState.Stay);
                    }
                    else
                        return;
                }
                break;
        }
    }

    public void DeadFunc()
    {
        StopAllCoroutines();
        bossZone.Clear = true;
        bossZone.DisappearDoor();
        anim.SetTrigger("Dead");
        SetState(NamedState.Die);      
    }

    private float ChargeDistVal()
    {
        float val = Random.Range(MinChargeDist, MaxChargeDist);
        return val;
    }

    IEnumerator UpdateMove(Vector2 startPos, Vector2 targetPos)
    {
        Vector2 pos = startPos + (RayDir * ChargeDistVal());
        // 이동 시작 위치 설정
        Vector2 position = startPos;
        //transform.localPosition = position;

        float Dist = 0;
        Dist = Vector3.Distance(startPos, pos);

        while (Dist > 0.1f )
        {
            
            transform.localPosition = Vector3.MoveTowards(position, pos, MoveSpeed*Time.deltaTime);
            position = transform.localPosition;
            Dist = Vector3.Distance(position, pos);
            // Debug.Log(Dist);

            yield return null;
        }

        anim.SetTrigger("Stop");
        curTime = WaitTime;
        RunEffect.SetActive(false);
        SetState(NamedState.Stay);
    }

    private void Think()
    {
        if(PlayerCheack())
        {
            anim.SetBool("Ready", true);
            curTime = 1.0f;
            SetState(NamedState.Ready);
            
        }
        else
        {
            LookAtPlayer();
        }
    }

    private void StoneShower()
    {
        StartCoroutine(StoneSpawn(4));
    }

    IEnumerator StoneSpawn(int repeat)
    {
        int cnt = 0;

        while (cnt < repeat)
        {
            int RandomFirePos = Random.Range(0, FirePos.Count);
            GameObject bullets = PoolingManager.instance.GetForestNamedBullet();
            
            if (bullets != null)
            {
                bullets.transform.position = FirePos[RandomFirePos].position;
                bullets.transform.rotation = FirePos[RandomFirePos].rotation;
                bullets.SetActive(true);
            }
            //Instantiate(Stone, FirePos[RandomFirePos].position, FirePos[RandomFirePos].rotation);
            cnt++;
            yield return new WaitForSeconds(0.5f);
        }
    }

    private void ChargeThink()
    {
        int val = Random.Range(0, 10);
        if (val < 4)
        {
            RepeatCharge = true;
            anim.SetBool("Ready", true);
            LookAtPlayer();
            curTime = 1.0f;
            SetState(NamedState.Ready);
        }
        else
        {
            anim.SetTrigger("Idle");
            SetState(NamedState.Stay);
        }
    }

    private bool PlayerCheack()
    {
        RaycastHit2D hitInfo = Physics2D.Raycast(transform.position + new Vector3(0,-1f,0), RayDir, RayDist, LayerMask.GetMask("Player"));
        Debug.DrawRay(transform.position + new Vector3(0, -1f, 0), RayDir * RayDist, Color.red);
        if (hitInfo)
        {
            Target = hitInfo.transform;
            return true;
        }
        return false;
    }

    private void SetState(NamedState InputState)
    {
        if (state == InputState)
            return;

        state = InputState;
    }

    private bool WallCheck()
    {
        RaycastHit2D hitInfo = Physics2D.Raycast(transform.position, RayDir, 4f, LayerMask.GetMask("Platforms"));
        Debug.DrawRay(transform.position, RayDir * 4f, Color.red);
        if (hitInfo)
        {
            return true;
        }
        return false;
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
            RayDir = Vector3.left;
        }
        else if (transform.position.x < Target.position.x && !isFlipped)
        {
            transform.localScale = flipped;
            transform.Rotate(0f, 180f, 0f);
            isFlipped = true;
            RayDir = Vector3.right;
        }
    }

    private void DataLoad()
    {
        MoveSpeed = (float)GetComponent<MonsterDataLoad>().Speed;
        //MovementSpeed = WalkSpeed;
    }

    private void RotatePos()
    {
        Vector3 flipped = transform.localScale;
        flipped.z *= -1f;
        transform.Rotate(0f, 180f, 0f);
        RayDir = RayDir * -1;
        if (isFlipped)
            isFlipped = false;
        else
            isFlipped = true;
        
    }

    public void WalkFeedbackFunc()
    {
        WalkFeedback.PlayFeedbacks();
    }

    public void ReadyFeedbackFunc()
    {
        ReadyFeedback.PlayFeedbacks();
    }

    public void Skill01FeedbackFunc()
    {
        Skill01Feedback.PlayFeedbacks();
    }

    public void Skill02FeedbackFunc()
    {
        Skill02Feedback.PlayFeedbacks();
    }

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MoreMountains.CorgiEngine;
using MoreMountains.Feedbacks;

public class Golem : MonoBehaviour
{
    private enum GolemState
    {
        Idle,
        Skill01,
        Skill02,
        Skill03,
        Die,
        Appear,
        Sleep
    }

    public BossZone bossZone;
    public string AppearParam = "Appear";
    public string Skill01Param = "Skill01";
    public string Skill02Param = "Skill02";
    public string Skill03Param = "Skill03";
    public string IdleParm = "Idle";
    public float Skill01Time = 10;
    public float Skill03Time = 6;
    public Transform Target;   
    public BoxCollider2D PlayerCheckBoxSkill03;
    public BoxCollider2D TargetInfuCheckZone;
    public Transform FirePos;
    public GameObject GolemBall;
    public MMFeedbacks ShakeFeedback;
    public List<Transform> StoneShowerPos = new List<Transform>();
    public GameObject Stone;
    public GameObject Skill03DamageZone;
    public float MoveSpeed = 5.0f;
    public float RayDist = 2;
    public int MaxPool = 5;
    public MMFeedbacks WalkFeedback;
    public MMFeedbacks Skill1chargeFeedback;
    public MMFeedbacks Skill1Feedback;
    public MMFeedbacks Skill2Feedback;
    public MMFeedbacks Skill3Feedback;
    public MMFeedbacks AppearFeedback;

    private GolemState state = GolemState.Sleep;
    private Health health;
    //private int curHealth = 0;
    private Animator _anim;
    private float curTime = 0;
    private float Skill01curTime = 0;
    private float Skill02curTime = 0;
    private float Skill03curTime = 0;
    private List<GameObject> BallPool = new List<GameObject>();
    private List<GameObject> StonePool = new List<GameObject>();

    private GameObject objectPools;
    private bool isFlipped = false;
    private Vector2 WallRayDir = Vector2.left;
    private DamageOnTouch DamageZone;

    void Start()
    {
        Init();
    }

    // Update is called once per frame
    void Update()
    {
        if (!PlayerCheack(TargetInfuCheckZone))
            Target = null;
        

        switch (state)
        {
            case GolemState.Idle:
                {
                    if(curTime <=0)
                    {
                        Think();
                    }
                    LookAtPlayer();
                    curTime -= Time.deltaTime;
                    Skill01curTime -= Time.deltaTime;
                    Skill03curTime -= Time.deltaTime;

                }
                break;
            case GolemState.Skill01:
                {
                    
                    curTime -= Time.deltaTime;
                    Skill01curTime -= Time.deltaTime;
                    Skill03curTime -= Time.deltaTime;
                }
                break;
            case GolemState.Skill02:
                {
                    if (!WallCheck(WallRayDir))
                    {
                        transform.Translate(Vector2.left * MoveSpeed * Time.deltaTime);
                        if (Skill02curTime <= 0)
                        {
                            StoneShower();
                            Skill02curTime = 2.0f;
                        }
                    }
                    else
                    {
                        _anim.SetBool(Skill02Param, false);
                        SetState(GolemState.Idle);
                    }

                    curTime -= Time.deltaTime;
                    Skill01curTime -= Time.deltaTime;
                    Skill02curTime -= Time.deltaTime;
                    Skill03curTime -= Time.deltaTime;
                }
                break;
            case GolemState.Skill03:
                {
                    curTime -= Time.deltaTime;
                    Skill01curTime -= Time.deltaTime;
                    Skill03curTime -= Time.deltaTime;
                }
                break;
            case GolemState.Die:
                {

                }
                break;
            case GolemState.Appear:
                {

                }
                break;
            case GolemState.Sleep:
                {
                    if(AppearWait())
                    {
                        bossZone.SpawnDoor();
                        _anim.SetTrigger(AppearParam);
                        DamageZone.enabled = true;
                        AppearFeedback.PlayFeedbacks();
                        SetState(GolemState.Appear);
                    }
                }
                break;
        }
    }

    private void TestBtn()
    {
        FireGolemBall();
    }

    private void Think()
    {
        if(Skill01curTime <= 0)
        {
            Skill01Process();
        }
        else 
        {
            if (Skill03curTime <= 0)
            {
                if (PlayerCheack(PlayerCheckBoxSkill03))
                    Skill03Process();
                else
                    Skill02Process();
            }
            else
            {
                Skill02Process();
            }
        }
    }

    private void Skill01Process()
    {
        Skill01curTime = Skill01Time;
        curTime = 4.0f;
        _anim.SetTrigger(Skill01Param);
        Skill1chargeFeedback.PlayFeedbacks();
        SetState(GolemState.Skill01);
    }

    private void Skill02Process()
    {
        curTime = 10.0f;
        _anim.SetBool(Skill02Param,true);
        Skill02curTime = 0.5f;
        SetState(GolemState.Skill02);
    }

    private void Skill03Process()
    {
        Skill03curTime = Skill03Time;
        curTime = 3.0f;
        _anim.SetBool(Skill03Param,true);       
        SetState(GolemState.Skill03);
    }

    IEnumerator StoneProcess(int val)
    {
        int cnt = 0;
        while(cnt < val)
        {
            int RandomVal = Random.Range(0, StoneShowerPos.Count);
            GameObject tempobj = GetStonese();
            if(tempobj == null)
            {
                AddStone();
                tempobj = GetStonese();
            }
            tempobj.transform.position = StoneShowerPos[RandomVal].position;
            tempobj.SetActive(true);

            //Instantiate(Stone, StoneShowerPos[RandomVal].position, StoneShowerPos[RandomVal].rotation);
            cnt++;
            yield return new WaitForSeconds(0.5f);
        }
    }

    private void StoneShower()
    {
        Skill2Feedback.PlayFeedbacks();
        int RandomVal = Random.Range(0, StoneShowerPos.Count);

        GameObject tempobj = GetStonese();
        if (tempobj == null)
        {
            AddStone();
            tempobj = GetStonese();
        }
        tempobj.transform.position = StoneShowerPos[RandomVal].position;
        tempobj.SetActive(true);
        //Instantiate(Stone, StoneShowerPos[RandomVal].position, StoneShowerPos[RandomVal].rotation);
    }

    private void StoneShower(int val)
    {
        StartCoroutine(StoneProcess(val));
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

    private void LookAtPlayer()
    {
        Vector3 flipped = transform.localScale;
        flipped.z *= -1f;
        if (transform.position.x > Target.position.x && isFlipped)
        {
            transform.localScale = flipped;
            transform.Rotate(0f, 180f, 0f);
            isFlipped = false;
            WallRayDir = Vector2.left;
        }
        else if (transform.position.x < Target.position.x && !isFlipped)
        {
            transform.localScale = flipped;
            transform.Rotate(0f, 180f, 0f);
            isFlipped = true;
            WallRayDir = Vector2.right;
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

    private void SetState(GolemState golemState)
    {
        if (golemState == state)
            return;
        else
            state = golemState;
    }

    private bool AppearWait()
    {
        if (health.CurrentHealth != health.MaximumHealth)
            return true;
        else
            return false;
    }

    private void DataLoad()
    {
        MoveSpeed = (float)GetComponent<MonsterDataLoad>().Speed;
        //MovementSpeed = WalkSpeed;
    }

    public void FireGolemBall()
    {
        //GameObject tempobj = Instantiate(GolemBall, FirePos.position, FirePos.rotation);
        GameObject tempobj = GetGolemBall();
        if (tempobj == null)
        {
            AddGolemBall();
            tempobj = GetGolemBall();
        }
        Skill1Feedback.PlayFeedbacks();
        tempobj.transform.position = FirePos.position;
        Vector3 dir = (Target.position - FirePos.position).normalized;
        tempobj.GetComponent<Projectile>().Direction = dir;
        tempobj.SetActive(true);
        _anim.SetBool(Skill01Param, false);
        SetState(GolemState.Idle);
    }

    public void ResetState()
    {
        _anim.SetBool(Skill01Param, false);
        _anim.SetBool(Skill03Param, false);
        _anim.SetBool(Skill02Param, false);
        SetState(GolemState.Idle);
    }

    public void StartState()
    {
        _anim.SetTrigger(IdleParm);
        SetState(GolemState.Idle);
    }

    public void DeadFunc()
    {
        StopAllCoroutines();
        bossZone.Clear = true;
        bossZone.DisappearDoor();
        _anim.SetTrigger("Dead");
        SetState(GolemState.Die);
    }

    private void Init()
    {
        health = GetComponent<Health>();
        _anim = GetComponent<Animator>();
        DamageZone = GetComponent<DamageOnTouch>();       
        Skill01curTime = 0;
        Skill03curTime = 0;
        curTime = 0;
        objectPools = new GameObject("GolemObjectPools");

        for (int i = 0; i < MaxPool; i++)
        {
            GameObject objBall = Instantiate(GolemBall, objectPools.transform);
            GameObject objStone = Instantiate(Stone, objectPools.transform);
            objBall.name = "objBall" + i.ToString("00");
            objStone.name = "objStone" + i.ToString("00");
            objBall.SetActive(false);
            objStone.SetActive(false);
            BallPool.Add(objBall);
            StonePool.Add(objStone);
            objectPools.transform.parent = bossZone.transform;
        }
    }

    private GameObject GetGolemBall()
    {
        for (int i = 0; i < BallPool.Count; i++)
        {
            if (BallPool[i].activeSelf == false)
            {
                return BallPool[i];
            }
        }
        return null;
    }

    private GameObject GetStonese()
    {
        for (int i = 0; i < StonePool.Count; i++)
        {
            if (StonePool[i].activeSelf == false)
            {
                return StonePool[i];
            }
        }
        return null;
    }

    private void AddGolemBall()
    {
        int num = BallPool.Count;

        for (int i = num; i < num + MaxPool; i++)
        {
            GameObject obj = Instantiate(GolemBall, objectPools.transform);
            obj.name = "objBall" + i.ToString("00");
            obj.SetActive(false);
            BallPool.Add(obj);
        }
    }

    private void AddStone()
    {
        int num = StonePool.Count;

        for (int i = num; i < num + MaxPool; i++)
        {
            GameObject obj = Instantiate(Stone, objectPools.transform);
            obj.name = "objStone" + i.ToString("00");
            obj.SetActive(false);
            StonePool.Add(obj);
        }
    }

    public void PlayShake()
    {
        ShakeFeedback.PlayFeedbacks();
    }
    public void DamageZoneSwitch()
    {

        if (Skill03DamageZone.activeSelf)
            Skill03DamageZone.SetActive(false);
        else
        {
            Skill03DamageZone.SetActive(true);
            StoneShower(5);
        }
            
    }

    public void WalkFeedBackFunc()
    {
        WalkFeedback.PlayFeedbacks();
    }

    public void Skill3FeedbackFunc()
    {
        Skill3Feedback.PlayFeedbacks();
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.white;

        Gizmos.DrawWireCube(PlayerCheckBoxSkill03.bounds.center, PlayerCheckBoxSkill03.bounds.size);     
    }

}

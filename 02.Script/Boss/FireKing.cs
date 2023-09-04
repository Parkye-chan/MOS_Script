using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MoreMountains.CorgiEngine;
using MoreMountains.Feedbacks;

public class FireKing : MonoBehaviour
{
    private enum FireKingState
    {
        IDLE,
        SKILL01,
        SKILL02,
        SKILL03,
        SKILL04,
        SKILL05,
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
    public string IdleParm = "Idle";
    public string ReadyParam = "Ready";
    public string TriggerParam = "Trigger";
    public float Skill01Time = 4.0f;
    public float Skill02Time = 3.0f;
    public float Skill03Time = 9.0f;
    public float Skill04Time = 7.0f;
    public float Skill05Time = 8.0f;
    public GameObject TopBody;
    public GameObject BottomBody;
    public Transform Target;
    public Transform[] GeneralSpawnPoint;
    public Transform[] AltarSpawnPoint;
    public Transform[] FilarSpawnPoint;
    public GameObject Altar;
    public GameObject FireBall;
    public GameObject FireFilarWarning;
    public GameObject FireFilar;
    public GameObject SpawnEffect;
    public GameObject Shield;
    public GameObject BlackholeEffect;
    public GameObject BlackHole;
    public MMFeedbacks ShakeFeedback;
    public MMFeedbacks AppearShakeFeedback;
    public MMFeedbacks AppearSound;
    public MMFeedbacks Skill01Feedback;
    public MMFeedbacks Skill02Feedback;
    public MMFeedbacks Skill04Feedback;
    public MMFeedbacks Skill05Feedback1;
    public MMFeedbacks Skill05Feedback2;
    public MMFeedbacks Phase2Feedback;
    public MMFeedbacks DeathFeedback2;

    private Rigidbody2D rb;
    private FireKingState state = FireKingState.SLEEP;
    private Health health;
    private Animator TopAnim;
    private Animator BottomAnim;
    private BossFOV FOV;
    private float curTime = 0;
    private bool isFlipped = false;
    private bool Phase2 = false;
    private int UseSkillCnt = 0;
    public List<GameObject> Generals = new List<GameObject>();
    [SerializeField]
    private List<GameObject> SpawnGenerals = new List<GameObject>();
    public List<GameObject> Altars = new List<GameObject>();
    public int testval = 0;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        health = GetComponent<Health>();
        TopAnim = TopBody.GetComponent<Animator>();
        BottomAnim = BottomBody.GetComponent<Animator>();
        FOV = GetComponent<BossFOV>();
        curTime = 0;
    }

    // Update is called once per frame
    void Update()
    {
        ShieldCheck();

        switch (state)
        {
            case FireKingState.IDLE:
                {

                    if (curTime <= 0)
                        Think();
                    else
                        curTime -= Time.deltaTime;
                }
                break;
            case FireKingState.SKILL01:
                {
                    curTime -= Time.deltaTime;
                }
                break;
            case FireKingState.SKILL02:
                {
                    
                    

                    curTime -= Time.deltaTime;
                }
                break;
            case FireKingState.SKILL03:
                {
                    curTime -= Time.deltaTime;
                }
                break;
            case FireKingState.SKILL04:
                {
                    curTime -= Time.deltaTime;
                }
                break;
            case FireKingState.SKILL05:
                {
                    curTime -= Time.deltaTime;
                }
                break;
            case FireKingState.DEAD:
                break;
            case FireKingState.APPEAR:
                {
                    if(curTime <= 0)
                    {
                        TopAnim.SetBool(AppearParam, false);
                        SetState(FireKingState.IDLE);
                    }
                    curTime -= Time.deltaTime;

                }
                break;
            case FireKingState.SLEEP:
                {
                    if (!Target)
                        return;
                    else
                    {
                        TopAnim.SetBool(AppearParam, true);
                        
                        curTime = 5.0f;
                        SetState(FireKingState.APPEAR);
                    }
                }
                break;
        }
    }

    private void TestBtn()
    {
        if (testval == 0)
            Skill01Process();
        else if (testval == 1)
            Skill02Process();
        else if (testval == 2)
            Skill03Process();
        else if (testval == 3)
            Skill04Process();
        else if (testval == 4)
            Skill05Process();
    }

    private void Think()
    {
        if(HealthCheck(health.CurrentHealth))
        {
            Think2Phase();
        }
        else
        {
            if(SpawnGeneralCheck())
            {

                Skill01Process();
            }
            else
            {
                if(Altars.Count == 0)
                {
                    Skill02Process();
                }
                else
                {
                    if(UseSkillCnt%2 == 0)
                    {
                        Skill03Process();
                    }
                    else
                    {
                        Skill04Process();
                    }
                }
            }
        }
    }

    private void Think2Phase()
    {
        if (Phase2)
        {
            if(UseSkillCnt >= 5)
            {
                Skill05Process();
            }
            else
            {
                if (SpawnGeneralCheck())
                {
                    Skill01Process();
                }
                else
                {
                    if (Altars.Count == 0)
                    {
                        Skill02Process();
                    }
                    else
                    {
                        if (UseSkillCnt % 2 == 0)
                        {
                            Skill03Process();
                        }
                        else
                        {
                            Skill04Process();
                        }
                    }
                }
            }
        }
        else
        {
            curTime = 4.0f;
            TopAnim.SetBool(Phase2Param, true);
            BottomAnim.SetTrigger(TriggerParam);
            UseSkillCnt = 5;
            Phase2 = true;
        }
    }

    private void Skill01Process()
    {
        curTime = Skill01Time;
        TopAnim.SetBool(Skill01Param, true);
        StartCoroutine(Skill01());
        SetState(FireKingState.SKILL01);
    }

    private void Skill02Process()
    {
        curTime = Skill02Time;
        TopAnim.SetBool(Skill02Param, true);
        for (int i = 0; i < AltarSpawnPoint.Length; i++)
        {
            GameObject temp = Instantiate(Altar, AltarSpawnPoint[i].position, AltarSpawnPoint[i].rotation);
            Altars.Add(temp);
        }
        StartCoroutine(Skill02());
        SetState(FireKingState.SKILL02);
    }

    private void Skill03Process()
    {
        curTime = Skill03Time;        
        StartCoroutine(Skill03());
        SetState(FireKingState.SKILL03);
    }

    private void Skill04Process()
    {
        curTime = Skill04Time;
        TopAnim.SetBool(Skill04Param, true);
        StartCoroutine(Skill04());
        SetState(FireKingState.SKILL04);
    }

    private void Skill05Process()
    {
        curTime = Skill05Time;
        StartCoroutine(Skill05());
    }

    IEnumerator Skill01()
    {
        for (int i = 0; i < GeneralSpawnPoint.Length; i++)
        {
            Instantiate(SpawnEffect, GeneralSpawnPoint[i].position, GeneralSpawnPoint[i].rotation);
        }

        yield return new WaitForSeconds(2.0f);

        for (int i = 0; i < GeneralSpawnPoint.Length; i++)
        {
            GameObject temp = Instantiate(Generals[i], GeneralSpawnPoint[i].position, GeneralSpawnPoint[i].rotation);
            temp.GetComponent<Health>().InitialHealth = 100;
            temp.GetComponent<Health>().CurrentHealth = 100;
            SpawnGenerals.Add(temp);
        }

        TopAnim.SetBool(Skill01Param, false);
        UseSkillCnt++;
        SetState(FireKingState.IDLE);
    }

    private bool SpawnGeneralCheck()
    {
        for (int i = 0; i < SpawnGenerals.Count; i++)
        {
            if (SpawnGenerals[i].activeSelf)
                return false;
        }
        return true;
    }

    IEnumerator Skill02()
    {
        yield return new WaitForSeconds(0.7f);
        TopAnim.SetBool(Skill02Param, false);
        UseSkillCnt++;
        SetState(FireKingState.IDLE);
    }

    IEnumerator Skill03()
    {
        int SpawnCnt = 0;
        TopAnim.SetBool(Skill03Param, true);
        List<int> Spawnpoint = new List<int>();
        while (SpawnCnt < 5)
        {         
            TopAnim.SetTrigger(TriggerParam);
            for (int i = 0; i < 2; i++)
            {
                int RandomVal = Random.Range(0, FilarSpawnPoint.Length);
                if(Spawnpoint.Contains(RandomVal))
                {
                    RandomVal = Random.Range(0, FilarSpawnPoint.Length);
                }

                Instantiate(FireFilarWarning, FilarSpawnPoint[RandomVal].position, FilarSpawnPoint[RandomVal].rotation);
                Spawnpoint.Add(RandomVal);
            }
            
            yield return new WaitForSeconds(1.0f);

            for (int i = 0; i < Spawnpoint.Count; i++)
            {
                Instantiate(FireFilar, FilarSpawnPoint[Spawnpoint[i]].position, FilarSpawnPoint[Spawnpoint[i]].rotation);
            }
            yield return new WaitForSeconds(1.5f);
            Spawnpoint.Clear();
            SpawnCnt++;
            
        }
        TopAnim.SetBool(Skill03Param, false);
        UseSkillCnt++;
        SetState(FireKingState.IDLE);     
    }

    IEnumerator Skill04()
    {
        int SpawnCnt = 0;

        while (SpawnCnt < 10)
        {
            Vector3 TargetPos = Target.transform.position;
            yield return new WaitForSeconds(0.5f);
            GameObject temp = Instantiate(FireBall, TopBody.transform.position, TopBody.transform.rotation);
            temp.GetComponent<Projectile>().Direction = (Target.position - TopBody.transform.position).normalized;
            Destroy(temp, 3);
            SpawnCnt++;
        }

        TopAnim.SetBool(Skill04Param, false);
        UseSkillCnt++;
        SetState(FireKingState.IDLE);
    }

    IEnumerator Skill05()
    {
        TopAnim.SetBool(Skill05Param, true);
        BlackHole.SetActive(true);
        
        yield return new WaitForSeconds(6.0f);


        BlackHole.SetActive(false);
        Instantiate(BlackholeEffect, transform.position, transform.rotation);
        
        TopAnim.SetBool(Skill05Param, false);
        UseSkillCnt = 0;

    }

    private void ShieldCheck()
    {

        if (Altars.Count > 0)
        {
            if (Altars[0] != null || Altars[1] != null)
            {
                health.Invulnerable = true;
                if (!Shield.activeSelf)
                    Shield.SetActive(true);
            }
            else if (Altars[0] == null && Altars[1] == null)
            {
                health.Invulnerable = false;
                if (Shield.activeSelf)
                    Shield.GetComponent<Animator>().SetTrigger(TriggerParam);
            }

        }
        else
        {
            health.Invulnerable = false;
            if (Shield.activeSelf)
                Shield.GetComponent<Animator>().SetTrigger(TriggerParam);
        }
    }

    public void DeadFunc()
    {
        StopAllCoroutines();

        bossZone.Clear = true;
        bossZone.DisappearDoor();
        TopAnim.SetTrigger("Dead");
        BottomBody.SetActive(false);
        SetState(FireKingState.DEAD);
    }


    public void stateStart()
    {
        state = FireKingState.IDLE;
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

    public void ShakeFunc()
    {
        ShakeFeedback.PlayFeedbacks();
    }

    public void ApeearShake()
    {
        AppearShakeFeedback.PlayFeedbacks();
    }

    public void ApeearSound()
    {
        AppearSound.PlayFeedbacks();
    }

    public void Skill01FeedbackFunc()
    {
        Skill01Feedback.PlayFeedbacks();
    }

    public void Skill02FeedbackFunc()
    {
        Skill02Feedback.PlayFeedbacks();
    }

    public void Skill04FeedbackFunc()
    {
        Skill04Feedback.PlayFeedbacks();
    }

    public void Skill05FeedbackFunc()
    {
        Skill05Feedback1.PlayFeedbacks();
    }

    public void Skill05FeedbackFunc2()
    {
        Skill05Feedback2.PlayFeedbacks();
    }

    public void Phase2FeedbackFunc()
    {
        Phase2Feedback.PlayFeedbacks();
    }

    public void DeathFeedbackFunc()
    {
        DeathFeedback2.PlayFeedbacks();
    }

    public void BodyAppear()
    {
        BottomBody.SetActive(true);
    }

    private bool HealthCheck(int curHp)
    {
        if (curHp <= (health.MaximumHealth * 0.5f))
            return true;
        else
            return false;
    }

    private void SetState(FireKingState TargetState)
    {
        if (TargetState == state)
            return;
        else
            state = TargetState;
    }


    /*
private void OnGUI()
{
    if (GUI.Button(new Rect(200, 0, 200, 80), "ATTACK"))
    {
        TestBtn();
    }
}
*/
}

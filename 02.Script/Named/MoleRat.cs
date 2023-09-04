using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MoreMountains.Feedbacks;


public class MoleRat : MonoBehaviour
{
    private enum MoleRatState
    {
        Appear,
        Idle,
        Skill01,
        Dive,
        Skill03,
        BackStep,
        Die
    }


    private MoleRatState State = MoleRatState.Appear;
    private float curTime;
    private Animator _anim;
    private bool isFlipped = true;
    private Vector2 JumpVector = new Vector2(2, 1);
    private Vector2 WallRayDir = Vector2.right;
    private float Skill01curTime;
    private float Skill02curTime;
    private float Skill03curTime;
    private BoxCollider2D boxCollider;
    private Vector3 Skilltarget;
    private Rigidbody2D rb;


    public Transform ModelPos;
    public Transform FirePos;
    public float BulletPower = 4.0f;
    public float Power = 10.0f;
    public float Skill01CoolTime = 3.0f;
    public float Skill02CoolTime = 6.0f;
    public float Skill03CoolTime = 3.0f;
    public Transform Target;
    public float WaitTime;
    public GameObject PoisonCloud;
    public BoxCollider2D PlayerCheckBoxSkill03;
    public BoxCollider2D PlayerCheckBoxSkill01;
    public GameObject Rock;
    public GameObject ExploionRock;
    public GameObject RollRock;
    public BossZone bosszone;
    public float RayDist = 1;
    public MMFeedbacks Skill1Feedback;
    public MMFeedbacks Skill2Feedback1;
    public MMFeedbacks Skill2Feedback2;
    public MMFeedbacks Skill2Feedback3;
    public MMFeedbacks Skill3Feedback;
    public MMFeedbacks AppearFeedback;

    void Start()
    {
        _anim = GetComponent<Animator>();
        boxCollider = GetComponent<BoxCollider2D>();
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!Target)
            return;

        switch (State)
        {
            case MoleRatState.Appear:
                {
                    //AppearFeedback.PlayFeedbacks();
                    //return;
                }
                break;
            case MoleRatState.Idle:
                {
                    if (GroundCheck())
                        rb.velocity = Vector3.zero;

                    LookAtPlayer();
                    Skill01curTime -= Time.deltaTime;
                    Skill02curTime -= Time.deltaTime;
                    Skill03curTime -= Time.deltaTime;
                    curTime -= Time.deltaTime;

                    if (curTime <= 0)
                        Think();                              
                }
                break;
            case MoleRatState.Skill01:
                {
                    return;
                }
            case MoleRatState.Dive:
                {
                    curTime -= Time.deltaTime;
                    Skill01curTime -= Time.deltaTime;
                    Skill03curTime -= Time.deltaTime;
                }
                break;
            case MoleRatState.Skill03:
                {
                    
                    if (WallCheck(WallRayDir))
                    {
                        _anim.SetTrigger("Idle");
                        LookAtPlayer();
                        SetState(MoleRatState.Idle);
                    }
                    else if (GroundCheck())
                    {
                        rb.velocity = Vector3.zero;
                        //StartCoroutine(dumbleing());
                        SetState(MoleRatState.BackStep);
                        StartCoroutine(BackStepCoroutine());
                                    
                    }
                    curTime -= Time.deltaTime;
                    Skill01curTime -= Time.deltaTime;
                    Skill02curTime -= Time.deltaTime;
                }
                break;
            case MoleRatState.BackStep:
                {
                    //LookAtPlayer();
                    Skill01curTime -= Time.deltaTime;
                    Skill02curTime -= Time.deltaTime;
                    Skill03curTime -= Time.deltaTime;
                    curTime -= Time.deltaTime;
                                     
                }
                break;
            case MoleRatState.Die:
                break;
        }
    }

    public void StateStart()
    {
        _anim.SetTrigger("Idle");
        LookAtPlayer();
        Skill01curTime = 0;
        Skill02curTime = 0;
        Skill03curTime = 0;
        curTime = 0;
        State = MoleRatState.Idle;
    }

    public void BackStepFunc()
    {
        Vector2 tempJumpVector = new Vector2(-JumpVector.x, JumpVector.y);
        rb.AddForce(tempJumpVector * Power, ForceMode2D.Impulse);
    }

    IEnumerator dumbleing()
    {
        _anim.SetTrigger("Dumblering");
        yield return new WaitForSeconds(1.0f);
        
        LookAtPlayer();
        yield return StartCoroutine(BackStepCoroutine());
    }

    IEnumerator BackStepCoroutine()
    {
        if (WallCheck(-WallRayDir))
        {
            _anim.SetTrigger("Idle");
            LookAtPlayer();
            SetState(MoleRatState.Idle);
        }
        else
        {
            _anim.SetTrigger("BackStep");
            SetState(MoleRatState.Idle);
        }
        yield return null;
    }

    IEnumerator DiveAttack()
    {
        yield return new WaitForSeconds(2.0f);

        _anim.SetTrigger("Skill02Appear");
        transform.position = new Vector3(Target.position.x, transform.position.y, transform.position.z);
    }

    private void Think()
    {
        if(Skill02curTime <= 0)
        {
            Skill2Feedback1.PlayFeedbacks();
            _anim.SetTrigger("Skill02");
            Skill02curTime = Skill02CoolTime;
            curTime = 5.0f;
            StartCoroutine(DiveAttack());
            SetState(MoleRatState.Dive);
        }
        else
        {
            if (Skill03curTime <= 0)
            {
                if (PlayerCheack(PlayerCheckBoxSkill03))
                {
                    skill03();
                }
                else
                {
                    if (Skill01curTime <= 0)
                    {
                        if (PlayerCheack(PlayerCheckBoxSkill01))
                        {
                            Skill01();
                        }
                        else
                        {
                            _anim.SetTrigger("Idle");
                            LookAtPlayer();
                            SetState(MoleRatState.Idle);
                        }
                    }
                }
            }
            else
            {
                if (Skill01curTime <= 0)
                {
                    if (PlayerCheack(PlayerCheckBoxSkill01))
                    {
                        Skill01();
                    }
                    else
                    {
                        _anim.SetTrigger("Idle");
                        LookAtPlayer();
                        SetState(MoleRatState.Idle);
                    }
                }
            }
        }
    }

    private IEnumerator Skill03Process()
    {
        _anim.SetTrigger("Skill03");
        curTime = 4.0f;
        Skill03curTime = Skill03CoolTime;
        rb.AddForce(JumpVector * Power, ForceMode2D.Impulse);
        yield return new WaitForSeconds(0.1f);
        SetState(MoleRatState.Skill03);
    }

    private void Skill01()
    {
        Skill1Feedback.PlayFeedbacks();
        _anim.SetTrigger("Skill01");       
        Skill01curTime = Skill01CoolTime;
        curTime = 3.0f;
        SetState(MoleRatState.Skill01);
    }

    public void FireStone()
    {
        for (int i = 0; i < 2; i++)
        {
            switch (i)
            {
                case 0:
                    {
                        GameObject tempobj = Instantiate(RollRock, FirePos.position, FirePos.rotation);
                        Rigidbody2D bulletRb = tempobj.GetComponent<Rigidbody2D>();
                        Vector2 dir = new Vector2(6, 2);                       
                        if(isFlipped)
                            bulletRb.AddForce(dir* BulletPower, ForceMode2D.Impulse);
                        else
                        {
                            dir = new Vector2(-dir.x, dir.y);
                            bulletRb.AddForce(dir * BulletPower, ForceMode2D.Impulse);
                        }                          
                    }
                    break;
                    /*
                case 2:
                    {
                        GameObject tempobj = Instantiate(ExploionRock, FirePos.position, FirePos.rotation);
                        Rigidbody2D bulletRb = tempobj.GetComponent<Rigidbody2D>();
                        Vector2 dir = new Vector2(4, 5);
                        if (isFlipped)
                            bulletRb.AddForce(dir* BulletPower, ForceMode2D.Impulse);
                        else
                        {
                            dir = new Vector2(-dir.x, dir.y);
                            bulletRb.AddForce(dir * BulletPower, ForceMode2D.Impulse);
                        }
                    }
                    break;*/
                case 1:
                    {
                        GameObject tempobj = Instantiate(Rock, FirePos.position, FirePos.rotation);
                        Rigidbody2D bulletRb = tempobj.GetComponent<Rigidbody2D>();
                        Vector2 dir = new Vector2(2, 7);
                        if (isFlipped)
                            bulletRb.AddForce(dir* BulletPower, ForceMode2D.Impulse);
                        else
                        {
                            dir = new Vector2(-dir.x, dir.y);
                            bulletRb.AddForce(dir * BulletPower, ForceMode2D.Impulse);
                        }
                    }
                    break;
            }
        }
        StartCoroutine(BackStepCoroutine());
    }

    public void skill03()
    {
        StartCoroutine(Skill03Process());
    }

    public void OnOffCollider()
    {
        if(boxCollider.enabled)
            boxCollider.enabled = false;
        else
            boxCollider.enabled = true;
    }

    public void SpawnPosionCloud()
    {
        boxCollider.enabled = true;
        if (PoisonCloud)
        {
            GameObject tempobj = Instantiate(PoisonCloud, transform.position, transform.rotation);
            Destroy(tempobj, 2);
        }
        Skill2Feedback3.PlayFeedbacks();
        Skill02curTime = Skill02CoolTime;
        _anim.SetTrigger("Idle");
        LookAtPlayer();
        SetState(MoleRatState.Idle);
    }

    private void SetState(MoleRatState InputState)
    {
        if (State == InputState)
            return;

        State = InputState;
    }

    private bool PlayerCheack(BoxCollider2D PlayerCheckBox)
    {
        RaycastHit2D hitInfo = Physics2D.BoxCast(PlayerCheckBox.bounds.center, PlayerCheckBox.bounds.size, 0f,Vector2.zero,0,LayerMask.GetMask("Player"));        
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
            JumpVector = new Vector2(-2, 1);
            WallRayDir = Vector2.left;
        }
        else if (transform.position.x < Target.position.x && !isFlipped)
        {
            transform.localScale = flipped;
            transform.Rotate(0f, 180f, 0f);
            isFlipped = true;
            JumpVector = new Vector2(2, 1);
            WallRayDir = Vector2.right;
        }
    }

    private bool WallCheck(Vector2 RayDir)
    {
        RaycastHit2D hitInfo = Physics2D.Raycast(ModelPos.position, RayDir, 1.5f, LayerMask.GetMask("Platforms"));
        Debug.DrawRay(ModelPos.position, RayDir * 1.5f, Color.red);
        if (hitInfo)
        {
            return true;
        }
        return false;
    }

    public void DeadFunc()
    {
        StopAllCoroutines();
        _anim.SetTrigger("Dead");
        bosszone.Clear = true;
        bosszone.DisappearDoor();
        SetState(MoleRatState.Die);
    }

    private bool GroundCheck()
    {
        RaycastHit2D hitInfo = Physics2D.Raycast(ModelPos.position, Vector2.down, RayDist, LayerMask.GetMask("Platforms"));
        Debug.DrawRay(ModelPos.position, RayDist * Vector2.down, Color.red);
        if (hitInfo)
        {
            return true;
        }
        return false;
    }


    public void Skill2FeedbackFunc()
    {
        Skill2Feedback2.PlayFeedbacks();
    }

    public void AppearFeedbackFunc()
    {
        AppearFeedback.PlayFeedbacks();
    }

    public void Skill03FeedbackFunc()
    {
        Skill3Feedback.PlayFeedbacks();
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.white;

        Gizmos.DrawWireCube(PlayerCheckBoxSkill03.bounds.center, PlayerCheckBoxSkill03.bounds.size);

        Gizmos.DrawWireCube(PlayerCheckBoxSkill01.bounds.center, PlayerCheckBoxSkill01.bounds.size);
    }
    /*
    private void OnGUI()
    {
        if (GUI.Button(new Rect(200, 0, 200, 80), "ATTACK"))
        {
            FireStone();
        }

        if (GUI.Button(new Rect(200, 400, 200, 80), "Flip"))
        {
            LookAtPlayer();
        }
    }*/


}

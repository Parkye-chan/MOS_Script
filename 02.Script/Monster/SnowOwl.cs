using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MoreMountains.CorgiEngine;
using MoreMountains.Feedbacks;

public class SnowOwl : MonoBehaviour
{

    private enum OwlState
    {
        WALK,
        ATTACK,
        DEAD

    }
    public Transform[] MovePos;
    public float WalkSpeed = 10;
    public GameObject Bullet;
    public string AttackParam = "Awake";
    public string DeadParam = "Dead";
    public MMFeedbacks AttackFeedbacks;
    private Animator _anim;
    private Rigidbody2D rb;
    private MonsterFOV FOV;
    private OwlState state = OwlState.WALK;
    private Transform Target;
    private float MovementSpeed;
    private bool isFlipped = false;
    private int slowCnt = 0;
    private Vector2 Dir = Vector2.zero;
    private Vector3 MoveDir;
    private Vector3 StartPos;
    private SpriteRenderer sprite;
    private bool Rotate = true;
    private Vector3[] initMovePos;
    private float curTime = 0;
    private int FireCnt = 0;

    void Start()
    {
        _anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        FOV = GetComponent<MonsterFOV>();
        sprite = GetComponent<SpriteRenderer>();
        StartPos = transform.position;
        DataLoad();
        sprite.flipX = isFlipped;
        initMovePos = new Vector3[MovePos.Length];
        for (int i = 0; i < MovePos.Length; i++)
        {
            initMovePos[i] = MovePos[i].position ;
        }
    }

    private void OnDisable()
    {
        transform.position = StartPos;
        Target = null;
        Rotate = true;
        Dir = Vector2.zero;
        isFlipped = false;
        sprite.flipX = isFlipped;
        MovementSpeed = WalkSpeed;
        slowCnt = 0;
        for (int i = 0; i < MovePos.Length; i++)
        {
            initMovePos[i] = MovePos[i].position;
        }
        state = OwlState.WALK;
    }

    // Update is called once per frame
    void Update()
    {
        switch (state)
        {
            case OwlState.WALK:
                {
                    if(FOV.TargetDetect())
                    {
                        Target = FOV.target;
                        
                        SetState(OwlState.ATTACK);
                    }
                    else
                    {
                        if (Rotate)
                        {
                            float Dist = (this.transform.position - initMovePos[0]).sqrMagnitude;

                            MoveDir = (initMovePos[0] - transform.position).normalized;
                            transform.position += MoveDir * MovementSpeed * Time.deltaTime;

                            if (Dist < 0.1)
                            {
                                Rotate = false;
                                Turn();
                            }
                        }
                        else
                        {

                            float Dist = (this.transform.position - initMovePos[1]).sqrMagnitude;

                            MoveDir = (initMovePos[1] - transform.position).normalized;
                            transform.position += MoveDir * MovementSpeed * Time.deltaTime;

                            if (Dist < 0.1)
                            {
                                Rotate = true;
                                Turn();
                            }
                        }
                    }


                    curTime -= Time.deltaTime;
                    
                }
                break;
            case OwlState.ATTACK:
                {
                    if (curTime <= 0)
                    {
                        LookAtPlayer();
                        AttackProcess();
                    }
                    
                    
                        curTime -= Time.deltaTime;
                    
                    //else
                   // {
                        //LooatDir();
                        //SetState(OwlState.WALK);
                  //  }

                }
                break;
            case OwlState.DEAD:
                {

                }
                break;
        }
    }

    private void LooatDir()
    {
        sprite.flipX = MoveDir.x > 0 ? true : false;
        /*
        if (MoveDir.x > 0)
        {
            sprite.flipX = true;
        }
        else
        {
            sprite.flipX = false;
        }*/
    }

    private void LookAtPlayer()
    {
        Vector3 flipped = transform.localScale;
        flipped.z *= -1f;
        if (transform.position.x > Target.position.x && isFlipped)
        {
            transform.localScale = flipped;
            //transform.Rotate(0f, 180f, 0f);
            //isFlipped = false;
            sprite.flipX = true;
            Dir = -Dir;
            FOV.RayDir = -FOV.RayDir;
        }
        else if (transform.position.x < Target.position.x && !isFlipped)
        {
            transform.localScale = flipped;
            //transform.Rotate(0f, 180f, 0f);
            //isFlipped = true;
            sprite.flipX = false;
            Dir = -Dir;
            FOV.RayDir = -FOV.RayDir;
        }
    }

    private void AttackProcess()
    {
        if (FireCnt >= 2)
        {
            FireCnt = 0;
            SetState(OwlState.WALK);
        }

        curTime = 1.0f;
        _anim.SetTrigger(AttackParam);
       GameObject temp = Instantiate(Bullet, transform.position, transform.rotation);      
        Vector3 Firedir = (Target.position - transform.position).normalized;
        temp.GetComponent<Projectile>().Direction = Firedir;
        FireCnt++;
    }


    public void SlowFunc()
    {
        slowCnt++;
        if (slowCnt > 1)
        {
            StartCoroutine(SlowProcess());
            slowCnt = 0;
        }
    }

    private void Turn()
    {
        if (isFlipped)
        {
            isFlipped = false;
            sprite.flipX = isFlipped;
            Dir = -Dir;
            FOV.RayDir = -FOV.RayDir;
        }
        else
        {
            isFlipped = true;
            sprite.flipX = isFlipped;
            Dir = -Dir;
            FOV.RayDir = -FOV.RayDir;
        }
        //transform.Rotate(0, 180, 0);
    }

    public void DeadFunc()
    {
        StopAllCoroutines();
        _anim.SetTrigger(DeadParam);
        SetState(OwlState.DEAD);
    }

    IEnumerator SlowProcess()
    {
        MovementSpeed = WalkSpeed * 0.5f;
        yield return new WaitForSecondsRealtime(5.0f);
        MovementSpeed = WalkSpeed;
    }

    private void DataLoad()
    {
        WalkSpeed = (float)GetComponent<MonsterDataLoad>().Speed;
        MovementSpeed = WalkSpeed;
    }

    public void AttackFeedback()
    {
        AttackFeedbacks.PlayFeedbacks();
    }

    private void SetState(OwlState Targetstate)
    {
        if (state == Targetstate)
            return;

        state = Targetstate;
    }


}

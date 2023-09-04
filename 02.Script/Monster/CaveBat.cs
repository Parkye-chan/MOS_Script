using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MoreMountains.CorgiEngine;

public class CaveBat : MonoBehaviour
{

    private enum BatState
    {
        SLEEP,
        TRACKE,
        DEAD

    }

    public float WalkSpeed = 10;
    public string AttackParam = "Awake";
    public string DeadParam = "Dead";
    private Animator _anim;
    private Rigidbody2D rb;
    private MonsterFOV FOV;
    private BatState state;
    private Transform Target;
    private float MovementSpeed;
    private bool isFlipped = false;
    private int slowCnt = 0;
    private Vector2 Dir = Vector2.zero;
    private SpriteRenderer sprite;

    private void Start()
    {
        _anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        FOV = GetComponent<MonsterFOV>();
        sprite = GetComponent<SpriteRenderer>();
        sprite.flipX = isFlipped;
        DataLoad();
    }

    private void OnDisable()
    {
        Target = null;
        Dir = Vector2.zero;
        isFlipped = false;
        MovementSpeed = WalkSpeed;
        slowCnt = 0;
        SetState(BatState.SLEEP);
    }

    private void Update()
    {
        switch (state)
        {
            case BatState.SLEEP:
                {
                    if (FOV.TargetDetect())
                    {
                        Target = FOV.target;
                        _anim.SetTrigger(AttackParam);
                        SetState(BatState.TRACKE);
                    }
                    else
                        return;
                }
                break;
            case BatState.TRACKE:
                {
                    Dir = (Target.position - transform.position).normalized;
                    LookAtPlayer();
                    
                    float Dist = Vector2.Distance(transform.position, Target.position);

                    if (Dist <= 0.1f)
                        return;
                    else
                        transform.Translate(Dir * MovementSpeed * Time.deltaTime);
                }
                break;
            case BatState.DEAD:
                {
                    return;
                }
        }
    }


    private void LookAtPlayer()
    {
        Vector3 flipped = transform.localScale;
        flipped.z *= -1f;
        if (transform.position.x > Target.position.x && isFlipped)
        {
            transform.localScale = flipped;
            //transform.Rotate(0f, 180f, 0f);
            isFlipped = false;
            sprite.flipX = isFlipped;
            Dir = -Dir;
            FOV.RayDir = -FOV.RayDir;
        }
        else if (transform.position.x < Target.position.x && !isFlipped)
        {
            transform.localScale = flipped;
            //transform.Rotate(0f, 180f, 0f);
            isFlipped = true;
            sprite.flipX = isFlipped;
            Dir = -Dir;
            FOV.RayDir = -FOV.RayDir;
        }
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

    public void DeadFunc()
    {
        StopAllCoroutines();
        _anim.SetTrigger(DeadParam);
        SetState(BatState.DEAD);
    }

    IEnumerator SlowProcess()
    {
        MovementSpeed = WalkSpeed * 0.5f;
        yield return new WaitForSecondsRealtime(5.0f);
        MovementSpeed = WalkSpeed;
    }

    private void SetState(BatState Targetstate)
    {
        if (state == Targetstate)
            return;

        state = Targetstate;
    }

    private void DataLoad()
    {
        WalkSpeed = (float)GetComponent<MonsterDataLoad>().Speed;
        MovementSpeed = WalkSpeed;
    }

}

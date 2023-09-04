using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireBat : MonoBehaviour
{
    private enum BatState
    {
        SLEEP,
        TRACKE,
        DEAD

    }


    public float WalkSpeed = 10;
    public string WalkParam = "Walk";
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
    private float curTime = 0f;

    // Start is called before the first frame update
    void Start()
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


    // Update is called once per frame
    void Update()
    {
        switch (state)
        {
            case BatState.SLEEP:
                {
                    if (FOV.TargetDetect())
                    {
                        Target = FOV.target;
                        Dir = (Target.position - transform.position).normalized;
                        _anim.SetBool(WalkParam,true);
                        curTime = 0.3f;
                        SetState(BatState.TRACKE);
                    }
                    else
                        return;
                }
                break;
            case BatState.TRACKE:
                {
                    
                    if (curTime <= 0)
                    {
                        LookAtPlayer();
                        transform.Translate(Dir * MovementSpeed * Time.deltaTime);
                    }

                    curTime -= Time.deltaTime;
                }
                break;
            case BatState.DEAD:
                {

                }
                break;
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

        }
        else if (transform.position.x < Target.position.x && !isFlipped)
        {
            transform.localScale = flipped;
            //transform.Rotate(0f, 180f, 0f);
            isFlipped = true;
            sprite.flipX = isFlipped;

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

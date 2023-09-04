using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OldMovingPlatform : MonoBehaviour
{

    [SerializeField]
    float speed = 1.0f;
    [SerializeField]
    Transform start;
    [SerializeField]
    Transform end;

    public bool isActivate = false;
    public Transform ParentsObj;

    private EarthGimmick[] gimmicks;
    private GameObject target;
    Rigidbody2D Rb;

    // Start is called before the first frame update
    void Start()
    {
        //target = start.gameObject;
        target = null;
        Rb = GetComponent<Rigidbody2D>();
        gimmicks = ParentsObj.GetComponentsInChildren<EarthGimmick>();
        //StartCoroutine(moveStart());
    }

    private void FixedUpdate()
    {
        if (isActivate)
        {
            if (target)
            {

                float Dist = (this.transform.position - start.transform.position).sqrMagnitude;

                Vector3 dir = (start.transform.position - transform.position).normalized;
                transform.position += dir * speed * Time.deltaTime;

                if (Dist < 0.1)
                {
                    target = null;
                    isActivate = false;
                    if(gimmicks.Length >0)
                    {
                        for (int i = 0; i < gimmicks.Length; i++)
                        {
                            gimmicks[i].AnimStop();
                        }
                    }
                }
            }
            else
            {
                float Dist = (this.transform.position - end.transform.position).sqrMagnitude;

                Vector3 dir = (end.transform.position - transform.position).normalized;
                transform.position += dir * speed * Time.deltaTime;

                if (Dist < 0.1)
                {                   
                    target = start.gameObject;
                    isActivate = false;
                    if (gimmicks.Length > 0)
                    {
                        for (int i = 0; i < gimmicks.Length; i++)
                        {
                            gimmicks[i].AnimStop();
                        }
                    }
                }
            }
        }
        else
            return;
    }

    public void ChangeDir()
    {
        if (target == null)
            target = start.gameObject;
        else
            target = null;
    }


    IEnumerator moveStart()
    {
        float fDist = (this.transform.position - start.position).sqrMagnitude;

        Vector2 curPos = this.transform.position;

        while (fDist > float.Epsilon)
        {
            Vector2 newPosition = Vector2.MoveTowards(Rb.position, start.position, speed * Time.deltaTime);

            Rb.MovePosition(newPosition);

            fDist = (this.transform.position - start.position).sqrMagnitude;

            yield return null;
        }

        yield return StartCoroutine(moveend());
    }

    IEnumerator moveend()
    {
        float fDist = (this.transform.position - end.position).sqrMagnitude;

        Vector2 curPos = this.transform.position;

        while (fDist > float.Epsilon)
        {
            Vector2 newPosition = Vector2.MoveTowards(Rb.position, end.position, speed * Time.deltaTime);

            Rb.MovePosition(newPosition);

            fDist = (this.transform.position - end.position).sqrMagnitude;

            yield return null;
        }

        yield return StartCoroutine(moveStart());
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
            collision.collider.transform.SetParent(transform);
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
            collision.collider.transform.SetParent(null);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
            collision.transform.SetParent(transform);
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
            collision.transform.SetParent(null);
    }

}

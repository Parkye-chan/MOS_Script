using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class TestMonster : MonoBehaviour
{

    Rigidbody2D rb;
    Vector2 direction;
    BoxCollider2D box;
    [SerializeField]
    Transform point;
    [SerializeField]
    Transform FowardPoint;
    float val = 0;
    [SerializeField]
    List<Transform> PatrollPoint = new List<Transform>();
    Transform curpoint;
    int pointNum = 0;
    float axis = 0;
    [SerializeField]
    float GroundFrontDist = 0.2f;
    [SerializeField]
    float GroundCheckPivot;
    [SerializeField]
    float moveSpeed;
    SpriteRenderer sprite;
    bool isTurn = false;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        box = GetComponent<BoxCollider2D>();
        curpoint = PatrollPoint[0];
        sprite = GetComponent<SpriteRenderer>();

    }

    // Update is called once per frame

    private void FixedUpdate()
    {

        //Move();
       
        LeftRightTest();       
        //Test3();
    }



    private void LeftRightTest()
    {
        Vector2 frontVec;
        if (!isTurn)
        {
            rb.velocity = Vector2.left * moveSpeed;
            frontVec = new Vector2(transform.position.x + -1 * GroundFrontDist, transform.position.y - GroundCheckPivot);
        }
            
        else
        {
            rb.velocity = Vector2.right * moveSpeed;
            frontVec = new Vector2(transform.position.x + 1 * GroundFrontDist, transform.position.y - GroundCheckPivot);
        }
            //rb.velocity = new Vector2(moveSpeed,rb.velocity.y);

        
        RaycastHit2D hitInfo = Physics2D.Raycast(frontVec, Vector3.down, 1, LayerMask.GetMask("Ground"));
        //Debug.DrawRay(point.position, (point.position - transform.position).normalized* 0.2f, Color.red);
        Debug.DrawRay(frontVec, Vector3.down, Color.red);
        RaycastHit2D hitFront = Physics2D.Raycast(FowardPoint.position, (FowardPoint.position - transform.position).normalized, 0.2f, LayerMask.GetMask("Ground"));
        Debug.DrawRay(FowardPoint.position, (FowardPoint.position - transform.position).normalized*0.2f, Color.red);
        if (hitInfo.collider == null)
        {                     
            Turn();
        }
        else if(hitFront)
        {
            Turn();
        }
        
    }

    private void Turn()
    {
        // moveSpeed *= -1;
        // sprite.flipX = moveSpeed == 1;
        if (isTurn)
        {
            transform.rotation = Quaternion.Euler(0, 0, 0);
            isTurn = false;
        }
        else
        {
            transform.rotation = Quaternion.Euler(0, 180, 0);
            isTurn = true;
        }
    }

    private void Test3()
    {

        //Vector2 dir = curpoint.position - transform.position;        
        Vector3 myPos = transform.position;
        Vector3 targetPos = curpoint.position;
        targetPos.z = myPos.z;

        Vector3 vectotarget = targetPos - myPos;
        Vector3 quaterniontotarget = Quaternion.Euler(0, 0, axis-90) * vectotarget;

        Quaternion targetRotation = Quaternion.LookRotation(Vector3.forward,  quaterniontotarget);

        transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, 60 * Time.deltaTime);

        Vector2 newpos = Vector2.MoveTowards(rb.position, curpoint.position, 1.0f* Time.deltaTime);
        rb.MovePosition(newpos);
        //rb.velocity = (curpoint.position - transform.position).normalized * 1.0f;
        // transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(dir), 10.0f * Time.deltaTime);

        float dist = Vector3.Distance(transform.position,curpoint.position);
        Debug.Log(dist);
        if (dist < 0.1)
        {
            pointNum++;
            if (pointNum >= PatrollPoint.Count)
                pointNum = 0;

            curpoint = PatrollPoint[pointNum];
        }
    }
 
    private void TopDownTest()
    {
        Vector2 frontVec;
        RaycastHit2D hitInfo;
        if (!isTurn)
        {
            rb.velocity = Vector2.up * moveSpeed;
            frontVec = new Vector2(transform.position.x + -1 * GroundFrontDist, transform.position.y - GroundCheckPivot);
           
        }

        else
        {
            rb.velocity = Vector2.down * moveSpeed;
            frontVec = new Vector2(transform.position.x + 1 * GroundFrontDist, transform.position.y - GroundCheckPivot);
            
        }

     
        {
            hitInfo = Physics2D.Raycast(frontVec, Vector3.left, 1, LayerMask.GetMask("Ground"));
            Debug.DrawRay(frontVec, Vector3.left, Color.red);
        }
      
        {
            hitInfo = Physics2D.Raycast(frontVec, Vector3.right, 1, LayerMask.GetMask("Ground"));
            Debug.DrawRay(frontVec, Vector3.right, Color.red);
        }
        
        
        RaycastHit2D hitFront = Physics2D.Raycast(FowardPoint.position, (FowardPoint.position - transform.position).normalized, 0.2f, LayerMask.GetMask("Ground"));
        Debug.DrawRay(FowardPoint.position, (FowardPoint.position - transform.position).normalized * 0.2f, Color.red);

        if (hitInfo.collider == null)
        {
            Turn();
        }
        else if (hitFront)
        {
            Turn();
        }
    }
}

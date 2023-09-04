using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TopDownMonster : MonoBehaviour
{


    private ConstantForce2D gravity;
    private Rigidbody2D rb;
    [SerializeField]
    private float moveSpeed;
    [SerializeField]
    private float GroundCheckPivot;
    [SerializeField]
    private Transform FrontPoint;
    private bool isTurn = false;
    private float frontvecval = 1;
    void Start()
    {
        gravity = GetComponent<ConstantForce2D>();
        rb = GetComponent<Rigidbody2D>();

    }

    private void FixedUpdate()
    {

        UpDownFunc();
    }


    private void UpDownFunc()
    {

        Vector2 frontVec;
        frontVec = new Vector2(transform.position.x , transform.position.y - +frontvecval * GroundCheckPivot);
        RaycastHit2D hitInfoLeft = Physics2D.Raycast(frontVec, Vector3.left, 1, LayerMask.GetMask("Ground"));
        RaycastHit2D hitInfoRight = Physics2D.Raycast(frontVec, Vector3.right, 1, LayerMask.GetMask("Ground"));
        RaycastHit2D hitFront = Physics2D.Raycast(FrontPoint.position, (FrontPoint.position - transform.position).normalized, 0.2f, LayerMask.GetMask("Ground"));

        Debug.DrawRay(frontVec, Vector3.left , Color.red);
        Debug.DrawRay(frontVec, Vector3.right, Color.red);
        Debug.DrawRay(FrontPoint.position, (FrontPoint.position - transform.position).normalized * 0.2f, Color.red);
        if (hitInfoLeft)
        {
            if (gravity.force == Vector2.zero)
            {
                gravity.force = new Vector2(-9.8f, 0);
                
            }
            Move();
        }
        else if (hitInfoRight)
        {
            if (gravity.force == Vector2.zero)
            {
                gravity.force = new Vector2(9.8f, 0);
                
            }
            Move();
        }
        else if(hitInfoLeft.collider == null || hitInfoRight.collider == null)
        {
            Turn();
        }

        if (hitFront)
            Turn();

    }

    private void Move()
    {
        if (isTurn)
            rb.velocity = Vector2.up * moveSpeed;
        else
            rb.velocity = Vector2.down * moveSpeed;
    }

    private void Turn()
    {
        if (isTurn)
        {
            transform.rotation = Quaternion.Euler(0, 0, 180);
            frontvecval *= -1;
            isTurn = false;
        }
        else
        {
            transform.rotation = Quaternion.Euler(0, 0, 0);
            frontvecval *= -1;
            isTurn = true;
        }
    }
}

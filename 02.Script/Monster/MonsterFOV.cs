using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterFOV : MonoBehaviour
{
    public LayerMask ObstaclesLayer;
    public LayerMask TargetLayer;
    public float TargetRayDist;
    public float AttackRayDist;
    public float WallCheckRayDist;
    public float GroundFrontDist = 0.2f;
    public float GroundCheckPivot;
    public float Radius = 0.5f;
    public bool UseRadius = false;

    public BoxCollider2D TargetDetectBox;
    public BoxCollider2D AttackRangeBox;
    public Vector2 RayDir = Vector2.left;
    public Transform target;

    public bool TargetDetect()
    {
        if (!UseRadius)
        {
            if (!TargetDetectBox)
                return false;

            RaycastHit2D hitInfo = Physics2D.BoxCast(TargetDetectBox.bounds.center, TargetDetectBox.bounds.size, 0f, Vector2.zero, 0, TargetLayer);
            if (hitInfo)
            {
                target = hitInfo.transform;
                return true;
            }
            return false;
        }
        else
        {
            Collider2D hitInfo = Physics2D.OverlapCircle(transform.position, Radius, TargetLayer);

            if (hitInfo)
            {
                target = hitInfo.transform;
                return true;
            }
            return false;
        }      
    }

    public bool AttackRange()
    {
        if (!AttackRangeBox)
            return false;

        RaycastHit2D hitInfo = Physics2D.BoxCast(AttackRangeBox.bounds.center, AttackRangeBox.bounds.size, 0f, Vector2.zero, 0, TargetLayer);
        if (hitInfo)
        {
            return true;
        }
        return false;
    }

    public bool WallCheck()
    {
        RaycastHit2D hitInfo = Physics2D.Raycast(transform.position, RayDir, WallCheckRayDist, ObstaclesLayer);
        Debug.DrawRay(transform.position, RayDir * WallCheckRayDist, Color.red);
        if (hitInfo)
        {
            return true;
        }
        return false;
    }

    public bool GroundCheck()
    {
        Vector2 frontVec = new Vector2(transform.position.x + RayDir.x * GroundFrontDist, transform.position.y - GroundCheckPivot);

        Debug.DrawRay(frontVec, Vector3.down, Color.red);
        RaycastHit2D rayhit = Physics2D.Raycast(frontVec, Vector3.down, 1, ObstaclesLayer);

        if (rayhit.collider == null)
            return false;
        else
            return true;
    }

    private void OnDrawGizmos()
    {
        if (UseRadius)
        {
            Gizmos.color = Color.white;
            Gizmos.DrawWireSphere(transform.position, Radius);
        }
    }
}

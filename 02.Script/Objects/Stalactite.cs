using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MoreMountains.CorgiEngine;
using MoreMountains.Tools;

public class Stalactite : MonoBehaviour
{


    public LayerMask ObstaclesLayer;
    public LayerMask TargetLayer;
    public float DetectionDistance = 10f; //  위아래 길이
    public float RayWidth = 1; // 너비
    private Vector2 _boxcastSize = Vector2.zero;
    private Vector2 _raycastOrigin;
    private Color _gizmosColor = Color.yellow;
    private Vector3 _gizmoCenter;
    private Vector3 _gizmoSize;
    private Rigidbody2D rb;
    private bool Active = false;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.gravityScale = 0;
    }

    private void OnDisable()
    {
        Active = false;
        rb.gravityScale = 0;
        rb.velocity = Vector3.zero;
    }

    // Update is called once per frame
    void Update()
    {
        if (TargetDetect() && !Active)
        {
            StartCoroutine(DropFunc());
        }
        else
            return;
    }

    private bool TargetDetect()
    {
        bool hit = false;
        RaycastHit2D raycast;
        _boxcastSize.x = RayWidth;
        _boxcastSize.y = DetectionDistance;
        _raycastOrigin.x = transform.position.x;
        _raycastOrigin.y = transform.position.y;

        raycast = Physics2D.BoxCast(_raycastOrigin + Vector2.down * DetectionDistance / 2f, _boxcastSize, 0f, Vector2.zero, DetectionDistance, TargetLayer);

        if (raycast)
        {
            hit = true;
            if(hit)
            {
                raycast = Physics2D.BoxCast(_raycastOrigin + Vector2.down * DetectionDistance / 2f, _boxcastSize, 0f, Vector2.zero, DetectionDistance, ObstaclesLayer);
                
                if (raycast)
                    return false;                
                else
                    return true;
            }
            else
                return false;
        }
        else
            return false;
    }

    IEnumerator DropFunc()
    {
        Active = true;
        yield return new WaitForSecondsRealtime(1.0f);
        rb.gravityScale = 1;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = _gizmosColor;

        _gizmoCenter = (Vector3)_raycastOrigin + Vector3.down * DetectionDistance / 2f;
        _gizmoSize.x = RayWidth;
        _gizmoSize.y = DetectionDistance ;
        Gizmos.DrawWireCube(_gizmoCenter, _boxcastSize);
    }
}

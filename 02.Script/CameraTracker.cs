using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using MoreMountains.CorgiEngine;

public class CameraTracker : MonoBehaviour
{
    [SerializeField]
    public GameObject objTarget;
    [SerializeField]
    float lerfSpeed;
    [SerializeField]
    private CinemachineVirtualCamera cinemachine;

    public float moveSpeed; // 카메라가 얼마나 빠른 속도로
    public bool BosszonOn = false;
    public bool LookUp = false;

    public BoxCollider2D bound;

    private Vector3 minBound;
    private Vector3 maxBound;
    private float halfWidth;
    private float halfHeight;
    private Camera theCamera;

    private Vector3 targetPosition; // 대상의 현재 위치 

    private void Start()
    {
        theCamera = GetComponent<Camera>();
       // minBound = bound.bounds.min;
        //maxBound = bound.bounds.max;
       // halfHeight = theCamera.orthographicSize;
       // halfWidth = halfHeight * Screen.width / Screen.height;
    }

    private void FixedUpdate()
    {
        /*
        if (objTarget && !BosszonOn)
        {          
            if (!LookUp)
            {
                /*
                Vector3 vTargetPos = objTarget.transform.position;
                vTargetPos.z = -10f;
                //Vector3 vDir = vDist.normalized; // 거리벡터에서방향 추출
                //float fDist = vDist.magnitude;
                this.transform.position = vTargetPos;
                *//*
                targetPosition.Set(objTarget.transform.position.x, objTarget.transform.position.y, this.transform.position.z);

                this.transform.position = Vector3.Lerp(this.transform.position, targetPosition, moveSpeed * Time.deltaTime); // 1초에 movespeed만큼 이동.

                float clampedX = Mathf.Clamp(this.transform.position.x, minBound.x + halfWidth, maxBound.x - halfWidth);
                float clampedY = Mathf.Clamp(this.transform.position.y, minBound.y + halfHeight, maxBound.y - halfHeight);

                this.transform.position = new Vector3(clampedX, clampedY, this.transform.position.z);
                cinemachine.Follow = objTarget.transform;
            }
            else
            {
                Vector3 vTargetPos = objTarget.transform.position;
                vTargetPos.z = this.transform.position.z;
                this.transform.position = vTargetPos + new Vector3(0, 3, 0);
            }
        }
        else if (objTarget && BosszonOn)
        {
            Vector3 FixedPos =
                new Vector3(
                objTarget.transform.position.x,
               this.transform.position.y, -10f);
            //Vector2 temp = new Vector2(objTarget.transform.position.x, 0);
            transform.position = Vector3.Lerp(transform.position, FixedPos, Time.deltaTime * lerfSpeed);
            StartCoroutine(Bosszonin());
        }
        else if (GameManager.instance.BossBattle && !GameManager.instance.player)
        {
            Debug.Log("GameOver");
        }*/
        if (MoreMountains.CorgiEngine.GameManager.Instance.PersistentCharacter)
        {
           
            if(objTarget)
            {
                Vector3 vTargetPos = objTarget.transform.position;
                vTargetPos.z = -10f;
               
                this.transform.position = vTargetPos;
            }
            else
            {
                objTarget = MoreMountains.CorgiEngine.GameManager.Instance.PersistentCharacter.gameObject;
            }
        }
        else
            return;
    }
    

        IEnumerator Bosszonin()
        {
            yield return new WaitForSeconds(3.0f);
            
            BosszonOn = false;
        }

        public void SetBound(BoxCollider2D newBound)
        {
        bound = newBound;
        CompositeCollider2D tempCol = newBound.GetComponent<CompositeCollider2D>();
        cinemachine.GetComponent<CinemachineConfiner>().m_BoundingShape2D = tempCol;
        

        cinemachine.transform.position = newBound.transform.position;

        minBound = bound.bounds.min;
            maxBound = bound.bounds.max;
        }
    }


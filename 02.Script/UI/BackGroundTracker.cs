using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackGroundTracker : MonoBehaviour
{

    
    private GameObject target;

    [SerializeField]
    Camera theCamera;

    public BoxCollider2D bound;
    public float moveSpeed;

    private Vector3 minBound;
    private Vector3 maxBound;
    private float halfWidth;
    private float halfHeight;
    private Vector3 targetPosition; // 대상의 현재 위치 


    // Start is called before the first frame update
    void Start()
    {


        minBound = bound.bounds.min;
        maxBound = bound.bounds.max;
        halfHeight = theCamera.orthographicSize;
        halfWidth = halfHeight * Screen.width / Screen.height;
    }



    private void FixedUpdate()
    {
        /*
        if(target)
        transform.position = new Vector3(target.transform.position.x, target.transform.position.y, +20);
        else if(!target && GameManager.instance.player)
            target = GameManager.instance.player.gameObject;
            */
        if (target)
        {
            targetPosition.Set(target.transform.position.x, target.transform.position.y, this.transform.position.z);

            this.transform.position = Vector3.Lerp(this.transform.position, targetPosition, moveSpeed * Time.deltaTime); // 1초에 movespeed만큼 이동.

            float clampedX = Mathf.Clamp(this.transform.position.x, minBound.x + halfWidth, maxBound.x - halfWidth);
            float clampedY = Mathf.Clamp(this.transform.position.y, minBound.y + halfHeight, maxBound.y - halfHeight);

            this.transform.position = new Vector3(clampedX, clampedY, this.transform.position.z);

        }

    }
}

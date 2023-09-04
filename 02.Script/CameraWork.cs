using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MoreMountains.CorgiEngine;
using Cinemachine;

public class CameraWork : MonoBehaviour
{
    [SerializeField]
    float ScreenX = 0;
    [SerializeField]
    float ScreenY = 0;
    [SerializeField]
    Room TargetRoom;
    [SerializeField]
    Transform CameraTransform;
    [SerializeField]
    float softWidth = 0;
    [SerializeField]
    float softHeight = 0;
    [SerializeField]
    float offsetX = 0;
    [SerializeField]
    float offsetY = 0;
    [SerializeField]
    float DeadZonWidth = 0;
    [SerializeField]
    float DeadZonHeight = 0;

    public bool TargetPlayer = false;
    

    private CinemachineVirtualCamera camera;
    private CinemachineFramingTransposer cameraPos;

    private void Start()
    {
        if (!TargetRoom)
        {
            try
            {
                TargetRoom = GetComponentInParent<Room>();
            }
            catch
            {
                Debug.Log("no targetRoom");
            }
        }
        camera = TargetRoom.VirtualCamera;
        cameraPos = camera.GetCinemachineComponent<CinemachineFramingTransposer>();
    }

    private void CameraWorks(float X, float Y, Transform Pos, Collider2D col)
    {
        if (Pos)
        {
            StartCoroutine(softZoneOn(cameraPos.m_SoftZoneWidth, cameraPos.m_SoftZoneHeight));
            
            camera.Follow = Pos.transform;
        }
        else if(X != 0 && Y != 0 && Pos)
        {
            StartCoroutine(softZoneOn(cameraPos.m_SoftZoneWidth, cameraPos.m_SoftZoneHeight));
        }
        else
        {
            camera.Follow = col.transform.Find("CameraTarget");
            StartCoroutine(CameraPosReturn(ScreenX, ScreenY));
            
        }
       
    }

    public void ActiveCameraWorks()
    {
        if (CameraTransform)
        {
            StartCoroutine(softZoneOn(cameraPos.m_SoftZoneWidth, cameraPos.m_SoftZoneHeight));

            camera.Follow = CameraTransform.transform;
        }
        else if (ScreenX != 0 && ScreenY != 0 && CameraTransform)
        {
            StartCoroutine(softZoneOn(cameraPos.m_SoftZoneWidth, cameraPos.m_SoftZoneHeight));
        }
        else
        {
            camera.Follow = MoreMountains.CorgiEngine.GameManager.Instance.PersistentCharacter.transform.Find("CameraTarget");
            StartCoroutine(CameraPosReturn(ScreenX, ScreenY));

        }
    }

    IEnumerator softZoneOn(float SoftWidth,float SoftHeight)
    {
        float tempsoftWidthVal = SoftWidth;
        float tempsoftHeightVal = SoftHeight;

        cameraPos.m_SoftZoneWidth = 2.0f;
        cameraPos.m_SoftZoneHeight = 2.0f;

        yield return new WaitForSeconds(0.5f);

        cameraPos.m_SoftZoneWidth = tempsoftWidthVal;
        cameraPos.m_SoftZoneHeight = tempsoftHeightVal;

    }

    IEnumerator CameraPosReturn(float X, float Y)
    {
        cameraPos.m_SoftZoneWidth = 2.0f;
        cameraPos.m_SoftZoneHeight = 2.0f;
        cameraPos.m_DeadZoneHeight = 0f;
        cameraPos.m_DeadZoneWidth = 0f;
        cameraPos.m_TrackedObjectOffset = new Vector3(offsetX, offsetY, 0);
        cameraPos.m_ScreenX = X;
        cameraPos.m_ScreenY = Y;
        cameraPos.m_DeadZoneHeight = DeadZonHeight;
        cameraPos.m_DeadZoneWidth = DeadZonWidth;
        yield return new WaitForSeconds(0.5f);
        cameraPos.m_SoftZoneWidth = softWidth;
        cameraPos.m_SoftZoneHeight = softHeight;


    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
  
        if (collision.CompareTag("Player"))
        {
            /*
           CameraMoveStateCheck check =  collision.GetComponent<CameraMoveStateCheck>();
            if (TargetPlayer == check.PlayerTarget)
                return;
           else
            {
                if(!check.PlayerTarget)
                check.PlayerTarget = true;
                else
                    check.PlayerTarget = false;
                    */
                CameraWorks(ScreenX, ScreenY, CameraTransform, collision);
        //    }
        }
    }


}

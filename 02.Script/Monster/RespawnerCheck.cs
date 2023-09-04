using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MoreMountains.CorgiEngine;

public class RespawnerCheck : MonoBehaviour
{
    private Respawner[] Startrespawners;
    private Respawner[] Endrespawners;
    public Room curRoom;

    public Room Targetroom;
    public GameObject MapMarker;
    public Transform MonsterSpawner;
   // public Camera minimapCamera;
    public bool isGate = false;
    public bool BGMChange = false;
    public GameObject TargetPlatform;
    public GameObject CurRoomPlatform;


    private void Start()
    {
        if(MonsterSpawner)
        Startrespawners = MonsterSpawner.GetComponentsInChildren<Respawner>();
        if(Targetroom)
        Endrespawners = Targetroom.GetComponentsInChildren<Respawner>();

        curRoom = GetComponentInParent<Room>();

       // if (minimapCamera)
        //    minimapCamera.transform.position = this.transform.position+ new Vector3(0,0,-10);

    }


    public void GateSpawnerInit()
    {
        Endrespawners = Targetroom.GetComponentsInChildren<Respawner>();
        int BGMNum = Targetroom.BGM_Number;
        Targetroom.MusicChange(BGMNum);
    }

    public void ResponTimerStart()
    {
        if (Endrespawners != null)
        {

            for (int i = 0; i < Endrespawners.Length; i++)
            {
                Endrespawners[i].SpawnObject();
                Endrespawners[i].RespawnStart = true;
            }
        }
    }

    public void ResponTimerEnd()
    {
        if (Startrespawners == null)
            return;

        for (int i = 0; i < Startrespawners.Length; i++)
        {
            Startrespawners[i].KillSelf();
        }
    }

    private void ActivateMapMarker()
    {
        if (MapMarker && curRoom.PlayerMapMarker)
        {
            MapMarker.SetActive(true);
            curRoom.PlayerMapMarker.transform.parent = Targetroom.transform;
            curRoom.PlayerMapMarker.transform.position = MapMarker.transform.position;
            Targetroom.PlayerMapMarker = curRoom.PlayerMapMarker.transform;
            Targetroom.MinimapCamera = curRoom.MinimapCamera;
            curRoom.MinimapCamera.transform.position = MapMarker.transform.position + new Vector3(0, 0, -10); ;
        }
    }

    private void ActivateBackGround()
    {
        curRoom.BackGround.SetActive(false);
        Targetroom.BackGround.SetActive(true);
    }

    public void GateUse()
    {
        if (GetComponent<Gate>())
        {
            GetComponent<Gate>().DestinaionOn = false;
            StartCoroutine(RoomMoveProcess());
        }

        else
            return;
    }

    public void MustGateUse()
    {
        StartCoroutine(MustRoomMove());
    }

    private void PlatformLoad()
    {
        if (!TargetPlatform)
            return;

        if (TargetPlatform == CurRoomPlatform)
            return;

        TargetPlatform.SetActive(true);
        CurRoomPlatform.SetActive(false);
    }

    IEnumerator RoomMoveProcess()
    {
        yield return new WaitForSecondsRealtime(0.5f);
        PlatformLoad();
        ResponTimerStart();
        ResponTimerEnd();
        Targetroom.BackGround.SetActive(true);
        Targetroom.Objects.SetActive(true);
        ActivateMapMarker();
        yield return new WaitForSecondsRealtime(0.5f);
        if(curRoom.BackGround)
        curRoom.BackGround.SetActive(false);
        if (curRoom.Objects)
        curRoom.Objects.SetActive(false);
        if (BGMChange)
            Targetroom.MusicChange(Targetroom.BGM_Number);
    }

    IEnumerator MustRoomMove()
    {
        yield return new WaitForSecondsRealtime(0.5f);
        PlatformLoad();
        Targetroom.BackGround.SetActive(true);
        Targetroom.Objects.SetActive(true);
        yield return new WaitForSecondsRealtime(0.5f);
        if (curRoom.BackGround)
            curRoom.BackGround.SetActive(false);
        if (curRoom.Objects)
            curRoom.Objects.SetActive(false);
        if (BGMChange)
            Targetroom.MusicChange(Targetroom.BGM_Number);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Player") && !isGate)
            StartCoroutine(RoomMoveProcess());
        /*
        ResponTimerStart();
        ResponTimerEnd();
        ActivateMapMarker();
        ActivateBackGround();*/
    }
}

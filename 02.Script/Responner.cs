using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Responner : MonoBehaviour
{
    public GameObject objPlayer;

    public string prefabName;

    [SerializeField]
    float ResponTime;

    //private bool Reserv;
    public bool Dead = false;
    /*
    IEnumerator ProcTime()
    {
        
        InitStatus();
        Reserv = true;
        yield return new WaitForSeconds(ResponTime);
        ResponObject();
        Reserv = false;
    }*/

    private void Start()
    {
        //if (prefabName != "None" && !Dead)
            ResponObject(); // 시작할때만들고시작
    }

    public void ResponObject()
    {
        if (prefabName != "None")
        {
            if(objPlayer && !objPlayer.activeSelf)
            {
                objPlayer.SetActive(true);
                objPlayer.transform.position = this.transform.position;
                return;
            }

            GameObject prefab = Resources.Load(prefabName) as GameObject;
            if (!prefab)
                return;
            else
            {
                //오브젝트 생성
                objPlayer = Instantiate(prefab);
                objPlayer.name = prefabName;
                //생성된 오브젝트를 부활위치로 옮겨줌
                objPlayer.transform.position = this.transform.position;
            }
        }
        else
        {
            if (!objPlayer)
                return;
            else
            {

                //오브젝트 생성
                Debug.Log("sad");
                objPlayer.SetActive(true);
                //objPlayer.name = prefabName;
                //생성된 오브젝트를 부활위치로 옮겨줌
                objPlayer.transform.position = this.transform.position;
                MonsterInit();
            }
        }
    }

    private void InitStatus()
    {
        Dead = false;       

    }

    public void MonsterInit()
    {
        Dead = false;

        Rigidbody2D rigidbody2D = objPlayer.GetComponent<Rigidbody2D>();
        rigidbody2D.constraints = RigidbodyConstraints2D.None;
        rigidbody2D.constraints = RigidbodyConstraints2D.FreezeRotation;
        BoxCollider2D boxCollider2D = objPlayer.GetComponent<BoxCollider2D>();
        boxCollider2D.enabled = true;

    }

    void Update()
    {
        /*
        if (objPlayer != null && !Reserv && !GameManager.instance.BossBattle)
        {
            if(Dead)
            StartCoroutine(ProcTime());
           
        }3month 5 EA
        */
        /*
        if (objPlayer && objPlayer.CompareTag("Player"))
        {
            if (!objPlayer.activeSelf && !Dead)
            {
                Status status = objPlayer.GetComponent<Status>();
                status.Dead = false;
                status.Init(objPlayer.name, status.m_nPotion, status.m_nHP, status.m_nMP,status.m_ndecreaseMp ,status.m_nStr,status.m_nSpeed,status.m_nMoneyVal);
                GameManager.instance.InitHpbar();
                objPlayer.SetActive(true);
               // objPlayer.GetComponent<SuperMode>().SetSuperMode();
            }
        }*/
    }
}

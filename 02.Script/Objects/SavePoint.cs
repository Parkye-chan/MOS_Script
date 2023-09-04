using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MoreMountains.CorgiEngine;
using MoreMountains.Feedbacks;

public class SavePoint : MonoBehaviour
{

    public CheckPoint checkPoint;
    public bool CurSave;
    public GameObject SaveRight;
    public string TriggerParam = "Trigger";
    public GameObject Platform;
    public GameObject Background;
    public GameObject Mapobj;
    public Transform MonsterSpawner;
    public Room room;
    public MMFeedbacks SaveSound;
    private Animator _anim;
    private Respawner[] Startrespawners;

    void Start()
    {
        Init();       
    }

    public void SaveFunc()
    {
        if (InventoryManager.Instance.OpenInventory)
            return;

        SaveSound.PlayFeedbacks();
        _anim.SetTrigger(TriggerParam);
        checkPoint.gameObject.SetActive(true);
        CurSave = true;
        LevelManager.Instance.PotionInit();
        Health health = GameManager.Instance.PersistentCharacter.GetComponent<Health>();
        health.ResetHealthToMaxHealth();
        InfoManager.instance.GiveInitVal(health.MaximumHealth, health.CurrentHealth);
        PlayerInfoManager.instance.SavePointSave(this);
        StartCoroutine(SaveTime());
    }

    

    IEnumerator SaveTime()
    {
        yield return new WaitForSeconds(1.0f);
        
        checkPoint.gameObject.SetActive(false);
    }

    public void MonsterSpawn()
    {
        if (MonsterSpawner)
        {
            Startrespawners = MonsterSpawner.GetComponentsInChildren<Respawner>();
            for (int i = 0; i < Startrespawners.Length; i++)
            {
                Startrespawners[i].SpawnObject();
                Startrespawners[i].RespawnStart = true;
            }
        }
    }

    private void Init()
    {
        _anim = SaveRight.GetComponent<Animator>();
        if (CurSave)
        {
            room.MusicChange(room.BGM_Number);
        }
    }
}

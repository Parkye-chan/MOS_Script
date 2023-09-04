using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MoreMountains.CorgiEngine;
using MoreMountains.Tools;


public class Respawner : MonoBehaviour
{

    public GameObject objPlayer;
    public float RespawnTime;
    public bool RespawnStart = false;

    private bool Dead = false;

    private float curRespawnTime = 0f;
    private Character _character;
    private DamageOnTouch damage;
    private AIBrain brain;
    private GameObject tempItem;

    private void Start()
    {
        _character = objPlayer.GetComponent<Character>();
        damage = objPlayer.GetComponent<DamageOnTouch>();
        brain = objPlayer.GetComponent<AIBrain>();
        if (RespawnStart)
            SpawnObject();
    }

    void Update()
    {
        if (RespawnStart) // 플레이어가 방안에 있으면
        {
           
            if (curRespawnTime <= 0)
            {
                //StartCoroutine(SpawnTime(RespawnTime));
                SpawnObject();
            }

            if (!objPlayer.activeSelf && !Dead)
            {
                Dead = true;
                DropItem drop = objPlayer.GetComponent<DropItem>();
                if (drop)
                {
                    tempItem = drop.DropFunc();
                }
            }

        }
        else // 플레이어가 방밖에 있으면
        {
            if (curRespawnTime > 0)
                curRespawnTime -= Time.deltaTime;
        }

    }

    public void KillSelf()
    {
        if (tempItem != null)
        {
            Destroy(tempItem);
            tempItem = null;
        }
        objPlayer.SetActive(false);
        Dead = true;
        curRespawnTime = RespawnTime;
        RespawnStart = false;
    }

    public void SpawnObject()
    {
        objPlayer.SetActive(true);
        objPlayer.transform.position = this.transform.position;
        Dead = false;
        if (_character)
        {
            _character.ConditionState.ChangeState(CharacterStates.CharacterConditions.Normal);
            damage.enabled = true;
        }
        if (brain)
        {
            brain.enabled = true;
            brain.Target = null;           
        }

        curRespawnTime = RespawnTime;
        return;
    }
    /*
    IEnumerator SpawnTime(float time)
    {
        Resurrect = true;
        yield return new WaitForSeconds(time);
        SpawnObject();
        Resurrect = false;
    }*/
}



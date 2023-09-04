using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MoreMountains.CorgiEngine;
using MoreMountains.Tools;

public class BarrackRespawner : MonoBehaviour
{

    public Transform[] SpawnPoses;
    public float SpawnTime = 2.0f;
    public GameObject[] Monsters;
    public int SpawnCount = 3;
    public GameObject ModelPos;

    private float curTime = 0;
    private Animator _anim;
    private List<GameObject> monsterPool = new List<GameObject>();
    private int curCount = 0;
    private List<GameObject> curSpawnMonster = new List<GameObject>();
    private GameObject monsterPoolPos;
    private Health health;

    void Start()
    {
        _anim = GetComponent<Animator>();
        health = GetComponent<Health>();
        monsterPoolPos = new GameObject("BarrackStorage");
        PoolingMonster();
        
    }

    private void OnEnable()
    {
        if(curSpawnMonster.Count > 0)
        {

            for (int i = 0; i < curSpawnMonster.Count; i++)
            {
                curSpawnMonster[i].SetActive(true);
            }
        }
    }

    private void OnDisable()
    {
        if (health.CurrentHealth <= 0)
        {
            //_anim.SetTrigger("Trigger");
            health.Revive();
            //gameObject.SetActive(true);
            ModelPos.SetActive(true);
        }

        if (curSpawnMonster.Count > 0)
        {
            for (int i = 0; i < curSpawnMonster.Count; i++)
            {
                if (curSpawnMonster[i] == null)
                    continue;

                curSpawnMonster[i].SetActive(false);
            }
        }
    }

    void Update()
    {

        if (health.CurrentHealth <= 0)
            return;

        if (curTime <= 0)
        {
            SpawnObject();
        }
        else if(curTime > 0)
            curTime -= Time.deltaTime;

        if(curCount >= SpawnCount)
            DeathCheck();

    }


    private void SpawnObject()
    {
        if (curCount < SpawnCount)
        {
            int RandomVal = Random.Range(0, monsterPool.Count);
            if (monsterPool[RandomVal].activeSelf)
                SpawnObject();
            else
            {
                int RandomSpawn = Random.Range(0,2);
                monsterPool[RandomVal].transform.position = SpawnPoses[RandomSpawn].position;
                monsterPool[RandomVal].SetActive(true);
                curSpawnMonster.Add(monsterPool[RandomVal]);
                curCount++;
                curTime = SpawnTime;
            }
        }
        else
            return;
    }

    private void PoolingMonster()
    {
        for (int i = 0; i < Monsters.Length; i++)
        {
            for (int j = 0; j < SpawnCount; j++)
            {
                GameObject tempobj = Instantiate(Monsters[i], monsterPoolPos.transform);
                tempobj.SetActive(false);
                monsterPool.Add(tempobj);
                
            }            
        }
    }

    private void DeathCheck()
    {
        for (int i = 0; i < curSpawnMonster.Count; i++)
        {
            if(!curSpawnMonster[i].activeSelf)
            {
                curTime = SpawnTime;
                curCount--;
                Character _character = curSpawnMonster[i].GetComponent<Character>();
                AIBrain brain = curSpawnMonster[i].GetComponent<AIBrain>();
                DamageOnTouch damage = curSpawnMonster[i].GetComponent<DamageOnTouch>();

                if (_character)
                {
                    _character.ConditionState.ChangeState(CharacterStates.CharacterConditions.Normal);
                    damage.enabled = true;
                }
                if (brain)
                    brain.enabled = true;

                curSpawnMonster.RemoveAt(i);
            }
        }
    }

}

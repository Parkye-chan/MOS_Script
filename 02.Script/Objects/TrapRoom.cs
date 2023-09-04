using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MoreMountains.CorgiEngine;
using UnityEngine.UI;

public class TrapRoom : MonoBehaviour
{

    public Health BoxHp;
    public GameObject[] Door;
    public float time = 0f;
    public TrapRespawners[] SpawnPos;
    public int WaveCount = 3;
    public int KillCount = 0;
    private int maxBoxHp;
    private bool isPlaying = false;

    void Start()
    {
        maxBoxHp = BoxHp.CurrentHealth;
        SpawnPos = transform.GetComponentsInChildren<TrapRespawners>();
        WaveCount = SpawnPos.Length;
    }


    void Update()
    {
        if (maxBoxHp != BoxHp.CurrentHealth)
            TrapFunc();
        else
            return;
    }


    private void TrapFunc()
    {

        if (!isPlaying)
            SpawnDoor();
            //StartCoroutine(FadeIn(time));

    }

    private void SpawnDoor()
    {
        isPlaying = true;
        BoxHp.enabled = false;
        for (int i = 0; i < Door.Length; i++)
        {
            Door[i].SetActive(true);            
        }
        StartCoroutine(SpawnWave(WaveCount));
    }


    private void DisappearDoor()
    {
        for (int i = 0; i < Door.Length; i++)
        {
            Door[i].GetComponent<Animator>().SetBool("Spawn", false);
        }
    }

    IEnumerator FadeIn(float fadeOutTime)
    {

        isPlaying = true;
        BoxHp.enabled = false;
        for (int i = 0; i < Door.Length; i++)
        {
            Door[i].SetActive(true);
            //SpriteRenderer sr = Door[i].GetComponent<SpriteRenderer>();
            Color tempColor = Door[i].GetComponent<SpriteRenderer>().color;
            while (tempColor.a < 1f)
            {
                tempColor.a += Time.deltaTime / fadeOutTime;
                Door[i].GetComponent<SpriteRenderer>().color = tempColor;

                if (tempColor.a >= 1f) tempColor.a = 1f;

                yield return null;
            }

            Door[i].GetComponent<SpriteRenderer>().color = tempColor;
        }

        StartCoroutine(SpawnWave(WaveCount));

    }

    IEnumerator FadeOut(float fadeOutTime)
    {

        isPlaying = true;

        for (int i = 0; i < Door.Length; i++)
        {
            
            //SpriteRenderer sr = Door[i].GetComponent<SpriteRenderer>();
            Color tempColor = Door[i].GetComponent<SpriteRenderer>().color;
            while (tempColor.a > 0f)
            {
                tempColor.a -= Time.deltaTime / fadeOutTime;
                Door[i].GetComponent<SpriteRenderer>().color = tempColor;

                if (tempColor.a <= 0f) tempColor.a = 0f;

                yield return null;
            }

            Door[i].GetComponent<SpriteRenderer>().color = tempColor;
            Door[i].SetActive(false);
        }      
    }

    IEnumerator OpenCheck(int curwave)
    {
        int checkcount = 0;
        for (int i = 0; i < SpawnPos[curwave].respawns.Length; i++)
        {
            if (!SpawnPos[curwave].respawns[i].Enemy.activeSelf)
            {
                checkcount++;
            }
        }

        yield return new WaitForSeconds(1.0f);

        if (checkcount == SpawnPos[curwave].respawns.Length)
        {
            StartCoroutine(BoxHp.gameObject.GetComponent<ItemBox>().BoxOpen());
            DisappearDoor();
            //StartCoroutine(FadeOut(time));
        }
        else
            StartCoroutine(OpenCheck(WaveCount));
    }

    IEnumerator SpawnWave(int curwave)
    {
        if (KillCount <= 0)
        {
            for (int i = 0; i < SpawnPos[curwave-1].respawns.Length; i++)
            {
                SpawnPos[curwave-1].respawns[i].EnemySpawn();
            }
            WaveCount--;
            StartCoroutine(SpawnWave(WaveCount));
        }
        else
        {
            if (WaveCount <= 0)
            {
                StartCoroutine(OpenCheck(WaveCount));
                yield break;
            }
            else
            {
                yield return new WaitForSeconds(2.0f);
                StartCoroutine(SpawnWave(WaveCount));
            }
        }
    }
}


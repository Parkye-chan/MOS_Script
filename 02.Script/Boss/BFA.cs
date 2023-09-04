using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BFA : MonoBehaviour
{
    public List<GameObject> WarningLine = new List<GameObject>();
    public List<GameObject> BFAEffect = new List<GameObject>();
    public ForestBoss Tiger;

    public void WarningFunc()
    {
        StartCoroutine(Warnings());
    }
    
    IEnumerator Warnings()
    {
        int cnt = 0;
        while (cnt < WarningLine.Count)
        {
            WarningLine[cnt].SetActive(true);
            yield return new WaitForSecondsRealtime(0.5f);
            cnt++;
        }

        //yield return new WaitForSecondsRealtime(1.0f);

        yield return StartCoroutine(AttackFunc());

    }

    private IEnumerator AttackFunc()
    {
        for (int i = 0; i < WarningLine.Count; i++)
        {
            WarningLine[i].SetActive(false);
        }


        int cnt = 0;
        while (cnt < BFAEffect.Count)
        {
            BFAEffect[cnt].SetActive(true);
            yield return new WaitForSecondsRealtime(0.5f);
            cnt++;
        }
        yield return StartCoroutine(Tiger.BFAGroundtouch());
    }
}

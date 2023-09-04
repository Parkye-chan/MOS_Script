using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MoreMountains.CorgiEngine;

public class Ending : MonoBehaviour
{
    public GameObject EndingDoor;
    public GameObject EndingCreditUI;

    private float Times = 0;

    public void SpawnDoor()
    {
        EndingDoor.SetActive(true);
    }

    public void EndingCredit()
    {
        EndingCreditUI.SetActive(true);
        StartCoroutine(EndingGo());
    }

    IEnumerator EndingGo()
    {
        while (Times < 25)
        {         

            if(Input.GetKeyDown(KeyCode.Escape))
            {
                EndingCreditUI.SetActive(false);
                PlayerInfoManager.instance.ClearSave();
                LevelManager.Instance.GotoLevel("StartScene");
                yield break;
            }

            Times += Time.deltaTime;
            yield return null;
        }
        PlayerInfoManager.instance.ClearSave();
        LevelManager.Instance.GotoLevel("StartScene");
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MoreMountains.CorgiEngine;
using MoreMountains.Feedbacks;

public class BossRoomObject : MonoBehaviour
{

    public string TrigerParam = "Trigger";
    public Sprite ClearImg;
    public GameObject CameraWork;
    public string EndeingSceneName;
    public MMFeedbacks Feedback01;
    public MMFeedbacks Feedback02;
    private Animator _anim;
    

    void Start()
    {
        _anim = GetComponent<Animator>();
    }

    public void MaskInsert()
    {
        _anim.SetTrigger(TrigerParam);
    }

    public void MaskClear()
    {
        GetComponent<SpriteRenderer>().sprite = ClearImg;
        
        StartCoroutine(GoEnding());
    }

    public void FeedBack01()
    {
        Feedback01.PlayFeedbacks();
    }

    public void FeedBack02()
    {
        Feedback02.PlayFeedbacks();
    }

    IEnumerator GoEnding()
    {
        
        yield return new WaitForSeconds(1.5f);
        DialogueManager.instance.Talk();
        CameraWork.SetActive(true);
        yield return new WaitForSeconds(4.0f);
        PlayerInfoManager.instance.ClearSave();
        LevelManager.Instance.GotoLevel(EndeingSceneName);
    }

}

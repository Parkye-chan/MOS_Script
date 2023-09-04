using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MoreMountains.Feedbacks;

public class PlayFeedback : MonoBehaviour
{
    public MMFeedbacks Feedback;
    public bool isEnable = false;
    public bool isDisable = false;
    public bool isStart = false;
    public bool isUpdate = false;

    private void OnEnable()
    {
        if (!isEnable)
            return;
        else
        {
            Feedback.PlayFeedbacks();
        }
    }

    private void Start()
    {
        if (!isStart)
            return;
        else
            Feedback.PlayFeedbacks();
    }

    private void OnDisable()
    {
        if (!isDisable)
            return;
        else
            Feedback.PlayFeedbacks();
    }

    private void Update()
    {
        if (!isUpdate)
            return;
        else
            Feedback.PlayFeedbacks();
    }

}

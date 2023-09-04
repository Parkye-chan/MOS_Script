using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MoreMountains.CorgiEngine;
using MoreMountains.Tools;
using MoreMountains.Feedbacks;

public class TrapRespawn : MonoBehaviour
{
    [Tooltip("the delay (in seconds) to apply before running the sequence")]
    public float InitialDelay = 0.1f;
    /// the duration (in seconds) after the initial delay covering for the fade out of the scene
    [Tooltip("the duration (in seconds) after the initial delay covering for the fade out of the scene")]
    public float FadeOutDuration = 0.2f;
    /// the duration (in seconds) to wait for after the fade out and before the fade in
    [Tooltip("the duration (in seconds) to wait for after the fade out and before the fade in")]
    public float DelayBetweenFades = 0.3f;
    /// the duration (in seconds) after the initial delay covering for the fade in of the scene
    [Tooltip("the duration (in seconds) after the initial delay covering for the fade in of the scene")]
    public float FadeInDuration = 0.2f;
    /// the duration (in seconds) to apply after the fade in of the scene
    [Tooltip("the duration (in seconds) to apply after the fade in of the scene")]
    public float FinalDelay = 0.1f;
    [MMCondition("TriggerFade", true)]
    [Tooltip("the ID of the fader to target")]
    public int FaderID = 0;
    /// the curve to use to fade to black
    [MMCondition("TriggerFade", true)]
    [Tooltip("the curve to use to fade to black")]
    public MMTweenType FadeTween = new MMTweenType(MMTween.MMTweenCurve.EaseInCubic);

    public float waitTime = 1.0f;


    IEnumerator TrapProcess(Collider2D collider)
    {
        MMFadeInEvent.Trigger(FadeOutDuration, FadeTween, FaderID, false, LevelManager.Instance.Players[0].transform.position);
        collider.GetComponent<Character>().ConditionState.ChangeState(CharacterStates.CharacterConditions.Frozen);
        yield return new WaitForSecondsRealtime(waitTime);
        collider.transform.position = collider.GetComponentInChildren<MaskSkills>().RespawnPos.position;
        yield return new WaitForSecondsRealtime(waitTime);
        MMFadeOutEvent.Trigger(FadeInDuration, FadeTween, FaderID, false, LevelManager.Instance.Players[0].transform.position);
        collider.GetComponent<Character>().ConditionState.ChangeState(CharacterStates.CharacterConditions.Normal);
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Player"))
        {
            if (collision.GetComponent<Health>().CurrentHealth <= 0)
                return;
            else
            {
                if (collision.GetComponentInChildren<MaskSkills>().RespawnPos)
                    StartCoroutine(TrapProcess(collision));
                else
                    Debug.Log("Plase Check RespawnPos");
            }
        }
    }
}

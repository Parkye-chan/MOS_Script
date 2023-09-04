using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MoreMountains.Tools;
using MoreMountains.CorgiEngine;

public class DelayDamage : MonoBehaviour
{

    public float Times = 3;
    public bool isFlicker = true;
    public Color FlickerColor = new Color32(255, 20, 20, 255);
    public bool PlayerBullet = false;
    public int Damage = 1;
    Health TargetHealth;
    SpriteRenderer sprite;
    GameObject Target;
    Character character;
    float curTime = 0;
    
    void Update()
    {
        if(TargetHealth)
        {
            if (!character)
                return;

            if (character.GetComponent<Character>().ConditionState.CurrentState == CharacterStates.CharacterConditions.Dead)
                return;

            if (curTime <= 0)
            {

                //TargetHealth.Damage(1,gameObject,0,0);
                TargetHealth.CurrentHealth -= Damage;
                StartCoroutine(Flicker(sprite,sprite.color,FlickerColor,0.5f,0.5f));
                curTime = Times;
            }
            else
                curTime -= Time.deltaTime;
        }
    }

    IEnumerator Flicker(SpriteRenderer renderer, Color initialColor, Color flickerColor, float flickerSpeed, float flickerDuration)
    {
        if (renderer == null)
        {
            yield break;
        }

        if (initialColor == flickerColor)
        {
            yield break;
        }

        if(!isFlicker)
        {
            yield break;
        }

        float flickerStop = Time.time + flickerDuration;

        while (Time.time < flickerStop)
        {
            renderer.color = flickerColor;
            yield return MMCoroutine.WaitFor(flickerSpeed);
            renderer.color = initialColor;
            yield return MMCoroutine.WaitFor(flickerSpeed);
        }

        renderer.color = initialColor;
    }

    private void OnTriggerStay2D(Collider2D collision)
    {      
        if (PlayerBullet && collision.gameObject.layer ==  LayerMask.NameToLayer("Enemies"))
        {
            TargetHealth = collision.GetComponent<Health>();
            character = collision.GetComponent<Character>();
            if (character)
                Target = character.CharacterModel;
            else
                Target = collision.gameObject;

            sprite = Target.GetComponent<SpriteRenderer>();

        }
        else if(!PlayerBullet && collision.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            TargetHealth = collision.GetComponent<Health>();
            character = collision.GetComponent<Character>();
            if (character)
                Target = character.CharacterModel;
            else
                Target = collision.gameObject;

            sprite = Target.GetComponent<SpriteRenderer>();
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Enemies") || collision.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            TargetHealth = null;
            curTime = 0;
        }
    }
}

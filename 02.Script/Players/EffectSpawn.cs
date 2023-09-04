using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MoreMountains.CorgiEngine;
using MoreMountains.Feedbacks;

public class EffectSpawn : MonoBehaviour
{
    public enum ElementState
    {
        None,
        Fire,
        Frost,
        Earth,
        Poison
    }

    public GameObject ParentsModel;
    public GameObject wallclibEffect;
    public GameObject DashEffect;
    public GameObject DashLightEffect;
    public GameObject DashAttackZone;
    public GameObject DoubleDashEffect;
    public GameObject ChargeEffect;
    public GameObject ChargeCompleteEffect;
    public GameObject[] ChargingAttackEffects;
    public GameObject Attack01Effect;
    public GameObject Attack02Effect;
    public GameObject Attack03Effect;
    public GameObject EarthShield;
    public GameObject FireFilar;
    public GameObject PosionMist;
    public GameObject FireDash;
    public GameObject FrostDash;
    public GameObject FireOrora;
    public Transform SkillPos;
    public GameObject SleepEffect;
    public bool ChargeOn = false;
    public MMFeedbacks FireDashSound;
    public MMFeedbacks IceDashSound;
    public ElementState state = ElementState.None;
    
    private Character character;
    private Health health;

    void Start()
    {
        character = ParentsModel.GetComponent<Character>();
        health = ParentsModel.GetComponent<Health>();
    }

    // Update is called once per frame
    void Update()
    {
        WallClibEffectSpawn();
    }

    private void WallClibEffectSpawn()
    {
        if (character.MovementState.CurrentState == CharacterStates.MovementStates.WallClinging && Input.GetKeyDown(KeyCode.Space))
        {
            Instantiate(wallclibEffect, transform.position, transform.rotation);
        }
    }

    public void StateChange(int StateNum)
    {
        state = (ElementState)StateNum;
        switch (state)
        {
            case ElementState.None:
                {
                    FireOrora.SetActive(false);
                }
                break;
            case ElementState.Fire:
                {
                    FireOrora.SetActive(true);
                }
                break;
            case ElementState.Frost:
                {
                    FireOrora.SetActive(false);
                }
                break;
            case ElementState.Earth:
                {
                    FireOrora.SetActive(false);
                }
                break;
            case ElementState.Poison:
                {
                    FireOrora.SetActive(false);
                }
                break;
        }
    }

    public void ChargeEffectSpawn()
    {
        ChargeEffect.SetActive(true);
    }

    public void ChargeEffectOff()
    {
        ChargeEffect.SetActive(false);
    }

    public void ChargeEffectComplete()
    {
        if (!ChargeOn)
        {
            ChargeEffect.SetActive(false);
            ChargeCompleteEffect.SetActive(true);
            ChargeOn = true;
        }
    }

    public void Attack01EffectSpawn()
    {
        Instantiate(Attack01Effect, transform.position, transform.rotation);
    }

    public void Attack02EffectSpawn()
    {
        Instantiate(Attack02Effect, transform.position, transform.rotation);
    }

    public void Attack03EffectSpawn()
    {
        Instantiate(Attack03Effect, transform.position, transform.rotation);
    }

    public void DashEffectSpawn()
    {
        Instantiate(DashEffect, transform.position, transform.rotation);
    }

    public void DashLightEffectSpawn()
    {
        //CharcterPos = Instantiate(DashLightEffect, transform.position, transform.rotation);
        DashLightEffect.SetActive(true);
    }

    public void ChargingAttackEffectSpawn()
    {
        switch (state)
        {
            case ElementState.None:
                {
                    Instantiate(ChargingAttackEffects[(int)state], transform.position, transform.rotation);
                }
                break;
            case ElementState.Fire:
                {
                    Instantiate(ChargingAttackEffects[(int)state], transform.position, transform.rotation);
                    Instantiate(FireFilar, SkillPos.position, SkillPos.rotation);
                }
                break;
            case ElementState.Frost:
                {
                    Instantiate(ChargingAttackEffects[(int)state], transform.position, transform.rotation);
                }
                break;
            case ElementState.Earth:
                {
                    Instantiate(ChargingAttackEffects[(int)state], transform.position, transform.rotation);
                    EarthShield.SetActive(true);
                }
                break;
            case ElementState.Poison:
                {
                    Instantiate(ChargingAttackEffects[(int)state], transform.position, transform.rotation);
                    GameObject temp = Instantiate(PosionMist, SkillPos.position, SkillPos.rotation);
                    Destroy(temp, 3.0f);
                }
                break;
        }
        
    }

    public void DashObjSpawn(Vector3 dir)
    {
        switch (state)
        {
            case ElementState.None:
                {
                    return;
                }
            case ElementState.Fire:
                {
                    GameObject tempbullet = Instantiate(FireDash, transform.position, transform.rotation);
                    tempbullet.GetComponent<Projectile>().Direction = dir;
                    FireDashSound.PlayFeedbacks();
                }
                break;
            case ElementState.Frost:
                {
                    GameObject tempbullet = Instantiate(FrostDash, transform.position, transform.rotation);
                    IceDashSound.PlayFeedbacks();
                }
                break;
            case ElementState.Earth:
                {
                    return;
                }
            case ElementState.Poison:
                {
                    //GameObject temp = Instantiate(PosionMist, transform.position, transform.rotation);
                    StartCoroutine(PosionDashMist(2));
                }
                break;
        }
    }

    IEnumerator PosionDashMist(float Times)
    {
        float curTime = Times;

        if (curTime <= 0)
            yield break;

        Instantiate(PosionMist, transform.position, transform.rotation);

         yield return new WaitForSeconds(0.05f);

         curTime -= Time.deltaTime;
        yield return StartCoroutine(PosionDashMist(curTime));

        
        
    }

    public void SpawnSpark()
    {
        SleepEffect.SetActive(true);
    }

    public void StateEffectDisable()
    {
        SleepEffect.SetActive(false);
    }

    public void PosionDashStop()
    {
        switch (state)
        {           
            case ElementState.Poison:
                {
                    StopAllCoroutines();
                    GetComponent<MaskSkills>().DashStop();
                }
                break;
            default:
                {
                    GetComponent<MaskSkills>().DashStop();
                }
                break;
        }      
        //health.Invulnerable = false;
    }

    public void DashAttackZoneOn()
    {
        DashAttackZone.SetActive(true);
        DoubleDashEffect.SetActive(true);
    }

}

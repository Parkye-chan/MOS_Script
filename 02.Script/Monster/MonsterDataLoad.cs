using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MoreMountains.CorgiEngine;

public class MonsterDataLoad : MonoBehaviour
{
    public string Name = "";
    public double Speed;
    public int Damage;
    public double InvTime;
    public int Health;
    public double DieTime;
    public int AttackDamage;
    public double AttackInvTime;
    public GameObject AttackPos;
    public bool Boss = false;

    private void Awake()
    {
        if (Name == "")
            Name = gameObject.name;

        if (!Boss)
            LoadData();
        else
            BossLoadData();
    }


    private void BossLoadData()
    {
        List<Dictionary<string, object>> data;
        data = CSVReader.Read("BossData");

        if (data != null)
        {
            for (int i = 0; i < data.Count; i++)
            {
                if (data[i]["Name"].ToString() == Name)
                {
                    Speed = (int)data[i]["MoveSpeed"];
                    Damage = (int)data[i]["Damage"];
                    InvTime = (int)data[i]["InvTime"];
                    Health = (int)data[i]["Health"];
                    DieTime = (int)data[i]["DieTime"]; // 실수형
                    if (GetComponent<CharacterHorizontalMovement>())
                    {
                        GetComponent<CharacterHorizontalMovement>().WalkSpeed = (float)Speed;
                    }
                    if (GetComponent<DamageOnTouch>())
                    {
                        DamageOnTouch bodyDamage = GetComponent<DamageOnTouch>();
                        bodyDamage.DamageCaused = Damage;
                        bodyDamage.InvincibilityDuration = (float)InvTime;
                    }
                    if (GetComponent<Health>())
                    {
                        Health health = GetComponent<Health>();
                        health.InitialHealth = Health;
                        health.MaximumHealth = Health;
                        health.DelayBeforeDestruction = (float)(DieTime * 0.01f);
                    }
                    return;
                }
            }
        }
    }

    private void LoadData()
    {
        List<Dictionary<string, object>> data;
        data = CSVReader.Read("MonsterData");

        if (data != null)
        {
            for (int i = 0; i < data.Count; i++)
            {
                if(data[i]["Name"].ToString() == Name)
                {
                    Speed = (int)data[i]["MoveSpeed"];
                    Damage = (int)data[i]["Damage"];
                    InvTime = (int)data[i]["InvTime"];
                    Health = (int)data[i]["Health"];
                    AttackDamage = (int)data[i]["AttackDamage"];
                    AttackInvTime = (int)data[i]["AttackInv"];
                    DieTime = (int)data[i]["DieTime"]; // 실수형
                    if (GetComponent<CharacterHorizontalMovement>())
                    {                      
                        GetComponent<CharacterHorizontalMovement>().WalkSpeed = (float)Speed;
                    }
                    if(GetComponent<DamageOnTouch>())
                    {
                        DamageOnTouch bodyDamage = GetComponent<DamageOnTouch>();
                        bodyDamage.DamageCaused = Damage;
                        bodyDamage.InvincibilityDuration = (float)InvTime;
                    }
                    if(GetComponent<Health>())
                    {
                        Health health = GetComponent<Health>();
                        health.InitialHealth = Health;
                        health.MaximumHealth = Health;
                        health.DelayBeforeDestruction = (float)(DieTime*0.01f);
                    }
                    if(AttackPos && AttackPos.GetComponent<DamageOnTouch>())
                    {
                        DamageOnTouch damage = AttackPos.GetComponent<DamageOnTouch>();
                        damage.DamageCaused = AttackDamage;
                        damage.InvincibilityDuration = (float)AttackInvTime;
                    }
                    if(GetComponent<CharacterHandleWeapon>())
                    {
                        MeleeWeapon weapon = GetComponent<CharacterHandleWeapon>().InitialWeapon.GetComponent<MeleeWeapon>();
                        if (weapon)
                        {
                            weapon.DamageCaused = AttackDamage;
                            weapon.InvincibilityDuration = (float)AttackInvTime;
                        }
                        else
                        {
                            DamageOnTouch damage = AttackPos.GetComponent<DamageOnTouch>();
                            damage.DamageCaused = AttackDamage;
                            damage.InvincibilityDuration = (float)AttackInvTime;
                        }
                    }

                    return;
                }

            }
        }
    }

}

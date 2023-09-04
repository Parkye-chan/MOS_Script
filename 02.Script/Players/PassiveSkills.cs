using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MoreMountains.CorgiEngine;


public class ValCollect
{
    public double FirstVal;
    public double SecondVal;
    public double ThirdVal;


    public ValCollect()
    {
        this.FirstVal = 0;
        this.SecondVal = 0;
        this.ThirdVal = 0;
    }


    public ValCollect(double m_FirstVal, double m_SecondVal, double m_ThirdVal)
    {
        this.FirstVal = m_FirstVal;
        this.SecondVal = m_SecondVal;
        this.ThirdVal = m_ThirdVal;
    }
}

public class PassiveSkills : MonoBehaviour
{

    private PoolingManager poolingManager;
    private MaskSkills maskskill;
    private EffectSpawn effect;
    private CharacterHandleWeapon weapon;
    private List<Dictionary<string, object>> UIdata;
    private List<Dictionary<string, object>> Passivedata;
    private List<string> codes = new List<string>();
    private List<ValCollect> Vals = new List<ValCollect>();

    private void Start()
    {
        Init();   
    }

    private void Init()
    {
        maskskill = GetComponent<MaskSkills>();
        effect = GetComponent<EffectSpawn>();
        weapon = GetComponentInParent<CharacterHandleWeapon>();
        poolingManager = PoolingManager.instance;
        UIdata = CSVReader.Read("UIdata");
        Passivedata = CSVReader.Read("PassiveData");

        for (int i = 0; i < UIdata.Count; i++)
        {
            codes.Add(UIdata[i]["ItemCode"].ToString());
        }
        
        for (int i = 0; i < Passivedata.Count; i++)
        {
            ValCollect collect = new ValCollect(double.Parse(Passivedata[i]["FirstVal"].ToString()), double.Parse(Passivedata[i]["SecondVal"].ToString()), double.Parse(Passivedata[i]["ThirdVal"].ToString()));
            Vals.Add(collect);
        }
    }

    public void PassiveActive(string slotcod,bool isEqit)
    {
        
        switch (slotcod)
        {
            case string code when code == codes[4]:
                {                
                    //검술의달인
                    if (isEqit)
                    {
                        //weapon.CurrentWeapon.GetComponentInChildren<DamageOnTouch>().DamageCaused = weapon.CurrentWeapon.GetComponentInChildren<DamageOnTouch>().DamageCaused + 5;
                        //maskskill.Chargeweapon[0].GetComponent<MeleeWeapon>().DamageCaused = maskskill.Chargeweapon[0].GetComponent<MeleeWeapon>().DamageCaused + 5;
                        weapon.CurrentWeapon.GetComponentInChildren<DamageOnTouch>().DamageCaused = 20 + (int)(Vals[0].FirstVal / 100);
                        maskskill.Chargeweapon[0].GetComponent<MeleeWeapon>().DamageCaused = 30 + (int)(Vals[0].FirstVal / 100);
                    }
                    else
                    {
                        weapon.CurrentWeapon.GetComponentInChildren<DamageOnTouch>().DamageCaused = weapon.CurrentWeapon.GetComponentInChildren<DamageOnTouch>().DamageCaused - (int)(Vals[0].FirstVal / 100);
                        maskskill.Chargeweapon[0].GetComponent<MeleeWeapon>().DamageCaused = maskskill.Chargeweapon[0].GetComponent<MeleeWeapon>().DamageCaused - (int)(Vals[0].FirstVal / 100);
                    }
                }
                break;
            case string code when code == codes[5]:
                {
                    //투척의달인
                    if (isEqit)
                    {
                        //poolingManager.PlayerBullet.GetComponent<DamageOnTouch>().DamageCaused = poolingManager.PlayerBullet.GetComponent<DamageOnTouch>().DamageCaused + 5;
                        //poolingManager.PlayerFireBullet.GetComponent<DamageOnTouch>().DamageCaused = poolingManager.PlayerFireBullet.GetComponent<DamageOnTouch>().DamageCaused + 5;
                        poolingManager.PlayerBullet.GetComponent<DamageOnTouch>().DamageCaused = 10 + (int)(Vals[1].FirstVal / 100);
                        poolingManager.PlayerFireBullet.GetComponent<DamageOnTouch>().DamageCaused = 15 + (int)(Vals[1].FirstVal / 100);
                    }
                    else
                    {
                        poolingManager.PlayerBullet.GetComponent<DamageOnTouch>().DamageCaused = poolingManager.PlayerBullet.GetComponent<DamageOnTouch>().DamageCaused - (int)(Vals[1].FirstVal / 100);
                        poolingManager.PlayerFireBullet.GetComponent<DamageOnTouch>().DamageCaused = poolingManager.PlayerFireBullet.GetComponent<DamageOnTouch>().DamageCaused - (int)(Vals[1].FirstVal / 100);
                    }
                }
                break;
            case string code when code == codes[6]:
                {
                    //발도의 대가
                    if (isEqit)
                    {
                        maskskill.MeleeChargingTime = (float)(Vals[2].FirstVal*0.01d);
                    }
                    else
                    {
                        maskskill.MeleeChargingTime = 2.0f;
                    }
                }
                break;
            case string code when code == codes[7]:
                {
                    //속공의 대가
                    if (isEqit)
                    {
                        //poolingManager.PlayerBullet.GetComponent<Projectile>().Speed = 250f;
                        //poolingManager.PlayerFireBullet.GetComponent<Projectile>().Speed = 250f;
                        poolingManager.PlayerBullet.GetComponent<Projectile>().Speed = (float)(Vals[3].FirstVal * 0.01d);
                        poolingManager.PlayerFireBullet.GetComponent<Projectile>().Speed = (float)(Vals[3].FirstVal * 0.01d);
                    }
                    else
                    {
                        poolingManager.PlayerBullet.GetComponent<Projectile>().Speed = 200;
                        poolingManager.PlayerFireBullet.GetComponent<Projectile>().Speed = 200;
                    }
                }
                break;
            case string code when code == codes[8]:
                {
                    //땅의주인
                    if (isEqit)
                    {
                        effect.EarthShield.GetComponent<ShiledPos>().DestroyTime = 10.0f + (float)(Vals[4].SecondVal*0.01d);
                        Health[] healths = effect.EarthShield.GetComponentsInChildren<Health>();
                        healths[0].InitialHealth = (int)(Vals[4].FirstVal / 100);
                        healths[0].InitialHealth = (int)(Vals[4].FirstVal / 100);
                        healths[1].InitialHealth = (int)(Vals[4].FirstVal / 100);
                        healths[1].InitialHealth = (int)(Vals[4].FirstVal / 100);
                        poolingManager.PlayerEarthBullet.GetComponent<Projectile>().LifeTime = 2.0f + (float)(Vals[4].SecondVal * 0.01d);
                    }
                    else
                    {
                        effect.EarthShield.GetComponent<ShiledPos>().DestroyTime = effect.EarthShield.GetComponent<ShiledPos>().DestroyTime - (float)(Vals[4].SecondVal * 0.01d);
                        Health[] healths = effect.EarthShield.GetComponentsInChildren<Health>();
                        healths[0].InitialHealth = healths[0].InitialHealth - (int)(Vals[4].FirstVal / 100);
                        healths[0].InitialHealth = healths[0].MaximumHealth - (int)(Vals[4].FirstVal / 100);
                        healths[1].InitialHealth = healths[1].InitialHealth - (int)(Vals[4].FirstVal / 100);
                        healths[1].InitialHealth = healths[1].MaximumHealth - (int)(Vals[4].FirstVal / 100);
                        poolingManager.PlayerEarthBullet.GetComponent<Projectile>().LifeTime = poolingManager.PlayerEarthBullet.GetComponent<Projectile>().LifeTime - (float)(Vals[4].SecondVal * 0.01d);
                    }
                }
                break;
            case string code when code == codes[9]:
                {
                    //독의 주인
                    if (isEqit)
                    {
                        effect.PosionMist.GetComponent<DelayDamage>().Times = (float)(Vals[5].SecondVal * 0.01d);
                        effect.PosionMist.GetComponent<CircleCollider2D>().radius = (float)(Vals[5].ThirdVal * 0.01d);
                    }
                    else
                    {
                        effect.PosionMist.GetComponent<DelayDamage>().Times = 3.0f;
                        effect.PosionMist.GetComponent<CircleCollider2D>().radius = 1.7f;
                    }
                }
                break;
            case string code when code == codes[10]:
                {
                    //불의주인
                    if (isEqit)
                    {
                        effect.FireFilar.GetComponent<DamageOnTouch>().DamageCaused = 10 + (int)(Vals[6].SecondVal / 100);
                        effect.FireDash.GetComponent<DamageOnTouch>().DamageCaused = 40+(int)(Vals[6].SecondVal / 100);
                        effect.FireOrora.GetComponent<CircleCollider2D>().radius = (float)(Vals[6].ThirdVal * 0.01d);
                        poolingManager.PlayerFireChargingBullet.GetComponent<DamageOnTouch>().DamageCaused = (int)(Vals[6].SecondVal / 100);
                        //effect.FireFilar.GetComponent<DamageOnTouch>().DamageCaused = effect.FireFilar.GetComponent<DamageOnTouch>().DamageCaused + 10;
                        //effect.FireDash.GetComponent<DamageOnTouch>().DamageCaused = effect.FireDash.GetComponent<DamageOnTouch>().DamageCaused + 10;
                        //effect.FireOrora.GetComponent<CircleCollider2D>().radius = 2.85f;
                        //poolingManager.PlayerFireChargingBullet.GetComponent<DamageOnTouch>().DamageCaused = poolingManager.PlayerFireChargingBullet.GetComponent<DamageOnTouch>().DamageCaused + 10;
                    }
                    else
                    {
                        effect.FireFilar.GetComponent<DamageOnTouch>().DamageCaused = effect.FireFilar.GetComponent<DamageOnTouch>().DamageCaused - (int)(Vals[6].SecondVal / 100);
                        effect.FireDash.GetComponent<DamageOnTouch>().DamageCaused = effect.FireDash.GetComponent<DamageOnTouch>().DamageCaused - (int)(Vals[6].SecondVal / 100);
                        effect.FireOrora.GetComponent<CircleCollider2D>().radius = 2.56f;
                        poolingManager.PlayerFireChargingBullet.GetComponent<DamageOnTouch>().DamageCaused = poolingManager.PlayerFireChargingBullet.GetComponent<DamageOnTouch>().DamageCaused - (int)(Vals[6].SecondVal / 100);
                    }
                }
                break;
            case string code when code == codes[11]:
                {
                    //얼음의주인
                    if (isEqit)
                    {
                        poolingManager.PlayerIceBullet.GetComponent<Projectile>().SlowTime = (float)(Vals[7].FirstVal * 0.01d);
                        poolingManager.PlayerIceChargingBullet.GetComponent<SpreadTrap>().FireTime = (float)(Vals[7].SecondVal * 0.01d);

                    }
                    else
                    {
                        poolingManager.PlayerIceBullet.GetComponent<Projectile>().SlowTime = poolingManager.PlayerIceBullet.GetComponent<Projectile>().SlowTime - (float)(Vals[7].FirstVal * 0.01d);
                        poolingManager.PlayerIceChargingBullet.GetComponent<SpreadTrap>().FireTime = 2.0f;
                    }
                }
                break;
            case string code when code == codes[12]:
                {
                    //역마살
                    if (isEqit)
                    {
                        maskskill.ParentsModel.GetComponent<CharacterHorizontalMovement>().WalkSpeed = (float)(Vals[8].FirstVal * 0.01d);
                        maskskill.ParentsModel.GetComponent<CharacterHorizontalMovement>().MovementSpeed = (float)(Vals[8].FirstVal * 0.01d);
                        maskskill.ParentsModel.GetComponent<CharacterDash>().DashCooldown = (float)(Vals[8].SecondVal * 0.01d);
                        //maskskill.ParentsModel.GetComponent<CharacterHorizontalMovement>().WalkSpeed = 10.0f;
                        //maskskill.ParentsModel.GetComponent<CharacterHorizontalMovement>().MovementSpeed = 10.0f;
                        //maskskill.ParentsModel.GetComponent<CharacterDash>().DashCooldown = 0.5f;
                    }
                    else
                    {
                        maskskill.ParentsModel.GetComponent<CharacterHorizontalMovement>().WalkSpeed = 8.0f;
                        maskskill.ParentsModel.GetComponent<CharacterHorizontalMovement>().MovementSpeed = 8.0f;
                        maskskill.ParentsModel.GetComponent<CharacterDash>().DashCooldown = 0.8f;
                    }
                }
                break;
            case string code when code == codes[13]:
                {
                    //생명력흡수
                    if (isEqit)
                    {
                        LevelManager.Instance.DrainOn = true;
                    }
                    else
                    {
                        LevelManager.Instance.DrainOn = false;
                    }
                }
                break;
            case string code when code == codes[14]:
                {
                    //곱빼기
                    if (isEqit)
                    {
                        maskskill.PotionGetHealth = 1 + (int)(Vals[10].FirstVal / 100);
                    }
                    else
                    {
                        maskskill.PotionGetHealth = maskskill.PotionGetHealth - (int)(Vals[10].FirstVal / 100);
                    }
                }
                break;
            case string code when code == codes[15]:
                {
                    //놀부심성
                    if (isEqit)
                    {
                        LevelManager.Instance.NolbooOn = true;
                    }
                    else
                    {
                        LevelManager.Instance.NolbooOn = false;
                    }
                }
                break;
            case string code when code == codes[16]:
                {
                    //똥밟았네
                    if (isEqit)
                    {
                        maskskill.ParentsModel.GetComponent<CharacterHorizontalMovement>().WalkSpeed = 8.0f - (float)(Vals[12].FirstVal * 0.01d);
                        maskskill.ParentsModel.GetComponent<CharacterHorizontalMovement>().MovementSpeed = 8.0f - (float)(Vals[12].FirstVal * 0.01d);
                        //maskskill.ParentsModel.GetComponent<CharacterHorizontalMovement>().WalkSpeed = maskskill.ParentsModel.GetComponent<CharacterHorizontalMovement>().WalkSpeed - 2.0f;
                        //maskskill.ParentsModel.GetComponent<CharacterHorizontalMovement>().MovementSpeed = maskskill.ParentsModel.GetComponent<CharacterHorizontalMovement>().MovementSpeed - 2.0f;
                    }
                    else
                    {
                        maskskill.ParentsModel.GetComponent<CharacterHorizontalMovement>().WalkSpeed = maskskill.ParentsModel.GetComponent<CharacterHorizontalMovement>().WalkSpeed + (float)(Vals[12].FirstVal * 0.01d);
                        maskskill.ParentsModel.GetComponent<CharacterHorizontalMovement>().MovementSpeed = maskskill.ParentsModel.GetComponent<CharacterHorizontalMovement>().MovementSpeed + (float)(Vals[12].FirstVal * 0.01d);
                    }
                }
                break;
        }
    }

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MoreMountains.CorgiEngine;
using MoreMountains.Tools;
using MoreMountains.Feedbacks;

public class MaskSkills : MonoBehaviour
{
  
    public enum PlayerState
    {
        Normal,
        Pause,
        Stun,
        Sleep,
        DEAD
    }

    public enum ElementState
    {
        None,
        Fire,
        Frost,
        Earth,
        Poison
    }

    [SerializeField]
    public Transform Rightpos;
    [SerializeField]
    public Transform Leftpos;
    [SerializeField]
    GameObject SkillPosBackGround;
    [SerializeField]
    Transform SkillPos;
    [SerializeField]
    float startDashTime;
    [SerializeField]
    float maxDegree = 90;
    [SerializeField]
    GameObject SkillBullet;
    [SerializeField]
    float ShootPower;
    [SerializeField]
    GameObject HealEffect;
    private int direction;
    private Rigidbody2D rb;
    private float dashTime;
    private float ChargingTime;
    private float DegreeCheck = 0f;
    private Animator anim;
    private Transform bulletPos;
    private bool isHeal = false;
    private bool SuperModeCheck = false;

    public GameObject MapMarker;
    public bool SlowOn = false;
   // public bool jumping = false;
    //public bool DashOn = false;
    public bool FocusOn = false;
    public bool DoubleDashOn = false;
    public float FireCurTime = 0f;
    public float DoubleDashCoolTime = 2.0f;
    public float MeleeChargingTime = 2.0f;
    public int PotionGetHealth = 1;
    public Transform RespawnPos;
    [SerializeField]
    float FocusTime;
    private float FocusChargingTime = 0f;
    [SerializeField]
    public float FireDelay;
    public GameObject ParentsModel;
    public GameObject tempStonShowerBullet;
    public Transform[] Showerpos;
    public Weapon[] Chargeweapon;
    public Weapon InitWeapon;
    public MMFeedbacks ChargeFeedback;
    public MMFeedbacks[] FireFeedback;
    public MMFeedbacks[] ChargingFireFeedback;
    public MMFeedbacks HealFeedback;
    public MMFeedbacks ElemetalChangeFeedback;
    public bool MeetNPC;
    private GameObject curBullet;
    private Health health;
    private Vector3 vDir;
    private EffectSpawn effect;
    private Character character;
    private MMStateMachine<CharacterStates.CharacterConditions> _condition;
    private MMStateMachine<CharacterStates.MovementStates> _movement;
    private CorgiController corgiController;
    private CharacterHandleWeapon handleWeapon;
    private CharacterDash characterDash;
    private CharacterJump characterJump;
    private CharacterWallClinging clinging;

    //CharacterHandleWeapon weapon;
    private ComboWeapon comboweapon;
    private Weapon chargeAttackScript;
    private CharacterJetpack jetpack;
    private int StateNum = 0;
    private int SleepCount = 0;
    private PlayerState playerState;
    public ElementState elementSate;
    private float curDegree = 0;
    private bool isCharge = false;
    private void Start()
    {
        Init();
    }

    private void Update()
    {

        if (character.ConditionState.CurrentState == CharacterStates.CharacterConditions.Paused && GUIManager.Instance.PauseScreen.activeSelf)
            return;

        switch (playerState)
        {
            case PlayerState.Normal:
                {
                    //StonShower();
                    HpCheck();
                    DashEffectSpawn();
                    if ((Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.Joystick1Button5)) && MeetNPC)
                    {
                        DialogueManager.instance.Talk();
                        Stateabnormality(PlayerState.Pause);
                    }

                    if (!SlowOn)
                        FireDirection();

                    if (Input.GetKey(KeyCode.Z) || Input.GetKey(KeyCode.Joystick1Button1))
                    {
                        
                        ChargingTime += Time.deltaTime;
                        if (ChargingTime > MeleeChargingTime)
                        {
                            effect.ChargeEffectComplete();
                        }
                        else if (ChargingTime > MeleeChargingTime*0.5f)
                        {
                            if(!isCharge)
                                ChargeFeedback.PlayFeedbacks();
                            
                                
                            isCharge = true;
                            effect.ChargeEffectSpawn();
                            
                        }
                    }
                    else
                    {
                        if (ChargingTime > MeleeChargingTime)
                        {
                            Charging();
                        }
                        effect.ChargeEffectOff();
                        ChargingTime = 0;
                        isCharge = false;
                    }


                    if ((Input.GetKey(KeyCode.UpArrow) || (Input.GetAxis("Player1_Vertical") == 1)) && SlowOn)
                    {
                        if (DegreeCheck < 1)
                        {
                            DegreeCheck += Time.unscaledDeltaTime * 1f;
                            SetSkillTransform(DegreeCheck);

                        }
                    }
                    else if ((Input.GetKey(KeyCode.DownArrow) || (Input.GetAxis("Player1_Vertical") == -1)) && SlowOn)
                    {
                        if (DegreeCheck > -1)
                        {
                            DegreeCheck -= Time.unscaledDeltaTime * 1f;
                            SetSkillTransform(DegreeCheck);
                        }
                    }

                    else if ((Input.GetKeyDown(KeyCode.X) || Input.GetKeyDown(KeyCode.Joystick1Button0)) && SlowOn)
                    {
                        FocusFire();
                    }

                    if (FireCurTime > 0)
                    {
                        FireCurTime -= Time.deltaTime;
                        if (FocusChargingTime > 0)
                            FocusChargingTime = 0;
                    }

                    if (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.Joystick1Button4)) 
                    {
                        ChangeElement();
                    }

                    if (character.MovementState.CurrentState == CharacterStates.MovementStates.Dashing && (Input.GetKeyDown(KeyCode.C) || Input.GetKeyDown(KeyCode.Joystick1Button3)) && dashTime <= 0)
                    {
                        if (!DoubleDashOn)
                            return;

                        if (jetpack.JetpackFuelDurationLeft >= 2)
                        {
                            float tempDistance;
                            float tempForce;
                            tempDistance = characterDash.DashDistance;
                            tempForce = characterDash.DashForce;
                            characterDash.ResetAbility();
                            characterDash.DashDistance = tempDistance * 2f;
                            characterDash.DashForce = tempForce * 2f;
                            characterDash.InitiateDash();
                            dashTime = DoubleDashCoolTime;
                            characterDash.DashDistance = tempDistance;
                            characterDash.DashForce = tempForce;
                            jetpack.JetpackFuelDurationLeft -= 2.0f;
                            jetpack._jetpackStoppedAt = Time.time;
                            jetpack.UpdateJetpackBar();
                            health.Invulnerable = true;
                            effect.DashAttackZoneOn();
                            effect.DashLightEffectSpawn();
                            //DashOn = true;
                        }
                    }
                    else if (dashTime > 0)
                        dashTime -= Time.deltaTime;
                }
                break;
            case PlayerState.Pause:
                {
                    if ((Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.Joystick1Button5)) && MeetNPC)
                    {
                        DialogueManager.instance.Talk();
                    }
                } 
                break;
            case PlayerState.Stun:
                break;
            case PlayerState.Sleep:
                {
                    //character.ConditionState.ChangeState(CharacterStates.CharacterConditions.ControlledMovement);


                    if (Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.RightArrow)) //|| Input.GetAxis("Player1_Horizontal") == 1 || Input.GetAxis("Player1_Horizontal") == -1)
                        SleepCount++;                   
                    else if (SleepCount > 10)
                    {
                        character.ConditionState.ChangeState(CharacterStates.CharacterConditions.Normal);
                        character.MovementState.ChangeState(CharacterStates.MovementStates.Idle);
                        corgiController._fallSlowFactor = 0;
                        SleepCount = 0;
                        playerState = PlayerState.Normal;
                        corgiController.State.Reset();
                        effect.StateEffectDisable();
                    }
                   

                }
                break;
            case PlayerState.DEAD:
                {
                    StopAllCoroutines();
                    _condition.ChangeState(CharacterStates.CharacterConditions.Dead);
                    anim.SetTrigger("Alive");
                }
                break;
        }
    }

    private void FixedUpdate()
    {
        // if(DashOn)
        //      Dash();

        switch (playerState)
        {
            case PlayerState.Normal:
                {
                    if (corgiController.State.IsGrounded)
                    {

                        if (LevelManager.Instance.PotionNum > 0)
                            Heal();

                        if (jetpack.JetpackFuelDurationLeft >= 1)
                            FireLogic();
                                            
                    }
                }
                break;
            case PlayerState.Pause:
                break;
            case PlayerState.Stun:
                break;
            case PlayerState.Sleep:
                break;
            case PlayerState.DEAD:
                break;
        }
    }


    private void FireDirection()
    {
        if (Input.GetKey(KeyCode.LeftArrow) || Input.GetAxis("Player1_Horizontal") == -1)
        {
            vDir = Vector2.left;
            SkillPosBackGround.transform.position = Leftpos.position;
            SkillPosBackGround.transform.rotation = Leftpos.rotation;
            bulletPos = Leftpos;
        }
        else if (Input.GetKey(KeyCode.RightArrow) | Input.GetAxis("Player1_Horizontal") == 1)
        {
            vDir = Vector2.right;
            SkillPosBackGround.transform.position = Rightpos.position;
            SkillPosBackGround.transform.rotation = Rightpos.rotation;
            bulletPos = Rightpos;
        }
    }

    private void StonShower()
    {
        if(Input.GetKeyDown(KeyCode.E))
        {
            for (int i = 0; i < Showerpos.Length; i++)
            {
                Instantiate(tempStonShowerBullet, Showerpos[i].position, Showerpos[i].rotation);
            }
        }
    }

    public void Charging()
    {
        
        Debug.Log("Charging");
        StartCoroutine(ChargeAttackFunc());
        /*
        Collider2D[] collider2D = Physics2D.OverlapBoxAll(Rightpos.position, boxsize, 0);
        bool temp = true;
        foreach (Collider2D colider in collider2D)
        {
            
            if (colider.CompareTag("Enemy") && temp)
            {
                colider.GetComponent<Monster>().TakeDamage(status.m_nStr);
            }
            else if (colider.GetComponent<ItemBox>())
            {
                colider.GetComponent<ItemBox>().hp--;
            }
            if (colider.gameObject.CompareTag("Boss"))
            {
                Debug.Log(colider.gameObject.name);
                if (colider.gameObject.GetComponent<Boss>())
                    colider.gameObject.GetComponent<Boss>().TakeDamage(status.m_nStr);
                else if (colider.gameObject.GetComponent<FrogBoss>())
                    colider.gameObject.GetComponent<FrogBoss>().TakeDamage(status.m_nStr);
                else if (colider.gameObject.GetComponent<KnightBoss>())
                    colider.gameObject.GetComponent<KnightBoss>().TakeDamage(status.m_nStr);

            }
        }*/
    }

    IEnumerator ChargeAttackFunc()
    {

        switch (elementSate)
        {
            case ElementState.None:
                {
                    var tempWeapon = handleWeapon.CurrentWeapon;

                    handleWeapon.ChangeWeapon(Chargeweapon[0], null, false);

                    yield return new WaitForSeconds(0.1f);

                    handleWeapon.CurrentWeapon.WeaponInputStart();

                    yield return new WaitForSeconds(0.5f);

                    handleWeapon.CurrentWeapon.TurnWeaponOff();
                    handleWeapon.ChangeWeapon(InitWeapon, null, false);
                    effect.ChargeOn = false;
                }
                break;
            case ElementState.Fire:
                {
                    var tempWeapon = handleWeapon.CurrentWeapon;

                    handleWeapon.ChangeWeapon(Chargeweapon[1], null, false);

                    yield return new WaitForSeconds(0.1f);

                    handleWeapon.CurrentWeapon.WeaponInputStart();

                    yield return new WaitForSeconds(0.5f);

                    handleWeapon.CurrentWeapon.TurnWeaponOff();
                    handleWeapon.ChangeWeapon(InitWeapon, null, false);
                    effect.ChargeOn = false;
                    jetpack.JetpackFuelDurationLeft -= 2.0f;
                    jetpack._jetpackStoppedAt = Time.time;
                    jetpack.UpdateJetpackBar();
                }
                break;
            case ElementState.Frost:
                {
                    var tempWeapon = handleWeapon.CurrentWeapon;

                    handleWeapon.ChangeWeapon(Chargeweapon[2], null, false);

                    yield return new WaitForSeconds(0.1f);

                    handleWeapon.CurrentWeapon.WeaponInputStart();

                    yield return new WaitForSeconds(0.5f);

                    handleWeapon.CurrentWeapon.TurnWeaponOff();
                    handleWeapon.ChangeWeapon(InitWeapon, null, false);
                    effect.ChargeOn = false;
                }
                break;
            case ElementState.Earth:
                {
                    var tempWeapon = handleWeapon.CurrentWeapon;

                    handleWeapon.ChangeWeapon(Chargeweapon[3], null, false);

                    yield return new WaitForSeconds(0.1f);

                    handleWeapon.CurrentWeapon.WeaponInputStart();

                    yield return new WaitForSeconds(0.5f);

                    handleWeapon.CurrentWeapon.TurnWeaponOff();
                    handleWeapon.ChangeWeapon(InitWeapon, null, false);
                    effect.ChargeOn = false;
                    jetpack.JetpackFuelDurationLeft -= 1.0f;
                    jetpack._jetpackStoppedAt = Time.time;
                    jetpack.UpdateJetpackBar();
                }
                break;
            case ElementState.Poison:
                {
                    var tempWeapon = handleWeapon.CurrentWeapon;

                    handleWeapon.ChangeWeapon(Chargeweapon[4], null, false);

                    yield return new WaitForSeconds(0.1f);

                    handleWeapon.CurrentWeapon.WeaponInputStart();

                    yield return new WaitForSeconds(0.5f);

                    handleWeapon.CurrentWeapon.TurnWeaponOff();
                    handleWeapon.ChangeWeapon(InitWeapon, null, false);
                    effect.ChargeOn = false;
                    jetpack.JetpackFuelDurationLeft -= 1.0f;
                    jetpack._jetpackStoppedAt = Time.time;
                    jetpack.UpdateJetpackBar();
                }
                break;
        }           
    }
    
    public void Dash()
    {


        if (dashTime <= 0)
        {
            dashTime = startDashTime;
            rb.velocity = Vector2.zero;
            //DashOn = false;
            
        }
        else
        {
            dashTime -= Time.deltaTime;
            //rb.velocity = vDir * Speed;
            
            //rb.velocity = vDir * Speed;
            //playerCtrl.skillUsing = true;
            //if (temp)
            //     temp.transform.position = transform.position;
            //temp.transform.position = new Vector2(transform.position.x - 2, transform.position.y);



        }

    }

    private void DashEffectSpawn()
    {
        if (Input.GetKeyDown(KeyCode.C) && characterDash.SuccessiveDashesLeft > 0)
        {
            switch (elementSate)
            {
                case ElementState.None:
                    {
                        effect.DashObjSpawn(vDir);
                    }
                    break;
                case ElementState.Fire:
                    {
                        if (jetpack.JetpackFuelDurationLeft < 1)
                            return;

                        characterDash.enabled = true;
                        effect.DashObjSpawn(vDir);
                        jetpack.JetpackFuelDurationLeft -= 1.0f;
                        jetpack._jetpackStoppedAt = Time.time;
                        jetpack.UpdateJetpackBar();
                    }
                    break;
                case ElementState.Frost:
                    {

                        if (jetpack.JetpackFuelDurationLeft < 1)
                            return;

                        characterDash.enabled = true;
                        effect.DashObjSpawn(vDir);
                        jetpack.JetpackFuelDurationLeft -= 1.0f;
                        jetpack._jetpackStoppedAt = Time.time;
                        jetpack.UpdateJetpackBar();
                    }
                    break;
                case ElementState.Earth:
                    {
                        return;
                    }
                case ElementState.Poison:
                    {
                        if (jetpack.JetpackFuelDurationLeft < 1)
                            return;

                        characterDash.enabled = true;

                        effect.DashObjSpawn(vDir);
                    }
                    break;
            }
        }
        else
            return;
    }

    public void DashStop()
    {     
        //characterDash.enabled = false;
        health.Invulnerable = SuperModeCheck;
    }

    public void ChangeElement()
    {
        ElemetalChangeFeedback.PlayFeedbacks();

        StateNum++;
        if (StateNum > 4)
            StateNum = 0;

        elementSate = (ElementState)StateNum;
        PlayerInfoManager.instance.ElementalEqit(StateNum);
        effect.StateChange(StateNum);

    }

    public void Focus(float val, float FocusTime)
    {
        //보는 방향에서 180도
        switch (elementSate)
        {
            case ElementState.None:
                {
                    if (val >= FocusTime)
                    {
                        _condition.ChangeState(CharacterStates.CharacterConditions.Frozen);
                        _movement.ChangeState(CharacterStates.MovementStates.Idle);
                        corgiController.GravityActive(true);
                        SkillPosBackGround.SetActive(true);
                        SlowOn = true;
                        //Time.timeScale = 0.1f;
                        // Time.fixedDeltaTime = 0.02f * Time.timeScale;
                    }
                }
                break;
            case ElementState.Fire:
                {
                    if (val >= FocusTime)
                    {
                        _condition.ChangeState(CharacterStates.CharacterConditions.Frozen);
                        _movement.ChangeState(CharacterStates.MovementStates.Idle);
                        corgiController.GravityActive(true);
                        SkillPosBackGround.SetActive(true);
                        SlowOn = true;
                        //Time.timeScale = 0.1f;
                        // Time.fixedDeltaTime = 0.02f * Time.timeScale;
                    }
                }
                break;
            case ElementState.Frost:
                {
                    if (val >= FocusTime)
                    {
                        _condition.ChangeState(CharacterStates.CharacterConditions.Frozen);
                        _movement.ChangeState(CharacterStates.MovementStates.Idle);
                        corgiController.GravityActive(true);
                        SkillPosBackGround.SetActive(true);
                        SlowOn = true;
                        //Time.timeScale = 0.1f;
                        // Time.fixedDeltaTime = 0.02f * Time.timeScale;
                    }
                }
                break;
            case ElementState.Earth:
                {
                    if (val >= FocusTime)
                    {
                        _condition.ChangeState(CharacterStates.CharacterConditions.Frozen);
                        _movement.ChangeState(CharacterStates.MovementStates.Idle);
                        corgiController.GravityActive(true);
                        SkillPosBackGround.SetActive(true);
                        SlowOn = true;
                        //Time.timeScale = 0.1f;
                        // Time.fixedDeltaTime = 0.02f * Time.timeScale;
                    }
                }
                break;
            case ElementState.Poison:
                {
                    if (val >= FocusTime)
                    {
                        _condition.ChangeState(CharacterStates.CharacterConditions.Frozen);
                        _movement.ChangeState(CharacterStates.MovementStates.Idle);
                        corgiController.GravityActive(true);
                        SkillPosBackGround.SetActive(true);
                        SlowOn = true;
                        //Time.timeScale = 0.1f;
                        // Time.fixedDeltaTime = 0.02f * Time.timeScale;
                    }
                }
                break;
        }
        

    }

    public void FocusFire()
    {
        if (SlowOn)
        {

            switch (elementSate)
            {
                case ElementState.None:
                    {
                        //Time.timeScale = 1;
                        //Time.fixedDeltaTime = 0.02f * Time.timeScale;
                        _condition.ChangeState(CharacterStates.CharacterConditions.Normal);
                        corgiController.GravityActive(true);
                        //GameObject tempcurBullet = PoolingManager.instance.GetPlayerFireBullet();
                        GameObject tempcurBullet = Instantiate(PoolingManager.instance.PlayerFireBullet);
                        if (tempcurBullet != null)
                        {
                            tempcurBullet.transform.position = SkillPos.position;
                            tempcurBullet.transform.rotation = SkillPos.rotation;
                            tempcurBullet.GetComponent<Projectile>().Direction = Vector3.zero;
                            tempcurBullet.GetComponent<Projectile>().Direction = SkillPos.right;
                            tempcurBullet.SetActive(true);
                        }
                        //tempbullet.GetComponent<Rigidbody2D>().velocity = SkillPos.right * ShootPower;
                        ChargingFireFeedback[(int)elementSate].PlayFeedbacks();
                        SkillPos.localRotation = Quaternion.identity;
                        SkillPosBackGround.SetActive(false);
                        SlowOn = false;
                        FocusChargingTime = 0;
                        FireCurTime = FireDelay;
                        DegreeCheck = 0;
                        jetpack.JetpackFuelDurationLeft -= 1.0f;
                        jetpack._jetpackStoppedAt = Time.time;
                        jetpack.UpdateJetpackBar();
                        Destroy(tempcurBullet, tempcurBullet.GetComponent<Projectile>().LifeTime);
                    }
                    break;
                case ElementState.Fire:
                    {
                        _condition.ChangeState(CharacterStates.CharacterConditions.Normal);
                        corgiController.GravityActive(true);

                        //GameObject tempcurBullet = PoolingManager.instance.GetPlayerFireChargingBullet();
                        GameObject tempcurBullet = Instantiate(PoolingManager.instance.PlayerFireChargingBullet);
                        if (tempcurBullet != null)
                        {
                            tempcurBullet.transform.position = SkillPos.position;
                            tempcurBullet.transform.rotation = SkillPos.rotation;
                            tempcurBullet.SetActive(true);
                            tempcurBullet.GetComponent<Rigidbody2D>().velocity = tempcurBullet.transform.right * 20f;
                            //tempcurBullet.GetComponentInChildren<Projectile>().Direction = SkillPos.right;
                            //tempcurBullet.transform.Rotate(SkillPos.right);
                            //tempcurBullet.GetComponent<Rigidbody2D>().AddForce(SkillPos.right * ShootPower, ForceMode2D.Impulse);

                        }
                        ChargingFireFeedback[(int)elementSate].PlayFeedbacks();
                        SkillPos.localRotation = Quaternion.identity;
                        SkillPosBackGround.SetActive(false);
                        SlowOn = false;
                        FocusChargingTime = 0;
                        FireCurTime = FireDelay;
                        DegreeCheck = 0;
                        jetpack.JetpackFuelDurationLeft -= 1.0f;
                        jetpack._jetpackStoppedAt = Time.time;
                        jetpack.UpdateJetpackBar();
                        Destroy(tempcurBullet, tempcurBullet.GetComponentInChildren<Projectile>().LifeTime);
                    }
                    break;
                case ElementState.Frost:
                    {
                        _condition.ChangeState(CharacterStates.CharacterConditions.Normal);
                        corgiController.GravityActive(true);

                         //GameObject tempcurBullet = PoolingManager.instance.GetPlayerIceChargingBullet();
                        GameObject tempcurBullet = Instantiate(PoolingManager.instance.PlayerIceChargingBullet);
                        if (tempcurBullet != null)
                        {
                            Debug.Log(tempcurBullet.transform.position);
                            tempcurBullet.transform.position = SkillPos.position;
                            tempcurBullet.GetComponent<Projectile>().Direction = Vector3.zero;
                            tempcurBullet.GetComponent<Projectile>().Direction = SkillPos.right;
                            tempcurBullet.SetActive(true);
                            //tempcurBullet.GetComponent<Rigidbody2D>().AddForce(SkillPos.right * ShootPower, ForceMode2D.Impulse);
                        }
                        ChargingFireFeedback[(int)elementSate].PlayFeedbacks();
                        SkillPos.localRotation = Quaternion.identity;
                        SkillPosBackGround.SetActive(false);
                        SlowOn = false;
                        FocusChargingTime = 0;
                        FireCurTime = FireDelay;
                        DegreeCheck = 0;
                        jetpack.JetpackFuelDurationLeft -= 1.0f;
                        jetpack._jetpackStoppedAt = Time.time;
                        jetpack.UpdateJetpackBar();
                        Destroy(tempcurBullet, tempcurBullet.GetComponent<Projectile>().LifeTime);
                    }
                    break;
                case ElementState.Earth:
                    {
                        _condition.ChangeState(CharacterStates.CharacterConditions.Normal);
                        corgiController.GravityActive(true);
                        for (int i = 0; i < 5; i++)
                        {
                            //GameObject tempcurBullet = PoolingManager.instance.GetPlayerEarthChargingBullet();
                            GameObject tempcurBullet = Instantiate(PoolingManager.instance.PlayerEarthChargingBullet);
                            if (tempcurBullet != null)
                            {
                                tempcurBullet.transform.position = SkillPos.position;
                                tempcurBullet.transform.rotation = SkillPos.rotation;
                                float RandomPower = Random.Range(ShootPower*0.5f,ShootPower);
                                tempcurBullet.SetActive(true);
                                tempcurBullet.GetComponent<Rigidbody2D>().AddForce(SkillPos.right * RandomPower, ForceMode2D.Impulse);
                                Destroy(tempcurBullet, tempcurBullet.GetComponent<Projectile>().LifeTime);
                            }
                            else
                            {
                                PoolingManager.instance.addPlayerEarthChargingBullet();
                                tempcurBullet = PoolingManager.instance.GetPlayerEarthChargingBullet();
                                tempcurBullet.transform.position = SkillPos.position;
                                tempcurBullet.transform.rotation = SkillPos.rotation;
                                float RandomPower = Random.Range(ShootPower * 0.5f, ShootPower);
                                tempcurBullet.SetActive(true);
                                tempcurBullet.GetComponent<Rigidbody2D>().AddForce(SkillPos.right * RandomPower, ForceMode2D.Impulse);
                                continue;
                            }
                        }
                        ChargingFireFeedback[(int)elementSate].PlayFeedbacks();
                        SkillPos.localRotation = Quaternion.identity;
                        SkillPosBackGround.SetActive(false);
                        SlowOn = false;
                        FocusChargingTime = 0;
                        FireCurTime = FireDelay;
                        DegreeCheck = 0;
                        jetpack.JetpackFuelDurationLeft -= 1.0f;
                        jetpack._jetpackStoppedAt = Time.time;
                        jetpack.UpdateJetpackBar();
                        
                    }
                    break;
                case ElementState.Poison:
                    {
                        _condition.ChangeState(CharacterStates.CharacterConditions.Normal);
                        corgiController.GravityActive(true);
                        //GameObject tempcurBullet = PoolingManager.instance.GetPlayerFireBullet();
                        GameObject tempcurBullet = Instantiate(PoolingManager.instance.PlayerFireBullet);
                        if (tempcurBullet != null)
                        {
                            tempcurBullet.transform.position = SkillPos.position;
                            tempcurBullet.transform.rotation = SkillPos.rotation;
                            tempcurBullet.GetComponent<Projectile>().Direction = Vector3.zero;
                            tempcurBullet.GetComponent<Projectile>().Direction = SkillPos.right;
                            tempcurBullet.SetActive(true);
                        }
                        //tempbullet.GetComponent<Rigidbody2D>().velocity = SkillPos.right * ShootPower;
                        ChargingFireFeedback[(int)elementSate].PlayFeedbacks();
                        SkillPos.localRotation = Quaternion.identity;
                        SkillPosBackGround.SetActive(false);
                        SlowOn = false;
                        FocusChargingTime = 0;
                        FireCurTime = FireDelay;
                        DegreeCheck = 0;
                        jetpack.JetpackFuelDurationLeft -= 1.0f;
                        jetpack._jetpackStoppedAt = Time.time;
                        jetpack.UpdateJetpackBar();
                        Destroy(tempcurBullet, tempcurBullet.GetComponent<Projectile>().LifeTime);
                    }
                    break;
            }
            
        }
    }

    private void FireLogic()
    {
        if (FocusOn && FireCurTime <= 0)
        {
            if (Input.GetKey(KeyCode.X) || Input.GetKey(KeyCode.Joystick1Button0))
            {
                
                anim.SetTrigger("Fire");
                FocusChargingTime += Time.deltaTime;
                if (FocusChargingTime > FocusTime)
                {
                    Focus(FocusChargingTime, FocusTime);
                }
            }
            else
            {
                if (FocusChargingTime > 0.01f && FocusChargingTime < FocusTime)
                {
                    if (FireCurTime <= 0)
                    {
                        
                        FireCurTime = FireDelay;
                        anim.SetTrigger("Fire");
                        FocusChargingTime = 0;
                    }
                }
                else if (FocusChargingTime > FocusTime)
                {
                   
                    anim.SetTrigger("ChargingFire");
                    FocusFire();
                    FocusChargingTime = 0;
                }
            }
        }
        else
        {
            if (Input.GetKeyDown(KeyCode.X))
            {
                if (FireCurTime <= 0)
                {
                    
                    FireCurTime = FireDelay;
                    anim.SetTrigger("Fire");

                }
            }
        }    
    }

    public void Fire()
    {

        switch (elementSate)
        {
            case ElementState.None:
                {
                    // tempbullet.GetComponent<Rigidbody2D>().AddForce(ShootPower*vDir,ForceMode2D.Impulse);
                    curBullet = Instantiate(PoolingManager.instance.PlayerBullet);
                    //curBullet = PoolingManager.instance.GetPlayerBullet();
                    FireFeedback[(int)elementSate].PlayFeedbacks();
                    if (curBullet != null)
                    {
                        curBullet.transform.position = bulletPos.position;
                        curBullet.transform.rotation = bulletPos.rotation;
                        curBullet.SetActive(true);
                    }
                    //status.m_nMP
                    //tempbullet.GetComponent<Rigidbody2D>().AddForce(ShootPower * vDir, ForceMode2D.Impulse);
                    curBullet.GetComponent<Projectile>().Direction = vDir;
                    jetpack.JetpackFuelDurationLeft -= 1.0f;
                    jetpack._jetpackStoppedAt = Time.time;
                    jetpack.UpdateJetpackBar();
                    Destroy(curBullet, curBullet.GetComponent<Projectile>().LifeTime);
                }
                break;
            case ElementState.Fire:
                {
                    /*
                    // tempbullet.GetComponent<Rigidbody2D>().AddForce(ShootPower*vDir,ForceMode2D.Impulse);
                    curBullet = PoolingManager.instance.GetPlayerFireBullet();
                    if (curBullet != null)
                    {
                        curBullet.transform.position = bulletPos.position;
                        curBullet.transform.rotation = bulletPos.rotation;
                        curBullet.SetActive(true);
                    }
                    //status.m_nMP
                    //tempbullet.GetComponent<Rigidbody2D>().AddForce(ShootPower * vDir, ForceMode2D.Impulse);
                    curBullet.GetComponent<Projectile>().Direction = vDir;
                    jetpack.JetpackFuelDurationLeft -= 2.0f;
                    jetpack._jetpackStoppedAt = Time.time;
                    jetpack.UpdateJetpackBar();*/
                    //curBullet = PoolingManager.instance.GetPlayerBullet();
                    curBullet = Instantiate(PoolingManager.instance.PlayerBullet);
                    if (curBullet != null)
                    {
                        curBullet.transform.position = bulletPos.position;
                        curBullet.transform.rotation = bulletPos.rotation;
                        curBullet.SetActive(true);
                    }
                    FireFeedback[(int)elementSate].PlayFeedbacks();
                    curBullet.GetComponent<Projectile>().Direction = vDir;
                    jetpack.JetpackFuelDurationLeft -= 1.0f;
                    jetpack._jetpackStoppedAt = Time.time;
                    jetpack.UpdateJetpackBar();
                    Destroy(curBullet, curBullet.GetComponent<Projectile>().LifeTime);
                }
                break;
            case ElementState.Frost:
                {
                    curBullet = Instantiate(PoolingManager.instance.PlayerIceBullet);
                    //curBullet = PoolingManager.instance.GetPlayerIceBullet();
                    if (curBullet != null)
                    {
                        curBullet.transform.position = bulletPos.position;
                        curBullet.transform.rotation = bulletPos.rotation;
                        curBullet.SetActive(true);
                    }
                    FireFeedback[(int)elementSate].PlayFeedbacks();
                    curBullet.GetComponent<Projectile>().Direction = vDir;
                    //jetpack.JetpackFuelDurationLeft -= 1.0f;
                    jetpack._jetpackStoppedAt = Time.time;
                    jetpack.UpdateJetpackBar();
                    Destroy(curBullet, curBullet.GetComponent<Projectile>().LifeTime);
                }
                break;
            case ElementState.Earth:
                {
                    curBullet = Instantiate(PoolingManager.instance.PlayerEarthBullet);
                    //curBullet = PoolingManager.instance.GetPlayerEarthBullet();
                    if (curBullet != null)
                    {
                        curBullet.transform.position = bulletPos.position;
                        curBullet.transform.rotation = bulletPos.rotation;
                        curBullet.SetActive(true);
                    }
                    FireFeedback[(int)elementSate].PlayFeedbacks();
                    curBullet.GetComponent<Projectile>().Direction = vDir;
                    //curBullet.GetComponent<Rigidbody2D>().AddForce(ShootPower * vDir, ForceMode2D.Impulse);
                    jetpack.JetpackFuelDurationLeft -= 1.0f;
                    jetpack._jetpackStoppedAt = Time.time;
                    jetpack.UpdateJetpackBar();
                    Destroy(curBullet, curBullet.GetComponent<Projectile>().LifeTime);
                }
                break;
            case ElementState.Poison:
                {
                    curBullet = Instantiate(PoolingManager.instance.PlayerPoisonBullet);
                    //curBullet = PoolingManager.instance.GetPlayerPosionBullet();
                    if (curBullet != null)
                    {
                        curBullet.transform.position = bulletPos.position;
                        curBullet.transform.rotation = bulletPos.rotation;
                        curBullet.SetActive(true);
                    }
                    FireFeedback[(int)elementSate].PlayFeedbacks();
                    curBullet.GetComponent<Projectile>().Direction = vDir;
                    jetpack.JetpackFuelDurationLeft -= 1.0f;
                    jetpack._jetpackStoppedAt = Time.time;
                    jetpack.UpdateJetpackBar();
                    Destroy(curBullet, curBullet.GetComponent<Projectile>().LifeTime);
                }
                break;
        }
    }

    

    private void Heal()
    {
        if ((Input.GetKeyDown(KeyCode.V) || Input.GetKeyDown(KeyCode.Joystick1Button6)) && !isHeal)
        {
            isHeal = true;
            //int curHp = GetComponent<Health>().CurrentHealth;
            //GetComponent<CharacterHorizontalMovement>().ReadInput = false;
            _condition.ChangeState(CharacterStates.CharacterConditions.Frozen);
            corgiController.GravityActive(true);
            _movement.ChangeState(CharacterStates.MovementStates.Idle);
            anim.SetTrigger("Heal");

            StartCoroutine(Healing());
        }
    }
    
    private void HpCheck()
    {
        if(health.CurrentHealth <= 0)
        {
            StopAllCoroutines();
            _condition.ChangeState(CharacterStates.CharacterConditions.Dead);
            Stateabnormality(PlayerState.DEAD);
        }
    }

    IEnumerator Healing()
    {
       
        yield return new WaitForSeconds(1.0f);
        //Instantiate(HealEffect, transform.position + new Vector3(0, -0.55f, 0), transform.rotation);
        Instantiate(HealEffect, transform.position + new Vector3(0, -0.2f, 0), transform.rotation);
        health.GetHealth(PotionGetHealth, gameObject);
        HealFeedback.PlayFeedbacks();        
       // HealFeedback.PlayFeedbacks();
        LevelManager.Instance.PotionNum--;
        InfoManager.instance.UpdatePotionBar(LevelManager.Instance.PotionNum);
        yield return new WaitForSeconds(0.5f);
           
        corgiController.GravityActive(true);
        _condition.ChangeState(CharacterStates.CharacterConditions.Normal);
        //Instantiate(HealEffect, transform.position + new Vector3(0, -0.55f, 0), transform.rotation);
        Instantiate(HealEffect, transform.position + new Vector3(0, -0.2f, 0), transform.rotation);
        isHeal = false;
        
    }

    public void SetSkillTransform(float val)
    {
        Quaternion rotation = Quaternion.Euler(0, 0, val * curDegree);

        SkillPos.localRotation = rotation;
    }

    public void Stateabnormality(PlayerState state)
    {
        if (health.CurrentHealth <= 0)
            return;

        if (state == PlayerState.Sleep)
            effect.SpawnSpark();

        //character.ConditionState.ChangeState(CharacterStates.CharacterConditions.Paused);
        character.Stop();
        _movement.ChangeState(CharacterStates.MovementStates.Idle);
        corgiController._fallSlowFactor = 0;
        //corgiController.SlowFall(0f);
        //corgiController._fallSlowFactor = corgiController.DefaultParameters.Gravity;
        playerState = state;
    }

    public void StateReturn(PlayerState state)
    {
        character.ConditionState.ChangeState(CharacterStates.CharacterConditions.Normal);
        _movement.ChangeState(CharacterStates.MovementStates.Idle);
        //corgiController._fallSlowFactor = 0;
        playerState = state;

        effect.StateEffectDisable();
    }

    private void Init()
    {
        vDir = Vector2.right;
        bulletPos = Rightpos;
        rb = ParentsModel.GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        dashTime = startDashTime;
        character = ParentsModel.GetComponent<Character>();
        _condition = character.ConditionState;
        _movement = character.MovementState;
        //weapon = GetComponent<CharacterHandleWeapon>();
        chargeAttackScript = ParentsModel.GetComponent<CharacterHandleWeapon>().CurrentWeapon;
        corgiController = ParentsModel.GetComponent<CorgiController>();
        jetpack = ParentsModel.GetComponent<CharacterJetpack>();
        health = ParentsModel.GetComponent<Health>();
        //comboweapon = chargeAttackScript.GetComponent<ComboWeapon>();
        handleWeapon = ParentsModel.GetComponent<CharacterHandleWeapon>();
        characterDash = ParentsModel.GetComponent<CharacterDash>();
        characterJump = ParentsModel.GetComponent<CharacterJump>();
        clinging = ParentsModel.GetComponent<CharacterWallClinging>();
        effect = GetComponent<EffectSpawn>();
        corgiController.GravityActive(true);
        curDegree = maxDegree;
        characterDash.AbilityPermitted = PlayerInfoManager.instance.SkillInfo.Dash;
        characterJump.NumberOfJumps = PlayerInfoManager.instance.SkillInfo.Jumpnum;
        clinging.AbilityPermitted = PlayerInfoManager.instance.SkillInfo.Climb;
        DoubleDashOn = PlayerInfoManager.instance.SkillInfo.Doubledash;
        SuperModeCheck = health.Invulnerable;
    }


    /* private void OnGUI()
     {
     }*/

}

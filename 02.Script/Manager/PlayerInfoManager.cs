using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;
using MoreMountains.CorgiEngine;

public class PlayerInfoManager : MonoBehaviour
{

    public static PlayerInfoManager instance;
    public List<StageInfo> StageInfos;
    public List<SavePoint> checkPoints = new List<SavePoint>();
    public SlotManager slotManager;
    public LevelManager levelManager;
    public CharacterDash dash;
    public CharacterJump jump;
    public CharacterWallClinging clinging;
    public MaskSkills skills;
    public PassiveSkills Passivese;
    public GateManager gateManager;
    public ElementState elementState;
    public bool BossMode = false;
    public SaveData SkillInfo;
    public enum ElementState
    {
        None,
        Fire,
        Frost,
        Earth,
        Poison
    }

    private DateTime m_AppQuitTime = new DateTime(1970, 1, 1).ToLocalTime(); 
    private Coroutine m_RechargeTimerCoroutine = null;
    private SaveData saveData;
    

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);

        //DontDestroyOnLoad(this);
        if (!BossMode)
        {
            Init();
            PlayerLoad();
            InventroyLoad();
            GateStateLoad();
        }
    }

    private void Start()
    {
        if (!BossMode)
        {
            StageLoad();
            Passivese = GameManager.Instance.PersistentCharacter.GetComponentInChildren<PassiveSkills>();
        }

        elementState = ElementState.None;
        slotManager.ElementalStatusSlot.sprite = null;
        slotManager.ElementalStatusSlot.color = Color.clear;
    }

    private void Init()
    {
        dash = levelManager.PlayerPrefabs[0].GetComponent<CharacterDash>();
        jump = levelManager.PlayerPrefabs[0].GetComponent<CharacterJump>();
        clinging = levelManager.PlayerPrefabs[0].GetComponent<CharacterWallClinging>();
        skills = levelManager.PlayerPrefabs[0].GetComponentInChildren<MaskSkills>();
        //Passivese = levelManager.PlayerPrefabs[0].GetComponentInChildren<PassiveSkills>();
    }

    public void StageSave()
    {
        for (int i = 0; i < StageInfos.Count; i++)
        {
            StageInfos[i].Updatedata();
        }
        DataManager.StageStateSave(StageInfos);
    }

    public void StageLoad()
    {
        StageData stageData = DataManager.StageStateLoad();
        //StageInfos = stageData.stageinfo;

        if(stageData == null)
        {
            for (int i = 0; i < StageInfos.Count; i++)
            {
                for (int j = 0; j < StageInfos[i].Visit.Count; j++)
                {
                    StageInfos[i].Visit[j] = false;
                }
                for (int k = 0; k < StageInfos[i].BossClear.Count; k++)
                {
                    StageInfos[i].BossClear[k] = false;
                }
                for (int z = 0; z < StageInfos[i].Active.Count; z++)
                {
                    StageInfos[i].Active[z] = false;
                }
                StageInfos[i].LoadInit();
            }
            StageSave();
        }
        else
        {
            int ClearVal = 0;
            for (int i = 0; i < StageInfos.Count; i++)
            {
                for (int j = 0; j < StageInfos[i].Visit.Count; j++)
                {
                    StageInfos[i].Visit[j] = stageData.visit[j];
                }
                for (int k = 0; k < StageInfos[i].BossClear.Count; k++)
                {
                    StageInfos[i].BossClear[k] = stageData.clear[ClearVal];
                    ClearVal++;
                }
                for (int z = 0; z < StageInfos[i].objects.Count; z++)
                {
                    StageInfos[i].Active[z] = stageData.ObjectActive[z];
                }
                StageInfos[i].LoadInit();
            }
        }            
    }

    public void SavePointSave(SavePoint point)
    {
        for (int i = 0; i < checkPoints.Count; i++)
        {
            checkPoints[i].CurSave = false;
            if (checkPoints[i] == point)
            {
                checkPoints[i].CurSave = true;
            }
                
        }

        if (GameManager.Instance.PersistentCharacter)
            DataManager.PlayerDataSave(checkPoints, GameManager.Instance.PersistentCharacter.GetComponent<CharacterDash>(), GameManager.Instance.PersistentCharacter.GetComponent<CharacterJump>(), GameManager.Instance.PersistentCharacter.GetComponent<CharacterWallClinging>(), GameManager.Instance.PersistentCharacter.GetComponentInChildren<MaskSkills>());
        else
        {
            Debug.Log("NoCH");
        }
    }

    public void SkillGetSave(string code)
    {
        if(code == "I0301")
        {
            dash.AbilityPermitted = true;
            GameManager.Instance.PersistentCharacter.GetComponent<CharacterDash>().AbilityPermitted = dash.AbilityPermitted;
            SkillInfo.Dash = true;
            Debug.Log("dash");
        }
        else if(code == "I0302")
        {
            jump.NumberOfJumps = 2;
            GameManager.Instance.PersistentCharacter.GetComponent<CharacterJump>().NumberOfJumps = jump.NumberOfJumps;
            SkillInfo.Jumpnum = 2;
            Debug.Log("jump");
        }
        else if (code == "I0303")
        {
            clinging.AbilityPermitted = true;
            GameManager.Instance.PersistentCharacter.GetComponent<CharacterWallClinging>().AbilityPermitted = clinging.AbilityPermitted;
            SkillInfo.Climb = true;
        }
        else if (code == "I0304")
        {
            skills.DoubleDashOn = true;
            GameManager.Instance.PersistentCharacter.GetComponentInChildren<MaskSkills>().DoubleDashOn = skills.DoubleDashOn;
            SkillInfo.Doubledash = true;
        }
        DataManager.PlayerDataSave(checkPoints, GameManager.Instance.PersistentCharacter.GetComponent<CharacterDash>(), GameManager.Instance.PersistentCharacter.GetComponent<CharacterJump>(),
            GameManager.Instance.PersistentCharacter.GetComponent<CharacterWallClinging>(), GameManager.Instance.PersistentCharacter.GetComponentInChildren<MaskSkills>());
        //PlayerLoad();
    }

    public void PlayerLoad()
    {
        SaveData saveData = DataManager.PlayerDataLoad();
        int PointNum = checkPoints.Count-1;
        if (saveData == null)
        {
            checkPoints[0].checkPoint.gameObject.SetActive(true);
            checkPoints[0].Platform.gameObject.SetActive(true);
            checkPoints[0].Background.SetActive(true);
            checkPoints[0].room.ObjectsActive = true;
            checkPoints[0].MonsterSpawn();
            
            levelManager.CurrentCheckPoint = checkPoints[0].checkPoint;
            checkPoints[0].CurSave = true;
            dash.AbilityPermitted = false;
            jump.NumberOfJumps = 1;
            clinging.AbilityPermitted = false;
            skills.DoubleDashOn = false;
            DataManager.PlayerDataSave(checkPoints, dash, jump, clinging, skills);
            Debug.Log("NoData");
            return;
        }
        else
        {
            SkillInfo = saveData;
            for (int i = 0; i < saveData.CheckPoints.Count; i++)
            {
                checkPoints[i].CurSave = saveData.CheckPoints[i];
                if (saveData.CheckPoints[i] == true)
                {
                    checkPoints[i].checkPoint.gameObject.SetActive(true);
                    checkPoints[i].Platform.gameObject.SetActive(true);
                    checkPoints[i].Background.SetActive(true);
                    checkPoints[i].room.ObjectsActive = true;
                    
                    checkPoints[i].MonsterSpawn();
                    levelManager.CurrentCheckPoint = checkPoints[i].checkPoint;
                    //dash.AbilityPermitted = saveData.Dash;
                    //jump.NumberOfJumps = saveData.Jumpnum;
                    //clinging.AbilityPermitted = saveData.Climb;
                    //skills.DoubleDashOn = saveData.Doubledash;
                    PointNum = i;
                }
                else
                {
                    if (checkPoints[i].Platform == checkPoints[PointNum].Platform)
                        continue;
                    else
                        checkPoints[i].Platform.gameObject.SetActive(false);
                }
            }
        }     
    }

    public void ItemGetProcess(Items items)
    {
        switch (items.State)
        {
            case Slot.SlotState.ABILITY:
                {
                    for (int i = 0; i < slotManager.SkillSlot.Count; i++)
                    {
                        if (slotManager.SkillSlot[i].GetComponent<SlotData>().ItemCode == items.ItemCode)
                        {
                            slotManager.SkillSlot[i].GetItem();
                            slotManager.SkillSlot[i].Init();
                        }
                    }
                }
                break;
            case Slot.SlotState.ETC:
                {
                    for (int i = 0; i < slotManager.InventorylSlot.Count; i++)
                    {
                        if (slotManager.InventorylSlot[i].GetComponent<SlotData>().ItemCode == items.ItemCode)
                        {
                            slotManager.InventorylSlot[i].GetItem();
                            slotManager.InventorylSlot[i].Init();
                        }
                    }
                }
                break;
            case Slot.SlotState.PASSIVE:
                {
                    for (int i = 0; i < slotManager.PassiveSlot.Count; i++)
                    {
                        if (slotManager.PassiveSlot[i].GetComponent<SlotData>().ItemCode == items.ItemCode)
                        {
                            slotManager.PassiveSlot[i].GetItem();
                            slotManager.PassiveSlot[i].Init();
                        }
                    }
                }
                break;
            case Slot.SlotState.ELEMENTAL:
                {
                    for (int i = 0; i < slotManager.ElemetalSlot.Count; i++)
                    {
                        if (slotManager.ElemetalSlot[i].GetComponent<SlotData>().ItemCode == items.ItemCode)
                        {
                            slotManager.ElemetalSlot[i].GetItem();
                            slotManager.ElemetalSlot[i].Init();
                        }
                    }
                }
                break;
        }


        InventorySave();
    }

    public void InventorySave()
    {
        slotManager.SlotUpdate();
        DataManager.InventorySave(slotManager);
    }

    public void InventroyLoad()
    {
        InventoryData itemdata = DataManager.InventorLoad();
        if(itemdata == null)
        {
            slotManager.Init();
            InventorySave();
        }
        else
        {
            slotManager.Elemetal_get = itemdata.Elemental;
            slotManager.Skill_get = itemdata.Skills;
            slotManager.Inventory_get = itemdata.Inventory;
            slotManager.Passive_get = itemdata.Passive;
            slotManager.EqitSlot_eqit = itemdata.Eqit;
            slotManager.SlotLoad();
        }
    }

    public void GateStateSave()
    {

        List<bool> tempGatedata = new List<bool>();
        for (int i = 0; i < gateManager.gates.Count; i++)
        {
            tempGatedata.Add(gateManager.gates[i].GetComponent<Gate>().GateMeet);
        }

        DataManager.GateSave(tempGatedata);
    }

    public void GateStateLoad()
    {
        GateMeetData meetData = DataManager.GateLoad();
        if(meetData == null)
        {
            List<bool> tempGatedata = new List<bool>();
            for (int i = 0; i < gateManager.gates.Count; i++)
            {
                tempGatedata.Add(false);
            }
            DataManager.GateSave(tempGatedata);
            GateStateLoad();
        }
        else
        {
            for (int i = 0; i < meetData.GateMeet.Count; i++)
            {
                gateManager.gates[i].GetComponent<Gate>().GateMeet = meetData.GateMeet[i];
            }
        }
    }

    public void ElementalEqit(int StateNum)
    {
        elementState = (ElementState)StateNum;

        switch (elementState)
        {
            case ElementState.None:
                {
                    slotManager.ElemetalEqitSlot.color = Color.clear;
                    slotManager.ElementalStatusSlot.sprite = null;
                    slotManager.ElementalStatusSlot.color = Color.clear;
                }
                break;
            case ElementState.Fire:
                {
                    slotManager.ElementalStatusSlot.sprite = Iconstorage.instance.ElementIcon[0];
                    slotManager.ElementalStatusSlot.color = Color.white;
                    for (int i = 0; i < slotManager.ElemetalSlot.Count; i++)
                    {
                        if (slotManager.ElemetalSlot[i].GetComponent<SlotData>().ItemCode == "I0103")
                        {
                            if (!slotManager.ElemetalSlot[i].isGet)
                            {
                                GameManager.Instance.PersistentCharacter.GetComponentInChildren<MaskSkills>().ChangeElement();
                                return;
                            }

                            slotManager.ElemetalEqitSlot.sprite = slotManager.ElemetalSlot[i].CurImage.sprite;
                            slotManager.ElemetalEqitSlot.color = Color.white;
                        }
                    }                   
                }
                break;
            case ElementState.Frost:
                {
                    slotManager.ElementalStatusSlot.sprite = Iconstorage.instance.ElementIcon[1];
                    slotManager.ElementalStatusSlot.color = Color.white;
                    for (int i = 0; i < slotManager.ElemetalSlot.Count; i++)
                    {
                        if (slotManager.ElemetalSlot[i].GetComponent<SlotData>().ItemCode == "I0104")
                        {
                            if (!slotManager.ElemetalSlot[i].isGet)
                            {
                                GameManager.Instance.PersistentCharacter.GetComponentInChildren<MaskSkills>().ChangeElement();
                                return;
                            }

                            slotManager.ElemetalEqitSlot.sprite = slotManager.ElemetalSlot[i].CurImage.sprite;
                            slotManager.ElemetalEqitSlot.color = Color.white;
                        }
                    }
                }
                break;
            case ElementState.Earth:
                {
                    slotManager.ElementalStatusSlot.sprite = Iconstorage.instance.ElementIcon[2];
                    slotManager.ElementalStatusSlot.color = Color.white;
                    for (int i = 0; i < slotManager.ElemetalSlot.Count; i++)
                    {
                        if (slotManager.ElemetalSlot[i].GetComponent<SlotData>().ItemCode == "I0101")
                        {
                            if (!slotManager.ElemetalSlot[i].isGet)
                            {
                                GameManager.Instance.PersistentCharacter.GetComponentInChildren<MaskSkills>().ChangeElement();
                                return;
                            }

                            slotManager.ElemetalEqitSlot.sprite = slotManager.ElemetalSlot[i].CurImage.sprite;
                            slotManager.ElemetalEqitSlot.color = Color.white;
                        }
                    }
                }
                break;
            case ElementState.Poison:
                {
                    slotManager.ElementalStatusSlot.sprite = Iconstorage.instance.ElementIcon[3];
                    slotManager.ElementalStatusSlot.color = Color.white;
                    for (int i = 0; i < slotManager.ElemetalSlot.Count; i++)
                    {
                        if (slotManager.ElemetalSlot[i].GetComponent<SlotData>().ItemCode == "I0102")
                        {
                            if (!slotManager.ElemetalSlot[i].isGet)
                            {
                                GameManager.Instance.PersistentCharacter.GetComponentInChildren<MaskSkills>().ChangeElement();
                                return;
                            }

                            slotManager.ElemetalEqitSlot.sprite = slotManager.ElemetalSlot[i].CurImage.sprite;
                            slotManager.ElemetalEqitSlot.color = Color.white;
                        }
                    }
                }
                break;
        }
    }

    public void PassiveEqit(string code, bool isEqit)
    {
        if(!Passivese)
            Passivese = GameManager.Instance.PersistentCharacter.GetComponentInChildren<PassiveSkills>();

        Passivese.PassiveActive(code, isEqit);
    }

    public void ClearSave()
    {
        DataManager.ClearSsave();
    }

    /*
    public void OnApplicationFocus(bool focus)
    {
        if(focus)
        {
            if (saveData != null)
            {
                LoadHeartInfo();
                LoadAppQuitTime();
                SetRechargeScheduler();
            }
        }
        else
        {
            if (saveData != null)
            {
                SaveHeartInfo();
                SaveAppQuitTime();

                if (m_RechargeTimerCoroutine != null)
                    StopCoroutine(m_RechargeTimerCoroutine);
            }
        }
    }

    public void OnApplicationQuit()
    {
        Debug.Log("게임종료");
        SaveHeartInfo();
        SaveAppQuitTime();
    }
    */

    /*
    public bool LoadHeartInfo()
    {
      //  Debug.Log("LoadHeartInfo");
        bool result = false;
        try
        {
            if (PlayerPrefs.HasKey("HeartAmount"))
            {
                StaminaInfo = PlayerPrefs.GetInt("HeartAmount");
                if (StaminaInfo < 0)
                {
                    StaminaInfo = 0;
                }
            }
            else
            {
                StaminaInfo = 0;
            }
            //heartAmountLabel.text = StaminaInfo.ToString();
            result = true;
        }
        catch (System.Exception e)
        {
            Debug.LogError("LoadHeartInfo Failed (" + e.Message + ")");
        }
        return result;
    }
    public bool SaveHeartInfo()
    {
      //  Debug.Log("SaveHeartInfo");
        bool result = false;
        try
        {
            PlayerPrefs.SetInt("HeartAmount", StaminaInfo);
            PlayerPrefs.Save();
            result = true;
        }
        catch (System.Exception e)
        {
            Debug.LogError("SaveHeartInfo Failed (" + e.Message + ")");
        }
        return result;
    }

    
    public bool LoadAppQuitTime()
    {
       // Debug.Log("LoadAppQuitTime");
        bool result = false;
        try
        {
            if (PlayerPrefs.HasKey("AppQuitTime"))
            {
                var appQuitTime = string.Empty;
                appQuitTime = PlayerPrefs.GetString("AppQuitTime");
                m_AppQuitTime = DateTime.FromBinary(Convert.ToInt64(appQuitTime));
            }
            else
            {
                Debug.Log("현재시간 불러옴");
                m_AppQuitTime = DateTime.Now.ToLocalTime();
            }
            //appQuitTimeLabel.text = string.Format("AppQuitTime : {0}", m_AppQuitTime.ToString());
            result = true;
        }
        catch (System.Exception e)
        {
            Debug.LogError("LoadAppQuitTime Failed (" + e.Message + ")");
        }
        return result;
    }

    public bool SaveAppQuitTime()
    {
       // Debug.Log("SaveAppQuitTime");
        bool result = false;
        try
        {
            var appQuitTime = DateTime.Now.ToLocalTime().ToBinary().ToString();
            PlayerPrefs.SetString("AppQuitTime", appQuitTime);
            PlayerPrefs.SetInt("RemainTime", m_RechargeRemainTime);
            PlayerPrefs.Save();
            result = true;
        }
        catch (System.Exception e)
        {
            Debug.LogError("SaveAppQuitTime Failed (" + e.Message + ")");
        }
        return result;
    }

    public void SetRechargeScheduler(Action onFinish = null)
    {
        if (m_RechargeTimerCoroutine != null)
        {
            StopCoroutine(m_RechargeTimerCoroutine);
        }
        var timeDifferenceInSec = (int)((DateTime.Now.ToLocalTime() - m_AppQuitTime).TotalSeconds);
        int origintimeDifferenceInSec = timeDifferenceInSec;
        int remainTime = 0;
        int heartToAdd;
        if (timeDifferenceInSec > 0)
        {
            timeDifferenceInSec = PlayerPrefs.GetInt("RemainTime") - timeDifferenceInSec;

            if (timeDifferenceInSec <= 0)
            {
                StaminaInfo++;
                timeDifferenceInSec = Math.Abs(timeDifferenceInSec);
                heartToAdd = timeDifferenceInSec / StaminaRechargeInterval;
                if (heartToAdd == 0)
                    remainTime = StaminaRechargeInterval - timeDifferenceInSec;
                else
                    remainTime = StaminaRechargeInterval - (timeDifferenceInSec & StaminaRechargeInterval);

            }
            else
            {
                heartToAdd = timeDifferenceInSec / StaminaRechargeInterval;
                if (heartToAdd == 0)
                    remainTime = PlayerPrefs.GetInt("RemainTime") - origintimeDifferenceInSec;
            }
            StaminaInfo += heartToAdd;
        }

        else if (timeDifferenceInSec < 0)
            Debug.Log("편법을 쓰셨군요");

        if (StaminaInfo >= MaxStamina)
        {
            StaminaInfo = MaxStamina;
        }
        else
        {
            m_RechargeTimerCoroutine = StartCoroutine(DoRechargeTimer(remainTime, onFinish));
        }
        //heartAmountLabel.text = string.Format("Hearts : {0}", m_HeartAmount.ToString());
    }

    public void UseStamina(Action onFinish = null)
    {
        if (StaminaInfo <= 0)
        {
            return;
        }

        StaminaInfo--;
       // heartAmountLabel.text = string.Format("Hearts : {0}", StaminaInfo.ToString());
        if (m_RechargeTimerCoroutine == null)
        {
            m_RechargeTimerCoroutine = StartCoroutine(DoRechargeTimer(StaminaRechargeInterval));
        }
        if (onFinish != null)
        {
            onFinish();
        }
    }
    
    private IEnumerator DoRechargeTimer(int remainTime, Action onFinish = null)
    {
        if (remainTime <= 0)
        {
            m_RechargeRemainTime = StaminaRechargeInterval;
        }
        else
        {
            m_RechargeRemainTime = remainTime;
        }
        //heartRechargeTimer.text = string.Format("Timer : {0} s", m_RechargeRemainTime);

        while (m_RechargeRemainTime > 0)
        {
            //Debug.Log("heartRechargeTimer : " + m_RechargeRemainTime + "s");
            // heartRechargeTimer.text = string.Format("Timer : {0} s", m_RechargeRemainTime);
            if (MainMenuManager.instance)
            {
                TimeSpan timespan = TimeSpan.FromSeconds(m_RechargeRemainTime);

                MainMenuManager.instance.StaminaRechargeTimer.text = string.Format("Next Stamina recovery {0:00} : {1:00} ", timespan.Minutes,timespan.Seconds);
                //MainMenuManager.instance.StaminaRechargeTimer.text = string.Format("Next Stamina recovery {0} s", m_RechargeRemainTime);
            }
            m_RechargeRemainTime -= 1;
            yield return new WaitForSeconds(1f);
        }
        StaminaInfo++;
        if (MainMenuManager.instance)
        {
            MainMenuManager.instance.MoneyRefesh();
        }

        if (StaminaInfo >= MaxStamina)
        {
            StaminaInfo = MaxStamina;
            m_RechargeRemainTime = 0;
            //heartRechargeTimer.text = string.Format("Timer : {0} s", m_RechargeRemainTime);
            Debug.Log("HeartAmount reached max amount");
            m_RechargeTimerCoroutine = null;
        }
        else
        {
            m_RechargeTimerCoroutine = StartCoroutine(DoRechargeTimer(StaminaRechargeInterval, onFinish));
        }
        //heartAmountLabel.text = string.Format("Hearts : {0}", m_HeartAmount.ToString());
        //Debug.Log("HeartAmount : " + m_HeartAmount);
    }
    */
    /*
    private void OnGUI()
    {
        if (GUI.Button(new Rect(200, 0, 200, 80), "ATTACK"))
        {
            PassiveEqit();
        }
    }*/

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SlotManager : MonoBehaviour
{
    public List<Slot> ElemetalSlot = new List<Slot>();
    public Image ElemetalEqitSlot;
    public Image ElementalStatusSlot;
    public List<Slot> SkillSlot = new List<Slot>();
    public List<Slot> InventorylSlot = new List<Slot>();
    public List<Slot> PassiveSlot = new List<Slot>();
    public List<SlotData> EqitSlot = new List<SlotData>();
    

    public List<bool> Elemetal_get = new List<bool>();
    public List<bool> Skill_get = new List<bool>();
    public List<bool> Inventory_get = new List<bool>();
    public List<bool> Passive_get = new List<bool>();
    public List<string> EqitSlot_eqit = new List<string>();

    public void Init()
    {
        for (int i = 0; i < ElemetalSlot.Count; i++)
        {
            Elemetal_get.Add(ElemetalSlot[i].isGet);
        }

        for (int i = 0; i < SkillSlot.Count; i++)
        {
            Skill_get.Add(SkillSlot[i].isGet);
        }

        for (int i = 0; i < InventorylSlot.Count; i++)
        {
            Inventory_get.Add(InventorylSlot[i].isGet);
        }

        for (int i = 0; i < PassiveSlot.Count; i++)
        {
            Passive_get.Add(PassiveSlot[i].isGet);
        }

        for (int i = 0; i < EqitSlot.Count; i++)
        {
            EqitSlot_eqit.Add(EqitSlot[i].ItemCode);
        }

       // PlayerInfoManager.instance.InventroyLoad();
       // PlayerInfoManager.instance.ItemInfo.Add(Elemetal_get);
       // PlayerInfoManager.instance.ItemInfo.Add(Skill_get);
      //  PlayerInfoManager.instance.ItemInfo.Add(Inventory_get);
     //   PlayerInfoManager.instance.ItemInfo.Add(Passive_get);
    }

    public void SlotLoad()
    {
        for (int i = 0; i < ElemetalSlot.Count; i++)
        {
            ElemetalSlot[i].isGet = Elemetal_get[i];
        }

        for (int i = 0; i < SkillSlot.Count; i++)
        {
            SkillSlot[i].isGet =Skill_get[i];
        }

        for (int i = 0; i < InventorylSlot.Count; i++)
        {
             InventorylSlot[i].isGet = Inventory_get[i];
        }

        for (int i = 0; i < PassiveSlot.Count; i++)
        {
            PassiveSlot[i].isGet = Passive_get[i];
        }

        for (int i = 0; i < EqitSlot.Count; i++)
        {
            EqitSlot[i].ItemCode = EqitSlot_eqit[i];
            EqitSlot[i].DataLoad();
            if(EqitSlot[i].ItemCode != "")
            {
                EqitSlot[i].GetComponent<Slot>().isEqit = true;
            }
        }
    }

    public void SlotUpdate()
    {
        for (int i = 0; i < ElemetalSlot.Count; i++)
        {
            Elemetal_get[i] = ElemetalSlot[i].isGet;
        }

        for (int i = 0; i < SkillSlot.Count; i++)
        {
            Skill_get[i] = SkillSlot[i].isGet;
        }

        for (int i = 0; i < InventorylSlot.Count; i++)
        {
            Inventory_get[i] = InventorylSlot[i].isGet;
        }

        for (int i = 0; i < PassiveSlot.Count; i++)
        {
            Passive_get[i] = PassiveSlot[i].isGet;
        }

        for (int i = 0; i < EqitSlot.Count; i++)
        {
            EqitSlot_eqit[i] = EqitSlot[i].ItemCode;
        }
    }

}

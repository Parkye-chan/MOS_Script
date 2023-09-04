using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class InventoryData
{
    public List<bool> Elemental; // 원소석
    public List<bool> Skills; // 스킬
    public List<bool> Inventory; // 인벤토리아이템
    public List<bool> Passive; // 패시브스킬
    public List<string> Eqit; // 장착스킬

    public InventoryData()
    {
        this.Elemental = new List<bool>();
        this.Skills = new List<bool>();
        this.Inventory = new List<bool>();
        this.Passive = new List<bool>();
        this.Eqit = new List<string>();
    }

    public InventoryData(List<bool> m_Elemental, List<bool> m_Skills, List<bool> m_Inventory, List<bool> m_Passive, List<string> m_eqit)
    {
        this.Elemental = m_Elemental;
        this.Skills = m_Skills;
        this.Inventory = m_Inventory;
        this.Passive = m_Passive;
        this.Eqit = m_eqit;
    }
}

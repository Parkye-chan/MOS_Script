using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class InventoryData
{
    public List<bool> Elemental; // ���Ҽ�
    public List<bool> Skills; // ��ų
    public List<bool> Inventory; // �κ��丮������
    public List<bool> Passive; // �нú꽺ų
    public List<string> Eqit; // ������ų

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

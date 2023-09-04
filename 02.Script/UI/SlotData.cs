using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class SlotData : MonoBehaviour
{
    public string ItemCode; //�������ڵ�
    public Slot.SlotState ItemClass; //������ ����
    public string ItemName; // �������̸�
    public string ImageCode; //�������� �̹���
    public string ImageCode2; //�������� ������
    public string Description;  //������ ����

    private void Start()
    {
        DataLoad();
    }

    public static class EnumUtil<T>
    {
        public static T Parse(string s)
        {
            return (T)Enum.Parse(typeof(T), s);
        }
    }

    public void DataLoad()
    {
        List<Dictionary<string, object>> data = InventoryManager.Instance.data;

        if (data != null)
        {
            
            for (int i = 0; i < data.Count; i++)
            {
                if(data[i]["ItemCode"].ToString() == ItemCode)
                {
                    
                    //ItemCode = data[i]["ItemCode"].ToString();
                    ItemClass = EnumUtil<Slot.SlotState>.Parse(data[i]["ItemClass"].ToString());
                    ItemName = data[i]["ItemName"].ToString();
                    ImageCode = data[i]["ImageCode"].ToString();
                    ImageCode2 = data[i]["ImageCode2"].ToString();
                    Description = data[i]["Description"].ToString();
                }
            }
        }
    }
}

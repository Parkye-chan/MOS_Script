using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class SlotData : MonoBehaviour
{
    public string ItemCode; //아이템코드
    public Slot.SlotState ItemClass; //아이템 종류
    public string ItemName; // 아이템이름
    public string ImageCode; //아이템의 이미지
    public string ImageCode2; //아이템의 프리팹
    public string Description;  //아이템 설명

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

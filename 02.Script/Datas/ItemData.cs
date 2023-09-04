using System.Collections;
using System.Collections.Generic;
using UnityEngine;



[System.Serializable]
public class ItemData
{
    //임시 데이터
    public string StoneCode; // 원소석 고유코드
    public string StoneName; // 원소석 이름
    public  Item.ItemType Type; // 능력치 타입
    public int UpgradeVal; // 업그레이드 증가값
    public string itemImage;
    public int ItemLv;


    public ItemData(int code,string name, Item.ItemType type, int UpgradeVal, string _ItemImage, int _ItemLv)
    {
        this.StoneCode = code.ToString();
        this.StoneName = name;
        this.Type = type;
        this.UpgradeVal = UpgradeVal;
        this.itemImage = _ItemImage;
        this.ItemLv = _ItemLv;
    }
}

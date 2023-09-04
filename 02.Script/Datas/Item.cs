using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Item", menuName = "New Item/item")]
public class Item : ScriptableObject
{
    public string itemName; // 아이템이름
    public ItemType itemType; //아이템의 유형
    public Sprite itemImage; //아이템의 이미지
    public GameObject itemPrefab; //아이템의 프리팹
    public int UpgradeVal; // 업그레이드 증가값
    public int itemLv = 1;

    public enum ItemType
    {
        STR,
        Money,
        ManaCost,
        SPEED,
        None
    }
}

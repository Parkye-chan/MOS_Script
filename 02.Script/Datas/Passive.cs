using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Item", menuName = "New Item/Passive")]
public class Passive : ScriptableObject
{

    public string ItemCode; //아이템코드
    public SlotState ItemClass; //아이템 종류
    public string ItemName; // 아이템이름
    public Sprite ImageCode; //아이템의 이미지
    public Sprite ImageCode2; //아이템의 프리팹
    public int Description; // 업그레이드 증가값

    public enum SlotState
    {
        ELEMENTAL,
        ABILLTY,
        PASSIVE
    }
}

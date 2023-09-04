using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Mask", menuName = "New Mask/mask")]
public class Mask : ScriptableObject
{
    public string MaskCode; // 마스크코드
    public string MaskName; // 마스크이름
    public string MaskType; //마스크의 타입
    public int MaskState; //마스크의 상태정보
    public Sprite MaskImage; //마스크의 이미지
    public string MaskSkill_1; //마스크의 스킬 1
    public string MaskSkill_2; //마스크의 스킬 2
    public Sprite MaskSkill_1_Image; //마스크 스킬 1의 이미지
    public Sprite MaskSkill_2_Image; //마스크 스킬 2의 이미지

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public class MaskData
{
    public string MaskCode; //마스크 코드
    public string MaskName; //마스크 이름
    public string MaskSkill_1; // 마스크에 할당된 스킬1
    public string MaskSkill_2; // 마스크에 할당된 스킬2
    /*
    public bool FirstPoint = false; //첫번째 강화 해금 유무
    public bool SecondPoint = false; //두번째 강화 해금 유무
    public bool ThirdPoint = false; //세번째 강화 해금 유무
    public PlayerSkills.SkillState FirstSkillState; //첫번째 스킬 강화 상태
    public PlayerSkills.SkillState SecondSkillState; //두번째 스킬 강화 상태
    public PlayerSkills.SkillState ThirdSkillState; //세번째 스킬 강화 상태
    */
    public int MaskState; // 마스크의 enum번호
    public string MaskType; //마스크의 타입
    public string Skill2_Type;
    public bool Maskget; // 마스크 흭득 여부

    public MaskData()
    {
        this.MaskCode = "0";
        this.MaskName = "0";
        this.MaskSkill_1 = "0";
        this.MaskSkill_2 = "0";
        this.MaskState = 0;
        this.MaskType = "";
        this.Skill2_Type = "";
        this.Maskget = false;
    }


    public MaskData(string m_MaskCode,string m_MaskName,string m_MaskSkill_1,string m_MaskSkill_2, int m_MaskState,string m_maskType,string m_Skill2_Type, bool m_Maskget)
    {
        this.MaskCode = m_MaskCode;
        this.MaskName = m_MaskName;
        this.MaskSkill_1 = m_MaskSkill_1;
        this.MaskSkill_2 = m_MaskSkill_2;
        /*
        this.FirstPoint = m_FirstPoint;
        this.SecondPoint = m_SecondPointm;
        this.ThirdPoint = m_ThirdPoint;
        this.FirstSkillState = m_FirstSkillState;
        this.SecondSkillState = m_SecondSkillState;
        this.ThirdSkillState = m_ThirdSkillState;
        */
        this.MaskState = m_MaskState;
        this.MaskType = m_maskType;
        this.Skill2_Type = m_Skill2_Type;
        this.Maskget = m_Maskget;
    }
}

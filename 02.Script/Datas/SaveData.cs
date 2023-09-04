using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public class SaveData
{
    //임시 데이터
    public List<bool> CheckPoints;
    public int Gold; // 돈
    public int Gem; // 보석
    public bool Dash; // 기술해금 유무
    public int Jumpnum;
    public bool Climb;
    public bool Doubledash;


    public SaveData()
    {
        this.CheckPoints = new List<bool>();
        CheckPoints.Add(true);
        this.Gold = 0;
        this.Gem = 0;
        this.Dash = false;
        this.Jumpnum = 1;
        this. Climb = false;
        this.Doubledash = false;

    }

public SaveData(List<bool> m_checkpoint, int gold, int gem, bool m_Dash,int m_jumpnum, bool m_climb, bool m_doubledash)
    {
        this.CheckPoints = m_checkpoint;
        this.Gold = gold;
        this.Gem = gem;
        this.Dash = m_Dash;
        this.Jumpnum = m_jumpnum;
        this.Climb = m_climb;
        this.Doubledash = m_doubledash;
    }



}

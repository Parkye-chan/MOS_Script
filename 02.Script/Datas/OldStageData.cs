using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class OldStageData
{
    public string StageCode;
    public bool Open;
    public StageInfoData.LevelType Type;

    public OldStageData(string m_Stagecode,bool m_Open, StageInfoData.LevelType m_type )
    {
        this.StageCode = m_Stagecode;
        this.Open = m_Open;
        this.Type = m_type;
    }

}

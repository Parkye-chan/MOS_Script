using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public class StageData
{
    public List<StageInfo> stageinfo;
    public List<bool> visit;
    public List<bool> clear;
    public List<bool> ObjectActive;
    public StageData()
    {
        stageinfo = new List<StageInfo>();
        visit = new List<bool>();
        clear = new List<bool>();
        ObjectActive = new List<bool>();
    }

    public StageData(List<StageInfo> m_stageInfos, List<bool> m_visit, List<bool> m_clear, List<bool> m_ObjectActive)
    {
        stageinfo = m_stageInfos;
        visit = m_visit;
        clear = m_clear;
        ObjectActive = m_ObjectActive;
    }

}

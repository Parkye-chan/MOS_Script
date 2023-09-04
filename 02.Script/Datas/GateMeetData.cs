using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GateMeetData
{

    public List<bool> GateMeet;



    public GateMeetData()
    {
        this.GateMeet = new List<bool>();

    }

    public GateMeetData(List<bool> m_GateMeet)
    {
        this.GateMeet = m_GateMeet;
    }


}

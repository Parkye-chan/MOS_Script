using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DailyManager : MonoBehaviour
{
    [SerializeField]
    List<Dictionary<string, object>> data;
    [SerializeField]
    List<DailyData> dailyDatas = new List<DailyData>();
    [SerializeField]
    Sprite gold;
    [SerializeField]
    Sprite piece;
    [SerializeField]
    Sprite wind;
    [SerializeField]
    Sprite fire;
    [SerializeField]
    Sprite earth;
    [SerializeField]
    Sprite water;
    [SerializeField]
    Sprite diamond;

    void Start()
    {
        Init();
    }

    public void Init()
    {
        data = CSVReader.Read("DailyData");
        for (int i = 0; i < data.Count; i++)
        {                     
            dailyDatas[i].dayText.text = data[i]["Day"].ToString();
            dailyDatas[i].countText.text = data[i]["DailyCount"].ToString();
           
            if(data[i]["DailyItem"].ToString() == "gold")
            {
                dailyDatas[i].GetComponent<Image>().sprite = gold;
            }
            else if (data[i]["DailyItem"].ToString() == "piece")
            {
                dailyDatas[i].GetComponent<Image>().sprite = piece;
            }
            else if(data[i]["DailyItem"].ToString() == "wind")
            {
                dailyDatas[i].GetComponent<Image>().sprite = wind;
            }
            else if(data[i]["DailyItem"].ToString() == "fire")
            {
                dailyDatas[i].GetComponent<Image>().sprite = fire;
            }
            else if(data[i]["DailyItem"].ToString() == "water")
            {
                dailyDatas[i].GetComponent<Image>().sprite = water;
            }
            else if(data[i]["DailyItem"].ToString() == "earth")
            {
                dailyDatas[i].GetComponent<Image>().sprite = earth;
            }
            else if(data[i]["DailyItem"].ToString() == "diamond")
            {
                dailyDatas[i].GetComponent<Image>().sprite = diamond;
            }
        }
    }
}

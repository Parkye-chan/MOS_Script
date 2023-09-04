using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DailyData : MonoBehaviour
{

    public Text countText;
    public Text dayText;
    public Image CheckImg;


    private void Awake()
    {
        countText = transform.Find("CountText").GetComponent<Text>();
        dayText = transform.Find("DayText").GetComponent<Text>();
        CheckImg = transform.Find("ItemImage").GetComponent<Image>();
    }
}

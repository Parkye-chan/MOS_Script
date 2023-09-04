using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;

public class StageInfoData : MonoBehaviour
{

    public enum LevelType
    {
        Normal,
        BossBattle,
        Challenge
    }

    [SerializeField]
    public string StageCode;
    [SerializeField]
    public bool Open = false;
    public bool Choice = false;
    public Text StageTxt;
    public Transform TwincleImg;
    public int SmallStage;
    private string curstage;
    [SerializeField]
    List<Dictionary<string, object>> data;
    [SerializeField]
    private LevelType levelType;
    [SerializeField]
    public int TempSceneNumber;

    public List<string> monsterInfo = new List<string>();
    public List<string> rewardInfo = new List<string>();


    private void Awake()
    {
        switch (levelType)
        {
            case LevelType.Normal:
                {
                    Init();
                    TwincleImg = this.transform.Find("TwincleImg");
                    StageTxt = transform.Find("Text").GetComponent<Text>();
                    if (StageTxt)
                    {
                        if (int.Parse(StageCode) / (int)Mathf.Pow(10, 4 - 1) % 10 == 2)
                            StageTxt.text = "Boss";
                        else
                        {
                            //int BigStage = (StageNum / (int)Mathf.Pow(10, 3 - 1)) % 10;
                            SmallStage = int.Parse(StageCode) / (int)Mathf.Pow(10, 1 - 1) % 10;
                            StageTxt.text = curstage + "-" + SmallStage;
                        }
                    }
                }
                break;
            case LevelType.BossBattle:
                {
                    Init();
                    Color color = this.gameObject.GetComponent<Image>().color;
                    StageTxt = transform.Find("StageNum").GetComponent<Text>();
                    int Stage = (int.Parse(StageCode) / (int)Mathf.Pow(10, 1 - 1) % 10);
                    if (StageTxt)
                    {
                        if (int.Parse(StageCode) / (int)Mathf.Pow(10, 1 - 1) % 10 == 0)
                            StageTxt.text = "10 Stage - " + curstage;
                        else
                            StageTxt.text = int.Parse(StageCode) / (int)Mathf.Pow(10, 1 - 1) % 10 + " Stage - " + curstage;

                        color = new Color(135 / 255f, 135 / 255f, 135 / 255f);
                    }
                }
                break;
            case LevelType.Challenge:
                {
                    Init();
                    StageTxt = transform.Find("Text").GetComponent<Text>();
                    TwincleImg = this.transform.Find("Image");
                    if (StageTxt)
                    {
                        StageTxt.text = curstage + " Stage";
                    }
                }
                break;
        }
    }

    public void Init()
    {
        data = CSVReader.Read("StageData");
        for (int i = 0; i < data.Count; i++)
        {
            if (data[i]["StageCode"].ToString() ==  StageCode)
            {
                this.curstage = data[i]["StageNum"].ToString();           
                monsterInfo[0] = data[i]["Monster01"].ToString();
                monsterInfo[1] = data[i]["Monster02"].ToString();
                monsterInfo[2] = data[i]["Monster03"].ToString();
                rewardInfo[0] = data[i]["Reward01"].ToString();
                rewardInfo[1] = data[i]["Reward02"].ToString();
                rewardInfo[2] = data[i]["Reward03"].ToString();
                
            }
        }
    }


}

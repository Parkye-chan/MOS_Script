using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MoreMountains.CorgiEngine;
using MoreMountains.Tools;
using UnityEngine.UI;

public class MainMenuManager : MonoBehaviour
{


    public enum MainMenuState
    {
        MAIN,
        BOSS
    }

    public GameObject MainPanel;
    public GameObject BossPanel;
    public List<BossModeSlot> BossSlots = new List<BossModeSlot>();
    public List<GameObject> MainSlot = new List<GameObject>();
    public List<Text> MainText = new List<Text>();
    public List<Button> MainBtn = new List<Button>();

    private int curVal = 0;
    private MainMenuState state;
    private Color txtcolor = new Color(135/255f,245/255f , 255/255f);
    private void Awake()
    {
        ClearCheck();
    }

    // Start is called before the first frame update
    void Start()
    {
        Init();
        BossPanel.SetActive(false);
        state = MainMenuState.MAIN;
    }
    // Update is called once per frame
    void Update()
    {

        switch (state)
        {
            case MainMenuState.MAIN:
                {
                    if (Input.GetKeyDown(KeyCode.DownArrow) || (Input.GetAxis("Player1_Vertical") == 1))
                    {
                        curVal++;
                        MainSlot[curVal-1].SetActive(false);
                        MainText[curVal - 1].color = Color.white;

                        if (curVal < MainBtn.Count-1)
                        {
                            if(!MainBtn[curVal].gameObject.activeSelf)
                            curVal++;
                        }
                        MainSelection();
                    }
                    else if (Input.GetKeyDown(KeyCode.UpArrow) || (Input.GetAxis("Player1_Vertical") == -1))
                    {
                        curVal--;
                        MainSlot[curVal+1].SetActive(false);
                        MainText[curVal + 1].color = Color.white;
                        if (curVal > 0)
                        {
                            if(!MainBtn[curVal].gameObject.activeSelf)
                            curVal--;
                        }
                        MainSelection();
                    }
                    else if (Input.GetKeyDown(KeyCode.Z) || Input.GetKeyDown(KeyCode.Joystick1Button1))
                    {
                        MainBtn[curVal].onClick.Invoke();
                    }
                }
                break;
            case MainMenuState.BOSS:
                {
                    if (Input.GetKeyDown(KeyCode.RightArrow) || Input.GetAxis("Player1_Horizontal") == 1)
                    {
                        BossSlots[curVal].ChoiceBack.SetActive(false);
                        BossSlots[curVal].NormalBack.SetActive(true);
                        curVal++;
                        BossSelection();
                    }
                    else if (Input.GetKeyDown(KeyCode.LeftArrow)|| Input.GetAxis("Player1_Horizontal") == -1)
                    {
                        BossSlots[curVal].ChoiceBack.SetActive(false);
                        BossSlots[curVal].NormalBack.SetActive(true);
                        curVal--;
                        BossSelection();
                    }
                    else if (Input.GetKeyDown(KeyCode.Z) || Input.GetKeyDown(KeyCode.Joystick1Button1))
                    {
                        if (BossSlots[curVal].LevelName == "" || BossSlots[curVal].LevelName == null)
                            return;

                        LoadingSceneManager.LoadScene(BossSlots[curVal].LevelName);
                    }
                    else if (Input.GetKeyDown(KeyCode.X) || Input.GetKeyDown(KeyCode.Joystick1Button0))
                    {
                        Init();
                        MainPanel.SetActive(true);
                        BossPanel.SetActive(false);
                        state = MainMenuState.MAIN;
                        curVal = 0;
                        //isBossMode = false;
                    }
                }
                break;
        }
    }

    private void Init()
    {
        for (int i = 0; i < BossSlots.Count; i++)
        {
            BossSlots[i].ChoiceBack.SetActive(false);
            BossSlots[i].NormalBack.SetActive(true);
        }
        for (int i = 0; i < MainSlot.Count; i++)
        {
            MainSlot[i].SetActive(false);
        }

        BossSlots[0].ChoiceBack.SetActive(true);
        BossSlots[0].NormalBack.SetActive(false);
        MainSlot[0].SetActive(true);
        
    }

    private void ClearCheck()
    {
        string Key = DataManager.ClearLoad();
        if(Key == "Xion" )
        {
            MainBtn[1].gameObject.SetActive(true);
        }
        else
        {
            MainBtn[1].gameObject.SetActive(false);
        }
    }

    private void BossSelection()
    {
        if (curVal >= BossSlots.Count)
            curVal = 0;
        else if (curVal < 0)
            curVal = BossSlots.Count - 1;

        BossSlots[curVal].ChoiceBack.SetActive(true);
        BossSlots[curVal].NormalBack.SetActive(false);
    }

    private void MainSelection()
    {

        if (curVal >= MainSlot.Count)
            curVal = 0;
        else if (curVal < 0)
            curVal = MainSlot.Count - 1;

        MainSlot[curVal].SetActive(true);
        MainText[curVal].color = txtcolor;
    }

    public void BossModeWindow()
    {
        Init();
        curVal = 0;
        state = MainMenuState.BOSS;
        //isBossMode = true;
        MainPanel.SetActive(false);
        BossPanel.SetActive(true);
    }
}

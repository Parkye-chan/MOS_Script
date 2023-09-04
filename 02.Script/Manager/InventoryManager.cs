using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using MoreMountains.CorgiEngine;

public class InventoryManager : MonoBehaviour
{

    public static InventoryManager Instance;

    public CanvasGroup Target;
    public RectTransform Panel;
    public GameObject[] Panels;
    public RectTransform center;
    public SlotCollecter Ability;
    public SlotCollecter Inventory;
    public SlotCollecter Passive;
    public List<Dictionary<string, object>> data;
    public GateManager gateManager;
    public bool OpenInventory = false;
    private float[] Distance;
    private float[] DistRePosition;
    private bool dragging;
    private int PanelDistance;
    private int MinPanelNum;
    private int PanelLenght;
    private float LerpSpeed = 5f;
    private bool TargetNearestButton = true;
    private int curnum = 0;
    private SlotCollecter curCollector;
    private bool LeftArrowKey = false;
    private bool RightArrowKey = false;
    private SlotCollecter.SlotState slotState;
    private Character _character;
    private MaskSkills skills;


    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(this);


        DataLoad();
    }

    private void Start()
    {
        _character = GameManager.Instance.PersistentCharacter.GetComponent<Character>();
        skills = _character.CharacterModel.GetComponent<MaskSkills>();
        Target.alpha = 0;
        Target.blocksRaycasts = false;
        PanelLenght = Panels.Length;
        Distance = new float[PanelLenght];
        DistRePosition = new float[PanelLenght];
        PanelDistance = (int)Mathf.Abs(Panels[1].GetComponent<RectTransform>().anchoredPosition.x - Panels[0].GetComponent<RectTransform>().anchoredPosition.x);
        curnum = 0;
        curCollector = Ability;
        slotState = curCollector.State;
    }

    private void Update()
    {

        if (PlayerInfoManager.instance.BossMode)
            return;

        if ((Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.Joystick1Button7)) && OpenInventory)
        {
            Target.blocksRaycasts = false;
            GoToButton(1);
            OpenInventory = false;
            RightArrowKey = false;
            LeftArrowKey = false;
            GameManager.Instance._inventoryOpen = false;
            PlayerInfoManager.instance.InventorySave();
            Target.alpha = 0;
            if (skills != null)
                skills.StateReturn(MaskSkills.PlayerState.Normal);
        }

        if (Input.GetKeyDown(KeyCode.I))
        {
            if (gateManager.InputKey)
                return;

            ShowInventory();
        }

        if (!OpenInventory)
            return;
        else
        {

            switch (slotState)
            {
                case SlotCollecter.SlotState.Ability:
                    {
                        if (LeftArrowKey && (Input.GetKeyDown(KeyCode.Z) || Input.GetKeyDown(KeyCode.Joystick1Button1)))
                        {
                            curnum = 0;
                            curCollector = Panels[2].GetComponent<SlotCollecter>();
                            slotState = curCollector.State;
                            Selection();
                            GoToButton(3);
                            LeftArrowKey = false;

                        }
                        if (RightArrowKey && (Input.GetKeyDown(KeyCode.Z) || Input.GetKeyDown(KeyCode.Joystick1Button1)))
                        {
                            curnum = 0;
                            curCollector = Panels[1].GetComponent<SlotCollecter>();
                            slotState = curCollector.State;
                            Selection();
                            GoToButton(2);
                            RightArrowKey = false;

                        }


                        if (Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKeyDown(KeyCode.Joystick1Button5))
                        {
                            curCollector.slots[curnum].SelectOff();
                            curCollector.ArrowSlots[0].SelectOff();
                            
                            if (curnum > 3)
                            {
                                curCollector.ArrowSlots[1].SelectOn();
                                RightArrowKey = true;
                            }
                            else if (LeftArrowKey)
                            {

                                curnum = 0;
                                LeftArrowKey = false;
                                Selection();
                            }
                            else
                            {
                                curnum += 4;
                                Selection();
                            }
                        }
                        else if (Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.Joystick1Button4))
                        {
                            curCollector.slots[curnum].SelectOff();
                            curCollector.ArrowSlots[1].SelectOff();
                           
                            if (curnum < 4)
                            {
                                curCollector.ArrowSlots[0].SelectOn();
                                LeftArrowKey = true;
                            }
                            else if (RightArrowKey)
                            {
                                curnum = 4;
                                RightArrowKey = false;
                                Selection();
                            }
                            else
                            {
                                curnum -= 4;
                                Selection();
                            }
                        }
                        else if (Input.GetKeyDown(KeyCode.UpArrow) || Input.GetAxis("Player1_Vertical") == -1)
                        {
                            curCollector.slots[curnum].SelectOff();
                            curnum -= 1;
                            Selection();
                        }
                        else if (Input.GetKeyDown(KeyCode.DownArrow) || Input.GetAxis("Player1_Vertical") == 1)
                        {
                            curCollector.slots[curnum].SelectOff();
                            curnum += 1;
                            Selection();
                        }
                    }
                    break;
                case SlotCollecter.SlotState.Inventory:
                    {
                        if (LeftArrowKey && (Input.GetKeyDown(KeyCode.Z) || Input.GetKeyDown(KeyCode.Joystick1Button1)))
                        {
                            curnum = 0;
                            curCollector = Panels[0].GetComponent<SlotCollecter>();
                            slotState = curCollector.State;
                            Selection();
                            GoToButton(1);
                            LeftArrowKey = false;

                        }
                        if (RightArrowKey && (Input.GetKeyDown(KeyCode.Z) || Input.GetKeyDown(KeyCode.Joystick1Button1)))
                        {
                            curnum = 0;
                            curCollector = Panels[2].GetComponent<SlotCollecter>();
                            slotState = curCollector.State;
                            Selection();
                            GoToButton(3);
                            RightArrowKey = false;

                        }



                        if (Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKeyDown(KeyCode.Joystick1Button5))
                        {
                            curCollector.slots[curnum].SelectOff();
                            curCollector.slots[curnum].NoShowInventoryImageOff();
                            curCollector.ArrowSlots[0].SelectOff();
                            if (curnum == 3 || curnum == 7 || curnum == 11)
                            {
                                curCollector.ArrowSlots[1].SelectOn();
                                RightArrowKey = true;
                            }
                            else if (LeftArrowKey)
                            {

                                curnum = 0;
                                LeftArrowKey = false;
                                Selection();
                            }
                            else
                            {
                                curnum += 1;
                                Selection();
                            }
                        }
                        else if (Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.Joystick1Button4))
                        {
                            curCollector.slots[curnum].SelectOff();
                            curCollector.slots[curnum].NoShowInventoryImageOff();
                            curCollector.ArrowSlots[1].SelectOff();
                            if (curnum == 0 || curnum == 4 || curnum == 8)
                            {
                                curCollector.ArrowSlots[0].SelectOn();
                                LeftArrowKey = true;
                            }
                            else if (RightArrowKey)
                            {
                                curnum = 3;
                                RightArrowKey = false;
                                Selection();
                            }
                            else
                            {
                                curnum -= 1;
                                Selection();
                            }
                        }
                        else if (Input.GetKeyDown(KeyCode.UpArrow) || Input.GetAxis("Player1_Vertical") == -1)
                        {
                            curCollector.slots[curnum].SelectOff();
                            curCollector.slots[curnum].NoShowInventoryImageOff();
                            curnum -= 4;
                            Selection();
                        }
                        else if (Input.GetKeyDown(KeyCode.DownArrow) || Input.GetAxis("Player1_Vertical") == 1)
                        {
                            curCollector.slots[curnum].SelectOff();
                            curCollector.slots[curnum].NoShowInventoryImageOff();
                            curnum += 4;
                            Selection();
                        }
                    }
                    break;
                case SlotCollecter.SlotState.Passive:
                    {

                        if (Input.GetKeyDown(KeyCode.Z) || Input.GetKeyDown(KeyCode.Joystick1Button1))
                        {
                            if (!RightArrowKey && !LeftArrowKey)
                            {
                                curCollector.slots[curnum].EqitPassive();
                            }
                        }


                        if (LeftArrowKey && (Input.GetKeyDown(KeyCode.Z) || Input.GetKeyDown(KeyCode.Joystick1Button1)))
                        {
                            curnum = 0;
                            curCollector = Panels[1].GetComponent<SlotCollecter>();
                            slotState = curCollector.State;
                            Selection();
                            GoToButton(2);
                            LeftArrowKey = false;

                        }
                        if (RightArrowKey && (Input.GetKeyDown(KeyCode.Z) || Input.GetKeyDown(KeyCode.Joystick1Button1)))
                        {
                            curnum = 0;
                            curCollector = Panels[0].GetComponent<SlotCollecter>();
                            slotState = curCollector.State;
                            Selection();
                            GoToButton(1);
                            RightArrowKey = false;

                        }


                        if (Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKeyDown(KeyCode.Joystick1Button5))
                        {
                            curCollector.slots[curnum].SelectOff();
                            curCollector.slots[curnum].NoShowPassiveImage();
                            curCollector.ArrowSlots[0].SelectOff();
                            if (curnum == 2 | curnum == 5 || curnum == 8 || curnum == 11)
                            {
                                curCollector.ArrowSlots[1].SelectOn();
                                RightArrowKey = true;
                            }
                            else if (LeftArrowKey)
                            {

                                curnum = 0;
                                LeftArrowKey = false;
                                Selection();
                            }
                            else
                            {
                                curnum += 1;
                                Selection();
                            }
                        }
                        else if (Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.Joystick1Button4))
                        {
                            curCollector.slots[curnum].SelectOff();
                            curCollector.slots[curnum].NoShowPassiveImage();
                            curCollector.ArrowSlots[1].SelectOff();
                            if (curnum == 0 | curnum == 3 || curnum == 6 || curnum == 9)
                            {
                                curCollector.ArrowSlots[0].SelectOn();
                                LeftArrowKey = true;
                            }
                            else if (RightArrowKey)
                            {
                                curnum = 4;
                                RightArrowKey = false;
                                Selection();
                            }
                            else
                            {
                                curnum -= 1;
                                Selection();
                            }
                        }
                        else if (Input.GetKeyDown(KeyCode.UpArrow) || Input.GetAxis("Player1_Vertical") == -1)
                        {
                            curCollector.slots[curnum].SelectOff();
                            curCollector.slots[curnum].NoShowPassiveImage();
                            curnum -= 3;
                            Selection();
                        }
                        else if (Input.GetKeyDown(KeyCode.DownArrow) || Input.GetAxis("Player1_Vertical") == 1)
                        {
                            curCollector.slots[curnum].SelectOff();
                            curCollector.slots[curnum].NoShowPassiveImage();
                            curnum += 3;
                            Selection();
                        }
                    }
                    break;
            }




            for (int i = 0; i < Panels.Length; i++)
            {
                DistRePosition[i] = center.GetComponent<RectTransform>().position.x - Panels[i].GetComponent<RectTransform>().position.x;
                //Distance[i] = Mathf.Abs(center.transform.position.x - Panels[i].transform.position.x);
                Distance[i] = Mathf.Abs(DistRePosition[i]);

                if (DistRePosition[i] > 71)
                {
                    float curX = Panels[i].GetComponent<RectTransform>().anchoredPosition.x;
                    float curY = Panels[i].GetComponent<RectTransform>().anchoredPosition.y;

                    Vector2 newAnchoredPos = new Vector2(curX + (PanelLenght * PanelDistance), curY);
                    Panels[i].GetComponent<RectTransform>().anchoredPosition = newAnchoredPos;
                }

                if (DistRePosition[i] < -71)
                {
                    float curX = Panels[i].GetComponent<RectTransform>().anchoredPosition.x;
                    float curY = Panels[i].GetComponent<RectTransform>().anchoredPosition.y;

                    Vector2 newAnchoredPos = new Vector2(curX - (PanelLenght * PanelDistance), curY);
                    Panels[i].GetComponent<RectTransform>().anchoredPosition = newAnchoredPos;
                }
            }

            if (TargetNearestButton)
            {
                float minDistnace = Mathf.Min(Distance);

                for (int j = 0; j < Panels.Length; j++)
                {
                    if (minDistnace == Distance[j])
                    {
                        MinPanelNum = j;
                    }
                }
            }
            if (!dragging)
            {
                //LerptoBttn(MinPanelNum * -PanelDistance);
                LerptoBttn(-Panels[MinPanelNum].GetComponent<RectTransform>().anchoredPosition.x);
            }
        }
    }

    private void Selection()
    {
        if (curCollector.slots.Length <= curnum)
            curnum = (curnum - curCollector.slots.Length);
        else if (0 > curnum)
            curnum = (curCollector.slots.Length + curnum);

        switch (slotState)
        {
            case SlotCollecter.SlotState.Ability:
                {
                    curCollector.slots[curnum].SelectOn();
                    curCollector.slots[curnum].ShowAbillity();
                    //curCollector.slots[curnum].Test();
                }
                break;
            case SlotCollecter.SlotState.Inventory:
                {
                    curCollector.slots[curnum].SelectOn();
                    curCollector.slots[curnum].ShowInventoryImage();
                }
                break;
            case SlotCollecter.SlotState.Passive:
                {
                    curCollector.slots[curnum].SelectOn();
                    curCollector.slots[curnum].ShowPassiveImage();
                    curCollector.slots[curnum].EqitSlotCheck();
                }
                break;
        }
    }

    private void LerptoBttn(float pos)
    {
        float newX = Mathf.Lerp(Panel.anchoredPosition.x, pos, Time.deltaTime * 10f);
        Vector2 newPos = new Vector2(newX, Panel.anchoredPosition.y);

        Panel.anchoredPosition = newPos;
    }

    public void StartDarg()
    {
        dragging = true;
        LerpSpeed = 5f;
        TargetNearestButton = true;
    }

    public void EndDrag()
    {
        dragging = false;
    }


    public void GoToButton(int buttonIndex)
    {
        TargetNearestButton = false;
        MinPanelNum = buttonIndex - 1;
    }

    public void ShowInventory()
    {
        StartCoroutine(EnableUI(0.5f));
    }

    IEnumerator EnableUI(float FadeTime)
    {
        if (Target.alpha == 0)
        {
            GameManager.Instance._inventoryOpen = true;
            curnum = 0;
            curCollector = Ability;
            slotState = curCollector.State;
            Selection();
            float alphaVal = Target.alpha;
            while (alphaVal < 1f)
            {
                alphaVal += Time.deltaTime / FadeTime;
                Target.alpha = alphaVal;
                yield return null;

                if(Input.GetKeyDown(KeyCode.Escape))
                {
                    GameManager.Instance._inventoryOpen = false;
                    Target.blocksRaycasts = false;
                    OpenInventory = false;
                    RightArrowKey = false;
                    LeftArrowKey = false;
                    Target.alpha = 0f;
                    if (skills != null)
                        skills.StateReturn(MaskSkills.PlayerState.Normal);
                  
                    yield break;
                }

            }
            Target.alpha = alphaVal;
            Target.blocksRaycasts = true;
            OpenInventory = true;
            if (skills != null)
                skills.Stateabnormality(MaskSkills.PlayerState.Pause);
        }
        else
        {
            float alphaVal = Target.alpha;
            Target.blocksRaycasts = false;
            GoToButton(1);
            OpenInventory = false;
            GameManager.Instance._inventoryOpen = false;
            while (alphaVal > 0f)
            {
                alphaVal -= Time.deltaTime / FadeTime;
                Target.alpha = alphaVal;
                yield return null;

            }
            RightArrowKey = false;
            LeftArrowKey = false;
            PlayerInfoManager.instance.InventorySave();
            Target.alpha = alphaVal;
            if (skills != null)
                skills.StateReturn(MaskSkills.PlayerState.Normal);
        }
    }

    private void DataLoad()
    {
        data = CSVReader.Read("UIdata");
    }

}

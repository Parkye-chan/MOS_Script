using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using MoreMountains.CorgiEngine;
using MoreMountains.Tools;

public class GateManager : MonoBehaviour
{
    public Text[] texts;
    public List<Teleporter> gates = new List<Teleporter>();
    public Transform DestinationGrid;
    public Teleporter curGate = null;

    protected int curVal = 0;
    protected List<GateData> ImgList = new List<GateData>();
    public bool InputKey = false;
    protected MMStateMachine<CharacterStates.CharacterConditions> _condition;
    protected Character _character;
    protected MaskSkills skills;
    protected CanvasGroup canvasGroup;

    private void Start()
    {
        /*
        for (int i = 0; i < gates.Count; i++)
        {

            if (i == 0)
            {
                gates[0].GetComponent<Gate>().gateManager = this.gameObject.GetComponent<GateManager>();
                DefaultText.text = gates[0].GetComponent<Gate>().GateName;
                ImgList.Add(DefaultText.GetComponent<GateData>());
                continue;
            }

            var temp = Instantiate(DefaultText, transform);
            temp.gameObject.name = gates[i].GetComponent<Gate>().GateName;
            temp.gameObject.GetComponent<Text>().text = temp.gameObject.name;
            temp.transform.SetParent(DestinationGrid);
            gates[i].GetComponent<Gate>().gateManager = this.gameObject.GetComponent<GateManager>();
            ImgList.Add(temp.GetComponent<GateData>());
        }
        */
        if (PlayerInfoManager.instance.BossMode)
            return;

        for (int i = 0; i < gates.Count; i++)
        {
            texts[i].text = gates[i].GetComponent<Gate>().GateName;
            ImgList.Add(texts[i].GetComponent<GateData>());
            gates[i].GetComponent<Gate>().gateManager = this.gameObject.GetComponent<GateManager>();
            if (!gates[i].GetComponent<Gate>().GateMeet)
                texts[i].gameObject.SetActive(false);
        }
        //Á¤Áö
        _condition = GameManager.Instance.PersistentCharacter.GetComponent<Character>().ConditionState;
        _character = GameManager.Instance.PersistentCharacter.GetComponent<Character>();
        skills = _character.CharacterModel.GetComponent<MaskSkills>();
        canvasGroup = GetComponent<CanvasGroup>();
    }

    private void Update()
    {
        if(InputKey)
        {
            if (Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKeyDown(KeyCode.Joystick1Button5))
            {
                if (curVal < gates.Count-1)
                    curVal++;
                else
                    return;

                Selection(false);
            }
            else if (Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.Joystick1Button4))
            {
                if (curVal > 0)
                    curVal--;
                else
                    return;

                Selection(true);
            }
            else if (Input.GetKeyDown(KeyCode.Z) || Input.GetKeyDown(KeyCode.Joystick1Button1))                          
              ActiveButton(gates[curVal]);                                  
            else if(Input.GetKeyDown(KeyCode.X) || Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.Joystick1Button0))
            {
                DisablePanel();
            }
        }
    }


    public void EnablePanel(Teleporter gate)
    {
        //if (_condition != null)
        //    _condition.ChangeState(CharacterStates.CharacterConditions.Frozen);
        if (skills != null)
            skills.Stateabnormality(MaskSkills.PlayerState.Pause);

        canvasGroup.alpha = 1;
        curGate = gate;
        InputKey = true;
        Selection();
    }

    public void DisablePanel()
    {
        //if (_condition != null)
        //    _condition.ChangeState(CharacterStates.CharacterConditions.Normal);
        

        InputKey = false;
        //curGate.GetComponent<Gate>().DestinaionOn = false;
        curGate.GetComponent<Teleporter>().Activable = false;
        curGate = null;
        canvasGroup.alpha = 0;
        curVal = 0;
        StartCoroutine(PauseReturn());
        //Selection();
    }

    IEnumerator PauseReturn()
    {

        yield return new WaitForSeconds(1.5f);

        if (skills != null)
            skills.StateReturn(MaskSkills.PlayerState.Normal);
    }

    private void Selection(bool left)
    {
        if(!texts[curVal].gameObject.activeSelf && !left)
        {
            curVal++;
            if (curVal >= gates.Count)
                curVal = 0;

            Selection(left);
            return;
        }
        else if (!texts[curVal].gameObject.activeSelf && left)
        {
            curVal--;
            if (curVal < 0)
                curVal = gates.Count-1;

            Selection(left);
            return;
        }

        for (int i = 0; i < ImgList.Count; i++)
        {
            ImgList[i].ChoiceImg.GetComponent<Image>().color = new Color(255f / 255f, 255f / 255f, 255f / 255f, 0);
        }
        ImgList[curVal].ChoiceImg.GetComponent<Image>().color = new Color(255f / 255f, 255f / 255f, 255f / 255f , 1);
        curGate.GetComponent<RespawnerCheck>().Targetroom = gates[curVal].GetComponent<RespawnerCheck>().curRoom;
        curGate.GetComponent<RespawnerCheck>().TargetPlatform = gates[curVal].GetComponent<RespawnerCheck>().CurRoomPlatform;

    }

    private void Selection()
    {
        for (int i = 0; i < ImgList.Count; i++)
        {
            ImgList[i].ChoiceImg.GetComponent<Image>().color = new Color(255f / 255f, 255f / 255f, 255f / 255f, 0);
        }
        ImgList[curVal].ChoiceImg.GetComponent<Image>().color = new Color(255f / 255f, 255f / 255f, 255f / 255f, 1);
        curGate.GetComponent<RespawnerCheck>().Targetroom = gates[curVal].GetComponent<RespawnerCheck>().curRoom;
        curGate.GetComponent<RespawnerCheck>().TargetPlatform = gates[curVal].GetComponent<RespawnerCheck>().CurRoomPlatform;

    }



    public void ActiveButton(Teleporter TargetGate)
    {
        if (curGate == gates[curVal])
        {
            Debug.Log("plase othe Target");
            return;
        }
        curGate.GetComponent<Teleporter>().Activable = true;
        curGate.GetComponent<RespawnerCheck>().GateSpawnerInit();
        curGate.Destination = TargetGate;
        curGate.TargetRoom = TargetGate.CurrentRoom;
        //curGate.GetComponent<RespawnerCheck>().TargetPlatform = TargetGate.GetComponent<RespawnerCheck>().CurRoomPlatform;
        curGate.GetComponent<Gate>().DestinaionOn = true;
        curGate.GetComponent<Teleporter>().TriggerButtonAction(_character.gameObject);
        DisablePanel();
    }

    public void GateUpdate()
    {
        for (int i = 0; i < gates.Count; i++)
        {
            if (gates[i].GetComponent<Gate>().GateMeet)
                texts[i].gameObject.SetActive(true);
        }
    }


}

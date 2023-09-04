using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogueManager : MonoBehaviour
{
    public static DialogueManager instance;

    [SerializeField]
    public Text text;
    public Text Nametext;
    [SerializeField]
    public GameObject TalkPanel;
    public GameObject curNPC;
    public int talkIndex;
    public GameObject Player;

    Dictionary<int, string[]> talkData = new Dictionary<int,string[]>();

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(this);

        InitDialog();
    }

    private void InitDialog()
    {
        /*
        talkData.Add(1000, new string[] { "예전엔 이 마을이 시끌벅적했는데 지금은 너무 고요해.. 섬뜩할 정도로. :0","이런 깜깜한 밤이 언제 동안 계속될 것 같아? 이제 아침도 잊어버렸어. :1"
            , "저 숲은 원래 저렇게 크지 않았어. 갑자기 어느 순간 커져 있었거든. 보통 사람들에겐 너무나도 위험한 장소가 되어버렸어. :2"
            , "숲 쪽에 이상한 기운이 느껴지니까.. 가던지 말던지. 조심해.:3" });
        talkData.Add(2000, new string[] { "못 보던 얼굴이군, 넌 누구지? 나와 같은 떠돌이 검사인가? :0", "이곳은 정말 이상해... 계속 밤이 지속되고 있어. 분명 어떤 사악한 일이 일어나고 있을 거다.:1", "저 멀리에서 차가운 바람이 불어오고 있는 게 느껴져. 아마 저 멀리에서 보이는 설산 때문인 것 같군.:2", "이상한 일이야... 다른 곳은 전혀 춥지 않은데.:3"
            , });

        talkData.Add(3000, new string[] { "마을을 나갈 거라면 조심하시오. 밖의 분위기가 예전과는 많이 달라졌소. 짐승들이 사납게 달려들 거요.:0"
            ,"미물들마저 사악한 요괴로 변모하다니.. 참으로 무서운 일이오. :1", "땅 아래에서 불길한 기운이 느껴지고 있소. 무엇인가 이질적이고 아주 불쾌한 기운이오. :2"," 아래로 내려간다면 준비를 단단히 하고 가야 할 거요. 조심하시오. :3", });

        talkData.Add(4000, new string[] { "마을에 사람이라곤 없네. 장사가 안 되는 것도 당연한 일이지.:0", "이곳을 떠나고 싶지만, 다른 곳도 마찬가지인 것 같아. :1", "이곳 어딘가에 왕묘가 있다고 들었어. 오래 전에 이 지역을 통치한 왕의 무덤이지. :3", "그 곳에는 보물이 정말 많을 텐데... :4" });
        talkData.Add(5000, new string[] { "이런 곳에서 사람을 만날 줄은 몰랐군.:0", "여기까지 왔으니 알겠지만, 여긴 정말 위험하다.:1", "이 곳의 동물들은 정말 이상해. 어디서 본 것 같은데 내가 기억하는 것과는 달라.:2", "확실한 건 그것들이 절대 우호적인 건 아니라는 거지.:3" });
        */
        talkData.Add(1000, new string[] { "호걸의 가면을 얻었구나! :0","C키를 누르면 새로운 힘을 사용할수 있어! :1" });
        talkData.Add(2000, new string[] { "영노의 가면을 얻었구나! :0", "이제 공중도약을 사용할수 있겠군! :1" });

        talkData.Add(3000, new string[] { "염왕의 가면을 얻었구나! :0"," 이제 벽을 올라 갈 수 있겠구나 :1"});

        talkData.Add(4000, new string[] { "마을을 나서기 전에 알아 둘 것 : 0","Z는 공격 X는 원거리 공격 V는 포션 사용 스페이스는 점프 : 1","차지 공격은 Z나 X를 누르고 유지하기 : 2","I 는 인벤토리 인벤토리에서 Z는 선택 ESC는 일시정지 및 나가기 상호작용은 위쪽 방향키 : 3","마지막으로 숨겨진 힘을 얻는다면 A키로 바꿀수 있어 : 4"
        ,"힌트는 여기까지! 행운을 빈다!"});
        talkData.Add(5000, new string[] { "암흑서리 의 가면을 얻었구나! :0","어이어이 믿고 있었다고! :1" ,"대쉬 도중 정확한 타이밍에 C를 한번 더 눌러봐 :2", "뭔가가 무너지는 소리가 났어 그곳이 마지막 싸움이 있는 곳일지도.... : 3","행운을 빈다 :4" });

        talkData.Add(6000, new string[] { "그리하여 세상의 모든 가면을 모은 청오차사는 또 다시 강림도령의 부름을 받게 되었다..:0" });
    }

    public void Talk()
    {
        if(curNPC)
        {
            TalkPanel.SetActive(true);
            NPCData datas = curNPC.GetComponent<NPCData>();
            string sentence = Translation(datas.Num,talkIndex);
            if(sentence == null)
            {
                talkIndex = 0;
                NPCInteraction(datas.Num);
                Player.GetComponentInChildren<MaskSkills>().MeetNPC = false;
                Player.GetComponentInChildren<MaskSkills>().StateReturn(MaskSkills.PlayerState.Normal);
                return;
            }
            if (datas.NPC)
            {
                text.text = sentence.Split(':')[0];
                Nametext.text = datas.NPCName;
            }
            else if(!datas.NPC)
            {
                text.text = sentence.Split(':')[0];
                Nametext.text = datas.NPCName;
            }

            talkIndex++;
        }        
    }

    private void NPCInteraction(int val)
    {
        if (val == 1000)
        {
            TalkPanel.SetActive(false);
         
        }
        else if(val == 2000)
        {
            TalkPanel.SetActive(false);

        }
        else
        {
            return;
        }

    }


    private string Translation(int id, int talkIndex)
    {
        if (talkIndex == talkData[id].Length)
            return null;
        else
            return talkData[id][talkIndex];
    }
}

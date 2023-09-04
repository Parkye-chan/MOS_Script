using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MoreMountains.CorgiEngine;
using MoreMountains.Tools;

public class BossZone : MonoBehaviour
{

    public GameObject BossObject;
    public GameObject[] Door;
    public GameObject[] BossProps;
    public GameObject SpawnNPC;
    public GameObject SpawnObj;
    public GameObject SpawnMask;
    public Slot MaskSlot;
    public bool Clear = false;
    public float WaitTime = 4.0f;
    public BackgroundMusic backgroundMusic;
    public int BossBGMNum = 6;
    private bool BossVisit = false;
    private GameObject Player;

    MMStateMachine<CharacterStates.CharacterConditions> _condition;
    MMStateMachine<CharacterStates.MovementStates> _movement;

    private void Start()
    {
        if (Clear)
        {
            BossObject.SetActive(false);
            if (BossProps.Length != 0)
            {
                for (int i = 0; i < BossProps.Length; i++)
                {
                    BossProps[i].SetActive(false);
                }
            }

            if (SpawnNPC)
            {
                SpawnNPC.SetActive(true);

            }

            if (SpawnObj)
                SpawnObj.SetActive(true);

            if(MaskSlot)
            {
                if(!MaskSlot.isGet)
                {
                    SpawnMask.SetActive(true);
                }
            }
        }
    }

    public void ClearCheck()
    {
        if (Clear)
        {
            BossObject.SetActive(false);
            if (BossProps.Length != 0)
            {
                for (int i = 0; i < BossProps.Length; i++)
                {
                    BossProps[i].SetActive(false);
                }
            }
            if (SpawnNPC)
            {
                SpawnNPC.SetActive(true);
                
            }
            if (SpawnObj)
                SpawnObj.SetActive(true);

            if (MaskSlot)
            {
                if (!MaskSlot.isGet)
                {
                    SpawnMask.SetActive(true);
                }
            }
        }
    }

    private void BattleStart(Collider2D Player)
    {
        if (BossObject.GetComponent<ForestBoss>())
        {
            BossObject.GetComponent<ForestBoss>().Target = Player.transform;
            //backgroundMusic.ChangeMusic(BossBGMNum);
        }
        else if (BossObject.GetComponent<MoleRat>())
        {
            BossObject.GetComponent<MoleRat>().Target = Player.transform;
        }
        else if (BossObject.GetComponent<BigSnake>())
        {
            BossObject.GetComponent<BigSnake>().Target = Player.transform;
            //backgroundMusic.ChangeMusic(BossBGMNum);
        }
        else if (BossObject.GetComponent<FireKing>())
        {
            BossObject.GetComponent<FireKing>().Target = Player.transform;
            //backgroundMusic.ChangeMusic(BossBGMNum);
        }
        else if (BossObject.GetComponent<DarkFrost>())
        {
            BossObject.GetComponent<DarkFrost>().Target = Player.transform;
            // backgroundMusic.ChangeMusic(BossBGMNum);
        }
        else if (BossObject.GetComponent<RedReaper>())
        {
            BossObject.GetComponent<RedReaper>().Target = Player.transform;
            // backgroundMusic.ChangeMusic(BossBGMNum);
        }
        backgroundMusic.ChangeMusic(BossBGMNum);
        BossObject.SetActive(true);
        SpawnDoor();
    }

    IEnumerator CutSeanePause(Collider2D Player)
    {
        Player.GetComponentInChildren<MaskSkills>().Stateabnormality(MaskSkills.PlayerState.Pause);

        yield return new WaitForSeconds(WaitTime);

        Player.GetComponentInChildren<MaskSkills>().StateReturn(MaskSkills.PlayerState.Normal);
    }

    public void SpawnDoor()
    {
        if (Door.Length == 0)
            return;

        for (int i = 0; i < Door.Length; i++)
        {
            Door[i].SetActive(true);
        }

        backgroundMusic.ChangeMusic(BossBGMNum);
    }

    public void DisappearDoor()
    {
        for (int i = 0; i < Door.Length; i++)
        {
            Door[i].GetComponent<Animator>().SetBool("Spawn", false);
        }
        /*
        if (BossObject.GetComponent<ForestBoss>())
        {
            backgroundMusic.StopSound();
        }
        else if (BossObject.GetComponent<BigSnake>())
        {
            backgroundMusic.StopSound();
        }
        else if(BossObject.GetComponent<FireKing>())
        {
            backgroundMusic.StopSound();
        }
        else if (BossObject.GetComponent<DarkFrost>())
        {
            backgroundMusic.StopSound();
        }
        else if(BossObject.GetComponent<RedReaper>())
        {
            backgroundMusic.StopSound();
        }
        else if(BossObject.GetComponent<SuperRedReaper>())
        {
            backgroundMusic.StopSound();
        }*/
        backgroundMusic.StopSound();
        Clear = true;

        if (BossProps.Length != 0)
        {
            for (int i = 0; i < BossProps.Length; i++)
            {
                BossProps[i].SetActive(false);
            }
        }

        if (SpawnNPC)
        {
            SpawnNPC.SetActive(true);
            DialogueManager.instance.curNPC = SpawnNPC.gameObject;
            Player.GetComponentInChildren<MaskSkills>().MeetNPC = true;
            DialogueManager.instance.Talk();
        }

        if (SpawnObj)
            SpawnObj.SetActive(true);

        PlayerInfoManager.instance.StageSave();
        }

    private void OnTriggerEnter2D(Collider2D collision)
    {

        if (Clear && collision.CompareTag("Player"))
            return;
        else if (!Clear && collision.CompareTag("Player"))
        {
            if (BossVisit == false && collision.CompareTag("Player"))
            {
                BossVisit = true;
                DialogueManager.instance.Player = collision.gameObject;
                BattleStart(collision);
                Player = collision.gameObject;
                StartCoroutine(CutSeanePause(collision));
            }
        }
    }

}

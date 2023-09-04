using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCData : MonoBehaviour
{
    [SerializeField]
    public int Num;
    [SerializeField]
    public bool NPC;
    public string NPCName;


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && NPC)
        {
            DialogueManager.instance.curNPC = this.gameObject;
            DialogueManager.instance.Player = collision.gameObject;
            collision.GetComponentInChildren<MaskSkills>().MeetNPC = true;
        }
        else if(collision.CompareTag("Player") && !NPC)
        {
            DialogueManager.instance.curNPC = this.gameObject;
            DialogueManager.instance.Player = collision.gameObject;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && NPC)
        {
            DialogueManager.instance.curNPC = null;           
            collision.GetComponentInChildren<MaskSkills>().MeetNPC = false;
            
            if(DialogueManager.instance.TalkPanel.activeSelf)
            {
                DialogueManager.instance.TalkPanel.SetActive(false);
                DialogueManager.instance.talkIndex = 0;
                collision.GetComponentInChildren<MaskSkills>().StateReturn(MaskSkills.PlayerState.Normal);
            }

        }
        else if(collision.CompareTag("Player") && !NPC)
        {
            DialogueManager.instance.curNPC = null;
            //collision.GetComponentInChildren<MaskSkills>().MeetNPC = false;            
        }
    }

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Statusabnormality : MonoBehaviour
{

    public MaskSkills.PlayerState state;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Player"))
        {
            MaskSkills skills = collision.GetComponentInChildren<MaskSkills>();
            skills.Stateabnormality(state);
        }
    }
}

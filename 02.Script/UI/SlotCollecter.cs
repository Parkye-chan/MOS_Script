using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlotCollecter : MonoBehaviour
{
    public GameObject SlotTransform;
    public Slot[] slots;
    public Slot[] ArrowSlots;
    public SlotState State;

    public enum SlotState
    {
        Ability,
        Inventory,
        Passive
    }


    private void Start()
    {
        slots = SlotTransform.GetComponentsInChildren<Slot>();
    }
}


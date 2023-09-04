using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackEffect : MonoBehaviour
{
    

    public void AutoDestroy()
    {
        Destroy(this.gameObject);
    }
}

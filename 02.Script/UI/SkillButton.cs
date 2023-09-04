using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SkillButton : MonoBehaviour
{
    [SerializeField]
    public Transform skillimage;

    private void Start()
    {
        skillimage = this.transform.Find("SkillImg");
    }
}

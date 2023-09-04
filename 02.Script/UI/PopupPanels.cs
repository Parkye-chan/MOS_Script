using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PopupPanels : MonoBehaviour
{

    public int PosVal;
    public string skillname;

    public void Closethis()
    {
        this.gameObject.SetActive(false);
    }
}

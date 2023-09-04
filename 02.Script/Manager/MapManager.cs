using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapManager : MonoBehaviour
{

    
    public GameObject MapPanel;

    private bool isOpen = false;
    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.M))
        {
            PanelOn();
        }
    }


    private void PanelOn()
    {
        if (!isOpen)
        {
            MapPanel.SetActive(true);
            isOpen = true;
        }
        else
        {
            MapPanel.SetActive(false);
            isOpen = false;
        }
    }
}

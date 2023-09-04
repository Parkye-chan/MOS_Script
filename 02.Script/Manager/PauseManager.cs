using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MoreMountains.Tools;

public class PauseManager : MonoBehaviour
{


    public GameObject[] Btn;

    private int curVal = 0;

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.UpArrow) || (Input.GetAxis("Player1_Vertical") == -1))
        {
            if (curVal == Btn.Length-1)
                curVal = 0;
            else if (curVal <= 0)
                curVal = 0;

            curVal--;

            if(curVal <= 0)
                curVal = 0;
        }
        else if (Input.GetKeyDown(KeyCode.DownArrow) || (Input.GetAxis("Player1_Vertical") == 1))
        {
            
           
            curVal++;

            if (curVal >= Btn.Length)
                curVal = Btn.Length - 1;
        }
        else if(Input.GetKeyDown(KeyCode.Z))
        {
            Select();
        }
    }

    private void Select()
    {
        Btn[curVal].GetComponent<MMTouchButton>().ButtonPressedFirstTime.Invoke();
    }
}

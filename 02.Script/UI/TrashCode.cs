using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MoreMountains.CorgiEngine;

public class TrashCode : MonoBehaviour
{
    public StartScreen startScreen;


    public void StartButton()
    {
        string Key = DataManager.NewGameLoad();
        if (Key == "NotNewbie")
        {
            startScreen.ButtonPressed();
        }
        else
        {
            DataManager.NewGameSave();
            startScreen.StartButtonPressed();
        }
    }
}

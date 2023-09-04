using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;
using UnityEngine.UI;
using MoreMountains.CorgiEngine;
using UnityEngine.SceneManagement;

public class MovieSceneManager : MonoBehaviour
{

    public VideoPlayer vp;
    public Text skiltxt;
    public StartScreen startScreen;
    public string SceneName;
    public bool IntroScene = true;
    public bool CanSkip = true;
    public int LoadSceneNum;
    private int ClickCnt = 0;
    // Update is called once per frame
    void Update()
    {
        if (vp.time > 0)
        {
            if (!vp.isPlaying && !IntroScene)
                startScreen.ButtonPressed();
            else if(!vp.isPlaying && IntroScene)
                SceneManager.LoadScene(LoadSceneNum, LoadSceneMode.Single);
        }

        if (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.Return))
            SkipProcess();

    }


    private void SkipProcess()
    {
        if (!CanSkip)
            return;
        /* ClickCnt++;
         if (ClickCnt == 3)
             skiltxt.gameObject.SetActive(true);
         else if (ClickCnt > 3)*/
        if(!IntroScene)
            startScreen.ButtonPressed();
        else
            SceneManager.LoadScene(LoadSceneNum, LoadSceneMode.Single);
    }

}

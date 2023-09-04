using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ScenesManager : MonoBehaviour
{

    public static ScenesManager instance;


    public IDictionary<string, LoadSceneMode> loadScens = new Dictionary<string, LoadSceneMode>();

    // Start is called before the first frame update
    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);

        DontDestroyOnLoad(this);

        Init();
    }

    private void Init()
    {
        loadScens.Add("MainMenu", LoadSceneMode.Single);
        //loadScens.Add("PlayScene", LoadSceneMode.Single);
    }

    public void MoveScene(int i)
    {

        if (!Application.CanStreamedLevelBeLoaded(i))
        {
            Debug.Log("No Scene");
            return;
        }

            SceneManager.LoadScene(i, LoadSceneMode.Single);
    }

    public AsyncOperation GiveSceneInfo(string SceneName)
    {
        AsyncOperation op = SceneManager.LoadSceneAsync(SceneName);

        return op;
    }

}

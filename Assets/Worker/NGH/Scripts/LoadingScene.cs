using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadingScene : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        DataManager.Instance.OnLoadCompleted += GameStart;
    }

    private void OnDisable()
    {
        DataManager.Instance.OnLoadCompleted -= GameStart;
    }

    public void GameStart()
    {
        SceneManager.LoadScene("LobbyTest");
    }
}

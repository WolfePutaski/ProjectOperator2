using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SC_GameSceneManager : MonoBehaviour
{

    public static SC_GameSceneManager sharedInstance;

    void Awake()
    {
        DontDestroyOnLoad(gameObject);

        if (sharedInstance == null)
        {
            sharedInstance = this;
        }
        else
        {
            Destroy(sharedInstance.gameObject);
            sharedInstance = this;
        }
    }

    public void RestartScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void GoToScene(string name)
    {
        SceneManager.LoadScene(name);
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}

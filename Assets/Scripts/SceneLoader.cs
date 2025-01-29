using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class SceneLoader : MonoBehaviour
{
    void Start()
    {
        if (ClientManager.Instance != null)
        {
            ClientManager.Instance.RefreshUIReferences();
        }
    }

    public void LoadScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }

    public void LoadMainScene()
    {        
        SceneManager.LoadScene("MainMenuScene");      
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;
using TMPro;

public class TransitionLoader : MonoBehaviour
{
    public TextMeshPro screenText;

    void Start()
    {
        ClientManager.Instance.OnMessageReceived += HandleMessageReceived;
    }

    void OnDestroy()
    {
        if (ClientManager.Instance != null)
            ClientManager.Instance.OnMessageReceived -= HandleMessageReceived;
    }
    private void HandleMessageReceived(Message message)
    {
        if (message.type == "START_VR")
        {
            screenText.text = "Lancement ...";
            LoadScene("MainScene");
        }
    }


    public void LoadScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }
}

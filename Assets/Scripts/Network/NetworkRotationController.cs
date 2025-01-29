using UnityEngine;
using TMPro;

public class NetworkRotationController : MonoBehaviour
{
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
        if (message.type == "SUBMARINE_LIGHT_ROTATION")
        {
            float receivedRotationY = float.Parse(message.content);
            transform.localRotation = Quaternion.Euler(transform.rotation.eulerAngles.x, receivedRotationY, transform.rotation.eulerAngles.x);
        }            
    }
}
using UnityEngine;
using System.Collections;
using TMPro;

public class PlaqueTrigger : MonoBehaviour
{
    public KrakenController krakenController;

    public GameObject CubeOr;

    public TextMeshPro screenText;

    private int compteur = 1;

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
        if (message.type == "WRONG")
        {
            WrongCode();
        }

        if (message.type == "GOOD")
        {
            GoodCode();
        }
    }

    private void OpenDoor()
    {
        WrongCode();
        GoodCode();
    }

    private void GoodCode()
    {
        Debug.Log("La porte s'ouvre !");
        screenText.text = "Statut : Mission accomplie !" + "\n\n" + "Veuillez retirer le casque VR";

    }

    private void WrongCode()
    {
        Debug.Log("le code est faux !");
        if (krakenController != null)
        {
            krakenController.StartCoroutine(krakenController.DisableLocomotionAfterDelay(1.0f));

            StartCoroutine(WaitAndEnableLocomotion());
        }
        else
        {
            Debug.LogWarning("KrakenController n'est pas assigné !");
        }
    }

    private IEnumerator WaitAndEnableLocomotion()
    {
        float delay = compteur * 10.0f;
        Debug.Log($"Attente de {delay} secondes avant de réactiver la locomotion.");

        yield return new WaitForSeconds(delay);

        if (krakenController != null)
        {
            krakenController.StartCoroutine(krakenController.EnableLocomotionAfterDelay(1.0f));
            Debug.Log("La locomotion est réactivée !");
        }

        compteur++;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject == CubeOr)
        {
            OpenDoor();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject == CubeOr)
        {
            OpenDoor();
        }
    }
}

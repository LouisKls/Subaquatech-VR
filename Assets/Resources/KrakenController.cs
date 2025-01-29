using UnityEngine.XR.Interaction.Toolkit.Interactables;
using UnityEngine.XR.Interaction.Toolkit.Locomotion.Teleportation;
using UnityEngine.XR.Interaction.Toolkit.Transformers;
using UnityEngine;
using System.Collections;

public class KrakenController : MonoBehaviour
{
    public AudioSource audioSource;
    public CharacterController characterController;
    public TeleportationProvider teleportationProvider;

    public GameObject prefabToSpawn;
    public Transform spawnLocation;

    public XRGrabInteractable cubeOrGrab;
    private XRGeneralGrabTransformer grabTransformer;

    private bool isLocomotionDisabled = false;
    private GameObject spawnedPrefab;

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
        if (message.type == "ADMIN_RELEASE_KRAKEN")
        {
            StartCoroutine(DisableLocomotionAfterDelay(1.0f));
        }

        if (message.type == "KILL_KRAKEN")
        {
            StartCoroutine(EnableLocomotionAfterDelay(1.0f));
        }
    }

    public IEnumerator DisableLocomotionAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);

        // Empêche la création d'un second Kraken si l'ancien existe encore
        if (spawnedPrefab != null)
        {
            Debug.LogWarning("Un Kraken est déjà présent. Attente de sa disparition.");
            yield break;
        }

        if (audioSource != null)
        {
            audioSource.Play();
        }

        if (prefabToSpawn != null && spawnLocation != null)
        {
            Vector3 newPosition = spawnLocation.position + new Vector3(0.9f, 0, 1.0f);
            spawnedPrefab = Instantiate(prefabToSpawn, newPosition, Quaternion.identity);
            Debug.Log("Prefab apparu au niveau du transform spécifié.");
        }

        // Désactive le cube
        if (cubeOrGrab != null)
        {
            cubeOrGrab.gameObject.SetActive(false);
            Debug.Log("Cube désactivé !");
        }

        DisableLocomotion();
    }

    private void DisableLocomotion()
    {
        if (characterController != null)
        {
            characterController.enabled = false;
            Debug.Log("Character Controller désactivé !");
        }

        if (teleportationProvider != null)
        {
            teleportationProvider.enabled = false;
            Debug.Log("Téléportation désactivée !");
        }

        isLocomotionDisabled = true;
    }

    private void TriggerAnimators(GameObject spawnedPrefab, string trigger)
    {
        Animator[] animators = spawnedPrefab.GetComponentsInChildren<Animator>();

        foreach (Animator childAnimator in animators)
        {
            if (childAnimator != null)
            {
                childAnimator.SetTrigger(trigger);
                Debug.Log($"Animation déclenchée pour {childAnimator.gameObject.name}");
            }
        }
    }

    public IEnumerator EnableLocomotionAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);

        if (spawnedPrefab != null)
        {
            TriggerAnimators(spawnedPrefab, "RETREAT");
            yield return new WaitForSeconds(2.0f); // Attente pour la fin de l'animation
            Destroy(spawnedPrefab);
            Debug.Log("Kraken détruit après l'animation RETREAT.");
            spawnedPrefab = null; // Réinitialise la référence pour permettre un nouvel spawn
        }

        // Réactive le cube
        if (cubeOrGrab != null)
        {
            cubeOrGrab.gameObject.SetActive(true);
            Debug.Log("Cube réactivé !");
        }

        EnableLocomotion();
    }

    private void EnableLocomotion()
    {
        if (characterController != null)
        {
            characterController.enabled = true;
            Debug.Log("Character Controller réactivé !");
        }

        if (teleportationProvider != null)
        {
            teleportationProvider.enabled = true;
            Debug.Log("Téléportation réactivée !");
        }

        isLocomotionDisabled = false;
        Debug.Log("Locomotion réactivée !");
    }
}
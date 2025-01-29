using UnityEngine;
using UnityEngine.InputSystem; // Important pour le nouvel input system
using UnityEngine.InputSystem.XR;

public class ShowMenuOnStickPress : MonoBehaviour
{
    [Header("Input Actions")]
    [Tooltip("Référence à l'action d'entrée pour la pression du joystick gauche.")]
    public InputActionProperty leftStickPress;

    [Header("Menu Settings")]
    [Tooltip("Le GameObject représentant le menu à afficher/masquer.")]
    public GameObject menu;

    [Tooltip("Si vrai, le menu disparait quand on relâche le stick.")]
    public bool hideOnRelease = false;

    private void OnEnable()
    {
        // S'abonner aux événements "performed" (appui) et "canceled" (relâchement)
        if (leftStickPress != null && leftStickPress.action != null)
        {
            leftStickPress.action.performed += OnStickPressed;
            leftStickPress.action.canceled += OnStickReleased;
        }
    }

    private void OnDisable()
    {
        if (leftStickPress != null && leftStickPress.action != null)
        {
            leftStickPress.action.performed -= OnStickPressed;
            leftStickPress.action.canceled -= OnStickReleased;
        }
    }

    private void OnStickPressed(InputAction.CallbackContext context)
    {
        Debug.Log("BUTTON CLICKED");
        // Afficher le menu
        if (menu != null)
        {
            menu.SetActive(true);
        }
    }

    private void OnStickReleased(InputAction.CallbackContext context)
    {
        Debug.Log("BUTTON RELEASED");
        // Optionnel : masquer le menu si souhaité
        if (hideOnRelease && menu != null)
        {
            menu.SetActive(false);
        }
    }
}

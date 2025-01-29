using UnityEngine;
using UnityEngine.InputSystem; // Important pour le nouvel input system
using UnityEngine.InputSystem.XR;

public class ShowMenuOnStickPress : MonoBehaviour
{
    [Header("Input Actions")]
    [Tooltip("R�f�rence � l'action d'entr�e pour la pression du joystick gauche.")]
    public InputActionProperty leftStickPress;

    [Header("Menu Settings")]
    [Tooltip("Le GameObject repr�sentant le menu � afficher/masquer.")]
    public GameObject menu;

    [Tooltip("Si vrai, le menu disparait quand on rel�che le stick.")]
    public bool hideOnRelease = false;

    private void OnEnable()
    {
        // S'abonner aux �v�nements "performed" (appui) et "canceled" (rel�chement)
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
        // Optionnel : masquer le menu si souhait�
        if (hideOnRelease && menu != null)
        {
            menu.SetActive(false);
        }
    }
}

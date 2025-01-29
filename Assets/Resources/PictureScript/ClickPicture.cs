using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;
using UnityEngine.InputSystem; // Pour le New Input System

[RequireComponent(typeof(XRGrabInteractable))]
[RequireComponent(typeof(SavePicture))]
public class ClickPicture : MonoBehaviour
{
    private XRGrabInteractable interactable;
    private SavePicture savePicture;

    [Header("Input Actions (New Input System)")]
    [SerializeField] private InputActionReference placeAction;    // Existant

    public PhoneManager phoneManager;

    private bool isPhoneGrab = false;

    private void OnEnable()
    {
        if (placeAction?.action != null)
            placeAction.action.performed += OnPlacePerformed;

        placeAction?.action?.Enable();
    }

    private void OnDisable()
    {
        if (placeAction?.action != null)
            placeAction.action.performed -= OnPlacePerformed;

        placeAction?.action?.Disable();
    }

    private void Start()
    {
        interactable = GetComponent<XRGrabInteractable>();
        savePicture = GetComponent<SavePicture>();

        interactable.selectExited.AddListener(PhoneDropped);
        interactable.selectEntered.AddListener(PhoneGrab);
    }

    private void OnPlacePerformed(InputAction.CallbackContext ctx)
    {
        if (isPhoneGrab)
        {
            savePicture.TakePicture();
            Debug.Log("took picture");
        }    
    }

    private void PhoneGrab(SelectEnterEventArgs arg0)
    {
        phoneManager.OnSelfieModeEnter();
    }
    
    private void PhoneDropped(SelectExitEventArgs arg0)
    {
        phoneManager.OnSelfieModeExit();
    }
}
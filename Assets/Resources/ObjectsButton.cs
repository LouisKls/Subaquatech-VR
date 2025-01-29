using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectsButton : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private GameObject objectsMenu;

    [Header("Sounds")]
    public AudioSource audioSource;
    public AudioClip soundClip;

    public void OnClick()
    {
        objectsMenu.SetActive(!objectsMenu.activeSelf);
        audioSource.PlayOneShot(soundClip);
    }
}

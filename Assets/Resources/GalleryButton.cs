using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GalleryButton : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private GameObject galleryMenu;

    [Header("Sounds")]
    public AudioSource audioSource;
    public AudioClip soundClip;

    public void OnClick()
    {
        galleryMenu.SetActive(!galleryMenu.activeSelf);
        audioSource.PlayOneShot(soundClip);
    }
}

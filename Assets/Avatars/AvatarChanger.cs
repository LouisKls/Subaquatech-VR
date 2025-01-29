using UnityEngine;

public class AvatarChanger : MonoBehaviour
{
    [Header("Liste des prefabs d'avatars (Humanoid)")]
    public GameObject[] avatarPrefabs;

    [Header("Où instancier l'avatar ?")]
    public Transform avatarParent;  // ex. un GO enfant du XR Origin

    [Header("Références VR (scène)")]
    public Transform xrCamera;      // Main Camera (du XR Origin)
    public Transform leftController;
    public Transform rightController;

    private GameObject currentAvatar;
    private int currentIndex = 0;

    void Start()
    {
        if (avatarPrefabs.Length > 0)
        {
            currentIndex = 0;
            InstantiateAvatar(currentIndex);
        }
    }

    public void NextAvatar()
    {
        if (avatarPrefabs.Length == 0) return;

        DestroyCurrentAvatar();
        currentIndex = (currentIndex + 1) % avatarPrefabs.Length;
        InstantiateAvatar(currentIndex);
    }

    public void PreviousAvatar()
    {
        if (avatarPrefabs.Length == 0) return;

        DestroyCurrentAvatar();
        currentIndex--;
        if (currentIndex < 0)
            currentIndex = avatarPrefabs.Length - 1;
        InstantiateAvatar(currentIndex);
    }

    private void InstantiateAvatar(int index)
    {
        currentAvatar = Instantiate(
            avatarPrefabs[index],
            avatarParent.position,
            avatarParent.rotation,
            avatarParent
        );
    }

    private void DestroyCurrentAvatar()
    {
        if (currentAvatar != null)
        {
            Destroy(currentAvatar);
            currentAvatar = null;
        }
    }
}

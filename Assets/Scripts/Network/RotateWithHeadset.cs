using UnityEngine;

public class RotateWithHeadset : MonoBehaviour
{
    [SerializeField] private Transform vrCamera;
    // (Assigne la "Main Camera" VR ici, par glisser-d�poser dans l'Inspector)

    void LateUpdate()
    {
        // On r�cup�re l'angle Y du casque
        float targetY = vrCamera.eulerAngles.y;

        // On applique cet angle sur le Y du personnage, en gardant X=0 et Z=0
        transform.rotation = Quaternion.Euler(0, targetY, 0);
    }
}

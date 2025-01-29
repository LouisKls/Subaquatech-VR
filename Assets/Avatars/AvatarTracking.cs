using UnityEngine;

public class AvatarTracking : MonoBehaviour
{
    [Header("Bones de l'avatar (assign�s dans le prefab)")]
    public Transform headBone;
    public Transform leftHandBone;
    public Transform rightHandBone;

    [Header("R�f�rences VR (assign�es apr�s l'Instantiate)")]
    public Transform vrHead;             // Cam�ra VR
    public Transform vrLeftController;   // Contr�leur gauche
    public Transform vrRightController;  // Contr�leur droit

    // Param�tres de position/rotation (offset) si besoin
    // par exemple pour ajuster la taille ou le placement
    public Vector3 headOffsetPosition;
    public Vector3 handOffsetPosition;

    void Update()
    {
        // S�curit� : si on n'a pas encore de r�f�rences VR, on ne fait rien
        if (vrHead == null || vrLeftController == null || vrRightController == null)
            return;

        // HEAD
        if (headBone)
        {
            headBone.position = vrHead.position + headOffsetPosition;
            headBone.rotation = vrHead.rotation;
        }

        // MAIN GAUCHE
        if (leftHandBone)
        {
            leftHandBone.position = vrLeftController.position + handOffsetPosition;
            leftHandBone.rotation = vrLeftController.rotation;
        }

        // MAIN DROITE
        if (rightHandBone)
        {
            rightHandBone.position = vrRightController.position + handOffsetPosition;
            rightHandBone.rotation = vrRightController.rotation;
        }
    }
}

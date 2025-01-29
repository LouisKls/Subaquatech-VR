using UnityEngine;

public class AvatarTracking : MonoBehaviour
{
    [Header("Bones de l'avatar (assignés dans le prefab)")]
    public Transform headBone;
    public Transform leftHandBone;
    public Transform rightHandBone;

    [Header("Références VR (assignées après l'Instantiate)")]
    public Transform vrHead;             // Caméra VR
    public Transform vrLeftController;   // Contrôleur gauche
    public Transform vrRightController;  // Contrôleur droit

    // Paramètres de position/rotation (offset) si besoin
    // par exemple pour ajuster la taille ou le placement
    public Vector3 headOffsetPosition;
    public Vector3 handOffsetPosition;

    void Update()
    {
        // Sécurité : si on n'a pas encore de références VR, on ne fait rien
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

using UnityEngine;
using UnityEngine.UI;

public class CompassBar : MonoBehaviour
{
    // Assigner le transform du joueur ou de la caméra, qui donne l'orientation
    public Transform playerTransform;

    // Référence vers l'image de la boussole (RawImage de préférence pour le UV offset)
    public RawImage compassImage;

    // Angle initial si nécessaire (par exemple, 0 = Nord)
    private float initialRotation = 0f;

    void Update()
    {
        // On récupère l'angle Y du joueur (orientation sur l’axe vertical)
        float playerYaw = playerTransform.eulerAngles.y;

        // On convertit cet angle en une valeur entre 0 et 1 pour un offset UV
        // Supposons que notre texture répète la totalité d’un tour (360°).
        // Offset = (angle / 360) indique quelle portion de la texture afficher
        float uvOffset = playerYaw / 360f;

        // Appliquer cet offset au RawImage
        // On va décaler la texture horizontalement. Si la texture de la boussole 
        // est conçue pour se répéter, ce sera fluide.
        compassImage.uvRect = new Rect(uvOffset -0.22f, 0f, 1f, 1f);
    }
}

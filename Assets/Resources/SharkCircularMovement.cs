using UnityEngine;

public class SharkCircularMovement : MonoBehaviour
{
    public Transform centerPoint; // Point central de rotation
    public float rotationSpeed = 20f; // Vitesse de rotation
    public float radius = 5f; // Rayon du cercle
    private float angle = 0f; // Angle actuel (en radians)

    void Update()
    {
        // Calculer la position sur le cercle
        angle += rotationSpeed * Time.deltaTime;
        float x = Mathf.Cos(angle) * radius;
        float z = Mathf.Sin(angle) * radius;

        // Mettre à jour la position du requin
        transform.position = new Vector3(centerPoint.position.x + x, transform.position.y, centerPoint.position.z + z);

        // Faire face à la direction de déplacement
        Vector3 direction = new Vector3(-Mathf.Sin(angle), 0, Mathf.Cos(angle));
        transform.forward = direction;
    }
}
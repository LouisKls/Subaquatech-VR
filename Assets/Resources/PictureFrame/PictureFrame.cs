using UnityEngine;

public class PictureFrame : MonoBehaviour
{
    [SerializeField] private Renderer targetRenderer;

    public void SetImage(Sprite newSprite)
    {
        if (targetRenderer != null && newSprite != null)
        {
            // Convertir le Sprite en Texture2D ou utiliser directly newSprite.texture (s'il existe)
            // Nota: newSprite.texture est parfois un Texture2D
            Texture2D tex = newSprite.texture;
            targetRenderer.material.mainTexture = tex;
        }
    }
}

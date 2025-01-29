using System.IO;
using UnityEngine;
using UnityEngine.UI; // Pour l'UI
using UnityEngine.Events;
using System.Linq;


public class DynamicImageLoader : MonoBehaviour
{
    [Header("Gallery Settings")]
    [Tooltip("Chemin vers le dossier contenant les images. Exemple : Application.persistentDataPath + \"/Screenshots\"")]
    public string folderPath;

    [Header("UI References")]
    [Tooltip("Panel (ou Content) où on va générer les items dans la ScrollView.")]
    public Transform galleryContent;
    [Tooltip("Prefab d'un bouton contenant un UI Image.")]
    public GameObject imageButtonPrefab;

    [Header("Events")]
    public UnityEvent<Sprite> onImageSelected;
    // Cet event permettra de signaler qu'une image a été cliquée,
    // et donc qu'on doit l'appliquer au cadre mural

    [SerializeField] PictureFrame pictureFrame;

    void Start()
    {
        folderPath = Path.Combine(Application.persistentDataPath, "Snapshots");
        //persistentDataPath
        Debug.Log(folderPath);
        ClearScreenshotsFolder();
        RefreshGallery();
    }

    private void ClearScreenshotsFolder()
    {
        // Construire le chemin du dossier Screenshots
        string folderPath = Path.Combine(Application.persistentDataPath, "Snapshots");

        // Vérifier si le dossier existe
        if (Directory.Exists(folderPath))
        {
            // Lister tous les fichiers
            string[] files = Directory.GetFiles(folderPath);

            // Supprimer chaque fichier
            foreach (string file in files)
            {
                File.Delete(file);
            }

            Debug.Log($"Dossier '{folderPath}' vidé avec succès.");
        }
        else
        {
            Debug.Log($"Dossier '{folderPath}' inexistant, aucune suppression nécessaire.");
        }
    }

    /// <summary>
    /// Méthode à appeler pour rafraîchir la liste d’images.
    /// </summary>
    public void RefreshGallery()
    {
        // 1) Vider le contenu existant
        foreach (Transform child in galleryContent)
        {
            Destroy(child.gameObject);
        }

        // 2) Vérifier l'existence du dossier
        if (!Directory.Exists(folderPath))
        {
            Debug.LogWarning("Le dossier n'existe pas : " + folderPath);
            Directory.CreateDirectory(folderPath);
            //return;
        }

        // 3) Récupérer tous les fichiers .png et .jpg
        string[] files = Directory.GetFiles(folderPath, "*.*")
            // Filtrer seulement PNG/JPG
            .Where(s => s.EndsWith(".png") || s.EndsWith(".jpg"))
            .ToArray();

        Debug.Log(files.Length);

        // 4) Pour chaque fichier, on crée un bouton
        foreach (string file in files)
        {
            // Charger l'image sous forme de Sprite
            Sprite sprite = LoadSpriteFromFile(file);
            Debug.Log($"LoadSpriteFromFile returned {sprite} with type {(sprite ? sprite.GetType().ToString() : "null")}");
            if (sprite == null)
                continue; // Fichier corrompu ou autre problème

            // Instancier le bouton
            GameObject newButtonObj = Instantiate(imageButtonPrefab, galleryContent);
            // On récupère la composante Image
            Image buttonImage = newButtonObj.GetComponentInChildren<Image>();
            buttonImage.sprite = sprite;

            // On abonne le bouton OnClick()
            Button btn = newButtonObj.GetComponentInChildren<Button>();
            Debug.Log("Function : " + onImageSelected);
            Debug.Log("Sprite : " + sprite.name);
            Debug.Log($"LoadSpriteFromFile returned {sprite} with type {(sprite ? sprite.GetType().ToString() : "null")}");
            Debug.Log("Adding listener to : " + btn);
            btn.onClick.AddListener(() =>
            {
                // Quand on clique, on invoque l'event
                pictureFrame.SetImage(sprite);
            });
            Debug.Log("Listener correctly added");
        }
    }

    /// <summary>
    /// Charge un fichier .png ou .jpg en Sprite
    /// </summary>
    private Sprite LoadSpriteFromFile(string filePath)
    {
        if (!File.Exists(filePath))
            return null;

        byte[] fileData = File.ReadAllBytes(filePath);
        Texture2D tex = new Texture2D(2, 2);

        if (tex.LoadImage(fileData))
        {
            // On crée un Sprite à partir de la Texture2D
            Sprite sprite = Sprite.Create(tex, new Rect(0, 0, tex.width, tex.height), new Vector2(0.5f, 0.5f));
            sprite.name = Path.GetFileNameWithoutExtension(filePath);

            return sprite;
        }
        return null;
    }

}

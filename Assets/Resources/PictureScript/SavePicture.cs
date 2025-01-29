using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SavePicture : MonoBehaviour
{
    [SerializeField] private Camera cam;
    [SerializeField] private Camera cam2;
    private int resWidth;
    private int resHeight;
    private bool takePicture = false;

    public DynamicImageLoader loader;

    public PhoneManager phoneManager;

    private void Awake()
    {
        resWidth = cam.targetTexture.width;
        resHeight = cam.targetTexture.height;
    }

    private void Update()
    {
        // Check if the "P" key is pressed to take a picture
        if (Input.GetKeyDown(KeyCode.P))
        {
            TakePicture();
        }
    }

    private string SnapShotName()
    {
        return string.Format("{0}/Snapshots/snap_{1}.png", Application.persistentDataPath, System.DateTime.Now.ToString("yyy-mm-dd_HH-mm-ss"));
    }

    public void TakePicture()
    {
        // Créer une texture pour capturer l'image
        Texture2D snapShot = new Texture2D(resWidth, resHeight, TextureFormat.RGB24, false);

        if(phoneManager.isSelfieModeEnabled)
        {
            cam2.Render();
            RenderTexture.active = cam2.targetTexture;
        } else
        {
            cam.Render();
            RenderTexture.active = cam.targetTexture;
        }
        
        snapShot.ReadPixels(new Rect(0, 0, resWidth, resHeight), 0, 0);

        // Encoder l'image en PNG
        byte[] bytes = snapShot.EncodeToPNG();
        string filename = SnapShotName();

        // Vérifier et créer le répertoire si nécessaire
        string directoryPath = System.IO.Path.GetDirectoryName(filename);
        if (!System.IO.Directory.Exists(directoryPath))
        {
            System.IO.Directory.CreateDirectory(directoryPath);
        }

        // Sauvegarder l'image
        System.IO.File.WriteAllBytes(filename, bytes);
        Debug.Log($"Snapshot taken and saved to {filename}");

        loader.RefreshGallery();
    }
}

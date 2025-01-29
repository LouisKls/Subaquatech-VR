using UnityEngine;
using UnityEngine.InputSystem; // Pour le New Input System


public class ObjectsPlacementManager : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private UnityEngine.XR.Interaction.Toolkit.Interactors.XRBaseInteractor nearFarInteractor;
    [SerializeField] private GameObject objectToPlacePrefab;

    // Deux LayerMasks :
    [Header("Layers")]
    [SerializeField] private LayerMask floorLayer;  // correspond à "Floor"
    [SerializeField] private LayerMask wallLayer;   // correspond à "Wall"

    [Header("Input Actions (New Input System)")]
    [SerializeField] private InputActionReference placeAction;    // Existant
    [SerializeField] private InputActionReference cancelAction;   // Existant

    [Header("Sounds")]
    public AudioSource audioSource;
    public AudioClip soundClip;

    // ---> Nouveau champ <---
    [SerializeField] private InputActionReference rotateAction;   // Action déclenchée par la manette gauche

    private GameObject currentPreviewObject;

    // On garde une référence au type pour savoir sur quelle surface on peut le poser
    private PlacementType currentPlacementType;

    private void OnEnable()
    {
        // On s'abonne aux events "performed" sur Place/Cancel
        if (placeAction?.action != null)
            placeAction.action.performed += OnPlacePerformed;
        if (cancelAction?.action != null)
            cancelAction.action.performed += OnCancelPerformed;

        // ---> On s'abonne aussi à l'Action de rotation <---
        if (rotateAction?.action != null)
            rotateAction.action.performed += OnRotatePerformed;

        // Active les actions
        placeAction?.action?.Enable();
        cancelAction?.action?.Enable();
        rotateAction?.action?.Enable();
    }

    private void OnDisable()
    {
        if (placeAction?.action != null)
            placeAction.action.performed -= OnPlacePerformed;
        if (cancelAction?.action != null)
            cancelAction.action.performed -= OnCancelPerformed;

        // ---> Désabonnement Action rotation <---
        if (rotateAction?.action != null)
            rotateAction.action.performed -= OnRotatePerformed;

        placeAction?.action?.Disable();
        cancelAction?.action?.Disable();
        rotateAction?.action?.Disable();
    }

    // Méthode appelée par les boutons UI du menu pour sélectionner un nouvel objet
    public void SelectObject(GameObject prefab)
    {
        objectToPlacePrefab = prefab;

        // Récupérer le script PlaceableObject (s’il existe) pour connaître son type
        PlaceableObject placeableInfo = prefab.GetComponent<PlaceableObject>();
        if (placeableInfo != null)
        {
            currentPlacementType = placeableInfo.placementType;
        }
        else
        {
            // Par défaut, si rien n’est spécifié, on considère FloorOnly ou autoriser tout
            currentPlacementType = PlacementType.FloorOnly;
        }

        CreatePreview();
    }

    private void Update()
    {
        if (currentPreviewObject != null)
        {
            UpdatePreviewPosition();
        }
    }

    private void CreatePreview()
    {
        if (currentPreviewObject != null)
            Destroy(currentPreviewObject);

        currentPreviewObject = Instantiate(objectToPlacePrefab);
        SetObjectTransparent(currentPreviewObject);
    }

    private void SetObjectTransparent(GameObject obj)
    {
        foreach (var rend in obj.GetComponentsInChildren<Renderer>())
        {
            foreach (var mat in rend.materials)
            {
                var c = mat.color;
                c.a = 0.5f;
                mat.color = c;
            }
        }
    }

    private void UpdatePreviewPosition()
    {
        // Selon le type d’objet, on choisit l’un des deux LayerMasks
        LayerMask placementLayer = (currentPlacementType == PlacementType.FloorOnly) ? floorLayer : wallLayer;

        // Raycast manuel depuis nearFarInteractor
        Transform t = nearFarInteractor.transform;
        Ray ray = new Ray(t.position, t.forward);
        float maxDist = 10f;

        if (Physics.Raycast(ray, out RaycastHit hit, maxDist, placementLayer))
        {
            currentPreviewObject.transform.position = hit.point;
        }
        else
        {
            currentPreviewObject.transform.position = new Vector3(0, -100, 0);
        }
    }

    // Callback quand on presse l'action "Place"
    private void OnPlacePerformed(InputAction.CallbackContext ctx)
    {
        if (currentPreviewObject != null)
            PlaceObject();
    }

    // Callback quand on presse l'action "Cancel"
    private void OnCancelPerformed(InputAction.CallbackContext ctx)
    {
        if (currentPreviewObject != null)
            CancelPlacement();
    }

    // ---> Callback pour la rotation <---
    private void OnRotatePerformed(InputAction.CallbackContext ctx)
    {
        if (currentPreviewObject != null)
        {
            // Tourne autour de l'axe Y de 90°
            currentPreviewObject.transform.Rotate(0f, 90f, 0f);
        }
    }

    public void PlaceObject()
    {
        if (currentPreviewObject != null && objectToPlacePrefab != null)
        {
            // On instantiate l'objet final seulement si on n'est pas "hors de vue"
            if (currentPreviewObject.transform.position.y > -50)
            {
                // => On suppose qu'on a bien trouvé le bon layer
                Vector3 pos = currentPreviewObject.transform.position;
                Quaternion rot = currentPreviewObject.transform.rotation;
                Instantiate(objectToPlacePrefab, pos, rot);
                audioSource.PlayOneShot(soundClip);
            }

            // Dans tous les cas, on détruit le preview
            Destroy(currentPreviewObject);
            currentPreviewObject = null;
        }
    }

    private void CancelPlacement()
    {
        if (currentPreviewObject != null)
        {
            Destroy(currentPreviewObject);
            currentPreviewObject = null;
        }
    }
}

using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

/// <summary>
/// Places towers in the battle stage with the mouse.
///
/// Used Tim's BuildingPlacer in the Town as reference, so both placement systems behave alike and use the same Input System:
///   1. A shop button calls SelectTower(towerData).
///   2. A preview of the tower follows the mouse over the ground and shows its range.
///   3. Left click places it. Right click cancels.
///
/// Differences from BuildingPlacer:
/// - Clicks on UI buttons are ignored, so clicking the shop never drops a tower behind it.
/// - The tower is told it is a preview, so it does not shoot while you are still moving it.
///
/// Setup:  Put this on an empty "TowerManager" object in the battle scene and set Ground Layer to the "Ground" layer. 
///         The ground object must be on that layer.
/// </summary>
public class TowerPlacer : MonoBehaviour
{
    /// <summary>The one TowerPlacer in the scene, so buttons and other scripts can reach it.</summary>
    public static TowerPlacer Instance { get; private set; }

    [Header("Setup")]
    [Tooltip("Only objects on this layer count as ground you can place on.")]
    public LayerMask groundLayer;

    [Tooltip("Lift the tower this far above the ground point (use it if the model sinks into the floor).")]
    public float heightOffset = 0f;

    private Camera mainCamera;
    private Tower preview;              // the tower following the mouse, or null
    private bool previewIsOnGround;     // false while the mouse is not over the ground
    [SerializeField] private AudioClip placementSound;
    [SerializeField] private AudioSource audioSource;

    /// <summary>True while a tower is following the mouse.</summary>
    public bool IsPlacing { get { return preview != null; } }

    /// <summary>The frame a tower was placed on. TowerSelector ignores that click so it does not also select.</summary>
    public int LastPlacementFrame { get; private set; } = -1;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        mainCamera = Camera.main;
    }

    private void Update()
    {
        if (preview == null || Mouse.current == null)
        {
            return;
        }

        MovePreviewToMouse();

        if (Mouse.current.rightButton.wasPressedThisFrame)
        {
            CancelPlacement();
            return;
        }

        if (Mouse.current.leftButton.wasPressedThisFrame && previewIsOnGround && !IsPointerOverUI())
        {
            ConfirmPlacement();
        }
    }

    /// <summary>Start placing a tower. Shop buttons use this.</summary>
    public void SelectTower(TowerData towerData)
    {
        CancelPlacement();

        if (towerData == null || towerData.towerPrefab == null)
        {
            Debug.LogWarning("TowerPlacer: this TowerData has no tower prefab assigned.");
            return;
        }

        GameObject towerObject = Instantiate(towerData.towerPrefab);
        preview = towerObject.GetComponent<Tower>();
        if (preview == null)
        {
            Debug.LogError("TowerPlacer: the prefab of " + towerData.towerName + " has no Tower component on its root.");
            Destroy(towerObject);
            return;
        }

        preview.BeginPreview();
        SetCollidersEnabled(towerObject, false); // Here so that the preview does not block its own raycast
        previewIsOnGround = false;

        if (TowerSelector.Instance != null)
        {
            TowerSelector.Instance.Deselect();
        }
    }

    /// <summary>Throw the preview away. Also usable from a "Cancel" UI button.</summary>
    public void CancelPlacement()
    {
        if (preview != null)
        {
            Destroy(preview.gameObject);
            preview = null;
        }
    }

    private void MovePreviewToMouse()
    {
        Vector2 mousePosition = Mouse.current.position.ReadValue();
        Ray ray = mainCamera.ScreenPointToRay(mousePosition);

        if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, groundLayer))
        {
            Vector3 position = hit.point;
            position.y += heightOffset;
            preview.transform.position = position;
            previewIsOnGround = true;
        }
        else
        {
            previewIsOnGround = false;
        }
    }

    private void ConfirmPlacement()
    {
        SetCollidersEnabled(preview.gameObject, true);
        preview.Place();
        preview = null;
        LastPlacementFrame = Time.frameCount;
        if (placementSound != null && audioSource != null)
        {
            audioSource.PlayOneShot(placementSound);
        }
        else if (placementSound != null)
        {
            AudioSource.PlayClipAtPoint(placementSound, transform.position);
        }
    }

    private static void SetCollidersEnabled(GameObject root, bool isEnabled)
    {
        foreach (Collider col in root.GetComponentsInChildren<Collider>())
        {
            // The range sphere's collider always stays off (TowerRange).
            if (col.GetComponent<TowerRange>() != null)
            {
                continue;
            }
            col.enabled = isEnabled;
        }
    }

    private static bool IsPointerOverUI()
    {
        return EventSystem.current != null && EventSystem.current.IsPointerOverGameObject();
    }
}

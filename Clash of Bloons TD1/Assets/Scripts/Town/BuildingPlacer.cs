using UnityEngine;
using UnityEngine.InputSystem;

public class BuildingPlacer : MonoBehaviour
{
    [Header("Setup")]
    public LayerMask groundLayer;
    public float heightOffset = 0f;
    public float rotationSpeed = 90f;

    private GameObject activePreview;
    private BuildingInfo currentBuildingInfo;
    private Camera mainCamera;
    private float currentRotation = 0f;

    void Start()
    {
        mainCamera = Camera.main;
    }

    void Update()
    {
        if (activePreview != null)
        {
            UpdatePreviewPosition();
            HandleRotation();

            if (Mouse.current != null && Mouse.current.leftButton.wasPressedThisFrame)
            {
                ConfirmPlacement();
            }
        }
    }

    public void SelectBuilding(GameObject prefab)
    {
        if (activePreview != null)
        {
            Destroy(activePreview);
        }

        if (prefab == null) return;

        currentBuildingInfo = prefab.GetComponent<BuildingInfo>();

        StartPlacement(prefab);
    }

    void StartPlacement(GameObject prefab)
    {
        int cost = currentBuildingInfo != null ? currentBuildingInfo.cost : 100;

        if (GameManager.Instance != null && GameManager.Instance.townCurrency < cost)
        {
            Debug.Log("Niet genoeg munten!");
            return;
        }

        activePreview = Instantiate(prefab);
        currentRotation = 0f;

        Collider[] colliders = activePreview.GetComponentsInChildren<Collider>();
        foreach (Collider col in colliders)
        {
            col.enabled = false;
        }
    }

    void UpdatePreviewPosition()
    {
        if (Mouse.current == null || activePreview == null) return;

        Vector2 mousePos = Mouse.current.position.ReadValue();
        Ray ray = mainCamera.ScreenPointToRay(mousePos);

        if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, groundLayer))
        {
            Vector3 targetPosition = hit.point;
            targetPosition.y += heightOffset;

            activePreview.transform.position = targetPosition;

            Vector3 offset = currentBuildingInfo != null ? currentBuildingInfo.rotationOffset : Vector3.zero;
            activePreview.transform.rotation = Quaternion.Euler(
                offset.x,
                offset.y + currentRotation,
                offset.z
            );
        }
    }

    void HandleRotation()
    {
        if (Keyboard.current != null && Keyboard.current.rKey.wasPressedThisFrame)
        {
            currentRotation += rotationSpeed;
            if (currentRotation >= 360f) currentRotation -= 360f;
        }
    }

    void ConfirmPlacement()
    {
        int cost = currentBuildingInfo != null ? currentBuildingInfo.cost : 100;

        if (GameManager.Instance != null)
        {
            if (GameManager.Instance.townCurrency < cost)
            {
                Debug.Log("Niet genoeg munten om te plaatsen!");
                Destroy(activePreview);
                activePreview = null;
                return;
            }
            GameManager.Instance.townCurrency -= cost;
        }

        Collider[] colliders = activePreview.GetComponentsInChildren<Collider>();
        foreach (Collider col in colliders)
        {
            col.enabled = true;
        }

        activePreview = null;
    }
}
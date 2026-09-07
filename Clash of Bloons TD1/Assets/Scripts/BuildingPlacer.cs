using UnityEngine;
using UnityEngine.InputSystem;

public class BuildingPlacer : MonoBehaviour
{
    [Header("Setup")]
    public GameObject buildingPrefab;
    public LayerMask groundLayer;

    [Header("Settings")]
    public float heightOffset = 0f;
    public float rotationSpeed = 90f;

    private GameObject activePreview;
    private Camera mainCamera;
    private float currentRotation = 0f;

    void Start()
    {
        mainCamera = Camera.main;
        StartPlacement();
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

    void StartPlacement()
    {
        activePreview = Instantiate(buildingPrefab);
        currentRotation = 0f;

        Collider[] colliders = activePreview.GetComponentsInChildren<Collider>();
        foreach (Collider col in colliders)
        {
            col.enabled = false;
        }
    }

    void UpdatePreviewPosition()
    {
        if (Mouse.current == null) return;

        Vector2 mousePos = Mouse.current.position.ReadValue();
        Ray ray = mainCamera.ScreenPointToRay(mousePos);

        if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, groundLayer))
        {
            Vector3 targetPosition = hit.point;
            targetPosition.y += heightOffset;

            activePreview.transform.position = targetPosition;
            activePreview.transform.rotation = Quaternion.Euler(-90f, 0f, currentRotation);
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
        Collider[] colliders = activePreview.GetComponentsInChildren<Collider>();
        foreach (Collider col in colliders)
        {
            col.enabled = true;
        }

        activePreview = null;
    }
}
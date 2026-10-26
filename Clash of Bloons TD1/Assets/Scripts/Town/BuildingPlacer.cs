using UnityEngine;
using UnityEngine.InputSystem;

public class BuildingPlacer : MonoBehaviour
{
    [Header("Setup")]
    public LayerMask groundLayer;
    public float heightOffset = 0f;
    public float rotationSpeed = 90f;

    [Header("Audio Settings")]
    [SerializeField] private AudioClip placementSound;
    [SerializeField] private AudioSource audioSource;

    [Header("Currency Audio Settings")]
    [SerializeField] private AudioClip currencySpendSound;

    private GameObject activePreview;
    private BuildingInfo currentBuildingInfo;
    private Camera mainCamera;
    private float currentRotation = 0f;

    void Start()
    {
        mainCamera = Camera.main;

        if (audioSource == null)
        {
            audioSource = GetComponent<AudioSource>();
        }
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

        int cost = currentBuildingInfo != null ? currentBuildingInfo.cost : 100;

        if (GameManager.Instance != null)
        {
            if (GameManager.Instance.townCurrency < cost)
            {
                Debug.Log("Niet genoeg munten om deze toren te kopen!");
                currentBuildingInfo = null;
                return;
            }
            GameManager.Instance.townCurrency -= cost;
            if (currencySpendSound != null && audioSource != null)
            {
                audioSource.PlayOneShot(currencySpendSound);
            }
            else if (currencySpendSound != null)
            {
                AudioSource.PlayClipAtPoint(currencySpendSound, transform.position);
            }
        }

        StartPlacement(prefab);
    }

    void StartPlacement(GameObject prefab)
    {
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
        if (activePreview != null)
        {
            activePreview.tag = "BuildTower";
            Collider[] colliders = activePreview.GetComponentsInChildren<Collider>();
            foreach (Collider col in colliders)
            {
                col.enabled = true;
            }
            BuildingInfo info = activePreview.GetComponent<BuildingInfo>();
            if (info == null)
            {
                Debug.LogWarning("Let op: dit geplaatste object heeft geen BuildingInfo script!");
            }
            if (placementSound != null && audioSource != null)
            {
                audioSource.PlayOneShot(placementSound);
            }
            else if (placementSound != null)
            {
                AudioSource.PlayClipAtPoint(placementSound, activePreview.transform.position);
            }

            activePreview = null;
        }
    }
}
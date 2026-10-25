using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

/// <summary>
/// Click a placed tower to select it: its range sphere shows and the info panel opens.
/// Click empty ground to deselect.
///
/// Towers are found with a raycast on the "Towers" layer, so each tower prefab needs a
/// collider (a Box Collider on the root works) and must be on the Towers layer.
///
/// Setup: put this on the same "TowerManager" object as TowerPlacer, set Tower Layer to
/// "Towers", and drag the TowerInfoPanel into Info Panel.
/// </summary>
public class TowerSelector : MonoBehaviour
{
    public static TowerSelector Instance { get; private set; }

    [Tooltip("The layer tower prefabs are on.")]
    public LayerMask towerLayer;

    [Tooltip("The panel that shows the selected tower's info and buttons.")]
    public TowerInfoPanel infoPanel;

    private Camera mainCamera;
    private Tower selected;

    public Tower Selected { get { return selected; } }

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
        if (Mouse.current == null || !Mouse.current.leftButton.wasPressedThisFrame)
        {
            return;
        }

        TowerPlacer placer = TowerPlacer.Instance;
        if (placer != null && (placer.IsPlacing || placer.LastPlacementFrame == Time.frameCount))
        {
            return;
        }
        if (EventSystem.current != null && EventSystem.current.IsPointerOverGameObject())
        {
            return;
        }

        Ray ray = mainCamera.ScreenPointToRay(Mouse.current.position.ReadValue());
        if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, towerLayer))
        {
            // The collider may sit on a child of the tower.
            Tower tower = hit.collider.GetComponentInParent<Tower>();
            if (tower != null && tower.IsPlaced)
            {
                Select(tower);
                return;
            }
        }

        Deselect();
    }

    public void Select(Tower tower)
    {
        if (selected != null)
        {
            selected.ShowRange(false);
        }

        selected = tower;
        selected.ShowRange(true);

        if (infoPanel != null)
        {
            infoPanel.Show(tower);
        }
    }

    public void Deselect()
    {
        if (selected != null)
        {
            selected.ShowRange(false);
            selected = null;
        }

        if (infoPanel != null)
        {
            infoPanel.Hide();
        }
    }
}

using TMPro;
using UnityEngine;

/// <summary>
/// The panel that opens when you click a placed tower. It shows the tower's stats and has buttons to:
/// - Change the target priority (First, Last, Closest)
/// - Close the panel
///
/// Setup (Inside a Canvas):
/// - Put this component on the Canvas or on an empty object, not on the panel itself
/// - Panel Root: the panel object that gets shown and hidden.
/// - Texts and buttons: drag in the TextMeshPro texts. Point each Button's OnClick at OnPriorityButton / OnCloseButton on this component.
/// </summary>
public class TowerInfoPanel : MonoBehaviour
{
    [SerializeField] private GameObject panelRoot;
    [SerializeField] private TMP_Text nameText;
    [SerializeField] private TMP_Text descriptionText;
    [SerializeField] private TMP_Text statsText;
    [SerializeField] private TMP_Text priorityButtonText;

    private Tower tower;

    private void Start()
    {
        Hide();
    }

    private void Update()
    {
        // Refresh while open, so what it shows always matches the tower.
        if (tower != null && panelRoot.activeSelf)
        {
            Refresh();
        }
    }

    public void Show(Tower newTower)
    {
        tower = newTower;
        panelRoot.SetActive(true);
        Refresh();
    }

    public void Hide()
    {
        tower = null;
        panelRoot.SetActive(false);
    }

    private void Refresh()
    {
        if (nameText != null)
        {
            nameText.text = tower.Data.towerName;
        }

        if (descriptionText != null)
        {
            descriptionText.text = tower.Data.description;
        }

        if (statsText != null)
        {
            string damageLine = "Damage: " + tower.Damage.ToString("0.#");

            statsText.text = damageLine
                + "\nCooldown: " + tower.Data.attackCooldown.ToString("0.#") + " s"
                + "\nRange: " + tower.Range.Radius.ToString("0.#");
        }

        if (priorityButtonText != null)
        {
            priorityButtonText.text = "Target: " + tower.Priority;
        }
    }

    // ---- Button hooks: point each Button's OnClick here ----

    public void OnPriorityButton()
    {
        if (tower == null) return;
        tower.CyclePriority();
        Refresh();
    }

    public void OnCloseButton()
    {
        if (TowerSelector.Instance != null)
        {
            TowerSelector.Instance.Deselect();
        }
        else
        {
            Hide();
        }
    }
}

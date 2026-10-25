using TMPro;
using UnityEngine;

/// <summary>
/// The panel that opens when you click a placed tower. It shows the tower's stats and has buttons to:
/// - Change the target priority (First, Last, Closest)
/// - Switch the Archer between Spread and Homing
/// - Close the panel
///
/// Setup (Inside a Canvas):
/// - Put this component on the Canvas or on an empty object, not on the panel itself
/// - Panel Root: the panel object that gets shown and hidden.
/// - Texts and buttons: drag in the TextMeshPro texts. Point each Button's OnClick at OnPriorityButton / OnModeButton / OnCloseButton on this component.
/// </summary>
public class TowerInfoPanel : MonoBehaviour
{
    [SerializeField] private GameObject panelRoot;
    [SerializeField] private TMP_Text nameText;
    [SerializeField] private TMP_Text descriptionText;
    [SerializeField] private TMP_Text statsText;
    [SerializeField] private TMP_Text priorityButtonText;

    [Tooltip("The whole mode button. Hidden for towers without a mode.")]
    [SerializeField] private GameObject modeButton;
    [SerializeField] private TMP_Text modeButtonText;

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

            // Show the Baker's bonus when a Baker covers this tower.
            float bonus = tower.Damage - tower.Data.damage;
            if (bonus > 0f)
            {
                damageLine += " (+" + bonus.ToString("0.#") + " from Baker)";
            }

            statsText.text = damageLine
                + "\nCooldown: " + tower.Data.attackCooldown.ToString("0.#") + " s"
                + "\nRange: " + tower.Range.Radius.ToString("0.#");
        }

        if (priorityButtonText != null)
        {
            priorityButtonText.text = "Target: " + tower.Priority;
        }

        // The mode button only shows for towers that have a mode.
        if (modeButton != null)
        {
            modeButton.SetActive(tower.HasMode);
            if (tower.HasMode && modeButtonText != null)
            {
                modeButtonText.text = "Mode: " + tower.ModeLabel;
            }
        }
    }

    // ---- Button hooks: point each Button's OnClick here ----

    public void OnPriorityButton()
    {
        if (tower == null) return;
        tower.CyclePriority();
        Refresh();
    }

    public void OnModeButton()
    {
        if (tower == null) return;
        tower.ToggleMode();
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

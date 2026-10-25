using TMPro;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// One button in the battle stage's tower shop. Clicking it starts placing that tower.
///
/// Setup: Canvas > right-click > UI > Button - TextMeshPro. 
/// Add this component to it, drag in the TowerData asset and the button's text. 
/// No OnClick setup needed.
/// </summary>
[RequireComponent(typeof(Button))]
public class TowerShopButton : MonoBehaviour
{
    [SerializeField] private TowerData towerData;

    [Tooltip("Optional: the button's text. It is filled with the tower's name.")]
    [SerializeField] private TMP_Text label;

    private Button button;

    private void Start()
    {
        button = GetComponent<Button>();
        button.onClick.AddListener(OnClick);
        Refresh();
    }

    /// <summary>Show the tower's name on the button.</summary>
    public void Refresh()
    {
        if (towerData == null)
        {
            Debug.LogWarning(name + ": no TowerData assigned to this shop button.", this);
            button.interactable = false;
            return;
        }

        if (label != null)
        {
            label.text = towerData.towerName;
        }
    }

    private void OnClick()
    {
        if (TowerPlacer.Instance != null)
        {
            TowerPlacer.Instance.SelectTower(towerData);
        }
        else
        {
            Debug.LogWarning("There is no TowerPlacer in this scene.");
        }
    }
}

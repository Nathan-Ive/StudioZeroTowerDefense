using UnityEngine;

public class BossUIManager : MonoBehaviour
{
    public static BossUIManager Instance;

    [Header("UI Panels")]
    public GameObject winUiPanel;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }
        if (winUiPanel != null)
        {
            winUiPanel.SetActive(false);
        }
    }

    public void ShowWinScreen()
    {
        if (winUiPanel != null)
        {
            winUiPanel.SetActive(true);
        }
    }
}
using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public int townCurrency = 100;
    public TextMeshProUGUI coinText;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Update()
    {
        if (coinText != null)
        {
            coinText.text = townCurrency.ToString();
        }
    }
}
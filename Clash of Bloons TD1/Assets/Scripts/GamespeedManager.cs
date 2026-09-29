using UnityEngine;
using UnityEngine.UI;

public class GamespeedManager : MonoBehaviour
{
    private Button speedButton;
    private Image buttonImage;
    private bool isFast = false;
    private Color normalColor = Color.white;
    private Color fastColor = new Color(157f / 255f, 154f / 255f, 117f / 255f, 255f / 255f);

    void Start()
    {
        speedButton = GetComponent<Button>();
        buttonImage = GetComponent<Image>();
        speedButton.onClick.AddListener(ToggleGameSpeed);
        UpdateColor();
    }

    void ToggleGameSpeed()
    {
        isFast = !isFast;

        if (isFast)
        {
            Time.timeScale = 2f;
        }
        else
        {
            Time.timeScale = 1f;
        }

        UpdateColor();
    }
    void UpdateColor()
    {
        if (buttonImage != null)
        {
            buttonImage.color = isFast ? fastColor : normalColor;
        }
    }
}
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LoadSceneOnButtonClick : MonoBehaviour
{
    [SerializeField] private Button targetButton;
    [SerializeField] private string sceneToLoad;

    private void Awake()
    {
        if (targetButton == null)
        {
            targetButton = GetComponent<Button>();
        }
    }

    private void OnEnable()
    {
        if (targetButton != null)
        {
            targetButton.onClick.AddListener(OnButtonClicked);
        }
    }
    private void OnDisable()
    {

        if (targetButton != null)
        {
            targetButton.onClick.RemoveListener(OnButtonClicked);
        }
    }

    private void OnButtonClicked()
    {
        SceneManager.LoadScene(sceneToLoad);
    }
}
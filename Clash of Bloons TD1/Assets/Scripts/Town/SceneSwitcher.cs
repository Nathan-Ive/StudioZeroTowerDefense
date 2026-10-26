using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneSwitcher : MonoBehaviour
{
    [Header("Doelscene")]
    public string targetSceneName = "CopyTestBattleGround_Nathan";

    private void OnMouseDown()
    {
        if (!string.IsNullOrEmpty(targetSceneName))
        {
            Debug.Log("Scene laden: " + targetSceneName);
            SceneManager.LoadScene(targetSceneName);
        }
        else
        {
            Debug.LogWarning("Geen scene naam opgegeven op " + gameObject.name);
        }
    }
}
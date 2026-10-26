using UnityEngine;
using UnityEngine.SceneManagement;

public class Scenechanger : MonoBehaviour
{
    public void GoToGameBySaving(string NameOfScene)
    {
        if (TowerManager.Instance != null)
        {
            TowerManager.Instance.SaveTower();
        }
        else
        {
            Debug.LogWarning("no tower manager");
        }

        SceneManager.LoadScene(NameOfScene);
    }
}
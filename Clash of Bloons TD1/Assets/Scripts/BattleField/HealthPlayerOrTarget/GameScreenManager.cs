using UnityEngine;
using UnityEngine.SceneManagement;

public class GameScreenManager : MonoBehaviour
{
    [Header("UI Panelen")]
    public GameObject menuUI;
    public AudioClip buttonSound;

    private bool isGameStopped = false;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isGameStopped)
            {
                ResumeGame();
            }
            else
            {
                PauseGame();
            }
        }
    }
    public void PauseGame()
    {
        isGameStopped = true;
        Time.timeScale = 0f;

        if (menuUI != null)
        {
            menuUI.SetActive(true);
        }
    }
    public void ResumeGame()
    {
        isGameStopped = false;
        Time.timeScale = 1f; 

        if (menuUI != null)
        {
            menuUI.SetActive(false);
        }
    }
    public void TriggerGameOver()
    {
        isGameStopped = true;
        Time.timeScale = 0f;

        if (menuUI != null)
        {
            menuUI.SetActive(true);
        }
    }
    public void RestartScene()
    {
        PlaySound();
        Time.timeScale = 1f;
        Scene currentScene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(currentScene.name);
    }
    public void LoadScene(string sceneName)
    {
        PlaySound();
        Time.timeScale = 1f;
        SceneManager.LoadScene(sceneName);
    }
    private void PlaySound()
    {
        if (buttonSound != null)
        {
            AudioSource.PlayClipAtPoint(buttonSound, Camera.main.transform.position);
        }
    }
}
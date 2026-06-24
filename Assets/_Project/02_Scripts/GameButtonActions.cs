using UnityEngine;
using UnityEngine.SceneManagement;

public class GameButtonActions : MonoBehaviour
{
    [SerializeField] private string gameSceneName = "AllanScene";
    [SerializeField] private GameObject creditsPanel;
    [SerializeField] private GameObject winPanel;

    public void StartGame()
    {
        LoadScene(gameSceneName);
    }

    public void PlayAgain()
    {
        LoadScene(SceneManager.GetActiveScene().name);
    }

    public void QuitGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }

    public void ShowCredits()
    {
        if (winPanel) winPanel.SetActive(false);
        if (creditsPanel) creditsPanel.SetActive(true);
    }

    private void LoadScene(string sceneName)
    {
        if (string.IsNullOrWhiteSpace(sceneName)) return;

        Time.timeScale = 1f;
        SceneManager.LoadScene(sceneName);
    }
}

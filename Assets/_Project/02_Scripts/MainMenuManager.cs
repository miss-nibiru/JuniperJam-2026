using UnityEngine;
using UnityEngine.SceneManagement;
public class MainMenuManager : MonoBehaviour
{
    [Header("Scene")]
    [SerializeField] private string gameplaySceneName;
    [SerializeField] private GameObject settingsCanvas;

    private void Start()
    {
        ShowSettings(false);
    }

    public void StartGame()
    {
        SceneManager.LoadScene(gameplaySceneName);
    }

    public void QuitGame()
    {
        Application.Quit();
        Debug.Log("Quit Game");
    }

    public void OpenSettings()
    {
        ShowSettings(true);
    }

    public void CloseSettings()
    {
        ShowSettings(false);
    }

    private void ShowSettings(bool show)
    {
        if (settingsCanvas)
        {
            settingsCanvas.SetActive(show);
        }
    }
}
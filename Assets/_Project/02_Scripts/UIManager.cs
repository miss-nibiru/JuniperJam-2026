
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance { get; private set; }
    
    [Header("Canvas Panels")]
    [SerializeField] private GameObject spinPanel;
    [SerializeField] private GameObject deadPanel;
    [SerializeField] private GameObject winPanel;
    [SerializeField] private GameObject creditsPanel;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject); return;
        }
        
        Instance = this;
        
    }
    
    private void Start()
    {
        ShowDeadCanvas(false);
        ShowWinCanvas(false);
        ShowCredits(false);
    }

    public void ShowSpinPanel(bool canvasOn)
    {
        spinPanel?.SetActive(canvasOn);
    }

    public void ShowDeadCanvas(bool canvasOn)
    {
        deadPanel.SetActive(canvasOn);
    }

    public void ShowWinCanvas(bool canvasOn)
    {
        winPanel.SetActive(canvasOn);
    }

    public void ShowCredits(bool canvasOn)
    {
        creditsPanel.SetActive(canvasOn);
    }
    
}

using TMPro;
using UnityEngine;
using UnityEngine.InputSystem.LowLevel;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance { get; private set; }

    [SerializeField] private GameObject spinPanel;

    private void Start()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    public void ShowSpinPanel(bool enabled)
    {
        spinPanel?.SetActive(enabled);
    }
}

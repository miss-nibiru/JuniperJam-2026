using UnityEngine;

/// <summary>
/// Coomunicates witht he level state machine and recieves the signal for the event
/// changes the background depednign on the level
/// </summary>
public class LevelBackgroundManager : MonoBehaviour
{
    [SerializeField] private LevelStateMachine levelStateMachine;
    [SerializeField] private GameObject[] levelBackgrounds;

    private void Awake()
    {
        if (!levelStateMachine) levelStateMachine = FindFirstObjectByType<LevelStateMachine>();
    }

    private void OnEnable()
    {
        if (levelStateMachine) levelStateMachine.LevelStarted += ShowBackgroundForLevel;
    }

    private void OnDisable()
    {
        if (levelStateMachine) levelStateMachine.LevelStarted -= ShowBackgroundForLevel;
        
    }

    private void Start()
    {
        ShowBackgroundForLevel(0);
    }

    private void ShowBackgroundForLevel(int levelIndex)
    {
        if (levelBackgrounds == null) return;

        for (int i = 0; i < levelBackgrounds.Length; i++)
        {
            if (!levelBackgrounds[i]) continue;
            levelBackgrounds[i].SetActive(i == levelIndex);
        }
    }
}
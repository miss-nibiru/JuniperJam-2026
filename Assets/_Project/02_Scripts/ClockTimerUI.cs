using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// fully control the clock timer ui, coomunicates with level manager that controls it, this guy just shows the time ui thingy
/// </summary>

public class ClockTimerUI : MonoBehaviour
{
    [SerializeField] private LevelStateMachine levelStateMachine;
    [SerializeField] private Image clockFillImage;

    private void Awake()
    {
        if (!levelStateMachine) levelStateMachine = FindFirstObjectByType<LevelStateMachine>();
    }

    private void OnEnable()
    {
        if (levelStateMachine)
        {
            levelStateMachine.LevelTimerUpdated += UpdateClock;
        }
    }

    private void OnDisable()
    {
        if (levelStateMachine)
        {
            levelStateMachine.LevelTimerUpdated -= UpdateClock;
        }
    }

    private void UpdateClock(float timeRemaining, float totalTime)
    {
        if (!clockFillImage) return;
        if (totalTime <= 0) return;
        float fillAmount = timeRemaining / totalTime; 
        clockFillImage.fillAmount = fillAmount;
    }
    
}

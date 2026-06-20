using UnityEngine;

/// <summary>
/// This controlls all the levels that are running. Has communication with enemies and spawner and knows timer to switch to the next scene
/// </summary>
public class LevelStateMachine : MonoBehaviour
{
    private ILevelState _currentLevelState;

    private void Start()
    {
        ChangeLevelState(new StageWaveState());
    }

    private void Update()
    {
        _currentLevelState?.ExecuteLevel();
    }

    public void ChangeLevelState(ILevelState newLevelState)
    {
        _currentLevelState?.StopLevel();

        _currentLevelState = newLevelState;

        _currentLevelState?.StartLevel();
    }
}
using UnityEngine;

/// <summary>
/// This controls all the levels that are running.
/// Has communication with enemies and spawner and knows timer to switch to the next scene.
/// </summary>
public class LevelStateMachine : MonoBehaviour
{
    [SerializeField] private LevelWaveData[] levels;
    [SerializeField] private EnemySpawner enemySpawner;
    [SerializeField] private EnemyManager enemyManager;

    private int _currentLevelIndex;
    private ILevelState _currentLevelState;

    private void Start()
    {
        _currentLevelIndex = 0;
        if (levels == null || levels.Length == 0) return;
        Debug.Log("Starting level wave: " + levels[_currentLevelIndex].LevelName);

        ChangeLevelState(new StageWaveState(
            levels[_currentLevelIndex],
            enemySpawner,
            enemyManager,
            this
        ));
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

    public void GoToNextLevel()
    {
        _currentLevelIndex++; // moves to next stage

        if (_currentLevelIndex >= levels.Length)
        {
            Debug.Log("All level waves complete.");
            return;
        }

        Debug.Log("Starting level wave: " + levels[_currentLevelIndex].LevelName);
        ChangeLevelState(new StageWaveState(levels[_currentLevelIndex], enemySpawner, enemyManager, this));
    }
    
}
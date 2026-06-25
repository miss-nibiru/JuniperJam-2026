using System;
using UnityEngine;

/// <summary>
/// This controls all the levels that are running.
/// Has communication with enemies and spawner and knows timer to switch to the next scene.
/// this is now attached to runstatemanager to move forward when a level is finished
/// </summary>
public class LevelStateMachine : MonoBehaviour
{
    [SerializeField] private LevelWaveData[] levels;
    [SerializeField] private EnemySpawner enemySpawner;
    [SerializeField] private EnemyManager enemyManager;
    [SerializeField] private BossDramaQueenEntrance bossEntrance;

    private int _currentLevelIndex;
    private ILevelState _currentLevelState;
    private bool _levelIsActive;
    
    //okay -- first time using events ~ wish me luck, mr poof owo

    public event Action LevelComplete;
    public event Action AllLevelsComplete;

    private void Start()
    {
        _currentLevelIndex = 0;
        _levelIsActive = false;
    }

    private void Update()
    {
        if (!_levelIsActive) return;
        _currentLevelState?.ExecuteLevel();
    }

    public void StartNewLevel()
    {
        if (levels == null || levels.Length == 0)
        {
            Debug.Log("No levels found.");
            AllLevelsComplete?.Invoke();
            return;
        }

        if (_currentLevelIndex >= levels.Length)
        {
            Debug.Log("Levels completed!");
            AllLevelsComplete?.Invoke();
            return;
        }
        
        LevelWaveData currentLevel = levels[_currentLevelIndex];
        Debug.Log("Starting level: " + currentLevel.LevelName);

        if (currentLevel.HasBossEntrance && bossEntrance)
        {
            _levelIsActive = false;

            bossEntrance.PlayBossIntro(() =>
            {
                _levelIsActive = true;
                ChangeLevelState(new LevelWaveState(currentLevel, enemySpawner, enemyManager, this));
            });

            return;
        }

        _levelIsActive = true;
        ChangeLevelState(new LevelWaveState(currentLevel, enemySpawner, enemyManager, this));
        
    }
    
    private void ChangeLevelState(ILevelState newLevelState)
    {
        _currentLevelState?.StopLevel();
        _currentLevelState = newLevelState;
        _currentLevelState?.StartLevel();
    }

    public void FinishCurrentLevel()
    {
        if(!_levelIsActive) return;
        _levelIsActive = false;
        
        _currentLevelState?.StopLevel();
        _currentLevelState = null;
        
        _currentLevelIndex++;

        if (_currentLevelIndex >= levels.Length)
        {
            Debug.Log("All levels completed!");
            AllLevelsComplete?.Invoke();
            return;
        }
        
        LevelComplete?.Invoke();
        
    }

    public void StopCurrentLevel()
    {
        _levelIsActive = false;
        _currentLevelState?.StopLevel();
        _currentLevelState = null;
        enemyManager?.RemoveAllEnemies();
        
    }

    public void ResetLevels()
    {
        StopCurrentLevel();
        _currentLevelIndex = 0;
        
    }

    // public void ChangeLevelState(ILevelState newLevelState)
    // {
    //     _currentLevelState?.StopLevel();
    //     _currentLevelState = newLevelState;
    //     _currentLevelState?.StartLevel();
    // }

    public void GoToNextLevel()
    {
        
        FinishCurrentLevel();
        
    }
    
}
using UnityEngine;

/// <summary>
/// Controls one playable wave of the level.
/// </summary>
public class StageWaveState : ILevelState
{
    private LevelWaveData _levelWaveData;
    private EnemySpawner _enemySpawner;
    private EnemyManager _enemyManager;
    private LevelStateMachine _levelStateMachine;

    private float _currentWaveTimer;
    private float _currentSpawnTimer;
    private bool _levelFinished;

    public StageWaveState(
        LevelWaveData levelWaveData,
        EnemySpawner enemySpawner,
        EnemyManager enemyManager,
        LevelStateMachine levelStateMachine
    )
    {
        _levelWaveData = levelWaveData;
        _enemySpawner = enemySpawner;
        _enemyManager = enemyManager;
        _levelStateMachine = levelStateMachine;
    }

    public void StartLevel()
    {
        _currentWaveTimer = _levelWaveData.WaveTime;
        _currentSpawnTimer = _levelWaveData.SpawnDelay;
        _levelFinished = false;

        Debug.Log("Wave started: " + _levelWaveData.LevelName);
    }
    
    public void ExecuteLevel()
    {
        if (_levelFinished)
        {
            return;
        }

        _currentWaveTimer -= Time.deltaTime;

        if (_currentWaveTimer <= 0)
        {
            _currentWaveTimer = 0;
            _levelFinished = true;

            Debug.Log("Wave timer finished: " + _levelWaveData.LevelName);

            _levelStateMachine.GoToNextLevel();
        }
    }

    public void StopLevel()
    {
        Debug.Log("Incoming next wave");
    }
}
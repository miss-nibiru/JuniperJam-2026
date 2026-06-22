using UnityEngine;

/// <summary>
/// Controls each wave and how they run and when they end. Also controls the timer in the wave for enemies and total time for each level.
/// Connected to enemy spawner to spawn certain groups
/// </summary>
public class LevelWaveState : ILevelState
{
    private LevelWaveData _levelWaveData;
    private EnemySpawner _enemySpawner;
    private EnemyManager _enemyManager;
    private LevelStateMachine _levelStateMachine;

    private float _currentWaveTimer;
    private float _currentSpawnTimer;
    private bool _levelFinished;

    public LevelWaveState(
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
        if (_levelFinished) return;
        _currentWaveTimer -= Time.deltaTime; //countdown wave timer --- should pause before the wave starts

        if (_currentWaveTimer > 0) // if the level still has timer available, more things keep spawning
        {
            _currentSpawnTimer -= Time.deltaTime;

            if (_currentSpawnTimer <= 0)
            {
                EnemySpawnGroupData[] enemyGroups = _levelWaveData.EnemyGroups;

                if (enemyGroups == null || enemyGroups.Length == 0)
                {
                    Debug.LogWarning("Assign the enemies to the wave!! " + _levelWaveData.LevelName);
                    _currentSpawnTimer = _levelWaveData.SpawnDelay;
                    return;
                }

                int randomEnemyGroupIndex = Random.Range(0, enemyGroups.Length);
                EnemySpawnGroupData chosenEnemyGroup = enemyGroups[randomEnemyGroupIndex];
                Debug.Log("Spawning enemies now! " + chosenEnemyGroup.name);

                _enemySpawner.SpawnEnemy(chosenEnemyGroup);
                _currentSpawnTimer = _levelWaveData.SpawnDelay;

            }

            return;
        }
        
        // at this point the wave timer on the wave is done -- spawning should stop and wave ends after all enemies are destroyed
        _currentWaveTimer = 0;
        
        if (!_levelWaveData.MustClearAllEnemiesToFinish)
        {
            _enemyManager.RemoveAllEnemies();
            FinishWave();
            Debug.Log("Wave finished: " + _levelWaveData.LevelName);
            return;
        }

        if (_enemyManager.ActiveEnemyCount == 0)
        {
            FinishWave();
            Debug.Log("Wave finished: " + _levelWaveData.LevelName);
            return;
        }
        
        Debug.Log(_levelWaveData.LevelName);
        
    }

    private void FinishWave()
    {
        
        _levelFinished = true;
        _levelStateMachine.GoToNextLevel();
        
    }

    public void StopLevel()
    {
        Debug.Log("Incoming next wave");
    }
    
}
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
    
    private bool _bossHasSpawned;
    private bool _thisWaveHasBoss;
    
    // i need someway to have delay on spawn for each group?

    private float[] _groupCooldowns;
    private int[] _groupSpawnAmounts;

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

        EnemySpawnGroupData[] enemyGroups = _levelWaveData.EnemyGroups;
        _bossHasSpawned = false;
        _thisWaveHasBoss = WaveHasBoss(enemyGroups);

        if (enemyGroups == null || enemyGroups.Length == 0)
        {
            FinishWave();
            return;
        }

        _groupCooldowns = new float[enemyGroups.Length];
        _groupSpawnAmounts = new int[enemyGroups.Length];
        
    }
    
    public void ExecuteLevel()
    {
        if (_levelFinished) return;
        _currentWaveTimer -= Time.deltaTime; //countdown wave timer --- should pause before the wave starts
        UpdateGroupCooldowns();
        
        if (_thisWaveHasBoss && _bossHasSpawned && _enemyManager.ActiveBossCount == 0)
        {
            _enemyManager.RemoveAllEnemies();
            FinishWave();
            Debug.Log("Boss defeated. Finishing level: " + _levelWaveData.LevelName);
            return;
        }

        if (_currentWaveTimer > 0) // if the level still has timer available, more things keep spawning
        {
            _currentSpawnTimer -= Time.deltaTime;

            if (_currentSpawnTimer <= 0)
            {
                TrySpawnEnemyCluster();
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
            
            return;
        }

        if (_enemyManager.ActiveEnemyCount == 0) FinishWave();
        
        
    }

    private void TrySpawnEnemyCluster()
    {
        EnemySpawnGroupData[] enemyGroups = _levelWaveData.EnemyGroups;

        if (enemyGroups == null || enemyGroups.Length == 0)
        {
            FinishWave();
            return;
        }
        
        int chosenGroupIndex = GetRandomAvailableGroupIndex(enemyGroups);
        if(chosenGroupIndex == -1) return;
        
        EnemySpawnGroupData chosenEnemyGroup = enemyGroups [chosenGroupIndex];
        if (chosenEnemyGroup.EnemyData.MovementType == EnemyData.EnemyMovementType.Boss) _bossHasSpawned = true; //this connects to the boss movement type and detects if boss or not
        
        
        _enemySpawner.SpawnEnemy(chosenEnemyGroup);
        _groupSpawnAmounts[chosenGroupIndex]++;

        if (chosenEnemyGroup.DelayBetweenSpawns > 0)
            _groupCooldowns[chosenGroupIndex] = chosenEnemyGroup.DelayBetweenSpawns;
        

    }
    
    private int GetRandomAvailableGroupIndex(EnemySpawnGroupData[] enemyGroups)
    {
        int[] availableGroupIndexes = new int[enemyGroups.Length];
        int availableCount = 0;

        for (int i = 0; i < enemyGroups.Length; i++)
        {
            EnemySpawnGroupData enemyGroup = enemyGroups[i];

            if (!enemyGroup) continue;
            if (_groupCooldowns[i] > 0) continue;
            if (!_enemyManager.CanSpawnEnemy(enemyGroup)) continue;

            bool hasSpawnLimit = enemyGroup.AmountToSpawn > 0;

            if (hasSpawnLimit && _groupSpawnAmounts[i] >= enemyGroup.AmountToSpawn) continue;
            

            availableGroupIndexes[availableCount] = i;
            availableCount++;
        }

        if (availableCount == 0) return -1;

        int randomAvailableIndex = Random.Range(0, availableCount);
        return availableGroupIndexes[randomAvailableIndex];
    }

    private void UpdateGroupCooldowns()
    {
        if (_groupCooldowns == null) return;

        for (int i = 0; i < _groupCooldowns.Length; i++)
        {
            if (_groupCooldowns[i] > 0) _groupCooldowns[i] -= Time.deltaTime;
        }
    }
    
    private bool WaveHasBoss(EnemySpawnGroupData[] enemyGroups)
    {
        if (enemyGroups == null) return false;

        foreach (EnemySpawnGroupData enemyGroup in enemyGroups)
        {
            if (!enemyGroup) continue;
            if (!enemyGroup.EnemyData) continue;

            if (enemyGroup.EnemyData.MovementType == EnemyData.EnemyMovementType.Boss)
            {
                return true;
            }
        }

        return false;
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
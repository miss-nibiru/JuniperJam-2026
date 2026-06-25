using UnityEngine;

/// <summary>
///  this is essentially what builds the waves, what is allowed per wave and how it behaves
/// Lets me add a million thousand levels if i want!!
/// </summary>

[CreateAssetMenu(fileName = "LevelWaveData", menuName = "Scriptable Objects/LevelWaveData")]
public class LevelWaveData : ScriptableObject
{
    
    [SerializeField] private string levelName;
    [SerializeField] private float waveTime;

    [SerializeField] private EnemySpawnGroupData[] enemyGroups;

    [SerializeField] private bool mustClearAllEnemiesToFinish;
    [SerializeField] private bool hasBossEntrance;
    [SerializeField] private float spawnDelay = 3f;

    public string LevelName => levelName;
    public float WaveTime => waveTime;
    public EnemySpawnGroupData[] EnemyGroups => enemyGroups;
    public bool MustClearAllEnemiesToFinish => mustClearAllEnemiesToFinish;
    public bool HasBossEntrance => hasBossEntrance;
    public float SpawnDelay => spawnDelay;
    
}

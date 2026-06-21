using UnityEngine;

[CreateAssetMenu(fileName = "LevelWaveData", menuName = "Scriptable Objects/LevelWaveData")]
public class LevelWaveData : ScriptableObject
{
    
    [SerializeField] private string levelName;
    [SerializeField] private float waveTime;

    [SerializeField] private EnemySpawnGroupData[] enemyGroups;

    [SerializeField] private bool mustClearAllEnemiesToFinish;
    [SerializeField] private float spawnDelay = 3f;

    public string LevelName => levelName;
    public float WaveTime => waveTime;
    public EnemySpawnGroupData[] EnemyGroups => enemyGroups;
    public bool MustClearAllEnemiesToFinish => mustClearAllEnemiesToFinish;
    public float SpawnDelay => spawnDelay;
    
}

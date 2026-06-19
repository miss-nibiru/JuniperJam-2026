using UnityEngine;

[CreateAssetMenu(fileName = "EnemySpawnGroupData", menuName = "Scriptable Objects/EnemySpawnGroupData")]
public class EnemySpawnGroupData : ScriptableObject
{
    public enum EnemySpawnPattern
    {
        Single,
        Multiple,
        Line,
        Circle
    }
    
    public enum EnemySpawnCoordinate
    {
        North,
        South,
        East,
        West
    }

    [SerializeField] private EnemyData enemyData;
    [SerializeField] private EnemySpawnPattern[] spawnPattern;
    [SerializeField] private EnemySpawnCoordinate[] spawnCoordinate;
    
    [SerializeField] private int amountToSpawn;
    [SerializeField] private int maxActiveOfType;
    [SerializeField] private float delayBetweenSpawns;

    public EnemyData EnemyData => enemyData;
    
    public EnemySpawnPattern[] SpawnPattern => spawnPattern;
    
    public EnemySpawnCoordinate[] SpawnCoordinate => spawnCoordinate;
    
    public int AmountToSpawn => amountToSpawn;
    
    public int MaxActiveOfType => maxActiveOfType;
    
    public float DelayBetweenSpawns => delayBetweenSpawns;
    
    
    
}

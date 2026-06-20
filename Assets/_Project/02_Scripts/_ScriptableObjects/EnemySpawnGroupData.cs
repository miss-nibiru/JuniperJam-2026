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

    public enum ClusterMovementType
    {
        None,
        SpaceInvaders
    }
    
    public enum EnemySpawnCoordinate
    {
        North,
        South,
        East,
        West
    }

    public enum EnemySpawnLocation
    {
        RandomCoordinate,
        SelectedGridCell,
    }

    public enum EnemyPatternDirection
    {
        Horizontal,
        Vertical,
        DiagonalUp,
        DiagonalDown,
    }

    [SerializeField] private EnemyData enemyData;
    [SerializeField] private EnemySpawnPattern[] spawnPattern;
    [SerializeField] private EnemySpawnCoordinate[] spawnCoordinate;
    [SerializeField] private EnemySpawnLocation spawnLocation;
    [SerializeField] private Vector2Int[] selectedGridCell;
    [SerializeField] private EnemyPatternDirection patternDirection;
    
    [SerializeField] private ClusterMovementType clusterMovementType;
    [SerializeField] private float groupHorizontalDistance;
    [SerializeField] private float groupHorizontalSpeed;
    [SerializeField] private float groupAdvanceSpeed;
    
    [SerializeField] private int amountToSpawn; // How many enemies are spawned DURING the time secs of the wave
    [SerializeField] private int enemiesPerPattern; // how many enemies can appear in the pattern clusters
    [SerializeField] private float patternSpacing; // how much spacing from the starting point can they take
    
    [SerializeField] private int maxActiveOfType;
    [SerializeField] private float delayBetweenSpawns;

    public EnemyData EnemyData => enemyData;
    
    public EnemySpawnPattern[] SpawnPattern => spawnPattern;
    
    public EnemySpawnCoordinate[] SpawnCoordinate => spawnCoordinate;
    public EnemyPatternDirection PatternDirection => patternDirection;
    public int AmountToSpawn => amountToSpawn;
    
    public int EnemiesPerPattern => enemiesPerPattern;
    public float PatternSpacing => patternSpacing;
    
    public int MaxActiveOfType => maxActiveOfType;
    
    public float DelayBetweenSpawns => delayBetweenSpawns;
    public EnemySpawnLocation SpawnLocation => spawnLocation;
    public Vector2Int[] SelectedGridCell => selectedGridCell;
    
    public ClusterMovementType GroupMovementType => clusterMovementType;
    public float GroupHorizontalDistance => groupHorizontalDistance;
    public float GroupHorizontalSpeed => groupHorizontalSpeed;
    public float GroupAdvanceSpeed => groupAdvanceSpeed;
    
    
    
}

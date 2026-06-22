using UnityEngine;

/// <summary>
/// holds data for each monster. Each attribute can be changed directly easily like doing an interface
/// </summary>

[CreateAssetMenu(fileName = "EnemyData", menuName = "Scriptable Objects/EnemyData")]
public class EnemyData : ScriptableObject
{
    public enum EnemyMovementType
    {
        None,
        Chase,
        LeftRight,
        Spiral,
        Spline,
        StaticShooter,
        Boss
    }

    [SerializeField] private string enemyName;
    [SerializeField] private GameObject enemyPrefab;

    [SerializeField] private int maxHealth;
    [SerializeField] private int damageAmount;

    [SerializeField] private float moveSpeed;

    [SerializeField] private EnemyMovementType movementType;
    
    // Welcome to complicating my life! --- lets be specific for entrances and movement now:

    [SerializeField] private int introRows;
    [SerializeField] private float introSpeed;
    [SerializeField] private float chaseDelay;
    [SerializeField] private float gridDestroyPadding;
    
    public string EnemyName => enemyName;
    public GameObject EnemyPrefab => enemyPrefab;
    public int MaxHealth => maxHealth;
    public int DamageAmount => damageAmount;

    public float MoveSpeed => moveSpeed;

    public EnemyMovementType MovementType => movementType;
    
    public int IntroRows => introRows;
    public float IntroSpeed => introSpeed;
    public float ChaseDelay => chaseDelay;
    
    public float GridDestroyPadding => gridDestroyPadding;

}

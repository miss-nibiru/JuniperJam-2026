using UnityEngine;

/// <summary>
/// holds data for each monster. Each attribute can be changed directly easily like doing an interface
/// </summary>

[CreateAssetMenu(fileName = "EnemyData", menuName = "Scriptable Objects/EnemyData")]
public class EnemyData : ScriptableObject
{
    public enum EnemyMovementType
    {
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
    

    //[SerializeField] private WeaponData weaponWeakness;

    public string EnemyName => enemyName;
    public GameObject EnemyPrefab => enemyPrefab;
    public int MaxHealth => maxHealth;
    public int DamageAmount => damageAmount;

    public float MoveSpeed => moveSpeed;

    public EnemyMovementType MovementType => movementType;
    
    
    

}

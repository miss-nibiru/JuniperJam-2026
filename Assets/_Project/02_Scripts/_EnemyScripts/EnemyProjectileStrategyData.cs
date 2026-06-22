using UnityEngine;

/// <summary>
/// some enemies can have projectiles and these can have patterns
/// </summary>


[CreateAssetMenu(fileName = "EnemyProjectileStrategyData", menuName = "Scriptable Objects/EnemyProjectileStrategyData")]
public class EnemyProjectileStrategyData : ScriptableObject
{
    public enum ShootingPattern
    {
        Single,
        Burst,
        Fan
    }

    
    [SerializeField] private ProjectileData projectileData;
    [SerializeField] private ShootingPattern shootingPattern;
    [SerializeField] private float firstShotDelay;
    [SerializeField] private float timeBetweenShots;
    
    [SerializeField] private int shotsPerBurst;
    [SerializeField] private float delayBetweenBurstShots;
    [SerializeField] private float burstAngleSpread;

    [SerializeField] private float fanAngleSpread;

    public ProjectileData ProjectileData => projectileData;
    public ShootingPattern Pattern => shootingPattern;
    public float FirstShotDelay => firstShotDelay;
    public float TimeBetweenShots => timeBetweenShots;
    public int ShotsPerBurst => shotsPerBurst;
    public float DelayBetweenBurstShots => delayBetweenBurstShots;
    public float FanAngleSpread => fanAngleSpread;
    public float BurstAngleSpread => burstAngleSpread;
    
    
}
using UnityEngine;

/// <summary>
/// Controls enemy projectiles and how they behave against the player
/// </summary>
public class EnemyProjectileController : MonoBehaviour
{
    [SerializeField] private MainGridManager mainGrid;
    [SerializeField] private ProjectileData projectileData;
    [SerializeField] private Transform playerTargetPoint;
    [SerializeField] private float projectileOutBounds = 1f;

    private int _currentHealth;
    private float _projectileLifeTimer;

    public void InitializeProjectile(ProjectileData projectileData, MainGridManager mainGrid, Transform playerTargetPoint)
    {
        this.projectileData = projectileData;
        this.mainGrid = mainGrid;
        this.playerTargetPoint = playerTargetPoint;

        _currentHealth = projectileData.ProjectileHealth;
        _projectileLifeTimer = projectileData.ProjectileLifeTime;
    }

    private void Start()
    {
        if (!projectileData) return;

        _currentHealth = projectileData.ProjectileHealth;
        _projectileLifeTimer = projectileData.ProjectileLifeTime;
    }

    private void Update()
    {
        if (!projectileData) return;

        _projectileLifeTimer -= Time.deltaTime;

        if (_projectileLifeTimer <= 0)
        {
            Destroy(gameObject);
            return;
        }

        if (mainGrid && mainGrid.IsThingOutOfBounds(transform.position, projectileOutBounds))
        {
            Destroy(gameObject);
            return;
        }

        if (projectileData.Behaviour == ProjectileData.ProjectileBehaviour.Chase)
        {
            MoveTowardPlayer();
        }
    }

    private void MoveTowardPlayer()
    {
        if (!playerTargetPoint) return;

        transform.position = Vector3.MoveTowards(
            transform.position,
            playerTargetPoint.position,
            projectileData.ProjectileSpeed * Time.deltaTime
        );
    }

    public void TakeDamage(int damage)
    {
        if (!projectileData.CanBeShot) return;

        _currentHealth -= damage;

        if (_currentHealth <= 0)
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            // Player takes damage here
            Destroy(gameObject);
        }
    }
}

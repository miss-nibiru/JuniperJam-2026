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
    private Vector3 _moveDirection;
    private bool _projectileExists;

    public void InitializeProjectile(ProjectileData projectile, MainGridManager grid, Transform targetPoint,
        Vector3 projectileDirection)
    {
        projectileData = projectile;
        mainGrid = grid;
        playerTargetPoint = targetPoint;

        _currentHealth = projectile.ProjectileHealth;
        _projectileLifeTimer = projectile.ProjectileLifeTime;
        _moveDirection = projectileDirection.normalized;

        if (_moveDirection == Vector3.zero) _moveDirection = transform.up;
        _projectileExists = true;

    }

    private void Start()
    {
        if(_projectileExists) return;
        if (!projectileData) return;

        _currentHealth = projectileData.ProjectileHealth;
        _projectileLifeTimer = projectileData.ProjectileLifeTime;
        _moveDirection = transform.up * projectileData.ProjectileSpeed;
        
    }

    private void Update()
    {
        if (!projectileData) return;
        _projectileLifeTimer -= Time.deltaTime;

        if (_projectileLifeTimer <= 0)
        {
            Destroy(gameObject); return;
        }

        if (mainGrid && mainGrid.IsThingOutOfBounds(transform.position, projectileOutBounds))
        {
            Destroy(gameObject); return;
        }

        if (projectileData.Behaviour == ProjectileData.ProjectileBehaviour.Chase) MoveTowardPlayer();
        

        if (projectileData.Behaviour == ProjectileData.ProjectileBehaviour.Spread) MoveForwardOnly();

    }

    private void MoveForwardOnly() // Spread
    {
        transform.position += _moveDirection * projectileData.ProjectileSpeed * Time.deltaTime;
    }

    private void MoveTowardPlayer() //chase
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

        if (_currentHealth <= 0) Destroy(gameObject);
        
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerHealth playerHealth = other.GetComponent<PlayerHealth>();
            if (playerHealth) playerHealth.TakeDamage(projectileData.ProjectileDamageAmount);

            Destroy(gameObject);
        }
    }
}

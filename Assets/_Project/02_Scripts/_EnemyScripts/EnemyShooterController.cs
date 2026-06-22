using System.Collections;
using UnityEngine;

/// <summary>
/// Some enemies shoot projectiles.
/// This controls which projectile strategy they use and from which spawn points they shoot.
/// </summary>
/// 
public class EnemyShooterController : MonoBehaviour
{
    [SerializeField] private EnemyProjectileStrategyData projectileStrategyData;
    [SerializeField] private Transform[] projectileSpawnPoints;

    private MainGridManager _mainGrid;
    private Transform _playerTargetPoint;

    private Coroutine _shootingRoutine;

    public void InitializeShooter(MainGridManager mainGrid, Transform playerTargetPoint)
    {
        _mainGrid = mainGrid;
        _playerTargetPoint = playerTargetPoint;

        StartShooting();
    }

    private void StartShooting()
    {
        if (!projectileStrategyData) return;
        if (!projectileStrategyData.ProjectileData) return;
        if (projectileSpawnPoints == null || projectileSpawnPoints.Length == 0) return;

        if (_shootingRoutine != null)
        {
            StopCoroutine(_shootingRoutine);
        }

        _shootingRoutine = StartCoroutine(ShootingRoutine());
    }

    private IEnumerator ShootingRoutine()
    {
        yield return new WaitForSeconds(projectileStrategyData.FirstShotDelay);

        while (true)
        {
            if (projectileStrategyData.Pattern == EnemyProjectileStrategyData.ShootingPattern.Single)
            {
                ShootSingleFromAllPoints();
            }

            if (projectileStrategyData.Pattern == EnemyProjectileStrategyData.ShootingPattern.Burst)
            {
                yield return StartCoroutine(ShootBurstFromAllPoints());
            }

            if (projectileStrategyData.Pattern == EnemyProjectileStrategyData.ShootingPattern.Fan)
            {
                yield return StartCoroutine(FanAttackFromAllPoints());
            }

            yield return new WaitForSeconds(projectileStrategyData.TimeBetweenShots);
        }
    }

    private void ShootSingleFromAllPoints()
    {
        foreach (Transform spawnPoint in projectileSpawnPoints)
        {
            if (!spawnPoint) continue;

            ShootProjectileTowardPlayer(spawnPoint);
        }
    }

    private IEnumerator ShootBurstFromAllPoints()
    {
        int projectileCount = Mathf.Max(1, projectileStrategyData.ShotsPerBurst);

        for (int i = 0; i < projectileCount; i++)
        {
            foreach (Transform spawnPoint in projectileSpawnPoints)
            {
                if (!spawnPoint) continue;

                Vector3 directionToPlayer = (_playerTargetPoint.position - spawnPoint.position).normalized;

                if (directionToPlayer == Vector3.zero)
                {
                    directionToPlayer = spawnPoint.up;
                }

                float randomAngle = Random.Range(
                    -projectileStrategyData.BurstAngleSpread,
                    projectileStrategyData.BurstAngleSpread
                );

                Vector3 projectileDirection = Quaternion.Euler(0f, 0f, randomAngle) * directionToPlayer;

                ShootProjectileInDirection(spawnPoint, projectileDirection);
            }

            yield return new WaitForSeconds(projectileStrategyData.DelayBetweenBurstShots);
        }
    }

    private IEnumerator FanAttackFromAllPoints()
    {
        int projectileCount = Mathf.Max(1, projectileStrategyData.ShotsPerBurst);
        float angleSpread = projectileStrategyData.FanAngleSpread;

        for (int i = 0; i < projectileCount; i++)
        {
            float angle = 0f;

            if (projectileCount > 1)
            {
                float angleStep = angleSpread / (projectileCount - 1);
                angle = -angleSpread / 2f + angleStep * i;
            }

            foreach (Transform spawnPoint in projectileSpawnPoints)
            {
                if (!spawnPoint) continue;

                Vector3 centerDirection = (_playerTargetPoint.position - spawnPoint.position).normalized;

                if (centerDirection == Vector3.zero)
                {
                    centerDirection = spawnPoint.up;
                }

                Vector3 projectileDirection = Quaternion.Euler(0f, 0f, angle) * centerDirection;

                ShootProjectileInDirection(spawnPoint, projectileDirection);
            }

            yield return new WaitForSeconds(projectileStrategyData.DelayBetweenBurstShots);
        }
    }

    private void ShootProjectileTowardPlayer(Transform spawnPoint)
    {
        Vector3 directionToPlayer = (_playerTargetPoint.position - spawnPoint.position).normalized;

        if (directionToPlayer == Vector3.zero)
        {
            directionToPlayer = spawnPoint.up;
        }

        ShootProjectileInDirection(spawnPoint, directionToPlayer);
    }

    private void ShootProjectileInDirection(Transform spawnPoint, Vector3 projectileDirection)
    {
        ProjectileData projectileData = projectileStrategyData.ProjectileData;

        if (!projectileData) return;
        if (!projectileData.ProjectilePrefab) return;
        if (!_mainGrid) return;
        if (!_playerTargetPoint) return;
        if (!spawnPoint) return;

        GameObject projectileObject = Instantiate(
            projectileData.ProjectilePrefab,
            spawnPoint.position,
            spawnPoint.rotation
        );

        EnemyProjectileController projectileController = projectileObject.GetComponent<EnemyProjectileController>();

        if (!projectileController) return;

        projectileController.InitializeProjectile(
            projectileData,
            _mainGrid,
            _playerTargetPoint,
            projectileDirection
        );
    }

    private void OnDisable()
    {
        if (_shootingRoutine != null)
        {
            StopCoroutine(_shootingRoutine);
            _shootingRoutine = null;
        }
    }
}
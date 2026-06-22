using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// some enemies shoot projectiles and this will be the script that controls which ones - communicates with projectiledata
/// </summary>

public class EnemyShooterController : MonoBehaviour
{
    [SerializeField] private EnemyProjectileStrategyData projectileStrategyData;
    [SerializeField] private Transform projectileSpawnPoint;

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
        if (!projectileSpawnPoint) return;

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
                ShootProjectile();
            }

            if (projectileStrategyData.Pattern == EnemyProjectileStrategyData.ShootingPattern.Burst)
            {
                yield return StartCoroutine(ShootBurst());
            }

            yield return new WaitForSeconds(projectileStrategyData.TimeBetweenShots);
        }
    }

    private IEnumerator ShootBurst()
    {
        for (int i = 0; i < projectileStrategyData.ShotsPerBurst; i++)
        {
            ShootProjectile();

            yield return new WaitForSeconds(projectileStrategyData.DelayBetweenBurstShots);
        }
    }

    private void ShootProjectile()
    {
        ProjectileData projectileData = projectileStrategyData.ProjectileData;

        if (!projectileData) return;
        if (!projectileData.ProjectilePrefab) return;
        if (!_mainGrid) return;
        if (!_playerTargetPoint) return;

        GameObject projectileObject = Instantiate(
            projectileData.ProjectilePrefab,
            projectileSpawnPoint.position,
            projectileSpawnPoint.rotation
        );

        EnemyProjectileController projectileController = projectileObject.GetComponent<EnemyProjectileController>();

        if (!projectileController) return;

        projectileController.InitializeProjectile(projectileData, _mainGrid, _playerTargetPoint);
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
using System;
using UnityEngine;

/// <summary>
/// the final boss has child colliders that need to take damage and damage the player
/// to avoid bugs, better to have an independent script handling child objects
/// </summary>

public class BossHitBox : MonoBehaviour
{
    [SerializeField] private EnemyController enemyController;

    private void Awake()
    {
        if (!enemyController) enemyController = GetComponentInParent<EnemyController>();
    }

    public void TakeHitsFromBullets(int damage)
    {
        if(!enemyController) return;
        enemyController.TakeDamage(damage);
    }
    
}

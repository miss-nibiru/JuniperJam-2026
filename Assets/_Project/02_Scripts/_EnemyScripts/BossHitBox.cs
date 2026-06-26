
using UnityEngine;

/// <summary>
/// the final boss has child colliders that need to take damage and damage the player
/// to avoid bugs, better to have an independent script handling child objects
/// added the flashing communication so its not in one script
/// </summary>

public class BossHitBox : MonoBehaviour
{
    [SerializeField] private EnemyController enemyController;
    [SerializeField] private BossEyeHitFeedback hitEye;

    private void Awake()
    {
        if (!enemyController) enemyController = GetComponentInParent<EnemyController>();
        if (!hitEye) hitEye = GetComponentInChildren<BossEyeHitFeedback>();
    }

    public void TakeHitsFromBullets(int damage)
    {
        if(!enemyController) return;
        if(hitEye) hitEye.Flash();
        enemyController.TakeDamage(damage);
    }
    
}

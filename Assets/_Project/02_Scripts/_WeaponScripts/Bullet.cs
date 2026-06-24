using UnityEngine;

[RequireComponent (typeof(Rigidbody2D))]
public class Bullet : MonoBehaviour
{
    [SerializeField] private float bulletSpeed;
    [SerializeField] private float lifetime;
    [SerializeField] private int bulletDamage = 1;

    private Rigidbody2D _rb;

    void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
        Destroy(gameObject, lifetime);
    }

    private void Update()
    {
        _rb.freezeRotation = true;
        _rb.linearVelocity = transform.up * bulletSpeed;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Hit(collision.gameObject);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Hit(collision.gameObject);
    }

    private void Hit(GameObject hitTarget)
    {
        BossHitBox enemyHurtbox = hitTarget.GetComponent<BossHitBox>();

        if (enemyHurtbox)
        {
            enemyHurtbox.TakeHitsFromBullets(bulletDamage);
            Destroy(gameObject);
            return;
        }

        EnemyController enemy = hitTarget.GetComponent<EnemyController>();

        if (enemy)
        {
            enemy.TakeDamage(bulletDamage);
            Destroy(gameObject);
            return;
        }

        EnemyProjectileController enemyProjectile = hitTarget.GetComponent<EnemyProjectileController>();

        if (enemyProjectile)
        {
            enemyProjectile.TakeDamage(bulletDamage);
            Destroy(gameObject);
            return;
        }
    }
}

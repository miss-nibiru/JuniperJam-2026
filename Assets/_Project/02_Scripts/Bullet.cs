using UnityEngine;

[RequireComponent (typeof(Rigidbody2D))]
public class Bullet : MonoBehaviour
{
    [SerializeField] private float bulletSpeed;
    [SerializeField] private float lifetime;
    [SerializeField] private int bulletDamage;

    private Rigidbody2D rb;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        Destroy(gameObject, lifetime);
    }

    private void Update()
    {
        rb.freezeRotation = true;
        rb.linearVelocity = transform.up * bulletSpeed;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        EnemyController enemy = collision.GetComponent<EnemyController>();

        if (enemy)
        {
            enemy.TakeDamage(bulletDamage);
            Destroy(gameObject);
            return;
        }

        EnemyProjectileController enemyProjectile = collision.GetComponent<EnemyProjectileController>(); //if it collides with a projectile with the main brain attached

        if (enemyProjectile)
        {
            enemyProjectile.TakeDamage(bulletDamage);
            Destroy(gameObject);
            return;
            
        }

    }
}

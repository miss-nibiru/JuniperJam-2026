using UnityEngine;

[RequireComponent (typeof(Rigidbody2D))]
public class Bullet : MonoBehaviour
{
    [SerializeField] private float bulletSpeed;
    [SerializeField] private float lifetime;

    private Rigidbody2D rb;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        rb.freezeRotation = true;
        rb.linearVelocity = transform.up * bulletSpeed;

        Destroy(gameObject, lifetime);
    }
}

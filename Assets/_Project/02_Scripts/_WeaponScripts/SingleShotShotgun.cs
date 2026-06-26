using UnityEngine;
using UnityEngine.InputSystem;

public class SingleShotShotgun : GunBase
{
    private Vector2 aimDirection;
    private float aimAngle;

    [SerializeField] private GameObject shootPoint;

    public override void Shoot(GameObject bulletPrefab)
    {
        Instantiate(bulletPrefab, shootPoint.transform.position, Quaternion.Euler(0f, 0f, aimAngle));
    }

    private void Update()
    {
        aimDirection = (Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue()) - new Vector3(transform.position.x, transform.position.y)).normalized;
        aimAngle = Mathf.Atan2(aimDirection.y, aimDirection.x) * Mathf.Rad2Deg - 90f;
    }
}

using UnityEngine;
using System.Collections;
using UnityEngine.InputSystem;

public class Shotgun : GunBase
{
    [SerializeField] private float positionSpacingBetweenEachBullet;

    [SerializeField] private GameObject bullet;

    [SerializeField] private float autoShootDelay;

    [SerializeField] private float coneAngle;

    private Vector2 aimDirection;

    private Vector2 centerOfScreen;

    private float angleSpacingBetweenEachBullet;

    private float firstBulletAngle;

    private float firstBulletDistance;

    private float aimAngle;


    public override void Shoot(GameObject bulletPrefab)
    {
        for (int i = 0; i <  numberOfBulletsPerShot; i++)
        {
            Instantiate(bulletPrefab, 
                transform.position + new Vector3(firstBulletDistance + (i * positionSpacingBetweenEachBullet), 0f, 0f), 
                Quaternion.Euler(0f, 0f, firstBulletAngle + i * angleSpacingBetweenEachBullet + aimAngle));
        }
    }

    private void Awake()
    {
        firstBulletAngle = coneAngle / -2f;
        angleSpacingBetweenEachBullet = coneAngle / (numberOfBulletsPerShot - 1);
        firstBulletDistance = -positionSpacingBetweenEachBullet * (numberOfBulletsPerShot - 1) / 2f;
    }

    private void Start()
    {
        StartCoroutine(ShootBulletsOverTime());
        centerOfScreen = new Vector2(Screen.width * .5f, Screen.height * .5f);
    }

    private IEnumerator ShootBulletsOverTime()
    {
        while (true)
        {
            Shoot(bullet);
            yield return new WaitForSeconds(autoShootDelay);
        }
    }

    private void Update()
    {
        aimDirection = (Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue()) - new Vector3(transform.position.x, transform.position.y).normalized);
        aimAngle = Mathf.Atan2(aimDirection.y, aimDirection.x) * Mathf.Rad2Deg - 90f;
    }
}

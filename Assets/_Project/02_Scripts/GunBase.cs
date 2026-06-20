using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;

public abstract class GunBase : MonoBehaviour
{
    [SerializeField] protected int numberOfBulletsPerShot;
    [SerializeField] protected float shootDelay;
    [SerializeField] protected GameObject bulletPrefab;

    protected bool isHoldingShoot;

    protected float timeThatShootDelayWillBeOver;

    public abstract void Shoot(GameObject bulletPrefab);

    public void OnShoot(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            isHoldingShoot = true;
            StartCoroutine(KeepShooting());
        }

        if (context.canceled)
        {
            isHoldingShoot = false;
        }
    }

    public IEnumerator KeepShooting()
    {
        while (isHoldingShoot && Time.time > timeThatShootDelayWillBeOver)
        {
            Shoot(bulletPrefab);
            timeThatShootDelayWillBeOver = Time.time + shootDelay;
            yield return new WaitForSeconds(shootDelay);
        }
    }
}

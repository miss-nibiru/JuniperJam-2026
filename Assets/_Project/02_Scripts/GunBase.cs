using UnityEngine;

public abstract class GunBase : MonoBehaviour
{
    [SerializeField] protected int numberOfBulletsPerShot;
    [SerializeField] protected float shootDelay;

    public abstract void Shoot(GameObject bulletPrefab);
}

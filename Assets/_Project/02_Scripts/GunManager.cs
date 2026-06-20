using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;

public class GunManager : MonoBehaviour
{
    private bool isHoldingShoot;
    private float timeThatShootDelayWillBeOver;

    [SerializeField] private GunBase shotgun;
    [SerializeField] private GunBase machineGun;
    [SerializeField] private GunBase singleShotShotgun;

    private GunBase weaponBeingUsed;


    public enum CurrentWeapon
    {
        Shotgun,
        MachineGun,
        SingleShotShotgun
    }

    private void Awake()
    {
        int number = Random.Range(0, 3);
        currentWeapon = (CurrentWeapon)number;
    }

    public CurrentWeapon currentWeapon;

    private void Start()
    {
        if (currentWeapon == CurrentWeapon.Shotgun) weaponBeingUsed = shotgun;
       
        if (currentWeapon == CurrentWeapon.MachineGun) weaponBeingUsed = machineGun;

        if (currentWeapon == CurrentWeapon.SingleShotShotgun) weaponBeingUsed = singleShotShotgun;
    }

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
            weaponBeingUsed.Shoot(weaponBeingUsed.bulletPrefab);
            timeThatShootDelayWillBeOver = Time.time + weaponBeingUsed.shootDelay;
            yield return new WaitForSeconds(weaponBeingUsed.shootDelay);
        }
    }

}

using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;

public class GunController : MonoBehaviour
{
    private bool isHoldingShoot;
    private float timeThatShootDelayWillBeOver;

    [SerializeField] private GunBase shotgun;
    [SerializeField] private GunBase machineGun;
    [SerializeField] private GunBase singleShotShotgun;

    private bool shootingEnabled;
    public GunBase weaponBeingUsed;

    /*public enum CurrentWeapon
    {
        Shotgun,
        MachineGun,
        SingleShotShotgun
    }*/

    private void Awake()
    {
        //int number = Random.Range(0, 3);
        //currentWeapon = (CurrentWeapon)number;
    }

    //public CurrentWeapon currentWeapon;

    private void Start()
    {
        /*if (currentWeapon == CurrentWeapon.Shotgun) weaponBeingUsed = shotgun;
       
        if (currentWeapon == CurrentWeapon.MachineGun) weaponBeingUsed = machineGun;

        if (currentWeapon == CurrentWeapon.SingleShotShotgun) weaponBeingUsed = singleShotShotgun;*/
    }

    public void OnShoot(InputAction.CallbackContext context)
    {
        if (context.performed && shootingEnabled)
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

    public void SetShootingEnabled(bool enabled)
    {
        if (enabled)
        {
            shootingEnabled = true;
        }
        else
        {
            shootingEnabled = false;
        }
    }

    public void SelectWeapon(SpinningWheel.WeaponChoice weaponChoice)
    {
        switch (weaponChoice)
        {
            case SpinningWheel.WeaponChoice.Shotgun:
                weaponBeingUsed = shotgun;
                break;

            case SpinningWheel.WeaponChoice.MachineGun:
                weaponBeingUsed = machineGun;
                break;

            case SpinningWheel.WeaponChoice.SingleShotShotgun:
                weaponBeingUsed = singleShotShotgun;
                break;

            default:
                break;
        }
    }
}

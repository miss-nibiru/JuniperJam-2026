using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;

public class GunController : MonoBehaviour
{
    private bool isHoldingShoot;
    private float timeThatShootDelayWillBeOver;
    private Coroutine shootingRoutine;

    [SerializeField] private GunBase shotgun;
    [SerializeField] private GunBase machineGun;
    [SerializeField] private GunBase singleShotShotgun;

    private bool shootingEnabled;
    public GunBase weaponBeingUsed;
    private SpinningWheel.WeaponChoice currentWeaponChoice = SpinningWheel.WeaponChoice.None;

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
            if (isHoldingShoot) return;

            isHoldingShoot = true;
            shootingRoutine = StartCoroutine(KeepShooting());
        }

        if (context.canceled)
        {
            isHoldingShoot = false;

            if (shootingRoutine != null)
            {
                StopCoroutine(shootingRoutine);
                shootingRoutine = null;
            }
        }
    }

    public IEnumerator KeepShooting()
    {
        while (isHoldingShoot)
        {
            if (weaponBeingUsed && Time.time > timeThatShootDelayWillBeOver)
            {
                weaponBeingUsed.Shoot(weaponBeingUsed.bulletPrefab);
                AudioManager.Instance?.PlayWeaponShot(currentWeaponChoice);
                timeThatShootDelayWillBeOver = Time.time + weaponBeingUsed.shootDelay;
            }

            yield return null;
        }

        shootingRoutine = null;
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
            isHoldingShoot = false;

            if (shootingRoutine != null)
            {
                StopCoroutine(shootingRoutine);
                shootingRoutine = null;
            }
        }
    }

    public void SelectWeapon(SpinningWheel.WeaponChoice weaponChoice)
    {
        currentWeaponChoice = weaponChoice;

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

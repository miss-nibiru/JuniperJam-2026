using UnityEngine;
using UnityEngine.InputSystem;

public class RotateGunAroundPlayer : MonoBehaviour
{
    Vector3 aimDirection;
    float aimAngle;

    [SerializeField] private GameObject[] guns;

    [SerializeField] private GameObject gunEquipped;
    //[SerializeField] private GunController gunController;
    [SerializeField] private SpinningWheel spinningWheel;

    [SerializeField] private Transform playerTransform;

    private SpriteRenderer gunSpriteRenderer;

    void Update()
    {
        if (spinningWheel?.weaponChoiceWon == SpinningWheel.WeaponChoice.Shotgun)
        {
            gunEquipped = guns[0];
        }
        else if (spinningWheel?.weaponChoiceWon == SpinningWheel.WeaponChoice.MachineGun)
        {
            gunEquipped = guns[1];
        }
        else if (spinningWheel?.weaponChoiceWon == SpinningWheel.WeaponChoice.SingleShotShotgun)
        {
            gunEquipped = guns[2];
        }

        gunSpriteRenderer = gunEquipped.GetComponent<SpriteRenderer>();

        aimDirection = (Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue()) - new Vector3(transform.position.x, transform.position.y)).normalized;
        aimAngle = Mathf.Atan2(aimDirection.y, aimDirection.x) * Mathf.Rad2Deg - 180;

        ShowWeaponEquipped();

        transform.rotation = Quaternion.Euler(0f, 0f, aimAngle);

        if (Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue()).x < playerTransform.position.x)
        {
            gunSpriteRenderer.flipY = false;
        }
        else
        {
            gunSpriteRenderer.flipY = true;
        }
    }

    public void ShowWeaponEquipped()
    {
        foreach (GameObject gun in guns)
        {
            gun.SetActive(false);
        }
        gunEquipped?.SetActive(true);
    }


}

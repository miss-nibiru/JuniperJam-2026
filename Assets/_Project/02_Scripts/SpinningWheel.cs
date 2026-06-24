using System;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class SpinningWheel : MonoBehaviour
{
    [SerializeField] private float rotatePower;
    
    [SerializeField] private float stopPower;

    [SerializeField] private int numberOfWeapons = 3;

    [SerializeField] private float angleOffsetForWheel;
    
    public event Action<WeaponChoice> SpinFinished;

    private bool inSpin;
    private bool hasStartedSpinning;

    private float finalAngle;

    private float angleSpacingBetweenEachWeapon;

    private Rigidbody2D rb;

    public enum WeaponChoice
    {
        None,
        Shotgun,
        MachineGun,
        SingleShotShotgun
    }

    public WeaponChoice weaponChoiceWon = WeaponChoice.None;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        //angleSpacingBetweenEachWeapon = 360 / numberOfWeapons;
    }

    private void Update()
    {
        if (inSpin && rb.angularVelocity > 0)
        {
            rb.angularVelocity -= stopPower * Time.deltaTime;

            if (rb.angularVelocity < 0)
            {
                rb.angularVelocity = 0;
            }

            hasStartedSpinning = true;
        }

        if (rb.angularVelocity <= 0 && inSpin && hasStartedSpinning) 
        {
            finalAngle = transform.rotation.eulerAngles.z;
            GetWeaponChoice();
            inSpin = false;
            hasStartedSpinning = false;
            AudioManager.Instance?.PlaySpinFinished();
            SpinFinished?.Invoke(weaponChoiceWon);
        }
    }

    public void Spin()
    {
        if (!inSpin)
        {
            rb.angularVelocity = rotatePower * UnityEngine.Random.Range(1f, 2f);
            hasStartedSpinning = false;
            inSpin = true;
            AudioManager.Instance?.PlaySpinStart();
        }
    }

    private void GetWeaponChoice()
    {
        angleSpacingBetweenEachWeapon = 360f / numberOfWeapons;
        float halfAngleSpacing = angleSpacingBetweenEachWeapon / 2f;
        float angleToCheck = finalAngle + angleOffsetForWheel;

        angleToCheck %= 360f;

        if (angleToCheck < 0f)
        {
            angleToCheck += 360f;
        }

        if (angleToCheck <= halfAngleSpacing || angleToCheck > 360f - halfAngleSpacing)
        {
            weaponChoiceWon = WeaponChoice.Shotgun;
        }
        else if (angleToCheck <= halfAngleSpacing + angleSpacingBetweenEachWeapon)
        {
            weaponChoiceWon = WeaponChoice.SingleShotShotgun;
        }
        else
        {
            weaponChoiceWon = WeaponChoice.MachineGun;
        }

        Debug.Log($"Angle is {finalAngle} and weapon is {weaponChoiceWon}");
    }
    
}

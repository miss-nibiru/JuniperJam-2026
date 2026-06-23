using System;
using System.Runtime.CompilerServices;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class SpinningWheel : MonoBehaviour
{
    [SerializeField] private float rotatePower;
    
    [SerializeField] private float stopPower;

    [SerializeField] private int numberOfWeapons;

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
        if (rb.angularVelocity > 0)
        {
            rb.angularVelocity -= stopPower * Time.deltaTime;
            hasStartedSpinning = true;
        }
        if (rb.angularVelocity <= 0 && inSpin && hasStartedSpinning) 
        {
            finalAngle = transform.rotation.eulerAngles.z;
            GetWeaponChoice();
            inSpin = false;
            SpinFinished?.Invoke(weaponChoiceWon);
        }
    }

    public void Spin()
    {
        if (!inSpin)
        {
            rb.AddTorque(rotatePower * UnityEngine.Random.Range(1f, 2f));
            inSpin = true;
        }
    }

    /*private void GetWeaponChoice()
    {
        float rotation = transform.eulerAngles.z;

        if (rotation > -60f && rotation < 60f)
        {
            weaponChoiceWon = WeaponChoice.Shotgun;
        }
        else if (rotation > 60f && rotation < 180f)
        {
            weaponChoiceWon = WeaponChoice.SingleShotShotgun;
        }
        else if (rotation > 180f && rotation < 300f)
        {
            weaponChoiceWon = WeaponChoice.MachineGun;
        }
    }*/

    // Whatever number I get for angle, modulus 360- subtract offset then do modulus
    // take that angle, pin it to one of the

    private void GetWeaponChoice()
    {
        angleSpacingBetweenEachWeapon = 360 / (numberOfWeapons + 1);
        int amount = Enum.GetValues(typeof(WeaponChoice)).Length;
        int weapon = (int)(finalAngle / angleSpacingBetweenEachWeapon);
        Debug.Log($"Angle is {finalAngle} with a spacing of {angleSpacingBetweenEachWeapon} and amount is {amount} and weapon is {weapon}");
        //switch (numberOfWeapons - 1)
        //{
        //    case 0:
        //        if (finalAngle > 0f && finalAngle < angleSpacingBetweenEachWeapon)
        //        {

        //        }
        //        break;
        //    case 1:
        //        if (finalAngle > angleSpacingBetweenEachWeapon)
        //        {

        //        }
        //        break;
        //    case 2:
        //        if (finalAngle)
        //        {

        //        }
        //        break;
        //    case 3:
        //        if (finalAngle)
        //        {

        //        }
        //        break;
        //    case 4:
        //        if (finalAngle)
        //        {

        //        }
        //        break;
        //}
        //if (finalAngle)
        //{

        //}
    }
    
}

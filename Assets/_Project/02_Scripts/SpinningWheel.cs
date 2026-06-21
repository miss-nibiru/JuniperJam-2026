using System;
using System.Runtime.CompilerServices;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class SpinningWheel : MonoBehaviour
{
    [SerializeField] private float rotatePower;
    [SerializeField] private float stopPower;

    public event Action<WeaponChoice> SpinFinished;

    private bool inSpin;
    private bool hasStartedSpinning;

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
    }

    private void Update()
    {
        if (rb.angularVelocity > 0)
        {
            rb.angularVelocity -= stopPower * Time.deltaTime;
            hasStartedSpinning = true;
        }
        if (rb.angularVelocity == 0 && inSpin && hasStartedSpinning) 
        {
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

    private void GetWeaponChoice()
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
    }
}
